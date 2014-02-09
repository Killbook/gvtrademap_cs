// Type: Utility.ScreenCapture
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using win32;

namespace Utility
{
  public class ScreenCapture : IDisposable
  {
    private Bitmap m_bitmap;
    private byte[] m_image;
    private int m_stride;
    private Size m_size;

    public byte[] Image
    {
      get
      {
        return this.m_image;
      }
    }

    public int Stride
    {
      get
      {
        return this.m_stride;
      }
    }

    public Size Size
    {
      get
      {
        return this.m_size;
      }
    }

    public ScreenCapture(int size_x, int size_y)
    {
      this.m_bitmap = new Bitmap(size_x, size_y);
      this.m_size.Width = size_x;
      this.m_size.Height = size_y;
      this.m_image = (byte[]) null;
      this.m_stride = 0;
    }

    public void DoCapture(IntPtr src_hdc, Point dst_pos, Point src_pos, Size size)
    {
      Graphics graphics = Graphics.FromImage((Image) this.m_bitmap);
      IntPtr hdc = graphics.GetHdc();
      gdi32.BitBlt(hdc, dst_pos.X, dst_pos.Y, size.Width, size.Height, src_hdc, src_pos.X, src_pos.Y, 13369376);
      graphics.ReleaseHdc(hdc);
      graphics.Dispose();
    }

    public void CreateImage()
    {
      BitmapData bitmapdata = this.m_bitmap.LockBits(new Rectangle(0, 0, this.m_size.Width, this.m_size.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
      IntPtr scan0 = bitmapdata.Scan0;
      int length = bitmapdata.Height * bitmapdata.Stride;
      this.m_stride = bitmapdata.Stride;
      this.update_image_buffer(length);
      Marshal.Copy(scan0, this.m_image, 0, length);
      this.m_bitmap.UnlockBits(bitmapdata);
    }

    private void update_image_buffer(int length)
    {
      if (this.m_image == null)
      {
        this.m_image = new byte[length];
      }
      else
      {
        if (this.m_image.Length == length)
          return;
        this.m_image = new byte[length];
      }
    }

    public void Dispose()
    {
      if (this.m_bitmap != null)
        this.m_bitmap.Dispose();
      this.m_bitmap = (Bitmap) null;
    }
  }
}
