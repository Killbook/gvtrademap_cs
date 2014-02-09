// Type: gvtrademap_cs.setting_sea_area_form
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Utility;

namespace gvtrademap_cs
{
  public class setting_sea_area_form : Form
  {
    private const string TEXT0 = "危険海域(通常状態)";
    private const string TEXT1 = "安全海域";
    private const string TEXT2 = "無法海域";
    private IContainer components;
    private Button button2;
    private Button button1;
    private Button button3;
    private ListView listView1;
    private ComboBox comboBox1;
    private ToolTip toolTip1;
    private Button button4;
    private sea_area m_sea_area;
    private ListViewItem m_li;
    private int m_X;
    private int m_Y;

    public setting_sea_area_form(sea_area area)
    {
      this.m_sea_area = area;
      this.InitializeComponent();
      Useful.SetFontMeiryo((Form) this, 8f);
      this.toolTip1.AutoPopDelay = 30000;
      this.toolTip1.BackColor = Color.LightYellow;
      this.comboBox1.Hide();
      this.listView1.Columns.Add("海域群", 200);
      this.listView1.Columns.Add("状態", 150);
      foreach (sea_area.sea_area_once seaAreaOnce in this.m_sea_area.groups)
      {
        string type = seaAreaOnce.type != sea_area.sea_area_once.sea_type.normal ? (seaAreaOnce.type != sea_area.sea_area_once.sea_type.safty ? "無法海域" : "安全海域") : "危険海域(通常状態)";
        this.add_item(seaAreaOnce.name, type, seaAreaOnce.GetToolTipString());
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
      this.components = (IContainer) new Container();
      this.button2 = new Button();
      this.button1 = new Button();
      this.button3 = new Button();
      this.listView1 = new ListView();
      this.comboBox1 = new ComboBox();
      this.toolTip1 = new ToolTip(this.components);
      this.button4 = new Button();
      this.SuspendLayout();
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(329, 340);
      this.button2.Name = "button2";
      this.button2.Size = new Size(83, 23);
      this.button2.TabIndex = 11;
      this.button2.Text = "キャンセル";
      this.button2.UseVisualStyleBackColor = true;
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button1.DialogResult = DialogResult.OK;
      this.button1.Location = new Point(240, 340);
      this.button1.Name = "button1";
      this.button1.Size = new Size(83, 23);
      this.button1.TabIndex = 10;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button3.Location = new Point(12, 311);
      this.button3.Name = "button3";
      this.button3.Size = new Size(212, 23);
      this.button3.TabIndex = 12;
      this.button3.Text = "全ての海域群を通常状態に戻す";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new EventHandler(this.button3_Click);
      this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listView1.BorderStyle = BorderStyle.FixedSingle;
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.listView1.HideSelection = false;
      this.listView1.Location = new Point(12, 12);
      this.listView1.Name = "listView1";
      this.listView1.ShowItemToolTips = true;
      this.listView1.Size = new Size(400, 293);
      this.listView1.TabIndex = 13;
      this.toolTip1.SetToolTip((Control) this.listView1, "おまじない");
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.listView1.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.listView1_ColumnWidthChanged);
      this.listView1.MouseDown += new MouseEventHandler(this.listView1_MouseDown);
      this.listView1.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
      this.listView1.ColumnWidthChanging += new ColumnWidthChangingEventHandler(this.listView1_ColumnWidthChanging);
      this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox1.FlatStyle = FlatStyle.Popup;
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Items.AddRange(new object[3]
      {
        (object) "危険海域(通常状態)",
        (object) "安全海域",
        (object) "無法海域"
      });
      this.comboBox1.Location = new Point(122, 77);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new Size(122, 20);
      this.comboBox1.TabIndex = 14;
      this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
      this.toolTip1.AutoPopDelay = 50000;
      this.toolTip1.InitialDelay = 500;
      this.toolTip1.ReshowDelay = 100;
      this.button4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button4.Location = new Point(12, 340);
      this.button4.Name = "button4";
      this.button4.Size = new Size(212, 23);
      this.button4.TabIndex = 15;
      this.button4.Text = "海域情報収集を起動する";
      this.toolTip1.SetToolTip((Control) this.button4, "海域変動収集を起動します。\r\n海域変動収集は\r\n・したらば掲示板の安全海域情報スレ\r\n・大航海時代オンラインまとめwiki(統括)\r\nから海域変動情報を取得します。");
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new EventHandler(this.button4_Click);
      this.AcceptButton = (IButtonControl) this.button1;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.button2;
      this.ClientSize = new Size(424, 375);
      this.Controls.Add((Control) this.button4);
      this.Controls.Add((Control) this.comboBox1);
      this.Controls.Add((Control) this.listView1);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new Size(430, 328);
      this.Name = "setting_sea_area_form";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "危険海域変動システムの設定";
      this.FormClosed += new FormClosedEventHandler(this.setting_sea_area_form_FormClosed);
      this.ResumeLayout(false);
    }

    private void add_item(string str, string type, string tooltipstring)
    {
      this.listView1.Items.Add(new ListViewItem(str, 0)
      {
        SubItems = {
          type
        },
        ToolTipText = tooltipstring
      });
    }

    private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
    {
      this.ajust_combobox_size();
      if (!e.IsSelected)
      {
        this.comboBox1.Hide();
      }
      else
      {
        this.comboBox1.Location = new Point(2 + this.listView1.Columns[0].Width + this.listView1.Location.X, this.m_li.Position.Y + this.listView1.Location.Y);
        this.comboBox1.Size = new Size(this.listView1.Columns[1].Width, this.m_li.Bounds.Height);
        this.comboBox1.Text = this.m_li.SubItems[1].Text;
        this.comboBox1.Show();
      }
    }

    private void ajust_combobox_size()
    {
      if (this.listView1.SelectedItems.Count <= 0)
      {
        this.comboBox1.Hide();
      }
      else
      {
        this.comboBox1.Location = new Point(2 + this.listView1.Columns[0].Width + this.listView1.Location.X, this.m_li.Position.Y + this.listView1.Location.Y);
        this.comboBox1.Size = new Size(this.listView1.Columns[1].Width, this.m_li.Bounds.Height);
      }
    }

    private void listView1_MouseDown(object sender, MouseEventArgs e)
    {
      this.m_li = this.listView1.GetItemAt(e.X, e.Y);
      this.m_X = e.X;
      this.m_Y = e.Y;
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.m_li.SubItems[1].Text = this.comboBox1.Text;
    }

    private void setting_sea_area_form_FormClosed(object sender, FormClosedEventArgs e)
    {
    }

    public void Update(sea_area area)
    {
      foreach (ListViewItem listViewItem in this.listView1.Items)
      {
        sea_area.sea_area_once.sea_type type = !(listViewItem.SubItems[1].Text == "危険海域(通常状態)") ? (!(listViewItem.SubItems[1].Text == "安全海域") ? sea_area.sea_area_once.sea_type.lawless : sea_area.sea_area_once.sea_type.safty) : sea_area.sea_area_once.sea_type.normal;
        this.m_sea_area.SetType(listViewItem.SubItems[0].Text, type);
      }
    }

    private void button3_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.listView1.Items)
        listViewItem.SubItems[1].Text = "危険海域(通常状態)";
      if (!this.comboBox1.Visible)
        return;
      this.comboBox1.Text = "危険海域(通常状態)";
    }

    private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
    {
      this.ajust_combobox_size();
    }

    private void listView1_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
    {
      this.ajust_combobox_size();
    }

    private void button4_Click(object sender, EventArgs e)
    {
      try
      {
        Process.Start("gvoac.exe");
        this.button2.PerformClick();
      }
      catch
      {
        int num = (int) MessageBox.Show("海域情報収集の起動に失敗しました。", "起動エラ\x30FC", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }
  }
}
