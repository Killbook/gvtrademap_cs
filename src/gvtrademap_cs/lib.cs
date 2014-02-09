/*-------------------------------------------------------------------------

 よく使うものをまとめたもの
 DirectXの関係から最後にDispose()を呼ぶこと
 (少なくともテクスチャ等を破棄した後Dispose()が呼ばれなくてはならない)

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using directx;
using System.Text;
using System.Reflection;
using Utility.Ini;
using Utility.KeyAssign;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/// <summary>
	/// キー割り当て機能
	/// </summary>
	public enum KeyFunction{
		map_change,
		map_reset_scale,
		map_zoom_in,
		map_zoom_out,
		blue_line_reset,
		cancel_spot,
		cancel_select_sea_routes,
		item_window_show_min,
		item_window_next_tab,
		item_window_prev_tab,
		setting_window_show_min,
		setting_window_button_00,
		setting_window_button_01,
		setting_window_button_02,
		setting_window_button_03,
		setting_window_button_04,
		setting_window_button_05,
		setting_window_button_06,
		setting_window_button_07,
		setting_window_button_08,
		setting_window_button_09,
		setting_window_button_10,
		setting_window_button_11,
		setting_window_button_12,
		setting_window_button_13,
		setting_window_button_exec_gvoac,		// 海域情報収集の起動
		folder_open_00,
		folder_open_01,
		folder_open_02,
		folder_open_03,
		window_top_most,
		window_maximize,
		window_minimize,
		window_change_border_style,
		window_close,

		unknown_function	= 65535,			// 定義されていない機能
	};

	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class gvt_lib : IDisposable
	{
		private d3d_device			m_d3d_device;			// 描画
		private LoopXImage		m_loop_x_image;			// 地図管理
		private icons				m_icons;				// アイコン管理
		private infonameimage		m_infonameimage;		// 街等の文字の絵管理
		private seainfonameimage	m_seainfonameimage;		// 海域の文字の絵管理
		private setting				m_setting;				// 設定項目
		private IniProfileSetting	m_ini_manager;			// 設定の読み書き管理
		private KeyAssignManager	m_key_assign_manager;	// キー割り当て管理

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public LoopXImage loop_image{				get{	return m_loop_x_image;			}}
		public d3d_device device{					get{	return m_d3d_device;			}}
		public icons icons{							get{	return m_icons;					}}
		public infonameimage infonameimage{			get{	return m_infonameimage;			}}
		public seainfonameimage seainfonameimage{	get{	return m_seainfonameimage;		}}
		public setting setting{						get{	return m_setting;				}}
		public IniProfileSetting IniManager{		get{	return m_ini_manager;			}}
		public KeyAssignManager KeyAssignManager{	get{	return m_key_assign_manager;	}}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public gvt_lib(System.Windows.Forms.Form form, string ini_file_name)
		{
			// 設定項目の読み書き管理
			m_ini_manager				= new IniProfileSetting(ini_file_name);

			// 設定項目
			m_setting					= new setting();
			// キー割り当て管理
			m_key_assign_manager		= new KeyAssignManager();

			// 登録
			m_ini_manager.AddIniSaveLoad(m_setting);
			m_ini_manager.AddIniSaveLoad(m_key_assign_manager, "key_assign");
	
			// メインウインドウ描画用
			m_d3d_device				= new d3d_device(form);
			m_d3d_device.skip_max		= def.SKIP_DRAW_FRAME_MAX;		// 描画スキップ数

			// 地図管理
			m_loop_x_image				= new LoopXImage(m_d3d_device);

			// アイコン管理
			m_icons						= new icons(m_d3d_device, def.ICONSIMAGE_FULLNAME);
			// 街等の文字の絵管理
			m_infonameimage				= new infonameimage(m_d3d_device, def.INFONAMEIMAGE_FULLNAME);
			// 海域の文字の絵管理
			m_seainfonameimage			= new seainfonameimage(m_d3d_device, def.SEAINFONAMEIMAGE_FULLNAME);

			// キー割り当て初期化
			init_key_assign();
		}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public void Dispose()
		{
			if(m_loop_x_image != null)		m_loop_x_image.Dispose();
			if(m_icons != null)				m_icons.Dispose();
			if(m_infonameimage != null)		m_infonameimage.Dispose();
			if(m_seainfonameimage != null)	m_seainfonameimage.Dispose();

			// 最後にD3Ddeviceを破棄する
			if(m_d3d_device != null)		m_d3d_device.Dispose();
	
			m_loop_x_image		= null;
			m_icons				= null;
			m_infonameimage		= null;
			m_seainfonameimage	= null;

			m_d3d_device		= null;
		}

		/// <summary>
		/// キー割り当て初期化
		/// </summary>
		private void init_key_assign()
		{
			m_key_assign_manager.List.AddAssign("地図の変更", "地図",Keys.M,													KeyFunction.map_change);
			m_key_assign_manager.List.AddAssign("地図の縮尺リセット", "地図", Keys.Home,										KeyFunction.map_reset_scale);
			m_key_assign_manager.List.AddAssign("地図を拡大", "地図", Keys.Add,												KeyFunction.map_zoom_in);
			m_key_assign_manager.List.AddAssign("地図を縮小", "地図", Keys.Subtract,											KeyFunction.map_zoom_out);
			m_key_assign_manager.List.AddAssign("ブルーラインリセット", "進行方向線", Keys.B,								KeyFunction.blue_line_reset);
			m_key_assign_manager.List.AddAssign("スポット解除", "解除", Keys.Escape,											KeyFunction.cancel_spot);
			m_key_assign_manager.List.AddAssign("航路図の選択解除", "解除", Keys.Escape,										KeyFunction.cancel_select_sea_routes);
			m_key_assign_manager.List.AddAssign("アイテムウインドウの表示/最小化", "アイテムウインドウ", Keys.None,		KeyFunction.item_window_show_min);
			m_key_assign_manager.List.AddAssign("次のタブへ移動", "アイテムウインドウ", Keys.Tab|Keys.Control,				KeyFunction.item_window_next_tab);
			m_key_assign_manager.List.AddAssign("前のタブへ移動", "アイテムウインドウ", Keys.Tab|Keys.Control|Keys.Shift,	KeyFunction.item_window_prev_tab);
			m_key_assign_manager.List.AddAssign("設定ウインドウの表示/最小化", "設定ウインドウ", Keys.None,				KeyFunction.setting_window_show_min);
			m_key_assign_manager.List.AddAssign("航路記録ON/OFF", "設定ウインドウ", Keys.None,								KeyFunction.setting_window_button_00);
			m_key_assign_manager.List.AddAssign("共有している船表示ON/OFF", "設定ウインドウ", Keys.None,					KeyFunction.setting_window_button_01);
			m_key_assign_manager.List.AddAssign("@Webアイコン表示ON/OFF", "設定ウインドウ", Keys.None,						KeyFunction.setting_window_button_02);
			m_key_assign_manager.List.AddAssign("メモアイコン表示ON/OFF", "設定ウインドウ", Keys.None,						KeyFunction.setting_window_button_03);
			m_key_assign_manager.List.AddAssign("航路線表示ON/OFF", "設定ウインドウ", Keys.None,								KeyFunction.setting_window_button_04);
			m_key_assign_manager.List.AddAssign("日付ふきだし表示切り替え", "設定ウインドウ", Keys.None,					KeyFunction.setting_window_button_05);
			m_key_assign_manager.List.AddAssign("災害ポップアップ表示ON/OFF", "設定ウインドウ", Keys.None,					KeyFunction.setting_window_button_06);
			m_key_assign_manager.List.AddAssign("現在位置中心表示ON/OFF", "設定ウインドウ", Keys.None,						KeyFunction.setting_window_button_07);
			m_key_assign_manager.List.AddAssign("進行方向線表示ON/OFF", "設定ウインドウ", Keys.None,							KeyFunction.setting_window_button_08);
			m_key_assign_manager.List.AddAssign("海域変動システムの設定", "設定ウインドウ", Keys.None,						KeyFunction.setting_window_button_09);
			m_key_assign_manager.List.AddAssign("航路図のスクリーンショット", "設定ウインドウ", Keys.None,					KeyFunction.setting_window_button_10);
			m_key_assign_manager.List.AddAssign("航路図一覧を開く", "設定ウインドウ", Keys.L|Keys.Control,					KeyFunction.setting_window_button_11);
			m_key_assign_manager.List.AddAssign("できるだけ検索を開く", "設定ウインドウ", Keys.F|Keys.Control,				KeyFunction.setting_window_button_12);
			m_key_assign_manager.List.AddAssign("設定ダイアログを開く", "設定ウインドウ", Keys.None,						KeyFunction.setting_window_button_13);
			m_key_assign_manager.List.AddAssign("海域情報収集を起動する", "設定ウインドウ", Keys.A|Keys.Control,			KeyFunction.setting_window_button_exec_gvoac);
			m_key_assign_manager.List.AddAssign("航路図のスクリーンショットフォルダを開く", "フォルダ", Keys.None,		KeyFunction.folder_open_00);
			m_key_assign_manager.List.AddAssign("大航海時代Onlineのメールフォルダを開く", "フォルダ", Keys.None,			KeyFunction.folder_open_01);
			m_key_assign_manager.List.AddAssign("大航海時代Onlineのチャットフォルダを開く", "フォルダ", Keys.None,			KeyFunction.folder_open_02);
			m_key_assign_manager.List.AddAssign("大航海時代Onlineのスクリーンショットフォルダを開く", "フォルダ", Keys.None,	KeyFunction.folder_open_03);
			m_key_assign_manager.List.AddAssign("最前面表示ON/OFF", "ウインドウ", Keys.None,									KeyFunction.window_top_most);
			m_key_assign_manager.List.AddAssign("最大化/通常化", "ウインドウ", Keys.None,										KeyFunction.window_maximize);
			m_key_assign_manager.List.AddAssign("最小化", "ウインドウ", Keys.None,											KeyFunction.window_minimize);
			m_key_assign_manager.List.AddAssign("ウインドウ枠の表示/非表示", "ウインドウ", Keys.None,						KeyFunction.window_change_border_style);
			m_key_assign_manager.List.AddAssign("交易MAP C#を閉じる", "ウインドウ", Keys.None,								KeyFunction.window_close);
		}
	}
}
