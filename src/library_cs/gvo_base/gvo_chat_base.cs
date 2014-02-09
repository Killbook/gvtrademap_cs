/*-------------------------------------------------------------------------

 チャット解析のベース
 リクエスト付き
 指定したルールでの解析が可能
 解析は1行単位のみ
 複数行での解析は実機からリアルタイムにログが書き出されるため、
 まだ必要なログが書き出されていない可能性がある

 スレッド対応版

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

using Utility;
using System.Text.RegularExpressions;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvo_base
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public abstract class gvo_chat_base : request_ctrl
	{
		public enum type{
			index0,		// 1文字目から一致するもの
			any_index,	// どこかに含まれればいいもの
			regex,		// 正規表現
		};
	
		/*-------------------------------------------------------------------------
		 解析対象
		---------------------------------------------------------------------------*/
		public class analize_data
		{
			private string				m_analize;
			private type				m_type;
			private Regex				m_regex;		// 正規表現時の解析対象
			private object				m_tag;			// 解析結果参照用タグ

			/*-------------------------------------------------------------------------
			
			---------------------------------------------------------------------------*/
			public string analize		{	get{	return m_analize;	}}
			public type type			{	get{	return m_type;		}}
			public object tag			{	get{	return m_tag;		}}

			/*-------------------------------------------------------------------------
			
			---------------------------------------------------------------------------*/
			public analize_data(string analize, type type)
			{
				init(analize, type);
			}
			public analize_data(string analize, type type, object tag)
			{
				init(analize, type);
				m_tag		= tag;
			}

			/*-------------------------------------------------------------------------
			 init
			---------------------------------------------------------------------------*/
			private void init(string analize, type type)
			{
				m_analize	= analize;
				m_type		= type;
				m_regex		= null;
				m_tag		= null;

				// 正規表現時はコンパイルしておく
				if(type == type.regex){
					m_regex	= new Regex(analize);
				}
			}

			/*-------------------------------------------------------------------------
			 解析
			---------------------------------------------------------------------------*/
			public bool Analize(string line, List<analized_data> list)
			{
				switch(m_type){
				case type.index0:
					// index 0 から見つかる
					if(line.IndexOf(m_analize) == 0){
						list.Add(new analized_data(this, line));
						return true;
					}
					break;
				case type.any_index:
					// どこかに含まれる
					if(line.IndexOf(m_analize) >= 0){
						list.Add(new analized_data(this, line));
						return true;
					}
					break;
				case type.regex:
					// 正規表現
					Match	match	= m_regex.Match(line);
					if(match.Success){
						list.Add(new analized_data(this, line, match));
						return true;
					}
					break;
				}
				return false;
			}
		};

		/*-------------------------------------------------------------------------
		 解析結果
		---------------------------------------------------------------------------*/
		public class analized_data
		{
			private analize_data			m_analize_data;
			private string					m_line;
			private Match					m_match;	// 正規表現時

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public string line			{	get{	return m_line;					}}
			public Match match			{	get{	return m_match;					}}
			public object tag			{	get{	return m_analize_data.tag;		}}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public analized_data(analize_data analize, string line)
			{
				m_analize_data		= analize;
				m_line				= line;
				m_match				= null;
			}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public analized_data(analize_data analize, string line, Match match)
			{
				m_analize_data		= analize;
				m_line				= line;
				m_match				= match;
			}
		};
	
		private string						m_path;							// ログパス
		private FileInfo					m_newest_chat_file_info;		// 最新のログファイル情報
		private FileInfo					m_chat_file_info;				// 解析対象のログファイル情報
		private int							m_analyze_lines;				// 解析した行数

		private List<analize_data>			m_analize_list;					// 解析対象
		private List<analized_data>			m_analized_list;				// 解析結果

		// スレッド対応
		private readonly object				m_syncobject	= new object();

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public List<analized_data> analized_list	{
											get{
												List<analized_data>	list;
												lock(m_syncobject){
													// 新しい解析情報は差し替えのため、
													// 参照が得られればよい
													list	= m_analized_list;
												}
												return list;
											}
									private set{
												lock(m_syncobject){
													m_analized_list	= value;
												}
											}
									}

		public string current_log_fullname	{	get{
													if(m_chat_file_info == null)	return "";
													return m_chat_file_info.FullName;
												}
											}
		public string current_log_name		{	get{
													if(m_chat_file_info == null)	return "";
													return m_chat_file_info.Name;
												}
											}
		public bool is_update				{	get{
													if(analized_list == null)		return false;
													if(analized_list.Count <= 0)	return false;
													return true;
												}
											}
		public string path					{	get{	return m_path;			}
												set{
													m_path				= value;
													ResetNewestLogInfo();
												}
											}
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public gvo_chat_base()
		{
			init(gvo_def.GetGvoLogPath());
		}
		public gvo_chat_base(string path)
		{
			init(path);
		}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		private void init(string path)
		{
			m_path						= path;

			m_newest_chat_file_info		= null;		// 更新が最も新しいログ
			m_chat_file_info			= null;		// 解析対象のログ
			m_analyze_lines				= 0;		// 解析状況

			m_analize_list				= new List<analize_data>();
			m_analized_list				= null;
		}
	
		/*-------------------------------------------------------------------------
		 解析情報を追加する
		 tagで判定すること
		---------------------------------------------------------------------------*/
		protected void AddAnalizeList(string analize, type _type, object tag)
		{
			m_analize_list.Add(new analize_data(analize, _type, tag));
		}

		/*-------------------------------------------------------------------------
		 解析情報を追加する
		 tagで判定すること
		 預金の利息用
		---------------------------------------------------------------------------*/
		protected void AddAnalizeList_Interest(object tag)
		{
			m_analize_list.Add(new analize_data("預金の利息を", type.index0, tag));
		}

		/*-------------------------------------------------------------------------
		 解析情報をクリアする
		---------------------------------------------------------------------------*/
		protected void ClearAnalizeList()
		{
			m_analize_list.Clear();
		}

		/*-------------------------------------------------------------------------
		 最も新しいログファイル名を得る
		---------------------------------------------------------------------------*/
		private FileInfo get_newest_log_file()
		{
			// ログファイル一覧を得る
			// 最終書き込み時間でソートされる
			FileInfo[]	log_list	= GetLogFiles();

			if(log_list == null)		return null;	// 失敗
			if(log_list.Length <= 0)	return null;	// ログが1つもない

			// 最後のファイルが最新ログ
			return log_list[log_list.Length - 1];
		}
	
		/*-------------------------------------------------------------------------
		 ログファイル名一覧を得る
		---------------------------------------------------------------------------*/
		public FileInfo[] GetLogFiles()
		{
			return GetLogFiles(m_path);
		}
		public static FileInfo[] GetLogFiles(string path)
		{
			try{
				DirectoryInfo	info	= new DirectoryInfo(path);
				if(!info.Exists)		return null;		// ログフォルダが存在しない

				FileInfo[]	file_info	= info.GetFiles("*.html");
				FileInfo[]	file_info2	= info.GetFiles("*.txt");

				// 連結
				int	size	= file_info.Length;
				Array.Resize<FileInfo>(ref file_info, size + file_info2.Length);
				Array.Copy(file_info2, 0, file_info, size, file_info2.Length);

				// ログが1つもない場合はnullを返す
				if(file_info.Length <= 0)	return null;

				// 最終書き込み時間でソートする
				SortFileInfo_LastWriteTime(file_info);
				return file_info;
			}catch{
				return null;
			}
		}

		/*-------------------------------------------------------------------------
		 ソート
		 最終書き込み時間で比較する
		---------------------------------------------------------------------------*/
		static public void SortFileInfo_LastWriteTime(FileInfo[] list)
		{
			if(list == null)	return;

			// 最終書き込み時間でソートする
			Array.Sort<FileInfo>(list, new file_info_compare());
		}
	
		/*-------------------------------------------------------------------------
		 比較
		 最終書き込み時間で比較する
		---------------------------------------------------------------------------*/
		public class file_info_compare : IComparer<FileInfo>
		{
			public int Compare(FileInfo x, FileInfo y)
			{
				if(x.LastWriteTime < y.LastWriteTime)	return -1;
				if(x.LastWriteTime > y.LastWriteTime)	return 1;
				return 0;
			}
		}

		/*-------------------------------------------------------------------------
		 最新のログ解析
		---------------------------------------------------------------------------*/
		public virtual bool AnalyzeNewestChatLog()
		{
			// 解析リストをクリア
			ResetAnalizedList();

			// 更新をチェック
			if(!check_update_log())		return false;

			// 更新された部分を解析
			List<analized_data>	list	= new List<analized_data>();
			if(do_analize(m_chat_file_info, list, ref m_analyze_lines)){
				analized_list		= list;
				return true;
			}
			return false;
		}

		/*-------------------------------------------------------------------------
		 指定されたログ解析
		---------------------------------------------------------------------------*/
		public List<analized_data> AnalyzeChatLog(string fname)
		{
			try{
				FileInfo	info		= new FileInfo(fname);
				return AnalyzeChatLog(info);
			}catch{
				return null;
			}
		}
		public List<analized_data> AnalyzeChatLog(FileInfo info)
		{
			List<analized_data>	list	= new List<analized_data>();

			int	lines			= 0;
			if(!do_analize(info, list, ref lines)){
				// 失敗
				list	= null;
			}
			return list;
		}

		/*-------------------------------------------------------------------------
		 最新のログ情報をリセットする
		---------------------------------------------------------------------------*/
		public void ResetNewestLogInfo()
		{
			m_chat_file_info	= null;
		}

		/*-------------------------------------------------------------------------
		 更新をチェックする
		---------------------------------------------------------------------------*/
		private bool check_update_log()
		{
			// 現在のカレントログがロックされているかどうかを調べる
			// ロックされているなら大航海時代Onlineがログを握ってるとする
			// その場合新しいログを検索しない
			if(!is_locked_log_file()){
				// ロックされていない
				// 最も新しいログを得る
				m_newest_chat_file_info	= get_newest_log_file();
			}

			// ログファイル情報の取得に失敗
			if(m_newest_chat_file_info == null)		return false;

			if(m_chat_file_info == null){
				// 初めての解析
				m_chat_file_info	= m_newest_chat_file_info;
				m_analyze_lines		= 0;

				Debug.WriteLine("1st " + m_chat_file_info.FullName);
			}else{
				// 更新されたか調べる
				if(m_newest_chat_file_info.FullName != m_chat_file_info.FullName){
					// 最新のログが違う
					// 無条件で解析する
					m_chat_file_info	= m_newest_chat_file_info;
					m_analyze_lines		= 0;

					Debug.WriteLine("new log " + m_chat_file_info.FullName);
				}else{
	//				if(m_newest_chat_file_info.LastWriteTime <= m_chat_file_info.LastWriteTime){
					if(m_newest_chat_file_info.Length <= m_chat_file_info.Length){
						// 更新されていない
						Debug.WriteLine("skip log " + m_chat_file_info.FullName);
						return false;
					}
					Debug.WriteLine("update log " + m_chat_file_info.FullName);
					// 更新された
					m_chat_file_info	= m_newest_chat_file_info;
				}
			}
			return true;		// 更新された
		}

		/*-------------------------------------------------------------------------
		 現在のカレントログがロックされているかどうかを調べる
		 ロックされているなら大航海時代Onlineがログを握ってるとする
		 その場合新しいログを検索しない
		---------------------------------------------------------------------------*/
		private bool is_locked_log_file()
		{
			// カレントログを未取得
			if(m_chat_file_info == null)	return false;
			// カレントログが存在しない
			if(!m_chat_file_info.Exists)	return false;

			// 最新情報を新しく作成
			m_newest_chat_file_info		= new FileInfo(m_chat_file_info.FullName);
	
			try{
				// 情報更新
				m_newest_chat_file_info.Refresh();
			}catch{
				// 情報更新に失敗
				return false;
			}
			try{
				// 開いてみる
				using(FileStream stream = new FileStream(m_newest_chat_file_info.FullName, FileMode.Open, FileAccess.Read, FileShare.None)){
					// 開けた場合は大航海時代Onlineがログをロックしていない
				}
			}catch{
				// 大航海時代Onlineによってロックされていると判断する
				Debug.WriteLine("locked log file.[" + m_newest_chat_file_info.FullName + " ]");
				return true;
			}
			// ロックされていないので最新ログを取得する必要あり
			m_newest_chat_file_info	= null;
			Debug.WriteLine("unlocked log file.");
			return false;
		}

		/*-------------------------------------------------------------------------
		 解析する
		 解析結果はlist内容に追加される
		---------------------------------------------------------------------------*/
		private bool do_analize(FileInfo file_info, List<analized_data> list, ref int analyze_lines)
		{
			// スレッド時にnullになる可能性がある
			// pathのset時
			if(file_info == null)	return false;

			try{
				using(FileStream stream = new FileStream(file_info.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)){
					string	line		= "";
					int		line_count	= 0;

					// TXTとHTMLに対応
					string		ex		= Path.GetExtension(file_info.FullName);
					bool		is_text;
					Encoding	encoder	= null;
					if(ex == ".txt"){
						is_text	= true;
						encoder	= Encoding.GetEncoding("Shift_JIS");
					}else{
						is_text	= false;
						encoder	= Encoding.UTF8;
					}

					try{
						using(StreamReader	sr	= new StreamReader(stream, encoder)){
							while((line = sr.ReadLine()) != null){
								if(is_text){
									if(line_count++ < analyze_lines)	continue;	// 解析済みの行
								}else{
									if(line.Length < 22)				continue;	// ログではないと判断する
									if(line_count++ < analyze_lines)	continue;	// 解析済みの行
									line	= line.Substring(22);					// 
								}
								// この行を解析する
								AnalyzeLine(line, list);
							}
							// 解析した場所を覚えておく
							analyze_lines	= line_count;
						}
					}catch{
						// 失敗
						return false;
					}
				}						
			}catch{
				// 失敗
				Debug.WriteLine("can't open " + file_info.FullName);
				return false;
			}
			return true;
		}
	
		/*-------------------------------------------------------------------------
		 解析
		---------------------------------------------------------------------------*/
		protected virtual void AnalyzeLine(string line, List<analized_data> list)
		{
			// 登録されているルールでの解析
			foreach(analize_data d in m_analize_list){
				if(d.Analize(line, list)){
					// 対象が見つかった
					return;
				}
			}
		}

		/*-------------------------------------------------------------------------
		 解析内容をリセットする
		---------------------------------------------------------------------------*/
		public void ResetAnalizedList()
		{
			analized_list			= null;
		}
	}
}
