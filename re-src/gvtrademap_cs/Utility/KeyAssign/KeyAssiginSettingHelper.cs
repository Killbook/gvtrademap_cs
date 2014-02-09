// Type: Utility.KeyAssign.KeyAssiginSettingHelper
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Utility.KeyAssign
{
  public sealed class KeyAssiginSettingHelper
  {
    private ComboBox m_select_group_cbox;
    private Button m_assign_button;
    private Button m_remove_assign_button;
    private Button m_default_all_assign_button;
    private ListView m_list_view;
    private Form m_form;
    private KeyAssignList m_assign_list;

    public KeyAssignList List
    {
      get
      {
        return this.m_assign_list;
      }
    }

    public KeyAssiginSettingHelper(KeyAssignList assign_list, Form form, ComboBox cbox, ListView list_view, Button assign_button, Button remove_assign_button, Button default_all_assign_button)
    {
      this.m_assign_list = assign_list.DeepClone();
      this.m_form = form;
      this.m_select_group_cbox = cbox;
      this.m_list_view = list_view;
      this.m_assign_button = assign_button;
      this.m_remove_assign_button = remove_assign_button;
      this.m_default_all_assign_button = default_all_assign_button;
      this.init_ctrl();
      this.init();
      this.update_assign_button();
    }

    private void init_ctrl()
    {
      if (this.m_select_group_cbox != null)
      {
        this.m_select_group_cbox.DropDownStyle = ComboBoxStyle.DropDownList;
        this.m_select_group_cbox.FormattingEnabled = true;
        this.m_select_group_cbox.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
      }
      if (this.m_assign_button != null)
        this.m_assign_button.Click += new EventHandler(this.button1_Click);
      if (this.m_remove_assign_button != null)
        this.m_remove_assign_button.Click += new EventHandler(this.button2_Click);
      if (this.m_default_all_assign_button != null)
        this.m_default_all_assign_button.Click += new EventHandler(this.button3_Click);
      if (this.m_list_view == null)
        return;
      this.m_list_view.FullRowSelect = true;
      this.m_list_view.GridLines = true;
      this.m_list_view.HideSelection = false;
      this.m_list_view.MultiSelect = false;
      this.m_list_view.ShowItemToolTips = true;
      this.m_list_view.UseCompatibleStateImageBehavior = false;
      this.m_list_view.View = View.Details;
      this.m_list_view.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
      this.m_list_view.DoubleClick += new EventHandler(this.listView1_DoubleClick);
    }

    private void init()
    {
      this.m_list_view.Columns.Add("グル\x30FCプ", 90);
      this.m_list_view.Columns.Add("機能", 180);
      this.m_list_view.Columns.Add("割り当て", 100);
      List<string> groupList = this.m_assign_list.GetGroupList();
      if (groupList == null)
        return;
      if (this.m_select_group_cbox != null)
      {
        this.m_select_group_cbox.Items.Clear();
        this.m_select_group_cbox.Items.Add((object) "すべて");
        foreach (object obj in groupList)
          this.m_select_group_cbox.Items.Add(obj);
        if (this.m_select_group_cbox.Items.Count <= 0)
          return;
        this.m_select_group_cbox.SelectedIndex = 0;
      }
      else
        this.update_list();
    }

    private void update_list()
    {
      this.m_list_view.BeginUpdate();
      this.m_list_view.Items.Clear();
      List<KeyAssignList.Assign> list = (List<KeyAssignList.Assign>) null;
      if (this.m_select_group_cbox != null && this.m_select_group_cbox.Text != "すべて")
        list = this.m_assign_list.GetAssignedListFromGroup(this.m_select_group_cbox.Text);
      if (list != null)
      {
        foreach (KeyAssignList.Assign i in list)
          this.add_item(this.m_list_view, i);
      }
      else
      {
        foreach (KeyAssignList.Assign i in this.m_assign_list)
          this.add_item(this.m_list_view, i);
      }
      this.m_list_view.EndUpdate();
      this.update_assign_button();
    }

    private void add_item(ListView listview, KeyAssignList.Assign i)
    {
      listview.Items.Add(new ListViewItem(i.Group, 0)
      {
        Tag = (object) i,
        SubItems = {
          i.Name,
          i.KeysString
        }
      });
    }

    private void update_assign_button()
    {
      KeyAssignList.Assign selectedItem = this.get_selected_item();
      if (selectedItem != null)
      {
        this.update_button(this.m_assign_button, true);
        this.update_button(this.m_remove_assign_button, selectedItem.Keys != Keys.None);
      }
      else
      {
        this.update_button(this.m_assign_button, false);
        this.update_button(this.m_remove_assign_button, false);
      }
    }

    private void update_button(Button button, bool enable)
    {
      if (button == null || button.Enabled == enable)
        return;
      button.Enabled = enable;
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.update_assign_button();
    }

    private void listView1_DoubleClick(object sender, EventArgs e)
    {
      this.button1_Click(sender, e);
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.update_list();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      KeyAssignList.Assign selectedItem = this.get_selected_item();
      if (selectedItem == null)
        return;
      using (KeyAssignForm keyAssignForm = new KeyAssignForm(selectedItem))
      {
        if (keyAssignForm.ShowDialog((IWin32Window) this.m_form) != DialogResult.OK)
          return;
        selectedItem.Keys = keyAssignForm.NewAssignKey;
        this.m_list_view.SelectedItems[0].SubItems[2].Text = selectedItem.KeysString;
        this.update_assign_button();
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      KeyAssignList.Assign selectedItem = this.get_selected_item();
      if (selectedItem == null)
        return;
      selectedItem.Keys = Keys.None;
      this.m_list_view.SelectedItems[0].SubItems[2].Text = selectedItem.KeysString;
      this.update_assign_button();
    }

    private KeyAssignList.Assign get_selected_item()
    {
      if (this.m_list_view.SelectedItems.Count <= 0)
        return (KeyAssignList.Assign) null;
      else
        return this.m_list_view.SelectedItems[0].Tag as KeyAssignList.Assign;
    }

    private void button3_Click(object sender, EventArgs e)
    {
      this.m_assign_list.DefaultAll();
      foreach (ListViewItem listViewItem in this.m_list_view.Items)
      {
        KeyAssignList.Assign assign = listViewItem.Tag as KeyAssignList.Assign;
        if (assign != null)
          listViewItem.SubItems[2].Text = assign.KeysString;
      }
    }
  }
}
