// Type: gvtrademap_cs.gvo_capture
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using gvo_base;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Drawing;
using Utility;

namespace gvtrademap_cs
{
  public class gvo_capture : gvo_capture_base
  {
    private gvt_lib m_lib;
    private Texture m_debug_texture;
    private Texture m_debug_texture2;
    private float m_factor;
    private float m_angle_x;
    private float m_l;

    public gvo_capture(gvt_lib lib)
    {
      this.m_lib = lib;
      this.m_debug_texture = (Texture) null;
      this.m_debug_texture2 = (Texture) null;
      this.m_factor = 36f;
      this.m_angle_x = 31.5f;
      this.m_l = 54f;
    }

    protected override ScreenCapture CreateScreenCapture(int size_x, int size_y)
    {
      return (ScreenCapture) new gvo_capture.capture_image(size_x, size_y);
    }

    public override void Dispose()
    {
      this.release_debug_textures();
    }

    private void release_debug_textures()
    {
      if (this.m_debug_texture != (Texture) null)
        this.m_debug_texture.Dispose();
      if (this.m_debug_texture2 != (Texture) null)
        this.m_debug_texture2.Dispose();
      this.m_debug_texture = (Texture) null;
      this.m_debug_texture2 = (Texture) null;
    }

    public override bool CaptureAll()
    {
      if (!base.CaptureAll())
        return false;
      if (this.m_lib.setting.draw_capture_info)
      {
        this.release_debug_textures();
        this.m_debug_texture = ((gvo_capture.capture_image) this.capture2).CreateTexture(this.m_lib.device.device);
        this.m_debug_texture2 = ((gvo_capture.capture_image) this.capture1).CreateTexture(this.m_lib.device.device);
      }
      return true;
    }

    private void create_compass_tbl()
    {
      this.m_compass_pos = new Point[360];
      Matrix sourceMatrix = Matrix.RotationX(Useful.ToRadian(this.m_angle_x)) * Matrix.Translation(0.0f, 0.0f, -100f) * Matrix.PerspectiveFovRH(Useful.ToRadian(60f), 1f, 0.1f, 1000f);
      float valueX = this.m_factor;
      float num1 = 1f / Vector3.TransformCoordinate(new Vector3(valueX, 0.0f, 0.0f), sourceMatrix).X;
      for (int index = 0; index < 360; ++index)
      {
        float num2 = Useful.ToRadian((float) (1.0 * (double) index - 90.0));
        float num3 = (float) Math.Cos((double) num2);
        float num4 = (float) Math.Sin((double) num2);
        float num5 = num1 * this.m_l;
        Vector3 vector3 = Vector3.TransformCoordinate(new Vector3(num3 * valueX, num4 * valueX, 0.0f), sourceMatrix);
        vector3.X *= num5;
        vector3.Y *= num5;
        int num6 = 56;
        int num7 = 39;
        int x = num6 + (int) vector3.X;
        int y = num7 + (int) vector3.Y;
        if (x < 0)
          x = 0;
        if (x >= 128)
          x = (int) sbyte.MaxValue;
        if (y < 0)
          y = 0;
        if (y >= 128)
          y = (int) sbyte.MaxValue;
        this.m_compass_pos[index] = new Point(x, y);
      }
    }

    public void DrawCapturedTexture()
    {
      if (!this.m_lib.setting.draw_capture_info)
        return;
      Vector3 pos1 = new Vector3((float) ((double) this.m_lib.device.client_size.X - 128.0 - 4.0), 4f, 1.0 / 500.0);
      this.m_lib.device.DrawFillRect(pos1, new Vector2(80f, 52f), -1073741824);
      pos1.X += 2f;
      pos1.Y += 2f;
      this.m_lib.device.systemfont.locate = pos1;
      this.m_lib.device.systemfont.Puts(string.Format("index1={0}\nindex2={1}\n", (object) this.m_1st_com_index, (object) this.m_com_index), Color.White);
      this.m_lib.device.systemfont.Puts(string.Format("index3={0}\nquadrant={1}\n", (object) this.m_com_index2, (object) this.m_an_index), Color.White);
      Vector3 pos2 = new Vector3(this.m_lib.device.client_size.X - 4f, 64f, 1.0 / 1000.0);
      this.draw_debug_texture(this.m_debug_texture2, pos2, 1f);
      pos2.Y += 8f + d3d_utility.GetTextureSize(this.m_debug_texture2).Y;
      this.draw_debug_texture(this.m_debug_texture, pos2, 1f);
    }

    private void draw_debug_texture(Texture tex, Vector3 pos, float scale)
    {
      if (tex == (Texture) null)
        return;
      Vector2 textureSize = d3d_utility.GetTextureSize(tex);
      pos.X -= textureSize.X;
      this.m_lib.device.DrawTexture(tex, pos, textureSize * scale);
    }

    private class capture_image : ScreenCapture
    {
      public capture_image(int size_x, int size_y)
        : base(size_x, size_y)
      {
      }

      public Texture CreateTexture(Device device)
      {
        if (this.Image == null)
          return (Texture) null;
        try
        {
          using (Texture src_texture = new Texture(device, this.Size.Width, this.Size.Height, 1, Usage.Dynamic, Format.X8R8G8B8, Pool.SystemMemory))
          {
            uint[,] numArray = (uint[,]) src_texture.LockRectangle(typeof (uint), 0, LockFlags.Discard, new int[2]
            {
              this.Size.Height,
              this.Size.Width
            });
            int num = 0;
            for (int index1 = 0; index1 < this.Size.Height; ++index1)
            {
              for (int index2 = 0; index2 < this.Size.Width; ++index2)
                numArray[index1, index2] = (uint) ((int) this.Image[num + index2 * 3 + 2] << 16 | (int) this.Image[num + index2 * 3 + 1] << 8 | (int) this.Image[num + index2 * 3] | -16777216);
              num += this.Stride;
            }
            src_texture.UnlockRectangle(0);
            return d3d_utility.CreateTextureFromTexture(device, src_texture);
          }
        }
        catch
        {
          return (Texture) null;
        }
      }
    }
  }
}
