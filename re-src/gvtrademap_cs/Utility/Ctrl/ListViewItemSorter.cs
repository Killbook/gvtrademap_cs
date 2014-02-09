// Type: Utility.Ctrl.ListViewItemSorter
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Collections;
using System.Windows.Forms;

namespace Utility.Ctrl
{
  public class ListViewItemSorter
  {
    private int m_sort_order;
    private int m_default_sort_oder;
    private int m_current_colum_index;

    public bool IsSortOrderNormal
    {
      get
      {
        return this.m_sort_order > 0;
      }
    }

    public bool IsDefaultSortOderNormal
    {
      get
      {
        return this.m_default_sort_oder > 0;
      }
    }

    public ListViewItemSorter()
      : this(true)
    {
    }

    public ListViewItemSorter(bool is_default_sort_oder_normal)
    {
      this.m_current_colum_index = -1;
      this.SetDefaultSortOrder(is_default_sort_oder_normal);
      this.ResetSortOrder();
    }

    public void SetSortOrder(bool is_normal)
    {
      this.m_sort_order = 1;
      if (is_normal)
        return;
      this.m_sort_order *= -1;
    }

    public void SetDefaultSortOrder(bool is_normal)
    {
      this.m_default_sort_oder = 1;
      if (is_normal)
        return;
      this.m_default_sort_oder *= -1;
    }

    public void ResetSortOrder()
    {
      this.SetSortOrder(this.IsDefaultSortOderNormal);
    }

    public void FlipSortOrder()
    {
      this.m_sort_order *= -1;
    }

    public bool Sort(ListView listview, int colum_index)
    {
      if (colum_index >= listview.Columns.Count)
        return false;
      if (this.m_current_colum_index != colum_index)
        this.ResetSortOrder();
      this.m_current_colum_index = colum_index;
      listview.ListViewItemSorter = (IComparer) new ListViewItemSorter.ListViewItemComparer(colum_index, this.m_sort_order);
      listview.Sort();
      this.FlipSortOrder();
      return true;
    }

    private class ListViewItemComparer : IComparer
    {
      private int col;
      private int sortOrder;

      public ListViewItemComparer(int col, int sortOrder)
      {
        this.col = col;
        this.sortOrder = sortOrder;
      }

      public int Compare(object x, object y)
      {
        ListViewItem listViewItem1 = x as ListViewItem;
        ListViewItem listViewItem2 = y as ListViewItem;
        if (listViewItem1 == null || listViewItem2 == null || (this.col >= listViewItem1.SubItems.Count || this.col >= listViewItem2.SubItems.Count))
          return 0;
        string text1 = listViewItem1.SubItems[this.col].Text;
        string text2 = listViewItem2.SubItems[this.col].Text;
        double result1;
        double result2;
        if (!double.TryParse(text1, out result1) || !double.TryParse(text2, out result2))
          return this.cmp_string(text1, text2) * this.sortOrder;
        if (result1 == result2)
          return 0;
        if (result1 < result2)
          return -1 * this.sortOrder;
        else
          return this.sortOrder;
      }

      private int cmp_string(string cmp1, string cmp2)
      {
        return string.Compare(cmp1, cmp2);
      }
    }
  }
}
