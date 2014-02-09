// Type: directx.d3d_windows
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace directx
{
  public class d3d_windows
  {
    private d3d_device m_device;
    private List<d3d_windows.window> m_windows;

    public List<d3d_windows.window> window_list
    {
      get
      {
        return this.m_windows;
      }
    }

    public d3d_windows(d3d_device device)
    {
      this.m_device = device;
      this.m_windows = new List<d3d_windows.window>();
    }

    public void Update()
    {
      foreach (d3d_windows.window window in this.m_windows)
        window.Update();
    }

    public void Draw()
    {
      foreach (d3d_windows.window window in this.m_windows)
        window.Draw();
    }

    public d3d_windows.window Add(d3d_windows.window _window)
    {
      _window.ctrl = this;
      this.m_windows.Add(_window);
      return _window;
    }

    public void Remove(d3d_windows.window _window)
    {
      try
      {
        this.m_windows.Remove(_window);
      }
      catch
      {
      }
    }

    public bool HitTest(Vector2 pos)
    {
      foreach (d3d_windows.window window in this.m_windows)
      {
        if (window.HitTest(pos) != d3d_windows.hit_check.outside)
          return true;
      }
      return false;
    }

    public bool HitTest(Point pos)
    {
      foreach (d3d_windows.window window in this.m_windows)
      {
        if (window.HitTest(pos) != d3d_windows.hit_check.outside)
          return true;
      }
      return false;
    }

    public bool OnMouseDoubleClick(Point pos, MouseButtons button)
    {
      foreach (d3d_windows.window window in this.m_windows)
      {
        if (window.OnMouseDClik(pos, button))
          return true;
      }
      return false;
    }

    public bool OnMouseDown(Point pos, MouseButtons button)
    {
      foreach (d3d_windows.window window in this.m_windows)
      {
        if (window.OnMouseDown(pos, button))
          return true;
      }
      return false;
    }

    public bool OnMouseClick(Point pos, MouseButtons button)
    {
      foreach (d3d_windows.window window in this.m_windows)
      {
        if (window.OnMouseClik(pos, button))
          return true;
      }
      return false;
    }

    public bool OnMouseWheel(Point pos, int delta)
    {
      foreach (d3d_windows.window window in this.m_windows)
      {
        if (window.OnMouseWheel(pos, delta))
          return true;
      }
      return false;
    }

    public string GetToolTipString(Point pos)
    {
      foreach (d3d_windows.window window in this.m_windows)
      {
        string str = window.OnToolTipString(pos);
        if (str != null)
          return str;
      }
      return (string) null;
    }

    public d3d_windows.window FindWindow(string title)
    {
      foreach (d3d_windows.window window in this.m_windows)
      {
        if (window.title == title)
          return window;
      }
      return (d3d_windows.window) null;
    }

    public enum hit_check
    {
      title,
      title_button,
      client,
      inside,
      outside,
    }

    public class window
    {
      private const int SMALL_HEADER_WIDTH = 48;
      private d3d_windows m_ctrl;
      private d3d_device m_device;
      private d3d_windows.window.mode m_window_mode;
      private bool m_is_draw_header;
      private Vector2 m_pos;
      private Vector2 m_size;
      private Vector2 m_client_pos;
      private Vector2 m_client_size;
      private Vector2 m_screen_size;
      private float m_z;
      private int m_back_color;
      private int m_title_color;
      private int m_frame_color;
      private string m_title;

      public d3d_device device
      {
        get
        {
          return this.m_device;
        }
      }

      public Vector2 pos
      {
        get
        {
          return this.m_pos;
        }
        set
        {
          this.m_pos = value;
          this.update_pos();
        }
      }

      public Vector2 size
      {
        get
        {
          if (this.m_window_mode == d3d_windows.window.mode.small)
            return this.small_size;
          else
            return this.normal_size;
        }
        set
        {
          this.m_size = value;
          this.update_size();
        }
      }

      public Vector2 normal_size
      {
        get
        {
          return this.m_size;
        }
      }

      public Vector2 small_size
      {
        get
        {
          return new Vector2(48f, 10f);
        }
      }

      public int back_color
      {
        get
        {
          return this.m_back_color;
        }
        set
        {
          this.m_back_color = value;
        }
      }

      public int title_color
      {
        get
        {
          return this.m_title_color;
        }
        set
        {
          this.m_title_color = value;
        }
      }

      public int frame_color
      {
        get
        {
          return this.m_frame_color;
        }
        set
        {
          this.m_frame_color = value;
        }
      }

      public Vector2 client_pos
      {
        get
        {
          return this.m_pos + this.m_client_pos;
        }
      }

      public Vector2 client_size
      {
        get
        {
          return this.m_client_size;
        }
        set
        {
          this.m_client_size = value;
          this.update_client_size();
        }
      }

      public float z
      {
        get
        {
          return this.m_z;
        }
      }

      public d3d_windows.window.mode window_mode
      {
        get
        {
          return this.m_window_mode;
        }
        set
        {
          this.m_window_mode = value;
        }
      }

      public Vector2 screen_size
      {
        get
        {
          return this.m_screen_size;
        }
      }

      public bool is_draw_header
      {
        get
        {
          return this.m_is_draw_header;
        }
        set
        {
          this.m_is_draw_header = value;
        }
      }

      public string title
      {
        get
        {
          return this.m_title;
        }
        set
        {
          this.m_title = value;
        }
      }

      internal d3d_windows ctrl
      {
        set
        {
          this.m_ctrl = value;
        }
      }

      public window(d3d_device device, Vector2 pos, Vector2 size, float z)
      {
        this.m_device = device;
        this.m_pos = pos;
        this.m_size = size;
        this.m_z = z;
        this.title = "タイトル";
        this.m_back_color = Color.FromArgb(180, 170, 170, 170).ToArgb();
        this.title_color = Color.SkyBlue.ToArgb();
        this.m_frame_color = Color.Black.ToArgb();
        this.m_window_mode = d3d_windows.window.mode.normal;
        this.m_screen_size = new Vector2(0.0f, 0.0f);
        this.is_draw_header = true;
        this.update_pos();
        this.update_size();
      }

      private void update_pos()
      {
        if ((double) this.m_pos.X < 0.0)
          this.m_pos.X = 0.0f;
        if ((double) this.m_pos.X >= (double) this.m_device.client_size.X - 10.0)
          this.m_pos.X = this.m_device.client_size.X - 10f;
        if ((double) this.m_pos.Y < 0.0)
          this.m_pos.Y = 0.0f;
        if ((double) this.m_pos.Y < (double) this.m_device.client_size.Y - 10.0)
          return;
        this.m_pos.Y = this.m_device.client_size.Y - 10f;
      }

      private void update_size()
      {
        if ((double) this.m_size.X < 30.0)
          this.m_size.X = 10f;
        if ((double) this.m_size.Y < 18.0)
          this.m_size.Y = 18f;
        if (this.is_draw_header)
        {
          this.m_client_pos = new Vector2(4f, 14f);
          this.m_client_size = new Vector2(this.m_size.X - 8f, this.m_size.Y - 18f);
        }
        else
        {
          this.m_client_pos = new Vector2(4f, 4f);
          this.m_client_size = new Vector2(this.m_size.X - 8f, this.m_size.Y - 8f);
        }
      }

      private void update_client_size()
      {
        this.m_size.X = this.m_client_size.X + 8f;
        this.m_size.Y = this.m_client_size.Y + 8f;
        if (this.is_draw_header)
          this.m_size.Y += 10f;
        this.update_size();
      }

      public bool OnMouseDown(Point pos, MouseButtons button)
      {
        switch (this.HitTest(pos))
        {
          case d3d_windows.hit_check.title:
          case d3d_windows.hit_check.title_button:
          case d3d_windows.hit_check.inside:
            return true;
          case d3d_windows.hit_check.client:
            this.OnMouseDownClient(pos, button);
            return true;
          default:
            return false;
        }
      }

      public bool OnMouseClik(Point pos, MouseButtons button)
      {
        switch (this.HitTest(pos))
        {
          case d3d_windows.hit_check.title:
          case d3d_windows.hit_check.inside:
            return true;
          case d3d_windows.hit_check.title_button:
            if ((button & MouseButtons.Left) != MouseButtons.None)
              this.ToggleWindowMode();
            return true;
          case d3d_windows.hit_check.client:
            this.OnMouseClikClient(pos, button);
            return true;
          default:
            return false;
        }
      }

      public bool OnMouseDClik(Point pos, MouseButtons button)
      {
        switch (this.HitTest(pos))
        {
          case d3d_windows.hit_check.title:
          case d3d_windows.hit_check.title_button:
            if ((button & MouseButtons.Left) != MouseButtons.None)
              this.ToggleWindowMode();
            return true;
          case d3d_windows.hit_check.client:
            this.OnMouseDClikClient(pos, button);
            return true;
          case d3d_windows.hit_check.inside:
            return true;
          default:
            return false;
        }
      }

      public bool OnMouseWheel(Point pos, int delta)
      {
        switch (this.HitTest(pos))
        {
          case d3d_windows.hit_check.title:
          case d3d_windows.hit_check.title_button:
          case d3d_windows.hit_check.inside:
            return true;
          case d3d_windows.hit_check.client:
            this.OnMouseWheelClient(pos, delta);
            return true;
          default:
            return false;
        }
      }

      public string OnToolTipString(Point pos)
      {
        switch (this.HitTest(pos))
        {
          case d3d_windows.hit_check.title:
            if (this.window_mode == d3d_windows.window.mode.normal)
              return this.title + "\nダブルクリックで最小化";
            else
              return this.title + "\nダブルクリックで元のサイズに戻す";
          case d3d_windows.hit_check.title_button:
            return this.window_mode == d3d_windows.window.mode.normal ? "クリックで最小化" : "クリックで元のサイズに戻す";
          case d3d_windows.hit_check.client:
            return this.OnToolTipStringClient(pos);
          default:
            return (string) null;
        }
      }

      public void Update()
      {
        this.m_screen_size = this.m_device.client_size;
        this.OnUpdateClient();
      }

      public void Draw()
      {
        if (this.m_window_mode == d3d_windows.window.mode.small)
        {
          this.m_device.DrawFillRect(new Vector3(this.m_pos.X, this.m_pos.Y, this.m_z), new Vector2(48f, 10f), this.m_title_color & 16777215 | 1073741824);
          this.m_device.DrawLineRect(new Vector3(this.m_pos.X, this.m_pos.Y, this.m_z), new Vector2(48f, 10f), this.m_frame_color);
          this.m_device.DrawLineRect(new Vector3((float) ((double) this.m_pos.X + 48.0 - 10.0), this.m_pos.Y, this.m_z), new Vector2(10f, 10f), Color.Black.ToArgb());
          this.m_device.DrawLineRect(new Vector3((float) ((double) this.m_pos.X + 48.0 - 7.0), this.m_pos.Y + 3f, this.m_z), new Vector2(4f, 4f), Color.FromArgb(50, 50, 50).ToArgb());
        }
        else
        {
          this.m_device.DrawFillRect(new Vector3(this.m_pos.X, this.m_pos.Y, this.m_z), this.m_size, this.m_back_color);
          if (this.is_draw_header)
          {
            this.m_device.DrawFillRect(new Vector3(this.m_pos.X, this.m_pos.Y, this.m_z), new Vector2(this.m_size.X, 10f), this.m_title_color);
            this.m_device.DrawLineRect(new Vector3(this.m_pos.X, this.m_pos.Y, this.m_z), new Vector2(this.m_size.X, 10f), this.m_frame_color);
          }
          this.m_device.DrawLineRect(new Vector3(this.m_pos.X, this.m_pos.Y, this.m_z), this.m_size, this.m_frame_color);
          this.m_device.DrawFillRect(new Vector3((float) ((double) this.m_pos.X + (double) this.m_size.X - 10.0), this.m_pos.Y, this.m_z), new Vector2(10f, 10f), Color.LightGray.ToArgb());
          this.m_device.DrawLineRect(new Vector3((float) ((double) this.m_pos.X + (double) this.m_size.X - 10.0), this.m_pos.Y, this.m_z), new Vector2(10f, 10f), Color.Black.ToArgb());
          this.m_device.DrawLine(new Vector3((float) ((double) this.m_pos.X + (double) this.m_size.X - 7.0), this.m_pos.Y + 7f, this.m_z), new Vector2((float) ((double) this.m_pos.X + (double) this.m_size.X - 2.0), this.m_pos.Y + 7f), Color.FromArgb(50, 50, 50).ToArgb());
          if ((double) this.m_client_size.Y < 0.0)
            return;
          this.OnDrawClient();
        }
      }

      public void DrawCurrentButtonBack(Vector3 pos, Vector2 size)
      {
        int color = this.title_color & 16777215 | int.MinValue;
        this.device.DrawFillRect(pos, size, color);
        this.device.DrawLineRect(pos, size, Color.FromArgb((int) byte.MaxValue, 0, 0, (int) byte.MaxValue).ToArgb());
      }

      public void DrawCurrentButtonBack_WithoutFrame(Vector3 pos, Vector2 size)
      {
        int color = this.title_color & 16777215 | int.MinValue;
        this.device.DrawFillRect(pos, size, color);
      }

      public d3d_windows.hit_check HitTest(Point pos)
      {
        return this.HitTest(new Vector2((float) pos.X, (float) pos.Y));
      }

      public d3d_windows.hit_check HitTest(Vector2 pos)
      {
        if (this.m_window_mode == d3d_windows.window.mode.small)
        {
          if ((double) pos.X < (double) this.m_pos.X || (double) pos.X >= (double) this.m_pos.X + 48.0 || ((double) pos.Y < (double) this.m_pos.Y || (double) pos.Y >= (double) this.m_pos.Y + 10.0))
            return d3d_windows.hit_check.outside;
          return (double) pos.X < (double) this.m_pos.X + 48.0 - 10.0 ? d3d_windows.hit_check.title : d3d_windows.hit_check.title_button;
        }
        else
        {
          if ((double) pos.X < (double) this.m_pos.X || (double) pos.X >= (double) this.m_pos.X + (double) this.m_size.X || ((double) pos.Y < (double) this.m_pos.Y || (double) pos.Y >= (double) this.m_pos.Y + (double) this.m_size.Y))
            return d3d_windows.hit_check.outside;
          if (this.is_draw_header && (double) pos.Y < (double) this.m_pos.Y + 10.0)
            return (double) pos.X < (double) this.m_pos.X + (double) this.m_size.X - 10.0 ? d3d_windows.hit_check.title : d3d_windows.hit_check.title_button;
          else
            return (double) pos.X < (double) this.client_pos.X || (double) pos.X >= (double) this.client_pos.X + (double) this.client_size.X || ((double) pos.Y < (double) this.client_pos.Y || (double) pos.Y >= (double) this.client_pos.Y + (double) this.client_size.Y) ? d3d_windows.hit_check.inside : d3d_windows.hit_check.client;
        }
      }

      public void ToggleWindowMode()
      {
        if (this.m_window_mode == d3d_windows.window.mode.small)
          this.m_window_mode = d3d_windows.window.mode.normal;
        else
          this.m_window_mode = d3d_windows.window.mode.small;
      }

      public d3d_windows.window FindWindow(string title)
      {
        if (this.m_ctrl == null)
          return (d3d_windows.window) null;
        else
          return this.m_ctrl.FindWindow(title);
      }

      protected virtual void OnUpdateClient()
      {
      }

      protected virtual void OnDrawClient()
      {
      }

      protected virtual void OnMouseDownClient(Point pos, MouseButtons button)
      {
      }

      protected virtual void OnMouseClikClient(Point pos, MouseButtons button)
      {
      }

      protected virtual void OnMouseDClikClient(Point pos, MouseButtons button)
      {
      }

      protected virtual void OnMouseWheelClient(Point pos, int delta)
      {
      }

      protected virtual string OnToolTipStringClient(Point pos)
      {
        return (string) null;
      }

      public enum mode
      {
        normal,
        small,
      }
    }
  }
}
