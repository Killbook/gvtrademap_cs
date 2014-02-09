// Type: gvtrademap_cs.assembly_load_error_form
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace gvtrademap_cs
{
  public class assembly_load_error_form : Form
  {
    private IContainer components;
    private Button button3;
    private Button button2;
    private Button button1;
    private TextBox textBox1;
    private AssemblyName m_assembly_name;

    public assembly_load_error_form(AssemblyName assembly_name)
    {
      this.InitializeComponent();
      this.m_assembly_name = assembly_name;
      string str = "大航海時代Online 交易MAP C# ver.1.32.3\n" + assembly_name.FullName + "\n" + "の読み込みに失敗しました。\n\n" + "交易Map C#の起動にはMicrsoft DirectX 9.0C以降、Managed DirectX(MDX1.1) が必要です。\n" + "MDX1.1をインスト\x30FCルするには DirectX End-User Runtime Web Installer を実行してください。\n" + "DirectX End-User Runtime Web InstallerはMDX1.1をインスト\x30FCルしてくれます。\n" + "\n" + "MDX1.1をインスト\x30FCルしたにも関わらず起動できない場合はエラ\x30FC内容を報告してもらえると対応できるかもしれません。";
      this.textBox1.AcceptsReturn = true;
      this.textBox1.Lines = str.Split(new char[1]
      {
        '\n'
      });
      this.textBox1.Select(0, 0);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.button3 = new Button();
      this.button2 = new Button();
      this.button1 = new Button();
      this.textBox1 = new TextBox();
      this.SuspendLayout();
      this.button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.button3.Location = new Point(12, 209);
      this.button3.Name = "button3";
      this.button3.Size = new Size(539, 23);
      this.button3.TabIndex = 2;
      this.button3.Text = "DirectX End-User Runtime Web Installer ダウンロ\x30FCドペ\x30FCジを開く";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new EventHandler(this.button3_Click);
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.button2.Location = new Point(12, 238);
      this.button2.Name = "button2";
      this.button2.Size = new Size(539, 23);
      this.button2.TabIndex = 1;
      this.button2.Text = "エラ\x30FC報告を行うペ\x30FCジを開く";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.button1.DialogResult = DialogResult.OK;
      this.button1.Location = new Point(12, 267);
      this.button1.Name = "button1";
      this.button1.Size = new Size(539, 23);
      this.button1.TabIndex = 0;
      this.button1.Text = "閉じる";
      this.button1.UseVisualStyleBackColor = true;
      this.textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBox1.Location = new Point(12, 12);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.ScrollBars = ScrollBars.Both;
      this.textBox1.Size = new Size(539, 191);
      this.textBox1.TabIndex = 3;
      this.AcceptButton = (IButtonControl) this.button1;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(563, 302);
      this.Controls.Add((Control) this.textBox1);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "assembly_load_error_form";
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "起動エラ\x30FC";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void button3_Click(object sender, EventArgs e)
    {
      Process.Start("http://www.microsoft.com/downloads/details.aspx?displaylang=ja&FamilyID=2da43d38-db71-4c1b-bc6a-9b6652cd92a3");
    }

    private void button2_Click(object sender, EventArgs e)
    {
      Process.Start("http://jbbs.livedoor.jp/netgame/7161/");
    }
  }
}
