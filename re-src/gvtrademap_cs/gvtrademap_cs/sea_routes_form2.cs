// Type: gvtrademap_cs.sea_routes_form2
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Utility;

namespace gvtrademap_cs
{
  public class sea_routes_form2 : Form
  {
    private IContainer components;
    private Button button1;
    private ToolTip toolTip1;
    private TabControl tabControl1;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private ListView listView1;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem add_AToolStripMenuItem;
    private ToolStripMenuItem delete_DToolStripMenuItem;
    private ToolStripMenuItem show_SToolStripMenuItem;
    private ListView listView2;
    private Label label1;
    private Button button3;
    private TabPage tabPage3;
    private ListView listView3;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem all_select_AToolStripMenuItem;
    private ToolStripMenuItem move_trash_RToolStripMenuItem;
    private ContextMenuStrip contextMenuStrip2;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripMenuItem toolStripMenuItem3;
    private ToolStripMenuItem toolStripMenuItem4;
    private ToolStripSeparator toolStripSeparator4;
    private ToolStripMenuItem toolStripMenuItem5;
    private ContextMenuStrip contextMenuStrip3;
    private ToolStripMenuItem toolStripMenuItem6;
    private ToolStripMenuItem toolStripMenuItem8;
    private ToolStripSeparator toolStripSeparator6;
    private ToolStripMenuItem toolStripMenuItem9;
    private ToolStripMenuItem hide_ToolStripMenuItem;
    private ToolStripMenuItem toolStripMenuItem7;
    private ToolStripMenuItem toolStripMenuItem2;
    private gvt_lib m_lib;
    private GvoDatabase m_db;
    private bool m_disable_update_select;
    private sea_routes_form2.list_view_db m_view1;
    private sea_routes_form2.list_view_db m_view2;
    private sea_routes_form2.list_view_db m_view3;

    public sea_routes_form2(gvt_lib lib, GvoDatabase db)
    {
      this.m_lib = lib;
      this.m_db = db;
      this.InitializeComponent();
      Useful.SetFontMeiryo((Form) this, 8f);
      this.m_disable_update_select = false;
      this.toolTip1.AutoPopDelay = 30000;
      this.toolTip1.BackColor = Color.LightYellow;
      this.m_view1 = new sea_routes_form2.list_view_db(this.listView1, this.m_db.SeaRoute.searoutes);
      this.m_view2 = new sea_routes_form2.list_view_db(this.listView2, this.m_db.SeaRoute.favorite_sea_routes);
      this.m_view3 = new sea_routes_form2.list_view_db(this.listView3, this.m_db.SeaRoute.trash_sea_routes);
      this.init_page1();
      this.init_page2();
      this.init_page3();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.button1 = new Button();
      this.toolTip1 = new ToolTip(this.components);
      this.listView1 = new ListView();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.show_SToolStripMenuItem = new ToolStripMenuItem();
      this.hide_ToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.add_AToolStripMenuItem = new ToolStripMenuItem();
      this.delete_DToolStripMenuItem = new ToolStripMenuItem();
      this.move_trash_RToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.all_select_AToolStripMenuItem = new ToolStripMenuItem();
      this.listView2 = new ListView();
      this.contextMenuStrip2 = new ContextMenuStrip(this.components);
      this.toolStripMenuItem7 = new ToolStripMenuItem();
      this.toolStripMenuItem2 = new ToolStripMenuItem();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.toolStripMenuItem3 = new ToolStripMenuItem();
      this.toolStripMenuItem4 = new ToolStripMenuItem();
      this.toolStripSeparator4 = new ToolStripSeparator();
      this.toolStripMenuItem5 = new ToolStripMenuItem();
      this.listView3 = new ListView();
      this.contextMenuStrip3 = new ContextMenuStrip(this.components);
      this.toolStripMenuItem6 = new ToolStripMenuItem();
      this.toolStripMenuItem8 = new ToolStripMenuItem();
      this.toolStripSeparator6 = new ToolStripSeparator();
      this.toolStripMenuItem9 = new ToolStripMenuItem();
      this.tabControl1 = new TabControl();
      this.tabPage1 = new TabPage();
      this.tabPage2 = new TabPage();
      this.tabPage3 = new TabPage();
      this.label1 = new Label();
      this.button3 = new Button();
      this.contextMenuStrip1.SuspendLayout();
      this.contextMenuStrip2.SuspendLayout();
      this.contextMenuStrip3.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.SuspendLayout();
      this.button1.DialogResult = DialogResult.Cancel;
      this.button1.Location = new Point(12, 12);
      this.button1.Name = "button1";
      this.button1.Size = new Size(18, 8);
      this.button1.TabIndex = 0;
      this.button1.TabStop = false;
      this.button1.Text = "button1";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listView1.BorderStyle = BorderStyle.FixedSingle;
      this.listView1.ContextMenuStrip = this.contextMenuStrip1;
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.HideSelection = false;
      this.listView1.Location = new Point(6, 6);
      this.listView1.Name = "listView1";
      this.listView1.ShowItemToolTips = true;
      this.listView1.Size = new Size(630, 192);
      this.listView1.TabIndex = 6;
      this.toolTip1.SetToolTip((Control) this.listView1, "おまじない");
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.listView1.VirtualMode = true;
      this.listView1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
      this.listView1.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.listView1_RetrieveVirtualItem);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.show_SToolStripMenuItem,
        (ToolStripItem) this.hide_ToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.add_AToolStripMenuItem,
        (ToolStripItem) this.delete_DToolStripMenuItem,
        (ToolStripItem) this.move_trash_RToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.all_select_AToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(253, 148);
      this.contextMenuStrip1.Opening += new CancelEventHandler(this.contextMenuStrip1_Opening);
      this.show_SToolStripMenuItem.Name = "show_SToolStripMenuItem";
      this.show_SToolStripMenuItem.Size = new Size(252, 22);
      this.show_SToolStripMenuItem.Text = "表示にする(&S)";
      this.show_SToolStripMenuItem.Click += new EventHandler(this.show_hide_SToolStripMenuItem_Click);
      this.hide_ToolStripMenuItem.Name = "hide_ToolStripMenuItem";
      this.hide_ToolStripMenuItem.Size = new Size(252, 22);
      this.hide_ToolStripMenuItem.Text = "非表示にする(&H)";
      this.hide_ToolStripMenuItem.Click += new EventHandler(this.hide_ToolStripMenuItem_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(249, 6);
      this.add_AToolStripMenuItem.Name = "add_AToolStripMenuItem";
      this.add_AToolStripMenuItem.Size = new Size(252, 22);
      this.add_AToolStripMenuItem.Text = "お気に入り航路図一覧に移動(&M)";
      this.add_AToolStripMenuItem.Click += new EventHandler(this.add_AToolStripMenuItem_Click);
      this.delete_DToolStripMenuItem.Name = "delete_DToolStripMenuItem";
      this.delete_DToolStripMenuItem.Size = new Size(252, 22);
      this.delete_DToolStripMenuItem.Text = "過去の航路図一覧に移動(&D)";
      this.delete_DToolStripMenuItem.Click += new EventHandler(this.delete_DToolStripMenuItem_Click);
      this.move_trash_RToolStripMenuItem.Name = "move_trash_RToolStripMenuItem";
      this.move_trash_RToolStripMenuItem.Size = new Size(252, 22);
      this.move_trash_RToolStripMenuItem.Text = "削除(&R)";
      this.move_trash_RToolStripMenuItem.Click += new EventHandler(this.move_trash_RToolStripMenuItem_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(249, 6);
      this.all_select_AToolStripMenuItem.Name = "all_select_AToolStripMenuItem";
      this.all_select_AToolStripMenuItem.Size = new Size(252, 22);
      this.all_select_AToolStripMenuItem.Text = "全て選択(&A)";
      this.all_select_AToolStripMenuItem.Click += new EventHandler(this.all_select_AToolStripMenuItem_Click);
      this.listView2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listView2.BorderStyle = BorderStyle.FixedSingle;
      this.listView2.ContextMenuStrip = this.contextMenuStrip2;
      this.listView2.FullRowSelect = true;
      this.listView2.GridLines = true;
      this.listView2.HideSelection = false;
      this.listView2.Location = new Point(6, 6);
      this.listView2.Name = "listView2";
      this.listView2.ShowItemToolTips = true;
      this.listView2.Size = new Size(630, 192);
      this.listView2.TabIndex = 7;
      this.toolTip1.SetToolTip((Control) this.listView2, "おまじない");
      this.listView2.UseCompatibleStateImageBehavior = false;
      this.listView2.View = View.Details;
      this.listView2.VirtualMode = true;
      this.listView2.SelectedIndexChanged += new EventHandler(this.listView2_SelectedIndexChanged);
      this.listView2.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.listView2_RetrieveVirtualItem);
      this.contextMenuStrip2.Items.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.toolStripMenuItem7,
        (ToolStripItem) this.toolStripMenuItem2,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.toolStripMenuItem3,
        (ToolStripItem) this.toolStripMenuItem4,
        (ToolStripItem) this.toolStripSeparator4,
        (ToolStripItem) this.toolStripMenuItem5
      });
      this.contextMenuStrip2.Name = "contextMenuStrip1";
      this.contextMenuStrip2.Size = new Size(228, 126);
      this.contextMenuStrip2.Opening += new CancelEventHandler(this.contextMenuStrip2_Opening);
      this.toolStripMenuItem7.Name = "toolStripMenuItem7";
      this.toolStripMenuItem7.Size = new Size(227, 22);
      this.toolStripMenuItem7.Text = "表示にする(&S)";
      this.toolStripMenuItem7.Click += new EventHandler(this.toolStripMenuItem7_Click);
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new Size(227, 22);
      this.toolStripMenuItem2.Text = "非表示にする(&H)";
      this.toolStripMenuItem2.Click += new EventHandler(this.toolStripMenuItem2_Click);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new Size(224, 6);
      this.toolStripMenuItem3.Name = "toolStripMenuItem3";
      this.toolStripMenuItem3.Size = new Size(227, 22);
      this.toolStripMenuItem3.Text = "過去の航路図一覧に移動(&D)";
      this.toolStripMenuItem3.Click += new EventHandler(this.toolStripMenuItem3_Click);
      this.toolStripMenuItem4.Name = "toolStripMenuItem4";
      this.toolStripMenuItem4.Size = new Size(227, 22);
      this.toolStripMenuItem4.Text = "削除(&R)";
      this.toolStripMenuItem4.Click += new EventHandler(this.toolStripMenuItem4_Click);
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new Size(224, 6);
      this.toolStripMenuItem5.Name = "toolStripMenuItem5";
      this.toolStripMenuItem5.Size = new Size(227, 22);
      this.toolStripMenuItem5.Text = "全て選択(&A)";
      this.toolStripMenuItem5.Click += new EventHandler(this.toolStripMenuItem5_Click);
      this.listView3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listView3.BorderStyle = BorderStyle.FixedSingle;
      this.listView3.ContextMenuStrip = this.contextMenuStrip3;
      this.listView3.FullRowSelect = true;
      this.listView3.GridLines = true;
      this.listView3.HideSelection = false;
      this.listView3.Location = new Point(6, 6);
      this.listView3.Name = "listView3";
      this.listView3.ShowItemToolTips = true;
      this.listView3.Size = new Size(630, 192);
      this.listView3.TabIndex = 8;
      this.toolTip1.SetToolTip((Control) this.listView3, "おまじない");
      this.listView3.UseCompatibleStateImageBehavior = false;
      this.listView3.View = View.Details;
      this.listView3.VirtualMode = true;
      this.listView3.SelectedIndexChanged += new EventHandler(this.listView3_SelectedIndexChanged);
      this.listView3.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.listView3_RetrieveVirtualItem);
      this.contextMenuStrip3.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.toolStripMenuItem6,
        (ToolStripItem) this.toolStripMenuItem8,
        (ToolStripItem) this.toolStripSeparator6,
        (ToolStripItem) this.toolStripMenuItem9
      });
      this.contextMenuStrip3.Name = "contextMenuStrip1";
      this.contextMenuStrip3.Size = new Size(253, 98);
      this.contextMenuStrip3.Opening += new CancelEventHandler(this.contextMenuStrip3_Opening);
      this.toolStripMenuItem6.Name = "toolStripMenuItem6";
      this.toolStripMenuItem6.Size = new Size(252, 22);
      this.toolStripMenuItem6.Text = "お気に入り航路図一覧に移動(&M)";
      this.toolStripMenuItem6.Click += new EventHandler(this.toolStripMenuItem6_Click);
      this.toolStripMenuItem8.Name = "toolStripMenuItem8";
      this.toolStripMenuItem8.Size = new Size(252, 22);
      this.toolStripMenuItem8.Text = "削除(&R)";
      this.toolStripMenuItem8.Click += new EventHandler(this.toolStripMenuItem8_Click);
      this.toolStripSeparator6.Name = "toolStripSeparator6";
      this.toolStripSeparator6.Size = new Size(249, 6);
      this.toolStripMenuItem9.Name = "toolStripMenuItem9";
      this.toolStripMenuItem9.Size = new Size(252, 22);
      this.toolStripMenuItem9.Text = "全て選択(&A)";
      this.toolStripMenuItem9.Click += new EventHandler(this.toolStripMenuItem9_Click);
      this.tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControl1.Controls.Add((Control) this.tabPage1);
      this.tabControl1.Controls.Add((Control) this.tabPage2);
      this.tabControl1.Controls.Add((Control) this.tabPage3);
      this.tabControl1.Location = new Point(12, 12);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.ShowToolTips = true;
      this.tabControl1.Size = new Size(650, 230);
      this.tabControl1.TabIndex = 1;
      this.tabPage1.Controls.Add((Control) this.listView1);
      this.tabPage1.Location = new Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(642, 204);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "航路図";
      this.tabPage1.ToolTipText = "通常の航路図一覧です。設定項目の保持数設定を超えると古い航路図から自動で過去の航路図に移動されます。";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.tabPage2.BackColor = Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.tabPage2.Controls.Add((Control) this.listView2);
      this.tabPage2.Location = new Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new Padding(3);
      this.tabPage2.Size = new Size(642, 204);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "お気に入り航路図";
      this.tabPage2.ToolTipText = "お気に入りの航路図を登録します。自動では削除されません。";
      this.tabPage3.BackColor = Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.tabPage3.Controls.Add((Control) this.listView3);
      this.tabPage3.Location = new Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Padding = new Padding(3);
      this.tabPage3.Size = new Size(642, 204);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "ごみ箱";
      this.tabPage3.ToolTipText = "過去の航路図は一覧表示のみで描画されません。描画されないため、CPU負荷が軽くたくさんの航路図を保持しておけます。";
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 253);
      this.label1.Name = "label1";
      this.label1.Size = new Size(180, 12);
      this.label1.TabIndex = 3;
      this.label1.Text = "右クリックメニュ\x30FCで削除したりできます";
      this.button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button3.Location = new Point(491, 248);
      this.button3.Name = "button3";
      this.button3.Size = new Size(161, 23);
      this.button3.TabIndex = 7;
      this.button3.Text = "選択状態を解除";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new EventHandler(this.button3_Click);
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.button1;
      this.ClientSize = new Size(674, 283);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.button1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "sea_routes_form2";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.Manual;
      this.Text = "航路図一覧";
      this.Shown += new EventHandler(this.sea_routes_form2_Shown);
      this.FormClosing += new FormClosingEventHandler(this.sea_routes_form2_FormClosing);
      this.contextMenuStrip1.ResumeLayout(false);
      this.contextMenuStrip2.ResumeLayout(false);
      this.contextMenuStrip3.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.tabPage3.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void init_page1()
    {
      this.listView1.Columns.Add("表示", 50);
      this.listView1.Columns.Add("開始位置", 160);
      this.listView1.Columns.Add("終了位置", 160);
      this.listView1.Columns.Add("航海日数", 60);
      this.listView1.Columns.Add("航海日時", 170);
    }

    private void init_page2()
    {
      this.listView2.Columns.Add("表示", 50);
      this.listView2.Columns.Add("開始位置", 160);
      this.listView2.Columns.Add("終了位置", 160);
      this.listView2.Columns.Add("航海日数", 60);
      this.listView2.Columns.Add("航海日時", 170);
    }

    private void init_page3()
    {
      this.listView3.Columns.Add("表示", 50);
      this.listView3.Columns.Add("開始位置", 160);
      this.listView3.Columns.Add("終了位置", 160);
      this.listView3.Columns.Add("航海日数", 60);
      this.listView3.Columns.Add("航海日時", 170);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.Hide();
    }

    private void sea_routes_form2_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason != CloseReason.UserClosing)
        return;
      e.Cancel = true;
      this.Hide();
      this.m_db.SeaRoute.ResetSelectFlag();
    }

    private void sea_routes_form2_Shown(object sender, EventArgs e)
    {
      this.UpdateAllList();
    }

    public void UpdateAllList()
    {
      this.update_sea_routes_list();
      this.update_favorite_sea_routes_list();
      this.update_trash_sea_routes_list();
    }

    public void RedrawNewestSeaRoutes()
    {
      switch (this.tabControl1.SelectedIndex)
      {
        case 0:
          int count = this.m_view1.view.Items.Count;
          if (count <= 0)
            break;
          this.m_view1.view.Invalidate(this.m_view1.view.GetItemRect(count - 1));
          this.m_view1.view.Update();
          break;
      }
    }

    private void update_sea_routes_list()
    {
      this.tabPage1.Text = string.Format("航路図({0})", (object) this.m_db.SeaRoute.searoutes.Count);
      this.update_list_count(this.m_view1);
    }

    private void update_favorite_sea_routes_list()
    {
      this.tabPage2.Text = string.Format("お気に入り航路({0})", (object) this.m_db.SeaRoute.favorite_sea_routes.Count);
      this.update_list_count(this.m_view2);
    }

    private void update_trash_sea_routes_list()
    {
      this.tabPage3.Text = string.Format("過去の航路図(非表示)({0})", (object) this.m_db.SeaRoute.trash_sea_routes.Count);
      this.update_list_count(this.m_view3);
    }

    private void update_list_count(sea_routes_form2.list_view_db view)
    {
      ListViewItem topItem = view.view.TopItem;
      if (topItem != null)
      {
        int index = topItem.Index;
      }
      view.view.VirtualListSize = 0;
      view.view.VirtualListSize = view.db.Count;
      if (view.view.Items.Count <= 0)
        return;
      view.view.EnsureVisible(view.view.Items.Count - 1);
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selected_index_changed(this.m_view1);
    }

    private void listView2_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selected_index_changed(this.m_view2);
    }

    private void listView3_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selected_index_changed(this.m_view3);
    }

    private void selected_index_changed(sea_routes_form2.list_view_db view)
    {
      if (this.m_disable_update_select)
        return;
      this.m_db.SeaRoute.ResetSelectFlag();
      if (view.view.SelectedIndices.Count < 1)
        return;
      SeaRoutes.Voyage route1 = this.get_route(view.db, view.view.SelectedIndices[0]);
      if (route1 != null)
      {
        if (route1.GamePoint1st.X < 0 || route1.GamePoint1st.Y < 0)
          return;
        this.m_lib.setting.centering_gpos = route1.GamePoint1st;
        this.m_lib.setting.req_centering_gpos.Request();
      }
      foreach (int index in view.view.SelectedIndices)
      {
        SeaRoutes.Voyage route2 = this.get_route(view.db, index);
        if (route2 != null)
          route2.IsSelected = true;
      }
    }

    private SeaRoutes.Voyage get_route(List<SeaRoutes.Voyage> list, int index)
    {
      if (list == null)
        return (SeaRoutes.Voyage) null;
      if (index < 0)
        return (SeaRoutes.Voyage) null;
      if (index >= list.Count)
        return (SeaRoutes.Voyage) null;
      else
        return list[index];
    }

    private void show_hide_SToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.set_draw_flag(this.m_view1, true);
      this.update_sea_routes_list();
    }

    private void toolStripMenuItem7_Click(object sender, EventArgs e)
    {
      this.set_draw_flag(this.m_view2, true);
      this.update_favorite_sea_routes_list();
    }

    private void hide_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.set_draw_flag(this.m_view1, false);
      this.update_sea_routes_list();
    }

    private void toolStripMenuItem2_Click(object sender, EventArgs e)
    {
      this.set_draw_flag(this.m_view2, false);
      this.update_favorite_sea_routes_list();
    }

    private void set_draw_flag(sea_routes_form2.list_view_db view, bool is_show)
    {
      this.m_db.SeaRoute.ResetSelectFlag();
      if (view.view.SelectedIndices.Count < 1)
        return;
      this.m_disable_update_select = true;
      foreach (int index in view.view.SelectedIndices)
      {
        SeaRoutes.Voyage route = this.get_route(view.db, index);
        if (route != null)
          route.IsEnableDraw = is_show;
      }
      this.m_disable_update_select = false;
    }

    private void all_select_AToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.select_all(this.m_view1);
    }

    private void toolStripMenuItem5_Click(object sender, EventArgs e)
    {
      this.select_all(this.m_view2);
    }

    private void toolStripMenuItem9_Click(object sender, EventArgs e)
    {
      this.select_all(this.m_view3);
    }

    private void select_all(sea_routes_form2.list_view_db view)
    {
      this.m_disable_update_select = true;
      for (int itemIndex = 0; itemIndex < view.db.Count; ++itemIndex)
        view.view.SelectedIndices.Add(itemIndex);
      this.m_disable_update_select = false;
      this.selected_index_changed(view);
    }

    private void button3_Click(object sender, EventArgs e)
    {
      this.m_db.SeaRoute.ResetSelectFlag();
      this.m_disable_update_select = true;
      this.listView1.SelectedIndices.Clear();
      this.listView2.SelectedIndices.Clear();
      this.listView3.SelectedIndices.Clear();
      this.m_disable_update_select = false;
    }

    private void move_trash_RToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<SeaRoutes.Voyage> selectedRoutesList = this.get_selected_routes_list(this.m_view1);
      if (selectedRoutesList == null || !this.ask_remove())
        return;
      this.m_db.SeaRoute.RemoveSeaRoutes(selectedRoutesList);
      this.m_db.SeaRoute.ResetSelectFlag();
      this.update_sea_routes_list();
    }

    private void toolStripMenuItem4_Click(object sender, EventArgs e)
    {
      List<SeaRoutes.Voyage> selectedRoutesList = this.get_selected_routes_list(this.m_view2);
      if (selectedRoutesList == null || !this.ask_remove())
        return;
      this.m_db.SeaRoute.RemoveFavoriteSeaRoutes(selectedRoutesList);
      this.m_db.SeaRoute.ResetSelectFlag();
      this.update_favorite_sea_routes_list();
    }

    private void toolStripMenuItem8_Click(object sender, EventArgs e)
    {
      List<SeaRoutes.Voyage> selectedRoutesList = this.get_selected_routes_list(this.m_view3);
      if (selectedRoutesList == null || !this.ask_remove())
        return;
      this.m_db.SeaRoute.RemoveTrashSeaRoutes(selectedRoutesList);
      this.m_db.SeaRoute.ResetSelectFlag();
      this.update_trash_sea_routes_list();
    }

    private bool ask_remove()
    {
      return MessageBox.Show("選択された航路図を削除します。\n削除すると元に戻せません。\nよろしいですか？", "航路図削除の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.No;
    }

    private List<SeaRoutes.Voyage> get_selected_routes_list(sea_routes_form2.list_view_db view)
    {
      if (view.view.SelectedIndices.Count < 1)
        return (List<SeaRoutes.Voyage>) null;
      List<SeaRoutes.Voyage> list = new List<SeaRoutes.Voyage>();
      foreach (int index in view.view.SelectedIndices)
      {
        SeaRoutes.Voyage route = this.get_route(view.db, index);
        if (route != null)
          list.Add(route);
      }
      if (list.Count <= 0)
        return (List<SeaRoutes.Voyage>) null;
      else
        return list;
    }

    private void add_AToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<SeaRoutes.Voyage> selectedRoutesList = this.get_selected_routes_list(this.m_view1);
      if (selectedRoutesList == null)
        return;
      this.m_db.SeaRoute.MoveSeaRoutesToFavoriteSeaRoutes(selectedRoutesList);
      this.m_db.SeaRoute.ResetSelectFlag();
      this.UpdateAllList();
    }

    private void delete_DToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<SeaRoutes.Voyage> selectedRoutesList = this.get_selected_routes_list(this.m_view1);
      if (selectedRoutesList == null)
        return;
      this.m_db.SeaRoute.MoveSeaRoutesToTrashSeaRoutes(selectedRoutesList);
      this.m_db.SeaRoute.ResetSelectFlag();
      this.UpdateAllList();
    }

    private void toolStripMenuItem3_Click(object sender, EventArgs e)
    {
      List<SeaRoutes.Voyage> selectedRoutesList = this.get_selected_routes_list(this.m_view2);
      if (selectedRoutesList == null)
        return;
      this.m_db.SeaRoute.MoveFavoriteSeaRoutesToTrashSeaRoutes(selectedRoutesList);
      this.m_db.SeaRoute.ResetSelectFlag();
      this.UpdateAllList();
    }

    private void toolStripMenuItem6_Click(object sender, EventArgs e)
    {
      List<SeaRoutes.Voyage> selectedRoutesList = this.get_selected_routes_list(this.m_view3);
      if (selectedRoutesList == null)
        return;
      this.m_db.SeaRoute.MoveTrashSeaRoutesToFavoriteSeaRoutes(selectedRoutesList);
      this.m_db.SeaRoute.ResetSelectFlag();
      this.UpdateAllList();
    }

    private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
      this.set_item(this.m_view1, e);
    }

    private void listView2_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
      this.set_item(this.m_view2, e);
    }

    private void listView3_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
      this.set_item(this.m_view3, e);
    }

    private void set_item(sea_routes_form2.list_view_db view, RetrieveVirtualItemEventArgs e)
    {
      SeaRoutes.Voyage route = this.get_route(view.db, e.ItemIndex);
      if (route == null)
        e.Item = new ListViewItem("---", 0)
        {
          SubItems = {
            "---",
            "---",
            "---",
            "---"
          }
        };
      else
        e.Item = this.create_item(route, view != this.m_view3);
    }

    private ListViewItem create_item(SeaRoutes.Voyage i, bool is_draw_show_flag)
    {
      GvoWorldInfo.Info infoWithoutSea1 = this.m_db.World.FindInfo_WithoutSea(transform.ToPoint(i.MapPoint1st));
      string str1 = infoWithoutSea1 != null ? infoWithoutSea1.Name : "";
      GvoWorldInfo.Info infoWithoutSea2 = this.m_db.World.FindInfo_WithoutSea(transform.ToPoint(i.MapPointLast));
      string str2 = infoWithoutSea2 != null ? infoWithoutSea2.Name : "";
      string text = i.IsEnableDraw ? "表示" : "非表示";
      if (!is_draw_show_flag)
        text = "---";
      ListViewItem listViewItem = new ListViewItem(text, 0);
      listViewItem.UseItemStyleForSubItems = false;
      listViewItem.Tag = (object) i;
      listViewItem.SubItems.Add(str1 + "(" + i.GamePoint1stStr + ")");
      listViewItem.SubItems.Add(str2 + "(" + i.GamePointLastString + ")");
      listViewItem.SubItems.Add(i.MaxDaysString);
      listViewItem.SubItems.Add(i.DateTimeString);
      if (is_draw_show_flag)
        listViewItem.SubItems[0].ForeColor = i.IsEnableDraw ? Color.Blue : Color.Red;
      return listViewItem;
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      if (this.set_context_menu_state(this.contextMenuStrip1, this.listView1, 7))
        return;
      e.Cancel = true;
    }

    private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
    {
      if (this.set_context_menu_state(this.contextMenuStrip2, this.listView2, 6))
        return;
      e.Cancel = true;
    }

    private void contextMenuStrip3_Opening(object sender, CancelEventArgs e)
    {
      if (this.set_context_menu_state(this.contextMenuStrip3, this.listView3, 3))
        return;
      e.Cancel = true;
    }

    private bool set_context_menu_state(ContextMenuStrip menu, ListView list_view, int all_select_index)
    {
      if (list_view.Items.Count <= 0)
        return false;
      if (list_view.SelectedIndices.Count <= 0)
      {
        foreach (ToolStripItem toolStripItem in (ArrangedElementCollection) menu.Items)
          toolStripItem.Enabled = false;
      }
      else
      {
        foreach (ToolStripItem toolStripItem in (ArrangedElementCollection) menu.Items)
          toolStripItem.Enabled = true;
      }
      if (list_view.Items.Count > 0 && menu.Items.Count > all_select_index)
        menu.Items[all_select_index].Enabled = true;
      return true;
    }

    private class list_view_db
    {
      private ListView m_list_view;
      private List<SeaRoutes.Voyage> m_db_list;

      public ListView view
      {
        get
        {
          return this.m_list_view;
        }
      }

      public List<SeaRoutes.Voyage> db
      {
        get
        {
          return this.m_db_list;
        }
      }

      public list_view_db(ListView view, List<SeaRoutes.Voyage> db)
      {
        this.m_list_view = view;
        this.m_db_list = db;
      }
    }
  }
}
