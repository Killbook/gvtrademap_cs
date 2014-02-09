// Type: gvtrademap_cs.map_mark_form
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using gvtrademap_cs.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Utility;

namespace gvtrademap_cs
{
  public class map_mark_form : Form
  {
    private IContainer components;
    private Button button1;
    private Button button2;
    private Button button3;
    private Button button4;
    private Button button5;
    private Button button6;
    private Button button7;
    private Button button8;
    private Button button9;
    private Button button10;
    private Button button11;
    private Button button12;
    private Button button13;
    private Button button14;
    private Button button15;
    private Button button16;
    private Button button17;
    private Button button18;
    private Button button19;
    private Button button20;
    private TextBox textBox1;
    private ToolTip toolTip1;
    private Button button21;
    private Button button22;
    private Button button23;
    private Label label1;
    private TextBox textBox2;
    private TextBox textBox3;
    private Label label2;
    private Label label3;
    private Label label4;
    private int m_icon_index;
    private Point m_position;
    private string m_memo;

    public int icon_index
    {
      get
      {
        return this.m_icon_index;
      }
    }

    public string memo
    {
      get
      {
        return this.m_memo;
      }
    }

    public Point position
    {
      get
      {
        return this.m_position;
      }
    }

    public map_mark_form(Point position)
    {
      this.init(position, 0, "");
    }

    public map_mark_form(Point position, int index, string memo)
    {
      this.init(position, index, memo);
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
      this.textBox1 = new TextBox();
      this.toolTip1 = new ToolTip(this.components);
      this.button21 = new Button();
      this.button22 = new Button();
      this.label1 = new Label();
      this.button23 = new Button();
      this.button20 = new Button();
      this.button19 = new Button();
      this.button18 = new Button();
      this.button17 = new Button();
      this.button16 = new Button();
      this.button15 = new Button();
      this.button14 = new Button();
      this.button13 = new Button();
      this.button12 = new Button();
      this.button11 = new Button();
      this.button10 = new Button();
      this.button9 = new Button();
      this.button8 = new Button();
      this.button7 = new Button();
      this.button6 = new Button();
      this.button5 = new Button();
      this.button4 = new Button();
      this.button3 = new Button();
      this.button2 = new Button();
      this.button1 = new Button();
      this.textBox2 = new TextBox();
      this.textBox3 = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.SuspendLayout();
      this.textBox1.Location = new Point(106, 114);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(134, 19);
      this.textBox1.TabIndex = 0;
      this.button21.DialogResult = DialogResult.Cancel;
      this.button21.Location = new Point(157, 139);
      this.button21.Name = "button21";
      this.button21.Size = new Size(83, 23);
      this.button21.TabIndex = 4;
      this.button21.Text = "キャンセル";
      this.button21.UseVisualStyleBackColor = true;
      this.button22.DialogResult = DialogResult.OK;
      this.button22.Location = new Point(68, 139);
      this.button22.Name = "button22";
      this.button22.Size = new Size(83, 23);
      this.button22.TabIndex = 3;
      this.button22.Text = "OK";
      this.button22.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 117);
      this.label1.Name = "label1";
      this.label1.Size = new Size(35, 12);
      this.label1.TabIndex = 24;
      this.label1.Text = "label1";
      this.button23.BackgroundImage = (Image) Resources.memo_icon00;
      this.button23.BackgroundImageLayout = ImageLayout.Stretch;
      this.button23.FlatStyle = FlatStyle.Flat;
      this.button23.Location = new Point(12, 79);
      this.button23.Name = "button23";
      this.button23.Size = new Size(32, 32);
      this.button23.TabIndex = 23;
      this.button23.TabStop = false;
      this.button23.UseVisualStyleBackColor = true;
      this.button20.BackgroundImage = (Image) Resources.memo_icon21;
      this.button20.BackgroundImageLayout = ImageLayout.Stretch;
      this.button20.FlatStyle = FlatStyle.Flat;
      this.button20.Location = new Point(220, 38);
      this.button20.Name = "button20";
      this.button20.Size = new Size(20, 20);
      this.button20.TabIndex = 19;
      this.button20.TabStop = false;
      this.button20.UseVisualStyleBackColor = true;
      this.button19.BackgroundImage = (Image) Resources.memo_icon20;
      this.button19.BackgroundImageLayout = ImageLayout.Stretch;
      this.button19.FlatStyle = FlatStyle.Flat;
      this.button19.Location = new Point(194, 38);
      this.button19.Name = "button19";
      this.button19.Size = new Size(20, 20);
      this.button19.TabIndex = 18;
      this.button19.TabStop = false;
      this.button19.UseVisualStyleBackColor = true;
      this.button18.BackgroundImage = (Image) Resources.memo_icon19;
      this.button18.BackgroundImageLayout = ImageLayout.Stretch;
      this.button18.FlatStyle = FlatStyle.Flat;
      this.button18.Location = new Point(168, 38);
      this.button18.Name = "button18";
      this.button18.Size = new Size(20, 20);
      this.button18.TabIndex = 17;
      this.button18.TabStop = false;
      this.button18.UseVisualStyleBackColor = true;
      this.button17.BackgroundImage = (Image) Resources.memo_icon18;
      this.button17.BackgroundImageLayout = ImageLayout.Stretch;
      this.button17.FlatStyle = FlatStyle.Flat;
      this.button17.Location = new Point(142, 38);
      this.button17.Name = "button17";
      this.button17.Size = new Size(20, 20);
      this.button17.TabIndex = 16;
      this.button17.TabStop = false;
      this.button17.UseVisualStyleBackColor = true;
      this.button16.BackgroundImage = (Image) Resources.memo_icon17;
      this.button16.BackgroundImageLayout = ImageLayout.Stretch;
      this.button16.FlatStyle = FlatStyle.Flat;
      this.button16.Location = new Point(116, 38);
      this.button16.Name = "button16";
      this.button16.Size = new Size(20, 20);
      this.button16.TabIndex = 15;
      this.button16.TabStop = false;
      this.button16.UseVisualStyleBackColor = true;
      this.button15.BackgroundImage = (Image) Resources.memo_icon16;
      this.button15.BackgroundImageLayout = ImageLayout.Stretch;
      this.button15.FlatStyle = FlatStyle.Flat;
      this.button15.Location = new Point(90, 38);
      this.button15.Name = "button15";
      this.button15.Size = new Size(20, 20);
      this.button15.TabIndex = 14;
      this.button15.TabStop = false;
      this.button15.UseVisualStyleBackColor = true;
      this.button14.BackgroundImage = (Image) Resources.memo_icon15;
      this.button14.BackgroundImageLayout = ImageLayout.Stretch;
      this.button14.FlatStyle = FlatStyle.Flat;
      this.button14.Location = new Point(220, 12);
      this.button14.Name = "button14";
      this.button14.Size = new Size(20, 20);
      this.button14.TabIndex = 13;
      this.button14.TabStop = false;
      this.button14.UseVisualStyleBackColor = true;
      this.button13.BackgroundImage = (Image) Resources.memo_icon14;
      this.button13.BackgroundImageLayout = ImageLayout.Stretch;
      this.button13.FlatStyle = FlatStyle.Flat;
      this.button13.Location = new Point(194, 12);
      this.button13.Name = "button13";
      this.button13.Size = new Size(20, 20);
      this.button13.TabIndex = 12;
      this.button13.TabStop = false;
      this.button13.UseVisualStyleBackColor = true;
      this.button12.BackgroundImage = (Image) Resources.memo_icon13;
      this.button12.BackgroundImageLayout = ImageLayout.Stretch;
      this.button12.FlatStyle = FlatStyle.Flat;
      this.button12.Location = new Point(168, 12);
      this.button12.Name = "button12";
      this.button12.Size = new Size(20, 20);
      this.button12.TabIndex = 11;
      this.button12.TabStop = false;
      this.button12.UseVisualStyleBackColor = true;
      this.button11.BackgroundImage = (Image) Resources.memo_icon12;
      this.button11.BackgroundImageLayout = ImageLayout.Stretch;
      this.button11.FlatStyle = FlatStyle.Flat;
      this.button11.Location = new Point(142, 12);
      this.button11.Name = "button11";
      this.button11.Size = new Size(20, 20);
      this.button11.TabIndex = 10;
      this.button11.TabStop = false;
      this.button11.UseVisualStyleBackColor = true;
      this.button10.BackgroundImage = (Image) Resources.memo_icon11;
      this.button10.BackgroundImageLayout = ImageLayout.Stretch;
      this.button10.FlatStyle = FlatStyle.Flat;
      this.button10.Location = new Point(116, 12);
      this.button10.Name = "button10";
      this.button10.Size = new Size(20, 20);
      this.button10.TabIndex = 9;
      this.button10.TabStop = false;
      this.button10.UseVisualStyleBackColor = true;
      this.button9.BackgroundImage = (Image) Resources.memo_icon10;
      this.button9.BackgroundImageLayout = ImageLayout.Stretch;
      this.button9.FlatStyle = FlatStyle.Flat;
      this.button9.Location = new Point(90, 12);
      this.button9.Name = "button9";
      this.button9.Size = new Size(20, 20);
      this.button9.TabIndex = 8;
      this.button9.TabStop = false;
      this.button9.UseVisualStyleBackColor = true;
      this.button8.BackgroundImage = (Image) Resources.memo_icon07;
      this.button8.BackgroundImageLayout = ImageLayout.Stretch;
      this.button8.FlatStyle = FlatStyle.Flat;
      this.button8.Location = new Point(12, 34);
      this.button8.Name = "button8";
      this.button8.Size = new Size(16, 16);
      this.button8.TabIndex = 7;
      this.button8.TabStop = false;
      this.button8.UseVisualStyleBackColor = true;
      this.button7.BackgroundImage = (Image) Resources.memo_icon06;
      this.button7.BackgroundImageLayout = ImageLayout.Stretch;
      this.button7.FlatStyle = FlatStyle.Flat;
      this.button7.Location = new Point(12, 56);
      this.button7.Name = "button7";
      this.button7.Size = new Size(16, 16);
      this.button7.TabIndex = 6;
      this.button7.TabStop = false;
      this.button7.UseVisualStyleBackColor = true;
      this.button6.BackgroundImage = (Image) Resources.memo_icon05;
      this.button6.BackgroundImageLayout = ImageLayout.Stretch;
      this.button6.FlatStyle = FlatStyle.Flat;
      this.button6.Location = new Point(34, 56);
      this.button6.Name = "button6";
      this.button6.Size = new Size(16, 16);
      this.button6.TabIndex = 5;
      this.button6.TabStop = false;
      this.button6.UseVisualStyleBackColor = true;
      this.button5.BackgroundImage = (Image) Resources.memo_icon04;
      this.button5.BackgroundImageLayout = ImageLayout.Stretch;
      this.button5.FlatStyle = FlatStyle.Flat;
      this.button5.Location = new Point(56, 56);
      this.button5.Name = "button5";
      this.button5.Size = new Size(16, 16);
      this.button5.TabIndex = 4;
      this.button5.TabStop = false;
      this.button5.UseVisualStyleBackColor = true;
      this.button4.BackgroundImage = (Image) Resources.memo_icon03;
      this.button4.BackgroundImageLayout = ImageLayout.Stretch;
      this.button4.FlatStyle = FlatStyle.Flat;
      this.button4.Location = new Point(56, 34);
      this.button4.Name = "button4";
      this.button4.Size = new Size(16, 16);
      this.button4.TabIndex = 3;
      this.button4.TabStop = false;
      this.button4.UseVisualStyleBackColor = true;
      this.button3.BackgroundImage = (Image) Resources.memo_icon02;
      this.button3.BackgroundImageLayout = ImageLayout.Stretch;
      this.button3.FlatStyle = FlatStyle.Flat;
      this.button3.Location = new Point(56, 12);
      this.button3.Name = "button3";
      this.button3.Size = new Size(16, 16);
      this.button3.TabIndex = 2;
      this.button3.TabStop = false;
      this.button3.UseVisualStyleBackColor = true;
      this.button2.BackgroundImage = (Image) Resources.memo_icon01;
      this.button2.BackgroundImageLayout = ImageLayout.Stretch;
      this.button2.FlatStyle = FlatStyle.Flat;
      this.button2.Location = new Point(34, 12);
      this.button2.Name = "button2";
      this.button2.Size = new Size(16, 16);
      this.button2.TabIndex = 1;
      this.button2.TabStop = false;
      this.button2.UseVisualStyleBackColor = true;
      this.button1.BackgroundImage = (Image) Resources.memo_icon00;
      this.button1.BackgroundImageLayout = ImageLayout.Stretch;
      this.button1.FlatStyle = FlatStyle.Flat;
      this.button1.Location = new Point(12, 12);
      this.button1.Name = "button1";
      this.button1.Size = new Size(16, 16);
      this.button1.TabIndex = 0;
      this.button1.TabStop = false;
      this.button1.UseVisualStyleBackColor = true;
      this.textBox2.Location = new Point(168, 64);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(72, 19);
      this.textBox2.TabIndex = 1;
      this.textBox2.TextAlign = HorizontalAlignment.Right;
      this.textBox3.Location = new Point(168, 89);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new Size(72, 19);
      this.textBox3.TabIndex = 2;
      this.textBox3.TextAlign = HorizontalAlignment.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new Point((int) sbyte.MaxValue, 67);
      this.label2.Name = "label2";
      this.label2.Size = new Size(36, 12);
      this.label2.TabIndex = 27;
      this.label2.Text = "位置X";
      this.label3.AutoSize = true;
      this.label3.Location = new Point((int) sbyte.MaxValue, 92);
      this.label3.Name = "label3";
      this.label3.Size = new Size(36, 12);
      this.label3.TabIndex = 28;
      this.label3.Text = "位置Y";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(74, 117);
      this.label4.Name = "label4";
      this.label4.Size = new Size(22, 12);
      this.label4.TabIndex = 29;
      this.label4.Text = "メモ";
      this.AcceptButton = (IButtonControl) this.button22;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.button21;
      this.ClientSize = new Size(252, 170);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.textBox3);
      this.Controls.Add((Control) this.textBox2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.button23);
      this.Controls.Add((Control) this.button21);
      this.Controls.Add((Control) this.button22);
      this.Controls.Add((Control) this.textBox1);
      this.Controls.Add((Control) this.button20);
      this.Controls.Add((Control) this.button19);
      this.Controls.Add((Control) this.button18);
      this.Controls.Add((Control) this.button17);
      this.Controls.Add((Control) this.button16);
      this.Controls.Add((Control) this.button15);
      this.Controls.Add((Control) this.button14);
      this.Controls.Add((Control) this.button13);
      this.Controls.Add((Control) this.button12);
      this.Controls.Add((Control) this.button11);
      this.Controls.Add((Control) this.button10);
      this.Controls.Add((Control) this.button9);
      this.Controls.Add((Control) this.button8);
      this.Controls.Add((Control) this.button7);
      this.Controls.Add((Control) this.button6);
      this.Controls.Add((Control) this.button5);
      this.Controls.Add((Control) this.button4);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "map_mark_form";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "メモアイコンの設定";
      this.FormClosed += new FormClosedEventHandler(this.map_mark_form_FormClosed);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void init(Point position, int index, string memo)
    {
      this.InitializeComponent();
      Useful.SetFontMeiryo((Form) this, 8f);
      this.m_position = position;
      this.textBox1.Text = memo;
      this.textBox2.Text = position.X.ToString();
      this.textBox3.Text = position.Y.ToString();
      this.toolTip1.AutoPopDelay = 30000;
      this.toolTip1.BackColor = Color.LightYellow;
      this.toolTip1.SetToolTip((Control) this.button1, "風向き");
      this.toolTip1.SetToolTip((Control) this.button2, "風向き");
      this.toolTip1.SetToolTip((Control) this.button3, "風向き");
      this.toolTip1.SetToolTip((Control) this.button4, "風向き");
      this.toolTip1.SetToolTip((Control) this.button5, "風向き");
      this.toolTip1.SetToolTip((Control) this.button6, "風向き");
      this.toolTip1.SetToolTip((Control) this.button7, "風向き");
      this.toolTip1.SetToolTip((Control) this.button8, "風向き");
      this.toolTip1.SetToolTip((Control) this.button9, "サメ");
      this.toolTip1.SetToolTip((Control) this.button10, "火災");
      this.toolTip1.SetToolTip((Control) this.button11, "藻");
      this.toolTip1.SetToolTip((Control) this.button12, "セイレ\x30FCン");
      this.toolTip1.SetToolTip((Control) this.button13, "地場異常");
      this.toolTip1.SetToolTip((Control) this.button14, "漁場");
      this.toolTip1.SetToolTip((Control) this.button15, "マンボウ");
      this.toolTip1.SetToolTip((Control) this.button16, "free");
      this.toolTip1.SetToolTip((Control) this.button17, "free");
      this.toolTip1.SetToolTip((Control) this.button18, "free");
      this.toolTip1.SetToolTip((Control) this.button19, "free");
      this.toolTip1.SetToolTip((Control) this.button20, "目的地");
      this.toolTip1.SetToolTip((Control) this.textBox1, "メモを入力してください");
      this.toolTip1.SetToolTip((Control) this.textBox2, "位置Xを指定してください");
      this.toolTip1.SetToolTip((Control) this.textBox3, "位置Yを指定してください");
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.Click += new EventHandler(this.button2_Click);
      this.button3.Click += new EventHandler(this.button3_Click);
      this.button4.Click += new EventHandler(this.button4_Click);
      this.button5.Click += new EventHandler(this.button5_Click);
      this.button6.Click += new EventHandler(this.button6_Click);
      this.button7.Click += new EventHandler(this.button7_Click);
      this.button8.Click += new EventHandler(this.button8_Click);
      this.button9.Click += new EventHandler(this.button9_Click);
      this.button10.Click += new EventHandler(this.button10_Click);
      this.button11.Click += new EventHandler(this.button11_Click);
      this.button12.Click += new EventHandler(this.button12_Click);
      this.button13.Click += new EventHandler(this.button13_Click);
      this.button14.Click += new EventHandler(this.button14_Click);
      this.button15.Click += new EventHandler(this.button15_Click);
      this.button16.Click += new EventHandler(this.button16_Click);
      this.button17.Click += new EventHandler(this.button17_Click);
      this.button18.Click += new EventHandler(this.button18_Click);
      this.button19.Click += new EventHandler(this.button19_Click);
      this.button20.Click += new EventHandler(this.button20_Click);
      this.change_icon(index);
    }

    private void change_icon(int index)
    {
      Button[] buttonArray = new Button[20]
      {
        this.button1,
        this.button2,
        this.button3,
        this.button4,
        this.button5,
        this.button6,
        this.button7,
        this.button8,
        this.button9,
        this.button10,
        this.button11,
        this.button12,
        this.button13,
        this.button14,
        this.button15,
        this.button16,
        this.button17,
        this.button18,
        this.button19,
        this.button20
      };
      if (index < 0)
        index = 0;
      if (index >= 20)
        index = 19;
      this.m_icon_index = index;
      string toolTip = this.toolTip1.GetToolTip((Control) buttonArray[this.m_icon_index]);
      this.toolTip1.SetToolTip((Control) this.button23, toolTip);
      this.label1.Text = toolTip;
      this.button23.BackgroundImage = buttonArray[this.m_icon_index].BackgroundImage;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.change_icon(0);
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.change_icon(1);
    }

    private void button3_Click(object sender, EventArgs e)
    {
      this.change_icon(2);
    }

    private void button4_Click(object sender, EventArgs e)
    {
      this.change_icon(3);
    }

    private void button5_Click(object sender, EventArgs e)
    {
      this.change_icon(4);
    }

    private void button6_Click(object sender, EventArgs e)
    {
      this.change_icon(5);
    }

    private void button7_Click(object sender, EventArgs e)
    {
      this.change_icon(6);
    }

    private void button8_Click(object sender, EventArgs e)
    {
      this.change_icon(7);
    }

    private void button9_Click(object sender, EventArgs e)
    {
      this.change_icon(8);
    }

    private void button10_Click(object sender, EventArgs e)
    {
      this.change_icon(9);
    }

    private void button11_Click(object sender, EventArgs e)
    {
      this.change_icon(10);
    }

    private void button12_Click(object sender, EventArgs e)
    {
      this.change_icon(11);
    }

    private void button13_Click(object sender, EventArgs e)
    {
      this.change_icon(12);
    }

    private void button14_Click(object sender, EventArgs e)
    {
      this.change_icon(13);
    }

    private void button15_Click(object sender, EventArgs e)
    {
      this.change_icon(14);
    }

    private void button16_Click(object sender, EventArgs e)
    {
      this.change_icon(15);
    }

    private void button17_Click(object sender, EventArgs e)
    {
      this.change_icon(16);
    }

    private void button18_Click(object sender, EventArgs e)
    {
      this.change_icon(17);
    }

    private void button19_Click(object sender, EventArgs e)
    {
      this.change_icon(18);
    }

    private void button20_Click(object sender, EventArgs e)
    {
      this.change_icon(19);
    }

    private void map_mark_form_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.m_memo = this.textBox1.Text;
      try
      {
        int num1 = Convert.ToInt32(this.textBox2.Text);
        int num2 = Convert.ToInt32(this.textBox3.Text);
        this.m_position.X = num1;
        this.m_position.Y = num2;
      }
      catch
      {
      }
    }
  }
}
