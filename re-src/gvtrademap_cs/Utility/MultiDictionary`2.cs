// Type: Utility.MultiDictionary`2
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Collections;
using System.Collections.Generic;

namespace Utility
{
  public class MultiDictionary<TKey, TValue> : IEnumerable<TValue>, IEnumerable where TValue : IDictionaryNode<TKey>
  {
    protected Dictionary<TKey, List<TValue>> m_database;

    public MultiDictionary()
    {
      this.m_database = new Dictionary<TKey, List<TValue>>();
    }

    public void Add(TValue t)
    {
      List<TValue> list = (List<TValue>) null;
      if (this.m_database.TryGetValue(t.Key, out list))
        list.Add(t);
      else
        this.m_database.Add(t.Key, new List<TValue>()
        {
          t
        });
    }

    public void Remove(TValue t)
    {
      List<TValue> list = (List<TValue>) null;
      if (!this.m_database.TryGetValue(t.Key, out list))
        return;
      list.Remove(t);
      if (list.Count > 0)
        return;
      this.Remove(t.Key);
    }

    public void Remove(TKey key)
    {
      this.m_database.Remove(key);
    }

    public TValue GetValue(TKey key)
    {
      List<TValue> list = (List<TValue>) null;
      if (this.m_database.TryGetValue(key, out list) && list.Count > 0)
        return list[0];
      else
        return default (TValue);
    }

    public TValue[] GetValueList(TKey key)
    {
      List<TValue> list = (List<TValue>) null;
      if (this.m_database.TryGetValue(key, out list) && list.Count > 0)
        return list.ToArray();
      else
        return (TValue[]) null;
    }

    public void Clear()
    {
      this.m_database.Clear();
    }

    public IEnumerator<TValue> GetEnumerator()
    {
      foreach (KeyValuePair<TKey, List<TValue>> keyValuePair in this.m_database)
      {
        foreach (TValue obj in keyValuePair.Value)
          yield return obj;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
