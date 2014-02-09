// Type: gvtrademap_cs.LoopXImage
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Utility;

namespace gvtrademap_cs
{
  public class LoopXImage : IDisposable
  {
    private const int TEXSIZE_STEP = 512;
    private const int HEIGHT_MARGIN = 200;
    private const float SCALE_MIN = 0.3f;
    private const float SCALE_MAX = 3f;
    private d3d_device m_device;
    private Vector2 m_image_size;
    private List<TextureUnit> m_textures;
    private Vector2 m_offset;
    private float m_scale;
    private Vector2 m_offset_shelter;
    private float m_scale_shelter;
    private bool m_is_pushed_params;
    private int m_load_current;
    private int m_load_max;
    private string m_load_str;
    private bool m_device_lost;
    public int MargeImageMS;

    public Vector2 OffsetPosition
    {
      get
      {
        return this.m_offset;
      }
      set
      {
        this.m_offset = value;
      }
    }

    public Vector2 ImageSize
    {
      get
      {
        return this.m_image_size;
      }
    }

    public float ImageScale
    {
      get
      {
        return this.m_scale;
      }
    }

    public int LoadCurrent
    {
      get
      {
        return this.m_load_current;
      }
    }

    public int LoadMax
    {
      get
      {
        return this.m_load_max;
      }
    }

    public string LoadStr
    {
      get
      {
        return this.m_load_str;
      }
    }

    public d3d_device Device
    {
      get
      {
        return this.m_device;
      }
    }

    public LoopXImage(d3d_device device)
    {
      this.m_device = device;
      this.m_offset = new Vector2(0.0f, 0.0f);
      this.m_textures = new List<TextureUnit>();
      this.m_is_pushed_params = false;
      this.m_scale = 1f;
      this.m_offset_shelter = new Vector2(0.0f, 0.0f);
      this.m_scale_shelter = 1f;
      this.m_device_lost = false;
      if (!(this.m_device.device != (Device) null))
        return;
      this.m_device.device.DeviceReset += new EventHandler(this.device_reset);
    }

    public void AddOffset(Vector2 add_offset)
    {
      this.m_offset += add_offset * (1f / this.ImageScale);
    }

    public void SetScale(float scale, Point center_pos, bool is_center_mouse)
    {
      if ((double) scale < 0.300000011920929)
        scale = 0.3f;
      if ((double) scale > 3.0)
        scale = 3f;
      if ((double) scale > 0.999999 && (double) scale < 1.000001)
        scale = 1f;
      if (is_center_mouse)
      {
        Vector2 vector2 = new Vector2((float) center_pos.X, (float) center_pos.Y);
        this.m_offset -= vector2 * (1f / this.m_scale) - vector2 * (1f / scale);
        this.m_scale = scale;
      }
      else
        this.m_scale = scale;
    }

    public void MoveCenterOffset(Point center)
    {
      this.MoveCenterOffset(center, new Point(0, 0));
    }

    public void MoveCenterOffset(Point center, Point offset)
    {
      Vector2 vector2_1 = new Vector2((float) center.X, (float) center.Y);
      Vector2 vector2_2 = new Vector2((float) (((double) this.m_device.client_size.X - (double) offset.X) / 2.0), (float) (((double) this.m_device.client_size.Y - (double) offset.Y) / 2.0));
      Vector2 vector2_3 = new Vector2((float) offset.X, (float) offset.Y) * (1f / this.ImageScale);
      Vector2 vector2_4 = vector2_2 * (1f / this.ImageScale);
      this.m_offset = -(vector2_1 - vector2_4 - vector2_3);
    }

    private void device_reset(object sender, EventArgs e)
    {
      this.m_device_lost = true;
    }

    public void InitializeCreateImage()
    {
      this.m_load_current = 0;
      this.m_load_max = 0;
      this.m_load_str = "";
    }

    public bool CreateImage(string file_name)
    {
      this.Dispose();
      this.InitializeCreateImage();
      try
      {
        this.m_load_str = file_name;
        Bitmap bitmap = new Bitmap(file_name);
        this.m_image_size = new Vector2((float) bitmap.Width, (float) bitmap.Height);
        int num1 = bitmap.Width / 512;
        if (bitmap.Width % 512 != 0)
          ++num1;
        int num2 = bitmap.Height / 512;
        if (bitmap.Height % 512 != 0)
          ++num2;
        this.m_load_max = num1 * num2;
        BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppRgb565);
        IntPtr scan0 = bitmapdata.Scan0;
        int length = bitmapdata.Height * bitmapdata.Stride;
        int stride = bitmapdata.Stride;
        byte[] image = new byte[length];
        Marshal.Copy(scan0, image, 0, length);
        bitmap.UnlockBits(bitmapdata);
        bitmap.Dispose();
        this.m_load_str = "テクスチャ転送中...";
        int y = 0;
        while ((double) y < (double) this.m_image_size.Y)
        {
          int x = 0;
          while ((double) x < (double) this.m_image_size.X)
          {
            TextureUnit textureUnit = new TextureUnit();
            textureUnit.Create(this.m_device, ref image, new Size((int) this.m_image_size.X, (int) this.m_image_size.Y), stride, new Rectangle(x, y, 512, 512));
            this.m_textures.Add(textureUnit);
            ++this.m_load_current;
            x += 512;
          }
          y += 512;
        }
        this.m_load_str = "完了";
        GC.Collect();
      }
      catch
      {
        return false;
      }
      return true;
    }

    public void MergeImage(LoopXImage.DrawHandler handler, bool must_merge)
    {
      if (!this.m_device_lost && !must_merge)
        return;
      DateTimer dateTimer = new DateTimer();
      foreach (TextureUnit textureUnit in this.m_textures)
        textureUnit.RefreshTexture();
      if (handler == null)
      {
        this.m_device_lost = false;
        this.MargeImageMS = dateTimer.GetSectionTimeMilliseconds();
      }
      else
      {
        Surface surface = this.m_device.device.DepthStencilSurface;
        Surface backBuffer = this.m_device.device.GetBackBuffer(0, 0, BackBufferType.Mono);
        this.m_device.device.DepthStencilSurface = (Surface) null;
        try
        {
          foreach (TextureUnit textureUnit in this.m_textures)
          {
            if (!(textureUnit.Texture == (Texture) null))
            {
              this.m_device.device.SetRenderTarget(0, textureUnit.Texture.GetSurfaceLevel(0));
              this.m_device.UpdateClientSize();
              this.PushDrawParams();
              this.m_offset = -textureUnit.Offset;
              this.SetScale(1f, new Point(0, 0), false);
              handler(this.m_offset, this);
              this.PopDrawParams();
            }
          }
        }
        catch
        {
          this.PopDrawParams();
        }
        this.m_device.device.DepthStencilSurface = surface;
        this.m_device.device.SetRenderTarget(0, backBuffer);
        this.m_device.UpdateClientSize();
        backBuffer.Dispose();
        surface.Dispose();
        this.m_device_lost = false;
        this.MargeImageMS = dateTimer.GetSectionTimeMilliseconds();
      }
    }

    public void Draw()
    {
      if ((double) this.m_image_size.X <= 0.0 || (double) this.m_image_size.Y <= 0.0)
        return;
      this.m_device.device.RenderState.ZBufferEnable = false;
      this.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_proc), 0.0f);
      this.m_device.device.RenderState.ZBufferEnable = true;
    }

    private void ajust_draw_start_offset_x(float outside_length_x)
    {
      while ((double) this.m_offset.X > -(double) outside_length_x)
        this.m_offset.X -= this.m_image_size.X;
      while ((double) this.m_offset.X <= -((double) this.m_image_size.X + (double) outside_length_x))
        this.m_offset.X += this.m_image_size.X;
    }

    private void draw_proc(Vector2 offset, LoopXImage image)
    {
      Vector3 pos = new Vector3(offset.X, offset.Y, 0.9f);
      foreach (TextureUnit textureUnit in this.m_textures)
        textureUnit.Draw(pos, this.ImageScale);
    }

    public void Dispose()
    {
      foreach (TextureUnit textureUnit in this.m_textures)
        textureUnit.Dispose();
      this.m_textures.Clear();
    }

    public Point MousePos2GlobalPos(Point mouse_pos)
    {
      return transform.ToPoint(this.MousePos2GlobalPos(transform.ToVector2(mouse_pos)));
    }

    public Vector2 MousePos2GlobalPos(Vector2 mouse_pos)
    {
      mouse_pos *= 1f / this.ImageScale;
      Vector2 vector2 = mouse_pos - this.m_offset;
      while ((double) vector2.X >= (double) this.m_image_size.X)
        vector2.X -= this.m_image_size.X;
      return vector2;
    }

    public Vector2 GetDrawOffset()
    {
      Vector2 offsetPosition = this.OffsetPosition;
      offsetPosition.X = (float) (int) offsetPosition.X;
      offsetPosition.Y = (float) (int) offsetPosition.Y;
      return offsetPosition;
    }

    public Vector2 GlobalPos2LocalPos(Vector2 global_pos)
    {
      global_pos += this.GetDrawOffset();
      global_pos *= this.ImageScale;
      return global_pos;
    }

    public Vector2 AjustLocalPos(Vector2 pos)
    {
      if ((double) pos.X > 0.0)
      {
        while ((double) pos.X >= (double) this.m_device.device.Viewport.Width)
          pos.X -= this.m_image_size.X * this.ImageScale;
      }
      else
      {
        while ((double) pos.X < 0.0)
          pos.X += this.m_image_size.X * this.ImageScale;
      }
      return pos;
    }

    public Vector2 GlobalPos2LocalPos(Vector2 global_pos, Vector2 _offset)
    {
      global_pos += _offset;
      global_pos *= this.ImageScale;
      return global_pos;
    }

    public void EnumDrawCallBack(LoopXImage.DrawHandler handler, float outside_offset_x)
    {
      if (handler == null)
        return;
      this.ajust_draw_start_offset_x(outside_offset_x);
      Vector2 drawOffset = this.GetDrawOffset();
      do
      {
        handler(drawOffset, this);
        drawOffset.X += this.ImageSize.X;
      }
      while ((double) drawOffset.X < 1.0 / (double) this.ImageScale * (double) this.m_device.client_size.X + (double) outside_offset_x);
    }

    public void PushDrawParams()
    {
      this.m_offset_shelter = this.m_offset;
      this.m_scale_shelter = this.m_scale;
      this.m_is_pushed_params = true;
    }

    public void PopDrawParams()
    {
      if (!this.m_is_pushed_params)
        return;
      this.m_offset = this.m_offset_shelter;
      this.m_scale = this.m_scale_shelter;
      this.m_is_pushed_params = false;
    }

    public delegate void DrawHandler(Vector2 draw_offset, LoopXImage image);
  }
}
