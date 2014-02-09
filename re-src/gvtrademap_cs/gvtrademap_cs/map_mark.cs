// Type: gvtrademap_cs.map_mark
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using Utility;

namespace gvtrademap_cs
{
  public class map_mark
  {
    private gvt_lib m_lib;
    private hittest_list m_datas;

    public map_mark(gvt_lib lib, string fname)
    {
      this.m_lib = lib;
      this.m_datas = new hittest_list();
      this.load(fname);
    }

    public void Add(Point pos, map_mark.map_mark_type type, string memo)
    {
      this.m_datas.Add((hittest) new map_mark.data(this.m_lib, pos, type, memo));
    }

    private void load(string fname)
    {
      if (!File.Exists(fname))
        return;
      try
      {
        using (StreamReader streamReader = new StreamReader(fname, Encoding.GetEncoding("Shift_JIS")))
        {
          string line;
          while ((line = streamReader.ReadLine()) != null)
          {
            if (!(line == ""))
            {
              map_mark.data data = new map_mark.data(this.m_lib);
              if (data.Load(line))
                this.m_datas.Add((hittest) data);
            }
          }
        }
      }
      catch
      {
      }
    }

    public bool Write(string fname)
    {
      if (this.m_datas.Count <= 0)
      {
        file_ctrl.RemoveFile(fname);
        return true;
      }
      else
      {
        try
        {
          using (StreamWriter streamWriter = new StreamWriter(fname, false, Encoding.GetEncoding("Shift_JIS")))
          {
            foreach (map_mark.data data in this.m_datas)
            {
              string str = "" + (object) data.position.X + "\t" + (object) data.position.Y + "\t" + ((int) data.type).ToString() + "\t" + data.memo;
              streamWriter.WriteLine(str);
            }
          }
        }
        catch
        {
          return false;
        }
        return true;
      }
    }

    public void Draw()
    {
      if (!this.m_lib.setting.draw_icons)
        return;
      this.m_lib.device.sprites.BeginDrawSprites(this.m_lib.icons.texture);
      this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_proc), 32f);
      this.m_lib.device.sprites.EndDrawSprites();
    }

    private void draw_proc(Vector2 offset, LoopXImage image)
    {
      float num1 = image.ImageScale;
      if ((double) num1 < 0.5)
        num1 = 0.5f;
      else if ((double) num1 > 1.0)
        num1 = 1f;
      Vector2 vector2_1 = new Vector2(num1, num1);
      float num2 = num1 * 1.5f;
      Vector2 vector2_2 = new Vector2(num2, num2);
      foreach (map_mark.data data in this.m_datas)
      {
        int index = (int) data.type;
        if (index < 0)
          index = 0;
        if (index > 19)
          index = 19;
        if (this.is_draw(index))
        {
          Vector2 vector2_3 = image.GlobalPos2LocalPos(new Vector2((float) data.position.X, (float) data.position.Y), offset);
          this.m_lib.device.sprites.AddDrawSprites(new Vector3(vector2_3.X, vector2_3.Y, 0.5f), this.m_lib.icons.GetIcon((icons.icon_index) (103 + index)), index == 19 ? vector2_2 : vector2_1, Color.White.ToArgb());
        }
      }
    }

    private bool is_draw(int index)
    {
      DrawSettingMemoIcons settingMemoIcons = this.m_lib.setting.draw_setting_memo_icons;
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
          if ((settingMemoIcons & DrawSettingMemoIcons.wind) == (DrawSettingMemoIcons) 0)
            return false;
          else
            break;
        case 8:
          if ((settingMemoIcons & DrawSettingMemoIcons.memo_0) == (DrawSettingMemoIcons) 0)
            return false;
          else
            break;
        case 9:
          if ((settingMemoIcons & DrawSettingMemoIcons.memo_1) == (DrawSettingMemoIcons) 0)
            return false;
          else
            break;
        case 10:
          if ((settingMemoIcons & DrawSettingMemoIcons.memo_2) == (DrawSettingMemoIcons) 0)
            return false;
          else
            break;
        case 11:
          if ((settingMemoIcons & DrawSettingMemoIcons.memo_3) == (DrawSettingMemoIcons) 0)
            return false;
          else
            break;
        case 12:
          if ((settingMemoIcons & DrawSettingMemoIcons.memo_4) == (DrawSettingMemoIcons) 0)
            return false;
          else
            break;
        case 13:
          if ((settingMemoIcons & DrawSettingMemoIcons.memo_5) == (DrawSettingMemoIcons) 0)
            return false;
          else
            break;
        case 14:
          if ((settingMemoIcons & DrawSettingMemoIcons.memo_6) == (DrawSettingMemoIcons) 0)
            return false;
          else
            break;
        case 15:
          if ((settingMemoIcons & DrawSettingMemoIcons.memo_7) == (DrawSettingMemoIcons) 0)
            return false;
          else
            break;
        case 16:
          if ((settingMemoIcons & DrawSettingMemoIcons.memo_8) == (DrawSettingMemoIcons) 0)
            return false;
          else
            break;
        case 17:
          if ((settingMemoIcons & DrawSettingMemoIcons.memo_9) == (DrawSettingMemoIcons) 0)
            return false;
          else
            break;
        case 18:
          if ((settingMemoIcons & DrawSettingMemoIcons.memo_10) == (DrawSettingMemoIcons) 0)
            return false;
          else
            break;
        case 19:
          if ((settingMemoIcons & DrawSettingMemoIcons.memo_11) == (DrawSettingMemoIcons) 0)
            return false;
          else
            break;
      }
      return true;
    }

    public map_mark.data FindData(Point pos)
    {
      if (!this.m_lib.setting.draw_icons)
        return (map_mark.data) null;
      foreach (map_mark.data data in this.m_datas)
      {
        if (data.HitTest(pos))
          return data;
      }
      return (map_mark.data) null;
    }

    public string GetToolTip(Point pos)
    {
      map_mark.data data = this.FindData(pos);
      if (data == null)
        return (string) null;
      else
        return data.tooltipstr;
    }

    public void RemoveData(map_mark.data d)
    {
      if (d == null)
        return;
      try
      {
        this.m_datas.Remove((hittest) d);
      }
      catch
      {
      }
    }

    public void RemoveAllData()
    {
      this.m_datas.Clear();
    }

    public void RemoveAllTargetData()
    {
      while (true)
      {
        map_mark.data d = this.get_1st_target_memo();
        if (d != null)
          this.RemoveData(d);
        else
          break;
      }
    }

    private map_mark.data get_1st_target_memo()
    {
      foreach (map_mark.data data in this.m_datas)
      {
        if (data.type == map_mark.map_mark_type.icon11)
          return data;
      }
      return (map_mark.data) null;
    }

    public enum map_mark_type
    {
      axis0,
      axis1,
      axis2,
      axis3,
      axis4,
      axis5,
      axis6,
      axis7,
      icon0,
      icon1,
      icon2,
      icon3,
      icon4,
      icon5,
      icon6,
      icon7,
      icon8,
      icon9,
      icon10,
      icon11,
    }

    public class data : hittest
    {
      private gvt_lib m_lib;
      private string m_memo;
      private map_mark.map_mark_type m_type;

      public string memo
      {
        get
        {
          return this.m_memo;
        }
        set
        {
          this.m_memo = value;
        }
      }

      public map_mark.map_mark_type type
      {
        get
        {
          return this.m_type;
        }
        set
        {
          this.m_type = value;
        }
      }

      public Point gposition
      {
        get
        {
          return transform.map_pos2_game_pos(this.position, this.m_lib.loop_image);
        }
      }

      public string tooltipstr
      {
        get
        {
          Point gposition = this.gposition;
          return string.Format("{0}\n[{1},{2}]", (object) this.memo, (object) gposition.X, (object) gposition.Y);
        }
      }

      public data(gvt_lib lib)
      {
        this.m_lib = lib;
        this.m_memo = "";
        this.m_type = map_mark.map_mark_type.axis0;
      }

      public data(gvt_lib lib, Point pos, map_mark.map_mark_type type, string memo)
      {
        this.m_lib = lib;
        this.set_data(pos, type, memo);
      }

      public bool Load(string line)
      {
        string[] strArray1 = line.Split(new char[1]
        {
          '\t'
        });
        string[] strArray2 = new string[4];
        for (int index = 0; index < strArray2.Length; ++index)
          strArray2[index] = "";
        int num = 4;
        if (strArray1.Length < num)
          num = strArray1.Length;
        for (int index = 0; index < num; ++index)
          strArray2[index] = strArray1[index];
        try
        {
          this.set_data(new Point(Convert.ToInt32(strArray2[0]), Convert.ToInt32(strArray2[1])), (map_mark.map_mark_type) Convert.ToInt32(strArray2[2]), strArray2[3]);
        }
        catch
        {
          return false;
        }
        return true;
      }

      private void set_data(Point pos, map_mark.map_mark_type type, string memo)
      {
        this.position = pos;
        this.m_type = type;
        this.m_memo = memo;
        this.rect = new Rectangle(-8, -8, 16, 16);
      }
    }
  }
}
