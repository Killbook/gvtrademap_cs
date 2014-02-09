/*-------------------------------------------------------------------------

 パッチのダウンロード

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Utility;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	class download_patch
	{
		private List<string>			m_patch_list;	// ダウンロードするパッチリスト	
	
		// スレッドでの読み込みを行った場合の詳細情報読み込み場所
		private int						m_load_current;
		private string					m_load_str;

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public int load_current			{	get{	return m_load_current;			}}
		public int load_max				{	get{	return m_patch_list.Count;		}}
		public string load_str			{	get{	return m_load_str;				}}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public download_patch()
		{
			m_patch_list		= new List<string>();

			m_load_current		= 0;
			m_load_str			= "オフラインモード";
		}
	
		/*-------------------------------------------------------------------------
		 パッチをダウンロードする
		---------------------------------------------------------------------------*/
		public void DoDownload(string path, bool is_connect_web_icon)
		{
			// 同盟国情報
			m_load_str		= "domain info";
			download_domains();

			// 設定によっては取得しない
			if(is_connect_web_icon){
				// webアイコン
				m_load_str		= "@web icons";
				download_web_icons();
			}

			// パッチを当てるファイルを得る
			// 日付でダウンロードするかどうか決定する
			m_load_str		= "patch list";
			if(download_patch_lists(path)){
				// パッチをダウンロードする
				download_pathes(path);
			}else{
				m_load_str	= "ダウンロードエラー";
			}
		}

		/*-------------------------------------------------------------------------
		 同盟国情報をダウンロードする
		---------------------------------------------------------------------------*/
		private bool download_domains()
		{
			string	data	= HttpDownload.Download(def.URL_HP_ORIGINAL + @"/domain.php", Encoding.UTF8);
			if(data == null)	return false;		// 失敗

			string[]	split	= data.Split(new string[]{"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);

			// 書き出す
			try{
				using(StreamWriter	sw	= new StreamWriter(
					def.DOMAINS_INFO_FULLFNAME, false, Encoding.GetEncoding("Shift_JIS"))) {
					foreach(string s in split){
						if(s == "start")	continue;
						if(s == "end")		break;
						sw.WriteLine(s);
					}
				}
			}catch{
				return false;
			}
			return true;
		}

		/*-------------------------------------------------------------------------
		 webアイコン
		---------------------------------------------------------------------------*/
		private bool download_web_icons()
		{
			string	data	= HttpDownload.Download(def.URL_HP_ORIGINAL + @"/getwebicon.php", Encoding.UTF8);
			if(data == null)	return false;		// 失敗

			string[]	split	= data.Split(new string[]{"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);
			try{
				using (StreamWriter	sw	= new StreamWriter(
					def.WEB_ICONS_FULLFNAME, false, Encoding.GetEncoding("Shift_JIS"))) {
					foreach(string s in split){
						if(s == @"<list>")	continue;
						if(s == @"</list>")	break;

						string[]	split2	= s.Split(new char[]{'\t'}, StringSplitOptions.RemoveEmptyEntries);

						if(split2.Length < 3)	continue;
						string	line	= split2[0];
						line			+= ",";
						line			+= split2[1];
						line			+= ",";
						line			+= split2[2];
						line			+= ",";
						if(split2.Length >= 4){
							line			+= split2[3];
						}
						sw.WriteLine(line);
					}
				}
			}catch{
				return false;
			}
			return true;
		}
	
		/*-------------------------------------------------------------------------
		 パッチを当てるファイルを得る
		 日付でダウンロードするかどうか決定する
		---------------------------------------------------------------------------*/
		private bool download_patch_lists(string path)
		{
			string	data	= HttpDownload.Download(def.URL_HP_ORIGINAL + @"/showcitymaptimestamp20b.php", Encoding.GetEncoding("shift_jis"));
			if(data == null)	return false;		// 失敗

			m_patch_list.Clear();
			string[]	split	= data.Split(new string[]{"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);

			// 閏年とかどうなる？？
			int[]		monthday	= new int[] { 0, 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };

			foreach(string s in split){
				if(s == "start")	continue;
				if(s == "end")		break;

				string[]	split2	= s.Split(new char[]{ ',' });
				if(split2.Length != 3)			continue;	// データがおかしい

				if(split2[0] == "worldmap.txt")	continue;

				//split2[0]:filename
				//split2[1]:timestamp_s
				//split2[2]:filesize
				int	timestamp	= 0;
				if(File.Exists(path + split2[0])){
					//timestampDT = File.GetCreationTimeUtc(exedir + @"cityinfo20\" + temp[0]);
					//timestampDT = File.GetLastWriteTime(exedir + @"cityinfo20\" + temp[0]);
					DateTime	date	= File.GetLastWriteTime(path + split2[0]);

					timestamp = (Convert.ToInt32(date.ToString("yyyy")) - 2000) * 365 * 24 * 60;
					timestamp += monthday[Convert.ToInt32(date.ToString("MM"))] * 24 * 60;
					timestamp += (Convert.ToInt32(date.ToString("dd")) - 1) * 24 * 60;
					timestamp += Convert.ToInt32(date.ToString("HH")) * 60;
					timestamp += Convert.ToInt32(date.ToString("mm"));
				}
				// サーバのファイルが新しければリストに追加
				if(timestamp < Convert.ToInt32(split2[1])){
					m_patch_list.Add(split2[0]);
				}
			}
			return true;
		}

		/*-------------------------------------------------------------------------
		 パッチをダウンロードする
		---------------------------------------------------------------------------*/
		private void download_pathes(string path)
		{
			m_load_current	= 0;
			if(m_patch_list.Count == 0){
				m_load_str		= "最新です";
				return;
			}

			// リスト数分ダウンロード
			foreach(string f in m_patch_list){
				m_load_str	= f;	// 現在ダウンロード中のファイル名
				download_path(f, path);
				m_load_current++;	// ダウンロードした数
			}
			m_load_str		= "完了";
		}

		/*-------------------------------------------------------------------------
		 パッチをダウンロードする
		 ファイル1つ
		---------------------------------------------------------------------------*/
		private bool download_path(string fname, string path)
		{
			string	data	= HttpDownload.Download(def.URL_HP_ORIGINAL
											+ @"/getnewfile20.php?city=" + Useful.UrlEncodeShiftJis(fname),
											Encoding.GetEncoding("shift_jis"));
			if(data == null)	return false;

			string[]	split	= data.Split(new string[]{"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);

			// 書き出す
			try{
				using (StreamWriter	sw	= new StreamWriter(
					path + fname, false, Encoding.GetEncoding("Shift_JIS"))) {
					foreach(string s in split){
						if(s == "start")	continue;
						if(s == "")			continue;
						if(s == "end")		break;
						sw.WriteLine(s);
					}
				}
			}catch{
				return false;
			}
			return true;
		}
	}
}
