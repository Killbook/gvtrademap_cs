// Type: Utility.UniqueString
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Collections;
using System.Collections.Generic;

namespace Utility
{
  public sealed class UniqueString : IEnumerable<string>, IEnumerable
  {
    private const int MAX = 20;
    private const int MIN = 1;
    private List<string> m_strings;
    private int m_max;

    public string this[int i]
    {
      get
      {
        return this.m_strings[i];
      }
    }

    public int Count
    {
      get
      {
        return this.m_strings.Count;
      }
    }

    public string Top
    {
      get
      {
        if (this.m_strings.Count <= 0)
          return "";
        else
          return this.m_strings[0];
      }
    }

    public int Max
    {
      get
      {
        return this.m_max;
      }
      set
      {
        this.m_max = value;
        if (this.m_max < 1)
          this.m_max = 1;
        this.ajust_count();
      }
    }

    public UniqueString()
    {
      this.m_strings = new List<string>();
      this.m_max = 20;
    }

    public string[] ToArray()
    {
      return this.m_strings.ToArray();
    }

    public IEnumerator<string> GetEnumerator()
    {
      for (int i = 0; i < this.m_strings.Count; ++i)
        yield return this.m_strings[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    public bool Add(string str)
    {
      if (string.IsNullOrEmpty(str))
        return false;
      this.Remove(str);
      this.m_strings.Insert(0, str);
      this.ajust_count();
      return true;
    }

    public void SetRange(string[] list)
    {
      this.m_strings.Clear();
      foreach (string str in list)
        this.AddLast(str);
    }

    public bool AddLast(string str)
    {
      if (string.IsNullOrEmpty(str))
        return false;
      this.Remove(str);
      this.m_strings.Add(str);
      this.ajust_count();
      return true;
    }

    public void Clear()
    {
      this.m_strings.Clear();
    }

    public bool Remove(string str)
    {
      if (!this.has_string(str))
        return false;
      this.m_strings.Remove(str);
      return true;
    }

    public void Clone(UniqueString list)
    {
      this.Clear();
      this.Max = list.Max;
      foreach (string str in list)
        this.m_strings.Add(str);
    }

    private bool has_string(string str)
    {
      foreach (string str1 in this.m_strings)
      {
        if (str1 == str)
          return true;
      }
      return false;
    }

    private void ajust_count()
    {
      while (this.m_strings.Count > this.m_max)
        this.m_strings.RemoveAt(this.m_strings.Count - 1);
    }
  }
}
