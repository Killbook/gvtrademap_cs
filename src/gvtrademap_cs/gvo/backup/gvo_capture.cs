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
#define	DEBUG_WRITE_ANALYZE_POINT		// 解析位置を赤く塗る

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
using utility;
using System.Diagnostics;


/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public class gvo_capture : IDisposable
	{
		// 専用の変換テーブルを使用するため、分解能変更不可
		private const int			COMPASS_DIV			= 360;				// コンパスの分解能
																			// 180の倍数であること
		private const int			COMPASS_1ST_STEP	= 2;				// 最初の検索時の間隔
		private const int			COMPASS_2ND_RANGE	= 4*2;				// 詳細検索時の範囲(+-なのでこの値の倍の範囲を調べる)
		private const int			COMPASS_DIV_90		= COMPASS_DIV/4;	// 90度分の分解能
		private const int			COMPASS_DIV_45		= COMPASS_DIV/8;	// 45度分の分解能
	
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

				using(Texture tex	= new Texture(device, base.size.Width, base.size.Height,
													1, 0, Format.X8R8G8B8, Pool.SystemMemory)){
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
					return d3d_device.CreateTextureFromTexture(device, tex);
				}
			}
		}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public enum mode{
			xp,
			vista,
		};
		private gvt_lib				m_lib;

		private mode				m_capture_mode;		// 動作モード

		private	capture_image		m_capture1;			// 測量と日付用
		private	capture_image		m_capture2;			// 進行方向用
		// 解析結果
		private Point				m_point;			// 解析後の測量座標
		private int					m_days;				// 解析後の日数
		private float				m_angle;			// コンパス方向
		private Vector2				m_angle_vector;		// コンパスベクトル

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
		private Point[]				m_compass_pos;		// コンパスからの角度算出用
		private float[]				m_ajust_compass;	// 調整用テーブル

		// コンパス解析詳細用
		private int					m_1st_com_index;	// 黄色矢印位置インデックス
		private float				m_com_index;		// テーブルインデックス
		private float				m_com_index2;		// テーブルインデックス
		private int					m_an_index;
	
		// コンパス調整用
/*		private float				m_com_index2;		// テーブルインデックス
		private int					m_an_index2;
		private float				m_angle2;
*/	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public Point point{					get{	return m_point;				}}
		public int days{					get{	return m_days;				}}
		public float angle{					get{	return m_angle;				}}
		public Vector2 angle_vector{		get{	return m_angle_vector;		}}
		public bool capture_point_success{
			get{
				if(point.X < 0)		return false;
				if(point.Y < 0)		return false;
				return true;		// 正常にキャプチャできた
			}
		}
		public bool capture_days_success{	get{	return (days < 0)? false: true;	}}

		public bool capture_success{
			get{
				if(!capture_days_success)	return false;
				return capture_point_success;
			}
		}
		public mode capture_mode{			get{	return m_capture_mode;		}
											set{	m_capture_mode	= value;	}}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public gvo_capture(gvt_lib lib)
		{
			m_lib			= lib;

			// キャプチャ用ビットマップを作成する
			m_capture1		= new capture_image(64, 24);
			m_capture2		= new capture_image(128, 128);

			// 解析結果初期化
			m_point			= new Point(-1, -1);
			m_days			= -1;
			m_angle			= -1;
			m_angle_vector	= new Vector2(0, 0);

			// キャプチャモード
			capture_mode	= mode.xp;

			// テーブル生成用調整値
			m_factor		= 36f;
			m_angle_x		= 31.5f;
			m_l				= 54f;
	
			// コンパスからの角度算出用テーブルを作成する
			create_compass_tbl();
			// 調整テーブルの作成
			create_ajust_compass_tbl();

			m_debug_texture		= null;
			m_debug_texture2	= null;
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public void Dispose()
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
		 大航海時代Onlineのウインドウを捜す
		 見つかった場合はtrueを返す
		---------------------------------------------------------------------------*/
		static public bool IsFoundGvoWindow()
		{
			IntPtr	hwnd	= find_gvo_window();
			if(hwnd == IntPtr.Zero)	return false;
			return true;
		}

		/*-------------------------------------------------------------------------
		 大航海時代Onlineのウインドウを得る
		 見つからなかった場合は IntPtr.Zero を返す
		---------------------------------------------------------------------------*/
		static private IntPtr find_gvo_window()
		{
			return user32.FindWindowA(	def.GVO_CLASS_NAME,
										def.GVO_WINDOW_NAME);
		}

		/*-------------------------------------------------------------------------
		 大航海時代Onlineのウインドウを捜し
		 見つかれば画面座標で矩形を返す
		 見つかればウインドウハンドルを返す
		 見つからなければnullを返す
		---------------------------------------------------------------------------*/
		static private IntPtr find_gvo_window(out Rectangle rect)
		{
			rect	= new Rectangle();

			// ウインドウを捜す
			IntPtr	hwnd	= find_gvo_window();

			if(hwnd == IntPtr.Zero)	return IntPtr.Zero;

			// クライアント領域を画面座標に変換する
			Point	p	= new Point();
			user32.GetClientRect(hwnd, ref rect);
			user32.ClientToScreen(hwnd, ref p);

			// サイズが0なら最小化されてる
			if(rect.Width <= 0)		return IntPtr.Zero;
			if(rect.Height <= 0)	return IntPtr.Zero;

			rect.X		= p.X;
			rect.Y		= p.Y;
			return hwnd;
		}

		/*-------------------------------------------------------------------------
		 画面をキャプチャし、日付や座標、進行方向を得る
		---------------------------------------------------------------------------*/
		public bool CaptureAll()
		{
			// 画面をキャプチャ
			if(!capture_dol_window()){
				// 測量位置
				m_point		= new Point(-1, -1);
				return false;
			}

			// キャプチャしたイメージを得る
			m_capture1.CreateImage();
			m_capture2.CreateImage();

			// 日付を得る
			analize_days();

			// 測量座標を得る
			analize_point();

			// 進行方向を得る
			analize_angle();

			if(m_lib.setting.draw_capture_info){
	// 解析する場所を書き込む
	//			debug_compass();
				// キャプチャしたイメージをテクスチャにする
				// コンパスイメージをテクスチャにする
				release_debug_textures();
				m_debug_texture		= m_capture2.CreateTexture(m_lib.device.device);
				// 日付と測量位置イメージをテクスチャにする
				m_debug_texture2	= m_capture1.CreateTexture(m_lib.device.device);
			}
			return true;
		}

		/*-------------------------------------------------------------------------
		 画面をキャプチャする
		---------------------------------------------------------------------------*/
		private bool capture_dol_window()
		{
			Rectangle	rect;
			IntPtr		hwnd	= find_gvo_window(out rect);
			if(hwnd == IntPtr.Zero)	return false;		// 見つからない

			if(capture_mode == mode.xp){
				// XP,VistaのAero以外

				// オフセットは0にする
				rect.X	= 0;
				rect.Y	= 0;
			}else{
				// Vista Aero
				// 直接キャプチャできないのでデスクトップを得る
				hwnd	= IntPtr.Zero;
			}

			// キャプチャ
			IntPtr	hdc	= user32.GetDC(hwnd);
			if(hdc == IntPtr.Zero)	return false;		// キャプチャ失敗

			// 1つ目のキャプチャ
			// 測量情報を得る
			m_capture1.DoCapture(hdc,
									new Point(0, 0),
									new Point(	rect.X + (rect.Width  -  72),
												rect.Y + (rect.Height - 272)),
									new Size(60, 11));
			// 航海日数を得る
			m_capture1.DoCapture(hdc,
									new Point(0, 12),
									new Point(	rect.X + 14,
												rect.Y + 19),
									new Size(21, 11));

			// 2つ目のキャプチャ
			// コンパスを得る
			m_capture2.DoCapture(hdc,
									new Point(0, 0),
									new Point(	rect.X + (rect.Width  - (144+4)),
												rect.Y + (rect.Height - (104+4))),
									new Size(105+(4*2), 99+(4*2)));

			user32.ReleaseDC(hwnd, hdc);
			return true;
		}

		/*-------------------------------------------------------------------------
		 数値を解析する
		---------------------------------------------------------------------------*/
		private bool analize_number(int index, out int num)
		{
            string[]	check_tbl	= new string[]{
				"011111110100000001100000001011111110",		// 0
				"000000000010000000111111111000000000",		// 1
				"011000011100001101100010001011100001",		// 2
				"011000110100010001100010001011101110",		// 3
				"000001100000110100011000100111111111",		// 4
				"111110110100100001100100001100011110",		// 5
				"011111110100010001100010001011001110",		// 6
				"100000000100000111100111000111000000",		// 7
				"011101110100010001100010001011101110",		// 8
				"011100110100010001100010001011111110",		// 9
				"000000010000000010000000011000000000" };	// ,

			byte[]	image	= m_capture1.image;
			int		stride	= m_capture1.stride;
				
			int		offset	= index * 6 * 3;
			string	chk		= "";
			for(int x=1; x<5; x++){
				int offset_y	= 0;
				for(int y=0; y<9; y++){
					int		tmp		= offset_y + offset + (x * 3);
					int		color	= image[tmp + 0]
										+ image[tmp + 1]
										+ image[tmp + 2];
					if(color > 720)	chk	+= '1';
					else			chk	+= '0';

					offset_y	+= stride;
				}
			}

			// 初期値
			num		= 0;
			for(int i=0; i<10; i++){
				if(chk == check_tbl[i]){
					num		= i;
					return true;
				}
			}

			// 数値として認識できなかった
			return false;
		}

		/*-------------------------------------------------------------------------
		 測量座標を得る
		---------------------------------------------------------------------------*/
		private void analize_point()
		{
			bool	chk	= false;

			// x
			m_point.X	= 0;
			for(int i=0; i<5; i++){
				int num;
				if(analize_number(i, out num)){
					chk			= true;		// 解析できた
					m_point.X	*= 10;
					m_point.X	+= num;
				}else{
					if(chk){
						// 1文字以上キャプチャ出来てて、途中で失敗
						chk		= false;
						break;				// 解析中止
					}
				}
			}
			if(!chk){
				m_point		= new Point(-1, -1);	// 解析失敗
				return;
			}

			// y
			chk			= false;
			m_point.Y	= 0;
			for(int i=6; i<10; i++){
				int num;
				if(analize_number(i, out num)){
					chk			= true;		// 解析できた
					m_point.Y	*= 10;
					m_point.Y	+= num;
				}else{
					if(chk){
						// 1文字以上キャプチャ出来てて、途中で失敗
						break;				// 成功したと見るしかない
					}
				}
			}
			if(!chk){
				m_point		= new Point(-1, -1);	// 解析失敗
			}
		}

		/*-------------------------------------------------------------------------
		 日付を得る
		---------------------------------------------------------------------------*/
		private void analize_days()
		{
			string[]	check_tbl	= new string[] {
				"111110000001011100",		// 0
				"100001111111000000",		// 1
				"110011001011110011",		// 2
				"110011001001110111",		// 3
				"001000011111000001",		// 4
				"101011101001000110",		// 5
				"111111001001100110",		// 6
				"100011101110100000",		// 7
				"110110001001110110",		// 8
				"111011000001011110" };		// 9

			byte[]	image	= m_capture1.image;
			int		stride	= m_capture1.stride;

			int		max;
			int		start;
	
			// 桁を調べる
			int		index;
			int		color_0, color_1;
			index	= (stride * 16) + 10 * 3;
			color_0	= image[index + 0]
						+ image[index + 1]
						+ image[index + 2];
			index	= (stride * 22) + 10 * 3;
			color_1	= image[index + 0]
						+ image[index + 1]
						+ image[index + 2];

			if(color_0 > 128*3 || color_1 > 128*3){
				// 3桁か1桁
				// さらにチェックする
				index	= (stride * 16) + 2 * 3;
				color_0	= image[index + 0]
							+ image[index + 1]
							+ image[index + 2];
				index	= (stride * 22) + 2 * 3;
				color_1	= image[index + 0]
							+ image[index + 1]
							+ image[index + 2];

				if(color_0 > 128*3 || color_1 > 128*3){
					// 3桁で解析する
					max		= 3;
					start	= 16 - 8*2;
				}else{
					// 1桁で解析する
					max		= 1;
					start	= 8;
				}
			}else{
				// 2桁で解析する
				max		= 2;
				start	= 12 - 8;
			}

			m_days			= 0;
			for(int i=0; i<max; i++){
				string	chk		= "";
				int		offset	= ((start + (i * 8)) * 3) + (12 * stride);
				for(int x=0; x<6; x+=2){
					int	offset_y	= 0;
					for(int y=0; y<12; y+=2){
						int		tmp		= offset_y + offset + (x * 3);
						int		color	= image[tmp + 0]
											+ image[tmp + 1]
											+ image[tmp + 2];
						if(color > 128*3)	chk	+= '1';
						else				chk	+= '0';

						offset_y	+= stride * 2;
					}
				}

				bool	is_find		= false;
				for(int j=0; j<10; j++){
					if(chk == check_tbl[j]){
						m_days		*= 10;
						m_days		+= j;
						is_find		= true;
						break;
					}
				}
				if(!is_find){
					// 解析失敗
					m_days	= -1;
					return;
				}
			}
		}

		/*-------------------------------------------------------------------------
		 進行方向を得る
		 コンパスから解析する
		---------------------------------------------------------------------------*/
		private void analize_angle()
		{
#if DEBUG_COMPASS
			// debug
			// パラメータを反映させてテーブルを再構築
			create_compass_tbl();
#endif

			// 初期値
			m_angle			= -1;
			m_angle_vector	= new Vector2(0, 0);
	
			// キャプチャが成功してなければ解析しない
			if(!capture_success)	return;
	
			// 大まかな分解能で調べる
			int	index	= analize_angle_1st_step();
			if(index < 0)			return;

			// 大まかな分解能のインデックスを覚えておく
			m_1st_com_index	= index;

			float	angle	= 0;
			float	angle2	= 0;
#if aa
			// 一番解像度の高い135～225度で解析する
			if(   (index >= COMPASS_DIV_45)
				&&(index < COMPASS_DIV_45+COMPASS_DIV_90*1) ){
				// +90度の位置
				angle	= analize_angle_2nd_step2(index + COMPASS_DIV_90);
				angle	-= COMPASS_DIV_90;
				angle	+= 1;		// 270度をきっちり出すための調整
			}else if( (index >= COMPASS_DIV_45+COMPASS_DIV_90*1)
					&&(index < COMPASS_DIV_45+COMPASS_DIV_90*2) ){
				// そのまま
				angle	= analize_angle_2nd_step(index);
			}else if( (index >= COMPASS_DIV_45+COMPASS_DIV_90*2)
//					&&(index < COMPASS_DIV_45+COMPASS_DIV_90*3) ){
					&&(index <= (COMPASS_DIV_45+COMPASS_DIV_90*3)-1) ){	// 314を含む
				// -90度の位置
				angle	= analize_angle_2nd_step2(index - COMPASS_DIV_90);
				angle	+= COMPASS_DIV_90;
			}else{
				// +180度の位置
				angle	= analize_angle_2nd_step2(index + COMPASS_DIV_90*2);
				angle	-= COMPASS_DIV_90*2;

////////////////////////////////////////////////
				// 44度の無理やり判定
				// これがないと46度と判別できない
				if((int)angle == 314){
					angle	= 315f;
				}
////////////////////////////////////////////////
			}
#else
			// 下半分の180度分で解析する
			// 黄色の矢印の位置で見ない
//			if(index > COMPASS_DIV_90*3 +4){	// 受け渡し時の微妙な調整値を含む
//			if(index > COMPASS_DIV_90*3){
			if(index > COMPASS_DIV_90*3 +2){	// 再調整
				angle	= analize_angle_2nd_step2(index + COMPASS_DIV_90*2);
				angle	-= COMPASS_DIV_90*2;

				angle2	= analize_angle_2nd_step2(index - COMPASS_DIV_90);
				angle2	+= COMPASS_DIV_90;

				m_an_index	= 0;
			}else if(index <= COMPASS_DIV_90*1){
				// 
				angle	= analize_angle_2nd_step2(index + COMPASS_DIV_90*2);
				angle	-= COMPASS_DIV_90*2;

				angle2	= analize_angle_2nd_step2(index + COMPASS_DIV_90);
				angle2	-= COMPASS_DIV_90;

				m_an_index	= 1;
			}else if(  (index > COMPASS_DIV_90*1)
					 &&(index <= COMPASS_DIV_90*2) ){
				angle	= analize_angle_2nd_step2(index + COMPASS_DIV_90);
				angle	-= COMPASS_DIV_90;

				angle2	= analize_angle_2nd_step2(index - COMPASS_DIV_90);
				angle2	+= COMPASS_DIV_90;

				m_an_index	= 2;
			}else{
				// 
				angle	= analize_angle_2nd_step2(index - COMPASS_DIV_90);
				angle	+= COMPASS_DIV_90;

				angle2	= analize_angle_2nd_step2(index + COMPASS_DIV_90);
				angle2	-= COMPASS_DIV_90;

				m_an_index	= 3;
			}

			// インデックス開示
			m_com_index		= angle;
			m_com_index2	= angle2;

			// 修正テーブルを用いて角度に変換
			m_angle	= update_angle_with_ajust(angle);
			// ベクトルの更新
			update_vector(m_angle);
/*
////////////////
			// 調整用
			if(index > COMPASS_DIV_90*3 +2){
				angle	= analize_angle_2nd_step2(index + COMPASS_DIV_90*2);
				angle	-= COMPASS_DIV_90*2;

				m_an_index2	= 0;
			}else if(index <= COMPASS_DIV_90*1){
				// 
				angle	= analize_angle_2nd_step2(index + COMPASS_DIV_90*2);
				angle	-= COMPASS_DIV_90*2;

				m_an_index2	= 1;
			}else if(  (index > COMPASS_DIV_90*1)
					 &&(index <= COMPASS_DIV_90*2) ){
				angle	= analize_angle_2nd_step2(index + COMPASS_DIV_90);
				angle	-= COMPASS_DIV_90;

				m_an_index2	= 2;
			}else{
				// 
				angle	= analize_angle_2nd_step2(index - COMPASS_DIV_90);
				angle	+= COMPASS_DIV_90;

				m_an_index2	= 3;
			}
			// インデックス開示
			m_com_index2		= angle;
			if(m_com_index2 >= COMPASS_DIV)	m_com_index2	-= COMPASS_DIV;
			if(m_com_index2 < 0)			m_com_index2	+= COMPASS_DIV;

			// 修正テーブルを用いて角度に変換
			m_angle2	= update_angle_with_ajust(angle);
			// ベクトルの更新
//			update_vector(m_angle2);
*/
////////////////
#endif
		}

		/*-------------------------------------------------------------------------
		 大まかな分解能で調べる
		 細かく調べる前段階
		 失敗した場合は-1を返す
		---------------------------------------------------------------------------*/
		private int analize_angle_1st_step()
		{
			for(int i=0; i<COMPASS_DIV; i+=COMPASS_1ST_STEP){
				if(analize_angle_sub(i))	return i;
			}
			return -1;
		}

		/*-------------------------------------------------------------------------
		 黄色の位置で解析
		---------------------------------------------------------------------------*/
/*
		private float analize_angle_2nd_step(int index)
		{
			// index 付近を詳しく調べる
			// 開始位置
			int	start	= index;
			for(int i=0; i<COMPASS_2ND_RANGE; i++){
				int		tmp	= index - (i + 1);
				if(analize_angle_sub(tmp))	start	= tmp;
			}
			// 終了位置
			int	last	= index;
			for(int i=0; i<COMPASS_2ND_RANGE; i++){
				int		tmp	= index + (i + 1);
				if(analize_angle_sub(tmp))	last	= tmp;
			}

			// 真ん中を角度とする
			return ((float)(start + last)) * 0.5f;
		}
*/	
		/*-------------------------------------------------------------------------
		 白色の位置で解析
		---------------------------------------------------------------------------*/
		private float analize_angle_2nd_step2(int index)
		{
			// index 付近を詳しく調べる
			// 開始位置
			int	start	= index;
			for(int i=0; i<COMPASS_2ND_RANGE; i++){
				int		tmp	= index - (i + 1);
				if(analize_angle_sub2(tmp))	start	= tmp;
			}
			// 終了位置
			int	last	= index;
			for(int i=0; i<COMPASS_2ND_RANGE; i++){
				int		tmp	= index + (i + 1);
				if(analize_angle_sub2(tmp))	last	= tmp;
			}

			// 真ん中を角度とする
			return ((float)(start + last)) * 0.5f;
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
		 進行方向を得る
		 コンパスから解析する
		 指定された場所を調べる
		---------------------------------------------------------------------------*/
		private bool analize_angle_sub(int i)
		{
			byte[]	image	= m_capture2.image;
			int		stride	= m_capture2.stride;

			int		index;
			int		r, g, b;

			// テーブルの範囲に丸める
//			i		&= COMPASS_DIV-1;
			if(i >= COMPASS_DIV)	i	-= COMPASS_DIV;
			if(i < 0)				i	+= COMPASS_DIV;
	
			index	= (stride * (m_compass_pos[i].Y)) + (m_compass_pos[i].X * 3);
			b		= image[index + 0];
			g		= image[index + 1];
			r		= image[index + 2];

			if((r > (b + 10)) && (g > (b + 10))){
				// 黄色い点が見つかった
#if DEBUG_WRITE_ANALYZE_POINT
				image[index + 0]	= 0;
				image[index + 1]	= 0;
				image[index + 2]	= 255;
#endif
				return true;
			}
			return false;
		}

		/*-------------------------------------------------------------------------
		 進行方向を得る
		 コンパスから解析する
		 指定された場所を調べる
		 白の矢印解析用
		---------------------------------------------------------------------------*/
		private bool analize_angle_sub2(int i)
		{
			byte[]	image	= m_capture2.image;
			int		stride	= m_capture2.stride;

			int		index;
			int		r, g, b;

			// テーブルの範囲に丸める
//			i		&= COMPASS_DIV-1;
			if(i >= COMPASS_DIV)	i	-= COMPASS_DIV;
			if(i < 0)				i	+= COMPASS_DIV;

			index	= (stride * (m_compass_pos[i].Y)) + (m_compass_pos[i].X * 3);
			b		= image[index + 0];
			g		= image[index + 1];
			r		= image[index + 2];

			if((r >= 137)&&(r == g)&&(g == b)){
				// 白い点が見つかった
#if DEBUG_WRITE_ANALYZE_POINT
				image[index + 0]	= 0;
				image[index + 1]	= 0;
				image[index + 2]	= 255;
#endif
				return true;
			}
			return false;
		}
	
		/*-------------------------------------------------------------------------
		 角度を設定する
		 iは 0～(COMPASS_DIV-1)
		 少しこの範囲をはみ出るのは問題ない
		---------------------------------------------------------------------------*/
/*
		private void update_angle(float i)
		{
			m_angle				= ((float)COMPASS_DIV-i) * ((float)(360d / COMPASS_DIV));
			if(m_angle >= 360)	m_angle	-= 360;
			if(m_angle < 0)		m_angle	+= 360;

			float	angle		= useful.ToRadian(m_angle - 90f);
			m_angle_vector.X	= (float)Math.Cos(angle);
			m_angle_vector.Y	= (float)Math.Sin(angle);
		}
*/
		/*-------------------------------------------------------------------------
		 コンパスからの角度算出用テーブルを作成する
		---------------------------------------------------------------------------*/
		private void create_compass_tbl()
		{
			m_compass_pos	= new Point[COMPASS_DIV];
	
			Matrix	mat		= Matrix.PerspectiveFovRH(useful.ToRadian(60), 1f, 0.1f, 1000f);
			Matrix	mat2	= Matrix.RotationX(useful.ToRadian(m_angle_x));
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
				float	angle		= useful.ToRadian(-90 + ((360f/COMPASS_DIV) * i));
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
		 コンパス解析の修正テーブルの作成
		---------------------------------------------------------------------------*/
		private void create_ajust_compass_tbl()
		{
			m_ajust_compass		= new float[]{
				0	,0	,358	,358	,356	,354	,354	,354	,352	,352	,
				350	,350	,348	,348	,346	,346	,344	,344	,342	,342	,
				340	,340	,338	,338	,336	,336	,334	,334	,332	,332	,
				330	,330	,328	,328	,326	,326	,324	,324	,322	,322	,
				320	,320	,318	,318	,316	,316	,314	,314	,312	,310	,
				310	,308	,308	,306	,306	,304	,304	,302	,302	,300	,
				300	,298	,298	,296	,296	,294	,294	,292	,292	,290	,
				290	,288	,288	,286	,286	,284	,284	,282	,282	,280	,
				280	,278	,276	,276	,276	,274	,274	,272	,272	,270	,
//				280	,278	,278	,276	,276	,274	,274	,272	,272	,270	,
				270	,268	,268	,266	,266	,264	,264	,262	,262	,260	,
				260	,258	,258	,256	,256	,254	,254	,252	,252	,250	,
				250	,248	,248	,246	,246	,244	,244	,242	,242	,240	,
				240	,238	,238	,236	,236	,234	,234	,232	,232	,230	,
				230	,228	,228	,226	,226	,224	,224	,222	,222	,220	,
				220	,218	,218	,216	,216	,214	,214	,212	,212	,210	,
				210	,208	,208	,206	,206	,204	,204	,202	,202	,200	,
				200	,198	,198	,196	,196	,194	,194	,192	,192	,190	,
				190	,188	,188	,186	,186	,184	,184	,182	,182	,180	,
				180	,178	,178	,176	,174	,174	,172	,172	,170	,170	,
				168	,168	,166	,166	,164	,164	,162	,162	,160	,160	,
				158	,158	,156	,156	,154	,154	,152	,152	,150	,150	,
				148	,148	,146	,146	,144	,144	,142	,142	,140	,140	,
				138	,138	,136	,136	,134	,134	,132	,132	,130	,130	,
				128	,128	,126	,126	,124	,124	,122	,122	,120	,120	,
				120	,118	,118	,116	,116	,114	,114	,112	,112	,110	,
				110	,108	,108	,106	,106	,104	,104	,102	,102	,100	,
				100	,98	,98	,96	,96	,94	,94	,92	,92	,90	,
				90	,88	,88	,86	,86	,84	,84	,82	,82	,80	,
				78	,78	,76	,76	,74	,74	,72	,72	,70	,70	,
				68	,68	,66	,66	,64	,64	,62	,62	,60	,60	,
				58	,58	,56	,56	,54	,54	,52	,52	,50	,50	,
				48	,48	,46	,46	,44	,44	,42	,42	,40	,40	,
				38	,38	,36	,36	,34	,34	,32	,32	,30	,30	,
				30	,28	,28	,26	,26	,24	,24	,22	,22	,20	,
				20	,18	,18	,16	,16	,14	,14	,12	,12	,10	,
				10	,8	,8	,6	,6	,4	,4	,2	,2	,0	,
			};
		}

		/*-------------------------------------------------------------------------
		 修正テーブルを用いて角度に変換	
		---------------------------------------------------------------------------*/
		private float update_angle_with_ajust(float angle_index)
		{
			// 小数部は捨てる
			int		index	= (int)angle_index;

			if(index >= COMPASS_DIV)	index	-= COMPASS_DIV;
			if(index < 0)				index	+= COMPASS_DIV;
			if(index >= COMPASS_DIV)	return -1f;	// 範囲エラー
			if(index < 0)				return -1f;	// 範囲エラー

			// 調整された角度をテーブルから得る
			// 調整テーブルは作成に時間が掛かるので注意
			// 角度は2度刻みとなる
			return m_ajust_compass[index];
		}

		/*-------------------------------------------------------------------------
		 ベクトルを更新する
		 angleはdegree
		---------------------------------------------------------------------------*/
		private void update_vector(float angle)
		{
			if(angle < 0)	return;

			// vector
			angle				= useful.ToRadian(angle - 90f);
			m_angle_vector.X	= (float)Math.Cos(angle);
			m_angle_vector.Y	= (float)Math.Sin(angle);
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
//				m_lib.device.DrawFillRect(spos, new Vector2(64+16, (12*(3+3))+2+2), (int)0xC0000000);
			}
			spos.X	+= 2;
			spos.Y	+= 2;
			m_lib.device.systemfont.locate	= spos;
			m_lib.device.systemfont.Puts(String.Format("index1={0}\nindex2={1}\n",
											m_1st_com_index, m_com_index), Color.White);
			m_lib.device.systemfont.Puts(String.Format("index3={0}\nquadrant={1}\n",
											m_com_index2, m_an_index), Color.White);
//			m_lib.device.systemfont.Puts(String.Format("index2={0}\nquadrant={1}\nangle2={2}\n",
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
