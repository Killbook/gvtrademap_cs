// Type: gvtrademap_cs.setting_form2
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Controls;
using gvtrademap_cs.Properties;
using net_base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Utility;
using Utility.KeyAssign;

namespace gvtrademap_cs
{
  public class setting_form2 : Form
  {
    private GlobalSettings m_setting;
    private KeyAssignList m_key_assign_list;
    private KeyAssiginSettingHelper m_key_assign_helper;
    private IContainer components;
    private PanelManager panelManager1;
    private ManagedPanel managedPanel1;
    private ManagedPanel managedPanel2;
    private ListBox listBox1;
    private ToolTip toolTip1;
    private GroupBox groupBox1;
    private GroupBox groupBox8;
    private ComboBox comboBox8;
    private GroupBox groupBox7;
    private ComboBox comboBox5;
    private GroupBox groupBox4;
    private ComboBox comboBox7;
    private ComboBox comboBox6;
    private ComboBox comboBox1;
    private GroupBox groupBox3;
    private ComboBox comboBox3;
    private ComboBox comboBox2;
    private GroupBox groupBox5;
    private ManagedPanel managedPanel3;
    private GroupBox groupBox9;
    private CheckBox checkBox1;
    private CheckBox checkBox9;
    private GroupBox groupBox6;
    private CheckBox checkBox6;
    private Label label4;
    private Label label3;
    private TextBox textBox2;
    private TextBox textBox1;
    private ManagedPanel managedPanel4;
    private GroupBox groupBox10;
    private ManagedPanel managedPanel5;
    private GroupBox groupBox11;
    private CheckBox checkBox16;
    private CheckBox checkBox13;
    private CheckBox checkBox2;
    private CheckBox checkBox12;
    private CheckBox checkBox10;
    private CheckBox checkBox11;
    private ManagedPanel managedPanel6;
    private GroupBox groupBox12;
    private GroupBox groupBox13;
    private TextBox textBox4;
    private GroupBox groupBox14;
    private TextBox textBox5;
    private TextBox textBox6;
    private TextBox textBox7;
    private GroupBox groupBox15;
    private CheckBox checkBox17;
    private GroupBox groupBox16;
    private ComboBox comboBox4;
    private CheckBox checkBox3;
    private CheckBox checkBox5;
    private RadioButton radioButton2;
    private RadioButton radioButton1;
    private GroupBox groupBox17;
    private Button button2;
    private Button button1;
    private GroupBox groupBox18;
    private Label label1;
    private LinkLabel linkLabel2;
    private LinkLabel linkLabel1;
    private Label label2;
    private Label label5;
    private PictureBox pictureBox1;
    private Button button4;
    private ImageList imageList1;
    private TabControl tabControl1;
    private TabPage tabPage1;
    private CheckBox checkBox105;
    private CheckBox checkBox104;
    private CheckBox checkBox103;
    private CheckBox checkBox102;
    private CheckBox checkBox101;
    private CheckBox checkBox100;
    private TabPage tabPage2;
    private CheckBox checkBox212;
    private CheckBox checkBox211;
    private CheckBox checkBox210;
    private CheckBox checkBox209;
    private CheckBox checkBox208;
    private CheckBox checkBox207;
    private CheckBox checkBox206;
    private CheckBox checkBox205;
    private CheckBox checkBox204;
    private CheckBox checkBox203;
    private CheckBox checkBox202;
    private CheckBox checkBox201;
    private CheckBox checkBox200;
    private TabPage tabPage3;
    private CheckBox checkBox310;
    private CheckBox checkBox309;
    private CheckBox checkBox308;
    private CheckBox checkBox307;
    private CheckBox checkBox306;
    private CheckBox checkBox305;
    private CheckBox checkBox304;
    private CheckBox checkBox303;
    private CheckBox checkBox302;
    private CheckBox checkBox301;
    private CheckBox checkBox300;
    private TabPage tabPage4;
    private CheckBox checkBox401;
    private CheckBox checkBox402;
    private CheckBox checkBox400;
    private CheckBox checkBox403;
    private CheckBox checkBox8;
    private ManagedPanel managedPanel7;
    private GroupBox groupBox2;
    private CheckBox checkBox14;
    private Label label6;
    private TextBox textBox3;
    private Label label7;
    private TextBox textBox8;
    private GroupBox groupBox20;
    private Label label8;
    private TextBox textBox9;
    private GroupBox groupBox19;
    private CheckBox checkBox18;
    private CheckBox checkBox19;
    private ManagedPanel managedPanel8;
    private GroupBox groupBox21;
    private ListView listView1;
    private Button button3;
    private ComboBox comboBox9;
    private Button button5;
    private Button button6;
    private CheckBox checkBox4;
    private CheckBox checkBox7;

    public GlobalSettings setting
    {
      get
      {
        return this.m_setting;
      }
    }

    public KeyAssignList KeyAssignList
    {
      get
      {
        return this.m_key_assign_list;
      }
    }

    public setting_form2(GlobalSettings _setting, KeyAssignList assign_list, string device_info)
    {
      this.init(_setting, assign_list, device_info, setting_form2.tab_index.general, DrawSettingPage.WebIcons);
    }

    public setting_form2(GlobalSettings _setting, KeyAssignList assign_list, string device_info, DrawSettingPage _draw_setting_page)
    {
      this.init(_setting, assign_list, device_info, setting_form2.tab_index.draw_flags, _draw_setting_page);
    }

    public setting_form2(GlobalSettings _setting, KeyAssignList assign_list, string device_info, setting_form2.tab_index _tab_index)
    {
      this.init(_setting, assign_list, device_info, _tab_index, DrawSettingPage.WebIcons);
    }

    private void init(GlobalSettings _setting, KeyAssignList assign_list, string device_info, setting_form2.tab_index index, DrawSettingPage page)
    {
      this.m_setting = _setting.Clone();
      this.m_key_assign_list = assign_list.DeepClone();
      this.InitializeComponent();
      Useful.SetFontMeiryo((Form) this, 8f);
      Useful.SetFontMeiryo((Control) this.listBox1, 9f);
      this.toolTip1.AutoPopDelay = 30000;
      this.toolTip1.BackColor = Color.LightYellow;
      this.toolTip1.SetToolTip((Control) this.comboBox2, "プレイしているサ\x30FCバを選択します");
      this.toolTip1.SetToolTip((Control) this.comboBox3, "属している国を選択します");
      this.toolTip1.SetToolTip((Control) this.comboBox1, "地図を選択します");
      this.toolTip1.SetToolTip((Control) this.comboBox5, "緯度、経度線の描画方法を選択します\n単位は測量で得られる値です\n初期値は座標のみ描画です");
      this.toolTip1.SetToolTip((Control) this.textBox1, "航路共有用のグル\x30FCプ名を指定します\n空白にすると航路共有されません");
      this.toolTip1.SetToolTip((Control) this.textBox2, "航路共有描画時に表示される名前を指定します\n指定した名前を航路共有する他のメンバ\x30FCに伝えます\n空白にすると航路共有されません");
      this.toolTip1.SetToolTip((Control) this.checkBox8, "チェックを入れると街名、街アイコン等をできるだけ等倍で表示します。\nチェックを外すと地図の縮尺に合わせて拡縮されて表示します。\nチェックを外したほうが描画が軽くなります。");
      this.toolTip1.SetToolTip((Control) this.checkBox1, "インタ\x30FCネットに接続するかどうかを指定します\nチェックを入れると起動時のデ\x30FCタ更新、航路共有が有効になります");
      this.toolTip1.SetToolTip((Control) this.checkBox2, "マウスの戻る・進むボタンでスキル・道具窓を開きます");
      this.toolTip1.SetToolTip((Control) this.checkBox6, "航路共有を有効にする場合はチェックを入れてください\nインタ\x30FCネットから更新情報等を受け取るにチェックを入れている必要があります");
      this.toolTip1.SetToolTip((Control) this.checkBox9, "起動時インタ\x30FCネットから@Webアイコンを取得します\n取得した@Webアイコンはロ\x30FCカルに保存されます\n起動時に毎回取得する必要がない場合はチェックをはずしてください");
      this.toolTip1.SetToolTip((Control) this.comboBox6, "街アイコンのサイズを選択します。\n海岸線がアイコンで隠れるのがいやな方は小さいアイコンを選択してください。");
      this.toolTip1.SetToolTip((Control) this.comboBox7, "街名等を描画するかどうかを選択します。\n描画しない場合はマウスを乗せると街名がポップアップします。");
      this.toolTip1.SetToolTip((Control) this.comboBox8, "スクリ\x30FCンショットのフォ\x30FCマットを選択します。\n初期値はbmpです。");
      this.toolTip1.SetToolTip((Control) this.checkBox4, "造船中でなくても造船カウンタを表示するかどうかを設定します");
      this.toolTip1.SetToolTip((Control) this.checkBox10, "右クリック時の動作を選択します\n" + "チェック有\n" + "  右クリックでコンテキストメニュ\x30FCが開く\n" + "  右クリックでも街を選択できる\n" + "  スポット解除はESCキ\x30FCのみ\n" + "チェックなし\n" + "  Ctrl+右クリックでコンテキストメニュ\x30FCが開く\n" + "  右クリックでは街を選択できない\n" + "  スポット解除はESCキ\x30FCかどこかの街を選択\n");
      this.toolTip1.SetToolTip((Control) this.checkBox11, "お気に入り航路と合成した地図を使用します\nこの項目はお気に入り航路の使用／不使用切り替えのために用意されています");
      this.toolTip1.SetToolTip((Control) this.checkBox12, "ウインドウを常に最前面に表示します");
      this.toolTip1.SetToolTip((Control) this.checkBox13, "航路図、コンパスの角度線、進路予想線の描画方法を指定します\nチェックを入れた場合アンチエイリアスで描画されます");
      this.toolTip1.SetToolTip((Control) this.checkBox14, "一番新しい航路図以外を半透明で描画します\n日付、災害ポップアップも半透明になります");
      this.toolTip1.SetToolTip((Control) this.checkBox16, "@Webアイコン描画時に同じ種類で距離が近い場合、1つにまとめます。\n@Webアイコン表示時のごちゃごちゃした感じを軽減します。");
      this.toolTip1.SetToolTip((Control) this.checkBox3, "画面キャプチャ方法を指定します\nWindows Vistaを使用していて航路図がうまく書かれない場合チェックを入れてください。\nWindows7ではこのチェックを入れる必要はありません。");
      this.toolTip1.SetToolTip((Control) this.checkBox5, "災害ポップアップ、利息からの経過日数、海域変動システム用にログ解析を行います");
      this.toolTip1.SetToolTip((Control) this.checkBox17, "キャプチャした画像を右に表示します\nコンパス解析の角度ずれの確認用です\n通常はチェックを入れる必要はありません");
      this.toolTip1.SetToolTip((Control) this.comboBox4, "画面キャプチャ間隔を選択します\n短い間隔でキャプチャするほどコンパスの角度のレスポンスがよくなりますがCPU時間を多く消費します\nCPUに余裕がある場合は0.5秒に1回を選択してください\nさらにCPUに余裕がある場合は0.25秒に1回を選択してください\n初期値は1秒に1回です");
      this.toolTip1.SetToolTip((Control) this.textBox6, "TCPサ\x30FCバが使用するポ\x30FCト番号を指定します\n特に変更する必要はありません");
      this.toolTip1.SetToolTip((Control) this.checkBox7, "情報ウインドウの座標系を測量系ではなく地図系にします\n開発時の位置取得用です");
      this.toolTip1.SetToolTip((Control) this.textBox8, "描画する最低航海日数を指定します\n" + "この設定は最も新しい航路図には影響を与えません\n" + "狭い範囲を航海すると航路図がごちゃごちゃしてしまうのを軽減できます\n" + "例えば3に設定すると航海日数2日以下の航路図は描画されなくなります\n" + "0に設定すると全ての航路図が描画されます\n" + "初期値は0です");
      this.toolTip1.SetToolTip((Control) this.checkBox18, "お気に入り航路図を半透明で描画します");
      this.toolTip1.SetToolTip((Control) this.checkBox19, "お気に入り航路図の災害ポップアップを描画します");
      this.toolTip1.SetToolTip((Control) this.textBox9, "過去の航路図を保持する数を設定します\n" + "過去の航路図は描画されないため、CPU負荷が低く多くの航路図を保持しても問題ありません\n" + "初期値は200です");
      this.toolTip1.SetToolTip((Control) this.textBox3, "航路図を保持する数を指定します\n" + "保持数を多くすると描画負荷が増えます\n" + "海に出る度に航路図を全て削除している方は1を指定してください\n" + "初期値は20です");
      this.comboBox2.SelectedIndex = (int) this.m_setting.server;
      this.comboBox3.SelectedItem = (object) GvoWorldInfo.GetCountryString(this.m_setting.country);
      this.comboBox1.SelectedIndex = (int) this.m_setting.map;
      this.comboBox5.SelectedIndex = (int) this.m_setting.tude_interval;
      this.comboBox6.SelectedIndex = (int) this.m_setting.map_icon;
      this.comboBox7.SelectedIndex = (int) this.m_setting.map_draw_names;
      this.comboBox8.SelectedIndex = (int) this.m_setting.ss_format;
      this.textBox1.Text = this.m_setting.share_group;
      this.textBox2.Text = this.m_setting.share_group_myname;
      this.textBox3.Text = this.m_setting.searoutes_group_max.ToString();
      this.textBox9.Text = this.m_setting.trash_searoutes_group_max.ToString();
      this.textBox8.Text = this.m_setting.minimum_draw_days.ToString();
      this.checkBox1.Checked = this.m_setting.connect_network;
      this.checkBox2.Checked = this.m_setting.hook_mouse;
      this.checkBox6.Checked = this.m_setting.is_share_routes;
      this.checkBox9.Checked = this.m_setting.connect_web_icon;
      this.checkBox10.Checked = this.m_setting.compatible_windows_rclick;
      this.checkBox11.Checked = this.m_setting.use_mixed_map;
      this.checkBox12.Checked = this.m_setting.window_top_most;
      this.checkBox13.Checked = this.m_setting.enable_line_antialias;
      this.checkBox14.Checked = this.m_setting.enable_sea_routes_aplha;
      this.checkBox16.Checked = this.m_setting.remove_near_web_icons;
      this.checkBox8.Checked = !this.m_setting.is_mixed_info_names;
      this.checkBox18.Checked = this.m_setting.enable_favorite_sea_routes_alpha;
      this.checkBox19.Checked = this.m_setting.draw_favorite_sea_routes_alpha_popup;
      this.checkBox4.Checked = this.m_setting.force_show_build_ship;
      this.checkBox7.Checked = this.m_setting.debug_flag_show_potision;
      if (this.m_setting.capture_interval == CaptureIntervalIndex.Per250ms)
        this.comboBox4.SelectedIndex = 0;
      else
        this.comboBox4.SelectedIndex = (int) (this.m_setting.capture_interval + 1);
      this.checkBox3.Checked = this.m_setting.windows_vista_aero;
      this.checkBox5.Checked = this.m_setting.enable_analize_log_chat;
      this.checkBox17.Checked = this.m_setting.draw_capture_info;
      this.textBox6.Text = this.m_setting.port_index.ToString();
      try
      {
        this.textBox5.AppendText(net_useful.GetHostName() + "\n");
        IPAddress[] localIpAddressIpv4 = net_useful.GetLocalIpAddress_Ipv4();
        if (localIpAddressIpv4 != null)
        {
          if (localIpAddressIpv4.Length > 0)
            this.textBox5.AppendText(localIpAddressIpv4[0].ToString());
        }
      }
      catch
      {
        this.textBox5.AppendText("PC名\n");
        this.textBox5.AppendText("IPアドレスの取得に失敗");
      }
      if (this.m_setting.is_server_mode)
        this.radioButton2.Checked = true;
      else
        this.radioButton1.Checked = true;
      this.textBox4.Lines = device_info.Split(new string[2]
      {
        "\n",
        "\r\n"
      }, StringSplitOptions.None);
      this.label5.Text = "大航海時代Online 交易MAP C# ver.1.32.3";
      this.init_draw_setting(page);
      this.update_gray_ctrl();
      this.listBox1.SelectedIndex = (int) index;
      this.linkLabel1.Text = "http://www.geocities.jp/cookiezephyros/";
      this.m_key_assign_helper = new KeyAssiginSettingHelper(this.m_key_assign_list, (Form) this, this.comboBox9, this.listView1, this.button3, this.button5, this.button6);
    }

    private void init_draw_setting(DrawSettingPage page)
    {
      DrawSettingWebIcons drawSettingWebIcons = this.m_setting.draw_setting_web_icons;
      this.checkBox100.Checked = (drawSettingWebIcons & DrawSettingWebIcons.wind) != (DrawSettingWebIcons) 0;
      this.checkBox101.Checked = (drawSettingWebIcons & DrawSettingWebIcons.accident_0) != (DrawSettingWebIcons) 0;
      this.checkBox102.Checked = (drawSettingWebIcons & DrawSettingWebIcons.accident_1) != (DrawSettingWebIcons) 0;
      this.checkBox103.Checked = (drawSettingWebIcons & DrawSettingWebIcons.accident_2) != (DrawSettingWebIcons) 0;
      this.checkBox104.Checked = (drawSettingWebIcons & DrawSettingWebIcons.accident_3) != (DrawSettingWebIcons) 0;
      this.checkBox105.Checked = (drawSettingWebIcons & DrawSettingWebIcons.accident_4) != (DrawSettingWebIcons) 0;
      DrawSettingMemoIcons settingMemoIcons = this.m_setting.draw_setting_memo_icons;
      this.checkBox200.Checked = (settingMemoIcons & DrawSettingMemoIcons.wind) != (DrawSettingMemoIcons) 0;
      this.checkBox201.Checked = (settingMemoIcons & DrawSettingMemoIcons.memo_0) != (DrawSettingMemoIcons) 0;
      this.checkBox202.Checked = (settingMemoIcons & DrawSettingMemoIcons.memo_1) != (DrawSettingMemoIcons) 0;
      this.checkBox203.Checked = (settingMemoIcons & DrawSettingMemoIcons.memo_2) != (DrawSettingMemoIcons) 0;
      this.checkBox204.Checked = (settingMemoIcons & DrawSettingMemoIcons.memo_3) != (DrawSettingMemoIcons) 0;
      this.checkBox205.Checked = (settingMemoIcons & DrawSettingMemoIcons.memo_4) != (DrawSettingMemoIcons) 0;
      this.checkBox206.Checked = (settingMemoIcons & DrawSettingMemoIcons.memo_5) != (DrawSettingMemoIcons) 0;
      this.checkBox207.Checked = (settingMemoIcons & DrawSettingMemoIcons.memo_6) != (DrawSettingMemoIcons) 0;
      this.checkBox208.Checked = (settingMemoIcons & DrawSettingMemoIcons.memo_7) != (DrawSettingMemoIcons) 0;
      this.checkBox209.Checked = (settingMemoIcons & DrawSettingMemoIcons.memo_8) != (DrawSettingMemoIcons) 0;
      this.checkBox210.Checked = (settingMemoIcons & DrawSettingMemoIcons.memo_9) != (DrawSettingMemoIcons) 0;
      this.checkBox211.Checked = (settingMemoIcons & DrawSettingMemoIcons.memo_10) != (DrawSettingMemoIcons) 0;
      this.checkBox212.Checked = (settingMemoIcons & DrawSettingMemoIcons.memo_11) != (DrawSettingMemoIcons) 0;
      DrawSettingAccidents settingAccidents = this.m_setting.draw_setting_accidents;
      this.checkBox300.Checked = (settingAccidents & DrawSettingAccidents.accident_0) != (DrawSettingAccidents) 0;
      this.checkBox301.Checked = (settingAccidents & DrawSettingAccidents.accident_1) != (DrawSettingAccidents) 0;
      this.checkBox302.Checked = (settingAccidents & DrawSettingAccidents.accident_2) != (DrawSettingAccidents) 0;
      this.checkBox303.Checked = (settingAccidents & DrawSettingAccidents.accident_3) != (DrawSettingAccidents) 0;
      this.checkBox304.Checked = (settingAccidents & DrawSettingAccidents.accident_4) != (DrawSettingAccidents) 0;
      this.checkBox305.Checked = (settingAccidents & DrawSettingAccidents.accident_5) != (DrawSettingAccidents) 0;
      this.checkBox306.Checked = (settingAccidents & DrawSettingAccidents.accident_6) != (DrawSettingAccidents) 0;
      this.checkBox307.Checked = (settingAccidents & DrawSettingAccidents.accident_7) != (DrawSettingAccidents) 0;
      this.checkBox308.Checked = (settingAccidents & DrawSettingAccidents.accident_8) != (DrawSettingAccidents) 0;
      this.checkBox309.Checked = (settingAccidents & DrawSettingAccidents.accident_9) != (DrawSettingAccidents) 0;
      this.checkBox310.Checked = (settingAccidents & DrawSettingAccidents.accident_10) != (DrawSettingAccidents) 0;
      DrawSettingMyShipAngle settingMyshipAngle = this.m_setting.draw_setting_myship_angle;
      this.checkBox400.Checked = (settingMyshipAngle & DrawSettingMyShipAngle.draw_0) != (DrawSettingMyShipAngle) 0;
      this.checkBox401.Checked = this.m_setting.draw_setting_myship_angle_with_speed_pos;
      this.checkBox402.Checked = (settingMyshipAngle & DrawSettingMyShipAngle.draw_1) != (DrawSettingMyShipAngle) 0;
      this.checkBox403.Checked = this.m_setting.draw_setting_myship_expect_pos;
      if (page < DrawSettingPage.WebIcons)
        page = DrawSettingPage.WebIcons;
      if (page > DrawSettingPage.MyShipAngle)
        page = DrawSettingPage.MyShipAngle;
      this.tabControl1.SelectTab((int) page);
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (this.listBox1.SelectedIndex)
      {
        case 0:
          this.panelManager1.SelectedPanel = this.managedPanel1;
          break;
        case 1:
          this.panelManager1.SelectedPanel = this.managedPanel7;
          break;
        case 2:
          this.panelManager1.SelectedPanel = this.managedPanel2;
          break;
        case 3:
          this.panelManager1.SelectedPanel = this.managedPanel3;
          break;
        case 4:
          this.panelManager1.SelectedPanel = this.managedPanel4;
          break;
        case 5:
          this.panelManager1.SelectedPanel = this.managedPanel5;
          break;
        case 6:
          this.panelManager1.SelectedPanel = this.managedPanel8;
          break;
        case 7:
          this.panelManager1.SelectedPanel = this.managedPanel6;
          break;
      }
    }

    private void setting_form2_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.m_setting.server = GvoWorldInfo.GetServerFromString(this.comboBox2.Text);
      this.m_setting.country = GvoWorldInfo.GetCountryFromString(this.comboBox3.Text);
      this.m_setting.map = (MapIndex) this.comboBox1.SelectedIndex;
      this.m_setting.map_icon = (MapIcon) this.comboBox6.SelectedIndex;
      this.m_setting.map_draw_names = (MapDrawNames) this.comboBox7.SelectedIndex;
      this.m_setting.ss_format = (SSFormat) this.comboBox8.SelectedIndex;
      this.m_setting.tude_interval = (TudeInterval) this.comboBox5.SelectedIndex;
      this.m_setting.share_group = this.textBox1.Text;
      this.m_setting.share_group_myname = this.textBox2.Text;
      this.m_setting.connect_network = this.checkBox1.Checked;
      this.m_setting.hook_mouse = this.checkBox2.Checked;
      this.m_setting.is_share_routes = this.checkBox6.Checked;
      this.m_setting.connect_web_icon = this.checkBox9.Checked;
      this.m_setting.compatible_windows_rclick = this.checkBox10.Checked;
      this.m_setting.use_mixed_map = this.checkBox11.Checked;
      this.m_setting.window_top_most = this.checkBox12.Checked;
      this.m_setting.enable_line_antialias = this.checkBox13.Checked;
      this.m_setting.enable_sea_routes_aplha = this.checkBox14.Checked;
      this.m_setting.remove_near_web_icons = this.checkBox16.Checked;
      this.m_setting.is_mixed_info_names = !this.checkBox8.Checked;
      this.m_setting.enable_favorite_sea_routes_alpha = this.checkBox18.Checked;
      this.m_setting.draw_favorite_sea_routes_alpha_popup = this.checkBox19.Checked;
      this.m_setting.force_show_build_ship = this.checkBox4.Checked;
      this.m_setting.debug_flag_show_potision = this.checkBox7.Checked;
      this.m_setting.searoutes_group_max = Useful.ToInt32(this.textBox3.Text, -1);
      this.m_setting.trash_searoutes_group_max = Useful.ToInt32(this.textBox9.Text, -1);
      this.m_setting.minimum_draw_days = Useful.ToInt32(this.textBox8.Text, -1);
      this.m_setting.is_server_mode = this.radioButton2.Checked;
      this.m_setting.port_index = Useful.ToInt32(this.textBox6.Text, 16612);
      this.m_setting.capture_interval = this.comboBox4.SelectedIndex != 0 ? (CaptureIntervalIndex) (this.comboBox4.SelectedIndex - 1) : CaptureIntervalIndex.Per250ms;
      this.m_setting.windows_vista_aero = this.checkBox3.Checked;
      this.m_setting.enable_analize_log_chat = this.checkBox5.Checked;
      this.m_setting.draw_capture_info = this.checkBox17.Checked;
      this.save_draw_setting();
      this.m_key_assign_list = this.m_key_assign_helper.List;
    }

    private void save_draw_setting()
    {
      this.m_setting.draw_setting_web_icons = (DrawSettingWebIcons) (0 | (this.checkBox100.Checked ? 1 : 0) | (this.checkBox101.Checked ? 2 : 0) | (this.checkBox102.Checked ? 4 : 0) | (this.checkBox103.Checked ? 8 : 0) | (this.checkBox104.Checked ? 16 : 0) | (this.checkBox105.Checked ? 32 : 0));
      this.m_setting.draw_setting_memo_icons = (DrawSettingMemoIcons) (0 | (this.checkBox200.Checked ? 1 : 0) | (this.checkBox201.Checked ? 2 : 0) | (this.checkBox202.Checked ? 4 : 0) | (this.checkBox203.Checked ? 8 : 0) | (this.checkBox204.Checked ? 16 : 0) | (this.checkBox205.Checked ? 32 : 0) | (this.checkBox206.Checked ? 64 : 0) | (this.checkBox207.Checked ? 128 : 0) | (this.checkBox208.Checked ? 256 : 0) | (this.checkBox209.Checked ? 512 : 0) | (this.checkBox210.Checked ? 1024 : 0) | (this.checkBox211.Checked ? 2048 : 0) | (this.checkBox212.Checked ? 4096 : 0));
      this.m_setting.draw_setting_accidents = (DrawSettingAccidents) (0 | (this.checkBox300.Checked ? 1 : 0) | (this.checkBox301.Checked ? 2 : 0) | (this.checkBox302.Checked ? 4 : 0) | (this.checkBox303.Checked ? 8 : 0) | (this.checkBox304.Checked ? 16 : 0) | (this.checkBox305.Checked ? 32 : 0) | (this.checkBox306.Checked ? 64 : 0) | (this.checkBox307.Checked ? 128 : 0) | (this.checkBox308.Checked ? 256 : 0) | (this.checkBox309.Checked ? 512 : 0) | (this.checkBox310.Checked ? 1024 : 0));
      this.m_setting.draw_setting_myship_angle = (DrawSettingMyShipAngle) (0 | (this.checkBox400.Checked ? 1 : 0) | (this.checkBox402.Checked ? 2 : 0));
      this.m_setting.draw_setting_myship_angle_with_speed_pos = this.checkBox401.Checked;
      this.m_setting.draw_setting_myship_expect_pos = this.checkBox403.Checked;
    }

    private void radioButton1_CheckedChanged(object sender, EventArgs e)
    {
      this.update_gray_ctrl();
    }

    private void radioButton2_CheckedChanged(object sender, EventArgs e)
    {
      this.update_gray_ctrl();
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
      this.update_gray_ctrl();
    }

    private void checkBox6_CheckedChanged(object sender, EventArgs e)
    {
      this.update_gray_ctrl();
    }

    private void checkBox4_CheckedChanged(object sender, EventArgs e)
    {
      this.update_gray_ctrl();
    }

    private void checkBox400_CheckedChanged(object sender, EventArgs e)
    {
      this.update_gray_ctrl();
    }

    private void update_gray_ctrl()
    {
      if (this.checkBox1.Checked)
      {
        this.checkBox6.Enabled = true;
        this.checkBox9.Enabled = true;
      }
      else
      {
        this.checkBox6.Enabled = false;
        this.checkBox9.Enabled = false;
      }
      if (this.checkBox1.Checked && this.checkBox6.Checked)
      {
        this.textBox1.Enabled = true;
        this.textBox2.Enabled = true;
      }
      else
      {
        this.textBox1.Enabled = false;
        this.textBox2.Enabled = false;
      }
      if (this.radioButton1.Checked)
      {
        this.comboBox4.Enabled = true;
        this.checkBox3.Enabled = true;
        this.checkBox5.Enabled = true;
        this.checkBox17.Enabled = true;
        this.textBox6.Enabled = false;
      }
      else
      {
        this.comboBox4.Enabled = false;
        this.checkBox3.Enabled = false;
        this.checkBox5.Enabled = false;
        this.checkBox17.Enabled = false;
        this.textBox6.Enabled = true;
      }
      if (this.checkBox400.Checked)
      {
        this.checkBox401.Enabled = true;
        this.checkBox403.Enabled = true;
      }
      else
      {
        this.checkBox401.Enabled = false;
        this.checkBox403.Enabled = false;
      }
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("http://www.geocities.jp/cookiezephyros/");
      this.linkLabel1.LinkVisited = true;
    }

    private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("http://gvtrademap.daa.jp/");
      this.linkLabel2.LinkVisited = true;
    }

    private void button4_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      bool flag = HttpDownload.Download(def.VERSION_URL, def.VERSION_FNAME);
      this.Cursor = Cursors.Default;
      if (!flag)
      {
        int num1 = (int) MessageBox.Show((IWin32Window) this, "更新情報が取得できませんでした。\nインタ\x30FCネットの接続を確認してください。", "更新確認エラ\x30FC", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        List<string> list = new List<string>();
        int num2 = 0;
        try
        {
          using (StreamReader streamReader = new StreamReader(def.VERSION_FNAME, Encoding.GetEncoding("Shift_JIS")))
          {
            num2 = Convert.ToInt32(streamReader.ReadLine());
            string str;
            while ((str = streamReader.ReadLine()) != null)
              list.Add(str);
          }
        }
        catch
        {
          int num3 = (int) MessageBox.Show((IWin32Window) this, "バ\x30FCジョン情報が確認できません。\n更新確認に失敗しました。", "更新確認エラ\x30FC", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (num2 > def.VERSION)
        {
          check_update_result checkUpdateResult = new check_update_result(list.ToArray());
          int num3 = (int) checkUpdateResult.ShowDialog((IWin32Window) this);
          checkUpdateResult.Dispose();
        }
        else
        {
          int num4 = (int) MessageBox.Show((IWin32Window) this, "更新されたソフトウェアは見つかりませんでした。\nお使いのバ\x30FCジョンが最新です。", "更新確認結果", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (setting_form2));
      this.imageList1 = new ImageList(this.components);
      this.listBox1 = new ListBox();
      this.toolTip1 = new ToolTip(this.components);
      this.listView1 = new ListView();
      this.button2 = new Button();
      this.button1 = new Button();
      this.panelManager1 = new PanelManager();
      this.managedPanel1 = new ManagedPanel();
      this.groupBox1 = new GroupBox();
      this.groupBox8 = new GroupBox();
      this.comboBox8 = new ComboBox();
      this.groupBox7 = new GroupBox();
      this.comboBox5 = new ComboBox();
      this.groupBox4 = new GroupBox();
      this.checkBox8 = new CheckBox();
      this.comboBox7 = new ComboBox();
      this.comboBox6 = new ComboBox();
      this.comboBox1 = new ComboBox();
      this.groupBox3 = new GroupBox();
      this.comboBox3 = new ComboBox();
      this.comboBox2 = new ComboBox();
      this.managedPanel2 = new ManagedPanel();
      this.groupBox5 = new GroupBox();
      this.groupBox14 = new GroupBox();
      this.groupBox17 = new GroupBox();
      this.textBox5 = new TextBox();
      this.textBox6 = new TextBox();
      this.textBox7 = new TextBox();
      this.groupBox15 = new GroupBox();
      this.checkBox17 = new CheckBox();
      this.groupBox16 = new GroupBox();
      this.comboBox4 = new ComboBox();
      this.checkBox3 = new CheckBox();
      this.checkBox5 = new CheckBox();
      this.radioButton2 = new RadioButton();
      this.radioButton1 = new RadioButton();
      this.managedPanel3 = new ManagedPanel();
      this.groupBox9 = new GroupBox();
      this.checkBox1 = new CheckBox();
      this.checkBox9 = new CheckBox();
      this.groupBox6 = new GroupBox();
      this.checkBox6 = new CheckBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.textBox2 = new TextBox();
      this.textBox1 = new TextBox();
      this.managedPanel4 = new ManagedPanel();
      this.groupBox10 = new GroupBox();
      this.tabControl1 = new TabControl();
      this.tabPage1 = new TabPage();
      this.checkBox105 = new CheckBox();
      this.checkBox104 = new CheckBox();
      this.checkBox103 = new CheckBox();
      this.checkBox102 = new CheckBox();
      this.checkBox101 = new CheckBox();
      this.checkBox100 = new CheckBox();
      this.tabPage2 = new TabPage();
      this.checkBox212 = new CheckBox();
      this.checkBox211 = new CheckBox();
      this.checkBox210 = new CheckBox();
      this.checkBox209 = new CheckBox();
      this.checkBox208 = new CheckBox();
      this.checkBox207 = new CheckBox();
      this.checkBox206 = new CheckBox();
      this.checkBox205 = new CheckBox();
      this.checkBox204 = new CheckBox();
      this.checkBox203 = new CheckBox();
      this.checkBox202 = new CheckBox();
      this.checkBox201 = new CheckBox();
      this.checkBox200 = new CheckBox();
      this.tabPage3 = new TabPage();
      this.checkBox310 = new CheckBox();
      this.checkBox309 = new CheckBox();
      this.checkBox308 = new CheckBox();
      this.checkBox307 = new CheckBox();
      this.checkBox306 = new CheckBox();
      this.checkBox305 = new CheckBox();
      this.checkBox304 = new CheckBox();
      this.checkBox303 = new CheckBox();
      this.checkBox302 = new CheckBox();
      this.checkBox301 = new CheckBox();
      this.checkBox300 = new CheckBox();
      this.tabPage4 = new TabPage();
      this.checkBox403 = new CheckBox();
      this.checkBox401 = new CheckBox();
      this.checkBox402 = new CheckBox();
      this.checkBox400 = new CheckBox();
      this.managedPanel5 = new ManagedPanel();
      this.groupBox11 = new GroupBox();
      this.checkBox4 = new CheckBox();
      this.checkBox16 = new CheckBox();
      this.checkBox13 = new CheckBox();
      this.checkBox2 = new CheckBox();
      this.checkBox12 = new CheckBox();
      this.checkBox10 = new CheckBox();
      this.checkBox11 = new CheckBox();
      this.managedPanel6 = new ManagedPanel();
      this.groupBox12 = new GroupBox();
      this.button4 = new Button();
      this.groupBox18 = new GroupBox();
      this.label1 = new Label();
      this.linkLabel2 = new LinkLabel();
      this.linkLabel1 = new LinkLabel();
      this.label2 = new Label();
      this.label5 = new Label();
      this.pictureBox1 = new PictureBox();
      this.groupBox13 = new GroupBox();
      this.textBox4 = new TextBox();
      this.managedPanel7 = new ManagedPanel();
      this.groupBox20 = new GroupBox();
      this.label8 = new Label();
      this.textBox9 = new TextBox();
      this.groupBox19 = new GroupBox();
      this.checkBox19 = new CheckBox();
      this.checkBox18 = new CheckBox();
      this.groupBox2 = new GroupBox();
      this.label7 = new Label();
      this.textBox8 = new TextBox();
      this.checkBox14 = new CheckBox();
      this.label6 = new Label();
      this.textBox3 = new TextBox();
      this.managedPanel8 = new ManagedPanel();
      this.groupBox21 = new GroupBox();
      this.button6 = new Button();
      this.button5 = new Button();
      this.button3 = new Button();
      this.comboBox9 = new ComboBox();
      this.checkBox7 = new CheckBox();
      this.panelManager1.SuspendLayout();
      this.managedPanel1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox8.SuspendLayout();
      this.groupBox7.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.managedPanel2.SuspendLayout();
      this.groupBox5.SuspendLayout();
      this.groupBox14.SuspendLayout();
      this.groupBox17.SuspendLayout();
      this.groupBox15.SuspendLayout();
      this.groupBox16.SuspendLayout();
      this.managedPanel3.SuspendLayout();
      this.groupBox9.SuspendLayout();
      this.groupBox6.SuspendLayout();
      this.managedPanel4.SuspendLayout();
      this.groupBox10.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.tabPage4.SuspendLayout();
      this.managedPanel5.SuspendLayout();
      this.groupBox11.SuspendLayout();
      this.managedPanel6.SuspendLayout();
      this.groupBox12.SuspendLayout();
      this.groupBox18.SuspendLayout();
      this.pictureBox1.BeginInit();
      this.groupBox13.SuspendLayout();
      this.managedPanel7.SuspendLayout();
      this.groupBox20.SuspendLayout();
      this.groupBox19.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.managedPanel8.SuspendLayout();
      this.groupBox21.SuspendLayout();
      this.SuspendLayout();
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = Color.FromArgb(13, 111, 161);
      this.imageList1.Images.SetKeyName(0, "memo_icon22.bmp");
      this.imageList1.Images.SetKeyName(1, "memo_icon16.bmp");
      this.imageList1.Images.SetKeyName(2, "memo_icon23.bmp");
      this.imageList1.Images.SetKeyName(3, "memo_icon24.bmp");
      this.imageList1.Images.SetKeyName(4, "memo_icon25.bmp");
      this.imageList1.Images.SetKeyName(5, "memo_icon10.bmp");
      this.imageList1.Images.SetKeyName(6, "memo_icon11.bmp");
      this.imageList1.Images.SetKeyName(7, "memo_icon12.bmp");
      this.imageList1.Images.SetKeyName(8, "memo_icon13.bmp");
      this.imageList1.Images.SetKeyName(9, "memo_icon14.bmp");
      this.imageList1.Images.SetKeyName(10, "memo_icon15.bmp");
      this.imageList1.Images.SetKeyName(11, "memo_icon17.bmp");
      this.imageList1.Images.SetKeyName(12, "memo_icon18.bmp");
      this.imageList1.Images.SetKeyName(13, "memo_icon19.bmp");
      this.imageList1.Images.SetKeyName(14, "memo_icon20.bmp");
      this.imageList1.Images.SetKeyName(15, "memo_icon21.bmp");
      this.imageList1.Images.SetKeyName(16, "memo_icon26.bmp");
      this.imageList1.Images.SetKeyName(17, "memo_icon27.bmp");
      this.imageList1.Images.SetKeyName(18, "memo_icon28.bmp");
      this.imageList1.Images.SetKeyName(19, "memo_icon29.bmp");
      this.imageList1.Images.SetKeyName(20, "memo_icon30.bmp");
      this.imageList1.Images.SetKeyName(21, "memo_icon31.bmp");
      this.listBox1.Font = new Font("MS UI Gothic", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) sbyte.MinValue);
      this.listBox1.FormattingEnabled = true;
      this.listBox1.HorizontalScrollbar = true;
      this.listBox1.ItemHeight = 15;
      this.listBox1.Items.AddRange(new object[8]
      {
        (object) "基本設定",
        (object) "航路図設定",
        (object) "キャプチャ、ログ解析",
        (object) "ネット接続、航路共有",
        (object) "表示項目",
        (object) "その他",
        (object) "キ\x30FCボ\x30FCド割り当て",
        (object) "バ\x30FCジョン情報"
      });
      this.listBox1.Location = new Point(12, 12);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new Size(150, 319);
      this.listBox1.TabIndex = 5;
      this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
      this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.HideSelection = false;
      this.listView1.Location = new Point(6, 44);
      this.listView1.MultiSelect = false;
      this.listView1.Name = "listView1";
      this.listView1.ShowItemToolTips = true;
      this.listView1.Size = new Size(398, 224);
      this.listView1.TabIndex = 3;
      this.toolTip1.SetToolTip((Control) this.listView1, "a");
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(495, 330);
      this.button2.Name = "button2";
      this.button2.Size = new Size(83, 23);
      this.button2.TabIndex = 14;
      this.button2.Text = "キャンセル";
      this.button2.UseVisualStyleBackColor = true;
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button1.DialogResult = DialogResult.OK;
      this.button1.Location = new Point(406, 330);
      this.button1.Name = "button1";
      this.button1.Size = new Size(83, 23);
      this.button1.TabIndex = 13;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.panelManager1.Controls.Add((Control) this.managedPanel1);
      this.panelManager1.Controls.Add((Control) this.managedPanel2);
      this.panelManager1.Controls.Add((Control) this.managedPanel3);
      this.panelManager1.Controls.Add((Control) this.managedPanel4);
      this.panelManager1.Controls.Add((Control) this.managedPanel5);
      this.panelManager1.Controls.Add((Control) this.managedPanel6);
      this.panelManager1.Controls.Add((Control) this.managedPanel7);
      this.panelManager1.Controls.Add((Control) this.managedPanel8);
      this.panelManager1.Location = new Point(168, 12);
      this.panelManager1.Name = "panelManager1";
      this.panelManager1.SelectedIndex = 4;
      this.panelManager1.SelectedPanel = this.managedPanel5;
      this.panelManager1.Size = new Size(410, 313);
      this.panelManager1.TabIndex = 0;
      this.managedPanel1.Controls.Add((Control) this.groupBox1);
      this.managedPanel1.Location = new Point(0, 0);
      this.managedPanel1.Name = "managedPanel1";
      this.managedPanel1.Size = new Size(0, 0);
      this.managedPanel1.Text = "managedPanel1";
      this.groupBox1.Controls.Add((Control) this.groupBox8);
      this.groupBox1.Controls.Add((Control) this.groupBox7);
      this.groupBox1.Controls.Add((Control) this.groupBox4);
      this.groupBox1.Controls.Add((Control) this.groupBox3);
      this.groupBox1.Location = new Point(0, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(410, 307);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "基本設定";
      this.groupBox8.Controls.Add((Control) this.comboBox8);
      this.groupBox8.FlatStyle = FlatStyle.Flat;
      this.groupBox8.Location = new Point(169, 75);
      this.groupBox8.Name = "groupBox8";
      this.groupBox8.Size = new Size(157, 51);
      this.groupBox8.TabIndex = 0;
      this.groupBox8.TabStop = false;
      this.groupBox8.Text = "スクリ\x30FCンショット";
      this.comboBox8.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox8.FormattingEnabled = true;
      this.comboBox8.Items.AddRange(new object[3]
      {
        (object) "bmp",
        (object) "png",
        (object) "jpeg"
      });
      this.comboBox8.Location = new Point(7, 18);
      this.comboBox8.Name = "comboBox8";
      this.comboBox8.Size = new Size(144, 20);
      this.comboBox8.TabIndex = 0;
      this.groupBox7.Controls.Add((Control) this.comboBox5);
      this.groupBox7.FlatStyle = FlatStyle.Flat;
      this.groupBox7.Location = new Point(169, 18);
      this.groupBox7.Name = "groupBox7";
      this.groupBox7.Size = new Size(157, 51);
      this.groupBox7.TabIndex = 3;
      this.groupBox7.TabStop = false;
      this.groupBox7.Text = "緯度、経度線";
      this.comboBox5.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox5.FormattingEnabled = true;
      this.comboBox5.Items.AddRange(new object[4]
      {
        (object) "描画しない",
        (object) "1000刻みで描画",
        (object) "100刻みで描画",
        (object) "座標のみ描画"
      });
      this.comboBox5.Location = new Point(6, 18);
      this.comboBox5.Name = "comboBox5";
      this.comboBox5.Size = new Size(145, 20);
      this.comboBox5.TabIndex = 0;
      this.groupBox4.Controls.Add((Control) this.checkBox8);
      this.groupBox4.Controls.Add((Control) this.comboBox7);
      this.groupBox4.Controls.Add((Control) this.comboBox6);
      this.groupBox4.Controls.Add((Control) this.comboBox1);
      this.groupBox4.FlatStyle = FlatStyle.Flat;
      this.groupBox4.Location = new Point(6, 101);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new Size(157, 125);
      this.groupBox4.TabIndex = 2;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "地図";
      this.checkBox8.AutoSize = true;
      this.checkBox8.Location = new Point(6, 100);
      this.checkBox8.Name = "checkBox8";
      this.checkBox8.Size = new Size(145, 16);
      this.checkBox8.TabIndex = 12;
      this.checkBox8.Text = "街名等を等倍表示にする";
      this.checkBox8.UseVisualStyleBackColor = true;
      this.comboBox7.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox7.FormattingEnabled = true;
      this.comboBox7.Items.AddRange(new object[2]
      {
        (object) "街名を描画する",
        (object) "街名を描画しない"
      });
      this.comboBox7.Location = new Point(6, 74);
      this.comboBox7.Name = "comboBox7";
      this.comboBox7.Size = new Size(145, 20);
      this.comboBox7.TabIndex = 2;
      this.comboBox6.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox6.FormattingEnabled = true;
      this.comboBox6.Items.AddRange(new object[2]
      {
        (object) "街アイコン大",
        (object) "街アイコン小"
      });
      this.comboBox6.Location = new Point(6, 46);
      this.comboBox6.Name = "comboBox6";
      this.comboBox6.Size = new Size(145, 20);
      this.comboBox6.TabIndex = 1;
      this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Items.AddRange(new object[2]
      {
        (object) "地図1(緑っぽい)",
        (object) "地図2(茶色っぽい)"
      });
      this.comboBox1.Location = new Point(6, 18);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new Size(145, 20);
      this.comboBox1.TabIndex = 0;
      this.groupBox3.Controls.Add((Control) this.comboBox3);
      this.groupBox3.Controls.Add((Control) this.comboBox2);
      this.groupBox3.FlatStyle = FlatStyle.Flat;
      this.groupBox3.Location = new Point(6, 18);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(157, 77);
      this.groupBox3.TabIndex = 1;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "サ\x30FCバ・自国";
      this.comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox3.FormattingEnabled = true;
      this.comboBox3.Items.AddRange(new object[7]
      {
        (object) "イングランド",
        (object) "イスパニア",
        (object) "ポルトガル",
        (object) "ネ\x30FCデルランド",
        (object) "フランス",
        (object) "ヴェネツィア",
        (object) "オスマントルコ"
      });
      this.comboBox3.Location = new Point(6, 46);
      this.comboBox3.Name = "comboBox3";
      this.comboBox3.Size = new Size(145, 20);
      this.comboBox3.TabIndex = 1;
      this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox2.FormattingEnabled = true;
      this.comboBox2.Items.AddRange(new object[4]
      {
        (object) "Euros",
        (object) "Zephyros",
        (object) "Notos",
        (object) "Boreas"
      });
      this.comboBox2.Location = new Point(6, 18);
      this.comboBox2.Name = "comboBox2";
      this.comboBox2.Size = new Size(145, 20);
      this.comboBox2.TabIndex = 0;
      this.managedPanel2.Controls.Add((Control) this.groupBox5);
      this.managedPanel2.Location = new Point(0, 0);
      this.managedPanel2.Name = "managedPanel2";
      this.managedPanel2.Size = new Size(0, 0);
      this.managedPanel2.Text = "managedPanel2";
      this.groupBox5.Controls.Add((Control) this.groupBox14);
      this.groupBox5.Controls.Add((Control) this.groupBox15);
      this.groupBox5.Controls.Add((Control) this.radioButton2);
      this.groupBox5.Controls.Add((Control) this.radioButton1);
      this.groupBox5.Location = new Point(0, 3);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new Size(410, 307);
      this.groupBox5.TabIndex = 0;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "キャプチャ、ログ解析";
      this.groupBox14.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox14.Controls.Add((Control) this.groupBox17);
      this.groupBox14.Controls.Add((Control) this.textBox7);
      this.groupBox14.Location = new Point(6, 152);
      this.groupBox14.Name = "groupBox14";
      this.groupBox14.Size = new Size(398, 121);
      this.groupBox14.TabIndex = 26;
      this.groupBox14.TabStop = false;
      this.groupBox17.Controls.Add((Control) this.textBox5);
      this.groupBox17.Controls.Add((Control) this.textBox6);
      this.groupBox17.Location = new Point(6, 18);
      this.groupBox17.Name = "groupBox17";
      this.groupBox17.Size = new Size(128, 95);
      this.groupBox17.TabIndex = 1;
      this.groupBox17.TabStop = false;
      this.groupBox17.Text = "ポ\x30FCト番号";
      this.textBox5.Location = new Point(6, 45);
      this.textBox5.Multiline = true;
      this.textBox5.Name = "textBox5";
      this.textBox5.ReadOnly = true;
      this.textBox5.Size = new Size(116, 44);
      this.textBox5.TabIndex = 3;
      this.textBox6.Location = new Point(6, 18);
      this.textBox6.Name = "textBox6";
      this.textBox6.Size = new Size(116, 19);
      this.textBox6.TabIndex = 1;
      this.textBox7.Location = new Point(140, 18);
      this.textBox7.Multiline = true;
      this.textBox7.Name = "textBox7";
      this.textBox7.ReadOnly = true;
      this.textBox7.ScrollBars = ScrollBars.Both;
      this.textBox7.Size = new Size(252, 95);
      this.textBox7.TabIndex = 0;
      this.textBox7.Text = "このモ\x30FCドは画面キャプチャ情報、ログ解析情報をナビゲ\x30FCションクライアントから得ます。\r\nこのモ\x30FCドはTCPサ\x30FCバを起動します。\r\nネットワ\x30FCクのセキュリティに注意が必要です。";
      this.groupBox15.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox15.Controls.Add((Control) this.checkBox17);
      this.groupBox15.Controls.Add((Control) this.groupBox16);
      this.groupBox15.Controls.Add((Control) this.checkBox3);
      this.groupBox15.Controls.Add((Control) this.checkBox5);
      this.groupBox15.Location = new Point(6, 35);
      this.groupBox15.Name = "groupBox15";
      this.groupBox15.Size = new Size(398, 91);
      this.groupBox15.TabIndex = 25;
      this.groupBox15.TabStop = false;
      this.checkBox17.AutoSize = true;
      this.checkBox17.Location = new Point(140, 63);
      this.checkBox17.Name = "checkBox17";
      this.checkBox17.Size = new Size(196, 16);
      this.checkBox17.TabIndex = 23;
      this.checkBox17.Text = "キャプチャした画像の詳細を表示する";
      this.checkBox17.UseVisualStyleBackColor = true;
      this.groupBox16.Controls.Add((Control) this.comboBox4);
      this.groupBox16.FlatStyle = FlatStyle.Flat;
      this.groupBox16.Location = new Point(6, 18);
      this.groupBox16.Name = "groupBox16";
      this.groupBox16.Size = new Size(128, 49);
      this.groupBox16.TabIndex = 15;
      this.groupBox16.TabStop = false;
      this.groupBox16.Text = "画面キャプチャ間隔";
      this.comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox4.FormattingEnabled = true;
      this.comboBox4.Items.AddRange(new object[4]
      {
        (object) "0.25秒に1回",
        (object) "0.5秒に1回",
        (object) "1秒に1回",
        (object) "2秒に1回"
      });
      this.comboBox4.Location = new Point(6, 18);
      this.comboBox4.Name = "comboBox4";
      this.comboBox4.Size = new Size(116, 20);
      this.comboBox4.TabIndex = 0;
      this.checkBox3.AutoSize = true;
      this.checkBox3.Location = new Point(140, 40);
      this.checkBox3.Name = "checkBox3";
      this.checkBox3.Size = new Size(160, 16);
      this.checkBox3.TabIndex = 18;
      this.checkBox3.Text = "Windows Vista Aeroを使用";
      this.checkBox3.UseVisualStyleBackColor = true;
      this.checkBox5.AutoSize = true;
      this.checkBox5.Location = new Point(140, 18);
      this.checkBox5.Name = "checkBox5";
      this.checkBox5.Size = new Size(94, 16);
      this.checkBox5.TabIndex = 17;
      this.checkBox5.Text = "ログ解析を行う";
      this.checkBox5.UseVisualStyleBackColor = true;
      this.radioButton2.AutoSize = true;
      this.radioButton2.Location = new Point(6, 132);
      this.radioButton2.Name = "radioButton2";
      this.radioButton2.Size = new Size(103, 16);
      this.radioButton2.TabIndex = 24;
      this.radioButton2.TabStop = true;
      this.radioButton2.Text = "TCPサ\x30FCバモ\x30FCド";
      this.radioButton2.UseVisualStyleBackColor = true;
      this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
      this.radioButton1.AutoSize = true;
      this.radioButton1.Location = new Point(6, 18);
      this.radioButton1.Name = "radioButton1";
      this.radioButton1.Size = new Size(75, 16);
      this.radioButton1.TabIndex = 23;
      this.radioButton1.TabStop = true;
      this.radioButton1.Text = "通常モ\x30FCド";
      this.radioButton1.UseVisualStyleBackColor = true;
      this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
      this.managedPanel3.Controls.Add((Control) this.groupBox9);
      this.managedPanel3.Controls.Add((Control) this.groupBox6);
      this.managedPanel3.Location = new Point(0, 0);
      this.managedPanel3.Name = "managedPanel3";
      this.managedPanel3.Size = new Size(0, 0);
      this.managedPanel3.Text = "managedPanel3";
      this.groupBox9.Controls.Add((Control) this.checkBox1);
      this.groupBox9.Controls.Add((Control) this.checkBox9);
      this.groupBox9.Location = new Point(0, 3);
      this.groupBox9.Name = "groupBox9";
      this.groupBox9.Size = new Size(410, 69);
      this.groupBox9.TabIndex = 13;
      this.groupBox9.TabStop = false;
      this.groupBox9.Text = "ネット接続";
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new Point(6, 18);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new Size(215, 16);
      this.checkBox1.TabIndex = 7;
      this.checkBox1.Text = "インタ\x30FCネットから更新情報等を受け取る";
      this.checkBox1.UseVisualStyleBackColor = true;
      this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
      this.checkBox9.AutoSize = true;
      this.checkBox9.Location = new Point(6, 42);
      this.checkBox9.Name = "checkBox9";
      this.checkBox9.Size = new Size(260, 16);
      this.checkBox9.TabIndex = 8;
      this.checkBox9.Text = "起動時に交易MAP@Webのアイコンメモを取得する";
      this.checkBox9.UseVisualStyleBackColor = true;
      this.groupBox6.Controls.Add((Control) this.checkBox6);
      this.groupBox6.Controls.Add((Control) this.label4);
      this.groupBox6.Controls.Add((Control) this.label3);
      this.groupBox6.Controls.Add((Control) this.textBox2);
      this.groupBox6.Controls.Add((Control) this.textBox1);
      this.groupBox6.FlatStyle = FlatStyle.Flat;
      this.groupBox6.Location = new Point(0, 78);
      this.groupBox6.Name = "groupBox6";
      this.groupBox6.Size = new Size(410, 92);
      this.groupBox6.TabIndex = 12;
      this.groupBox6.TabStop = false;
      this.groupBox6.Text = "航路共有";
      this.checkBox6.AutoSize = true;
      this.checkBox6.Location = new Point(6, 18);
      this.checkBox6.Name = "checkBox6";
      this.checkBox6.Size = new Size(67, 16);
      this.checkBox6.TabIndex = 0;
      this.checkBox6.Text = "共有する";
      this.checkBox6.UseVisualStyleBackColor = true;
      this.checkBox6.CheckedChanged += new EventHandler(this.checkBox6_CheckedChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(23, 65);
      this.label4.Name = "label4";
      this.label4.Size = new Size(41, 12);
      this.label4.TabIndex = 7;
      this.label4.Text = "表示名";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(23, 39);
      this.label3.Name = "label3";
      this.label3.Size = new Size(55, 12);
      this.label3.TabIndex = 6;
      this.label3.Text = "グル\x30FCプ名";
      this.textBox2.BorderStyle = BorderStyle.FixedSingle;
      this.textBox2.Location = new Point(92, 63);
      this.textBox2.MaxLength = 32;
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(109, 19);
      this.textBox2.TabIndex = 1;
      this.textBox1.BorderStyle = BorderStyle.FixedSingle;
      this.textBox1.Location = new Point(92, 36);
      this.textBox1.MaxLength = 32;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(109, 19);
      this.textBox1.TabIndex = 0;
      this.managedPanel4.Controls.Add((Control) this.groupBox10);
      this.managedPanel4.Location = new Point(0, 0);
      this.managedPanel4.Name = "managedPanel4";
      this.managedPanel4.Size = new Size(0, 0);
      this.managedPanel4.Text = "managedPanel4";
      this.groupBox10.Controls.Add((Control) this.tabControl1);
      this.groupBox10.Location = new Point(0, 3);
      this.groupBox10.Name = "groupBox10";
      this.groupBox10.Size = new Size(410, 199);
      this.groupBox10.TabIndex = 0;
      this.groupBox10.TabStop = false;
      this.groupBox10.Text = "表示項目";
      this.tabControl1.Controls.Add((Control) this.tabPage1);
      this.tabControl1.Controls.Add((Control) this.tabPage2);
      this.tabControl1.Controls.Add((Control) this.tabPage3);
      this.tabControl1.Controls.Add((Control) this.tabPage4);
      this.tabControl1.ImageList = this.imageList1;
      this.tabControl1.Location = new Point(6, 18);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.ShowToolTips = true;
      this.tabControl1.Size = new Size(398, 172);
      this.tabControl1.TabIndex = 1;
      this.tabPage1.Controls.Add((Control) this.checkBox105);
      this.tabPage1.Controls.Add((Control) this.checkBox104);
      this.tabPage1.Controls.Add((Control) this.checkBox103);
      this.tabPage1.Controls.Add((Control) this.checkBox102);
      this.tabPage1.Controls.Add((Control) this.checkBox101);
      this.tabPage1.Controls.Add((Control) this.checkBox100);
      this.tabPage1.ImageIndex = 0;
      this.tabPage1.Location = new Point(4, 23);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(390, 145);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "@Webアイコン";
      this.tabPage1.ToolTipText = "@Webアイコン表示時に表示する項目を設定します";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.checkBox105.AutoSize = true;
      this.checkBox105.ImageIndex = 9;
      this.checkBox105.ImageList = this.imageList1;
      this.checkBox105.Location = new Point(111, 94);
      this.checkBox105.Name = "checkBox105";
      this.checkBox105.Size = new Size(88, 16);
      this.checkBox105.TabIndex = 5;
      this.checkBox105.Text = "磁場異常";
      this.checkBox105.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox105.UseVisualStyleBackColor = true;
      this.checkBox104.AutoSize = true;
      this.checkBox104.ImageIndex = 8;
      this.checkBox104.ImageList = this.imageList1;
      this.checkBox104.Location = new Point(111, 72);
      this.checkBox104.Name = "checkBox104";
      this.checkBox104.Size = new Size(87, 16);
      this.checkBox104.TabIndex = 4;
      this.checkBox104.Text = "セイレ\x30FCン";
      this.checkBox104.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox104.UseVisualStyleBackColor = true;
      this.checkBox103.AutoSize = true;
      this.checkBox103.ImageIndex = 7;
      this.checkBox103.ImageList = this.imageList1;
      this.checkBox103.Location = new Point(111, 50);
      this.checkBox103.Name = "checkBox103";
      this.checkBox103.Size = new Size(52, 16);
      this.checkBox103.TabIndex = 3;
      this.checkBox103.Text = "藻";
      this.checkBox103.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox103.UseVisualStyleBackColor = true;
      this.checkBox102.AutoSize = true;
      this.checkBox102.ImageIndex = 6;
      this.checkBox102.ImageList = this.imageList1;
      this.checkBox102.Location = new Point(111, 28);
      this.checkBox102.Name = "checkBox102";
      this.checkBox102.Size = new Size(64, 16);
      this.checkBox102.TabIndex = 2;
      this.checkBox102.Text = "火災";
      this.checkBox102.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox102.UseVisualStyleBackColor = true;
      this.checkBox101.AutoSize = true;
      this.checkBox101.ImageIndex = 5;
      this.checkBox101.ImageList = this.imageList1;
      this.checkBox101.Location = new Point(111, 6);
      this.checkBox101.Name = "checkBox101";
      this.checkBox101.Size = new Size(58, 16);
      this.checkBox101.TabIndex = 1;
      this.checkBox101.Text = "サメ";
      this.checkBox101.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox101.UseVisualStyleBackColor = true;
      this.checkBox100.AutoSize = true;
      this.checkBox100.ImageIndex = 4;
      this.checkBox100.ImageList = this.imageList1;
      this.checkBox100.Location = new Point(6, 6);
      this.checkBox100.Name = "checkBox100";
      this.checkBox100.Size = new Size(97, 16);
      this.checkBox100.TabIndex = 0;
      this.checkBox100.Text = "風向き全般";
      this.checkBox100.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox100.UseVisualStyleBackColor = true;
      this.tabPage2.Controls.Add((Control) this.checkBox212);
      this.tabPage2.Controls.Add((Control) this.checkBox211);
      this.tabPage2.Controls.Add((Control) this.checkBox210);
      this.tabPage2.Controls.Add((Control) this.checkBox209);
      this.tabPage2.Controls.Add((Control) this.checkBox208);
      this.tabPage2.Controls.Add((Control) this.checkBox207);
      this.tabPage2.Controls.Add((Control) this.checkBox206);
      this.tabPage2.Controls.Add((Control) this.checkBox205);
      this.tabPage2.Controls.Add((Control) this.checkBox204);
      this.tabPage2.Controls.Add((Control) this.checkBox203);
      this.tabPage2.Controls.Add((Control) this.checkBox202);
      this.tabPage2.Controls.Add((Control) this.checkBox201);
      this.tabPage2.Controls.Add((Control) this.checkBox200);
      this.tabPage2.ImageIndex = 1;
      this.tabPage2.Location = new Point(4, 23);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new Padding(3);
      this.tabPage2.Size = new Size(390, 145);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "メモアイコン";
      this.tabPage2.ToolTipText = "メモアイコン表示時に表示する項目を設定します";
      this.tabPage2.UseVisualStyleBackColor = true;
      this.checkBox212.AutoSize = true;
      this.checkBox212.ImageIndex = 15;
      this.checkBox212.ImageList = this.imageList1;
      this.checkBox212.Location = new Point(223, 116);
      this.checkBox212.Name = "checkBox212";
      this.checkBox212.Size = new Size(76, 16);
      this.checkBox212.TabIndex = 18;
      this.checkBox212.Text = "目的地";
      this.checkBox212.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox212.UseVisualStyleBackColor = true;
      this.checkBox211.AutoSize = true;
      this.checkBox211.ImageIndex = 14;
      this.checkBox211.ImageList = this.imageList1;
      this.checkBox211.Location = new Point(223, 94);
      this.checkBox211.Name = "checkBox211";
      this.checkBox211.Size = new Size(60, 16);
      this.checkBox211.TabIndex = 17;
      this.checkBox211.Text = "free";
      this.checkBox211.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox211.UseVisualStyleBackColor = true;
      this.checkBox210.AutoSize = true;
      this.checkBox210.ImageIndex = 13;
      this.checkBox210.ImageList = this.imageList1;
      this.checkBox210.Location = new Point(223, 72);
      this.checkBox210.Name = "checkBox210";
      this.checkBox210.Size = new Size(60, 16);
      this.checkBox210.TabIndex = 16;
      this.checkBox210.Text = "free";
      this.checkBox210.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox210.UseVisualStyleBackColor = true;
      this.checkBox209.AutoSize = true;
      this.checkBox209.ImageIndex = 12;
      this.checkBox209.ImageList = this.imageList1;
      this.checkBox209.Location = new Point(223, 50);
      this.checkBox209.Name = "checkBox209";
      this.checkBox209.Size = new Size(60, 16);
      this.checkBox209.TabIndex = 15;
      this.checkBox209.Text = "free";
      this.checkBox209.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox209.UseVisualStyleBackColor = true;
      this.checkBox208.AutoSize = true;
      this.checkBox208.ImageIndex = 11;
      this.checkBox208.ImageList = this.imageList1;
      this.checkBox208.Location = new Point(223, 28);
      this.checkBox208.Name = "checkBox208";
      this.checkBox208.Size = new Size(60, 16);
      this.checkBox208.TabIndex = 14;
      this.checkBox208.Text = "free";
      this.checkBox208.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox208.UseVisualStyleBackColor = true;
      this.checkBox207.AutoSize = true;
      this.checkBox207.ImageIndex = 1;
      this.checkBox207.ImageList = this.imageList1;
      this.checkBox207.Location = new Point(223, 6);
      this.checkBox207.Name = "checkBox207";
      this.checkBox207.Size = new Size(77, 16);
      this.checkBox207.TabIndex = 13;
      this.checkBox207.Text = "マンボウ";
      this.checkBox207.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox207.UseVisualStyleBackColor = true;
      this.checkBox206.AutoSize = true;
      this.checkBox206.ImageIndex = 10;
      this.checkBox206.ImageList = this.imageList1;
      this.checkBox206.Location = new Point(111, 116);
      this.checkBox206.Name = "checkBox206";
      this.checkBox206.Size = new Size(64, 16);
      this.checkBox206.TabIndex = 12;
      this.checkBox206.Text = "漁場";
      this.checkBox206.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox206.UseVisualStyleBackColor = true;
      this.checkBox205.AutoSize = true;
      this.checkBox205.ImageIndex = 9;
      this.checkBox205.ImageList = this.imageList1;
      this.checkBox205.Location = new Point(111, 94);
      this.checkBox205.Name = "checkBox205";
      this.checkBox205.Size = new Size(88, 16);
      this.checkBox205.TabIndex = 11;
      this.checkBox205.Text = "磁場異常";
      this.checkBox205.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox205.UseVisualStyleBackColor = true;
      this.checkBox204.AutoSize = true;
      this.checkBox204.ImageIndex = 8;
      this.checkBox204.ImageList = this.imageList1;
      this.checkBox204.Location = new Point(111, 72);
      this.checkBox204.Name = "checkBox204";
      this.checkBox204.Size = new Size(87, 16);
      this.checkBox204.TabIndex = 10;
      this.checkBox204.Text = "セイレ\x30FCン";
      this.checkBox204.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox204.UseVisualStyleBackColor = true;
      this.checkBox203.AutoSize = true;
      this.checkBox203.ImageIndex = 7;
      this.checkBox203.ImageList = this.imageList1;
      this.checkBox203.Location = new Point(111, 50);
      this.checkBox203.Name = "checkBox203";
      this.checkBox203.Size = new Size(52, 16);
      this.checkBox203.TabIndex = 9;
      this.checkBox203.Text = "藻";
      this.checkBox203.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox203.UseVisualStyleBackColor = true;
      this.checkBox202.AutoSize = true;
      this.checkBox202.ImageIndex = 6;
      this.checkBox202.ImageList = this.imageList1;
      this.checkBox202.Location = new Point(111, 28);
      this.checkBox202.Name = "checkBox202";
      this.checkBox202.Size = new Size(64, 16);
      this.checkBox202.TabIndex = 8;
      this.checkBox202.Text = "火災";
      this.checkBox202.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox202.UseVisualStyleBackColor = true;
      this.checkBox201.AutoSize = true;
      this.checkBox201.ImageIndex = 5;
      this.checkBox201.ImageList = this.imageList1;
      this.checkBox201.Location = new Point(111, 6);
      this.checkBox201.Name = "checkBox201";
      this.checkBox201.Size = new Size(58, 16);
      this.checkBox201.TabIndex = 7;
      this.checkBox201.Text = "サメ";
      this.checkBox201.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox201.UseVisualStyleBackColor = true;
      this.checkBox200.AutoSize = true;
      this.checkBox200.ImageIndex = 4;
      this.checkBox200.ImageList = this.imageList1;
      this.checkBox200.Location = new Point(6, 6);
      this.checkBox200.Name = "checkBox200";
      this.checkBox200.Size = new Size(97, 16);
      this.checkBox200.TabIndex = 6;
      this.checkBox200.Text = "風向き全般";
      this.checkBox200.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox200.UseVisualStyleBackColor = true;
      this.tabPage3.Controls.Add((Control) this.checkBox310);
      this.tabPage3.Controls.Add((Control) this.checkBox309);
      this.tabPage3.Controls.Add((Control) this.checkBox308);
      this.tabPage3.Controls.Add((Control) this.checkBox307);
      this.tabPage3.Controls.Add((Control) this.checkBox306);
      this.tabPage3.Controls.Add((Control) this.checkBox305);
      this.tabPage3.Controls.Add((Control) this.checkBox304);
      this.tabPage3.Controls.Add((Control) this.checkBox303);
      this.tabPage3.Controls.Add((Control) this.checkBox302);
      this.tabPage3.Controls.Add((Control) this.checkBox301);
      this.tabPage3.Controls.Add((Control) this.checkBox300);
      this.tabPage3.ImageIndex = 2;
      this.tabPage3.Location = new Point(4, 23);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Padding = new Padding(3);
      this.tabPage3.Size = new Size(390, 145);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "災害";
      this.tabPage3.ToolTipText = "災害アイコン表示時に表示する項目を設定します";
      this.tabPage3.UseVisualStyleBackColor = true;
      this.checkBox310.AutoSize = true;
      this.checkBox310.ImageIndex = 21;
      this.checkBox310.ImageList = this.imageList1;
      this.checkBox310.Location = new Point(106, 94);
      this.checkBox310.Name = "checkBox310";
      this.checkBox310.Size = new Size(94, 16);
      this.checkBox310.TabIndex = 23;
      this.checkBox310.Text = "何かいい物";
      this.checkBox310.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox310.UseVisualStyleBackColor = true;
      this.checkBox309.AutoSize = true;
      this.checkBox309.ImageIndex = 20;
      this.checkBox309.ImageList = this.imageList1;
      this.checkBox309.Location = new Point(106, 72);
      this.checkBox309.Name = "checkBox309";
      this.checkBox309.Size = new Size(64, 16);
      this.checkBox309.TabIndex = 22;
      this.checkBox309.Text = "戦闘";
      this.checkBox309.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox309.UseVisualStyleBackColor = true;
      this.checkBox308.AutoSize = true;
      this.checkBox308.ImageIndex = 19;
      this.checkBox308.ImageList = this.imageList1;
      this.checkBox308.Location = new Point(106, 50);
      this.checkBox308.Name = "checkBox308";
      this.checkBox308.Size = new Size(141, 16);
      this.checkBox308.TabIndex = 21;
      this.checkBox308.Text = "得体の知れない怪物";
      this.checkBox308.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox308.UseVisualStyleBackColor = true;
      this.checkBox307.AutoSize = true;
      this.checkBox307.ImageIndex = 18;
      this.checkBox307.ImageList = this.imageList1;
      this.checkBox307.Location = new Point(106, 28);
      this.checkBox307.Name = "checkBox307";
      this.checkBox307.Size = new Size(67, 16);
      this.checkBox307.TabIndex = 20;
      this.checkBox307.Text = "ネズミ";
      this.checkBox307.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox307.UseVisualStyleBackColor = true;
      this.checkBox306.AutoSize = true;
      this.checkBox306.ImageIndex = 17;
      this.checkBox306.ImageList = this.imageList1;
      this.checkBox306.Location = new Point(106, 6);
      this.checkBox306.Name = "checkBox306";
      this.checkBox306.Size = new Size(64, 16);
      this.checkBox306.TabIndex = 19;
      this.checkBox306.Text = "吹雪";
      this.checkBox306.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox306.UseVisualStyleBackColor = true;
      this.checkBox305.AutoSize = true;
      this.checkBox305.ImageIndex = 16;
      this.checkBox305.ImageList = this.imageList1;
      this.checkBox305.Location = new Point(6, 116);
      this.checkBox305.Name = "checkBox305";
      this.checkBox305.Size = new Size(52, 16);
      this.checkBox305.TabIndex = 18;
      this.checkBox305.Text = "嵐";
      this.checkBox305.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox305.UseVisualStyleBackColor = true;
      this.checkBox304.AutoSize = true;
      this.checkBox304.ImageIndex = 9;
      this.checkBox304.ImageList = this.imageList1;
      this.checkBox304.Location = new Point(6, 94);
      this.checkBox304.Name = "checkBox304";
      this.checkBox304.Size = new Size(88, 16);
      this.checkBox304.TabIndex = 17;
      this.checkBox304.Text = "磁場異常";
      this.checkBox304.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox304.UseVisualStyleBackColor = true;
      this.checkBox303.AutoSize = true;
      this.checkBox303.ImageIndex = 8;
      this.checkBox303.ImageList = this.imageList1;
      this.checkBox303.Location = new Point(6, 72);
      this.checkBox303.Name = "checkBox303";
      this.checkBox303.Size = new Size(87, 16);
      this.checkBox303.TabIndex = 16;
      this.checkBox303.Text = "セイレ\x30FCン";
      this.checkBox303.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox303.UseVisualStyleBackColor = true;
      this.checkBox302.AutoSize = true;
      this.checkBox302.ImageIndex = 7;
      this.checkBox302.ImageList = this.imageList1;
      this.checkBox302.Location = new Point(6, 50);
      this.checkBox302.Name = "checkBox302";
      this.checkBox302.Size = new Size(52, 16);
      this.checkBox302.TabIndex = 15;
      this.checkBox302.Text = "藻";
      this.checkBox302.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox302.UseVisualStyleBackColor = true;
      this.checkBox301.AutoSize = true;
      this.checkBox301.ImageIndex = 6;
      this.checkBox301.ImageList = this.imageList1;
      this.checkBox301.Location = new Point(6, 28);
      this.checkBox301.Name = "checkBox301";
      this.checkBox301.Size = new Size(64, 16);
      this.checkBox301.TabIndex = 14;
      this.checkBox301.Text = "火災";
      this.checkBox301.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox301.UseVisualStyleBackColor = true;
      this.checkBox300.AutoSize = true;
      this.checkBox300.ImageIndex = 5;
      this.checkBox300.ImageList = this.imageList1;
      this.checkBox300.Location = new Point(6, 6);
      this.checkBox300.Name = "checkBox300";
      this.checkBox300.Size = new Size(58, 16);
      this.checkBox300.TabIndex = 13;
      this.checkBox300.Text = "サメ";
      this.checkBox300.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox300.UseVisualStyleBackColor = true;
      this.tabPage4.Controls.Add((Control) this.checkBox403);
      this.tabPage4.Controls.Add((Control) this.checkBox401);
      this.tabPage4.Controls.Add((Control) this.checkBox402);
      this.tabPage4.Controls.Add((Control) this.checkBox400);
      this.tabPage4.ImageIndex = 3;
      this.tabPage4.Location = new Point(4, 23);
      this.tabPage4.Name = "tabPage4";
      this.tabPage4.Padding = new Padding(3);
      this.tabPage4.Size = new Size(390, 145);
      this.tabPage4.TabIndex = 3;
      this.tabPage4.Text = "航路予想線";
      this.tabPage4.ToolTipText = "航路予想線表示時に表示する項目を設定します";
      this.tabPage4.UseVisualStyleBackColor = true;
      this.checkBox403.AutoSize = true;
      this.checkBox403.ImageList = this.imageList1;
      this.checkBox403.Location = new Point(24, 49);
      this.checkBox403.Name = "checkBox403";
      this.checkBox403.Size = new Size(251, 16);
      this.checkBox403.TabIndex = 2;
      this.checkBox403.Text = "一時的に測量スキルが途切れたときの予想位置";
      this.checkBox403.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox403.UseVisualStyleBackColor = true;
      this.checkBox401.AutoSize = true;
      this.checkBox401.ImageList = this.imageList1;
      this.checkBox401.Location = new Point(24, 28);
      this.checkBox401.Name = "checkBox401";
      this.checkBox401.Size = new Size(169, 16);
      this.checkBox401.TabIndex = 1;
      this.checkBox401.Text = "速度から求めた到達予想位置";
      this.checkBox401.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox401.UseVisualStyleBackColor = true;
      this.checkBox402.AutoSize = true;
      this.checkBox402.ImageIndex = 3;
      this.checkBox402.ImageList = this.imageList1;
      this.checkBox402.Location = new Point(6, 71);
      this.checkBox402.Name = "checkBox402";
      this.checkBox402.Size = new Size(218, 16);
      this.checkBox402.TabIndex = 3;
      this.checkBox402.Text = "過去の位置から解析した航路予想線";
      this.checkBox402.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox402.UseVisualStyleBackColor = true;
      this.checkBox400.AutoSize = true;
      this.checkBox400.ImageIndex = 9;
      this.checkBox400.ImageList = this.imageList1;
      this.checkBox400.Location = new Point(6, 6);
      this.checkBox400.Name = "checkBox400";
      this.checkBox400.Size = new Size(172, 16);
      this.checkBox400.TabIndex = 0;
      this.checkBox400.Text = "コンパスから解析した予想線";
      this.checkBox400.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.checkBox400.UseVisualStyleBackColor = true;
      this.checkBox400.CheckedChanged += new EventHandler(this.checkBox400_CheckedChanged);
      this.managedPanel5.Controls.Add((Control) this.groupBox11);
      this.managedPanel5.Location = new Point(0, 0);
      this.managedPanel5.Name = "managedPanel5";
      this.managedPanel5.Size = new Size(410, 313);
      this.managedPanel5.Text = "managedPanel5";
      this.groupBox11.Controls.Add((Control) this.checkBox7);
      this.groupBox11.Controls.Add((Control) this.checkBox4);
      this.groupBox11.Controls.Add((Control) this.checkBox16);
      this.groupBox11.Controls.Add((Control) this.checkBox13);
      this.groupBox11.Controls.Add((Control) this.checkBox2);
      this.groupBox11.Controls.Add((Control) this.checkBox12);
      this.groupBox11.Controls.Add((Control) this.checkBox10);
      this.groupBox11.Controls.Add((Control) this.checkBox11);
      this.groupBox11.Location = new Point(0, 3);
      this.groupBox11.Name = "groupBox11";
      this.groupBox11.Size = new Size(410, 208);
      this.groupBox11.TabIndex = 15;
      this.groupBox11.TabStop = false;
      this.groupBox11.Text = "その他";
      this.checkBox4.AutoSize = true;
      this.checkBox4.Location = new Point(6, 160);
      this.checkBox4.Name = "checkBox4";
      this.checkBox4.Size = new Size(215, 16);
      this.checkBox4.TabIndex = 13;
      this.checkBox4.Text = "造船中でなくても造船カウンタを表示する";
      this.checkBox4.UseVisualStyleBackColor = true;
      this.checkBox16.AutoSize = true;
      this.checkBox16.Location = new Point(6, 42);
      this.checkBox16.Name = "checkBox16";
      this.checkBox16.Size = new Size(234, 16);
      this.checkBox16.TabIndex = 8;
      this.checkBox16.Text = "交易MAP@Webアイコンの描画を最適化する";
      this.checkBox16.UseVisualStyleBackColor = true;
      this.checkBox13.AutoSize = true;
      this.checkBox13.Location = new Point(6, 114);
      this.checkBox13.Name = "checkBox13";
      this.checkBox13.Size = new Size(174, 16);
      this.checkBox13.TabIndex = 11;
      this.checkBox13.Text = "線描画時のギザギザを軽減する";
      this.checkBox13.UseVisualStyleBackColor = true;
      this.checkBox2.AutoSize = true;
      this.checkBox2.Location = new Point(6, 66);
      this.checkBox2.Name = "checkBox2";
      this.checkBox2.Size = new Size(245, 16);
      this.checkBox2.TabIndex = 9;
      this.checkBox2.Text = "マウスの戻る/進むボタンでスキル/道具窓を開く";
      this.checkBox2.UseVisualStyleBackColor = true;
      this.checkBox12.AutoSize = true;
      this.checkBox12.Location = new Point(6, 138);
      this.checkBox12.Name = "checkBox12";
      this.checkBox12.Size = new Size(187, 16);
      this.checkBox12.TabIndex = 12;
      this.checkBox12.Text = "ウインドウを常に最前面に表示する";
      this.checkBox12.UseVisualStyleBackColor = true;
      this.checkBox10.AutoSize = true;
      this.checkBox10.Location = new Point(6, 18);
      this.checkBox10.Name = "checkBox10";
      this.checkBox10.Size = new Size(227, 16);
      this.checkBox10.TabIndex = 7;
      this.checkBox10.Text = "右クリックの動作をWindows標準に合わせる";
      this.checkBox10.UseVisualStyleBackColor = true;
      this.checkBox11.AutoSize = true;
      this.checkBox11.Location = new Point(6, 90);
      this.checkBox11.Name = "checkBox11";
      this.checkBox11.Size = new Size(225, 16);
      this.checkBox11.TabIndex = 10;
      this.checkBox11.Text = "お気に入り航路と合成した地図を使用する";
      this.checkBox11.UseVisualStyleBackColor = true;
      this.managedPanel6.Controls.Add((Control) this.groupBox12);
      this.managedPanel6.Location = new Point(0, 0);
      this.managedPanel6.Name = "managedPanel6";
      this.managedPanel6.Size = new Size(410, 313);
      this.managedPanel6.Text = "managedPanel6";
      this.groupBox12.Controls.Add((Control) this.button4);
      this.groupBox12.Controls.Add((Control) this.groupBox18);
      this.groupBox12.Controls.Add((Control) this.linkLabel1);
      this.groupBox12.Controls.Add((Control) this.label2);
      this.groupBox12.Controls.Add((Control) this.label5);
      this.groupBox12.Controls.Add((Control) this.pictureBox1);
      this.groupBox12.Controls.Add((Control) this.groupBox13);
      this.groupBox12.Location = new Point(0, 3);
      this.groupBox12.Name = "groupBox12";
      this.groupBox12.Size = new Size(410, 307);
      this.groupBox12.TabIndex = 0;
      this.groupBox12.TabStop = false;
      this.groupBox12.Text = "バ\x30FCジョン情報";
      this.button4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.button4.Location = new Point(230, 132);
      this.button4.Name = "button4";
      this.button4.Size = new Size(174, 23);
      this.button4.TabIndex = 25;
      this.button4.Text = "ソフトウェアの更新を確認";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new EventHandler(this.button4_Click);
      this.groupBox18.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupBox18.Controls.Add((Control) this.label1);
      this.groupBox18.Controls.Add((Control) this.linkLabel2);
      this.groupBox18.Location = new Point(6, 75);
      this.groupBox18.Name = "groupBox18";
      this.groupBox18.Size = new Size(398, 51);
      this.groupBox18.TabIndex = 24;
      this.groupBox18.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 32);
      this.label1.Name = "label1";
      this.label1.Size = new Size(357, 12);
      this.label1.TabIndex = 17;
      this.label1.Text = "が公開されている 交易MAP C# 版 を冒険者向けに機能拡張したものです";
      this.linkLabel2.AutoSize = true;
      this.linkLabel2.LinkArea = new LinkArea(10, 18);
      this.linkLabel2.Location = new Point(6, 15);
      this.linkLabel2.Name = "linkLabel2";
      this.linkLabel2.Size = new Size(277, 17);
      this.linkLabel2.TabIndex = 16;
      this.linkLabel2.TabStop = true;
      this.linkLabel2.Text = "このソフトウェアは 大航海時代Online ツ\x30FCル配布所 さん";
      this.linkLabel2.UseCompatibleTextRendering = true;
      this.linkLabel2.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.Location = new Point(56, 60);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new Size(41, 12);
      this.linkLabel1.TabIndex = 23;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "リンク先";
      this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(56, 38);
      this.label2.Name = "label2";
      this.label2.Size = new Size(228, 12);
      this.label2.TabIndex = 22;
      this.label2.Text = "Copyright (C) 2009-2011  cookie@Zephyros";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(56, 18);
      this.label5.Name = "label5";
      this.label5.Size = new Size(35, 12);
      this.label5.TabIndex = 21;
      this.label5.Text = "label5";
      this.pictureBox1.Image = (Image) Resources.gvicon;
      this.pictureBox1.Location = new Point(6, 18);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.TabIndex = 20;
      this.pictureBox1.TabStop = false;
      this.groupBox13.Anchor = AnchorStyles.Bottom;
      this.groupBox13.Controls.Add((Control) this.textBox4);
      this.groupBox13.Location = new Point(6, 202);
      this.groupBox13.Name = "groupBox13";
      this.groupBox13.Size = new Size(398, 99);
      this.groupBox13.TabIndex = 14;
      this.groupBox13.TabStop = false;
      this.groupBox13.Text = "デバイス情報";
      this.textBox4.BorderStyle = BorderStyle.None;
      this.textBox4.Location = new Point(8, 17);
      this.textBox4.Multiline = true;
      this.textBox4.Name = "textBox4";
      this.textBox4.ReadOnly = true;
      this.textBox4.ScrollBars = ScrollBars.Both;
      this.textBox4.Size = new Size(384, 73);
      this.textBox4.TabIndex = 0;
      this.managedPanel7.Controls.Add((Control) this.groupBox20);
      this.managedPanel7.Controls.Add((Control) this.groupBox19);
      this.managedPanel7.Controls.Add((Control) this.groupBox2);
      this.managedPanel7.Location = new Point(0, 0);
      this.managedPanel7.Name = "managedPanel7";
      this.managedPanel7.Size = new Size(410, 313);
      this.managedPanel7.Text = "managedPanel7";
      this.groupBox20.Controls.Add((Control) this.label8);
      this.groupBox20.Controls.Add((Control) this.textBox9);
      this.groupBox20.Location = new Point(0, 165);
      this.groupBox20.Name = "groupBox20";
      this.groupBox20.Size = new Size(411, 46);
      this.groupBox20.TabIndex = 7;
      this.groupBox20.TabStop = false;
      this.groupBox20.Text = "過去の航路図(非表示)";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(3, 23);
      this.label8.Name = "label8";
      this.label8.Size = new Size(77, 12);
      this.label8.TabIndex = 15;
      this.label8.Text = "航路図保持数";
      this.textBox9.BorderStyle = BorderStyle.FixedSingle;
      this.textBox9.ImeMode = ImeMode.Off;
      this.textBox9.Location = new Point(141, 18);
      this.textBox9.Name = "textBox9";
      this.textBox9.Size = new Size(83, 19);
      this.textBox9.TabIndex = 14;
      this.textBox9.TextAlign = HorizontalAlignment.Right;
      this.groupBox19.Controls.Add((Control) this.checkBox19);
      this.groupBox19.Controls.Add((Control) this.checkBox18);
      this.groupBox19.Location = new Point(0, 96);
      this.groupBox19.Name = "groupBox19";
      this.groupBox19.Size = new Size(410, 63);
      this.groupBox19.TabIndex = 6;
      this.groupBox19.TabStop = false;
      this.groupBox19.Text = "お気に入り航路図";
      this.checkBox19.AutoSize = true;
      this.checkBox19.Location = new Point(6, 40);
      this.checkBox19.Name = "checkBox19";
      this.checkBox19.Size = new Size(151, 16);
      this.checkBox19.TabIndex = 3;
      this.checkBox19.Text = "災害ポップアップを描画する";
      this.checkBox19.UseVisualStyleBackColor = true;
      this.checkBox18.AutoSize = true;
      this.checkBox18.Location = new Point(6, 18);
      this.checkBox18.Name = "checkBox18";
      this.checkBox18.Size = new Size(113, 16);
      this.checkBox18.TabIndex = 2;
      this.checkBox18.Text = "半透明で描画する";
      this.checkBox18.UseVisualStyleBackColor = true;
      this.groupBox2.Controls.Add((Control) this.label7);
      this.groupBox2.Controls.Add((Control) this.textBox8);
      this.groupBox2.Controls.Add((Control) this.checkBox14);
      this.groupBox2.Controls.Add((Control) this.label6);
      this.groupBox2.Controls.Add((Control) this.textBox3);
      this.groupBox2.FlatStyle = FlatStyle.Flat;
      this.groupBox2.Location = new Point(0, 3);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(410, 87);
      this.groupBox2.TabIndex = 5;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "航路図";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(6, 43);
      this.label7.Name = "label7";
      this.label7.Size = new Size(120, 12);
      this.label7.TabIndex = 15;
      this.label7.Text = "描画する最低航海日数";
      this.textBox8.BorderStyle = BorderStyle.FixedSingle;
      this.textBox8.ImeMode = ImeMode.Off;
      this.textBox8.Location = new Point(144, 39);
      this.textBox8.Name = "textBox8";
      this.textBox8.Size = new Size(83, 19);
      this.textBox8.TabIndex = 14;
      this.textBox8.TextAlign = HorizontalAlignment.Right;
      this.checkBox14.AutoSize = true;
      this.checkBox14.Location = new Point(6, 64);
      this.checkBox14.Name = "checkBox14";
      this.checkBox14.Size = new Size(218, 16);
      this.checkBox14.TabIndex = 1;
      this.checkBox14.Text = "一番新しい航路図以外を半透明で描画";
      this.checkBox14.UseVisualStyleBackColor = true;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(6, 19);
      this.label6.Name = "label6";
      this.label6.Size = new Size(77, 12);
      this.label6.TabIndex = 13;
      this.label6.Text = "航路図保持数";
      this.textBox3.BorderStyle = BorderStyle.FixedSingle;
      this.textBox3.ImeMode = ImeMode.Off;
      this.textBox3.Location = new Point(144, 14);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new Size(83, 19);
      this.textBox3.TabIndex = 0;
      this.textBox3.TextAlign = HorizontalAlignment.Right;
      this.managedPanel8.Controls.Add((Control) this.groupBox21);
      this.managedPanel8.Location = new Point(0, 0);
      this.managedPanel8.Name = "managedPanel8";
      this.managedPanel8.Size = new Size(410, 313);
      this.managedPanel8.Text = "managedPanel8";
      this.groupBox21.Controls.Add((Control) this.button6);
      this.groupBox21.Controls.Add((Control) this.button5);
      this.groupBox21.Controls.Add((Control) this.listView1);
      this.groupBox21.Controls.Add((Control) this.button3);
      this.groupBox21.Controls.Add((Control) this.comboBox9);
      this.groupBox21.Location = new Point(0, 3);
      this.groupBox21.Name = "groupBox21";
      this.groupBox21.Size = new Size(410, 307);
      this.groupBox21.TabIndex = 0;
      this.groupBox21.TabStop = false;
      this.groupBox21.Text = "キ\x30FCボ\x30FCド割り当て";
      this.button6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.button6.Location = new Point(252, 274);
      this.button6.Name = "button6";
      this.button6.Size = new Size(117, 27);
      this.button6.TabIndex = 7;
      this.button6.Text = "全て初期値に戻す";
      this.button6.UseVisualStyleBackColor = true;
      this.button5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.button5.Location = new Point(129, 274);
      this.button5.Name = "button5";
      this.button5.Size = new Size(117, 27);
      this.button5.TabIndex = 6;
      this.button5.Text = "割り当て解除";
      this.button5.UseVisualStyleBackColor = true;
      this.button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.button3.Location = new Point(6, 274);
      this.button3.Name = "button3";
      this.button3.Size = new Size(117, 27);
      this.button3.TabIndex = 5;
      this.button3.Text = "割り当て";
      this.button3.UseVisualStyleBackColor = true;
      this.comboBox9.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.comboBox9.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox9.FormattingEnabled = true;
      this.comboBox9.Location = new Point(6, 18);
      this.comboBox9.Name = "comboBox9";
      this.comboBox9.Size = new Size(398, 20);
      this.comboBox9.TabIndex = 4;
      this.checkBox7.AutoSize = true;
      this.checkBox7.Location = new Point(6, 182);
      this.checkBox7.Name = "checkBox7";
      this.checkBox7.Size = new Size(304, 16);
      this.checkBox7.TabIndex = 14;
      this.checkBox7.Text = "情報ウインドウの座標表示を地図座標系にする(デバッグ用)";
      this.checkBox7.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.button1;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.button2;
      this.ClientSize = new Size(591, 365);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.listBox1);
      this.Controls.Add((Control) this.panelManager1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "setting_form2";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "設定";
      this.FormClosed += new FormClosedEventHandler(this.setting_form2_FormClosed);
      this.panelManager1.ResumeLayout(false);
      this.managedPanel1.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox8.ResumeLayout(false);
      this.groupBox7.ResumeLayout(false);
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.managedPanel2.ResumeLayout(false);
      this.groupBox5.ResumeLayout(false);
      this.groupBox5.PerformLayout();
      this.groupBox14.ResumeLayout(false);
      this.groupBox14.PerformLayout();
      this.groupBox17.ResumeLayout(false);
      this.groupBox17.PerformLayout();
      this.groupBox15.ResumeLayout(false);
      this.groupBox15.PerformLayout();
      this.groupBox16.ResumeLayout(false);
      this.managedPanel3.ResumeLayout(false);
      this.groupBox9.ResumeLayout(false);
      this.groupBox9.PerformLayout();
      this.groupBox6.ResumeLayout(false);
      this.groupBox6.PerformLayout();
      this.managedPanel4.ResumeLayout(false);
      this.groupBox10.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.tabPage2.PerformLayout();
      this.tabPage3.ResumeLayout(false);
      this.tabPage3.PerformLayout();
      this.tabPage4.ResumeLayout(false);
      this.tabPage4.PerformLayout();
      this.managedPanel5.ResumeLayout(false);
      this.groupBox11.ResumeLayout(false);
      this.groupBox11.PerformLayout();
      this.managedPanel6.ResumeLayout(false);
      this.groupBox12.ResumeLayout(false);
      this.groupBox12.PerformLayout();
      this.groupBox18.ResumeLayout(false);
      this.groupBox18.PerformLayout();
      this.pictureBox1.EndInit();
      this.groupBox13.ResumeLayout(false);
      this.groupBox13.PerformLayout();
      this.managedPanel7.ResumeLayout(false);
      this.groupBox20.ResumeLayout(false);
      this.groupBox20.PerformLayout();
      this.groupBox19.ResumeLayout(false);
      this.groupBox19.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.managedPanel8.ResumeLayout(false);
      this.groupBox21.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public enum tab_index
    {
      general,
      sea_routes,
      capture_log,
      access_network,
      draw_flags,
      other,
      version,
    }
  }
}
