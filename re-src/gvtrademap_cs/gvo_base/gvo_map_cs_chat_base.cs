// Type: gvo_base.gvo_map_cs_chat_base
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Collections.Generic;

namespace gvo_base
{
  public class gvo_map_cs_chat_base : gvo_chat_base
  {
    private readonly object m_syncobject = new object();
    private gvo_map_cs_chat_base.accident m_accident;
    private bool m_is_interest;
    private List<gvo_map_cs_chat_base.sea_area_type> m_sea_area_type_list;
    private bool m_is_start_build_ship;
    private string m_build_ship_name;
    private bool m_is_finish_build_ship;

    public gvo_map_cs_chat_base.accident _accident
    {
      get
      {
        lock (this.m_syncobject)
          return this.m_accident;
      }
      private set
      {
        lock (this.m_syncobject)
          this.m_accident = value;
      }
    }

    public bool is_interest
    {
      get
      {
        lock (this.m_syncobject)
          return this.m_is_interest;
      }
      private set
      {
        lock (this.m_syncobject)
          this.m_is_interest = value;
      }
    }

    public gvo_map_cs_chat_base.sea_area_type[] sea_area_type_list
    {
      get
      {
        lock (this.m_syncobject)
          return this.m_sea_area_type_list.ToArray();
      }
    }

    public bool is_start_build_ship
    {
      get
      {
        lock (this.m_syncobject)
          return this.m_is_start_build_ship;
      }
      set
      {
        lock (this.m_syncobject)
          this.m_is_start_build_ship = value;
      }
    }

    public string build_ship_name
    {
      get
      {
        lock (this.m_syncobject)
          return this.m_build_ship_name;
      }
      set
      {
        lock (this.m_syncobject)
          this.m_build_ship_name = value;
      }
    }

    public bool is_finish_build_ship
    {
      get
      {
        lock (this.m_syncobject)
          return this.m_is_finish_build_ship;
      }
      set
      {
        lock (this.m_syncobject)
          this.m_is_finish_build_ship = value;
      }
    }

    public gvo_map_cs_chat_base()
    {
      this.init();
    }

    public gvo_map_cs_chat_base(string path)
      : base(path)
    {
      this.init();
    }

    private void init()
    {
      this.m_accident = gvo_map_cs_chat_base.accident.none;
      this.m_is_interest = false;
      this.m_sea_area_type_list = new List<gvo_map_cs_chat_base.sea_area_type>();
      this.init_analyze_list();
      this.ResetAll();
    }

    private void init_analyze_list()
    {
      this.ResetAnalizedList();
      this.AddAnalizeList("船員がサメに襲われています", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.shark1);
      this.AddAnalizeList("人喰いザメが現れました！", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.shark2);
      this.AddAnalizeList("火災が発生しました！", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.fire);
      this.AddAnalizeList("藻が舵に絡まっています！", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.seaweed);
      this.AddAnalizeList("気味の悪い歌声が聞こえてきました", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.seiren);
      this.AddAnalizeList("磁場が狂っています。羅針盤が使いものになりません", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.compass);
      this.AddAnalizeList("嵐が来ました！　帆を広げていると転覆してしまいます！", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.storm);
      this.AddAnalizeList("吹雪になりました！　帆を広げていると凍りついてしまいます", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.blizzard);
      this.AddAnalizeList("ネズミが大量発生しました！", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.mouse);
      this.AddAnalizeList("得体の知れない怪物が現れました", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.UMA);
      this.AddAnalizeList("このあたりで何かいい物が見つかるかもしれません", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.treasure1);
      this.AddAnalizeList("このあたりで何か見つかるかもしれません", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.treasure2);
      this.AddAnalizeList("このあたりで高価なものが見つかるかもしれません", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.treasure3);
      this.AddAnalizeList("全船が戦場を離れました", gvo_chat_base.type.index0, (object) gvo_map_cs_chat_base.accident.escape_battle);
      this.AddAnalizeList("に勝利しました！", gvo_chat_base.type.any_index, (object) gvo_map_cs_chat_base.accident.win_battle);
      this.AddAnalizeList("に敗北しました…", gvo_chat_base.type.any_index, (object) gvo_map_cs_chat_base.accident.lose_battle);
      this.AddAnalizeList_Interest((object) gvo_map_cs_chat_base.accident.interest);
      this.AddAnalizeList("^(.+)が危険海域に戻りました！", gvo_chat_base.type.regex, (object) gvo_map_cs_chat_base.accident.sea_type_normal);
      this.AddAnalizeList("^(.+)が安全海域となりました！", gvo_chat_base.type.regex, (object) gvo_map_cs_chat_base.accident.sea_type_safty);
      this.AddAnalizeList("^(.+)が無法海域となりました！", gvo_chat_base.type.regex, (object) gvo_map_cs_chat_base.accident.sea_type_lawless);
      this.AddAnalizeList("^(.+)の建造を注文しました", gvo_chat_base.type.regex, (object) gvo_map_cs_chat_base.accident.buildship_start);
      this.AddAnalizeList("^(.+)の強化を依頼しました", gvo_chat_base.type.regex, (object) gvo_map_cs_chat_base.accident.buildship_start);
      this.AddAnalizeList("」が無事進水しました", gvo_chat_base.type.any_index, (object) gvo_map_cs_chat_base.accident.buildship_finish);
    }

    public override bool AnalyzeNewestChatLog()
    {
      this._accident = gvo_map_cs_chat_base.accident.none;
      if (!base.AnalyzeNewestChatLog())
        return false;
      if (!this.is_update)
        return true;
      this.update_analyze();
      return true;
    }

    private void update_analyze()
    {
      foreach (gvo_chat_base.analized_data analizedData in this.analized_list)
      {
        switch ((gvo_map_cs_chat_base.accident) analizedData.tag)
        {
          case gvo_map_cs_chat_base.accident.shark1:
          case gvo_map_cs_chat_base.accident.shark2:
          case gvo_map_cs_chat_base.accident.fire:
          case gvo_map_cs_chat_base.accident.seaweed:
          case gvo_map_cs_chat_base.accident.seiren:
          case gvo_map_cs_chat_base.accident.compass:
          case gvo_map_cs_chat_base.accident.storm:
          case gvo_map_cs_chat_base.accident.blizzard:
          case gvo_map_cs_chat_base.accident.mouse:
          case gvo_map_cs_chat_base.accident.UMA:
          case gvo_map_cs_chat_base.accident.treasure1:
          case gvo_map_cs_chat_base.accident.treasure2:
          case gvo_map_cs_chat_base.accident.treasure3:
          case gvo_map_cs_chat_base.accident.escape_battle:
          case gvo_map_cs_chat_base.accident.win_battle:
          case gvo_map_cs_chat_base.accident.lose_battle:
            this._accident = (gvo_map_cs_chat_base.accident) analizedData.tag;
            continue;
          case gvo_map_cs_chat_base.accident.interest:
            this.m_is_interest = true;
            continue;
          case gvo_map_cs_chat_base.accident.sea_type_normal:
            lock (this.m_syncobject)
            {
              this.m_sea_area_type_list.Add(new gvo_map_cs_chat_base.sea_area_type(analizedData.match.Groups[1].Value, gvo_map_cs_chat_base.sea_type.normal));
              continue;
            }
          case gvo_map_cs_chat_base.accident.sea_type_safty:
            lock (this.m_syncobject)
            {
              this.m_sea_area_type_list.Add(new gvo_map_cs_chat_base.sea_area_type(analizedData.match.Groups[1].Value, gvo_map_cs_chat_base.sea_type.safty));
              continue;
            }
          case gvo_map_cs_chat_base.accident.sea_type_lawless:
            lock (this.m_syncobject)
            {
              this.m_sea_area_type_list.Add(new gvo_map_cs_chat_base.sea_area_type(analizedData.match.Groups[1].Value, gvo_map_cs_chat_base.sea_type.lawless));
              continue;
            }
          case gvo_map_cs_chat_base.accident.buildship_start:
            this.is_start_build_ship = true;
            this.build_ship_name = analizedData.match.Groups[1].Value;
            continue;
          case gvo_map_cs_chat_base.accident.buildship_finish:
            this.is_finish_build_ship = true;
            continue;
          default:
            continue;
        }
      }
    }

    public void ResetAll()
    {
      this.ResetAccident();
      this.ResetInterest();
      this.ResetSeaArea();
      this.ResetBuildShip();
    }

    public void ResetAccident()
    {
      this._accident = gvo_map_cs_chat_base.accident.none;
    }

    public void ResetInterest()
    {
      this.is_interest = false;
    }

    public void ResetSeaArea()
    {
      lock (this.m_syncobject)
        this.m_sea_area_type_list.Clear();
    }

    public void ResetBuildShip()
    {
      this.m_is_start_build_ship = false;
      this.m_build_ship_name = "";
      this.m_is_finish_build_ship = false;
    }

    public static string ToSeaTypeString(gvo_map_cs_chat_base.sea_type type)
    {
      switch (type)
      {
        case gvo_map_cs_chat_base.sea_type.safty:
          return "安全";
        case gvo_map_cs_chat_base.sea_type.lawless:
          return "無法";
        default:
          return "通常";
      }
    }

    public static gvo_map_cs_chat_base.sea_type ToSeaType(string type)
    {
      switch (type)
      {
        case "安全":
          return gvo_map_cs_chat_base.sea_type.safty;
        case "無法":
          return gvo_map_cs_chat_base.sea_type.lawless;
        default:
          return gvo_map_cs_chat_base.sea_type.normal;
      }
    }

    public static string ToAccidentString(gvo_map_cs_chat_base.accident __accident)
    {
      switch (__accident)
      {
        case gvo_map_cs_chat_base.accident.shark1:
          return "サメ1";
        case gvo_map_cs_chat_base.accident.shark2:
          return "サメ2";
        case gvo_map_cs_chat_base.accident.fire:
          return "火災";
        case gvo_map_cs_chat_base.accident.seaweed:
          return "藻";
        case gvo_map_cs_chat_base.accident.seiren:
          return "セイレ\x30FCン";
        case gvo_map_cs_chat_base.accident.compass:
          return "羅針盤";
        case gvo_map_cs_chat_base.accident.storm:
          return "嵐";
        case gvo_map_cs_chat_base.accident.blizzard:
          return "吹雪";
        case gvo_map_cs_chat_base.accident.mouse:
          return "ネズミ";
        case gvo_map_cs_chat_base.accident.UMA:
          return "怪物";
        case gvo_map_cs_chat_base.accident.treasure1:
          return "何かいい物";
        case gvo_map_cs_chat_base.accident.treasure2:
          return "何か見つかるかも";
        case gvo_map_cs_chat_base.accident.treasure3:
          return "高価なもの";
        case gvo_map_cs_chat_base.accident.escape_battle:
          return "海戦離脱";
        case gvo_map_cs_chat_base.accident.win_battle:
          return "海戦勝利";
        case gvo_map_cs_chat_base.accident.lose_battle:
          return "海戦敗北";
        default:
          return "なし";
      }
    }

    public static gvo_map_cs_chat_base.accident ToAccident(string str)
    {
      switch (str)
      {
        case "サメ1":
          return gvo_map_cs_chat_base.accident.shark1;
        case "サメ2":
          return gvo_map_cs_chat_base.accident.shark2;
        case "火災":
          return gvo_map_cs_chat_base.accident.fire;
        case "藻":
          return gvo_map_cs_chat_base.accident.seaweed;
        case "セイレ\x30FCン":
          return gvo_map_cs_chat_base.accident.seiren;
        case "羅針盤":
          return gvo_map_cs_chat_base.accident.compass;
        case "嵐":
          return gvo_map_cs_chat_base.accident.storm;
        case "吹雪":
          return gvo_map_cs_chat_base.accident.blizzard;
        case "ネズミ":
          return gvo_map_cs_chat_base.accident.mouse;
        case "怪物":
          return gvo_map_cs_chat_base.accident.UMA;
        case "何かいい物":
          return gvo_map_cs_chat_base.accident.treasure1;
        case "何か見つかるかも":
          return gvo_map_cs_chat_base.accident.treasure2;
        case "高価なもの":
          return gvo_map_cs_chat_base.accident.treasure3;
        case "海戦離脱":
          return gvo_map_cs_chat_base.accident.escape_battle;
        case "海戦勝利":
          return gvo_map_cs_chat_base.accident.win_battle;
        case "海戦敗北":
          return gvo_map_cs_chat_base.accident.lose_battle;
        default:
          return gvo_map_cs_chat_base.accident.none;
      }
    }

    public enum sea_type
    {
      normal,
      safty,
      lawless,
    }

    public class sea_area_type
    {
      private string m_name;
      private gvo_map_cs_chat_base.sea_type m_type;

      public string name
      {
        get
        {
          return this.m_name;
        }
      }

      public gvo_map_cs_chat_base.sea_type type
      {
        get
        {
          return this.m_type;
        }
      }

      public sea_area_type(string name, gvo_map_cs_chat_base.sea_type type)
      {
        this.m_name = name;
        this.m_type = type;
      }
    }

    public enum accident
    {
      none,
      shark1,
      shark2,
      fire,
      seaweed,
      seiren,
      compass,
      storm,
      blizzard,
      mouse,
      UMA,
      treasure1,
      treasure2,
      treasure3,
      escape_battle,
      win_battle,
      lose_battle,
      interest,
      sea_type_normal,
      sea_type_safty,
      sea_type_lawless,
      buildship_start,
      buildship_finish,
    }
  }
}
