/*-------------------------------------------------------------------------

 x方向にループするイメージ
 イメージに合成してしまうモードに対応
 イメージはレンダーターゲットとして確保される

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using directx;
using Utility;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	class TextureUnit : IDisposable
	{
		private d3d_device			m_device;				// デバイス
		private Texture				m_texture;				// 描画するテクスチャ
		private Texture				m_texture_sysmem;		// システムメモリに確保したテクスチャ
		private Vector2				m_offset;				// 切り出しオフセット
		private Size				m_size;					// 切り出しサイズ
		private Size				m_texture_size;			// テクスチャサイズ
															// 

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public bool IsCreate{		get{	return (m_texture != null);		}}

		public Texture Texture{		get{	return m_texture;				}}
		public Vector2 Offset{		get{	return m_offset;				}}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public TextureUnit()
		{
			m_device			= null;
			m_texture			= null;
			m_texture_sysmem	= null;
		}

		/*-------------------------------------------------------------------------
		 image から rect サイズを切り取ってテクスチャを作成
		 レンダーターゲットを作成することにより、サイズを2のべき乗に調整する
		---------------------------------------------------------------------------*/
		virtual public void Create(d3d_device device, ref byte[] image, Size size, int stride, Rectangle rect)
		{
			m_device		= device;
			m_offset.X		= rect.X;
			m_offset.Y		= rect.Y;
			
			m_size.Width	= rect.Width;
			m_size.Height	= rect.Height;
			if((m_size.Width + rect.X) > size.Width){
				m_size.Width	= size.Width - rect.X;
			}
			if((m_size.Height + rect.Y) > size.Height){
				m_size.Height	= size.Height - rect.Y;
			}

			// テクスチャサイズを2のべき乗にする
			m_texture_size	= d3d_utility.TextureSizePow2(m_size);
//			m_texture_size	= m_size;
	
			// テクスチャの作成とロック
			if(m_texture_sysmem != null)	m_texture_sysmem.Dispose();
			m_texture_sysmem	= new Texture(m_device.device, m_texture_size.Width, m_texture_size.Height,
												1, Usage.Dynamic, Format.R5G6B5, Pool.SystemMemory);
			UInt16[,] buf		= (UInt16[,])m_texture_sysmem.LockRectangle(typeof(UInt16), 0, LockFlags.Discard, m_texture_size.Height, m_texture_size.Width);

			// テクスチャ内容の生成
			int		index	= (stride * rect.Y) + (rect.X * sizeof(UInt16));
			for(int y=0; y<m_size.Height; y++){
				UInt16	color	= 0;
				for(int x=0; x<m_size.Width; x++){
					color		= (UInt16)((image[index + x*sizeof(UInt16) + 1] << 8)
										| (image[index + x*sizeof(UInt16) + 0] << 0));
					buf[y, x]	= color;
				}
				// サイズ調整をした場合は1ドット余分にコピーする
				// バイリニア時の参照場所対策
				if(m_texture_size.Width > m_size.Width){
					buf[y, m_size.Width]	= color;
				}
				index	+= stride;
			}
			m_texture_sysmem.UnlockRectangle(0);

			if(m_texture != null)	m_texture.Dispose();

			// Managedなテクスチャに転送する
//			m_texture	= d3d_utility.CreateTextureFromTexture(m_device.Device, Texture);
			// レンダーターゲットのテクスチャに転送する
			RefreshTexture();
		}

		/*-------------------------------------------------------------------------
		 image から rect サイズを切り取ってテクスチャを作成
		 マスク用
		---------------------------------------------------------------------------*/
		virtual public void CreateFromMask(d3d_device device, ref byte[] image, Size size, int stride, Rectangle rect)
		{
			m_device		= device;
			m_offset.X		= rect.X;
			m_offset.Y		= rect.Y;
			
			m_size.Width	= rect.Width;
			m_size.Height	= rect.Height;
			if((m_size.Width + rect.X) > size.Width){
				m_size.Width	= size.Width - rect.X;
			}
			if((m_size.Height + rect.Y) > size.Height){
				m_size.Height	= size.Height - rect.Y;
			}

			m_texture_size	= m_size;

			// テクスチャの作成とロック
			using(Texture texture = new Texture(m_device.device, m_size.Width, m_size.Height,
												1, Usage.Dynamic, Format.A1R5G5B5, Pool.SystemMemory) ){
				UInt16[,] buf		= (UInt16[,])texture.LockRectangle(typeof(UInt16), 0, LockFlags.Discard, m_size.Height, m_size.Width);

				// テクスチャ内容の生成
				int		index	= (stride * rect.Y) + (rect.X * sizeof(UInt16));
				for(int y=0; y<m_size.Height; y++){
					for(int x=0; x<m_size.Width; x++){
						UInt16		color	= (UInt16)((image[index + x*sizeof(UInt16) + 1] << 8)
												| (image[index + x*sizeof(UInt16) + 0] << 0));

						// jpgなのである程度誤差を認める
						if(   (((color >> 0) & 0x1f) < 0x10)
							||(((color >> 5) & 0x3f) < 0x30)
							||(((color >> 5+6) & 0x1f) < 0x10) ){
							color = 0x7fff;		// 抜き
						}

						buf[y, x]	= color;
					}
					index	+= stride;
				}
				texture.UnlockRectangle(0);

				// Managedなテクスチャに転送する
				if(m_texture != null)	m_texture.Dispose();
				m_texture	= d3d_utility.CreateTextureFromTexture(m_device.device, texture);
			}
		}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public void Dispose()
		{
			if(m_texture != null){
				m_texture.Dispose();
				m_texture		= null;
			}
			if(m_texture_sysmem != null){
				m_texture_sysmem.Dispose();
				m_texture_sysmem	= null;
			}
		}

		/*-------------------------------------------------------------------------
		 リフレッシュ
		 デバイスロスト対策
		---------------------------------------------------------------------------*/
		public void RefreshTexture()
		{
			// 特殊な使用をしない
			if(m_texture_sysmem == null)	return;

			// すでにあれば解放する
			if(m_texture != null)	m_texture.Dispose();

			// レンダーターゲットとして使用できるテクスチャを作成し、転送する
			m_texture	= d3d_utility.CreateRenderTargetTextureSameSize(m_device.device, m_texture_sysmem);
			d3d_utility.CopyTexture(m_device.device, m_texture, m_texture_sysmem);
		}

		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		public void Draw(Vector3 pos, float scale)
		{
			Draw(pos, scale, Color.White.ToArgb());
		}
		public void Draw(Vector3 pos, float scale, int color)
		{
			if(m_texture == null)	return;

			Vector2		offset		= m_offset * scale;
//			Vector2		ImageSize		= new Vector2(ImageScale * m_size.Width, ImageScale * m_size.Height);
			Vector2		size		= new Vector2(scale * m_texture_size.Width, scale * m_texture_size.Height);
			Vector2		client_size	= m_device.client_size;

			// オフセットを考慮
			pos.X		*= scale;
			pos.Y		*= scale;
			pos.X		+= offset.X;
			pos.Y		+= offset.Y;

			// 描画範囲チェック
			// debug
			// 範囲内の端付近でカリングする
//			if(pos.X + ImageSize.X < 0+32)		return;
//			if(pos.Y + ImageSize.Y < 0+32)		return;
//			if(pos.X > client_size.X-32)	return;
//			if(pos.Y > client_size.Y-32)	return;
			// 通常のカリング
			if(pos.X + size.X < 0)			return;
			if(pos.Y + size.Y < 0)			return;
			if(pos.X > client_size.X)		return;
			if(pos.Y > client_size.Y)		return;

			// 描画
			m_device.DrawTexture(m_texture, pos, size, color);
		}
	}
	
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class LoopXImage : IDisposable
	{
		private const int			TEXSIZE_STEP		= 512;		// 分割サイズ
		private const int			HEIGHT_MARGIN		= 200;		// 上下のスクロールマージン
		private const float			SCALE_MIN			= 0.3f;		// 縮小限界
		private const float			SCALE_MAX			= 3;		// 拡大限界
	
		private d3d_device			m_device;					// デバイス
	
		private Vector2				m_image_size;				// イメージ全体のサイズ
		private List<TextureUnit>	m_textures;					// テクスチャリスト

		// 描画位置とスケール
		private Vector2				m_offset;					// オフセット
		private float				m_scale;					// 拡大縮小率

		// 描画位置とスケール退避用
		private Vector2				m_offset_shelter;			// オフセット
		private float				m_scale_shelter;			// 拡大縮小率
		private bool				m_is_pushed_params;			// 
	
		// スレッドで読み込む場合の進行具合
		private int					m_load_current;
		private int					m_load_max;
		private string				m_load_str;

		// 描画支援デリゲート
		// 描画時のX方向ループに対応し、オフセットを解決する
		// このデリゲートが何回呼ばれるかはクライアントサイズと画像サイズによる
		public delegate void DrawHandler(Vector2 draw_offset, LoopXImage image);

		private bool				m_device_lost;

		public int					MargeImageMS;

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		// 設定ファイル読み書き用
		public Vector2 OffsetPosition{	get{		return m_offset;		}
										set{		m_offset	= value;	}}

		public Vector2 ImageSize{		get{		return m_image_size;	}}
		public float ImageScale{		get{		return m_scale;			}}

		public int LoadCurrent{			get{		return m_load_current;	}}
		public int LoadMax{				get{		return m_load_max;		}}
		public string LoadStr{			get{		return m_load_str;		}}


		public d3d_device Device{		get{		return m_device;		}}

		/*-------------------------------------------------------------------------
		 オフセットの加算
		---------------------------------------------------------------------------*/
		public void AddOffset(Vector2 add_offset)
		{
			// スケールが小さいほど移動量が大きくなる
			m_offset	+= add_offset * (1 / ImageScale);
		}
	
		/*-------------------------------------------------------------------------
		 スケールの設定
		 指定する位置を中心にする場合はtrueを渡し、
		 中心とする位置をposに設定する
		 マウスのクライアント座標を渡す、もしくはクライアント領域の中心を渡す
		 false時はposを参照しない
		---------------------------------------------------------------------------*/
		public void SetScale(float scale, Point center_pos, bool is_center_mouse)
		{
			// 範囲チェック
			if(scale < SCALE_MIN)	scale	= SCALE_MIN;
			if(scale > SCALE_MAX)	scale	= SCALE_MAX;

			if((scale > 1- 1e-6)&&(scale < 1+ 1e-6)){
				scale	= 1;
			}

			if(is_center_mouse){
				// 指定された位置が中心になるようにオフセットを変更する
				Vector2		pos			= new Vector2(center_pos.X, center_pos.Y);

				// 中心位置の差分を得る
				Vector2		old_offset	= pos * (1 / m_scale);
				Vector2		new_offset	= pos * (1 / scale);

				// オフセットを修正する
				m_offset	-= old_offset - new_offset;

				m_scale		= scale;
			}else{
				// オフセットを変更しない
				m_scale		= scale;
			}
		}

		/*-------------------------------------------------------------------------
		 指定された座標が中心になるようにオフセットを変更する
		---------------------------------------------------------------------------*/
		public void MoveCenterOffset(Point center)
		{
			MoveCenterOffset(center, new Point(0, 0));
		}
		public void MoveCenterOffset(Point center, Point offset)
		{
			Vector2		pos			= new Vector2(center.X, center.Y);
			Vector2		soffset		= new Vector2((m_device.client_size.X - offset.X) / 2, (m_device.client_size.Y - offset.Y) / 2);
			Vector2		_offset		= new Vector2(offset.X, offset.Y);

			_offset		*= (1f/ImageScale);
			soffset		*= (1f/ImageScale);
			pos			-= soffset;
			pos			-= _offset;
			m_offset	= -pos;
		}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public LoopXImage(d3d_device device)
		{
			m_device			= device;
			m_offset			= new Vector2(0, 0);
			m_textures			= new List<TextureUnit>();
			m_is_pushed_params	= false;

			m_scale				= 1;

			m_offset_shelter	= new Vector2(0, 0);
			m_scale_shelter		= 1;

			m_device_lost		= false;
			if(m_device.device != null){
				m_device.device.DeviceReset	+= new System.EventHandler(device_reset);
			}
		}

		/*-------------------------------------------------------------------------
		 デバイスロスト
		 フラグだけ立てる
		---------------------------------------------------------------------------*/
		private void device_reset(object sender, System.EventArgs e)
		{
			m_device_lost		= true;
		}

		/*-------------------------------------------------------------------------
		 イメージの構築
		 進捗状況の初期化
		---------------------------------------------------------------------------*/
		public void InitializeCreateImage()
		{
			m_load_current			= 0;
			m_load_max				= 0;
			m_load_str				= "";
		}
	
		/*-------------------------------------------------------------------------
		 イメージの構築
		---------------------------------------------------------------------------*/
		public bool CreateImage(string file_name)
		{
			// テクスチャリストを解放
			Dispose();

			InitializeCreateImage();

			try{
				// イメージの読み込み
				m_load_str			= file_name;
				Bitmap	bitmap		= new Bitmap(file_name);
				Vector2	size		= new Vector2(bitmap.Width, bitmap.Height);
				m_image_size		= size;

				// 読み込む枚数を数える
				int	count_x			= bitmap.Width / TEXSIZE_STEP;
				if((bitmap.Width % TEXSIZE_STEP) != 0)	count_x++;
				int	count_y			= bitmap.Height / TEXSIZE_STEP;
				if((bitmap.Height % TEXSIZE_STEP) != 0)	count_y++;
				m_load_max			= count_x * count_y;

				// ロックしてイメージ取り出し
				// R5G6B5に変換しておく
				BitmapData bmpdata	= bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
													ImageLockMode.ReadOnly,
													PixelFormat.Format16bppRgb565);
//													PixelFormat.Format32bppArgb);

				IntPtr		ptr		= bmpdata.Scan0;
				int			length	= bmpdata.Height * bmpdata.Stride;
				int			stride	= bmpdata.Stride;
				byte[]		image	= new byte[length];
				Marshal.Copy(ptr, image, 0, length);
				bitmap.UnlockBits(bmpdata);

				// オリジナルは解放しておく
				bitmap.Dispose();
				bitmap				= null;

				// 分割してテクスチャを構築する
				m_load_str			= "テクスチャ転送中...";
				for(int y=0; y<m_image_size.Y; y+=TEXSIZE_STEP){
					for(int x=0; x<m_image_size.X; x+=TEXSIZE_STEP){
						TextureUnit	tex		= new TextureUnit();
						tex.Create(	m_device,
									ref image,
									new Size((int)m_image_size.X, (int)m_image_size.Y),
									stride,
									new Rectangle(x, y, TEXSIZE_STEP, TEXSIZE_STEP));
						m_textures.Add(tex);
						m_load_current++;
					}
				}
				image				= null;
				m_load_str			= "完了";

				System.GC.Collect();
			}catch{
				// 何かを失敗
				return false;
			}
			return true;
		}

		/*-------------------------------------------------------------------------
		 イメージとの合成
		 Begin()を読んだ直後に呼ぶこと
		 must_margeがtrueのとき必ず合成する
		 デバイスロスト時は必ず合成する
		 合成しないときは handler にnullを指定できる

		 デバイスロスト時の処理を含むため、必ず呼び出す必要がある
		---------------------------------------------------------------------------*/
		public void MergeImage(DrawHandler handler, bool must_merge)
		{
			if(!m_device_lost && !must_merge)	return;

			date_timer	d	= new date_timer();

			// テクスチャ更新
			foreach(TextureUnit tex in m_textures){
				tex.RefreshTexture();
			}
			// 合成対象がなければ返る
			if(handler == null){
				m_device_lost	= false;
				// 合成に掛かった時間	
				MargeImageMS	= d.GetSectionTimeMilliseconds();
				return;
			}

			// 合成
			// レンダーターゲットを指定
			Surface	depth				= m_device.device.DepthStencilSurface;
			Surface	backbuffer			= m_device.device.GetBackBuffer(0, 0, BackBufferType.Mono);
			m_device.device.DepthStencilSurface	= null;		// zバッファなし
	
			try{
				foreach(TextureUnit tex in m_textures){
					// レンダーターゲットを設定

					if(tex.Texture == null)		continue;		// テクスチャが作れていない

//					Surface	a	= ((Texture)null).GetSurfaceLevel(0);
					m_device.device.SetRenderTarget(0, tex.Texture.GetSurfaceLevel(0));
					m_device.UpdateClientSize();

					// 描画位置とスケールを退避
					PushDrawParams();
					m_offset	= -tex.Offset;
					SetScale(1, new Point(0, 0), false);

					// レンダリング
					handler(m_offset, this);

					PopDrawParams();
				}
			}catch{
				// 保険
				PopDrawParams();
			}

			// レンダーターゲットを元に戻す
			m_device.device.DepthStencilSurface	= depth;
			m_device.device.SetRenderTarget(0, backbuffer);
			m_device.UpdateClientSize();

			backbuffer.Dispose();
			depth.Dispose();

			m_device_lost	= false;
			// 合成に掛かった時間	
			MargeImageMS	= d.GetSectionTimeMilliseconds();
		}

		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		public void Draw()
		{
			// 読み込みチェック
			if(m_image_size.X <= 0)		return;
			if(m_image_size.Y <= 0)		return;

			// Y方向クランプなし
			// 自由にスクロールする

			// 描画
			m_device.device.RenderState.ZBufferEnable	= false;
			EnumDrawCallBack(new DrawHandler(draw_proc), 0);
			m_device.device.RenderState.ZBufferEnable	= true;
		}

		/*-------------------------------------------------------------------------
		 描画開始位置を求める
		 画面外にどれだけ余りを持たせるか指定できる
		---------------------------------------------------------------------------*/
		private void ajust_draw_start_offset_x(float outside_length_x)
		{
			while(m_offset.X > -outside_length_x)						m_offset.X -= m_image_size.X;
			while(m_offset.X <= -(m_image_size.X+outside_length_x))		m_offset.X += m_image_size.X;
		}

		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		private void draw_proc(Vector2 offset, LoopXImage image)
		{
			Vector3		pos		= new Vector3(offset.X, offset.Y, 0.9f);
			foreach(TextureUnit tex in m_textures){
				tex.Draw(pos, ImageScale);
			}
		}
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public void Dispose()
		{
			foreach(TextureUnit tex in m_textures){
				tex.Dispose();
			}
			m_textures.Clear();
		}

		/*-------------------------------------------------------------------------
		 マウス座標と表示オフセットを解決した座標を得る
		 マウス座標はクライアントレクトからの相対座標であること
		---------------------------------------------------------------------------*/
		public Point MousePos2GlobalPos(Point mouse_pos)
		{
			return transform.ToPoint(MousePos2GlobalPos(transform.ToVector2(mouse_pos)));
		}
		public Vector2 MousePos2GlobalPos(Vector2 mouse_pos)
		{
			// スケールの逆数
			mouse_pos	*= 1 / ImageScale;
	
			Vector2		pos	= mouse_pos - m_offset;

			// イメージサイズに収まるように調整
			while(pos.X >= m_image_size.X)	pos.X	-= m_image_size.X;
			return pos;
		}

		/*-------------------------------------------------------------------------
		 描画オフセットを得る
		 描画オフセットは整数に丸められている
		---------------------------------------------------------------------------*/	
		public Vector2 GetDrawOffset()
		{
			Vector2		_offset		= OffsetPosition;
			_offset.X	= (float)(int)_offset.X;
			_offset.Y	= (float)(int)_offset.Y;
			return _offset;
		}

		/*-------------------------------------------------------------------------
		 グローバルな地図座標から描画用の座標に変換する
		---------------------------------------------------------------------------*/
		public Vector2 GlobalPos2LocalPos(Vector2 global_pos)
		{
			global_pos	+= GetDrawOffset();
			global_pos	*= ImageScale;
			return global_pos;
		}

		/*-------------------------------------------------------------------------
		 オフセット座標がクライアント内に入るように調整する
		 X方向のみ
		---------------------------------------------------------------------------*/
		public Vector2 AjustLocalPos(Vector2 pos)
		{
			if(pos.X > 0){
				while(pos.X >= m_device.device.Viewport.Width){
					pos.X	-= m_image_size.X * ImageScale;
				}
			}else{
				while(pos.X < 0){
					pos.X	+= m_image_size.X * ImageScale;
				}
			}
			return pos;
		}

		/*-------------------------------------------------------------------------
		 グローバルな地図座標から描画用の座標に変換する
		 オフセット指定版
		---------------------------------------------------------------------------*/
		public Vector2 GlobalPos2LocalPos(Vector2 global_pos, Vector2 _offset)
		{
			global_pos	+= _offset;
			global_pos	*= ImageScale;
			return global_pos;
		}

		/*-------------------------------------------------------------------------
		 描画支援

		 最終的な描画座標は
		 GlobalPos2LocalPos();
		 で得ること
		---------------------------------------------------------------------------*/
		public void EnumDrawCallBack(DrawHandler handler, float outside_offset_x)
		{
			if(handler == null)		return;

			ajust_draw_start_offset_x(outside_offset_x);

			Vector2	offset	= GetDrawOffset();
			do{
				// 描画コールバック呼び出し
				handler(offset, this);

				// オフセット調整
				offset.X	+= ImageSize.X;
			}while(offset.X < (((1f / ImageScale) * m_device.client_size.X) + outside_offset_x));
		}

		/*-------------------------------------------------------------------------
		 描画位置とスケールをpush
		 1回しかpushできないので注意
		---------------------------------------------------------------------------*/
		public void PushDrawParams()
		{
			m_offset_shelter	= m_offset;
			m_scale_shelter		= m_scale;
			m_is_pushed_params	= true;
		}
	
		/*-------------------------------------------------------------------------
		 描画位置とスケールをpop
		 1回しかpopできないので注意
		---------------------------------------------------------------------------*/
		public void PopDrawParams()
		{
			if(!m_is_pushed_params)		return;

			m_offset			= m_offset_shelter;
			m_scale				= m_scale_shelter;
			m_is_pushed_params	= false;
		}

	}
}
