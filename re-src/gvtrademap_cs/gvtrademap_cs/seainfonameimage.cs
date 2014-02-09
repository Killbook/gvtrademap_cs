// Type: gvtrademap_cs.seainfonameimage
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace gvtrademap_cs
{
  public class seainfonameimage : d3d_sprite_rects
  {
    public seainfonameimage(d3d_device device, string infoimage_fname)
      : base(device, infoimage_fname)
    {
      if (device.device == (Device) null)
        return;
      this.add_rects();
    }

    public d3d_sprite_rects.rect GetWindArrowIcon()
    {
      return this.GetRect(this.rect_count - 2);
    }

    public d3d_sprite_rects.rect GetCityIcon()
    {
      return this.GetRect(this.rect_count - 1);
    }

    private void add_rects()
    {
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 0, 60, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 12, 74, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 24, 22, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 36, 38, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 48, 70, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 60, 70, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 72, 50, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 84, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 96, 42, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 108, 50, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 120, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 132, 50, 24));
      this.AddRect(new Vector2(-5f, -5f), new Rectangle(0, 156, 42, 24));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 180, 46, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 192, 56, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 204, 48, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 216, 46, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 228, 42, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 240, 22, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 252, 50, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 264, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 276, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 288, 42, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 300, 42, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 312, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 324, 40, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 336, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 348, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 360, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 372, 82, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 384, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 396, 70, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 408, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 420, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 432, 22, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 444, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 456, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 468, 60, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 480, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(0, 492, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 0, 54, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 12, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 24, 80, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 36, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 48, 82, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 60, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 72, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 84, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 96, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 108, 72, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 120, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 132, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 144, 90, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 156, 82, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 168, 72, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 180, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 192, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 204, 72, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 216, 40, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 228, 42, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 240, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 252, 42, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 264, 72, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 276, 72, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 288, 102, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 300, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 312, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 324, 102, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 336, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 348, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 360, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 372, 72, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 384, 72, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 396, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 408, 72, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 420, 52, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 432, 42, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 444, 72, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 456, 82, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 468, 72, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 480, 82, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(84, 492, 72, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(180, 0, 42, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(168, 12, 82, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(180, 24, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(180, 36, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(180, 48, 62, 12));
      this.AddRect(new Vector2(-5f, -6f), new Rectangle(180, 60, 72, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(180, 72, 72, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(168, 84, 84, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(180, 96, 60, 12));
      this.AddRect(new Vector2(-4f, -7f), new Rectangle(240, 492, 10, 12));
      this.AddRect(new Vector2(-2f, -2f), new Rectangle(240, 480, 6, 6));
    }
  }
}
