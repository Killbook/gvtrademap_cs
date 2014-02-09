// Type: gvtrademap_cs.latitude_longitude
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using System.Drawing;

namespace gvtrademap_cs
{
  internal static class latitude_longitude
  {
    public static void DrawLines(gvt_lib lib)
    {
      lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(latitude_longitude.draw_lines_proc), 0.0f);
      LoopXImage loopImage = lib.loop_image;
      Vector2 clientSize = loopImage.Device.client_size;
      Vector2 drawOffset = loopImage.GetDrawOffset();
      int num = 0;
      float valueY = 0.0f;
      while ((double) valueY < 8192.0)
      {
        Vector2 global_pos = transform.game_pos2_map_pos(new Vector2(0.0f, valueY), loopImage);
        Vector2 vector2 = loopImage.GlobalPos2LocalPos(global_pos, drawOffset);
        if ((double) vector2.Y >= 0.0 && (double) vector2.Y < (double) clientSize.Y)
          loopImage.Device.DrawLine(new Vector3(0.0f, vector2.Y, 0.79f), new Vector2(clientSize.X, vector2.Y), Color.FromArgb(128, 0, 0, 0).ToArgb());
        valueY += 1000f;
        ++num;
      }
    }

    private static void draw_lines_proc(Vector2 offset, LoopXImage image)
    {
      Vector2 clientSize = image.Device.client_size;
      int num = 0;
      float valueX = 0.0f;
      while ((double) valueX < 16384.0)
      {
        Vector2 global_pos = transform.game_pos2_map_pos(new Vector2(valueX, 0.0f), image);
        Vector2 vector2 = image.GlobalPos2LocalPos(global_pos, offset);
        if ((double) vector2.X >= 0.0 && (double) vector2.X < (double) clientSize.X)
          image.Device.DrawLine(new Vector3(vector2.X, 0.0f, 0.79f), new Vector2(vector2.X, clientSize.Y), Color.FromArgb(128, 0, 0, 0).ToArgb());
        valueX += 1000f;
        ++num;
      }
    }

    public static void DrawLines100(gvt_lib lib)
    {
      lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(latitude_longitude.draw_lines100_proc), 0.0f);
      LoopXImage loopImage = lib.loop_image;
      Vector2 clientSize = loopImage.Device.client_size;
      Vector2 drawOffset = loopImage.GetDrawOffset();
      int num = 0;
      float valueY = 0.0f;
      while ((double) valueY < 8192.0)
      {
        Vector2 global_pos = transform.game_pos2_map_pos(new Vector2(0.0f, valueY), loopImage);
        Vector2 vector2 = loopImage.GlobalPos2LocalPos(global_pos, drawOffset);
        if (num >= 10)
          num = 0;
        if ((double) vector2.Y >= 0.0 && (double) vector2.Y < (double) clientSize.Y)
        {
          int color = num == 0 ? Color.FromArgb(128, 0, 0, 0).ToArgb() : Color.FromArgb(128, 128, 128, 128).ToArgb();
          loopImage.Device.DrawLine(new Vector3(0.0f, vector2.Y, 0.79f), new Vector2(clientSize.X, vector2.Y), color);
        }
        valueY += 100f;
        ++num;
      }
    }

    private static void draw_lines100_proc(Vector2 offset, LoopXImage image)
    {
      Vector2 clientSize = image.Device.client_size;
      int num = 0;
      float valueX = 0.0f;
      while ((double) valueX < 16384.0)
      {
        Vector2 global_pos = transform.game_pos2_map_pos(new Vector2(valueX, 0.0f), image);
        Vector2 vector2 = image.GlobalPos2LocalPos(global_pos, offset);
        if (num >= 10)
          num = 0;
        if ((double) vector2.X >= 0.0 && (double) vector2.X < (double) clientSize.X)
        {
          int color = num == 0 ? Color.FromArgb(128, 0, 0, 0).ToArgb() : Color.FromArgb(128, 128, 128, 128).ToArgb();
          image.Device.DrawLine(new Vector3(vector2.X, 0.0f, 0.79f), new Vector2(vector2.X, clientSize.Y), color);
        }
        valueX += 100f;
        ++num;
      }
    }

    public static void DrawPoints(gvt_lib lib)
    {
      d3d_systemfont systemfont = lib.device.systemfont;
      systemfont.Begin();
      lib.device.device.RenderState.ZBufferEnable = false;
      lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(latitude_longitude.draw_points_proc), 0.0f);
      Vector2 clientSize = lib.loop_image.Device.client_size;
      Vector2 drawOffset = lib.loop_image.GetDrawOffset();
      float valueY = 0.0f;
      while ((double) valueY < 8192.0)
      {
        Vector2 global_pos = transform.game_pos2_map_pos(new Vector2(0.0f, valueY), lib.loop_image);
        Vector2 vector2 = lib.loop_image.GlobalPos2LocalPos(global_pos, drawOffset);
        Rectangle rectangle = systemfont.MeasureText(valueY.ToString());
        vector2.Y -= 5f;
        if ((double) vector2.Y + (double) (rectangle.Height + 4) >= 0.0 && (double) vector2.Y < (double) clientSize.Y)
        {
          lib.device.DrawFillRect(new Vector3((float) ((double) clientSize.X - (double) rectangle.Width - 2.0), vector2.Y - 1f, 0.1f), new Vector2((float) (rectangle.Width + 4), (float) rectangle.Height), Color.FromArgb(220, 100, 100, 100).ToArgb());
          systemfont.DrawTextR(valueY.ToString(), (int) clientSize.X, (int) vector2.Y, Color.White);
        }
        valueY += 1000f;
      }
      systemfont.End();
      lib.device.device.RenderState.ZBufferEnable = true;
    }

    private static void draw_points_proc(Vector2 offset, LoopXImage image)
    {
      d3d_systemfont systemfont = image.Device.systemfont;
      Vector2 clientSize = image.Device.client_size;
      float valueX = 0.0f;
      while ((double) valueX < 16384.0)
      {
        Vector2 global_pos = transform.game_pos2_map_pos(new Vector2(valueX, 0.0f), image);
        Vector2 vector2 = image.GlobalPos2LocalPos(global_pos, offset);
        Rectangle rectangle = systemfont.MeasureText(valueX.ToString());
        if ((double) vector2.X + (double) (rectangle.Width + 4) >= 0.0 && (double) vector2.X < (double) clientSize.X)
        {
          image.Device.DrawFillRect(new Vector3((float) ((double) vector2.X - (double) (rectangle.Width / 2) - 3.0), 0.0f, 0.1f), new Vector2((float) (rectangle.Width + 4), (float) rectangle.Height), Color.FromArgb(220, 100, 100, 100).ToArgb());
          systemfont.DrawTextC(valueX.ToString(), (int) vector2.X, 0, Color.White);
        }
        valueX += 1000f;
      }
    }
  }
}
