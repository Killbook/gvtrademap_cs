// Type: gvtrademap_cs.setting_window
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using System.Drawing;
using System.Windows.Forms;
using Utility;

namespace gvtrademap_cs
{
  internal class setting_window : d3d_windows.window
  {
    private const int WINDOW_POS_X = 3;
    private const int WINDOW_POS_Y = 3;
    private const float WINDOW_POS_Z = 0.2f;
    private const int WINDOW_SIZE_X = 250;
    private const int WINDOW_SIZE_Y = 200;
    private const int CLIENT_SIZE_X = 246;
    private const int SETTING_ICONS_STEP = 16;
    private gvtrademap_cs_form m_form;
    private gvt_lib m_lib;
    private GvoDatabase m_db;
    private hittest_list m_hittest_list;

    public setting_window(gvt_lib lib, GvoDatabase db, gvtrademap_cs_form form)
      : base(lib.device, new Vector2(3f, 3f), new Vector2(250f, 200f), 0.2f)
    {
      this.title = "設定ウインドウ";
      this.m_form = form;
      this.m_lib = lib;
      this.m_db = db;
      this.m_hittest_list = new hittest_list();
      this.m_hittest_list.Add(new hittest());
      this.m_hittest_list.Add(new hittest());
    }

    protected override void OnUpdateClient()
    {
      this.client_size = new Vector2(246f, 16f);
      this.pos = new Vector2(3f, (float) ((double) this.screen_size.Y - (double) this.size.Y - 4.0));
      Point point = transform.ToPoint(this.client_pos);
      foreach (hittest hittest in this.m_hittest_list)
        hittest.position = point;
      this.m_hittest_list[0].rect = new Rectangle(1, 0, 192, 16);
      this.m_hittest_list[1].rect = new Rectangle((int) this.client_size.X - 48, 0, 48, 16);
    }

    protected override void OnDrawClient()
    {
      this.draw_seting_back();
      this.draw_current_setting_back();
      this.device.sprites.BeginDrawSprites(this.m_lib.icons.texture);
      this.draw_setting();
      this.draw_setting_button();
      this.device.sprites.EndDrawSprites();
    }

    private void draw_seting_back()
    {
      Rectangle rectangle = this.m_hittest_list[0].CalcRect();
      this.device.DrawFillRect(new Vector3(this.client_pos.X, (float) rectangle.Y, this.z), new Vector2(this.client_size.X, (float) (rectangle.Height + 1)), Color.FromArgb((int) byte.MaxValue, 96, 96, 96).ToArgb());
    }

    private void draw_current_setting_back()
    {
      Point clientMousePosition = this.device.GetClientMousePosition();
      hittest hittest = this.m_hittest_list[0];
      if (!hittest.HitTest(clientMousePosition))
        return;
      Rectangle rectangle = hittest.CalcRect();
      clientMousePosition.X -= rectangle.X;
      clientMousePosition.X /= 16;
      this.DrawCurrentButtonBack(new Vector3((float) (rectangle.X + 16 * clientMousePosition.X), (float) rectangle.Y, this.z), new Vector2(16f, 16f));
    }

    private void draw_setting_button()
    {
      Rectangle rectangle = this.m_hittest_list[1].CalcRect();
      this.device.sprites.AddDrawSpritesNC(new Vector3((float) rectangle.X, (float) rectangle.Y, this.z), this.m_lib.icons.GetIcon(icons.icon_index.setting_button));
    }

    private void draw_setting()
    {
      Rectangle rectangle = this.m_hittest_list[0].CalcRect();
      Vector3 pos = new Vector3((float) rectangle.X, (float) rectangle.Y, this.z);
      this.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(this.m_lib.setting.save_searoutes ? icons.icon_index.setting_0 : icons.icon_index.setting_gray_0));
      pos.X += 16f;
      this.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(this.m_lib.setting.draw_share_routes ? icons.icon_index.setting_1 : icons.icon_index.setting_gray_1));
      pos.X += 16f;
      this.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(this.m_lib.setting.draw_web_icons ? icons.icon_index.setting_8 : icons.icon_index.setting_gray_8));
      pos.X += 16f;
      this.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(this.m_lib.setting.draw_icons ? icons.icon_index.setting_2 : icons.icon_index.setting_gray_2));
      pos.X += 16f;
      this.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(this.m_lib.setting.draw_sea_routes ? icons.icon_index.setting_3 : icons.icon_index.setting_gray_3));
      pos.X += 16f;
      this.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(this.m_lib.setting.draw_popup_day_interval == 1 ? icons.icon_index.setting_4 : (this.m_lib.setting.draw_popup_day_interval == 5 ? icons.icon_index.setting_12 : icons.icon_index.setting_gray_4)));
      pos.X += 16f;
      this.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(this.m_lib.setting.draw_accident ? icons.icon_index.setting_5 : icons.icon_index.setting_gray_5));
      pos.X += 16f;
      this.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(this.m_lib.setting.center_myship ? icons.icon_index.setting_6 : icons.icon_index.setting_gray_6));
      pos.X += 16f;
      this.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(this.m_lib.setting.draw_myship_angle ? icons.icon_index.setting_7 : icons.icon_index.setting_gray_7));
      pos.X += 16f;
      for (int index = 0; index < 3; ++index)
      {
        this.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon((icons.icon_index) (74 + index)));
        pos.X += 16f;
      }
    }

    protected override void OnMouseClikClient(Point pos, MouseButtons button)
    {
      if ((button & MouseButtons.Left) != MouseButtons.None)
      {
        this._window_on_mouse_l_click(pos);
      }
      else
      {
        if ((button & MouseButtons.Right) == MouseButtons.None)
          return;
        this._window_on_mouse_r_click(pos);
      }
    }

    protected override void OnMouseDClikClient(Point pos, MouseButtons button)
    {
      if ((button & MouseButtons.Left) == MouseButtons.None)
        return;
      this._window_on_mouse_l_click(pos);
    }

    private void _window_on_mouse_l_click(Point pos)
    {
      switch (this.m_hittest_list.HitTest_Index(pos))
      {
        case 0:
          this.on_mouse_l_click_setting(pos);
          break;
        case 1:
          this.on_mouse_l_click_setting_button(pos);
          break;
      }
    }

    private void on_mouse_l_click_setting(Point pos)
    {
      Rectangle rectangle = this.m_hittest_list[0].CalcRect();
      pos.X -= rectangle.X;
      pos.X /= 16;
      this.m_form.ExecFunction(this.get_extc_function((setting_window.setting_icons_index) pos.X));
    }

    private KeyFunction get_extc_function(setting_window.setting_icons_index index)
    {
      switch (index)
      {
        case setting_window.setting_icons_index.save_searoutes:
          return KeyFunction.setting_window_button_00;
        case setting_window.setting_icons_index.share_routes:
          return KeyFunction.setting_window_button_01;
        case setting_window.setting_icons_index.web_icons:
          return KeyFunction.setting_window_button_02;
        case setting_window.setting_icons_index.memo_icons:
          return KeyFunction.setting_window_button_03;
        case setting_window.setting_icons_index.searoutes:
          return KeyFunction.setting_window_button_04;
        case setting_window.setting_icons_index.popup_day_interval:
          return KeyFunction.setting_window_button_05;
        case setting_window.setting_icons_index.accident:
          return KeyFunction.setting_window_button_06;
        case setting_window.setting_icons_index.center_my_ship:
          return KeyFunction.setting_window_button_07;
        case setting_window.setting_icons_index.myship_angle:
          return KeyFunction.setting_window_button_08;
        case setting_window.setting_icons_index.sea_area:
          return KeyFunction.setting_window_button_09;
        case setting_window.setting_icons_index.screen_shot:
          return KeyFunction.setting_window_button_10;
        case setting_window.setting_icons_index.show_searoutes_list:
          return KeyFunction.setting_window_button_11;
        default:
          return KeyFunction.unknown_function;
      }
    }

    private void on_mouse_l_click_setting_button(Point pos)
    {
      Rectangle rectangle = this.m_hittest_list[1].CalcRect();
      pos.X -= rectangle.X;
      if (pos.X < 16)
        this.m_form.ExecFunction(KeyFunction.setting_window_button_12);
      else
        this.m_form.ExecFunction(KeyFunction.setting_window_button_13);
    }

    protected override string OnToolTipStringClient(Point pos)
    {
      switch (this.m_hittest_list.HitTest_Index(pos))
      {
        case 0:
          return this.get_tooltip_string_setting(pos);
        case 1:
          return this.get_tooltip_string_setting_button(pos);
        default:
          return (string) null;
      }
    }

    private string get_tooltip_string_setting(Point pos)
    {
      Rectangle rectangle = this.m_hittest_list[0].CalcRect();
      pos.X -= rectangle.X;
      pos.X /= 16;
      setting_window.setting_icons_index index = (setting_window.setting_icons_index) pos.X;
      string str;
      switch (index)
      {
        case setting_window.setting_icons_index.save_searoutes:
          str = "航路記録（要測量）";
          break;
        case setting_window.setting_icons_index.share_routes:
          str = "共有している船表示（要設定）";
          break;
        case setting_window.setting_icons_index.web_icons:
          str = "@Webアイコン表示\n右クリックで表示項目設定";
          break;
        case setting_window.setting_icons_index.memo_icons:
          str = "メモアイコン表示\n右クリックで表示項目設定";
          break;
        case setting_window.setting_icons_index.searoutes:
          str = "航路線表示";
          break;
        case setting_window.setting_icons_index.popup_day_interval:
          str = "ふきだし表示";
          break;
        case setting_window.setting_icons_index.accident:
          str = "災害表示\n右クリックで表示項目設定";
          break;
        case setting_window.setting_icons_index.center_my_ship:
          str = "現在位置中心表示";
          break;
        case setting_window.setting_icons_index.myship_angle:
          str = "コンパスの角度線、進路予想線表示\n右クリックで表示項目設定";
          break;
        case setting_window.setting_icons_index.sea_area:
          str = "危険海域変動システムの設定";
          break;
        case setting_window.setting_icons_index.screen_shot:
          str = "航路のスクリ\x30FCンショット保存";
          break;
        case setting_window.setting_icons_index.show_searoutes_list:
          str = "航路図一覧\n右クリックで航路図設定";
          break;
        default:
          return (string) null;
      }
      return str + this.get_assign_shortcut_text(this.get_extc_function(index));
    }

    private string get_assign_shortcut_text(KeyFunction function)
    {
      string assignShortcutText = this.m_lib.KeyAssignManager.List.GetAssignShortcutText((object) function);
      if (string.IsNullOrEmpty(assignShortcutText))
        return "";
      else
        return "(" + assignShortcutText + ")";
    }

    private string get_tooltip_string_setting_button(Point pos)
    {
      Rectangle rectangle = this.m_hittest_list[1].CalcRect();
      pos.X -= rectangle.X;
      if (pos.X < 16)
        return "できるだけ検索ダイアログを開く" + this.get_assign_shortcut_text(KeyFunction.setting_window_button_12);
      else
        return "設定ダイアログを開く" + this.get_assign_shortcut_text(KeyFunction.setting_window_button_13);
    }

    private void _window_on_mouse_r_click(Point pos)
    {
      if (this.m_hittest_list.HitTest_Index(pos) != 0)
        return;
      this.on_mouse_r_click_setting(pos);
    }

    private void on_mouse_r_click_setting(Point pos)
    {
      Rectangle rectangle = this.m_hittest_list[0].CalcRect();
      pos.X -= rectangle.X;
      pos.X /= 16;
      switch (pos.X)
      {
        case 2:
          this.set_draw_setting(DrawSettingPage.WebIcons);
          break;
        case 3:
          this.set_draw_setting(DrawSettingPage.MemoIcons);
          break;
        case 6:
          this.set_draw_setting(DrawSettingPage.Accidents);
          break;
        case 8:
          this.set_draw_setting(DrawSettingPage.MyShipAngle);
          break;
        case 11:
          using (setting_form2 dlg = new setting_form2(this.m_lib.setting, this.m_lib.KeyAssignManager.List, this.m_lib.device.deviec_info_string, setting_form2.tab_index.sea_routes))
          {
            if (dlg.ShowDialog((IWin32Window) this.device.form) != DialogResult.OK)
              break;
            this.m_form.UpdateSettings(dlg);
            break;
          }
      }
    }

    private void set_draw_setting(DrawSettingPage type)
    {
      using (setting_form2 dlg = new setting_form2(this.m_lib.setting, this.m_lib.KeyAssignManager.List, this.m_lib.device.deviec_info_string, type))
      {
        if (dlg.ShowDialog((IWin32Window) this.device.form) != DialogResult.OK)
          return;
        this.m_form.UpdateSettings(dlg);
      }
    }

    private enum item_index
    {
      setting,
      setting_button,
      max,
    }

    private enum setting_icons_index
    {
      save_searoutes,
      share_routes,
      web_icons,
      memo_icons,
      searoutes,
      popup_day_interval,
      accident,
      center_my_ship,
      myship_angle,
      sea_area,
      screen_shot,
      show_searoutes_list,
      max,
    }
  }
}
