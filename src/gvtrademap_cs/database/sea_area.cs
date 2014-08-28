/*-------------------------------------------------------------------------

 海域変動システムのエリア表示
 マスクからエリアを作成する機能付き

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;

using directx;
using Utility;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public class sea_area : IDisposable
	{
		/*-------------------------------------------------------------------------
		 海域群1つ
		---------------------------------------------------------------------------*/
		public class sea_area_once : IDisposable
		{
			/*-------------------------------------------------------------------------
			 海域1つ
			---------------------------------------------------------------------------*/
			class data : TextureUnit
			{
				private string				m_name;
				private Vector2				m_pos;
				private Vector2				m_size;
				private bool				m_is_create_from_mask;

				// 矩形判定用
				private hittest				m_hittest;
	
				/*-------------------------------------------------------------------------
				 
				---------------------------------------------------------------------------*/
				public string name{						get{	return m_name;		}}
				public Vector2 position{				get{	return m_pos;		}}
				public Vector2 size{					get{	return m_size;		}}
				public bool is_create_from_mask{		get{	return m_is_create_from_mask;	}}
	
				/*-------------------------------------------------------------------------
				 
				---------------------------------------------------------------------------*/
				public data(string name, Vector2 pos, Vector2 size, bool is_create_from_mask) : base()
				{
					m_name					= name;
					m_pos					= pos;
					m_size					= size;
					m_is_create_from_mask	= is_create_from_mask;

					m_hittest				= new hittest(new Rectangle(0, 0, (int)size.X, (int)size.Y), transform.ToPoint(pos));
				}

				/*-------------------------------------------------------------------------
				 マスクから作成
				---------------------------------------------------------------------------*/
				public void CreateFromMask(d3d_device device, ref byte[] image, Size size, int stride)
				{
					// マスクから作る必要のない海域は矩形でよい
					if(!m_is_create_from_mask)	return;

					Rectangle rect	= new Rectangle((int)m_pos.X, (int)m_pos.Y, (int)m_size.X & ~1, (int)m_size.Y & ~1);
					base.CreateFromMask(device, ref image, size, stride, rect);
				}

				/*-------------------------------------------------------------------------
				 描画
				---------------------------------------------------------------------------*/
				public void Draw(Vector2 offset, LoopXImage image, int color)
				{
					if(IsCreate){
						// マスクから作ったテクスチャあり
						Draw(new Vector3(offset.X, offset.Y, 0.79f), image.ImageScale, color);
					}else{
						// 単純な矩形
						// マスクを作らない分テクスチャ容量を削る
						Vector2 pos0	= m_pos;
						Vector2 pos		= image.GlobalPos2LocalPos(pos0, offset);
						Vector2	size	= m_size;
						size.X			-= 1;
						size.Y			-= 1;
						size			*= image.ImageScale;

						// 画面外カリング
						if(pos.X + size.X < 0)	return;
						if(pos.Y + size.Y < 0)	return;
						Vector2 csize	= image.Device.client_size;
						if(pos.X >= csize.X)	return;
						if(pos.Y >= csize.Y)	return;
			
						image.Device.DrawFillRect(new Vector3(pos.X, pos.Y, 0.79f), size, color);
					}
				}

				/*-------------------------------------------------------------------------
				 ヒットテスト
				---------------------------------------------------------------------------*/
				public bool HitTest(Point pos)
				{
					return m_hittest.HitTest(pos);
				}
			}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public enum sea_type{
				normal,		// 通常
				safty,		// 安全化
				lawless,	// 無法化
			};

			private List<data>				m_list;
			private string					m_name;
			private sea_type				m_type;

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public string name{			get{	return m_name;		}}
			public sea_type type{		get{	return m_type;		}
										set{	m_type	= value;	}}
			public string type_str{		get{	return ToString(m_type);	}}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public sea_area_once(string name)
			{
				m_name			= name;
				m_type			= sea_type.normal;
				m_list			= new List<data>();
			}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public void Dispose()
			{
				if(m_list == null)	return;

				foreach(data d in m_list){
					d.Dispose();
				}
				m_list.Clear();
			}
	
			/*-------------------------------------------------------------------------
			 追加
			---------------------------------------------------------------------------*/
			public void Add(string name, Vector2 pos, Vector2 size, bool is_create_from_mask)
			{
				m_list.Add(new data(name, pos, size, is_create_from_mask));
			}

			/*-------------------------------------------------------------------------
			 マスクから作成
			---------------------------------------------------------------------------*/
			public void CreateFromMask(d3d_device device, ref byte[] image, Size size, int stride)
			{
				foreach(data d in m_list){
					d.CreateFromMask(device, ref image, size, stride);
				}
			}

			/*-------------------------------------------------------------------------
			 ツールチップ用の文字列を得る
			---------------------------------------------------------------------------*/
			public string GetToolTipString()
			{
				string	str	= "";
				foreach(data d in m_list){
					if(d.name != ""){
						if(str != "")	str	+= "\n";
						str	+= d.name;
					}
				}
				return str;
			}

			/*-------------------------------------------------------------------------
			 描画
			---------------------------------------------------------------------------*/
			public void Draw(Vector2 offset, LoopXImage image, int alpha, int alpha2, color_type type)
			{
				if(m_type == sea_type.normal)	return;		// 通常状態

				int color;
				// 地図の種類によって色を若干変える
				if(m_type == sea_type.safty){
					// 安全
					if(type == color_type.type1)	color	= Color.FromArgb(alpha, 0, 128, 220).ToArgb();
					else							color	= Color.FromArgb(alpha, 0,  64, 200).ToArgb();
				}else{
					// 無法
					if(type == color_type.type1)	color	= Color.FromArgb(alpha2, 200, 0, 0).ToArgb();
					else							color	= Color.FromArgb(alpha2, 200, 0, 0).ToArgb();
				}
				foreach(data d in m_list){
					d.Draw(offset, image, color);
				}
			}

			/*-------------------------------------------------------------------------
			 ヒットテスト
			 地図座標で渡すこと
			---------------------------------------------------------------------------*/
			public bool HitTest(Point pos)
			{
				foreach(data d in m_list){
					if(d.HitTest(pos))	return true;
				}
				return false;
			}

			/*-------------------------------------------------------------------------
			 sea_typeを文字列で得る
			---------------------------------------------------------------------------*/
			public static string ToString(sea_type t)
			{
				switch(t){
				case sea_type.normal:		return "危険海域";
				case sea_type.safty:		return "安全海域";
				case sea_type.lawless:		return "無法海域";
				}
				return "unknown";
			}
		}

		public enum name{
			carib,
			west_africa,
			south_atlantic,
			east_africa,
			red_sea,
			indian,
			east_latin_america,
			southeast_asia,
			south_pacific,
			west_latin_america,
			east_asia,
			max
		};
		public enum color_type{
			type1,
			type2,
		};

		private const int					ALPHA_MIN		= 35;
		private const int					ALPHA_MAX		= 60;
		private const int					ALPHA_CENTER	= (ALPHA_MAX-ALPHA_MIN)/2;
		private const int					ANGLE_STEP		= 2;
		private const int					ANGLE_STEP2		= 2*2;

		private gvt_lib						m_lib;
		private float						m_angle;
		private float						m_angle2;
		private int							m_alpha;
		private int							m_alpha2;
		private List<sea_area_once>			m_groups;
		private color_type					m_color_type;

		private int							m_progress_max;
		private int							m_progress_current;
		private string						m_progress_info_str;
		private bool						m_is_loaded_mask;		// マスクを読み込み済のときtrue
	
		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public List<sea_area_once> groups{		get{	return m_groups;			}}
		public color_type color{				get{	return m_color_type;		}
												set{	m_color_type	= value;	}}

		public int progress_max{				get{	return m_progress_max;		}}
		public int progress_current{			get{	return m_progress_current;	}}
		public string progress_info_str{		get{	return m_progress_info_str;		}}

		public bool IsLoadedMask{				get{	return m_is_loaded_mask;	}}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public sea_area(gvt_lib lib, string fname)
		{
			m_lib			= lib;

			m_angle			= 0;
			m_angle2		= 0;
			m_color_type	= color_type.type1;
			m_groups		= new List<sea_area_once>();

			m_progress_max		= 0;
			m_progress_current	= 0;
			m_progress_info_str		= "";
			m_is_loaded_mask	= false;

			sea_area_once	once;

			// カリブ海
			once		= new sea_area_once("カリブ海");
			once.Add("サンフアン沖", new Vector2(3970,1049), new Vector2(73,149), true);
			once.Add("アンティル諸島沖", new Vector2(3819,1049), new Vector2(150,149), true);
			once.Add("中央大西洋", new Vector2(4044,	1199), new Vector2(373,148), false);
			once.Add("西カリブ海", new Vector2(3670,1049), new Vector2(148,136), true);
			once.Add("", new Vector2(3753,1185), new Vector2(65,124), true);

			once.Add("コッド岬沖", new Vector2(3819,750), new Vector2(224,148), true);
			once.Add("バミューダ諸島沖", new Vector2(3717,899), new Vector2(326,149), true);
			once.Add("テラ・ノヴァ海", new Vector2(3790-1,600), new Vector2(254,149), true);
			m_groups.Add(once);
	
			// アフリカ西岸
			once		= new sea_area_once("アフリカ西岸");
			once.Add("穀物海岸沖", new Vector2(4418,1199), new Vector2(224,299), true);
			once.Add("黄金海岸沖", new Vector2(4643,1325), new Vector2(149,173), true);
			once.Add("ギニア湾", new Vector2(1,1322), new Vector2(164,176), true);
			m_groups.Add(once);

			// 南大西洋
			once		= new sea_area_once("南大西洋");
			once.Add("ナミビア沖", new Vector2(1,1499), new Vector2(180,149), true);
			once.Add("喜望峰沖", new Vector2(1,1649), new Vector2(297,223), true);
			once.Add("ケープ海盆", new Vector2(1,1873), new Vector2(297,224), false);
			once.Add("南大西洋", new Vector2(4494,1499), new Vector2(298,599), false);
			m_groups.Add(once);

			// アフリカ東岸
			once		= new sea_area_once("アフリカ東岸");
			once.Add("アガラス岬沖", new Vector2(299,1683), new Vector2(149,189), true);
			once.Add("アガラス海盆", new Vector2(299,1873), new Vector2(299,225), false);
			once.Add("モザンビーク海峡", new Vector2(449,1649), new Vector2(149,223), true);
			once.Add("マダガスカル沖", new Vector2(458,1500), new Vector2(440,149), true);
			once.Add("南西インド洋", new Vector2(599,1649), new Vector2(299,449), true);
			m_groups.Add(once);

			// 紅海
			once		= new sea_area_once("紅海");
			once.Add("ザンジバル沖", new Vector2(513,1348), new Vector2(385,151), true);
			once.Add("アラビア海", new Vector2(600,1199), new Vector2(298,149), true);
			once.Add("紅海", new Vector2(457,1076), new Vector2(142,202), true);
			once.Add("ペルシャ湾", new Vector2(638,1086), new Vector2(260,112), true);
			m_groups.Add(once);
	
			// インド洋
			once		= new sea_area_once("インド洋");
			once.Add("インド西岸沖", new Vector2(899,1125), new Vector2(109,149), true);
			once.Add("インド南岸沖", new Vector2(899,1274), new Vector2(224,149), true);
			once.Add("ベンガル湾", new Vector2(1067,1134), new Vector2(272,139), true);
			once.Add("中部インド洋", new Vector2(899,1424), new Vector2(449,224), true);
			once.Add("南インド洋", new Vector2(899,1649), new Vector2(301,449), false);
			once.Add("南東インド洋", new Vector2(1201,1649), new Vector2(296,449), false);
			m_groups.Add(once);

			// 中南米東岸
			once		= new sea_area_once("中南米東岸");
			once.Add("南カリブ海", new Vector2(3819,1199), new Vector2(224,148), true);
			once.Add("メキシコ湾", new Vector2(3497,1055), new Vector2(173,132), true);
			once.Add("サンロケ岬沖", new Vector2(4194,1348), new Vector2(223,150), true);
			once.Add("アマゾン川流域", new Vector2(3900,1348), new Vector2(293,117), true);
			once.Add("南西大西洋", new Vector2(4194,1499), new Vector2(299,373), true);
			once.Add("ブエノスアイレス沖", new Vector2(3946,1678), new Vector2(248,195), true);
			once.Add("アルゼンチン海盆", new Vector2(3894,1873), new Vector2(374,225), true);
			once.Add("ジョージア海盆", new Vector2(4269,1873), new Vector2(224,225), false);
			m_groups.Add(once);

			// 東南アジア
			once		= new sea_area_once("東南アジア");
			once.Add("アンダマン海", new Vector2(1124,1274), new Vector2(224,149), true);
			once.Add("ジャワ海", new Vector2(1349,1348), new Vector2(224,150), true);
			once.Add("ジャワ島南方沖", new Vector2(1349,1500), new Vector2(224,73), false);
			once.Add("", new Vector2(1349,1572), new Vector2(148,76), false);
			once.Add("シャム湾", new Vector2(1349,1199), new Vector2(149,148), true);
			once.Add("バンダ海", new Vector2(1574,1423), new Vector2(298,150), true);
			once.Add("セレベス海", new Vector2(1499,1199), new Vector2(223,148), true);
			once.Add("", new Vector2(1574,1347), new Vector2(148,76), true);
			once.Add("西カロリン海盆", new Vector2(1723,1199), new Vector2(149,223), true);
			m_groups.Add(once);

			// 南太平洋
			once		= new sea_area_once("南太平洋");
			once.Add("チリ海盆", new Vector2(3595,1797), new Vector2(299,301), true);
			once.Add("西オーストラリア海盆", new Vector2(1498,1573), new Vector2(224,224), true);
			once.Add("パース海盆", new Vector2(1498,1798), new Vector2(224,300), true);
			once.Add("南オーストラリア海盆", new Vector2(1723,1893), new Vector2(299,205), true);
			once.Add("アラフラ海", new Vector2(1723,1573), new Vector2(373,211), true);
			once.Add("", new Vector2(1873,1497), new Vector2(223,76), true);
			once.Add("東カロリン海盆", new Vector2(1873,1199), new Vector2(223,297), true);
			once.Add("メラネシア海盆", new Vector2(2097,1199), new Vector2(223,298), false);
			once.Add("コーラル海", new Vector2(2097,1498), new Vector2(299,299), true);
			once.Add("タスマン海", new Vector2(2023,1798), new Vector2(448,300), true);
			once.Add("中央太平洋海盆西", new Vector2(2321,1199), new Vector2(373,298), false);
			once.Add("サモア海盆", new Vector2(2397,1498), new Vector2(224,299), true);
			once.Add("南太平洋海盆西", new Vector2(2472,1798), new Vector2(298,300), false);
			once.Add("中央太平洋海盆", new Vector2(2695,1199), new Vector2(449,298), false);
			once.Add("南太平洋海盆北", new Vector2(2622,1498), new Vector2(523,299), false);
			once.Add("南太平洋海盆", new Vector2(2771,1798), new Vector2(374,300), false);
			once.Add("南太平洋海盆東", new Vector2(3146,1498), new Vector2(448,600), false);
            once.Add("ハワイ沖", new Vector2(2321,751), new Vector2(449,447), true);
            m_groups.Add(once);

			// 中南米西岸
			once		= new sea_area_once("中南米西岸");
			once.Add("ペルー海盆", new Vector2(3595,1498), new Vector2(260,298), true);
			once.Add("グアヤキル湾", new Vector2(3595,1350), new Vector2(126,147), true);
			once.Add("パナマ湾", new Vector2(3595,1234), new Vector2(136,114), true);
			once.Add("中央太平洋海盆東", new Vector2(3145,1199), new Vector2(299,298), true);
			once.Add("ガラパゴス諸島沖", new Vector2(3445,1350), new Vector2(149,147), true);
			once.Add("テワンテペク湾", new Vector2(3445,1234), new Vector2(149,114), true);
			m_groups.Add(once);

			// 東アジア
			once		= new sea_area_once("東アジア");
			once.Add("東アジア西部", new Vector2(1393,912), new Vector2(254,286), true);
			once.Add("東アジア東部", new Vector2(1649,751), new Vector2(373,447), true);
			once.Add("北西太平洋海盆", new Vector2(2024,751), new Vector2(297,447), true);
			m_groups.Add(once);

			// 極北大西洋
			once		= new sea_area_once("極北大西洋");
			once.Add("フラム海峡",           new Vector2(4344,   0), new Vector2(224,294), true);
			once.Add("デンマーク海盆",       new Vector2(4344, 295), new Vector2(224,155), false);
			once.Add("ロフォーテン海盆",     new Vector2(4569,   0), new Vector2(223,294), true);
			once.Add("ノルウェー海盆",       new Vector2(4569, 295), new Vector2(224,155), false);
            once.Add("ノルウェー海盆2",      new Vector2(   0, 295), new Vector2( 74,155), false);
            m_groups.Add(once);

			// ヨーロッパ極北
			once		= new sea_area_once("ヨーロッパ極北");
            once.Add("西バレンツ海",         new Vector2(   1,   0), new Vector2(447,294), true);
            once.Add("北ノルウェー海",       new Vector2(  75, 295), new Vector2(300,155), true);
            once.Add("東バレンツ海",         new Vector2( 449,   0), new Vector2(304,294), true);
            once.Add("東バレンツ海2",        new Vector2( 601, 294), new Vector2(152,156), true);
            once.Add("白海",                 new Vector2( 376, 295), new Vector2(223,225), true);
            m_groups.Add(once);

            // ユーラシア北
            once = new sea_area_once("ユーラシア北");
            once.Add("西カラ海",             new Vector2( 754,   0), new Vector2(369,450), true);
			once.Add("東カラ海",             new Vector2(1124,   0), new Vector2(294,450), true);
			once.Add("ラプテフ海",           new Vector2(1419,   0), new Vector2(298,450), true);
			m_groups.Add(once);

			// ユーラシア極東
			once		= new sea_area_once("ユーラシア極東");
            once.Add("コテリヌイ島沖",       new Vector2(1718,   0), new Vector2(304,450), true);
            once.Add("東シベリア海",         new Vector2(2023,   0), new Vector2(373,450), true);
            once.Add("チュクチ海",           new Vector2(2397,   0), new Vector2(297,450), true);
			m_groups.Add(once);

			// ベーリング海
			once		= new sea_area_once("ベーリング海");
            once.Add("東ベーリング海",       new Vector2(2472, 451), new Vector2(222,298), true);
            once.Add("西ベーリング海",       new Vector2(2246, 451), new Vector2(225,298), true);
            once.Add("カムチャツカ半島沖",   new Vector2(2023, 451), new Vector2(222,298), true);
            once.Add("オホーツク海",         new Vector2(1648, 451), new Vector2(374,298), true);
            m_groups.Add(once);

			// 北米西岸
			once		= new sea_area_once("北米西岸");
			once.Add("アレキサンダー諸島沖", new Vector2(2994, 451), new Vector2(223,447), true);
			once.Add("北東太平洋",           new Vector2(2770, 750), new Vector2(223,448), false);
			once.Add("アラスカ湾",           new Vector2(2695, 451), new Vector2(298,298), true);
			once.Add("カリフォルニア湾",     new Vector2(3218, 899), new Vector2(254,299), true);
			once.Add("サンフランシスコ沖",   new Vector2(2994, 899), new Vector2(223,299), true);
			m_groups.Add(once);

			// 東カナダ
			once		= new sea_area_once("東カナダ");
			once.Add("ハドソン海峡",         new Vector2(3746, 451), new Vector2(148,148), true);
            once.Add("ハドソン湾",           new Vector2(3595, 451), new Vector2(150,298), true);
			once.Add("バフィン湾",           new Vector2(3895, 295), new Vector2(147,155), false);
			once.Add("バフィン島沖",         new Vector2(3595, 295), new Vector2(299,155), true);
			once.Add("エルズミーア島沖",     new Vector2(3595,   0), new Vector2(447,294), true);
			m_groups.Add(once);

			// 西カナダ
			once		= new sea_area_once("西カナダ");
			once.Add("北極諸島沖",           new Vector2(3218,   0), new Vector2(376,450), true);
			once.Add("ボーフォート海",       new Vector2(2994,   0), new Vector2(223,450), true);
			once.Add("バロー岬沖",           new Vector2(2695,   0), new Vector2(298,450), true);
			m_groups.Add(once);

			// 読み込み
			load(fname);
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public void Dispose()
		{
			if(m_groups == null)	return;

			foreach(sea_area_once i in m_groups){
				i.Dispose();
			}
			m_groups.Clear();
		}

		/*-------------------------------------------------------------------------
		 読み込み
		---------------------------------------------------------------------------*/
		private void load(string fname)
		{
			if(!File.Exists(fname))	return;		// ファイルが見つからない

			string line = "";
			try{
				using (StreamReader	sr	= new StreamReader(
					fname, Encoding.GetEncoding("Shift_JIS"))) {

					while((line = sr.ReadLine()) != null){
						if(line == "")			continue;

						string[]	split	= line.Split(new char[]{','});
						if(split.Length < 2)	continue;

						SetType(split[0], sea_area_once.sea_type.normal + Useful.ToInt32(split[1], 0));
					}
				}
			}catch{
				// 読み込み失敗
			}
		}

		/*-------------------------------------------------------------------------
		 書き出し
		---------------------------------------------------------------------------*/
		public void WriteSetting(string fname)
		{
			try{
				using (StreamWriter	sw	= new StreamWriter(
					fname, false, Encoding.GetEncoding("Shift_JIS"))) {

					string	str;
					foreach(sea_area_once d in m_groups){
						str	= d.name + ",";
						str	+= (int)d.type;
						sw.WriteLine(str);
					}
				}
			}catch{
				// 書き出し失敗
			}
		}
	
		/*-------------------------------------------------------------------------
		 マスク読み込み用情報の初期化
		---------------------------------------------------------------------------*/
		public void InitializeFromMaskInfo()
		{
			m_progress_max			= 0;
			m_progress_current		= 0;
			m_progress_info_str		= "読み込み中...";
		}
	
		/*-------------------------------------------------------------------------
		 マスクから作成する
		 マスクが必要ない海域はなにもしない
		---------------------------------------------------------------------------*/
		public bool CreateFromMask(string fname)
		{
			try{
				InitializeFromMaskInfo();

				// イメージの読み込み
				Bitmap	bitmap			= new Bitmap(fname);
				Size	size			= new Size(bitmap.Width, bitmap.Height);

				// ロックしてイメージ取り出し
				// R5G6B5に変換しておく
				BitmapData bmpdata	= bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
													ImageLockMode.ReadOnly,
													PixelFormat.Format16bppRgb565);

				IntPtr		ptr		= bmpdata.Scan0;
				int			length	= bmpdata.Height * bmpdata.Stride;
				int			stride	= bmpdata.Stride;
				byte[]		image	= new byte[length];
				Marshal.Copy(ptr, image, 0, length);
				bitmap.UnlockBits(bmpdata);

				// オリジナルは解放しておく
				bitmap.Dispose();
				bitmap					= null;

				m_progress_max			= m_groups.Count;
				m_progress_current		= 0;
				m_progress_info_str		= "";
				foreach(sea_area_once d in m_groups){
					m_progress_info_str	= "テクスチャ転送中... " + d.name;
					d.CreateFromMask(m_lib.device, ref image, size, stride);
					m_progress_current++;
				}
				m_progress_info_str	= "完了";

				image				= null;
				m_is_loaded_mask	= true;		// マスクを読み込み済

				System.GC.Collect();
			}catch{
				// 何かを失敗
				return false;
			}
			return true;
		}
	
		/*-------------------------------------------------------------------------
		 更新
		---------------------------------------------------------------------------*/
		public void Update()
		{
			m_alpha		= ALPHA_MIN + ALPHA_CENTER + (int)((float)Math.Sin(Useful.ToRadian(m_angle)) * ALPHA_CENTER);
			m_angle		+= ANGLE_STEP;
			if(m_angle >= 360)	m_angle		-= 360;

			m_alpha2	= ALPHA_MIN + ALPHA_CENTER + (int)((float)Math.Sin(Useful.ToRadian(m_angle2)) * ALPHA_CENTER);
			m_angle2	+= ANGLE_STEP2;
			if(m_angle2 >= 360)	m_angle2	-= 360;

			// アニメーションやめ
			// 可変フレームレートになったため
			m_angle		= 30;
			m_angle2	= 30;
		}

		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		public void Draw()
		{
			m_lib.device.device.RenderState.ZBufferEnable	= false;
			m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(draw_proc), 0);
			m_lib.device.device.RenderState.ZBufferEnable	= true;
		}

		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		private void draw_proc(Vector2 offset, LoopXImage image)
		{
			foreach(sea_area_once d in m_groups){
// debug
//				d.Type	= sea_area_once.sea_type.lawless;
				d.Draw(offset, image, m_alpha, m_alpha2, m_color_type);
			}
		}

		/*-------------------------------------------------------------------------
		 状態を設定する
		---------------------------------------------------------------------------*/
		public bool SetType(string name, sea_area.sea_area_once.sea_type type)
		{
			sea_area.sea_area_once d	= find(name);
			if(d == null)		return false;
			if(d.name == null)	return false;

			if((int)type < 0)									type	= sea_area_once.sea_type.normal;
			if((int)type > (int)sea_area_once.sea_type.lawless)	type	= sea_area_once.sea_type.normal;
	
			// 以前と違う場合地図の合成をリクエストする
			if(d.type != type){
				d.type	= type;
				m_lib.setting.req_update_map.Request();
			}
			return true;
		}

		/*-------------------------------------------------------------------------
		 リセット
		 全ての海域群を通常状態にする
		---------------------------------------------------------------------------*/
		public void ResetSeaType()
		{
			foreach(sea_area.sea_area_once d in m_groups){
				d.type	= sea_area_once.sea_type.normal;
			}
		}
	
		/*-------------------------------------------------------------------------
		 地図座標から海域群名を得る
		 得られた名前は SetType() に使用できる
		 海域群名が得られない場合はnullを返す
		---------------------------------------------------------------------------*/
		public string Find(Vector2 pos)
		{
			return Find(transform.ToPoint(pos));
		}
		public string Find(Point pos)
		{
			foreach(sea_area.sea_area_once d in m_groups){
				if(d.HitTest(pos))	return d.name;		// 見つかった海域群名を返す
			}
			return null;
		}
	
		/*-------------------------------------------------------------------------
		 検索
		---------------------------------------------------------------------------*/
		private sea_area.sea_area_once find(string name)
		{
			if(name == null)	return null;
			foreach(sea_area.sea_area_once d in m_groups){
				if(d.name == name)	return d;
			}
			// 指定された名前の海域群が存在しない
			return null;
		}

		/*-------------------------------------------------------------------------
		 ドラッグ&ドロップからの解析
		---------------------------------------------------------------------------*/
		public static List<sea_area_once_from_dd> AnalizeFromDD(string str)
		{
			List<sea_area_once_from_dd>	list	= new List<sea_area_once_from_dd>();
			string[]	lines	= str.Split(new string[]{"\r\n","\n"}, StringSplitOptions.RemoveEmptyEntries);
			foreach(string l in lines){
				sea_area_once_from_dd	data	= new sea_area_once_from_dd();
				if(data.Analize(l)){
					list.Add(data);
				}
			}
			return list;
		}

		/*-------------------------------------------------------------------------
		 ドラッグ&ドロップからの解析を反映
		 リスト全てを反映させる
		---------------------------------------------------------------------------*/
		public void UpdateFromDD(List<sea_area_once_from_dd> list, bool is_clear)
		{
			if(list == null)	return;
			if(is_clear)	ResetSeaType();			// 全てを危険海域にする

			// 逆順で反映させる
			for(int i=list.Count-1; i>=0; i--){
				SetType(list[i].name, list[i]._sea_type);
			}
		}
	}

	/*-------------------------------------------------------------------------
	 海域群1つ
	 ドラッグ&ドロップからの解析内容
	---------------------------------------------------------------------------*/
	public class sea_area_once_from_dd
	{
		private string								m_name;			// 海域群名
		private string								m_server;		// サーバ
		private DateTime							m_date;			// 期限
		private sea_area.sea_area_once.sea_type		m_sea_type;		// 状況
	
		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public string							name		{	get{	return m_name;		}}
		public string							server_str	{	get{	return m_server;	}}
		public DateTime							date		{	get{	return m_date;		}}
		public string							date_str	{	get{	return Useful.TojbbsDateTimeString(m_date);	}}
		public sea_area.sea_area_once.sea_type	_sea_type	{	get{	return m_sea_type;	}}
		public string							_sea_type_str	{	get{	return sea_area.sea_area_once.ToString(m_sea_type);	}}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public sea_area_once_from_dd()
		{
			m_name			= "";						// 海域群名
			m_server		= "";						// サーバ
			m_date			= new DateTime();			// 期限
			m_sea_type		= sea_area.sea_area_once.sea_type.normal;		// 状況
		}
	
		/*-------------------------------------------------------------------------
		 解析
		 1データ分
		 フォーマット
		   サーバ,海域名,状態,終了日時,補足
		 補足はあってもなくてもよい
		---------------------------------------------------------------------------*/
		public bool Analize(string line)
		{
			try{
				string[]	split	= line.Split(new char[]{','});
				if(split.Length < 4)	return false;		// 項目が少なすぎる

				m_server	= split[0];
				m_name		= split[1];
				if(split[2].IndexOf("安全") >= 0){
					m_sea_type	= sea_area.sea_area_once.sea_type.safty;
				}else if(split[2].IndexOf("無法") >= 0){
					m_sea_type	= sea_area.sea_area_once.sea_type.lawless;
				}else{
					m_sea_type	= sea_area.sea_area_once.sea_type.normal;
				}
				m_date		= Useful.ToDateTime(split[3]);
			}catch{
				return false;
			}
			return true;
		}
	}
}
