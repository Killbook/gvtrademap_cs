// Type: gvo_base.gvo_season
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using Utility;

namespace gvo_base
{
  public class gvo_season
  {
    private DateTime m_next_season_start;
    private DateTime m_now_season_start;
    private gvo_season.season m_now_season;
    private DateTime m_base_season_start;

    public DateTime next_season_start
    {
      get
      {
        return this.m_next_season_start;
      }
    }

    public string next_season_start_jbbsstr
    {
      get
      {
        return Useful.TojbbsDateTimeString(this.m_next_season_start);
      }
    }

    public string next_season_start_shortstr
    {
      get
      {
        return Useful.ToShortDateTimeString(this.m_next_season_start);
      }
    }

    public DateTime now_season_start
    {
      get
      {
        return this.m_now_season_start;
      }
    }

    public string now_season_start_jbbsstr
    {
      get
      {
        return Useful.TojbbsDateTimeString(this.m_now_season_start);
      }
    }

    public string now_season_start_shortstr
    {
      get
      {
        return Useful.ToShortDateTimeString(this.m_now_season_start);
      }
    }

    public gvo_season.season now_season
    {
      get
      {
        return this.m_now_season;
      }
    }

    public string now_season_str
    {
      get
      {
        return gvo_season.ToSeasonString(this.m_now_season);
      }
    }

    public gvo_season()
    {
      this.m_base_season_start = new DateTime(2010, 3, 2, 13, 30, 0);
      this.m_now_season = gvo_season.season.MAX;
      this.UpdateSeason();
    }

    public bool UpdateSeason()
    {
      bool flag = false;
      long num = (DateTime.Now.Ticks - this.m_base_season_start.Ticks) / TimeSpan.FromHours(9.0).Ticks;
      if (num < 0L)
        --num;
      gvo_season.season season = (num & 1L) == 0L ? gvo_season.season.summer : gvo_season.season.winter;
      if (season != this.m_now_season)
        flag = true;
      this.m_now_season = season;
      this.m_now_season_start = this.m_base_season_start.AddHours((double) (num * 9L));
      this.m_next_season_start = this.m_base_season_start.AddHours((double) ((num + 1L) * 9L));
      return flag;
    }

    public static string ToSeasonString(gvo_season.season s)
    {
      switch (s)
      {
        case gvo_season.season.summer:
          return "夏";
        case gvo_season.season.winter:
          return "冬";
        default:
          return "不明";
      }
    }

    public enum season
    {
      summer,
      winter,
      MAX,
    }
  }
}
