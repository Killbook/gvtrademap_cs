// Type: gvtrademap_cs.GvoWorldInfo
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using gvo_base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using Utility;

namespace gvtrademap_cs
{
  public class GvoWorldInfo : IDisposable
  {
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
    private gvt_lib m_lib;
    private gvo_season m_season;
    private draw_infonames m_draw_infonames;
    private hittest_list m_world;
    private MultiDictionary<string, GvoWorldInfo.Info> m_world_hash_table;
    private GvoItemTypeDatabase m_item_type_db;
    private hittest_list m_nonseas;
    private hittest_list m_seas;
    private GvoWorldInfo.Server m_server;
    private GvoWorldInfo.Country m_my_country;

    public hittest_list World
    {
      get
      {
        return this.m_world;
      }
    }

    public GvoWorldInfo.Server MyServer
    {
      get
      {
        return this.m_server;
      }
    }

    public GvoWorldInfo.Country MyCountry
    {
      get
      {
        return this.m_my_country;
      }
    }

    public hittest_list NoSeas
    {
      get
      {
        return this.m_nonseas;
      }
    }

    public hittest_list Seas
    {
      get
      {
        return this.m_seas;
      }
    }

    public gvo_season Season
    {
      get
      {
        return this.m_season;
      }
    }

    static GvoWorldInfo()
    {
    }

    public GvoWorldInfo(gvt_lib lib, gvo_season season, string world_info_fname, string memo_path)
    {
      this.m_lib = lib;
      this.m_season = season;
      this.m_world = new hittest_list();
      this.m_world_hash_table = new MultiDictionary<string, GvoWorldInfo.Info>();
      this.m_item_type_db = new GvoItemTypeDatabase();
      this.m_nonseas = new hittest_list();
      this.m_seas = new hittest_list();
      this.m_draw_infonames = new draw_infonames(lib, this);
      this.m_server = GvoWorldInfo.Server.Unknown;
      this.m_my_country = GvoWorldInfo.Country.Unknown;
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      this.load_info_xml(world_info_fname);
      Console.WriteLine("load_info_xml()=" + stopwatch.ElapsedMilliseconds.ToString());
      this.load_memo(memo_path);
      this.link_database();
      this.create_seas_list();
    }

    public void Dispose()
    {
      if (this.m_draw_infonames != null)
        this.m_draw_infonames.Dispose();
      this.m_draw_infonames = (draw_infonames) null;
    }

    private void link_database()
    {
      int num = 0;
      foreach (GvoWorldInfo.Info info in this.m_world)
      {
        if (info.InfoType != GvoWorldInfo.InfoType.Sea)
        {
          bool flag = true;
          if (info.CityInfo != null)
            flag = info.CityInfo.HasNameImage;
          info.UpdateDrawRects(this.m_lib, flag ? num : -1);
          if (flag)
            ++num;
        }
      }
    }

    private void create_seas_list()
    {
      this.m_nonseas.Clear();
      this.m_seas.Clear();
      foreach (GvoWorldInfo.Info info in this.m_world)
      {
        if (info.InfoType == GvoWorldInfo.InfoType.Sea)
        {
          if (info.SeaInfo != null)
            this.m_seas.Add((hittest) info);
        }
        else
          this.m_nonseas.Add((hittest) info);
      }
    }

    private void load_memo(string path)
    {
      foreach (GvoWorldInfo.Info info in this.m_world)
        info.LoadMemo(path);
    }

    public void WriteMemo(string path)
    {
      foreach (GvoWorldInfo.Info info in this.m_world)
        info.WriteMemo(path);
    }

    public GvoWorldInfo.Info FindInfo(Point map_pos)
    {
      return (GvoWorldInfo.Info) this.m_world.HitTest(map_pos);
    }

    public GvoWorldInfo.Info FindInfo_WithoutSea(Point map_pos)
    {
      GvoWorldInfo.Info info = (GvoWorldInfo.Info) this.m_world.HitTest(map_pos);
      if (info == null)
        return (GvoWorldInfo.Info) null;
      if (info.InfoType == GvoWorldInfo.InfoType.Sea)
        return (GvoWorldInfo.Info) null;
      else
        return (GvoWorldInfo.Info) this.m_world.HitTest(map_pos);
    }

    public GvoWorldInfo.Info FindInfo(string name)
    {
      if (string.IsNullOrEmpty(name))
        return (GvoWorldInfo.Info) null;
      else
        return this.m_world_hash_table.GetValue(name);
    }

    public void FindAll(string find_string, List<GvoDatabase.Find> list, GvoDatabase.Find.FindHandler handler)
    {
      foreach (GvoWorldInfo.Info info in this.m_world)
        info.FindAll(find_string, list, handler);
    }

    public void FindAll_FromType(string find_string, List<GvoDatabase.Find> list, GvoDatabase.Find.FindHandler handler)
    {
      foreach (GvoWorldInfo.Info info in this.m_world)
        info.FindAll_FromType(find_string, list, handler);
    }

    public GvoDatabase.Find[] CulturalSphereList()
    {
      List<GvoDatabase.Find> list = new List<GvoDatabase.Find>();
      foreach (GvoWorldInfo.CulturalSphere cs in Enum.GetValues(typeof (GvoWorldInfo.CulturalSphere)))
      {
        if (cs != GvoWorldInfo.CulturalSphere.Unknown)
        {
          string culturalSphereTooltip = this.get_cultural_sphere_tooltip(cs);
          list.Add(new GvoDatabase.Find(cs, culturalSphereTooltip));
        }
      }
      return list.ToArray();
    }

    private string get_cultural_sphere_tooltip(GvoWorldInfo.CulturalSphere cs)
    {
      string str = "";
      foreach (GvoWorldInfo.Info info in this.m_world)
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
      if (this.load_domains_from_old_data(this.download_domains()))
      {
        this.WriteDomains(domaininfo_file_name);
        Console.WriteLine("同盟国ダウンロ\x30FCド" + stopwatch.ElapsedMilliseconds.ToString());
        return true;
      }
      else
      {
        Console.WriteLine("同盟国ダウンロ\x30FCド(失敗)" + stopwatch.ElapsedMilliseconds.ToString());
        return false;
      }
    }

    public bool Load(string info_file_name, string domaininfo_file_name, string local_domaininfo_file_name)
    {
      this.load_domains_xml(domaininfo_file_name, local_domaininfo_file_name);
      this.m_item_type_db.Load();
      this.load_items_xml(info_file_name);
      this.SetServerAndCountry(GvoWorldInfo.Server.Euros, GvoWorldInfo.Country.England);
      return true;
    }

    public void LinkItemDatabase(ItemDatabaseCustom item_db)
    {
      foreach (GvoWorldInfo.Info info in this.m_world)
        info.LinkItemDatabase(item_db);
    }

    public void SetServerAndCountry(GvoWorldInfo.Server server, GvoWorldInfo.Country country)
    {
      if (this.m_server == server && this.m_my_country == country)
        return;
      this.m_server = server;
      this.m_my_country = country;
      this.update_domains();
    }

    private void update_domains()
    {
      foreach (GvoWorldInfo.Info info in this.m_world)
        info.UpdateDomains(this.m_item_type_db, this.m_server, this.m_my_country);
    }

    public bool SetDomain(string city_name, GvoWorldInfo.Country country)
    {
      GvoWorldInfo.Info info = this.FindInfo(city_name);
      if (info == null || info.CityInfo == null)
        return false;
      info.CityInfo.SetDomain(this.m_server, country);
      this.update_domains();
      return true;
    }

    public void DrawCityName()
    {
      this.m_draw_infonames.DrawCityName();
    }

    public void DrawSeaName()
    {
      this.m_draw_infonames.DrawSeaName();
    }

    public string GetNetUpdateString(string city_name)
    {
      GvoWorldInfo.Info info = this.FindInfo(city_name);
      if (info == null)
        return (string) null;
      if (info.CityInfo == null)
        return (string) null;
      else
        return info.CityInfo.GetNetUpdateString(this.m_server);
    }

    public static int GetTaxPrice(int price)
    {
      return price + (int) (0.14300000667572 * (double) price);
    }

    private void load_info_xml(string file_name)
    {
      this.m_world.Clear();
      this.m_world_hash_table.Clear();
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
            this.m_world.Add((hittest) t);
            this.m_world_hash_table.Add(t);
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
      foreach (GvoWorldInfo.Info info in this.m_world)
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
            GvoWorldInfo.Info info = this.FindInfo(n.Attributes["name"].Value);
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
      foreach (GvoWorldInfo.Info info in this.m_world)
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
            GvoWorldInfo.Info info = this.FindInfo(node.Attributes["name"].Value);
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
        foreach (GvoWorldInfo.Info info in this.m_world)
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

    public static GvoWorldInfo.Server GetServerFromString(string str)
    {
      return EnumParserUtility<GvoWorldInfo.Server>.ToEnum(str, GvoWorldInfo.Server.Unknown);
    }

    public static string GetCountryString(GvoWorldInfo.Country _country)
    {
      return EnumParserUtility<GvoWorldInfo.Country>.ToString(GvoWorldInfo.m_country_enum_param, _country, "所属無");
    }

    public static GvoWorldInfo.Country GetCountryFromString(string str)
    {
      return EnumParserUtility<GvoWorldInfo.Country>.ToEnum(GvoWorldInfo.m_country_enum_param, str, GvoWorldInfo.Country.Unknown);
    }

    public static string GetCityTypeString(GvoWorldInfo.CityType _city_type)
    {
      return EnumParserUtility<GvoWorldInfo.CityType>.ToString(GvoWorldInfo.m_citytype_enum_param, _city_type, "不明");
    }

    public static GvoWorldInfo.CityType GetCityTypeFromString(string str)
    {
      return EnumParserUtility<GvoWorldInfo.CityType>.ToEnum(GvoWorldInfo.m_citytype_enum_param, str, GvoWorldInfo.CityType.City);
    }

    public static string GetAllianceTypeString(GvoWorldInfo.AllianceType _alliance)
    {
      return EnumParserUtility<GvoWorldInfo.AllianceType>.ToString(GvoWorldInfo.m_alliancetype_enum_param, _alliance, "同盟なし");
    }

    public static GvoWorldInfo.AllianceType GetAllianceTypeFromString(string str)
    {
      return EnumParserUtility<GvoWorldInfo.AllianceType>.ToEnum(GvoWorldInfo.m_alliancetype_enum_param, str, GvoWorldInfo.AllianceType.Unknown);
    }

    public static string GetCulturalSphereString(GvoWorldInfo.CulturalSphere cs)
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
      foreach (GvoWorldInfo.Info info in this.m_world)
      {
        if (info.CityInfo != null)
          info.CityInfo.LoadDomainFromNeworkData(domains);
      }
      return true;
    }

    public enum InfoType
    {
      City,
      Sea,
      Shore,
      Shore2,
      OutsideCity,
      PF,
    }

    public enum Server
    {
      Euros,
      Zephyros,
      Notos,
      Boreas,
      Unknown,
    }

    public enum Country
    {
      Unknown,
      England,
      Spain,
      Portugal,
      Netherlands,
      France,
      Venezia,
      Turkey,
    }

    public enum CityType
    {
      Capital,
      City,
      CapitalIslam,
      CityIslam,
    }

    public enum AllianceType
    {
      Unknown,
      Alliance,
      Capital,
      Territory,
      Piratical,
    }

    public enum CulturalSphere
    {
      Unknown,
      NorthEurope,
      Germany,
      Netherlands,
      Britain,
      NorthFrance,
      Iberian,
      Atlantic,
      ItalySouthFrance,
      Balkan,
      Turkey,
      NearEast,
      NorthAfrica,
      WestAfrica,
      EastAfrica,
      Arab,
      Persia,
      India,
      Indochina,
      SoutheastAsia,
      Oceania,
      Caribbean,
      EastLatinAmerica,
      WestLatinAmerica,
      China,
      Japan,
      Taiwan,
      Korea,
      NorthAmerica,
    }

    public class Info : hittest, IDictionaryNode<string>
    {
      private string[] m_group_name_tbl = new string[12]
      {
        "[交易]",
        "[道具]",
        "[工房]",
        "[人物]",
        "[船]",
        "[船大工]",
        "[大砲]",
        "[板]",
        "[帆]",
        "[像]",
        "[行商人]",
        ""
      };
      private const int CHECK_MARGIN = 10;
      private string m_name;
      private GvoWorldInfo.InfoType m_info_type;
      private int m_url_index;
      private string m_url;
      private GvoWorldInfo.Server m_server;
      private GvoWorldInfo.CityInfo m_cityinfo;
      private GvoWorldInfo.SeaInfo m_seainfo;
      private string m_memo;
      private string[] m_memo_div_lines;
      private Point m_string_offset1;
      private Point m_string_offset2;
      private d3d_sprite_rects.rect m_icon_rect;
      private d3d_sprite_rects.rect m_small_icon_rect;
      private d3d_sprite_rects.rect m_string_rect;
      private List<GvoWorldInfo.Info.Group> m_groups;

      public string Key
      {
        get
        {
          return this.m_name;
        }
      }

      public string Name
      {
        get
        {
          return this.m_name;
        }
      }

      public GvoWorldInfo.InfoType InfoType
      {
        get
        {
          return this.m_info_type;
        }
      }

      public string InfoTypeStr
      {
        get
        {
          return GvoWorldInfo.GetInfoTypeString(this.m_info_type);
        }
      }

      public int UrlIndex
      {
        get
        {
          return this.m_url_index;
        }
      }

      public string Url
      {
        get
        {
          return this.m_url;
        }
      }

      public GvoWorldInfo.Server MyServer
      {
        get
        {
          return this.m_server;
        }
      }

      public GvoWorldInfo.Country MyCountry
      {
        get
        {
          if (this.m_cityinfo == null)
            return GvoWorldInfo.Country.Unknown;
          else
            return this.m_cityinfo.GetDomain(this.m_server);
        }
      }

      public string CountryStr
      {
        get
        {
          return GvoWorldInfo.GetCountryString(this.MyCountry);
        }
      }

      public string Lang1
      {
        get
        {
          if (this.m_cityinfo != null)
            return this.m_cityinfo.Lang1;
          else
            return "";
        }
      }

      public string Lang2
      {
        get
        {
          if (this.m_cityinfo != null)
            return this.m_cityinfo.Lang2;
          else
            return "";
        }
      }

      public int Sakaba
      {
        get
        {
          if (this.m_cityinfo != null)
            return this.m_cityinfo.Sakaba;
          else
            return 0;
        }
      }

      public GvoWorldInfo.CityType CityType
      {
        get
        {
          if (this.m_cityinfo != null)
            return this.m_cityinfo.CityType;
          else
            return GvoWorldInfo.CityType.City;
        }
      }

      public string CityTypeStr
      {
        get
        {
          return GvoWorldInfo.GetCityTypeString(this.CityType);
        }
      }

      public GvoWorldInfo.AllianceType AllianceType
      {
        get
        {
          if (this.m_cityinfo != null)
            return this.m_cityinfo.AllianceType;
          else
            return GvoWorldInfo.AllianceType.Unknown;
        }
      }

      public string AllianceTypeStr
      {
        get
        {
          return GvoWorldInfo.GetAllianceTypeString(this.AllianceType);
        }
      }

      public GvoWorldInfo.CulturalSphere CulturalSphere
      {
        get
        {
          if (this.m_cityinfo != null)
            return this.m_cityinfo.CulturalSphere;
          else
            return GvoWorldInfo.CulturalSphere.Unknown;
        }
      }

      public string CulturalSphereStr
      {
        get
        {
          return GvoWorldInfo.GetCulturalSphereString(this.CulturalSphere);
        }
      }

      public GvoWorldInfo.SeaInfo SeaInfo
      {
        get
        {
          return this.m_seainfo;
        }
        internal set
        {
          this.m_seainfo = value;
        }
      }

      public GvoWorldInfo.CityInfo CityInfo
      {
        get
        {
          return this.m_cityinfo;
        }
        internal set
        {
          this.m_cityinfo = value;
        }
      }

      public string Memo
      {
        get
        {
          return this.m_memo;
        }
        set
        {
          this.m_memo = value;
          this.div_memo_lines();
        }
      }

      public bool IsUrl
      {
        get
        {
          return this.UrlIndex >= 0 || this.Url != "";
        }
      }

      public Point StringOffset1
      {
        get
        {
          return this.m_string_offset1;
        }
      }

      public Point StringOffset2
      {
        get
        {
          return this.m_string_offset2;
        }
      }

      public d3d_sprite_rects.rect IconRect
      {
        get
        {
          return this.m_icon_rect;
        }
      }

      public d3d_sprite_rects.rect SmallIconRect
      {
        get
        {
          return this.m_small_icon_rect;
        }
      }

      public d3d_sprite_rects.rect NameRect
      {
        get
        {
          return this.m_string_rect;
        }
      }

      public string TooltipString
      {
        get
        {
          return this.__get_tool_tip_string();
        }
      }

      private Info()
      {
        int num = 0;
        this.m_groups = new List<GvoWorldInfo.Info.Group>();
        foreach (string name in this.m_group_name_tbl)
          this.m_groups.Add(new GvoWorldInfo.Info.Group(name, this, num++));
        this.m_memo = "";
        this.div_memo_lines();
        this.rect = new Rectangle(-10, -10, 20, 20);
        this.m_server = GvoWorldInfo.Server.Euros;
        this.m_cityinfo = (GvoWorldInfo.CityInfo) null;
        this.m_seainfo = (GvoWorldInfo.SeaInfo) null;
        this.m_icon_rect = (d3d_sprite_rects.rect) null;
        this.m_small_icon_rect = (d3d_sprite_rects.rect) null;
        this.m_string_rect = (d3d_sprite_rects.rect) null;
      }

      public GvoWorldInfo.Info.Group.Data HasItem(string find_item)
      {
        foreach (GvoWorldInfo.Info.Group group in this.m_groups)
        {
          GvoWorldInfo.Info.Group.Data data = group.HasItem(find_item);
          if (data != null)
            return data;
        }
        return (GvoWorldInfo.Info.Group.Data) null;
      }

      public void LoadMemo(string path)
      {
        path = path + this.m_name + "_memo.txt";
        if (!File.Exists(path))
          return;
        try
        {
          using (StreamReader streamReader = new StreamReader(path, Encoding.GetEncoding("Shift_JIS")))
            this.m_memo = streamReader.ReadToEnd();
          this.div_memo_lines();
        }
        catch
        {
          this.m_memo = "";
        }
      }

      public void WriteMemo(string path)
      {
        path = path + this.m_name + "_memo.txt";
        if (this.m_memo == "")
        {
          file_ctrl.RemoveFile(path);
        }
        else
        {
          try
          {
            using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.GetEncoding("Shift_JIS")))
              streamWriter.Write(this.Memo);
          }
          catch
          {
          }
        }
      }

      public string LearnPerson(string find_str)
      {
        GvoWorldInfo.Info.Group.Data data = this.m_groups[3].HasItem(find_str);
        if (data == null)
          return (string) null;
        else
          return data.Price;
      }

      public int GetCount(GvoWorldInfo.Info.GroupIndex index)
      {
        if (index < GvoWorldInfo.Info.GroupIndex._0 || index >= GvoWorldInfo.Info.GroupIndex.max)
          return 0;
        else
          return this.m_groups[(int) index].GetCount();
      }

      public GvoWorldInfo.Info.Group GetGroup(GvoWorldInfo.Info.GroupIndex index)
      {
        if (index < GvoWorldInfo.Info.GroupIndex._0)
          return (GvoWorldInfo.Info.Group) null;
        if (index >= GvoWorldInfo.Info.GroupIndex.max)
          return (GvoWorldInfo.Info.Group) null;
        else
          return this.m_groups[(int) index];
      }

      private GvoWorldInfo.Info.Group get_group(string name)
      {
        foreach (GvoWorldInfo.Info.Group group in this.m_groups)
        {
          if (group.Name == name)
            return group;
        }
        return (GvoWorldInfo.Info.Group) null;
      }

      public GvoWorldInfo.Info.Group.Data GetData(GvoWorldInfo.Info.GroupIndex index, int data_index)
      {
        GvoWorldInfo.Info.Group group = this.GetGroup(index);
        if (group == null)
          return (GvoWorldInfo.Info.Group.Data) null;
        else
          return group.GetData(data_index);
      }

      public void UpdateDomains(GvoItemTypeDatabase type, GvoWorldInfo.Server server, GvoWorldInfo.Country my_country)
      {
        this.m_server = server;
        foreach (GvoWorldInfo.Info.Group group in this.m_groups)
          group.UpdateDomains(type, this.MyCountry != my_country);
      }

      public void UpdateDrawRects(gvt_lib lib, int index)
      {
        if (this.m_info_type == GvoWorldInfo.InfoType.Sea)
          return;
        if (this.m_info_type == GvoWorldInfo.InfoType.City)
        {
          switch (this.CityType)
          {
            case GvoWorldInfo.CityType.Capital:
              this.m_icon_rect = lib.infonameimage.GetIcon(0);
              break;
            case GvoWorldInfo.CityType.City:
              this.m_icon_rect = lib.infonameimage.GetIcon(this.AllianceType == GvoWorldInfo.AllianceType.Territory ? 2 : 3);
              break;
            case GvoWorldInfo.CityType.CapitalIslam:
              this.m_icon_rect = lib.infonameimage.GetIcon(1);
              break;
            case GvoWorldInfo.CityType.CityIslam:
              this.m_icon_rect = lib.infonameimage.GetIcon(this.AllianceType == GvoWorldInfo.AllianceType.Territory ? 4 : 5);
              break;
          }
          this.m_small_icon_rect = lib.infonameimage.GetIcon(8);
          this.m_string_rect = lib.infonameimage.GetCityName(index);
        }
        else
        {
          switch (this.InfoType)
          {
            case GvoWorldInfo.InfoType.Shore:
              this.m_icon_rect = lib.infonameimage.GetIcon(9);
              break;
            case GvoWorldInfo.InfoType.Shore2:
              this.m_icon_rect = lib.infonameimage.GetIcon(6);
              break;
            case GvoWorldInfo.InfoType.OutsideCity:
              this.m_icon_rect = lib.infonameimage.GetIcon(7);
              break;
            case GvoWorldInfo.InfoType.PF:
              this.m_icon_rect = lib.infonameimage.GetIcon(11);
              break;
          }
          this.m_small_icon_rect = this.m_icon_rect;
          this.m_string_rect = lib.infonameimage.GetCityName(index);
        }
      }

      public static string GetGroupName(GvoWorldInfo.Info.GroupIndex index)
      {
        switch (index)
        {
          case GvoWorldInfo.Info.GroupIndex._0:
            return "交易所店主";
          case GvoWorldInfo.Info.GroupIndex._1:
            return "道具屋主人";
          case GvoWorldInfo.Info.GroupIndex._2:
            return "工房職人";
          case GvoWorldInfo.Info.GroupIndex._3:
            return "人物";
          case GvoWorldInfo.Info.GroupIndex._4:
            return "造船所親方";
          case GvoWorldInfo.Info.GroupIndex._4_1:
            return "船大工";
          case GvoWorldInfo.Info.GroupIndex._5:
            return "武器職人";
          case GvoWorldInfo.Info.GroupIndex._6:
            return "製材職人";
          case GvoWorldInfo.Info.GroupIndex._7:
            return "製帆職人";
          case GvoWorldInfo.Info.GroupIndex._8:
            return "彫刻家";
          case GvoWorldInfo.Info.GroupIndex._9:
            return "行商人";
          case GvoWorldInfo.Info.GroupIndex.max:
            return "メモ";
          default:
            return "不明";
        }
      }

      public int GetMemoLines()
      {
        if (this.m_memo_div_lines == null)
          return 0;
        else
          return this.m_memo_div_lines.Length;
      }

      public string GetMemo(int line_index)
      {
        if (line_index < 0 || line_index >= this.GetMemoLines())
          return "";
        else
          return this.m_memo_div_lines[line_index];
      }

      private void div_memo_lines()
      {
        if (this.m_memo == "")
          this.m_memo_div_lines = (string[]) null;
        else
          this.m_memo_div_lines = this.m_memo.Split(new char[1]
          {
            '\n'
          });
      }

      internal void LinkItemDatabase(ItemDatabaseCustom item_db)
      {
        foreach (GvoWorldInfo.Info.Group group in this.m_groups)
          group.LinkItemDatabase(item_db, this.Name);
      }

      public string GetToolTipString_HP()
      {
        if (!this.IsUrl)
          return (string) null;
        if (this.UrlIndex == -1)
          return "大航海時代Online上陸地点地図(仮)へ\n左クリックでブラウザを開く";
        return this.InfoType == GvoWorldInfo.InfoType.City ? "大商戦へ\n左クリックでブラウザを開く" : "D.K.K_mapへ\n左クリックでブラウザを開く";
      }

      public void FindAll(string find_string, List<GvoDatabase.Find> list, GvoDatabase.Find.FindHandler handler)
      {
        if (handler(this.Name, find_string))
          list.Add(new GvoDatabase.Find(this.Name));
        if (handler(this.Lang1, find_string))
          list.Add(new GvoDatabase.Find(this.Name, this.Lang1));
        if (handler(this.Lang2, find_string))
          list.Add(new GvoDatabase.Find(this.Name, this.Lang2));
        foreach (GvoWorldInfo.Info.Group group in this.m_groups)
          group.FindAll(find_string, list, this.Name, handler);
      }

      public void FindAll_FromType(string find_string, List<GvoDatabase.Find> list, GvoDatabase.Find.FindHandler handler)
      {
        foreach (GvoWorldInfo.Info.Group group in this.m_groups)
          group.FindAll_FromType(find_string, list, this.Name, handler);
      }

      private string __get_tool_tip_string()
      {
        string str = this.Name + "\n";
        if (this.SeaInfo != null)
          return str + this.SeaInfo.TooltipString;
        switch (this.InfoType)
        {
          case GvoWorldInfo.InfoType.City:
            switch (this.AllianceType)
            {
              case GvoWorldInfo.AllianceType.Unknown:
              case GvoWorldInfo.AllianceType.Piratical:
                str = str + this.AllianceTypeStr;
                break;
              case GvoWorldInfo.AllianceType.Alliance:
                str = str + this.AllianceTypeStr + " " + this.CountryStr;
                break;
              case GvoWorldInfo.AllianceType.Capital:
              case GvoWorldInfo.AllianceType.Territory:
                str = str + this.CountryStr + " " + this.AllianceTypeStr;
                break;
            }
            str = str + "\n種類:" + this.CityTypeStr + "\n文化圏:" + this.CulturalSphereStr;
            if (this.Lang1 != "")
              str = str + "\n使用言語:" + this.Lang1;
            if (this.Lang2 != "")
            {
              str = str + "\n使用言語:" + this.Lang2;
              break;
            }
            else
              break;
          case GvoWorldInfo.InfoType.Sea:
          case GvoWorldInfo.InfoType.Shore:
          case GvoWorldInfo.InfoType.Shore2:
          case GvoWorldInfo.InfoType.OutsideCity:
          case GvoWorldInfo.InfoType.PF:
            str = str + this.InfoTypeStr;
            break;
        }
        return str;
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
        XmlNode node = Useful.XmlAddNode((XmlNode) p_node, "info", this.Name);
        this.write_info_sub(node);
        if (this.m_cityinfo != null)
          this.m_cityinfo.WriteXml(node);
        if (this.m_seainfo == null)
          return;
        this.m_seainfo.WriteXml(node);
      }

      internal void LoadItemsXml(XmlNode n)
      {
        if (n.ChildNodes == null || n.ChildNodes.Count <= 0)
          return;
        foreach (XmlNode n1 in n.ChildNodes)
        {
          if (n1.Attributes["name"] != null)
          {
            GvoWorldInfo.Info.Group group = this.get_group(n1.Attributes["name"].Value);
            if (group != null)
              group.LoadXml(n1);
          }
        }
      }

      private void write_info_sub(XmlNode node)
      {
        Useful.XmlAddPoint(node, "position", this.position);
        if (this.m_url_index == -1)
          Useful.XmlAddAttribute(node, "url_string", this.m_url);
        else
          Useful.XmlAddAttribute(node, "url_index", this.m_url_index.ToString());
        Useful.XmlAddAttribute(node, "info_type", ((object) this.m_info_type).ToString());
        Useful.XmlAddPoint(node, "name_offset1", this.m_string_offset1);
        Useful.XmlAddPoint(node, "name_offset2", this.m_string_offset2);
      }

      internal void WriteItemsXml(XmlElement xmlElement)
      {
        XmlNode xmlNode = Useful.XmlAddNode((XmlNode) xmlElement, "cityinfo", this.Name);
        foreach (GvoWorldInfo.Info.Group group in this.m_groups)
          group.WriteInfoXml(xmlNode);
        if (xmlNode.ChildNodes.Count > 0)
          return;
        xmlElement.RemoveChild(xmlNode);
      }

      public enum GroupIndex
      {
        _0,
        _1,
        _2,
        _3,
        _4,
        _4_1,
        _5,
        _6,
        _7,
        _8,
        _9,
        max,
      }

      public class Group
      {
        private string m_name;
        private List<GvoWorldInfo.Info.Group.Data> m_datas;
        private GvoWorldInfo.Info m_info;
        private int m_index;

        public string Name
        {
          get
          {
            return this.m_name;
          }
        }

        public GvoWorldInfo.Info Info
        {
          get
          {
            return this.m_info;
          }
        }

        public int Index
        {
          get
          {
            return this.m_index;
          }
        }

        public Group(string name, GvoWorldInfo.Info info, int index)
        {
          this.m_name = name;
          this.m_info = info;
          this.m_index = index;
          this.m_datas = new List<GvoWorldInfo.Info.Group.Data>();
        }

        public GvoWorldInfo.Info.Group.Data HasItem(string find_item)
        {
          foreach (GvoWorldInfo.Info.Group.Data data in this.m_datas)
          {
            if (data.Name == find_item)
              return data;
          }
          return (GvoWorldInfo.Info.Group.Data) null;
        }

        public int GetSakabaFlag()
        {
          int num = 0;
          foreach (GvoWorldInfo.Info.Group.Data data in this.m_datas)
          {
            if (data.Tag == "@")
              num |= 4;
            else if (data.Tag == "#")
              num |= 8;
            else if (data.Tag == "")
              data.Tag = "-";
          }
          return num;
        }

        public int GetCount()
        {
          return this.m_datas.Count;
        }

        public GvoWorldInfo.Info.Group.Data GetData(int index)
        {
          if (index < 0)
            return (GvoWorldInfo.Info.Group.Data) null;
          if (index >= this.GetCount())
            return (GvoWorldInfo.Info.Group.Data) null;
          else
            return this.m_datas[index];
        }

        internal void UpdateDomains(GvoItemTypeDatabase item_type_db, bool is_tax)
        {
          foreach (GvoWorldInfo.Info.Group.Data data in this.m_datas)
            data.UpdateDomains(item_type_db, is_tax);
        }

        internal void LinkItemDatabase(ItemDatabaseCustom item_db, string name)
        {
          foreach (GvoWorldInfo.Info.Group.Data data in this.m_datas)
            data.LinkItemDatabase(item_db, name);
        }

        public void FindAll(string find_string, List<GvoDatabase.Find> list, string info_name, GvoDatabase.Find.FindHandler handler)
        {
          foreach (GvoWorldInfo.Info.Group.Data _data in this.m_datas)
          {
            if (handler(_data.Name, find_string))
              list.Add(new GvoDatabase.Find(GvoDatabase.Find.FindType.Data, info_name, _data));
            if (handler(_data.Price, find_string))
              list.Add(new GvoDatabase.Find(GvoDatabase.Find.FindType.DataPrice, info_name, _data));
          }
        }

        public void FindAll_FromType(string find_string, List<GvoDatabase.Find> list, string info_name, GvoDatabase.Find.FindHandler handler)
        {
          foreach (GvoWorldInfo.Info.Group.Data _data in this.m_datas)
          {
            if (handler(_data.Type, find_string))
              list.Add(new GvoDatabase.Find(GvoDatabase.Find.FindType.Data, info_name, _data));
          }
        }

        internal void LoadXml(XmlNode n)
        {
          this.m_datas.Clear();
          if (n.ChildNodes == null || n.ChildNodes.Count <= 0)
            return;
          foreach (XmlNode nn in n.ChildNodes)
          {
            GvoWorldInfo.Info.Group.Data data = GvoWorldInfo.Info.Group.Data.FromXml(nn, this.m_info, this.m_index);
            if (data != null)
              this.m_datas.Add(data);
          }
        }

        internal void WriteInfoXml(XmlNode node)
        {
          if (this.m_datas.Count <= 0)
            return;
          XmlNode node1 = Useful.XmlAddNode(node, "group", this.Name);
          if (node1 == null)
            return;
          foreach (GvoWorldInfo.Info.Group.Data data in this.m_datas)
            data.WriteInfoXml(node1);
        }

        public class Data
        {
          private GvoWorldInfo.Info m_info;
          private string m_name;
          private string m_tag;
          private string m_tag2;
          private int m_group_index;
          private bool m_is_bonus_item;
          private string m_price;
          private Color m_color;
          private Color m_price_color;
          private string m_investment;
          private ItemDatabase.Data m_item_db;

          public string Name
          {
            get
            {
              return this.m_name;
            }
          }

          public GvoWorldInfo.Info Info
          {
            get
            {
              return this.m_info;
            }
          }

          public bool IsBonusItem
          {
            get
            {
              return this.m_is_bonus_item;
            }
          }

          public string Price
          {
            get
            {
              return this.m_price;
            }
          }

          public string GroupIndexString
          {
            get
            {
              return GvoWorldInfo.Info.GetGroupName((GvoWorldInfo.Info.GroupIndex) this.m_group_index);
            }
          }

          public Color Color
          {
            get
            {
              return this.m_color;
            }
          }

          public Color PriceColor
          {
            get
            {
              return this.m_price_color;
            }
          }

          public string Investment
          {
            get
            {
              return this.m_investment;
            }
          }

          public ItemDatabase.Data ItemDb
          {
            get
            {
              return this.m_item_db;
            }
          }

          public bool HasTooltip
          {
            get
            {
              return this.ItemDb != null;
            }
          }

          public string TooltipString
          {
            get
            {
              if (this.ItemDb == null)
                return "名称:" + this.Name + "\nアイテムデ\x30FCタベ\x30FCスに詳細が見つかりませんでした。\n" + "名称が微妙に間違っているか未知のデ\x30FCタです。\n";
              else
                return this.getMixedToolTipString();
            }
          }

          public string Type
          {
            get
            {
              if (this.ItemDb == null)
                return "不明";
              else
                return this.ItemDb.Type;
            }
          }

          public ItemDatabase.Categoly Categoly
          {
            get
            {
              if (this.ItemDb == null)
                return ItemDatabase.Categoly.Unknown;
              else
                return this.ItemDb.Categoly;
            }
          }

          public Color CategolyColor
          {
            get
            {
              if (this.ItemDb == null)
                return Color.Black;
              else
                return this.ItemDb.CategolyColor;
            }
          }

          internal string Tag
          {
            get
            {
              return this.m_tag;
            }
            set
            {
              this.m_tag = value;
            }
          }

          internal string Tag2
          {
            get
            {
              return this.m_tag2;
            }
          }

          private Data(GvoWorldInfo.Info info, int index)
          {
            this.m_name = "unknown";
            this.m_tag = "";
            this.m_tag2 = "0";
            this.m_investment = "";
            this.m_info = info;
            this.m_group_index = index;
            this.m_is_bonus_item = false;
            this.m_price = "";
            this.m_color = Color.Black;
            this.m_price_color = Color.Black;
            this.m_item_db = (ItemDatabase.Data) null;
          }

          private Color _get_color()
          {
            switch (this.m_tag)
            {
              case "s":
                return Color.Black;
              case "h":
                return Color.DarkRed;
              case "*":
                return Color.Gray;
              case "$":
                return Color.Blue;
              case "%":
                return Color.DarkGreen;
              case "@":
                return Color.DarkRed;
              case "+":
                return Color.MediumPurple;
              case "#":
                return Color.Green;
              case "-":
                return Color.Gray;
              default:
                return Color.Black;
            }
          }

          private string _get_price()
          {
            switch (this.m_tag)
            {
              case "$":
                return "投資";
              case "%":
                return "備え付け";
              case "@":
                return "翻訳家";
              case "+":
                return "販売員";
              case "#":
                return "豪商";
              case "-":
                return "行商人";
              default:
                return this.calc_price(false);
            }
          }

          private string calc_price(bool is_tax)
          {
            int result;
            if (!int.TryParse(this.m_tag2, out result))
              result = 0;
            if (is_tax)
              result = GvoWorldInfo.GetTaxPrice(result);
            return string.Format("{0:#,0}", (object) result);
          }

          internal void UpdateDomains(GvoItemTypeDatabase item_type_db, bool is_tax)
          {
            this.m_color = this._get_color();
            this.m_price_color = this.m_color;
            if (this.m_group_index == 0)
            {
              this.m_is_bonus_item = item_type_db.IsBonusItem(this.Name);
              if (this.m_info.InfoType == GvoWorldInfo.InfoType.City)
              {
                if (item_type_db.IsNanbanTradeItem(this.Name))
                  this.m_price = "南蛮";
                else
                  this.m_price = this.calc_price(is_tax);
              }
              else
                this.update_rank(item_type_db);
            }
            else
            {
              this.m_is_bonus_item = false;
              if (this.m_group_index != 3)
                this.m_price = this._get_price();
              else
                this.m_price = this.Tag2;
            }
          }

          private void update_rank(GvoItemTypeDatabase type)
          {
            switch (Useful.ToInt32(this.m_tag2, 0))
            {
              case -3:
                this.m_price = "探索";
                this.m_price_color = Color.DarkViolet;
                break;
              case -2:
                this.m_price = "調達R" + this.rank_to_str(type.GetChotatuRank(this.Name));
                this.m_price_color = Color.Olive;
                break;
              case -1:
                this.m_price = "採集R" + this.rank_to_str(type.GetSaisyuRank(this.Name));
                this.m_price_color = Color.DarkCyan;
                break;
              default:
                this.m_price = "釣りR" + this.rank_to_str(type.GetFishingRank(this.Name));
                this.m_price_color = Color.Gray;
                break;
            }
          }

          private string rank_to_str(int rank)
          {
            if (rank < 1 || rank >= 20)
              return "??";
            else
              return rank.ToString();
          }

          internal void LinkItemDatabase(ItemDatabaseCustom db, string info_name)
          {
            if (string.IsNullOrEmpty(this.Name))
              return;
            ItemDatabase.Data data = db.Find(this.Name);
            if (data == null)
              Console.WriteLine(string.Format("{0} {1}", (object) info_name, (object) this.Name));
            this.m_item_db = data;
          }

          private static void update_rename(GvoWorldInfo.Info.Group.Data d)
          {
            switch (d.m_name)
            {
              case "仕立て道具":
                d.m_name = "裁縫道具";
                d.m_tag2 = "10000";
                break;
              case "回避指南書第1巻":
                d.m_name = "連撃指南書第1巻";
                break;
              case "攻撃指南書第1巻":
                d.m_name = "猛攻指南書第1巻";
                break;
              case "回復指南書第1巻":
                d.m_name = "活用指南書第1巻";
                break;
              case "防御指南書第1巻":
                d.m_name = "奇手指南書第1巻";
                break;
              case "パデットロ\x30FCル":
                d.m_name = "パデッドロ\x30FCル";
                break;
              case "診察室の製法":
                d.m_name = "造船素材・診察室";
                break;
              case "天子の像":
                d.m_name = "天使の像";
                break;
              case "海域":
                d.m_name = "地理";
                break;
              case "高級上納品の梱包":
                d.m_name = "高級上納品の梱包(NO.1)";
                break;
            }
          }

          private string getMixedToolTipString()
          {
            if (this.ItemDb == null)
              return "";
            string str1 = "名称:" + this.ItemDb.Name + "\n";
            string str2;
            if (this.Categoly != ItemDatabase.Categoly.Unknown)
              str2 = str1 + "種類:" + this.Type + "(カテゴリ" + ((int) (this.Categoly + 1)).ToString() + ")\n";
            else
              str2 = str1 + "種類:" + this.Type + "\n";
            if (this.Investment != "")
              str2 = str2 + "投資:" + this.Investment + "\n";
            return str2 + "説明:\n" + this.ItemDb.Document;
          }

          internal static GvoWorldInfo.Info.Group.Data FromXml(XmlNode nn, GvoWorldInfo.Info info, int index)
          {
            GvoWorldInfo.Info.Group.Data data = new GvoWorldInfo.Info.Group.Data(info, index);
            data.m_name = Useful.XmlGetAttribute(nn, "name", data.m_name);
            data.m_tag = Useful.XmlGetAttribute(nn, "option", data.m_tag);
            data.m_tag2 = Useful.XmlGetAttribute(nn, "price", data.m_tag2);
            data.m_investment = Useful.XmlGetAttribute(nn, "investment", data.m_investment);
            return data;
          }

          internal void WriteInfoXml(XmlNode node)
          {
            XmlNode node1 = Useful.XmlAddNode(node, "item", this.Name);
            Useful.XmlAddAttribute(node1, "option", this.m_tag);
            if (this.m_tag2 != "0")
              Useful.XmlAddAttribute(node1, "price", this.m_tag2);
            Useful.XmlAddAttribute(node1, "investment", this.m_investment);
          }
        }
      }
    }

    public class SeaInfo
    {
      private string m_name;
      private Point m_wind_pos;
      private float m_summer_angle;
      private float m_winter_angle;
      private int m_speedup_rate;
      private int m_summer_angle_deg;
      private int m_winter_angle_deg;
      private string m_summer_angle_string;
      private string m_winter_angle_string;

      public string Name
      {
        get
        {
          return this.m_name;
        }
      }

      public Point WindPos
      {
        get
        {
          return this.m_wind_pos;
        }
      }

      public float SummerAngle
      {
        get
        {
          return this.m_summer_angle;
        }
      }

      public string SummerAngleString
      {
        get
        {
          return "夏:" + this.m_summer_angle_string;
        }
      }

      public float WinterAngle
      {
        get
        {
          return this.m_winter_angle;
        }
      }

      public string WinterAngleString
      {
        get
        {
          return "冬:" + this.m_winter_angle_string;
        }
      }

      public int SpeedUpRate
      {
        get
        {
          return this.m_speedup_rate;
        }
      }

      public string SpeedUpRateString
      {
        get
        {
          return "速度上昇:" + (this.SpeedUpRate == 0 ? "未調査" : this.SpeedUpRate.ToString() + "%");
        }
      }

      public string TooltipString
      {
        get
        {
          return "" + this.SummerAngleString + "\n" + this.WinterAngleString + "\n" + this.SpeedUpRateString;
        }
      }

      private SeaInfo(string name)
      {
        this.m_name = name;
        this.m_wind_pos = Point.Empty;
        this.m_speedup_rate = 0;
        this.m_summer_angle_deg = 0;
        this.m_winter_angle_deg = 0;
        this.update_angle();
      }

      private void update_angle()
      {
        this.m_summer_angle = Useful.ToRadian((float) this.m_summer_angle_deg);
        this.m_winter_angle = Useful.ToRadian((float) this.m_winter_angle_deg);
        this.m_summer_angle_string = this.angle_to_string(this.m_summer_angle_deg);
        this.m_winter_angle_string = this.angle_to_string(this.m_winter_angle_deg);
      }

      private string angle_to_string(int angle)
      {
        float num = (float) angle + 11.25f;
        if ((double) num < 0.0)
          num = 0.0f;
        angle = (int) ((double) num / 22.5);
        switch (angle)
        {
          case 0:
            return "南の風";
          case 1:
            return "南南西の風";
          case 2:
            return "南西の風";
          case 3:
            return "西南西の風";
          case 4:
            return "西の風";
          case 5:
            return "西北西の風";
          case 6:
            return "北西の風";
          case 7:
            return "北北西の風";
          case 8:
            return "北の風";
          case 9:
            return "北北東の風";
          case 10:
            return "北東の風";
          case 11:
            return "東北東の風";
          case 12:
            return "東の風";
          case 13:
            return "東南東の風";
          case 14:
            return "南東の風";
          default:
            return "南南東の風";
        }
      }

      internal static GvoWorldInfo.SeaInfo FromXml(XmlNode node, string name)
      {
        if (node == null)
          return (GvoWorldInfo.SeaInfo) null;
        if (node.Name != "sea_detail")
          return (GvoWorldInfo.SeaInfo) null;
        if (node.ChildNodes == null)
          return (GvoWorldInfo.SeaInfo) null;
        GvoWorldInfo.SeaInfo seaInfo = new GvoWorldInfo.SeaInfo(name);
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
        Useful.XmlAddAttribute(xmlNode, "speedup_rate", this.m_speedup_rate.ToString());
        Useful.XmlAddAttribute(xmlNode, "summer_angle_deg", this.m_summer_angle_deg.ToString());
        Useful.XmlAddAttribute(xmlNode, "winter_angle_deg", this.m_winter_angle_deg.ToString());
        Useful.XmlAddPoint(xmlNode, "name_position", this.m_wind_pos);
      }
    }

    public class CityInfo
    {
      private string m_name;
      private GvoWorldInfo.Country[] m_domains;
      private int m_index;
      private GvoWorldInfo.CityType m_city_type;
      private GvoWorldInfo.AllianceType m_alliance_type;
      private GvoWorldInfo.CulturalSphere m_cultural_sphere;
      private bool m_has_name_image;
      private string m_lang1;
      private string m_lang2;
      private int m_sakaba_flag;

      public string Name
      {
        get
        {
          return this.m_name;
        }
      }

      public int Index
      {
        get
        {
          return this.m_index;
        }
      }

      public GvoWorldInfo.CityType CityType
      {
        get
        {
          return this.m_city_type;
        }
      }

      public GvoWorldInfo.AllianceType AllianceType
      {
        get
        {
          return this.m_alliance_type;
        }
      }

      public GvoWorldInfo.CulturalSphere CulturalSphere
      {
        get
        {
          return this.m_cultural_sphere;
        }
      }

      public bool HasNameImage
      {
        get
        {
          return this.m_has_name_image;
        }
      }

      public string Lang1
      {
        get
        {
          return this.m_lang1;
        }
      }

      public string Lang2
      {
        get
        {
          return this.m_lang2;
        }
      }

      public int Sakaba
      {
        get
        {
          return this.m_sakaba_flag;
        }
      }

      private CityInfo(string name)
      {
        this.m_name = name;
        this.m_index = 0;
        this.m_city_type = GvoWorldInfo.CityType.City;
        this.m_alliance_type = GvoWorldInfo.AllianceType.Alliance;
        this.m_cultural_sphere = GvoWorldInfo.CulturalSphere.Unknown;
        this.m_has_name_image = false;
        this.m_lang1 = "";
        this.m_lang2 = "";
        this.m_sakaba_flag = 0;
        this.m_domains = new GvoWorldInfo.Country[Enum.GetValues(typeof (GvoWorldInfo.Server)).Length];
        for (int index = 0; index < this.m_domains.Length; ++index)
          this.m_domains[index] = GvoWorldInfo.Country.Unknown;
      }

      public void SetDomain(GvoWorldInfo.Server server_index, GvoWorldInfo.Country country_index)
      {
        if (this.AllianceType != GvoWorldInfo.AllianceType.Alliance)
          return;
        this.m_domains[(int) server_index] = country_index;
      }

      public void SetDomain(GvoWorldInfo.Country country_index)
      {
        if (this.AllianceType == GvoWorldInfo.AllianceType.Alliance)
          return;
        for (int index = 0; index < this.m_domains.Length; ++index)
          this.m_domains[index] = country_index;
      }

      public string GetNetUpdateString(GvoWorldInfo.Server server_index)
      {
        return ((int) server_index).ToString() + "+" + this.Index.ToString() + "+" + ((int) this.GetDomain(server_index)).ToString();
      }

      public GvoWorldInfo.Country GetDomain(GvoWorldInfo.Server server_index)
      {
        return this.m_domains[(int) server_index];
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
        Useful.XmlAddAttribute(node1, "index", this.m_index.ToString());
        Useful.XmlAddAttribute(node1, "city_type", ((object) this.m_city_type).ToString());
        Useful.XmlAddAttribute(node1, "alliance_type", ((object) this.m_alliance_type).ToString());
        Useful.XmlAddAttribute(node1, "cultural_sphere", ((object) this.m_cultural_sphere).ToString());
        Useful.XmlAddAttribute(node1, "has_name_image", this.m_has_name_image.ToString());
        if (this.m_alliance_type == GvoWorldInfo.AllianceType.Capital || this.m_alliance_type == GvoWorldInfo.AllianceType.Territory)
          Useful.XmlAddAttribute(node1, "default_country", ((object) this.GetDomain(GvoWorldInfo.Server.Euros)).ToString());
        Useful.XmlAddAttribute(node1, "lang1", this.m_lang1);
        Useful.XmlAddAttribute(node1, "lang2", this.m_lang2);
        Useful.XmlAddAttribute(node1, "bar_flags", this.m_sakaba_flag.ToString());
      }

      public void LoadDomainXml(XmlNode node)
      {
        if (node == null || node.Name != "domain_info" || (node.Attributes["name"] == null || node.Attributes["name"].Value != this.m_name) || (node.ChildNodes == null || this.m_alliance_type != GvoWorldInfo.AllianceType.Alliance))
          return;
        foreach (XmlNode node1 in node)
        {
          this.load_domain_sub(node1, GvoWorldInfo.Server.Euros);
          this.load_domain_sub(node1, GvoWorldInfo.Server.Zephyros);
          this.load_domain_sub(node1, GvoWorldInfo.Server.Notos);
          this.load_domain_sub(node1, GvoWorldInfo.Server.Boreas);
        }
      }

      private void load_domain_sub(XmlNode node, GvoWorldInfo.Server server)
      {
        if (node.Name != "server" || node.Attributes["name"] == null || node.Attributes["name"].Value != ((object) server).ToString())
          return;
        this.m_domains[(int) server] = GvoWorldInfo.GetCountryFromString(Useful.XmlGetAttribute(node, "country", ((object) GvoWorldInfo.Country.Unknown).ToString()));
      }

      public void WriteDomainXml(XmlNode node)
      {
        if (node == null || this.m_alliance_type != GvoWorldInfo.AllianceType.Alliance)
          return;
        XmlNode p_node = Useful.XmlAddNode(node, "domain_info", this.m_name);
        Useful.XmlAddAttribute(Useful.XmlAddNode(p_node, "server", ((object) GvoWorldInfo.Server.Euros).ToString()), "country", ((object) this.m_domains[0]).ToString());
        Useful.XmlAddAttribute(Useful.XmlAddNode(p_node, "server", ((object) GvoWorldInfo.Server.Zephyros).ToString()), "country", ((object) this.m_domains[1]).ToString());
        Useful.XmlAddAttribute(Useful.XmlAddNode(p_node, "server", ((object) GvoWorldInfo.Server.Notos).ToString()), "country", ((object) this.m_domains[2]).ToString());
        Useful.XmlAddAttribute(Useful.XmlAddNode(p_node, "server", ((object) GvoWorldInfo.Server.Boreas).ToString()), "country", ((object) this.m_domains[3]).ToString());
      }

      public bool LoadDomainFromNeworkData(string[] domains)
      {
        if (this.m_alliance_type != GvoWorldInfo.AllianceType.Alliance || domains == null || (domains.Length != 4 || this.Index < 0) || (domains[0].Length < this.Index || domains[1].Length < this.Index || (domains[2].Length < this.Index || domains[3].Length < this.Index)))
          return false;
        this.m_domains[0] = GvoWorldInfo.GetCountryFromString(domains[0][this.Index].ToString());
        this.m_domains[1] = GvoWorldInfo.GetCountryFromString(domains[1][this.Index].ToString());
        this.m_domains[2] = GvoWorldInfo.GetCountryFromString(domains[2][this.Index].ToString());
        this.m_domains[3] = GvoWorldInfo.GetCountryFromString(domains[3][this.Index].ToString());
        return true;
      }
    }
  }
}
