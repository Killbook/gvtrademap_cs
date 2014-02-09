// Type: gvtrademap_cs.globalmouse_hook
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using win32;

namespace gvtrademap_cs
{
  internal class globalmouse_hook : IDisposable
  {
    private IntPtr m_handle;

    public globalmouse_hook()
      : this(globalmouse_hook.SendKeyType.TOGGLE_SKILL, globalmouse_hook.SendKeyType.OPEN_ITEM_WINDOW)
    {
    }

    public globalmouse_hook(globalmouse_hook.SendKeyType xbutton1, globalmouse_hook.SendKeyType xbutton2)
    {
      this.m_handle = kernel32.LoadLibrary("mousehook.dll");
      if (this.m_handle == IntPtr.Zero)
      {
        int num1 = (int) MessageBox.Show("mousehook.dll の読み込みに失敗");
      }
      else
      {
        IntPtr procAddress = kernel32.GetProcAddress(this.m_handle, "SetDolMouseHookEx");
        if (procAddress == IntPtr.Zero)
        {
          int num2 = (int) MessageBox.Show("SetDolMouseHookEx() のアドレス取得に失敗");
        }
        else
        {
          int num3 = ((globalmouse_hook.SetDolMouseHookEx) Marshal.GetDelegateForFunctionPointer(procAddress, typeof (globalmouse_hook.SetDolMouseHookEx)))((int) xbutton1, (int) xbutton2);
        }
      }
    }

    ~globalmouse_hook()
    {
      this.Dispose();
    }

    public void Dispose()
    {
      if (this.m_handle == IntPtr.Zero)
        return;
      IntPtr procAddress = kernel32.GetProcAddress(this.m_handle, "UnhookDolMouseHook");
      if (procAddress == IntPtr.Zero)
      {
        int num1 = (int) MessageBox.Show("UnhookDolMouseHook() のアドレス取得に失敗");
      }
      else
      {
        int num2 = ((globalmouse_hook.UnhookDolMouseHook) Marshal.GetDelegateForFunctionPointer(procAddress, typeof (globalmouse_hook.UnhookDolMouseHook)))();
        kernel32.FreeLibrary(this.m_handle);
        this.m_handle = IntPtr.Zero;
      }
    }

    public enum SendKeyType
    {
      TOGGLE_SKILL = 1,
      TOGGLE_CUSTOM_SLOT = 2,
      OPEN_ITEM_WINDOW = 3,
    }

    private delegate int SetDolMouseHookEx(int xbutton1_keytype, int xbutton2_keytype);

    private delegate int UnhookDolMouseHook();
  }
}
