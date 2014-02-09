/*-------------------------------------------------------------------------
 
 キーアサイン

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using Utility;
using Utility.KeyAssign;
using System.Windows.Forms;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{

#if aa
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public class gvotms_key_assign : key_assign_manager
	{
		// 機能
		public enum KEY_FUNCTION{
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
		public gvotms_key_assign()
			: base()
		{
			init();
		}
	
		/*-------------------------------------------------------------------------
		 初期化
		---------------------------------------------------------------------------*/
		private void init()
		{
			add_assign("地図の変更", "地図",Keys.M,													KEY_FUNCTION.map_change);
			add_assign("地図の縮尺リセット", "地図", Keys.Home,										KEY_FUNCTION.map_reset_scale);
			add_assign("地図を拡大", "地図", Keys.Add,													KEY_FUNCTION.map_zoom_in);
			add_assign("地図を縮小", "地図", Keys.Subtract,											KEY_FUNCTION.map_zoom_out);
			add_assign("ブルーラインリセット", "進行方向線", Keys.B,									KEY_FUNCTION.blue_line_reset);
			add_assign("スポット解除", "解除", Keys.Escape,											KEY_FUNCTION.cancel_spot);
			add_assign("航路図の選択解除", "解除", Keys.Escape,										KEY_FUNCTION.cancel_select_sea_routes);
			add_assign("アイテムウインドウの表示/最小化", "アイテムウインドウ", Keys.None,		KEY_FUNCTION.item_window_show_min);
			add_assign("次のタブへ移動", "アイテムウインドウ", Keys.Tab|Keys.Control,				KEY_FUNCTION.item_window_next_tab);
			add_assign("前のタブへ移動", "アイテムウインドウ", Keys.Tab|Keys.Control|Keys.Shift,	KEY_FUNCTION.item_window_prev_tab);
			add_assign("設定ウインドウの表示/最小化", "設定ウインドウ", Keys.None,					KEY_FUNCTION.setting_window_show_min);
			add_assign("航路記録ON/OFF", "設定ウインドウ", Keys.None,									KEY_FUNCTION.setting_window_button_00);
			add_assign("共有している船表示ON/OFF", "設定ウインドウ", Keys.None,						KEY_FUNCTION.setting_window_button_01);
			add_assign("@Webアイコン表示ON/OFF", "設定ウインドウ", Keys.None,						KEY_FUNCTION.setting_window_button_02);
			add_assign("メモアイコン表示ON/OFF", "設定ウインドウ", Keys.None,						KEY_FUNCTION.setting_window_button_03);
			add_assign("航路線表示ON/OFF", "設定ウインドウ", Keys.None,								KEY_FUNCTION.setting_window_button_04);
			add_assign("日付ふきだし表示切り替え", "設定ウインドウ", Keys.None,						KEY_FUNCTION.setting_window_button_05);
			add_assign("災害ポップアップ表示ON/OFF", "設定ウインドウ", Keys.None,					KEY_FUNCTION.setting_window_button_06);
			add_assign("現在位置中心表示ON/OFF", "設定ウインドウ", Keys.None,						KEY_FUNCTION.setting_window_button_07);
			add_assign("進行方向線表示ON/OFF", "設定ウインドウ", Keys.None,							KEY_FUNCTION.setting_window_button_08);
			add_assign("海域変動システムの設定", "設定ウインドウ", Keys.None,						KEY_FUNCTION.setting_window_button_09);
			add_assign("航路図のスクリーンショット", "設定ウインドウ", Keys.None,					KEY_FUNCTION.setting_window_button_10);
			add_assign("航路図一覧を開く", "設定ウインドウ", Keys.L|Keys.Control,					KEY_FUNCTION.setting_window_button_11);
			add_assign("できるだけ検索を開く", "設定ウインドウ", Keys.F|Keys.Control,				KEY_FUNCTION.setting_window_button_12);
			add_assign("設定ダイアログを開く", "設定ウインドウ", Keys.None,							KEY_FUNCTION.setting_window_button_13);
			add_assign("海域情報収集を起動する", "設定ウインドウ", Keys.A|Keys.Control,				KEY_FUNCTION.setting_window_button_exec_gvoac);
			add_assign("航路図のスクリーンショットフォルダを開く", "フォルダ", Keys.None,			KEY_FUNCTION.folder_open_00);
			add_assign("大航海時代Onlineのメールフォルダを開く", "フォルダ", Keys.None,			KEY_FUNCTION.folder_open_01);
			add_assign("大航海時代Onlineのチャットフォルダを開く", "フォルダ", Keys.None,			KEY_FUNCTION.folder_open_02);
			add_assign("大航海時代Onlineのスクリーンショットフォルダを開く", "フォルダ", Keys.None,	KEY_FUNCTION.folder_open_03);
			add_assign("最前面表示ON/OFF", "ウインドウ", Keys.None,									KEY_FUNCTION.window_top_most);
			add_assign("最大化/通常化", "ウインドウ", Keys.None,										KEY_FUNCTION.window_maximize);
			add_assign("最小化", "ウインドウ", Keys.None,												KEY_FUNCTION.window_minimize);
			add_assign("ウインドウ枠の表示/非表示", "ウインドウ", Keys.None,						KEY_FUNCTION.window_change_border_style);
			add_assign("交易MAP C#を閉じる", "ウインドウ", Keys.None,									KEY_FUNCTION.window_close);
		}

		/*-------------------------------------------------------------------------
		 機能の追加
		---------------------------------------------------------------------------*/
		private void add_assign(string name, string group, Keys key, KEY_FUNCTION kf)
		{
			AddAssign(name, group, key, kf, kf.ToString());
		}

		/*-------------------------------------------------------------------------
		 キー入力から機能を得る
		 同一のキーが割り当てられている場合すべてのリストが返る
		 KeyDownイベントから呼ばれることを期待している
		 割り当てられた機能がない場合nullを返す
		---------------------------------------------------------------------------*/
		public List<KEY_FUNCTION> KeysToFunction(KeyEventArgs e)
		{
			List<assign>	alist	= GetAssignedList(e);
			if(alist == null)	return null;		// 割り当てられた機能はない

			List<KEY_FUNCTION>	list	= new List<KEY_FUNCTION>();
			foreach(assign i in alist){
				// 機能に変換して追加
				try{
					KEY_FUNCTION	f	= (KEY_FUNCTION)i.Tag;
					list.Add(f);
				}catch{
				}
			}
			if(list.Count <= 0)	return null;
			return list;
		}
	}
#endif
}
