// Type: Utility.SequentialDictionary`2
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Collections;
using System.Collections.Generic;

namespace Utility
{
  public class SequentialDictionary<TKey, TValue> : IEnumerable<TValue>, IEnumerable where TValue : IDictionaryNode<TKey>
  {
    protected List<TValue> m_sequential_database;
    protected Dictionary<TKey, TValue> m_database;

    public SequentialDictionary()
    {
      this.m_sequential_database = new List<TValue>();
      this.m_database = new Dictionary<TKey, TValue>();
    }

    public void Add(TValue t)
    {
      try
      {
        this.m_database.Add(t.Key, t);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      this.m_sequential_database.Add(t);
    }

    public void Remove(TValue t)
    {
      this.m_sequential_database.Remove(t);
      this.m_database.Remove(t.Key);
    }

    public void Remove(TKey key)
    {
      TValue t = this.GetValue(key);
      if ((object) t == null)
        return;
      this.Remove(t);
    }

    public TValue GetValue(TKey key)
    {
      TValue obj;
      if (this.m_database.TryGetValue(key, out obj))
        return obj;
      else
        return default (TValue);
    }

    public void Clear()
    {
      this.m_sequential_database.Clear();
      this.m_database.Clear();
    }

    public IEnumerator<TValue> GetEnumerator()
    {
      foreach (TValue obj in this.m_sequential_database)
        yield return obj;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
