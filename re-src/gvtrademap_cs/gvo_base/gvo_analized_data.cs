// Type: gvo_base.gvo_analized_data
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

namespace gvo_base
{
  public class gvo_analized_data
  {
    private int m_days;
    private int m_pos_x;
    private int m_pos_y;
    private bool m_interest;
    private float m_angle;
    private gvo_map_cs_chat_base.accident m_accident;
    private bool m_is_start_build_ship;
    private string m_build_ship_name;
    private bool m_is_finish_build_ship;

    public bool capture_point_success
    {
      get
      {
        return this.pos_x >= 0 && this.pos_y >= 0;
      }
    }

    public bool capture_days_success
    {
      get
      {
        return this.days >= 0;
      }
    }

    public bool capture_success
    {
      get
      {
        if (!this.capture_days_success)
          return false;
        else
          return this.capture_point_success;
      }
    }

    public int days
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

    public int pos_x
    {
      get
      {
        return this.m_pos_x;
      }
      set
      {
        this.m_pos_x = value;
      }
    }

    public int pos_y
    {
      get
      {
        return this.m_pos_y;
      }
      set
      {
        this.m_pos_y = value;
      }
    }

    public float angle
    {
      get
      {
        return this.m_angle;
      }
      set
      {
        this.m_angle = value;
      }
    }

    public bool interest
    {
      get
      {
        return this.m_interest;
      }
      set
      {
        this.m_interest = value;
      }
    }

    public gvo_map_cs_chat_base.accident accident
    {
      get
      {
        return this.m_accident;
      }
      set
      {
        this.m_accident = value;
      }
    }

    public bool is_start_build_ship
    {
      get
      {
        return this.m_is_start_build_ship;
      }
      set
      {
        this.m_is_start_build_ship = value;
      }
    }

    public string build_ship_name
    {
      get
      {
        return this.m_build_ship_name;
      }
      set
      {
        this.m_build_ship_name = value;
      }
    }

    public bool is_finish_build_ship
    {
      get
      {
        return this.m_is_finish_build_ship;
      }
      set
      {
        this.m_is_finish_build_ship = value;
      }
    }

    public gvo_analized_data()
    {
      this.Clear();
    }

    public void Clear()
    {
      this.m_days = -1;
      this.m_pos_x = -1;
      this.m_pos_y = -1;
      this.m_angle = -1f;
      this.m_interest = false;
      this.m_accident = gvo_map_cs_chat_base.accident.none;
      this.m_is_start_build_ship = false;
      this.m_build_ship_name = "";
      this.m_is_finish_build_ship = false;
    }

    public gvo_analized_data Clone()
    {
      return new gvo_analized_data()
      {
        m_days = this.m_days,
        m_pos_x = this.m_pos_x,
        m_pos_y = this.m_pos_y,
        m_angle = this.m_angle,
        m_interest = this.m_interest,
        m_accident = this.m_accident,
        m_is_start_build_ship = this.m_is_start_build_ship,
        m_build_ship_name = this.m_build_ship_name,
        m_is_finish_build_ship = this.m_is_finish_build_ship
      };
    }

    public static gvo_analized_data FromAnalizedData(gvo_capture_base capture, gvo_map_cs_chat_base chat)
    {
      gvo_analized_data gvoAnalizedData = new gvo_analized_data();
      gvoAnalizedData.m_days = capture.days;
      gvoAnalizedData.m_pos_x = capture.point.X;
      gvoAnalizedData.m_pos_y = capture.point.Y;
      gvoAnalizedData.m_angle = capture.angle;
      gvoAnalizedData.m_interest = chat.is_interest;
      gvoAnalizedData.m_accident = chat._accident;
      gvoAnalizedData.m_is_start_build_ship = chat.is_start_build_ship;
      gvoAnalizedData.m_build_ship_name = chat.build_ship_name;
      gvoAnalizedData.m_is_finish_build_ship = chat.is_finish_build_ship;
      chat.ResetBuildShip();
      if (gvoAnalizedData.capture_days_success)
        chat.ResetInterest();
      if (gvoAnalizedData.capture_success)
        chat.ResetAccident();
      return gvoAnalizedData;
    }
  }
}
