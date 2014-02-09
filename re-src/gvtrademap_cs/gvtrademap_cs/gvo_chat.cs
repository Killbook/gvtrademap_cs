// Type: gvtrademap_cs.gvo_chat
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using gvo_base;

namespace gvtrademap_cs
{
  public class gvo_chat : gvo_map_cs_chat_base
  {
    private sea_area m_sea_area;

    public gvo_chat(sea_area _sea_area)
    {
      this.m_sea_area = _sea_area;
    }

    public gvo_chat(string path, sea_area _sea_area)
      : base(path)
    {
      this.m_sea_area = _sea_area;
    }

    public void UpdateSeaArea_DoRequest()
    {
      if (!this.IsRequest())
        return;
      this.update_sea_area();
    }

    public static int ToIndex(gvo_map_cs_chat_base.accident a)
    {
      switch (a)
      {
        case gvo_map_cs_chat_base.accident.shark1:
        case gvo_map_cs_chat_base.accident.shark2:
          return 101;
        case gvo_map_cs_chat_base.accident.fire:
          return 102;
        case gvo_map_cs_chat_base.accident.seaweed:
          return 103;
        case gvo_map_cs_chat_base.accident.seiren:
          return 104;
        case gvo_map_cs_chat_base.accident.compass:
          return 105;
        case gvo_map_cs_chat_base.accident.storm:
          return 106;
        case gvo_map_cs_chat_base.accident.blizzard:
          return 107;
        case gvo_map_cs_chat_base.accident.mouse:
          return 108;
        case gvo_map_cs_chat_base.accident.UMA:
          return 109;
        case gvo_map_cs_chat_base.accident.treasure1:
        case gvo_map_cs_chat_base.accident.treasure2:
        case gvo_map_cs_chat_base.accident.treasure3:
          return 111;
        case gvo_map_cs_chat_base.accident.escape_battle:
        case gvo_map_cs_chat_base.accident.win_battle:
        case gvo_map_cs_chat_base.accident.lose_battle:
          return 110;
        default:
          return -1;
      }
    }

    private void update_sea_area()
    {
      gvo_map_cs_chat_base.sea_area_type[] seaAreaTypeList = this.sea_area_type_list;
      if (seaAreaTypeList != null)
      {
        foreach (gvo_map_cs_chat_base.sea_area_type seaAreaType in seaAreaTypeList)
        {
          switch (seaAreaType.type)
          {
            case gvo_map_cs_chat_base.sea_type.normal:
              this.m_sea_area.SetType(seaAreaType.name, sea_area.sea_area_once.sea_type.normal);
              break;
            case gvo_map_cs_chat_base.sea_type.safty:
              this.m_sea_area.SetType(seaAreaType.name, sea_area.sea_area_once.sea_type.safty);
              break;
            case gvo_map_cs_chat_base.sea_type.lawless:
              this.m_sea_area.SetType(seaAreaType.name, sea_area.sea_area_once.sea_type.lawless);
              break;
          }
        }
      }
      this.ResetSeaArea();
    }
  }
}
