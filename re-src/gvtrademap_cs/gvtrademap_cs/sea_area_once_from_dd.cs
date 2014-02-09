// Type: gvtrademap_cs.sea_area_once_from_dd
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using Utility;

namespace gvtrademap_cs
{
  public class sea_area_once_from_dd
  {
    private string m_name;
    private string m_server;
    private DateTime m_date;
    private sea_area.sea_area_once.sea_type m_sea_type;

    public string name
    {
      get
      {
        return this.m_name;
      }
    }

    public string server_str
    {
      get
      {
        return this.m_server;
      }
    }

    public DateTime date
    {
      get
      {
        return this.m_date;
      }
    }

    public string date_str
    {
      get
      {
        return Useful.TojbbsDateTimeString(this.m_date);
      }
    }

    public sea_area.sea_area_once.sea_type _sea_type
    {
      get
      {
        return this.m_sea_type;
      }
    }

    public string _sea_type_str
    {
      get
      {
        return sea_area.sea_area_once.ToString(this.m_sea_type);
      }
    }

    public sea_area_once_from_dd()
    {
      this.m_name = "";
      this.m_server = "";
      this.m_date = new DateTime();
      this.m_sea_type = sea_area.sea_area_once.sea_type.normal;
    }

    public bool Analize(string line)
    {
      try
      {
        string[] strArray = line.Split(new char[1]
        {
          ','
        });
        if (strArray.Length < 4)
          return false;
        this.m_server = strArray[0];
        this.m_name = strArray[1];
        this.m_sea_type = strArray[2].IndexOf("安全") < 0 ? (strArray[2].IndexOf("無法") < 0 ? sea_area.sea_area_once.sea_type.normal : sea_area.sea_area_once.sea_type.lawless) : sea_area.sea_area_once.sea_type.safty;
        this.m_date = Useful.ToDateTime(strArray[3]);
      }
      catch
      {
        return false;
      }
      return true;
    }
  }
}
