/*-------------------------------------------------------------------------

 街とか海域とか上陸地点情報

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using directx;
using Utility;
using gvo_base;
using System.Diagnostics;
using Microsoft.DirectX;
using System.Xml;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class GvoWorldInfo : IDisposable
	{
		public enum InfoType{
			City,			// 街
			Sea,			// 海域
			Shore,			// 上陸地点
			Shore2,			// 上陸地点 奥地
			OutsideCity,	// 郊外
			PF,				// プライベートファーム
		};

        // サーバ
        public enum Server
        {
            Euros,
            Zephyros,
            Notos,
            Boreas,
            Unknown,
        };
        // 国
        public enum Country
        {
            Unknown,			// 所属無(補給港など)
            England,
            Spain,
            Portugal,
            Netherlands,
            France,
            Venezia,
            Turkey,
        };

        // 街の種類
        public enum CityType
        {
            Capital,			// 首都
            City,				// 街
            CapitalIslam,		// 首都(イスラム)
            CityIslam,			// 街(イスラム)
        };
        // 同盟の種類
        public enum AllianceType
        {
            Unknown,			// なし(補給港など)
            Alliance,			// 同盟
            Capital,			// 首都
            Territory,			// 領土
            Piratical,			// 海賊島
        };

        // 文化圏
        public enum CulturalSphere
        {
            Unknown,			// 不明
            NorthEurope,		// 北欧
            Germany,			// ドイツ
            Netherlands,		// ネーデルランド
            Britain,			// ブリテン島
            NorthFrance,		// フランス北部
            Iberian,			// イベリア
            Atlantic,			// 大西洋
            ItalySouthFrance,	// イタリア・南仏
            Balkan,				// バルカン
            Turkey,				// トルコ
            NearEast,			// 近東
            NorthAfrica,		// 北アフリカ
            WestAfrica,		// 西アフリカ
            EastAfrica,		// 東アフリカ
            Arab,				// アラブ
            Persia,				// ペルシャ
            India,				// インド
            Indochina,			// インドシナ
            SoutheastAsia,		// 東南アジア
            Oceania,			// オセアニア
            Caribbean,			// カリブ
            EastLatinAmerica,	// 中南米東岸
            WestLatinAmerica,	// 中南米西岸
            China,				// 華南
            Japan,				// 日本
            Taiwan,				// 台湾
            Korea,				// 朝鮮
            NorthAmerica,		// 北米
        };


        private static EnumParser<GvoWorldInfo.InfoType>[] m_infotype_enum_param = new EnumParser<GvoWorldInfo.InfoType>[6]
    {
      new EnumParser<GvoWorldInfo.InfoType>(GvoWorldInfo.InfoType.City, "街"),
      new EnumParser<GvoWorldInfo.InfoType>(GvoWorldInfo.InfoType.Sea, "海域"),
      new EnumParser<GvoWorldInfo.InfoType>(GvoWorldInfo.InfoType.Shore, "上陸地点"),
      new EnumParser<GvoWorldInfo.InfoType>(GvoWorldInfo.InfoType.Shore2, "上陸地点 奥地"),
      new EnumParser<GvoWorldInfo.InfoType>(GvoWorldInfo.InfoType.OutsideCity, "郊外"),
      new EnumParser<GvoWorldInfo.InfoType>(GvoWorldInfo.InfoType.PF, "プライベ\x30FCトファ\x30FCム")
    };
        private static EnumParser<GvoWorldInfo.Country>[] m_country_enum_param = new EnumParser<GvoWorldInfo.Country>[8]
    {
      new EnumParser<GvoWorldInfo.Country>(GvoWorldInfo.Country.Unknown, "所属無"),
      new EnumParser<GvoWorldInfo.Country>(GvoWorldInfo.Country.England, "イングランド"),
      new EnumParser<GvoWorldInfo.Country>(GvoWorldInfo.Country.Spain, "イスパニア"),
      new EnumParser<GvoWorldInfo.Country>(GvoWorldInfo.Country.Portugal, "ポルトガル"),
      new EnumParser<GvoWorldInfo.Country>(GvoWorldInfo.Country.Netherlands, "ネ\x30FCデルランド"),
      new EnumParser<GvoWorldInfo.Country>(GvoWorldInfo.Country.France, "フランス"),
      new EnumParser<GvoWorldInfo.Country>(GvoWorldInfo.Country.Venezia, "ヴェネツィア"),
      new EnumParser<GvoWorldInfo.Country>(GvoWorldInfo.Country.Turkey, "オスマントルコ")
    };
        private static EnumParser<GvoWorldInfo.CityType>[] m_citytype_enum_param = new EnumParser<GvoWorldInfo.CityType>[4]
    {
      new EnumParser<GvoWorldInfo.CityType>(GvoWorldInfo.CityType.Capital, "首都"),
      new EnumParser<GvoWorldInfo.CityType>(GvoWorldInfo.CityType.City, "街"),
      new EnumParser<GvoWorldInfo.CityType>(GvoWorldInfo.CityType.CapitalIslam, "首都(イスラム)"),
      new EnumParser<GvoWorldInfo.CityType>(GvoWorldInfo.CityType.CityIslam, "街(イスラム)")
    };
        private static EnumParser<GvoWorldInfo.AllianceType>[] m_alliancetype_enum_param = new EnumParser<GvoWorldInfo.AllianceType>[5]
    {
      new EnumParser<GvoWorldInfo.AllianceType>(GvoWorldInfo.AllianceType.Unknown, "同盟なし"),
      new EnumParser<GvoWorldInfo.AllianceType>(GvoWorldInfo.AllianceType.Alliance, "同盟国"),
      new EnumParser<GvoWorldInfo.AllianceType>(GvoWorldInfo.AllianceType.Capital, "本拠地"),
      new EnumParser<GvoWorldInfo.AllianceType>(GvoWorldInfo.AllianceType.Territory, "領地"),
      new EnumParser<GvoWorldInfo.AllianceType>(GvoWorldInfo.AllianceType.Piratical, "海賊島")
    };
        private static EnumParser<GvoWorldInfo.CulturalSphere>[] m_culturalsphere_enum_param = new EnumParser<GvoWorldInfo.CulturalSphere>[29]
    {
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Unknown, "不明"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.NorthEurope, "北欧"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Germany, "ドイツ"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Netherlands, "ネ\x30FCデルランド"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Britain, "ブリテン島"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.NorthFrance, "フランス北部"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Iberian, "イベリア"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Atlantic, "大西洋"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.ItalySouthFrance, "イタリア・南仏"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Balkan, "バルカン"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Turkey, "トルコ"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.NearEast, "近東"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.NorthAfrica, "北アフリカ"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.WestAfrica, "西アフリカ"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.EastAfrica, "東アフリカ"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Arab, "アラブ"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Persia, "ペルシャ"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.India, "インド"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Indochina, "インドシナ"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.SoutheastAsia, "東南アジア"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Oceania, "オセアニア"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Caribbean, "カリブ"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.EastLatinAmerica, "中南米東岸"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.WestLatinAmerica, "中南米西岸"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.China, "華南"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Japan, "日本"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Taiwan, "台湾島"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.Korea, "朝鮮"),
      new EnumParser<GvoWorldInfo.CulturalSphere>(GvoWorldInfo.CulturalSphere.NorthAmerica, "北米")
    };

		/*-------------------------------------------------------------------------
		 GvoWorldInfo
		---------------------------------------------------------------------------*/
		private gvt_lib					m_lib;
		private gvo_season				m_season;
	
		private draw_infonames			m_draw_infonames;		// 街名などの描画
	
		private hittest_list			m_world;				// 世界の情報
		private MultiDictionary<string, GvoWorldInfo.Info> m_world_hash_table;
		private GvoItemTypeDatabase		m_item_type_db;			// アイテムの種類情報

		private hittest_list			m_nonseas;				// 海域以外の一覧
		private hittest_list			m_seas;					// 海域一覧

		private GvoWorldInfo.Server		m_server;				// カレントサーバ
		private GvoWorldInfo.Country		m_my_country;			// カレント国

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public hittest_list World			{	get{	return m_world;					}}

		public GvoWorldInfo.Server MyServer		{	get{	return m_server;				}}
		public GvoWorldInfo.Country MyCountry	{	get{	return m_my_country;			}}

		public hittest_list NoSeas			{	get{	return m_nonseas;				}}
		public hittest_list Seas			{	get{	return m_seas;					}}
		public gvo_season Season			{	get{	return m_season;				}}

		/*-------------------------------------------------------------------------
		 世界の情報管理
		---------------------------------------------------------------------------*/
		public GvoWorldInfo(gvt_lib lib, gvo_season season, string world_info_fname, string memo_path)
		{
			m_lib				= lib;
			m_season			= season;
			m_world				= new hittest_list();
			m_world_hash_table	= new MultiDictionary<string, GvoWorldInfo.Info>();
			m_item_type_db		= new GvoItemTypeDatabase();
			m_nonseas			= new hittest_list();
			m_seas				= new hittest_list();
			m_draw_infonames	= new draw_infonames(lib, this);
	
			// サーバと国を初期化
			// エラー番号としておく
			m_server	= GvoWorldInfo.Server.Unknown;
			m_my_country	= GvoWorldInfo.Country.Unknown;
	
			// XML情報を読み込む
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			load_info_xml(world_info_fname);
			Console.WriteLine("load_info_xml()=" + stopwatch.ElapsedMilliseconds.ToString());

			// メモを読み込む
			load_memo(memo_path);

			// GvoDomains、GvoItemTypeDatabase等のデータベースとリンクする
			link_database();

			// 海域一覧とその他一覧を構築する
			create_seas_list();
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public void Dispose()
		{
			if(m_draw_infonames != null)	m_draw_infonames.Dispose();
			m_draw_infonames	= null;
		}

		/*-------------------------------------------------------------------------
		 GvoDomains、GvoItemTypeDatabase等のデータベースとリンクする
		---------------------------------------------------------------------------*/
		private void link_database()
		{
			int index	= 0;
			foreach(Info i in m_world){
				if (i.InfoType != InfoType.Sea){
					// 海域以外
					bool	has_name_image	= true;

					if(i.CityInfo != null){
						has_name_image		= i.CityInfo.HasNameImage;
					}

					// 描画用の矩形を更新する
					i.UpdateDrawRects(m_lib, has_name_image? index: -1);
					if(has_name_image)	index++;
				}
			}
		}

		/*-------------------------------------------------------------------------
		 海域一覧とその他一覧を構築する
		---------------------------------------------------------------------------*/
		private void create_seas_list()
		{
			m_nonseas.Clear();
			m_seas.Clear();

			foreach(Info i in m_world){
				if(i.InfoType == InfoType.Sea){
					// 海域
					if(i.SeaInfo != null)	m_seas.Add(i);
				}else{
					// 海域以外
					m_nonseas.Add(i);
				}
			}
		}

		/*-------------------------------------------------------------------------
		 メモを読み込む
		---------------------------------------------------------------------------*/
		private void load_memo(string path)
		{
			foreach(Info i in m_world){
				i.LoadMemo(path);
			}
		}

		/*-------------------------------------------------------------------------
		 メモを書き出す
		---------------------------------------------------------------------------*/
		public void WriteMemo(string path)
		{
			foreach(Info i in m_world){
				i.WriteMemo(path);
			}
		}
	
		/*-------------------------------------------------------------------------
		 マップ座標から検索
		---------------------------------------------------------------------------*/
		public Info FindInfo(Point map_pos)
		{
			return (Info)m_world.HitTest(map_pos);
		}

		/*-------------------------------------------------------------------------
		 マップ座標から検索
		 海域を含まない
		---------------------------------------------------------------------------*/
		public Info FindInfo_WithoutSea(Point map_pos)
		{
			Info	i	= (Info)m_world.HitTest(map_pos);
			if(i == null)						return null;
			if(i.InfoType == InfoType.Sea)	return null;
			return (Info)m_world.HitTest(map_pos);
		}

		/*-------------------------------------------------------------------------
		 名前から検索
		---------------------------------------------------------------------------*/
		public Info FindInfo(string name)
		{
			if(string.IsNullOrEmpty(name))	return null;
			return m_world_hash_table.GetValue(name);
		}

		/*-------------------------------------------------------------------------
		 できるだけ検索
		 検索する文字を含むものをできるだけ検索する
		---------------------------------------------------------------------------*/
		public void FindAll(string find_string, List<GvoDatabase.Find> list, GvoDatabase.Find.FindHandler handler)
		{
			foreach(Info _info in m_world){
				_info.FindAll(find_string, list, handler);
			}
		}

		/*-------------------------------------------------------------------------
		 種類で検索
		 検索する文字を含むものをできるだけ検索する
		 例えば 食料品 等での検索用
		---------------------------------------------------------------------------*/
		public void FindAll_FromType(string find_string, List<GvoDatabase.Find> list, GvoDatabase.Find.FindHandler handler)
		{
			foreach(Info _info in m_world){
				_info.FindAll_FromType(find_string, list, handler);
			}
		}

		/*-------------------------------------------------------------------------
		 文化圏リストを作成する
		---------------------------------------------------------------------------*/
    public GvoDatabase.Find[] CulturalSphereList()
    {
      List<GvoDatabase.Find> list = new List<GvoDatabase.Find>();
      foreach (GvoWorldInfo.CulturalSphere cs in Enum.GetValues(typeof (GvoWorldInfo.CulturalSphere)))
      {
        if (cs != GvoWorldInfo.CulturalSphere.Unknown)
        {
          string culturalSphereTooltip = get_cultural_sphere_tooltip(cs);
          list.Add(new GvoDatabase.Find(cs, culturalSphereTooltip));
        }
      }
      return list.ToArray();
    }

    private string get_cultural_sphere_tooltip(GvoWorldInfo.CulturalSphere cs)
    {
      string str = "";
      foreach (GvoWorldInfo.Info info in m_world)
      {
        if (info.CulturalSphere == cs)
          str = str + info.Name + "\n";
      }
      return str;
    }

    public bool DownloadDomains(string domaininfo_file_name)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      if (load_domains_from_old_data(download_domains()))
      {
        WriteDomains(domaininfo_file_name);
        Console.WriteLine("同盟国ダウンロ\x30FCド" + stopwatch.ElapsedMilliseconds.ToString());
        return true;
      }
      else
      {
        Console.WriteLine("同盟国ダウンロ\x30FCド(失敗)" + stopwatch.ElapsedMilliseconds.ToString());
        return false;
      }
    }

		/*-------------------------------------------------------------------------
		 詳細情報の読み込み
		 スレッド読みに対応
		---------------------------------------------------------------------------*/
		public bool Load(string info_file_name, string domaininfo_file_name, string local_domaininfo_file_name)
		{
			// 同盟状況読み込み
			load_domains_xml(domaininfo_file_name, local_domaininfo_file_name);

			// アイテム分類情報読み込み
			m_item_type_db.Load();
			load_items_xml(info_file_name);

			// サーバと自国を反映させる
			SetServerAndCountry(GvoWorldInfo.Server.Euros, GvoWorldInfo.Country.England);
			return true;
		}

		/*-------------------------------------------------------------------------
		 アイテムデータベースとリンクさせる
		---------------------------------------------------------------------------*/
		public void LinkItemDatabase(ItemDatabaseCustom item_db)
		{
			foreach(Info _info in m_world){
				_info.LinkItemDatabase(item_db);
			}
		}
			
		/*-------------------------------------------------------------------------
		 サーバと自国を設定する
		---------------------------------------------------------------------------*/
		public void SetServerAndCountry(GvoWorldInfo.Server server, GvoWorldInfo.Country country)
		{
			if(   (m_server == server)
				&&(m_my_country == country)){
				// 前回と同じならなにもしない
				return;
			}
	
			// 設定更新
			m_server	= server;
			m_my_country	= country;

			// 設定を反映させておく
			update_domains();
		}

		/*-------------------------------------------------------------------------
		 サーバと自国を設定する
		---------------------------------------------------------------------------*/
		private void update_domains()
		{
			foreach(Info _info in m_world){
				// データベースを更新する
				_info.UpdateDomains(m_item_type_db, m_server, m_my_country);
			}
		}

		/*-------------------------------------------------------------------------
		 同盟国を変更する
		---------------------------------------------------------------------------*/
		public bool SetDomain(string city_name, GvoWorldInfo.Country country)
		{
			GvoWorldInfo.Info info = FindInfo(city_name);
			if (info == null || info.CityInfo == null){
				return false;
			}

			// 変更
			info.CityInfo.SetDomain(m_server, country);

			// 設定を反映させておく
			update_domains();
			return true;
		}

		/*-------------------------------------------------------------------------
		 街名描画
		 上陸地点も含む
		---------------------------------------------------------------------------*/
		public void DrawCityName()
		{
			m_draw_infonames.DrawCityName();
		}
	
		/*-------------------------------------------------------------------------
		 海域名描画
		---------------------------------------------------------------------------*/
		public void DrawSeaName()
		{
			m_draw_infonames.DrawSeaName();
		}

		/*-------------------------------------------------------------------------
		 同盟国更新用の文字列を得る
		 "サーバ番号"+"街番号"+"同盟国番号"
		---------------------------------------------------------------------------*/
		public string GetNetUpdateString(string city_name)
		{
      GvoWorldInfo.Info info = FindInfo(city_name);
      if (info == null)
        return (string) null;
      if (info.CityInfo == null)
        return (string) null;
      else
        return info.CityInfo.GetNetUpdateString(m_server);
		}

        /*-------------------------------------------------------------------------
         税率を考慮した価格を得る
        ---------------------------------------------------------------------------*/
		static public int GetTaxPrice(int price)
		{
			return price + (int)(def.TAX * price);
		}

    private void load_info_xml(string file_name)
    {
      m_world.Clear();
      m_world_hash_table.Clear();
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(file_name);
        if (xmlDocument.DocumentElement == null || xmlDocument.DocumentElement.ChildNodes.Count <= 0)
          return;
        foreach (XmlNode n in xmlDocument.DocumentElement.ChildNodes)
        {
          GvoWorldInfo.Info t = GvoWorldInfo.Info.FromXml(n);
          if (t != null)
          {
            m_world.Add((hittest) t);
            m_world_hash_table.Add(t);
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("load_info_xml()");
        Console.WriteLine(ex.ToString());
      }
    }

    private void write_info_xml(string file_name)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.AppendChild((XmlNode) xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", (string) null));
      xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement("info_root"));
      foreach (GvoWorldInfo.Info info in m_world)
        info.WriteInfoXml(xmlDocument.DocumentElement);
      xmlDocument.Save(file_name);
    }

    private void load_items_xml(string file_name)
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(file_name);
        if (xmlDocument.DocumentElement == null || xmlDocument.DocumentElement.ChildNodes.Count <= 0)
          return;
        foreach (XmlNode n in xmlDocument.DocumentElement.ChildNodes)
        {
          if (n.Attributes["name"] != null)
          {
            GvoWorldInfo.Info info = FindInfo(n.Attributes["name"].Value);
            if (info != null)
              info.LoadItemsXml(n);
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("load_items_xml()");
        Console.WriteLine(ex.ToString());
      }
    }

    private void write_items_xml(string file_name)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.AppendChild((XmlNode) xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", (string) null));
      xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement("cityinfo_root"));
      foreach (GvoWorldInfo.Info info in m_world)
        info.WriteItemsXml(xmlDocument.DocumentElement);
      xmlDocument.Save(file_name);
    }

    private void load_domains_xml(string init_file_name, string local_domaininfo_file_name)
    {
      string filename = !File.Exists(local_domaininfo_file_name) ? init_file_name : local_domaininfo_file_name;
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(filename);
        if (xmlDocument.DocumentElement == null || xmlDocument.DocumentElement.ChildNodes.Count <= 0)
          return;
        foreach (XmlNode node in xmlDocument.DocumentElement.ChildNodes)
        {
          if (node.Attributes["name"] != null)
          {
            GvoWorldInfo.Info info = FindInfo(node.Attributes["name"].Value);
            if (info != null && info.CityInfo != null)
              info.CityInfo.LoadDomainXml(node);
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("load_domains_xml()");
        Console.WriteLine(ex.ToString());
      }
    }

    public bool WriteDomains(string file_name)
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.AppendChild((XmlNode) xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", (string) null));
        xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement("domaininfo_root"));
        foreach (GvoWorldInfo.Info info in m_world)
        {
          if (info.CityInfo != null)
            info.CityInfo.WriteDomainXml((XmlNode) xmlDocument.DocumentElement);
        }
        xmlDocument.Save(file_name);
      }
      catch (Exception ex)
      {
        Console.WriteLine("WriteDomains()");
        Console.WriteLine(ex.ToString());
        return false;
      }
      return true;
    }

		/*-------------------------------------------------------------------------
		 InfoType
		---------------------------------------------------------------------------*/
    public static string GetInfoTypeString(GvoWorldInfo.InfoType __type)
    {
      return EnumParserUtility<GvoWorldInfo.InfoType>.ToString(GvoWorldInfo.m_infotype_enum_param, __type, "不明");
    }

    public static GvoWorldInfo.InfoType GetInfoTypeFromString(string str)
    {
      return EnumParserUtility<GvoWorldInfo.InfoType>.ToEnum(GvoWorldInfo.m_infotype_enum_param, str, GvoWorldInfo.InfoType.City);
    }

    public static string GetServerStringForShare(GvoWorldInfo.Server _server)
    {
      switch (_server)
      {
        case GvoWorldInfo.Server.Zephyros:
          return "zephyros";
        case GvoWorldInfo.Server.Notos:
          return "notos";
        case GvoWorldInfo.Server.Boreas:
          return "boreas";
        default:
          return "euros";
      }
    }

		/*-------------------------------------------------------------------------
		 サーバ名から Server を得る
		---------------------------------------------------------------------------*/
		public static Server GetServerFromString(string str)
		{
			return EnumParserUtility<GvoWorldInfo.Server>.ToEnum(str, GvoWorldInfo.Server.Unknown);
		}

		/*-------------------------------------------------------------------------
		 国名を得る
		---------------------------------------------------------------------------*/
		public static string GetCountryString(Country _country)
		{
			return EnumParserUtility<GvoWorldInfo.Country>.ToString(GvoWorldInfo.m_country_enum_param, _country, "所属無");
		}

		/*-------------------------------------------------------------------------
		 国名から Country を得る
		---------------------------------------------------------------------------*/
    public static GvoWorldInfo.Country GetCountryFromString(string str)
    {
      return EnumParserUtility<GvoWorldInfo.Country>.ToEnum(GvoWorldInfo.m_country_enum_param, str, GvoWorldInfo.Country.Unknown);
    }

		/*-------------------------------------------------------------------------
		 街の種類を得る
		---------------------------------------------------------------------------*/
		public static string GetCityTypeString(CityType _city_type)
		{
			return EnumParserUtility<GvoWorldInfo.CityType>.ToString(GvoWorldInfo.m_citytype_enum_param, _city_type, "不明");
		}

    public static GvoWorldInfo.CityType GetCityTypeFromString(string str)
    {
      return EnumParserUtility<GvoWorldInfo.CityType>.ToEnum(GvoWorldInfo.m_citytype_enum_param, str, GvoWorldInfo.CityType.City);
    }

		/*-------------------------------------------------------------------------
		 同盟状況を得る
		---------------------------------------------------------------------------*/
		public static string GetAllianceTypeString(AllianceType _alliance)
		{
			return EnumParserUtility<GvoWorldInfo.AllianceType>.ToString(GvoWorldInfo.m_alliancetype_enum_param, _alliance, "同盟なし");
		}

    public static GvoWorldInfo.AllianceType GetAllianceTypeFromString(string str)
    {
      return EnumParserUtility<GvoWorldInfo.AllianceType>.ToEnum(GvoWorldInfo.m_alliancetype_enum_param, str, GvoWorldInfo.AllianceType.Unknown);
    }

		/*-------------------------------------------------------------------------
		 文化圏を得る
		---------------------------------------------------------------------------*/
		public static string GetCulturalSphereString(CulturalSphere cs)
		{
			return EnumParserUtility<GvoWorldInfo.CulturalSphere>.ToString(GvoWorldInfo.m_culturalsphere_enum_param, cs, "不明");
		}

    public static GvoWorldInfo.CulturalSphere GetCulturalSphereFromString(string str)
    {
      return EnumParserUtility<GvoWorldInfo.CulturalSphere>.ToEnum(GvoWorldInfo.m_culturalsphere_enum_param, str, GvoWorldInfo.CulturalSphere.Unknown);
    }

    private string[] download_domains()
    {
      string str1 = HttpDownload.Download("http://gvtrademap.daa.jp/domain.php", Encoding.UTF8);
      if (str1 == null)
        return (string[]) null;
      string[] strArray = str1.Split(new string[2]
      {
        "\r\n",
        "\n"
      }, StringSplitOptions.RemoveEmptyEntries);
      List<string> list = new List<string>();
      foreach (string str2 in strArray)
      {
        if (!(str2 == "start"))
        {
          if (!(str2 == "end"))
            list.Add(str2);
          else
            break;
        }
      }
      return list.ToArray();
    }

    private bool load_domains_from_old_data(string[] domains)
    {
      if (domains == null || domains.Length != 4)
        return false;
      foreach (GvoWorldInfo.Info info in m_world)
      {
        if (info.CityInfo != null)
          info.CityInfo.LoadDomainFromNeworkData(domains);
      }
      return true;
    }

		/*-------------------------------------------------------------------------
		 1つの街、上陸地点、又は海域
		---------------------------------------------------------------------------*/
		public class Info : hittest, IDictionaryNode<string>
		{
			private string[]				m_group_name_tbl	= new string[]{
												"[交易]", "[道具]", "[工房]", "[人物]",
												"[船]", "[船大工]", "[大砲]", "[板]",
												"[帆]", "[像]", "[行商人]",
												"",
											};

			// 判定用マージン
			const int							CHECK_MARGIN	= 10;

			private string						m_name;				// 名前
			private InfoType					m_info_type;		// 種類(街等)
			private int							m_url_index;		// URL番号
			private string						m_url;				// URLそのもの
																	// 直接リンク
			private GvoWorldInfo.Server			m_server;			// プレイしているサーバ
            private GvoWorldInfo.CityInfo       m_cityinfo;			// 街情報
			private GvoWorldInfo.SeaInfo		m_seainfo;			// 海域情報

			private string						m_memo;				// メモ
			private string[]					m_memo_div_lines;	// 行単位に分割されたメモ

			private Point						m_string_offset1;	// 街名用オフセット(大きいアイコン用)
			private Point						m_string_offset2;	// 街名用オフセット(小さいアイコン用)
			private d3d_sprite_rects.rect		m_icon_rect;		// アイコン用矩形
			private d3d_sprite_rects.rect		m_small_icon_rect;	// アイコン用矩形(小)
			private d3d_sprite_rects.rect		m_string_rect;		// 文字用矩形
	
			private List<Group>				m_groups;		// group_index分のデータ

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public string		Key{						get{	return m_name;			}}
			public string		Name{						get{	return m_name;			}}
			public InfoType		InfoType{					get{	return m_info_type;		}}
			public string		InfoTypeStr{				get{	return GvoWorldInfo.GetInfoTypeString(m_info_type);	}}
			public int			UrlIndex{					get{	return m_url_index;		}}
			public string		Url{						get{	return m_url;			}}

			public GvoWorldInfo.Server MyServer{				get{	return m_server;		}}
			// 同盟国
			public GvoWorldInfo.Country MyCountry{
				get{
					if(m_cityinfo == null)	return GvoWorldInfo.Country.Unknown;
					return m_cityinfo.GetDomain(m_server);
				}
			}
            public string CountryStr {                      get {   return GvoWorldInfo.GetCountryString(this.MyCountry); } }
            public string Lang1 {
              get {
                if (this.m_cityinfo != null)
                  return this.m_cityinfo.Lang1;
                else
                  return "";
              }
            }
            public string Lang2 {
              get {
                if (this.m_cityinfo != null)
                  return this.m_cityinfo.Lang2;
                else
                  return "";
              }
            }
            public int Sakaba {
              get {
                if (this.m_cityinfo != null)
                  return this.m_cityinfo.Sakaba;
                else
                  return 0;
              }
            }

			public GvoWorldInfo.CityType CityType{
				get{
					if(m_cityinfo == null)	return GvoWorldInfo.CityType.City;
					return m_cityinfo.CityType;
				}
			}
			public string CityTypeStr{						get{	return GvoWorldInfo.GetCityTypeString(CityType);	}}
			public GvoWorldInfo.AllianceType AllianceType{
				get{
					if(m_cityinfo == null)	return GvoWorldInfo.AllianceType.Unknown;
					return m_cityinfo.AllianceType;
				}
			}
			public string AllianceTypeStr{					get{	return GvoWorldInfo.GetAllianceTypeString(this.AllianceType);	}}
			public GvoWorldInfo.CulturalSphere CulturalSphere{
				get{
					if(m_cityinfo == null)	return GvoWorldInfo.CulturalSphere.Unknown;
					return m_cityinfo.CulturalSphere;
				}
			}
			public string CulturalSphereStr{				get{	return GvoWorldInfo.GetCulturalSphereString(this.CulturalSphere);	}}

			public GvoWorldInfo.SeaInfo SeaInfo{					get{	return m_seainfo;		}
												internal	set{	m_seainfo	= value;	}}
			public GvoWorldInfo.CityInfo CityInfo{			get{	return m_cityinfo;		}
												internal	set{	m_cityinfo	= value;	}}

			// メモ全体
			// 行単位は専用の関数を使うこと
			public string Memo{						get{	return m_memo;			}
													set{	m_memo	= value;
															div_memo_lines();		}}
			public bool IsUrl{						get{	if(UrlIndex >= 0)	return true;
															if(Url != "")		return true;
															return false;
													}}

			public Point StringOffset1{				get{	return m_string_offset1;	}}
			public Point StringOffset2{				get{	return m_string_offset2;	}}
			public d3d_sprite_rects.rect IconRect{			get{	return m_icon_rect;			}}
			public d3d_sprite_rects.rect SmallIconRect{	get{	return m_small_icon_rect;	}}
			public d3d_sprite_rects.rect NameRect{		get{	return m_string_rect;		}}

			// ツールチップ
			public string TooltipString{				get{	return __get_tool_tip_string();	}}
			
			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			private Info()
			{
				int i= 0;
				m_groups	= new List<Group>();
				foreach(string s in m_group_name_tbl){
					m_groups.Add(new Group(s, this, i++));
				}

				m_memo				= "";
				div_memo_lines();

				// 判定用矩形を指定しておく
				base.rect			= new Rectangle(-CHECK_MARGIN,
													-CHECK_MARGIN,
													CHECK_MARGIN*2,
													CHECK_MARGIN*2);

				m_server			= GvoWorldInfo.Server.Euros;	// 初期値はユーロス
				m_cityinfo			= null;
				m_seainfo			= null;

				m_icon_rect			= null;
				m_small_icon_rect	= null;
				m_string_rect		= null;
			}

			/*-------------------------------------------------------------------------
			 指定されたアイテムがあるかを得る
			 完全一致のみで調べる
			---------------------------------------------------------------------------*/
			public Group.Data HasItem(string find_item)
			{
				foreach(Group g in m_groups){
					Group.Data	d	= g.HasItem(find_item);
					if(d != null)	return d;
				}
				return null;
			}

			/*-------------------------------------------------------------------------
			 メモの読み込み
			---------------------------------------------------------------------------*/
			public void LoadMemo(string path)
			{
				// ファイル名作成
				path	+= m_name + "_memo.txt";
	
				if(!File.Exists(path))	return;		// ファイルが見つからない

				try{
					using (StreamReader	sr	= new StreamReader(
						path, Encoding.GetEncoding("Shift_JIS"))) {

						// 全部読み込む
						m_memo	= sr.ReadToEnd();
					}
					div_memo_lines();
				}catch{
					// 読み込み失敗
					m_memo	= "";
				}
			}

			/*-------------------------------------------------------------------------
			 メモの書き出し
			---------------------------------------------------------------------------*/
			public void WriteMemo(string path)
			{
				// ファイル名作成
				path	+= m_name + "_memo.txt";
	
				// メモ情報がなければ書き出さない
				if(m_memo == ""){
					// ファイルがあれば削除する
					file_ctrl.RemoveFile(path);
					return;
				}

				try{
					using (StreamWriter	sw	= new StreamWriter(
						path, false, Encoding.GetEncoding("Shift_JIS"))) {
						sw.Write(Memo);
					}
				}catch{
					// 書き出し失敗
				}
			}

			/*-------------------------------------------------------------------------
			 教えてくれる人を調べる
			 教えてくれる人が居なければnullを返す
			---------------------------------------------------------------------------*/
			public string LearnPerson(string find_str)
			{
				Group.Data d	= m_groups[(int)GroupIndex._3].HasItem(find_str);
				if(d == null)	return null;
				return d.Price;
			}

			/*-------------------------------------------------------------------------
			 数を得る
			---------------------------------------------------------------------------*/
			public int GetCount(GroupIndex index)
			{
				if(index < 0)					return 0;
				if(index >= GroupIndex.max)	return 0;
				return m_groups[(int)index].GetCount();
			}
	
			/*-------------------------------------------------------------------------
			 グループを得る
			---------------------------------------------------------------------------*/
			public Group GetGroup(GroupIndex index)
			{
				if(index < 0)					return null;
				if(index >= GroupIndex.max)	return null;
				return m_groups[(int)index];
			}

			/*-------------------------------------------------------------------------
			 グループ名からデータを得る
			---------------------------------------------------------------------------*/
			private Info.Group get_group(string name)
			{
				foreach (GvoWorldInfo.Info.Group group in m_groups)
				{
					if (group.Name == name){
						return group;
					}
				}
				return null;
			}

			/*-------------------------------------------------------------------------
			 データを得る
			---------------------------------------------------------------------------*/
			public Group.Data GetData(GroupIndex index, int data_index)
			{
				Group	g	= GetGroup(index);
				if(g == null)	return null;
				return g.GetData(data_index);
			}

			/*-------------------------------------------------------------------------
			 参照用にデータ更新
			---------------------------------------------------------------------------*/
			public void UpdateDomains(GvoItemTypeDatabase type, GvoWorldInfo.Server server, GvoWorldInfo.Country my_country)
			{
				m_server	= server;	// プレイしているサーバの更新
				foreach(Group g in m_groups){
					g.UpdateDomains(type, this.MyCountry != my_country);
				}
			}

			/*-------------------------------------------------------------------------
			 描画用の矩形を更新する
			---------------------------------------------------------------------------*/
			public void UpdateDrawRects(gvt_lib lib, int index)
			{
				if(m_info_type == InfoType.Sea)	return;
	
				if(m_info_type == InfoType.City){
					switch(this.CityType){
					case GvoWorldInfo.CityType.Capital:
						m_icon_rect	= lib.infonameimage.GetIcon(0);
						break;
					case GvoWorldInfo.CityType.City:
						m_icon_rect	= lib.infonameimage.GetIcon(
												(this.AllianceType == GvoWorldInfo.AllianceType.Territory)? 2: 3);
						break;
					case GvoWorldInfo.CityType.CapitalIslam:
						m_icon_rect	= lib.infonameimage.GetIcon(1);
						break;
					case GvoWorldInfo.CityType.CityIslam:
						m_icon_rect	= lib.infonameimage.GetIcon(
												(this.AllianceType == GvoWorldInfo.AllianceType.Territory)? 4: 5);
						break;
					}
					m_small_icon_rect	= lib.infonameimage.GetIcon(8);
					m_string_rect		= lib.infonameimage.GetCityName(index);
				}else{
					switch(InfoType){
					case InfoType.Shore:
						m_icon_rect		= lib.infonameimage.GetIcon(9);
						break;
					case InfoType.Shore2:
						m_icon_rect		= lib.infonameimage.GetIcon(6);
						break;
					case InfoType.OutsideCity:
						m_icon_rect		= lib.infonameimage.GetIcon(7);
						break;
					case InfoType.PF:
						m_icon_rect		= lib.infonameimage.GetIcon(11);
						break;
					}
					m_small_icon_rect	= m_icon_rect;		// 同じアイコンを使用する
					m_string_rect		= lib.infonameimage.GetCityName(index);
				}
			}

			/*-------------------------------------------------------------------------
			 アイテムのグループ名を得る
			---------------------------------------------------------------------------*/
			static public string GetGroupName(GroupIndex index)
			{
				switch(index){
				case Info.GroupIndex._0:	return "交易所店主";
				case Info.GroupIndex._1:	return "道具屋主人";
				case Info.GroupIndex._2:	return "工房職人";
				case Info.GroupIndex._3:	return "人物";
				case Info.GroupIndex._4:	return "造船所親方";
				case Info.GroupIndex._4_1:	return "船大工";
				case Info.GroupIndex._5:	return "武器職人";
				case Info.GroupIndex._6:	return "製材職人";
				case Info.GroupIndex._7:	return "製帆職人";
				case Info.GroupIndex._8:	return "彫刻家";
				case Info.GroupIndex._9:	return "行商人";
				case Info.GroupIndex.max:	return "メモ";
				}
				return "不明";
			}

			/*-------------------------------------------------------------------------
			 メモの行数を得る
			---------------------------------------------------------------------------*/
			public int GetMemoLines()
			{
				if(m_memo_div_lines == null)	return 0;
				return m_memo_div_lines.Length;
			}

			/*-------------------------------------------------------------------------
			 メモを得る
			 行指定
			---------------------------------------------------------------------------*/
			public string GetMemo(int line_index)
			{
				if(line_index < 0)					return "";
				if(line_index >= GetMemoLines())	return "";
				return m_memo_div_lines[line_index];
			}

			/*-------------------------------------------------------------------------
			 メモを行毎に分割する
			---------------------------------------------------------------------------*/
			private void div_memo_lines()
			{
				if(m_memo == ""){
					m_memo_div_lines	= null;
				}else{
					m_memo_div_lines	= m_memo.Split(new char[]{'\n'});
				}
			}

			/*-------------------------------------------------------------------------
			 アイテムデータベースとリンクさせる
			---------------------------------------------------------------------------*/
			internal void LinkItemDatabase(ItemDatabaseCustom item_db)
			{
				foreach(Group g in m_groups){
					g.LinkItemDatabase(item_db, Name);
				}
			}
	
			/*-------------------------------------------------------------------------
			 HPを開くtooltipを得る
			---------------------------------------------------------------------------*/
			public string GetToolTipString_HP()
			{
				if(!IsUrl)		return null;		// URLが登録されていない
	
				if(UrlIndex != -1){
					if(InfoType == GvoWorldInfo.InfoType.City){
						// 大商戦
						return "大商戦へ\n左クリックでブラウザを開く";
					}else{
						// DKKmap
						return "D.K.K_mapへ\n左クリックでブラウザを開く";
					}
				}else{
					// クリスタル商会
//					return "直接リンク\n左クリックでブラウザを開く";
					return "大航海時代Online上陸地点地図(仮)へ\n左クリックでブラウザを開く";
				}
			}

			/*-------------------------------------------------------------------------
			 できるだけ検索
			---------------------------------------------------------------------------*/
			public void FindAll(string find_string, List<GvoDatabase.Find> list, GvoDatabase.Find.FindHandler handler)
			{
				// 名前
				if(handler(Name, find_string)){
					list.Add(new GvoDatabase.Find(Name));
				}

				// 言語
				if(handler(Lang1, find_string)){
					list.Add(new GvoDatabase.Find(Name, Lang1));
				}
				if(handler(Lang2, find_string)){
					list.Add(new GvoDatabase.Find(Name, Lang2));
				}

				// アイテムから検索
				foreach(Group g in m_groups){
					g.FindAll(find_string, list, Name, handler);
				}
			}

			/*-------------------------------------------------------------------------
			 できるだけ検索
			 種類からの検索用
			---------------------------------------------------------------------------*/
			public void FindAll_FromType(string find_string, List<GvoDatabase.Find> list, GvoDatabase.Find.FindHandler handler)
			{
				// アイテムから検索
				foreach(Group g in m_groups){
					g.FindAll_FromType(find_string, list, Name, handler);
				}
			}

			/*-------------------------------------------------------------------------
			 ツールチップ用文字列の取得
			---------------------------------------------------------------------------*/
			private string __get_tool_tip_string()
			{
				string	tip		= Name + "\n";
				if(SeaInfo != null){
					// 海域
					return tip + SeaInfo.TooltipString;
				}else{
					// その他
					switch(InfoType){
					case GvoWorldInfo.InfoType.City:
						switch(AllianceType){
						case GvoWorldInfo.AllianceType.Unknown:
						case GvoWorldInfo.AllianceType.Piratical:
							tip	+= AllianceTypeStr;
							break;
						case GvoWorldInfo.AllianceType.Alliance:
							tip	+= AllianceTypeStr + " " + CountryStr;
							break;
						case GvoWorldInfo.AllianceType.Capital:
						case GvoWorldInfo.AllianceType.Territory:
							tip	+= CountryStr + " " + AllianceTypeStr;
							break;
						}
						tip	+= "\n種類:" + CityTypeStr;
						tip	+= "\n文化圏:" + CulturalSphereStr;

						// 使用言語
						if(Lang1 != "")	tip	+= "\n使用言語:" + Lang1;
						if(Lang2 != "")	tip	+= "\n使用言語:" + Lang2;
						break;
					case GvoWorldInfo.InfoType.Sea:
					case GvoWorldInfo.InfoType.Shore:
					case GvoWorldInfo.InfoType.Shore2:
					case GvoWorldInfo.InfoType.OutsideCity:
					case GvoWorldInfo.InfoType.PF:
						tip	+= InfoTypeStr;
						break;
					}
					return tip;
				}
			}
	

      internal static GvoWorldInfo.Info FromXml(XmlNode n)
      {
        if (n == null)
          return (GvoWorldInfo.Info) null;
        if (n.ChildNodes == null)
          return (GvoWorldInfo.Info) null;
        if (n.Name != "info")
          return (GvoWorldInfo.Info) null;
        if (n.Attributes["name"] == null)
          return (GvoWorldInfo.Info) null;
        GvoWorldInfo.Info info = new GvoWorldInfo.Info();
        info.m_name = Useful.XmlGetAttribute(n, "name", info.m_name);
        info.position = Useful.XmlGetPoint(n, "position", info.position);
        info.m_url_index = Useful.ToInt32(Useful.XmlGetAttribute(n, "url_index", ""), -1);
        info.m_url = "";
        if (info.m_url_index == -1)
          info.m_url = Useful.XmlGetAttribute(n, "url_string", info.m_url);
        info.m_info_type = GvoWorldInfo.GetInfoTypeFromString(Useful.XmlGetAttribute(n, "info_type", ((object) info.m_info_type).ToString()));
        info.m_string_offset1 = Useful.XmlGetPoint(n, "name_offset1", info.m_string_offset1);
        info.m_string_offset2 = Useful.XmlGetPoint(n, "name_offset2", info.m_string_offset2);
        foreach (XmlNode node in n.ChildNodes)
        {
          GvoWorldInfo.SeaInfo seaInfo = GvoWorldInfo.SeaInfo.FromXml(node, info.m_name);
          if (seaInfo != null)
            info.m_seainfo = seaInfo;
          GvoWorldInfo.CityInfo cityInfo = GvoWorldInfo.CityInfo.FromXml(node, info.m_name);
          if (cityInfo != null)
            info.m_cityinfo = cityInfo;
        }
        return info;
      }

      internal void WriteInfoXml(XmlElement p_node)
      {
        XmlNode node = Useful.XmlAddNode((XmlNode) p_node, "info", Name);
        write_info_sub(node);
        if (m_cityinfo != null)
          m_cityinfo.WriteXml(node);
        if (m_seainfo == null)
          return;
        m_seainfo.WriteXml(node);
      }

      internal void LoadItemsXml(XmlNode n)
      {
        if (n.ChildNodes == null || n.ChildNodes.Count <= 0)
          return;
        foreach (XmlNode n1 in n.ChildNodes)
        {
          if (n1.Attributes["name"] != null)
          {
            Group group = get_group(n1.Attributes["name"].Value);
            if (group != null)
              group.LoadXml(n1);
          }
        }
      }

      private void write_info_sub(XmlNode node)
      {
        Useful.XmlAddPoint(node, "position", position);
        if (m_url_index == -1)
          Useful.XmlAddAttribute(node, "url_string", m_url);
        else
          Useful.XmlAddAttribute(node, "url_index", m_url_index.ToString());
        Useful.XmlAddAttribute(node, "info_type", ((object) m_info_type).ToString());
        Useful.XmlAddPoint(node, "name_offset1", m_string_offset1);
        Useful.XmlAddPoint(node, "name_offset2", m_string_offset2);
      }

      internal void WriteItemsXml(XmlElement xmlElement)
      {
        XmlNode xmlNode = Useful.XmlAddNode((XmlNode) xmlElement, "cityinfo", Name);
        foreach (Group group in m_groups)
          group.WriteInfoXml(xmlNode);
        if (xmlNode.ChildNodes.Count > 0)
          return;
        xmlElement.RemoveChild(xmlNode);
      }

			public enum GroupIndex{
				_0,		// 交易
				_1,		// 道具
				_2,		// 工房
				_3,		// 人物
				_4,		// 造船所親方
				_4_1,	// 船大工
				_5,		// 大砲
				_6,		// 板
				_7,		// 帆
				_8,		// 像
				_9,		// 行商人
				max
			};
	
			/*-------------------------------------------------------------------------
			 データグループ
			---------------------------------------------------------------------------*/
			public class Group
			{
				/*-------------------------------------------------------------------------
				 
				---------------------------------------------------------------------------*/
				private string					m_name;				// 名前
				private List<Data>				m_datas;			// データ
				private GvoWorldInfo.Info m_info;
				private int m_index;

				/*-------------------------------------------------------------------------
				 
				---------------------------------------------------------------------------*/
				public string Name				{	get{	return m_name;		}}
				public GvoWorldInfo.Info Info	{	get{	return m_info;		}}
				public int Index				{	get{	return m_index;		}}

				/*-------------------------------------------------------------------------
				 
				---------------------------------------------------------------------------*/
				public Group(string name, GvoWorldInfo.Info info, int index)
				{
					m_name		= name;
                    m_info      = info;
					m_index		= index;
					m_datas		= new List<Data>();
				}

				/*-------------------------------------------------------------------------
				 指定されたアイテムがあるかを得る
				 完全一致のみで調べる
				---------------------------------------------------------------------------*/
				public Data HasItem(string find_item)
				{
					foreach(Data d in m_datas){
						if(d.Name == find_item)		return d;
					}
					return null;
				}

				/*-------------------------------------------------------------------------
				 豪商と翻訳家フラグを得る
				 ついでに行商人フラグを設定する
				---------------------------------------------------------------------------*/
				public int GetSakabaFlag()
				{
					int	flag	= 0;
					foreach(Data d in m_datas){
						if(d.Tag == "@"){			// 翻訳家
							flag	|= 4;
						}else if(d.Tag == "#"){		// 豪商
							flag	|= 8;
						}else if(d.Tag == ""){
							// 行商人扱い
							d.Tag	= "-";
						}
					}
					return flag;
				}

				/*-------------------------------------------------------------------------
				 数を得る
				---------------------------------------------------------------------------*/
				public int GetCount()
				{
					return m_datas.Count;
				}

				/*-------------------------------------------------------------------------
				 データを得る
				---------------------------------------------------------------------------*/
				public Data GetData(int index)
				{
					if(index < 0)			return null;
					if(index >= GetCount())	return null;
					return m_datas[index];
				}

				/*-------------------------------------------------------------------------
				 データを更新する
				---------------------------------------------------------------------------*/
				internal void UpdateDomains(GvoItemTypeDatabase item_type_db, bool is_tax)
				{
					foreach(Data d in m_datas){
						d.UpdateDomains(item_type_db, is_tax);
					}
				}

				/*-------------------------------------------------------------------------
				 アイテムデータベースとリンクさせる
				---------------------------------------------------------------------------*/
				internal void LinkItemDatabase(ItemDatabaseCustom item_db, string name)
				{
					foreach(Data d in m_datas){
						d.LinkItemDatabase(item_db, name);
					}
				}

				/*-------------------------------------------------------------------------
				 できるだけ検索
				---------------------------------------------------------------------------*/
				public void FindAll(string find_string, List<GvoDatabase.Find> list, string info_name, GvoDatabase.Find.FindHandler handler)
				{
					foreach(Data d in m_datas){
						if(handler(d.Name, find_string)){
							list.Add(new GvoDatabase.Find(GvoDatabase.Find.FindType.Data, info_name, d));
						}
						if(handler(d.Price, find_string)){
							list.Add(new GvoDatabase.Find(GvoDatabase.Find.FindType.DataPrice, info_name, d));
						}
					}
				}

				/*-------------------------------------------------------------------------
				 できるだけ検索
				 種類からの検索用
				---------------------------------------------------------------------------*/
				public void FindAll_FromType(string find_string, List<GvoDatabase.Find> list, string info_name, GvoDatabase.Find.FindHandler handler)
				{
					foreach(Data d in m_datas){
						if(handler(d.Type, find_string)){
							list.Add(new GvoDatabase.Find(GvoDatabase.Find.FindType.Data, info_name, d));
						}
					}
				}

        internal void LoadXml(XmlNode n)
        {
          m_datas.Clear();
          if (n.ChildNodes == null || n.ChildNodes.Count <= 0)
            return;
          foreach (XmlNode nn in n.ChildNodes)
          {
            Data data = Data.FromXml(nn, m_info, m_index);
            if (data != null)
              m_datas.Add(data);
          }
        }

        internal void WriteInfoXml(XmlNode node)
        {
          if (m_datas.Count <= 0)
            return;
          XmlNode node1 = Useful.XmlAddNode(node, "group", Name);
          if (node1 == null)
            return;
          foreach (Data data in m_datas)
            data.WriteInfoXml(node1);
        }

				/*-------------------------------------------------------------------------
				 データ
				---------------------------------------------------------------------------*/
				public class Data
				{
					private Info				m_info;					// 属している街など
					private string				m_name;					// 名前
					private string				m_tag;					// タグ
					private string				m_tag2;					// タグ2
					private int					m_group_index;			// 属しているグループ番号

					// 外からの参照用
					private bool				m_is_bonus_item;		// 名産品
					private string				m_price;				// 値段、人物名など
					private Color				m_color;				// 表示色
					private Color				m_price_color;			// 値段用色
					private string				m_investment;			// 必要な投資額
					private ItemDatabaseCustom.Data	m_item_db;				// アイテムデータベースの詳細
					
					/*-------------------------------------------------------------------------
					 
					---------------------------------------------------------------------------*/
					public string Name				{		get{	return m_name;					}}
					public Info Info				{		get{	return m_info;					}}
					public bool IsBonusItem			{		get{	return m_is_bonus_item;			}}
					public string Price				{		get{	return m_price;					}}
					public string GroupIndexString	{		get{	return GetGroupName(GroupIndex._0 + m_group_index);	}}
					public Color Color				{		get{	return m_color;					}}
					public Color PriceColor			{		get{	return m_price_color;			}}
					public string Investment		{		get{	return m_investment;			}}
					public ItemDatabaseCustom.Data ItemDb	{		get{	return m_item_db;				}}
					public bool HasTooltip			{		get{	return (ItemDb == null)? false: true;	}}
					public string TooltipString		{
						get{
							if(ItemDb == null){
								string str;
								str		= "名称:" + Name;
								str		+= "\nアイテムデータベースに詳細が見つかりませんでした。\n";
								str		+= "名称が微妙に間違っているか未知のデータです。\n";
								return str;
							}else{
								return getMixedToolTipString();
							}
						}
					}
					public string Type
					{
						get{
							if(ItemDb == null)		return "不明";
							return ItemDb.Type;
						}
					}
					public ItemDatabaseCustom.Categoly Categoly
					{
						get{
							if(ItemDb == null)		return ItemDatabaseCustom.Categoly.Unknown;
							return ItemDb.Categoly;
						}
					}
					public Color CategolyColor
					{
						get{
							if(ItemDb == null)		return Color.Black;
							return ItemDb.CategolyColor;
						}
					}

					internal string Tag		{		get{	return m_tag;		}
													set{	m_tag	= value;	}} 
					internal string Tag2	{		get{	return m_tag2;		}}

					/*-------------------------------------------------------------------------
					 
					---------------------------------------------------------------------------*/
					private Data(Info info, int index)
					{
						m_name			= "unknown";
						m_tag			= "";
						m_tag2			= "0";
						m_investment	= "";

						m_info				= info;
						m_group_index		= index;

						// 外からの参照用
						m_is_bonus_item		= false;
						m_price				= "";
						m_color				= Color.Black;
						m_price_color		= Color.Black;
						m_item_db			= null;
					}

					/*-------------------------------------------------------------------------
					 描画用の色を得る
					---------------------------------------------------------------------------*/
					private Color _get_color()
					{
						switch(m_tag){
						//スキル
						case "s":		return Color.Black;
						//報告
						case "h":		return Color.DarkRed;
						//レシピ
//						case "%":		return Color.DarkGreen;

						// 投資後リスト入り
						case "*":		return Color.Gray;
						// 投資で得られる
						case "$":		return Color.Blue;
						// 備え付け
						//レシピ
						case "%":		return Color.DarkGreen;
						// 翻訳家
						case "@":		return Color.DarkRed;
						// 販売員
						case "+":		return Color.MediumPurple;
						// 豪商
						case "#":		return Color.Green;
						// 行商人
						case "-":		return Color.Gray;
						// その他
						default:		return Color.Black;
						}
					}
					
					/*-------------------------------------------------------------------------
					 描画用の値段を得る
					 種類によっては値段ではないものが返る
					---------------------------------------------------------------------------*/
					private string _get_price()
					{
						switch(m_tag){
						// 投資で得られる
						case "$":		return "投資";
						// 備え付け
						case "%":		return "備え付け";
						// 翻訳家
						case "@":		return "翻訳家";
						// 販売員
						case "+":		return "販売員";
						// 豪商
						case "#":		return "豪商";
						// 行商人
						case "-":		return "行商人";

						// その他
						default:
							return calc_price(false);
						}
					}

					/*-------------------------------------------------------------------------
					 値段を得る
					 税率を考慮する
					---------------------------------------------------------------------------*/
					private string calc_price(bool is_tax)
					{
						int			p;
						if(!Int32.TryParse(m_tag2, out p))	p	= 0;
						if(is_tax)	p	= GetTaxPrice(p);
						return String.Format("{0:#,0}", p);
					}

					/*-------------------------------------------------------------------------
					 更新
					---------------------------------------------------------------------------*/
					internal void UpdateDomains(GvoItemTypeDatabase item_type_db, bool is_tax)
					{
						// 色
						m_color					= _get_color();
						m_price_color			= m_color;

						// 交易品専用の情報
						if(m_group_index == 0){
							// 交易品
							// 名産品かどうかを調べる
							m_is_bonus_item		= item_type_db.IsBonusItem(Name);

							// 値段
							if(m_info.InfoType == InfoType.City){
								if(item_type_db.IsNanbanTradeItem(Name))	m_price	= "南蛮";
								else								m_price	= calc_price(is_tax);
							}else{
								update_rank(item_type_db);
							}
						}else{
							// その他
							m_is_bonus_item		= false;

							// 値段
							if(m_group_index != 3)	m_price	= _get_price();
							else					m_price	= Tag2;				// 人物
						}
					}

					/*-------------------------------------------------------------------------
					 調達などのランクを更新する
					---------------------------------------------------------------------------*/
					private void update_rank(GvoItemTypeDatabase type)
					{
						int		index		= Useful.ToInt32(m_tag2, 0);
		
						switch(index){
						case -1:
							m_price			= "採集R" + rank_to_str(type.GetSaisyuRank(Name));
							m_price_color	= Color.DarkCyan;
							break;
						case -2:
							m_price			= "調達R" + rank_to_str(type.GetChotatuRank(Name));
							m_price_color	= Color.Olive;
							break;	
						case -3:
							// -3は予約されているが、データには含まれない
							m_price			= "探索";
							m_price_color	= Color.DarkViolet;
							break;
						case 0:
						default:
							m_price			= "釣りR" + rank_to_str(type.GetFishingRank(Name));
							m_price_color	= Color.Gray;
							break;
						}
					}

					/*-------------------------------------------------------------------------
					 ランクを文字列に変換する
					 R1～R19以外は??に変換される
					---------------------------------------------------------------------------*/
					private string rank_to_str(int rank)
					{
						if(rank < 1)	return "??";
						if(rank >= 20)	return "??";
						return rank.ToString();
					}

					/*-------------------------------------------------------------------------
					 アイテムデータベースとリンクさせる
					---------------------------------------------------------------------------*/
					internal void LinkItemDatabase(ItemDatabaseCustom db, string info_name)
					{
						if(String.IsNullOrEmpty(Name))	return;

						ItemDatabaseCustom.Data	d	= db.Find(Name);
#if DEBUG
//						if(d == null){
//							Debug.WriteLine(String.Format("{0} {1}", info_name, name));
//						}
#endif
						m_item_db		= d;
					}

					/*-------------------------------------------------------------------------
					 アイテム名変更に対応する
					---------------------------------------------------------------------------*/
					static private void update_rename(Data d)
					{
						switch(d.m_name){
						case "仕立て道具":
							{
								d.m_name	= "裁縫道具";
								d.m_tag2	= "10000";
							}
							break;
						case "回避指南書第1巻":
							d.m_name	= "連撃指南書第1巻";
							break;
						case "攻撃指南書第1巻":
							d.m_name	= "猛攻指南書第1巻";
							break;
						case "回復指南書第1巻":
							d.m_name	= "活用指南書第1巻";
							break;
						case "防御指南書第1巻":
							d.m_name	= "奇手指南書第1巻";
							break;
						case "パデットロール":
							d.m_name	= "パデッドロール";
							break;
						case "診察室の製法":
							d.m_name	= "造船素材・診察室";
							break;
						case "天子の像":
							d.m_name	= "天使の像";
							break;
						case "海域":
							d.m_name	= "地理";
							break;
						case "高級上納品の梱包":
							d.m_name	= "高級上納品の梱包(NO.1)";
							break;
						}
					}

					/*-------------------------------------------------------------------------
					 詳細とミックスしたツールチップ用の文字列を得る
					---------------------------------------------------------------------------*/
					private string getMixedToolTipString()
					{
						if(ItemDb == null)		return "";

						string	str		= "名称:" + this.ItemDb.Name + "\n";
						if(Categoly != ItemDatabaseCustom.Categoly.Unknown){
							str				+= "種類:" + Type + "(カテゴリ" + ((int)Categoly + 1).ToString() + ")\n";
						}else{
							str				+= "種類:" + Type + "\n";
						}
						if(this.Investment != ""){
							str				+= "投資:" + this.Investment + "\n";
						}
						str				+= "説明:\n" + ItemDb.Document;
						return str;
					}
          internal static Group.Data FromXml(XmlNode nn, Info info, int index)
          {
              Group.Data data = new Group.Data(info, index);
            data.m_name = Useful.XmlGetAttribute(nn, "name", data.m_name);
            data.m_tag = Useful.XmlGetAttribute(nn, "option", data.m_tag);
            data.m_tag2 = Useful.XmlGetAttribute(nn, "price", data.m_tag2);
            data.m_investment = Useful.XmlGetAttribute(nn, "investment", data.m_investment);
            return data;
          }

          internal void WriteInfoXml(XmlNode node)
          {
            XmlNode node1 = Useful.XmlAddNode(node, "item", Name);
            Useful.XmlAddAttribute(node1, "option", m_tag);
            if (m_tag2 != "0")
              Useful.XmlAddAttribute(node1, "price", m_tag2);
            Useful.XmlAddAttribute(node1, "investment", m_investment);
          }
				}
			}
		}

		/*-------------------------------------------------------------------------
		 海域情報
		 主に風向き
		---------------------------------------------------------------------------*/
		public class SeaInfo
		{
				private string					m_name;
				private Point					m_wind_pos;			// 風向きを描画する位置の中心
				private float					m_summer_angle;		// 夏の風向き
				private float					m_winter_angle;		// 冬の風向き
				private int						m_speedup_rate;		// 最大速度上昇
				private int						m_summer_angle_deg;	// 夏の風向き
				private int						m_winter_angle_deg;	// 夏の風向き
				private string					m_summer_angle_string;	// 16段階の風向き
				private string					m_winter_angle_string;	// 16段階の風向き

				/*-------------------------------------------------------------------------
				 
				---------------------------------------------------------------------------*/
				public string Key					{	get{	return m_name;			}}
				public string Name					{	get{	return m_name;			}}
				public Point WindPos				{	get{	return m_wind_pos;		}}
				public float SummerAngle			{	get{	return m_summer_angle;	}}
				public string SummerAngleString		{	get{	return "夏:" + m_summer_angle_string;	}}
				public float WinterAngle			{	get{	return m_winter_angle;	}}
				public string WinterAngleString		{	get{	return "冬:" + m_winter_angle_string;	}}
				public int SpeedUpRate				{	get{	return m_speedup_rate;	}}
				public string SpeedUpRateString		{	get{	return "速度上昇:" + ((SpeedUpRate==0)?"未調査":SpeedUpRate.ToString() + "%");	}}
				public string TooltipString			{
					get{
						string	tip	= "";
						tip			+= SummerAngleString + "\n";
						tip			+= WinterAngleString + "\n";
						tip			+= SpeedUpRateString;
						return tip;
					}
				}

				/*-------------------------------------------------------------------------
				 
				---------------------------------------------------------------------------*/
				private SeaInfo(string name)
				{
					m_name				= name;
					m_wind_pos			= Point.Empty;
					m_speedup_rate		= 0;
					m_summer_angle_deg	= 0;
					m_winter_angle_deg	= 0;
					update_angle();
				}

      private void update_angle()
      {
        m_summer_angle = Useful.ToRadian(m_summer_angle_deg);
        m_winter_angle = Useful.ToRadian(m_winter_angle_deg);
        m_summer_angle_string = angle_to_string(m_summer_angle_deg);
        m_winter_angle_string = angle_to_string(m_winter_angle_deg);
      }

				/*-------------------------------------------------------------------------
				 角度から文字列に変換する
				 与える角度が逆のため、
				   0度=南からの風
				   90度=西からの風
				   180度=北からの風
				   270度=東からの風
				---------------------------------------------------------------------------*/
				private string angle_to_string(int angle)
				{
					float	tmp	= (float)angle + ((360f/16f)/2);
					if(tmp < 0)	tmp	= 0;
					angle	= (int)(tmp / (360f/16f));
					switch(angle){
					case 0:		return "南の風";
					case 1:		return "南南西の風";
					case 2:		return "南西の風";
					case 3:		return "西南西の風";
					case 4:		return "西の風";
					case 5:		return "西北西の風";
					case 6:		return "北西の風";
					case 7:		return "北北西の風";
					case 8:		return "北の風";
					case 9:		return "北北東の風";
					case 10:	return "北東の風";
					case 11:	return "東北東の風";
					case 12:	return "東の風";
					case 13:	return "東南東の風";
					case 14:	return "南東の風";
					default:	return "南南東の風";
					}
				}
		
      internal static SeaInfo FromXml(XmlNode node, string name)
      {
        if (node == null)
          return (SeaInfo) null;
        if (node.Name != "sea_detail")
          return (SeaInfo) null;
        if (node.ChildNodes == null)
          return (SeaInfo) null;
        SeaInfo seaInfo = new SeaInfo(name);
        seaInfo.m_speedup_rate = Useful.ToInt32(Useful.XmlGetAttribute(node, "speedup_rate", ""), seaInfo.m_speedup_rate);
        seaInfo.m_summer_angle_deg = Useful.ToInt32(Useful.XmlGetAttribute(node, "summer_angle_deg", ""), seaInfo.m_summer_angle_deg);
        seaInfo.m_winter_angle_deg = Useful.ToInt32(Useful.XmlGetAttribute(node, "winter_angle_deg", ""), seaInfo.m_winter_angle_deg);
        seaInfo.m_wind_pos = Useful.XmlGetPoint(node, "name_position", seaInfo.m_wind_pos);
        seaInfo.update_angle();
        return seaInfo;
      }

      public void WriteXml(XmlNode node)
      {
        XmlNode xmlNode = Useful.XmlAddNode(node, "sea_detail");
        Useful.XmlAddAttribute(xmlNode, "speedup_rate", m_speedup_rate.ToString());
        Useful.XmlAddAttribute(xmlNode, "summer_angle_deg", m_summer_angle_deg.ToString());
        Useful.XmlAddAttribute(xmlNode, "winter_angle_deg", m_winter_angle_deg.ToString());
        Useful.XmlAddPoint(xmlNode, "name_position", m_wind_pos);
      }
		}

		/*-------------------------------------------------------------------------
		 街情報
		---------------------------------------------------------------------------*/
		public class CityInfo
		{
				private string					    m_name;				// 名前
				private GvoWorldInfo.Country[]	    m_domains;			// 同盟状況
				private int						    m_index;			// 街番号
				private GvoWorldInfo.CityType		m_city_type;		// 街の種類
				private GvoWorldInfo.AllianceType	m_alliance_type;	// 同盟の種類
				private GvoWorldInfo.CulturalSphere	m_cultural_sphere;	// 文化圏
				private bool					    m_has_name_image;	// 名前の絵情報を持つときtrue
                private string m_lang1;
                private string m_lang2;
                private int m_sakaba_flag;
		
				/*-------------------------------------------------------------------------
				 
				---------------------------------------------------------------------------*/
				public string Key					{	get{	return m_name;				}}
				public string Name					{	get{	return m_name;				}}
				public int Index					{	get{	return m_index;				}}
				public GvoWorldInfo.CityType CityType			{	get{	return m_city_type;			}}
				public GvoWorldInfo.AllianceType AllianceType	{	get{	return m_alliance_type;		}}
				public GvoWorldInfo.CulturalSphere CulturalSphere{	get{	return m_cultural_sphere;	}}
				public bool HasNameImage			{	get{	return m_has_name_image;	}}
				public string Lang1					{	get{	return m_lang1;				}}
				public string Lang2					{	get{	return m_lang2;				}}
				public int Sakaba					{	get{	return m_sakaba_flag;		}}

				/*-------------------------------------------------------------------------
				 
				---------------------------------------------------------------------------*/
				private CityInfo(string name)
				{
					m_name				= name;
					m_index				= 0;
					m_city_type			= GvoWorldInfo.CityType.City;
					m_alliance_type		= GvoWorldInfo.AllianceType.Alliance;
					m_cultural_sphere	= GvoWorldInfo.CulturalSphere.Unknown;
					m_has_name_image	= false;
        m_lang1 = "";
        m_lang2 = "";
        m_sakaba_flag = 0;
        m_domains = new GvoWorldInfo.Country[Enum.GetValues(typeof (GvoWorldInfo.Server)).Length];

					// すべて不明
					for(int i=0; i<m_domains.Length; i++){
						m_domains[i]	= GvoWorldInfo.Country.Unknown;
					}
				}

				/*-------------------------------------------------------------------------
				 同盟状況を設定
				---------------------------------------------------------------------------*/
				public void SetDomain(GvoWorldInfo.Server server_index, GvoWorldInfo.Country country_index)
				{
					// 同盟国のときのみ設定する
					if(this.AllianceType != GvoWorldInfo.AllianceType.Alliance)	return;

					m_domains[(int)server_index]	= country_index;
				}

				/*-------------------------------------------------------------------------
				 同盟状況を設定
				 全サーバに同じ値を設定する
				 同盟国を指定できない街用
				---------------------------------------------------------------------------*/
				public void SetDomain(GvoWorldInfo.Country country_index)
				{
					// 同盟国のときは一括設定無効
					if(this.AllianceType == GvoWorldInfo.AllianceType.Alliance)	return;

					for(int i=0; i<this.m_domains.Length; i++){
						m_domains[i]	= country_index;
					}
				}

      public string GetNetUpdateString(GvoWorldInfo.Server server_index)
      {
        return ((int) server_index).ToString() + "+" + Index.ToString() + "+" + ((int) GetDomain(server_index)).ToString();
      }

				/*-------------------------------------------------------------------------
				 同盟状況を得る
				---------------------------------------------------------------------------*/
				public GvoWorldInfo.Country GetDomain(GvoWorldInfo.Server server_index)
				{
					return m_domains[(int)server_index];
				}

      internal static GvoWorldInfo.CityInfo FromXml(XmlNode node, string name)
      {
        if (node == null)
          return (GvoWorldInfo.CityInfo) null;
        if (node.Name != "city_detail")
          return (GvoWorldInfo.CityInfo) null;
        GvoWorldInfo.CityInfo cityInfo = new GvoWorldInfo.CityInfo(name);
        cityInfo.m_index = Useful.ToInt32(Useful.XmlGetAttribute(node, "index", ""), cityInfo.m_index);
        cityInfo.m_city_type = GvoWorldInfo.GetCityTypeFromString(Useful.XmlGetAttribute(node, "city_type", ((object) cityInfo.m_city_type).ToString()));
        cityInfo.m_alliance_type = GvoWorldInfo.GetAllianceTypeFromString(Useful.XmlGetAttribute(node, "alliance_type", ((object) cityInfo.m_alliance_type).ToString()));
        cityInfo.m_cultural_sphere = GvoWorldInfo.GetCulturalSphereFromString(Useful.XmlGetAttribute(node, "cultural_sphere", ((object) cityInfo.m_cultural_sphere).ToString()));
        cityInfo.m_has_name_image = Useful.ToBool(Useful.XmlGetAttribute(node, "has_name_image", ""), cityInfo.m_has_name_image);
        cityInfo.m_lang1 = Useful.XmlGetAttribute(node, "lang1", cityInfo.m_lang1);
        cityInfo.m_lang2 = Useful.XmlGetAttribute(node, "lang2", cityInfo.m_lang2);
        cityInfo.m_sakaba_flag = Useful.ToInt32(Useful.XmlGetAttribute(node, "bar_flags", ""), cityInfo.m_sakaba_flag);
        cityInfo.SetDomain(GvoWorldInfo.GetCountryFromString(Useful.XmlGetAttribute(node, "default_country", ((object) GvoWorldInfo.Country.Unknown).ToString())));
        return cityInfo;
      }

      public void WriteXml(XmlNode node)
      {
        XmlNode node1 = Useful.XmlAddNode(node, "city_detail");
        Useful.XmlAddAttribute(node1, "index", m_index.ToString());
        Useful.XmlAddAttribute(node1, "city_type", ((object) m_city_type).ToString());
        Useful.XmlAddAttribute(node1, "alliance_type", ((object) m_alliance_type).ToString());
        Useful.XmlAddAttribute(node1, "cultural_sphere", ((object) m_cultural_sphere).ToString());
        Useful.XmlAddAttribute(node1, "has_name_image", m_has_name_image.ToString());
        if (m_alliance_type == GvoWorldInfo.AllianceType.Capital || m_alliance_type == GvoWorldInfo.AllianceType.Territory)
          Useful.XmlAddAttribute(node1, "default_country", ((object) GetDomain(GvoWorldInfo.Server.Euros)).ToString());
        Useful.XmlAddAttribute(node1, "lang1", m_lang1);
        Useful.XmlAddAttribute(node1, "lang2", m_lang2);
        Useful.XmlAddAttribute(node1, "bar_flags", m_sakaba_flag.ToString());
      }

      public void LoadDomainXml(XmlNode node)
      {
        if (node == null || node.Name != "domain_info" || (node.Attributes["name"] == null || node.Attributes["name"].Value != m_name) || (node.ChildNodes == null || m_alliance_type != GvoWorldInfo.AllianceType.Alliance))
          return;
        foreach (XmlNode node1 in node)
        {
          load_domain_sub(node1, GvoWorldInfo.Server.Euros);
          load_domain_sub(node1, GvoWorldInfo.Server.Zephyros);
          load_domain_sub(node1, GvoWorldInfo.Server.Notos);
          load_domain_sub(node1, GvoWorldInfo.Server.Boreas);
        }
      }

      private void load_domain_sub(XmlNode node, GvoWorldInfo.Server server)
      {
        if (node.Name != "server" || node.Attributes["name"] == null || node.Attributes["name"].Value != ((object) server).ToString())
          return;
        m_domains[(int) server] = GvoWorldInfo.GetCountryFromString(Useful.XmlGetAttribute(node, "country", ((object) GvoWorldInfo.Country.Unknown).ToString()));
      }

      public void WriteDomainXml(XmlNode node)
      {
        if (node == null || m_alliance_type != GvoWorldInfo.AllianceType.Alliance)
          return;
        XmlNode p_node = Useful.XmlAddNode(node, "domain_info", m_name);
        Useful.XmlAddAttribute(Useful.XmlAddNode(p_node, "server", ((object) GvoWorldInfo.Server.Euros).ToString()), "country", ((object) m_domains[0]).ToString());
        Useful.XmlAddAttribute(Useful.XmlAddNode(p_node, "server", ((object) GvoWorldInfo.Server.Zephyros).ToString()), "country", ((object) m_domains[1]).ToString());
        Useful.XmlAddAttribute(Useful.XmlAddNode(p_node, "server", ((object) GvoWorldInfo.Server.Notos).ToString()), "country", ((object) m_domains[2]).ToString());
        Useful.XmlAddAttribute(Useful.XmlAddNode(p_node, "server", ((object) GvoWorldInfo.Server.Boreas).ToString()), "country", ((object) m_domains[3]).ToString());
      }

      public bool LoadDomainFromNeworkData(string[] domains)
      {
        if (m_alliance_type != GvoWorldInfo.AllianceType.Alliance || domains == null || (domains.Length != 4 || Index < 0) || (domains[0].Length < Index || domains[1].Length < Index || (domains[2].Length < Index || domains[3].Length < Index)))
          return false;
        m_domains[0] = GvoWorldInfo.GetCountryFromString(domains[0][Index].ToString());
        m_domains[1] = GvoWorldInfo.GetCountryFromString(domains[1][Index].ToString());
        m_domains[2] = GvoWorldInfo.GetCountryFromString(domains[2][Index].ToString());
        m_domains[3] = GvoWorldInfo.GetCountryFromString(domains[3][Index].ToString());
        return true;
      }
		}
	}

	/*-------------------------------------------------------------------------
	 アイテムの種類
	 名産、採集、探索ランク
	 カテゴリはアイテムデータベースに移動した
	---------------------------------------------------------------------------*/
	public class GvoItemTypeDatabase
	{
		private MultiDictionary<string, ItemRank>	m_bonus_items;			// 名産品扱いのアイテム
		private MultiDictionary<string, ItemRank>	m_nanban_trade_items;	// 南蛮品扱いのアイテム
		private MultiDictionary<string, ItemRank>	m_fishting_ranks;		// 釣りランク
		private MultiDictionary<string, ItemRank>	m_collect_ranks;		// 採集ランク
		private MultiDictionary<string, ItemRank>	m_supply_ranks;			// 調達ランク

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public GvoItemTypeDatabase()
		{
			m_bonus_items			= new MultiDictionary<string, ItemRank>();
			m_nanban_trade_items	= new MultiDictionary<string, ItemRank>();

			m_fishting_ranks		= new MultiDictionary<string, ItemRank>();
			m_collect_ranks			= new MultiDictionary<string, ItemRank>();
			m_supply_ranks			= new MultiDictionary<string, ItemRank>();
		}
	
    public void Load()
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(def.ITEM_TYPE_DB);
        if (xmlDocument.DocumentElement.ChildNodes == null)
          return;
        foreach (XmlNode node in xmlDocument.DocumentElement.ChildNodes)
        {
          if (node.Attributes["name"] != null)
          {
            switch (node.Attributes["name"].Value)
            {
              case "fish_rank":
                load_sub(node, m_fishting_ranks);
                continue;
              case "collect_ranks":
                load_sub(node, m_collect_ranks);
                continue;
              case "supply_ranks":
                load_sub(node, m_supply_ranks);
                continue;
              case "bonus_items":
                load_sub(node, m_bonus_items);
                continue;
              case "nanban_trade_items":
                load_sub(node, m_nanban_trade_items);
                continue;
              default:
                continue;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("採取ランク等の読み込みで問題発生");
        Console.Write(ex.Message);
      }
    }

    private void load_sub(XmlNode node, MultiDictionary<string, ItemRank> list)
    {
      list.Clear();
      if (node == null || node.ChildNodes == null)
        return;
      foreach (XmlNode n in node.ChildNodes)
      {
        ItemRank t = ItemRank.FromXml(n);
        if (t != null)
          list.Add(t);
      }
    }

    private void write_xml(string file_name)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.AppendChild((XmlNode) xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", (string) null));
      xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement("itemtype_db_root"));
      write_item_ranks(xmlDocument.DocumentElement, "fish_rank", m_fishting_ranks);
      write_item_ranks(xmlDocument.DocumentElement, "collect_ranks", m_collect_ranks);
      write_item_ranks(xmlDocument.DocumentElement, "supply_ranks", m_supply_ranks);
      write_item_ranks(xmlDocument.DocumentElement, "bonus_items", m_bonus_items);
      write_item_ranks(xmlDocument.DocumentElement, "nanban_trade_items", m_nanban_trade_items);
      xmlDocument.Save(file_name);
    }

    private void write_item_ranks(XmlElement p_node, string name, MultiDictionary<string, ItemRank> list)
    {
      XmlNode node = Useful.XmlAddNode((XmlNode) p_node, "group", name);
      foreach (ItemRank itemRank in list)
        itemRank.WriteXml(node);
    }


		/*-------------------------------------------------------------------------
		 名産品かどうか調べる
		---------------------------------------------------------------------------*/
		public bool IsBonusItem(string name)
		{
      if (m_bonus_items.GetValue(name) != null)
        return true;
      else
        return IsNanbanTradeItem(name);
		}

		/*-------------------------------------------------------------------------
		 南蛮品かどうか調べる
		---------------------------------------------------------------------------*/
		public bool IsNanbanTradeItem(string name)
		{
      return m_nanban_trade_items.GetValue(name) != null;
		}

    private int get_rank(MultiDictionary<string, ItemRank> list, string name)
    {
      ItemRank itemRank = list.GetValue(name);
      if (itemRank == null)
        return 0;
      else
        return itemRank.Rank;
    }

		/*-------------------------------------------------------------------------
		 釣りランクを得る
		---------------------------------------------------------------------------*/
		public int GetFishingRank(string name)
		{
      return get_rank(m_fishting_ranks, name);
		}
	
		/*-------------------------------------------------------------------------
		 採集ランクを得る
		---------------------------------------------------------------------------*/
		public int GetSaisyuRank(string name)
		{
      return get_rank(m_collect_ranks, name);
		}

		/*-------------------------------------------------------------------------
		 調達ランクを得る
		---------------------------------------------------------------------------*/
		public int GetChotatuRank(string name)
		{
      return get_rank(m_supply_ranks, name);
		}
		
		/*-------------------------------------------------------------------------
		 釣り、採集、調達用ランク取得
		---------------------------------------------------------------------------*/
		public class ItemRank : IDictionaryNode<string>
		{
			private string					m_name;				// アイテム名
			private int						m_rank;				// 必要ランク

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public string Key			{	get{	return m_name;		}}
			public string Name			{	get{	return m_name;		}}
			public int Rank				{	get{	return m_rank;		}}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public ItemRank()
			{
				m_name			= "";
				m_rank			= 0;
			}

      internal static ItemRank FromXml(XmlNode n)
      {
        if (n == null)
          return (ItemRank) null;
        ItemRank itemRank = new ItemRank();
        itemRank.m_name = Useful.XmlGetAttribute(n, "name", itemRank.m_name);
        itemRank.m_rank = Useful.ToInt32(Useful.XmlGetAttribute(n, "rank", "0"), 0);
        if (string.IsNullOrEmpty(itemRank.m_name))
          return null;
        else
          return itemRank;
      }

      internal void WriteXml(XmlNode node)
      {
        Useful.XmlAddAttribute(Useful.XmlAddNode(node, "item", m_name), "rank", m_rank.ToString());
      }
		}
	}
}
