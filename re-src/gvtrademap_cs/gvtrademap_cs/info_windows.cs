// Type: gvtrademap_cs.info_windows
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using gvo_base;
using gvo_net_base;
using Microsoft.DirectX;
using System;
using System.Drawing;
using System.Windows.Forms;
using Utility;

namespace gvtrademap_cs
{
  public class info_windows
  {
    private static Color m_back_color = Color.FromArgb(220, 80, 80, 80);
    private const int OFFSET_X = -2;
    private gvt_lib m_lib;
    private GvoDatabase m_db;
    private myship_info m_myship_info;
    private hittest_list m_windows;
    private Point m_select_pos;
    private Point m_mouse_pos;

    static info_windows()
    {
    }

    public info_windows(gvt_lib lib, GvoDatabase db, myship_info myship)
    {
      this.m_lib = lib;
      this.m_db = db;
      this.m_myship_info = myship;
      this.m_select_pos = new Point(0, 0);
      this.m_mouse_pos = new Point(0, 0);
      this.m_windows = new hittest_list();
      this.m_windows.Add(new hittest());
      this.m_windows.Add(new hittest());
      this.m_windows.Add(new hittest());
      this.m_windows.Add(new hittest());
      this.m_windows.Add(new hittest());
      this.m_windows.Add(new hittest());
    }

    public void Update(Point select_pos, Point mouse_pos)
    {
      this.m_select_pos = select_pos;
      this.m_mouse_pos = mouse_pos;
      Size size1 = new Size((int) this.m_lib.device.client_size.X, (int) this.m_lib.device.client_size.Y);
      hittest hittest1 = this.m_windows[0];
      Size size2 = new Size(47, 51);
      hittest1.rect = new Rectangle(size1.Width - size2.Width - 3, 3, size2.Width, size2.Height);
      hittest hittest2 = this.m_windows[1];
      Size size3 = new Size(78, 72);
      hittest2.rect = new Rectangle(size1.Width - size3.Width, size1.Height - size3.Height, size3.Width, size3.Height);
      hittest hittest3 = this.m_windows[2];
      hittest3.enable = true;
      if (!this.m_lib.setting.enable_share_routes)
        hittest3.enable = false;
      Size size4 = new Size(78, 16);
      Rectangle rect = this.m_windows[1].rect;
      hittest3.rect = new Rectangle(size1.Width - size4.Width, rect.Y - size4.Height - 3, size4.Width, size4.Height);
      hittest hittest4 = this.m_windows[3];
      hittest4.enable = true;
      if (!this.m_lib.setting.enable_analize_log_chat)
        hittest4.enable = false;
      if (!this.m_lib.setting.save_searoutes)
        hittest4.enable = false;
      if (this.m_lib.setting.is_server_mode)
        hittest4.enable = true;
      Size size5 = new Size(78, 16);
      Rectangle parentWindowRect1 = this.get_parent_window_rect(3);
      hittest4.rect = new Rectangle(size1.Width - size5.Width, parentWindowRect1.Y - size5.Height - 3, size5.Width, size5.Height);
      hittest hittest5 = this.m_windows[4];
      hittest5.enable = true;
      if (!this.m_lib.setting.enable_analize_log_chat)
        hittest5.enable = false;
      if (!this.m_lib.setting.save_searoutes)
        hittest5.enable = false;
      if (this.m_lib.setting.is_server_mode)
        hittest5.enable = true;
      if (!this.m_lib.setting.force_show_build_ship)
        hittest5.enable = this.m_db.BuildShipCounter.IsNowBuild;
      Size size6 = new Size(78, 16);
      Rectangle parentWindowRect2 = this.get_parent_window_rect(4);
      hittest5.rect = new Rectangle(size1.Width - size6.Width, parentWindowRect2.Y - size6.Height - 3, size6.Width, size6.Height);
      hittest hittest6 = this.m_windows[5];
      hittest6.enable = true;
      if (!this.m_lib.setting.is_server_mode)
        hittest6.enable = false;
      Size size7 = new Size(78, 16);
      Rectangle parentWindowRect3 = this.get_parent_window_rect(5);
      hittest6.rect = new Rectangle(size1.Width - size7.Width, parentWindowRect3.Y - size7.Height - 3, size7.Width, size7.Height);
    }

    private Rectangle get_parent_window_rect(int start_index)
    {
      --start_index;
      if (start_index <= 1)
        return this.m_windows[1].rect;
      for (; start_index > 1; --start_index)
      {
        if (this.m_windows[start_index].enable)
          return this.m_windows[start_index].rect;
      }
      return this.m_windows[1].rect;
    }

    public void Draw()
    {
      this.m_lib.device.systemfont.Begin();
      this.m_lib.device.sprites.BeginDrawSprites(this.m_lib.icons.texture);
      this.draw_gpos();
      this.draw_speed();
      this.draw_share();
      this.draw_interest();
      this.draw_build_ship();
      this.draw_tcp_server();
      this.m_lib.device.sprites.EndDrawSprites();
      this.m_lib.device.systemfont.End();
    }

    private void draw_gpos()
    {
      hittest hittest = this.m_windows[1];
      Vector3 pos = new Vector3((float) hittest.rect.X, (float) hittest.rect.Y, 0.1f);
      Vector2 size = new Vector2((float) hittest.rect.Width, (float) hittest.rect.Height);
      this.m_lib.device.DrawFillRect(pos, size, info_windows.m_back_color.ToArgb());
      this.m_lib.device.DrawLineRect(pos, size, Color.Black.ToArgb());
      int y1 = (int) pos.Y + 2 + 1;
      this.m_lib.device.systemfont.DrawTextR(string.Format("{0}%", (object) (int) Math.Round((double) this.m_lib.loop_image.ImageScale * 100.0)), (int) pos.X + 75 - 2, y1, Color.White);
      this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3(pos.X + 6f, (float) y1, 0.1f), this.m_lib.icons.GetIcon(icons.icon_index.string04));
      int num = y1 + 13;
      this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3(pos.X + 6f, (float) num, 0.1f), this.m_lib.icons.GetIcon(icons.icon_index.string03));
      d3d_sprite_rects.rect icon = this.m_lib.icons.GetIcon(this.m_db.GvoSeason.now_season == gvo_season.season.summer ? icons.icon_index.string06 : icons.icon_index.string05);
      this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3((float) ((double) pos.X + 75.0 - 2.0 - 12.0), (float) num, 0.1f), icon);
      int y2 = num + 13;
      string text = "--- , ---";
      Point point = this.game_pos_2_map_pos_for_debug(this.m_myship_info.pos);
      if (this.m_myship_info.is_analized_pos)
        text = string.Format("{0} , {1}", (object) point.X, (object) point.Y);
      this.m_lib.device.systemfont.DrawTextR(text, (int) pos.X + 75 - 2, y2, Color.White);
      int y3 = y2 + 13;
      point = this.game_pos_2_map_pos_for_debug(this.m_select_pos);
      this.m_lib.device.systemfont.DrawTextR(string.Format("{0} , {1}", (object) point.X, (object) point.Y), (int) pos.X + 75 - 2, y3, Color.White);
      int y4 = y3 + 13;
      point = this.game_pos_2_map_pos_for_debug(transform.client_pos2_game_pos(this.m_mouse_pos, this.m_lib.loop_image));
      this.m_lib.device.systemfont.DrawTextR(string.Format("{0} , {1}", (object) point.X, (object) point.Y), (int) pos.X + 75 - 2, y4, Color.White);
      Vector2 vector2 = this.m_lib.loop_image.AjustLocalPos(this.m_lib.loop_image.GlobalPos2LocalPos(transform.game_pos2_map_pos(transform.ToVector2(this.m_select_pos), this.m_lib.loop_image)));
      this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3(vector2.X, vector2.Y, 0.3f), this.m_lib.icons.GetIcon(icons.icon_index.select_cross));
    }

    private Point game_pos_2_map_pos_for_debug(Point p)
    {
      if (!this.m_lib.setting.debug_flag_show_potision)
        return p;
      else
        return transform.game_pos2_map_pos(p, this.m_lib.loop_image);
    }

    private void draw_speed()
    {
      hittest hittest = this.m_windows[0];
      Vector3 pos1 = new Vector3((float) hittest.rect.X, (float) hittest.rect.Y, 0.3f);
      this.m_lib.device.sprites.AddDrawSpritesNC(pos1, this.m_lib.icons.GetIcon(icons.icon_index.speed_background));
      Vector3 pos2 = pos1;
      Vector3 pos3 = pos1;
      pos3.Z += -0.1f;
      pos2.Y += 17f;
      pos3.Y += 19f;
      pos3.X += 33f;
      this.m_lib.device.sprites.AddDrawSpritesNC(pos2, this.m_lib.icons.GetIcon(icons.icon_index.speed_background));
      this.m_lib.device.sprites.AddDrawSpritesNC(pos3, this.m_lib.icons.GetIcon(icons.icon_index.degree));
      pos2.Y += 17f;
      pos3.Y += 17f;
      this.m_lib.device.sprites.AddDrawSpritesNC(pos2, this.m_lib.icons.GetIcon(icons.icon_index.speed_background));
      this.m_lib.device.sprites.AddDrawSpritesNC(pos3, this.m_lib.icons.GetIcon(icons.icon_index.degree));
      ++pos2.X;
      ++pos2.Y;
      pos2.Z -= 0.05f;
      this.m_lib.device.DrawFillRect(pos2, new Vector2(this.m_db.SpeedCalculator.angle_precision * (float) (hittest.rect.Width - 1 - 1), 13f), Color.FromArgb((int) byte.MaxValue, 160, 210, (int) byte.MaxValue).ToArgb());
      int x = (int) pos1.X + hittest.rect.Width - 2;
      this.m_lib.device.systemfont.DrawTextR(this.m_db.SpeedCalculator.speed_knot.ToString("0.00") + "Kt", x, (int) pos1.Y + 1 + 1, Color.Black);
      if ((double) this.m_myship_info.angle >= 0.0)
        this.m_lib.device.systemfont.DrawTextR(this.m_myship_info.angle.ToString("0.0"), x - 12, (int) pos1.Y + 1 + 1 + 16 + 1, Color.Black);
      if ((double) this.m_db.SpeedCalculator.angle < 0.0)
        return;
      this.m_lib.device.systemfont.DrawTextR(this.m_db.SpeedCalculator.angle.ToString("0.0"), x - 12, (int) pos1.Y + 1 + 1 + 16 + 1 + 16 + 1, Color.Black);
    }

    private void draw_share()
    {
      hittest hittest = this.m_windows[2];
      if (!hittest.enable)
        return;
      Vector3 pos = new Vector3((float) hittest.rect.X, (float) hittest.rect.Y, 0.1f);
      Vector2 size = new Vector2((float) hittest.rect.Width, (float) hittest.rect.Height);
      this.m_lib.device.DrawFillRect(pos, size, info_windows.m_back_color.ToArgb());
      this.m_lib.device.DrawLineRect(pos, size, Color.Black.ToArgb());
      this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3(pos.X + 6f, pos.Y + 3f, 0.1f), this.m_lib.icons.GetIcon(icons.icon_index.string01));
      this.m_lib.device.systemfont.DrawTextR(string.Format("{0}", (object) this.m_db.ShareRoutes.ShareList.Count), (int) pos.X + 75 - 2, (int) pos.Y + 3, Color.White);
    }

    private void draw_interest()
    {
      hittest hittest = this.m_windows[3];
      if (!hittest.enable)
        return;
      Vector3 pos = new Vector3((float) hittest.rect.X, (float) hittest.rect.Y, 0.1f);
      Vector2 size = new Vector2((float) hittest.rect.Width, (float) hittest.rect.Height);
      this.m_lib.device.DrawFillRect(pos, size, info_windows.m_back_color.ToArgb());
      this.m_lib.device.DrawLineRect(pos, size, Color.Black.ToArgb());
      this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3(pos.X + 6f, pos.Y + 3f, 0.1f), this.m_lib.icons.GetIcon(icons.icon_index.string02));
      this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3((float) ((double) pos.X + 6.0 + 75.0 - 2.0 - 13.0), pos.Y + 3f, 0.1f), this.m_lib.icons.GetIcon(icons.icon_index.string00));
      this.m_lib.device.systemfont.DrawTextR(string.Format("{0}", (object) this.m_db.InterestDays.GetDays()), (int) pos.X + 75 - 2 - 8, (int) pos.Y + 3, Color.White);
    }

    private void draw_build_ship()
    {
      hittest hittest = this.m_windows[4];
      if (!hittest.enable)
        return;
      Vector3 pos = new Vector3((float) hittest.rect.X, (float) hittest.rect.Y, 0.1f);
      Vector2 size = new Vector2((float) hittest.rect.Width, (float) hittest.rect.Height);
      this.m_lib.device.DrawFillRect(pos, size, info_windows.m_back_color.ToArgb());
      this.m_lib.device.DrawLineRect(pos, size, Color.Black.ToArgb());
      this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3(pos.X + 6f, pos.Y + 3f, 0.1f), this.m_lib.icons.GetIcon(icons.icon_index.string07));
      this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3((float) ((double) pos.X + 6.0 + 75.0 - 2.0 - 13.0), pos.Y + 3f, 0.1f), this.m_lib.icons.GetIcon(icons.icon_index.string00));
      this.m_lib.device.systemfont.DrawTextR(string.Format("{0}", (object) this.m_db.BuildShipCounter.GetDays()), (int) pos.X + 75 - 2 - 8, (int) pos.Y + 3, Color.White);
    }

    private void draw_tcp_server()
    {
      hittest hittest = this.m_windows[5];
      if (!hittest.enable)
        return;
      Vector3 pos = new Vector3((float) hittest.rect.X, (float) hittest.rect.Y, 0.1f);
      Vector2 size = new Vector2((float) hittest.rect.Width, (float) hittest.rect.Height);
      this.m_lib.device.DrawFillRect(pos, size, info_windows.m_back_color.ToArgb());
      this.m_lib.device.DrawLineRect(pos, size, Color.Black.ToArgb());
      this.m_lib.device.systemfont.DrawText("TCP SERVER", (int) pos.X + 3, (int) pos.Y + 3, Color.White);
    }

    public bool HitTest(Point point)
    {
      switch (this.m_windows.HitTest_Index(point))
      {
        case 0:
          return true;
        case 1:
          return true;
        case 2:
          return true;
        case 3:
          return true;
        case 5:
          return true;
        default:
          return false;
      }
    }

    public bool OnMouseClick(Point point, MouseButtons mouseButtons, Form form)
    {
      switch (this.m_windows.HitTest_Index(point))
      {
        case 0:
          return true;
        case 1:
          return true;
        case 2:
          if ((mouseButtons & MouseButtons.Left) == MouseButtons.None)
            return true;
          using (share_routes_form shareRoutesForm = new share_routes_form(this.m_db.ShareRoutes.ShareList))
          {
            if (shareRoutesForm.ShowDialog((IWin32Window) form) == DialogResult.OK)
            {
              if (shareRoutesForm.is_selected)
              {
                this.m_lib.setting.centering_gpos = shareRoutesForm.selected_position;
                this.m_lib.setting.req_centering_gpos.Request();
                this.m_lib.setting.draw_share_routes = true;
              }
            }
          }
          return true;
        case 4:
          if ((mouseButtons & MouseButtons.Right) == MouseButtons.None || MessageBox.Show("造船カウンタをリセットしますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return true;
          this.m_db.BuildShipCounter.FinishBuildShip();
          return true;
        case 5:
          return true;
        default:
          return false;
      }
    }

    public string OnToolTipString(Point pos)
    {
      switch (this.m_windows.HitTest_Index(pos))
      {
        case 0:
          return "速度\nコンパスから解析した角度\n測量から解析した角度";
        case 1:
          string str1 = "地図拡縮率\n季節(" + this.m_db.GvoSeason.next_season_start_shortstr + "まで)\n自分の船の位置\nクロスカ\x30FCソルの位置\nマウスの位置";
          if (this.m_lib.setting.debug_flag_show_potision)
            str1 = str1 + "\n(デバッグフラグ有効)";
          return str1;
        case 2:
          string str2 = "";
          foreach (ShareRoutes.ShareShip shareShip in this.m_db.ShareRoutes.ShareList)
          {
            if (str2 != "")
              str2 = str2 + "\n";
            str2 = str2 + shareShip.Name;
          }
          if (str2 == "")
            str2 = "航路共有メンバ\x30FCが居ません";
          return str2;
        case 3:
          return this.m_db.InterestDays.GetPopupString();
        case 4:
          return this.m_db.BuildShipCounter.GetPopupString() + "\n(右クリックでリセット)";
        case 5:
          gvo_server_service serverService = this.m_myship_info.server_service;
          gvo_tcp_client client = serverService.GetClient();
          if (client != null)
            return "TCPサ\x30FCバモ\x30FCド\n接続済\n通信対象:" + client.remote_ep.ToString();
          return serverService.is_listening ? "TCPサ\x30FCバモ\x30FCド\n接続待ち" : "TCPサ\x30FCバモ\x30FCド\nサ\x30FCバの起動に失敗";
        default:
          return (string) null;
      }
    }

    private enum window_index
    {
      speed,
      position,
      share,
      interest,
      build_ship,
      tcp_server,
    }
  }
}
