// Type: Utility.KeyAssign.KeyAssignListForm
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Utility.Ctrl;

namespace Utility.KeyAssign
{
  public class KeyAssignListForm : Form
  {
    private IContainer components;
    private ComboBox comboBox1;
    private Button button1;
    private Button button2;
    private Button button3;
    private ListViewDoubleBufferd listView1;
    private ToolTip toolTip1;
    private Button button4;
    private Button button5;
    private KeyAssiginSettingHelper m_helper;

    public KeyAssignList List
    {
      get
      {
        return this.m_helper.List;
      }
    }

    public KeyAssignListForm(KeyAssignList assign_list)
    {
      this.InitializeComponent();
      this.listView1.EnableSort(true);
      this.m_helper = new KeyAssiginSettingHelper(assign_list, (Form) this, this.comboBox1, (ListView) this.listView1, this.button1, this.button4, this.button5);
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
      this.comboBox1 = new ComboBox();
      this.button1 = new Button();
      this.button2 = new Button();
      this.button3 = new Button();
      this.listView1 = new ListViewDoubleBufferd();
      this.toolTip1 = new ToolTip(this.components);
      this.button4 = new Button();
      this.button5 = new Button();
      this.SuspendLayout();
      this.comboBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new Point(12, 12);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new Size(386, 20);
      this.comboBox1.TabIndex = 1;
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.button1.Location = new Point(12, 272);
      this.button1.Name = "button1";
      this.button1.Size = new Size(117, 27);
      this.button1.TabIndex = 2;
      this.button1.Text = "割り当て";
      this.button1.UseVisualStyleBackColor = true;
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button2.DialogResult = DialogResult.OK;
      this.button2.Location = new Point(158, 305);
      this.button2.Name = "button2";
      this.button2.Size = new Size(117, 27);
      this.button2.TabIndex = 5;
      this.button2.Text = "OK";
      this.button2.UseVisualStyleBackColor = true;
      this.button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button3.DialogResult = DialogResult.Cancel;
      this.button3.Location = new Point(281, 305);
      this.button3.Name = "button3";
      this.button3.Size = new Size(117, 27);
      this.button3.TabIndex = 6;
      this.button3.Text = "キャンセル";
      this.button3.UseVisualStyleBackColor = true;
      this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.HideSelection = false;
      this.listView1.Location = new Point(12, 38);
      this.listView1.MultiSelect = false;
      this.listView1.Name = "listView1";
      this.listView1.ShowItemToolTips = true;
      this.listView1.Size = new Size(386, 228);
      this.listView1.TabIndex = 0;
      this.toolTip1.SetToolTip((Control) this.listView1, "a");
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.button4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.button4.Location = new Point(135, 272);
      this.button4.Name = "button4";
      this.button4.Size = new Size(117, 27);
      this.button4.TabIndex = 3;
      this.button4.Text = "割り当て解除";
      this.button4.UseVisualStyleBackColor = true;
      this.button5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.button5.Location = new Point(258, 272);
      this.button5.Name = "button5";
      this.button5.Size = new Size(117, 27);
      this.button5.TabIndex = 4;
      this.button5.Text = "全て初期値に戻す";
      this.button5.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.button2;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.button3;
      this.ClientSize = new Size(410, 344);
      this.Controls.Add((Control) this.button5);
      this.Controls.Add((Control) this.button4);
      this.Controls.Add((Control) this.listView1);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.comboBox1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "KeyAssignListForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "キ\x30FC割り当て";
      this.ResumeLayout(false);
    }
  }
}
