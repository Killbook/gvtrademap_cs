// Type: gvtrademap_cs.icons
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace gvtrademap_cs
{
  public class icons : d3d_sprite_rects
  {
    public icons(d3d_device device, string fname)
      : base(device, fname)
    {
      if (device.device == (Device) null)
        return;
      this.add_rects();
    }

    private void add_rects()
    {
      this.AddRect(new Vector2(-5f, -14f), new Rectangle(24, 160, 23, 15));
      this.AddRect(new Vector2(-3f, -16f), new Rectangle(0, 232, 6, 9));
      this.AddRect(new Vector2(-3f, -16f), new Rectangle(8, 232, 6, 9));
      this.AddRect(new Vector2(-3f, -16f), new Rectangle(16, 232, 6, 9));
      this.AddRect(new Vector2(-3f, -16f), new Rectangle(24, 232, 6, 9));
      this.AddRect(new Vector2(-3f, -16f), new Rectangle(32, 232, 6, 9));
      this.AddRect(new Vector2(-3f, -16f), new Rectangle(40, 232, 6, 9));
      this.AddRect(new Vector2(-3f, -16f), new Rectangle(48, 232, 6, 9));
      this.AddRect(new Vector2(-3f, -16f), new Rectangle(56, 232, 6, 9));
      this.AddRect(new Vector2(-3f, -16f), new Rectangle(64, 232, 6, 9));
      this.AddRect(new Vector2(-3f, -16f), new Rectangle(72, 232, 6, 9));
      this.AddRect(new Vector2(-6f, -9f), new Rectangle(216, 32, 12, 11));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 0, 24, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 16, 24, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 32, 24, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 48, 24, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 64, 24, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 80, 24, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 96, 24, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 112, 24, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 128, 24, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 144, 24, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 160, 24, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 176, 24, 16));
      this.AddRect(new Vector2(1f, 0.0f), new Rectangle(25, 16, 15, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(40, 16, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(56, 16, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(72, 16, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(88, 16, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(144, 208, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(104, 16, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(120, 16, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(136, 16, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(152, 16, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(168, 16, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(184, 16, 16, 16));
      this.AddRect(new Vector2(1f, 0.0f), new Rectangle(25, 0, 15, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(40, 0, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(56, 0, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(72, 0, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(88, 0, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(160, 208, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(104, 0, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(120, 0, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(136, 0, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(152, 0, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(168, 0, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(184, 0, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(24, 48, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(40, 48, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(200, 16, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(216, 16, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(24, 32, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(40, 32, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(200, 0, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(216, 0, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(56, 48, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(72, 48, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(88, 48, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(104, 48, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(120, 48, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(136, 48, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(152, 48, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(168, 48, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(216, 80, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(56, 32, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(72, 32, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(88, 32, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(104, 32, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(120, 32, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(136, 32, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(152, 32, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(168, 32, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(216, 64, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(232, 64, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(184, 32, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(184, 48, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(200, 48, 16, 16));
      this.AddRect(new Vector2(-20f, -20f), new Rectangle(25, 97, 40, 40));
      this.AddRect(new Vector2(-20f, -20f), new Rectangle(66, 97, 40, 40));
      this.AddRect(new Vector2(-20f, -26f), new Rectangle(107, 97, 40, 40));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(232, 0, 17, 17));
      this.AddRect(new Vector2(0.0f, -3f), new Rectangle(40, 144, 8, 8));
      this.AddRect(new Vector2(0.0f, -3f), new Rectangle(48, 144, 8, 8));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(24, 144, 14, 14));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(128, 144, 48, 17));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(80, 144, 47, 16));
      this.AddRect(new Vector2(-12f, -8f), new Rectangle(0, 0, 24, 16));
      this.AddRect(new Vector2(-12f, -8f), new Rectangle(0, 16, 24, 16));
      this.AddRect(new Vector2(-12f, -8f), new Rectangle(0, 32, 24, 16));
      this.AddRect(new Vector2(-12f, -8f), new Rectangle(0, 48, 24, 16));
      this.AddRect(new Vector2(-12f, -8f), new Rectangle(0, 64, 24, 16));
      this.AddRect(new Vector2(-12f, -8f), new Rectangle(0, 80, 24, 16));
      this.AddRect(new Vector2(-12f, -8f), new Rectangle(0, 96, 24, 16));
      this.AddRect(new Vector2(-12f, -8f), new Rectangle(0, 112, 24, 16));
      this.AddRect(new Vector2(-12f, -8f), new Rectangle(0, 128, 24, 16));
      this.AddRect(new Vector2(-12f, -8f), new Rectangle(0, 144, 24, 16));
      this.AddRect(new Vector2(-12f, -8f), new Rectangle(0, 160, 24, 16));
      this.AddRect(new Vector2(-12f, -8f), new Rectangle(0, 176, 24, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(24, 48, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(40, 48, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(200, 16, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(216, 16, 16, 16));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(64, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(80, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(96, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(112, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(128, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(144, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(160, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(176, 192, 12, 12));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(24, 64, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(40, 64, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(56, 64, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(72, 64, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(88, 64, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(104, 64, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(120, 64, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(136, 64, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(152, 64, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(168, 64, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(184, 64, 16, 16));
      this.AddRect(new Vector2(-8f, -13f), new Rectangle(201, 65, 15, 15));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(64, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(80, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(96, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(112, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(128, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(144, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(160, 192, 12, 12));
      this.AddRect(new Vector2(-6f, -6f), new Rectangle(176, 192, 12, 12));
      this.AddRect(new Vector2(-7f, -7f), new Rectangle(88, 168, 14, 14));
      this.AddRect(new Vector2(-7f, -7f), new Rectangle(104, 168, 14, 14));
      this.AddRect(new Vector2(-7f, -7f), new Rectangle(120, 168, 14, 14));
      this.AddRect(new Vector2(-7f, -7f), new Rectangle(136, 168, 14, 14));
      this.AddRect(new Vector2(-7f, -7f), new Rectangle(152, 168, 14, 14));
      this.AddRect(new Vector2(-8f, -32f), new Rectangle(24, 80, 16, 15));
      this.AddRect(new Vector2(-8f, -32f), new Rectangle(40, 80, 16, 15));
      this.AddRect(new Vector2(-8f, -32f), new Rectangle(56, 80, 16, 15));
      this.AddRect(new Vector2(-8f, -32f), new Rectangle(72, 80, 16, 15));
      this.AddRect(new Vector2(-8f, -32f), new Rectangle(88, 80, 16, 15));
      this.AddRect(new Vector2(-8f, -32f), new Rectangle(104, 80, 16, 15));
      this.AddRect(new Vector2(-8f, -32f), new Rectangle(120, 80, 16, 15));
      this.AddRect(new Vector2(-8f, -32f), new Rectangle(136, 80, 16, 15));
      this.AddRect(new Vector2(-8f, -32f), new Rectangle(152, 80, 16, 15));
      this.AddRect(new Vector2(-8f, -32f), new Rectangle(168, 80, 16, 15));
      this.AddRect(new Vector2(-8f, -32f), new Rectangle(184, 80, 16, 15));
      this.AddRect(new Vector2(-14f, -33f), new Rectangle(152, 96, 30, 34));
      this.AddRect(new Vector2(-3f, -17f), new Rectangle(48, 168, 30, 18));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(24, 16, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(40, 16, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(56, 16, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(72, 16, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(88, 16, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(144, 208, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(104, 16, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(120, 16, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(136, 16, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(152, 16, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(168, 16, 16, 16));
      this.AddRect(new Vector2(-8f, -8f), new Rectangle(184, 16, 16, 16));
      this.AddRect(new Vector2(-4f, -4f), new Rectangle(80, 160, 10, 10));
      this.AddRect(new Vector2(-11f, -18f), new Rectangle(232, 160, 24, 26));
      this.AddRect(new Vector2(-6f, -11f), new Rectangle(232, 192, 14, 16));
      this.AddRect(new Vector2(-10f, -12f), new Rectangle(232, 216, 22, 20));
      this.AddRect(new Vector2(-6f, -11f), new Rectangle(232, 240, 14, 16));
      this.AddRect(new Vector2(-5f, -5f), new Rectangle(240, 96, 10, 10));
      this.AddRect(new Vector2(-5f, -5f), new Rectangle(240, 112, 10, 10));
      this.AddRect(new Vector2(-4f, -4f), new Rectangle(240, 128, 8, 8));
      this.AddRect(new Vector2(-4f, -4f), new Rectangle(240, 144, 8, 8));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(168, 168, 16, 16));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(208, 144, 12, 12));
      this.AddRect(new Vector2(-3f, -5f), new Rectangle(32, 184, 6, 6));
      this.AddRect(new Vector2(-8f, -19f), new Rectangle(0, 192, 18, 20));
      this.AddRect(new Vector2(-12f, -19f), new Rectangle(24, 192, 26, 20));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(224, 144, 10, 12));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(0, 216, 48, 12));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(48, 216, 42, 12));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(200, 192, 24, 12));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(200, 208, 24, 12));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(208, 160, 12, 12));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(208, 176, 12, 12));
      this.AddRect(new Vector2(0.0f, 0.0f), new Rectangle(96, 216, 44, 12));
    }

    public d3d_sprite_rects.rect GetIcon(icons.icon_index index)
    {
      return this.GetRect((int) index);
    }

    public enum icon_index
    {
      days_big_shadow,
      number_0,
      number_1,
      number_2,
      number_3,
      number_4,
      number_5,
      number_6,
      number_7,
      number_8,
      number_9,
      myship,
      country_0,
      country_1,
      country_2,
      country_3,
      country_4,
      country_5,
      country_6,
      country_7,
      country_8,
      country_9,
      country_10,
      country_11,
      tab_0,
      tab_1,
      tab_2,
      tab_3,
      tab_4,
      tab_5,
      tab_6,
      tab_7,
      tab_8,
      tab_9,
      tab_10,
      tab_11,
      tab_gray_0,
      tab_gray_1,
      tab_gray_2,
      tab_gray_3,
      tab_gray_4,
      tab_gray_5,
      tab_gray_6,
      tab_gray_7,
      tab_gray_8,
      tab_gray_9,
      tab_gray_10,
      tab_gray_11,
      tab2_0,
      tab2_1,
      tab2_2,
      tab2_3,
      tab2_gray_0,
      tab2_gray_1,
      tab2_gray_2,
      tab2_gray_3,
      setting_0,
      setting_1,
      setting_2,
      setting_3,
      setting_4,
      setting_5,
      setting_6,
      setting_7,
      setting_8,
      setting_gray_0,
      setting_gray_1,
      setting_gray_2,
      setting_gray_3,
      setting_gray_4,
      setting_gray_5,
      setting_gray_6,
      setting_gray_7,
      setting_gray_8,
      setting_10,
      setting_11,
      setting_11_1,
      setting_12,
      spot_0,
      spot_1,
      spot_2,
      map_icon,
      select_0,
      select_1,
      select_cross,
      setting_button,
      speed_background,
      spot_country_0,
      spot_country_1,
      spot_country_2,
      spot_country_3,
      spot_country_4,
      spot_country_5,
      spot_country_6,
      spot_country_7,
      spot_country_8,
      spot_country_9,
      spot_country_10,
      spot_country_11,
      spot_tab2_0,
      spot_tab2_1,
      spot_tab2_2,
      spot_tab2_3,
      memo_icon_0,
      memo_icon_1,
      memo_icon_2,
      memo_icon_3,
      memo_icon_4,
      memo_icon_5,
      memo_icon_6,
      memo_icon_7,
      memo_icon_8,
      memo_icon_9,
      memo_icon_10,
      memo_icon_11,
      memo_icon_12,
      memo_icon_13,
      memo_icon_14,
      memo_icon_15,
      memo_icon_16,
      memo_icon_17,
      memo_icon_18,
      memo_icon_19,
      web_icon_0,
      web_icon_1,
      web_icon_2,
      web_icon_3,
      web_icon_4,
      web_icon_5,
      web_icon_6,
      web_icon_7,
      web_icon_8,
      web_icon_9,
      web_icon_10,
      web_icon_11,
      web_icon_12,
      accident_0,
      accident_1,
      accident_2,
      accident_3,
      accident_4,
      accident_5,
      accident_6,
      accident_7,
      accident_8,
      accident_9,
      accident_10,
      accident_popup,
      accident_popup_shadow,
      spot_tab_0,
      spot_tab_1,
      spot_tab_2,
      spot_tab_3,
      spot_tab_4,
      spot_tab_5,
      spot_tab_6,
      spot_tab_7,
      spot_tab_8,
      spot_tab_9,
      spot_tab_10,
      spot_tab_11,
      share_city,
      city_icon_0,
      city_icon_1,
      city_icon_2,
      city_icon_3,
      city_icon_4,
      city_icon_5,
      city_icon_6,
      city_icon_7,
      web,
      degree,
      days_mini_6,
      days_big_6,
      days_big_100,
      string00,
      string01,
      string02,
      string03,
      string04,
      string05,
      string06,
      string07,
      max,
    }
  }
}
