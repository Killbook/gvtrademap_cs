// Type: gvtrademap_cs.transform
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX;
using System;
using System.Drawing;

namespace gvtrademap_cs
{
  public static class transform
  {
    public static Point ToPoint(Vector2 p)
    {
      return new Point((int) p.X, (int) p.Y);
    }

    public static Vector2 ToVector2(Point p)
    {
      return new Vector2((float) p.X, (float) p.Y);
    }

    public static Vector2 ToVector2(Size p)
    {
      return new Vector2((float) p.Width, (float) p.Height);
    }

    public static Vector3 ToVector3(Vector2 p, float z)
    {
      return new Vector3(p.X, p.Y, z);
    }

    public static PointF ToPointF(Vector2 p)
    {
      return new PointF(p.X, p.Y);
    }

    public static Vector2 ToVector2(PointF p)
    {
      return new Vector2(p.X, p.Y);
    }

    public static Vector2 client_pos2_game_pos(Vector2 pos, LoopXImage loop_image)
    {
      return transform.map_pos2_game_pos(loop_image.MousePos2GlobalPos(pos), loop_image);
    }

    public static Point client_pos2_game_pos(Point pos, LoopXImage loop_image)
    {
      return transform.ToPoint(transform.map_pos2_game_pos(loop_image.MousePos2GlobalPos(transform.ToVector2(pos)), loop_image));
    }

    public static Vector2 map_pos2_game_pos(Vector2 pos, LoopXImage loop_image)
    {
      if ((double) pos.X < 0.0)
        pos.X = 0.0f;
      if ((double) pos.Y < 0.0)
        pos.Y = 0.0f;
      if ((double) pos.Y >= (double) loop_image.ImageSize.Y)
        pos.Y = loop_image.ImageSize.Y - 1f;
      pos.X = pos.X - (float) (int) ((double) pos.X / (double) loop_image.ImageSize.X) * loop_image.ImageSize.X;
      pos.X = pos.X * transform.get_rate_to_game_x(loop_image);
      pos.Y = pos.Y * transform.get_rate_to_game_y(loop_image);
      return pos;
    }

    public static Point map_pos2_game_pos(Point pos, LoopXImage loop_image)
    {
      return transform.ToPoint(transform.map_pos2_game_pos(transform.ToVector2(pos), loop_image));
    }

    public static Vector2 game_pos2_map_pos(Vector2 pos, LoopXImage loop_image)
    {
      if ((double) pos.X < 0.0)
        pos.X = 0.0f;
      if ((double) pos.Y < 0.0)
        pos.Y = 0.0f;
      if ((double) pos.Y >= 8192.0)
        pos.Y = 8191f;
      pos.X = pos.X - (float) ((int) ((double) pos.X / 16384.0) * 16384);
      pos.X = pos.X * transform.get_rate_to_map_x(loop_image);
      pos.Y = pos.Y * transform.get_rate_to_map_y(loop_image);
      return pos;
    }

    public static Point game_pos2_map_pos(Point pos, LoopXImage loop_image)
    {
      return transform.ToPoint(transform.game_pos2_map_pos(transform.ToVector2(pos), loop_image));
    }

    public static float get_rate_to_map_x(LoopXImage loop_image)
    {
      return loop_image.ImageSize.X / 16384f;
    }

    public static float get_rate_to_map_y(LoopXImage loop_image)
    {
      return loop_image.ImageSize.Y / 8192f;
    }

    public static float get_rate_to_game_x(LoopXImage loop_image)
    {
      return 16384f / loop_image.ImageSize.X;
    }

    public static float get_rate_to_game_y(LoopXImage loop_image)
    {
      return 8192f / loop_image.ImageSize.Y;
    }

    public static Vector2 SubVector_LoopX(Point v0, Point v1, int size_x)
    {
      float valueY = (float) (v0.Y - v1.Y);
      float valueX = (float) (v0.X - v1.X);
      float num1 = (float) (v0.X - size_x - v1.X);
      float num2 = (float) (v0.X + size_x - v1.X);
      float num3 = Math.Abs(valueX);
      float num4 = Math.Abs(num1);
      float num5 = Math.Abs(num2);
      if ((double) num3 < (double) num4)
      {
        if ((double) num3 > (double) num5)
          valueX = num2;
      }
      else
      {
        float num6 = num4;
        valueX = num1;
        if ((double) num6 > (double) num5)
          valueX = num2;
      }
      return new Vector2(valueX, valueY);
    }
  }
}
