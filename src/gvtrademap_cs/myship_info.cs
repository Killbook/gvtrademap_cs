/*-------------------------------------------------------------------------

 自分の船情報
 情報取得はLocalPCもしくはナビゲーションクライアントから行う
 日付しか得られないときの位置予測表示付き

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

using directx;
using gvo_base;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Utility;
using win32;

using net_base;
using gvo_net_base;
using System.Drawing;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public class myship_info
	{
		// ナビゲーションクライアントからの受信タイムアウト
		// タイムアウト時はコンパスの角度を初期化する
		// 航路共有時は海上ではない扱いとなる
		private const int			FROM_CLIENT_RECEIVE_TIME_OUT	= 1000*5;	// 5秒
		// 海上ではないと判断する時間
		// この時間日付がキャプチャできなければ海上ではないと判断する
		// 到達予想位置のリセットも絡むため、少し長めに設定する
		private const int			OUT_OF_SEA_TIME_OUT				= 1000*5;	// 5秒
		// 到達予想位置
		private const float			STEP_POSITION_SPEED_MIN			= 1;		// なにも表示しない速度
		private const float			STEP_POSITION_SPEED_MIN1		= 2.8f;		// 5日間隔での表示になる速度
		private const int			STEP_POSITION_DAYS_MAX			= 50;		// 50日分まで表示
		// 進行方向線の長さ
		private const float			ANGLE_LINE_LENGTH				= 3000;

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		private gvt_lib				m_lib;					// 
		private GvoDatabase			m_db;					// 

	
		private Point				m_pos;					// 自分の船位置
															// 航路共有時にこの値を渡す
		private float				m_angle;				// 自分の船の向き
		private bool				m_is_in_the_sea;		// 海上のときtrue
//		private float				m_show_speed;			// 到達予想表示アニメーション用速度

		private gvo_server_service	m_server_service;		// ナビゲーションクライアントからの受信

		private DateTimer			m_capture_timer;		// キャプチャ間隔用
		private int					m_capture_interval;		// キャプチャ間隔
		private DateTimer			m_expect_pos_timer;		// 予想位置計算用
		private DateTimer			m_expect_delay_timer;	// 予想位置消去用ディレイタイマ

		private Vector2				m_expect_vector;		// 予想位置用角度
		private bool				m_is_draw_expect_pos;	// 予想位置を描画するときtrue

		private bool				m_capture_sucess;		// 測量位置を得たときtrue
															// UpdateDomains()を呼ぶ毎にfalseにされ、得られたときのみtrueになる
	
		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public Point	pos							{	get{	return m_pos;					}}
		public float	angle						{	get{	return m_angle;					}}
		public gvo_server_service	server_service	{	get{	return m_server_service;		}}
		public bool		is_in_the_sea				{	get{	return m_is_in_the_sea;			}}
		public bool		is_analized_pos
		{
			get{
				if(m_pos.X < 0)		return false;
				if(m_pos.Y < 0)		return false;
				return true;
			}
		}
		private bool is_draw_expect_pos
		{
			get{
				if(!m_is_draw_expect_pos)				return false;
				if(m_expect_vector == Vector2.Empty)	return false;
				return true;
			}
		}
		public bool capture_sucess					{	get{	return m_capture_sucess;		}}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public myship_info(gvt_lib lib, GvoDatabase db)
		{
			m_lib					= lib;
			m_db					= db;
	
			m_pos					= new Point(-1, -1);
			m_angle					= -1;
			m_is_in_the_sea			= false;
			// 到達予想アニメーション用速度
//			m_show_speed			= 0;

			m_server_service		= new gvo_server_service();		// ナビゲーションクライアントからの受信
			m_capture_timer			= new DateTimer();				// キャプチャ間隔用
			m_expect_pos_timer		= new DateTimer();				// 予想位置計算用
			m_expect_delay_timer	= new DateTimer();				// 予想位置消去用ディレイタイマ

			m_capture_sucess		= false;

			// 到達予想位置をリセット
			reset_expect();
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		private void reset_expect()
		{
			m_expect_vector			= Vector2.Empty;
			m_is_draw_expect_pos	= false;
			m_expect_pos_timer.StartSection();
		}

		/*-------------------------------------------------------------------------
		 更新
		---------------------------------------------------------------------------*/
		public void Update()
		{
			m_capture_sucess	= false;
	
			// サーバ更新
			update_server();
	
			// キャプチャ間隔の更新
			update_capture_interval();

			// 情報の更新
			if(m_lib.setting.is_server_mode){
				// ナビゲーションクライアントからの受信
				do_receive_client();
			}else{
				// 画面キャプチャ
				do_capture();
				// ログからの海域変動更新
				m_db.GvoChat.UpdateSeaArea_DoRequest();
			}
		}

		/*-------------------------------------------------------------------------
		 サーバ更新
		---------------------------------------------------------------------------*/
		private void update_server()
		{
			if(m_lib.setting.is_server_mode){
				// サーバモード
				// 起動していなければ起動する
				if(!m_server_service.is_listening){
					m_server_service.Listen(m_lib.setting.port_index);
				}
			}else{
				// 通常モード
				// 起動していれば終了する
				m_server_service.Close();
			}
		}

		/*-------------------------------------------------------------------------
		 画面キャプチャ間隔を更新する
		---------------------------------------------------------------------------*/
		private void update_capture_interval()
		{
			switch(m_lib.setting.capture_interval){
			case CaptureIntervalIndex.Per250ms:
				m_capture_interval			= 250;
				break;
			case CaptureIntervalIndex.Per500ms:
				m_capture_interval			= 500;
				break;
			case CaptureIntervalIndex.Per2000ms:
				m_capture_interval			= 2000;
				break;
			case CaptureIntervalIndex.Per1000ms:
			default:
				m_capture_interval			= 1000;
				break;
			}
		}

		/*-------------------------------------------------------------------------
		 キャプチャ
		---------------------------------------------------------------------------*/
		private void do_capture()
		{
			// 前回のキャプチャからの経過時間を得る
			int	sectiontime	= m_capture_timer.GetSectionTimeMilliseconds();
			if(sectiontime < m_capture_interval){
				// キャプチャ間隔ではない
				return;
			}
			// 間隔測定リセット
			m_capture_timer.StartSection();

			// 情報更新
			update_myship_data(sectiontime, get_myship_data());
		}

		/*-------------------------------------------------------------------------
		 自分の船の情報を得る
		 キャプチャ関係の情報
		 ある時点でのスナップショットとなる
		---------------------------------------------------------------------------*/
		private gvo_analized_data get_myship_data()
		{
			// 航路記録なしならなにもしない
			if(!m_lib.setting.save_searoutes)	return null;
	
			// キャプチャ方法の指定
			if(m_lib.setting.windows_vista_aero)	m_db.Capture.capture_mode = gvo_capture.mode.vista;
			else									m_db.Capture.capture_mode = gvo_capture.mode.xp;

			// 画面キャプチャ
			// 日数、測量座標、コンパスの角度
			m_db.Capture.CaptureAll();

			// 情報を構築して返す
			return gvo_analized_data.FromAnalizedData(m_db.Capture, m_db.GvoChat);
		}

		/*-------------------------------------------------------------------------
		 クライアントからの受信チェック
		---------------------------------------------------------------------------*/
		private void do_receive_client()
		{
			// キャプチャ情報を受信したか調べる
			gvo_analized_data	data	= get_received_capture_data();

			if(data == null){
				if(m_capture_timer.GetSectionTimeMilliseconds() >= FROM_CLIENT_RECEIVE_TIME_OUT){
					// 受信タイムアウト
					// 海上に居ないと判断する
					m_is_in_the_sea	= false;
					// 到達予想をリセット
					reset_expect();
				}
				return;
			}

			// 前回からの経過時間を得る
			int		sectiontime		= m_capture_timer.GetSectionTimeMilliseconds();
			// 間隔測定リセット
			m_capture_timer.StartSection();

			// 情報更新
			update_myship_data(sectiontime, data);
		}
	
		/*-------------------------------------------------------------------------
		 受信情報を得る
		 キャプチャ関係の情報
		 海域変動はこのメソッド内で更新してしまう
		---------------------------------------------------------------------------*/
		private gvo_analized_data get_received_capture_data()
		{
			gvo_tcp_client	client		= m_server_service.GetClient();
			if(client == null)		return null;

			// 海域変動
			gvo_map_cs_chat_base.sea_area_type[]	sea_info	= client.sea_info;		// コピーを得る
			if(sea_info != null){
				foreach(gvo_map_cs_chat_base.sea_area_type i in sea_info){
					switch(i.type){
					case gvo_map_cs_chat_base.sea_type.safty:
						m_db.SeaArea.SetType(i.name, sea_area.sea_area_once.sea_type.safty);
						break;
					case gvo_map_cs_chat_base.sea_type.lawless:
						m_db.SeaArea.SetType(i.name, sea_area.sea_area_once.sea_type.lawless);
						break;
					default:
						m_db.SeaArea.SetType(i.name, sea_area.sea_area_once.sea_type.normal);
						break;
					}
				}
			}

			gvo_analized_data	data	= client.capture_data;	// コピーを得る
			// キャプチャ情報を受信していれば情報を返す
			if(data.capture_days_success || data.capture_success){
				return data;
			}
			// 情報を受信していない
			return null;
		}

		/*-------------------------------------------------------------------------
		 自分の船の情報更新
		 クライアントから受信した情報か自ら得た情報かは考慮されない
		---------------------------------------------------------------------------*/
		private void update_myship_data(int sectiontime, gvo_analized_data data)
		{
			// 
			if(data == null)	return;

			// 航路記録なしならなにもしない
			if(!m_lib.setting.save_searoutes){
				// 位置と角度は初期化する
				m_pos				= new Point(-1, -1);
				m_is_in_the_sea		= false;
				// 到達予想をリセット
				reset_expect();
				return;
			}
	
			// 経過時間だけ追加
			m_db.SpeedCalculator.AddIntervalOnly(sectiontime);

			// 造船開始と終了
			// 同時にフラグが立った場合は終了を優先する
			if(data.is_start_build_ship){
				m_db.BuildShipCounter.StartBuildShip(data.build_ship_name);
			}
			if(data.is_finish_build_ship){
				m_db.BuildShipCounter.FinishBuildShip();
			}
			
			// 日付キャプチャ成功なら利息からの日数を更新する
			if(data.capture_days_success){
				// 利息からの日数を更新する
				m_db.InterestDays.Update(data.days, data.interest);
				// 造船日数を更新する
				m_db.BuildShipCounter.Update(data.days);
			}else{
				if(m_expect_delay_timer.GetSectionTimeMilliseconds() > OUT_OF_SEA_TIME_OUT){
					// 海上ではない
					m_is_in_the_sea			= false;
					// 到達予想をリセット
					reset_expect();
				}
				// 日付キャプチャ失敗ならそれ以外の解析を行わない
				return;
			}

			// キャプチャが成功したかチェック
			if(!data.capture_success){
				// 日付のみキャプチャ成功
				m_is_draw_expect_pos	= true;		// 到達予想位置を描画する必要あり
				// ディレイタイマリセット
				m_expect_delay_timer.StartSection();
				return;
			}

			// 自分の船の位置
			m_pos				= new Point(data.pos_x, data.pos_y);
			m_angle				= data.angle;
			m_is_in_the_sea		= true;		// 海上
			m_capture_sucess	= true;		// キャプチャ成功

			// 測量位置を追加する
			// 位置によっては追加されない
			m_db.SeaRoute.AddPoint(	m_pos,
									data.days,
									gvo_chat.ToIndex(data.accident));

			// 速度算出
			// 更新間隔はすでに設定済み
			m_db.SpeedCalculator.Add(m_pos, 0);

			// キャプチャ時は描画をスキップしない
			m_lib.device.SetMustDrawFlag();

			// 到達位置予想用角度の更新
			m_expect_vector			= transform.ToVector2(gvo_capture.AngleToVector(m_angle));
			// タイマリセット
			m_expect_pos_timer.StartSection();
			m_expect_delay_timer.StartSection();
			m_is_draw_expect_pos	= false;	// 解析できてるので予想位置を描画する必要がない
		}

		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		public void Draw()
		{
			if(!is_analized_pos)	return;

			m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(draw_myship_proc), ANGLE_LINE_LENGTH);
		}

		/*-------------------------------------------------------------------------
		 自分の船を描く
		---------------------------------------------------------------------------*/
		private void draw_myship_proc(Vector2 offset, LoopXImage image)
		{
			// 地図座標に変換
			Vector2 pos		= image.GlobalPos2LocalPos(transform.game_pos2_map_pos(transform.ToVector2(m_pos), m_lib.loop_image), offset);

			// 角度
			draw_angle_line_all(pos, image);

			m_lib.device.sprites.BeginDrawSprites(m_lib.icons.texture);
			// 到達予想位置
			int color	= -1;
			if(m_lib.setting.draw_setting_myship_expect_pos){
				if(draw_expect_pos(pos, m_db.SpeedCalculator.speed_map)){
					color	= Color.FromArgb(160, 255, 255, 255).ToArgb();
				}
			}
			// 自分の船位置
			m_lib.device.sprites.AddDrawSpritesNC(new Vector3(pos.X, pos.Y, 0.3f+0.001f), m_lib.icons.GetIcon(icons.icon_index.myship), color);
			m_lib.device.sprites.EndDrawSprites();
		}

		/*-------------------------------------------------------------------------
		 角度線を描画する
		---------------------------------------------------------------------------*/
		private void draw_angle_line_all(Vector2 pos, LoopXImage image)
		{
			// 
			if(!m_lib.setting.draw_myship_angle)	return;
	
			DrawSettingMyShipAngle	flag	= m_lib.setting.draw_setting_myship_angle;

			if((flag & DrawSettingMyShipAngle.draw_1) != 0){
				// 測量から
				// 精度が高いほど不透明で描画される
				// 精度が低いときはほとんど見えない
				int		alpha	= (int)((255f * (m_db.SpeedCalculator.angle_precision * m_db.SpeedCalculator.angle_precision)));
				draw_angle_line(pos, image, m_db.SpeedCalculator.angle, Color.FromArgb(alpha, 0, 0, 255));
			}
			if(   (is_in_the_sea)
				&&((flag & DrawSettingMyShipAngle.draw_0) != 0) ){
				// コンパスから
				Color	color	= (is_draw_expect_pos)? Color.Tomato: Color.Black;
				draw_angle_line(pos, image, m_angle, color);

				// 速度から求めた到達予想位置
				if(m_lib.setting.draw_setting_myship_angle_with_speed_pos){
					draw_step_position2(pos,
										image,
										m_angle,
										m_db.SpeedCalculator.speed_map);
				}
			}
		}

		/*-------------------------------------------------------------------------
		 現在の位置から現在の速度で進んだ場合の到達位置の描画
		 矢印っぽい感じ
		---------------------------------------------------------------------------*/
		private void draw_step_position2(Vector2 pos, LoopXImage image, float angle, float speed)
		{
			if(angle < 0)									return;
			float	speed_knot	= speed_calculator.MapToKnotSpeed(speed);	// knotに変換
			if(speed_knot < STEP_POSITION_SPEED_MIN)		return;		// 最低速度

			// 表示用の速度を求める
			speed	= transform_speed(speed, image);

			m_lib.device.device.RenderState.ZBufferEnable	= false;

			m_lib.device.line.Width			= 1;
			m_lib.device.line.Antialias		= m_lib.setting.enable_line_antialias;
			m_lib.device.line.Pattern		= -1;
			m_lib.device.line.PatternScale	= 1.0f;
			m_lib.device.line.Begin();
	
			float		length	= 0;
			int			count	= 1;
			int			days	= 0;
			int			days2	= 0;
			Vector2		vec		= transform.ToVector2(gvo_capture.AngleToVector(angle));
			Vector2		vec2	= new Vector2(vec.Y, -vec.X);	// 90度回転させたベクトル
			Vector2[]	points	= new Vector2[3];
			float		max		= ANGLE_LINE_LENGTH * image.ImageScale;
			while(length < max){
				Vector2		step	= vec * (speed * count);
				Vector2		step2	= vec * ((speed * count) - 4.5f);
				Color		color	= Color.Black;
				float		l		= 2.5f;

				if(++days >= 5){
					// 5日区切りで色を変える
					if(++days2 >= 2){
						// 10日区切り
						color		= Color.Red;
						days2		= 0;
						l			= 4;
					}else{
						color		= Color.Tomato;
						l			= 3;
					}
					days			= 0;

					points[0]	= pos + step2 + (vec2 *  l);
					points[1]	= pos + step;
					points[2]	= pos + step2 + (vec2 * -l);
					m_lib.device.line.Draw(points, color);
				}else{
					// 1日間隔
					// 速度によっては表示されない
					if(speed_knot > STEP_POSITION_SPEED_MIN1){
						points[0]	= pos + step2 + (vec2 *  l);
						points[1]	= pos + step;
						points[2]	= pos + step2 + (vec2 * -l);
						m_lib.device.line.Draw(points, color);
					}
				}

				length	+= speed;
				// 日数制限
				if(++count > STEP_POSITION_DAYS_MAX)	break;
			}
			m_lib.device.line.End();
			//

			m_lib.device.device.RenderState.ZBufferEnable	= true;
		}

		/*-------------------------------------------------------------------------
		 速度を1日に進んだ距離に変換する
		 地図の拡縮が考慮される
		---------------------------------------------------------------------------*/
		private float transform_speed(float speed, LoopXImage image)
		{
			speed	= update_step_position_speed(speed);
			// マップ座標系に変換
			// 測量座標の縦横比と地図座標の縦横比が異なる場合おかしくなるので注意
			speed	*= transform.get_rate_to_map_x(image);
			// 表示スケールを考慮
			speed	*= image.ImageScale;

			return speed;
		}
	
		/*-------------------------------------------------------------------------
		 表示用速度
		 速度と表示用速度の差が大きいほど大きく速度に近づき
		 差が小さいときはゆっくり速度に近づく
		---------------------------------------------------------------------------*/
		private float update_step_position_speed(float speed_map)
		{
			speed_map	*= 60;										// 1分間に進める距離
//
			return speed_map;
//
/*
			float	gap	= Math.Abs(speed_map - m_show_speed);

			gap	*= gap;						// ギャップが大きい程速く近づく
			if(gap > 300)	gap	= 300;		// 最大ギャップ
			gap		*= 1f/(30);				// 
			if(gap < 0.4f)	gap	= 0.4f;		// 到達速度が遅すぎる場合の調整する

			if(speed_map > m_show_speed){
				m_show_speed	+= gap;
				if(m_show_speed > speed_map)	m_show_speed = speed_map;
			}else{
				m_show_speed	-= gap;
				if(m_show_speed < speed_map)	m_show_speed = speed_map;
			}
			return m_show_speed;
*/
		}

		/*-------------------------------------------------------------------------
		 進行方向の描画
		---------------------------------------------------------------------------*/
		private void draw_angle_line(Vector2 pos, LoopXImage image, float angle, Color color)
		{
			if(angle < 0)		return;

			m_lib.device.device.RenderState.ZBufferEnable	= false;

			m_lib.device.line.Width			= 1;
			m_lib.device.line.Antialias		= m_lib.setting.enable_line_antialias;
			m_lib.device.line.Pattern		= -1;
			m_lib.device.line.PatternScale	= 1.0f;
			m_lib.device.line.Begin();
	
			Vector2		vec		= transform.ToVector2(gvo_capture.AngleToVector(angle));
			Vector2[]	points	= new Vector2[2];
			points[0]			= pos;
			points[1]			= pos + (vec * (ANGLE_LINE_LENGTH * image.ImageScale));
			m_lib.device.line.Draw(points, color);

			m_lib.device.line.End();
			m_lib.device.device.RenderState.ZBufferEnable	= true;
		}

		/*-------------------------------------------------------------------------
		 到達予想位置
		 到達予想位置を描画したときtrueを返す
		---------------------------------------------------------------------------*/
		private bool draw_expect_pos(Vector2 pos, float speed)
		{
			// 表示する必要があるかチェック
			if(!is_draw_expect_pos)				return false;
			// 最低速度
			if(speed_calculator.MapToKnotSpeed(speed) < STEP_POSITION_SPEED_MIN)	return false;	

			// 経過時間
			int	sectiontime		= m_expect_pos_timer.GetSectionTimeMilliseconds();

			// 表示用の速度を求める
			speed	= transform_speed(speed, m_lib.loop_image);
			speed	*= 1f/60;					// 1秒での距離に変換
			speed	*= 1f/1000;					// sectiontime に合わせる(1sec=1000)
			speed	*= sectiontime;				// 経過時間分距離を進める
			pos		+= m_expect_vector * speed;	// 移動ベクトルを更新

			// 予想位置描画
			m_lib.device.sprites.AddDrawSpritesNC(new Vector3(pos.X, pos.Y, 0.3f), m_lib.icons.GetIcon(icons.icon_index.myship));
			return true;
		}
	}
}
