// Type: gvtrademap_cs.LoadInfosStatus
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

namespace gvtrademap_cs
{
  public class LoadInfosStatus
  {
    public int NowStep { get; set; }

    public int MaxStep { get; set; }

    public string StatusMessage { get; set; }

    public void Start(int max, string message)
    {
      this.MaxStep = max;
      this.NowStep = 0;
      this.StatusMessage = message;
    }

    public void IncStep(string next_message)
    {
      this.StatusMessage = next_message;
      if (++this.NowStep < this.MaxStep)
        return;
      this.NowStep = this.MaxStep;
      this.StatusMessage = "完了";
    }
  }
}
