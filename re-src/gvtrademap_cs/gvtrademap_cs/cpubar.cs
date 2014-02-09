// Type: gvtrademap_cs.cpubar
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using System.Drawing;

namespace gvtrademap_cs
{
  public class cpubar
  {
    private const int PEAK_WAIT = 15;
    private const int PEAK_STEP = 2;
    private d3d_device m_device;
    private int m_peak;
    private int m_peak_wait;

    public cpubar(d3d_device device)
    {
      this.m_device = device;
      this.m_peak = 0;
      this.m_peak_wait = 0;
    }

    public void Update(float val, float max)
    {
      int num = (int) ((double) val / (double) max * 200.0);
      int color;
      if (num > 200)
      {
        this.m_peak = 200;
        this.m_peak_wait = 15;
        num = 200;
        color = Color.Red.ToArgb();
      }
      else
      {
        if (num > this.m_peak)
        {
          this.m_peak = num;
          this.m_peak_wait = 15;
        }
        else if (--this.m_peak_wait <= 0)
        {
          this.m_peak -= 2;
          if (this.m_peak < 0)
            this.m_peak = 0;
          this.m_peak_wait = 0;
        }
        color = Color.LightGreen.ToArgb();
      }
      this.m_device.DrawFillRect(new Vector3(0.0f, 0.0f, 0.1f), new Vector2(200f, 4f), Color.Gray.ToArgb());
      this.m_device.DrawFillRect(new Vector3(0.0f, 0.0f, 0.1f), new Vector2((float) num, 4f), color);
      this.m_device.DrawLine(new Vector3((float) this.m_peak, 0.0f, 0.1f), new Vector2((float) this.m_peak, 4f), Color.Red.ToArgb());
      this.m_device.DrawLineRect(new Vector3(0.0f, 0.0f, 0.1f), new Vector2(200f, 4f), Color.Black.ToArgb());
    }
  }
}
