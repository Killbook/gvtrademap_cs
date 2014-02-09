// Type: Utility.hittest_list
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Utility
{
  public class hittest_list : IEnumerable<hittest>, IEnumerable
  {
    private List<hittest> m_list;

    public int Count
    {
      get
      {
        return this.m_list.Count;
      }
    }

    public hittest this[int index]
    {
      get
      {
        return this.m_list[index];
      }
    }

    public hittest_list()
    {
      this.m_list = new List<hittest>();
    }

    public hittest Add(hittest _hittest)
    {
      this.m_list.Add(_hittest);
      return _hittest;
    }

    public void Remove(hittest _hittest)
    {
      this.m_list.Remove(_hittest);
    }

    public int HitTest_Index(Point pos)
    {
      int num = 0;
      foreach (hittest hittest in this.m_list)
      {
        if (hittest.HitTest(pos))
          return num;
        ++num;
      }
      return -1;
    }

    public hittest HitTest(Point pos)
    {
      foreach (hittest hittest in this.m_list)
      {
        if (hittest.HitTest(pos))
          return hittest;
      }
      return (hittest) null;
    }

    public int HitTest_Index(Point pos, int type)
    {
      int num = 0;
      foreach (hittest hittest in this.m_list)
      {
        if (hittest.HitTest(pos, type))
          return num;
        ++num;
      }
      return -1;
    }

    public hittest HitTest(Point pos, int type)
    {
      foreach (hittest hittest in this.m_list)
      {
        if (hittest.HitTest(pos, type))
          return hittest;
      }
      return (hittest) null;
    }

    public IEnumerator<hittest> GetEnumerator()
    {
      foreach (hittest hittest in this.m_list)
        yield return hittest;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    public void Clear()
    {
      this.m_list.Clear();
    }
  }
}
