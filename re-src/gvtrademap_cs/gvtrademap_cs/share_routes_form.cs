// Type: gvtrademap_cs.share_routes_form
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Utility;
using Utility.Ctrl;

namespace gvtrademap_cs
{
  public class share_routes_form : Form
  {
    private Point m_selected_position;
    private ListViewItemSorter m_sorter;
    private IContainer components;
    private Label label2;
    private Button button2;
    private Button button1;
    private ListView listView1;

    public bool is_selected
    {
      get
      {
        return this.m_selected_position.X >= 0 && this.m_selected_position.Y >= 0;
      }
    }

    public Point selected_position
    {
      get
      {
        return this.m_selected_position;
      }
    }

    public share_routes_form(List<ShareRoutes.ShareShip> list)
    {
      this.m_sorter = new ListViewItemSorter();
      this.m_selected_position = new Point(-1, -1);
      this.InitializeComponent();
      Useful.SetFontMeiryo((Form) this, 8f);
      this.label2.Text = string.Format("{0}人", (object) list.Count);
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.Columns.Add("名前", 150);
      this.listView1.Columns.Add("場所", 100);
      this.listView1.Columns.Add("状態", 100);
      foreach (ShareRoutes.ShareShip shareShip in list)
      {
        ListViewItem listViewItem = new ListViewItem(shareShip.Name, 0);
        listViewItem.SubItems.Add(string.Format("{0},{1}", (object) shareShip.Position.X, (object) shareShip.Position.Y));
        if (shareShip.State == ShareRoutes.State.in_the_sea)
          listViewItem.SubItems.Add("航海中");
        else
          listViewItem.SubItems.Add("街の中等");
        this.listView1.Items.Add(listViewItem);
      }
    }

    private void share_routes_form_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (this.listView1.SelectedItems.Count <= 0)
        return;
      string[] strArray = this.listView1.SelectedItems[0].SubItems[1].Text.Split(new char[1]
      {
        ','
      });
      if (strArray.Length != 2)
        return;
      this.m_selected_position = Useful.ToPoint(strArray[0], strArray[1], new Point(-1, -1));
    }

    private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      this.m_sorter.Sort(this.listView1, e.Column);
    }

    private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      this.button1.PerformClick();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label2 = new Label();
      this.button2 = new Button();
      this.button1 = new Button();
      this.listView1 = new ListView();
      this.SuspendLayout();
      this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(346, 11);
      this.label2.Name = "label2";
      this.label2.Size = new Size(35, 12);
      this.label2.TabIndex = 18;
      this.label2.Text = "label2";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(299, 240);
      this.button2.Name = "button2";
      this.button2.Size = new Size(83, 23);
      this.button2.TabIndex = 16;
      this.button2.Text = "キャンセル";
      this.button2.UseVisualStyleBackColor = true;
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button1.DialogResult = DialogResult.OK;
      this.button1.Location = new Point(210, 240);
      this.button1.Name = "button1";
      this.button1.Size = new Size(83, 23);
      this.button1.TabIndex = 15;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listView1.BorderStyle = BorderStyle.FixedSingle;
      this.listView1.HideSelection = false;
      this.listView1.Location = new Point(13, 30);
      this.listView1.Name = "listView1";
      this.listView1.Size = new Size(369, 204);
      this.listView1.TabIndex = 14;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.listView1.MouseDoubleClick += new MouseEventHandler(this.listView1_MouseDoubleClick);
      this.listView1.ColumnClick += new ColumnClickEventHandler(this.listView1_ColumnClick);
      this.AcceptButton = (IButtonControl) this.button1;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.button2;
      this.ClientSize = new Size(394, 275);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.listView1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new Size(410, 312);
      this.Name = "share_routes_form";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "航路共有詳細";
      this.FormClosed += new FormClosedEventHandler(this.share_routes_form_FormClosed);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
