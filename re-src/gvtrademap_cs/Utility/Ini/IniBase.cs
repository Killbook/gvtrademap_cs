// Type: Utility.Ini.IniBase
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Collections.Generic;

namespace Utility.Ini
{
  public abstract class IniBase : IIni
  {
    protected abstract bool HasProfile(string group_name, string name);

    public abstract string GetProfile(string group_name, string name, string default_value);

    public abstract string[] GetProfile(string group_name, string name, string[] default_value);

    public abstract void SetProfile(string group_name, string name, string value);

    public abstract void SetProfile(string group_name, string name, string[] value);

    public bool GetProfile(string group_name, string name, bool default_value)
    {
      if (!this.HasProfile(group_name, name))
        return default_value;
      string profile = this.GetProfile(group_name, name, "");
      try
      {
        return this.to_bool(profile);
      }
      catch
      {
        return default_value;
      }
    }

    public int GetProfile(string group_name, string name, int default_value)
    {
      if (!this.HasProfile(group_name, name))
        return default_value;
      string profile = this.GetProfile(group_name, name, "");
      try
      {
        return this.to_int(profile);
      }
      catch
      {
        return default_value;
      }
    }

    public long GetProfile(string group_name, string name, long default_value)
    {
      if (!this.HasProfile(group_name, name))
        return default_value;
      string profile = this.GetProfile(group_name, name, "");
      try
      {
        return this.to_long(profile);
      }
      catch
      {
        return default_value;
      }
    }

    public double GetProfile(string group_name, string name, double default_value)
    {
      if (!this.HasProfile(group_name, name))
        return default_value;
      string profile = this.GetProfile(group_name, name, "");
      try
      {
        return this.to_double(profile);
      }
      catch
      {
        return default_value;
      }
    }

    public float GetProfile(string group_name, string name, float default_value)
    {
      if (!this.HasProfile(group_name, name))
        return default_value;
      string profile = this.GetProfile(group_name, name, "");
      try
      {
        return this.to_float(profile);
      }
      catch
      {
        return default_value;
      }
    }

    public void SetProfile(string group_name, string name, bool value)
    {
      this.SetProfile(group_name, name, this.to_string(value));
    }

    public void SetProfile(string group_name, string name, int value)
    {
      this.SetProfile(group_name, name, this.to_string(value));
    }

    public void SetProfile(string group_name, string name, long value)
    {
      this.SetProfile(group_name, name, this.to_string(value));
    }

    public void SetProfile(string group_name, string name, double value)
    {
      this.SetProfile(group_name, name, this.to_string(value));
    }

    public void SetProfile(string group_name, string name, float value)
    {
      this.SetProfile(group_name, name, this.to_string(value));
    }

    public bool[] GetProfile(string group_name, string name, bool[] default_value)
    {
      if (!this.HasProfile(group_name, name))
        return default_value;
      string[] profile = this.GetProfile(group_name, name, new string[0]);
      try
      {
        List<bool> list = new List<bool>();
        foreach (string str in profile)
        {
          try
          {
            bool flag = this.to_bool(str);
            list.Add(flag);
          }
          catch
          {
          }
        }
        return list.ToArray();
      }
      catch
      {
        return default_value;
      }
    }

    public int[] GetProfile(string group_name, string name, int[] default_value)
    {
      if (!this.HasProfile(group_name, name))
        return default_value;
      string[] profile = this.GetProfile(group_name, name, new string[0]);
      try
      {
        List<int> list = new List<int>();
        foreach (string str in profile)
        {
          try
          {
            int num = this.to_int(str);
            list.Add(num);
          }
          catch
          {
          }
        }
        return list.ToArray();
      }
      catch
      {
        return default_value;
      }
    }

    public long[] GetProfile(string group_name, string name, long[] default_value)
    {
      if (!this.HasProfile(group_name, name))
        return default_value;
      string[] profile = this.GetProfile(group_name, name, new string[0]);
      try
      {
        List<long> list = new List<long>();
        foreach (string str in profile)
        {
          try
          {
            long num = this.to_long(str);
            list.Add(num);
          }
          catch
          {
          }
        }
        return list.ToArray();
      }
      catch
      {
        return default_value;
      }
    }

    public double[] GetProfile(string group_name, string name, double[] default_value)
    {
      if (!this.HasProfile(group_name, name))
        return default_value;
      string[] profile = this.GetProfile(group_name, name, new string[0]);
      try
      {
        List<double> list = new List<double>();
        foreach (string str in profile)
        {
          try
          {
            double num = this.to_double(str);
            list.Add(num);
          }
          catch
          {
          }
        }
        return list.ToArray();
      }
      catch
      {
        return default_value;
      }
    }

    public float[] GetProfile(string group_name, string name, float[] default_value)
    {
      if (!this.HasProfile(group_name, name))
        return default_value;
      string[] profile = this.GetProfile(group_name, name, new string[0]);
      try
      {
        List<float> list = new List<float>();
        foreach (string str in profile)
        {
          try
          {
            float num = this.to_float(str);
            list.Add(num);
          }
          catch
          {
          }
        }
        return list.ToArray();
      }
      catch
      {
        return default_value;
      }
    }

    public void SetProfile(string group_name, string name, bool[] value)
    {
      List<string> list = new List<string>();
      foreach (bool flag in value)
        list.Add(this.to_string(flag));
      this.SetProfile(group_name, name, list.ToArray());
    }

    public void SetProfile(string group_name, string name, int[] value)
    {
      List<string> list = new List<string>();
      foreach (int num in value)
        list.Add(this.to_string(num));
      this.SetProfile(group_name, name, list.ToArray());
    }

    public void SetProfile(string group_name, string name, long[] value)
    {
      List<string> list = new List<string>();
      foreach (long num in value)
        list.Add(this.to_string(num));
      this.SetProfile(group_name, name, list.ToArray());
    }

    public void SetProfile(string group_name, string name, double[] value)
    {
      List<string> list = new List<string>();
      foreach (double num in value)
        list.Add(this.to_string(num));
      this.SetProfile(group_name, name, list.ToArray());
    }

    public void SetProfile(string group_name, string name, float[] value)
    {
      List<string> list = new List<string>();
      foreach (float num in value)
        list.Add(this.to_string(num));
      this.SetProfile(group_name, name, list.ToArray());
    }

    private string to_string(bool value)
    {
      return value ? "1" : "0";
    }

    private string to_string(int value)
    {
      return value.ToString();
    }

    private string to_string(long value)
    {
      return value.ToString();
    }

    private string to_string(double value)
    {
      return value.ToString();
    }

    private string to_string(float value)
    {
      return value.ToString();
    }

    private bool to_bool(string str)
    {
      if (string.IsNullOrEmpty(str))
        throw new Exception();
      return !(str == "0");
    }

    private int to_int(string str)
    {
      if (string.IsNullOrEmpty(str))
        throw new Exception();
      else
        return Convert.ToInt32(str);
    }

    private long to_long(string str)
    {
      if (string.IsNullOrEmpty(str))
        throw new Exception();
      else
        return Convert.ToInt64(str);
    }

    private double to_double(string str)
    {
      if (string.IsNullOrEmpty(str))
        throw new Exception();
      else
        return Convert.ToDouble(str);
    }

    private float to_float(string str)
    {
      if (string.IsNullOrEmpty(str))
        throw new Exception();
      else
        return (float) Convert.ToDouble(str);
    }
  }
}
