﻿/*-------------------------------------------------------------------------

 Direct3D
 点

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;


/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace directx
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class d3d_point
	{
		private const int				DRAW_POINTS_ONCE	= 512;

		/*-------------------------------------------------------------------------
		 点描画用
		---------------------------------------------------------------------------*/
		public class point
		{
			private Vector3				m_pos;
			private int					m_color;

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public Vector3		position{		get{	return m_pos;		}}
			public int			color{			get{	return m_color;		}}

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public point(Vector3 pos, int color)
			{
				m_pos		= pos;
				m_color		= color;
			}
			public point(float x, float y, float z, int color)
			{
				m_pos.X		= x;
				m_pos.Y		= y;
				m_pos.Z		= z;
				m_color		= color;
			}
		}
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		private Device								m_d3d_device;

		private	List<point>							m_point_list;
		private float								m_point_size;

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public d3d_point(Device device)
		{
			m_d3d_device	= device;
			m_point_list	= new List<point>();
			m_point_size	= 1f;
		}

		/*-------------------------------------------------------------------------
		 点の描画
		 ある程度まとまった数を描く
		---------------------------------------------------------------------------*/
		private void draw_points(List<point> list, float size)
		{
			int		count	= list.Count;
	
			CustomVertex.TransformedColored[]	vb	= new CustomVertex.TransformedColored[count];
									
			for(int i=0; i<count; i++){
				vb[i].X		= list[i].position.X;
				vb[i].Y		= list[i].position.Y;
				vb[i].Z		= list[i].position.Z;
				vb[i].Color	= list[i].color;
				vb[i].Rhw	= 1f;
			}

			// point Size
			m_d3d_device.RenderState.PointSize		= size;

			// draw primitives
			m_d3d_device.VertexFormat	= CustomVertex.TransformedColored.Format;
			m_d3d_device.SetTexture(0, null);
			m_d3d_device.DrawUserPrimitives(PrimitiveType.PointList, count, vb);
		}

		/*-------------------------------------------------------------------------
		 点の描画開始
		---------------------------------------------------------------------------*/
		public void BeginDrawPoints(float size)
		{
			m_point_list.Clear();
			m_point_size		= size;
		}
		/*-------------------------------------------------------------------------
		 点の描画 追加
		---------------------------------------------------------------------------*/
		public void AddDrawPoints(Vector3 pos, int color)
		{
			m_point_list.Add(new point(pos, color));
			if(m_point_list.Count >= DRAW_POINTS_ONCE){
				// ある程度溜まったらまとめて描画する
				draw_points(m_point_list, m_point_size);
				// 描画リストをクリア
				m_point_list.Clear();
			}
		}
		/*-------------------------------------------------------------------------
		 点の描画終了
		---------------------------------------------------------------------------*/
		public void EndDrawPoints()
		{
			if(m_point_list.Count > 0){
				// 残りを描画
				draw_points(m_point_list, m_point_size);
			}
	
			m_point_list.Clear();
			m_point_size		= 0;
		}
	}
}
