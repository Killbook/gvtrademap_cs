/*-------------------------------------------------------------------------

 大航海時代Online画面キャプチャ
 船の角度は2度間隔で確定
 360段階の分解能から180段階の値を得る
 0度は実機の仕様により向けない

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 define
---------------------------------------------------------------------------*/
// コンパス解析デバッグ
//#define	DEBUG_COMPASS

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using directx;

using win32;
using Utility;
using gvo_base;
using System.Diagnostics;


/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public class gvo_capture : gvo_capture_base
	{
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		class capture_image : screen_capture
		{
			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public capture_image(int size_x, int size_y)
				: base(size_x, size_y)
			{
			}

			/*-------------------------------------------------------------------------
			 キャプチャした内容をテクスチャにする
			 デバッグ用
			---------------------------------------------------------------------------*/
			public Texture CreateTexture(Device device)
			{
				if(base.image == null)	return null;

				try{
					using(Texture tex	= new Texture(device, base.size.Width, base.size.Height,
														1, Usage.Dynamic, Format.X8R8G8B8, Pool.SystemMemory)){
						UInt32[,] buf	= (UInt32[,])tex.LockRectangle(typeof(UInt32), 0, LockFlags.Discard, base.size.Height, base.size.Width);

						// テクスチャ内容の生成
						int		index	= 0;
						for(int y=0; y<base.size.Height; y++){
							for(int x=0; x<base.size.Width; x++){
								buf[y, x]	= (UInt32)((image[index + x * 3 + 2] << 16)
													| (image[index + x * 3 + 1] << 8)
													| (image[index + x * 3 + 0] << 0)
													| (255 << 24));
							}
							index	+= stride;
						}
						tex.UnlockRectangle(0);

						// 使用できるメモリに転送
						return d3d_utility.CreateTextureFromTexture(device, tex);
					}
				}catch{
					// 失敗
					return null;
				}
			}
		}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		private gvt_lib				m_lib;

		// キャプチャ画像チェック用
		private Texture				m_debug_texture;
		private Texture				m_debug_texture2;

#if DEBUG_COMPASS
		public float				m_factor;
		public float				m_angle_x;
		public float				m_l;
#else
		private float				m_factor;
		private float				m_angle_x;
		private float				m_l;
#endif

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public gvo_capture(gvt_lib lib)
			: base()
		{
			m_lib				= lib;

			m_debug_texture		= null;
			m_debug_texture2	= null;

			// テーブル生成用調整値
			m_factor			= 36f;
			m_angle_x			= 31.5f;
			m_l					= 54f;

			// コンパスからの角度算出用テーブルを作成する
//			create_compass_tbl();
			// 調整テーブルの作成
//			create_ajust_compass_tbl();
		}

		/*-------------------------------------------------------------------------
		 screen_captureを作成する
		 screen_captureを継承したものを使用する場合オーバーライドすること
		 コンストラクタ内で呼ばれるので注意
		---------------------------------------------------------------------------*/
		protected override screen_capture CreateScreenCapture(int size_x, int size_y)
		{
			return new capture_image(size_x, size_y);
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public override void Dispose()
		{
			release_debug_textures();
		}
	
		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		private void release_debug_textures()
		{
			if(m_debug_texture != null)		m_debug_texture.Dispose();
			if(m_debug_texture2 != null)	m_debug_texture2.Dispose();
			m_debug_texture		= null;
			m_debug_texture2	= null;
		}
	
		/*-------------------------------------------------------------------------
		 画面をキャプチャし、日付や座標、進行方向を得る
		---------------------------------------------------------------------------*/
		public override bool CaptureAll()
		{
			if(!base.CaptureAll()){
				// キャプチャ失敗
				return false;
			}

			if(m_lib.setting.draw_capture_info){
	// 解析する場所を書き込む
	//			debug_compass();
				// キャプチャしたイメージをテクスチャにする
				// コンパスイメージをテクスチャにする
				release_debug_textures();
				m_debug_texture		= ((capture_image)capture2).CreateTexture(m_lib.device.device);
				// 日付と測量位置イメージをテクスチャにする
				m_debug_texture2	= ((capture_image)capture1).CreateTexture(m_lib.device.device);
			}
			return true;
		}

#if DEBUG_COMPASS
		/*-------------------------------------------------------------------------
		 debug
		 検索する場所を赤にする
		 テクスチャ表示時のテスト用
		---------------------------------------------------------------------------*/
		private void debug_compass()
		{
			byte[]	image	= m_capture2.image;
			int		stride	= m_capture2.stride;

			for(int i=0; i<COMPASS_DIV; i++){
				int		index;

				index	= (stride * (m_compass_pos[i].Y)) + (m_compass_pos[i].X * 3);
				image[index + 0]	= 0;
				image[index + 1]	= 0;
				image[index + 2]	= 255;
			}
		}
#endif

		/*-------------------------------------------------------------------------
		 コンパスからの角度算出用テーブルを作成する
		 調整用
		---------------------------------------------------------------------------*/
		private void create_compass_tbl()
		{
			m_compass_pos	= new Point[COMPASS_DIV];
	
			Matrix	mat		= Matrix.PerspectiveFovRH(Useful.ToRadian(60), 1f, 0.1f, 1000f);
			Matrix	mat2	= Matrix.RotationX(Useful.ToRadian(m_angle_x));
			Matrix	mat3	= Matrix.Translation(0, 0, -100f);
			mat		= mat2 * mat3 * mat;
			float	tmp		= m_factor;
			float	factor;
			{
				Vector3	vec		= new Vector3(tmp, 0, 0);
				Vector3	vec2	= Vector3.TransformCoordinate(vec, mat);
				factor	= 1f / vec2.X;
			}

			for(int i=0; i<COMPASS_DIV; i++){
				float	angle		= Useful.ToRadian(-90 + ((360f/COMPASS_DIV) * i));
				float	x			= (float)Math.Cos(angle);
				float	y			= (float)Math.Sin(angle);

				float	l		= factor * m_l;
				Vector3	vec		= new Vector3(x*tmp, y*tmp, 0);
				Vector3	vec2	= Vector3.TransformCoordinate(vec, mat);

				vec2.X	*= l;
				vec2.Y	*= l;

				int		offset_x	= 52+4;
				int		offset_y	= 35+4;
				int		xx			= offset_x + (int)vec2.X;
				int		yy			= offset_y + (int)vec2.Y;

				if(xx < 0)		xx	= 0;
				if(xx >= 128)	xx	= 127;
				if(yy < 0)		yy	= 0;
				if(yy >= 128)	yy	= 127;
				m_compass_pos[i]	= new Point(xx, yy);
				Debug.WriteLine(String.Format("{0}, {1}		// {2}",
												m_compass_pos[i].X, m_compass_pos[i].Y, i));
			}
		}

		/*-------------------------------------------------------------------------
		 キャプチャ詳細の表示
		---------------------------------------------------------------------------*/
		public void DrawCapturedTexture()
		{
			if(!m_lib.setting.draw_capture_info)		return;
	
			Vector3		spos	= new Vector3(m_lib.device.client_size.X - 128 -4, 4, 0.002f);
			unchecked{
				m_lib.device.DrawFillRect(spos, new Vector2(64+16, (12*4)+2+2), (int)0xC0000000);
//				m_lib.Device.DrawFillRect(spos, new Vector2(64+16, (12*(3+3))+2+2), (int)0xC0000000);
			}
			spos.X	+= 2;
			spos.Y	+= 2;
			m_lib.device.systemfont.locate	= spos;
			m_lib.device.systemfont.Puts(String.Format("index1={0}\nindex2={1}\n",
											m_1st_com_index, m_com_index), Color.White);
			m_lib.device.systemfont.Puts(String.Format("index3={0}\nquadrant={1}\n",
											m_com_index2, m_an_index), Color.White);
//			m_lib.Device.systemfont.Puts(String.Format("index2={0}\nquadrant={1}\nangle2={2}\n",
//											m_com_index2, m_an_index2, m_angle2), Color.White);


			Vector3		pos		= new Vector3(	m_lib.device.client_size.X - 4,
												64,
												0.001f);

			// 測量と航海日数
			draw_debug_texture(m_debug_texture2, pos, 1f);
			// コンパス
			pos.Y	+= 8f + d3d_utility.GetTextureSize(m_debug_texture2).Y;
			draw_debug_texture(m_debug_texture, pos, 1f);
		}
		private void draw_debug_texture(Texture tex, Vector3 pos, float scale)
		{
			if(tex == null)		return;

			Vector2		size	= d3d_utility.GetTextureSize(tex);
			pos.X	-= size.X;
			m_lib.device.DrawTexture(tex, pos, size * scale);
		}
	}
}
