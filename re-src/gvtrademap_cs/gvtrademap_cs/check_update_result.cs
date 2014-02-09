// Type: gvtrademap_cs.check_update_result
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
  public class check_update_result : Form
  {
    private IContainer components;
    private TextBox textBox1;
    private Label label1;
    private Button button1;
    private Button button4;
    private Label label2;

    public check_update_result(string[] data)
    {
      this.InitializeComponent();
      this.textBox1.AcceptsReturn = true;
      this.textBox1.Lines = data;
      this.textBox1.Select(0, 0);
      Useful.SetFontMeiryo((Form) this, 8f);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.textBox1 = new TextBox();
      this.label1 = new Label();
      this.button1 = new Button();
      this.button4 = new Button();
      this.label2 = new Label();
      this.SuspendLayout();
      this.textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBox1.Font = new Font("MS UI Gothic", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) sbyte.MinValue);
      this.textBox1.Location = new Point(12, 42);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.ScrollBars = ScrollBars.Vertical;
      this.textBox1.Size = new Size(498, 251);
      this.textBox1.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(331, 12);
      this.label1.TabIndex = 1;
      this.label1.Text = "更新されたソフトウェアが見つかりました。更新内容は以下の通りです。";
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button1.DialogResult = DialogResult.OK;
      this.button1.Location = new Point(427, 299);
      this.button1.Name = "button1";
      this.button1.Size = new Size(83, 23);
      this.button1.TabIndex = 9;
      this.button1.Text = "閉じる";
      this.button1.UseVisualStyleBackColor = true;
      this.button4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button4.Location = new Point(237, 299);
      this.button4.Name = "button4";
      this.button4.Size = new Size(184, 23);
      this.button4.TabIndex = 12;
      this.button4.Text = "ダウンロ\x30FCドペ\x30FCジを開く";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new EventHandler(this.button4_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 23);
      this.label2.Name = "label2";
      this.label2.Size = new Size(378, 12);
      this.label2.TabIndex = 13;
      this.label2.Text = "自動更新はされません。ダウンロ\x30FCドペ\x30FCジからダウンロ\x30FCドして更新してください。";
      this.AcceptButton = (IButtonControl) this.button1;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(522, 334);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.button4);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBox1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "check_update_result";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "更新確認結果";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void button4_Click(object sender, EventArgs e)
    {
      Process.Start(def.DOWNLOAD_URL);
    }
  }
}
