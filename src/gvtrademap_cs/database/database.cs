/*-------------------------------------------------------------------------

 情報管理

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using gvo_base;
using Utility;
using System.IO;
using Microsoft.DirectX;
using System.Diagnostics;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class database : IDisposable
	{
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		private GvoWorldInfo			m_world_info;		// 世界の情報
		private gvo_capture			m_capture;			// キャプチャ
		private SeaRoutes			m_searoute;			// 航路図
		private speed_calculator	m_speed;			// 速度
		private ShareRoutes		m_share_routes;		// 航路共有
		private WebIcons			m_web_icons;		// @web icon
		private gvo_chat			m_gvochat;			// ログ解析
		private interest_days		m_interest_days;	// 利息からの経過日数
		private gvo_build_ship_counter	m_build_ship_counter;	// 造船日数管理
		private map_mark			m_map_mark;			// メモアイコン
		private ItemDatabaseCustom		m_item_database;	// アイテムデータベース
		private sea_area			m_sea_area;			// 危険海域変動システム
		private gvo_season			m_season;			// 季節チェック
	
		private gvt_lib				m_lib;

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public GvoWorldInfo world				{	get{	return m_world_info;		}}
		public gvo_capture capture			{	get{	return m_capture;			}}
		public SeaRoutes searoute			{	get{	return m_searoute;			}}
		public speed_calculator speed		{	get{	return m_speed;				}}
		public ShareRoutes share_routes	{	get{	return m_share_routes;		}}
		public WebIcons web_icons			{	get{	return m_web_icons;			}}
		public gvo_chat gvochat				{	get{	return m_gvochat;			}}
		public interest_days interest_days	{	get{	return m_interest_days;		}}
		public gvo_build_ship_counter build_ship_counter	{	get{	return m_build_ship_counter;	}}
		public map_mark map_mark			{	get{	return m_map_mark;			}}
		public ItemDatabaseCustom item_database	{	get{	return m_item_database;		}}
		public sea_area sea_area			{	get{	return m_sea_area;			}}
		public gvo_season season			{	get{	return m_season;			}}
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public database(gvt_lib lib)
		{
			m_lib						= lib;

			// 季節チェック
			m_season					= new gvo_season();

			// 世界の情報
			m_world_info				= new GvoWorldInfo(	lib,
															m_season,
															def.WORLDINFO_FULLFNAME,
															def.DOMAINS_INDEX_FULLFNAME,
															def.MEMO_PATH);

			// アイテムデータベース
			m_item_database				= new ItemDatabaseCustom(def.ITEMDB_FULLNAME);

			// 速度
			m_speed						= new speed_calculator(def.GAME_WIDTH);

			// 航路図
			m_searoute					= new SeaRoutes(	lib,
															def.SEAROUTE_FULLFNAME,
															def.FAVORITE_SEAROUTE_FULLFNAME,
															def.TRASH_SEAROUTE_FULLFNAME);

			// @web icons
			m_web_icons					= new WebIcons(lib);
			// メモアイコン
			m_map_mark					= new map_mark(lib, def.MEMO_ICONS_FULLFNAME);
			// 航路共有
			m_share_routes				= new ShareRoutes(lib);
			// 画面キャプチャ
			m_capture					= new gvo_capture(lib);
	
			// 利息からの経過日数
			m_interest_days				= new interest_days(lib.setting);
			// 造船日数管理
			m_build_ship_counter		= new gvo_build_ship_counter(lib.setting);

			// 危険海域変動システム
			m_sea_area					= new sea_area(lib, def.SEAAREA_FULLFNAME);

			// ログ解析
			m_gvochat					= new gvo_chat(m_sea_area);
			// 1度ログ解析をしておく
			// 解析内容は捨てる
			m_gvochat.AnalyzeNewestChatLog();
			m_gvochat.ResetAll();
		}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public void Dispose()
		{
			if(m_world_info != null)	m_world_info.Dispose();
			if(m_sea_area != null)		m_sea_area.Dispose();
			if(m_capture != null)		m_capture.Dispose();

			m_world_info	= null;
			m_sea_area		= null;
			m_capture		= null;
		}
	
		/*-------------------------------------------------------------------------
		 設定項目の書き出し
		---------------------------------------------------------------------------*/
		public void WriteSettings()
		{
			// 航路図の書き出し
			m_searoute.Write(def.SEAROUTE_FULLFNAME);
			// お気に入り航路図の書き出し
			m_searoute.WriteFavorite(def.FAVORITE_SEAROUTE_FULLFNAME);
			// ごみ箱航路図の書き出し
			m_searoute.WriteTrash(def.TRASH_SEAROUTE_FULLFNAME);

			// 詳細情報書き出し
			m_world_info.Write(def.INFO_PATH);

			// メモ書き出し
			m_world_info.WriteMemo(def.MEMO_PATH);

			// メモアイコン書き出し
			m_map_mark.Write(def.MEMO_ICONS_FULLFNAME);

			// 危険海域変動システム情報書き出し
			m_sea_area.WriteSetting(def.SEAAREA_FULLFNAME);
		}

		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		public void Draw() 
		{
			DrawForScreenShot();
			// 共有船描画
			m_share_routes.Draw();
		}

		/*-------------------------------------------------------------------------
		 スクリーンショット用描画
		---------------------------------------------------------------------------*/
		public void DrawForScreenShot()
		{
			// @web icons
			m_web_icons.Draw();
			// 航路図描画
			m_searoute.DrawRoutesLines();

			if(!m_lib.setting.is_mixed_info_names){
				// 海域名と風向き描画
				m_world_info.DrawSeaName();
				// 街名と上陸地点名描画
				m_world_info.DrawCityName();
			}

			// 吹き出し描画
			m_searoute.DrawPopups();

			// メモアイコン
			m_map_mark.Draw();
		}

		/*-------------------------------------------------------------------------
		 地図と街名等の合成用
		 街名を合成しない場合は海域変動システムのみ合成する
		---------------------------------------------------------------------------*/
		public void DrawForMargeInfoNames(Vector2 draw_offset, LoopXImage image)
		{
			// 海域変動システム
			m_sea_area.Draw();

			if(m_lib.setting.is_mixed_info_names){
				// 海域名と風向き描画
				m_world_info.DrawSeaName();
				// 街名と上陸地点名描画
				m_world_info.DrawCityName();
			}
		}

		/*-------------------------------------------------------------------------
		 できるだけ検索
		---------------------------------------------------------------------------*/
		public List<find> FindAll(string find_string)
		{
			List<find>	list	= new List<find>();

			find.FindHandler	handler;

			// 検索方法
			// 部分一致等
			switch(m_lib.setting.find_filter3){
			case _find_filter3.full_match:
				handler		= new find.FindHandler(find.FindHander2);
				break;
			case _find_filter3.prefix_search:
				handler		= new find.FindHandler(find.FindHander3);
				break;
			case _find_filter3.suffix_search:
				handler		= new find.FindHandler(find.FindHander4);
				break;
			case _find_filter3.full_text_search:
			default:
				handler		= new find.FindHandler(find.FindHander1);
				break;
			}
			
			// 検索
			if(m_lib.setting.find_filter2 == _find_filter2.name){
				// 名称等から検索
				// 世界の情報から検索
				if(m_lib.setting.find_filter == _find_filter.both
					|| m_lib.setting.find_filter == _find_filter.world_info){
					world.FindAll(find_string, list, handler);
				}
				// アイテムデータベースから検索
				if(m_lib.setting.find_filter == _find_filter.both
					|| m_lib.setting.find_filter == _find_filter.item_database){
					m_item_database.FindAll(find_string, list, handler);
				}
			}else{
				// 種類から検索
				// 世界の情報から検索
				if(m_lib.setting.find_filter == _find_filter.both
					|| m_lib.setting.find_filter == _find_filter.world_info){
					world.FindAll_FromType(find_string, list, handler);
				}
				// アイテムデータベースから検索
				if(m_lib.setting.find_filter == _find_filter.both
					|| m_lib.setting.find_filter == _find_filter.item_database){
					m_item_database.FindAll_FromType(find_string, list, handler);
				}
			}
			return list;
		}

		/*-------------------------------------------------------------------------
		 文化圏一覧を得る
		---------------------------------------------------------------------------*/
		public List<find> GetCulturalSphereList()
		{
			List<find>	list	= new List<find>();
			// 文化圏検索
			// 必ず特定の一覧が得られる
			world.CulturalSphereList(list);
			return list;
		}

		/*-------------------------------------------------------------------------
		 できるだけ検索
		---------------------------------------------------------------------------*/
		public class find
		{
			public enum find_type{
				data,				// アイテム
				data_price,			// アイテムの値段内
				database,			// アイテムデータベース
				info_name,			// 街名等
				lang,				// 使用言語
				cultural_sphere,	// 文化圏
				MAX
			};

			private find_type						m_type;				// 見つかった種類

			private GvoWorldInfo.Info.Group.Data	m_data;				// アイテム
			private ItemDatabaseCustom.Data				m_database;			// アイテムデータベース
			private string							m_info_name;		// 街名等
																		// スポット用に Type が database 以外は内容が入る
			private string							m_lang;				// 使用言語
			private GvoDomains.CulturalSphere		m_cultural_sphere;	// 文化圏検索時
			private string							m_cultural_sphere_tool_tip;

			// str1 に str2 が含まれるかどうかを返す
			public delegate bool FindHandler(string str1, string str2);

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public find_type type						{	get{	return m_type;		}}
			public GvoWorldInfo.Info.Group.Data data	{	get{	return m_data;		}}
			public ItemDatabaseCustom.Data database			{	get{	return m_database;	}}
			public string info_name						{	get{	return m_info_name;	}}
			public string lang							{	get{	return m_lang;		}}
			public GvoDomains.CulturalSphere cultural_sphere	{	get{	return m_cultural_sphere;	}}

			// リストへの表示用
			public string name_string{
				get{
					switch(type){
					case find_type.data:
						if(data == null)		break;
						return data.Name + "[" + data.Price + "]";
					case find_type.data_price:
						if(data == null)		break;
						return data.Price + "[" + data.Name + "]";
					case find_type.database:
						if(database == null)	break;
						return database.Name;
					case find_type.info_name:
						if(info_name == null)	break;
						return info_name;
					case find_type.lang:
						if(lang == null)		break;
						return lang;
					case find_type.cultural_sphere:
						return GvoDomains.GetCulturalSphereString(cultural_sphere);
					}
					return "不明";
				}
			}
			public string type_string{
				get{
					switch(type){
					case find_type.data:
						if(data == null)		break;
						if(data.ItemDb != null)	return data.ItemDb.Type;
						return data.GroupIndexString;
					case find_type.data_price:
						if(data == null)		break;
						if(data.ItemDb != null)	return data.ItemDb.Type + "[TAG]";
						return data.GroupIndexString + "[TAG]";
					case find_type.database:
						if(database == null)	break;
						return database.Type;
					case find_type.info_name:
						return "街名等";
					case find_type.lang:
						return "使用言語";
					case find_type.cultural_sphere:
						return "文化圏";
					}
					return "不明";
				}
			}
			public string spot_string{
				get{
					switch(type){
					case find_type.data:
					case find_type.data_price:
					case find_type.lang:
					case find_type.info_name:
						if(info_name == null)	break;
						return info_name;
					case find_type.database:
						return "アイテムデータベース";
					case find_type.cultural_sphere:
						return "";
					}
					return "不明";
				}
			}
			public string tool_tip{
				get{
					switch(type){
					case find_type.database:
						if(database == null)	break;
						return database.GetToolTipString();
					case find_type.data:
					case find_type.data_price:
						if(data == null)		break;
						return data.TooltipString;
					case find_type.info_name:
					case find_type.lang:
						break;
					case find_type.cultural_sphere:
						return m_cultural_sphere_tool_tip;
					}
					return "";
				}
			}
			
			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public find(string _info_name)
			{
				m_type				= find_type.info_name;

				m_data				= null;
				m_database			= null;
				m_info_name			= _info_name;
				m_lang				= null;
				m_cultural_sphere	= GvoDomains.CulturalSphere.unknown;
				m_cultural_sphere_tool_tip	= "";
			}
			public find(find_type _type, string _info_name, GvoWorldInfo.Info.Group.Data _data)
			{
				m_type				= _type;

				m_data				= _data;
				m_database			= null;
				if(m_data != null){
					m_database	= m_data.ItemDb;
				}
				m_info_name			= _info_name;
				m_lang				= null;
				m_cultural_sphere	= GvoDomains.CulturalSphere.unknown;
				m_cultural_sphere_tool_tip	= "";
			}
			public find(string _info_name, string _lang)
			{
				m_type				= find_type.lang;

				m_data				= null;
				m_database			= null;
				m_info_name			= _info_name;
				m_lang				= _lang;
				m_cultural_sphere	= GvoDomains.CulturalSphere.unknown;
				m_cultural_sphere_tool_tip	= "";
			}
			public find(ItemDatabaseCustom.Data _database)
			{
				m_type				= find_type.database;

				m_data				= null;
				m_database			= _database;
				m_info_name			= null;
				m_lang				= null;
				m_cultural_sphere	= GvoDomains.CulturalSphere.unknown;
				m_cultural_sphere_tool_tip	= "";
			}
			public find(GvoDomains.CulturalSphere cs, string tooltip_str)
			{
				m_type				= find_type.cultural_sphere;

				m_data				= null;
				m_database			= null;
				m_info_name			= GvoDomains.GetCulturalSphereString(cs);
				m_lang				= null;
				m_cultural_sphere	= cs;
				m_cultural_sphere_tool_tip	= tooltip_str;
			}

			/*-------------------------------------------------------------------------
			 一致判定
			 部分一致
			---------------------------------------------------------------------------*/
			static public bool FindHander1(string str1, string str2)
			{
				if(str1.IndexOf(str2) >= 0){
					return true;
				}
				return false;
			}

			/*-------------------------------------------------------------------------
			 一致判定
			 完全一致
			---------------------------------------------------------------------------*/
			static public bool FindHander2(string str1, string str2)
			{
				return str1 == str2;
			}

			/*-------------------------------------------------------------------------
			 一致判定
			 前方一致
			---------------------------------------------------------------------------*/
			static public bool FindHander3(string str1, string str2)
			{
				return str1.StartsWith(str2);
			}

			/*-------------------------------------------------------------------------
			 一致判定
			 後方一致
			---------------------------------------------------------------------------*/
			static public bool FindHander4(string str1, string str2)
			{
				return str1.EndsWith(str2);
			}
		}
	}
}
