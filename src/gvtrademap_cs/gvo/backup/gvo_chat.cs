/*-------------------------------------------------------------------------

 チャット解析
 リクエスト付き
 預金の利息は災害とは独立して解析される
 危険海域変動システムも独立して解析される

 ログの更新チェックに時間が掛かるため、スレッド推奨
 海域変動は専用のメソッドUpdateSeaArea_DoRequest()でメインスレッドから行うこと

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

using utility;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class gvo_chat : request_ctrl
	{
		/*-------------------------------------------------------------------------
		 海域変動用
		---------------------------------------------------------------------------*/
		class sea_area_type{
			private string							m_name;
			private sea_area.sea_area_once.sea_type	m_type;

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public string name							{	get{	return m_name;		}}
			public sea_area.sea_area_once.sea_type type	{	get{	return m_type;		}}

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public sea_area_type(string name, sea_area.sea_area_once.sea_type type)
			{
				m_name		= name;
				m_type		= type;
			}
		}
	
		public enum accident{
			none,				// なし

			shark1,				// サメ1
			shark2,				// サメ2
			fire,				// 火災
			seaweed,			// 藻
			seiren,				// セイレーン
			compass,			// 羅針盤
			storm,				// 嵐
			blizzard,			// 吹雪
			mouse,				// ネズミ
			UMA,				// 得体の知れない怪物
			treasure1,			// 何かいい物
			treasure2,			// 何か見つかるかも
			treasure3,			// 高価なもの
			escape_battle,		// 全船が戦場を離れました
			win_battle,			// 勝利
			lose_battle,		// 敗北

			max
		};
		private const string				TEMP_FNAME		= "__chattemp.txt";

		private string						m_path;
		private sea_area					m_sea_area;
	
		private FileInfo					m_newest_chat_file_info;	// 最新のログファイル情報
		private FileInfo					m_chat_file_info;			// 解析対象のログファイル情報
		private int							m_analyze_lines;			// 解析した行数

		private accident					m_accident;					// 今回解析した災害
		private bool						m_is_interest;				// 利息が来たらtrue
		private List<sea_area_type>			m_sea_area_type_list;		// 海域変動システム用

		// スレッド対応
		private readonly object				m_syncobject	= new object();

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public accident _accident	{		get{
												accident	acc;
												lock(m_syncobject){
													acc		= m_accident;
												}
												return acc;
											}
									private set{	
												lock(m_syncobject){
													m_accident	= value;
												}
											}
									}
		public bool is_interest		{		get{
												bool	is_interest;
												lock(m_syncobject){
													is_interest	= m_is_interest;
												}
												return is_interest;
											}
									private set{
												lock(m_syncobject){
													m_is_interest	= value;
												}
											}
									}
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public gvo_chat(string path, sea_area _sea_area)
		{
			m_path						= path;
			m_sea_area					= _sea_area;

			m_newest_chat_file_info		= null;	// 更新が最も新しいログ
			m_chat_file_info			= null;	// 解析対象のログ
			m_analyze_lines				= 0;	// 解析状況

			m_accident					= accident.none;
			m_is_interest				= false;
			m_sea_area_type_list		= new List<sea_area_type>();
		}
	
		/*-------------------------------------------------------------------------
		 最も新しいログファイル名を得る
		---------------------------------------------------------------------------*/
		private void get_log_file_name()
		{
			m_newest_chat_file_info	= null;

			try{
				DirectoryInfo	info	= new DirectoryInfo(m_path);
				if(!info.Exists)		return;		// ログフォルダが存在しない

				// ファイル一覧を得る
				// HTML
				FileInfo[]	file_info		= info.GetFiles("*.html");
				FileInfo	newest_finfo	= null;
				if(file_info.Length > 0){
					newest_finfo	= file_info[0];
					foreach(FileInfo i in file_info){
						if(i.LastWriteTime > newest_finfo.LastWriteTime){
							newest_finfo	= i;
						}
					}
				}
		
				// TXT
				file_info		= info.GetFiles("*.txt");
				if(file_info.Length > 0){
					if(newest_finfo == null){
						newest_finfo	= file_info[0];
					}
					foreach(FileInfo i in file_info){
						if(i.LastWriteTime > newest_finfo.LastWriteTime){
							newest_finfo	= i;
						}
					}
				}
				
				if(newest_finfo != null){
					m_newest_chat_file_info	= newest_finfo;
				}
			}catch{
				// 失敗
			}
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
				using(FileStream s	= File.OpenRead(m_newest_chat_file_info.FullName)){
				}
			}catch{
				// Share.Readで開けない
				// 大航海時代Onlineによってロックされていると判断する
				Debug.WriteLine("locked log file.[" + m_newest_chat_file_info.FullName + " ]");
				return true;
			}
			// ロックされていないので最新ログを取得する必要あり
			Debug.WriteLine("unlocked log file.");
			return false;
		}
	
		/*-------------------------------------------------------------------------
		 ログ解析
		 リクエストがあるときのみ解析する

		 スレッドに以降したため、削除
		---------------------------------------------------------------------------*/
/*		public void AnalyzeChatLog_DoRequest()
		{
			if(IsRequest()){
				// 解析
//				AnalyzeChatLog();
				// 海域変動システムを更新する
				update_sea_area();
			}
		}
*/	
		/*-------------------------------------------------------------------------
		 海域変動の更新
		 リクエストがあるときのみ解析する
		---------------------------------------------------------------------------*/
		public void UpdateSeaArea_DoRequest()
		{
			if(IsRequest()){
				update_sea_area();
			}
		}
	
		/*-------------------------------------------------------------------------
		 ログ解析
		 複数の災害ログがあっても、最後のものだけが有効とする
		 (たくさんのポップアップが1度に生まれる必要がないため)
		---------------------------------------------------------------------------*/
		public void AnalyzeChatLog()
		{
			// 災害なし
			_accident					= accident.none;
			// 利息が来たかどうかはここではクリアされない
			// 海域が変動したかどうかはここではクリアされない

			// 更新をチェック
			if(!check_update_log())		return;

			// 更新された部分を解析
			do_analize();
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
				// 最も新しいログを得る
				get_log_file_name();
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
		 解析する
		---------------------------------------------------------------------------*/
		private void do_analize()
		{
			// どうもそのまま開けないのでコピーする
			try{
				File.Copy(m_chat_file_info.FullName, TEMP_FNAME, true);
			}catch{
				// コピー失敗
				return;
			}

			string	line		= "";
			int		line_count	= 0;

			// 
			string ex	= Path.GetExtension(m_chat_file_info.FullName);
			if(ex == ".txt"){
				// TXT形式
				try{
					using (StreamReader	sr	= new StreamReader(
						TEMP_FNAME,
						Encoding.GetEncoding("Shift_JIS"))) {
						while((line = sr.ReadLine()) != null){
							if(line_count++ < m_analyze_lines)	continue;	// 解析済みの行
							analyze_line(line);
						}
						// 解析した場所を覚えておく
						m_analyze_lines	= line_count;
					}
				}catch{
					// 失敗
				}
			}else{
				// html形式
				try{
					using (StreamReader	sr	= new StreamReader(
						TEMP_FNAME,
						Encoding.UTF8)) {
						while((line = sr.ReadLine()) != null){
							if(line.Length < 22)				continue;	
							if(line_count++ < m_analyze_lines)	continue;	// 解析済みの行
							line	= line.Substring(22);
							analyze_line(line);
						}
						// 解析した場所を覚えておく
						m_analyze_lines	= line_count;
					}
				}catch{
					// 失敗
				}
			}
			// 終わったら消しておく
			file_ctrl.RemoveFile(TEMP_FNAME);
		}
	
		/*-------------------------------------------------------------------------
		 解析
		---------------------------------------------------------------------------*/
		private void analyze_line(string line)
		{
			// 災害
			if(analyze_accident(line))	return;
			// 預金
			if(analyze_interest(line))	return;
			// 海域変動システム
			if(analize_sea_area(line))	return;
		}

		/*-------------------------------------------------------------------------
		 災害があったかどうかを調べる
		---------------------------------------------------------------------------*/
		private bool analyze_accident(string line)
		{
			// 検索対象
			string[]	accident_tbl	= new string[]{
				"船員がサメに襲われています",										// 101
				"人喰いザメが現れました！",											// 101
				"火災が発生しました！",												// 102
				"藻が舵に絡まっています！",											// 103
				"気味の悪い歌声が聞こえてきました",									// 104
				"磁場が狂っています。羅針盤が使いものになりません",				// 105
				"嵐が来ました！　帆を広げていると転覆してしまいます！",			// 106
				"吹雪になりました！　帆を広げていると凍りついてしまいます",		// 107
				"ネズミが大量発生しました！",										// 108
				"得体の知れない怪物が現れました",									// 109
				"このあたりで何かいい物が見つかるかもしれません",					// 111
				"このあたりで何か見つかるかもしれません",							// 111
				"このあたりで高価なものが見つかるかもしれません",					// 111
//				"バザールを開始しました",
//				"バザールを中止しました",
//				"出展内容を変更しました",
				"全船が戦場を離れました",												// 110
			};

			int		index	= 0;
			foreach(string s in accident_tbl){
				if(line.IndexOf(s) == 0){
					// 見つかった
					_accident	= accident.shark1 + index;
					Debug.WriteLine("災害 " + s);
					return true;
				}
				index++;
			}
	
			// 戦闘の勝利、敗北のみ0始まりの縛りを入れない
			if(line.IndexOf("に勝利しました！") >= 0){
				_accident	= accident.win_battle;
				Debug.WriteLine("災害 勝利しました");
				return true;
			}
			if(line.IndexOf("に敗北しました…") >= 0){
				_accident	= accident.lose_battle;
				Debug.WriteLine("敗北しました…");
				return true;
			}
			return false;
		}

		/*-------------------------------------------------------------------------
		 預金が来たかどうかを解析する
		---------------------------------------------------------------------------*/
		private bool analyze_interest(string line)
		{
			// 利息
			if(line.IndexOf("預金の利息を") == 0){
				is_interest		= true;
				Debug.WriteLine("利息受け取り");
				return true;
			}
			return false;
		}
	
		/*-------------------------------------------------------------------------
		 海域変動システムが変動したかどうかを解析する
		---------------------------------------------------------------------------*/
		private bool analize_sea_area(string line)
		{
			lock(m_syncobject){
				foreach(sea_area.sea_area_once d in m_sea_area.groups){
					if(line.IndexOf(d.name + "が危険海域に戻りました！") == 0){
						Debug.WriteLine(d.name + "が危険海域に戻りました！");
						m_sea_area_type_list.Add(new sea_area_type(d.name, sea_area.sea_area_once.sea_type.normal));
						return true;
					}
					if(line.IndexOf(d.name + "が安全海域となりました！") == 0){
						Debug.WriteLine(d.name + "が安全海域となりました！");
						m_sea_area_type_list.Add(new sea_area_type(d.name, sea_area.sea_area_once.sea_type.safty));
						return true;
					}
					if(line.IndexOf(d.name + "が無法海域となりました！") == 0){
						Debug.WriteLine(d.name + "が無法海域となりました！");
						m_sea_area_type_list.Add(new sea_area_type(d.name, sea_area.sea_area_once.sea_type.lawless));
						return true;
					}
				}
			}
			return false;
		}
	
		/*-------------------------------------------------------------------------
		 全てリセットする
		---------------------------------------------------------------------------*/
		public void ResetAll()
		{
			ResetAccident();
			ResetInterest();
			reset_sea_area();
		}
	
		/*-------------------------------------------------------------------------
		 災害情報をリセットする
		---------------------------------------------------------------------------*/
		public void ResetAccident()
		{
			_accident				= accident.none;
//			Debug.WriteLine("災害リセット");
		}

		/*-------------------------------------------------------------------------
		 利息が来たかどうかをリセットする
		---------------------------------------------------------------------------*/
		public void ResetInterest()
		{
			is_interest				= false;
//			Debug.WriteLine("利息受け取りリセット");
		}

		/*-------------------------------------------------------------------------
		 海域変動をリセットする
		---------------------------------------------------------------------------*/
		private void reset_sea_area()
		{
			lock(m_syncobject){
				m_sea_area_type_list.Clear();
			}
			Debug.WriteLine("海域変動リセット");
		}

		/*-------------------------------------------------------------------------
		 災害情報から保存用の値に変換する
		---------------------------------------------------------------------------*/
		static public int ToIndex(accident a)
		{
			switch(a){
			case accident.shark1:				// サメ1
			case accident.shark2:				// サメ2
				return 101;
			case accident.fire:					// 火災
				return 102;
			case accident.seaweed:				// 藻
				return 103;
			case accident.seiren:				// セイレーン
				return 104;
			case accident.compass:				// 羅針盤
				return 105;
			case accident.storm:				// 嵐
				return 106;
			case accident.blizzard:				// 吹雪
				return 107;
			case accident.mouse:				// ネズミ
				return 108;
			case accident.UMA:					// 得体の知れない怪物
				return 109;
			case accident.treasure1:			// 何かいい物
			case accident.treasure2:			// 何か見つかるかも
			case accident.treasure3:			// 高価なもの
				return 111;
			case accident.escape_battle:		// 全船が戦場を離れました
			case accident.win_battle:			// 勝利
			case accident.lose_battle:			// 敗北
				return 110;
			}
			return -1;		// unknown
		}

		/*-------------------------------------------------------------------------
		 海域変動システムを更新する
		---------------------------------------------------------------------------*/
		private void update_sea_area()
		{
			lock(m_syncobject){
				foreach(sea_area_type d in m_sea_area_type_list){
					m_sea_area.SetType(d.name, d.type);
				}
			}
			Debug.WriteLine("海域変動反映");
			// リセット
			reset_sea_area();
		}
	}
}
