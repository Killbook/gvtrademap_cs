// Type: Utility.hittest
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Drawing;

namespace Utility
{
  public class hittest
  {
    private Rectangle m_rect;
    private Point m_position;
    private float m_scale;
    private bool m_enable;

    public Rectangle rect
    {
      get
      {
        return this.m_rect;
      }
      set
      {
        this.m_rect = value;
      }
    }

    public Point position
    {
      get
      {
        return this.m_position;
      }
      set
      {
        this.m_position = value;
      }
    }

    public float scale
    {
      get
      {
        return this.m_scale;
      }
      set
      {
        this.m_scale = value;
      }
    }

    public bool enable
    {
      get
      {
        return this.m_enable;
      }
      set
      {
        this.m_enable = value;
      }
    }

    public hittest()
    {
      this.m_rect = new Rectangle(0, 0, 0, 0);
      this.m_position = new Point(0, 0);
      this.m_scale = 1f;
      this.m_enable = true;
    }

    public hittest(Rectangle rect)
    {
      this.m_rect = rect;
      this.m_position = new Point(0, 0);
      this.m_scale = 1f;
      this.m_enable = true;
    }

    public hittest(Rectangle rect, Point position)
    {
      this.m_rect = rect;
      this.m_position = position;
      this.m_scale = 1f;
      this.m_enable = true;
    }

    public hittest(Rectangle rect, Point position, float scale)
    {
      this.m_rect = rect;
      this.m_position = position;
      this.m_scale = scale;
      this.m_enable = true;
    }

    public bool HitTest(hittest hit)
    {
      return this.HitTest(hit.CalcRect());
    }

    public bool HitTest(Rectangle rect)
    {
      if (!this.enable)
        return false;
      Rectangle rectangle = this.CalcRect();
      return rectangle.X < rect.X + rect.Width && rectangle.Y < rect.Y + rect.Height && (rectangle.X + rectangle.Width >= rect.X && rectangle.Y + rectangle.Height >= rect.Y);
    }

    public bool HitTest(Point pos)
    {
      if (!this.enable)
        return false;
      Rectangle rectangle = this.CalcRect();
      return pos.X >= rectangle.X && pos.Y >= rectangle.Y && (pos.X < rectangle.X + rectangle.Width && pos.Y < rectangle.Y + rectangle.Height);
    }

    public bool HitTest(hittest hit, int type)
    {
      if (!this.HitTest(hit))
        return false;
      this.OnHit(hit, type);
      return true;
    }

    public bool HitTest(Rectangle rect, int type)
    {
      if (!this.HitTest(rect))
        return false;
      this.OnHit(rect, type);
      return true;
    }

    public bool HitTest(Point pos, int type)
    {
      if (!this.HitTest(pos))
        return false;
      this.OnHit(pos, type);
      return true;
    }

    public Rectangle CalcRect()
    {
      return new Rectangle((int) ((float) this.m_rect.X + this.m_scale * (float) this.m_position.X), (int) ((float) this.m_rect.Y + this.m_scale * (float) this.m_position.Y), (int) (this.m_scale * (float) this.m_rect.Width), (int) (this.m_scale * (float) this.m_rect.Height));
    }

    protected virtual void OnHit(hittest hit, int type)
    {
    }

    protected virtual void OnHit(Rectangle rect, int type)
    {
    }

    protected virtual void OnHit(Point pos, int type)
    {
    }
  }
}
