/*-------------------------------------------------------------------------

 main form

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 define
---------------------------------------------------------------------------*/
// CPU使用状況を表示する
//#define	DEBUG_DRAW_CPU_BAR
// コンパス解析デバッグ
//#define	DEBUG_COMPASS
// 共有航路デバッグ
//#define	DEBUG_SHARE_ROUTES
// 進行方向線デバッグ
//#define	DEBUG_ANGLE_LINE
// デバッグ文字列の表示
//#define	DEBUG_DRAW_DEBUG_STRING

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using directx;
using gvo_base;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Utility;
using win32;

using net_base;
using gvo_net_base;
using Utility.KeyAssign;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public partial class gvtrademap_cs_form : Form
	{
		// タイマ間隔
		private const int			CHAT_LOG_TIMER_INTERVAL	= 5100;		// 5.1秒間隔
		private const int			SHARE_TIMER_INTERVAL	= 60*1000;	// 1分間隔
		
		// ツールチップ表示までの時間
//		private const int			TOOLTIP_INITIAL			= 7;		// 1/60単位
		private const int			TOOLTIP_INITIAL			= 15;		// 1/60単位

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		// カレントパス
		private string				m_current_path;

		// 位置情報
		private Point				m_old_mouse_pos;	// 前回のマウス位置
		private Point				m_mouse_move;		// マウスの移動量
		private myship_info			m_myship_info;		// 自分の船の情報

		private Point				m_select_pos;		// マーカーを置いた位置

		// 現在の地図情報
		private	MapIndex			m_map_index;		// 地図番号
		private bool				m_use_mixed_map;	// お気に入り航路と合成した地図を使用する

		private bool				m_pause;			// 最小化中はtrue

		// 	
		private gvt_lib				m_lib;				// よく使う機能をまとめたもの
		private GvoDatabase			m_db;				// 情報管理
	
		// ウインドウ管理
		// 描画関係
		private d3d_windows			m_windows;			// ウインドウ管理
		private	item_window			m_item_window;		// 左のウインドウ
		private setting_window		m_setting_window;	// 設定ボタンウインドウ
		private spot				m_spot;				// スポット表示
		private info_windows		m_info_windows;		// 情報表示ウインドウ管理

		// 検索ダイアログ
		private find_form2			m_find_form;		// 検索ダイアログ
		private RequestCtrl			m_req_show_find_form;
		// 航路図一覧ダイアログ
		private sea_routes_form2	m_sea_routes_form;
		private RequestCtrl			m_req_sea_routes_form;

		// マウスフック
		private globalmouse_hook	m_mouse_hook;		// マウスフック

		// ツールチップ
		private ToolTip				m_tooltip;			// ツールチップ
		private int					m_tooltip_interval;	// ツールチップ表示までのカウンタ
		private bool				m_show_tooltip;		// ツールチップ表示中のときtrue
		private Point				m_tooltip_old_mouse_pos;

		// スレッド
		private ManualResetEvent	m_exit_thread_event;	// スレッド終了イベント管理
		private Thread				m_load_map_t;		// 地図読み込みスレッド
		private Thread				m_load_info_t;		// 情報読み込みスレッド
		private Thread				m_share_t;			// 航路共有スレッド
		private Thread				m_chat_log_t;		// チャット解析スレッド

		// ***
		private LoadInfosStatus _LoadInfosStatus;

		// タイマ
		private System.Windows.Forms.Timer		m_share_timer;		// 航路共有タイマ

#if DEBUG_DRAW_CPU_BAR
		// for debug
		private qpctimer			m_qpct;				// パフォーマンス測定用
		private cpubar				m_cpubar;
#endif
		//
		private	Point				m_memo_icon_pos;	// メモアイコン追加用座標
		private map_mark.data		m_memo_icon_data;

#if DEBUG_ANGLE_LINE
		private float				m_debug_angle	= 0;
		private int					m_debug_angle_i	= 0;
#endif
		private string				m_device_info_string;

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		private bool is_load_map{
			get{
				if(m_load_map_t == null)	return false;
				else						return m_load_map_t.IsAlive;
			}
		}
		private bool is_load_info{
			get{
				if(m_load_info_t == null)	return false;
				else						return m_load_info_t.IsAlive;
			}
		}
		private bool is_load{
			get{
				if(is_load_map)		return true;
				if(is_load_info)	return true;
				return false;
			}
		}
		private bool is_chat_log_t{
			get{
				if(m_chat_log_t == null)	return false;
				else						return m_chat_log_t.IsAlive;
			}
		}
		// 航路共有スレッドが動いているかどうかを得る
		private bool is_share{
			get{
				if(m_share_t == null)	return false;
				else					return m_share_t.IsAlive;
			}
		}
		// スレッドが動いているかどうかを得る
		private bool is_run_thread{
			get{
				if(is_load)				return true;
				if(is_share)			return true;
				if(is_chat_log_t)		return true;
				return false;
			}
		}
		private Rectangle main_window_crect{	get{	return this.ClientRectangle;	}}

		private bool is_show_menu_strip{
			get{
				if(contextMenuStrip1.Visible)	return true;
				if(contextMenuStrip2.Visible)	return true;
				if(contextMenuStrip3.Visible)	return true;
				return false;
			}
		}
		public string device_info_string{		get{	return m_device_info_string;	}}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public gvtrademap_cs_form()
		{
			InitializeComponent();

			// タイトル
			this.Text	= def.WINDOW_TITLE;
#if DEBUG_SHARE_ROUTES
			this.Text	+= " 航路共有デバッグ";
#endif
			Useful.SetFontMeiryo(this, def.MEIRYO_POINT);
		}

		/*-------------------------------------------------------------------------
		 初期化
		---------------------------------------------------------------------------*/
		public bool Initialize()
		{
			// アプリケーションの格納パス
			m_current_path				= Path.GetDirectoryName(Application.ExecutablePath);
			// カレントディレクトリを設定する
			System.Environment.CurrentDirectory	= m_current_path;
	
			// 必要なパスの作成
			file_ctrl.CreatePath(def.MAP_PATH);
			file_ctrl.CreatePath(def.SEAROUTE_PATH);
			file_ctrl.CreatePath(def.SS_PATH);
			file_ctrl.CreatePath(def.MEMO_PATH);

			// マウスによるドラッグ用
			m_old_mouse_pos				= new Point(0, 0);
			m_mouse_move				= new Point(0, 0);
			
			// 情報
			m_select_pos				= new Point(0, 0);

			// メモアイコン設定用	
			m_memo_icon_pos				= new Point(0, 0);
			m_memo_icon_data			= null;

			// スレッド終了管理
			m_exit_thread_event			= new ManualResetEvent(false);
	
			// 描画関係
			m_lib						= new gvt_lib(this, Path.Combine(m_current_path, def.INI_FNAME));
			// デバイス情報を得ておく
			// 予期しないエラー表示時用
			m_device_info_string		= m_lib.device.deviec_info_string_short;

			// Cmdデリゲート
			m_lib.KeyAssignManager.OnProcessCmdKey		+= new OnProcessCmdKey(process_cnd_key);
			m_lib.KeyAssignManager.OnUpdateAssignList	+= new EventHandler(update_menu_shortcut);
			// メニューのタグを設定する
			init_menu_tag();

			// iniの読み込み
			m_lib.IniManager.Load();

			// ショートカットキーの表示をメニューに反映させる
			update_menu_shortcut(null, EventArgs.Empty);

			// GvoDatabase
			m_db						= new GvoDatabase(m_lib);

			// 自分の船の情報
			m_myship_info				= new myship_info(m_lib, m_db);

			// ***
			_LoadInfosStatus = new LoadInfosStatus();

			// スポット表示
			m_spot						= new spot(m_lib, m_db.World);
			// ウインドウ管理
			m_windows					= new d3d_windows(m_lib.device);
			m_item_window				= new item_window(m_lib, m_db, m_spot, textBox1, listView1, this);
			m_setting_window			= new setting_window(m_lib, m_db, this);
			// アイテムウインドウを先に登録する
			m_windows.Add(m_setting_window);
			m_windows.Add(m_item_window);
			// サブウインドウ
			m_info_windows				= new info_windows(m_lib, m_db, m_myship_info);

			// マウスフック
			// 使用するときだけフックする
			m_mouse_hook				= null;

			// ツールチップ
			m_tooltip					= new ToolTip();
			m_tooltip.SetToolTip(listView1, "おまじない");
			m_tooltip.AutoPopDelay		= 30*1000;		// 30秒表示
			m_tooltip.BackColor			= Color.LightYellow;
			m_show_tooltip				= false;
			m_tooltip_old_mouse_pos		= new Point(0, 0);

			// 設定情報初期化
			// ウインドウ位置等を反映
			init_setting();
	
			// 検索ダイアログ
			m_find_form					= new find_form2(m_lib, m_db, m_spot, m_item_window);
			m_find_form.Location		= m_lib.setting.find_window_location;
			m_find_form.Size			= m_lib.setting.find_window_size;
			m_req_show_find_form		= new RequestCtrl();
			// 検索で座標を検索していると起動時にそこがセンタリングされてしまう
			// なのでリクエストを空読みしてスキップする
			m_lib.setting.req_centering_gpos.IsRequest();

			// 航路図一覧ダイアログ
			m_sea_routes_form			= new sea_routes_form2(m_lib, m_db);
			m_sea_routes_form.Location	= m_lib.setting.sea_routes_window_location;
			m_sea_routes_form.Size		= m_lib.setting.sea_routes_window_size;
			m_req_sea_routes_form		= new RequestCtrl();
	
			// 地図読み込み
			m_map_index					= MapIndex.Max;				// 最初は必ず読み込み
			m_use_mixed_map				= m_lib.setting.use_mixed_map;	// 設定を読み込む
			load_map();													// 読み込みスレッド起動

			// タイマ
			m_share_timer				= new System.Windows.Forms.Timer();
			m_share_timer.Interval		= SHARE_TIMER_INTERVAL;
			m_share_timer.Tick			+= new EventHandler(share_timer_Tick);
			m_share_timer.Start();
	
			// 詳細情報読み込みスレッド
			m_load_info_t				= new Thread(new ThreadStart(load_info_proc));
			m_load_info_t.Name			= "load info";
			m_load_info_t.Start();
			// ログ解析スレッド
			m_chat_log_t				= new Thread(new ThreadStart(chat_log_proc));
			m_chat_log_t.Name			= "analize chat log";
			m_chat_log_t.Start();

#if DEBUG_DRAW_CPU_BAR
			// for debug
			m_cpubar					= new cpubar(m_lib.device);
			m_qpct						= new qpctimer();
#endif
			// ポーズフラグ
			m_pause						= false;
			return true;
		}

		/*-------------------------------------------------------------------------
		 設定情報設定
		---------------------------------------------------------------------------*/
		private void init_setting()
		{
			// ウインドウ位置、サイズの復元
			// 内部でOnMove()が呼ばれるため、ちょっといやな感じに設定する
			Size	size			= m_lib.setting.window_size;
			this.Location			= m_lib.setting.window_location;
			this.Size				= size;

			// ウインドウ枠なし
			if(m_lib.setting.is_border_style_none){
				ExecFunction(KeyFunction.window_change_border_style);
			}
	
			// 選択してる街
			m_item_window.info		= m_db.World.FindInfo(m_lib.setting.select_info);
			m_item_window.EnableItemWindow(false);

			// 位置
			m_lib.loop_image.OffsetPosition	= new Vector2(m_lib.setting.map_pos_x, m_lib.setting.map_pos_y);
			// スケール
			m_lib.loop_image.SetScale(m_lib.setting.map_scale, new Point(0, 0), false);

			// アイテムウインドウ状態
			// 最小化と通常化
			if(!m_lib.setting.is_item_window_normal_size){
				m_item_window.window_mode	= d3d_windows.window.mode.small;
			}
			if(!m_lib.setting.is_setting_window_normal_size){
				m_setting_window.window_mode	= d3d_windows.window.mode.small;
			}
		}
		
		/*-------------------------------------------------------------------------
		 メニューのタグを設定する
		 TagにKeyFunctionを指定することで自動でコマンドが実行されるようにする
		---------------------------------------------------------------------------*/
		private void init_menu_tag()
		{
			m_lib.KeyAssignManager.BindTagForMenuItem(exexgvoacToolStripMenuItem,			KeyFunction.setting_window_button_exec_gvoac);
			m_lib.KeyAssignManager.BindTagForMenuItem(openpathscreenshot2ToolStripMenuItem,	KeyFunction.folder_open_00);
			m_lib.KeyAssignManager.BindTagForMenuItem(openpathlogToolStripMenuItem,			KeyFunction.folder_open_01);
			m_lib.KeyAssignManager.BindTagForMenuItem(openpathmailToolStripMenuItem,		KeyFunction.folder_open_02);
			m_lib.KeyAssignManager.BindTagForMenuItem(openpathscreenshotToolStripMenuItem,	KeyFunction.folder_open_03);
			m_lib.KeyAssignManager.BindTagForMenuItem(changeBorderStyleToolStripMenuItem,	KeyFunction.window_change_border_style);
			m_lib.KeyAssignManager.BindTagForMenuItem(closeFormToolStripMenuItem,			KeyFunction.window_close);
			m_lib.KeyAssignManager.BindTagForMenuItem(clear_spotToolStripMenuItem,			KeyFunction.cancel_spot);
		}

		/*-------------------------------------------------------------------------
		 キー割り当てをメニューに反映させる
		 KeyAssignManager.OnUpdateAssignList用
		---------------------------------------------------------------------------*/
		private void update_menu_shortcut(object sender, EventArgs e)
		{
			m_lib.KeyAssignManager.UpdateMenuShortcutKeys(contextMenuStrip1);
			m_lib.KeyAssignManager.UpdateMenuShortcutKeys(contextMenuStrip2);
			m_lib.KeyAssignManager.UpdateMenuShortcutKeys(contextMenuStrip3);
		}

		/*-------------------------------------------------------------------------
		 Closed
		---------------------------------------------------------------------------*/
		private void gvtrademap_cs_form_FormClosed(object sender, FormClosedEventArgs e)
		{
			// スレッドを終了させる
			finish_all_threads();
	
			// マウスフックを終了する
			dispose_mouse_hook();
	
			// メモを更新する
			m_item_window.UpdateMemo();

			// 検索ダイアログの位置とサイズ
			m_lib.setting.find_window_location	= m_find_form.Location;
			m_lib.setting.find_window_size		= m_find_form.Size;
			m_lib.setting.find_window_visible	= m_find_form.Visible;

			// 航路図一覧ダイアログの位置とサイズ
			m_lib.setting.sea_routes_window_location	= m_sea_routes_form.Location;
			m_lib.setting.sea_routes_window_size		= m_sea_routes_form.Size;
			m_lib.setting.sea_routes_window_visible		= m_sea_routes_form.Visible;

			// 選択してる街
			if(m_item_window.info != null)	m_lib.setting.select_info	= m_item_window.info.Name;
			else							m_lib.setting.select_info	= "";

			// 位置
			m_lib.setting.map_pos_x		= m_lib.loop_image.OffsetPosition.X;
			m_lib.setting.map_pos_y		= m_lib.loop_image.OffsetPosition.Y;

			// スケール
			m_lib.setting.map_scale		= m_lib.loop_image.ImageScale;

			// アイテムウインドウ状態
			m_lib.setting.is_item_window_normal_size	= m_item_window.window_mode == d3d_windows.window.mode.normal;
			m_lib.setting.is_setting_window_normal_size	= m_setting_window.window_mode	== d3d_windows.window.mode.normal;

			// iniの書き出し
			m_lib.IniManager.Save();

			// GvoDatabaseで書きだす必要のある情報の書き出し
			m_db.WriteSettings();
		}

		/*-------------------------------------------------------------------------
		 スレッド終了シグナルを設定し、スレッド終了を待つ
		---------------------------------------------------------------------------*/
		private void finish_all_threads()
		{
			// スレッド終了イベントを設定する
			if(m_exit_thread_event != null){
				m_exit_thread_event.Set();
			}

			wait_finish_thread(m_load_map_t);
			wait_finish_thread(m_load_info_t);
			wait_finish_thread(m_share_t);
			wait_finish_thread(m_chat_log_t);
		}

		/*-------------------------------------------------------------------------
		 スレッド終了を待つ
		---------------------------------------------------------------------------*/
		private void wait_finish_thread(Thread t)
		{
			if(t == null)	return;
			if(!t.IsAlive)	return;		// 動いていない
			t.Join();					// 終了待ち
		}

		/*-------------------------------------------------------------------------
		 OnPaint
		---------------------------------------------------------------------------*/
		private void m_main_window_Paint(object sender, PaintEventArgs e)
		{
			// メインウインドウの更新
			// ダイアログ描画時のために用意しておく
			// 背景を描画する関数が不明のため、一度背景色で全体を塗られる
			m_lib.device.SetMustDrawFlag();
			update_main_window();
		}

		/*-------------------------------------------------------------------------
		 地図読み込み
		---------------------------------------------------------------------------*/
		private void load_map()
		{
			if(is_load)												return;		// なにか読み込み中
			if(  (m_map_index == m_lib.setting.map)
			   &&(m_use_mixed_map == m_lib.setting.use_mixed_map) )	return;		// 変更なし

			m_load_map_t		= new Thread(new ThreadStart(load_map_proc));
			m_load_map_t.Name	= "load map";
			m_load_map_t.Start();
		}

		/*-------------------------------------------------------------------------
		 座標変換
		 主に街の追加時用
		 測量座標を地図座標に変換してデバッグ出力する
		---------------------------------------------------------------------------*/
		private void debug_transform_pos(int x, int y)
		{
			Point	pos		= transform.game_pos2_map_pos(new Point(x, y), m_lib.loop_image);
			Debug.WriteLine(String.Format("{0},{1}", pos.X, pos.Y));
		}

		/*-------------------------------------------------------------------------
		 メインウインドウの更新
		---------------------------------------------------------------------------*/
		public void update_main_window()
		{
			if(m_lib.device.device == null)		return;

			// 最前列表示
			if(this.TopMost != m_lib.setting.window_top_most){
				this.TopMost = m_lib.setting.window_top_most;
			}

			// 地図変更チェック
			load_map();

			if(is_load){
				// 何か読み込み中

				// 最小化中は描画しない
				if(m_pause)						return;

				update_main_window_load();
			}else{
				// 通常時

				// タスクの実行
				do_tasks();

				// 最小化中は描画しない
				if(m_pause)						return;
	
				// 描画
				if(m_lib.device.IsNeedDraw()){
					do_draw();
				}
			}
		}

		/*-------------------------------------------------------------------------
		 定例処理
		---------------------------------------------------------------------------*/
		private void do_tasks()
		{
#if DEBUG_DRAW_CPU_BAR
			m_qpct.GetElapsedTime();
#endif
			
			//
			// リクエストの実行
			//
	
			// 検索ダイアログの表示
			// 起動時の1回のみ
			if(m_req_show_find_form.IsRequest()){
				show_find_dialog();
				this.Activate();
			}
			// 航路図一覧ダイアログの表示
			// 起動時の1回のみ
			if(m_req_sea_routes_form.IsRequest()){
				show_sea_routes_dialog();
				this.Activate();
			}
	
			// 自分の船の位置等更新
			m_myship_info.Update();

			// 現在の船の位置を中心に移動する
			if(m_lib.setting.center_myship){
				if(m_myship_info.capture_sucess){	// キャプチャが成功したときのみ
					centering_pos(transform.game_pos2_map_pos(m_myship_info.pos, m_lib.loop_image));
				}
			}

			// スポット
			do_spot_request();

			// 特定のゲーム座標をセンタリングする
			if(m_lib.setting.req_centering_gpos.IsRequest()){
				centering_pos(transform.game_pos2_map_pos(m_lib.setting.centering_gpos, m_lib.loop_image));
				m_select_pos	= m_lib.setting.centering_gpos;
			}

			// 
			// 設定の反映
			// 

			// サーバとベース国の設定
			m_db.World.SetServerAndCountry(m_lib.setting.server, m_lib.setting.country);
			// 最適化の更新
			// 設定ダイアログで内容が変更された場合描画リストを作りなおす
			m_db.WebIcons.Update();
			// 地図のオフセット更新
			m_lib.loop_image.AddOffset(new Vector2(m_mouse_move.X, m_mouse_move.Y));

			// アイテムウインドウ
			m_windows.Update();
			// tooltip
			update_tooltip();
			// 情報表示ウインドウ
			m_info_windows.Update(m_select_pos, m_old_mouse_pos);

			// ドラッグ量を0に戻す
			if((m_mouse_move.X != 0)||(m_mouse_move.Y != 0)){
				// ドラッグ中は描画スキップなし
				m_lib.device.SetMustDrawFlag();
			}

			m_mouse_move		= new Point(0, 0);

			// マウスフック起動と終了
			// 設定により起動と終了を切り替える
			do_mouse_hook();

			// 危険海域変動システム
			if(m_map_index == MapIndex.Map2)		m_db.SeaArea.color	= sea_area.color_type.type1;
			else									m_db.SeaArea.color	= sea_area.color_type.type2;
			m_db.SeaArea.Update();

			// スクリーンショット
			if(m_lib.setting.req_screen_shot.IsRequest()){
				screen_shot();
			}

			// 航路図一覧の更新
			if(m_db.SeaRoute.req_update_list.IsRequest()){
				// 再構築
				m_sea_routes_form.UpdateAllList();
				m_db.SeaRoute.req_redraw_list.IsRequest();		// 空読み
			}
			if(m_db.SeaRoute.req_redraw_list.IsRequest()){
				// 再描画
				// 最新の航路図のみ再描画
				m_sea_routes_form.RedrawNewestSeaRoutes();
			}
		}

		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		private void do_draw()
		{
			// 画面のクリア
			m_lib.device.Clear(Color.Black);
//			m_lib.Device.Clear(Color.FromArgb(255,152,176,190));

			// 描画開始
			if(!m_lib.device.Begin())	return;		// デバイスロスト中

			// 地図描画
			draw_map();

			// GvoDatabase
			m_db.Draw();

			// 自分船の位置等描画
			m_myship_info.Draw();

			// スポット
			m_spot.Draw();

			// 緯度と経度の描画
			// 座標
			if(m_lib.setting.tude_interval != TudeInterval.None){
				latitude_longitude.DrawPoints(m_lib);
			}

			// アイテムウインドウ
			m_windows.Draw();

			if(!m_lib.setting.is_server_mode){
				// キャプチャ詳細を描画
				m_db.Capture.DrawCapturedTexture();
			}

#if DEBUG_DRAW_DEBUG_STRING
			debug_draw_debug_string();
#endif
#if DEBUG_COMPASS
			debug_compass();
#endif
#if DEBUG_ANGLE_LINE
			debug_angle_line();
#endif
			// 情報表示ウインドウ
			m_info_windows.Draw();

			// 画面枠
			draw_frame();

#if DEBUG_DRAW_CPU_BAR
			float check	= m_qpct.GetElapsedTime();
			m_cpubar.Update(check, 1f/60);
#endif
			m_lib.device.End();

			// 画面を転送
			m_lib.device.Present();
		}

		/*-------------------------------------------------------------------------
		 地図を描画する
		 海域変動、緯度、経度線描画付き
		---------------------------------------------------------------------------*/
		private void draw_map()
		{
			// 地図の合成更新
			m_lib.loop_image.MergeImage(new LoopXImage.DrawHandler(m_db.DrawForMargeInfoNames),
										m_lib.setting.req_update_map.IsRequest());
			// 地図描画
			m_lib.loop_image.Draw();

			// 緯度と経度の描画
			switch(m_lib.setting.tude_interval){
			case TudeInterval.Interval1000:
				latitude_longitude.DrawLines(m_lib);
				break;
			case TudeInterval.Interval100:
				latitude_longitude.DrawLines100(m_lib);
				break;
			}
		}
	
		/*-------------------------------------------------------------------------
		 フレームを描く
		 windows Vista aeroのときは描かない
		---------------------------------------------------------------------------*/
		private void draw_frame()
		{
			if(m_lib.setting.windows_vista_aero)	return;
	
			Vector2	size	= m_lib.device.client_size;
			size.X	-= 1;
			size.Y	-= 1;
			m_lib.device.DrawLineRect(new Vector3(0, 0, 0.0001f), size, Color.Black.ToArgb());
		}

		/*-------------------------------------------------------------------------
		 メインウインドウにマウスカーソルがあるかどうかを調べる
		---------------------------------------------------------------------------*/
        private bool is_inside_mouse_cursor_main_window()
		{
			Point		pos		= this.PointToClient(MousePosition);
			hittest		test	= new hittest(this.ClientRectangle);

			return test.HitTest(pos);
		}

		#region 読み込み時の描画
		/*-------------------------------------------------------------------------
		 読み込み時の描画
		---------------------------------------------------------------------------*/
		public void update_main_window_load()
		{
			// メモウインドウを非表示
			m_item_window.EnableMemoWindow(false);
			m_item_window.EnableItemWindow(false);
	
			// マウスによるドラッグは無効
			m_mouse_move		= new Point(0, 0);

      try
      {
			m_lib.device.Clear(Color.White);
			if(!m_lib.device.Begin())	return;		// デバイスロスト中

			// 読み込みプログレスを表示する
			// 画面横サイズ/2 を最大とする
			float	size_x	= (float)(main_window_crect.Width / 2);
			float	y		= (float)(main_window_crect.Height / 2) - ((((16+8)+8) * 4) / 2);
			float	x		= size_x - (size_x / 2);

            // 街詳細
            draw_progress("街詳細...", _LoadInfosStatus.StatusMessage,
                            x, y, size_x,
                            _LoadInfosStatus.NowStep,
                            _LoadInfosStatus.MaxStep, Color.Tomato.ToArgb());

            // 地図読み込み
            y += 16 + 8 + 8;
            draw_progress("地図...", m_lib.loop_image.LoadStr,
							x, y, size_x,
							m_lib.loop_image.LoadCurrent,
							m_lib.loop_image.LoadMax, Color.SkyBlue.ToArgb());

			// マスク
			y	+= 16+8+8;
			draw_progress("陸地マスク...", m_db.SeaArea.progress_info_str,
							x, y, size_x,
							m_db.SeaArea.progress_current,
							m_db.SeaArea.progress_max, Color.SkyBlue.ToArgb());

			// 画面枠
			draw_frame();
			m_lib.device.End();
      }
      catch (Exception ex)
      {
        Console.WriteLine("描画中の例外をキャッチ(読み込み中)\n" + ex.StackTrace);
      }

			// 画面を転送
			m_lib.device.Present();
		}

		/*-------------------------------------------------------------------------
		 プログレスバーを描画する
		---------------------------------------------------------------------------*/
		private void draw_progress(string str, string str2, float x, float y, float size_x, int current, int max, int color)
		{
			m_lib.device.DrawText(font_type.normal, str, (int)x, (int)y, Color.Black);
			m_lib.device.DrawTextR(font_type.normal, String.Format("{0} {1}/{2}", str2, current, max),
									(int)x + (int)size_x, (int)y, Color.Black);

			// プログレスバー
			float	percent;
			if(max > 0)	percent	= (float)current / max;
			else		percent	= 0;
			m_lib.device.DrawFillRect(	new Vector3(x, y + 16, 0.1f),
										new Vector2(size_x * percent, 8),
										color);
			m_lib.device.DrawLineRect(	new Vector3(x, y + 16, 0.1f),
										new Vector2(size_x, 8), Color.Black.ToArgb());
		}
		#endregion

		#region スレッド
		/*-------------------------------------------------------------------------
		 地図読み込みスレッド
		---------------------------------------------------------------------------*/
		private void load_map_proc()
		{
			string[] map_name_tbl	= new string[]{
				def.MAP_FULLFNAME1,
				def.MAP_FULLFNAME2,
			};
			string[] map_name_tbl2	= new string[]{
				def.MIX_MAP_FULLFNAME1,
				def.MIX_MAP_FULLFNAME2,
			};

			// 進捗状況取得用に初期化
			m_lib.loop_image.InitializeCreateImage();
			if(!m_db.SeaArea.IsLoadedMask){
				m_db.SeaArea.InitializeFromMaskInfo();
			}

			// 地図の読み込み
			m_map_index		= m_lib.setting.map;
			m_use_mixed_map	= m_lib.setting.use_mixed_map;

			if(m_use_mixed_map){
				// 合成した地図を使用する

				// 合成してなければ合成する
				favoriteroute.MixMap(	map_name_tbl[(int)m_map_index],
										def.FAVORITEROUTE_FULLFNAME,
										map_name_tbl2[(int)m_map_index],
										ImageFormat.Png);
//										ImageFormat.Jpeg);
//										ImageFormat.Bmp);

				if(File.Exists(map_name_tbl2[(int)m_map_index])){
					// お気に入り航路合成後の地図がある
					m_lib.loop_image.CreateImage(map_name_tbl2[(int)m_map_index]);
				}else{
					// お気に入り航路合成後の地図がない
					m_lib.loop_image.CreateImage(map_name_tbl[(int)m_map_index]);
				}
			}else{
				// お気に入り航路合成後の地図を使用しない
				m_lib.loop_image.CreateImage(map_name_tbl[(int)m_map_index]);
			}

			// 海域群をマスクから作成
			if(!m_db.SeaArea.IsLoadedMask){
				m_db.SeaArea.CreateFromMask(def.MAP_MASK_FULLFNAME);
			}

			// 地図合成リクエスト
			m_lib.setting.req_update_map.Request();

			// 終了後少しだけ100%表示
			Thread.Sleep(100);		// 0.1秒
			Debug.WriteLine("finish load map.");

			// 座標変換
//			debug_transform_pos(13283, 2173);
//			debug_transform_pos(13086, 2797);
//			debug_transform_pos(12714, 3565);
		}

		/*-------------------------------------------------------------------------
		 詳細情報読み込みスレッド
		---------------------------------------------------------------------------*/
		private void load_info_proc()
		{
            _LoadInfosStatus.Start(3, "インターネットから同盟状況の取得");
            
            if (m_lib.setting.connect_network)
            {
                m_db.World.DownloadDomains(def.LOCAL_NEW_DOMAINS_INDEX_FULLFNAME);
            }

			// 詳細情報読み込み
            _LoadInfosStatus.IncStep("街情報の読み込み");
            m_db.World.Load(def.INFO_FULLNAME, def.NEW_DOMAINS_INDEX_FULLFNAME, def.LOCAL_NEW_DOMAINS_INDEX_FULLFNAME);

			// アイテムデータベースとリンクさせる
            m_db.World.LinkItemDatabase(m_db.ItemDatabase);

			// Webアイコン読み込み
			// ネットワークから取得するかどうかに関係なく読み込もうとする
            _LoadInfosStatus.IncStep("Webアイコンの読み込み");
            m_db.WebIcons.Load(def.WEB_ICONS_FULLFNAME);

			// 検索ダイアログを表示する
			if(m_lib.setting.find_window_visible)		m_req_show_find_form.Request();
			if(m_lib.setting.sea_routes_window_visible)	m_req_sea_routes_form.Request();
	
			// 終了後少しだけ100%表示
            _LoadInfosStatus.IncStep("完了");
            Thread.Sleep(100);
            Debug.WriteLine("finish load info.");
        }

		/*-------------------------------------------------------------------------
		 ログ解析スレッド
		---------------------------------------------------------------------------*/
		private void chat_log_proc()
		{
			int		total_sleep	= 0;
			bool	old_analize	= true;
			while(!m_exit_thread_event.WaitOne(0, false)){
				Thread.Sleep(200);
				total_sleep	+= 200;
				if(total_sleep > CHAT_LOG_TIMER_INTERVAL){
					total_sleep	-= CHAT_LOG_TIMER_INTERVAL;

					if((m_lib.setting.is_server_mode)
						||(!m_lib.setting.enable_analize_log_chat)
						||(!m_lib.setting.save_searoutes) ){
						// 解析しなかった
						old_analize		= false;
						continue;
					}

					// ログ解析有効時のみ
					m_db.GvoChat.AnalyzeNewestChatLog();	// ログ解析を行う
					m_db.GvoChat.Request();					// 海域変動を反映させてもらうリクエスト

					// 前回解析しなかった場合は解析結果を捨てる
					// 海域変動は反映させる
					// スレッドの関係で捨て損ねることがある
					// それほど問題にはならないので放置してる
					if(!old_analize){
						m_db.GvoChat.ResetAccident();
						m_db.GvoChat.ResetInterest();
						m_db.GvoChat.ResetBuildShip();
					}
		
					// 解析した
					old_analize		= true;
				}
			}
			Debug.WriteLine("finish analize chat log.");
		}

		/*-------------------------------------------------------------------------
		 航路共有スレッド
		---------------------------------------------------------------------------*/
		private void share_proc()
		{
#if DEBUG_SHARE_ROUTES
			m_db.share_routes.Share();
#else
			if(!m_myship_info.is_analized_pos){
				// 自分の位置が分からない
				m_db.ShareRoutes.Share(0, 0, ShareRoutes.State.outof_sea);
			}else{
				// コンパスの角度が得られていれば海上
				ShareRoutes.State	_state	= (m_myship_info.is_in_the_sea)
													? ShareRoutes.State.in_the_sea
													: ShareRoutes.State.outof_sea;
				m_db.ShareRoutes.Share((int)m_myship_info.pos.X, (int)m_myship_info.pos.Y, _state);
			}
#endif
		}
		#endregion

		#region タイマコールバック
		/*-------------------------------------------------------------------------
		 航路共有タイマコールバック
		 ついでに季節チェックを行う
		---------------------------------------------------------------------------*/
		private void share_timer_Tick(object sender, EventArgs e)
		{
			// 季節チェック更新
			if(m_db.GvoSeason.UpdateSeason()){
				// 季節が変わった
				// 地図更新をリクエストする
				m_lib.setting.req_update_map.Request();
			}

#if !DEBUG_SHARE_ROUTES
			// 共有が有効でなければなにもしない
			if(!m_lib.setting.enable_share_routes)			return;

			// 大航海時代Onlineが起動してなければ共有しない
			if(!gvo_capture.IsFoundGvoWindow())				return;

			// スレッドが動いている場合はなにもしない
			// 1分以上スレッドが動いているのはなにか問題を抱えている
			if(is_share)									return;
#endif
			if(m_exit_thread_event.WaitOne(0, false))		return;			// 終了しようとしている

			// ネットワークへの接続は時間がかかるのでスレッドで行う
			m_share_t		= new Thread(new ThreadStart(share_proc));
			m_share_t.Name	= "share network";
			m_share_t.Start();
		}
		#endregion

		#region マウス関係
		/*-------------------------------------------------------------------------
		 メインウインドウ内でマウスボタン押す
		---------------------------------------------------------------------------*/
		private void MainWindowMouseDown(object sender, MouseEventArgs e)
		{
			Point		pos	= new Point(e.X, e.Y);

			// 今回の位置を覚えておく
			m_old_mouse_pos		= pos;
			m_lib.device.SetMustDrawFlag();

			if(!m_item_window.HitTest_ItemList(pos)){
				// コントロールのフォーカスを外す
				ActiveControl	= null;
			}
	
			// ウインドウ管理チェック
			if(m_windows.OnMouseDown(pos, e.Button)){
				this.Capture	= false;
				return;
			}

			// 情報表示ウインドウ管理チェック
			if(m_info_windows.HitTest(pos)){
				this.Capture	= false;
				return;
			}

			// 設定により動作が異なる
			if(m_lib.setting.compatible_windows_rclick){
				// 右クリックでメニューが開く版

				// 選択等は左クリックか右クリック
				if(   ((e.Button & MouseButtons.Left) != 0)
					||((e.Button & MouseButtons.Right) != 0) ){
					m_select_pos			= transform.client_pos2_game_pos(pos, m_lib.loop_image);

					if(!m_spot.is_spot){
						// 何かあれば選択する
						m_item_window.info		= m_db.World.FindInfo(m_lib.loop_image.MousePos2GlobalPos(pos));
					}
				}
			}else{
				// Ctrl+右クリックでメニューが開く版

				// 選択等は左クリックのみ
				if((e.Button & MouseButtons.Left) != 0){
					m_select_pos			= transform.client_pos2_game_pos(pos, m_lib.loop_image);

					// 何かあれば選択する
					m_item_window.info		= m_db.World.FindInfo(m_lib.loop_image.MousePos2GlobalPos(pos));
				}
			}

			// マウスキャプチャー開始
			this.Capture	= true;
		}

		/*-------------------------------------------------------------------------
		 メインウインドウ内でマウスクリック
		---------------------------------------------------------------------------*/
		private void gvtrademap_cs_form_MouseClick(object sender, MouseEventArgs e)
		{
			Point		pos	= new Point(e.X, e.Y);

			// 今回の位置を覚えておく
			m_old_mouse_pos		= pos;
	
			// ウインドウ管理チェック
			if(m_windows.OnMouseClick(pos, e.Button)){
				return;
			}
	
			// 情報表示ウインドウ管理チェック
			if(m_info_windows.OnMouseClick(pos, e.Button, this)){
				return;
			}

			// 設定により動作が異なる
			if(m_lib.setting.compatible_windows_rclick){
				// 右クリックでメニューが開く版
				// メモアイコン
				if((e.Button & MouseButtons.Right) != 0){
					main_window_context_menu(pos);
					return;
				}
			}else{
				// Ctrl+右クリックでメニューが開く版
				// メモアイコン
				if(   ((e.Button & MouseButtons.Right) != 0)
					&&((user32.GetKeyState(user32.VK_CONTROL) & 0x8000) != 0) ){
					main_window_context_menu(pos);
					return;
				}
			}
		}
	
		/*-------------------------------------------------------------------------
		 メインウインドウ内でマウスボタン離す
		---------------------------------------------------------------------------*/
		private void MainWindowMouseUp(object sender, MouseEventArgs e)
		{
			// マウスキャプチャー終了
			this.Capture	= false;
		}

		/*-------------------------------------------------------------------------
		 メインウインドウ内でマウス移動
		---------------------------------------------------------------------------*/
		private void MainWindowMouseMove(object sender, MouseEventArgs e)
		{
			// マウスキャプチャー中なら
			if(this.Capture){
				// 移動量を加算する
				// 地図移動用
				m_mouse_move.X	+= e.X - m_old_mouse_pos.X;
				m_mouse_move.Y	+= e.Y - m_old_mouse_pos.Y;
			}

			// 今回の位置を覚えておく
			m_old_mouse_pos.X	= e.X;
			m_old_mouse_pos.Y	= e.Y;
		}

		/*-------------------------------------------------------------------------
		 メインウインドウ内でマウスダブルクリック
		---------------------------------------------------------------------------*/
		private void m_main_window_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			// ウインドウ管理が最初にチェック
			if(m_windows.OnMouseDoubleClick(new Point(e.X, e.Y), e.Button))	return;

			// ウインドウでは処理されなかった
		}

		/*-------------------------------------------------------------------------
		 マウスホイール
		---------------------------------------------------------------------------*/
        private void FormMouseWheel(object sender, MouseEventArgs e)
		{
			if(!is_inside_mouse_cursor_main_window())	return;

			// メインウインドウ内
			// ウインドウ管理が最初にチェック
			Point		client_mouse_pos	= this.PointToClient(MousePosition);
			if(m_windows.OnMouseWheel(client_mouse_pos, e.Delta))	return;

			// ウインドウでは処理されなかった
			// メインウインドウのスケール変更	
			zoom_map((e.Delta > 0)?true: false, client_mouse_pos);
		}
		#endregion

		#region ウインドウサイズ変更と移動
		/*-------------------------------------------------------------------------
		 ウインドウサイズ変更
		---------------------------------------------------------------------------*/
		private void gvtrademap_cs_form_Resize(object sender, EventArgs e)
		{
			// InitializeComponent()内で呼ばれる事例が報告されたため、
			// m_lib が作成されたかどうかをチェックする
			// このPCではInitializeComponent()内で呼ばれないため、デバッグ不可能
			if(m_lib == null)				return;
			if(m_lib.setting == null)		return;

			// ウィンドウが最小化あるいは表示されているかどうかをチェック
			m_pause = ((this.WindowState == FormWindowState.Minimized) || !this.Visible);

			// 設定情報を更新する
			update_windows_position();
		}

		/*-------------------------------------------------------------------------
		 ウインドウ移動
		---------------------------------------------------------------------------*/
		private void gvtrademap_cs_form_Move(object sender, EventArgs e)
		{
			// 設定情報を更新する
			update_windows_position();
		}

		/*-------------------------------------------------------------------------
		 設定情報用ウインドウサイズ更新
		 通常時のみ更新される
		---------------------------------------------------------------------------*/
		private void update_windows_position()
		{
			// 最大化及び最小化でないときのみ
			if(this.WindowState != FormWindowState.Normal)	return;

			if(m_lib == null)								return;
			if(m_lib.setting == null)						return;
			m_lib.setting.window_location	= this.Location;
			m_lib.setting.window_size		= this.Size;
		}
		#endregion

		#region マウスフック関係
		/*-------------------------------------------------------------------------
		 マウスフック起動と終了
		 設定により起動と終了を切り替える
		 使わない場合はフックしていないほうがいい
		---------------------------------------------------------------------------*/
		private void do_mouse_hook()
		{
			if(m_lib.setting.hook_mouse){
				// 使用
				if(m_mouse_hook != null)		return;		// すでに起動中

				// マウスフック開始
				m_mouse_hook	= new globalmouse_hook();
			}else{
				// 未使用
				// 使っていればフックを外す
				dispose_mouse_hook();
			}
		}

		/*-------------------------------------------------------------------------
		 マウスフック終了
		---------------------------------------------------------------------------*/
		private void dispose_mouse_hook()
		{
			if(m_mouse_hook == null)	return;

			// マウスフックを終了する
			m_mouse_hook.Dispose();
			m_mouse_hook	= null;
		}
		#endregion

		#region ツールチップ関係
		/*-------------------------------------------------------------------------
		 ツールチップの更新
		---------------------------------------------------------------------------*/
		private void update_tooltip()
		{
			// ツールチップの位置がウインドウ位置基準なのでLocationを基準とする
			Point mpos	= new Point(MousePosition.X - Location.X,
									MousePosition.Y - Location.Y);

			if(m_show_tooltip){
				// tooltip表示中
				if(is_show_menu_strip){
					try{
						m_tooltip.Hide(this);
					}catch{
//						MessageBox.Show("Hide()エラーをキャッチ");
					}
					m_show_tooltip			= false;
					m_tooltip_interval		= 0;
				}else{
					Vector2		v1	= new Vector2(mpos.X, mpos.Y);
					Vector2		v2	= new Vector2(m_tooltip_old_mouse_pos.X, m_tooltip_old_mouse_pos.Y);

					v1	-= v2;
					if(v1.LengthSq() >= 8f*8f){		// 8dot以上動いたら
						// 表示終了
						try{
							m_tooltip.Hide(this);
						}catch{
//							MessageBox.Show("Hide()エラーをキャッチ");
						}
						m_show_tooltip			= false;
						m_tooltip_old_mouse_pos	= mpos;
					}
					m_tooltip_interval		= 0;
				}
			}else{
				if(m_tooltip_old_mouse_pos == mpos){
					if(is_show_menu_strip){
						m_tooltip_interval		= 0;
					}else{
						// マウスの移動なし
						if(!is_inside_mouse_cursor_main_window()){
							// ウインドウ外
							m_tooltip_old_mouse_pos	= mpos;
							m_tooltip_interval		= 0;
						}else{
							if(++m_tooltip_interval >= TOOLTIP_INITIAL){
								// クライアント座標
								Point	pos	= this.PointToClient(MousePosition);
								// アイテムウインドウからツールチップを得る
								string	str	= m_windows.GetToolTipString(pos);
								// メインウインドウからツールチップを得る
								if(str == null)		str	= get_tooltip_string(pos);
								if(str != null){
									// なにかツールチップがあれば表示する
									// マウスの描画後のサイズ分ずらしたいが、取得のしかたが分からない
									try{
										m_tooltip.Show(str, this, mpos.X + 10, mpos.Y, 40000000);
									}catch{
									}
									m_show_tooltip	= true;
								}else{
									// ないのでスルー
									m_tooltip_interval	= TOOLTIP_INITIAL;
								}
							}
						}
					}
				}else{
					// マウスの移動あり
					m_tooltip_old_mouse_pos	= mpos;
					m_tooltip_interval		= 0;
				}
			}
		}

		/*-------------------------------------------------------------------------
		 ツールチップ文字列を得る
		 情報表示ウインドウとの判定
		 スポット時はスポットからのツールチップ文字列を得る
		---------------------------------------------------------------------------*/
		private string get_tooltip_string(Point pos)
		{
			// 情報表示ウインドウ
			string	tip		= m_info_windows.OnToolTipString(pos);
			
			// スポット
			Point	gpos	= m_lib.loop_image.MousePos2GlobalPos(pos);
			if(tip == null)		tip	= m_spot.GetToolTipString(gpos);

			// メモアイコン
			if(tip == null)		tip	= m_db.MapMark.GetToolTip(gpos);

			// 街名等
/*			if(m_lib.setting.map_draw_names == map_draw_names.draw){
				// 描画する設定なのでポップアップしない
				return tip;
			}
			if(tip == null){
				GvoWorldInfo.Info		Info	= m_db.World.FindInfo(gpos);
				if(Info != null){
					if(Info.InfoType != GvoWorldInfo.InfoType.SEA){
						// 海域はポップアップしない
						tip	= Info.Name;
					}
				}
			}
*/
			if(tip == null){
				GvoWorldInfo.Info		info	= m_db.World.FindInfo(gpos);
				if(info != null){
					tip		= info.TooltipString;
					if(m_lib.setting.map_draw_names == MapDrawNames.Draw){
						// 街名等を描画するモード
						// 上陸地点等はポップアップしない
						if(info.InfoType == GvoWorldInfo.InfoType.OutsideCity)	return null;
						if(info.InfoType == GvoWorldInfo.InfoType.Shore)		return null;
						if(info.InfoType == GvoWorldInfo.InfoType.Shore2)		return null;
					}
				}
			}

			return tip;
		}
		#endregion

		#region スクリーンショット
		/*-------------------------------------------------------------------------
		 スクリーンショット
		 ビデオメモリからメインメモリへの転送速度の関係で
		 フルサイズだとかなり時間がかかる
		 
		 512*512のレンダリング用サーフェイスを作成
		 レンダリング
		 取り出し
		 連結
		 を必要なサイズ分行う
		---------------------------------------------------------------------------*/
		private void screen_shot()
		{
			const int	RENDER_TARGET_SIZE_X		= 512;
			const int	RENDER_TARGET_SIZE_Y		= 512;
	
			// レンダリング開始位置とサイズ
			Point		tmp;
			Size		size;
			m_db.SeaRoute.CalcScreenShotBoundingBox(out tmp, out size);
			if((size.Width <= 0)||(size.Height <= 0)){
				MessageBox.Show("航路情報がないため、SSを作成できません。",
								"報告", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
	
			Device	device				= m_lib.device.device;
			Surface	depth				= device.DepthStencilSurface;
			Surface	backbuffer			= device.GetBackBuffer(0, 0, BackBufferType.Mono);
			Surface	rendertarget		= null;
			Surface	offscreen			= null;
			Surface	rendertarget_depth	= null;

			try{
				// レンダーターゲット
				rendertarget		= device.CreateRenderTarget(RENDER_TARGET_SIZE_X, RENDER_TARGET_SIZE_Y,
																Format.R5G6B5, MultiSampleType.None, 0, false);
				// メインメモリ上のサーフェイス
				// レンダリング結果取り出し用
				offscreen			= device.CreateOffscreenPlainSurface(RENDER_TARGET_SIZE_X, RENDER_TARGET_SIZE_Y,
																		Format.R5G6B5, Pool.SystemMemory);
				// 深度バッファ
				rendertarget_depth	= device.CreateDepthStencilSurface(RENDER_TARGET_SIZE_X, RENDER_TARGET_SIZE_Y,
																		DepthFormat.D16, MultiSampleType.None, 0, false);
			}catch{
				// レンダリング用バッファの作成に失敗
				if(rendertarget != null)		rendertarget.Dispose();
				if(offscreen != null)			offscreen.Dispose();
				if(rendertarget_depth != null)	rendertarget_depth.Dispose();

				MessageBox.Show("SSの保存に失敗しました。\nSS情報の作成に失敗しました。",
								"報告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			Cursor	= Cursors.WaitCursor;

			// 描画対象を変更する
			device.DepthStencilSurface	= rendertarget_depth;
			device.SetRenderTarget(0, rendertarget);

			// 描画位置とスケールを退避
			m_lib.loop_image.PushDrawParams();

			Vector2		offset	= new Vector2(tmp.X, tmp.Y);
			UInt16[]	buffer	= new UInt16[size.Width * size.Height];

			// スケールは等倍
			m_lib.loop_image.SetScale(1, new Point(0, 0), false);

			Vector2		pos		= new Vector2(0, 0);

			// レンダリング後、取り出し連結する
			for(; pos.Y<size.Height; pos.Y+=RENDER_TARGET_SIZE_Y){
				for(pos.X=0; pos.X<size.Width; pos.X+=RENDER_TARGET_SIZE_X){
					// オフセット設定
					m_lib.loop_image.OffsetPosition	= -(offset + pos);
					// レンダリング
					screen_shot_draw();
					// 結果を取りだす
					// レンダリング終了まで待たされる
					SurfaceLoader.FromSurface(offscreen, rendertarget, Filter.None, 0);
					// 連結する
					screen_shot_chain_image(buffer,
											size.Width, size.Height, size.Width,
											(int)pos.X, (int)pos.Y,
											offscreen);
				}
			}

			// 描画位置とスケールを元に戻す
			m_lib.loop_image.PopDrawParams();

			// 描画対象を元に戻す
			device.DepthStencilSurface	= depth;
			device.SetRenderTarget(0, backbuffer);
			m_lib.device.UpdateClientSize();
	
			// 確保したバッファの解放
			rendertarget.Dispose();
			offscreen.Dispose();
			rendertarget_depth.Dispose();
			depth.Dispose();
			backbuffer.Dispose();

			try{
				// UInt16[] を bitmapに変換して jpg で書きだし
				GCHandle	handle	= GCHandle.Alloc(buffer, GCHandleType.Pinned);
				Bitmap		bitmap	= new Bitmap(	size.Width, size.Height, size.Width*2,
													PixelFormat.Format16bppRgb565,
													handle.AddrOfPinnedObject());
				string	fname		= "searoute" + DateTime.Now.ToString("yyyyMMddHHmmss");
				switch(m_lib.setting.ss_format){
				case SSFormat.Png:
					fname	= Path.Combine(m_current_path, def.SS_PATH + fname + ".png");
					bitmap.Save(fname, ImageFormat.Png);
					break;
				case SSFormat.Jpeg:
					fname	= Path.Combine(m_current_path, def.SS_PATH + fname + ".jpg");
					bitmap.Save(fname, ImageFormat.Jpeg);
					break;
				case SSFormat.Bmp:
				default:
					fname	= Path.Combine(m_current_path, def.SS_PATH + fname + ".bmp");
					bitmap.Save(fname, ImageFormat.Bmp);
					break;
				}
				handle.Free();
				bitmap.Dispose();
				buffer	= null;
				System.GC.Collect();

				Cursor	= Cursors.Default;
				MessageBox.Show("スクリーンショットを保存しました。\n" + fname,
								"報告", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Process.Start(Path.Combine(m_current_path, def.SS_PATH));
			}catch{
				Cursor	= Cursors.Default;
				buffer	= null;
				System.GC.Collect();
				MessageBox.Show("スクリーンショットの保存に失敗しました。\nファイルの書き出しに失敗しました。",
								"報告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/*-------------------------------------------------------------------------
		 スクリーンショット
		 描画
		 地図と航路図のみ
		 航路図は表示設定がそのまま反映される
		---------------------------------------------------------------------------*/
		private void screen_shot_draw()
		{
			// 画面のクリア
			m_lib.device.Clear(Color.Black);

			// 描画開始
			if(!m_lib.device.Begin())	return;		// デバイスロスト中

			// 地図描画
			draw_map();

			// GvoDatabase
			m_db.DrawForScreenShot();
			m_lib.device.End();
		}

		/*-------------------------------------------------------------------------
		 スクリーンショット
		 得られたイメージを連結する
		 strideは通常size_xと同じ値となる
		---------------------------------------------------------------------------*/
		private void screen_shot_chain_image(UInt16[] image, int size_x, int size_y, int stride, int offset_x, int offset_y, Surface offscreen)
		{
			// 転送先の矩形外のときはそのまま帰る
			if(offset_x >= size_x)								return;
			if(offset_y >= size_y)								return;
			if(offset_x + offscreen.Description.Width < 0)		return;
			if(offset_y + offscreen.Description.Height < 0)		return;

			// ロック
			int			pitch;
			UInt16[,]	buf		= (UInt16[,])offscreen.LockRectangle(typeof(UInt16), LockFlags.ReadOnly, out pitch, offscreen.Description.Height, offscreen.Description.Width);

			// 転送先の矩形に収まるようにサイズを修正する
			int			src_size_x	= offscreen.Description.Width;
			int			src_size_y	= offscreen.Description.Height;
			if(offset_x + src_size_x > size_x)	src_size_x	-= (offset_x + src_size_x) - size_x;
			if(offset_y + src_size_y > size_y)	src_size_y	-= (offset_y + src_size_y) - size_y;

			// マイナスオフセットに対応
			int			start_x	= 0;
			int			start_y	= 0;
			if(offset_x < 0){
				start_x		= -offset_x;
				offset_x	= 0;
				src_size_x	-= start_x;
			}
			if(offset_y < 0){
				start_y		= -offset_y;
				offset_y	= 0;
				src_size_y	-= start_y;
			}
	
			// 転送
			int	index	= (offset_y * stride) + offset_x;
			for(int y=0; y<src_size_y; y++){
				for(int x=0; x<src_size_x; x++){
					image[index + x]	= buf[start_y + y, start_x + x];
				}
				index	+= stride;
			}
			
			// ロック解除
			offscreen.UnlockRectangle();
		}
		#endregion

		#region 同盟国変更用メニュー関係
		/*-------------------------------------------------------------------------
		 同盟国変更用メニューを開く
		---------------------------------------------------------------------------*/
		public void ShowChangeDomainsMenuStrip(Point pos)
		{
			contextMenuStrip1.Show(this, pos);
		}

		/*-------------------------------------------------------------------------
		 同盟国変更
		---------------------------------------------------------------------------*/
		private void ToolStripMenuItem_country0_Click(object sender, EventArgs e){	set_domain(GvoWorldInfo.Country.England);		}
		private void ToolStripMenuItem_country1_Click(object sender, EventArgs e){	set_domain(GvoWorldInfo.Country.Spain);			}
		private void ToolStripMenuItem_country2_Click(object sender, EventArgs e){	set_domain(GvoWorldInfo.Country.Portugal);		}
		private void ToolStripMenuItem_country3_Click(object sender, EventArgs e){	set_domain(GvoWorldInfo.Country.Netherlands);	}
		private void ToolStripMenuItem_country4_Click(object sender, EventArgs e){	set_domain(GvoWorldInfo.Country.France);			}
		private void ToolStripMenuItem_country5_Click(object sender, EventArgs e){	set_domain(GvoWorldInfo.Country.Venezia);		}
		private void ToolStripMenuItem_country6_Click(object sender, EventArgs e){	set_domain(GvoWorldInfo.Country.Turkey);			}
		private void ToolStripMenuItem_country00_Click(object sender, EventArgs e){	set_domain(GvoWorldInfo.Country.Unknown);		}

		/*-------------------------------------------------------------------------
		 同盟国変更
		---------------------------------------------------------------------------*/
		private void set_domain(GvoWorldInfo.Country country)
		{
			if(m_item_window.info == null)										return;

			// 更新
			if(!m_db.World.SetDomain(m_item_window.info.Name, country))	return;
	
			// 更新された
			string	str	= m_db.World.GetNetUpdateString(m_item_window.info.Name);
			if(str == null)						return;

			// サーバに更新情報を送る
			// オフラインモード時は送らない
			if(!m_lib.setting.connect_network)	return;

			// どうも変更できなくなってる？
			// オリジナル交易Mapでも変更できない
//			str	= Useful.UrlEncodeShiftJis(str);
            Console.WriteLine("同盟国変更:" + HttpDownload.Download(def.URL_HP_ORIGINAL + @"/gvgetdomain.cgi?" + str, Encoding.UTF8));

//			str				= "/gvgetdomain.cgi?" + str;
//			string	Data	= tcp_text.Download("gvtrademap.daa.jp", str, Encoding.UTF8);
		}
		#endregion

		#region メインウインドウ 右クリックメニュー
		/*-------------------------------------------------------------------------
		 メインウインドウ
		 右クリックメニュー
		---------------------------------------------------------------------------*/
		private void main_window_context_menu(Point p)
		{
			m_memo_icon_pos		= m_lib.loop_image.MousePos2GlobalPos(p);
			m_memo_icon_data	= m_db.MapMark.FindData(m_memo_icon_pos);

			if(m_memo_icon_data == null){
				edit_memo_icon_ToolStripMenuItem.Enabled	= false;
				remove_memo_icon_ToolStripMenuItem.Enabled	= false;
			}else{
				edit_memo_icon_ToolStripMenuItem.Enabled	= true;
				remove_memo_icon_ToolStripMenuItem.Enabled	= true;
			}

			// 海域変動システムの更新
			string	name	= m_db.SeaArea.Find(m_memo_icon_pos);
			if(name == null){
				normal_sea_area_ToolStripMenuItem.Text		= "--を危険海域(通常状態)に設定する";
				normal_sea_area_ToolStripMenuItem.Enabled	= false;
				safty_sea_area_ToolStripMenuItem.Text		= "--を安全海域に設定する";
				safty_sea_area_ToolStripMenuItem.Enabled	= false;
				lawless_sea_area_ToolStripMenuItem.Text		= "--を無法海域に設定する";
				lawless_sea_area_ToolStripMenuItem.Enabled	= false;
			}else{
				normal_sea_area_ToolStripMenuItem.Text		= name + "を危険海域(通常状態)に設定する";
				normal_sea_area_ToolStripMenuItem.Enabled	= true;
				safty_sea_area_ToolStripMenuItem.Text		= name + "を安全海域に設定する";
				safty_sea_area_ToolStripMenuItem.Enabled	= true;
				lawless_sea_area_ToolStripMenuItem.Text		= name + "を無法海域に設定する";
				lawless_sea_area_ToolStripMenuItem.Enabled	= true;
			}
			contextMenuStrip2.Show(this, p);
		}
	
		/*-------------------------------------------------------------------------
		 メモアイコン
		 目的地アイコンを追加
		---------------------------------------------------------------------------*/
		private void set_target_memo_icon_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m_db.MapMark.Add(	m_memo_icon_pos,
							map_mark.map_mark_type.icon11,
							"目的地周辺です");
			m_memo_icon_data			= null;

			// 追加されたのに非表示だとあれなので強制表示とする
			m_lib.setting.draw_icons	= true;
		}

		/*-------------------------------------------------------------------------
		 メモアイコン
		 メモアイコンを追加
		---------------------------------------------------------------------------*/
		private void add_memo_icon_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using(map_mark_form dlg = new map_mark_form(transform.map_pos2_game_pos(m_memo_icon_pos, m_lib.loop_image))){
				if(dlg.ShowDialog(this) == DialogResult.OK){
					// 追加する
					m_db.MapMark.Add(	transform.game_pos2_map_pos(dlg.position, m_lib.loop_image),
									map_mark.map_mark_type.axis0 + dlg.icon_index,
									dlg.memo);

					// 追加されたのに非表示だとあれなので強制表示とする
					m_lib.setting.draw_icons	= true;
				}
			}
			m_memo_icon_data	= null;
		}

		/*-------------------------------------------------------------------------
		 メモアイコン
		 メモアイコンを編集
		---------------------------------------------------------------------------*/
		private void edit_memo_icon_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(m_memo_icon_data == null)	return;

			using(map_mark_form dlg = new map_mark_form(m_memo_icon_data.gposition, (int)m_memo_icon_data.type, m_memo_icon_data.memo)){
				if(dlg.ShowDialog(this) == DialogResult.OK){
					m_memo_icon_data.position	= transform.game_pos2_map_pos(dlg.position, m_lib.loop_image);
					m_memo_icon_data.type		= map_mark.map_mark_type.axis0 + dlg.icon_index;
					m_memo_icon_data.memo		= dlg.memo;

					// 追加されたのに非表示だとあれなので強制表示とする
					m_lib.setting.draw_icons	= true;
				}
			}
			m_memo_icon_data	= null;		// 参照を切る
		}

		/*-------------------------------------------------------------------------
		 メモアイコン
		 メモアイコンを削除
		---------------------------------------------------------------------------*/
		private void remove_memo_icon_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(m_memo_icon_data == null)	return;
			m_db.MapMark.RemoveData(m_memo_icon_data);
			m_memo_icon_data	= null;		// 参照を切る
		}

		/*-------------------------------------------------------------------------
		 メモアイコン
		 全ての目的地メモアイコンを削除
		---------------------------------------------------------------------------*/
		private void remove_all_target_memo_icon_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m_db.MapMark.RemoveAllTargetData();
			m_memo_icon_data	= null;

			// 非表示だとあれなので強制表示とする
			m_lib.setting.draw_icons	= true;
		}

		/*-------------------------------------------------------------------------
		 メモアイコン
		 全て削除
		---------------------------------------------------------------------------*/
		private void remove_all_memo_icon_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m_db.MapMark.RemoveAllData();
			m_memo_icon_data	= null;
		}
		/*-------------------------------------------------------------------------
		 マウス位置の海域群を危険海域に設定する
		---------------------------------------------------------------------------*/
		private void normal_sea_area_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			set_sea_area_rclick(sea_area.sea_area_once.sea_type.normal);
		}
		/*-------------------------------------------------------------------------
		 マウス位置の海域群を安全海域に設定する
		---------------------------------------------------------------------------*/
		private void safty_sea_area_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			set_sea_area_rclick(sea_area.sea_area_once.sea_type.safty);
		}
		/*-------------------------------------------------------------------------
		 マウス位置の海域群を無法海域に設定する
		---------------------------------------------------------------------------*/
		private void lawless_sea_area_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			set_sea_area_rclick(sea_area.sea_area_once.sea_type.lawless);
		}
		/*-------------------------------------------------------------------------
		 マウス位置の海域群を設定する
		---------------------------------------------------------------------------*/
		private void set_sea_area_rclick(sea_area.sea_area_once.sea_type type)
		{
			m_db.SeaArea.SetType(	m_db.SeaArea.Find(m_memo_icon_pos),
									type);
		}
		#endregion

		#region アイテムリスト 右クリック
		/*-------------------------------------------------------------------------
		 アイテムリストをクリック
		 現状右クリックのみ
		---------------------------------------------------------------------------*/
		private void listView1_MouseClick(object sender, MouseEventArgs e)
		{
			if((e.Button & MouseButtons.Right) == 0)	return;		// 右クリックのみ

			GvoWorldInfo.Info.Group.Data	d		= get_selected_item();
			if(d == null)								return;
	
			// 表示位置調整
			Point		pos	= new Point(e.X, e.Y);

			ItemDatabaseCustom.Data			db		= d.ItemDb;
			if(db == null){
				// アイテムデータベースと一致しない
				open_recipe_wiki0_ToolStripMenuItem.Enabled		= false;
				open_recipe_wiki1_ToolStripMenuItem.Enabled		= false;
				copy_all_to_clipboardToolStripMenuItem.Enabled	= false;
			}else{
				copy_all_to_clipboardToolStripMenuItem.Enabled	= true;
				if(db.IsSkill || db.IsReport){
					// スキルか報告
					open_recipe_wiki0_ToolStripMenuItem.Enabled	= false;
					open_recipe_wiki1_ToolStripMenuItem.Enabled	= false;
				}else{
					if(db.IsRecipe){
						// レシピ
						open_recipe_wiki0_ToolStripMenuItem.Enabled	= true;
						open_recipe_wiki1_ToolStripMenuItem.Enabled	= false;
					}else{
						// レシピ以外
						open_recipe_wiki0_ToolStripMenuItem.Enabled	= false;
						open_recipe_wiki1_ToolStripMenuItem.Enabled	= true;
					}
				}
			}
			contextMenuStrip3.Show(listView1, pos);
		}

		/*-------------------------------------------------------------------------
		 レシピ情報wikiを開く
		 レシピ検索
		---------------------------------------------------------------------------*/
		private void open_recipe_wiki0_toolStripMenuItem_Click(object sender, EventArgs e)
		{
			GvoWorldInfo.Info.Group.Data	item	= get_selected_item();
			if(item == null)		return;
			ItemDatabaseCustom.Data			db		= item.ItemDb;
			if(db == null)			return;

			db.OpenRecipeWiki0();
		}

		/*-------------------------------------------------------------------------
		 レシピ情報wikiを開く
		 作成可能かどうか検索
		---------------------------------------------------------------------------*/
		private void open_recipe_wiki1_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GvoWorldInfo.Info.Group.Data	item	= get_selected_item();
			if(item == null)		return;
			ItemDatabaseCustom.Data			db		= item.ItemDb;
			if(db == null)			return;

			db.OpenRecipeWiki1();
		}

		/*-------------------------------------------------------------------------
		 名称をクリップボードにコピーする
		---------------------------------------------------------------------------*/
		private void copy_name_to_clipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GvoWorldInfo.Info.Group.Data	item	= get_selected_item();
			if(item == null)		return;
			ItemDatabaseCustom.Data			db		= item.ItemDb;
			if(db == null){
				// アイテムデータベースにないのでアイテム名をコピーする
				Clipboard.SetText(item.Name);
			}else{
				// アイテムデータベースの名称を使う
				// アイテムデータベースの名称が正しい
				Clipboard.SetText(db.Name);
			}
		}

		/*-------------------------------------------------------------------------
		 詳細をクリップボードにコピーする
		---------------------------------------------------------------------------*/
		private void copy_all_to_clipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GvoWorldInfo.Info.Group.Data	item	= get_selected_item();
			if(item == null)			return;
			Clipboard.SetText(item.TooltipString);
		}

		/*-------------------------------------------------------------------------
		 選択されているアイテムを得る
		---------------------------------------------------------------------------*/
		private GvoWorldInfo.Info.Group.Data get_selected_item()
		{
			if(listView1.SelectedItems.Count <= 0)		return null;

			ListViewItem				item	= listView1.SelectedItems[0];
			if(item.Tag == null)						return null;
			return (GvoWorldInfo.Info.Group.Data)item.Tag;
		}

		/*-------------------------------------------------------------------------
		 スポット表示
		---------------------------------------------------------------------------*/
		private void spotToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			GvoWorldInfo.Info.Group.Data	d		= get_selected_item();
			if(d == null)								return;
			m_spot.SetSpot(spot.type.has_item, d.Name);
			UpdateSpotList();
		}

		/*-------------------------------------------------------------------------
		 スポット表示解除
		---------------------------------------------------------------------------*/
		private void reset_spot()
		{
			m_spot.SetSpot(spot.type.none, "");
			UpdateSpotList();
		}
		#endregion

		#region ドラッグ＆ドロップ
		/*-------------------------------------------------------------------------
		 DragEnter
		---------------------------------------------------------------------------*/
		private void gvtrademap_cs_form_DragEnter(object sender, DragEventArgs e)
		{
			// Textのドロップを受け入れる
			if(e.Data.GetDataPresent(DataFormats.Text)){
				e.Effect	= DragDropEffects.Copy;
			}else{
				e.Effect	= DragDropEffects.None;
			}
		}

		/*-------------------------------------------------------------------------
		 DragDrop
		---------------------------------------------------------------------------*/
		private void gvtrademap_cs_form_DragDrop(object sender, DragEventArgs e)
		{
			AllowDrop	= false;
			try{
				string				data	= e.Data.GetData(DataFormats.Text) as string;

				List<sea_area_once_from_dd>	list	= sea_area.AnalizeFromDD(data);
				sea_area_dd_form			dlg		= new sea_area_dd_form(m_lib, m_db, list);
				if(dlg.ShowDialog(this) == DialogResult.OK){
					// 反映させる
					// フィルタの内容を反映したリストを使用する
					// 海域情報は全て初期化してから反映される
					m_db.SeaArea.UpdateFromDD(dlg.filterd_list, true);
				}
			}catch{
				MessageBox.Show("海域情報受け取りに失敗しました。");
			}
			AllowDrop	= true;
		}
		#endregion

		#region 定例処理関係
		/*-------------------------------------------------------------------------
		 スポットリクエストを実行する
		---------------------------------------------------------------------------*/
		private void do_spot_request()
		{
			if(m_lib.setting.req_spot_item.IsRequest()){
				// スポット開始
				GvoDatabase.Find	item	= m_lib.setting.req_spot_item.Arg1 as GvoDatabase.Find;
				m_item_window.SpotItem(item);
			}else if(m_lib.setting.req_spot_item_changed.IsRequest()){
				// スポット対象の変更
				spot.spot_once	item	= m_lib.setting.req_spot_item_changed.Arg1 as spot.spot_once;
				m_item_window.SpotItemChanged(item);
			}
		}

		/*-------------------------------------------------------------------------
		 指定された位置をセンタリング
		---------------------------------------------------------------------------*/
		private void centering_pos(Point pos)
		{
			Point	offset	= new Point(0, 0);
			if(m_item_window.window_mode == d3d_windows.window.mode.normal){
				offset	= new Point((int)(m_item_window.pos.X + m_item_window.size.X), 0);
			}
			m_lib.loop_image.MoveCenterOffset(pos, offset);
			m_lib.device.SetMustDrawFlag();
		}
		#endregion

		#region キー入力関係
		/*-------------------------------------------------------------------------
		 キーが押された
		---------------------------------------------------------------------------*/
		private void gvtrademap_cs_form_KeyDown(object sender, KeyEventArgs e)
		{
#if DEBUG_COMPASS
			// コンパス解析デバッグ用
			switch(e.KeyData){
			case Keys.Up:
				m_db.capture.m_angle_x	-= 0.5f;
				break;
			case Keys.Down:
				m_db.capture.m_angle_x	+= 0.5f;
				break;
			case Keys.Left:
				m_db.capture.m_factor	-= 0.5f;
				break;
			case Keys.Right:
				m_db.capture.m_factor	+= 0.5f;
				break;
			case Keys.NumPad1:
				m_db.capture.m_l		-= 0.5f;
				break;
			case Keys.NumPad2:
				m_db.capture.m_l		+= 0.5f;
				break;
			}
#endif
		}

		/*-------------------------------------------------------------------------
		 Cmd実行
		---------------------------------------------------------------------------*/
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			// フォーカスがTextBoxのときは優先的にTextBoxにキーを渡す
			if(this.ActiveControl != textBox1){
				// キー割り当て管理を呼ぶ
				if(m_lib.KeyAssignManager.ProcessCmdKey(keyData)){
					// キー割り当て管理で処理された
					return true;
				}
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		/*-------------------------------------------------------------------------
		 Cmd実行コールバック
		---------------------------------------------------------------------------*/
		private void process_cnd_key(object sender, KeyAssignEventArg arg)
		{
			if(!(arg.Tag is KeyFunction))	return;
			ExecFunction((KeyFunction)arg.Tag);
		}
		#endregion

		#region 機能の実行
		/*-------------------------------------------------------------------------
		 機能の実行
		---------------------------------------------------------------------------*/
		public void ExecFunction(KeyFunction func)
		{
			switch(func){
			case KeyFunction.map_change:
				// 地図切り替え
				if(!is_load){
					if(++m_lib.setting.map >= MapIndex.Max){
						m_lib.setting.map = MapIndex.Map1;
					}
				}
				break;
			case KeyFunction.map_reset_scale:
				// スケールのリセット
				// メインウインドウの中心を中心にスケール変更
				m_lib.loop_image.SetScale(1, client_center(), true);
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.blue_line_reset:
				// 角度計算のリセット
				m_db.SpeedCalculator.ResetAngle();
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.map_zoom_in:
				// 拡大
				zoom_map(true, client_center());
				break;
			case KeyFunction.map_zoom_out:
				// 縮小
				zoom_map(false, client_center());
				break;
			case KeyFunction.window_top_most:
				// 最前面表示切り替え
				m_lib.setting.window_top_most	= !m_lib.setting.window_top_most;
				break;
			case KeyFunction.window_maximize:
				// 最大化
				switch(this.WindowState){
				case FormWindowState.Normal:
					this.WindowState	= FormWindowState.Maximized;
					break;
				case FormWindowState.Maximized:
					this.WindowState	= FormWindowState.Normal;
					break;
				case FormWindowState.Minimized:
					this.WindowState	= FormWindowState.Normal;
					break;
				}
				break;
			case KeyFunction.window_minimize:
				// 最小化
				if(this.WindowState != FormWindowState.Minimized){
					this.WindowState	= FormWindowState.Minimized;
				}
				break;
			case KeyFunction.folder_open_01:
				// ログフォルダを開く
				Process.Start(gvo_def.GetGvoLogPath());
				break;	
			case KeyFunction.folder_open_02:
				// メールフォルダを開く
				Process.Start(gvo_def.GetGvoMailPath());
				break;	
			case KeyFunction.folder_open_03:
				// スクリーンショットフォルダを開く
				Process.Start(gvo_def.GetGvoScreenShotPath());
				break;	
			case KeyFunction.folder_open_00:
				// 航路図のスクリーンショットフォルダを開く
				Process.Start(Path.Combine(m_current_path, def.SS_PATH));
				break;
			case KeyFunction.cancel_spot:
				// スポットの中止
				reset_spot();
				break;
			case KeyFunction.cancel_select_sea_routes:
				// 航路図の選択をリセットする
				m_db.SeaRoute.ResetSelectFlag();
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.item_window_show_min:
				// アイテムウインドウの表示、最小化
				m_item_window.ToggleWindowMode();
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.item_window_next_tab:
				// アイテムウインドウの次のタブ
				m_item_window.ChangeTab(true);
				break;
			case KeyFunction.item_window_prev_tab:
				// アイテムウインドウの前のタブ
				m_item_window.ChangeTab(false);
				break;
			case KeyFunction.setting_window_show_min:
				// 設定ウインドウの表示、最小化
				m_setting_window.ToggleWindowMode();
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.setting_window_button_00:
				m_lib.setting.save_searoutes		= (m_lib.setting.save_searoutes)? false: true;
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.setting_window_button_01:
				m_lib.setting.draw_share_routes		= (m_lib.setting.draw_share_routes)? false: true;
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.setting_window_button_02:
				m_lib.setting.draw_web_icons		= (m_lib.setting.draw_web_icons)? false: true;
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.setting_window_button_03:
				m_lib.setting.draw_icons			= (m_lib.setting.draw_icons)? false: true;
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.setting_window_button_04:
				m_lib.setting.draw_sea_routes		= (m_lib.setting.draw_sea_routes)? false: true;
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.setting_window_button_05:
				if(m_lib.setting.draw_popup_day_interval == 0)		m_lib.setting.draw_popup_day_interval	= 1;
				else if(m_lib.setting.draw_popup_day_interval == 1)	m_lib.setting.draw_popup_day_interval	= 5;
				else												m_lib.setting.draw_popup_day_interval	= 0;
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.setting_window_button_06:
				m_lib.setting.draw_accident			= (m_lib.setting.draw_accident)? false: true;
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.setting_window_button_07:
				m_lib.setting.center_myship			= (m_lib.setting.center_myship)? false: true;
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.setting_window_button_08:
				m_lib.setting.draw_myship_angle		= (m_lib.setting.draw_myship_angle)? false: true;
				m_lib.device.SetMustDrawFlag();
				break;
			case KeyFunction.setting_window_button_09:
				// 危険海域変動システムの設定
				edit_sea_area_dlg();
				break;
			case KeyFunction.setting_window_button_10:
				// スクリーンショットリクエスト
				m_lib.setting.req_screen_shot.Request();
				break;
			case KeyFunction.setting_window_button_11:
				// 航路一覧表示
				show_sea_routes_dialog();
				break;
			case KeyFunction.setting_window_button_12:
				// 検索ダイアログを開く
				show_find_dialog();
				break;
			case KeyFunction.setting_window_button_13:
				// 設定ダイアログを開く
				do_setting_dlg();
				break;
			case KeyFunction.setting_window_button_exec_gvoac:
				// 海域情報収集を起動する
				try{
					Process.Start(def.SEAAREA_APP_FNAME);
				}catch{
					MessageBox.Show("海域情報収集の起動に失敗しました。", "起動エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				break;
			case KeyFunction.window_change_border_style:
				// ウインドウ枠の表示/非表示
				{
					Size	size			= this.Size;
					this.FormBorderStyle	= (this.FormBorderStyle == FormBorderStyle.None)
												? FormBorderStyle.Sizable
												: FormBorderStyle.None;
					this.Size				= size;
					m_lib.setting.is_border_style_none	= this.FormBorderStyle == FormBorderStyle.None;
				}
				break;
			case KeyFunction.window_close:
				// 終了
				this.Close();
				break;
			}
		}

		/*-------------------------------------------------------------------------
		 地図の拡縮
		---------------------------------------------------------------------------*/
        private void zoom_map(bool is_zoom_in, Point center)
		{
			// メインウインドウのスケール変更	
			float	scale;
			if(is_zoom_in)		scale	= m_lib.loop_image.ImageScale + 0.05f;
			else				scale	= m_lib.loop_image.ImageScale - 0.05f;

			// 適当に丸めておく
			scale	= (float)Math.Round((double)scale, 2);
			m_lib.loop_image.SetScale(scale, center, true);

			m_lib.device.SetMustDrawFlag();
		}

		/*-------------------------------------------------------------------------
		 クライアント矩形の中心を得る
		---------------------------------------------------------------------------*/
		private Point client_center()
		{
			Rectangle	rect	= this.ClientRectangle;
			return new Point(rect.Width / 2, rect.Height / 2);
		}

		/*-------------------------------------------------------------------------
		 検索ダイアログを表示する
		---------------------------------------------------------------------------*/
		private void show_find_dialog()
		{
			show_find_dialog(true);
		}
		private void show_find_dialog(bool is_active_find_mode)
		{
			if(is_active_find_mode)		m_find_form.SetFindMode();

			if(!m_find_form.Visible){
				// 表示されていないので表示する
				m_find_form.Show(this);
			}else{
				// 表示されているのでアクティブにする
				// 検索時のみでスポット時はフォーカスを移さない
				if(is_active_find_mode)		m_find_form.Activate();
			}
			m_lib.device.SetMustDrawFlag();
		}

		/*-------------------------------------------------------------------------
		 航路図一覧ダイアログを表示する
		---------------------------------------------------------------------------*/
		private void show_sea_routes_dialog()
		{
			if(!m_sea_routes_form.Visible){
				m_sea_routes_form.Show(this);
			}else{
				m_sea_routes_form.Activate();
			}
		}

		/*-------------------------------------------------------------------------
		 危険海域変更システムの設定ダイアログを開く
		---------------------------------------------------------------------------*/
		private void edit_sea_area_dlg()
		{
			using(setting_sea_area_form dlg = new setting_sea_area_form(m_db.SeaArea)){
				if(dlg.ShowDialog(this) == DialogResult.OK){
					// 更新する
					dlg.Update(m_db.SeaArea);
				}
			}
		}

		/*-------------------------------------------------------------------------
		 設定ダイアログを開く
		---------------------------------------------------------------------------*/
		private void do_setting_dlg()
		{
			string	info	= m_lib.device.deviec_info_string;
			using(setting_form2	dlg	= new setting_form2(m_lib.setting, m_lib.KeyAssignManager.List, info)){
				if(dlg.ShowDialog(this) == DialogResult.OK){
					// 設定項目を反映させる
					UpdateSettings(dlg);
				}
			}
		}

		/*-------------------------------------------------------------------------
		 設定内容を更新する
		---------------------------------------------------------------------------*/
		public void UpdateSettings(setting_form2 dlg)
		{
			m_lib.setting.Clone(dlg.setting);
			m_lib.KeyAssignManager.List	= dlg.KeyAssignList;
		}

		/*-------------------------------------------------------------------------
		 スポット一覧を表示する
		 スポット一覧の更新も含む
		---------------------------------------------------------------------------*/
		public void UpdateSpotList()
		{
			if(m_spot == null)			return;
			if(m_find_form == null)		return;

			if(m_spot.is_spot){
				show_find_dialog(false);
			}
			m_find_form.UpdateSpotList();
			this.Activate();
			m_lib.device.SetMustDrawFlag();
		}
		#endregion

		/*-------------------------------------------------------------------------
		 多重起動時にすでに起動している 交易MAP C# をアクティブにする
		 まだウインドウを作成していないこと
		---------------------------------------------------------------------------*/
		public static void ActiveGVTradeMap()
		{
			IntPtr	hwnd	= user32.FindWindowA(null, def.WINDOW_TITLE);
			if(hwnd == IntPtr.Zero)		return;		// 見つからない

			// アクティブにする
			user32.SetForegroundWindow(hwnd);
		}

#if DEBUG_COMPASS
		/*-------------------------------------------------------------------------
		 コンパスデバッグ
		---------------------------------------------------------------------------*/
		private void debug_compass()
		{
			m_lib.device.DrawText(font_type.normal,
									String.Format("angle={0}, fov={1}, l={2}", m_db.capture.m_angle_x, m_db.capture.m_factor, m_db.capture.m_l),
									10, 32, Color.Black);
		}
#endif

#if DEBUG_ANGLE_LINE
		/*-------------------------------------------------------------------------
		 進行方向線デバッグ
		---------------------------------------------------------------------------*/
		private void debug_angle_line()
		{
			Vector2		pos			= new Vector2(	this.ClientRectangle.Width /2,
													this.ClientRectangle.Height /2);
			// 速度(測量座標系)
			float		speed_map	= speed_calculator.KnotToMapSpeed(10.0f);
//			float		speed_map	= speed_calculator.KmToMapSpeed(1);

			++m_debug_angle_i;
			if(m_debug_angle_i < 60){
			}else if(m_debug_angle_i < 60*2){
				speed_map	= speed_calculator.KnotToMapSpeed(10.5f);
			}else if(m_debug_angle_i < 60*3){
				speed_map	= speed_calculator.KnotToMapSpeed(10.2f);
			}else{
				m_debug_angle_i	= 0;
			}
			
			m_lib.device.DrawText(font_type.normal,
									String.Format("angle={0}, speed={1}, {2}knt, {3}km/h",
													m_debug_angle,
													speed_map,
													speed_calculator.MapToKnotSpeed(speed_map),
													speed_calculator.MapToKmSpeed(speed_map)),
									10, 32, Color.Black);

			draw_angle_line(pos, m_lib.loop_image, m_debug_angle, Color.Black);
			draw_step_position2(pos, m_lib.loop_image, m_debug_angle, speed_map);

			m_debug_angle	+= 1.0f;
			if(m_debug_angle >= 360)	m_debug_angle	-= 360;
		}
#endif

#if DEBUG_DRAW_DEBUG_STRING
		/*-------------------------------------------------------------------------
		 デバッグ用文字列の描画
		---------------------------------------------------------------------------*/
		private void debug_draw_debug_string()
		{
			m_lib.device.systemfont.locate	= new Vector3(10, 100, 0.1f);
			m_lib.device.systemfont.Puts(String.Format("Marge map = {0}msec\n", m_lib.loop_image.MargeImageMS), Color.Black);
/*			m_lib.device.systemfont.Puts(String.Format("sprites = {0}\n", m_lib.device.sprites.draw_sprites_in_frame), Color.Black);
			m_lib.device.systemfont.Puts(String.Format("texturedfont cash = {0}\n", m_lib.device.textured_font.cash_count), Color.Black);
			m_lib.device.systemfont.Puts(String.Format("share BB = {0}\n", m_db.share_routes.share_bb.Count), Color.Black);

			// マウスカーソルの位置
			// ゲーム座標とマップ座標
			Point	map_pos		= m_lib.loop_image.MousePos2GlobalPos(m_old_mouse_pos);
			Point	game_pos	= transform.client_pos2_game_pos(m_old_mouse_pos, m_lib.loop_image);
			m_lib.device.systemfont.Puts(String.Format("game({0},{1}) map({2},{3})\n", game_pos.X, game_pos.Y, map_pos.X, map_pos.Y), Color.Black);
*/
		}
#endif
	}
}
