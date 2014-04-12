/*-------------------------------------------------------------------------

 定数定義

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public static partial class def
	{
		// HPの元
		public const string		URL_HP						= @"http://www.geocities.jp/cookiezephyros/";
	
		// 更新確認用
		static public int		VERSION						= 1323002;
		static public string	VERSION_URL					= URL_HP + @"download/gvtrademap_cs_version.dat";
		static public string	VERSION_FNAME				= "version.txt";
		static public string	DOWNLOAD_URL				= URL_HP + @"gvtrademap.html";

		// Window title 及びバージョン情報
//		public const string		WINDOW_TITLE				= "大航海時代Online 交易MAP C# ver.1.22β1";
		public const string		WINDOW_TITLE				= "大航海時代Online 交易MAP C# ver.1.32.3+2";

		// 海域変動収集アプリケーション
		public const string		SEAAREA_APP_FNAME			= "gvoac.exe";
	
		// パス
		public const string		DB_PATH						= "database\\";
		public const string		MAP_PATH					= "map\\";
		public const string		SEAROUTE_PATH				= "temp\\";
		public const string		MEMO_PATH					= "memo\\";
		public const string		SS_PATH						= "SS\\";

		// ファイル名(パス含む)
		public const string		WEB_ICONS_FULLFNAME			= DB_PATH + "webicons.txt";
		public const string		ITEMDB_FULLNAME				= DB_PATH + "item_db.txt";
		public const string		INFO_FULLNAME				= DB_PATH + "cityinfos.xml";
		public const string		ITEM_TYPE_DB				= DB_PATH + "itemtype_db.xml";
		public const string		WORLDINFOS_FULLNAME			= DB_PATH + "worldinfos.xml";
		public const string		SHIP_PARTS_FULLNAME			= DB_PATH + "ShipParts.xml";
		public const string		NEW_DOMAINS_INDEX_FULLFNAME			= DB_PATH + "domaininfo.xml";
		public const string		LOCAL_NEW_DOMAINS_INDEX_FULLFNAME	= SEAROUTE_PATH + "domaininfo.xml";
		public const string		SEAROUTE_FULLFNAME			= SEAROUTE_PATH + "searoute.txt";
		public const string		FAVORITE_SEAROUTE_FULLFNAME	= SEAROUTE_PATH + "favorite_searoute.txt";
		public const string		TRASH_SEAROUTE_FULLFNAME	= SEAROUTE_PATH + "trash_searoute.txt";
		public const string		SEAROUTE_FULLFNAME1			= SEAROUTE_PATH + "searoute_temp.txt";
		public const string		SEAROUTE_FULLFNAME2			= SEAROUTE_PATH + "searoute_temp2.txt";
		public const string		SEAAREA_FULLFNAME			= SEAROUTE_PATH + "seaarea.txt";
		public const string		MEMO_ICONS_FULLFNAME		= MAP_PATH + "mapmark.txt";

      /*
		public const string		DOMAINS_INFO_FULLFNAME		= INFO_PATH + "domaininfo.txt";
		public const string		DOMAINS_INDEX_FULLFNAME		= MAP_PATH + "domainsindex.txt";
		public const string		SEA_INFO_FULL_NAME			= MAP_PATH + "seainfo.txt";
		public const string		FISHING_RANKS_FULLFNAME		= INFO_PATH + "釣りR.txt";
		public const string		COLLECT_RANKS_FULLFNAME		= INFO_PATH + "採集R.txt";
		public const string		SUPPLY_RANKS_FULLFNAME		= INFO_PATH + "調達R.txt";
		public const string		BONUS_ITEMS_FULLFNAME		= INFO_PATH + "特産品.txt";
		public const string		NANBAN_TRADE_ITEMS_FULLFNAME	= INFO_LOCAL_PATH + "南蛮品.txt";
        */

		// 設定ファイル
		public const string		INI_FNAME					= "gvtrademap_cs.ini";

		// 地図ファイル(パス含む)
		public const string		MAP_FULLFNAME1				= MAP_PATH + "worldmap_r.png";
		public const string		MAP_FULLFNAME2				= MAP_PATH + "worldmap_c.png";
		public const string		MIX_MAP_FULLFNAME1			= MAP_PATH + "worldmap_r_mix.png";
		public const string		MIX_MAP_FULLFNAME2			= MAP_PATH + "worldmap_c_mix.png";

		public const string		FAVORITEROUTE_FULLFNAME		= MAP_PATH + "favoriteroute.bmp";
		public const string		MAP_MASK_FULLFNAME			= MAP_PATH + "worldmap_mask.png";

		public const string		INFONAMEIMAGE_FULLNAME		= MAP_PATH + "infonames.dds";
		public const string		SEAINFONAMEIMAGE_FULLNAME	= MAP_PATH + "seainfonames.dds";
		public const string		ICONSIMAGE_FULLNAME			= MAP_PATH + "gv_pics20.dds";

		// スプライト描画用シェーダ
		// シェーダは埋め込みリソース
		public const string		SPRITE_SHADER_RESOURCENAME	= "gvtrademap_cs.fx.sprite.fxo";
	
		// 詳細情報ページ
		public const string		URL0						= "dol.egret.jp/gtf/?page=city&id=";
		public const string		URL1						= "www2.atwiki.jp/harrington/pages/";
//		public const string		URL4						= @"http://showroom256.hp.infoseek.co.jp/cgi-bin/read.cgi";
		public const string		URL4						= @"http://jbbs.livedoor.jp/netgame/7161/";
		public const string		URL5						= @"wiki.livedoor.jp/kristall/d/";
		public const string		URL6						= @"http://www.microsoft.com/downloads/details.aspx?displaylang=ja&FamilyID=2da43d38-db71-4c1b-bc6a-9b6652cd92a3";

		public const string		URL_HP_ORIGINAL				= @"http://gvtrademap.daa.jp/";

		// ゲーム内座標の限界
		public const int		GAME_WIDTH					= 16384;
		public const int		GAME_HEIGHT					= 8192;

		// 税率
		public const float		TAX							= 0.143f;

		// アイコンサイズ
		public const int		ICON_SIZE_X					= 16;
		public const int		ICON_SIZE_Y					= 16;

		// メイリオフォントのデフォルトサイズ
		// MS ゴシック UI と同じくらいのサイズになるような値
		public const float		MEIRYO_POINT				= 8f;

		// 描画スキップの最大
		public const int		SKIP_DRAW_FRAME_MAX			= 5;

		// 接続先ポート番号
		public const int		DEFALUT_PORT_INDEX			= 16612;
	}
}
