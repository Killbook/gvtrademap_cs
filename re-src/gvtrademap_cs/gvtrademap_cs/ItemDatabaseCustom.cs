// Type: gvtrademap_cs.ItemDatabaseCustom
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using gvo_base;
using System.Collections.Generic;

namespace gvtrademap_cs
{
  public class ItemDatabaseCustom : ItemDatabase
  {
    public ItemDatabaseCustom(string fname)
      : base(fname)
    {
    }

    public void FindAll(string find_string, List<GvoDatabase.Find> list, GvoDatabase.Find.FindHandler handler)
    {
      IEnumerator<ItemDatabase.Data> enumerator = this.GetEnumerator();
      while (enumerator.MoveNext())
      {
        if (handler(enumerator.Current.Name, find_string))
          list.Add(new GvoDatabase.Find(enumerator.Current));
      }
    }

    public void FindAll_FromType(string find_string, List<GvoDatabase.Find> list, GvoDatabase.Find.FindHandler handler)
    {
      IEnumerator<ItemDatabase.Data> enumerator = this.GetEnumerator();
      while (enumerator.MoveNext())
      {
        if (handler(enumerator.Current.Type, find_string))
          list.Add(new GvoDatabase.Find(enumerator.Current));
      }
    }
  }
}
