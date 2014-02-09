﻿// Type: gvtrademap_cs.infonameimage
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace gvtrademap_cs
{
  public class infonameimage : d3d_sprite_rects
  {
    private const int ICON_START_INDEX = 0;
    private const int CITY_START_INDEX = 12;

    public infonameimage(d3d_device device, string infoimage_fname)
      : base(device, infoimage_fname)
    {
      if (device.device == (Device) null)
        return;
      this.add_rects();
    }

    public d3d_sprite_rects.rect GetIcon(int index)
    {
      if (index < 0)
        return (d3d_sprite_rects.rect) null;
      if (index >= this.rect_count)
        return (d3d_sprite_rects.rect) null;
      else
        return this.GetRect(index);
    }

    public d3d_sprite_rects.rect GetCityName(int index)
    {
      if (index < 0)
        return (d3d_sprite_rects.rect) null;
      if (12 + index >= this.rect_count)
        return (d3d_sprite_rects.rect) null;
      else
        return this.GetRect(12 + index);
    }

    private void add_rects()
    {
      this.AddRect(new Vector2(-12f, -22f), new Rectangle(168, 12, 24, 26));
      this.AddRect(new Vector2(-11f, -14f), new Rectangle(168, 40, 24, 20));
      this.AddRect(new Vector2(-7f, -11f), new Rectangle(72, 60, 14, 16));
      this.AddRect(new Vector2(-5f, -9f), new Rectangle(156, 0, 10, 12));
      this.AddRect(new Vector2(-7f, -11f), new Rectangle(72, 36, 14, 16));
      this.AddRect(new Vector2(-5f, -9f), new Rectangle(168, 0, 10, 12));
      this.AddRect(new Vector2(-2f, -2f), new Rectangle(176, 64, 6, 6));
      this.AddRect(new Vector2(-2f, -2f), new Rectangle(176, 72, 6, 6));
      this.AddRect(new Vector2(-2f, -2f), new Rectangle(176, 80, 6, 6));
      this.AddRect(new Vector2(-2f, -2f), new Rectangle(176, 88, 6, 6));
      this.AddRect(new Vector2(-2f, -2f), new Rectangle(176, 96, 6, 6));
      this.AddRect(new Vector2(-2f, -2f), new Rectangle(176, 104, 6, 6));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 0, 76, 16));
      this.AddRect(new Vector2(-12f, 0.0f), new Rectangle(0, 16, 64, 16));
      this.AddRect(new Vector2(-28f, -16f), new Rectangle(0, 32, 56, 16));
      this.AddRect(new Vector2(0.0f, -16f), new Rectangle(0, 48, 40, 16));
      this.AddRect(new Vector2(-34f, -16f), new Rectangle(0, 64, 68, 16));
      this.AddRect(new Vector2(-76f, -8f), new Rectangle(0, 80, 76, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 96, 60, 16));
      this.AddRect(new Vector2(-57f, -16f), new Rectangle(0, 112, 60, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 128, 56, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 144, 76, 16));
      this.AddRect(new Vector2(0.0f, -8f), new Rectangle(0, 160, 52, 16));
      this.AddRect(new Vector2(-22f, -16f), new Rectangle(0, 176, 44, 16));
      this.AddRect(new Vector2(-14f, 0.0f), new Rectangle(0, 192, 28, 16));
      this.AddRect(new Vector2(-24f, 0.0f), new Rectangle(0, 208, 60, 16));
      this.AddRect(new Vector2(0.0f, -8f), new Rectangle(0, 224, 68, 16));
      this.AddRect(new Vector2(-41f, -15f), new Rectangle(0, 240, 44, 16));
      this.AddRect(new Vector2(-42f, -1f), new Rectangle(0, 256, 44, 16));
      this.AddRect(new Vector2(-49f, -10f), new Rectangle(0, 272, 48, 16));
      this.AddRect(new Vector2(-41f, -16f), new Rectangle(0, 288, 48, 16));
      this.AddRect(new Vector2(-18f, 0.0f), new Rectangle(0, 304, 36, 16));
      this.AddRect(new Vector2(0.0f, -16f), new Rectangle(0, 320, 36, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 336, 48, 16));
      this.AddRect(new Vector2(0.0f, -8f), new Rectangle(0, 352, 48, 16));
      this.AddRect(new Vector2(-11f, 0.0f), new Rectangle(0, 368, 68, 16));
      this.AddRect(new Vector2(-20f, 0.0f), new Rectangle(0, 384, 40, 16));
      this.AddRect(new Vector2(0.0f, -16f), new Rectangle(0, 400, 48, 16));
      this.AddRect(new Vector2(-28f, -16f), new Rectangle(0, 416, 56, 16));
      this.AddRect(new Vector2(-18f, -16f), new Rectangle(0, 432, 40, 16));
      this.AddRect(new Vector2(-28f, -16f), new Rectangle(0, 448, 56, 16));
      this.AddRect(new Vector2(-18f, -1f), new Rectangle(0, 464, 36, 16));
      this.AddRect(new Vector2(-2f, -14f), new Rectangle(0, 480, 36, 16));
      this.AddRect(new Vector2(-22f, 0.0f), new Rectangle(0, 496, 44, 16));
      this.AddRect(new Vector2(-1f, -15f), new Rectangle(0, 512, 36, 16));
      this.AddRect(new Vector2(-28f, -16f), new Rectangle(0, 528, 56, 16));
      this.AddRect(new Vector2(-30f, -1f), new Rectangle(0, 544, 60, 16));
      this.AddRect(new Vector2(-1f, -15f), new Rectangle(0, 560, 48, 16));
      this.AddRect(new Vector2(-44f, -1f), new Rectangle(0, 576, 48, 16));
      this.AddRect(new Vector2(-34f, -15f), new Rectangle(0, 592, 68, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(0, 608, 48, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 624, 40, 16));
      this.AddRect(new Vector2(0.0f, -8f), new Rectangle(0, 640, 48, 16));
      this.AddRect(new Vector2(-45f, -8f), new Rectangle(0, 656, 48, 16));
      this.AddRect(new Vector2(-26f, -16f), new Rectangle(0, 672, 56, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 688, 48, 16));
      this.AddRect(new Vector2(0.0f, -8f), new Rectangle(0, 704, 56, 16));
      this.AddRect(new Vector2(-30f, 0.0f), new Rectangle(0, 720, 36, 16));
      this.AddRect(new Vector2(-14f, -15f), new Rectangle(0, 736, 28, 16));
      this.AddRect(new Vector2(-28f, -16f), new Rectangle(0, 752, 56, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(0, 768, 48, 16));
      this.AddRect(new Vector2(-1f, -2f), new Rectangle(0, 784, 32, 16));
      this.AddRect(new Vector2(-26f, -1f), new Rectangle(0, 800, 52, 16));
      this.AddRect(new Vector2(-41f, -14f), new Rectangle(0, 816, 44, 16));
      this.AddRect(new Vector2(-50f, -1f), new Rectangle(0, 832, 68, 16));
      this.AddRect(new Vector2(-23f, 0.0f), new Rectangle(0, 848, 48, 16));
      this.AddRect(new Vector2(-83f, -1f), new Rectangle(0, 864, 88, 16));
      this.AddRect(new Vector2(-67f, -8f), new Rectangle(0, 880, 68, 16));
      this.AddRect(new Vector2(-49f, -8f), new Rectangle(0, 896, 52, 16));
      this.AddRect(new Vector2(0.0f, -8f), new Rectangle(0, 912, 40, 16));
      this.AddRect(new Vector2(0.0f, -8f), new Rectangle(0, 928, 48, 16));
      this.AddRect(new Vector2(-34f, -15f), new Rectangle(0, 944, 68, 16));
      this.AddRect(new Vector2(-44f, -2f), new Rectangle(0, 960, 48, 16));
      this.AddRect(new Vector2(-22f, 0.0f), new Rectangle(0, 976, 44, 16));
      this.AddRect(new Vector2(-36f, 0.0f), new Rectangle(0, 992, 72, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(0, 1008, 64, 16));
      this.AddRect(new Vector2(-1f, -3f), new Rectangle(96, 0, 52, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(96, 16, 48, 16));
      this.AddRect(new Vector2(-24f, -15f), new Rectangle(96, 32, 48, 16));
      this.AddRect(new Vector2(-32f, -1f), new Rectangle(96, 48, 64, 16));
      this.AddRect(new Vector2(-38f, -15f), new Rectangle(96, 64, 76, 16));
      this.AddRect(new Vector2(-9f, -15f), new Rectangle(96, 80, 68, 16));
      this.AddRect(new Vector2(-26f, -1f), new Rectangle(96, 96, 52, 16));
      this.AddRect(new Vector2(-18f, -15f), new Rectangle(96, 112, 36, 16));
      this.AddRect(new Vector2(-51f, -8f), new Rectangle(96, 128, 56, 16));
      this.AddRect(new Vector2(-7f, -13f), new Rectangle(96, 144, 84, 16));
      this.AddRect(new Vector2(-26f, 0.0f), new Rectangle(96, 160, 52, 16));
      this.AddRect(new Vector2(-30f, 0.0f), new Rectangle(96, 176, 60, 16));
      this.AddRect(new Vector2(-2f, -8f), new Rectangle(96, 192, 52, 16));
      this.AddRect(new Vector2(-56f, -14f), new Rectangle(96, 208, 60, 16));
      this.AddRect(new Vector2(-34f, -14f), new Rectangle(96, 224, 68, 16));
      this.AddRect(new Vector2(-18f, -15f), new Rectangle(96, 240, 44, 16));
      this.AddRect(new Vector2(-38f, -2f), new Rectangle(96, 256, 84, 16));
      this.AddRect(new Vector2(-57f, -1f), new Rectangle(96, 272, 64, 16));
      this.AddRect(new Vector2(-51f, -8f), new Rectangle(96, 288, 56, 16));
      this.AddRect(new Vector2(-72f, -8f), new Rectangle(96, 304, 76, 16));
      this.AddRect(new Vector2(-86f, -13f), new Rectangle(96, 320, 100, 16));
      this.AddRect(new Vector2(-48f, -15f), new Rectangle(96, 336, 96, 16));
      this.AddRect(new Vector2(-83f, -8f), new Rectangle(96, 352, 88, 16));
      this.AddRect(new Vector2(-30f, -15f), new Rectangle(96, 368, 60, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(96, 384, 56, 16));
      this.AddRect(new Vector2(-36f, -1f), new Rectangle(96, 400, 72, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(96, 416, 56, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(96, 432, 40, 16));
      this.AddRect(new Vector2(-44f, -15f), new Rectangle(96, 448, 88, 16));
      this.AddRect(new Vector2(-26f, -1f), new Rectangle(96, 464, 52, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(96, 480, 76, 16));
      this.AddRect(new Vector2(-2f, -8f), new Rectangle(96, 496, 52, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(96, 512, 44, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(96, 528, 56, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(96, 544, 52, 16));
      this.AddRect(new Vector2(-39f, -8f), new Rectangle(96, 560, 40, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(96, 576, 64, 16));
      this.AddRect(new Vector2(-50f, -8f), new Rectangle(96, 592, 52, 16));
      this.AddRect(new Vector2(-49f, -8f), new Rectangle(96, 608, 56, 16));
      this.AddRect(new Vector2(-49f, -15f), new Rectangle(96, 624, 56, 16));
      this.AddRect(new Vector2(-44f, -15f), new Rectangle(96, 640, 64, 16));
      this.AddRect(new Vector2(-70f, -8f), new Rectangle(96, 656, 72, 16));
      this.AddRect(new Vector2(-2f, -8f), new Rectangle(96, 672, 64, 16));
      this.AddRect(new Vector2(-49f, -8f), new Rectangle(96, 688, 56, 16));
      this.AddRect(new Vector2(-20f, -15f), new Rectangle(96, 704, 40, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(96, 720, 52, 16));
      this.AddRect(new Vector2(-62f, -14f), new Rectangle(96, 736, 76, 16));
      this.AddRect(new Vector2(0.0f, -8f), new Rectangle(96, 752, 40, 16));
      this.AddRect(new Vector2(-37f, -8f), new Rectangle(96, 768, 40, 16));
      this.AddRect(new Vector2(0.0f, -15f), new Rectangle(96, 784, 56, 16));
      this.AddRect(new Vector2(-57f, -3f), new Rectangle(96, 800, 60, 16));
      this.AddRect(new Vector2(-48f, -8f), new Rectangle(96, 816, 52, 16));
      this.AddRect(new Vector2(-39f, -8f), new Rectangle(96, 832, 44, 16));
      this.AddRect(new Vector2(-2f, -15f), new Rectangle(96, 848, 56, 16));
      this.AddRect(new Vector2(-2f, -14f), new Rectangle(96, 864, 56, 16));
      this.AddRect(new Vector2(-58f, -12f), new Rectangle(96, 880, 60, 16));
      this.AddRect(new Vector2(0.0f, -8f), new Rectangle(96, 896, 28, 16));
      this.AddRect(new Vector2(-2f, -14f), new Rectangle(96, 912, 40, 16));
      this.AddRect(new Vector2(-50f, -4f), new Rectangle(96, 928, 56, 16));
      this.AddRect(new Vector2(-14f, -15f), new Rectangle(96, 944, 44, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(96, 960, 88, 16));
      this.AddRect(new Vector2(-53f, -14f), new Rectangle(96, 976, 76, 16));
      this.AddRect(new Vector2(-1f, -1f), new Rectangle(96, 992, 32, 16));
      this.AddRect(new Vector2(-25f, -14f), new Rectangle(96, 1008, 56, 16));
      this.AddRect(new Vector2(-21f, -14f), new Rectangle(200, 0, 44, 16));
      this.AddRect(new Vector2(-20f, -2f), new Rectangle(200, 16, 40, 16));
      this.AddRect(new Vector2(-51f, -8f), new Rectangle(200, 32, 56, 16));
      this.AddRect(new Vector2(-45f, -14f), new Rectangle(200, 48, 80, 16));
      this.AddRect(new Vector2(-2f, -12f), new Rectangle(200, 64, 44, 16));
      this.AddRect(new Vector2(-30f, -1f), new Rectangle(200, 80, 60, 16));
      this.AddRect(new Vector2(-16f, -14f), new Rectangle(200, 96, 44, 16));
      this.AddRect(new Vector2(-30f, -15f), new Rectangle(200, 112, 60, 16));
      this.AddRect(new Vector2(-2f, -8f), new Rectangle(200, 128, 32, 16));
      this.AddRect(new Vector2(-35f, -9f), new Rectangle(200, 144, 36, 16));
      this.AddRect(new Vector2(-1f, -2f), new Rectangle(200, 160, 44, 16));
      this.AddRect(new Vector2(-2f, -1f), new Rectangle(200, 176, 36, 16));
      this.AddRect(new Vector2(-30f, -16f), new Rectangle(200, 192, 60, 16));
      this.AddRect(new Vector2(-22f, -1f), new Rectangle(200, 208, 44, 16));
      this.AddRect(new Vector2(-3f, -15f), new Rectangle(200, 224, 36, 16));
      this.AddRect(new Vector2(-39f, -2f), new Rectangle(200, 240, 44, 16));
      this.AddRect(new Vector2(-30f, -1f), new Rectangle(200, 256, 60, 16));
      this.AddRect(new Vector2(-47f, -3f), new Rectangle(200, 272, 56, 16));
      this.AddRect(new Vector2(-16f, -14f), new Rectangle(200, 288, 32, 16));
      this.AddRect(new Vector2(-26f, 0.0f), new Rectangle(200, 304, 52, 16));
      this.AddRect(new Vector2(0.0f, -8f), new Rectangle(200, 320, 52, 16));
      this.AddRect(new Vector2(-13f, -15f), new Rectangle(200, 336, 56, 16));
      this.AddRect(new Vector2(-37f, -8f), new Rectangle(200, 352, 40, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(200, 368, 60, 16));
      this.AddRect(new Vector2(-1f, -8f), new Rectangle(200, 384, 72, 16));
      this.AddRect(new Vector2(-32f, -15f), new Rectangle(200, 400, 64, 16));
      this.AddRect(new Vector2(-52f, -14f), new Rectangle(200, 416, 56, 16));
      this.AddRect(new Vector2(-33f, -12f), new Rectangle(200, 432, 40, 16));
      this.AddRect(new Vector2(-2f, -8f), new Rectangle(200, 448, 64, 16));
      this.AddRect(new Vector2(-2f, -8f), new Rectangle(200, 464, 76, 16));
      this.AddRect(new Vector2(-2f, -1f), new Rectangle(200, 480, 28, 16));
      this.AddRect(new Vector2(-2f, -8f), new Rectangle(200, 496, 52, 16));
      this.AddRect(new Vector2(-2f, -8f), new Rectangle(200, 512, 76, 16));
      this.AddRect(new Vector2(-20f, -1f), new Rectangle(200, 528, 40, 16));
      this.AddRect(new Vector2(-26f, -1f), new Rectangle(200, 544, 52, 16));
      this.AddRect(new Vector2(-28f, -1f), new Rectangle(200, 560, 56, 16));
      this.AddRect(new Vector2(-25f, -1f), new Rectangle(200, 576, 32, 16));
      this.AddRect(new Vector2(-25f, -11f), new Rectangle(200, 592, 28, 16));
      this.AddRect(new Vector2(-3f, -14f), new Rectangle(200, 608, 16, 16));
      this.AddRect(new Vector2(0.0f, -8f), new Rectangle(200, 624, 32, 16));
      this.AddRect(new Vector2(-25f, -8f), new Rectangle(200, 640, 32, 16));
      this.AddRect(new Vector2(-13f, -14f), new Rectangle(200, 656, 32, 16));
      this.AddRect(new Vector2(-38f, -12f), new Rectangle(200, 672, 40, 16));
      this.AddRect(new Vector2(-27f, -8f), new Rectangle(200, 688, 32, 16));
      this.AddRect(new Vector2(-27f, -14f), new Rectangle(200, 704, 32, 16));
      this.AddRect(new Vector2(-26f, -8f), new Rectangle(200, 720, 28, 16));
      this.AddRect(new Vector2(-14f, -16f), new Rectangle(200, 736, 28, 16));
      this.AddRect(new Vector2(-16f, -16f), new Rectangle(200, 752, 32, 16));
      this.AddRect(new Vector2(-16f, -2f), new Rectangle(200, 768, 32, 16));
      this.AddRect(new Vector2(-101f, 6f), new Rectangle(168, 784, 98, 16));
      this.AddRect(new Vector2(-114f, -8f), new Rectangle(168, 800, 104, 16));
      this.AddRect(new Vector2(-124f, -17f), new Rectangle(168, 816, 116, 16));
      this.AddRect(new Vector2(-72f, -6f), new Rectangle(168, 832, 62, 16));
      this.AddRect(new Vector2(-70f, -20f), new Rectangle(168, 848, 62, 16));
      this.AddRect(new Vector2(-65f, -18f), new Rectangle(168, 864, 62, 16));
      this.AddRect(new Vector2(-58f, -31f), new Rectangle(168, 880, 48, 16));
      this.AddRect(new Vector2(-50f, -23f), new Rectangle(136, 896, 64, 16));
      this.AddRect(new Vector2(-14f, -23f), new Rectangle(240, 768, 32, 16));
      this.AddRect(new Vector2(-45f, -20f), new Rectangle(224, 880, 64, 16));
      this.AddRect(new Vector2(-45f, 5f), new Rectangle(240, 752, 40, 16));
      this.AddRect(new Vector2(19f, -13f), new Rectangle(208, 896, 80, 16));
      this.AddRect(new Vector2(-19f, 0.0f), new Rectangle(288, 0, 56, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 10, 56, 10));
      this.AddRect(new Vector2(-44f, -10f), new Rectangle(288, 20, 64, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 30, 82, 10));
      this.AddRect(new Vector2(-5f, -10f), new Rectangle(288, 40, 46, 10));
      this.AddRect(new Vector2(-28f, -10f), new Rectangle(288, 50, 56, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 60, 56, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 70, 56, 10));
      this.AddRect(new Vector2(-31f, 0.0f), new Rectangle(288, 80, 62, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 90, 54, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 100, 64, 10));
      this.AddRect(new Vector2(-27f, 0.0f), new Rectangle(288, 110, 54, 10));
      this.AddRect(new Vector2(-9f, 0.0f), new Rectangle(288, 120, 62, 10));
      this.AddRect(new Vector2(-59f, 0.0f), new Rectangle(288, 130, 90, 10));
      this.AddRect(new Vector2(-15f, -1f), new Rectangle(288, 140, 48, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 150, 42, 10));
      this.AddRect(new Vector2(-39f, -2f), new Rectangle(288, 160, 40, 10));
      this.AddRect(new Vector2(-24f, -10f), new Rectangle(288, 170, 48, 10));
      this.AddRect(new Vector2(-54f, -5f), new Rectangle(288, 180, 54, 10));
      this.AddRect(new Vector2(-45f, -5f), new Rectangle(288, 190, 46, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 200, 42, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 210, 36, 10));
      this.AddRect(new Vector2(-72f, -5f), new Rectangle(288, 220, 72, 10));
      this.AddRect(new Vector2(-33f, 0.0f), new Rectangle(288, 230, 56, 10));
      this.AddRect(new Vector2(-64f, -10f), new Rectangle(288, 240, 64, 10));
      this.AddRect(new Vector2(-23f, 0.0f), new Rectangle(288, 250, 46, 10));
      this.AddRect(new Vector2(-46f, -5f), new Rectangle(288, 260, 46, 10));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(288, 270, 54, 10));
      this.AddRect(new Vector2(-32f, -10f), new Rectangle(288, 280, 64, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 290, 82, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 300, 64, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 310, 72, 10));
      this.AddRect(new Vector2(-32f, -10f), new Rectangle(288, 320, 54, 10));
      this.AddRect(new Vector2(-72f, -5f), new Rectangle(288, 330, 72, 10));
      this.AddRect(new Vector2(-64f, -5f), new Rectangle(288, 340, 64, 10));
      this.AddRect(new Vector2(0.0f, -4f), new Rectangle(288, 350, 54, 20));
      this.AddRect(new Vector2(-90f, -5f), new Rectangle(288, 370, 90, 10));
      this.AddRect(new Vector2(-36f, -5f), new Rectangle(288, 380, 36, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 390, 36, 10));
      this.AddRect(new Vector2(-72f, -10f), new Rectangle(288, 400, 72, 10));
      this.AddRect(new Vector2(-29f, -10f), new Rectangle(288, 410, 58, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 420, 72, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 430, 46, 10));
      this.AddRect(new Vector2(0.0f, -10f), new Rectangle(288, 440, 72, 10));
      this.AddRect(new Vector2(0.0f, -10f), new Rectangle(288, 450, 46, 20));
      this.AddRect(new Vector2(-25f, -10f), new Rectangle(288, 470, 50, 10));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(288, 480, 58, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 490, 90, 10));
      this.AddRect(new Vector2(-32f, 0.0f), new Rectangle(288, 500, 64, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 510, 46, 10));
      this.AddRect(new Vector2(-41f, -10f), new Rectangle(288, 520, 82, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 530, 46, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 540, 38, 10));
      this.AddRect(new Vector2(-26f, 0.0f), new Rectangle(288, 550, 72, 10));
      this.AddRect(new Vector2(-46f, -5f), new Rectangle(288, 560, 46, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 570, 36, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 580, 38, 10));
      this.AddRect(new Vector2(-24f, -16f), new Rectangle(288, 590, 64, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 600, 42, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 610, 56, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 620, 64, 10));
      this.AddRect(new Vector2(-42f, -5f), new Rectangle(288, 630, 42, 10));
      this.AddRect(new Vector2(-34f, -5f), new Rectangle(288, 640, 34, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 650, 50, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 660, 56, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 670, 56, 10));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(288, 680, 42, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 690, 44, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 700, 64, 10));
      this.AddRect(new Vector2(-64f, -5f), new Rectangle(288, 710, 64, 10));
      this.AddRect(new Vector2(-37f, -10f), new Rectangle(288, 720, 74, 10));
      this.AddRect(new Vector2(0.0f, -10f), new Rectangle(288, 730, 38, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 740, 46, 10));
      this.AddRect(new Vector2(-46f, -5f), new Rectangle(288, 750, 46, 10));
      this.AddRect(new Vector2(-56f, -5f), new Rectangle(288, 760, 56, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 770, 46, 10));
      this.AddRect(new Vector2(-8f, -10f), new Rectangle(288, 780, 50, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 790, 48, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 800, 74, 10));
      this.AddRect(new Vector2(-64f, -5f), new Rectangle(288, 810, 64, 10));
      this.AddRect(new Vector2(-1f, -15f), new Rectangle(288, 820, 30, 20));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 840, 38, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 850, 58, 10));
      this.AddRect(new Vector2(-28f, -1f), new Rectangle(288, 860, 56, 10));
      this.AddRect(new Vector2(-23f, -10f), new Rectangle(288, 870, 56, 10));
      this.AddRect(new Vector2(0.0f, -10f), new Rectangle(288, 880, 54, 10));
      this.AddRect(new Vector2(-34f, -10f), new Rectangle(288, 890, 46, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 900, 56, 10));
      this.AddRect(new Vector2(-28f, -5f), new Rectangle(288, 910, 28, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(288, 920, 36, 10));
      this.AddRect(new Vector2(-58f, -5f), new Rectangle(288, 930, 58, 10));
      this.AddRect(new Vector2(-15f, -2f), new Rectangle(288, 940, 30, 20));
      this.AddRect(new Vector2(-72f, -5f), new Rectangle(380, 0, 72, 10));
      this.AddRect(new Vector2(-20f, 0.0f), new Rectangle(380, 10, 40, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 20, 72, 10));
      this.AddRect(new Vector2(-42f, -16f), new Rectangle(380, 30, 48, 20));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 50, 56, 10));
      this.AddRect(new Vector2(-56f, -2f), new Rectangle(380, 60, 56, 10));
      this.AddRect(new Vector2(-31f, -10f), new Rectangle(380, 70, 48, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 80, 54, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 90, 56, 10));
      this.AddRect(new Vector2(-27f, -1f), new Rectangle(380, 100, 54, 10));
      this.AddRect(new Vector2(0.0f, -1f), new Rectangle(380, 110, 44, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 120, 62, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 130, 54, 10));
      this.AddRect(new Vector2(-18f, 0.0f), new Rectangle(380, 140, 36, 10));
      this.AddRect(new Vector2(-56f, -5f), new Rectangle(380, 150, 56, 10));
      this.AddRect(new Vector2(-56f, -5f), new Rectangle(380, 160, 56, 10));
      this.AddRect(new Vector2(-6f, -2f), new Rectangle(380, 170, 64, 10));
      this.AddRect(new Vector2(-64f, -5f), new Rectangle(380, 180, 64, 10));
      this.AddRect(new Vector2(-1f, -2f), new Rectangle(380, 190, 66, 10));
      this.AddRect(new Vector2(-36f, -5f), new Rectangle(380, 200, 36, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 210, 64, 10));
      this.AddRect(new Vector2(-64f, -5f), new Rectangle(380, 220, 64, 10));
      this.AddRect(new Vector2(-64f, -5f), new Rectangle(380, 230, 64, 10));
      this.AddRect(new Vector2(-36f, -10f), new Rectangle(380, 240, 72, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 250, 46, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 260, 36, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 270, 90, 10));
      this.AddRect(new Vector2(-25f, 0.0f), new Rectangle(380, 280, 48, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 290, 80, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 300, 46, 10));
      this.AddRect(new Vector2(-31f, -10f), new Rectangle(380, 310, 62, 10));
      this.AddRect(new Vector2(-26f, -5f), new Rectangle(380, 320, 26, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 330, 46, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 340, 54, 10));
      this.AddRect(new Vector2(-21f, -1f), new Rectangle(380, 350, 42, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 360, 46, 10));
      this.AddRect(new Vector2(-37f, 0.0f), new Rectangle(380, 370, 74, 10));
      this.AddRect(new Vector2(-37f, -10f), new Rectangle(380, 380, 74, 10));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(380, 390, 64, 10));
      this.AddRect(new Vector2(0.0f, -10f), new Rectangle(380, 400, 64, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 410, 58, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 420, 48, 10));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(380, 430, 62, 10));
      this.AddRect(new Vector2(-27f, -10f), new Rectangle(380, 440, 54, 10));
      this.AddRect(new Vector2(0.0f, -10f), new Rectangle(380, 450, 16, 10));
      this.AddRect(new Vector2(-27f, -10f), new Rectangle(380, 460, 54, 10));
      this.AddRect(new Vector2(-54f, -1f), new Rectangle(380, 470, 54, 10));
      this.AddRect(new Vector2(-18f, -10f), new Rectangle(380, 480, 36, 10));
      this.AddRect(new Vector2(-46f, -5f), new Rectangle(380, 490, 46, 10));
      this.AddRect(new Vector2(0.0f, -5f), new Rectangle(380, 500, 54, 10));
      this.AddRect(new Vector2(-23f, -10f), new Rectangle(380, 510, 28, 10));
      this.AddRect(new Vector2(-12f, -10f), new Rectangle(380, 520, 24, 10));
      this.AddRect(new Vector2(-41f, -5f), new Rectangle(380, 530, 38, 10));
      this.AddRect(new Vector2(-41f, -5f), new Rectangle(380, 540, 38, 10));
      this.AddRect(new Vector2(0.0f, -10f), new Rectangle(380, 550, 28, 10));
      this.AddRect(new Vector2(-27f, -5f), new Rectangle(380, 560, 28, 10));
      this.AddRect(new Vector2(-36f, -5f), new Rectangle(380, 570, 38, 10));
      this.AddRect(new Vector2(-36f, -5f), new Rectangle(380, 580, 38, 10));
      this.AddRect(new Vector2(-36f, -5f), new Rectangle(380, 590, 38, 10));
      this.AddRect(new Vector2(-19f, -10f), new Rectangle(380, 600, 38, 10));
      this.AddRect(new Vector2(5f, -16f), new Rectangle(380, 610, 74, 10));
      this.AddRect(new Vector2(-61f, -5f), new Rectangle(380, 620, 56, 10));
      this.AddRect(new Vector2(-70f, -5f), new Rectangle(380, 630, 64, 10));
      this.AddRect(new Vector2(-5f, -5f), new Rectangle(376, 640, 80, 10));
      this.AddRect(new Vector2(-28f, -16f), new Rectangle(380, 650, 60, 10));
      this.AddRect(new Vector2(-42f, -16f), new Rectangle(380, 660, 90, 10));
      this.AddRect(new Vector2(-52f, 0.0f), new Rectangle(184, 960, 104, 16));
      this.AddRect(new Vector2(-31f, -16f), new Rectangle(184, 976, 62, 16));
      this.AddRect(new Vector2(0.0f, -8f), new Rectangle(184, 992, 76, 16));
      this.AddRect(new Vector2(-38f, -16f), new Rectangle(184, 1008, 76, 16));
    }
  }
}
