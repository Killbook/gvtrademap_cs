// Type: gvtrademap_cs.gvo_build_ship_counter
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Text.RegularExpressions;
using Utility;

namespace gvtrademap_cs
{
  public class gvo_build_ship_counter : gvo_day_counter
  {
    private const int COUNTER_MAX = 999;
    private GlobalSettings m_setting;
    private bool m_is_now_build;
    private string m_ship_name;
    private int m_finish_days;

    public bool IsNowBuild
    {
      get
      {
        return this.m_is_now_build;
      }
    }

    public gvo_build_ship_counter(GlobalSettings _setting)
    {
      this.CounterMax = 999;
      this.m_setting = _setting;
      this.Days = this.m_setting.build_ship_days;
      this.m_is_now_build = this.m_setting.is_now_build_ship;
      this.m_ship_name = this.m_setting.build_ship_name;
      this.m_finish_days = this.get_build_ship_days(this.m_ship_name);
    }

    private void update_settings()
    {
      this.m_setting.build_ship_days = this.get_true_days();
      this.m_setting.is_now_build_ship = this.m_is_now_build;
      this.m_setting.build_ship_name = this.m_ship_name;
    }

    public void StartBuildShip(string ship_name)
    {
      this.Reset();
      this.m_is_now_build = true;
      this.m_ship_name = ship_name;
      this.m_finish_days = this.get_build_ship_days(this.m_ship_name);
      this.update_settings();
    }

    public void FinishBuildShip()
    {
      this.m_is_now_build = false;
      this.Reset();
      this.m_ship_name = "";
      this.m_finish_days = -1;
      this.update_settings();
    }

    public void Update(int days)
    {
      if (!this.m_is_now_build)
        return;
      this.UpdateBase(days);
      this.update_settings();
    }

    public string GetPopupString()
    {
      if (!this.m_is_now_build)
        return "建造中ではありません";
      if (this.m_finish_days <= 0)
        return string.Format("[ {0} ]を建造中\n{1}日経過\n船名に 14日 のような名前を付けると\n残り日数を計算できます", (object) this.m_ship_name, (object) this.GetDays());
      if (this.GetDays() > this.m_finish_days)
        return string.Format("[ {0} ]を建造中\n{1}日経過\n完成から{2}日経過", (object) this.m_ship_name, (object) this.GetDays(), (object) (this.GetDays() - this.m_finish_days));
      if (this.GetDays() == this.m_finish_days)
        return string.Format("[ {0} ]を建造中\n{1}日経過\n完成しました", (object) this.m_ship_name, (object) this.GetDays());
      else
        return string.Format("[ {0} ]を建造中\n{1}日経過\n残り{2}日", (object) this.m_ship_name, (object) this.GetDays(), (object) (this.m_finish_days - this.GetDays()));
    }

    private int get_build_ship_days(string name)
    {
      name = Useful.AdjustNumber(name);
      Match match = Useful.match("([0-9]+)日", name);
      if (match == null)
        return -1;
      else
        return Useful.ToInt32(match.Groups[1].Value, -1);
    }
  }
}
