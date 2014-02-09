// Type: gvtrademap_cs.gvt_lib
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using System;
using System.Windows.Forms;
using Utility.Ini;
using Utility.KeyAssign;

namespace gvtrademap_cs
{
  public class gvt_lib : IDisposable
  {
    private d3d_device m_d3d_device;
    private LoopXImage m_loop_x_image;
    private icons m_icons;
    private infonameimage m_infonameimage;
    private seainfonameimage m_seainfonameimage;
    private GlobalSettings m_setting;
    private IniProfileSetting m_ini_manager;
    private KeyAssignManager m_key_assign_manager;

    public LoopXImage loop_image
    {
      get
      {
        return this.m_loop_x_image;
      }
    }

    public d3d_device device
    {
      get
      {
        return this.m_d3d_device;
      }
    }

    public icons icons
    {
      get
      {
        return this.m_icons;
      }
    }

    public infonameimage infonameimage
    {
      get
      {
        return this.m_infonameimage;
      }
    }

    public seainfonameimage seainfonameimage
    {
      get
      {
        return this.m_seainfonameimage;
      }
    }

    public GlobalSettings setting
    {
      get
      {
        return this.m_setting;
      }
    }

    public IniProfileSetting IniManager
    {
      get
      {
        return this.m_ini_manager;
      }
    }

    public KeyAssignManager KeyAssignManager
    {
      get
      {
        return this.m_key_assign_manager;
      }
    }

    public gvt_lib(Form form, string ini_file_name)
    {
      this.m_ini_manager = new IniProfileSetting(ini_file_name);
      this.m_setting = new GlobalSettings();
      this.m_key_assign_manager = new KeyAssignManager();
      this.m_ini_manager.AddIniSaveLoad((IIniSaveLoad) this.m_setting);
      this.m_ini_manager.AddIniSaveLoad((IIniSaveLoad) this.m_key_assign_manager, "key_assign");
      this.m_d3d_device = new d3d_device(form);
      this.m_d3d_device.skip_max = 5;
      this.m_loop_x_image = new LoopXImage(this.m_d3d_device);
      this.m_icons = new icons(this.m_d3d_device, "map\\gv_pics20.dds");
      this.m_infonameimage = new infonameimage(this.m_d3d_device, "map\\infonames.dds");
      this.m_seainfonameimage = new seainfonameimage(this.m_d3d_device, "map\\seainfonames.dds");
      this.init_key_assign();
    }

    public void Dispose()
    {
      if (this.m_loop_x_image != null)
        this.m_loop_x_image.Dispose();
      if (this.m_icons != null)
        this.m_icons.Dispose();
      if (this.m_infonameimage != null)
        this.m_infonameimage.Dispose();
      if (this.m_seainfonameimage != null)
        this.m_seainfonameimage.Dispose();
      if (this.m_d3d_device != null)
        this.m_d3d_device.Dispose();
      this.m_loop_x_image = (LoopXImage) null;
      this.m_icons = (icons) null;
      this.m_infonameimage = (infonameimage) null;
      this.m_seainfonameimage = (seainfonameimage) null;
      this.m_d3d_device = (d3d_device) null;
    }

    private void init_key_assign()
    {
      this.m_key_assign_manager.List.AddAssign("地図の変更", "地図", Keys.M, (object) KeyFunction.map_change);
      this.m_key_assign_manager.List.AddAssign("地図の縮尺リセット", "地図", Keys.Home, (object) KeyFunction.map_reset_scale);
      this.m_key_assign_manager.List.AddAssign("地図を拡大", "地図", Keys.Add, (object) KeyFunction.map_zoom_in);
      this.m_key_assign_manager.List.AddAssign("地図を縮小", "地図", Keys.Subtract, (object) KeyFunction.map_zoom_out);
      this.m_key_assign_manager.List.AddAssign("ブル\x30FCラインリセット", "進行方向線", Keys.B, (object) KeyFunction.blue_line_reset);
      this.m_key_assign_manager.List.AddAssign("スポット解除", "解除", Keys.Escape, (object) KeyFunction.cancel_spot);
      this.m_key_assign_manager.List.AddAssign("航路図の選択解除", "解除", Keys.Escape, (object) KeyFunction.cancel_select_sea_routes);
      this.m_key_assign_manager.List.AddAssign("アイテムウインドウの表示/最小化", "アイテムウインドウ", Keys.None, (object) KeyFunction.item_window_show_min);
      this.m_key_assign_manager.List.AddAssign("次のタブへ移動", "アイテムウインドウ", Keys.Tab | Keys.Control, (object) KeyFunction.item_window_next_tab);
      this.m_key_assign_manager.List.AddAssign("前のタブへ移動", "アイテムウインドウ", Keys.Tab | Keys.Shift | Keys.Control, (object) KeyFunction.item_window_prev_tab);
      this.m_key_assign_manager.List.AddAssign("設定ウインドウの表示/最小化", "設定ウインドウ", Keys.None, (object) KeyFunction.setting_window_show_min);
      this.m_key_assign_manager.List.AddAssign("航路記録ON/OFF", "設定ウインドウ", Keys.None, (object) KeyFunction.setting_window_button_00);
      this.m_key_assign_manager.List.AddAssign("共有している船表示ON/OFF", "設定ウインドウ", Keys.None, (object) KeyFunction.setting_window_button_01);
      this.m_key_assign_manager.List.AddAssign("@Webアイコン表示ON/OFF", "設定ウインドウ", Keys.None, (object) KeyFunction.setting_window_button_02);
      this.m_key_assign_manager.List.AddAssign("メモアイコン表示ON/OFF", "設定ウインドウ", Keys.None, (object) KeyFunction.setting_window_button_03);
      this.m_key_assign_manager.List.AddAssign("航路線表示ON/OFF", "設定ウインドウ", Keys.None, (object) KeyFunction.setting_window_button_04);
      this.m_key_assign_manager.List.AddAssign("日付ふきだし表示切り替え", "設定ウインドウ", Keys.None, (object) KeyFunction.setting_window_button_05);
      this.m_key_assign_manager.List.AddAssign("災害ポップアップ表示ON/OFF", "設定ウインドウ", Keys.None, (object) KeyFunction.setting_window_button_06);
      this.m_key_assign_manager.List.AddAssign("現在位置中心表示ON/OFF", "設定ウインドウ", Keys.None, (object) KeyFunction.setting_window_button_07);
      this.m_key_assign_manager.List.AddAssign("進行方向線表示ON/OFF", "設定ウインドウ", Keys.None, (object) KeyFunction.setting_window_button_08);
      this.m_key_assign_manager.List.AddAssign("海域変動システムの設定", "設定ウインドウ", Keys.None, (object) KeyFunction.setting_window_button_09);
      this.m_key_assign_manager.List.AddAssign("航路図のスクリ\x30FCンショット", "設定ウインドウ", Keys.None, (object) KeyFunction.setting_window_button_10);
      this.m_key_assign_manager.List.AddAssign("航路図一覧を開く", "設定ウインドウ", Keys.L | Keys.Control, (object) KeyFunction.setting_window_button_11);
      this.m_key_assign_manager.List.AddAssign("できるだけ検索を開く", "設定ウインドウ", Keys.F | Keys.Control, (object) KeyFunction.setting_window_button_12);
      this.m_key_assign_manager.List.AddAssign("設定ダイアログを開く", "設定ウインドウ", Keys.None, (object) KeyFunction.setting_window_button_13);
      this.m_key_assign_manager.List.AddAssign("海域情報収集を起動する", "設定ウインドウ", Keys.A | Keys.Control, (object) KeyFunction.setting_window_button_exec_gvoac);
      this.m_key_assign_manager.List.AddAssign("航路図のスクリ\x30FCンショットフォルダを開く", "フォルダ", Keys.None, (object) KeyFunction.folder_open_00);
      this.m_key_assign_manager.List.AddAssign("大航海時代Onlineのメ\x30FCルフォルダを開く", "フォルダ", Keys.None, (object) KeyFunction.folder_open_01);
      this.m_key_assign_manager.List.AddAssign("大航海時代Onlineのチャットフォルダを開く", "フォルダ", Keys.None, (object) KeyFunction.folder_open_02);
      this.m_key_assign_manager.List.AddAssign("大航海時代Onlineのスクリ\x30FCンショットフォルダを開く", "フォルダ", Keys.None, (object) KeyFunction.folder_open_03);
      this.m_key_assign_manager.List.AddAssign("最前面表示ON/OFF", "ウインドウ", Keys.None, (object) KeyFunction.window_top_most);
      this.m_key_assign_manager.List.AddAssign("最大化/通常化", "ウインドウ", Keys.None, (object) KeyFunction.window_maximize);
      this.m_key_assign_manager.List.AddAssign("最小化", "ウインドウ", Keys.None, (object) KeyFunction.window_minimize);
      this.m_key_assign_manager.List.AddAssign("ウインドウ枠の表示/非表示", "ウインドウ", Keys.None, (object) KeyFunction.window_change_border_style);
      this.m_key_assign_manager.List.AddAssign("交易MAP C#を閉じる", "ウインドウ", Keys.None, (object) KeyFunction.window_close);
    }
  }
}
