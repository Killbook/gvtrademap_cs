// Type: win32.user32
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace win32
{
  public static class user32
  {
    private const int WM_KEYDOWN = 256;
    private const int WM_KEYUP = 257;
    private const int WM_CHAR = 258;
    public const int VK_BACK = 8;
    public const int VK_TAB = 9;
    public const int VK_SHIFT = 16;
    public const int VK_CONTROL = 17;
    public const int VK_MENU = 18;
    public const int VK_RETURN = 13;
    public const int VK_ESCAPE = 27;
    public const int VK_SPACE = 32;
    public const int VK_INSERT = 45;
    public const int VK_DELETE = 46;
    public const int VK_0 = 48;
    public const int VK_1 = 49;
    public const int VK_2 = 50;
    public const int VK_3 = 51;
    public const int VK_4 = 52;
    public const int VK_5 = 53;
    public const int VK_6 = 54;
    public const int VK_7 = 55;
    public const int VK_8 = 56;
    public const int VK_9 = 57;
    public const int VK_A = 65;
    public const int VK_B = 66;
    public const int VK_C = 67;
    public const int VK_D = 68;
    public const int VK_E = 69;
    public const int VK_F = 70;
    public const int VK_G = 71;
    public const int VK_H = 72;
    public const int VK_I = 73;
    public const int VK_J = 74;
    public const int VK_K = 75;
    public const int VK_L = 76;
    public const int VK_M = 77;
    public const int VK_N = 78;
    public const int VK_O = 79;
    public const int VK_P = 80;
    public const int VK_Q = 81;
    public const int VK_R = 82;
    public const int VK_S = 83;
    public const int VK_T = 84;
    public const int VK_U = 85;
    public const int VK_V = 86;
    public const int VK_W = 87;
    public const int VK_X = 88;
    public const int VK_Y = 89;
    public const int VK_Z = 90;
    public const int VK_F1 = 112;
    public const int VK_F2 = 113;
    public const int VK_F3 = 114;
    public const int VK_F4 = 115;
    public const int VK_F5 = 116;
    public const int VK_F6 = 117;
    public const int VK_F7 = 118;
    public const int VK_F8 = 119;
    public const int VK_F9 = 120;
    public const int VK_F10 = 121;
    public const int VK_F11 = 122;
    public const int VK_F12 = 123;

    [DllImport("user32.dll")]
    public static IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static int GetWindowText(IntPtr hWnd, StringBuilder lpStr, int nMaxCount);

    [DllImport("user32.dll")]
    public static IntPtr FindWindowA(string pClassName, string pWindowName);

    [DllImport("user32.dll")]
    public static bool GetClientRect(IntPtr hWnd, ref Rectangle rect);

    [DllImport("user32.dll")]
    public static bool ClientToScreen(IntPtr hWnd, ref Point p);

    [DllImport("user32.dll")]
    public static IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static void ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("user32.dll")]
    public static int keybd_event(int VK, int scan, int flags, int extinfo);

    [DllImport("user32.dll")]
    public static short GetKeyState(int nVirtKey);

    [DllImport("user32.dll")]
    public static int SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

    [DllImport("user32.dll")]
    public static int PostMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

    public static void PostMessage_KEYDOWN(IntPtr handle, int vk, int flag)
    {
      if (handle == (IntPtr) 0)
        return;
      user32.PostMessage(handle, 256, (IntPtr) vk, (IntPtr) flag);
    }

    public static void PostMessage_KEYUP(IntPtr handle, int vk, int flag)
    {
      if (handle == (IntPtr) 0)
        return;
      user32.PostMessage(handle, 257, (IntPtr) vk, (IntPtr) flag);
    }
  }
}
