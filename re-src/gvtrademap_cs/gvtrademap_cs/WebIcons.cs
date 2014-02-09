// Type: gvtrademap_cs.WebIcons
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace gvtrademap_cs
{
  public class WebIcons
  {
    private const int BB_ONCE_SIZE = 400;
    private const int BB_OUTSIDESCREEEN_OFFSET = 16;
    private const float REMOVE_LENGTH_SQ = 1156f;
    private gvt_lib m_lib;
    private List<WebIcons.Data> m_list;
    private List<WebIcons.DataListBB> m_draw_list;
    private bool m_optimize;
    private DrawSettingWebIcons m_draw_flags;

    public WebIcons(gvt_lib lib)
    {
      this.m_lib = lib;
      this.m_list = new List<WebIcons.Data>();
      this.m_draw_list = new List<WebIcons.DataListBB>();
      this.m_draw_flags = (DrawSettingWebIcons) 0;
    }

    public bool Load(string fname)
    {
      if (!File.Exists(fname))
        return false;
      try
      {
        using (StreamReader streamReader = new StreamReader(fname, Encoding.GetEncoding("Shift_JIS")))
        {
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            if (!(str == ""))
            {
              string[] strArray = str.Split(new char[1]
              {
                ','
              });
              if (strArray.Length >= 3)
              {
                try
                {
                  int x = Convert.ToInt32(strArray[0]);
                  int y = Convert.ToInt32(strArray[1]);
                  int icon_index = Convert.ToInt32(strArray[2]);
                  string memo = "";
                  if (strArray.Length < 4)
                    memo = strArray[3];
                  this.m_list.Add(new WebIcons.Data(new Point(x, y), icon_index, memo));
                }
                catch
                {
                }
              }
            }
          }
        }
      }
      catch
      {
        return false;
      }
      this.create_draw_list();
      return true;
    }

    public void Update()
    {
      if (this.m_optimize == this.m_lib.setting.remove_near_web_icons && this.m_draw_flags == this.m_lib.setting.draw_setting_web_icons)
        return;
      this.create_draw_list();
    }

    private void create_draw_list()
    {
      this.m_draw_list.Clear();
      this.m_draw_flags = this.m_lib.setting.draw_setting_web_icons;
      List<WebIcons.Data> list = this.create_list();
      while (list.Count > 0)
        this.m_draw_list.Add(this.create_bb(ref list));
    }

    private List<WebIcons.Data> create_list()
    {
      List<WebIcons.Data> list = new List<WebIcons.Data>();
      this.m_optimize = this.m_lib.setting.remove_near_web_icons;
      if (!this.m_optimize)
      {
        foreach (WebIcons.Data data in this.m_list)
          list.Add(data);
      }
      else
      {
        foreach (WebIcons.Data wi in this.m_list)
        {
          if (this.is_add_list(wi, list))
            list.Add(wi);
        }
      }
      return list;
    }

    private bool is_add_list(WebIcons.Data wi, List<WebIcons.Data> list)
    {
      foreach (WebIcons.Data data in list)
      {
        if (wi.IconIndex == data.IconIndex && (double) (transform.ToVector2(wi.Position) - transform.ToVector2(data.Position)).LengthSq() <= 1156.0)
          return false;
      }
      return true;
    }

    private WebIcons.DataListBB create_bb(ref List<WebIcons.Data> free_list)
    {
      List<WebIcons.Data> list = free_list;
      free_list = new List<WebIcons.Data>();
      WebIcons.DataListBB dataListBb = new WebIcons.DataListBB();
      foreach (WebIcons.Data p in list)
      {
        if (this.is_draw(p.IconIndex) && !dataListBb.Add(p))
          free_list.Add(p);
      }
      return dataListBb;
    }

    public void Draw()
    {
      if (!this.m_lib.setting.draw_web_icons)
        return;
      this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_proc), 32f);
    }

    private void draw_proc(Vector2 offset, LoopXImage image)
    {
      float num1 = image.ImageScale;
      if ((double) num1 < 0.5)
        num1 = 0.5f;
      else if ((double) num1 > 1.0)
        num1 = 1f;
      Vector2 global_scale = new Vector2(num1, num1);
      D3dBB2d.CullingRect rect = new D3dBB2d.CullingRect(image.Device.client_size);
      this.m_lib.device.sprites.BeginDrawSprites(this.m_lib.icons.texture, offset, image.ImageScale, global_scale);
      foreach (WebIcons.DataListBB dataListBb in this.m_draw_list)
      {
        if (!dataListBb.IsCulling(offset, image.ImageScale, rect))
        {
          foreach (WebIcons.Data data in dataListBb.List)
          {
            int num2 = data.IconIndex;
            if (num2 < 0)
              num2 = 0;
            if (num2 > 12)
              num2 = 12;
            this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3((float) data.Position.X, (float) data.Position.Y, 0.8f), this.m_lib.icons.GetIcon((icons.icon_index) (123 + num2)));
          }
        }
      }
      this.m_lib.device.sprites.EndDrawSprites();
    }

    private bool is_draw(int index)
    {
      DrawSettingWebIcons drawSettingWebIcons = this.m_lib.setting.draw_setting_web_icons;
      switch (index)
      {
        case 0:
        case 1:
        case 2:
        case 3:
        case 4:
        case 5:
        case 6:
        case 7:
          if ((drawSettingWebIcons & DrawSettingWebIcons.wind) == (DrawSettingWebIcons) 0)
            return false;
          else
            break;
        case 8:
          if ((drawSettingWebIcons & DrawSettingWebIcons.accident_0) == (DrawSettingWebIcons) 0)
            return false;
          else
            break;
        case 9:
          if ((drawSettingWebIcons & DrawSettingWebIcons.accident_1) == (DrawSettingWebIcons) 0)
            return false;
          else
            break;
        case 10:
          if ((drawSettingWebIcons & DrawSettingWebIcons.accident_2) == (DrawSettingWebIcons) 0)
            return false;
          else
            break;
        case 11:
          if ((drawSettingWebIcons & DrawSettingWebIcons.accident_3) == (DrawSettingWebIcons) 0)
            return false;
          else
            break;
        case 12:
          if ((drawSettingWebIcons & DrawSettingWebIcons.accident_4) == (DrawSettingWebIcons) 0)
            return false;
          else
            break;
      }
      return true;
    }

    private enum icon_index
    {
      wind_0,
      wind_1,
      wind_2,
      wind_3,
      wind_4,
      wind_5,
      wind_6,
      wind_7,
      memo_0,
      memo_1,
      memo_2,
      memo_3,
      memo_4,
    }

    public class Data
    {
      private Point m_pos;
      private int m_icon_index;
      private string m_memo;

      public Point Position
      {
        get
        {
          return this.m_pos;
        }
      }

      public int IconIndex
      {
        get
        {
          return this.m_icon_index;
        }
      }

      public string Memo
      {
        get
        {
          return this.m_memo;
        }
      }

      public Data(Point pos, int icon_index, string memo)
      {
        this.m_pos = pos;
        this.m_icon_index = icon_index;
        this.m_memo = memo;
      }
    }

    public class DataListBB : D3dBB2d
    {
      private List<WebIcons.Data> m_list;

      public List<WebIcons.Data> List
      {
        get
        {
          return this.m_list;
        }
      }

      public DataListBB()
      {
        this.m_list = new List<WebIcons.Data>();
        this.OffsetLT = new Vector2(-16f, -16f);
        this.OffsetRB = new Vector2(16f, 16f);
      }

      public bool Add(WebIcons.Data p)
      {
        Vector2 size = this.IfUpdate(transform.ToVector2(p.Position)).Size;
        if ((double) size.X > 400.0 || (double) size.Y > 400.0)
          return false;
        this.m_list.Add(p);
        this.Update(transform.ToVector2(p.Position));
        return true;
      }
    }
  }
}
