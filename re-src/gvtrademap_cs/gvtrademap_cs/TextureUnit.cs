// Type: gvtrademap_cs.TextureUnit
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Drawing;

namespace gvtrademap_cs
{
  internal class TextureUnit : IDisposable
  {
    private d3d_device m_device;
    private Texture m_texture;
    private Texture m_texture_sysmem;
    private Vector2 m_offset;
    private Size m_size;
    private Size m_texture_size;

    public bool IsCreate
    {
      get
      {
        return this.m_texture != (Texture) null;
      }
    }

    public Texture Texture
    {
      get
      {
        return this.m_texture;
      }
    }

    public Vector2 Offset
    {
      get
      {
        return this.m_offset;
      }
    }

    public TextureUnit()
    {
      this.m_device = (d3d_device) null;
      this.m_texture = (Texture) null;
      this.m_texture_sysmem = (Texture) null;
    }

    public virtual void Create(d3d_device device, ref byte[] image, Size size, int stride, Rectangle rect)
    {
      this.m_device = device;
      this.m_offset.X = (float) rect.X;
      this.m_offset.Y = (float) rect.Y;
      this.m_size.Width = rect.Width;
      this.m_size.Height = rect.Height;
      if (this.m_size.Width + rect.X > size.Width)
        this.m_size.Width = size.Width - rect.X;
      if (this.m_size.Height + rect.Y > size.Height)
        this.m_size.Height = size.Height - rect.Y;
      this.m_texture_size = d3d_utility.TextureSizePow2(this.m_size);
      if (this.m_texture_sysmem != (Texture) null)
        this.m_texture_sysmem.Dispose();
      this.m_texture_sysmem = new Texture(this.m_device.device, this.m_texture_size.Width, this.m_texture_size.Height, 1, Usage.Dynamic, Format.R5G6B5, Pool.SystemMemory);
      ushort[,] numArray = (ushort[,]) this.m_texture_sysmem.LockRectangle(typeof (ushort), 0, LockFlags.Discard, new int[2]
      {
        this.m_texture_size.Height,
        this.m_texture_size.Width
      });
      int num1 = stride * rect.Y + rect.X * 2;
      for (int index1 = 0; index1 < this.m_size.Height; ++index1)
      {
        ushort num2 = (ushort) 0;
        for (int index2 = 0; index2 < this.m_size.Width; ++index2)
        {
          num2 = (ushort) ((uint) image[num1 + index2 * 2 + 1] << 8 | (uint) image[num1 + index2 * 2]);
          numArray[index1, index2] = num2;
        }
        if (this.m_texture_size.Width > this.m_size.Width)
          numArray[index1, this.m_size.Width] = num2;
        num1 += stride;
      }
      this.m_texture_sysmem.UnlockRectangle(0);
      if (this.m_texture != (Texture) null)
        this.m_texture.Dispose();
      this.RefreshTexture();
    }

    public virtual void CreateFromMask(d3d_device device, ref byte[] image, Size size, int stride, Rectangle rect)
    {
      this.m_device = device;
      this.m_offset.X = (float) rect.X;
      this.m_offset.Y = (float) rect.Y;
      this.m_size.Width = rect.Width;
      this.m_size.Height = rect.Height;
      if (this.m_size.Width + rect.X > size.Width)
        this.m_size.Width = size.Width - rect.X;
      if (this.m_size.Height + rect.Y > size.Height)
        this.m_size.Height = size.Height - rect.Y;
      this.m_texture_size = this.m_size;
      using (Texture src_texture = new Texture(this.m_device.device, this.m_size.Width, this.m_size.Height, 1, Usage.Dynamic, Format.A1R5G5B5, Pool.SystemMemory))
      {
        ushort[,] numArray = (ushort[,]) src_texture.LockRectangle(typeof (ushort), 0, LockFlags.Discard, new int[2]
        {
          this.m_size.Height,
          this.m_size.Width
        });
        int num1 = stride * rect.Y + rect.X * 2;
        for (int index1 = 0; index1 < this.m_size.Height; ++index1)
        {
          for (int index2 = 0; index2 < this.m_size.Width; ++index2)
          {
            ushort num2 = (ushort) ((uint) image[num1 + index2 * 2 + 1] << 8 | (uint) image[num1 + index2 * 2]);
            if (((int) num2 & 31) < 16 || ((int) num2 >> 5 & 63) < 48 || ((int) num2 >> 11 & 31) < 16)
              num2 = (ushort) short.MaxValue;
            numArray[index1, index2] = num2;
          }
          num1 += stride;
        }
        src_texture.UnlockRectangle(0);
        if (this.m_texture != (Texture) null)
          this.m_texture.Dispose();
        this.m_texture = d3d_utility.CreateTextureFromTexture(this.m_device.device, src_texture);
      }
    }

    public void Dispose()
    {
      if (this.m_texture != (Texture) null)
      {
        this.m_texture.Dispose();
        this.m_texture = (Texture) null;
      }
      if (!(this.m_texture_sysmem != (Texture) null))
        return;
      this.m_texture_sysmem.Dispose();
      this.m_texture_sysmem = (Texture) null;
    }

    public void RefreshTexture()
    {
      if (this.m_texture_sysmem == (Texture) null)
        return;
      if (this.m_texture != (Texture) null)
        this.m_texture.Dispose();
      this.m_texture = d3d_utility.CreateRenderTargetTextureSameSize(this.m_device.device, this.m_texture_sysmem);
      d3d_utility.CopyTexture(this.m_device.device, this.m_texture, this.m_texture_sysmem);
    }

    public void Draw(Vector3 pos, float scale)
    {
      this.Draw(pos, scale, Color.White.ToArgb());
    }

    public void Draw(Vector3 pos, float scale, int color)
    {
      if (this.m_texture == (Texture) null)
        return;
      Vector2 vector2 = this.m_offset * scale;
      Vector2 size = new Vector2(scale * (float) this.m_texture_size.Width, scale * (float) this.m_texture_size.Height);
      Vector2 clientSize = this.m_device.client_size;
      pos.X *= scale;
      pos.Y *= scale;
      pos.X += vector2.X;
      pos.Y += vector2.Y;
      if ((double) pos.X + (double) size.X < 0.0 || (double) pos.Y + (double) size.Y < 0.0 || ((double) pos.X > (double) clientSize.X || (double) pos.Y > (double) clientSize.Y))
        return;
      this.m_device.DrawTexture(this.m_texture, pos, size, color);
    }
  }
}
