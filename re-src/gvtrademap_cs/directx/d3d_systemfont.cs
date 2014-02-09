// Type: directx.d3d_systemfont
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace directx
{
  public class d3d_systemfont : IDisposable
  {
    private const int HEIGHT = 12;
    private const int DEF_WIDTH = 5;
    private d3d_device m_d3d_device;
    private d3d_sprite_rects m_sprite_rects;
    private d3d_sprite m_sprite;
    private int[] m_width_tbl;
    private Vector3 m_position;
    private float m_return_x;

    public Vector3 locate
    {
      get
      {
        return this.m_position;
      }
      set
      {
        this.m_position = value;
        this.m_return_x = this.m_position.X;
      }
    }

    public Texture texture
    {
      get
      {
        return this.m_sprite_rects.texture;
      }
    }

    public int sprite_count
    {
      get
      {
        return this.m_sprite.draw_sprites_in_frame;
      }
    }

    public d3d_systemfont(d3d_device device)
    {
      if (device == null)
        return;
      Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("directx.image.systemfont_8x16_ui_gothic.dds");
      this.m_sprite_rects = new d3d_sprite_rects(device, manifestResourceStream);
      this.initialize(device);
    }

    public void Dispose()
    {
      if (this.m_sprite_rects != null)
        this.m_sprite_rects.Dispose();
      if (this.m_sprite != null)
        this.m_sprite.Dispose();
      this.m_sprite_rects = (d3d_sprite_rects) null;
      this.m_sprite = (d3d_sprite) null;
    }

    private void initialize(d3d_device device)
    {
      this.m_d3d_device = device;
      if (device.device == (Device) null)
        return;
      this.m_sprite = new d3d_sprite(device.device, device.is_use_ve1_1_ps1_1);
      this.init_rects();
      this.locate = new Vector3(0.0f, 0.0f, 0.1f);
    }

    private void init_rects()
    {
      Vector2 offset = new Vector2(0.0f, 0.0f);
      for (int index1 = 0; index1 < 6; ++index1)
      {
        for (int index2 = 0; index2 < 16; ++index2)
          this.m_sprite_rects.AddRect(offset, new Rectangle(index2 * 8, index1 * 16, 8, 12));
      }
      this.m_width_tbl = new int[96]
      {
        6,
        4,
        6,
        7,
        6,
        7,
        7,
        4,
        4,
        4,
        6,
        6,
        3,
        6,
        3,
        7,
        6,
        6,
        6,
        6,
        6,
        6,
        6,
        6,
        6,
        6,
        4,
        4,
        6,
        6,
        6,
        6,
        8,
        7,
        7,
        8,
        7,
        6,
        6,
        8,
        7,
        4,
        6,
        6,
        5,
        8,
        7,
        8,
        6,
        8,
        8,
        7,
        8,
        7,
        9,
        8,
        8,
        8,
        7,
        4,
        6,
        4,
        6,
        5,
        4,
        6,
        6,
        6,
        6,
        6,
        4,
        6,
        6,
        2,
        3,
        6,
        2,
        8,
        6,
        6,
        6,
        6,
        4,
        6,
        4,
        6,
        6,
        8,
        6,
        6,
        5,
        4,
        2,
        4,
        6,
        6
      };
    }

    public void BeginFrame()
    {
      this.m_sprite.BeginFrame();
    }

    public Rectangle MeasureText(string text)
    {
      Rectangle rectangle = new Rectangle();
      rectangle.X = 0;
      rectangle.Y = 0;
      rectangle.Height = 12;
      rectangle.Width = 0;
      foreach (int num in text)
      {
        int index = num - 32;
        if (index > 0 && index <= 96)
          rectangle.Width += this.m_width_tbl[index];
        else
          rectangle.Width += 5;
      }
      return rectangle;
    }

    public void Begin()
    {
      this.m_sprite.BeginDrawSprites(this.m_sprite_rects.texture);
    }

    public void End()
    {
      this.m_sprite.EndDrawSprites();
    }

    public void DrawTextR(string text, int x, int y, Color color)
    {
      this.DrawTextR(text, x, y, 0.0f, color);
    }

    public void DrawTextC(string text, int x, int y, Color color)
    {
      this.DrawTextC(text, x, y, 0.0f, color);
    }

    public void DrawTextR(string text, int x, int y, float z, Color color)
    {
      Rectangle rectangle = this.MeasureText(text);
      this.DrawText(text, x - rectangle.Width, y, z, color);
    }

    public void DrawTextC(string text, int x, int y, float z, Color color)
    {
      Rectangle rectangle = this.MeasureText(text);
      this.DrawText(text, x - rectangle.Width / 2, y, z, color);
    }

    public void DrawText(string text, int x, int y, Color color)
    {
      this.DrawText(text, x, y, 0.0f, color);
    }

    public void DrawText(string text, int x, int y, float z, Color color)
    {
      Vector3 pos = new Vector3((float) x, (float) y, z);
      int color1 = color.ToArgb();
      foreach (int num in text)
      {
        int index = num - 32;
        if (index > 0 && index <= 96)
        {
          this.m_sprite.AddDrawSpritesNC(pos, this.m_sprite_rects.rects[index], color1);
          pos.X += (float) this.m_width_tbl[index];
        }
        else
          pos.X += 5f;
      }
    }

    public void Puts(string text, Color color)
    {
      int color1 = color.ToArgb();
      this.m_sprite.BeginDrawSprites(this.m_sprite_rects.texture);
      foreach (char ch in text)
      {
        int num = (int) ch;
        if ((int) ch == 10)
        {
          this.m_position.Y += 12f;
          this.m_position.X = this.m_return_x;
        }
        else if (num > 32 && num <= 128)
        {
          int index = num - 32;
          this.m_sprite.AddDrawSpritesNC(this.m_position, this.m_sprite_rects.rects[index], color1);
          this.m_position.X += (float) this.m_width_tbl[index];
        }
        else
          this.m_position.X += 5f;
      }
      this.m_sprite.EndDrawSprites();
    }
  }
}
