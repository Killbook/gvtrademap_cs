// Type: directx.d3d_textured_font
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace directx
{
  public class d3d_textured_font : IDisposable
  {
    private d3d_device m_device;
    private Microsoft.DirectX.Direct3D.Font m_font;
    private Dictionary<string, d3d_textured_font.textured_font> m_map;
    private Format m_texture_format;

    public int cash_count
    {
      get
      {
        return this.m_map.Count;
      }
    }

    public d3d_textured_font(d3d_device device, Microsoft.DirectX.Direct3D.Font font)
    {
      this.m_device = device;
      this.m_font = font;
      this.m_map = new Dictionary<string, d3d_textured_font.textured_font>();
      this.m_device.device.DeviceReset += new EventHandler(this.device_reset);
      this.m_texture_format = Format.A1R5G5B5;
      if (this.m_device.CheckDeviceFormat(Usage.RenderTarget, ResourceType.Textures, Format.A1R5G5B5))
        return;
      this.m_texture_format = Format.A8R8G8B8;
    }

    private void device_reset(object sender, EventArgs e)
    {
      this.Clear();
    }

    public void Clear()
    {
      foreach (d3d_textured_font.textured_font texturedFont in this.m_map.Values)
        texturedFont.Dispose();
      this.m_map.Clear();
    }

    public void Dispose()
    {
      if (this.m_map != null)
        this.Clear();
      this.m_map = (Dictionary<string, d3d_textured_font.textured_font>) null;
    }

    private d3d_textured_font.textured_font get_textured_font(string str)
    {
      d3d_textured_font.textured_font texturedFont1 = (d3d_textured_font.textured_font) null;
      if (this.m_map.TryGetValue(str, out texturedFont1))
      {
        ++texturedFont1.ref_count;
        return texturedFont1;
      }
      else
      {
        d3d_textured_font.textured_font texturedFont2 = new d3d_textured_font.textured_font(this.m_device, str, this.m_font, this.m_texture_format);
        this.m_map.Add(str, texturedFont2);
        return texturedFont2;
      }
    }

    private bool is_created_textured_font(string str)
    {
      d3d_textured_font.textured_font texturedFont = (d3d_textured_font.textured_font) null;
      return this.m_map.TryGetValue(str, out texturedFont);
    }

    public Rectangle MeasureText(string str, Color color)
    {
      if (!this.is_created_textured_font(str))
        return this.m_font.MeasureString((Sprite) null, str, DrawTextFormat.None, color);
      d3d_textured_font.textured_font texturedFont = this.get_textured_font(str);
      return new Rectangle(0, 0, (int) texturedFont.size.X, (int) texturedFont.size.Y);
    }

    public void DrawText(string str, Vector3 pos, Color color)
    {
      d3d_textured_font.textured_font texturedFont = this.get_textured_font(str);
      if (texturedFont.texture == (Texture) null)
        return;
      pos.X = (float) (int) pos.X;
      pos.Y = (float) (int) pos.Y;
      this.m_device.DrawTexture(texturedFont.texture, pos, texturedFont.texture_size, color.ToArgb());
    }

    public void DrawTextR(string str, Vector3 pos, Color color)
    {
      d3d_textured_font.textured_font texturedFont = this.get_textured_font(str);
      if (texturedFont.texture == (Texture) null)
        return;
      pos.X -= texturedFont.size.X;
      pos.X = (float) (int) pos.X;
      pos.Y = (float) (int) pos.Y;
      this.m_device.DrawTexture(texturedFont.texture, pos, texturedFont.texture_size, color.ToArgb());
    }

    public void DrawTextC(string str, Vector3 pos, Color color)
    {
      d3d_textured_font.textured_font texturedFont = this.get_textured_font(str);
      if (texturedFont.texture == (Texture) null)
        return;
      pos.X -= texturedFont.size.X / 2f;
      pos.X = (float) (int) pos.X;
      pos.Y = (float) (int) pos.Y;
      this.m_device.DrawTexture(texturedFont.texture, pos, texturedFont.texture_size, color.ToArgb());
    }

    public class textured_font : IDisposable
    {
      private string m_str;
      private Vector2 m_size;
      private Vector2 m_texture_size;
      private Texture m_texture;
      private int m_ref_count;

      public string str
      {
        get
        {
          return this.str;
        }
      }

      public Vector2 size
      {
        get
        {
          return this.m_size;
        }
      }

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

      public int ref_count
      {
        get
        {
          return this.m_ref_count;
        }
        internal set
        {
          this.m_ref_count = value;
        }
      }

      public textured_font(d3d_device device, string str, Microsoft.DirectX.Direct3D.Font font)
        : this(device, str, font, Format.A8R8G8B8)
      {
      }

      public textured_font(d3d_device device, string str, Microsoft.DirectX.Direct3D.Font font, Format format)
      {
        this.m_ref_count = 0;
        this.m_str = str;
        try
        {
          Rectangle rectangle = font.MeasureString((Sprite) null, str, DrawTextFormat.None, Color.White);
          this.m_size = new Vector2((float) rectangle.Right, (float) rectangle.Bottom);
          int width = (int) this.m_size.X + 3 & -4;
          int height = (int) this.m_size.Y + 3 & -4;
          this.m_texture_size = new Vector2((float) width, (float) height);
          this.m_texture = new Texture(device.device, width, height, 1, Usage.RenderTarget, format, Pool.Default);
        }
        catch
        {
          this.m_size = new Vector2(0.0f, 0.0f);
          this.m_texture_size = new Vector2(0.0f, 0.0f);
          this.m_texture = (Texture) null;
          return;
        }
        Surface surface = device.device.DepthStencilSurface;
        Surface backBuffer = device.device.GetBackBuffer(0, 0, BackBufferType.Mono);
        device.device.DepthStencilSurface = (Surface) null;
        device.device.SetRenderTarget(0, this.m_texture.GetSurfaceLevel(0));
        device.Clear(ClearFlags.Target, Color.FromArgb(0, 0, 0, 0));
        device.device.RenderState.ZBufferEnable = false;
        font.DrawText((Sprite) null, str, new Point(0, 0), Color.White);
        device.device.RenderState.ZBufferEnable = true;
        device.device.DepthStencilSurface = surface;
        device.device.SetRenderTarget(0, backBuffer);
        backBuffer.Dispose();
        surface.Dispose();
      }

      public void Dispose()
      {
        if (this.m_texture != (Texture) null)
          this.m_texture.Dispose();
        this.m_texture = (Texture) null;
      }
    }
  }
}
