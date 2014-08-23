/*-------------------------------------------------------------------------

 設定ダイアログ2
 カテゴリ毎に分けた
 表示項目を合体させた
 バージョン情報を合体させた
 海域変動は合体させてない

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Utility;
using Utility.KeyAssign;
using net_base;
using System.Net;
using System.Diagnostics;
using System.IO;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public partial class setting_form2 : Form
	{
		public enum tab_index{
			general,
			sea_routes,
			capture_log,
			access_network,
			draw_flags,
			other,
			version,
		};
	
		private	GlobalSettings						m_setting;
		private KeyAssignList				m_key_assign_list;
		private KeyAssiginSettingHelper		m_key_assign_helper;

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public GlobalSettings setting{					get{	return m_setting;			}}
		public KeyAssignList KeyAssignList{		get{	return m_key_assign_list;	}}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public setting_form2(GlobalSettings _setting, KeyAssignList assign_list, string device_info)
		{
			init(_setting, assign_list, device_info, tab_index.general, DrawSettingPage.WebIcons);
		}
		// 描画項目設定時用
		public setting_form2(GlobalSettings _setting, KeyAssignList assign_list, string device_info, DrawSettingPage _draw_setting_page)
		{
			init(_setting, assign_list, device_info, tab_index.draw_flags, _draw_setting_page);
		}
		// ページ指定
		public setting_form2(GlobalSettings _setting, KeyAssignList assign_list, string device_info, tab_index _tab_index)
		{
			init(_setting, assign_list, device_info, _tab_index, DrawSettingPage.WebIcons);
		}

		/*-------------------------------------------------------------------------
		 初期化
		---------------------------------------------------------------------------*/
		private void init(GlobalSettings _setting, KeyAssignList assign_list, string device_info, tab_index index, DrawSettingPage page)
		{
			// 設定内容をコピーして持つ
			m_setting				= _setting.Clone();
			m_key_assign_list		= assign_list.DeepClone();

			InitializeComponent();
			Useful.SetFontMeiryo(this, 8f);
			Useful.SetFontMeiryo(listBox1, 9f);

			// ツールチップを登録する
			toolTip1.AutoPopDelay	= 30*1000;		// 30秒表示
			toolTip1.BackColor		= Color.LightYellow;
			toolTip1.SetToolTip(comboBox2, "プレイしているサーバを選択します");
			toolTip1.SetToolTip(comboBox3, "属している国を選択します");
			toolTip1.SetToolTip(comboBox1, "地図を選択します");
			toolTip1.SetToolTip(comboBox5, "緯度、経度線の描画方法を選択します\n単位は測量で得られる値です\n初期値は座標のみ描画です");
			toolTip1.SetToolTip(textBox1, "航路共有用のグループ名を指定します\n空白にすると航路共有されません");
			toolTip1.SetToolTip(textBox2, "航路共有描画時に表示される名前を指定します\n指定した名前を航路共有する他のメンバーに伝えます\n空白にすると航路共有されません");
			toolTip1.SetToolTip(checkBox8, "チェックを入れると街名、街アイコン等をできるだけ等倍で表示します。\nチェックを外すと地図の縮尺に合わせて拡縮されて表示します。\nチェックを外したほうが描画が軽くなります。");
			toolTip1.SetToolTip(checkBox1, "インターネットに接続するかどうかを指定します\nチェックを入れると起動時のデータ更新、航路共有が有効になります");
			toolTip1.SetToolTip(checkBox2, "マウスの戻る・進むボタンでスキル・道具窓を開きます");
			toolTip1.SetToolTip(checkBox6, "航路共有を有効にする場合はチェックを入れてください\nインターネットから更新情報等を受け取るにチェックを入れている必要があります");
			toolTip1.SetToolTip(checkBox9, "起動時インターネットから@Webアイコンを取得します\n取得した@Webアイコンはローカルに保存されます\n起動時に毎回取得する必要がない場合はチェックをはずしてください");
			toolTip1.SetToolTip(comboBox6, "街アイコンのサイズを選択します。\n海岸線がアイコンで隠れるのがいやな方は小さいアイコンを選択してください。");
			toolTip1.SetToolTip(comboBox7, "街名等を描画するかどうかを選択します。\n描画しない場合はマウスを乗せると街名がポップアップします。");
			toolTip1.SetToolTip(comboBox8, "スクリーンショットのフォーマットを選択します。\n初期値はbmpです。");
			toolTip1.SetToolTip(checkBox4, "造船中でなくても造船カウンタを表示するかどうかを設定します");

			string	str		= "右クリック時の動作を選択します\n";
			str				+= "チェック有\n";
			str				+= "  右クリックでコンテキストメニューが開く\n";
			str				+= "  右クリックでも街を選択できる\n";
			str				+= "  スポット解除はESCキーのみ\n";
			str				+= "チェックなし\n";
			str				+= "  Ctrl+右クリックでコンテキストメニューが開く\n";
			str				+= "  右クリックでは街を選択できない\n";
			str				+= "  スポット解除はESCキーかどこかの街を選択\n";
			toolTip1.SetToolTip(checkBox10, str);
			toolTip1.SetToolTip(checkBox11, "お気に入り航路と合成した地図を使用します\nこの項目はお気に入り航路の使用／不使用切り替えのために用意されています");
			toolTip1.SetToolTip(checkBox12, "ウインドウを常に最前面に表示します");
			toolTip1.SetToolTip(checkBox13, "航路図、コンパスの角度線、進路予想線の描画方法を指定します\nチェックを入れた場合アンチエイリアスで描画されます");
			toolTip1.SetToolTip(checkBox14, "一番新しい航路図以外を半透明で描画します\n日付、災害ポップアップも半透明になります");
			toolTip1.SetToolTip(checkBox16, "@Webアイコン描画時に同じ種類で距離が近い場合、1つにまとめます。\n@Webアイコン表示時のごちゃごちゃした感じを軽減します。");

            toolTip1.SetToolTip(checkBox3, "画面キャプチャ方法を指定します\nWindows Vistaを使用していて航路図がうまく書かれない場合チェックを入れてください。\nWindows7ではこのチェックを入れる必要はありません。");
			toolTip1.SetToolTip(checkBox5, "災害ポップアップ、利息からの経過日数、海域変動システム用にログ解析を行います");
			toolTip1.SetToolTip(checkBox17, "キャプチャした画像を右に表示します\nコンパス解析の角度ずれの確認用です\n通常はチェックを入れる必要はありません");
			toolTip1.SetToolTip(comboBox4, "画面キャプチャ間隔を選択します\n短い間隔でキャプチャするほどコンパスの角度のレスポンスがよくなりますがCPU時間を多く消費します\nCPUに余裕がある場合は0.5秒に1回を選択してください\nさらにCPUに余裕がある場合は0.25秒に1回を選択してください\n初期値は1秒に1回です");
			toolTip1.SetToolTip(textBox6, "TCPサーバが使用するポート番号を指定します\n特に変更する必要はありません");
			toolTip1.SetToolTip(checkBox7, "情報ウインドウの座標系を測量系ではなく地図系にします\n開発時の位置取得用です");
			
			str		= "描画する最低航海日数を指定します\n";
			str		+= "この設定は最も新しい航路図には影響を与えません\n";
			str		+= "狭い範囲を航海すると航路図がごちゃごちゃしてしまうのを軽減できます\n";
			str		+= "例えば3に設定すると航海日数2日以下の航路図は描画されなくなります\n";
			str		+= "0に設定すると全ての航路図が描画されます\n";
			str		+= "初期値は0です";
			toolTip1.SetToolTip(textBox8, str);

			toolTip1.SetToolTip(checkBox18, "お気に入り航路図を半透明で描画します");
			toolTip1.SetToolTip(checkBox19, "お気に入り航路図の災害ポップアップを描画します");
			str		= "過去の航路図を保持する数を設定します\n";
			str		+= "過去の航路図は描画されないため、CPU負荷が低く多くの航路図を保持しても問題ありません\n";
			str		+= "初期値は200です";
			toolTip1.SetToolTip(textBox9, str);
	
			str		= "航路図を保持する数を指定します\n";
			str		+= "保持数を多くすると描画負荷が増えます\n";
			str		+= "海に出る度に航路図を全て削除している方は1を指定してください\n";
			str		+= "初期値は20です";
			toolTip1.SetToolTip(textBox3, str);

			// 設定項目を反映させる
			comboBox2.SelectedIndex	= (int)m_setting.server;
            comboBox3.SelectedItem  = GvoWorldInfo.GetCountryString(this.m_setting.country);
			comboBox1.SelectedIndex	= (int)m_setting.map;
			comboBox5.SelectedIndex	= (int)m_setting.tude_interval;
			comboBox6.SelectedIndex	= (int)m_setting.map_icon;
			comboBox7.SelectedIndex	= (int)m_setting.map_draw_names;
			comboBox8.SelectedIndex	= (int)m_setting.ss_format;
			textBox1.Text			= m_setting.share_group;
			textBox2.Text			= m_setting.share_group_myname;
			textBox3.Text			= m_setting.searoutes_group_max.ToString();
			textBox9.Text			= m_setting.trash_searoutes_group_max.ToString();
			textBox8.Text			= m_setting.minimum_draw_days.ToString();
			checkBox1.Checked		= m_setting.connect_network;
			checkBox2.Checked		= m_setting.hook_mouse;
			checkBox6.Checked		= m_setting.is_share_routes;
			checkBox9.Checked		= m_setting.connect_web_icon;
			checkBox10.Checked		= m_setting.compatible_windows_rclick;
			checkBox11.Checked		= m_setting.use_mixed_map;
			checkBox12.Checked		= m_setting.window_top_most;
			checkBox13.Checked		= m_setting.enable_line_antialias;
			checkBox14.Checked		= m_setting.enable_sea_routes_aplha;
			checkBox16.Checked		= m_setting.remove_near_web_icons;
			checkBox8.Checked		= !m_setting.is_mixed_info_names;
			checkBox18.Checked		= m_setting.enable_favorite_sea_routes_alpha;
			checkBox19.Checked		= m_setting.draw_favorite_sea_routes_alpha_popup;
			checkBox4.Checked		= m_setting.force_show_build_ship;
			checkBox7.Checked		= m_setting.debug_flag_show_potision;
            checkBox15.Checked      = m_setting.enable_dpi_scaling;
	
			if(m_setting.capture_interval == CaptureIntervalIndex.Per250ms){
				comboBox4.SelectedIndex	= 0;
			}else{
				comboBox4.SelectedIndex	= (int)m_setting.capture_interval + 1;
			}
			checkBox3.Checked		= m_setting.windows_vista_aero;
			checkBox5.Checked		= m_setting.enable_analize_log_chat;
			checkBox17.Checked		= m_setting.draw_capture_info;

			textBox6.Text			= m_setting.port_index.ToString();
			try{
				string	host_name		= net_useful.GetHostName();
				textBox5.AppendText(host_name + "\n");
				IPAddress[]	list		= net_useful.GetLocalIpAddress_Ipv4();
				if(   (list != null)
					&&(list.Length > 0) ){
					textBox5.AppendText(list[0].ToString());
				}
			}catch{
				textBox5.AppendText("PC名\n");
				textBox5.AppendText("IPアドレスの取得に失敗");
			}

			// モード設定
			if(m_setting.is_server_mode){
				radioButton2.Checked	= true;	
			}else{
				radioButton1.Checked	= true;	
			}
	
			// デバイス情報
			textBox4.Lines			= device_info.Split(new string[]{"\n", "\r\n"}, StringSplitOptions.None);

			// バージョン情報
			label5.Text				= def.WINDOW_TITLE;
	
			// 表示項目の初期化
			init_draw_setting(page);
	
			// 有効、無効の更新
			update_gray_ctrl();
	
			// 初期ページ
			listBox1.SelectedIndex	= (int)index;

			// HP
			linkLabel1.Text			= def.URL_HP;

			// キー割り当て
			m_key_assign_helper		= new KeyAssiginSettingHelper(	m_key_assign_list,
																		this, comboBox9, listView1, button3, button5, button6);
		}

		/*-------------------------------------------------------------------------
		 表示項目の初期化
		---------------------------------------------------------------------------*/
		private void init_draw_setting(DrawSettingPage page)
		{
			// @Web icons
			{
				DrawSettingWebIcons	flag	= m_setting.draw_setting_web_icons;
				checkBox100.Checked			= (flag & DrawSettingWebIcons.wind) != 0;
				checkBox101.Checked			= (flag & DrawSettingWebIcons.accident_0) != 0;
				checkBox102.Checked			= (flag & DrawSettingWebIcons.accident_1) != 0;
				checkBox103.Checked			= (flag & DrawSettingWebIcons.accident_2) != 0;
				checkBox104.Checked			= (flag & DrawSettingWebIcons.accident_3) != 0;
				checkBox105.Checked			= (flag & DrawSettingWebIcons.accident_4) != 0;
			}
			// Memo icons
			{
				DrawSettingMemoIcons	flag	= m_setting.draw_setting_memo_icons;
				checkBox200.Checked			= (flag & DrawSettingMemoIcons.wind) != 0;
				checkBox201.Checked			= (flag & DrawSettingMemoIcons.memo_0) != 0;
				checkBox202.Checked			= (flag & DrawSettingMemoIcons.memo_1) != 0;
				checkBox203.Checked			= (flag & DrawSettingMemoIcons.memo_2) != 0;
				checkBox204.Checked			= (flag & DrawSettingMemoIcons.memo_3) != 0;
				checkBox205.Checked			= (flag & DrawSettingMemoIcons.memo_4) != 0;
				checkBox206.Checked			= (flag & DrawSettingMemoIcons.memo_5) != 0;
				checkBox207.Checked			= (flag & DrawSettingMemoIcons.memo_6) != 0;
				checkBox208.Checked			= (flag & DrawSettingMemoIcons.memo_7) != 0;
				checkBox209.Checked			= (flag & DrawSettingMemoIcons.memo_8) != 0;
				checkBox210.Checked			= (flag & DrawSettingMemoIcons.memo_9) != 0;
				checkBox211.Checked			= (flag & DrawSettingMemoIcons.memo_10) != 0;
				checkBox212.Checked			= (flag & DrawSettingMemoIcons.memo_11) != 0;
			}
			// 災害
			{
				DrawSettingAccidents	flag	= m_setting.draw_setting_accidents;
				checkBox300.Checked			= (flag & DrawSettingAccidents.accident_0) != 0;
				checkBox301.Checked			= (flag & DrawSettingAccidents.accident_1) != 0;
				checkBox302.Checked			= (flag & DrawSettingAccidents.accident_2) != 0;
				checkBox303.Checked			= (flag & DrawSettingAccidents.accident_3) != 0;
				checkBox304.Checked			= (flag & DrawSettingAccidents.accident_4) != 0;
				checkBox305.Checked			= (flag & DrawSettingAccidents.accident_5) != 0;
				checkBox306.Checked			= (flag & DrawSettingAccidents.accident_6) != 0;
				checkBox307.Checked			= (flag & DrawSettingAccidents.accident_7) != 0;
				checkBox308.Checked			= (flag & DrawSettingAccidents.accident_8) != 0;
				checkBox309.Checked			= (flag & DrawSettingAccidents.accident_9) != 0;
				checkBox310.Checked			= (flag & DrawSettingAccidents.accident_10) != 0;
			}
			// 予想線
			{
				DrawSettingMyShipAngle	flag	= m_setting.draw_setting_myship_angle;
				checkBox400.Checked			= (flag & DrawSettingMyShipAngle.draw_0) != 0;
				checkBox401.Checked			= m_setting.draw_setting_myship_angle_with_speed_pos;
				checkBox402.Checked			= (flag & DrawSettingMyShipAngle.draw_1) != 0;
				checkBox403.Checked			= m_setting.draw_setting_myship_expect_pos;
			}

			// 表示するページの設定
			if((int)page < 0)							page	= DrawSettingPage.WebIcons;
			if(page > DrawSettingPage.MyShipAngle)	page	= DrawSettingPage.MyShipAngle;
			tabControl1.SelectTab((int)page);
		}	
		
		/*-------------------------------------------------------------------------
		 リストボックスの選択内容が変更された
		---------------------------------------------------------------------------*/
		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			// どうもパネルの順番が一定ではない感じなので
			// 直接指定する
			switch(listBox1.SelectedIndex){
			case 0:		// 基本設定
				panelManager1.SelectedPanel		= managedPanel1;
				break;
			case 1:		// 航路図設定
				panelManager1.SelectedPanel		= managedPanel7;
				break;
			case 2:		// キャプチャ、ログ解析
				panelManager1.SelectedPanel		= managedPanel2;
				break;
			case 3:		// ネット接続、航路共有
				panelManager1.SelectedPanel		= managedPanel3;
				break;
			case 4:		// 表示項目
				panelManager1.SelectedPanel		= managedPanel4;
				break;
			case 5:		// その他
				panelManager1.SelectedPanel		= managedPanel5;
				break;
			case 6:		// キーボード割り当て
				panelManager1.SelectedPanel		= managedPanel8;
				break;
			case 7:		// バージョン情報
				panelManager1.SelectedPanel		= managedPanel6;
				break;
			}
		}

		/*-------------------------------------------------------------------------
		 閉じられた
		---------------------------------------------------------------------------*/
		private void setting_form2_FormClosed(object sender, FormClosedEventArgs e)
		{
            m_setting.server                    = GvoWorldInfo.GetServerFromString(comboBox2.Text);
            m_setting.country                   = GvoWorldInfo.GetCountryFromString(comboBox3.Text);
			m_setting.map						= MapIndex.Map1 + comboBox1.SelectedIndex;
			m_setting.map_icon					= MapIcon.Big + comboBox6.SelectedIndex;
			m_setting.map_draw_names			= MapDrawNames.Draw + comboBox7.SelectedIndex;
			m_setting.ss_format					= SSFormat.Bmp + comboBox8.SelectedIndex;

			m_setting.tude_interval				= TudeInterval.None + comboBox5.SelectedIndex;
			m_setting.share_group				= textBox1.Text;
			m_setting.share_group_myname		= textBox2.Text;
			m_setting.connect_network			= checkBox1.Checked;
			m_setting.hook_mouse				= checkBox2.Checked;
			m_setting.is_share_routes			= checkBox6.Checked;
			m_setting.connect_web_icon			= checkBox9.Checked;
			m_setting.compatible_windows_rclick	= checkBox10.Checked;
			m_setting.use_mixed_map				= checkBox11.Checked;
			m_setting.window_top_most			= checkBox12.Checked;
			m_setting.enable_line_antialias		= checkBox13.Checked;
			m_setting.enable_sea_routes_aplha	= checkBox14.Checked;
			m_setting.remove_near_web_icons		= checkBox16.Checked;
			m_setting.is_mixed_info_names		= !checkBox8.Checked;
			m_setting.enable_favorite_sea_routes_alpha		= checkBox18.Checked;
			m_setting.draw_favorite_sea_routes_alpha_popup	= checkBox19.Checked;
			m_setting.force_show_build_ship		= checkBox4.Checked;
			m_setting.debug_flag_show_potision	= checkBox7.Checked;
            m_setting.enable_dpi_scaling        = checkBox15.Checked;

			m_setting.searoutes_group_max		= Useful.ToInt32(textBox3.Text, -1);
			m_setting.trash_searoutes_group_max	= Useful.ToInt32(textBox9.Text, -1);
			m_setting.minimum_draw_days			= Useful.ToInt32(textBox8.Text, -1);

			m_setting.is_server_mode			= radioButton2.Checked;
			m_setting.port_index				= Useful.ToInt32(textBox6.Text, def.DEFALUT_PORT_INDEX);

			if(comboBox4.SelectedIndex == 0){
				m_setting.capture_interval		= CaptureIntervalIndex.Per250ms;
			}else{
				m_setting.capture_interval		= CaptureIntervalIndex.Per500ms + (comboBox4.SelectedIndex - 1);
			}
			m_setting.windows_vista_aero		= checkBox3.Checked;
			m_setting.enable_analize_log_chat	= checkBox5.Checked;
			m_setting.draw_capture_info			= checkBox17.Checked;

			// 表示項目の保存
			save_draw_setting();

			// キー割り当て
			m_key_assign_list					= m_key_assign_helper.List;
		}

		/*-------------------------------------------------------------------------
		 表示項目の保存
		---------------------------------------------------------------------------*/
		private void save_draw_setting()
		{
			{
				DrawSettingWebIcons	flag	= 0;
				flag		|= (checkBox100.Checked)? DrawSettingWebIcons.wind: 0;
				flag		|= (checkBox101.Checked)? DrawSettingWebIcons.accident_0: 0;
				flag		|= (checkBox102.Checked)? DrawSettingWebIcons.accident_1: 0;
				flag		|= (checkBox103.Checked)? DrawSettingWebIcons.accident_2: 0;
				flag		|= (checkBox104.Checked)? DrawSettingWebIcons.accident_3: 0;
				flag		|= (checkBox105.Checked)? DrawSettingWebIcons.accident_4: 0;
				m_setting.draw_setting_web_icons	= flag;
			}
			{
				DrawSettingMemoIcons	flag	= 0;
				flag		|= (checkBox200.Checked)? DrawSettingMemoIcons.wind: 0;
				flag		|= (checkBox201.Checked)? DrawSettingMemoIcons.memo_0: 0;
				flag		|= (checkBox202.Checked)? DrawSettingMemoIcons.memo_1: 0;
				flag		|= (checkBox203.Checked)? DrawSettingMemoIcons.memo_2: 0;
				flag		|= (checkBox204.Checked)? DrawSettingMemoIcons.memo_3: 0;
				flag		|= (checkBox205.Checked)? DrawSettingMemoIcons.memo_4: 0;
				flag		|= (checkBox206.Checked)? DrawSettingMemoIcons.memo_5: 0;
				flag		|= (checkBox207.Checked)? DrawSettingMemoIcons.memo_6: 0;
				flag		|= (checkBox208.Checked)? DrawSettingMemoIcons.memo_7: 0;
				flag		|= (checkBox209.Checked)? DrawSettingMemoIcons.memo_8: 0;
				flag		|= (checkBox210.Checked)? DrawSettingMemoIcons.memo_9: 0;
				flag		|= (checkBox211.Checked)? DrawSettingMemoIcons.memo_10: 0;
				flag		|= (checkBox212.Checked)? DrawSettingMemoIcons.memo_11: 0;
				m_setting.draw_setting_memo_icons	= flag;
			}
			{
				DrawSettingAccidents	flag	= 0;
				flag		|= (checkBox300.Checked)? DrawSettingAccidents.accident_0: 0;
				flag		|= (checkBox301.Checked)? DrawSettingAccidents.accident_1: 0;
				flag		|= (checkBox302.Checked)? DrawSettingAccidents.accident_2: 0;
				flag		|= (checkBox303.Checked)? DrawSettingAccidents.accident_3: 0;
				flag		|= (checkBox304.Checked)? DrawSettingAccidents.accident_4: 0;
				flag		|= (checkBox305.Checked)? DrawSettingAccidents.accident_5: 0;
				flag		|= (checkBox306.Checked)? DrawSettingAccidents.accident_6: 0;
				flag		|= (checkBox307.Checked)? DrawSettingAccidents.accident_7: 0;
				flag		|= (checkBox308.Checked)? DrawSettingAccidents.accident_8: 0;
				flag		|= (checkBox309.Checked)? DrawSettingAccidents.accident_9: 0;
				flag		|= (checkBox310.Checked)? DrawSettingAccidents.accident_10: 0;
				m_setting.draw_setting_accidents	= flag;
			}
			{
				DrawSettingMyShipAngle	flag	= 0;
				flag		|= (checkBox400.Checked)? DrawSettingMyShipAngle.draw_0: 0;
				flag		|= (checkBox402.Checked)? DrawSettingMyShipAngle.draw_1: 0;
				m_setting.draw_setting_myship_angle	= flag;
				m_setting.draw_setting_myship_angle_with_speed_pos	= checkBox401.Checked;
				m_setting.draw_setting_myship_expect_pos	= checkBox403.Checked;
			}
		}
	
		/*-------------------------------------------------------------------------
		 有効、無効の更新
		---------------------------------------------------------------------------*/
		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			update_gray_ctrl();
		}
		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			update_gray_ctrl();
		}
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			update_gray_ctrl();
		}
		private void checkBox6_CheckedChanged(object sender, EventArgs e)
		{
			update_gray_ctrl();
		}
		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{
			update_gray_ctrl();
		}
        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            update_gray_ctrl();
        }
        private void checkBox400_CheckedChanged(object sender, EventArgs e)
		{
			update_gray_ctrl();
		}

		/*-------------------------------------------------------------------------
		 グレイ表示を更新する
		---------------------------------------------------------------------------*/
		private void update_gray_ctrl()
		{
			// 航路共有
			if(checkBox1.Checked){
				checkBox6.Enabled	= true;
				checkBox9.Enabled	= true;
			}else{
				checkBox6.Enabled	= false;
				checkBox9.Enabled	= false;
			}

			if(checkBox1.Checked && checkBox6.Checked){
				textBox1.Enabled	= true;
				textBox2.Enabled	= true;
			}else{
				textBox1.Enabled	= false;
				textBox2.Enabled	= false;
			}

			// モード
			if(radioButton1.Checked){
				// 通常モード
				comboBox4.Enabled	= true;
				checkBox3.Enabled	= true;
				checkBox5.Enabled	= true;
				checkBox17.Enabled	= true;
				textBox6.Enabled	= false;
			}else{
				// TCPサーバモード
				comboBox4.Enabled	= false;
				checkBox3.Enabled	= false;
				checkBox5.Enabled	= false;
				checkBox17.Enabled	= false;
				textBox6.Enabled	= true;
			}

			// 表示項目
			if(checkBox400.Checked){
				checkBox401.Enabled		= true;
				checkBox403.Enabled		= true;
			}else{
				checkBox401.Enabled		= false;
				checkBox403.Enabled		= false;
			}
		}

		/*-------------------------------------------------------------------------
		 HPを開く
		---------------------------------------------------------------------------*/
		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(def.URL_HP);
			linkLabel1.LinkVisited	= true;
		}

		/*-------------------------------------------------------------------------
		 HPを開く
		 大航海時代Online ツール配布所
		---------------------------------------------------------------------------*/
		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(def.URL_HP_ORIGINAL);
			linkLabel2.LinkVisited	= true;
		}

		/*-------------------------------------------------------------------------
		 最新バージョンチェック
		---------------------------------------------------------------------------*/
		private void button4_Click(object sender, EventArgs e)
		{
			Cursor	= Cursors.WaitCursor;
			// バージョンを確認する
			bool			result	= HttpDownload.Download(def.VERSION_URL, def.VERSION_FNAME);
			Cursor	= Cursors.Default;

			if(!result){
				// 更新しない
				MessageBox.Show(this, "更新情報が取得できませんでした。\nインターネットの接続を確認してください。", "更新確認エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
	
			// 更新されたデータがあるかどうか確認する
			string	line	= "";
			List<string>	data	= new List<string>();
			int		version	= 0;
			try{
				using (StreamReader	sr	= new StreamReader(
					def.VERSION_FNAME, Encoding.GetEncoding("Shift_JIS"))){

					// バージョン
					line	= sr.ReadLine();
					version	= Convert.ToInt32(line);

					// 残り
					while((line = sr.ReadLine()) != null){
						data.Add(line);
					}
				}
			}catch{
				MessageBox.Show(this, "バージョン情報が確認できません。\n更新確認に失敗しました。", "更新確認エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if(version > def.VERSION){
				// 更新されている
				check_update_result		dlg		= new check_update_result(data.ToArray());
				dlg.ShowDialog(this);
				dlg.Dispose();
			}else{
				// 更新されていない
				MessageBox.Show(this, "更新されたソフトウェアは見つかりませんでした。\nお使いのバージョンが最新です。", "更新確認結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
	}
}
