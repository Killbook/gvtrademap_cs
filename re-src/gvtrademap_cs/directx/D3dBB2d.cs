// Type: directx.D3dBB2d
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX;

namespace directx
{
  public class D3dBB2d
  {
    private Vector2 m_min;
    private Vector2 m_max;
    private bool m_is_1st;
    private Vector2 m_offset_lt;
    private Vector2 m_offset_rb;

    public Vector2 Min
    {
      get
      {
        return this.m_min;
      }
    }

    public Vector2 Max
    {
      get
      {
        return this.m_max;
      }
    }

    public Vector2 Size
    {
      get
      {
        return this.m_max - this.m_min;
      }
    }

    public Vector2 OffsetLT
    {
      get
      {
        return this.m_offset_lt;
      }
      set
      {
        this.m_offset_lt = value;
      }
    }

    public Vector2 OffsetRB
    {
      get
      {
        return this.m_offset_rb;
      }
      set
      {
        this.m_offset_rb = value;
      }
    }

    public D3dBB2d()
    {
      this.m_min = new Vector2(0.0f, 0.0f);
      this.m_max = new Vector2(0.0f, 0.0f);
      this.m_offset_lt = new Vector2(0.0f, 0.0f);
      this.m_offset_rb = new Vector2(0.0f, 0.0f);
      this.m_is_1st = false;
    }

    public void Update(Vector2 pos)
    {
      if (!this.m_is_1st)
      {
        this.m_min = pos;
        this.m_max = pos;
        this.m_is_1st = true;
      }
      else
      {
        if ((double) pos.X < (double) this.m_min.X)
          this.m_min.X = pos.X;
        if ((double) pos.X > (double) this.m_max.X)
          this.m_max.X = pos.X;
        if ((double) pos.Y < (double) this.m_min.Y)
          this.m_min.Y = pos.Y;
        if ((double) pos.Y <= (double) this.m_max.Y)
          return;
        this.m_max.Y = pos.Y;
      }
    }

    public D3dBB2d IfUpdate(Vector2 pos)
    {
      D3dBB2d d3dBb2d = new D3dBB2d();
      if (this.m_is_1st)
      {
        d3dBb2d.Update(this.Min);
        d3dBb2d.Update(this.Max);
      }
      d3dBb2d.Update(pos);
      d3dBb2d.OffsetLT = this.m_offset_lt;
      d3dBb2d.OffsetRB = this.m_offset_rb;
      return d3dBb2d;
    }

    public bool IsCulling(D3dBB2d.CullingRect rect)
    {
      return this.IsCulling(new Vector2(0.0f, 0.0f), 1f, rect);
    }

    public bool IsCulling(Vector2 offset, float scale, D3dBB2d.CullingRect rect)
    {
      if (!this.m_is_1st)
        return true;
      Vector2 vector2_1 = D3dBB2d.offsetscale(this.m_max, offset, scale) + this.m_offset_rb;
      if ((double) vector2_1.X < (double) rect.left_top.X || (double) vector2_1.Y < (double) rect.left_top.Y)
        return true;
      Vector2 vector2_2 = D3dBB2d.offsetscale(this.m_min + this.m_offset_lt, offset, scale) + this.m_offset_lt;
      return (double) vector2_2.X >= (double) rect.right_bottom.X || (double) vector2_2.Y >= (double) rect.right_bottom.Y;
    }

    private static Vector2 offsetscale(Vector2 p, Vector2 offset, float scale)
    {
      return (p + offset) * scale;
    }

    public void Draw(d3d_device device, float z, int color)
    {
      D3dBB2d.Draw(this, device, z, color);
    }

    public static void Draw(D3dBB2d bb, d3d_device device, float z, int color)
    {
      D3dBB2d.Draw(bb, device, z, new Vector2(0.0f, 0.0f), 1f, color);
    }

    public void Draw(d3d_device device, float z, Vector2 offset, float scale, int color)
    {
      D3dBB2d.Draw(this, device, z, offset, scale, color);
    }

    public static void Draw(D3dBB2d bb, d3d_device device, float z, Vector2 offset, float scale, int color)
    {
      Vector2 vector2_1 = D3dBB2d.offsetscale(bb.Min, offset, scale);
      Vector2 vector2_2 = D3dBB2d.offsetscale(bb.Max, offset, scale);
      Vector2 vector2_3 = vector2_1 + bb.OffsetLT;
      Vector2 size = vector2_2 + bb.OffsetRB - vector2_3;
      device.DrawLineRect(new Vector3(vector2_3.X, vector2_3.Y, z), size, color);
    }

    public struct CullingRect
    {
      public Vector2 left_top;
      public Vector2 right_bottom;

      public Vector2 Size
      {
        get
        {
          return this.right_bottom - this.left_top;
        }
        set
        {
          this.right_bottom = value + this.left_top;
        }
      }

      public CullingRect(Vector2 _left_top, Vector2 _right_bottom)
      {
        this.left_top = _left_top;
        this.right_bottom = _right_bottom;
      }

      public CullingRect(float left, float top, float right, float bottom)
      {
        this.left_top = new Vector2(left, top);
        this.right_bottom = new Vector2(right, bottom);
      }

      public CullingRect(Vector2 size)
      {
        this.left_top = new Vector2(0.0f, 0.0f);
        this.right_bottom = size;
      }
    }
  }
}
