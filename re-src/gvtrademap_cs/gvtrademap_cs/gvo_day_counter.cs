// Type: gvtrademap_cs.gvo_day_counter
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

namespace gvtrademap_cs
{
  public class gvo_day_counter
  {
    private const int DEF_COUNTER_MAX = 99;
    private int m_days;
    private int m_voyage_days_start;
    private int m_voyage_days;
    private int m_counter_max;

    protected int Days
    {
      get
      {
        return this.m_days;
      }
      set
      {
        this.m_days = value;
      }
    }

    protected int VoyageDaysStart
    {
      get
      {
        return this.m_voyage_days_start;
      }
    }

    protected int VoyageDays
    {
      get
      {
        return this.m_voyage_days;
      }
    }

    protected int CounterMax
    {
      get
      {
        return this.m_counter_max;
      }
      set
      {
        this.m_counter_max = value;
        if (this.m_counter_max >= 0)
          return;
        this.m_counter_max = 99;
      }
    }

    public gvo_day_counter()
      : this(0)
    {
    }

    public gvo_day_counter(int days)
    {
      this.Reset();
      this.m_days = days;
      this.m_counter_max = 99;
    }

    protected virtual void UpdateBase(int days)
    {
      if (this.m_voyage_days_start < 0)
      {
        this.m_voyage_days_start = days;
        this.m_voyage_days = days;
      }
      else
      {
        if (days < this.m_voyage_days)
        {
          this.m_days += this.m_voyage_days - this.m_voyage_days_start;
          this.m_voyage_days_start = days;
          this.m_voyage_days = days;
        }
        this.m_voyage_days = days;
      }
    }

    public void Reset()
    {
      this.Reset(-1);
    }

    public void Reset(int now_days)
    {
      this.m_days = 0;
      this.m_voyage_days_start = now_days;
      this.m_voyage_days = now_days;
    }

    public int GetDays()
    {
      int trueDays = this.get_true_days();
      if (trueDays > this.m_counter_max)
        return this.m_counter_max;
      else
        return trueDays;
    }

    protected int get_true_days()
    {
      return this.m_days + (this.m_voyage_days - this.m_voyage_days_start);
    }
  }
}
