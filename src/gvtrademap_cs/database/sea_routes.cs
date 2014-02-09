/*-------------------------------------------------------------------------

 航路図

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 define
---------------------------------------------------------------------------*/
//#define	DRAW_SEA_ROUTES_BOUNDINGBOX			// 航路図のバウンディングボックス描画
//#define	DRAW_POPUPS_BOUNDINGBOX				// ポップアップのバウンディングボックス描画
//#define	DISABLE_SEA_ROUTES_CHAIN_POINTS		// 航路図を出来るだけ繋げる処理無効

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using Utility;
using directx;
using System.Collections;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class SeaRoutes
	{	
		// 線とする距離の最大
		private const float			LINE_ROUTE_MAX					= 15*15;
		// 線として追加する最低距離
		private const float			ADD_POINT_MIN					= 7*7;
		// 離れすぎた場合に追加する角度(degree)
		private const float			ADD_POINT_ANGLE_GAP_MAX			= 16;
//		private const float			ADD_POINT_ANGLE_GAP_MAX			= 25;
		private const float			ADD_POINT_ANGLE_LENGTH_SQ_MAX	= 2000*2000;

		// スクリーンショットの左右の余裕
		private const int			SCREEN_SHOT_BOUNDING_BOX_GAP_X	= 64;
		// スクリーンショットの上下の余裕
		private const int			SCREEN_SHOT_BOUNDING_BOX_GAP_Y	= 64;

		// 最新の航路以外の半透明具合
		private const float			SEAROUTES_ALPHA					= 0.4f;

		// 1つの線とするバウンディングボックスのサイズ
		// サイズが大きすぎると描画で損するため、適当な大きさになったら線を分割する
		// 単位は地図座標系
		private const float			BB_SIZE_MAX						= 250;
		private const float			BB_POPUP_SIZE_MAX				= 350f;
		// 画面外判定時の余白
		private const float			BB_OUTSIDESCREEEN_OFFSET		= 32f;

		// スクリーンショットの分布チェック単位(dot)
		private const int			SS_DISTRIBUTION_X				= 64;
	
		/*-------------------------------------------------------------------------
		 航路座標
		---------------------------------------------------------------------------*/
		public class SeaRoutePoint
		{
			private Vector2				m_pos;				// 座標
			private int					m_color_index;		// 色番号
			private int					m_color;			// 描画色
															// 半透明値が0なので注意
			
			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public Vector2 Position		{	get{	return m_pos;			}
											set{	m_pos	= value;		}}
			public int ColorIndex		{	get{	return m_color_index;	}}
			public int Color			{	get{	return m_color;			}}

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public SeaRoutePoint(float x, float y, int color_index)
			{
				m_pos.X				= x;
				m_pos.Y				= y;
				m_color_index		= color_index;
				m_color				= DrawColor.GetColor(color_index);
			}
			public SeaRoutePoint(float x, float y)
			{
				m_pos.X				= x;
				m_pos.Y				= y;
				m_color_index		= 0;
				m_color				= DrawColor.GetColor(ColorIndex);
			}
			public SeaRoutePoint(Vector2 pos, int color_index)
			{
				m_pos				= pos;
				m_color_index		= color_index;
				m_color				= DrawColor.GetColor(color_index);
			}
			public SeaRoutePoint(Vector2 pos)
			{
				m_pos				= pos;
				m_color_index		= 0;
				m_color				= DrawColor.GetColor(ColorIndex);
			}
			public SeaRoutePoint(SeaRoutePoint p)
			{
				m_pos				= p.Position;
				m_color_index		= p.ColorIndex;
				m_color				= p.Color;
			}
			/*-------------------------------------------------------------------------
			 距離の2乗を返す
			---------------------------------------------------------------------------*/
			public float LengthSq(SeaRoutePoint p)
			{
				Vector2	l	= new Vector2(	p.Position.X - m_pos.X,
											p.Position.Y - m_pos.Y);
				return l.LengthSq();
			}
			/*-------------------------------------------------------------------------
			 距離を返す
			---------------------------------------------------------------------------*/
			public float Length(SeaRoutePoint p)
			{
				Vector2	l	= new Vector2(	p.Position.X - m_pos.X,
											p.Position.Y - m_pos.Y);
				return l.Length();
			}

			/*-------------------------------------------------------------------------
			 距離の2乗を返す
			 ループを考慮し、近いほうの距離を返す
			 近い位置を near_p に返す
			 near_p == p ならループを考慮しない位置が最短
			---------------------------------------------------------------------------*/
			public float LengthSq(SeaRoutePoint p, int size_x)
			{
				SeaRoutePoint	near	= null;
				return LengthSq(p, size_x, ref near);
			}
			public float LengthSq(SeaRoutePoint p, int size_x, ref SeaRoutePoint near_p)
			{
				float	l1	= LengthSq(p);
				SeaRoutePoint	p2	= new SeaRoutePoint(p);
				p2.build_loop_x(size_x);
				SeaRoutePoint	p3	= new SeaRoutePoint(p);
				p3.build_loop_x(-size_x);
				float	l2	= LengthSq(p2);
				float	l3	= LengthSq(p3);

				if(l1 < l2){
					if(l1 < l3){
						near_p	= p;
						return l1;
					}else{
						near_p	= p3;
						return l3;
					}
				}else{
					if(l2 < l3){
						near_p	= p2;
						return l2;
					}else{
						near_p	= p3;
						return l3;
					}
				}
			}

			/*-------------------------------------------------------------------------
			 距離を返す
			 ループを考慮し、近いほうの距離を返す
			---------------------------------------------------------------------------*/
			public float Length(SeaRoutePoint p, int size_x)
			{
				return (float)Math.Sqrt(LengthSq(p, size_x));
			}
			public float Length(SeaRoutePoint p, int size_x, ref SeaRoutePoint near_p)
			{
				SeaRoutePoint	near	= null;
				return (float)Math.Sqrt(LengthSq(p, size_x, ref near));
			}

			/*-------------------------------------------------------------------------
			 ループを考慮した位置を作成する
			 単純にposition.x + size_x
			---------------------------------------------------------------------------*/
			private void build_loop_x(int size_x)
			{
				m_pos.X		+= size_x;
			}

			/*-------------------------------------------------------------------------
			 渡されたpointからの移動ベクトルを得る
			 ループは考慮されない
			 ベクトルは正規化されていない
			 正規化したベクトルを得るにはGetVectorNormalized()を使用する
			---------------------------------------------------------------------------*/
			public Vector2 GetVector(SeaRoutePoint from)
			{
				return new Vector2(	this.Position.X - from.Position.X,
									this.Position.Y - from.Position.Y);
			}

			/*-------------------------------------------------------------------------
			 渡されたpointからの移動ベクトルを得る
			 ループは考慮されない
			 正規化されたベクトルを返す
			---------------------------------------------------------------------------*/
			public Vector2 GetVectorNormalized(SeaRoutePoint from)
			{
				Vector2		vec	= GetVector(from);
				vec.Normalize();
				return vec;
			}

			/*-------------------------------------------------------------------------
			 渡されたpointからの移動ベクトルを得る
			 ループが考慮される
			 ベクトルは正規化されていない
			 正規化したベクトルを得るにはGetVectorNormalized()を使用する
			---------------------------------------------------------------------------*/
			public Vector2 GetVector(SeaRoutePoint from, int loop_x_size)
			{
				SeaRoutePoint	near_p	= null;
				from.LengthSq(this, loop_x_size, ref near_p);
				return near_p.GetVector(from);
			}

			/*-------------------------------------------------------------------------
			 渡されたpointからの移動ベクトルを得る
			 ループが考慮される
			 正規化されたベクトルを返す
			---------------------------------------------------------------------------*/
			public Vector2 GetVectorNormalized(SeaRoutePoint from, int loop_x_size)
			{
				Vector2		vec	= GetVector(from, loop_x_size);
				vec.Normalize();
				return vec;
			}
		}

		/*-------------------------------------------------------------------------
		 ふきだし座標
		---------------------------------------------------------------------------*/
		public class SeaRoutePopupPoint : SeaRoutePoint
		{
			private int					m_days;				// 日数
			
			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public int Days				{	get{	return m_days;			}}

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public SeaRoutePopupPoint(float x, float y, int color_index, int days)
				: base(x, y, color_index)
			{
				m_days			= days;
			}
			public SeaRoutePopupPoint(Vector2 pos, int color_index, int days)
				: this(pos.X, pos.Y, color_index, days)
			{
			}
		}

		/*-------------------------------------------------------------------------
		 ふきだし座標
		 バウンディングボックス区切り
		---------------------------------------------------------------------------*/
		public class SeaRoutePopupPointsBB : D3dBB2d
		{
			private List<SeaRoutePopupPoint>		m_points;
			private int								m_max_days;

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public List<SeaRoutePopupPoint> Points		{	get{	return m_points;	}}
			public int MaxDays							{	get{	return m_max_days;	}}
	
			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public SeaRoutePopupPointsBB()
				: base()
			{
				m_points		= new List<SeaRoutePopupPoint>();
				base.OffsetLT	= new Vector2(-BB_OUTSIDESCREEEN_OFFSET, -BB_OUTSIDESCREEEN_OFFSET);
				base.OffsetRB	= new Vector2( BB_OUTSIDESCREEEN_OFFSET,  BB_OUTSIDESCREEEN_OFFSET);
				m_max_days		= 0;
			}

			/*-------------------------------------------------------------------------
			 追加
			 バウンディングボックスのサイズが一定以上のときは追加せずfalseを返す
			---------------------------------------------------------------------------*/
			public bool Add(SeaRoutePopupPoint p)
			{
				Vector2	size	= base.IfUpdate(p.Position).Size;
				if(size.X > BB_POPUP_SIZE_MAX)	return false;
				if(size.Y > BB_POPUP_SIZE_MAX)	return false;

				// 追加
				m_points.Add(p);
				base.Update(p.Position);

				// 最大航海日数の更新
				foreach(SeaRoutePopupPoint i in m_points){
					if(i.Days > m_max_days){
						m_max_days	= i.Days;
					}
				}

				return true;		// 追加した
			}
		}
	
		/*-------------------------------------------------------------------------
		 航路図用ライン
		---------------------------------------------------------------------------*/
		class RouteLineBB : D3dBB2d
		{
			private List<Vector2>	m_points;
			private int				m_color;			// alpha値は0なので描画時注意

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public int Color				{	get{	return m_color;					}}
			public int Count				{	get{	return m_points.Count;			}}

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public RouteLineBB(int color)
			{
				m_points	= new List<Vector2>();
				m_color		= color;
				base.OffsetLT	= new Vector2(-2, -2);
				base.OffsetRB	= new Vector2( 2,  2);
			}

			/*-------------------------------------------------------------------------
			 追加
			---------------------------------------------------------------------------*/
			public void AddPoint(Vector2 point)
			{
				m_points.Add(point);
				base.Update(point);
			}

			/*-------------------------------------------------------------------------
			 カリング情報を得る
			 trueを返したときカリングする
			---------------------------------------------------------------------------*/
			public bool IsCulling(Vector2 offset, LoopXImage image)
			{
				return base.IsCulling(offset, image.ImageScale, new D3dBB2d.CullingRect(image.Device.client_size));
			}

			/*-------------------------------------------------------------------------
			 バウンディングボックスを描画する
			 デバッグ用
			---------------------------------------------------------------------------*/
			public void DrawBB(Vector2 offset, LoopXImage image)
			{
				// 描画
				base.Draw(image.Device, 0.5f, offset, image.ImageScale, Color | (255<<24));
			}

			/*-------------------------------------------------------------------------
			 描画用に座標変換された配列を得る
			 IsCulling()でカリングするかどうかを調べてからこの関数を呼ぶこと
			---------------------------------------------------------------------------*/
			public Vector2[] BuildLinePoints(Vector2 offset, LoopXImage image)
			{
				Vector2[]	list	= new Vector2[m_points.Count];
				int			i		= 0;
				foreach(Vector2 p in m_points){
					list[i++]	= image.GlobalPos2LocalPos(p, offset);
				}
				return list;
			}
		}

		/*-------------------------------------------------------------------------
		 1航海分の航路
		---------------------------------------------------------------------------*/
		public class Voyage
		{
			private gvt_lib						m_lib;					//

			private List<SeaRoutePoint>			m_routes;				// 航路
			private List<SeaRoutePopupPointsBB>	m_popups;				// ふきだし
			private List<SeaRoutePopupPointsBB>	m_accidents;			// 災害
			private List<RouteLineBB>				m_line_routes;			// 描画用航路ライン
			private bool						m_is_build_line_routes;	// 航路ラインを作ったらtrue

			private float						m_alpha;				// 全体に掛かる半透明具合
			private float						m_gap_cos;				// できるだけラインを繋げる角度(cos)

			private bool						m_is_draw;				// 描画するときtrue
			private int							m_max_days;				// 最大航海日数
			private int							m_minimum_draw_days;	// 描画する最低航海日数
			private DateTime					m_date;					// 航海日時
			private bool						m_is_selected;			// 航路図一覧で選択されているときtrue

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			// 描画時の半透明度
			public float Alpha				{
				get{	return m_alpha;		}
				set{
					m_alpha		= value;
					if(m_alpha < 0)	m_alpha	= 0;
					if(m_alpha > 1)	m_alpha	= 1;
				}
			}
			// 空かどうかを得る
			public bool IsEmpty				{
				get{
					if(m_routes.Count > 1)		return false;
					if(m_popups.Count > 1)		return false;
					if(m_accidents.Count > 1)	return false;
					return true;
				}
			}
			// ユーザ指定による描画抑制
			public bool IsEnableDraw{
				get{	return m_is_draw;		}
				set{	m_is_draw	= value;	}
			}

			// 最大航海日数
			public int MaxDays{				get{	return m_max_days;	}}
			public string MaxDaysString{	get{	return String.Format("{0}日", MaxDays);	}}

			// 描画する最低航海日数
			public int MinimumDrawDays{
				get{	return m_minimum_draw_days;		}
				set{	m_minimum_draw_days	= value;	}
			}

			// 出発地点
			public Vector2 MapPoint1st{
				get{
					if(m_routes.Count > 0)	return m_routes[0].Position;
					return new Vector2(-1, -1);
				}
			}
			public string MapPoint1stString{	get{	return get_pos_str(MapPoint1st);	}}
			public Point GamePoint1st{
				get{
					if(m_routes.Count > 0){
						return transform.ToPoint(
									transform.map_pos2_game_pos(m_routes[0].Position, m_lib.loop_image));
					}
					return new Point(-1, -1);
				}
			}
			public string GamePoint1stStr{		get{	return get_pos_str(GamePoint1st);	}}

			// 終了地点
			public Vector2 MapPointLast{
				get{
					if(m_routes.Count > 0){
						return m_routes[m_routes.Count-1].Position;
					}
					return new Vector2(-1, -1);
				}
			}
			public string MapPointLastString{	get{	return get_pos_str(MapPointLast);	}}
			public Point GamePointLast{
				get{
					if(m_routes.Count > 0){
						return transform.ToPoint(
									transform.map_pos2_game_pos(m_routes[m_routes.Count-1].Position, m_lib.loop_image));
					}
					return new Point(-1, -1);
				}
			}
			public string GamePointLastString{	get{	return get_pos_str(GamePointLast);	}}

			public DateTime DateTime{		get{	return m_date;		}}
			public string DateTimeString{
				get{
					if(m_date.Ticks == 0){
//					if(m_date == null){
						return "不明(旧データ)";
					}
					return Useful.TojbbsDateTimeString(m_date);
				}
			}
			public bool IsSelected{				get{	return m_is_selected;		}
												set{	m_is_selected	= value;	}}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public Voyage(gvt_lib lib)
			{
				m_lib					= lib;

				m_popups				= new List<SeaRoutePopupPointsBB>();
				m_accidents				= new List<SeaRoutePopupPointsBB>();
				m_routes				= new List<SeaRoutePoint>();
				m_line_routes			= new List<RouteLineBB>();

				// 線の構築未完成
				m_is_build_line_routes	= false;

				Alpha					= 1;
				m_gap_cos				= (float)Math.Cos(Useful.ToRadian(ADD_POINT_ANGLE_GAP_MAX));
				m_is_draw				= true;
				m_max_days				= 0;
				m_minimum_draw_days		= 0;
				m_is_selected			= false;

				m_date					= DateTime.Now;
			}

			/*-------------------------------------------------------------------------
			 追加
			---------------------------------------------------------------------------*/
			public void AddPopup(SeaRoutePopupPoint p)
			{
				if(m_popups.Count <= 0){
					m_popups.Add(new SeaRoutePopupPointsBB());
				}
				if(!m_popups[m_popups.Count-1].Add(p)){
					m_popups.Add(new SeaRoutePopupPointsBB());
					m_popups[m_popups.Count-1].Add(p);
				}

				// 最大航海日数の更新
				foreach(SeaRoutePopupPointsBB i in m_popups){
					if(i.MaxDays > m_max_days)	m_max_days	= i.MaxDays;
				}
			}
			public void AddAccident(SeaRoutePopupPoint p)
			{
				if(m_accidents.Count <= 0){
					m_accidents.Add(new SeaRoutePopupPointsBB());
				}
				if(!m_accidents[m_accidents.Count-1].Add(p)){
					m_accidents.Add(new SeaRoutePopupPointsBB());
					m_accidents[m_accidents.Count-1].Add(p);
				}
			}
			public void AddPoint(SeaRoutePoint p)
			{
				m_routes.Add(p);
				m_is_build_line_routes	= false;	// 航路線構築リクエスト
			}

			/*-------------------------------------------------------------------------
			 航路図のラインを構築する
			 ある程度集まったラインを作成する
			 座標間の距離によってラインとするかどうかを判断する
			 バウンディングボックスのサイズが大きくなった場合線を分ける
			 座標間の距離が大きすぎる場合は角度差によってラインにするかを決定する
			---------------------------------------------------------------------------*/
			private void build_line_routes()
			{
				if(m_is_build_line_routes)	return;		// 作ってある
		
				m_line_routes.Clear();					// 破棄
				if(m_routes.Count < 2)		return;		// 線を作れない

				SeaRoutePoint		old_pos		= m_routes[0];	// 最初の座標
				SeaRoutePoint		old_pos2	= null;			// 2つ前の座標
				RouteLineBB	route		= null;

				for(int i=1; i<m_routes.Count; i++){
					if(route == null){
						// 開始
						route	= new RouteLineBB(old_pos.Color);
						route.AddPoint(old_pos.Position);
					}

					// ループを考慮した距離の2乗を得る
					// near_p は近い位置を返す
					// ループしない場合が最短の場合は near_p == m_routes[i]
					SeaRoutePoint	near_p	= null;
					float	length	= old_pos.LengthSq(	m_routes[i],
														(int)m_lib.loop_image.ImageSize.X,
														ref near_p);

					// 距離判定
					if(length > LINE_ROUTE_MAX){
#if DISABLE_SEA_ROUTES_CHAIN_POINTS
						// 出来るだけ繋げる処理無効
						near_p	= null;
#else
						if(length > ADD_POINT_ANGLE_LENGTH_SQ_MAX){
							// 距離が遠すぎる
							near_p	= null;
						}else{
							// 角度差判定
							// 角度差がありすぎるときはnullが返される
							near_p	= check_point_sub(old_pos, old_pos2, near_p);
						}
#endif
					}

					// 追加
					if(near_p != null)	route.AddPoint(near_p.Position);
			
					// バウンディングボックスのサイズチェック
					// 分割フラグかサイズが大きすぎるとき分割する
					if(   (near_p != m_routes[i])				// ループを考慮した位置を追加したか、追加されなかった
						||(route.Size.X >= BB_SIZE_MAX)			// バウンディングボックスのサイズを超えた
						||(route.Size.Y >= BB_SIZE_MAX) ){		// バウンディングボックスのサイズを超えた
						// 線を区切る
						add_route(route);
						route	= null;
					}

					// 以前の座標を更新する
					old_pos2	= old_pos;
					old_pos		= m_routes[i];
				}

				// 追加残りがあれば追加する
				if(route != null)		add_route(route);

				// 作成完了
				m_is_build_line_routes	= true;
			}
	
			/*-------------------------------------------------------------------------
			 距離が遠い場合のチェック
			 角度差で追加するかどうかを決める
			 角度差がありすぎるときはnullを返す
			---------------------------------------------------------------------------*/
			private SeaRoutePoint check_point_sub(SeaRoutePoint old_pos1, SeaRoutePoint old_pos2, SeaRoutePoint new_pos)
			{
				if(old_pos1 == null)	return null;
				if(old_pos2 == null)	return null;

				// 移動ベクトルを得る
				// ループが考慮される
				Vector2	old_vec	= old_pos1.GetVectorNormalized(old_pos2, (int)m_lib.loop_image.ImageSize.X);
				Vector2	new_vec	= new_pos.GetVectorNormalized(old_pos1, (int)m_lib.loop_image.ImageSize.X);

				// 角度差(cos)を得る
				float	gap			= Vector2.Dot(old_vec, new_vec);
				if(gap >= m_gap_cos){
					return new_pos;
				}
				return null;
			}

			/*-------------------------------------------------------------------------
			 線をリストに追加
			---------------------------------------------------------------------------*/
			private void add_route(RouteLineBB route)
			{
				if(route.Count < 2)		return;
				m_line_routes.Add(route);
			}
	
			/*-------------------------------------------------------------------------
			 航路図描画
			---------------------------------------------------------------------------*/
			public void DrawRoutes(Vector2 offset, LoopXImage image, bool is_select_mode)
			{
				// 描画する必要があるかチェック
				if(!can_draw(is_select_mode))	return;

				// ラインを構築
				// 作成済みであればなにもしない
				// 描画する必要がある場合のみ構築を試みる
				build_line_routes();

				// 半透明の値を得る
				int alpha	= (int)(m_alpha * 255);
				if(m_is_selected)	alpha	= 255;
				alpha		<<= 24;
	
				// 描画
				foreach(RouteLineBB line in m_line_routes){
					// バウンディングボックスでカリング
					if(line.IsCulling(offset, image))	continue;

					// 座標変換後描画
					m_lib.device.line.Draw(line.BuildLinePoints(offset, image), line.Color | alpha);
				}
			}

			/*-------------------------------------------------------------------------
			 描画するかチェック
			 選択モード中の選択なし
			 描画フラグ
			 航海日数が描画する最低航海日数に満たない
			---------------------------------------------------------------------------*/
			private bool can_draw(bool is_select_mode)
			{
				// 選択時は無条件で描画
				if(m_is_selected)						return true;
				// 選択モード中で選択中でない
				if(is_select_mode && (!m_is_selected))	return false;

				// 非表示時はなにもしない
				if(!m_is_draw)							return false;
				// 航海日数が描画する最低航海日数に満たないなら描画しない
				if(MaxDays < MinimumDrawDays)			return false;

				// 描画する
				return true;
			}

			/*-------------------------------------------------------------------------
			 バウンディングボックス描画
			 デバッグ用
			---------------------------------------------------------------------------*/
			public void DrawRoutesBB(Vector2 offset, LoopXImage image, bool is_select_mode)
			{
				// 描画する必要があるかチェック
				if(!can_draw(is_select_mode))	return;

				foreach(RouteLineBB line in m_line_routes){
					line.DrawBB(offset, image);
				}
			}

			/*-------------------------------------------------------------------------
			 ふきだし描画
			---------------------------------------------------------------------------*/
			public void DrawPopups(Vector2 offset, LoopXImage image, bool is_select_mode)
			{
				// 描画する必要があるかチェック
				if(!can_draw(is_select_mode))	return;

				float	size	= image.ImageScale;
				if(size < 0.5)		size	= 0.5f;
				else if(size > 1)	size	= 1;

				// 半透明具合を反映
				float	alpha	= (m_is_selected)? 1: m_alpha;
				int		alpha1	= ((int)(alpha * 255)) << 24;
				int		alpha2	= ((int)(alpha * 64)) << 24;
				int		color1	= 0x00ffffff | alpha1;
				int		color2	= 0x00ffffff | alpha2;

				D3dBB2d.CullingRect	rect	= new D3dBB2d.CullingRect(image.Device.client_size);
				foreach(SeaRoutePopupPointsBB b in m_popups){
					// バウンディングボックスで画面外かどうか調べる
					if(b.IsCulling(offset, image.ImageScale, rect)){
						continue;
					}
#if DRAW_POPUPS_BOUNDINGBOX
					d3d_bb2d.Draw(b.bb, image.device, 0.5f, offset, image.scale, Color.Blue.ToArgb());
#endif
					foreach(SeaRoutePopupPoint p in b.Points){
						// 日付ふきだし
						if(p.ColorIndex < 0)	continue;
						if(p.ColorIndex >= 8)	continue;

						Vector3		pos		= new Vector3(p.Position.X, p.Position.Y, 0.5f);

						if((p.Days % m_lib.setting.draw_popup_day_interval) != 0){
							// 通常の小さいアイコン
							m_lib.device.sprites.AddDrawSpritesNC(pos,
													m_lib.icons.GetIcon(icons.icon_index.days_mini_6),
													new Vector2(size, size),
													alpha1 | p.Color);
						}else{
							// 日付ふきだし

							// 影
							pos.Z	= 0.8f;
							m_lib.device.sprites.AddDrawSpritesNC(pos,
													m_lib.icons.GetIcon(icons.icon_index.days_big_shadow),
													new Vector2(1, 1),
													color2);
							pos.Z	= 0.5f;
							// 3桁の場合は横に広い絵を使用する
							icons.icon_index	icon	= (p.Days >= 100)? icons.icon_index.days_big_100: icons.icon_index.days_big_6;
							// フキダシ
							if(m_lib.device.sprites.AddDrawSpritesNC(	pos, m_lib.icons.GetIcon(icon), alpha1 | p.Color)){
								// ふきだしがカリングされなかったら日数を描く
								// 4桁以上はキャプチャできないと思う、たぶん
								int	days	= p.Days;
								if(days > 999)	days	= 999;
								if(days < 0)	days	= 0;

								int	max		= 1;
								Vector2		offset2		= new Vector2(0,0);
								if(days >= 100){		// 3桁
									offset2.X	+= 7;
									max			= 3;
								}else if(days >= 10){	// 2桁
									offset2.X	+= 4;
									max			= 2;
								}
								// 日数
								for(int i=0; i<max; i++){
									m_lib.device.sprites.AddDrawSpritesNC(pos, m_lib.icons.GetIcon(icons.icon_index.number_0 + (days % 10)), color1, offset2);
									offset2.X	-= 7;
									days		/= 10;
								}
							}
						}
					}
				}
			}

			/*-------------------------------------------------------------------------
			 災害描画
			---------------------------------------------------------------------------*/
			public void DrawAccidents(Vector2 offset, LoopXImage image, bool is_select_mode)
			{
				// 描画する必要があるかチェック
				if(!can_draw(is_select_mode))	return;

				// 半透明具合を反映
				float	alpha	= (m_is_selected)? 1: m_alpha;
				int		alpha1	= ((int)(alpha * 255)) << 24;
				int		alpha2	= ((int)(alpha * 64)) << 24;
				int		color1	= 0x00ffffff | alpha1;
				int		color2	= 0x00ffffff | alpha2;

				D3dBB2d.CullingRect	rect	= new D3dBB2d.CullingRect(image.Device.client_size);
				foreach(SeaRoutePopupPointsBB b in m_accidents){
					// バウンディングボックスで画面外かどうか調べる
					if(b.IsCulling(offset, image.ImageScale, rect)){
						continue;
					}
#if DRAW_POPUPS_BOUNDINGBOX
					d3d_bb2d.Draw(b.bb, image.device, 0.5f, offset, image.scale, Color.Red.ToArgb());
#endif

					foreach(SeaRoutePopupPoint p in b.Points){
						if(p.ColorIndex < 101)				continue;
						if(p.ColorIndex > 111)				continue;
						if(!is_draw_popups(p.ColorIndex))	continue;

						Vector3		pos		= new Vector3(p.Position.X, p.Position.Y, 0.5f);

						// 影
						pos.Z	= 0.8f;
						m_lib.device.sprites.AddDrawSpritesNC(pos,
												m_lib.icons.GetIcon(icons.icon_index.accident_popup_shadow),
												new Vector2(1, 1),
												color2);

						// フキダシ
						pos.Z	= 0.5f;
						m_lib.device.sprites.AddDrawSpritesNC(pos,
									m_lib.icons.GetIcon(icons.icon_index.accident_popup), color1);
						// 内容を描く
						m_lib.device.sprites.AddDrawSpritesNC(pos,
									m_lib.icons.GetIcon(icons.icon_index.accident_0 + (p.ColorIndex -101)), color1);
					}
				}
			}

			/*-------------------------------------------------------------------------
			 表示項目チェック
			---------------------------------------------------------------------------*/
			private bool is_draw_popups(int index)
			{
				// 描画フラグ
				draw_setting_accidents	flag	= m_lib.setting.draw_setting_accidents;

				switch(index){
				case 101:
					if((flag & draw_setting_accidents.accident_0) == 0)		return false;
					break;
				case 102:
					if((flag & draw_setting_accidents.accident_1) == 0)		return false;
					break;
				case 103:
					if((flag & draw_setting_accidents.accident_2) == 0)		return false;
					break;
				case 104:
					if((flag & draw_setting_accidents.accident_3) == 0)		return false;
					break;
				case 105:
					if((flag & draw_setting_accidents.accident_4) == 0)		return false;
					break;
				case 106:
					if((flag & draw_setting_accidents.accident_5) == 0)		return false;
					break;
				case 107:
					if((flag & draw_setting_accidents.accident_6) == 0)		return false;
					break;
				case 108:
					if((flag & draw_setting_accidents.accident_7) == 0)		return false;
					break;
				case 109:
					if((flag & draw_setting_accidents.accident_8) == 0)		return false;
					break;
				case 110:
					if((flag & draw_setting_accidents.accident_9) == 0)		return false;
					break;
				case 111:
					if((flag & draw_setting_accidents.accident_10) == 0)	return false;
					break;
				}
				return true;
			}
	
			/*-------------------------------------------------------------------------
			 読み込み開始
			 航海日時を初期化する
			---------------------------------------------------------------------------*/
			public void StartLoad()
			{
				m_date	= new DateTime();
			}

			/*-------------------------------------------------------------------------
			 読み込み
			---------------------------------------------------------------------------*/
			public void LoadFromLine(string line)
			{
				try{
					string[]	split	= line.Split(new char[]{','});

					switch(split[0]){
					case "popup":
					case "accidents":
						{
							int		x			= Convert.ToInt32(split[1]);
							int		y			= Convert.ToInt32(split[2]);
							int		days		= Convert.ToInt32(split[3]);
							int		color_index	= Convert.ToInt32(split[4]);

							// 追加
							if((color_index >= 101)&&(color_index <= 111)){
								// 災害
								AddAccident(new SeaRoutePopupPoint(x, y, color_index, days));
							}else{
								// 日付
								AddPopup(new SeaRoutePopupPoint(x, y, color_index, days));
							}
						}
						break;
					case "routes":
						{
							float	x			= (float)Convert.ToDouble(split[1]);
							float	y			= (float)Convert.ToDouble(split[2]);
							int		color_index	= Convert.ToInt32(split[3]);

							// 追加
							m_routes.Add(new SeaRoutePoint(x, y, color_index));
						}
						break;
					case "draw":
						{
							int		flag		= Convert.ToInt32(split[1]);
							m_is_draw			= (flag != 0)? true: false;
						}
						break;
					case "date":
						{
							m_date				= Useful.ToDateTime(split[1]);
						}
						break;
					default:
						break;
					}
				}catch{
					// 読み込み失敗
					// この行は無視される
				}
			}

			/*-------------------------------------------------------------------------
			 書き出し
			---------------------------------------------------------------------------*/
			public void Write(StreamWriter sw)
			{
				if(IsEnableDraw)		sw.WriteLine("draw,1");
				else			sw.WriteLine("draw,0");
				sw.WriteLine("date," + Useful.TojbbsDateTimeString(m_date));

				foreach(SeaRoutePopupPointsBB b in m_popups){
					foreach(SeaRoutePopupPoint p in b.Points){
						string	str		= "";
						str				+= "popup,";
						str				+= ((int)p.Position.X).ToString() + ",";
						str				+= ((int)p.Position.Y).ToString() + ",";
						str				+= p.Days + ",";
						str				+= p.ColorIndex;
						sw.WriteLine(str);
					}
				}
				foreach(SeaRoutePopupPointsBB b in m_accidents){
					foreach(SeaRoutePopupPoint p in b.Points){
						string	str		= "";
						str				+= "accidents,";
						str				+= ((int)p.Position.X).ToString() + ",";
						str				+= ((int)p.Position.Y).ToString() + ",";
						str				+= p.Days + ",";
						str				+= p.ColorIndex;
						sw.WriteLine(str);
					}
				}

				foreach(SeaRoutePoint p in m_routes){
					string	str		= "";
					str				+= "routes,";
					str				+= p.Position.X + ",";
					str				+= p.Position.Y + ",";
					str				+= p.ColorIndex;
					sw.WriteLine(str);
				}
			}

			/*-------------------------------------------------------------------------
			 描画用の色番号を得る
			---------------------------------------------------------------------------*/
			public int GetColorIndex()
			{
				if(m_routes.Count > 0){
					return m_routes[0].ColorIndex;
				}
				if(m_popups.Count > 0){
					if(m_popups[0].Points.Count > 0){
						return m_popups[0].Points[0].ColorIndex;
					}
				}
				return 0;
			}

			/*-------------------------------------------------------------------------
			 削除
			---------------------------------------------------------------------------*/
			public void Remove(bool popups, bool accident, bool routes)
			{
				if(popups)		m_popups.Clear();
				if(accident)	m_accidents.Clear();
				if(routes){
					m_routes.Clear();
					m_line_routes.Clear();
				}
			}

			/*-------------------------------------------------------------------------
			 スクリーンショット用
			 含まれる範囲を登録する
			---------------------------------------------------------------------------*/
			public void SS_AddMinMaxList(List<Point>[] map, ref int min_y, ref int max_y, bool is_select_mode)
			{
				// 描画する必要があるかチェック
				if(!can_draw(is_select_mode))	return;
	
				foreach(SeaRoutePoint p in m_routes){
					add_minmax_list(map,
									transform.ToPoint(p.Position),
									ref min_y, ref max_y);
				}
				foreach(SeaRoutePopupPointsBB b in m_popups){
					foreach(SeaRoutePopupPoint p in b.Points){
						add_minmax_list(map,
										transform.ToPoint(p.Position),
										ref min_y, ref max_y);
					}
				}
				foreach(SeaRoutePopupPointsBB b in m_accidents){
					foreach(SeaRoutePopupPoint p in b.Points){
						add_minmax_list(map,
										transform.ToPoint(p.Position),
										ref min_y, ref max_y);
					}
				}
			}

			/*-------------------------------------------------------------------------
			 X方向はおおまかな区切りに登録する
			 Y方向はそのまま最大最小を得る
			---------------------------------------------------------------------------*/
			private void add_minmax_list(List<Point>[] map, Point pos, ref int min_y, ref int max_y)
			{
				calc_bounding_box_y(pos.Y, ref min_y, ref max_y);
				int	index	= pos.X / SS_DISTRIBUTION_X;
				if(index < 0)				return;
				if(index >= map.Length)		return;
				map[index].Add(pos);
			}

			/*-------------------------------------------------------------------------
			 スクリーンショット用にバウンディングボックスを求める
			 縦方向
			---------------------------------------------------------------------------*/
			private void calc_bounding_box_y(int y, ref int min, ref int max)
			{
				if(y <= 64)		return;
				if(y > max)		max	= y;
				if(y < min)		min	= y;
			}

			/*-------------------------------------------------------------------------
			 位置を文字列で得る
			---------------------------------------------------------------------------*/
			private string get_pos_str(Point pos)
			{
				if(   (pos.X < 0)
					||(pos.Y < 0) ){
					return "不明な位置";
				}
				return String.Format("{0},{1}", pos.X, pos.Y);
			}
			private string get_pos_str(Vector2 pos)
			{
				if(   (pos.X < 0)
					||(pos.Y < 0) ){
					return "不明な位置";
				}
				return String.Format("{0},{1}", (int)pos.X, (int)pos.Y);
			}
		}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		private gvt_lib					m_lib;					//
	
		private List<Voyage>			m_sea_routes;			// 航路リスト
																// 色が変わるタイミングで区切られる
		private List<Voyage>			m_favorite_sea_routes;	// お気に入り航路リスト
		private List<Voyage>			m_trash_sea_routes;		// ごみ箱航路リスト

		// 追加時用
		private int						m_color_index;
		private int						m_old_days;
		private Point					m_old_day_pos;
		private Point					m_old_pos;
		private bool					m_is_1st;

		// 選択中は選択されている航路図のみ描画する
		private bool					m_is_select_mode;

		private request_ctrl			m_req_update_list;		// 航路図一覧更新リクエスト
		private request_ctrl			m_req_redraw_list;		// 航路図一覧再描画リクエスト

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public List<Voyage> searoutes						{	get{	return m_sea_routes;			}}
		public List<Voyage> favorite_sea_routes				{	get{	return m_favorite_sea_routes;	}}
		public List<Voyage> trash_sea_routes				{	get{	return m_trash_sea_routes;		}}
		public request_ctrl req_update_list					{	get{	return m_req_update_list;		}}
		public request_ctrl req_redraw_list					{	get{	return m_req_redraw_list;		}}
		
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public SeaRoutes(gvt_lib lib, string file_name, string favorite_file_name, string trash_file_name)
		{
			m_lib						= lib;

			m_sea_routes				= new List<Voyage>();
			m_favorite_sea_routes		= new List<Voyage>();
			m_trash_sea_routes			= new List<Voyage>();
			m_req_update_list			= new request_ctrl();
			m_req_redraw_list			= new request_ctrl();

			// 選択モード
			m_is_select_mode			= false;

			// 追加初期化
			init_add_points();

			// 以前のバージョンの読み込み
			load_old_routes(def.SEAROUTE_FULLFNAME1, def.SEAROUTE_FULLFNAME2);
	
			// 航路図読み込み
			load_routes(file_name);
			// お気に入り航路図読み込み
			load_routes_sub(m_favorite_sea_routes, favorite_file_name);
			// ごみ箱航路図読み込み
			load_routes_sub(m_trash_sea_routes, trash_file_name);
		}

		/*-------------------------------------------------------------------------
		 以前のバージョンの読み込み
		---------------------------------------------------------------------------*/
		private bool load_old_routes(string file_name1, string file_name2)
		{
			// どちらもなければなにもしない
			// 初回時に読み込んだ後は削除される
			if(   (!File.Exists(file_name1))
				&&(!File.Exists(file_name2)) ){
				return true;
			}
			
			string line			= "";
			int old_color_index	= -1;

			// 航路図
			try{
				using (StreamReader	sr	= new StreamReader(
					file_name2, Encoding.GetEncoding("Shift_JIS"))) {

					while((line = sr.ReadLine()) != null){
						try{
							string[]	split	= line.Split(new char[]{','});

							int		x			= Convert.ToInt32(split[0]);
							int		y			= Convert.ToInt32(split[1]);
							int		color_index	= Convert.ToInt32(split[2]);

							// 追加
							if(old_color_index != color_index){
								// 色が変わったらグループを追加
								add_sea_routes();
								old_color_index	= color_index;
							}
							add_point(new SeaRoutePoint(x, y, color_index));
						}catch{
						}
					}
				}
			}catch{
				// 読み込み失敗
				m_sea_routes.Clear();
			}

			// ポップアップ
			// トラブル回避のため、最新の場所にすべて読み込む
			try{
				using (StreamReader	sr	= new StreamReader(
					file_name1, Encoding.GetEncoding("Shift_JIS"))) {

					while((line = sr.ReadLine()) != null){
						try{
							string[]	split	= line.Split(new char[]{','});

							int		x			= Convert.ToInt32(split[0]);
							int		y			= Convert.ToInt32(split[1]);
							int		days		= Convert.ToInt32(split[2]);
							int		color_index	= Convert.ToInt32(split[3]);

							// 追加
							if((color_index >= 101)&&(color_index <= 111)){
								// 災害
								add_accident(new SeaRoutePopupPoint(x, y, color_index, days));
							}else{
								// 日付
								add_popup(new SeaRoutePopupPoint(x, y, color_index, days));
							}
						}catch{
						}
					}
				}
			}catch{
				// 読み込み失敗
				m_sea_routes.Clear();
			}
			
			// 古いバージョンのファイルは削除する
			file_ctrl.RemoveFile(file_name1);
			file_ctrl.RemoveFile(file_name2);
			return true;
		}
	
		/*-------------------------------------------------------------------------
		 読み込み
		---------------------------------------------------------------------------*/
		private bool load_routes(string file_name)
		{
			// 読み込み
			if(!load_routes_sub(m_sea_routes, file_name))		return false;

			// 新規に追加するときの色を決定する
			if(m_sea_routes.Count > 1){
				m_color_index	= get_newest_sea_routes().GetColorIndex();
				if(m_color_index < 0)		m_color_index	= 0;
				if(++m_color_index >= 8)	m_color_index	= 0;
			}
			return true;
		}

		/*-------------------------------------------------------------------------
		 読み込み
		 sub
		---------------------------------------------------------------------------*/
		private bool load_routes_sub(List<Voyage> list, string file_name)
		{
			string line = "";
			try{
				using (StreamReader	sr	= new StreamReader(
					file_name, Encoding.GetEncoding("Shift_JIS"))) {

					while((line = sr.ReadLine()) != null){
						if(line == "start routes"){
							add_sea_routes(list);
							get_newest_sea_routes(list).StartLoad();
						}else{
							get_newest_sea_routes(list).LoadFromLine(line);
						}
					}
				}
			}catch{
				// 読み込み失敗
				return false;
			}
			return true;
		}
	
		/*-------------------------------------------------------------------------
		 最新の航路情報を得る
		---------------------------------------------------------------------------*/
		private Voyage get_newest_sea_routes()
		{
			return get_newest_sea_routes(m_sea_routes);
		}
		private Voyage get_newest_sea_routes(List<Voyage> list)
		{
			if(list.Count < 1){
				// 無いので作る
				add_sea_routes(list);
			}
			return list[list.Count-1];
		}

		/*-------------------------------------------------------------------------
		 航路情報を追加する
		---------------------------------------------------------------------------*/
		private void add_sea_routes()
		{
			add_sea_routes(m_sea_routes);
		}
		private void add_sea_routes(List<Voyage> list)
		{
			list.Add(new Voyage(m_lib));

			// 一覧更新リクエスト
			m_req_update_list.Request();
		}

		/*-------------------------------------------------------------------------
		 保持数調整
		---------------------------------------------------------------------------*/
		private void ajust_size()
		{
			// 航路図一覧調整
			if(m_sea_routes.Count > 0){
				while(m_sea_routes.Count > m_lib.setting.searoutes_group_max){
					Voyage once	= m_sea_routes[0];
					m_sea_routes.RemoveAt(0);
					// 過去の航路図一覧に移動させる
					m_trash_sea_routes.Add(once);

					// 一覧更新リクエスト
					m_req_update_list.Request();
				}
			}
			// 過去の航路図一覧調整
			while(m_trash_sea_routes.Count > m_lib.setting.trash_searoutes_group_max){
				m_trash_sea_routes.RemoveAt(0);

				// 一覧更新リクエスト
				m_req_update_list.Request();
			}
		}

		/*-------------------------------------------------------------------------
		 描画
		 航路図
		---------------------------------------------------------------------------*/
		public void DrawRoutesLines()
		{
			// 保持数調整
			ajust_size();

			// 選択モードかどうかを判断する
			check_select_mode();

			// 半透明具合を反映させる
			set_alpha();
			// 描画する最低航海日数を反映させる
			set_minimum_draw_days();

			// 航路図描画
			draw_routes();
		}
	
		/*-------------------------------------------------------------------------
		 航路図を全て列挙する
		---------------------------------------------------------------------------*/
		private IEnumerable<Voyage> enum_all_sea_routes()
		{
			foreach(Voyage p in m_trash_sea_routes)	yield return p;
			foreach(Voyage p in m_favorite_sea_routes)	yield return p;
			foreach(Voyage p in m_sea_routes)			yield return p;
		}

		/*-------------------------------------------------------------------------
		 航路図を列挙する
		 ごみ箱は列挙されない
		---------------------------------------------------------------------------*/
		private IEnumerable<Voyage> enum_sea_routes_without_trash()
		{
			foreach(Voyage p in m_favorite_sea_routes)	yield return p;
			foreach(Voyage p in m_sea_routes)			yield return p;
		}
		
		/*-------------------------------------------------------------------------
		 選択モード中かどうかを判断する
		---------------------------------------------------------------------------*/
		private void check_select_mode()
		{
			m_is_select_mode	= false;

			IEnumerable<Voyage>	list	= enum_all_sea_routes();
			foreach(Voyage p in list){
				if(p.IsSelected){
					m_is_select_mode	= true;
					break;
				}
			}
		}

		/*-------------------------------------------------------------------------
		 描画
		 吹き出し
		 DrawRoutesLines()を呼んだ後であること
		---------------------------------------------------------------------------*/
		public void DrawPopups()
		{
			// 災害描画
			draw_accident();
			// ふきだし描画
			draw_popups();
		}

		/*-------------------------------------------------------------------------
		 半透明具合を指定する
		 設定により、最新の航路以外を薄くする
		---------------------------------------------------------------------------*/
		private void set_alpha()
		{
			if(m_lib.setting.enable_sea_routes_aplha){
				// 最新以外を薄くする
				foreach(Voyage p in m_sea_routes){
					p.Alpha	= SEAROUTES_ALPHA;
				}
				// 最新は不透明
				if(m_sea_routes.Count > 0){
					get_newest_sea_routes().Alpha	= 1;
				}
			}else{
				// 全て不透明
				foreach(Voyage p in m_sea_routes){
					p.Alpha	= 1;
				}
			}

			if(m_lib.setting.enable_favorite_sea_routes_alpha){
				foreach(Voyage p in m_favorite_sea_routes){
					p.Alpha	= SEAROUTES_ALPHA;
				}
			}else{
				foreach(Voyage p in m_favorite_sea_routes){
					p.Alpha	= 1;
				}
			}
		}

		/*-------------------------------------------------------------------------
		 描画する最低航海日数を反映させる
		---------------------------------------------------------------------------*/
		private void set_minimum_draw_days()
		{
			foreach(Voyage p in m_sea_routes){
				p.MinimumDrawDays	= m_lib.setting.minimum_draw_days;
			}
			// 最新は必ず描画
			if(m_sea_routes.Count > 0){
				get_newest_sea_routes().MinimumDrawDays	= 0;
			}
		}

		/*-------------------------------------------------------------------------
		 航路図描画
		---------------------------------------------------------------------------*/
		private void draw_routes()
		{
			if(!m_lib.setting.draw_sea_routes)	return;

			float	size	= 1 * m_lib.loop_image.ImageScale;
			if(size < 1)			size	= 1;
			else if(size > 2)		size	= 2;

			// 選択モード中は太く描画
			if(m_is_select_mode)	size	*= 3;

			m_lib.device.line.Width			= size;
			m_lib.device.line.Antialias		= m_lib.setting.enable_line_antialias;
			m_lib.device.line.Pattern		= -1;
			m_lib.device.line.PatternScale	= 1.0f;

			m_lib.device.device.RenderState.ZBufferEnable	= false;
			m_lib.device.line.Begin();
			m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(draw_routes_proc), 8f); 
			m_lib.device.line.End();

#if DRAW_SEA_ROUTES_BOUNDINGBOX
			// debug
			// バウンディングボックス描画
			m_lib.loop_image.EnumDrawCallBack(new loop_x_image.DrawHandler(draw_routes_bb_proc), 8f); 
#endif
			m_lib.device.device.RenderState.ZBufferEnable	= true;
		}

		/*-------------------------------------------------------------------------
		 航路図描画
		---------------------------------------------------------------------------*/
		private void draw_routes_proc(Vector2 offset, LoopXImage image)
		{
			IEnumerable<Voyage> list	= (m_is_select_mode)
														? enum_all_sea_routes()
														: enum_sea_routes_without_trash();
			foreach(Voyage p in list){
				p.DrawRoutes(offset, image, m_is_select_mode);
			}
		}

		/*-------------------------------------------------------------------------
		 バウンディングボックス描画
		 デバッグ用
		---------------------------------------------------------------------------*/
		private void draw_routes_bb_proc(Vector2 offset, LoopXImage image)
		{
			IEnumerable<Voyage> list	= (m_is_select_mode)
														? enum_all_sea_routes()
														: enum_sea_routes_without_trash();
			foreach(Voyage p in list){
				p.DrawRoutesBB(offset, image, m_is_select_mode);
			}
		}
	
		/*-------------------------------------------------------------------------
		 ふきだし描画
		---------------------------------------------------------------------------*/
		private void draw_popups()
		{
			if(m_lib.setting.draw_popup_day_interval == 0)	return;
			m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(draw_popups_proc), 32f);
		}

		/*-------------------------------------------------------------------------
		 ふきだし描画
		---------------------------------------------------------------------------*/
		private void draw_popups_proc(Vector2 offset, LoopXImage image)
		{
			m_lib.device.sprites.BeginDrawSprites(m_lib.icons.texture, offset, image.ImageScale, new Vector2(1,1));
			IEnumerable<Voyage> list	= (m_is_select_mode)
														? enum_all_sea_routes()
														: enum_sea_routes_without_trash();
			foreach(Voyage p in list){
				p.DrawPopups(offset, image, m_is_select_mode);
			}
			m_lib.device.sprites.EndDrawSprites();
		}

		/*-------------------------------------------------------------------------
		 災害描画
		---------------------------------------------------------------------------*/
		private void draw_accident()
		{
			if(!m_lib.setting.draw_accident)		return;

			m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(draw_accident_proc), 32f);
		}

		/*-------------------------------------------------------------------------
		 災害描画
		---------------------------------------------------------------------------*/
		private void draw_accident_proc(Vector2 offset, LoopXImage image)
		{
			m_lib.device.sprites.BeginDrawSprites(m_lib.icons.texture, offset, image.ImageScale, new Vector2(1,1));

			if(m_is_select_mode){
				IEnumerable<Voyage> list	= enum_all_sea_routes();
				foreach(Voyage p in list){
					p.DrawAccidents(offset, image, m_is_select_mode);
				}
			}else{
				if(m_lib.setting.draw_favorite_sea_routes_alpha_popup){
					// お気に入り航路図の災害ポップアップを描画する
					IEnumerable<Voyage> list	= enum_sea_routes_without_trash();
					foreach(Voyage p in list){
						p.DrawAccidents(offset, image, m_is_select_mode);
					}
				}else{
					// お気に入り航路図の災害ポップアップを描画しない
					foreach(Voyage p in m_sea_routes){
						p.DrawAccidents(offset, image, m_is_select_mode);
					}
				}
			}
			m_lib.device.sprites.EndDrawSprites();
		}

		/*-------------------------------------------------------------------------
		 座標追加関係
		---------------------------------------------------------------------------*/
		private void init_add_points()
		{
			m_color_index		= 0;
			m_old_days			= 0;
			m_old_day_pos		= new Point(0, 0);
			m_old_pos			= new Point(0, 0);
			m_is_1st			= true;
		}

		/*-------------------------------------------------------------------------
		 航路図の追加
		 距離が近い場合は追加しない
		 日付が前回より小さいと新しい色に変わる
		 posには測量座標を渡すこと
		---------------------------------------------------------------------------*/
		public void AddPoint(Point pos, int days, int accident)
		{
			if(days < 0)	return;
			if(pos.X < 0)	return;
			if(pos.Y < 0)	return;

			// 地図座標に変換
			Vector2	fpos	= transform.game_pos2_map_pos(transform.ToVector2(pos), m_lib.loop_image);
			
			if(m_is_1st){
				// 初めて
				add_sea_routes();				// 追加
				m_old_days		= days;
				m_old_pos		= pos;
				if(days > 0){
					add_popup(new SeaRoutePopupPoint(fpos, m_color_index, days));
				}
				// 追加のみでラインを構築しない
				add_point(new SeaRoutePoint(fpos, m_color_index));
				m_is_1st		= false;
			}else{
				// 2回目以降
				if(m_old_days != days){
					// 前回と同じ日付ではない
					if(m_old_days > days){
						// 一度寄港した
						if(++m_color_index >= 8)	m_color_index	= 0;
						add_sea_routes();				// 追加
					}
					// 0日は追加しない
					if(days > 0)	add_popup(new SeaRoutePopupPoint(fpos, m_color_index, days));
					m_old_days	= days;
				}
				// 航路図
				if(m_old_pos != pos){
					// 前回と位置が違う
					// 距離が近すぎる場合は追加しない
					SeaRoutePoint p1	= new SeaRoutePoint(transform.ToVector2(m_old_pos));
					SeaRoutePoint p2	= new SeaRoutePoint(transform.ToVector2(pos));
					float	min	= ADD_POINT_MIN * transform.get_rate_to_game_x(m_lib.loop_image);
					if(p1.LengthSq(p2, (int)m_lib.loop_image.ImageSize.X) >= min){
						add_point(new SeaRoutePoint(fpos, m_color_index));
						m_old_pos	= pos;
					}
				}
			}

			// 災害
			if((accident >= 101)&&(accident <= 111)){
				add_accident(new SeaRoutePopupPoint(fpos, accident, days));
			}
		}

		/*-------------------------------------------------------------------------
		 popup を追加する
		---------------------------------------------------------------------------*/
		private void add_popup(SeaRoutePopupPoint p)
		{
			Voyage	once	= get_newest_sea_routes();
			once.AddPopup(p);

			// 一覧再描画リクエスト
			m_req_redraw_list.Request();
		}

		/*-------------------------------------------------------------------------
		 accident を追加する
		---------------------------------------------------------------------------*/
		private void add_accident(SeaRoutePopupPoint p)
		{
			Voyage	once	= get_newest_sea_routes();
			once.AddAccident(p);

			// 一覧再描画リクエスト
			m_req_redraw_list.Request();
		}

		/*-------------------------------------------------------------------------
		 SeaRoutePoint を追加する
		---------------------------------------------------------------------------*/
		private void add_point(SeaRoutePoint p)
		{
			Voyage	once	= get_newest_sea_routes();
			once.AddPoint(p);

			// 一覧再描画リクエスト
			m_req_redraw_list.Request();
		}
		
		/*-------------------------------------------------------------------------
		 ふきだしと航路図の保存
		---------------------------------------------------------------------------*/
		public bool Write(string file_name)
		{
			return write(m_sea_routes, file_name);
		}

		/*-------------------------------------------------------------------------
		 お気に入り航路図の保存
		---------------------------------------------------------------------------*/
		public bool WriteFavorite(string file_name)
		{
			return write(m_favorite_sea_routes, file_name);
		}
	
		/*-------------------------------------------------------------------------
		 ごみ箱航路図の保存
		---------------------------------------------------------------------------*/
		public bool WriteTrash(string file_name)
		{
			return write(m_trash_sea_routes, file_name);
		}
	
		/*-------------------------------------------------------------------------
		 航路図書き出し
		 sub
		---------------------------------------------------------------------------*/
		static private bool write(List<Voyage> list, string file_name)
		{
			try{
				using (StreamWriter	sw	= new StreamWriter(
					file_name, false, Encoding.GetEncoding("Shift_JIS"))) {

					foreach(Voyage p in list){
						// 空なら書き出さない
						if(p.IsEmpty)		continue;

						sw.WriteLine("start routes");
						p.Write(sw);
					}
				}
			}catch{
				return false;
			}
			return true;
		}

		/*-------------------------------------------------------------------------
		 空きの航路を削除する
		---------------------------------------------------------------------------*/
		private void remove_empty_routes()
		{
			while(true){
				Voyage p	= find_empty_routes();
				if(p == null)	break;
				m_sea_routes.Remove(p);
			}
		}

		/*-------------------------------------------------------------------------
		 空きの航路を探す
		---------------------------------------------------------------------------*/
		private Voyage find_empty_routes()
		{
			foreach(Voyage p in m_sea_routes){
				if(p.IsEmpty)		return p;
			}
			return null;
		}

		/*-------------------------------------------------------------------------
		 スクリーンショット用にバウンディングボックスを求める
		 X方向にすべてのサイズが必要なときは
		 太平洋で区切られることに注意
		 それ以外は最小のサイズが得られる
		---------------------------------------------------------------------------*/
		public void CalcScreenShotBoundingBox(out Point offset, out Size size)
		{
			offset	= new Point(0, 0);
			size	= new Size((int)m_lib.loop_image.ImageSize.X, (int)m_lib.loop_image.ImageSize.Y);

			// 64ドット刻みで分布を調べる
			int				map_count	= (int)m_lib.loop_image.ImageSize.X / SS_DISTRIBUTION_X;
			if(((int)m_lib.loop_image.ImageSize.X % SS_DISTRIBUTION_X) != 0)	map_count++;
			List<Point>[]	map		= new List<Point>[map_count];
			for(int i=0; i<map_count; i++){
				map[i]		= new List<Point>();
			}
			
			int		min_y	= (int)m_lib.loop_image.ImageSize.Y;
			int		max_y	= 0;

			// X方向はおおまかな区切りに登録する
			// Y方向はそのまま最大最小を得る
			check_select_mode();
			IEnumerable<Voyage> list	= (m_is_select_mode)
														? enum_all_sea_routes()
														: enum_sea_routes_without_trash();
			foreach(Voyage once in list){
				once.SS_AddMinMaxList(map, ref min_y, ref max_y, m_is_select_mode);
			}

			// 1つも航路図がない場合はなにもせず帰る
			if(is_empty_list(map)){
				offset	= new Point(0, 0);
				size	= new Size(0, 0);
				return;
			}
	
			// X方向の最大と最小を求める
			// 空きの最大を得る
			int		start_index_x, free_count_x;
			calc_bounding_box_x(map, out start_index_x, out free_count_x);
			// 使用範囲を求める
			int		size_x, offset_x;
			calc_screenshot_range(map, start_index_x, free_count_x, out size_x, out offset_x);

			// オフセットとサイズ設定
			// 地図の横サイズよりも大きくなることがある
			size.Width	= size_x;
			size.Width	= (size.Width + 1) & ~1;	// 2の倍数とする
			offset.X	= offset_x;

			size.Height	= max_y - min_y;
			size.Height	= (size.Height + 1) & ~1;	// 2の倍数とする
			offset.Y	= min_y;

			// ギャップを追加する
			offset.X	-= SCREEN_SHOT_BOUNDING_BOX_GAP_X;
			offset.Y	-= SCREEN_SHOT_BOUNDING_BOX_GAP_Y;
			size.Width	+= SCREEN_SHOT_BOUNDING_BOX_GAP_X*2;
			size.Height	+= SCREEN_SHOT_BOUNDING_BOX_GAP_Y*2;
		}

		/*-------------------------------------------------------------------------
		 スクリーンショット用の範囲を求める
		---------------------------------------------------------------------------*/
		private void calc_screenshot_range(List<Point>[] map, int start_index_x, int free_count_x, out int size_x, out int offset_x)
		{
			if(free_count_x <= 0){
				// 空きがないので全部書き出す
				// 日付変更線あたりで区切る
				offset_x	= (int)-(m_lib.loop_image.ImageSize.X / 2);
				size_x		= (int)m_lib.loop_image.ImageSize.X;
				return;
			}

			// 空きから使用範囲を求める
			int	use_count	= map.Length - free_count_x;
			int	min			= start_index_x + free_count_x;
			if(min >= map.Length)	min	-= map.Length;
			if(min < 0)				min	+= map.Length;
			int	max			= min + use_count - 1;
			if(max >= map.Length)	max -= map.Length;
			if(max < 0)				max	+= map.Length;

			// 最大と最小を得る
			Point	min_point	= map[min][0];
			foreach(Point p in map[min]){
				if(p.X < min_point.X)	min_point	= p;
			}
			Point	max_point	= map[max][0];
			foreach(Point p in map[max]){
				if(p.X > max_point.X)	max_point	= p;
			}

			// 切り出し開始位置
			offset_x	= min_point.X;
			// サイズ
			if(max < min){
				// 最大のほうが小さい場合は地図サイズを足してから引く
				size_x	= (max_point.X + (int)m_lib.loop_image.ImageSize.X) - min_point.X;
			}else{
				// 普通に最大から最小を引く
				size_x	= max_point.X - min_point.X;
			}
		}
	
		/*-------------------------------------------------------------------------
		 1つも航路図がないかどうか調べる
		---------------------------------------------------------------------------*/
		private bool is_empty_list(List<Point>[] map)
		{
			int map_count	= map.Length;
			for(int i=0; i<map_count; i++){
				if(map[i].Count > 0)	return false;	// 航路図あり
			}
			// 航路図なし
			return true;
		}

		/*-------------------------------------------------------------------------
		 最も広い空きを見つける
		---------------------------------------------------------------------------*/
		private void calc_bounding_box_x(List<Point>[] map, out int start_index, out int free_count)
		{
			int map_count	= map.Length;
	
			int	max_start	= -1;
			int	max_count	= 0;
			for(int i=0; i<map_count; i++){
				if(map[i].Count <= 0){
					// 空いてる
					int count	= calc_bounding_box_x_sub(map, i);
					if(count > max_count){
						max_start	= i;
						max_count	= count;
					}
				}
			}
			start_index		= max_start;	// 空き開始インデックス
			free_count		= max_count;	// 空き数
		}

		/*-------------------------------------------------------------------------
		 連続する空きを調べる
		---------------------------------------------------------------------------*/
		private int calc_bounding_box_x_sub(List<Point>[] map, int start)
		{
			int map_count	= map.Length;
			int	index		= 0;
			int	empty_count	= 0;
			while(index < map_count){
				int	i	= start + index;
				if(i >= map_count)	i	-= map_count;

				if(map[i].Count > 0)	break;		// 空きではない
				empty_count++;
				index++;
			}
			return empty_count;		// 連続する空きを返す
		}

		/*-------------------------------------------------------------------------
		 選択状態をリセットする
		---------------------------------------------------------------------------*/
		public void ResetSelectFlag()
		{
			IEnumerable<Voyage> list	= enum_all_sea_routes();
			foreach(Voyage p in list)	p.IsSelected	= false;
		}

		/*-------------------------------------------------------------------------
		 航路一覧から削除する
		 最新の航路図が削除された場合に対応
		---------------------------------------------------------------------------*/
		public void RemoveSeaRoutes(List<Voyage> remove_list)
		{
			// 最新の航路図を削除するかどうかを得る
			if(is_newest_sea_routes(remove_list)){
				// 最新が消えるので
				// 起動直後の状態にする
				init_add_points();
			}

			// 削除
			remove_sea_routes(m_sea_routes, remove_list);
		}

		/*-------------------------------------------------------------------------
		 航路一覧から削除する
		---------------------------------------------------------------------------*/
		public void remove_sea_routes(List<Voyage>list, List<Voyage> remove_list)
		{
			foreach(Voyage i in remove_list){
				try{
					list.Remove(i);
				}catch{
				}
			}
		}

		/*-------------------------------------------------------------------------
		 お気に入り航路一覧から削除する
		 無条件で削除して問題ない
		---------------------------------------------------------------------------*/
		public void RemoveFavoriteSeaRoutes(List<Voyage> remove_list)
		{
			remove_sea_routes(m_favorite_sea_routes, remove_list);
		}

		/*-------------------------------------------------------------------------
		 ごみ箱入り航路一覧から削除する
		 無条件で削除して問題ない
		---------------------------------------------------------------------------*/
		public void RemoveTrashSeaRoutes(List<Voyage> remove_list)
		{
			remove_sea_routes(m_trash_sea_routes, remove_list);
		}

		/*-------------------------------------------------------------------------
		 最新の航路図を削除するかどうかを得る
		---------------------------------------------------------------------------*/
		private bool is_newest_sea_routes(List<Voyage> list)
		{
			Voyage	once	= get_newest_sea_routes();
			foreach(Voyage i in list){
				if(i == once)	return true;
			}
			return false;
		}

		/*-------------------------------------------------------------------------
		 航路図の移動
		 航路図からお気に入り航路図
		---------------------------------------------------------------------------*/
		public void MoveSeaRoutesToFavoriteSeaRoutes(List<Voyage> move_list)
		{
			// 追加
			add_sea_routes(m_sea_routes, m_favorite_sea_routes, move_list);
			// 元を削除
			RemoveSeaRoutes(move_list);
		}

		/*-------------------------------------------------------------------------
		 航路図の移動
		 航路図から過去の航路図
		---------------------------------------------------------------------------*/
		public void MoveSeaRoutesToTrashSeaRoutes(List<Voyage> move_list)
		{
			// 追加
			add_sea_routes(m_sea_routes, m_trash_sea_routes, move_list);
			// 元を削除
			RemoveSeaRoutes(move_list);
		}

		/*-------------------------------------------------------------------------
		 航路図の移動
		 お気に入り航路図から過去の航路図
		---------------------------------------------------------------------------*/
		public void MoveFavoriteSeaRoutesToTrashSeaRoutes(List<Voyage> move_list)
		{
			// 追加
			add_sea_routes(m_favorite_sea_routes, m_trash_sea_routes, move_list);
			// 元を削除
			RemoveFavoriteSeaRoutes(move_list);
		}

		/*-------------------------------------------------------------------------
		 航路図の移動
		 過去の航路図からお気に入り航路図
		---------------------------------------------------------------------------*/
		public void MoveTrashSeaRoutesToFavoriteSeaRoutes(List<Voyage> move_list)
		{
			// 追加
			add_sea_routes(m_trash_sea_routes, m_favorite_sea_routes, move_list);
			// 元を削除
			RemoveTrashSeaRoutes(move_list);
		}

		/*-------------------------------------------------------------------------
		 航路図の移動
		 移動元にある航路図のみ移動する
		 移動元になければ移動しない
		---------------------------------------------------------------------------*/
		private void add_sea_routes(List<Voyage> from_list, List<Voyage> to_list, List<Voyage> move_list)
		{
			foreach(Voyage i in move_list){
				if(has_sea_routes(from_list, i)){
					// 追加
					to_list.Add(i);
				}
			}
		}

		/*-------------------------------------------------------------------------
		 指定された航路図がリストに含まれるか調べる
		---------------------------------------------------------------------------*/
		private bool has_sea_routes(List<Voyage> list, Voyage routes)
		{
			foreach(Voyage i in list){
				if(i == routes)		return true;
			}
			return false;
		}
	}

	/*-------------------------------------------------------------------------
	 描画色
	 半透明部分は0を返すため、使用時に半透明値をorすること
	---------------------------------------------------------------------------*/
	static internal class DrawColor
	{
		// 描画色
		static private int[]		m_color_tbl	= new int[]{
										Color.FromArgb(0, 224,  64,  64).ToArgb(),	// 赤
										Color.FromArgb(0, 224, 160,   0).ToArgb(),	// オレンジっぽい
										Color.FromArgb(0, 224, 224,   0).ToArgb(),	// 黄色
										Color.FromArgb(0,  64, 160,  64).ToArgb(),	// 緑
										Color.FromArgb(0,  30, 160, 160).ToArgb(),	// 青っぽい
										Color.FromArgb(0, 120, 120, 230).ToArgb(),	// 薄い紫
										Color.FromArgb(0, 255, 255, 255).ToArgb(),	// 白
										Color.FromArgb(0, 255, 150, 150).ToArgb(),	// ピンク
									};

		/*-------------------------------------------------------------------------
		 描画色を得る
		---------------------------------------------------------------------------*/
		static public int GetColor(int color_index)
		{
			if(color_index < 0)		return m_color_tbl[0];
			if(color_index >= 8)	return m_color_tbl[0];
			return m_color_tbl[color_index];
		}
	}
}
