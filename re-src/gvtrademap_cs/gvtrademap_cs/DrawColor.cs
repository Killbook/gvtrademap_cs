// Type: gvtrademap_cs.DrawColor
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Drawing;

namespace gvtrademap_cs
{
  internal static class DrawColor
  {
    private static int[] m_color_tbl = new int[8]
    {
      Color.FromArgb(0, 224, 64, 64).ToArgb(),
      Color.FromArgb(0, 224, 160, 0).ToArgb(),
      Color.FromArgb(0, 224, 224, 0).ToArgb(),
      Color.FromArgb(0, 64, 160, 64).ToArgb(),
      Color.FromArgb(0, 30, 160, 160).ToArgb(),
      Color.FromArgb(0, 120, 120, 230).ToArgb(),
      Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue).ToArgb(),
      Color.FromArgb(0, (int) byte.MaxValue, 150, 150).ToArgb()
    };

    static DrawColor()
    {
    }

    public static int GetColor(int color_index)
    {
      if (color_index < 0 || color_index >= 8)
        return DrawColor.m_color_tbl[0];
      else
        return DrawColor.m_color_tbl[color_index];
    }
  }
}
