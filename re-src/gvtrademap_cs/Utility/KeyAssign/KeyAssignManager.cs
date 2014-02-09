// Type: Utility.KeyAssign.KeyAssignManager
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Utility.Ini;

namespace Utility.KeyAssign
{
  public class KeyAssignManager : IIniSaveLoad
  {
    private KeyAssignList m_list;
    private OnProcessCmdKey m_on_processcmdkey;
    private EventHandler m_on_update_assign_list;

    public KeyAssignList List
    {
      get
      {
        return this.m_list;
      }
      set
      {
        this.m_list = value;
        if (this.m_on_update_assign_list == null)
          return;
        this.m_on_update_assign_list((object) this, EventArgs.Empty);
      }
    }

    public string DefaultIniGroupName
    {
      get
      {
        return "KeyAssignManagerSettings";
      }
    }

    public event OnProcessCmdKey OnProcessCmdKey
    {
      add
      {
        this.m_on_processcmdkey += value;
      }
      remove
      {
        this.m_on_processcmdkey -= value;
      }
    }

    public event EventHandler OnUpdateAssignList
    {
      add
      {
        this.m_on_update_assign_list += value;
      }
      remove
      {
        this.m_on_update_assign_list -= value;
      }
    }

    public KeyAssignManager()
      : this(new KeyAssignRule())
    {
    }

    public KeyAssignManager(KeyAssignRule rule)
    {
      this.m_list = new KeyAssignList(rule);
    }

    public bool ProcessCmdKey(Keys keyData)
    {
      if (this.m_on_processcmdkey == null)
        throw new Exception("OnProcessCmdKeyにデリゲ\x30FCトを登録してから使用してください。");
      List<KeyAssignList.Assign> assignedList = this.m_list.GetAssignedList(keyData);
      if (assignedList == null)
        return false;
      foreach (KeyAssignList.Assign assign in assignedList)
        this.m_on_processcmdkey((object) this, new KeyAssignEventArg(assign.Tag));
      return true;
    }

    public void OnClickToolStripMenu(object sender, EventArgs e)
    {
      if (this.m_on_processcmdkey == null)
        throw new Exception("OnProcessCmdKeyにデリゲ\x30FCトを登録してから使用してください。");
      if (!(sender is ToolStripMenuItem))
        return;
      ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem) sender;
      if (toolStripMenuItem.Tag == null)
        return;
      this.m_on_processcmdkey((object) this, new KeyAssignEventArg(toolStripMenuItem.Tag));
    }

    public void BindTagForMenuItem(ToolStripMenuItem item, object tag)
    {
      if (item == null)
        throw new ArgumentNullException();
      if (tag == null)
        throw new ArgumentNullException();
      item.Tag = tag;
      item.Click += new EventHandler(this.OnClickToolStripMenu);
    }

    public void UpdateMenuShortcutKeys(ContextMenuStrip menu)
    {
      foreach (ToolStripItem toolStripItem in (ArrangedElementCollection) menu.Items)
        this.update_shortcut_keys(toolStripItem);
    }

    public void UpdateMenuShortcutKeys(MenuStrip menu)
    {
      foreach (ToolStripItem toolStripItem in (ArrangedElementCollection) menu.Items)
        this.update_shortcut_keys(toolStripItem);
    }

    private void update_shortcut_keys(ToolStripItem item)
    {
      if (!(item is ToolStripMenuItem))
        return;
      ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem) item;
      foreach (ToolStripItem toolStripItem in (ArrangedElementCollection) toolStripMenuItem.DropDownItems)
        this.update_shortcut_keys(toolStripItem);
      if (toolStripMenuItem.Tag == null)
        return;
      KeyAssignList.Assign assign = this.m_list.GetAssign(toolStripMenuItem.Tag);
      if (assign == null)
        return;
      toolStripMenuItem.ShortcutKeys = Keys.None;
      toolStripMenuItem.ShowShortcutKeys = true;
      toolStripMenuItem.ShortcutKeyDisplayString = assign.Keys == Keys.None ? "" : assign.KeysString;
    }

    public void IniLoad(IIni p, string group)
    {
      if (string.IsNullOrEmpty(group) || p == null)
        return;
      this.m_list.IniLoad(p, group);
    }

    public void IniSave(IIni p, string group)
    {
      if (string.IsNullOrEmpty(group) || p == null)
        return;
      this.m_list.IniSave(p, group);
    }

    public bool ShowSettingDialog(Form form)
    {
      using (KeyAssignListForm keyAssignListForm = new KeyAssignListForm(this.m_list))
      {
        if (keyAssignListForm.ShowDialog((IWin32Window) form) == DialogResult.OK)
        {
          this.List = keyAssignListForm.List;
          return true;
        }
      }
      return false;
    }
  }
}
