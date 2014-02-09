// Type: Utility.error_form
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Utility
{
  public class error_form : Form
  {
    private IContainer components;
    private Label label1;
    private Label label2;
    private TextBox textBox1;
    private Button button1;
    private Button button2;
    private Button button3;
    private string m_message;
    private string m_url;

    public error_form(string window_title, Exception ex, string message_top, string url)
    {
      this.initialize(window_title, ex, message_top, url, "");
    }

    public error_form(string window_title, Exception ex, string message_top, string url, string device_info_string)
    {
      this.initialize(window_title, ex, message_top, url, device_info_string);
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
      this.label2 = new Label();
      this.textBox1 = new TextBox();
      this.button1 = new Button();
      this.button2 = new Button();
      this.button3 = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(182, 12);
      this.label1.TabIndex = 0;
      this.label1.Text = "想定していないエラ\x30FCが発生しました。";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 28);
      this.label2.Name = "label2";
      this.label2.Size = new Size(312, 12);
      this.label2.TabIndex = 1;
      this.label2.Text = "下記のエラ\x30FC内容を報告してもらえると対応できるかもしれません。";
      this.textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBox1.Location = new Point(14, 48);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.ScrollBars = ScrollBars.Both;
      this.textBox1.Size = new Size(458, 229);
      this.textBox1.TabIndex = 3;
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.button1.DialogResult = DialogResult.OK;
      this.button1.Location = new Point(14, 341);
      this.button1.Name = "button1";
      this.button1.Size = new Size(458, 23);
      this.button1.TabIndex = 0;
      this.button1.Text = "閉じる";
      this.button1.UseVisualStyleBackColor = true;
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.button2.Location = new Point(14, 312);
      this.button2.Name = "button2";
      this.button2.Size = new Size(458, 23);
      this.button2.TabIndex = 1;
      this.button2.Text = "エラ\x30FC報告を行うペ\x30FCジを開く";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.button3.Location = new Point(14, 283);
      this.button3.Name = "button3";
      this.button3.Size = new Size(458, 23);
      this.button3.TabIndex = 2;
      this.button3.Text = "エラ\x30FC内容をクリップボ\x30FCドにコピ\x30FC";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new EventHandler(this.button3_Click);
      this.AcceptButton = (IButtonControl) this.button1;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.button1;
      this.ClientSize = new Size(484, 376);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.textBox1);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "error_form";
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "想定外のエラ\x30FC";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void initialize(string window_title, Exception ex, string message_top, string url, string device_info_string)
    {
      this.InitializeComponent();
      string str1 = "";
      if (!string.IsNullOrEmpty(message_top))
        str1 = str1 + message_top + "\n";
      string str2 = str1 + "DATE:" + Useful.TojbbsDateTimeString(DateTime.Now) + "\n";
      OperatingSystem osVersion = Environment.OSVersion;
      string str3 = str2 + "OS:" + osVersion.VersionString + "\n" + "OS:" + Useful.GetOsName(osVersion) + "\n";
      if (!string.IsNullOrEmpty(device_info_string))
        str3 = str3 + "DeviceInfo:" + device_info_string + "\n";
      this.m_message = ex != null ? str3 + "Message: " + ex.Message + "\nStackTrace:\n" + error_form.make_error_message(ex.StackTrace) : str3 + "エラ\x30FC内容が不明";
      if (!string.IsNullOrEmpty(window_title))
        this.Text = window_title;
      this.textBox1.AcceptsReturn = true;
      this.textBox1.Lines = this.m_message.Split(new char[1]
      {
        '\n'
      });
      this.textBox1.Select(0, 0);
      if (!string.IsNullOrEmpty(url))
      {
        this.m_url = url;
      }
      else
      {
        this.m_url = "";
        this.button2.Enabled = false;
        this.label2.Text = "--";
      }
    }

    private void button3_Click(object sender, EventArgs e)
    {
      Clipboard.SetText(this.m_message);
    }

    private void button2_Click(object sender, EventArgs e)
    {
      if (!(this.m_url != ""))
        return;
      Process.Start(this.m_url);
    }

    private static string make_error_message(string str)
    {
      try
      {
        str = str.Replace("\r\n", "\n");
        return str;
      }
      catch
      {
        return str;
      }
    }
  }
}
