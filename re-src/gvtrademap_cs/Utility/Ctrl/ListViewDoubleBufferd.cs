// Type: Utility.Ctrl.ListViewDoubleBufferd
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Windows.Forms;
using Utility;

namespace Utility.Ctrl
{
  public class ListViewDoubleBufferd : ListView
  {
    private ListViewItemSorter m_sorter;

    public ToolTip ToolTip { get; set; }

    public ListViewDoubleBufferd()
    {
      try
      {
        this.DoubleBuffered = true;
      }
      catch
      {
      }
      this.ToolTip = (ToolTip) null;
      this.MouseMove += new MouseEventHandler(this.mouse_move);
      this.ColumnClick += new ColumnClickEventHandler(this.column_click);
    }

    public void EnableSort(bool is_normal)
    {
      this.m_sorter = new ListViewItemSorter(is_normal);
    }

    public void DisableSort()
    {
      this.m_sorter = (ListViewItemSorter) null;
    }

    private void mouse_move(object sender, MouseEventArgs e)
    {
      Useful.UpdateListViewSubItemToolTip((ListView) this, this.ToolTip, e.X, e.Y);
    }

    private void column_click(object sender, ColumnClickEventArgs e)
    {
      if (this.m_sorter == null)
        return;
      this.m_sorter.Sort((ListView) this, e.Column);
    }
  }
}
