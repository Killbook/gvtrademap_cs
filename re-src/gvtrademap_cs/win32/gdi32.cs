// Type: win32.gdi32
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Runtime.InteropServices;

namespace win32
{
  public class gdi32
  {
    public const int SRCCOPY = 13369376;
    public const int BLACKNESS = 66;

    [DllImport("gdi32.dll", EntryPoint = "CreateDCA")]
    public static IntPtr CreateDC(string lpDriverName, string lpDeviceName, string lpOutput, string lpInitData);

    [DllImport("gdi32.dll")]
    public static IntPtr CreateCompatibleDC(IntPtr hDC);

    [DllImport("gdi32.dll")]
    public static IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

    [DllImport("gdi32.dll")]
    public static IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

    [DllImport("gdi32.dll")]
    public static int DeleteDC(IntPtr hDC);

    [DllImport("gdi32.dll")]
    public static int DeleteObject(IntPtr hObj);

    [DllImport("gdi32.dll")]
    public static int BitBlt(IntPtr desthDC, int destX, int destY, int destW, int destH, IntPtr srchDC, int srcX, int srcY, int op);
  }
}
