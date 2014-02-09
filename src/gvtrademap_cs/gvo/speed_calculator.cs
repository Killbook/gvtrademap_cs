/*-------------------------------------------------------------------------

 速度計算
 進行方向角度計算
 進行方向は測量の位置から得る

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.DirectX;
using Utility;


/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public class speed_calculator
	{
		// knot -> km 変換レート
		private const float						KM_KNOT_RATE	= 1.852f;
		// map -> knot 変換レート
		// 速度計算
		// from
		// 0.99版の速度計算とあっているかどうかは確認していませんが、自分で確認したところまで…
		// 1.赤道上の一周=40075.1km
		// 2.測量の数字では一周=16384point(と仮定)
		// 3.リアルで1s=ゲームの0.4h
		// 4.リアルで1s間ゲームの中で1ktで進むと(1.852km/h*0.4h=0.7408km)
		// 5.0.7408kmは測量で0.3029point
		// 1kt=0.3029point/real_second
		// たとえばリアル1s間6point動いたら (6/0.3029)*(0.3029point/1real_second)=約20kt
		private const float						MAP_KNOT_RATE	= 0.3029f;
	
		/*-------------------------------------------------------------------------
		 計算用
		---------------------------------------------------------------------------*/
		class data
		{
			private float						m_length;		// 移動距離
			private Point						m_pos;			// 位置
			private float						m_interval;		// サンプリング間隔

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public float length				{	get{	return m_length;		}}
			public Point position			{	get{	return m_pos;			}}
			public float interval			{	get{	return m_interval;		}}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public data(float length, float interval)
			{
				m_length		= length;
				m_pos			= new Point(0, 0);
				m_interval		= interval;
			}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public data(Point pos, float interval)
			{
				m_length		= 0;
				m_pos			= pos;
				m_interval		= interval;
			}
		}
		
		// 保持する最大間隔
		// 少なくとも計算用の間隔よりも大きいこと
		private const int						LIST_INTERVAL_MAX			= 120*1000;
//		// 速度計算用最低間隔
//		private const int						CALC_INTERVAL_MIN			= 6*1000;
		// 速度計算用最大間隔
		private const int						CALC_INTERVAL_MAX			= 20*1000;
//		// 最低間隔と最大間隔での速度差がこの値未満なら最大間隔の速度を採用する
//		private const float						CALC_INTERVAL_GAP_RATE_MAX	= 0.05f;

		// 角度計算時の最低計算間隔
		// あまり間隔が狭いと誤差が大きすぎて使い物にならない
		private const int						CALC_ANGLE_MIN_INTERVAL		= 20*1000;
		private const int						CALC_ANGLE_STEP_INTERVAL	= 5*1000;
		// 角度計算時の最高計算間隔
		// 間隔が広いほど誤差が少なくなるが、リアルタイム性が落ちる
		private const int						CALC_ANGLE_MAX_INTERVAL		= 60*1000;
		// 角度計算時の精度を求めるためのもの
		private const int						CALC_ANGLE_MAX_PRECISION_COUNTS	= 
			((CALC_ANGLE_MAX_INTERVAL-CALC_ANGLE_MIN_INTERVAL)/CALC_ANGLE_STEP_INTERVAL) +1;

		// 最大角度差(degree)
		// 角度差のため+-で指定する倍の範囲が有効
		private const float						ANGLE_GAP_MAX				= 2;

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		private List<data>						m_length_list;		// 速度計算用位置リスト
		private List<data>						m_angle_list;		// 角度計算用リスト
		private int								m_map_size_x;		// X方向のループサイズ(測量座標系)

		private float							m_interval;			// 追加インターバル
		private Point							m_old_pos;			// 前回の位置
	
		private float							m_speed;			// 速度(測量座標系)
		private float							m_angle;			// 角度

		private float							m_angle_gap_cos;	// 角度差用cos(θ)
		private float							m_angle_precision;	// 角度の精度
																	// 0～1

		private bool							m_req_reset_angle;	// 角度計算のリセット

//		private bool							m_is_speed2;

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		// 測量座標系での速度
		public float speed_map			{		get{	return m_speed;					}}
		// ノット
		public float speed_knot			{		get{	return MapToKnotSpeed(m_speed);	}}
		// Km/h
		public float speed_km			{		get{	return MapToKmSpeed(m_speed);	}}
		// 角度(degree)
		public float angle				{		get{	return m_angle;					}}
		// 角度の精度(0～1)
		public float angle_precision	{		get{	return m_angle_precision;		}}

//		public bool is_speed2			{		get{	return m_is_speed2;				}}

	
		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public speed_calculator(int map_size_x)
		{
			m_map_size_x		= map_size_x;
			m_length_list		= new List<data>();
			m_angle_list		= new List<data>();

			m_interval			= 0;
			m_old_pos			= new Point(0, 0);

			m_angle				= -1;

			m_angle_gap_cos		= (float)Math.Cos(Useful.ToRadian(ANGLE_GAP_MAX));
			m_angle_precision	= 0;

			m_req_reset_angle	= false;

			m_speed				= 0;
		}
	
		/*-------------------------------------------------------------------------
		 位置を追加
		 intervalはミリ秒指定
		 1000 = 1秒
		---------------------------------------------------------------------------*/
		public void Add(Point pos, int interval)
		{
			// 角度計算リセット
			if(m_req_reset_angle){
				m_angle_list.Clear();
				m_angle				= -1;
				m_angle_precision	= 0;
				m_req_reset_angle	= false;
			}
	
			// 間隔
			m_interval		+= interval;

			// 2秒未満の更新時はリストを更新しない
			if(m_interval < 2000){
				return;
			}
			
			// 速度
			SeaRoutes.SeaRoutePoint	p1	= new SeaRoutes.SeaRoutePoint(transform.ToVector2(m_old_pos));
			SeaRoutes.SeaRoutePoint	p2	= new SeaRoutes.SeaRoutePoint(transform.ToVector2(pos));
			float				l	= p1.Length(p2, m_map_size_x);		// 距離
			if(l < 100){
				// 大きすぎる距離は無視する
				m_length_list.Add(new data(l, m_interval));
			}

			// 角度	
			m_angle_list.Add(new data(pos, m_interval));

			// リストサイズを調整する
			ajust_list_size(m_length_list);
			ajust_list_size(m_angle_list);
	
			// 
			m_interval	= 0;	// 更新間隔初期化
			m_old_pos	= pos;	// 位置更新

			// 速度を計算する
			clac_speed();
			// 角度を計算する
			calc_angle();
		}

		/*-------------------------------------------------------------------------
		 データサイズを調整する
		---------------------------------------------------------------------------*/
		private void ajust_list_size(List<data> list)
		{
			while(true){
				float	tmp		= 0;
				foreach(data s in list){
					tmp	+= s.interval;
				}
				if(tmp > LIST_INTERVAL_MAX)		list.RemoveAt(0);
				else							break;
			}
		}

		/*-------------------------------------------------------------------------
		 間隔のみ追加
		 測量のキャプチャに失敗したとき用
		---------------------------------------------------------------------------*/
		public void AddIntervalOnly(int interval)
		{
			m_interval	+= interval;
		}

		/*-------------------------------------------------------------------------
		 速度を計算する
		---------------------------------------------------------------------------*/
		private void clac_speed()
		{
			m_speed			= calc_speed_sub(CALC_INTERVAL_MAX);

/*
			float	speed1	= calc_speed_sub(CALC_INTERVAL_MIN);
			float	speed2	= calc_speed_sub(CALC_INTERVAL_MAX);

			// 速度差によりどちらを使うかを決める
			float	tmp		= Math.Abs(speed1 - speed2);
			float	tmp2	= speed1 * CALC_INTERVAL_GAP_RATE_MAX;
			// 速度差がCALC_INTERVAL_GAP_RATE_MAX未満ならより間隔の長いのもを使用する
			if(tmp < tmp2){
				// より安定する速度
				m_speed		= speed2;
				m_is_speed2	= true;
			}else{
				// 方向転換等でリアルタイム性が高いことが望まれる速度
				m_speed		= speed1;
				m_is_speed2	= false;
			}
*/
/*	
			float	l			= 0;
			float	interval	= 0;

			// 距離と計測間隔の合計を出す
			// CALC_INTERVAL_MAX分を最大とする
			for(int i=m_length_list.Count-1; i>=0; i--){
				l			+= m_length_list[i].length;
				interval	+= m_length_list[i].interval;
				if(interval >= CALC_INTERVAL_MAX){
					// 安定する速度が得られる時間が得られた
					break;
				}
			}

			// 安定する速度が得られるサンプル数でなくても
			// 速度は求めておく

			// 1秒に進んだ距離
			// 測量座標系
			m_speed		= l / (interval / 1000);
*/
		}

		/*-------------------------------------------------------------------------
		 移動距離を得る
		 指定された間隔分の速度を得る
		---------------------------------------------------------------------------*/
		private float calc_speed_sub(int calc_interval)
		{
			float	length		= 0;
			float	interval	= 0;

			// 距離と計測間隔の合計を出す
			// calc_interval分を最大とする
			for(int i=m_length_list.Count-1; i>=0; i--){
				length		+= m_length_list[i].length;
				interval	+= m_length_list[i].interval;
				if(interval >= calc_interval){
					break;
				}
			}
			// 1秒に進んだ距離
			// 測量座標系
			return length / (interval / 1000);
		}
	
		/*-------------------------------------------------------------------------
		 測量 -> knot
		---------------------------------------------------------------------------*/
		public static float MapToKnotSpeed(float speed_map)
		{
			float knot	= speed_map * (1f / MAP_KNOT_RATE);

			// 速度が大きすぎる場合は0を返す
			if(knot < 100f)	return knot;
			return 0;
		}
	
		/*-------------------------------------------------------------------------
		 knot -> 測量
		---------------------------------------------------------------------------*/
		public static float KnotToMapSpeed(float knot)
		{
			return knot * MAP_KNOT_RATE;
		}
	
		/*-------------------------------------------------------------------------
		 knot -> km
		---------------------------------------------------------------------------*/
		public static float KnotToKmSpeed(float knot)
		{
			return knot * KM_KNOT_RATE;
		}
		
		/*-------------------------------------------------------------------------
		 km -> knot
		---------------------------------------------------------------------------*/
		public static float KmToKnotSpeed(float km)
		{
			return km * (1f / KM_KNOT_RATE);
		}
		
		/*-------------------------------------------------------------------------
		 km -> 測量
		---------------------------------------------------------------------------*/
		public static float KmToMapSpeed(float km)
		{
			return KnotToMapSpeed(KmToKnotSpeed(km));
		}

		/*-------------------------------------------------------------------------
		 測量 -> km
		---------------------------------------------------------------------------*/
		public static float MapToKmSpeed(float speed_map)
		{
			return KnotToKmSpeed(MapToKnotSpeed(speed_map));
		}

		/*-------------------------------------------------------------------------
		 角度を計算する
		 ぶれは最大間隔時+-0.1度程度
		---------------------------------------------------------------------------*/
		private void calc_angle()
		{
			if(m_angle_list.Count <= 1)		return;		// 角度を計算できない
	
			// 基準点
			int		count			= m_angle_list.Count;
			Point	pos				= m_angle_list[count -1].position;
			float	interval		= m_angle_list[count -1].interval;

			List<Point>	pos_list	= new List<Point>();

			// 条件の間隔を満たす座標を捜す
			float	next_interval	= CALC_ANGLE_MIN_INTERVAL;
			for(int i=count-2; i>=0; i--){
				interval	+= m_angle_list[i].interval;
				if(interval >= next_interval){
					pos_list.Add(m_angle_list[i].position);

					next_interval	+= CALC_ANGLE_STEP_INTERVAL;
					if(next_interval > CALC_ANGLE_MAX_INTERVAL){
						break;
					}
				}
			}

			if(pos_list.Count <= 0)		return;		// 角度が求められるサンプルが集まっていない

			// 各移動ベクトルを得る
			List<Vector2>	v_list		= new List<Vector2>();
			foreach(Point p in pos_list){
				Vector2		v_tmp	= transform.SubVector_LoopX(pos, p, m_map_size_x);
				v_tmp.Normalize();				// 正規化
				if(v_tmp.LengthSq() >= 0.7f){
					// ベクトルの長さが短すぎた場合追加しない
					v_list.Add(v_tmp);
				}
			}
			
			if(v_list.Count <= 0){
				// 角度が求められない
				m_angle				= -1;
				m_angle_precision	= 0;
				return;
			}

			// 最低でも1つの角度が得られることが確定している
			Vector2		v		= v_list[0];		// 基準とする角度

			// 安定する角度を選択する
			// できるだけ広い間隔で得られた角度がよい
			int		precision	= 1;
			if(v_list.Count >= 2){
				Vector2	v2	= v;
				for(int i=1; i<v_list.Count; i++){
					// 角度差を得る
					float	cos	= Vector2.Dot(v, v_list[i]);
					// 角度差が大きすぎなら終了
					if(cos <= m_angle_gap_cos)		break;

					// 安定した角度
					v2		= v_list[i];

					precision++;
				}
				v	= v2;
			}

			// 進行方向精度
			m_angle_precision	= (float)precision / CALC_ANGLE_MAX_PRECISION_COUNTS;

			// 表示向け角度をデグリーで得る	
			// radian -> degree
			m_angle		= Useful.ToDegree((float)Math.Atan2(v.Y, v.X));
			// 東が0度になってるので、北を0度にする
			m_angle		+= 90;
			// -180 ～ +180
			//    0 ～  360 に調整する
			while(m_angle < 0)		m_angle		+= 360;
			while(m_angle >= 360)	m_angle		-= 360;
		}

		/*-------------------------------------------------------------------------
		 角度計算をリセットする
		---------------------------------------------------------------------------*/
		public void ResetAngle()
		{
			m_req_reset_angle	= true;
		}
	}
}
