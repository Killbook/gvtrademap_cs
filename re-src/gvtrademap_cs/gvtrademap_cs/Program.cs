// Type: gvtrademap_cs.Program
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Utility;

namespace gvtrademap_cs
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Mutex mutex = new Mutex(false, "mutex_gvtrademap_cs_cookie_Zephyros");
      string str = "";
      if (mutex.WaitOne(0, false))
      {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        AssemblyName error_ass = (AssemblyName) null;
        if (!Useful.LoadReferencedAssembly(Assembly.GetExecutingAssembly(), out error_ass))
        {
          using (assembly_load_error_form assemblyLoadErrorForm = new assembly_load_error_form(error_ass))
          {
            int num = (int) assemblyLoadErrorForm.ShowDialog();
            return;
          }
        }
        else
        {
          try
          {
            using (gvtrademap_cs_form gvtrademapCsForm = new gvtrademap_cs_form())
            {
              if (gvtrademapCsForm.Initialize())
              {
                str = gvtrademapCsForm.device_info_string;
                ((Control) gvtrademapCsForm).Show();
                int tickCount = Environment.TickCount;
                while (gvtrademapCsForm.Created)
                {
                  if (Environment.TickCount - tickCount >= 16)
                  {
                    tickCount = Environment.TickCount;
                    gvtrademapCsForm.update_main_window();
                  }
                  Application.DoEvents();
                  Thread.Sleep(1);
                }
              }
            }
          }
          catch (Exception ex)
          {
            using (error_form errorForm = new error_form("大航海時代Online 交易MAP C# ver.1.32.3", ex, "大航海時代Online 交易MAP C# ver.1.32.3", "http://jbbs.livedoor.jp/netgame/7161/", str != "" ? str : "未取得"))
            {
              int num = (int) errorForm.ShowDialog();
            }
          }
        }
      }
      else
        gvtrademap_cs_form.ActiveGVTradeMap();
      GC.KeepAlive((object) mutex);
    }
  }
}
