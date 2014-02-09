// Type: gvtrademap_cs.spot
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using System.Drawing;
using Utility;

namespace gvtrademap_cs
{
  public class spot
  {
    private d3d_device m_device;
    private GvoWorldInfo m_world;
    private icons m_icons;
    private LoopXImage m_loop_image;
    private List<GvoWorldInfo.Info> m_spots;
    private spot.type m_spot_type;
    private string m_find_string;
    private List<spot.spot_once> m_spot_list;

    public bool is_spot
    {
      get
      {
        return this.m_spot_type != spot.type.none;
      }
    }

    public spot.type sopt_type
    {
      get
      {
        return this.m_spot_type;
      }
    }

    public string spot_type_str
    {
      get
      {
        return spot.GetTypeString(this.m_spot_type);
      }
    }

    public string spot_type_column_str
    {
      get
      {
        return spot.GetExColumnString(this.m_spot_type);
      }
    }

    public List<spot.spot_once> list
    {
      get
      {
        return this.m_spot_list;
      }
    }

    public spot(gvt_lib lib, GvoWorldInfo world)
    {
      this.m_device = lib.device;
      this.m_loop_image = lib.loop_image;
      this.m_icons = lib.icons;
      this.m_world = world;
      this.m_spots = new List<GvoWorldInfo.Info>();
      this.m_spot_list = new List<spot.spot_once>();
      this.m_spot_type = spot.type.none;
    }

    public void SetSpot(spot.type _type, string find_str)
    {
      this.m_spots.Clear();
      this.m_spot_list.Clear();
      this.m_find_string = find_str;
      this.m_spot_type = _type;
      switch (_type)
      {
        case spot.type.country_flags:
          using (IEnumerator<hittest> enumerator = this.m_world.World.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GvoWorldInfo.Info info = (GvoWorldInfo.Info) enumerator.Current;
              if (info.InfoType == GvoWorldInfo.InfoType.City)
              {
                this.m_spots.Add(info);
                this.m_spot_list.Add(new spot.spot_once(info, info.CountryStr, info.AllianceTypeStr));
              }
            }
            break;
          }
        case spot.type.icons_0:
        case spot.type.icons_1:
        case spot.type.icons_2:
        case spot.type.icons_3:
          using (IEnumerator<hittest> enumerator = this.m_world.World.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GvoWorldInfo.Info info = (GvoWorldInfo.Info) enumerator.Current;
              if ((info.Sakaba & 1 << (int) (_type - 2 & (spot.type) 31)) != 0)
              {
                this.m_spots.Add(info);
                this.m_spot_list.Add(new spot.spot_once(info, "", ""));
              }
            }
            break;
          }
        case spot.type.has_item:
          using (IEnumerator<hittest> enumerator = this.m_world.World.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GvoWorldInfo.Info info = (GvoWorldInfo.Info) enumerator.Current;
              GvoWorldInfo.Info.Group.Data data = info.HasItem(find_str);
              if (data != null)
              {
                this.m_spots.Add(info);
                this.m_spot_list.Add(new spot.spot_once(info, find_str, data.Price));
              }
            }
            break;
          }
        case spot.type.language:
          using (IEnumerator<hittest> enumerator = this.m_world.World.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GvoWorldInfo.Info info = (GvoWorldInfo.Info) enumerator.Current;
              if (info.Lang1 == find_str || info.Lang2 == find_str)
              {
                this.m_spots.Add(info);
                this.m_spot_list.Add(new spot.spot_once(info, find_str, ""));
              }
              else if (info.LearnPerson(find_str) != null)
              {
                this.m_spots.Add(info);
                this.m_spot_list.Add(new spot.spot_once(info, find_str, info.LearnPerson(find_str)));
              }
            }
            break;
          }
        case spot.type.tab_0:
        case spot.type.tab_1:
        case spot.type.tab_2:
        case spot.type.tab_3:
        case spot.type.tab_4:
        case spot.type.tab_4_1:
        case spot.type.tab_5:
        case spot.type.tab_6:
        case spot.type.tab_7:
        case spot.type.tab_8:
        case spot.type.tab_9:
          int num = (int) (_type - 8);
          using (IEnumerator<hittest> enumerator = this.m_world.World.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GvoWorldInfo.Info info = (GvoWorldInfo.Info) enumerator.Current;
              if (info.GetCount((GvoWorldInfo.Info.GroupIndex) num) > 0)
              {
                this.m_spots.Add(info);
                this.m_spot_list.Add(new spot.spot_once(info, "", ""));
              }
            }
            break;
          }
        case spot.type.tab_10:
          using (IEnumerator<hittest> enumerator = this.m_world.World.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GvoWorldInfo.Info info = (GvoWorldInfo.Info) enumerator.Current;
              if (info.GetMemoLines() > 0)
              {
                this.m_spots.Add(info);
                this.m_spot_list.Add(new spot.spot_once(info, "", ""));
              }
            }
            break;
          }
        case spot.type.city_name:
          using (IEnumerator<hittest> enumerator = this.m_world.World.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GvoWorldInfo.Info info = (GvoWorldInfo.Info) enumerator.Current;
              if (info.Name == find_str)
              {
                this.m_spots.Add(info);
                this.m_spot_list.Add(new spot.spot_once(info, "", ""));
              }
            }
            break;
          }
        case spot.type.cultural_sphere:
          using (IEnumerator<hittest> enumerator = this.m_world.World.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GvoWorldInfo.Info info = (GvoWorldInfo.Info) enumerator.Current;
              if (info.CulturalSphereStr == find_str)
              {
                this.m_spots.Add(info);
                this.m_spot_list.Add(new spot.spot_once(info, "文化圏", find_str));
              }
            }
            break;
          }
      }
    }

    public void Draw()
    {
      if (this.m_spot_type == spot.type.none || this.m_spots.Count == 0)
        return;
      this.m_device.DrawFillRect(new Vector3(0.0f, 0.0f, 0.31f), new Vector2(this.m_device.client_size.X, this.m_device.client_size.Y), Color.FromArgb(160, 0, 0, 0).ToArgb());
      this.m_loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_proc), 64f);
    }

    private void draw_proc(Vector2 offset, LoopXImage image)
    {
      switch (this.m_spot_type)
      {
        case spot.type.country_flags:
          this.draw_spot_mycountry(offset);
          this.spot_flags(offset);
          break;
        case spot.type.icons_0:
        case spot.type.icons_1:
        case spot.type.icons_2:
        case spot.type.icons_3:
          this.draw_spot(offset);
          this.spot_cities(offset);
          this.spot_icons(offset);
          break;
        case spot.type.has_item:
          this.draw_spot(offset);
          this.spot_cities(offset);
          break;
        case spot.type.language:
          this.draw_spot_for_lang(offset);
          this.spot_cities(offset);
          this.spot_learn_lang(offset);
          break;
        case spot.type.tab_0:
        case spot.type.tab_1:
        case spot.type.tab_2:
        case spot.type.tab_3:
        case spot.type.tab_4:
        case spot.type.tab_4_1:
        case spot.type.tab_5:
        case spot.type.tab_6:
        case spot.type.tab_7:
        case spot.type.tab_8:
        case spot.type.tab_9:
        case spot.type.tab_10:
          this.draw_spot(offset);
          this.spot_tab(offset);
          break;
        case spot.type.city_name:
        case spot.type.cultural_sphere:
          this.draw_spot(offset);
          this.spot_cities(offset);
          break;
      }
    }

    private void spot_flags(Vector2 offset)
    {
      float num = this.m_loop_image.ImageScale;
      if ((double) num < 0.5)
        num = 0.5f;
      else if ((double) num > 1.0)
        num = 1f;
      this.m_device.sprites.BeginDrawSprites(this.m_icons.texture, offset, this.m_loop_image.ImageScale, new Vector2(num, num));
      foreach (GvoWorldInfo.Info info in this.m_spots)
        this.m_device.sprites.AddDrawSprites(new Vector3((float) info.position.X, (float) info.position.Y, 0.3f), this.m_icons.GetIcon((icons.icon_index) (91 + info.MyCountry)));
      this.m_device.sprites.EndDrawSprites();
    }

    private void draw_spot(Vector2 offset)
    {
      this.m_device.device.RenderState.ZBufferFunction = Compare.Less;
      this.m_device.device.RenderState.SourceBlend = Blend.SourceAlpha;
      this.m_device.device.RenderState.DestinationBlend = Blend.One;
      float num = this.m_loop_image.ImageScale;
      if ((double) num < 0.5)
        num = 0.5f;
      else if ((double) num > 1.0)
        num = 1f;
      int color = Color.FromArgb(100, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue).ToArgb();
      this.m_device.sprites.BeginDrawSprites(this.m_icons.texture, offset, this.m_loop_image.ImageScale, new Vector2(num, num));
      foreach (GvoWorldInfo.Info info in this.m_spots)
        this.m_device.sprites.AddDrawSprites(new Vector3((float) info.position.X, (float) info.position.Y, 0.3f), this.m_icons.GetIcon(icons.icon_index.spot_0), color);
      this.m_device.sprites.EndDrawSprites();
      this.m_device.device.RenderState.ZBufferFunction = Compare.LessEqual;
      this.m_device.device.RenderState.SourceBlend = Blend.SourceAlpha;
      this.m_device.device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
    }

    private void draw_spot_mycountry(Vector2 offset)
    {
      this.m_device.device.RenderState.ZBufferFunction = Compare.Less;
      this.m_device.device.RenderState.SourceBlend = Blend.SourceAlpha;
      this.m_device.device.RenderState.DestinationBlend = Blend.One;
      float num = this.m_loop_image.ImageScale;
      if ((double) num < 0.5)
        num = 0.5f;
      else if ((double) num > 1.0)
        num = 1f;
      int color = Color.FromArgb(100, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue).ToArgb();
      this.m_device.sprites.BeginDrawSprites(this.m_icons.texture, offset, this.m_loop_image.ImageScale, new Vector2(num, num));
      foreach (GvoWorldInfo.Info info in this.m_spots)
      {
        if (info.MyCountry == this.m_world.MyCountry)
          this.m_device.sprites.AddDrawSprites(new Vector3((float) info.position.X, (float) info.position.Y, 0.3f), this.m_icons.GetIcon(icons.icon_index.spot_0), color);
      }
      this.m_device.sprites.EndDrawSprites();
      this.m_device.device.RenderState.ZBufferFunction = Compare.LessEqual;
      this.m_device.device.RenderState.SourceBlend = Blend.SourceAlpha;
      this.m_device.device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
    }

    private void draw_spot_for_lang(Vector2 offset)
    {
      this.m_device.device.RenderState.ZBufferFunction = Compare.Less;
      this.m_device.device.RenderState.SourceBlend = Blend.SourceAlpha;
      this.m_device.device.RenderState.DestinationBlend = Blend.One;
      float num = this.m_loop_image.ImageScale;
      if ((double) num < 0.5)
        num = 0.5f;
      else if ((double) num > 1.0)
        num = 1f;
      int color = Color.FromArgb(100, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue).ToArgb();
      this.m_device.sprites.BeginDrawSprites(this.m_icons.texture, offset, this.m_loop_image.ImageScale, new Vector2(num, num));
      foreach (GvoWorldInfo.Info info in this.m_spots)
      {
        icons.icon_index index = icons.icon_index.spot_2;
        if (info.LearnPerson(this.m_find_string) != null)
          index = icons.icon_index.spot_0;
        this.m_device.sprites.AddDrawSprites(new Vector3((float) info.position.X, (float) info.position.Y, 0.3f), this.m_icons.GetIcon(index), color);
      }
      this.m_device.sprites.EndDrawSprites();
      this.m_device.device.RenderState.ZBufferFunction = Compare.LessEqual;
      this.m_device.device.RenderState.SourceBlend = Blend.SourceAlpha;
      this.m_device.device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
    }

    private void spot_learn_lang(Vector2 offset)
    {
      float num = this.m_loop_image.ImageScale * 1.5f;
      if ((double) num < 0.5)
        num = 0.5f;
      else if ((double) num > 1.5)
        num = 1.5f;
      this.m_device.sprites.BeginDrawSprites(this.m_icons.texture, offset, this.m_loop_image.ImageScale, new Vector2(num, num));
      foreach (GvoWorldInfo.Info info in this.m_spots)
      {
        if (info.LearnPerson(this.m_find_string) != null)
          this.m_device.sprites.AddDrawSprites(new Vector3((float) info.position.X, (float) info.position.Y, 0.3f), this.m_icons.GetIcon(icons.icon_index.spot_tab_3));
      }
      this.m_device.sprites.EndDrawSprites();
    }

    private void spot_icons(Vector2 offset)
    {
      float num = this.m_loop_image.ImageScale * 1.2f;
      if ((double) num < 0.5)
        num = 0.5f;
      else if ((double) num > 1.20000004768372)
        num = 1.2f;
      this.m_device.sprites.BeginDrawSprites(this.m_icons.texture, offset, this.m_loop_image.ImageScale, new Vector2(num, num));
      foreach (GvoWorldInfo.Info info in this.m_spots)
        this.m_device.sprites.AddDrawSprites(new Vector3((float) info.position.X, (float) info.position.Y, 0.3f), this.m_icons.GetIcon((icons.icon_index) (99 + (this.m_spot_type - 2))));
      this.m_device.sprites.EndDrawSprites();
    }

    private void spot_cities(Vector2 offset)
    {
      float num = this.m_loop_image.ImageScale * 1.2f;
      if ((double) num < 0.600000023841858)
        num = 0.6f;
      else if ((double) num > 1.20000004768372)
        num = 1.2f;
      this.m_device.sprites.BeginDrawSprites(this.m_icons.texture, offset, this.m_loop_image.ImageScale, new Vector2(num, num));
      foreach (GvoWorldInfo.Info info in this.m_spots)
      {
        icons.icon_index index;
        switch (info.InfoType)
        {
          case GvoWorldInfo.InfoType.City:
            index = (icons.icon_index) (162 + info.CityType);
            break;
          case GvoWorldInfo.InfoType.Shore:
            index = icons.icon_index.city_icon_4;
            break;
          case GvoWorldInfo.InfoType.Shore2:
          case GvoWorldInfo.InfoType.PF:
            index = icons.icon_index.city_icon_5;
            break;
          case GvoWorldInfo.InfoType.OutsideCity:
            index = icons.icon_index.city_icon_6;
            break;
          default:
            continue;
        }
        this.m_device.sprites.AddDrawSprites(new Vector3((float) info.position.X, (float) info.position.Y, 0.3f), this.m_icons.GetIcon(index));
      }
      this.m_device.sprites.EndDrawSprites();
    }

    private void spot_tab(Vector2 offset)
    {
      float num = this.m_loop_image.ImageScale;
      if ((double) num < 0.5)
        num = 0.5f;
      else if ((double) num > 1.0)
        num = 1f;
      this.m_device.sprites.BeginDrawSprites(this.m_icons.texture, offset, this.m_loop_image.ImageScale, new Vector2(num, num));
      foreach (GvoWorldInfo.Info info in this.m_spots)
        this.m_device.sprites.AddDrawSprites(new Vector3((float) info.position.X, (float) info.position.Y, 0.3f), this.m_icons.GetIcon((icons.icon_index) (149 + (this.m_spot_type - 8))));
      this.m_device.sprites.EndDrawSprites();
    }

    public string GetToolTipString(Point pos)
    {
      switch (this.m_spot_type)
      {
        case spot.type.has_item:
          using (List<GvoWorldInfo.Info>.Enumerator enumerator = this.m_spots.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GvoWorldInfo.Info current = enumerator.Current;
              if (current.HitTest(pos))
              {
                GvoWorldInfo.Info.Group.Data data = current.HasItem(this.m_find_string);
                if (data != null)
                  return string.Format("{0}[{1}]", (object) data.Name, (object) data.Price);
              }
            }
            break;
          }
        case spot.type.language:
          using (List<GvoWorldInfo.Info>.Enumerator enumerator = this.m_spots.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GvoWorldInfo.Info current = enumerator.Current;
              if (current.HitTest(pos))
                return current.LearnPerson(this.m_find_string);
            }
            break;
          }
        case spot.type.cultural_sphere:
          using (List<GvoWorldInfo.Info>.Enumerator enumerator = this.m_spots.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GvoWorldInfo.Info current = enumerator.Current;
              if (current.HitTest(pos))
                return current.CulturalSphereStr;
            }
            break;
          }
      }
      return (string) null;
    }

    public static string GetTypeString(spot.type _type)
    {
      switch (_type)
      {
        case spot.type.none:
          return "スポットなし";
        case spot.type.country_flags:
          return "国旗";
        case spot.type.icons_0:
          return "看板娘";
        case spot.type.icons_1:
          return "書庫";
        case spot.type.icons_2:
          return "翻訳家";
        case spot.type.icons_3:
          return "豪商";
        case spot.type.has_item:
          return "アイテム等";
        case spot.type.language:
          return "使用言語";
        case spot.type.tab_0:
          return "交易所";
        case spot.type.tab_1:
          return "道具屋";
        case spot.type.tab_2:
          return "工房";
        case spot.type.tab_3:
          return "人物";
        case spot.type.tab_4:
          return "造船所親方";
        case spot.type.tab_4_1:
          return "船大工";
        case spot.type.tab_5:
          return "武器職人";
        case spot.type.tab_6:
          return "製材職人";
        case spot.type.tab_7:
          return "製帆職人";
        case spot.type.tab_8:
          return "彫刻家";
        case spot.type.tab_9:
          return "行商人";
        case spot.type.tab_10:
          return "メモ";
        case spot.type.city_name:
          return "街名";
        case spot.type.cultural_sphere:
          return "文化圏";
        default:
          return "不明なタイプ";
      }
    }

    public static string GetExColumnString(spot.type _type)
    {
      switch (_type)
      {
        case spot.type.none:
          return "";
        case spot.type.country_flags:
          return "状態";
        case spot.type.icons_0:
          return "";
        case spot.type.icons_1:
          return "";
        case spot.type.icons_2:
          return "";
        case spot.type.icons_3:
          return "";
        case spot.type.has_item:
          return "価格等";
        case spot.type.language:
          return "言語習得";
        case spot.type.tab_0:
          return "";
        case spot.type.tab_1:
          return "";
        case spot.type.tab_2:
          return "";
        case spot.type.tab_3:
          return "";
        case spot.type.tab_4:
          return "";
        case spot.type.tab_4_1:
          return "";
        case spot.type.tab_5:
          return "";
        case spot.type.tab_6:
          return "";
        case spot.type.tab_7:
          return "";
        case spot.type.tab_8:
          return "";
        case spot.type.tab_9:
          return "";
        case spot.type.tab_10:
          return "";
        case spot.type.city_name:
          return "";
        case spot.type.cultural_sphere:
          return "";
        default:
          return "";
      }
    }

    public enum type
    {
      none,
      country_flags,
      icons_0,
      icons_1,
      icons_2,
      icons_3,
      has_item,
      language,
      tab_0,
      tab_1,
      tab_2,
      tab_3,
      tab_4,
      tab_4_1,
      tab_5,
      tab_6,
      tab_7,
      tab_8,
      tab_9,
      tab_10,
      city_name,
      cultural_sphere,
    }

    public class spot_once
    {
      private GvoWorldInfo.Info m_info;
      private string m_name;
      private string m_ex;

      public GvoWorldInfo.Info info
      {
        get
        {
          return this.m_info;
        }
      }

      public string name
      {
        get
        {
          return this.m_name;
        }
      }

      public string ex
      {
        get
        {
          return this.m_ex;
        }
      }

      public spot_once(GvoWorldInfo.Info info, string name, string ex)
      {
        this.m_info = info;
        this.m_name = name;
        this.m_ex = ex;
      }
    }
  }
}
