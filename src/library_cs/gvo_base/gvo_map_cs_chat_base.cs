/*-------------------------------------------------------------------------

 チャット解析
 交易MapC#向け
 リクエスト付き
 預金の利息は災害とは独立して解析される
 危険海域変動システムも独立して解析される
 アクシデントは解析時の最後のもののみ

 スレッド対応

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

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvo_base
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class gvo_map_cs_chat_base : gvo_chat_base
	{
		// 海域変動状況
		public enum sea_type{
			normal,		// 通常
			safty,		// 安全化
			lawless,	// 無法化
		};

		/*-------------------------------------------------------------------------
		 海域変動用
		---------------------------------------------------------------------------*/
		public class sea_area_type
		{
			private string						m_name;
			private sea_type					m_type;

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public string		name			{	get{	return m_name;		}}
			public sea_type		type			{	get{	return m_type;		}}

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public sea_area_type(string name, sea_type type)
			{
				m_name		= name;
				m_type		= type;
			}
		}
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
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

			// 以下はアクシデントではないがtagとして使用される
			interest,			// 利息
			sea_type_normal,	// 海域変動 通常
			sea_type_safty,		// 海域変動 安全化
			sea_type_lawless,	// 海域変動 無法化

			buildship_start,	// 造船開始
			buildship_finish,	// 造船完了
		};
	
		private accident					m_accident;					// 今回解析した災害
		private bool						m_is_interest;				// 利息が来たらtrue
		private List<sea_area_type>			m_sea_area_type_list;		// 海域変動システム用

		// 造船
		private bool						m_is_start_build_ship;
		private string						m_build_ship_name;
		private bool						m_is_finish_build_ship;
	
		// スレッド対応
		private readonly object				m_syncobject	= new object();

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public accident _accident
		{
			get{
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
		public bool is_interest
		{
			get{
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
		public sea_area_type[] sea_area_type_list
		{
			get{
				lock(m_syncobject){
					return m_sea_area_type_list.ToArray();
				}
			}
		}
		public bool is_start_build_ship
		{
			get{
				lock(m_syncobject){
					return m_is_start_build_ship;
				}
			}
			set{
				lock(m_syncobject){
					m_is_start_build_ship	= value;
				}
			}
		}
		public string build_ship_name
		{
			get{
				lock(m_syncobject){
					return m_build_ship_name;
				}
			}
			set{
				lock(m_syncobject){
					m_build_ship_name	= value;
				}
			}
		}
		public bool is_finish_build_ship
		{
			get{
				lock(m_syncobject){
					return m_is_finish_build_ship;
				}
			}
			set{
				lock(m_syncobject){
					m_is_finish_build_ship	= value;
				}
			}
		}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public gvo_map_cs_chat_base()
			: base()
		{
			init();
		}
		public gvo_map_cs_chat_base(string path)
			: base(path)
		{
			init();
		}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		private void init()
		{
			m_accident					= accident.none;
			m_is_interest				= false;
			m_sea_area_type_list		= new List<sea_area_type>();

			// 解析対象を設定する
			init_analyze_list();

			ResetAll();
		}

		/*-------------------------------------------------------------------------
		 解析対象を設定する
		---------------------------------------------------------------------------*/
		private void init_analyze_list()
		{
			// 解析ルールを初期化する
			base.ResetAnalizedList();

			// index0の項目
			base.AddAnalizeList(@"船員がサメに襲われています", type.index0, accident.shark1);
			base.AddAnalizeList(@"人喰いザメが現れました！", type.index0, accident.shark2);
			base.AddAnalizeList(@"火災が発生しました！", type.index0, accident.fire);
			base.AddAnalizeList(@"藻が舵に絡まっています！", type.index0, accident.seaweed);
			base.AddAnalizeList(@"気味の悪い歌声が聞こえてきました", type.index0, accident.seiren);
			base.AddAnalizeList(@"磁場が狂っています。羅針盤が使いものになりません", type.index0, accident.compass);
			base.AddAnalizeList(@"嵐が来ました！　帆を広げていると転覆してしまいます！", type.index0, accident.storm);
			base.AddAnalizeList(@"吹雪になりました！　帆を広げていると凍りついてしまいます", type.index0, accident.blizzard);
			base.AddAnalizeList(@"ネズミが大量発生しました！", type.index0, accident.mouse);
			base.AddAnalizeList(@"得体の知れない怪物が現れました", type.index0, accident.UMA);
			base.AddAnalizeList(@"このあたりで何かいい物が見つかるかもしれません", type.index0, accident.treasure1);
			base.AddAnalizeList(@"このあたりで何か見つかるかもしれません", type.index0, accident.treasure2);
			base.AddAnalizeList(@"このあたりで高価なものが見つかるかもしれません", type.index0, accident.treasure3);
			base.AddAnalizeList(@"全船が戦場を離れました",	 type.index0, accident.escape_battle);

			// 
			base.AddAnalizeList(@"に勝利しました！", type.any_index, accident.win_battle);
			base.AddAnalizeList(@"に敗北しました…", type.any_index, accident.lose_battle);
	
			// 利息
			base.AddAnalizeList_Interest(accident.interest);

			// 海域変動
			base.AddAnalizeList(@"^(.+)が危険海域に戻りました！", type.regex, accident.sea_type_normal);
			base.AddAnalizeList(@"^(.+)が安全海域となりました！", type.regex, accident.sea_type_safty);
			base.AddAnalizeList(@"^(.+)が無法海域となりました！", type.regex, accident.sea_type_lawless);

			// 造船
			base.AddAnalizeList(@"^(.+)の建造を注文しました", type.regex, accident.buildship_start);
			base.AddAnalizeList(@"^(.+)の強化を依頼しました", type.regex, accident.buildship_start);
			base.AddAnalizeList(@"」が無事進水しました", type.any_index, accident.buildship_finish);
		}
		
		/*-------------------------------------------------------------------------
		 ログ解析
		 複数の災害ログがあっても、最後のものだけが有効とする
		 (たくさんのポップアップが1度に生まれる必要がないため)
		---------------------------------------------------------------------------*/
		public override bool AnalyzeNewestChatLog()
		{
			// 災害なし
			_accident							= accident.none;

			// 利息が来たかどうかはここではクリアされない
			// 海域が変動したかどうかはここではクリアされない

			// 最新ログを解析
			if(!base.AnalyzeNewestChatLog())	return false;
			// 更新されたかチェック
			if(!base.is_update)					return true;

			// 解析内容をチェックする
			update_analyze();
			return true;
		}

		/*-------------------------------------------------------------------------
		 解析内容をチェックする
		---------------------------------------------------------------------------*/
		private void update_analyze()
		{
			foreach(gvo_chat_base.analized_data d in base.analized_list){
				switch((accident)d.tag){
				case accident.shark1:			// サメ1
				case accident.shark2:			// サメ2
				case accident.fire:				// 火災
				case accident.seaweed:			// 藻
				case accident.seiren:			// セイレーン
				case accident.compass:			// 羅針盤
				case accident.storm:			// 嵐
				case accident.blizzard:			// 吹雪
				case accident.mouse:			// ネズミ
				case accident.UMA:				// 得体の知れない怪物
				case accident.treasure1:		// 何かいい物
				case accident.treasure2:		// 何か見つかるかも
				case accident.treasure3:		// 高価なもの
				case accident.escape_battle:	// 全船が戦場を離れました
				case accident.win_battle:		// 勝利
				case accident.lose_battle:		// 敗北
					Debug.WriteLine(d.line);
					_accident		= (accident)d.tag;	// 単純に上書き
					break;
				case accident.interest:			// 利息
					Debug.WriteLine(d.line);
					m_is_interest	= true;
					break;
				case accident.sea_type_normal:	// 海域変動 通常
					lock(m_syncobject){
						m_sea_area_type_list.Add(new sea_area_type(d.match.Groups[1].Value, sea_type.normal));
					}
					Debug.WriteLine(d.line);
					break;
				case accident.sea_type_safty:	// 海域変動 安全化
					lock(m_syncobject){
						m_sea_area_type_list.Add(new sea_area_type(d.match.Groups[1].Value, sea_type.safty));
					}
					Debug.WriteLine(d.line);
					break;
				case accident.sea_type_lawless:	// 海域変動 無法化
					lock(m_syncobject){
						m_sea_area_type_list.Add(new sea_area_type(d.match.Groups[1].Value, sea_type.lawless));
					}
					Debug.WriteLine(d.line);
					break;
				case accident.buildship_start:	// 造船開始
					is_start_build_ship		= true;
					build_ship_name			= d.match.Groups[1].Value;
					break;
				case accident.buildship_finish:	// 造船完了
					is_finish_build_ship	= true;
					break;
				default:
					break;
				}
			}
		}

		/*-------------------------------------------------------------------------
		 全てリセットする
		---------------------------------------------------------------------------*/
		public void ResetAll()
		{
			ResetAccident();
			ResetInterest();
			ResetSeaArea();
			ResetBuildShip();
		}
	
		/*-------------------------------------------------------------------------
		 災害情報をリセットする
		---------------------------------------------------------------------------*/
		public void ResetAccident()
		{
			_accident				= accident.none;
		}

		/*-------------------------------------------------------------------------
		 利息が来たかどうかをリセットする
		---------------------------------------------------------------------------*/
		public void ResetInterest()
		{
			is_interest				= false;
		}

		/*-------------------------------------------------------------------------
		 海域変動をリセットする
		---------------------------------------------------------------------------*/
		public void ResetSeaArea()
		{
			lock(m_syncobject){
				m_sea_area_type_list.Clear();
			}
		}

		/*-------------------------------------------------------------------------
		 造船情報をリセットする
		---------------------------------------------------------------------------*/
		public void ResetBuildShip()
		{
			m_is_start_build_ship	= false;
			m_build_ship_name		= "";
			m_is_finish_build_ship	= false;
		}

		/*-------------------------------------------------------------------------
		 海域変動状況変換
		---------------------------------------------------------------------------*/
		static public string ToSeaTypeString(sea_type type)
		{
			switch(type){
			case sea_type.safty:		return "安全";
			case sea_type.lawless:		return "無法";
			default:					return "通常";
			}
		}
		static public sea_type ToSeaType(string type)
		{
			switch(type){
			case "安全":				return sea_type.safty;
			case "無法":				return sea_type.lawless;
			default:					return sea_type.normal;
			}
		}

		/*-------------------------------------------------------------------------
		 アクシデントの変換
		---------------------------------------------------------------------------*/
		static public string ToAccidentString(accident __accident)
		{
			switch(__accident){
			case accident.shark1:			return "サメ1";
			case accident.shark2:			return "サメ2";
			case accident.fire:				return "火災";
			case accident.seaweed:			return "藻";
			case accident.seiren:			return "セイレーン";
			case accident.compass:			return "羅針盤";
			case accident.storm:			return "嵐";
			case accident.blizzard:			return "吹雪";
			case accident.mouse:			return "ネズミ";
			case accident.UMA:				return "怪物";
			case accident.treasure1:		return "何かいい物";
			case accident.treasure2:		return "何か見つかるかも";
			case accident.treasure3:		return "高価なもの";
			case accident.escape_battle:	return "海戦離脱";
			case accident.win_battle:		return "海戦勝利";
			case accident.lose_battle:		return "海戦敗北";
			default:						return "なし";
			}
		}
		static public accident ToAccident(string str)
		{
			switch(str){
			case "サメ1":				return accident.shark1;
			case "サメ2":				return accident.shark2;
			case "火災":				return accident.fire;
			case "藻":					return accident.seaweed;
			case "セイレーン":			return accident.seiren;
			case "羅針盤":				return accident.compass;
			case "嵐":					return accident.storm;
			case "吹雪":				return accident.blizzard;
			case "ネズミ":				return accident.mouse;
			case "怪物":				return accident.UMA;
			case "何かいい物":			return accident.treasure1;
			case "何か見つかるかも":	return accident.treasure2;
			case "高価なもの":			return accident.treasure3;
			case "海戦離脱":			return accident.escape_battle;
			case "海戦勝利":			return accident.win_battle;
			case "海戦敗北":			return accident.lose_battle;
			default:					return accident.none;
			}
		}
	}
}
