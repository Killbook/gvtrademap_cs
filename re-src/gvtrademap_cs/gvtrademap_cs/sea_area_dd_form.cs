// Type: gvtrademap_cs.sea_area_dd_form
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Utility;

namespace gvtrademap_cs
{
  public class sea_area_dd_form : Form
  {
    private gvt_lib m_lib;
    private List<sea_area_once_from_dd> m_list;
    private GvoDatabase m_db;
    private List<sea_area_once_from_dd> m_filterd_list;
    private IContainer components;
    private ListView listView1;
    private Button button2;
    private Button button1;
    private CheckBox checkBox1;
    private CheckBox checkBox2;

    public List<sea_area_once_from_dd> filterd_list
    {
      get
      {
        return this.m_filterd_list;
      }
    }

    public sea_area_dd_form(gvt_lib lib, GvoDatabase db, List<sea_area_once_from_dd> list)
    {
      this.m_lib = lib;
      this.m_list = list;
      this.m_db = db;
      this.m_filterd_list = new List<sea_area_once_from_dd>();
      this.InitializeComponent();
      Useful.SetFontMeiryo((Form) this, 8f);
      this.listView1.Columns.Add("サ\x30FCバ", 80);
      this.listView1.Columns.Add("海域名", 100);
      this.listView1.Columns.Add("状態", 100);
      this.listView1.Columns.Add("終了日時", 180);
      this.listView1.Columns.Add("補足", 100);
      this.checkBox1.Checked = true;
      this.checkBox2.Checked = true;
      this.update_list();
    }

    private void update_list()
    {
      this.listView1.BeginUpdate();
      this.listView1.Items.Clear();
      foreach (sea_area_once_from_dd seaAreaOnceFromDd in this.m_list)
      {
        ListViewItem listViewItem = new ListViewItem(seaAreaOnceFromDd.server_str, 0);
        listViewItem.UseItemStyleForSubItems = false;
        listViewItem.Tag = (object) seaAreaOnceFromDd;
        listViewItem.SubItems.Add(seaAreaOnceFromDd.name);
        listViewItem.SubItems.Add(seaAreaOnceFromDd._sea_type_str);
        listViewItem.SubItems.Add(Useful.TojbbsDateTimeString(seaAreaOnceFromDd.date));
        bool flag = !(seaAreaOnceFromDd.date < DateTime.Now);
        listViewItem.SubItems.Add(flag ? "継続中" : "期限切れ");
        switch (seaAreaOnceFromDd._sea_type)
        {
          case sea_area.sea_area_once.sea_type.safty:
            listViewItem.SubItems[2].ForeColor = Color.Blue;
            break;
          case sea_area.sea_area_once.sea_type.lawless:
            listViewItem.SubItems[2].ForeColor = Color.Red;
            break;
        }
        listViewItem.SubItems[4].ForeColor = flag ? Color.Green : Color.Red;
        if ((!this.checkBox1.Checked || this.m_db.World.MyServer == GvoWorldInfo.GetServerFromString(seaAreaOnceFromDd.server_str)) && (!this.checkBox2.Checked || !(seaAreaOnceFromDd.date < DateTime.Now)))
          this.listView1.Items.Add(listViewItem);
      }
      this.listView1.EndUpdate();
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
      this.update_list();
    }

    private void checkBox2_CheckedChanged(object sender, EventArgs e)
    {
      this.update_list();
    }

    private void sea_area_dd_form_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.m_filterd_list.Clear();
      foreach (ListViewItem listViewItem in this.listView1.Items)
      {
        sea_area_once_from_dd seaAreaOnceFromDd = listViewItem.Tag as sea_area_once_from_dd;
        if (seaAreaOnceFromDd != null)
          this.m_filterd_list.Add(seaAreaOnceFromDd);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.listView1 = new ListView();
      this.button2 = new Button();
      this.button1 = new Button();
      this.checkBox1 = new CheckBox();
      this.checkBox2 = new CheckBox();
      this.SuspendLayout();
      this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.Location = new Point(12, 12);
      this.listView1.Name = "listView1";
      this.listView1.Size = new Size(533, 202);
      this.listView1.TabIndex = 0;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(462, 252);
      this.button2.Name = "button2";
      this.button2.Size = new Size(83, 23);
      this.button2.TabIndex = 11;
      this.button2.Text = "キャンセル";
      this.button2.UseVisualStyleBackColor = true;
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button1.DialogResult = DialogResult.OK;
      this.button1.Location = new Point(373, 252);
      this.button1.Name = "button1";
      this.button1.Size = new Size(83, 23);
      this.button1.TabIndex = 10;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.checkBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new Point(12, 220);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new Size(175, 16);
      this.checkBox1.TabIndex = 12;
      this.checkBox1.Text = "プレイしているサ\x30FCバ以外を無視";
      this.checkBox1.UseVisualStyleBackColor = true;
      this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
      this.checkBox2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.checkBox2.AutoSize = true;
      this.checkBox2.Location = new Point(12, 242);
      this.checkBox2.Name = "checkBox2";
      this.checkBox2.Size = new Size(138, 16);
      this.checkBox2.TabIndex = 13;
      this.checkBox2.Text = "期限切れの情報を無視";
      this.checkBox2.UseVisualStyleBackColor = true;
      this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
      this.AcceptButton = (IButtonControl) this.button1;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.button2;
      this.ClientSize = new Size(557, 287);
      this.Controls.Add((Control) this.checkBox2);
      this.Controls.Add((Control) this.checkBox1);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.listView1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "sea_area_dd_form";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Drag&Dropでの海域情報受け取り";
      this.FormClosed += new FormClosedEventHandler(this.sea_area_dd_form_FormClosed);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
