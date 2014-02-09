// Type: gvtrademap_cs.interest_days
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

namespace gvtrademap_cs
{
  public class interest_days : gvo_day_counter
  {
    private GlobalSettings m_setting;

    public interest_days(GlobalSettings _setting)
    {
      this.m_setting = _setting;
      this.Days = this.m_setting.interest_days;
    }

    public void Update(int days, bool is_interest)
    {
      this.UpdateBase(days);
      if (is_interest)
        this.Reset(days);
      this.m_setting.interest_days = this.get_true_days();
    }

    public string GetPopupString()
    {
      if (this.GetDays() > 30)
        return "30日以上経過しました\n現在の経過日数は信頼できません\n利息を受け取ると正常にもどります";
      else
        return string.Format("残り{0}日\n{1}日航海中", (object) (30 - this.GetDays()), (object) (this.VoyageDays - this.VoyageDaysStart));
    }
  }
}
