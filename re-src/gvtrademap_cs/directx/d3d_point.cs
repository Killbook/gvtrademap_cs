// Type: directx.d3d_point
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;

namespace directx
{
  public class d3d_point
  {
    private const int DRAW_POINTS_ONCE = 512;
    private Device m_d3d_device;
    private List<d3d_point.point> m_point_list;
    private float m_point_size;

    public d3d_point(Device device)
    {
      this.m_d3d_device = device;
      this.m_point_list = new List<d3d_point.point>();
      this.m_point_size = 1f;
    }

    private void draw_points(List<d3d_point.point> list, float size)
    {
      int count = list.Count;
      CustomVertex.TransformedColored[] transformedColoredArray = new CustomVertex.TransformedColored[count];
      for (int index = 0; index < count; ++index)
      {
        transformedColoredArray[index].X = list[index].position.X;
        transformedColoredArray[index].Y = list[index].position.Y;
        transformedColoredArray[index].Z = list[index].position.Z;
        transformedColoredArray[index].Color = list[index].color;
        transformedColoredArray[index].Rhw = 1f;
      }
      this.m_d3d_device.RenderState.PointSize = size;
      this.m_d3d_device.VertexFormat = VertexFormats.Diffuse | VertexFormats.Transformed;
      this.m_d3d_device.SetTexture(0, (BaseTexture) null);
      this.m_d3d_device.DrawUserPrimitives(PrimitiveType.PointList, count, (object) transformedColoredArray);
    }

    public void BeginDrawPoints(float size)
    {
      this.m_point_list.Clear();
      this.m_point_size = size;
    }

    public void AddDrawPoints(Vector3 pos, int color)
    {
      this.m_point_list.Add(new d3d_point.point(pos, color));
      if (this.m_point_list.Count < 512)
        return;
      this.draw_points(this.m_point_list, this.m_point_size);
      this.m_point_list.Clear();
    }

    public void EndDrawPoints()
    {
      if (this.m_point_list.Count > 0)
        this.draw_points(this.m_point_list, this.m_point_size);
      this.m_point_list.Clear();
      this.m_point_size = 0.0f;
    }

    public class point
    {
      private Vector3 m_pos;
      private int m_color;

      public Vector3 position
      {
        get
        {
          return this.m_pos;
        }
      }

      public int color
      {
        get
        {
          return this.m_color;
        }
      }

      public point(Vector3 pos, int color)
      {
        this.m_pos = pos;
        this.m_color = color;
      }

      public point(float x, float y, float z, int color)
      {
        this.m_pos.X = x;
        this.m_pos.Y = y;
        this.m_pos.Z = z;
        this.m_color = color;
      }
    }
  }
}
