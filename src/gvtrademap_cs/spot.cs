﻿/*-------------------------------------------------------------------------

 スポット表示

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using System;

using directx;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public class spot
	{
		public enum type{
			none,						// スポットなし
			country_flags,				// 国旗表示
			icons_0,					// 看板娘
			icons_1,					// 書庫
			icons_2,					// 翻訳家
			icons_3,					// 豪商
			has_item,					// 指定したアイテムがある
			language,					// 言語

			tab_0,						// 交易
			tab_1,						// 道具
			tab_2,						// 工房
			tab_3,						// 人物
			tab_4,						// 船
			tab_5,						// 大砲
			tab_6,						// 板
			tab_7,						// 帆
			tab_8,						// 像
			tab_9,						// 行商人
			tab_10,						// メモ

			city_name,					// 街名
			cultural_sphere,			// 文化圏
		};

		/*-------------------------------------------------------------------------
		 外部参照用スポット内容
		---------------------------------------------------------------------------*/
		public class spot_once
		{
			private GvoWorldInfo.Info			m_info;				// スポット対象
			private string					m_name;				// 名前
			private string					m_ex;				// 追加する文字列
			
			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public GvoWorldInfo.Info	info	{	get{	return m_info;		}}
			public string			name	{	get{	return m_name;		}}
			public string			ex		{	get{	return m_ex;		}}
	
			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public spot_once(GvoWorldInfo.Info info, string name, string ex)
			{
				m_info		= info;
				m_name		= name;
				m_ex		= ex;
			}
		}
		
		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		private d3d_device					m_device;
		private GvoWorldInfo					m_world;
		private icons						m_icons;
		private LoopXImage				m_loop_image;

		private List<GvoWorldInfo.Info>		m_spots;			// スポット対象
		private type						m_spot_type;		// スポットタイプ
		private string						m_find_string;		// 検索文字列

		private List<spot_once>				m_spot_list;		// 外部参照向けリスト

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public bool is_spot{			get{	return m_spot_type != type.none;	}}
		public type sopt_type{			get{	return m_spot_type;					}}
		public string spot_type_str{	get{	return GetTypeString(m_spot_type);	}}
		public string spot_type_column_str{	get{	return GetExColumnString(m_spot_type);	}}
		public List<spot_once> list{	get{	return m_spot_list;					}}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public spot(gvt_lib lib, GvoWorldInfo world)
		{
			m_device		= lib.device;
			m_loop_image	= lib.loop_image;
			m_icons			= lib.icons;
			m_world			= world;

			m_spots			= new List<GvoWorldInfo.Info>();
			m_spot_list		= new List<spot_once>();
			m_spot_type		= type.none;
		}

		/*-------------------------------------------------------------------------
		 スポットの種類を設定する
		 スポットをやめるには Type.none を渡す
		---------------------------------------------------------------------------*/
		public void SetSpot(type _type, string find_str)
		{
			// 前回の内容をクリア
			m_spots.Clear();
			m_spot_list.Clear();
			m_find_string	= find_str;

			m_spot_type		= _type;
			switch(_type){
			case type.country_flags:		// 国旗
				// 国一覧を作成する
				foreach(GvoWorldInfo.Info i in m_world.World){
					if(i.InfoType == GvoWorldInfo.InfoType.CITY){
						m_spots.Add(i);
						m_spot_list.Add(new spot_once(i, i.CountryStr, i.AllianceTypeStr));
					}
				}
				break;
			case type.icons_0:				// 看板娘
			case type.icons_1:				// 書庫
			case type.icons_2:				// 翻訳家
			case type.icons_3:				// 豪商
				foreach(GvoWorldInfo.Info i in m_world.World){
					if((i.Sakaba & (1<<(_type-type.icons_0))) != 0){
						m_spots.Add(i);
						m_spot_list.Add(new spot_once(i, "", ""));
					}
				}
				break;
			case type.has_item:				// 指定したアイテムがある
				foreach(GvoWorldInfo.Info i in m_world.World){
					GvoWorldInfo.Info.Group.Data	d	= i.HasItem(find_str);
					if(d != null){
						m_spots.Add(i);
						m_spot_list.Add(new spot_once(i, find_str, d.Price));
					}
				}
				break;
			case type.language:				// 言語
				foreach(GvoWorldInfo.Info i in m_world.World){
					if(   (i.Lang1 == find_str)
						||(i.Lang2 == find_str) ){
						// 使用言語
						m_spots.Add(i);
						m_spot_list.Add(new spot_once(i, find_str, ""));
					}else{
						if(i.LearnPerson(find_str) != null){
							// 言語取得
							m_spots.Add(i);
							m_spot_list.Add(new spot_once(i, find_str, i.LearnPerson(find_str)));
						}
					}
				}
				break;
			case type.tab_0:				// 交易
			case type.tab_1:				// 道具
			case type.tab_2:				// 工房
			case type.tab_3:				// 人物
			case type.tab_4:				// 船
			case type.tab_5:				// 大砲
			case type.tab_6:				// 板
			case type.tab_7:				// 帆
			case type.tab_8:				// 像
			case type.tab_9:				// 行商人
				int	index	= (int)(_type - type.tab_0);
				foreach(GvoWorldInfo.Info i in m_world.World){
					if(i.GetCount(GvoWorldInfo.Info.GroupIndex._0 + index) > 0){
						m_spots.Add(i);
						m_spot_list.Add(new spot_once(i, "", ""));
					}
				}
				break;
			case type.tab_10:				// メモ
				foreach(GvoWorldInfo.Info i in m_world.World){
					if(i.GetMemoLines() > 0){
						m_spots.Add(i);
						m_spot_list.Add(new spot_once(i, "", ""));
					}
				}
				break;
			case type.city_name:			// 街名
				foreach(GvoWorldInfo.Info i in m_world.World){
					if(i.Name == find_str){
						m_spots.Add(i);
						m_spot_list.Add(new spot_once(i, "", ""));
					}
				}
				break;
			case type.cultural_sphere:		// 文化圏
				foreach(GvoWorldInfo.Info i in m_world.World){
					if(i.CulturalSphereStr == find_str){
						m_spots.Add(i);
						m_spot_list.Add(new spot_once(i, "文化圏", find_str));
					}
				}
				break;
			default:
				break;
			}
		}

		/*-------------------------------------------------------------------------
		 スポット表示
		---------------------------------------------------------------------------*/
		public void Draw()
		{
			if(m_spot_type == type.none)	return;
			if(m_spots.Count == 0)			return;

			// 暗くする
			m_device.DrawFillRect(new Vector3(0, 0, 0.3f+0.01f), new Vector2(m_device.client_size.X, m_device.client_size.Y), Color.FromArgb(160, 0, 0, 0).ToArgb());

			m_loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(draw_proc), 64f);
		}

		/*-------------------------------------------------------------------------
		 スポット表示
		---------------------------------------------------------------------------*/
		private void draw_proc(Vector2 offset, LoopXImage image)
		{
			switch(m_spot_type){
			case type.country_flags:		// 国旗
				draw_spot_mycountry(offset);
				spot_flags(offset);
				break;
			case type.icons_0:				// 看板娘
			case type.icons_1:				// 書庫
			case type.icons_2:				// 翻訳家
			case type.icons_3:				// 豪商
				draw_spot(offset);
				spot_cities(offset);
				spot_icons(offset);
				break;
			case type.has_item:				// 指定したアイテムがある
				draw_spot(offset);
				spot_cities(offset);
				break;
			case type.language:				// 言語
				draw_spot_for_lang(offset);
				spot_cities(offset);
				spot_learn_lang(offset);
				break;
			case type.tab_0:				// 交易
			case type.tab_1:				// 道具
			case type.tab_2:				// 工房
			case type.tab_3:				// 人物
			case type.tab_4:				// 船
			case type.tab_5:				// 大砲
			case type.tab_6:				// 板
			case type.tab_7:				// 帆
			case type.tab_8:				// 像
			case type.tab_9:				// 行商人
			case type.tab_10:				// メモ
				draw_spot(offset);
//				spot_cities(OffsetPosition);
				spot_tab(offset);
				break;
			case type.city_name:			// 街名
			case type.cultural_sphere:		// 文化圏
				draw_spot(offset);
				spot_cities(offset);
				break;
			}
		}

		/*-------------------------------------------------------------------------
		 スポット表示
		 国旗
		---------------------------------------------------------------------------*/
		private void spot_flags(Vector2 offset)
		{
			float	size	= m_loop_image.ImageScale;
			if(size < 0.5)		size	= 0.5f;
			else if(size > 1)	size	= 1;

			m_device.sprites.BeginDrawSprites(m_icons.texture, offset, m_loop_image.ImageScale, new Vector2(size, size));
			foreach(GvoWorldInfo.Info i in m_spots){
				m_device.sprites.AddDrawSprites(new Vector3(i.position.X, i.position.Y, 0.3f),
										m_icons.GetIcon(icons.icon_index.spot_country_4 + (int)i.Country));
			}
			m_device.sprites.EndDrawSprites();
		}

		/*-------------------------------------------------------------------------
		 スポット表示
		---------------------------------------------------------------------------*/
		private void draw_spot(Vector2 offset)
		{
//			return;

			m_device.device.RenderState.ZBufferFunction		= Compare.Less;
			m_device.device.RenderState.SourceBlend			= Blend.SourceAlpha;
			m_device.device.RenderState.DestinationBlend	= Blend.One;

			float	size	= m_loop_image.ImageScale;
			if(size < 0.5)		size	= 0.5f;
//			else if(ImageSize > 2)	ImageSize	= 2;
			else if(size > 1)	size	= 1;

			int	color		= Color.FromArgb(100, 255, 255, 255).ToArgb();
			m_device.sprites.BeginDrawSprites(m_icons.texture, offset, m_loop_image.ImageScale, new Vector2(size, size));
			foreach(GvoWorldInfo.Info i in m_spots){
				m_device.sprites.AddDrawSprites(new Vector3(i.position.X, i.position.Y, 0.3f),
										m_icons.GetIcon(icons.icon_index.spot_0), color);
			}
			m_device.sprites.EndDrawSprites();

			m_device.device.RenderState.ZBufferFunction		= Compare.LessEqual;
			m_device.device.RenderState.SourceBlend			= Blend.SourceAlpha;
			m_device.device.RenderState.DestinationBlend	= Blend.InvSourceAlpha;
		}

		/*-------------------------------------------------------------------------
		 スポット表示
		 自分の国のみ
		---------------------------------------------------------------------------*/
		private void draw_spot_mycountry(Vector2 offset)
		{
			m_device.device.RenderState.ZBufferFunction		= Compare.Less;
			m_device.device.RenderState.SourceBlend			= Blend.SourceAlpha;
			m_device.device.RenderState.DestinationBlend	= Blend.One;

			float	size	= m_loop_image.ImageScale;
			if(size < 0.5)		size	= 0.5f;
//			else if(ImageSize > 2)	ImageSize	= 2;
			else if(size > 1)	size	= 1;

			int	color		= Color.FromArgb(100, 255, 255, 255).ToArgb();
			m_device.sprites.BeginDrawSprites(m_icons.texture, offset, m_loop_image.ImageScale, new Vector2(size, size));
			foreach(GvoWorldInfo.Info i in m_spots){
				if(i.Country != m_world.MyCountry)	continue;

				m_device.sprites.AddDrawSprites(new Vector3(i.position.X, i.position.Y, 0.3f),
										m_icons.GetIcon(icons.icon_index.spot_0), color);
			}
			m_device.sprites.EndDrawSprites();

			m_device.device.RenderState.ZBufferFunction		= Compare.LessEqual;
			m_device.device.RenderState.SourceBlend			= Blend.SourceAlpha;
			m_device.device.RenderState.DestinationBlend	= Blend.InvSourceAlpha;
		}

		/*-------------------------------------------------------------------------
		 言語用スポット表示
		---------------------------------------------------------------------------*/
		private void draw_spot_for_lang(Vector2 offset)
		{
			m_device.device.RenderState.ZBufferFunction		= Compare.Less;
			m_device.device.RenderState.SourceBlend			= Blend.SourceAlpha;
			m_device.device.RenderState.DestinationBlend	= Blend.One;

			float	size	= m_loop_image.ImageScale;
			if(size < 0.5)		size	= 0.5f;
//			else if(ImageSize > 2)	ImageSize	= 2;
			else if(size > 1)	size	= 1;

			int	color		= Color.FromArgb(100, 255, 255, 255).ToArgb();
			m_device.sprites.BeginDrawSprites(m_icons.texture, offset, m_loop_image.ImageScale, new Vector2(size, size));
			foreach(GvoWorldInfo.Info i in m_spots){
				icons.icon_index	index	= icons.icon_index.spot_2;
				if(i.LearnPerson(m_find_string) != null){
					// 覚えられるところ
					index	= icons.icon_index.spot_0;
				}

				m_device.sprites.AddDrawSprites(new Vector3(i.position.X, i.position.Y, 0.3f),
										m_icons.GetIcon(index), color);
			}
			m_device.sprites.EndDrawSprites();

			m_device.device.RenderState.ZBufferFunction		= Compare.LessEqual;
			m_device.device.RenderState.SourceBlend			= Blend.SourceAlpha;
			m_device.device.RenderState.DestinationBlend	= Blend.InvSourceAlpha;
		}

		/*-------------------------------------------------------------------------
		 言語用スポット表示
		 覚えられる街に人アイコン表示
		---------------------------------------------------------------------------*/
		private void spot_learn_lang(Vector2 offset)
		{
			float	size	= m_loop_image.ImageScale * 1.5f;
			if(size < 0.5)		size	= 0.5f;
			else if(size > 1 * 1.5f)	size	= 1 * 1.5f;

			m_device.sprites.BeginDrawSprites(m_icons.texture, offset, m_loop_image.ImageScale, new Vector2(size, size));
			foreach(GvoWorldInfo.Info i in m_spots){
				if(i.LearnPerson(m_find_string) == null)	continue;

				m_device.sprites.AddDrawSprites(new Vector3(i.position.X, i.position.Y, 0.3f),
										m_icons.GetIcon(icons.icon_index.spot_tab_3));
			}
			m_device.sprites.EndDrawSprites();
		}

		/*-------------------------------------------------------------------------
		 スポット表示
		 アイコンたち
		---------------------------------------------------------------------------*/
		private void spot_icons(Vector2 offset)
		{
			float	size	= m_loop_image.ImageScale * 1.2f;
			if(size < 0.5)				size	= 0.5f;
			else if(size > 1 * 1.2f)	size	= 1 * 1.2f;
	
			m_device.sprites.BeginDrawSprites(m_icons.texture, offset, m_loop_image.ImageScale, new Vector2(size, size));
			foreach(GvoWorldInfo.Info i in m_spots){
				m_device.sprites.AddDrawSprites(new Vector3(i.position.X, i.position.Y, 0.3f),
										m_icons.GetIcon(icons.icon_index.spot_tab2_0 + (int)(m_spot_type - type.icons_0)) );
			}
			m_device.sprites.EndDrawSprites();
		}

		/*-------------------------------------------------------------------------
		 スポット表示
		 街
		---------------------------------------------------------------------------*/
		private void spot_cities(Vector2 offset)
		{
			float	size	= m_loop_image.ImageScale * 1.2f;
			if(size < 0.5*1.2f)			size	= 0.5f*1.2f;
			else if(size > 1 * 1.2f)	size	= 1 * 1.2f;
	
			m_device.sprites.BeginDrawSprites(m_icons.texture, offset, m_loop_image.ImageScale, new Vector2(size, size));
			foreach(GvoWorldInfo.Info i in m_spots){
				icons.icon_index	index;
				switch(i.InfoType){
				case GvoWorldInfo.InfoType.CITY:			// 街
					// 街の種類によりアイコンを変える
					index	= icons.icon_index.city_icon_0;
					index	+= (int)i.CityType;
					break;
				case GvoWorldInfo.InfoType.SHORE:		// 上陸地点
					index	= icons.icon_index.city_icon_4;
					break;

				case GvoWorldInfo.InfoType.SHORE2:		// 上陸地点 奥地
				case GvoWorldInfo.InfoType.PF:			// プライベートファーム
					index	= icons.icon_index.city_icon_5;
					break;
				case GvoWorldInfo.InfoType.OUTSIDE_CITY:	// 郊外
					index	= icons.icon_index.city_icon_6;
					break;

				case GvoWorldInfo.InfoType.SEA:			// 海域
				default:
					continue;
				}

				m_device.sprites.AddDrawSprites(new Vector3(i.position.X, i.position.Y, 0.3f),
										m_icons.GetIcon(index));
			}
			m_device.sprites.EndDrawSprites();
		}

		/*-------------------------------------------------------------------------
		 スポット表示
		 タブ
		---------------------------------------------------------------------------*/
		private void spot_tab(Vector2 offset)
		{
			float	size	= m_loop_image.ImageScale * 1.2f;
			if(size < 0.5)				size	= 0.5f;
			else if(size > 1 * 1.2f)	size	= 1 * 1.2f;
	
			m_device.sprites.BeginDrawSprites(m_icons.texture, offset, m_loop_image.ImageScale, new Vector2(size, size));
			foreach(GvoWorldInfo.Info i in m_spots){
				m_device.sprites.AddDrawSprites(new Vector3(i.position.X, i.position.Y, 0.3f),
										m_icons.GetIcon(icons.icon_index.spot_tab_0 + (int)(int)(m_spot_type - type.tab_0)) );
			}
			m_device.sprites.EndDrawSprites();
		}

		/*-------------------------------------------------------------------------
		 ツールチップ用文字列を得る
		---------------------------------------------------------------------------*/
		public string GetToolTipString(Point pos)
		{
			switch(m_spot_type){
			case type.country_flags:		// 国旗
			case type.icons_0:				// 看板娘
			case type.icons_1:				// 書庫
			case type.icons_2:				// 翻訳家
			case type.icons_3:				// 豪商
			case type.tab_0:				// 交易
			case type.tab_1:				// 道具
			case type.tab_2:				// 工房
			case type.tab_3:				// 人物
			case type.tab_4:				// 船
			case type.tab_5:				// 大砲
			case type.tab_6:				// 板
			case type.tab_7:				// 帆
			case type.tab_8:				// 像
			case type.tab_9:				// 行商人
			case type.city_name:			// 街名
				break;

			case type.has_item:				// 指定したアイテムがある
				foreach(GvoWorldInfo.Info i in m_spots){
					if(i.HitTest(pos)){
						GvoWorldInfo.Info.Group.Data	d	= i.HasItem(m_find_string);
						if(d != null)	return String.Format("{0}[{1}]", d.Name, d.Price);
					}
				}
				break;
			case type.language:				// 言語
				foreach(GvoWorldInfo.Info i in m_spots){
					if(i.HitTest(pos)){
						return i.LearnPerson(m_find_string);
					}
				}
				break;
			case type.cultural_sphere:		// 文化圏
				foreach(GvoWorldInfo.Info i in m_spots){
					if(i.HitTest(pos)){
						return i.CulturalSphereStr;
					}
				}
				break;
			}
			return null;
		}

		/*-------------------------------------------------------------------------
		 スポットタイプを文字列で得る
		---------------------------------------------------------------------------*/
		static public string GetTypeString(type _type)
		{
			switch(_type){
			case type.none:						return "スポットなし";
			case type.country_flags:			return "国旗";
			case type.icons_0:					return "看板娘";
			case type.icons_1:					return "書庫";
			case type.icons_2:					return "翻訳家";
			case type.icons_3:					return "豪商";
			case type.has_item:					return "アイテム等";
			case type.language:					return "使用言語";
			case type.tab_0:					return "交易所";
			case type.tab_1:					return "道具屋";
			case type.tab_2:					return "工房";
			case type.tab_3:					return "人物";
			case type.tab_4:					return "船";
			case type.tab_5:					return "大砲";
			case type.tab_6:					return "板";
			case type.tab_7:					return "帆";
			case type.tab_8:					return "像";
			case type.tab_9:					return "行商人";
			case type.tab_10:					return "メモ";
			case type.city_name:				return "街名";
			case type.cultural_sphere:			return "文化圏";
			}
			return "不明なタイプ";
		}

		/*-------------------------------------------------------------------------
		 スポットタイプからEx文字列を得る
		 スポット一覧用
		---------------------------------------------------------------------------*/
		static public string GetExColumnString(type _type)
		{
			switch(_type){
			case type.none:						return "";
			case type.country_flags:			return "状態";
			case type.icons_0:					return "";
			case type.icons_1:					return "";
			case type.icons_2:					return "";
			case type.icons_3:					return "";
			case type.has_item:					return "価格等";
			case type.language:					return "言語習得";
			case type.tab_0:					return "";
			case type.tab_1:					return "";
			case type.tab_2:					return "";
			case type.tab_3:					return "";
			case type.tab_4:					return "";
			case type.tab_5:					return "";
			case type.tab_6:					return "";
			case type.tab_7:					return "";
			case type.tab_8:					return "";
			case type.tab_9:					return "";
			case type.tab_10:					return "";
			case type.city_name:				return "";
			case type.cultural_sphere:			return "";
			}
			return "";
		}

	}
}
