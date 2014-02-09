// Type: win32.kernel32
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Runtime.InteropServices;

namespace win32
{
  public class kernel32
  {
    [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
    public static IntPtr LoadLibrary(string lpFileName);

    [DllImport("kernel32", SetLastError = true)]
    public static bool FreeLibrary(IntPtr hModule);

    [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
    public static IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
  }
}
