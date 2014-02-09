// Type: Utility.KeyAssign.KeyAssignForm
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Utility.KeyAssign
{
  public class KeyAssignForm : Form
  {
    private KeyAssignList.Assign m_assign;
    private Keys m_new_assign;
    private IContainer components;
    private Label label1;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
    private Button button1;
    private Button button2;
    private TextBox textBox1;
    private Label label2;
    private Label label3;

    public Keys NewAssignKey
    {
      get
      {
        return this.m_new_assign;
      }
    }

    public KeyAssignForm(KeyAssignList.Assign assign)
    {
      this.InitializeComponent();
      this.m_assign = assign;
      this.label4.Text = assign.Name;
      this.label3.Text = assign.Group;
      this.label1.Text = assign.KeysString;
      this.m_new_assign = Keys.None;
    }

    private void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
      e.SuppressKeyPress = true;
      this.textBox1.Text = this.m_assign.GetKeysString(e.KeyData);
      this.m_new_assign = this.m_assign.CanAssignKeys(e.KeyData) ? e.KeyData : Keys.None;
    }

    private void key_assign_form_Shown(object sender, EventArgs e)
    {
      this.textBox1.Focus();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.button1 = new Button();
      this.button2 = new Button();
      this.textBox1 = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(101, 52);
      this.label1.Name = "label1";
      this.label1.Size = new Size(35, 12);
      this.label1.TabIndex = 0;
      this.label1.Text = "label1";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(101, 30);
      this.label4.Name = "label4";
      this.label4.Size = new Size(35, 12);
      this.label4.TabIndex = 3;
      this.label4.Text = "label4";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 30);
      this.label5.Name = "label5";
      this.label5.Size = new Size(31, 12);
      this.label5.TabIndex = 4;
      this.label5.Text = "機能:";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(12, 52);
      this.label6.Name = "label6";
      this.label6.Size = new Size(82, 12);
      this.label6.TabIndex = 5;
      this.label6.Text = "現在の割り当て:";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(12, 74);
      this.label7.Name = "label7";
      this.label7.Size = new Size(79, 12);
      this.label7.TabIndex = 6;
      this.label7.Text = "新しい割り当て:";
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button1.DialogResult = DialogResult.OK;
      this.button1.Location = new Point(122, 101);
      this.button1.Name = "button1";
      this.button1.Size = new Size(110, 27);
      this.button1.TabIndex = 1;
      this.button1.TabStop = false;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(238, 100);
      this.button2.Name = "button2";
      this.button2.Size = new Size(110, 27);
      this.button2.TabIndex = 2;
      this.button2.TabStop = false;
      this.button2.Text = "キャンセル";
      this.button2.UseVisualStyleBackColor = true;
      this.textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.textBox1.ImeMode = ImeMode.Disable;
      this.textBox1.Location = new Point(103, 71);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(245, 19);
      this.textBox1.TabIndex = 0;
      this.textBox1.TabStop = false;
      this.textBox1.Text = "なし(割り当てたいキ\x30FCを押してください)";
      this.textBox1.KeyDown += new KeyEventHandler(this.textBox1_KeyDown);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(45, 12);
      this.label2.TabIndex = 8;
      this.label2.Text = "グル\x30FCプ:";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(101, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(35, 12);
      this.label3.TabIndex = 7;
      this.label3.Text = "label3";
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(360, 139);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.textBox1);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "KeyAssignForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "キ\x30FCの割り当て";
      this.Shown += new EventHandler(this.key_assign_form_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
