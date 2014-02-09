// Type: gvtrademap_cs.GvoDatabase
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using gvo_base;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;

namespace gvtrademap_cs
{
  public class GvoDatabase : IDisposable
  {
    private GvoWorldInfo m_world_info;
    private gvo_capture m_capture;
    private SeaRoutes m_searoute;
    private speed_calculator m_speed;
    private ShareRoutes m_share_routes;
    private WebIcons m_web_icons;
    private gvo_chat m_gvochat;
    private interest_days m_interest_days;
    private gvo_build_ship_counter m_build_ship_counter;
    private map_mark m_map_mark;
    private ItemDatabaseCustom m_item_database;
    private ShipPartsDataBase m_ship_parts_database;
    private sea_area m_sea_area;
    private gvo_season m_season;
    private gvt_lib m_lib;

    public GvoWorldInfo World
    {
      get
      {
        return this.m_world_info;
      }
    }

    public gvo_capture Capture
    {
      get
      {
        return this.m_capture;
      }
    }

    public SeaRoutes SeaRoute
    {
      get
      {
        return this.m_searoute;
      }
    }

    public speed_calculator SpeedCalculator
    {
      get
      {
        return this.m_speed;
      }
    }

    public ShareRoutes ShareRoutes
    {
      get
      {
        return this.m_share_routes;
      }
    }

    public WebIcons WebIcons
    {
      get
      {
        return this.m_web_icons;
      }
    }

    public gvo_chat GvoChat
    {
      get
      {
        return this.m_gvochat;
      }
    }

    public interest_days InterestDays
    {
      get
      {
        return this.m_interest_days;
      }
    }

    public gvo_build_ship_counter BuildShipCounter
    {
      get
      {
        return this.m_build_ship_counter;
      }
    }

    public map_mark MapMark
    {
      get
      {
        return this.m_map_mark;
      }
    }

    public ItemDatabaseCustom ItemDatabase
    {
      get
      {
        return this.m_item_database;
      }
    }

    public sea_area SeaArea
    {
      get
      {
        return this.m_sea_area;
      }
    }

    public gvo_season GvoSeason
    {
      get
      {
        return this.m_season;
      }
    }

    public GvoDatabase(gvt_lib lib)
    {
      this.m_lib = lib;
      this.m_season = new gvo_season();
      this.m_world_info = new GvoWorldInfo(lib, this.m_season, "database\\worldinfos.xml", "memo\\");
      this.m_item_database = new ItemDatabaseCustom("database\\item_db.txt");
      this.m_ship_parts_database = new ShipPartsDataBase("database\\ShipParts.xml");
      this.m_item_database.MergeShipPartsDatabase(this.m_ship_parts_database);
      this.m_speed = new speed_calculator(16384);
      this.m_searoute = new SeaRoutes(lib, "temp\\searoute.txt", "temp\\favorite_searoute.txt", "temp\\trash_searoute.txt");
      this.m_web_icons = new WebIcons(lib);
      this.m_map_mark = new map_mark(lib, "map\\mapmark.txt");
      this.m_share_routes = new ShareRoutes(lib);
      this.m_capture = new gvo_capture(lib);
      this.m_interest_days = new interest_days(lib.setting);
      this.m_build_ship_counter = new gvo_build_ship_counter(lib.setting);
      this.m_sea_area = new sea_area(lib, "temp\\seaarea.txt");
      this.m_gvochat = new gvo_chat(this.m_sea_area);
      this.m_gvochat.AnalyzeNewestChatLog();
      this.m_gvochat.ResetAll();
    }

    public void Dispose()
    {
      if (this.m_world_info != null)
        this.m_world_info.Dispose();
      if (this.m_sea_area != null)
        this.m_sea_area.Dispose();
      if (this.m_capture != null)
        this.m_capture.Dispose();
      this.m_world_info = (GvoWorldInfo) null;
      this.m_sea_area = (sea_area) null;
      this.m_capture = (gvo_capture) null;
    }

    public void WriteSettings()
    {
      this.m_searoute.Write("temp\\searoute.txt");
      this.m_searoute.WriteFavorite("temp\\favorite_searoute.txt");
      this.m_searoute.WriteTrash("temp\\trash_searoute.txt");
      this.m_world_info.WriteDomains("temp\\domaininfo.xml");
      this.m_world_info.WriteMemo("memo\\");
      this.m_map_mark.Write("map\\mapmark.txt");
      this.m_sea_area.WriteSetting("temp\\seaarea.txt");
    }

    public void Draw()
    {
      this.DrawForScreenShot();
      this.m_share_routes.Draw();
    }

    public void DrawForScreenShot()
    {
      this.m_web_icons.Draw();
      this.m_searoute.DrawRoutesLines();
      if (!this.m_lib.setting.is_mixed_info_names)
      {
        this.m_world_info.DrawSeaName();
        this.m_world_info.DrawCityName();
      }
      this.m_searoute.DrawPopups();
      this.m_map_mark.Draw();
    }

    public void DrawForMargeInfoNames(Vector2 draw_offset, LoopXImage image)
    {
      this.m_sea_area.Draw();
      if (!this.m_lib.setting.is_mixed_info_names)
        return;
      this.m_world_info.DrawSeaName();
      this.m_world_info.DrawCityName();
    }

    public List<GvoDatabase.Find> FindAll(string find_string)
    {
      List<GvoDatabase.Find> list = new List<GvoDatabase.Find>();
      GvoDatabase.Find.FindHandler handler;
      switch (this.m_lib.setting.find_filter3)
      {
        case _find_filter3.full_match:
          handler = new GvoDatabase.Find.FindHandler(GvoDatabase.Find.FindHander2);
          break;
        case _find_filter3.prefix_search:
          handler = new GvoDatabase.Find.FindHandler(GvoDatabase.Find.FindHander3);
          break;
        case _find_filter3.suffix_search:
          handler = new GvoDatabase.Find.FindHandler(GvoDatabase.Find.FindHander4);
          break;
        default:
          handler = new GvoDatabase.Find.FindHandler(GvoDatabase.Find.FindHander1);
          break;
      }
      if (this.m_lib.setting.find_filter2 == _find_filter2.name)
      {
        if (this.m_lib.setting.find_filter == _find_filter.both || this.m_lib.setting.find_filter == _find_filter.world_info)
          this.World.FindAll(find_string, list, handler);
        if (this.m_lib.setting.find_filter == _find_filter.both || this.m_lib.setting.find_filter == _find_filter.item_database)
          this.m_item_database.FindAll(find_string, list, handler);
      }
      else
      {
        if (this.m_lib.setting.find_filter == _find_filter.both || this.m_lib.setting.find_filter == _find_filter.world_info)
          this.World.FindAll_FromType(find_string, list, handler);
        if (this.m_lib.setting.find_filter == _find_filter.both || this.m_lib.setting.find_filter == _find_filter.item_database)
          this.m_item_database.FindAll_FromType(find_string, list, handler);
      }
      return list;
    }

    public List<GvoDatabase.Find> GetCulturalSphereList()
    {
      return new List<GvoDatabase.Find>((IEnumerable<GvoDatabase.Find>) this.World.CulturalSphereList());
    }

    public class Find
    {
      private GvoDatabase.Find.FindType m_type;
      private GvoWorldInfo.Info.Group.Data m_data;
      private ItemDatabase.Data m_database;
      private string m_info_name;
      private string m_lang;
      private GvoWorldInfo.CulturalSphere m_cultural_sphere;
      private string m_cultural_sphere_tool_tip;

      public GvoDatabase.Find.FindType Type
      {
        get
        {
          return this.m_type;
        }
      }

      public GvoWorldInfo.Info.Group.Data Data
      {
        get
        {
          return this.m_data;
        }
      }

      public ItemDatabase.Data Database
      {
        get
        {
          return this.m_database;
        }
      }

      public string InfoName
      {
        get
        {
          return this.m_info_name;
        }
      }

      public string Lang
      {
        get
        {
          return this.m_lang;
        }
      }

      public GvoWorldInfo.CulturalSphere CulturalSphere
      {
        get
        {
          return this.m_cultural_sphere;
        }
      }

      public string NameString
      {
        get
        {
          switch (this.Type)
          {
            case GvoDatabase.Find.FindType.Data:
              if (this.Data != null)
                return this.Data.Name + "[" + this.Data.Price + "]";
              else
                break;
            case GvoDatabase.Find.FindType.DataPrice:
              if (this.Data != null)
                return this.Data.Price + "[" + this.Data.Name + "]";
              else
                break;
            case GvoDatabase.Find.FindType.Database:
              if (this.Database != null)
                return this.Database.Name;
              else
                break;
            case GvoDatabase.Find.FindType.InfoName:
              if (this.InfoName != null)
                return this.InfoName;
              else
                break;
            case GvoDatabase.Find.FindType.Lang:
              if (this.Lang != null)
                return this.Lang;
              else
                break;
            case GvoDatabase.Find.FindType.CulturalSphere:
              return GvoWorldInfo.GetCulturalSphereString(this.CulturalSphere);
          }
          return "不明";
        }
      }

      public string TypeString
      {
        get
        {
          switch (this.Type)
          {
            case GvoDatabase.Find.FindType.Data:
              if (this.Data != null)
              {
                if (this.Data.ItemDb != null)
                  return this.Data.ItemDb.Type;
                else
                  return this.Data.GroupIndexString;
              }
              else
                break;
            case GvoDatabase.Find.FindType.DataPrice:
              if (this.Data != null)
              {
                if (this.Data.ItemDb != null)
                  return this.Data.ItemDb.Type + "[TAG]";
                else
                  return this.Data.GroupIndexString + "[TAG]";
              }
              else
                break;
            case GvoDatabase.Find.FindType.Database:
              if (this.Database != null)
                return this.Database.Type;
              else
                break;
            case GvoDatabase.Find.FindType.InfoName:
              return "街名等";
            case GvoDatabase.Find.FindType.Lang:
              return "使用言語";
            case GvoDatabase.Find.FindType.CulturalSphere:
              return "文化圏";
          }
          return "不明";
        }
      }

      public string SpotString
      {
        get
        {
          switch (this.Type)
          {
            case GvoDatabase.Find.FindType.Data:
            case GvoDatabase.Find.FindType.DataPrice:
            case GvoDatabase.Find.FindType.InfoName:
            case GvoDatabase.Find.FindType.Lang:
              if (this.InfoName != null)
                return this.InfoName;
              else
                break;
            case GvoDatabase.Find.FindType.Database:
              return "アイテムデ\x30FCタベ\x30FCス";
            case GvoDatabase.Find.FindType.CulturalSphere:
              return "";
          }
          return "不明";
        }
      }

      public string TooltipString
      {
        get
        {
          switch (this.Type)
          {
            case GvoDatabase.Find.FindType.Data:
            case GvoDatabase.Find.FindType.DataPrice:
              if (this.Data != null)
                return this.Data.TooltipString;
              else
                break;
            case GvoDatabase.Find.FindType.Database:
              if (this.Database != null)
                return this.Database.GetToolTipString();
              else
                break;
            case GvoDatabase.Find.FindType.CulturalSphere:
              return this.m_cultural_sphere_tool_tip;
          }
          return "";
        }
      }

      private Find()
      {
        this.m_type = GvoDatabase.Find.FindType.InfoName;
        this.m_data = (GvoWorldInfo.Info.Group.Data) null;
        this.m_database = (ItemDatabase.Data) null;
        this.m_info_name = "";
        this.m_lang = (string) null;
        this.m_cultural_sphere = GvoWorldInfo.CulturalSphere.Unknown;
        this.m_cultural_sphere_tool_tip = "";
      }

      public Find(string _info_name)
        : this()
      {
        this.m_type = GvoDatabase.Find.FindType.InfoName;
        this.m_info_name = _info_name;
      }

      public Find(GvoDatabase.Find.FindType _type, string _info_name, GvoWorldInfo.Info.Group.Data _data)
        : this()
      {
        this.m_type = _type;
        this.m_data = _data;
        if (this.m_data != null)
          this.m_database = this.m_data.ItemDb;
        this.m_info_name = _info_name;
      }

      public Find(string _info_name, string _lang)
        : this()
      {
        this.m_type = GvoDatabase.Find.FindType.Lang;
        this.m_info_name = _info_name;
        this.m_lang = _lang;
      }

      public Find(ItemDatabase.Data _database)
      {
        this.m_type = GvoDatabase.Find.FindType.Database;
        this.m_database = _database;
      }

      public Find(GvoWorldInfo.CulturalSphere cs, string tooltip_str)
        : this()
      {
        this.m_type = GvoDatabase.Find.FindType.CulturalSphere;
        this.m_info_name = GvoWorldInfo.GetCulturalSphereString(cs);
        this.m_cultural_sphere = cs;
        this.m_cultural_sphere_tool_tip = tooltip_str;
      }

      public static bool FindHander1(string str1, string str2)
      {
        return str1.IndexOf(str2) >= 0;
      }

      public static bool FindHander2(string str1, string str2)
      {
        return str1 == str2;
      }

      public static bool FindHander3(string str1, string str2)
      {
        return str1.StartsWith(str2);
      }

      public static bool FindHander4(string str1, string str2)
      {
        return str1.EndsWith(str2);
      }

      public enum FindType
      {
        Data,
        DataPrice,
        Database,
        InfoName,
        Lang,
        CulturalSphere,
      }

      public delegate bool FindHandler(string str1, string str2);
    }
  }
}
