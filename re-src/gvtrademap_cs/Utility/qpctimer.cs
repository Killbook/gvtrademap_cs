// Type: Utility.qpctimer
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Runtime.InteropServices;

namespace Utility
{
  public class qpctimer
  {
    private long m_last_counter;
    private long m_section_start_counter;
    private long m_qpf_ticks_per_sec;
    private float m_inv_qpf_ticks_per_sec;

    public qpctimer()
    {
      this.m_last_counter = 0L;
      this.m_inv_qpf_ticks_per_sec = 0.0f;
      this.m_section_start_counter = 0L;
      long x = 0L;
      int num1 = (int) qpctimer.QueryPerformanceFrequency(ref x);
      this.m_qpf_ticks_per_sec = x;
      this.m_inv_qpf_ticks_per_sec = 1f / (float) x;
      double num2 = (double) this.GetElapsedTime();
    }

    [DllImport("kernel32.dll")]
    private static short QueryPerformanceCounter(ref long x);

    [DllImport("kernel32.dll")]
    private static short QueryPerformanceFrequency(ref long x);

    public float GetElapsedTime()
    {
      long x = 0L;
      int num1 = (int) qpctimer.QueryPerformanceCounter(ref x);
      float num2 = this.m_inv_qpf_ticks_per_sec * (float) (x - this.m_last_counter);
      this.m_last_counter = x;
      return num2;
    }

    public int GetElapsedTimeMilliseconds()
    {
      return (int) ((double) this.GetElapsedTime() * 1000.0);
    }

    public void StartSection()
    {
      long x = 0L;
      int num = (int) qpctimer.QueryPerformanceCounter(ref x);
      this.m_section_start_counter = x;
    }

    public float GetSectionTime()
    {
      long x = 0L;
      int num = (int) qpctimer.QueryPerformanceCounter(ref x);
      return this.m_inv_qpf_ticks_per_sec * (float) (x - this.m_section_start_counter);
    }

    public void WaitSectionTime(float section)
    {
      long num1 = (long) ((double) section * (double) this.m_qpf_ticks_per_sec);
      long x = 0L;
      do
      {
        int num2 = (int) qpctimer.QueryPerformanceCounter(ref x);
      }
      while (x - this.m_section_start_counter < num1);
    }
  }
}
