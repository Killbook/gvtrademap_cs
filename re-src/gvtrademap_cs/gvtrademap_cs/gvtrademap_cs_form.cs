// Type: gvtrademap_cs.gvtrademap_cs_form
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using gvo_base;
using gvtrademap_cs.Properties;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Utility;
using Utility.KeyAssign;
using win32;

namespace gvtrademap_cs
{
  public class gvtrademap_cs_form : Form
  {
    private const int CHAT_LOG_TIMER_INTERVAL = 5100;
    private const int SHARE_TIMER_INTERVAL = 60000;
    private const int TOOLTIP_INITIAL = 15;
    private IContainer components;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem ToolStripMenuItem_country0;
    private ToolStripMenuItem ToolStripMenuItem_country1;
    private ToolStripMenuItem ToolStripMenuItem_country2;
    private ToolStripMenuItem ToolStripMenuItem_country3;
    private ToolStripMenuItem ToolStripMenuItem_country4;
    private ToolStripMenuItem ToolStripMenuItem_country5;
    private ToolStripMenuItem ToolStripMenuItem_country6;
    private ToolStripMenuItem ToolStripMenuItem_country00;
    private TextBox textBox1;
    private ContextMenuStrip contextMenuStrip2;
    private ToolStripMenuItem set_target_memo_icon_ToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem add_memo_icon_ToolStripMenuItem;
    private ToolStripMenuItem edit_memo_icon_ToolStripMenuItem;
    private ToolStripMenuItem remove_memo_icon_ToolStripMenuItem;
    private ToolStripMenuItem remove_all_memo_icon_ToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem remove_all_target_memo_icon_ToolStripMenuItem;
    private ContextMenuStrip contextMenuStrip3;
    private ToolStripMenuItem open_recipe_wiki1_ToolStripMenuItem;
    private ToolStripMenuItem open_recipe_wiki0_ToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripMenuItem copy_name_to_clipboardToolStripMenuItem;
    private ToolStripMenuItem copy_all_to_clipboardToolStripMenuItem;
    private ListView listView1;
    private ToolStripMenuItem clear_spotToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator4;
    private ToolStripMenuItem spotToolStripMenuItem1;
    private ToolStripSeparator toolStripSeparator5;
    private ToolStripMenuItem openpathlogToolStripMenuItem;
    private ToolStripMenuItem openpathmailToolStripMenuItem;
    private ToolStripMenuItem openpathscreenshotToolStripMenuItem;
    private ToolStripMenuItem setseaareastateToolStripMenuItem;
    private ToolStripMenuItem normal_sea_area_ToolStripMenuItem;
    private ToolStripMenuItem safty_sea_area_ToolStripMenuItem;
    private ToolStripMenuItem lawless_sea_area_ToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator6;
    private ToolStripMenuItem exexgvoacToolStripMenuItem;
    private ToolStripMenuItem openpathscreenshot2ToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator7;
    private ToolStripMenuItem changeBorderStyleToolStripMenuItem;
    private ToolStripMenuItem closeFormToolStripMenuItem;
    private string m_current_path;
    private Point m_old_mouse_pos;
    private Point m_mouse_move;
    private myship_info m_myship_info;
    private Point m_select_pos;
    private MapIndex m_map_index;
    private bool m_use_mixed_map;
    private bool m_pause;
    private gvt_lib m_lib;
    private GvoDatabase m_db;
    private d3d_windows m_windows;
    private item_window m_item_window;
    private setting_window m_setting_window;
    private spot m_spot;
    private info_windows m_info_windows;
    private find_form2 m_find_form;
    private RequestCtrl m_req_show_find_form;
    private sea_routes_form2 m_sea_routes_form;
    private RequestCtrl m_req_sea_routes_form;
    private globalmouse_hook m_mouse_hook;
    private ToolTip m_tooltip;
    private int m_tooltip_interval;
    private bool m_show_tooltip;
    private Point m_tooltip_old_mouse_pos;
    private ManualResetEvent m_exit_thread_event;
    private Thread m_load_map_t;
    private Thread m_load_info_t;
    private Thread m_share_t;
    private Thread m_chat_log_t;
    private LoadInfosStatus _LoadInfosStatus;
    private System.Windows.Forms.Timer m_share_timer;
    private Point m_memo_icon_pos;
    private map_mark.data m_memo_icon_data;
    private string m_device_info_string;

    private bool is_load_map
    {
      get
      {
        if (this.m_load_map_t == null)
          return false;
        else
          return this.m_load_map_t.IsAlive;
      }
    }

    private bool is_load_info
    {
      get
      {
        if (this.m_load_info_t == null)
          return false;
        else
          return this.m_load_info_t.IsAlive;
      }
    }

    private bool is_load
    {
      get
      {
        return this.is_load_map || this.is_load_info;
      }
    }

    private bool is_chat_log_t
    {
      get
      {
        if (this.m_chat_log_t == null)
          return false;
        else
          return this.m_chat_log_t.IsAlive;
      }
    }

    private bool is_share
    {
      get
      {
        if (this.m_share_t == null)
          return false;
        else
          return this.m_share_t.IsAlive;
      }
    }

    private bool is_run_thread
    {
      get
      {
        return this.is_load || this.is_share || this.is_chat_log_t;
      }
    }

    private Rectangle main_window_crect
    {
      get
      {
        return this.ClientRectangle;
      }
    }

    private bool is_show_menu_strip
    {
      get
      {
        return this.contextMenuStrip1.Visible || this.contextMenuStrip2.Visible || this.contextMenuStrip3.Visible;
      }
    }

    public string device_info_string
    {
      get
      {
        return this.m_device_info_string;
      }
    }

    public gvtrademap_cs_form()
    {
      this.InitializeComponent();
      this.Text = "大航海時代Online 交易MAP C# ver.1.32.3";
      Useful.SetFontMeiryo((Form) this, 8f);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
      {
        this.finish_all_threads();
        if (this.m_db != null)
          this.m_db.Dispose();
        if (this.m_lib != null)
          this.m_lib.Dispose();
        this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (gvtrademap_cs_form));
      this.openpathlogToolStripMenuItem = new ToolStripMenuItem();
      this.openpathmailToolStripMenuItem = new ToolStripMenuItem();
      this.openpathscreenshotToolStripMenuItem = new ToolStripMenuItem();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.ToolStripMenuItem_country0 = new ToolStripMenuItem();
      this.ToolStripMenuItem_country1 = new ToolStripMenuItem();
      this.ToolStripMenuItem_country2 = new ToolStripMenuItem();
      this.ToolStripMenuItem_country3 = new ToolStripMenuItem();
      this.ToolStripMenuItem_country4 = new ToolStripMenuItem();
      this.ToolStripMenuItem_country5 = new ToolStripMenuItem();
      this.ToolStripMenuItem_country6 = new ToolStripMenuItem();
      this.ToolStripMenuItem_country00 = new ToolStripMenuItem();
      this.textBox1 = new TextBox();
      this.contextMenuStrip2 = new ContextMenuStrip(this.components);
      this.set_target_memo_icon_ToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.add_memo_icon_ToolStripMenuItem = new ToolStripMenuItem();
      this.edit_memo_icon_ToolStripMenuItem = new ToolStripMenuItem();
      this.remove_memo_icon_ToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.remove_all_target_memo_icon_ToolStripMenuItem = new ToolStripMenuItem();
      this.remove_all_memo_icon_ToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator5 = new ToolStripSeparator();
      this.openpathscreenshot2ToolStripMenuItem = new ToolStripMenuItem();
      this.setseaareastateToolStripMenuItem = new ToolStripMenuItem();
      this.exexgvoacToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator6 = new ToolStripSeparator();
      this.normal_sea_area_ToolStripMenuItem = new ToolStripMenuItem();
      this.safty_sea_area_ToolStripMenuItem = new ToolStripMenuItem();
      this.lawless_sea_area_ToolStripMenuItem = new ToolStripMenuItem();
      this.contextMenuStrip3 = new ContextMenuStrip(this.components);
      this.spotToolStripMenuItem1 = new ToolStripMenuItem();
      this.clear_spotToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator4 = new ToolStripSeparator();
      this.open_recipe_wiki0_ToolStripMenuItem = new ToolStripMenuItem();
      this.open_recipe_wiki1_ToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.copy_name_to_clipboardToolStripMenuItem = new ToolStripMenuItem();
      this.copy_all_to_clipboardToolStripMenuItem = new ToolStripMenuItem();
      this.listView1 = new ListView();
      this.changeBorderStyleToolStripMenuItem = new ToolStripMenuItem();
      this.closeFormToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator7 = new ToolStripSeparator();
      ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
      this.contextMenuStrip1.SuspendLayout();
      this.contextMenuStrip2.SuspendLayout();
      this.contextMenuStrip3.SuspendLayout();
      this.SuspendLayout();
      toolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.openpathlogToolStripMenuItem,
        (ToolStripItem) this.openpathmailToolStripMenuItem,
        (ToolStripItem) this.openpathscreenshotToolStripMenuItem
      });
      toolStripMenuItem.Name = "openpathToolStripMenuItem";
      toolStripMenuItem.Size = new Size(328, 22);
      toolStripMenuItem.Text = "大航海時代Onlineのフォルダを開く";
      this.openpathlogToolStripMenuItem.Name = "openpathlogToolStripMenuItem";
      this.openpathlogToolStripMenuItem.Size = new Size(280, 22);
      this.openpathlogToolStripMenuItem.Text = "ログフォルダを開く...";
      this.openpathmailToolStripMenuItem.Name = "openpathmailToolStripMenuItem";
      this.openpathmailToolStripMenuItem.Size = new Size(280, 22);
      this.openpathmailToolStripMenuItem.Text = "メ\x30FCルフォルダを開く...";
      this.openpathscreenshotToolStripMenuItem.Name = "openpathscreenshotToolStripMenuItem";
      this.openpathscreenshotToolStripMenuItem.Size = new Size(280, 22);
      this.openpathscreenshotToolStripMenuItem.Text = "スクリ\x30FCンショットフォルダを開く...";
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.ToolStripMenuItem_country0,
        (ToolStripItem) this.ToolStripMenuItem_country1,
        (ToolStripItem) this.ToolStripMenuItem_country2,
        (ToolStripItem) this.ToolStripMenuItem_country3,
        (ToolStripItem) this.ToolStripMenuItem_country4,
        (ToolStripItem) this.ToolStripMenuItem_country5,
        (ToolStripItem) this.ToolStripMenuItem_country6,
        (ToolStripItem) this.ToolStripMenuItem_country00
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(169, 180);
      this.ToolStripMenuItem_country0.Image = (Image) Resources.country01;
      this.ToolStripMenuItem_country0.ImageScaling = ToolStripItemImageScaling.None;
      this.ToolStripMenuItem_country0.Name = "ToolStripMenuItem_country0";
      this.ToolStripMenuItem_country0.Size = new Size(168, 22);
      this.ToolStripMenuItem_country0.Text = "イングランド";
      this.ToolStripMenuItem_country0.Click += new EventHandler(this.ToolStripMenuItem_country0_Click);
      this.ToolStripMenuItem_country1.Image = (Image) Resources.country1;
      this.ToolStripMenuItem_country1.ImageScaling = ToolStripItemImageScaling.None;
      this.ToolStripMenuItem_country1.Name = "ToolStripMenuItem_country1";
      this.ToolStripMenuItem_country1.Size = new Size(168, 22);
      this.ToolStripMenuItem_country1.Text = "イスパニア";
      this.ToolStripMenuItem_country1.Click += new EventHandler(this.ToolStripMenuItem_country1_Click);
      this.ToolStripMenuItem_country2.Image = (Image) Resources.country2;
      this.ToolStripMenuItem_country2.ImageScaling = ToolStripItemImageScaling.None;
      this.ToolStripMenuItem_country2.Name = "ToolStripMenuItem_country2";
      this.ToolStripMenuItem_country2.Size = new Size(168, 22);
      this.ToolStripMenuItem_country2.Text = "ポルトガル";
      this.ToolStripMenuItem_country2.Click += new EventHandler(this.ToolStripMenuItem_country2_Click);
      this.ToolStripMenuItem_country3.Image = (Image) Resources.country3;
      this.ToolStripMenuItem_country3.ImageScaling = ToolStripItemImageScaling.None;
      this.ToolStripMenuItem_country3.Name = "ToolStripMenuItem_country3";
      this.ToolStripMenuItem_country3.Size = new Size(168, 22);
      this.ToolStripMenuItem_country3.Text = "ネ\x30FCデルランド";
      this.ToolStripMenuItem_country3.Click += new EventHandler(this.ToolStripMenuItem_country3_Click);
      this.ToolStripMenuItem_country4.Image = (Image) Resources.country4;
      this.ToolStripMenuItem_country4.ImageScaling = ToolStripItemImageScaling.None;
      this.ToolStripMenuItem_country4.Name = "ToolStripMenuItem_country4";
      this.ToolStripMenuItem_country4.Size = new Size(168, 22);
      this.ToolStripMenuItem_country4.Text = "フランス";
      this.ToolStripMenuItem_country4.Click += new EventHandler(this.ToolStripMenuItem_country4_Click);
      this.ToolStripMenuItem_country5.Image = (Image) Resources.country5;
      this.ToolStripMenuItem_country5.ImageScaling = ToolStripItemImageScaling.None;
      this.ToolStripMenuItem_country5.Name = "ToolStripMenuItem_country5";
      this.ToolStripMenuItem_country5.Size = new Size(168, 22);
      this.ToolStripMenuItem_country5.Text = "ヴェネツィア";
      this.ToolStripMenuItem_country5.Click += new EventHandler(this.ToolStripMenuItem_country5_Click);
      this.ToolStripMenuItem_country6.Image = (Image) Resources.country6;
      this.ToolStripMenuItem_country6.ImageScaling = ToolStripItemImageScaling.None;
      this.ToolStripMenuItem_country6.Name = "ToolStripMenuItem_country6";
      this.ToolStripMenuItem_country6.Size = new Size(168, 22);
      this.ToolStripMenuItem_country6.Text = "オスマントルコ";
      this.ToolStripMenuItem_country6.Click += new EventHandler(this.ToolStripMenuItem_country6_Click);
      this.ToolStripMenuItem_country00.Image = (Image) Resources.country00;
      this.ToolStripMenuItem_country00.ImageScaling = ToolStripItemImageScaling.None;
      this.ToolStripMenuItem_country00.Name = "ToolStripMenuItem_country00";
      this.ToolStripMenuItem_country00.Size = new Size(168, 22);
      this.ToolStripMenuItem_country00.Text = "所属無";
      this.ToolStripMenuItem_country00.Click += new EventHandler(this.ToolStripMenuItem_country00_Click);
      this.textBox1.BorderStyle = BorderStyle.FixedSingle;
      this.textBox1.Location = new Point(12, 12);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ScrollBars = ScrollBars.Vertical;
      this.textBox1.Size = new Size(317, 191);
      this.textBox1.TabIndex = 1;
      this.textBox1.TabStop = false;
      this.textBox1.Visible = false;
      this.contextMenuStrip2.Items.AddRange(new ToolStripItem[15]
      {
        (ToolStripItem) this.set_target_memo_icon_ToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.add_memo_icon_ToolStripMenuItem,
        (ToolStripItem) this.edit_memo_icon_ToolStripMenuItem,
        (ToolStripItem) this.remove_memo_icon_ToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.remove_all_target_memo_icon_ToolStripMenuItem,
        (ToolStripItem) this.remove_all_memo_icon_ToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator5,
        (ToolStripItem) this.openpathscreenshot2ToolStripMenuItem,
        (ToolStripItem) this.setseaareastateToolStripMenuItem,
        (ToolStripItem) toolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator7,
        (ToolStripItem) this.changeBorderStyleToolStripMenuItem,
        (ToolStripItem) this.closeFormToolStripMenuItem
      });
      this.contextMenuStrip2.Name = "contextMenuStrip2";
      this.contextMenuStrip2.Size = new Size(329, 292);
      this.set_target_memo_icon_ToolStripMenuItem.Image = (Image) Resources.memo_icon21;
      this.set_target_memo_icon_ToolStripMenuItem.ImageTransparentColor = Color.FromArgb(13, 111, 161);
      this.set_target_memo_icon_ToolStripMenuItem.Name = "set_target_memo_icon_ToolStripMenuItem";
      this.set_target_memo_icon_ToolStripMenuItem.Size = new Size(328, 22);
      this.set_target_memo_icon_ToolStripMenuItem.Text = "目的地メモアイコンをここに追加";
      this.set_target_memo_icon_ToolStripMenuItem.Click += new EventHandler(this.set_target_memo_icon_ToolStripMenuItem_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(325, 6);
      this.add_memo_icon_ToolStripMenuItem.Image = (Image) Resources.memo_icon15;
      this.add_memo_icon_ToolStripMenuItem.ImageTransparentColor = Color.FromArgb(13, 111, 161);
      this.add_memo_icon_ToolStripMenuItem.Name = "add_memo_icon_ToolStripMenuItem";
      this.add_memo_icon_ToolStripMenuItem.Size = new Size(328, 22);
      this.add_memo_icon_ToolStripMenuItem.Text = "メモアイコンを追加...";
      this.add_memo_icon_ToolStripMenuItem.Click += new EventHandler(this.add_memo_icon_ToolStripMenuItem_Click);
      this.edit_memo_icon_ToolStripMenuItem.Name = "edit_memo_icon_ToolStripMenuItem";
      this.edit_memo_icon_ToolStripMenuItem.Size = new Size(328, 22);
      this.edit_memo_icon_ToolStripMenuItem.Text = "メモアイコンを編集...";
      this.edit_memo_icon_ToolStripMenuItem.Click += new EventHandler(this.edit_memo_icon_ToolStripMenuItem_Click);
      this.remove_memo_icon_ToolStripMenuItem.Name = "remove_memo_icon_ToolStripMenuItem";
      this.remove_memo_icon_ToolStripMenuItem.Size = new Size(328, 22);
      this.remove_memo_icon_ToolStripMenuItem.Text = "メモアイコンを削除";
      this.remove_memo_icon_ToolStripMenuItem.Click += new EventHandler(this.remove_memo_icon_ToolStripMenuItem_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(325, 6);
      this.remove_all_target_memo_icon_ToolStripMenuItem.Name = "remove_all_target_memo_icon_ToolStripMenuItem";
      this.remove_all_target_memo_icon_ToolStripMenuItem.Size = new Size(328, 22);
      this.remove_all_target_memo_icon_ToolStripMenuItem.Text = "全ての目的地メモアイコンを削除";
      this.remove_all_target_memo_icon_ToolStripMenuItem.Click += new EventHandler(this.remove_all_target_memo_icon_ToolStripMenuItem_Click);
      this.remove_all_memo_icon_ToolStripMenuItem.Name = "remove_all_memo_icon_ToolStripMenuItem";
      this.remove_all_memo_icon_ToolStripMenuItem.Size = new Size(328, 22);
      this.remove_all_memo_icon_ToolStripMenuItem.Text = "全てのメモアイコンを削除";
      this.remove_all_memo_icon_ToolStripMenuItem.Click += new EventHandler(this.remove_all_memo_icon_ToolStripMenuItem_Click);
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new Size(325, 6);
      this.openpathscreenshot2ToolStripMenuItem.Name = "openpathscreenshot2ToolStripMenuItem";
      this.openpathscreenshot2ToolStripMenuItem.Size = new Size(328, 22);
      this.openpathscreenshot2ToolStripMenuItem.Text = "航路図のスクリ\x30FCンショットフォルダを開く...";
      this.setseaareastateToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.exexgvoacToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator6,
        (ToolStripItem) this.normal_sea_area_ToolStripMenuItem,
        (ToolStripItem) this.safty_sea_area_ToolStripMenuItem,
        (ToolStripItem) this.lawless_sea_area_ToolStripMenuItem
      });
      this.setseaareastateToolStripMenuItem.Name = "setseaareastateToolStripMenuItem";
      this.setseaareastateToolStripMenuItem.Size = new Size(328, 22);
      this.setseaareastateToolStripMenuItem.Text = "海域変動システムの設定";
      this.exexgvoacToolStripMenuItem.Name = "exexgvoacToolStripMenuItem";
      this.exexgvoacToolStripMenuItem.Size = new Size(220, 22);
      this.exexgvoacToolStripMenuItem.Text = "海域変動収集を起動する...";
      this.toolStripSeparator6.Name = "toolStripSeparator6";
      this.toolStripSeparator6.Size = new Size(217, 6);
      this.normal_sea_area_ToolStripMenuItem.Name = "normal_sea_area_ToolStripMenuItem";
      this.normal_sea_area_ToolStripMenuItem.Size = new Size(220, 22);
      this.normal_sea_area_ToolStripMenuItem.Text = "を危険海域に設定する";
      this.normal_sea_area_ToolStripMenuItem.Click += new EventHandler(this.normal_sea_area_ToolStripMenuItem_Click);
      this.safty_sea_area_ToolStripMenuItem.Name = "safty_sea_area_ToolStripMenuItem";
      this.safty_sea_area_ToolStripMenuItem.Size = new Size(220, 22);
      this.safty_sea_area_ToolStripMenuItem.Text = "を安全海域に設定する";
      this.safty_sea_area_ToolStripMenuItem.Click += new EventHandler(this.safty_sea_area_ToolStripMenuItem_Click);
      this.lawless_sea_area_ToolStripMenuItem.Name = "lawless_sea_area_ToolStripMenuItem";
      this.lawless_sea_area_ToolStripMenuItem.Size = new Size(220, 22);
      this.lawless_sea_area_ToolStripMenuItem.Text = "を無法海域に設定する";
      this.lawless_sea_area_ToolStripMenuItem.Click += new EventHandler(this.lawless_sea_area_ToolStripMenuItem_Click);
      this.contextMenuStrip3.Items.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.spotToolStripMenuItem1,
        (ToolStripItem) this.clear_spotToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator4,
        (ToolStripItem) this.open_recipe_wiki0_ToolStripMenuItem,
        (ToolStripItem) this.open_recipe_wiki1_ToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.copy_name_to_clipboardToolStripMenuItem,
        (ToolStripItem) this.copy_all_to_clipboardToolStripMenuItem
      });
      this.contextMenuStrip3.Name = "contextMenuStrip3";
      this.contextMenuStrip3.Size = new Size(352, 148);
      this.spotToolStripMenuItem1.Name = "spotToolStripMenuItem1";
      this.spotToolStripMenuItem1.Size = new Size(351, 22);
      this.spotToolStripMenuItem1.Text = "スポット表示";
      this.spotToolStripMenuItem1.Click += new EventHandler(this.spotToolStripMenuItem1_Click);
      this.clear_spotToolStripMenuItem.Name = "clear_spotToolStripMenuItem";
      this.clear_spotToolStripMenuItem.Size = new Size(351, 22);
      this.clear_spotToolStripMenuItem.Text = "スポット表示解除";
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new Size(348, 6);
      this.open_recipe_wiki0_ToolStripMenuItem.Name = "open_recipe_wiki0_ToolStripMenuItem";
      this.open_recipe_wiki0_ToolStripMenuItem.Size = new Size(351, 22);
      this.open_recipe_wiki0_ToolStripMenuItem.Text = "レシピの詳細をレシピ情報wikiで調べる";
      this.open_recipe_wiki0_ToolStripMenuItem.Click += new EventHandler(this.open_recipe_wiki0_toolStripMenuItem_Click);
      this.open_recipe_wiki1_ToolStripMenuItem.Name = "open_recipe_wiki1_ToolStripMenuItem";
      this.open_recipe_wiki1_ToolStripMenuItem.Size = new Size(351, 22);
      this.open_recipe_wiki1_ToolStripMenuItem.Text = "レシピで作成可能かどうかレシピ情報wikiで調べる";
      this.open_recipe_wiki1_ToolStripMenuItem.Click += new EventHandler(this.open_recipe_wiki1_ToolStripMenuItem_Click);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new Size(348, 6);
      this.copy_name_to_clipboardToolStripMenuItem.Name = "copy_name_to_clipboardToolStripMenuItem";
      this.copy_name_to_clipboardToolStripMenuItem.Size = new Size(351, 22);
      this.copy_name_to_clipboardToolStripMenuItem.Text = "名称をクリップボ\x30FCドにコピ\x30FC";
      this.copy_name_to_clipboardToolStripMenuItem.Click += new EventHandler(this.copy_name_to_clipboardToolStripMenuItem_Click);
      this.copy_all_to_clipboardToolStripMenuItem.Name = "copy_all_to_clipboardToolStripMenuItem";
      this.copy_all_to_clipboardToolStripMenuItem.Size = new Size(351, 22);
      this.copy_all_to_clipboardToolStripMenuItem.Text = "詳細をクリップボ\x30FCドにコピ\x30FC";
      this.copy_all_to_clipboardToolStripMenuItem.Click += new EventHandler(this.copy_all_to_clipboardToolStripMenuItem_Click);
      this.listView1.BorderStyle = BorderStyle.FixedSingle;
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.listView1.HideSelection = false;
      this.listView1.Location = new Point(12, 209);
      this.listView1.MultiSelect = false;
      this.listView1.Name = "listView1";
      this.listView1.ShowItemToolTips = true;
      this.listView1.Size = new Size(317, 102);
      this.listView1.TabIndex = 3;
      this.listView1.TabStop = false;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.listView1.Visible = false;
      this.listView1.MouseClick += new MouseEventHandler(this.listView1_MouseClick);
      this.changeBorderStyleToolStripMenuItem.Name = "changeBorderStyleToolStripMenuItem";
      this.changeBorderStyleToolStripMenuItem.Size = new Size(328, 22);
      this.changeBorderStyleToolStripMenuItem.Text = "ウインドウ枠の表示/非表示";
      this.closeFormToolStripMenuItem.Name = "closeFormToolStripMenuItem";
      this.closeFormToolStripMenuItem.Size = new Size(328, 22);
      this.closeFormToolStripMenuItem.Text = "交易MAP C#を閉じる";
      this.toolStripSeparator7.Name = "toolStripSeparator7";
      this.toolStripSeparator7.Size = new Size(325, 6);
      this.AllowDrop = true;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Silver;
      this.ClientSize = new Size(592, 323);
      this.Controls.Add((Control) this.listView1);
      this.Controls.Add((Control) this.textBox1);
      this.Cursor = Cursors.Default;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MinimumSize = new Size(300, 256);
      this.Name = "gvtrademap_cs_form";
      this.StartPosition = FormStartPosition.Manual;
      this.Text = "Form1";
      this.MouseWheel += new MouseEventHandler(this.FormMouseWheel);
      this.MouseUp += new MouseEventHandler(this.MainWindowMouseUp);
      this.MouseDoubleClick += new MouseEventHandler(this.m_main_window_MouseDoubleClick);
      this.Paint += new PaintEventHandler(this.m_main_window_Paint);
      this.MouseClick += new MouseEventHandler(this.gvtrademap_cs_form_MouseClick);
      this.DragDrop += new DragEventHandler(this.gvtrademap_cs_form_DragDrop);
      this.FormClosed += new FormClosedEventHandler(this.gvtrademap_cs_form_FormClosed);
      this.MouseDown += new MouseEventHandler(this.MainWindowMouseDown);
      this.DragEnter += new DragEventHandler(this.gvtrademap_cs_form_DragEnter);
      this.Move += new EventHandler(this.gvtrademap_cs_form_Move);
      this.Resize += new EventHandler(this.gvtrademap_cs_form_Resize);
      this.MouseMove += new MouseEventHandler(this.MainWindowMouseMove);
      this.KeyDown += new KeyEventHandler(this.gvtrademap_cs_form_KeyDown);
      this.contextMenuStrip1.ResumeLayout(false);
      this.contextMenuStrip2.ResumeLayout(false);
      this.contextMenuStrip3.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public bool Initialize()
    {
      this.m_current_path = Path.GetDirectoryName(Application.ExecutablePath);
      Environment.CurrentDirectory = this.m_current_path;
      file_ctrl.CreatePath("map\\");
      file_ctrl.CreatePath("temp\\");
      file_ctrl.CreatePath("SS\\");
      file_ctrl.CreatePath("memo\\");
      this.m_old_mouse_pos = new Point(0, 0);
      this.m_mouse_move = new Point(0, 0);
      this.m_select_pos = new Point(0, 0);
      this.m_memo_icon_pos = new Point(0, 0);
      this.m_memo_icon_data = (map_mark.data) null;
      this.m_exit_thread_event = new ManualResetEvent(false);
      this.m_lib = new gvt_lib((Form) this, Path.Combine(this.m_current_path, "gvtrademap_cs.ini"));
      this.m_device_info_string = this.m_lib.device.deviec_info_string_short;
      this.m_lib.KeyAssignManager.OnProcessCmdKey += new OnProcessCmdKey(this.process_cnd_key);
      this.m_lib.KeyAssignManager.OnUpdateAssignList += new EventHandler(this.update_menu_shortcut);
      this.init_menu_tag();
      this.m_lib.IniManager.Load();
      this.update_menu_shortcut((object) null, EventArgs.Empty);
      this.m_db = new GvoDatabase(this.m_lib);
      this.m_myship_info = new myship_info(this.m_lib, this.m_db);
      this._LoadInfosStatus = new LoadInfosStatus();
      this.m_spot = new spot(this.m_lib, this.m_db.World);
      this.m_windows = new d3d_windows(this.m_lib.device);
      this.m_item_window = new item_window(this.m_lib, this.m_db, this.m_spot, this.textBox1, this.listView1, this);
      this.m_setting_window = new setting_window(this.m_lib, this.m_db, this);
      this.m_windows.Add((d3d_windows.window) this.m_setting_window);
      this.m_windows.Add((d3d_windows.window) this.m_item_window);
      this.m_info_windows = new info_windows(this.m_lib, this.m_db, this.m_myship_info);
      this.m_mouse_hook = (globalmouse_hook) null;
      this.m_tooltip = new ToolTip();
      this.m_tooltip.SetToolTip((Control) this.listView1, "おまじない");
      this.m_tooltip.AutoPopDelay = 30000;
      this.m_tooltip.BackColor = Color.LightYellow;
      this.m_show_tooltip = false;
      this.m_tooltip_old_mouse_pos = new Point(0, 0);
      this.init_setting();
      this.m_find_form = new find_form2(this.m_lib, this.m_db, this.m_spot, this.m_item_window);
      this.m_find_form.Location = this.m_lib.setting.find_window_location;
      this.m_find_form.Size = this.m_lib.setting.find_window_size;
      this.m_req_show_find_form = new RequestCtrl();
      this.m_lib.setting.req_centering_gpos.IsRequest();
      this.m_sea_routes_form = new sea_routes_form2(this.m_lib, this.m_db);
      this.m_sea_routes_form.Location = this.m_lib.setting.sea_routes_window_location;
      this.m_sea_routes_form.Size = this.m_lib.setting.sea_routes_window_size;
      this.m_req_sea_routes_form = new RequestCtrl();
      this.m_map_index = MapIndex.Max;
      this.m_use_mixed_map = this.m_lib.setting.use_mixed_map;
      this.load_map();
      this.m_share_timer = new System.Windows.Forms.Timer();
      this.m_share_timer.Interval = 60000;
      this.m_share_timer.Tick += new EventHandler(this.share_timer_Tick);
      this.m_share_timer.Start();
      this.m_load_info_t = new Thread(new ThreadStart(this.load_info_proc));
      this.m_load_info_t.Name = "load info";
      this.m_load_info_t.Start();
      this.m_chat_log_t = new Thread(new ThreadStart(this.chat_log_proc));
      this.m_chat_log_t.Name = "analize chat log";
      this.m_chat_log_t.Start();
      this.m_pause = false;
      return true;
    }

    private void init_setting()
    {
      Size windowSize = this.m_lib.setting.window_size;
      this.Location = this.m_lib.setting.window_location;
      this.Size = windowSize;
      if (this.m_lib.setting.is_border_style_none)
        this.ExecFunction(KeyFunction.window_change_border_style);
      this.m_item_window.info = this.m_db.World.FindInfo(this.m_lib.setting.select_info);
      this.m_item_window.EnableItemWindow(false);
      this.m_lib.loop_image.OffsetPosition = new Vector2(this.m_lib.setting.map_pos_x, this.m_lib.setting.map_pos_y);
      this.m_lib.loop_image.SetScale(this.m_lib.setting.map_scale, new Point(0, 0), false);
      if (!this.m_lib.setting.is_item_window_normal_size)
        this.m_item_window.window_mode = d3d_windows.window.mode.small;
      if (this.m_lib.setting.is_setting_window_normal_size)
        return;
      this.m_setting_window.window_mode = d3d_windows.window.mode.small;
    }

    private void init_menu_tag()
    {
      this.m_lib.KeyAssignManager.BindTagForMenuItem(this.exexgvoacToolStripMenuItem, (object) KeyFunction.setting_window_button_exec_gvoac);
      this.m_lib.KeyAssignManager.BindTagForMenuItem(this.openpathscreenshot2ToolStripMenuItem, (object) KeyFunction.folder_open_00);
      this.m_lib.KeyAssignManager.BindTagForMenuItem(this.openpathlogToolStripMenuItem, (object) KeyFunction.folder_open_01);
      this.m_lib.KeyAssignManager.BindTagForMenuItem(this.openpathmailToolStripMenuItem, (object) KeyFunction.folder_open_02);
      this.m_lib.KeyAssignManager.BindTagForMenuItem(this.openpathscreenshotToolStripMenuItem, (object) KeyFunction.folder_open_03);
      this.m_lib.KeyAssignManager.BindTagForMenuItem(this.changeBorderStyleToolStripMenuItem, (object) KeyFunction.window_change_border_style);
      this.m_lib.KeyAssignManager.BindTagForMenuItem(this.closeFormToolStripMenuItem, (object) KeyFunction.window_close);
      this.m_lib.KeyAssignManager.BindTagForMenuItem(this.clear_spotToolStripMenuItem, (object) KeyFunction.cancel_spot);
    }

    private void update_menu_shortcut(object sender, EventArgs e)
    {
      this.m_lib.KeyAssignManager.UpdateMenuShortcutKeys(this.contextMenuStrip1);
      this.m_lib.KeyAssignManager.UpdateMenuShortcutKeys(this.contextMenuStrip2);
      this.m_lib.KeyAssignManager.UpdateMenuShortcutKeys(this.contextMenuStrip3);
    }

    private void gvtrademap_cs_form_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.finish_all_threads();
      this.dispose_mouse_hook();
      this.m_item_window.UpdateMemo();
      this.m_lib.setting.find_window_location = this.m_find_form.Location;
      this.m_lib.setting.find_window_size = this.m_find_form.Size;
      this.m_lib.setting.find_window_visible = this.m_find_form.Visible;
      this.m_lib.setting.sea_routes_window_location = this.m_sea_routes_form.Location;
      this.m_lib.setting.sea_routes_window_size = this.m_sea_routes_form.Size;
      this.m_lib.setting.sea_routes_window_visible = this.m_sea_routes_form.Visible;
      this.m_lib.setting.select_info = this.m_item_window.info == null ? "" : this.m_item_window.info.Name;
      this.m_lib.setting.map_pos_x = this.m_lib.loop_image.OffsetPosition.X;
      this.m_lib.setting.map_pos_y = this.m_lib.loop_image.OffsetPosition.Y;
      this.m_lib.setting.map_scale = this.m_lib.loop_image.ImageScale;
      this.m_lib.setting.is_item_window_normal_size = this.m_item_window.window_mode == d3d_windows.window.mode.normal;
      this.m_lib.setting.is_setting_window_normal_size = this.m_setting_window.window_mode == d3d_windows.window.mode.normal;
      this.m_lib.IniManager.Save();
      this.m_db.WriteSettings();
    }

    private void finish_all_threads()
    {
      if (this.m_exit_thread_event != null)
        this.m_exit_thread_event.Set();
      this.wait_finish_thread(this.m_load_map_t);
      this.wait_finish_thread(this.m_load_info_t);
      this.wait_finish_thread(this.m_share_t);
      this.wait_finish_thread(this.m_chat_log_t);
    }

    private void wait_finish_thread(Thread t)
    {
      if (t == null || !t.IsAlive)
        return;
      t.Join();
    }

    private void m_main_window_Paint(object sender, PaintEventArgs e)
    {
      this.m_lib.device.SetMustDrawFlag();
      this.update_main_window();
    }

    private void load_map()
    {
      if (this.is_load || this.m_map_index == this.m_lib.setting.map && this.m_use_mixed_map == this.m_lib.setting.use_mixed_map)
        return;
      this.m_load_map_t = new Thread(new ThreadStart(this.load_map_proc));
      this.m_load_map_t.Name = "load map";
      this.m_load_map_t.Start();
    }

    private void debug_transform_pos(int x, int y)
    {
      transform.game_pos2_map_pos(new Point(x, y), this.m_lib.loop_image);
    }

    public void update_main_window()
    {
      if (this.m_lib.device.device == (Device) null)
        return;
      if (this.TopMost != this.m_lib.setting.window_top_most)
        this.TopMost = this.m_lib.setting.window_top_most;
      this.load_map();
      if (this.is_load)
      {
        if (this.m_pause)
          return;
        this.update_main_window_load();
      }
      else
      {
        this.do_tasks();
        if (this.m_pause || !this.m_lib.device.IsNeedDraw())
          return;
        this.do_draw();
      }
    }

    private void do_tasks()
    {
      if (this.m_req_show_find_form.IsRequest())
      {
        this.show_find_dialog();
        this.Activate();
      }
      if (this.m_req_sea_routes_form.IsRequest())
      {
        this.show_sea_routes_dialog();
        this.Activate();
      }
      this.m_myship_info.Update();
      if (this.m_lib.setting.center_myship && this.m_myship_info.capture_sucess)
        this.centering_pos(transform.game_pos2_map_pos(this.m_myship_info.pos, this.m_lib.loop_image));
      this.do_spot_request();
      if (this.m_lib.setting.req_centering_gpos.IsRequest())
      {
        this.centering_pos(transform.game_pos2_map_pos(this.m_lib.setting.centering_gpos, this.m_lib.loop_image));
        this.m_select_pos = this.m_lib.setting.centering_gpos;
      }
      this.m_db.World.SetServerAndCountry(this.m_lib.setting.server, this.m_lib.setting.country);
      this.m_db.WebIcons.Update();
      this.m_lib.loop_image.AddOffset(new Vector2((float) this.m_mouse_move.X, (float) this.m_mouse_move.Y));
      this.m_windows.Update();
      this.update_tooltip();
      this.m_info_windows.Update(this.m_select_pos, this.m_old_mouse_pos);
      if (this.m_mouse_move.X != 0 || this.m_mouse_move.Y != 0)
        this.m_lib.device.SetMustDrawFlag();
      this.m_mouse_move = Point.Empty;
      this.do_mouse_hook();
      this.m_db.SeaArea.color = this.m_map_index != MapIndex.Map2 ? sea_area.color_type.type2 : sea_area.color_type.type1;
      this.m_db.SeaArea.Update();
      if (this.m_lib.setting.req_screen_shot.IsRequest())
        this.screen_shot();
      if (this.m_db.SeaRoute.req_update_list.IsRequest())
      {
        this.m_sea_routes_form.UpdateAllList();
        this.m_db.SeaRoute.req_redraw_list.IsRequest();
      }
      if (!this.m_db.SeaRoute.req_redraw_list.IsRequest())
        return;
      this.m_sea_routes_form.RedrawNewestSeaRoutes();
    }

    private void do_draw()
    {
      try
      {
        this.m_lib.device.Clear(Color.Black);
        if (!this.m_lib.device.Begin())
          return;
        this.draw_map();
        this.m_db.Draw();
        this.m_myship_info.Draw();
        this.m_spot.Draw();
        if (this.m_lib.setting.tude_interval != TudeInterval.None)
          latitude_longitude.DrawPoints(this.m_lib);
        this.m_windows.Draw();
        if (!this.m_lib.setting.is_server_mode)
          this.m_db.Capture.DrawCapturedTexture();
        this.m_info_windows.Draw();
        this.draw_frame();
      }
      catch (Exception ex)
      {
        Console.WriteLine("描画中の例外をキャッチ\n" + ex.StackTrace);
      }
      this.m_lib.device.End();
      if (this.m_lib.device.Present())
        return;
      int num = (int) MessageBox.Show("グラフィックドライバで内部エラ\x30FCが発生しました。\r\nこのエラ\x30FCからは復帰できません。\r\nアプリケ\x30FCションを終了します。");
      this.Close();
    }

    private void draw_map()
    {
      this.m_lib.loop_image.MergeImage(new LoopXImage.DrawHandler(this.m_db.DrawForMargeInfoNames), this.m_lib.setting.req_update_map.IsRequest());
      this.m_lib.loop_image.Draw();
      switch (this.m_lib.setting.tude_interval)
      {
        case TudeInterval.Interval1000:
          latitude_longitude.DrawLines(this.m_lib);
          break;
        case TudeInterval.Interval100:
          latitude_longitude.DrawLines100(this.m_lib);
          break;
      }
    }

    private void draw_frame()
    {
      if (this.m_lib.setting.windows_vista_aero)
        return;
      Vector2 clientSize = this.m_lib.device.client_size;
      --clientSize.X;
      --clientSize.Y;
      this.m_lib.device.DrawLineRect(new Vector3(0.0f, 0.0f, 0.0001f), clientSize, Color.Black.ToArgb());
    }

    private bool is_inside_mouse_cursor_main_window()
    {
      return new hittest(this.ClientRectangle).HitTest(this.PointToClient(Control.MousePosition));
    }

    public void update_main_window_load()
    {
      this.m_item_window.EnableMemoWindow(false);
      this.m_item_window.EnableItemWindow(false);
      this.m_mouse_move = new Point(0, 0);
      try
      {
        this.m_lib.device.Clear(Color.White);
        if (!this.m_lib.device.Begin())
          return;
        float size_x = (float) (this.main_window_crect.Width / 2);
        float y1 = (float) (this.main_window_crect.Height / 2) - 48f;
        float x = size_x - size_x / 2f;
        this.draw_progress("街詳細...", this._LoadInfosStatus.StatusMessage, x, y1, size_x, this._LoadInfosStatus.NowStep, this._LoadInfosStatus.MaxStep, Color.Tomato.ToArgb());
        float y2 = y1 + 32f;
        this.draw_progress("地図...", this.m_lib.loop_image.LoadStr, x, y2, size_x, this.m_lib.loop_image.LoadCurrent, this.m_lib.loop_image.LoadMax, Color.SkyBlue.ToArgb());
        float y3 = y2 + 32f;
        this.draw_progress("陸地マスク...", this.m_db.SeaArea.progress_info_str, x, y3, size_x, this.m_db.SeaArea.progress_current, this.m_db.SeaArea.progress_max, Color.SkyBlue.ToArgb());
        this.draw_frame();
        this.m_lib.device.End();
      }
      catch (Exception ex)
      {
        Console.WriteLine("描画中の例外をキャッチ(読み込み中)\n" + ex.StackTrace);
      }
      this.m_lib.device.Present();
    }

    private void draw_progress(string str, string str2, float x, float y, float size_x, int current, int max, int color)
    {
      this.m_lib.device.DrawText(font_type.normal, str, (int) x, (int) y, Color.Black);
      this.m_lib.device.DrawTextR(font_type.normal, string.Format("{0} {1}/{2}", (object) str2, (object) current, (object) max), (int) x + (int) size_x, (int) y, Color.Black);
      float num = max <= 0 ? 0.0f : (float) current / (float) max;
      this.m_lib.device.DrawFillRect(new Vector3(x, y + 16f, 0.1f), new Vector2(size_x * num, 8f), color);
      this.m_lib.device.DrawLineRect(new Vector3(x, y + 16f, 0.1f), new Vector2(size_x, 8f), Color.Black.ToArgb());
    }

    private void load_map_proc()
    {
      string[] strArray1 = new string[2]
      {
        "map\\worldmap_r.png",
        "map\\worldmap_c.png"
      };
      string[] strArray2 = new string[2]
      {
        "map\\worldmap_r_mix.png",
        "map\\worldmap_c_mix.png"
      };
      this.m_lib.loop_image.InitializeCreateImage();
      if (!this.m_db.SeaArea.IsLoadedMask)
        this.m_db.SeaArea.InitializeFromMaskInfo();
      this.m_map_index = this.m_lib.setting.map;
      this.m_use_mixed_map = this.m_lib.setting.use_mixed_map;
      if (this.m_use_mixed_map)
      {
        favoriteroute.MixMap(strArray1[(int) this.m_map_index], "map\\favoriteroute.bmp", strArray2[(int) this.m_map_index], ImageFormat.Png);
        if (File.Exists(strArray2[(int) this.m_map_index]))
          this.m_lib.loop_image.CreateImage(strArray2[(int) this.m_map_index]);
        else
          this.m_lib.loop_image.CreateImage(strArray1[(int) this.m_map_index]);
      }
      else
        this.m_lib.loop_image.CreateImage(strArray1[(int) this.m_map_index]);
      if (!this.m_db.SeaArea.IsLoadedMask)
        this.m_db.SeaArea.CreateFromMask("map\\worldmap_mask.png");
      this.m_lib.setting.req_update_map.Request();
      Thread.Sleep(100);
    }

    private void load_info_proc()
    {
      this._LoadInfosStatus.Start(3, "インタ\x30FCネットから同盟状況の取得");
      if (this.m_lib.setting.connect_network)
        this.m_db.World.DownloadDomains("temp\\domaininfo.xml");
      this._LoadInfosStatus.IncStep("街情報の読み込み");
      this.m_db.World.Load("database\\cityinfos.xml", "database\\domaininfo.xml", "temp\\domaininfo.xml");
      this.m_db.World.LinkItemDatabase(this.m_db.ItemDatabase);
      this._LoadInfosStatus.IncStep("Webアイコンの読み込み");
      this.m_db.WebIcons.Load("database\\webicons.txt");
      if (this.m_lib.setting.find_window_visible)
        this.m_req_show_find_form.Request();
      if (this.m_lib.setting.sea_routes_window_visible)
        this.m_req_sea_routes_form.Request();
      this._LoadInfosStatus.IncStep("完了");
      Thread.Sleep(100);
    }

    private void chat_log_proc()
    {
      int num = 0;
      bool flag = true;
      while (!this.m_exit_thread_event.WaitOne(0, false))
      {
        Thread.Sleep(200);
        num += 200;
        if (num > 5100)
        {
          num -= 5100;
          if (this.m_lib.setting.is_server_mode || !this.m_lib.setting.enable_analize_log_chat || !this.m_lib.setting.save_searoutes)
          {
            flag = false;
          }
          else
          {
            this.m_db.GvoChat.AnalyzeNewestChatLog();
            this.m_db.GvoChat.Request();
            if (!flag)
            {
              this.m_db.GvoChat.ResetAccident();
              this.m_db.GvoChat.ResetInterest();
              this.m_db.GvoChat.ResetBuildShip();
            }
            flag = true;
          }
        }
      }
    }

    private void share_proc()
    {
      if (!this.m_myship_info.is_analized_pos)
        this.m_db.ShareRoutes.Share(0, 0, ShareRoutes.State.outof_sea);
      else
        this.m_db.ShareRoutes.Share(this.m_myship_info.pos.X, this.m_myship_info.pos.Y, this.m_myship_info.is_in_the_sea ? ShareRoutes.State.in_the_sea : ShareRoutes.State.outof_sea);
    }

    private void share_timer_Tick(object sender, EventArgs e)
    {
      if (this.m_db.GvoSeason.UpdateSeason())
        this.m_lib.setting.req_update_map.Request();
      if (!this.m_lib.setting.enable_share_routes || !gvo_capture_base.IsFoundGvoWindow() || (this.is_share || this.m_exit_thread_event.WaitOne(0, false)))
        return;
      this.m_share_t = new Thread(new ThreadStart(this.share_proc));
      this.m_share_t.Name = "share network";
      this.m_share_t.Start();
    }

    private void MainWindowMouseDown(object sender, MouseEventArgs e)
    {
      Point point = new Point(e.X, e.Y);
      this.m_old_mouse_pos = point;
      this.m_lib.device.SetMustDrawFlag();
      if (!this.m_item_window.HitTest_ItemList(point))
        this.ActiveControl = (Control) null;
      if (this.m_windows.OnMouseDown(point, e.Button))
        this.Capture = false;
      else if (this.m_info_windows.HitTest(point))
      {
        this.Capture = false;
      }
      else
      {
        if (this.m_lib.setting.compatible_windows_rclick)
        {
          if ((e.Button & MouseButtons.Left) != MouseButtons.None || (e.Button & MouseButtons.Right) != MouseButtons.None)
          {
            this.m_select_pos = transform.client_pos2_game_pos(point, this.m_lib.loop_image);
            if (!this.m_spot.is_spot)
              this.m_item_window.info = this.m_db.World.FindInfo(this.m_lib.loop_image.MousePos2GlobalPos(point));
          }
        }
        else if ((e.Button & MouseButtons.Left) != MouseButtons.None)
        {
          this.m_select_pos = transform.client_pos2_game_pos(point, this.m_lib.loop_image);
          this.m_item_window.info = this.m_db.World.FindInfo(this.m_lib.loop_image.MousePos2GlobalPos(point));
        }
        this.Capture = true;
      }
    }

    private void gvtrademap_cs_form_MouseClick(object sender, MouseEventArgs e)
    {
      Point point = new Point(e.X, e.Y);
      this.m_old_mouse_pos = point;
      if (this.m_windows.OnMouseClick(point, e.Button) || this.m_info_windows.OnMouseClick(point, e.Button, (Form) this))
        return;
      if (this.m_lib.setting.compatible_windows_rclick)
      {
        if ((e.Button & MouseButtons.Right) == MouseButtons.None)
          return;
        this.main_window_context_menu(point);
      }
      else
      {
        if ((e.Button & MouseButtons.Right) == MouseButtons.None || ((int) user32.GetKeyState(17) & 32768) == 0)
          return;
        this.main_window_context_menu(point);
      }
    }

    private void MainWindowMouseUp(object sender, MouseEventArgs e)
    {
      this.Capture = false;
    }

    private void MainWindowMouseMove(object sender, MouseEventArgs e)
    {
      if (this.Capture)
      {
        this.m_mouse_move.X += e.X - this.m_old_mouse_pos.X;
        this.m_mouse_move.Y += e.Y - this.m_old_mouse_pos.Y;
      }
      this.m_old_mouse_pos.X = e.X;
      this.m_old_mouse_pos.Y = e.Y;
    }

    private void m_main_window_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      this.m_windows.OnMouseDoubleClick(new Point(e.X, e.Y), e.Button);
    }

    private void FormMouseWheel(object sender, MouseEventArgs e)
    {
      if (!this.is_inside_mouse_cursor_main_window())
        return;
      Point point = this.PointToClient(Control.MousePosition);
      if (this.m_windows.OnMouseWheel(point, e.Delta))
        return;
      this.zoom_map(e.Delta > 0, point);
    }

    private void gvtrademap_cs_form_Resize(object sender, EventArgs e)
    {
      if (this.m_lib == null || this.m_lib.setting == null)
        return;
      this.m_pause = this.WindowState == FormWindowState.Minimized || !this.Visible;
      this.update_windows_position();
    }

    private void gvtrademap_cs_form_Move(object sender, EventArgs e)
    {
      this.update_windows_position();
    }

    private void update_windows_position()
    {
      if (this.WindowState != FormWindowState.Normal || this.m_lib == null || this.m_lib.setting == null)
        return;
      this.m_lib.setting.window_location = this.Location;
      this.m_lib.setting.window_size = this.Size;
    }

    private void do_mouse_hook()
    {
      if (this.m_lib.setting.hook_mouse)
      {
        if (this.m_mouse_hook != null)
          return;
        this.m_mouse_hook = new globalmouse_hook();
      }
      else
        this.dispose_mouse_hook();
    }

    private void dispose_mouse_hook()
    {
      if (this.m_mouse_hook == null)
        return;
      this.m_mouse_hook.Dispose();
      this.m_mouse_hook = (globalmouse_hook) null;
    }

    private void update_tooltip()
    {
      Point point = new Point(Control.MousePosition.X - this.Location.X, Control.MousePosition.Y - this.Location.Y);
      if (this.m_show_tooltip)
      {
        if (this.is_show_menu_strip)
        {
          try
          {
            this.m_tooltip.Hide((IWin32Window) this);
          }
          catch
          {
          }
          this.m_show_tooltip = false;
          this.m_tooltip_interval = 0;
        }
        else
        {
          Vector2 vector2_1 = new Vector2((float) point.X, (float) point.Y);
          Vector2 vector2_2 = new Vector2((float) this.m_tooltip_old_mouse_pos.X, (float) this.m_tooltip_old_mouse_pos.Y);
          vector2_1 -= vector2_2;
          if ((double) vector2_1.LengthSq() >= 64.0)
          {
            try
            {
              this.m_tooltip.Hide((IWin32Window) this);
            }
            catch
            {
            }
            this.m_show_tooltip = false;
            this.m_tooltip_old_mouse_pos = point;
          }
          this.m_tooltip_interval = 0;
        }
      }
      else if (this.m_tooltip_old_mouse_pos == point)
      {
        if (this.is_show_menu_strip)
          this.m_tooltip_interval = 0;
        else if (!this.is_inside_mouse_cursor_main_window())
        {
          this.m_tooltip_old_mouse_pos = point;
          this.m_tooltip_interval = 0;
        }
        else
        {
          if (++this.m_tooltip_interval < 15)
            return;
          Point pos = this.PointToClient(Control.MousePosition);
          string text = this.m_windows.GetToolTipString(pos) ?? this.get_tooltip_string(pos);
          if (text != null)
          {
            try
            {
              this.m_tooltip.Show(text, (IWin32Window) this, point.X + 10, point.Y, 40000000);
            }
            catch
            {
            }
            this.m_show_tooltip = true;
          }
          else
            this.m_tooltip_interval = 15;
        }
      }
      else
      {
        this.m_tooltip_old_mouse_pos = point;
        this.m_tooltip_interval = 0;
      }
    }

    private string get_tooltip_string(Point pos)
    {
      string str = this.m_info_windows.OnToolTipString(pos);
      Point point = this.m_lib.loop_image.MousePos2GlobalPos(pos);
      if (str == null)
        str = this.m_spot.GetToolTipString(point);
      if (str == null)
        str = this.m_db.MapMark.GetToolTip(point);
      if (str == null)
      {
        GvoWorldInfo.Info info = this.m_db.World.FindInfo(point);
        if (info != null)
        {
          str = info.TooltipString;
          if (this.m_lib.setting.map_draw_names == MapDrawNames.Draw)
          {
            if (info.InfoType == GvoWorldInfo.InfoType.OutsideCity)
              return (string) null;
            if (info.InfoType == GvoWorldInfo.InfoType.Shore)
              return (string) null;
            if (info.InfoType == GvoWorldInfo.InfoType.Shore2)
              return (string) null;
          }
        }
      }
      return str;
    }

    private void screen_shot()
    {
      Point offset;
      Size size;
      this.m_db.SeaRoute.CalcScreenShotBoundingBox(out offset, out size);
      if (size.Width <= 0 || size.Height <= 0)
      {
        int num1 = (int) MessageBox.Show("航路情報がないため、SSを作成できません。", "報告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        Device device = this.m_lib.device.device;
        Surface surface1 = device.DepthStencilSurface;
        Surface backBuffer = device.GetBackBuffer(0, 0, BackBufferType.Mono);
        Surface surface2 = (Surface) null;
        Surface surface3 = (Surface) null;
        Surface surface4 = (Surface) null;
        Surface depthStencilSurface;
        try
        {
          surface2 = device.CreateRenderTarget(512, 512, Microsoft.DirectX.Direct3D.Format.R5G6B5, MultiSampleType.None, 0, false);
          surface3 = device.CreateOffscreenPlainSurface(512, 512, Microsoft.DirectX.Direct3D.Format.R5G6B5, Pool.SystemMemory);
          depthStencilSurface = device.CreateDepthStencilSurface(512, 512, DepthFormat.D16, MultiSampleType.None, 0, false);
        }
        catch
        {
          if ((Resource) surface2 != (Resource) null)
            surface2.Dispose();
          if ((Resource) surface3 != (Resource) null)
            surface3.Dispose();
          if ((Resource) surface4 != (Resource) null)
            surface4.Dispose();
          int num2 = (int) MessageBox.Show("SSの保存に失敗しました。\nSS情報の作成に失敗しました。", "報告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        this.Cursor = Cursors.WaitCursor;
        device.DepthStencilSurface = depthStencilSurface;
        device.SetRenderTarget(0, surface2);
        this.m_lib.loop_image.PushDrawParams();
        Vector2 vector2_1 = new Vector2((float) offset.X, (float) offset.Y);
        ushort[] image = new ushort[size.Width * size.Height];
        this.m_lib.loop_image.SetScale(1f, new Point(0, 0), false);
        Vector2 vector2_2 = new Vector2(0.0f, 0.0f);
        while ((double) vector2_2.Y < (double) size.Height)
        {
          vector2_2.X = 0.0f;
          while ((double) vector2_2.X < (double) size.Width)
          {
            this.m_lib.loop_image.OffsetPosition = -(vector2_1 + vector2_2);
            this.screen_shot_draw();
            SurfaceLoader.FromSurface(surface3, surface2, Microsoft.DirectX.Direct3D.Filter.None, 0);
            this.screen_shot_chain_image(image, size.Width, size.Height, size.Width, (int) vector2_2.X, (int) vector2_2.Y, surface3);
            vector2_2.X += 512f;
          }
          vector2_2.Y += 512f;
        }
        this.m_lib.loop_image.PopDrawParams();
        device.DepthStencilSurface = surface1;
        device.SetRenderTarget(0, backBuffer);
        this.m_lib.device.UpdateClientSize();
        surface2.Dispose();
        surface3.Dispose();
        depthStencilSurface.Dispose();
        surface1.Dispose();
        backBuffer.Dispose();
        ushort[] numArray;
        try
        {
          GCHandle gcHandle = GCHandle.Alloc((object) image, GCHandleType.Pinned);
          Bitmap bitmap = new Bitmap(size.Width, size.Height, size.Width * 2, PixelFormat.Format16bppRgb565, gcHandle.AddrOfPinnedObject());
          string str = "searoute" + DateTime.Now.ToString("yyyyMMddHHmmss");
          string filename;
          switch (this.m_lib.setting.ss_format)
          {
            case SSFormat.Png:
              filename = Path.Combine(this.m_current_path, "SS\\" + str + ".png");
              bitmap.Save(filename, ImageFormat.Png);
              break;
            case SSFormat.Jpeg:
              filename = Path.Combine(this.m_current_path, "SS\\" + str + ".jpg");
              bitmap.Save(filename, ImageFormat.Jpeg);
              break;
            default:
              filename = Path.Combine(this.m_current_path, "SS\\" + str + ".bmp");
              bitmap.Save(filename, ImageFormat.Bmp);
              break;
          }
          gcHandle.Free();
          bitmap.Dispose();
          numArray = (ushort[]) null;
          GC.Collect();
          this.Cursor = Cursors.Default;
          int num2 = (int) MessageBox.Show("スクリ\x30FCンショットを保存しました。\n" + filename, "報告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          Process.Start(Path.Combine(this.m_current_path, "SS\\"));
        }
        catch
        {
          this.Cursor = Cursors.Default;
          numArray = (ushort[]) null;
          GC.Collect();
          int num2 = (int) MessageBox.Show("スクリ\x30FCンショットの保存に失敗しました。\nファイルの書き出しに失敗しました。", "報告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    private void screen_shot_draw()
    {
      this.m_lib.device.Clear(Color.Black);
      if (!this.m_lib.device.Begin())
        return;
      this.draw_map();
      this.m_db.DrawForScreenShot();
      this.m_lib.device.End();
    }

    private void screen_shot_chain_image(ushort[] image, int size_x, int size_y, int stride, int offset_x, int offset_y, Surface offscreen)
    {
      if (offset_x >= size_x || offset_y >= size_y || offset_x + offscreen.Description.Width < 0 || offset_y + offscreen.Description.Height < 0)
        return;
      int pitch;
      ushort[,] numArray = (ushort[,]) offscreen.LockRectangle(typeof (ushort), LockFlags.ReadOnly, out pitch, new int[2]
      {
        offscreen.Description.Height,
        offscreen.Description.Width
      });
      int num1 = offscreen.Description.Width;
      int num2 = offscreen.Description.Height;
      if (offset_x + num1 > size_x)
        num1 -= offset_x + num1 - size_x;
      if (offset_y + num2 > size_y)
        num2 -= offset_y + num2 - size_y;
      int num3 = 0;
      int num4 = 0;
      if (offset_x < 0)
      {
        num3 = -offset_x;
        offset_x = 0;
        num1 -= num3;
      }
      if (offset_y < 0)
      {
        num4 = -offset_y;
        offset_y = 0;
        num2 -= num4;
      }
      int num5 = offset_y * stride + offset_x;
      for (int index1 = 0; index1 < num2; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
          image[num5 + index2] = numArray[num4 + index1, num3 + index2];
        num5 += stride;
      }
      offscreen.UnlockRectangle();
    }

    public void ShowChangeDomainsMenuStrip(Point pos)
    {
      this.contextMenuStrip1.Show((Control) this, pos);
    }

    private void ToolStripMenuItem_country0_Click(object sender, EventArgs e)
    {
      this.set_domain(GvoWorldInfo.Country.England);
    }

    private void ToolStripMenuItem_country1_Click(object sender, EventArgs e)
    {
      this.set_domain(GvoWorldInfo.Country.Spain);
    }

    private void ToolStripMenuItem_country2_Click(object sender, EventArgs e)
    {
      this.set_domain(GvoWorldInfo.Country.Portugal);
    }

    private void ToolStripMenuItem_country3_Click(object sender, EventArgs e)
    {
      this.set_domain(GvoWorldInfo.Country.Netherlands);
    }

    private void ToolStripMenuItem_country4_Click(object sender, EventArgs e)
    {
      this.set_domain(GvoWorldInfo.Country.France);
    }

    private void ToolStripMenuItem_country5_Click(object sender, EventArgs e)
    {
      this.set_domain(GvoWorldInfo.Country.Venezia);
    }

    private void ToolStripMenuItem_country6_Click(object sender, EventArgs e)
    {
      this.set_domain(GvoWorldInfo.Country.Turkey);
    }

    private void ToolStripMenuItem_country00_Click(object sender, EventArgs e)
    {
      this.set_domain(GvoWorldInfo.Country.Unknown);
    }

    private void set_domain(GvoWorldInfo.Country country)
    {
      if (this.m_item_window.info == null || !this.m_db.World.SetDomain(this.m_item_window.info.Name, country))
        return;
      string netUpdateString = this.m_db.World.GetNetUpdateString(this.m_item_window.info.Name);
      if (netUpdateString == null || !this.m_lib.setting.connect_network)
        return;
      Console.WriteLine("同盟国変更:" + HttpDownload.Download("http://gvtrademap.daa.jp/gvgetdomain.cgi?" + netUpdateString, Encoding.UTF8));
    }

    private void main_window_context_menu(Point p)
    {
      this.m_memo_icon_pos = this.m_lib.loop_image.MousePos2GlobalPos(p);
      this.m_memo_icon_data = this.m_db.MapMark.FindData(this.m_memo_icon_pos);
      if (this.m_memo_icon_data == null)
      {
        this.edit_memo_icon_ToolStripMenuItem.Enabled = false;
        this.remove_memo_icon_ToolStripMenuItem.Enabled = false;
      }
      else
      {
        this.edit_memo_icon_ToolStripMenuItem.Enabled = true;
        this.remove_memo_icon_ToolStripMenuItem.Enabled = true;
      }
      string str = this.m_db.SeaArea.Find(this.m_memo_icon_pos);
      if (str == null)
      {
        this.normal_sea_area_ToolStripMenuItem.Text = "--を危険海域(通常状態)に設定する";
        this.normal_sea_area_ToolStripMenuItem.Enabled = false;
        this.safty_sea_area_ToolStripMenuItem.Text = "--を安全海域に設定する";
        this.safty_sea_area_ToolStripMenuItem.Enabled = false;
        this.lawless_sea_area_ToolStripMenuItem.Text = "--を無法海域に設定する";
        this.lawless_sea_area_ToolStripMenuItem.Enabled = false;
      }
      else
      {
        this.normal_sea_area_ToolStripMenuItem.Text = str + "を危険海域(通常状態)に設定する";
        this.normal_sea_area_ToolStripMenuItem.Enabled = true;
        this.safty_sea_area_ToolStripMenuItem.Text = str + "を安全海域に設定する";
        this.safty_sea_area_ToolStripMenuItem.Enabled = true;
        this.lawless_sea_area_ToolStripMenuItem.Text = str + "を無法海域に設定する";
        this.lawless_sea_area_ToolStripMenuItem.Enabled = true;
      }
      this.contextMenuStrip2.Show((Control) this, p);
    }

    private void set_target_memo_icon_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.m_db.MapMark.Add(this.m_memo_icon_pos, map_mark.map_mark_type.icon11, "目的地周辺です");
      this.m_memo_icon_data = (map_mark.data) null;
      this.m_lib.setting.draw_icons = true;
    }

    private void add_memo_icon_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (map_mark_form mapMarkForm = new map_mark_form(transform.map_pos2_game_pos(this.m_memo_icon_pos, this.m_lib.loop_image)))
      {
        if (mapMarkForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          this.m_db.MapMark.Add(transform.game_pos2_map_pos(mapMarkForm.position, this.m_lib.loop_image), (map_mark.map_mark_type) mapMarkForm.icon_index, mapMarkForm.memo);
          this.m_lib.setting.draw_icons = true;
        }
      }
      this.m_memo_icon_data = (map_mark.data) null;
    }

    private void edit_memo_icon_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.m_memo_icon_data == null)
        return;
      using (map_mark_form mapMarkForm = new map_mark_form(this.m_memo_icon_data.gposition, (int) this.m_memo_icon_data.type, this.m_memo_icon_data.memo))
      {
        if (mapMarkForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          this.m_memo_icon_data.position = transform.game_pos2_map_pos(mapMarkForm.position, this.m_lib.loop_image);
          this.m_memo_icon_data.type = (map_mark.map_mark_type) mapMarkForm.icon_index;
          this.m_memo_icon_data.memo = mapMarkForm.memo;
          this.m_lib.setting.draw_icons = true;
        }
      }
      this.m_memo_icon_data = (map_mark.data) null;
    }

    private void remove_memo_icon_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.m_memo_icon_data == null)
        return;
      this.m_db.MapMark.RemoveData(this.m_memo_icon_data);
      this.m_memo_icon_data = (map_mark.data) null;
    }

    private void remove_all_target_memo_icon_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.m_db.MapMark.RemoveAllTargetData();
      this.m_memo_icon_data = (map_mark.data) null;
      this.m_lib.setting.draw_icons = true;
    }

    private void remove_all_memo_icon_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.m_db.MapMark.RemoveAllData();
      this.m_memo_icon_data = (map_mark.data) null;
    }

    private void normal_sea_area_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.set_sea_area_rclick(sea_area.sea_area_once.sea_type.normal);
    }

    private void safty_sea_area_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.set_sea_area_rclick(sea_area.sea_area_once.sea_type.safty);
    }

    private void lawless_sea_area_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.set_sea_area_rclick(sea_area.sea_area_once.sea_type.lawless);
    }

    private void set_sea_area_rclick(sea_area.sea_area_once.sea_type type)
    {
      this.m_db.SeaArea.SetType(this.m_db.SeaArea.Find(this.m_memo_icon_pos), type);
    }

    private void listView1_MouseClick(object sender, MouseEventArgs e)
    {
      if ((e.Button & MouseButtons.Right) == MouseButtons.None)
        return;
      GvoWorldInfo.Info.Group.Data selectedItem = this.get_selected_item();
      if (selectedItem == null)
        return;
      Point position = new Point(e.X, e.Y);
      ItemDatabase.Data itemDb = selectedItem.ItemDb;
      if (itemDb == null)
      {
        this.open_recipe_wiki0_ToolStripMenuItem.Enabled = false;
        this.open_recipe_wiki1_ToolStripMenuItem.Enabled = false;
        this.copy_all_to_clipboardToolStripMenuItem.Enabled = false;
      }
      else
      {
        this.copy_all_to_clipboardToolStripMenuItem.Enabled = true;
        if (itemDb.IsSkill || itemDb.IsReport)
        {
          this.open_recipe_wiki0_ToolStripMenuItem.Enabled = false;
          this.open_recipe_wiki1_ToolStripMenuItem.Enabled = false;
        }
        else if (itemDb.IsRecipe)
        {
          this.open_recipe_wiki0_ToolStripMenuItem.Enabled = true;
          this.open_recipe_wiki1_ToolStripMenuItem.Enabled = false;
        }
        else
        {
          this.open_recipe_wiki0_ToolStripMenuItem.Enabled = false;
          this.open_recipe_wiki1_ToolStripMenuItem.Enabled = true;
        }
      }
      this.contextMenuStrip3.Show((Control) this.listView1, position);
    }

    private void open_recipe_wiki0_toolStripMenuItem_Click(object sender, EventArgs e)
    {
      GvoWorldInfo.Info.Group.Data selectedItem = this.get_selected_item();
      if (selectedItem == null)
        return;
      ItemDatabase.Data itemDb = selectedItem.ItemDb;
      if (itemDb == null)
        return;
      itemDb.OpenRecipeWiki0();
    }

    private void open_recipe_wiki1_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      GvoWorldInfo.Info.Group.Data selectedItem = this.get_selected_item();
      if (selectedItem == null)
        return;
      ItemDatabase.Data itemDb = selectedItem.ItemDb;
      if (itemDb == null)
        return;
      itemDb.OpenRecipeWiki1();
    }

    private void copy_name_to_clipboardToolStripMenuItem_Click(object sender, EventArgs e)
    {
      GvoWorldInfo.Info.Group.Data selectedItem = this.get_selected_item();
      if (selectedItem == null)
        return;
      ItemDatabase.Data itemDb = selectedItem.ItemDb;
      if (itemDb == null)
        Clipboard.SetText(selectedItem.Name);
      else
        Clipboard.SetText(itemDb.Name);
    }

    private void copy_all_to_clipboardToolStripMenuItem_Click(object sender, EventArgs e)
    {
      GvoWorldInfo.Info.Group.Data selectedItem = this.get_selected_item();
      if (selectedItem == null)
        return;
      Clipboard.SetText(selectedItem.TooltipString);
    }

    private GvoWorldInfo.Info.Group.Data get_selected_item()
    {
      if (this.listView1.SelectedItems.Count <= 0)
        return (GvoWorldInfo.Info.Group.Data) null;
      ListViewItem listViewItem = this.listView1.SelectedItems[0];
      if (listViewItem.Tag == null)
        return (GvoWorldInfo.Info.Group.Data) null;
      else
        return (GvoWorldInfo.Info.Group.Data) listViewItem.Tag;
    }

    private void spotToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      GvoWorldInfo.Info.Group.Data selectedItem = this.get_selected_item();
      if (selectedItem == null)
        return;
      this.m_spot.SetSpot(spot.type.has_item, selectedItem.Name);
      this.UpdateSpotList();
    }

    private void reset_spot()
    {
      this.m_spot.SetSpot(spot.type.none, "");
      this.UpdateSpotList();
    }

    private void gvtrademap_cs_form_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.Text))
        e.Effect = DragDropEffects.Copy;
      else
        e.Effect = DragDropEffects.None;
    }

    private void gvtrademap_cs_form_DragDrop(object sender, DragEventArgs e)
    {
      this.AllowDrop = false;
      try
      {
        sea_area_dd_form seaAreaDdForm = new sea_area_dd_form(this.m_lib, this.m_db, sea_area.AnalizeFromDD(e.Data.GetData(DataFormats.Text) as string));
        if (seaAreaDdForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
          this.m_db.SeaArea.UpdateFromDD(seaAreaDdForm.filterd_list, true);
      }
      catch
      {
        int num = (int) MessageBox.Show("海域情報受け取りに失敗しました。");
      }
      this.AllowDrop = true;
    }

    private void do_spot_request()
    {
      if (this.m_lib.setting.req_spot_item.IsRequest())
      {
        this.m_item_window.SpotItem(this.m_lib.setting.req_spot_item.Arg1 as GvoDatabase.Find);
      }
      else
      {
        if (!this.m_lib.setting.req_spot_item_changed.IsRequest())
          return;
        this.m_item_window.SpotItemChanged(this.m_lib.setting.req_spot_item_changed.Arg1 as spot.spot_once);
      }
    }

    private void centering_pos(Point pos)
    {
      Point offset = new Point(0, 0);
      if (this.m_item_window.window_mode == d3d_windows.window.mode.normal)
        offset = new Point((int) ((double) this.m_item_window.pos.X + (double) this.m_item_window.size.X), 0);
      this.m_lib.loop_image.MoveCenterOffset(pos, offset);
      this.m_lib.device.SetMustDrawFlag();
    }

    private void gvtrademap_cs_form_KeyDown(object sender, KeyEventArgs e)
    {
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (this.ActiveControl != this.textBox1 && this.m_lib.KeyAssignManager.ProcessCmdKey(keyData))
        return true;
      else
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void process_cnd_key(object sender, KeyAssignEventArg arg)
    {
      if (!(arg.Tag is KeyFunction))
        return;
      this.ExecFunction((KeyFunction) arg.Tag);
    }

    public void ExecFunction(KeyFunction func)
    {
      switch (func)
      {
        case KeyFunction.map_change:
          if (this.is_load || ++this.m_lib.setting.map < MapIndex.Max)
            break;
          this.m_lib.setting.map = MapIndex.Map1;
          break;
        case KeyFunction.map_reset_scale:
          this.m_lib.loop_image.SetScale(1f, this.client_center(), true);
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.map_zoom_in:
          this.zoom_map(true, this.client_center());
          break;
        case KeyFunction.map_zoom_out:
          this.zoom_map(false, this.client_center());
          break;
        case KeyFunction.blue_line_reset:
          this.m_db.SpeedCalculator.ResetAngle();
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.cancel_spot:
          this.reset_spot();
          break;
        case KeyFunction.cancel_select_sea_routes:
          this.m_db.SeaRoute.ResetSelectFlag();
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.item_window_show_min:
          this.m_item_window.ToggleWindowMode();
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.item_window_next_tab:
          this.m_item_window.ChangeTab(true);
          break;
        case KeyFunction.item_window_prev_tab:
          this.m_item_window.ChangeTab(false);
          break;
        case KeyFunction.setting_window_show_min:
          this.m_setting_window.ToggleWindowMode();
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.setting_window_button_00:
          this.m_lib.setting.save_searoutes = !this.m_lib.setting.save_searoutes;
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.setting_window_button_01:
          this.m_lib.setting.draw_share_routes = !this.m_lib.setting.draw_share_routes;
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.setting_window_button_02:
          this.m_lib.setting.draw_web_icons = !this.m_lib.setting.draw_web_icons;
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.setting_window_button_03:
          this.m_lib.setting.draw_icons = !this.m_lib.setting.draw_icons;
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.setting_window_button_04:
          this.m_lib.setting.draw_sea_routes = !this.m_lib.setting.draw_sea_routes;
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.setting_window_button_05:
          this.m_lib.setting.draw_popup_day_interval = this.m_lib.setting.draw_popup_day_interval != 0 ? (this.m_lib.setting.draw_popup_day_interval != 1 ? 0 : 5) : 1;
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.setting_window_button_06:
          this.m_lib.setting.draw_accident = !this.m_lib.setting.draw_accident;
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.setting_window_button_07:
          this.m_lib.setting.center_myship = !this.m_lib.setting.center_myship;
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.setting_window_button_08:
          this.m_lib.setting.draw_myship_angle = !this.m_lib.setting.draw_myship_angle;
          this.m_lib.device.SetMustDrawFlag();
          break;
        case KeyFunction.setting_window_button_09:
          this.edit_sea_area_dlg();
          break;
        case KeyFunction.setting_window_button_10:
          this.m_lib.setting.req_screen_shot.Request();
          break;
        case KeyFunction.setting_window_button_11:
          this.show_sea_routes_dialog();
          break;
        case KeyFunction.setting_window_button_12:
          this.show_find_dialog();
          break;
        case KeyFunction.setting_window_button_13:
          this.do_setting_dlg();
          break;
        case KeyFunction.setting_window_button_exec_gvoac:
          try
          {
            Process.Start("gvoac.exe");
            break;
          }
          catch
          {
            int num = (int) MessageBox.Show("海域情報収集の起動に失敗しました。", "起動エラ\x30FC", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
        case KeyFunction.folder_open_00:
          Process.Start(Path.Combine(this.m_current_path, "SS\\"));
          break;
        case KeyFunction.folder_open_01:
          Process.Start(gvo_def.GetGvoLogPath());
          break;
        case KeyFunction.folder_open_02:
          Process.Start(gvo_def.GetGvoMailPath());
          break;
        case KeyFunction.folder_open_03:
          Process.Start(gvo_def.GetGvoScreenShotPath());
          break;
        case KeyFunction.window_top_most:
          this.m_lib.setting.window_top_most = !this.m_lib.setting.window_top_most;
          break;
        case KeyFunction.window_maximize:
          switch (this.WindowState)
          {
            case FormWindowState.Normal:
              this.WindowState = FormWindowState.Maximized;
              return;
            case FormWindowState.Minimized:
              this.WindowState = FormWindowState.Normal;
              return;
            case FormWindowState.Maximized:
              this.WindowState = FormWindowState.Normal;
              return;
            default:
              return;
          }
        case KeyFunction.window_minimize:
          if (this.WindowState == FormWindowState.Minimized)
            break;
          this.WindowState = FormWindowState.Minimized;
          break;
        case KeyFunction.window_change_border_style:
          Size size = this.Size;
          this.FormBorderStyle = this.FormBorderStyle == FormBorderStyle.None ? FormBorderStyle.Sizable : FormBorderStyle.None;
          this.Size = size;
          this.m_lib.setting.is_border_style_none = this.FormBorderStyle == FormBorderStyle.None;
          break;
        case KeyFunction.window_close:
          this.Close();
          break;
      }
    }

    private void zoom_map(bool is_zoom_in, Point center)
    {
      this.m_lib.loop_image.SetScale((float) Math.Round(!is_zoom_in ? (double) (this.m_lib.loop_image.ImageScale - 0.05f) : (double) (this.m_lib.loop_image.ImageScale + 0.05f), 2), center, true);
      this.m_lib.device.SetMustDrawFlag();
    }

    private Point client_center()
    {
      Rectangle clientRectangle = this.ClientRectangle;
      return new Point(clientRectangle.Width / 2, clientRectangle.Height / 2);
    }

    private void show_find_dialog()
    {
      this.show_find_dialog(true);
    }

    private void show_find_dialog(bool is_active_find_mode)
    {
      if (is_active_find_mode)
        this.m_find_form.SetFindMode();
      if (!this.m_find_form.Visible)
        this.m_find_form.Show((IWin32Window) this);
      else if (is_active_find_mode)
        this.m_find_form.Activate();
      this.m_lib.device.SetMustDrawFlag();
    }

    private void show_sea_routes_dialog()
    {
      if (!this.m_sea_routes_form.Visible)
        this.m_sea_routes_form.Show((IWin32Window) this);
      else
        this.m_sea_routes_form.Activate();
    }

    private void edit_sea_area_dlg()
    {
      using (setting_sea_area_form settingSeaAreaForm = new setting_sea_area_form(this.m_db.SeaArea))
      {
        if (settingSeaAreaForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        settingSeaAreaForm.Update(this.m_db.SeaArea);
      }
    }

    private void do_setting_dlg()
    {
      using (setting_form2 dlg = new setting_form2(this.m_lib.setting, this.m_lib.KeyAssignManager.List, this.m_lib.device.deviec_info_string))
      {
        if (dlg.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.UpdateSettings(dlg);
      }
    }

    public void UpdateSettings(setting_form2 dlg)
    {
      this.m_lib.setting.Clone(dlg.setting);
      this.m_lib.KeyAssignManager.List = dlg.KeyAssignList;
    }

    public void UpdateSpotList()
    {
      if (this.m_spot == null || this.m_find_form == null)
        return;
      if (this.m_spot.is_spot)
        this.show_find_dialog(false);
      this.m_find_form.UpdateSpotList();
      this.Activate();
      this.m_lib.device.SetMustDrawFlag();
    }

    public static void ActiveGVTradeMap()
    {
      IntPtr windowA = user32.FindWindowA((string) null, "大航海時代Online 交易MAP C# ver.1.32.3");
      if (windowA == IntPtr.Zero)
        return;
      user32.SetForegroundWindow(windowA);
    }
  }
}
