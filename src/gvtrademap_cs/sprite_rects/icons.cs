/*-------------------------------------------------------------------------

 アイコン管理

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

using directx;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	public class icons : d3d_sprite_rects
	{
		public enum icon_index{
			// 日付変わり時のふきだしアイコン
			days_big_shadow,	// 影

			// 日付表示用数字
			number_0,
			number_1,
			number_2,
			number_3,
			number_4,
			number_5,
			number_6,
			number_7,
			number_8,
			number_9,

			// 自分の船
			myship,

			// 国アイコン
			country_0,
			country_1,
			country_2,
			country_3,
			country_4,
			country_5,
			country_6,
			country_7,
			country_8,
			country_9,
			country_10,
			country_11,

			// タブアイコン
			tab_0,
			tab_1,
			tab_2,
			tab_3,
			tab_4,
			tab_5,
			tab_6,
			tab_7,
			tab_8,
			tab_9,
			tab_10,

			tab_gray_0,
			tab_gray_1,
			tab_gray_2,
			tab_gray_3,
			tab_gray_4,
			tab_gray_5,
			tab_gray_6,
			tab_gray_7,
			tab_gray_8,
			tab_gray_9,
			tab_gray_10,

			// 人物とかのアイコン
			tab2_0,
			tab2_1,
			tab2_2,
			tab2_3,

			tab2_gray_0,
			tab2_gray_1,
			tab2_gray_2,
			tab2_gray_3,

			// 設定アイコン
			setting_0,
			setting_1,
			setting_2,
			setting_3,
			setting_4,
			setting_5,
			setting_6,
			setting_7,
			setting_8,

			setting_gray_0,
			setting_gray_1,
			setting_gray_2,
			setting_gray_3,
			setting_gray_4,
			setting_gray_5,
			setting_gray_6,
			setting_gray_7,
			setting_gray_8,

			// スクリーンショットと航路図削除はグレイ表示なし
			setting_10,
			setting_11,
			setting_11_1,

			// 5日アイコン
			setting_12,

			// スポット
			spot_0,
			spot_1,
			spot_2,

			// 上陸地点の地図
			map_icon,

			// 選択用三角
			select_0,
			select_1,

			// 選択用クロスカーソル
			select_cross,

			// 設定ボタン
			setting_button,

			// 速度表示用背景
			speed_background,

			// スポット用国アイコン
			spot_country_0,
			spot_country_1,
			spot_country_2,
			spot_country_3,
			spot_country_4,
			spot_country_5,
			spot_country_6,
			spot_country_7,
			spot_country_8,
			spot_country_9,
			spot_country_10,
			spot_country_11,

			// 人物とかのアイコン
			spot_tab2_0,
			spot_tab2_1,
			spot_tab2_2,
			spot_tab2_3,

			// メモ用アイコン
			// 風向き
			memo_icon_0,
			memo_icon_1,
			memo_icon_2,
			memo_icon_3,
			memo_icon_4,
			memo_icon_5,
			memo_icon_6,
			memo_icon_7,
			// 災害アイコン
			memo_icon_8,
			memo_icon_9,
			memo_icon_10,
			memo_icon_11,
			memo_icon_12,
			memo_icon_13,
			memo_icon_14,
			memo_icon_15,
			memo_icon_16,
			memo_icon_17,
			memo_icon_18,
			memo_icon_19,

			// メモ用アイコン
			// 風向き
			web_icon_0,
			web_icon_1,
			web_icon_2,
			web_icon_3,
			web_icon_4,
			web_icon_5,
			web_icon_6,
			web_icon_7,
			// web災害アイコン
			web_icon_8,
			web_icon_9,
			web_icon_10,
			web_icon_11,
			web_icon_12,

			// 災害アイコン
			accident_0,
			accident_1,
			accident_2,
			accident_3,
			accident_4,
			accident_5,
			accident_6,
			accident_7,
			accident_8,
			accident_9,
			accident_10,

			accident_popup,
			accident_popup_shadow,
			
			// スポット用タブアイコン
			spot_tab_0,
			spot_tab_1,
			spot_tab_2,
			spot_tab_3,
			spot_tab_4,
			spot_tab_5,
			spot_tab_6,
			spot_tab_7,
			spot_tab_8,
			spot_tab_9,
			spot_tab_10,

			// 航路共有 街に居る
			share_city,

			// 街、郊外
			city_icon_0,	// 首都
			city_icon_1,	// 街
			city_icon_2,	// 首都(イスラム)
			city_icon_3,	// 街(イスラム)
			city_icon_4,	
			city_icon_5,	
			city_icon_6,	
			city_icon_7,	

			// web
			web,

			// 度
			degree,

			// 日付変わり時のアイコン
			days_mini_6,
			// 日付変わり時のふきだしアイコン
			days_big_6,
			days_big_100,		// 100日以降の左右が大きいもの

			string00,			// 日
			string01,			// 航路共有
			string02,			// 利息まで
			string03,			// 季節
			string04,			// 地図
			string05,			// 冬
			string06,			// 夏
			string07,			// 造船から

			max
		};
		
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public icons(d3d_device device, string fname)
			: base(device, fname)
		{
			if(device.device == null)	return;

			// 矩形登録
			add_rects();
		}
		
		/*-------------------------------------------------------------------------
		 矩形登録
		 icon_indexの順で割り当てる
		---------------------------------------------------------------------------*/
		private void add_rects()
		{
			// 影
			AddRect(	new Vector2(-5, -6-8), 
								new Rectangle(24, 160, 23, 15));

			// 日付表示用数値
			{
				Vector2		offset	= new Vector2(-3, -16);
				for(int i=0; i<10; i++){
					AddRect(	offset, 
										new Rectangle(i * 8, 232, 6, 9));
				}
			}
			// 船
			{
				AddRect(	new Vector2(-6, -9), 
									new Rectangle(216, 32, 12, 11));
			}

			// 国アイコン
			{
				Vector2		offset	= new Vector2(0, 0);
				for(int i=0; i<12; i++){
					AddRect(	offset, 
										new Rectangle(0, i*16, 24, 16));
				}
			}

			// タブアイコン
			{
				Vector2		offset	= new Vector2(0, 0);
				for(int i=0; i<11; i++){
					AddRect(	offset, 
										new Rectangle(24 + i*16, 16, 16, 16));
				}
				for(int i=0; i<11; i++){
					AddRect(	offset, 
										new Rectangle(24 + i*16, 0, 16, 16));
				}
			}

			// 人物とかのアイコン
			{
				Vector2		offset	= new Vector2(0, 0);
				for(int i=0; i<2; i++){
					AddRect(	offset, 
										new Rectangle(24 + i*16, 48, 16, 16));
				}
				for(int i=0; i<2; i++){
					AddRect(	offset, 
										new Rectangle(200 + i*16, 16, 16, 16));
				}
				for(int i=0; i<2; i++){
					AddRect(	offset, 
										new Rectangle(24 + i*16, 32, 16, 16));
				}
				for(int i=0; i<2; i++){
					AddRect(	offset, 
										new Rectangle(200 + i*16, 0, 16, 16));
				}
			}

			// 設定アイコン
			{
				Vector2		offset	= new Vector2(0, 0);
				for(int i=0; i<8; i++){
					AddRect(	offset, 
										new Rectangle(56 + i*16, 48, 16, 16));
				}
				// @Web icon
				AddRect(	offset, 
									new Rectangle(216, 80, 16, 16));

				for(int i=0; i<8; i++){
					AddRect(	offset, 
										new Rectangle(56 + i*16, 32, 16, 16));
				}
				// @Web icon
				AddRect(	offset, 
									new Rectangle(216, 64, 16, 16));

				AddRect(	offset, 
									new Rectangle(232, 64, 16, 16));
				AddRect(	offset, 
									new Rectangle(184, 32, 16, 16));
				AddRect(	offset, 
									new Rectangle(184, 48, 16, 16));
				// 5日アイコン
				AddRect(	offset, 
									new Rectangle(200, 48, 16, 16));
			}

			// スポット
			{
				Vector2		offset	= new Vector2(-20, -20);
				for(int i=0; i<3; i++){
					if(i == 2)	offset	= new Vector2(-20, -26);
					AddRect(	offset, 
										new Rectangle(25 + i*41, 97, 40, 40));
				}
			}

			// web
			{
				Vector2		offset	= new Vector2(0, 0);
				AddRect(	offset, 
									new Rectangle(232, 0, 17, 17));
			}

			// 選択用三角
			{
				Vector2		offset	= new Vector2(0, -3);
				for(int i=0; i<2; i++){
					AddRect(	offset, 
										new Rectangle(40 + i*8, 144, 8, 8));
				}
			}

			// 選択用クロスカーソル
			{
				Vector2		offset	= new Vector2(-6, -6);
				AddRect(	offset, 
									new Rectangle(24, 144, 14, 14));
			}

			// 設定ボタン
			{
				Vector2		offset	= new Vector2(0, 0);
				AddRect(	offset, 
									new Rectangle(128, 144, 48	, 17));
			}

			// 速度表示用背景
			{
				Vector2		offset	= new Vector2(0, 0);
				AddRect(	offset, 
									new Rectangle(80, 144, 43+4	, 16));
			}

			// スポット用国アイコン
			{
				Vector2		offset	= new Vector2(-12, -8);
				for(int i=0; i<12; i++){
					AddRect(	offset, 
										new Rectangle(0, i*16, 24, 16));
				}
			}

			// スポット用人物とかのアイコン
			{
				Vector2		offset	= new Vector2(-8, -8);
				for(int i=0; i<2; i++){
					AddRect(	offset, 
										new Rectangle(24 + i*16, 48, 16, 16));
				}
				for(int i=0; i<2; i++){
					AddRect(	offset, 
										new Rectangle(200 + i*16, 16, 16, 16));
				}
			}
	
			// メモ用アイコン
			{
				// 風向き
				for(int i=0; i<8; i++){
					AddRect(	new Vector2(-6, -6),
										new Rectangle(64 + i*16, 192, 12, 12));
				}
				// 災害アイコン
				for(int i=0; i<12; i++){
					if(i==11){
						AddRect(	new Vector2(-8, -13),		// 目的地のみ特殊
											new Rectangle(24 + i*16 + 1, 64 + 1, 15, 15));
					}else{
						AddRect(	new Vector2(-8, -8),
											new Rectangle(24 + i*16, 64, 16, 16));
					}
				}
			}

			// web icon
			{
				// 風向き
				for(int i=0; i<8; i++){
					AddRect(	new Vector2(-6, -6),
										new Rectangle(64 + i*16, 192, 12, 12));
				}
				// 災害アイコン
				for(int i=0; i<5; i++){
					AddRect(	new Vector2(-7, -7),
										new Rectangle(88 + i*16, 168, 14, 14));
				}
			}

			// 災害アイコン
			{
				Vector2		offset	= new Vector2(-8, -7 - 25);
				for(int i=0; i<11; i++){
					AddRect(	offset,
										new Rectangle(24 + i*16, 80, 16, 15));
				}

				offset		= new Vector2(-14, -33);
				AddRect(	offset,
									new Rectangle(152, 96, 30, 34));
				offset		= new Vector2(-3, -17);
				AddRect(	offset,
									new Rectangle(48, 168, 30, 18));

			}

			// スポット用タブアイコン
			{
				Vector2		offset	= new Vector2(-8, -8);
				for(int i=0; i<11; i++){
					AddRect(	offset, 
										new Rectangle(24 + i*16, 16, 16, 16));
				}
			}

			// 航路共有 街に居る
			{
				Vector2		offset	= new Vector2(-4, -4);
				AddRect(	offset, 
									new Rectangle(80, 160, 10, 10));
			}

			// 街、郊外
			{
				AddRect(	new Vector2(-11, -18), 
									new Rectangle(232, 160, 24, 26));
				AddRect(	new Vector2(-6, -11), 
									new Rectangle(232, 192, 14, 16));
				AddRect(	new Vector2(-10, -12), 
									new Rectangle(232, 216, 22, 20));
				AddRect(	new Vector2(-6, -11), 
									new Rectangle(232, 240, 14, 16));

				AddRect(	new Vector2(-5, -5), 
									new Rectangle(240, 96, 10, 10));
				AddRect(	new Vector2(-5, -5), 
									new Rectangle(240, 112, 10, 10));
				AddRect(	new Vector2(-4, -4), 
									new Rectangle(240, 128, 8, 8));
				AddRect(	new Vector2(-4, -4), 
									new Rectangle(240, 144, 8, 8));
			}

			{
				AddRect(	new Vector2(0, 0), 
									new Rectangle(168, 168, 16, 26));
				AddRect(	new Vector2(0, 0), 
									new Rectangle(208, 144, 12, 12));
			}

			// スクリプトにより切りだされたもの
			AddRect(new Vector2(-3, -5), new Rectangle(32, 184, 38 - 32, 190 - 184));	// 0
			AddRect(new Vector2(-8, -19), new Rectangle(0, 192, 18 - 0, 212 - 192));	// 1
			AddRect(new Vector2(-12, -19), new Rectangle(24, 192, 50 - 24, 212 - 192));	// 2
			AddRect(new Vector2(0, 0), new Rectangle(224, 144, 234 - 224, 156 - 144));	// 3
			AddRect(new Vector2(0, 0), new Rectangle(0, 216, 48 - 0, 228 - 216));	// 4
			AddRect(new Vector2(0, 0), new Rectangle(48, 216, 90 - 48, 228 - 216));	// 5
			AddRect(new Vector2(0, 0), new Rectangle(200, 192, 224 - 200, 204 - 192));	// 6
			AddRect(new Vector2(0, 0), new Rectangle(200, 208, 224 - 200, 220 - 208));	// 7
			AddRect(new Vector2(0, 0), new Rectangle(208, 160, 220 - 208, 172 - 160));	// 8
			AddRect(new Vector2(0, 0), new Rectangle(208, 176, 220 - 208, 188 - 176));	// 9
			AddRect(new Vector2(0, 0), new Rectangle(96, 216, 140 - 96, 228 - 216));	// 10
		}
	
		/*-------------------------------------------------------------------------
		 矩形を得る
		---------------------------------------------------------------------------*/
		public d3d_sprite_rects.rect GetIcon(icon_index index)
		{
			return GetRect((int)index);
		}
	}
}
