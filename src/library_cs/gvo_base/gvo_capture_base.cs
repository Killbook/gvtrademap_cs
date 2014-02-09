﻿/*-------------------------------------------------------------------------

 大航海時代Online画面キャプチャ
 船の角度は2度間隔で確定
 360段階の分解能から180段階の値を得る
 0度は実機の仕様により向けない

 無理やり引き剥がしたので実装がちょっとあれだがdirectxを必要としない

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 define
---------------------------------------------------------------------------*/
// コンパス解析デバッグ
#define	DEBUG_WRITE_ANALYZE_POINT		// 解析位置を赤く塗る

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using win32;
using Utility;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvo_base
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public class gvo_capture_base : IDisposable
	{
		// 専用の変換テーブルを使用するため、分解能変更不可
		protected const int			COMPASS_DIV			= 360;				// コンパスの分解能
																			// 180の倍数であること
		protected const int			COMPASS_1ST_STEP	= 2;				// 最初の検索時の間隔
		protected const int			COMPASS_2ND_RANGE	= 4*2;				// 詳細検索時の範囲(+-なのでこの値の倍の範囲を調べる)
		protected const int			COMPASS_DIV_90		= COMPASS_DIV/4;	// 90度分の分解能
		protected const int			COMPASS_DIV_45		= COMPASS_DIV/8;	// 45度分の分解能

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public enum mode{
			xp,
			vista,
		};
		private mode				m_capture_mode;		// 動作モード

		private	screen_capture		m_capture1;			// 測量と日付用
		private	screen_capture		m_capture2;			// 進行方向用

		// 解析結果
		private Point				m_point;			// 解析後の測量座標
		private int					m_days;				// 解析後の日数
		private float				m_angle;			// コンパス方向

		protected Point[]			m_compass_pos;		// コンパスからの角度算出用
		protected float[]			m_ajust_compass;	// 調整用テーブル

		// コンパス解析詳細用
		protected int				m_1st_com_index;	// 黄色矢印位置インデックス
		protected float				m_com_index;		// テーブルインデックス
		protected float				m_com_index2;		// テーブルインデックス
		protected int				m_an_index;			// 象限
	
		// コンパス調整用
/*		protected float				m_com_index2;		// テーブルインデックス
		protected int				m_an_index2;
		protected float				m_angle2;
*/	

		private bool				m_enable_point;		// 測量座標解析有効
		private bool				m_enable_days;		// 航海日数解析有効
		private bool				m_enable_angle;		// コンパス解析有効
		
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public Point point{					get{	return m_point;				}}
		public int days{					get{	return m_days;				}}
		public float angle{					get{	return m_angle;				}}
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


		protected screen_capture capture1{	get{	return m_capture1;			}}
		protected screen_capture capture2{	get{	return m_capture2;			}}

		protected bool enable_point{		get{	return m_enable_point;		}
											set{	m_enable_point	= value;	}}
		protected bool enable_days{			get{	return m_enable_days;		}
											set{	m_enable_days	= value;	}}
		protected bool enable_angle{		get{	return m_enable_angle;		}
											set{	m_enable_angle	= value;	}}
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public gvo_capture_base()
		{
			// キャプチャ用ビットマップを作成する
			m_capture1		= CreateScreenCapture(64, 24);
			m_capture2		= CreateScreenCapture(128, 128);

			// 解析有効フラグ
			m_enable_point	= true;		// 測量座標解析有効
			m_enable_days	= true;		// 航海日数解析有効
			m_enable_angle	= true;		// コンパス解析有効
	
			// 解析結果初期化
			m_point			= new Point(-1, -1);
			m_days			= -1;
			m_angle			= -1;

			// キャプチャモード
			capture_mode	= mode.xp;
	
			// コンパスからの角度算出用テーブルを作成する
			create_compass_tbl();
			// 調整テーブルの作成
			create_ajust_compass_tbl();
		}
		
		/*-------------------------------------------------------------------------
		 screen_captureを作成する
		 screen_captureを継承したものを使用する場合オーバーライドすること
		 コンストラクタ内で呼ばれるので注意
		---------------------------------------------------------------------------*/
		protected virtual screen_capture CreateScreenCapture(int size_x, int size_y)
		{
			return new screen_capture(size_x, size_y);
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public virtual void Dispose()
		{
		}

		/*-------------------------------------------------------------------------
		 大航海時代Onlineのウインドウを捜す
		 見つかった場合はtrueを返す
		---------------------------------------------------------------------------*/
		static public bool IsFoundGvoWindow()
		{
			IntPtr	hwnd	= FindGvoWindow();
			if(hwnd == IntPtr.Zero)	return false;
			return true;
		}

		/*-------------------------------------------------------------------------
		 大航海時代Onlineのウインドウを得る
		 見つからなかった場合は IntPtr.Zero を返す
		---------------------------------------------------------------------------*/
		static public IntPtr FindGvoWindow()
		{
			return user32.FindWindowA(	gvo_def.GVO_CLASS_NAME,
										gvo_def.GVO_WINDOW_NAME);
		}

		/*-------------------------------------------------------------------------
		 大航海時代Onlineのウインドウを捜し
		 見つかれば画面座標で矩形を返す
		 見つかればウインドウハンドルを返す
		 見つからなければnullを返す
		---------------------------------------------------------------------------*/
		static public IntPtr FindGvoWindow(out Rectangle rect)
		{
			rect	= new Rectangle();

			// ウインドウを捜す
			IntPtr	hwnd	= FindGvoWindow();

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
		public virtual bool CaptureAll()
		{
			// 画面をキャプチャ
			if(!capture_dol_window()){
				// 失敗時は全て初期値を設定する
				m_point		= new Point(-1, -1);
				m_days		= -1;
				m_angle		= -1;
				return false;
			}

			// キャプチャしたイメージを得る
			if(m_enable_point || m_enable_days)	m_capture1.CreateImage();
			if(m_enable_angle)					m_capture2.CreateImage();

			// 日付を得る
			if(m_enable_days)	analize_days();

			// 測量座標を得る
			if(m_enable_point)	analize_point();

			// 進行方向を得る
			if(m_enable_angle)	analize_angle();
			return true;
		}

		/*-------------------------------------------------------------------------
		 画面をキャプチャする
		---------------------------------------------------------------------------*/
		private bool capture_dol_window()
		{
			Rectangle	rect;
			IntPtr		hwnd	= FindGvoWindow(out rect);
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

			// キャプチャする
			DoCapture(hdc, rect);

			user32.ReleaseDC(hwnd, hdc);
			return true;
		}

		/*-------------------------------------------------------------------------
		 画面をキャプチャする
		---------------------------------------------------------------------------*/
		protected virtual void DoCapture(IntPtr hdc, Rectangle rect)
		{
			// 1つ目のキャプチャ
			// 測量情報を得る
			if(m_enable_point){
				m_capture1.DoCapture(hdc,
										new Point(0, 0),
										new Point(	rect.X + (rect.Width  -  72),
													rect.Y + (rect.Height - 272)),
										new Size(60, 11));
			}
			// 航海日数を得る
			if(m_enable_days){
				m_capture1.DoCapture(hdc,
										new Point(0, 12),
										new Point(	rect.X + 14,
													rect.Y + 19),
										new Size(21, 11));
			}

			// 2つ目のキャプチャ
			// コンパスを得る
			if(m_enable_angle){
				m_capture2.DoCapture(hdc,
										new Point(0, 0),
										new Point(	rect.X + (rect.Width  - (144+4)),
													rect.Y + (rect.Height - (104+4))),
										new Size(105+(4*2), 99+(4*2)));
			}
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
			// 初期値
			m_angle			= -1;
	
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
		}
*/
		/*-------------------------------------------------------------------------
		 コンパスからの角度算出用テーブルを作成する
		---------------------------------------------------------------------------*/
		private void create_compass_tbl()
		{
			m_compass_pos	= new Point[]{
				new Point(56, 1),new Point(56, 1),new Point(57, 1),new Point(58, 1),new Point(59, 1),new Point(59, 1),new Point(60, 1),new Point(61, 1),new Point(62, 1),new Point(63, 1),
				new Point(63, 1),new Point(64, 1),new Point(65, 1),new Point(66, 2),new Point(67, 2),new Point(67, 2),new Point(68, 2),new Point(69, 2),new Point(70, 2),new Point(70, 3),
				new Point(71, 3),new Point(72, 3),new Point(73, 3),new Point(73, 3),new Point(74, 4),new Point(75, 4),new Point(76, 4),new Point(76, 4),new Point(77, 5),new Point(78, 5),
				new Point(79, 5),new Point(79, 6),new Point(80, 6),new Point(81, 6),new Point(82, 6),new Point(82, 7),new Point(83, 7),new Point(84, 8),new Point(84, 8),new Point(85, 8),
				new Point(86, 9),new Point(87, 9),new Point(87, 9),new Point(88, 10),new Point(89, 10),new Point(89, 11),new Point(90, 11),new Point(91, 12),new Point(91, 12),new Point(92, 13),
				new Point(92, 13),new Point(93, 14),new Point(94, 14),new Point(94, 15),new Point(95, 15),new Point(95, 16),new Point(96, 16),new Point(97, 17),new Point(97, 17),new Point(98, 18),
				new Point(98, 18),new Point(99, 19),new Point(99, 20),new Point(100, 20),new Point(100, 21),new Point(101, 21),new Point(101, 22),new Point(102, 23),new Point(102, 23),new Point(103, 24),
				new Point(103, 25),new Point(104, 25),new Point(104, 26),new Point(104, 27),new Point(105, 27),new Point(105, 28),new Point(106, 29),new Point(106, 30),new Point(106, 30),new Point(107, 31),
				new Point(107, 32),new Point(107, 33),new Point(108, 33),new Point(108, 34),new Point(108, 35),new Point(108, 36),new Point(109, 36),new Point(109, 37),new Point(109, 38),new Point(109, 39),
				new Point(110, 39),new Point(110, 39),new Point(110, 40),new Point(110, 41),new Point(110, 42),new Point(110, 43),new Point(110, 43),new Point(110, 44),new Point(110, 45),new Point(110, 46),
				new Point(110, 47),new Point(110, 48),new Point(110, 48),new Point(110, 49),new Point(110, 50),new Point(110, 51),new Point(110, 52),new Point(110, 53),new Point(110, 54),new Point(110, 54),
				new Point(110, 55),new Point(110, 56),new Point(109, 57),new Point(109, 58),new Point(109, 59),new Point(109, 60),new Point(108, 60),new Point(108, 61),new Point(108, 62),new Point(107, 63),
				new Point(107, 64),new Point(107, 65),new Point(106, 66),new Point(106, 66),new Point(106, 67),new Point(105, 68),new Point(105, 69),new Point(104, 70),new Point(104, 71),new Point(103, 71),
				new Point(103, 72),new Point(102, 73),new Point(101, 74),new Point(101, 75),new Point(100, 75),new Point(100, 76),new Point(99, 77),new Point(98, 78),new Point(98, 78),new Point(97, 79),
				new Point(96, 80),new Point(95, 80),new Point(95, 81),new Point(94, 82),new Point(93, 82),new Point(92, 83),new Point(91, 84),new Point(90, 84),new Point(90, 85),new Point(89, 86),
				new Point(88, 86),new Point(87, 87),new Point(86, 87),new Point(85, 88),new Point(84, 88),new Point(83, 89),new Point(82, 89),new Point(81, 90),new Point(80, 90),new Point(79, 91),
				new Point(78, 91),new Point(77, 91),new Point(76, 92),new Point(75, 92),new Point(74, 93),new Point(73, 93),new Point(71, 93),new Point(70, 93),new Point(69, 94),new Point(68, 94),
				new Point(67, 94),new Point(66, 94),new Point(65, 95),new Point(64, 95),new Point(62, 95),new Point(61, 95),new Point(60, 95),new Point(59, 95),new Point(58, 95),new Point(57, 95),
				new Point(56, 95),new Point(55, 95),new Point(54, 95),new Point(53, 95),new Point(52, 95),new Point(51, 95),new Point(50, 95),new Point(48, 95),new Point(47, 95),new Point(46, 94),
				new Point(45, 94),new Point(44, 94),new Point(43, 94),new Point(42, 93),new Point(41, 93),new Point(39, 93),new Point(38, 93),new Point(37, 92),new Point(36, 92),new Point(35, 91),
				new Point(34, 91),new Point(33, 91),new Point(32, 90),new Point(31, 90),new Point(30, 89),new Point(29, 89),new Point(28, 88),new Point(27, 88),new Point(26, 87),new Point(25, 87),
				new Point(24, 86),new Point(23, 86),new Point(22, 85),new Point(22, 84),new Point(21, 84),new Point(20, 83),new Point(19, 82),new Point(18, 82),new Point(17, 81),new Point(17, 80),
				new Point(16, 80),new Point(15, 79),new Point(14, 78),new Point(14, 78),new Point(13, 77),new Point(12, 76),new Point(12, 75),new Point(11, 75),new Point(11, 74),new Point(10, 73),
				new Point(9, 72),new Point(9, 71),new Point(8, 71),new Point(8, 70),new Point(7, 69),new Point(7, 68),new Point(6, 67),new Point(6, 66),new Point(6, 66),new Point(5, 65),
				new Point(5, 64),new Point(5, 63),new Point(4, 62),new Point(4, 61),new Point(4, 60),new Point(3, 60),new Point(3, 59),new Point(3, 58),new Point(3, 57),new Point(2, 56),
				new Point(2, 55),new Point(2, 54),new Point(2, 54),new Point(2, 53),new Point(2, 52),new Point(2, 51),new Point(2, 50),new Point(2, 49),new Point(2, 48),new Point(2, 48),
				new Point(2, 47),new Point(2, 46),new Point(2, 45),new Point(2, 44),new Point(2, 43),new Point(2, 43),new Point(2, 42),new Point(2, 41),new Point(2, 40),new Point(2, 39),
				new Point(2, 39),new Point(3, 39),new Point(3, 38),new Point(3, 37),new Point(3, 36),new Point(4, 36),new Point(4, 35),new Point(4, 34),new Point(4, 33),new Point(5, 33),
				new Point(5, 32),new Point(5, 31),new Point(6, 30),new Point(6, 30),new Point(6, 29),new Point(7, 28),new Point(7, 27),new Point(8, 27),new Point(8, 26),new Point(8, 25),
				new Point(9, 25),new Point(9, 24),new Point(10, 23),new Point(10, 23),new Point(11, 22),new Point(11, 21),new Point(12, 21),new Point(12, 20),new Point(13, 20),new Point(13, 19),
				new Point(14, 18),new Point(14, 18),new Point(15, 17),new Point(15, 17),new Point(16, 16),new Point(17, 16),new Point(17, 15),new Point(18, 15),new Point(18, 14),new Point(19, 14),
				new Point(20, 13),new Point(20, 13),new Point(21, 12),new Point(21, 12),new Point(22, 11),new Point(23, 11),new Point(23, 10),new Point(24, 10),new Point(25, 9),new Point(25, 9),
				new Point(26, 9),new Point(27, 8),new Point(28, 8),new Point(28, 8),new Point(29, 7),new Point(30, 7),new Point(30, 6),new Point(31, 6),new Point(32, 6),new Point(33, 6),
				new Point(33, 5),new Point(34, 5),new Point(35, 5),new Point(36, 4),new Point(36, 4),new Point(37, 4),new Point(38, 4),new Point(39, 3),new Point(39, 3),new Point(40, 3),
				new Point(41, 3),new Point(42, 3),new Point(42, 2),new Point(43, 2),new Point(44, 2),new Point(45, 2),new Point(45, 2),new Point(46, 2),new Point(47, 1),new Point(48, 1),
				new Point(49, 1),new Point(49, 1),new Point(50, 1),new Point(51, 1),new Point(52, 1),new Point(53, 1),new Point(53, 1),new Point(54, 1),new Point(55, 1),new Point(56, 1),
			};
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
		 角度からベクトルを得る
		 angleはdegree
		 北が0で時計回り
		---------------------------------------------------------------------------*/
		static public PointF AngleToVector(float angle)
		{
			if(angle < 0)	return new PointF(0, 0);

			// vector
			PointF	vector	= new PointF();
			angle		= Useful.ToRadian(angle - 90f);
			vector.X	= (float)Math.Cos(angle);
			vector.Y	= (float)Math.Sin(angle);
			return vector;
		}
	}
}
