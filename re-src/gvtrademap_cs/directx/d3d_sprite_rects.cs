// Type: directx.d3d_sprite_rects
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace directx
{
  public class d3d_sprite_rects : IDisposable
  {
    private Texture m_texture;
    private Vector2 m_texture_size;
    private List<d3d_sprite_rects.rect> m_rects;

    public Texture texture
    {
      get
      {
        return this.m_texture;
      }
    }

    public Vector2 texture_size
    {
      get
      {
        return this.m_texture_size;
      }
    }

    public List<d3d_sprite_rects.rect> rects
    {
      get
      {
        return this.m_rects;
      }
    }

    public int rect_count
    {
      get
      {
        return this.m_rects.Count;
      }
    }

    public d3d_sprite_rects(d3d_device device, string fname)
    {
      try
      {
        this.m_texture = TextureLoader.FromFile(device.device, fname);
        this.m_texture_size = d3d_utility.GetTextureSize(this.m_texture);
      }
      catch
      {
        this.m_texture = (Texture) null;
        this.m_texture_size.X = 0.0f;
        this.m_texture_size.Y = 0.0f;
      }
      this.m_rects = new List<d3d_sprite_rects.rect>();
    }

    public d3d_sprite_rects(d3d_device device, Stream stream)
    {
      try
      {
        this.m_texture = TextureLoader.FromStream(device.device, stream);
        this.m_texture_size = d3d_utility.GetTextureSize(this.m_texture);
      }
      catch
      {
        this.m_texture = (Texture) null;
        this.m_texture_size.X = 0.0f;
        this.m_texture_size.Y = 0.0f;
      }
      this.m_rects = new List<d3d_sprite_rects.rect>();
    }

    public int AddRect(Vector2 offset, Rectangle _rect)
    {
      this.m_rects.Add(new d3d_sprite_rects.rect(this.m_texture_size, offset, _rect));
      return this.m_rects.Count - 1;
    }

    public void Dispose()
    {
      if (this.m_texture != (Texture) null)
      {
        this.m_texture.Dispose();
        this.m_texture = (Texture) null;
      }
      this.m_texture_size = new Vector2(0.0f, 0.0f);
    }

    public virtual d3d_sprite_rects.rect GetRect(int index)
    {
      return this.m_rects[index];
    }

    protected void DumpRects(string bmp_file_name)
    {
      foreach (d3d_sprite_rects.rect rect in this.m_rects)
        rect.DumpRects(bmp_file_name);
    }

    public class rect
    {
      private Vector2[] m_offset;
      private Vector2[] m_uv;
      private Vector2 m_lefttop;
      private Vector2 m_size;
      private Rectangle m_rect;

      public Vector2[] offset
      {
        get
        {
          return this.m_offset;
        }
      }

      public Vector2[] uv
      {
        get
        {
          return this.m_uv;
        }
      }

      public Vector2 lefttop
      {
        get
        {
          return this.m_lefttop;
        }
      }

      public Vector2 size
      {
        get
        {
          return this.m_size;
        }
      }

      public rect(Vector2 tex_size, Vector2 offset, Rectangle _rect)
      {
        this.m_lefttop = offset;
        this.m_rect = _rect;
        this.m_size = new Vector2((float) _rect.Width, (float) _rect.Height);
        Vector2 vector2_1 = new Vector2((float) _rect.X / tex_size.X, (float) _rect.Y / tex_size.Y);
        Vector2 vector2_2 = new Vector2(((float) _rect.X + (float) _rect.Width) / tex_size.X, ((float) _rect.Y + (float) _rect.Height) / tex_size.Y);
        this.m_offset = new Vector2[4];
        this.m_offset[0] = new Vector2(offset.X, offset.Y);
        this.m_offset[1] = new Vector2(offset.X + this.m_size.X, offset.Y);
        this.m_offset[2] = new Vector2(offset.X, offset.Y + this.m_size.Y);
        this.m_offset[3] = new Vector2(offset.X + this.m_size.X, offset.Y + this.m_size.Y);
        this.m_uv = new Vector2[4];
        this.m_uv[0] = vector2_1;
        this.m_uv[1] = new Vector2(vector2_2.X, vector2_1.Y);
        this.m_uv[2] = new Vector2(vector2_1.X, vector2_2.Y);
        this.m_uv[3] = vector2_2;
      }

      internal void DumpRects(string bmp_file_name)
      {
        Debug.WriteLine("OBJDT\ticons_0[] = {");
        Debug.WriteLine("\t{\t\"" + bmp_file_name + "\",");
        Debug.WriteLine("\t\t\t0,\t\t// PaletteName");
        Debug.WriteLine(string.Format("\t\t{0}, {1}, {2}, {3},", (object) this.m_rect.Left, (object) this.m_rect.Top, (object) this.m_rect.Right, (object) this.m_rect.Bottom));
        Debug.WriteLine(string.Format("\t\t{0}, {1},", (object) ((int) this.m_lefttop.X).ToString(), (object) ((int) this.m_lefttop.Y).ToString()));
        Debug.WriteLine("\t\t   0,\t   0,\t\t// Invers V , Invers H ");
        Debug.WriteLine("\t\t   1,\t\t\t\t// Term");
        Debug.WriteLine("\t\t   0,\t\t\t\t// Depth_Index");
        Debug.WriteLine("\t\t   0,\t\t// name");
        Debug.WriteLine("\t\t0,\t\t// bank");
        Debug.WriteLine("\t\t   0,\t\t\t\t// optimize");
        Debug.WriteLine("\t\t   0,\t\t\t\t// drawmode");
        Debug.WriteLine("\t\t   0,\t\t\t\t// lock");
        Debug.WriteLine("\t\t   0,\t\t\t\t// mode");
        Debug.WriteLine("\t}");
        Debug.WriteLine("};");
      }
    }
  }
}
