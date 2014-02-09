// Type: Utility.DateTimer
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;

namespace Utility
{
  public sealed class DateTimer
  {
    private DateTime m_elapsed_time_tmp;
    private DateTime m_section_time_tmp;

    public DateTimer()
    {
      this.m_elapsed_time_tmp = DateTime.Now;
      this.m_section_time_tmp = DateTime.Now;
    }

    public int GetElapsedTimeMilliseconds()
    {
      DateTime now = DateTime.Now;
      int num = (int) TimeSpan.FromTicks(DateTimer.sub_date_time(now, this.m_elapsed_time_tmp)).TotalMilliseconds;
      this.m_elapsed_time_tmp = now;
      return num;
    }

    public void StartSection()
    {
      this.m_section_time_tmp = DateTime.Now;
    }

    public int GetSectionTimeMilliseconds()
    {
      return (int) TimeSpan.FromTicks(DateTimer.sub_date_time(DateTime.Now, this.m_section_time_tmp)).TotalMilliseconds;
    }

    private static long sub_date_time(DateTime a, DateTime b)
    {
      return a.Ticks - b.Ticks;
    }
  }
}
