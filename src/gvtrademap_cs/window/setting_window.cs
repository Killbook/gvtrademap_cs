/*-------------------------------------------------------------------------

 左のウインドウ
 設定アイコンを描画する

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using System;
using System.Windows.Forms;

using directx;
using Utility;
using System.Diagnostics;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	class setting_window : d3d_windows.window
	{
		// 位置とサイズ(サイズは自動で調整される)
		private const int				WINDOW_POS_X			= 3;
		private const int				WINDOW_POS_Y			= 3;
		private const float				WINDOW_POS_Z			= 0.2f;
		private const int				WINDOW_SIZE_X			= 250;	// 初期サイズ
		private const int				WINDOW_SIZE_Y			= 200;	// 初期サイズ

		// クライアントサイズ
		// 縦はウインドウサイズから計算される
		private const int				CLIENT_SIZE_X			= (16+4)*(int)setting_icons_index.max + 6;

		// 配置間隔
		private const int				SETTING_ICONS_STEP		= def.ICON_SIZE_X+0;

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		private gvtrademap_cs_form		m_form;
		private gvt_lib					m_lib;						// 
		private GvoDatabase				m_db;						// データベース

		private hittest_list			m_hittest_list;				// 矩形管理
	
		private enum item_index{
			setting,			// 設定
			setting_button,		// 設定ボタン
			max
		};

		private enum setting_icons_index{
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
			max
		};

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public setting_window(gvt_lib lib, GvoDatabase db, gvtrademap_cs_form form)
			: base(lib.device, new Vector2(WINDOW_POS_X, WINDOW_POS_Y), new Vector2(WINDOW_SIZE_X, WINDOW_SIZE_Y), WINDOW_POS_Z)
		{
			base.title				= "設定ウインドウ";

			m_form					= form;
			m_lib					= lib;
			m_db					= db;

			// アイテム追加
			m_hittest_list			= new hittest_list();

			// 設定
			m_hittest_list.Add(new hittest());
			// 設定ボタン
			m_hittest_list.Add(new hittest());
		}

		/*-------------------------------------------------------------------------
		 更新
		---------------------------------------------------------------------------*/
		override protected void OnUpdateClient()
		{
			base.client_size	= new Vector2(CLIENT_SIZE_X, def.ICON_SIZE_Y);

			// 左下固定
			base.pos		= new Vector2(WINDOW_POS_X, screen_size.Y - base.size.Y - 4);

			Point		offset	= transform.ToPoint(base.client_pos);

			// オフセットの更新
			foreach(hittest h in m_hittest_list){
				h.position	= offset;
			}
	
			hittest		ht;

			// 設定
			ht		= m_hittest_list[(int)item_index.setting];
			ht.rect	= new Rectangle(1, 0, SETTING_ICONS_STEP * (int)setting_icons_index.max, def.ICON_SIZE_Y);

			// 設定ボタン
			ht		= m_hittest_list[(int)item_index.setting_button];
			ht.rect	= new Rectangle((int)base.client_size.X - 48, 0, 48, def.ICON_SIZE_Y);
		}

		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		override protected void OnDrawClient()
		{
			// 設定リストの背景
			draw_seting_back();

			// 現在のマウスがある場所の設定
			draw_current_setting_back();
	
			base.device.sprites.BeginDrawSprites(m_lib.icons.texture);{
				// 設定
				draw_setting();

				// 設定ボタン
				draw_setting_button();

			}base.device.sprites.EndDrawSprites();
		}

		/*-------------------------------------------------------------------------
		 設定リストの背景
		---------------------------------------------------------------------------*/
		private void draw_seting_back()
		{
			hittest		ht		= m_hittest_list[(int)item_index.setting];
			Rectangle	rect	= ht.CalcRect();

			Vector3		pos		= new Vector3(base.client_pos.X, rect.Y, base.z);
			base.device.DrawFillRect(pos, new Vector2(base.client_size.X, rect.Height+1), Color.FromArgb(255, 96, 96, 96).ToArgb());
		}

		/*-------------------------------------------------------------------------
		 現在のマウスがある場所の設定
		---------------------------------------------------------------------------*/
		private void draw_current_setting_back()
		{
			Point pos	= base.device.GetClientMousePosition();
	
			hittest		ht		= m_hittest_list[(int)item_index.setting];
			if(!ht.HitTest(pos))												return;
			Rectangle	rect	= ht.CalcRect();

			pos.X	-= rect.X;
			pos.X	/= SETTING_ICONS_STEP;

			base.DrawCurrentButtonBack(	new Vector3(rect.X + SETTING_ICONS_STEP * pos.X, rect.Y, base.z),
										new Vector2(def.ICON_SIZE_X, def.ICON_SIZE_Y));
		}
	
		/*-------------------------------------------------------------------------
		 設定ボタン
		---------------------------------------------------------------------------*/
		private void draw_setting_button()
		{
			hittest		ht		= m_hittest_list[(int)item_index.setting_button];
			Rectangle	rect	= ht.CalcRect();

			Vector3		pos		= new Vector3(rect.X, rect.Y, base.z);
			base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon(icons.icon_index.setting_button));
		}

		/*-------------------------------------------------------------------------
		 設定
		---------------------------------------------------------------------------*/
		private void draw_setting()
		{
			hittest		ht		= m_hittest_list[(int)item_index.setting];
			Rectangle	rect	= ht.CalcRect();

			Vector3		pos		= new Vector3(rect.X, rect.Y, base.z);
			
			// 
			base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon((m_lib.setting.save_searoutes)
											? icons.icon_index.setting_0
											: icons.icon_index.setting_gray_0));
			pos.X	+= SETTING_ICONS_STEP;
			base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon((m_lib.setting.draw_share_routes)
											? icons.icon_index.setting_1
											: icons.icon_index.setting_gray_1));
			pos.X	+= SETTING_ICONS_STEP;
			base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon((m_lib.setting.draw_web_icons)
											? icons.icon_index.setting_8
											: icons.icon_index.setting_gray_8));
			pos.X	+= SETTING_ICONS_STEP;
			base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon((m_lib.setting.draw_icons)
											? icons.icon_index.setting_2
											: icons.icon_index.setting_gray_2));

			pos.X	+= SETTING_ICONS_STEP;
			base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon((m_lib.setting.draw_sea_routes)
											? icons.icon_index.setting_3
											: icons.icon_index.setting_gray_3));
			pos.X	+= SETTING_ICONS_STEP;
			base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon((m_lib.setting.draw_popup_day_interval == 1)
											? icons.icon_index.setting_4
											: (m_lib.setting.draw_popup_day_interval == 5)
											? icons.icon_index.setting_12
											: icons.icon_index.setting_gray_4));
			pos.X	+= SETTING_ICONS_STEP;
			base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon((m_lib.setting.draw_accident)
											? icons.icon_index.setting_5
											: icons.icon_index.setting_gray_5));
			pos.X	+= SETTING_ICONS_STEP;
			base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon((m_lib.setting.center_myship)
											? icons.icon_index.setting_6
											: icons.icon_index.setting_gray_6));
			pos.X	+= SETTING_ICONS_STEP;
			base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon((m_lib.setting.draw_myship_angle)
											? icons.icon_index.setting_7
											: icons.icon_index.setting_gray_7));
			pos.X	+= SETTING_ICONS_STEP;

			// 残り3つはグレー表示なし
			for(int i=0; i<3; i++){
				base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon(icons.icon_index.setting_10 + i));
				pos.X	+= SETTING_ICONS_STEP;
			}
		}

		/*-------------------------------------------------------------------------
		 マウスクリック
		---------------------------------------------------------------------------*/
		override protected void OnMouseClikClient(Point pos, MouseButtons button)
		{
			if((button & MouseButtons.Left) != 0){
				_window_on_mouse_l_click(pos);
			}else if((button & MouseButtons.Right) != 0){
				_window_on_mouse_r_click(pos);
			}
		}

		/*-------------------------------------------------------------------------
		 マウスダブルクリック
		---------------------------------------------------------------------------*/
		override protected void OnMouseDClikClient(Point pos, MouseButtons button)
		{
			if((button & MouseButtons.Left) != 0){
				_window_on_mouse_l_click(pos);
			}
		}

		/*-------------------------------------------------------------------------
		 マウス左クリック
		---------------------------------------------------------------------------*/
		private void _window_on_mouse_l_click(Point pos)
		{
			switch(m_hittest_list.HitTest_Index(pos)){
			case (int)item_index.setting:
				on_mouse_l_click_setting(pos);
				break;
			case (int)item_index.setting_button:
				on_mouse_l_click_setting_button(pos);
				break;
			default:
				break;
			}
		}

		/*-------------------------------------------------------------------------
		 マウス左クリック
		 設定項目
		---------------------------------------------------------------------------*/
		private void on_mouse_l_click_setting(Point pos)
		{
			hittest		ht		= m_hittest_list[(int)item_index.setting];
			Rectangle	rect	= ht.CalcRect();

			pos.X	-= rect.X;
			pos.X	/= SETTING_ICONS_STEP;

			setting_icons_index	index	= setting_icons_index.save_searoutes + pos.X;
			m_form.ExecFunction(get_extc_function(index));
		}

		/*-------------------------------------------------------------------------
		 実行する機能を得る
		---------------------------------------------------------------------------*/
		private KeyFunction get_extc_function(setting_icons_index index)
		{
			switch(index){
			case setting_icons_index.save_searoutes:		return KeyFunction.setting_window_button_00;
			case setting_icons_index.share_routes:			return KeyFunction.setting_window_button_01;
			case setting_icons_index.web_icons:				return KeyFunction.setting_window_button_02;
			case setting_icons_index.memo_icons:			return KeyFunction.setting_window_button_03;
			case setting_icons_index.searoutes:				return KeyFunction.setting_window_button_04;
			case setting_icons_index.popup_day_interval:	return KeyFunction.setting_window_button_05;
			case setting_icons_index.accident:				return KeyFunction.setting_window_button_06;
			case setting_icons_index.center_my_ship:		return KeyFunction.setting_window_button_07;
			case setting_icons_index.myship_angle:			return KeyFunction.setting_window_button_08;
			case setting_icons_index.sea_area:				return KeyFunction.setting_window_button_09;
			case setting_icons_index.screen_shot:			return KeyFunction.setting_window_button_10;
			case setting_icons_index.show_searoutes_list:	return KeyFunction.setting_window_button_11;
			}

			// 未定義の機能を返す
			return KeyFunction.unknown_function;
		}

		/*-------------------------------------------------------------------------
		 設定ボタン
		---------------------------------------------------------------------------*/
		private void on_mouse_l_click_setting_button(Point pos)
		{
			hittest		ht		= m_hittest_list[(int)item_index.setting_button];
			Rectangle	rect	= ht.CalcRect();
			
			pos.X	-= rect.X;

			if(pos.X < 16){
				// 検索
				m_form.ExecFunction(KeyFunction.setting_window_button_12);
			}else{
				// 設定
				m_form.ExecFunction(KeyFunction.setting_window_button_13);
			}
		}

		/*-------------------------------------------------------------------------
		 ツールチップ表示用の文字列を得る
		 表示すべき文字列がない場合nullを返す
		---------------------------------------------------------------------------*/
		override protected string OnToolTipStringClient(Point pos)
		{
			switch(m_hittest_list.HitTest_Index(pos)){
			case (int)item_index.setting:			return get_tooltip_string_setting(pos);
			case (int)item_index.setting_button:	return get_tooltip_string_setting_button(pos);
			}
			return null;
		}
	
		/*-------------------------------------------------------------------------
		 ツールチップ表示用の文字列を得る
		 表示すべき文字列がない場合nullを返す
		 設定アイコン
		---------------------------------------------------------------------------*/
		private string get_tooltip_string_setting(Point pos)
		{
			hittest		ht		= m_hittest_list[(int)item_index.setting];
			Rectangle	rect	= ht.CalcRect();

			pos.X	-= rect.X;
			pos.X	/= SETTING_ICONS_STEP;

			setting_icons_index		index	= setting_icons_index.save_searoutes + pos.X;
			string	tip;
			switch(index){
			case setting_icons_index.save_searoutes:		tip	= "航路記録（要測量）"; break;
			case setting_icons_index.share_routes:			tip	= "共有している船表示（要設定）"; break;
			case setting_icons_index.web_icons:				tip	= "@Webアイコン表示\n右クリックで表示項目設定"; break;
			case setting_icons_index.memo_icons:			tip	= "メモアイコン表示\n右クリックで表示項目設定"; break;
			case setting_icons_index.searoutes:				tip	= "航路線表示"; break;
			case setting_icons_index.popup_day_interval:	tip	= "ふきだし表示"; break;
			case setting_icons_index.accident:				tip	= "災害表示\n右クリックで表示項目設定"; break;
			case setting_icons_index.center_my_ship:		tip	= "現在位置中心表示"; break;
			case setting_icons_index.myship_angle:			tip	= "コンパスの角度線、進路予想線表示\n右クリックで表示項目設定"; break;
			case setting_icons_index.sea_area:				tip	= "危険海域変動システムの設定"; break;
			case setting_icons_index.screen_shot:			tip	= "航路のスクリーンショット保存"; break;
			case setting_icons_index.show_searoutes_list:	tip	= "航路図一覧\n右クリックで航路図設定"; break;
			default:
				return null;
			}
			return tip + get_assign_shortcut_text(get_extc_function(index));
		}

		/*-------------------------------------------------------------------------
		 アサインされたキーの文字列を得る
		 アサインされていなければ""を返す
		---------------------------------------------------------------------------*/
		private string get_assign_shortcut_text(KeyFunction function)
		{
			string	shortcut	= m_lib.KeyAssignManager.List.GetAssignShortcutText(function);
			if(String.IsNullOrEmpty(shortcut))	return "";

			return "(" + shortcut + ")";
		}

		/*-------------------------------------------------------------------------
		 ツールチップ表示用の文字列を得る
		 表示すべき文字列がない場合nullを返す
		 設定ボタン
		---------------------------------------------------------------------------*/
		private string get_tooltip_string_setting_button(Point pos)
		{
			hittest		ht		= m_hittest_list[(int)item_index.setting_button];
			Rectangle	rect	= ht.CalcRect();
			
			pos.X	-= rect.X;

			if(pos.X < 16){
				return "できるだけ検索ダイアログを開く" + get_assign_shortcut_text(KeyFunction.setting_window_button_12);
			}else{
				return "設定ダイアログを開く" + get_assign_shortcut_text(KeyFunction.setting_window_button_13);
			}
		}

		/*-------------------------------------------------------------------------
		 マウス右クリック
		---------------------------------------------------------------------------*/
		private void _window_on_mouse_r_click(Point pos)
		{
			switch(m_hittest_list.HitTest_Index(pos)){
			case (int)item_index.setting:
				on_mouse_r_click_setting(pos);
				break;
			default:
				break;
			}
		}
	
		/*-------------------------------------------------------------------------
		 マウス右クリック
		 設定項目
		---------------------------------------------------------------------------*/
		private void on_mouse_r_click_setting(Point pos)
		{
			hittest		ht		= m_hittest_list[(int)item_index.setting];
			Rectangle	rect	= ht.CalcRect();
	
			pos.X	-= rect.X;
			pos.X	/= SETTING_ICONS_STEP;

			setting_icons_index	index	= setting_icons_index.save_searoutes + pos.X;
			switch(index){
			case setting_icons_index.save_searoutes:
				break;
			case setting_icons_index.share_routes:
				break;
			case setting_icons_index.searoutes:
				break;
			case setting_icons_index.popup_day_interval:
				break;
			case setting_icons_index.center_my_ship:
				break;
			case setting_icons_index.sea_area:
				break;
			case setting_icons_index.screen_shot:
				break;

			case setting_icons_index.web_icons:
				set_draw_setting(DrawSettingPage.WebIcons);
				break;
			case setting_icons_index.memo_icons:
				set_draw_setting(DrawSettingPage.MemoIcons);
				break;
			case setting_icons_index.accident:
				set_draw_setting(DrawSettingPage.Accidents);
				break;
			case setting_icons_index.myship_angle:
				set_draw_setting(DrawSettingPage.MyShipAngle);
				break;
			case setting_icons_index.show_searoutes_list:
				{
					string	info	= m_lib.device.deviec_info_string;
					using(setting_form2	dlg	= new setting_form2(m_lib.setting, m_lib.KeyAssignManager.List, info, setting_form2.tab_index.sea_routes)){
						if(dlg.ShowDialog(base.device.form) == DialogResult.OK){
							// 設定項目を反映させる
							m_form.UpdateSettings(dlg);
						}
					}
				}
				break;
			}
		}

		/*-------------------------------------------------------------------------
		 表示項目設定
		---------------------------------------------------------------------------*/
		private void set_draw_setting(DrawSettingPage type)
		{
			string	info	= m_lib.device.deviec_info_string;
			using(setting_form2	dlg	= new setting_form2(m_lib.setting, m_lib.KeyAssignManager.List, info, type)){
				if(dlg.ShowDialog(base.device.form) == DialogResult.OK){
					// 設定項目を反映させる
					m_form.UpdateSettings(dlg);
				}
			}
		}
	}
}
