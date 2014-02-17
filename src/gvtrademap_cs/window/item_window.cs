/*-------------------------------------------------------------------------

 左のウインドウ
 アイテム情報を描画する

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
using Utility.KeyAssign;
using System.Diagnostics;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class item_window : d3d_windows.window
	{
		// 位置とサイズ(サイズは自動で調整される)
		private const int				WINDOW_POS_X			= 3;
		private const int				WINDOW_POS_Y			= 3;
		private const float				WINDOW_POS_Z			= 0.2f;
		private const int				WINDOW_SIZE_X			= 250;	// 初期サイズ
		private const int				WINDOW_SIZE_Y			= 200;	// 初期サイズ

		// 配置間隔
		private const int				TABS_STEP_X				= def.ICON_SIZE_X+4;
		private const int				ICONS_STEP_X			= def.ICON_SIZE_X+4;
		// 基本的な縦方向の間隔
		private const int				STEP_Y0					= def.ICON_SIZE_Y+2;	// 上部のアイコン用
		private const int				STEP_Y					= def.ICON_SIZE_Y+4;

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		private gvt_lib					m_lib;						// 
		private GvoDatabase				m_db;						// データベース
		private spot					m_spot;						// スポット表示
		private gvtrademap_cs_form		m_form;						// 右クリックメニュー専用

		private GvoWorldInfo.Info			m_info;						// 描画対象
		private int						m_tab_index;				// 現在のタブ位置

		private hittest_list			m_hittest_list;				// 矩形管理
		private Vector2					m_window_size;				// ウインドウサイズ

		private TextBox					m_memo_text_box;			// メモ用テキストBOX
		private ListView				m_list_view;				// アイテム表示用リストビュー

		private bool					m_is_1st_draw;
	
		private enum item_index{
			item_list,			// アイテムリスト
			country,			// 国アイコン
			icons,				// 上のアイコン
			tabs,				// タブ
			web,				// webアイコン
			county_name,		// 国名
			lang1,				// 言語1
			lang2,				// 言語2
			max
		};

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public GvoWorldInfo.Info　info{	get{	return m_info;		}
										set{	set_info(value, true);	}}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public item_window(gvt_lib lib, GvoDatabase db, spot _spot, TextBox memo_text_box, ListView list_view, gvtrademap_cs_form form)
			: base(lib.device, new Vector2(WINDOW_POS_X, WINDOW_POS_Y), new Vector2(WINDOW_SIZE_X, WINDOW_SIZE_Y), WINDOW_POS_Z)
		{
			base.title				= "アイテムウインドウ";

			m_window_size			= new Vector2(0, 0);
			m_memo_text_box			= memo_text_box;
			m_list_view				= list_view;
			m_form					= form;
	
			m_lib					= lib;

			m_db					= db;
			m_spot					= _spot;

			m_tab_index				= 0;
			m_info					= null;

			m_is_1st_draw			= true;

			// アイテム追加
			m_hittest_list			= new hittest_list();

			// アイテムリスト
			m_hittest_list.Add(new hittest());
			// 国アイコン
			m_hittest_list.Add(new hittest());
			// アイコンたち
			m_hittest_list.Add(new hittest());
			// タブ
			m_hittest_list.Add(new hittest());
			// webアイコン
			m_hittest_list.Add(new hittest());
			// 国名
			m_hittest_list.Add(new hittest());
			// 言語1
			m_hittest_list.Add(new hittest());
			// 言語2
			m_hittest_list.Add(new hittest());
		}

		/*-------------------------------------------------------------------------
		 info設定
		---------------------------------------------------------------------------*/
		private void set_info(GvoWorldInfo.Info _info, bool with_spot_reset)
		{
			if(_info == null)		return;

			if(with_spot_reset){
				// 無条件でスポット終了
				m_spot.SetSpot(spot.type.none, "");
				m_form.UpdateSpotList();
			}

			// 内容が同じ場合はなにもしない
			if(m_info == _info)		return;
			
			// メモ
			// テキストボックスの内容をデータベースに退避
			UpdateMemo();		// 更新

			// 更新
			m_info					= _info;
			// メモ用テキストボックス更新
			m_memo_text_box.Text	= m_info.Memo;
			// アイテムリスト更新
			update_item_list();
		}
	
		/*-------------------------------------------------------------------------
		 マウス押し始め
		---------------------------------------------------------------------------*/
		override protected void OnMouseDownClient(Point pos, MouseButtons button)
		{
			if((button & MouseButtons.Left) != 0){
				_window_on_mouse_l_down(pos);
			}else if((button & MouseButtons.Right) != 0){
				_window_on_mouse_r_down(pos);
			}
		}

		/*-------------------------------------------------------------------------
		 マウス左押し始め
		---------------------------------------------------------------------------*/
		private void _window_on_mouse_l_down(Point pos)
		{
			switch(m_hittest_list.HitTest_Index(pos)){
			case (int)item_index.tabs:
				on_mouse_l_click_item_tabs(pos);
				break;
			default:
				break;
			}
		}
	
		/*-------------------------------------------------------------------------
		 マウス右押し始め
		---------------------------------------------------------------------------*/
		private void _window_on_mouse_r_down(Point pos)
		{
			switch(m_hittest_list.HitTest_Index(pos)){
			case (int)item_index.tabs:
				on_mouse_l_click_item_tabs(pos);
				break;
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
		 マウス左クリック
		---------------------------------------------------------------------------*/
		private void _window_on_mouse_l_click(Point pos)
		{
			switch(m_hittest_list.HitTest_Index(pos)){
			case (int)item_index.country:
				on_mouse_l_click_country(pos);
				break;
			case (int)item_index.icons:
				on_mouse_l_click_icons(pos);
				break;
			case (int)item_index.county_name:
				on_mouse_l_click_country_name(pos);
				break;
			case (int)item_index.lang1:
				on_mouse_l_click_lang(pos, 0);
				break;
			case (int)item_index.lang2:
				on_mouse_l_click_lang(pos, 1);
				break;
			case (int)item_index.web:
				on_mouse_l_click_web(pos);
				break;
			default:
				break;
			}
		}

		/*-------------------------------------------------------------------------
		 マウス右クリック
		---------------------------------------------------------------------------*/
		private void _window_on_mouse_r_click(Point pos)
		{
			switch(m_hittest_list.HitTest_Index(pos)){
			case (int)item_index.tabs:
				on_mouse_r_click_item_tabs(pos);
				break;
			case (int)item_index.country:
				on_mouse_r_click_country(pos);
				break;
			case (int)item_index.county_name:
				on_mouse_r_click_country_name(pos);
				break;
			}
		}

		/*-------------------------------------------------------------------------
		 マウスホイール
		---------------------------------------------------------------------------*/
		protected override void OnMouseWheelClient(Point pos, int delta)
		{
			switch(m_hittest_list.HitTest_Index(pos)){
			case (int)item_index.tabs:
				on_mouse_wheel_item_tabs(pos, delta);
				break;
			default:
				break;
			}
		}

		/*-------------------------------------------------------------------------
		 更新
		---------------------------------------------------------------------------*/
		override protected void OnUpdateClient()
		{
			// 最小化時はメモウインドウを非表示にする
			if(base.window_mode == d3d_windows.window.mode.small){
				EnableMemoWindow(false);
				EnableItemWindow(false);
				return;
			}

			// 8dot分のギャップを持つ
			d3d_windows.window	iw	= base.FindWindow("設定ウインドウ");
			if(iw != null)		base.size	= new Vector2(iw.normal_size.X, base.screen_size.Y - 4*2);
			else				base.size	= new Vector2(WINDOW_SIZE_X, base.screen_size.Y - 4*2);

			// 矩形を設定し直す
			update_rects();

			if(base.size != m_window_size){
				// メモ
				hittest		ht		= m_hittest_list[(int)item_index.item_list];
				Rectangle	rect	= ht.CalcRect();
				m_memo_text_box.Location	= new Point(rect.Left, rect.Top);
				m_memo_text_box.Size		= new Size(rect.Width, rect.Height + 1);
				m_list_view.Location		= m_memo_text_box.Location;
				m_list_view.Size			= m_memo_text_box.Size;
				// コラムサイズ調整
				ajust_item_columns_width();
			}
			update_memo_window();

			m_window_size	= base.size;
		}

		/*-------------------------------------------------------------------------
		 矩形を設定し直す
		---------------------------------------------------------------------------*/
		private void update_rects()
		{
			Point		offset	= transform.ToPoint(base.client_pos);

			// オフセットの更新
			foreach(hittest h in m_hittest_list){
				h.position	= offset;
			}

			hittest		ht;
			int			pos_y	= 0;

			// 国アイコン
			ht		= m_hittest_list[(int)item_index.country];
			ht.rect	= new Rectangle(0, pos_y, 24, def.ICON_SIZE_Y);
			pos_y	+= STEP_Y0;

			// アイコンたち
			ht		= m_hittest_list[(int)item_index.icons];
			ht.rect	= new Rectangle(2, pos_y, ICONS_STEP_X * 4, def.ICON_SIZE_Y);

			// webアイコン
			ht		= m_hittest_list[(int)item_index.web];
			ht.rect	= new Rectangle(2+ ICONS_STEP_X * 4 + (10), pos_y, def.ICON_SIZE_X+1, def.ICON_SIZE_Y+1);

			// 国名
			ht		= m_hittest_list[(int)item_index.county_name];
			ht.rect	= new Rectangle(24+4, 0+2, 130, def.ICON_SIZE_Y);

			// 言語1
			ht		= m_hittest_list[(int)item_index.lang1];
			ht.rect	= new Rectangle((int)base.client_size.X - 130, 0+2, 130, def.ICON_SIZE_Y);

			// 言語2
			ht		= m_hittest_list[(int)item_index.lang2];
			ht.rect	= new Rectangle((int)base.client_size.X - 130, pos_y+1, 130, def.ICON_SIZE_Y);

			pos_y	+= STEP_Y0;

			// アイテムリスト
			// アイテム数がうまく収まるようにサイズを調整する
			int		size_y	= (int)base.client_size.Y - (pos_y + STEP_Y) - (int)base.pos.Y;
			d3d_windows.window	iw	= base.FindWindow("設定ウインドウ");
			if(iw != null)	size_y	-= (int)iw.size.Y;

			ht		= m_hittest_list[(int)item_index.item_list];
			ht.rect	= new Rectangle(0, pos_y, (int)base.client_size.X, size_y);
			pos_y	+= size_y + 2;

			// タブ
			ht		= m_hittest_list[(int)item_index.tabs];
			ht.rect	= new Rectangle(2, pos_y, TABS_STEP_X * 12, def.ICON_SIZE_Y);
			pos_y	+= STEP_Y;

			// 縦サイズを更新する
			base.client_size	= new Vector2(base.client_size.X, pos_y);
		}
	
		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		override protected void OnDrawClient()
		{
			if(m_is_1st_draw){
				update_item_list();
				m_is_1st_draw	= false;
			}
	
			// アイテムリスト
			// タブのカレントを描画
			draw_current_tab();

			// 国名
			draw_country_name();

			// 言語
			draw_lang();

			// マウスがある位置の背景
			draw_current_back();

			base.device.sprites.BeginDrawSprites(m_lib.icons.texture);{

				// 国アイコン
				draw_country();
		
				// アイコンたち
				draw_icons();

				// webアイコン
				draw_web();
	
				// タブ
				draw_tabs();

			}base.device.sprites.EndDrawSprites();

			// 選択中アイコン
//			draw_select_cursor();
		}

		/*-------------------------------------------------------------------------
		 選択中アイコン
		---------------------------------------------------------------------------*/
/*
		private void draw_select_cursor()
		{
			if(Info == null)				return;

			int			Index	= m_select_index - m_draw_top_item_index;
			if(Index < 0)					return;
			if(Index >= m_item_list_max)	return;

			hittest		ht		= m_hittest_list[(int)item_index.item_list];
			Rectangle	rect	= ht.CalcRect();

			Viewport	view	= base.SetViewport(rect.X, rect.Y,
													rect.Width + 1, rect.Height);

			Vector3		pos		= new Vector3(rect.X, rect.Y, base.z);

			pos.X	+= 2;
			pos.Y	+= Index * STEP_Y;
			pos.Y	+= STEP_Y / 2;
			base.Device.sprites.BeginDrawSprites(m_lib.icons.Texture);
			base.Device.sprites.AddDrawSprites(pos, m_lib.icons.GetIcon(icons.IconIndex.select_0));
			base.Device.sprites.EndDrawSprites();
			base.SetViewport(view);
		}
*/
		/*-------------------------------------------------------------------------
		 現在のマウスがある場所の設定
		---------------------------------------------------------------------------*/
		private void draw_current_back()
		{
			Point pos	= base.device.GetClientMousePosition();
	
			switch(m_hittest_list.HitTest_Index(pos)){
			case (int)item_index.icons:
				draw_current_back_icons(pos);
				break;
			case (int)item_index.web:
				draw_current_back_web(pos);
				break;
			case (int)item_index.tabs:
				draw_current_back_tabs(pos);
				break;
			}
		}	

		/*-------------------------------------------------------------------------
		 現在のマウスがある場所の設定
		 アイコン達
		---------------------------------------------------------------------------*/
		private void draw_current_back_icons(Point pos)
		{
			hittest		ht		= m_hittest_list[(int)item_index.icons];
			Rectangle	rect	= ht.CalcRect();

			pos.X	-= rect.X;
			pos.X	/= ICONS_STEP_X;		// Index;

			Vector3		dpos		= new Vector3(rect.X + ICONS_STEP_X * pos.X, rect.Y, base.z);
			dpos.X	-= 1;
			dpos.Y	-= 1;
			base.DrawCurrentButtonBack(dpos, new Vector2(def.ICON_SIZE_X+2, def.ICON_SIZE_Y+2));
		}

		/*-------------------------------------------------------------------------
		 現在のマウスがある場所の設定
		 Webアイコン
		---------------------------------------------------------------------------*/
		private void draw_current_back_web(Point pos)
		{
			if(info == null)		return;
			if(!info.IsUrl)		return;
	
			hittest		ht		= m_hittest_list[(int)item_index.web];
			Rectangle	rect	= ht.CalcRect();

			Vector3		dpos		= new Vector3(rect.X, rect.Y, base.z);
			dpos.X	-= 1;
			dpos.Y	-= 1;
			base.DrawCurrentButtonBack(dpos, new Vector2(def.ICON_SIZE_X+2, def.ICON_SIZE_Y+2));
		}

		/*-------------------------------------------------------------------------
		 現在のマウスがある場所の設定
		 タブ
		---------------------------------------------------------------------------*/
		private void draw_current_back_tabs(Point pos)
		{
			hittest		ht		= m_hittest_list[(int)item_index.tabs];
			Rectangle	rect	= ht.CalcRect();

			pos.X	-= rect.X;
			pos.X	/= TABS_STEP_X;

			Vector3		dpos		= new Vector3(rect.X + TABS_STEP_X * pos.X, rect.Y, base.z);
			dpos.X	-= 2;
			dpos.Y	-= 2;
			base.DrawCurrentButtonBack(dpos, new Vector2(def.ICON_SIZE_X+4, def.ICON_SIZE_Y+5));
		}

		/*-------------------------------------------------------------------------
		 国名
		---------------------------------------------------------------------------*/
		private void draw_country_name()
		{
			if(info == null)	return;
	
			hittest		ht		= m_hittest_list[(int)item_index.county_name];
			Rectangle	rect	= ht.CalcRect();

			Vector3		pos		= new Vector3(rect.X, rect.Y, base.z);

			// 名前
			base.device.DrawText(font_type.normal, info.Name,
									(int)pos.X, (int)pos.Y, Color.Black);
		}

		/*-------------------------------------------------------------------------
		 言語
		---------------------------------------------------------------------------*/
		private void draw_lang()
		{
			if(info == null)	return;
	
			hittest		ht		= m_hittest_list[(int)item_index.lang1];
			Rectangle	rect	= ht.CalcRect();

			Vector3		pos		= new Vector3(rect.X, rect.Y, base.z);

			// 言語
			if(info.InfoType == GvoWorldInfo.InfoType.City){
				if(info.Lang1 != ""){
					base.device.DrawTextR(font_type.normal, info.Lang1,
											(int)pos.X + rect.Width, (int)pos.Y, Color.Black);
				}
				if(info.Lang2 != ""){
					base.device.DrawTextR(font_type.normal, info.Lang2,
											(int)pos.X + rect.Width, (int)pos.Y + (STEP_Y0-1), Color.Black);
				}
			}else if(info.InfoType == GvoWorldInfo.InfoType.Sea){
				if(info.SeaInfo != null){
					// 最大速度上昇
					base.device.DrawTextR(font_type.normal, info.SeaInfo.SpeedUpRateString,
											(int)pos.X + rect.Width, (int)pos.Y, Color.Black);
				}
			}
		}

		/*-------------------------------------------------------------------------
		 webアイコン
		---------------------------------------------------------------------------*/
		private void draw_web()
		{
			if(info == null)	return;
			if(!info.IsUrl)	return;

			hittest		ht		= m_hittest_list[(int)item_index.web];
			Rectangle	rect	= ht.CalcRect();

			Vector3		pos		= new Vector3(rect.X, rect.Y, base.z);

			icons.icon_index	icon	= (info.InfoType == GvoWorldInfo.InfoType.City)
											? icons.icon_index.web
											: icons.icon_index.map_icon;
			base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon(icon));
		}

		/*-------------------------------------------------------------------------
		 カレントタブを描画する
		---------------------------------------------------------------------------*/
		private void draw_current_tab()
		{
			hittest		ht		= m_hittest_list[(int)item_index.item_list];
			Rectangle	rect	= ht.CalcRect();

			Vector3		pos		= new Vector3(rect.X, rect.Y, base.z);
			Vector2		size	= new Vector2(rect.Width, rect.Height);
			Vector3		pos2	= pos;

			// タブのカレント
			pos2		= pos;
			pos2.Y		+= size.Y;
			pos2.X		+= TABS_STEP_X * m_tab_index;
			base.device.DrawFillRect(pos2, new Vector2(TABS_STEP_X, STEP_Y), Color.FromArgb(128, 255, 255, 255).ToArgb());
			base.device.DrawLineRect(pos2, new Vector2(TABS_STEP_X, STEP_Y+1), Color.Black.ToArgb());
		}

		/*-------------------------------------------------------------------------
		 アイテムリスト数を得る
		---------------------------------------------------------------------------*/
		private int get_item_list_count()
		{
			return get_item_list_count(m_tab_index);
		}
		private int get_item_list_count(int index)
		{
			if(info == null)	return 0;
			if(index == 11)		return info.GetMemoLines();		// メモ
			return info.GetCount(GvoWorldInfo.Info.GroupIndex._0 + index);
		}

		/*-------------------------------------------------------------------------
		 国アイコン描画
		---------------------------------------------------------------------------*/
		private void draw_country()
		{
			if(info == null)		return;

			hittest		ht		= m_hittest_list[(int)item_index.country];
			Rectangle	rect	= ht.CalcRect();
			Vector3		pos		= new Vector3(rect.X, rect.Y, base.z);

			icons.icon_index	index;
			switch(info.InfoType){
			case GvoWorldInfo.InfoType.PF:
				index	= icons.icon_index.country_0;
				break;
			case GvoWorldInfo.InfoType.Sea:
				index	= icons.icon_index.country_2;
				break;
			case GvoWorldInfo.InfoType.Shore:
			case GvoWorldInfo.InfoType.OutsideCity:
				index	= icons.icon_index.country_3;
				break;
			case GvoWorldInfo.InfoType.Shore2:
				index	= icons.icon_index.country_1;
				break;
			case GvoWorldInfo.InfoType.City:
			default:
				index	= icons.icon_index.country_4;
				// 同盟関係により旗を選択する
				index	+= (int)info.MyCountry;
				break;
			}
	
			// アイコン
			base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon(index));
		}

		/*-------------------------------------------------------------------------
		 アイコンたち描画
		---------------------------------------------------------------------------*/
		private void draw_icons()
		{
			hittest		ht		= m_hittest_list[(int)item_index.icons];
			Rectangle	rect	= ht.CalcRect();

			Vector3		pos		= new Vector3(rect.X, rect.Y, base.z);

			for(int i=0; i<4; i++){
				icons.icon_index	index	= icons.icon_index.tab2_gray_0;
				if(info != null){
					if((info.Sakaba & (1<<i)) != 0){
						index	= icons.icon_index.tab2_0;
					}
				}
				base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon(index + i));
				pos.X	+= ICONS_STEP_X;
			}
		}

		/*-------------------------------------------------------------------------
		 タブたち描画
		---------------------------------------------------------------------------*/
		private void draw_tabs()
		{
			hittest		ht		= m_hittest_list[(int)item_index.tabs];
			Rectangle	rect	= ht.CalcRect();

			Vector3		pos		= new Vector3(rect.X, rect.Y, base.z);

			for(int i=0; i<12; i++){
				icons.icon_index	index	= icons.icon_index.tab_gray_0;
				if(info != null){
					if(get_item_list_count(i) > 0)	index	= icons.icon_index.tab_0;
				}
				base.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon(index + i));
				pos.X	+= TABS_STEP_X;
			}
		}

		/*-------------------------------------------------------------------------
		 マウス左クリック
		 タブ
		---------------------------------------------------------------------------*/
		private void on_mouse_l_click_item_tabs(Point pos)
		{
			hittest		ht		= m_hittest_list[(int)item_index.tabs];
			Rectangle	rect	= ht.CalcRect();

			pos.X	-= rect.X;
			ajust_tab_index(pos.X / TABS_STEP_X);
		}

		/*-------------------------------------------------------------------------
		 マウス右クリック
		 タブ
		---------------------------------------------------------------------------*/
		private void on_mouse_r_click_item_tabs(Point pos)
		{
			on_mouse_l_click_item_tabs(pos);

			// タブをスポット表示
			m_spot.SetSpot(spot.type.tab_0 + m_tab_index, "");
			m_form.UpdateSpotList();
		}

		/*-------------------------------------------------------------------------
		 マウスホイール
		 タブ
		---------------------------------------------------------------------------*/
		private void on_mouse_wheel_item_tabs(Point pos, int delta)
		{
			if(delta > 0)	ajust_tab_index(m_tab_index - 1);
			else			ajust_tab_index(m_tab_index + 1);
		}

		/*-------------------------------------------------------------------------
		 タブ位置の設定と調整
		---------------------------------------------------------------------------*/
		private void ajust_tab_index(int index)
		{
			// ループさせる
			if(index < 0)		index	= 11;
			if(index >= 12)		index	= 0;

			m_tab_index				= index;

			// メモのウインドウ状態更新
// ちらつく問題でここで update_memo_window() を呼び出すのをやめ
//			update_memo_window();
			// アイテムリスト更新
			update_item_list();
		}
	
		/*-------------------------------------------------------------------------
		 検索されたアイテムをスポット表示する
		---------------------------------------------------------------------------*/
		public void SpotItem(GvoDatabase.Find select)
		{
			if(select == null){
				// 選択が無ければスポットを解除する
				m_spot.SetSpot(spot.type.none, "");
				m_form.UpdateSpotList();
				return;
			}
			// アイテムデータベースはスポット不可能
			if(select.Type == GvoDatabase.Find.FindType.Database)		return;

			GvoWorldInfo.Info	_info	= m_db.World.FindInfo(select.InfoName);
			if(_info != null){
				// スポットのリセットなしで設定
				set_info(_info, false);
				// センタリングしてもらう
				req_centering_info();
			}

			switch(select.Type){
			case GvoDatabase.Find.FindType.Data:
			case GvoDatabase.Find.FindType.DataPrice:
				// アイテムから選択されたものを検索する
				find_item_for_selected(select.Data.Name);
				// アイテムをスポット
				m_spot.SetSpot(spot.type.has_item, select.Data.Name);
				break;
			case GvoDatabase.Find.FindType.Database:
				// アイテムデータベースはスポット不可能
				break;
			case GvoDatabase.Find.FindType.InfoName:
				// 街をスポット
				m_spot.SetSpot(spot.type.city_name, select.InfoName);
				break;
			case GvoDatabase.Find.FindType.Lang:
				// 言語をスポット
				m_spot.SetSpot(spot.type.language, select.Lang);
				break;
			case GvoDatabase.Find.FindType.CulturalSphere:
				// 文化圏
				m_spot.SetSpot(spot.type.cultural_sphere, select.InfoName);
				break;
			}
			m_form.UpdateSpotList();
		}

		/*-------------------------------------------------------------------------
		 スポットアイテムの変更
		 スポット情報は変更されない
		---------------------------------------------------------------------------*/
		public void SpotItemChanged(spot.spot_once select)
		{
			if(select == null)		return;
			if(select.info == null)	return;

			GvoWorldInfo.Info	_info	= select.info;
			if(_info != null){
				// スポットのリセットなしで設定
				set_info(_info, false);
				// センタリングしてもらう
				req_centering_info();
			}

			// アイテムから選択されたものを検索する
			find_item_for_selected(select.name);
		}

		/*-------------------------------------------------------------------------
		 アイテムから選択されたものを検索する
		---------------------------------------------------------------------------*/
		private void find_item_for_selected(string find_item)
		{
			if(info == null)	return;

			for(GvoWorldInfo.Info.GroupIndex i=GvoWorldInfo.Info.GroupIndex._0; i<GvoWorldInfo.Info.GroupIndex.max; i++){
				for(int s=0; s<info.GetCount(i); s++){
					GvoWorldInfo.Info.Group.Data	d	= info.GetData(i, s);
					if(d == null)				continue;
					if(d.Name != find_item)		continue;

					// 選択したアイテムが見えるようにスクロール位置を調整
					ajust_tab_index((int)i);

					// 表示位置を調整
					if(m_list_view.Items.Count >= s){
						m_list_view.Items[s].Selected	= true;
						m_list_view.Items[s].EnsureVisible();
						m_list_view.Items[s].Focused	= true;
					}
					return;
				}
			}
		}
	
		/*-------------------------------------------------------------------------
		 センタリングされている街をセンタリングする
		 リクエスト
		---------------------------------------------------------------------------*/
		private void req_centering_info()
		{
			if(info == null)	return;

			m_lib.setting.centering_gpos	= transform.map_pos2_game_pos(info.position, m_lib.loop_image);
			m_lib.setting.req_centering_gpos.Request();
		}

		/*-------------------------------------------------------------------------
		 国旗を左クリック
		---------------------------------------------------------------------------*/
		private void on_mouse_l_click_country(Point pos)
		{
			// 国旗をスポット表示
			// 街のみ
			if(info == null)											return;
			if(info.InfoType != GvoWorldInfo.InfoType.City)				return;

			m_spot.SetSpot(spot.type.country_flags, "");
			m_form.UpdateSpotList();
		}

		/*-------------------------------------------------------------------------
		 国旗を右クリック
		---------------------------------------------------------------------------*/
		private void on_mouse_r_click_country(Point pos)
		{
			// 街のみ
			if(info == null)											return;
			if(info.InfoType != GvoWorldInfo.InfoType.City)				return;
			// 同盟できる街のみ
			if(info.AllianceType != GvoWorldInfo.AllianceType.Alliance)	return;

			// 右クリックメニューを開く
			m_form.ShowChangeDomainsMenuStrip(pos);
		}

		/*-------------------------------------------------------------------------
		 名前を右クリック
		---------------------------------------------------------------------------*/
		private void on_mouse_r_click_country_name(Point pos)
		{
			// 街のみ
			if(info == null)											return;
			if(info.InfoType != GvoWorldInfo.InfoType.City)				return;
			if(info.CulturalSphere == GvoWorldInfo.CulturalSphere.Unknown)	return;

			// スポット表示
			SpotItem(new GvoDatabase.Find(info.CulturalSphere, ""));
		}
	
		/*-------------------------------------------------------------------------
		 アイコンたちを左クリック
		---------------------------------------------------------------------------*/
		private void on_mouse_l_click_icons(Point pos)
		{
			hittest		ht		= m_hittest_list[(int)item_index.icons];
			Rectangle	rect	= ht.CalcRect();

			pos.X	-= rect.X;
			pos.X	/= ICONS_STEP_X;		// Index;

			// アイコンをスポット表示
			m_spot.SetSpot(spot.type.icons_0 + pos.X, "");
			m_form.UpdateSpotList();
		}

		/*-------------------------------------------------------------------------
		 国名を左クリック
		---------------------------------------------------------------------------*/
		private void on_mouse_l_click_country_name(Point pos)
		{
			if(info == null)		return;

			// センタリングリクエスト
			req_centering_info();

			// スポット
			m_spot.SetSpot(spot.type.city_name, info.Name);
			m_form.UpdateSpotList();
		}

		/*-------------------------------------------------------------------------
		 言語を左クリック
		---------------------------------------------------------------------------*/
		private void on_mouse_l_click_lang(Point pos, int index)
		{
			if(info == null)		return;

			// 言語をスポット表示
			string	lang	= (index == 0)? info.Lang1: info.Lang2;
			if(lang == "")			return;

			m_spot.SetSpot(spot.type.language, lang);
			m_form.UpdateSpotList();
		}

		/*-------------------------------------------------------------------------
		 Webアイコンを左クリック
		---------------------------------------------------------------------------*/
		private void on_mouse_l_click_web(Point pos)
		{
			if(info == null)			return;
			if(!info.IsUrl)			return;

			if(info.UrlIndex != -1){
				string	url;
				if(info.InfoType == GvoWorldInfo.InfoType.City){
					// 大商戦
					url		= def.URL0 + info.UrlIndex.ToString();
				}else{
					// DKKmap
					url		= def.URL1 + info.UrlIndex.ToString() + ".html";
				}
				Process.Start("http://" + url);
			}else{
				// クリスタル商会
				Process.Start("http://" + def.URL5 + Useful.UrlEncodeEUCJP(info.Url));
			}
		}

		/*-------------------------------------------------------------------------
		 tabを切り替える
		---------------------------------------------------------------------------*/
		public void ChangeTab(bool is_next)
		{
			if(is_next){
				ajust_tab_index(m_tab_index + 1);
			}else{
				ajust_tab_index(m_tab_index - 1);
			}
			m_lib.device.SetMustDrawFlag();
		}
	
		/*-------------------------------------------------------------------------
		 ツールチップ表示用の文字列を得る
		 表示すべき文字列がない場合nullを返す
		---------------------------------------------------------------------------*/
		override protected string OnToolTipStringClient(Point pos)
		{
			switch(m_hittest_list.HitTest_Index(pos)){
			case (int)item_index.icons:				return get_tooltip_string_icons(pos);
			case (int)item_index.tabs:				return get_tooltip_string_tabs(pos);
			}
	
			// 以下はinfoを参照する必要があるもの
			if(info == null)		return null;

			switch(m_hittest_list.HitTest_Index(pos)){
			case (int)item_index.web:
				return info.GetToolTipString_HP();
			case (int)item_index.county_name:
				{
					string	tmp		= info.TooltipString;
					tmp	+= "\n左クリックで中心に移動";
					if(info.InfoType == GvoWorldInfo.InfoType.City){
						tmp	+= "\n右クリックで属する文化圏をスポット表示";
					}
					return tmp;
				}
			case (int)item_index.lang1:
			case (int)item_index.lang2:
				if(   (info.Lang1 != "")
					||(info.Lang2 != "") ){
					return "使用言語\n左クリックでスポット表示";
				}
				break;
			case (int)item_index.country:
				switch(info.InfoType){
				case GvoWorldInfo.InfoType.City:
					switch(info.AllianceType){
					case GvoWorldInfo.AllianceType.Piratical:
					case GvoWorldInfo.AllianceType.Unknown:		return info.AllianceTypeStr + "\n左クリックでスポット表示";
					case GvoWorldInfo.AllianceType.Alliance:	return info.AllianceTypeStr + " " + info.CountryStr + "\n左クリックでスポット表示\n右クリックで同盟国変更";

					case GvoWorldInfo.AllianceType.Capital:
					case GvoWorldInfo.AllianceType.Territory:
							return info.CountryStr + " " + info.AllianceTypeStr + "\n左クリックでスポット表示";
					}
					break;
				case GvoWorldInfo.InfoType.OutsideCity:
				case GvoWorldInfo.InfoType.PF:
				case GvoWorldInfo.InfoType.Sea:
				case GvoWorldInfo.InfoType.Shore:
				case GvoWorldInfo.InfoType.Shore2:
//					return Info.InfoTypeStr + "\n左クリックでスポット表示";
					return info.InfoTypeStr;
				}
				break;
			}
			return null;
		}

		/*-------------------------------------------------------------------------
		 ツールチップ表示用の文字列を得る
		 表示すべき文字列がない場合nullを返す
		 アイテムリスト
		---------------------------------------------------------------------------*/
		private string get_tooltip_string_item_list(GvoWorldInfo.Info.Group.Data d)
		{
			if(d == null)		return null;

			string	str			= d.TooltipString;

			// ヘルプを含める
//			if((!db.IsSkill)&&(!db.IsReport)){
				str			+= "\n(右クリックでメニューを開きます)";
//			}
			return str;
		}
	
		/*-------------------------------------------------------------------------
		 ツールチップ表示用の文字列を得る
		 表示すべき文字列がない場合nullを返す
		 アイコンたち
		---------------------------------------------------------------------------*/
		private string get_tooltip_string_icons(Point pos)
		{
			hittest		ht		= m_hittest_list[(int)item_index.icons];
			Rectangle	rect	= ht.CalcRect();

			pos.X	-= rect.X;
			pos.X	/= ICONS_STEP_X;		// Index;

			switch(pos.X){
			case 0:		return "請負人/酒場娘/販売員\n左クリックでスポット表示";
			case 1:		return "書庫\n左クリックでスポット表示";
			case 2:		return "翻訳家\n左クリックでスポット表示";
			case 3:		return "豪商\n左クリックでスポット表示";
			}
			return null;
		}

		/*-------------------------------------------------------------------------
		 ツールチップ表示用の文字列を得る
		 表示すべき文字列がない場合nullを返す
		 タブ
		---------------------------------------------------------------------------*/
		private string get_tooltip_string_tabs(Point pos)
		{
			hittest		ht		= m_hittest_list[(int)item_index.tabs];
			Rectangle	rect	= ht.CalcRect();

			pos.X	-= rect.X;
			pos.X	/= TABS_STEP_X;

            if (0 < pos.X && pos.X < 12)
            {
                return GvoWorldInfo.Info.GetGroupName((GvoWorldInfo.Info.GroupIndex)pos.X) + "\n右クリックでスポット表示";
            }
			return null;
		}	

		/*-------------------------------------------------------------------------
		 メモのウインドウ状態更新
		---------------------------------------------------------------------------*/
		private void update_memo_window()
		{
			if(m_tab_index == 11){
				EnableMemoWindow(true);
				EnableItemWindow(false);
			}else{
				EnableMemoWindow(false);
				EnableItemWindow(true);
			}
		}

		/*-------------------------------------------------------------------------
		 メモのウインドウ状態設定
		---------------------------------------------------------------------------*/
		public void EnableMemoWindow(bool is_enable)
		{
			enable_ctrl(m_memo_text_box, is_enable);
		}

		/*-------------------------------------------------------------------------
		 アイテムのウインドウ状態設定
		---------------------------------------------------------------------------*/
		public void EnableItemWindow(bool is_enable)
		{
			enable_ctrl(m_list_view, is_enable);
		}

		/*-------------------------------------------------------------------------
		 コントロールの状態設定
		---------------------------------------------------------------------------*/
		private void enable_ctrl(Control ctrl, bool is_enable)
		{
			// 表示設定
			if(is_enable){
				if(!ctrl.Visible){
					ctrl.Visible		= true;
				}

				// 編集可能不可能の設定
				if(m_info == null){
					if(ctrl.Enabled)	ctrl.Enabled	= false;
				}else{
					if(!ctrl.Enabled)	ctrl.Enabled	= true;
				}
			}else{
				if(ctrl.Visible){
					ctrl.Visible		= false;
					base.device.form.Focus();		// フォーカスを返す
				}
			}
		}

		/*-------------------------------------------------------------------------
		 メモを更新する
		 終了時の更新用
		---------------------------------------------------------------------------*/
		public void UpdateMemo()
		{
			if(info == null)		return;
			info.Memo	= m_memo_text_box.Text;
		}

		/*-------------------------------------------------------------------------
		 アイテムリスト更新
		---------------------------------------------------------------------------*/
		private void update_item_list()
		{
			if(info == null){
				// 内容クリア
				m_list_view.Clear();
				return;
			}

			// 更新開始
			m_list_view.BeginUpdate();

			// 内容クリア
			m_list_view.Clear();

			// ヘッダ
			switch(m_tab_index){
			case 0:
				m_list_view.Columns.Add("名称",	80);
				m_list_view.Columns.Add("★",	25, HorizontalAlignment.Center);
				m_list_view.Columns.Add("種類",	52, HorizontalAlignment.Center);
				if(info.InfoType == GvoWorldInfo.InfoType.City){
					m_list_view.Columns.Add("値段",	55, HorizontalAlignment.Right);
				}else{
					m_list_view.Columns.Add("スキル", 60, HorizontalAlignment.Center);
				}
				break;
			case 3:	// スキル、報告
				m_list_view.Columns.Add("名称",	140);
				m_list_view.Columns.Add("人物",	120);
				break;
			case 10:	// 行商人
				m_list_view.Columns.Add("名称",	160);
				m_list_view.Columns.Add("人物",	60);
				break;
			default:
				m_list_view.Columns.Add("名称",	160);
				m_list_view.Columns.Add("値段",	80, HorizontalAlignment.Right);
				break;
			}

			// 追加
			int						count	= info.GetCount(GvoWorldInfo.Info.GroupIndex._0 + m_tab_index);
			GvoWorldInfo.Info.Group	g		= info.GetGroup(GvoWorldInfo.Info.GroupIndex._0 + m_tab_index);
			System.Drawing.Font		font	= m_list_view.Font;
			for(int i=0; i<count; i++){
				GvoWorldInfo.Info.Group.Data	d		= g.GetData(i);
				ListViewItem				item	= new ListViewItem(d.Name, 0);
				item.UseItemStyleForSubItems		= false;
				item.ForeColor						= d.Color;
				item.Tag							= d;
				string		tt						= get_tooltip_string_item_list(d);
				if(tt != null)	item.ToolTipText	= tt;
				if(m_tab_index == 0){
					item.SubItems.Add((d.IsBonusItem)? "★": "", Color.Tomato, item.BackColor, font);
					item.SubItems.Add(	d.Type,
										d.CategolyColor, item.BackColor, font);
				}
				item.SubItems.Add(d.Price, d.PriceColor, item.BackColor, font);
				m_list_view.Items.Add(item);
			}

			// 更新終了
			m_list_view.EndUpdate();

			// コラムサイズ調整
			ajust_item_columns_width();
		}

		/*-------------------------------------------------------------------------
		 アイテムリストのコラム幅調整
		 最初のコラムのサイズを調整して横スクロールバーが出ないようにする
		---------------------------------------------------------------------------*/
		private void ajust_item_columns_width()
		{
			if(m_list_view.Columns.Count < 1)	return;
			if(m_tab_index == 3)				return;		// 人物タブは特別になにもしない

			Size	size	= m_list_view.ClientSize;
			// 最初のコラム以外のコラムの幅の合計を求める
			int		width	= 0;
			for(int i=1; i<m_list_view.Columns.Count; i++){
				width	+= m_list_view.Columns[i].Width;
			}
			// 余りを最初のコラムの幅とする
			width	= size.Width - width;
			if(width <= 0)	width	= 20;
			m_list_view.Columns[0].Width	= width;
		}

		/*-------------------------------------------------------------------------
		 アイテムリストにマウスがあるかどうかを返す
		 フォーカス判定用
		---------------------------------------------------------------------------*/
		public bool HitTest_ItemList(Point pos)
		{
			if(base.window_mode == mode.small)		return false;	// 最小化中
			if(m_hittest_list.HitTest_Index(pos) == (int)item_index.item_list){
				return true;
			}
			return false;
		}
	}
}
