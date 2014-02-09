// Type: gvtrademap_cs.ShareRoutes
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Utility;

namespace gvtrademap_cs
{
  public class ShareRoutes
  {
    private const int BB_ONCE_SIZE = 400;
    private const float ANGLE_LINE_LENGTH = 48f;
    private const int TEXTURED_FONT_REFLESH_INTERVAL = 30;
    private gvt_lib m_lib;
    private bool m_share_side;
    private List<ShareRoutes.ShareShip> m_share1;
    private List<ShareRoutes.ShareShip> m_share2;
    private List<ShareRoutes.ShareShipListBB> m_share_bb1;
    private List<ShareRoutes.ShareShipListBB> m_share_bb2;
    private bool m_req_textured_font_reflesh;
    private int m_network_access_count;

    public List<ShareRoutes.ShareShip> ShareList
    {
      get
      {
        if (!this.m_share_side)
          return this.m_share2;
        else
          return this.m_share1;
      }
    }

    public List<ShareRoutes.ShareShipListBB> ShareListBB
    {
      get
      {
        if (!this.m_share_side)
          return this.m_share_bb2;
        else
          return this.m_share_bb1;
      }
    }

    private List<ShareRoutes.ShareShip> ShareUpdateList
    {
      get
      {
        if (!this.m_share_side)
          return this.m_share1;
        else
          return this.m_share2;
      }
    }

    private List<ShareRoutes.ShareShipListBB> ShareUpdateListBB
    {
      get
      {
        if (!this.m_share_side)
          return this.m_share_bb1;
        else
          return this.m_share_bb2;
      }
    }

    public ShareRoutes(gvt_lib lib)
    {
      this.m_lib = lib;
      this.m_share1 = new List<ShareRoutes.ShareShip>();
      this.m_share2 = new List<ShareRoutes.ShareShip>();
      this.m_share_bb1 = new List<ShareRoutes.ShareShipListBB>();
      this.m_share_bb2 = new List<ShareRoutes.ShareShipListBB>();
      this.m_share_side = false;
      this.m_req_textured_font_reflesh = false;
      this.m_network_access_count = 0;
    }

    public void Share()
    {
      string data = HttpDownload.Download("http://gvtrademap.daa.jp/getallpos.php", Encoding.GetEncoding("shift_jis"));
      if (data == null)
        return;
      this.update_list(data);
    }

    public void Share(int x, int y, ShareRoutes.State _state)
    {
      if (this.m_lib.setting.share_group == "" || this.m_lib.setting.share_group_myname == "")
        return;
      string data = HttpDownload.Download("http://gvtrademap.daa.jp/" + "getposition.php?server=" + GvoWorldInfo.GetServerStringForShare(this.m_lib.setting.server) + "&group=" + Useful.UrlEncodeShiftJis(this.m_lib.setting.share_group) + "&name=" + Useful.UrlEncodeShiftJis(this.m_lib.setting.share_group_myname) + "&x=" + x.ToString() + "&y=" + y.ToString() + "&out=" + ((int) _state).ToString(), Encoding.GetEncoding("shift_jis"));
      if (data == null)
        return;
      this.update_list(data);
    }

    public void update_list(string data)
    {
      List<ShareRoutes.ShareShip> shareUpdateList = this.ShareUpdateList;
      shareUpdateList.Clear();
      string str1 = data;
      string[] separator = new string[2]
      {
        "\r\n",
        "\n"
      };
      int num1 = 1;
      foreach (string str2 in str1.Split(separator, (StringSplitOptions) num1))
      {
        if (!(str2 == "<html>"))
        {
          if (!(str2 == "</html>"))
          {
            string[] strArray = str2.Split(new char[1]
            {
              ','
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length >= 4)
            {
              if (!(strArray[0] == this.m_lib.setting.share_group_myname))
              {
                try
                {
                  int x = Convert.ToInt32(strArray[1]);
                  int y = Convert.ToInt32(strArray[2]);
                  int num2 = Convert.ToInt32(strArray[3]);
                  shareUpdateList.Add(new ShareRoutes.ShareShip(strArray[0], new Point(x, y), (ShareRoutes.State) num2));
                }
                catch
                {
                }
              }
            }
          }
          else
            break;
        }
      }
      this.update_move_angle();
      this.update_bb_list();
      this.flip();
      if (++this.m_network_access_count < 30)
        return;
      this.m_req_textured_font_reflesh = true;
      this.m_network_access_count = 0;
    }

    public void Draw()
    {
      if (this.m_req_textured_font_reflesh)
      {
        this.m_lib.device.textured_font.Clear();
        this.m_req_textured_font_reflesh = false;
      }
      if (!this.m_lib.setting.draw_share_routes)
        return;
      this.m_lib.device.device.RenderState.ZBufferEnable = false;
      this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_angle_proc), 64f);
      this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_name_proc), 256f);
      this.m_lib.device.device.RenderState.ZBufferEnable = true;
      this.m_lib.device.sprites.BeginDrawSprites(this.m_lib.icons.texture);
      this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_proc), 32f);
      this.m_lib.device.sprites.EndDrawSprites();
    }

    private void draw_proc(Vector2 offset, LoopXImage image)
    {
      List<ShareRoutes.ShareShipListBB> shareListBb = this.ShareListBB;
      D3dBB2d.CullingRect rect = new D3dBB2d.CullingRect(image.Device.client_size);
      foreach (ShareRoutes.ShareShipListBB shareShipListBb in shareListBb)
      {
        if (!shareShipListBb.IsCulling(offset, image.ImageScale, rect))
        {
          foreach (ShareRoutes.ShareShip shareShip in shareShipListBb.List)
          {
            if (shareShip.Position.X >= 0 && shareShip.Position.Y >= 0)
            {
              Vector2 global_pos = transform.game_pos2_map_pos(transform.ToVector2(shareShip.Position), image);
              Vector2 vector2 = image.GlobalPos2LocalPos(global_pos, offset);
              icons.icon_index index = shareShip.State == ShareRoutes.State.in_the_sea ? icons.icon_index.myship : icons.icon_index.share_city;
              this.m_lib.device.sprites.AddDrawSprites(new Vector3(vector2.X, vector2.Y, 0.31f), this.m_lib.icons.GetIcon(index));
            }
          }
        }
      }
    }

    private void draw_name_proc(Vector2 offset, LoopXImage image)
    {
      List<ShareRoutes.ShareShipListBB> shareListBb = this.ShareListBB;
      D3dBB2d.CullingRect rect = new D3dBB2d.CullingRect(image.Device.client_size);
      d3d_textured_font texturedFont = this.m_lib.device.textured_font;
      foreach (ShareRoutes.ShareShipListBB shareShipListBb in shareListBb)
      {
        if (!shareShipListBb.IsCulling(offset, image.ImageScale, rect))
        {
          foreach (ShareRoutes.ShareShip shareShip in shareShipListBb.List)
          {
            if (shareShip.Position.X >= 0 && shareShip.Position.Y >= 0)
            {
              Vector2 global_pos = transform.game_pos2_map_pos(new Vector2((float) shareShip.Position.X, (float) shareShip.Position.Y), image);
              Vector2 vector2 = image.GlobalPos2LocalPos(global_pos, offset);
              Rectangle rectangle = texturedFont.MeasureText(shareShip.Name, Color.Black);
              rectangle.Width += 4;
              rectangle.Height += 4;
              int num = rectangle.Width / 2;
              Vector3 pos = new Vector3((float) ((double) vector2.X - (double) num - 2.0), vector2.Y - 3f, 0.31f);
              Vector2 size = new Vector2((float) rectangle.Width, (float) rectangle.Height);
              this.m_lib.device.DrawFillRect(pos, size, Color.FromArgb(128, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue).ToArgb());
              this.m_lib.device.DrawLineRect(pos, size, Color.Gray.ToArgb());
              texturedFont.DrawTextC(shareShip.Name, new Vector3(vector2.X, vector2.Y + 1f, 0.31f), Color.Black);
            }
          }
        }
      }
    }

    private void draw_angle_proc(Vector2 offset, LoopXImage image)
    {
      List<ShareRoutes.ShareShip> shareList = this.ShareList;
      this.m_lib.device.line.Width = 1f;
      this.m_lib.device.line.Antialias = this.m_lib.setting.enable_line_antialias;
      this.m_lib.device.line.Pattern = -1;
      this.m_lib.device.line.PatternScale = 1f;
      this.m_lib.device.line.Begin();
      foreach (ShareRoutes.ShareShip shareShip in shareList)
      {
        if (shareShip.Position.X >= 0 && shareShip.Position.Y >= 0 && (double) shareShip.AngleVector.LengthSq() >= 0.5)
        {
          Vector2 global_pos = transform.game_pos2_map_pos(new Vector2((float) shareShip.Position.X, (float) shareShip.Position.Y), image);
          Vector2 vector2 = image.GlobalPos2LocalPos(global_pos, offset);
          if ((double) vector2.X + 48.0 >= 0.0 && (double) vector2.X - 48.0 < (double) image.Device.client_size.X && ((double) vector2.Y + 48.0 >= 0.0 && (double) vector2.Y - 48.0 < (double) image.Device.client_size.Y))
            this.m_lib.device.line.Draw(new Vector2[2]
            {
              vector2,
              vector2 + shareShip.AngleVector * 48f
            }, Color.FromArgb(200, 64, 64, 64));
        }
      }
      this.m_lib.device.line.End();
    }

    private void flip()
    {
      this.m_share_side = !this.m_share_side;
    }

    private void update_move_angle()
    {
      foreach (ShareRoutes.ShareShip shareShip in this.ShareUpdateList)
      {
        ShareRoutes.ShareShip ship = this.find_ship(shareShip.Name);
        if (ship != null)
        {
          Vector2 vector2 = transform.SubVector_LoopX(shareShip.Position, ship.Position, 16384);
          vector2.Normalize();
          shareShip.AngleVector = (double) vector2.LengthSq() <= 0.5 ? new Vector2(0.0f, 0.0f) : vector2;
        }
      }
    }

    private void update_bb_list()
    {
      List<ShareRoutes.ShareShip> shareUpdateList = this.ShareUpdateList;
      List<ShareRoutes.ShareShipListBB> shareUpdateListBb = this.ShareUpdateListBB;
      shareUpdateListBb.Clear();
      List<ShareRoutes.ShareShip> free_list = new List<ShareRoutes.ShareShip>();
      foreach (ShareRoutes.ShareShip shareShip in shareUpdateList)
        free_list.Add(shareShip);
      while (free_list.Count > 0)
        shareUpdateListBb.Add(this.create_bb(ref free_list));
    }

    private ShareRoutes.ShareShipListBB create_bb(ref List<ShareRoutes.ShareShip> free_list)
    {
      List<ShareRoutes.ShareShip> list = free_list;
      free_list = new List<ShareRoutes.ShareShip>();
      ShareRoutes.ShareShipListBB shareShipListBb = new ShareRoutes.ShareShipListBB();
      foreach (ShareRoutes.ShareShip p in list)
      {
        if (!shareShipListBb.Add(p, this.m_lib.loop_image))
          free_list.Add(p);
      }
      return shareShipListBb;
    }

    private ShareRoutes.ShareShip find_ship(string name)
    {
      foreach (ShareRoutes.ShareShip shareShip in this.ShareList)
      {
        if (shareShip.Name == name)
          return shareShip;
      }
      return (ShareRoutes.ShareShip) null;
    }

    public enum State
    {
      outof_sea,
      in_the_sea,
    }

    public class ShareShip
    {
      private string m_name;
      private Point m_pos;
      private ShareRoutes.State m_state;
      private Vector2 m_angle_vector;

      public string Name
      {
        get
        {
          return this.m_name;
        }
      }

      public Point Position
      {
        get
        {
          return this.m_pos;
        }
      }

      public ShareRoutes.State State
      {
        get
        {
          return this.m_state;
        }
      }

      public Vector2 AngleVector
      {
        get
        {
          return this.m_angle_vector;
        }
        set
        {
          this.m_angle_vector = value;
        }
      }

      public ShareShip(string name, Point pos, ShareRoutes.State _state)
      {
        this.m_name = name;
        this.m_pos = pos;
        this.m_state = _state;
        this.m_angle_vector = new Vector2(0.0f, 0.0f);
      }
    }

    public class ShareShipListBB : D3dBB2d
    {
      private List<ShareRoutes.ShareShip> m_list;

      public List<ShareRoutes.ShareShip> List
      {
        get
        {
          return this.m_list;
        }
      }

      public ShareShipListBB()
      {
        this.m_list = new List<ShareRoutes.ShareShip>();
        this.OffsetLT = new Vector2(-64f, -8f);
        this.OffsetRB = new Vector2(64f, 16f);
      }

      public bool Add(ShareRoutes.ShareShip p, LoopXImage image)
      {
        Vector2 pos = transform.game_pos2_map_pos(transform.ToVector2(p.Position), image);
        Vector2 size = this.IfUpdate(pos).Size;
        if ((double) size.X > 400.0 || (double) size.Y > 400.0)
          return false;
        this.m_list.Add(p);
        this.Update(pos);
        return true;
      }
    }
  }
}
