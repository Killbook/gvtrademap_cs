// Type: gvtrademap_cs.find_form2
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using gvo_base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Utility;
using Utility.Ctrl;

namespace gvtrademap_cs
{
  public class find_form2 : Form
  {
    private const int LIST_MAX = 2000;
    private gvt_lib m_lib;
    private GvoDatabase m_db;
    private spot m_spot;
    private item_window m_item_window;
    private Point m_gpos;
    private ListViewItemSorter m_sorter;
    private bool m_now_find;
    private IContainer components;
    private Button button3;
    private ToolTip toolTip1;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem open_recipe_wiki0_ToolStripMenuItem;
    private ToolStripMenuItem open_recipe_wiki1_ToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripMenuItem copy_name_to_clipboardToolStripMenuItem;
    private ToolStripMenuItem copy_all_to_clipboardToolStripMenuItem;
    private TabControl tabControl1;
    private TabPage tabPage1;
    private ListView listView1;
    private Label label2;
    private ComboBox comboBox1;
    private Button button1;
    private TabPage tabPage2;
    private Label label3;
    private Label label1;
    private Button button2;
    private ListView listView2;
    private Button button4;
    private TabPage tabPage3;
    private Button button5;
    private ListView listView3;
    private ComboBox comboBox2;
    private ComboBox comboBox4;
    private ComboBox comboBox3;

    public find_form2(gvt_lib lib, GvoDatabase db, spot _spot, item_window _item_window)
    {
      this.m_lib = lib;
      this.m_db = db;
      this.m_spot = _spot;
      this.m_item_window = _item_window;
      this.m_gpos = new Point(-1, -1);
      this.m_sorter = new ListViewItemSorter();
      this.InitializeComponent();
      Useful.SetFontMeiryo((Form) this, 8f);
      this.m_now_find = false;
      this.toolTip1.AutoPopDelay = 30000;
      this.toolTip1.BackColor = Color.LightYellow;
      this.init_page1();
      this.init_page2();
      this.init_page3();
    }

    private void init_page1()
    {
      this.comboBox2.SelectedIndex = (int) this.m_lib.setting.find_filter;
      this.comboBox3.SelectedIndex = (int) this.m_lib.setting.find_filter2;
      this.comboBox4.SelectedIndex = (int) this.m_lib.setting.find_filter3;
      this.listView1.Columns.Add("一致名", 180);
      this.listView1.Columns.Add("種類", 80);
      this.listView1.Columns.Add("場所", 120);
      this.update_find_strings();
      this.update_find_result((List<GvoDatabase.Find>) null);
      this.update_spot_button_status();
    }

    private void init_page2()
    {
      this.listView2.Columns.Add("場所", 120);
      this.listView2.Columns.Add("種類", 150);
      this.listView2.Columns.Add("", 100);
      this.update_spot_list();
    }

    private void init_page3()
    {
      this.listView3.Columns.Add("名前", 180);
      this.listView3.Columns.Add("種類", 80);
      this.listView3.Columns.Add(" ", 120);
      this.update_cultural_sphere_list();
      this.update_cultural_sphere_button();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.do_find();
    }

    private void do_find()
    {
      string text = this.comboBox1.Text;
      if (text == "")
        return;
      this.m_now_find = true;
      this.m_lib.setting.find_strings.Add(text);
      if (this.do_centering_gpos(text))
      {
        this.listView1.Items.Clear();
        this.m_lib.setting.centering_gpos = this.m_gpos;
        this.m_lib.setting.req_centering_gpos.Request();
      }
      else
      {
        this.Cursor = Cursors.WaitCursor;
        List<GvoDatabase.Find> all = this.m_db.FindAll(text);
        this.update_find_strings();
        this.update_find_result(all);
        this.update_spot_button_status();
        this.Cursor = Cursors.Default;
      }
      this.m_now_find = false;
    }

    private void update_find_strings()
    {
      this.comboBox1.DropDownHeight = 200;
      this.comboBox1.Items.Clear();
      if (this.m_lib.setting.find_strings.Count <= 0)
        return;
      foreach (object obj in this.m_lib.setting.find_strings)
        this.comboBox1.Items.Add(obj);
      this.comboBox1.SelectedIndex = 0;
    }

    private void update_find_result(List<GvoDatabase.Find> results)
    {
      this.listView1.BeginUpdate();
      this.listView1.Items.Clear();
      bool flag = false;
      if (results != null)
      {
        foreach (GvoDatabase.Find i in results)
        {
          this.add_item(this.listView1, i);
          if (this.listView1.Items.Count >= 2000)
          {
            flag = true;
            break;
          }
        }
      }
      this.listView1.EndUpdate();
      if (results != null)
        this.label2.Text = string.Format("{0}件", (object) results.Count);
      else
        this.label2.Text = "0件";
      if (!flag)
        return;
      int num = (int) MessageBox.Show((IWin32Window) this, 2000.ToString() + "件以上はスキップされました。", "検索結果が多すぎます");
    }

    private void add_item(ListView listview, GvoDatabase.Find i)
    {
      if (i.Type != GvoDatabase.Find.FindType.CulturalSphere)
      {
        switch (this.m_lib.setting.find_filter)
        {
          case _find_filter.world_info:
            if (i.Type == GvoDatabase.Find.FindType.Database)
              return;
            else
              break;
          case _find_filter.item_database:
            if (i.Type != GvoDatabase.Find.FindType.Database)
              return;
            else
              break;
        }
      }
      listview.Items.Add(new ListViewItem(i.NameString, 0)
      {
        Tag = (object) i,
        ToolTipText = i.TooltipString,
        SubItems = {
          i.TypeString,
          i.SpotString
        }
      });
    }

    private bool do_centering_gpos(string str)
    {
      try
      {
        str = str.Replace('０', '0');
        str = str.Replace('１', '1');
        str = str.Replace('２', '2');
        str = str.Replace('３', '3');
        str = str.Replace('４', '4');
        str = str.Replace('５', '5');
        str = str.Replace('６', '6');
        str = str.Replace('７', '7');
        str = str.Replace('８', '8');
        str = str.Replace('９', '9');
        str = str.Replace('、', ',');
        str = str.Replace('，', ',');
        str = str.Replace('.', ',');
        str = str.Replace('。', ',');
        str = str.Replace('．', ',');
        string[] strArray = str.Split(new char[1]
        {
          ','
        });
        if (strArray.Length != 2)
          return false;
        Point point = new Point(Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]));
        if (point.X < 0 || point.Y < 0)
          return false;
        this.m_gpos = point;
        return true;
      }
      catch
      {
        return false;
      }
    }

    private GvoDatabase.Find get_selected_item_tag()
    {
      return this.get_selected_item_tag(this.listView1);
    }

    private GvoDatabase.Find get_selected_item_tag(ListView view)
    {
      if (view.SelectedItems.Count <= 0)
        return (GvoDatabase.Find) null;
      ListViewItem listViewItem = view.SelectedItems[0];
      if (listViewItem.Tag == null)
        return (GvoDatabase.Find) null;
      else
        return (GvoDatabase.Find) listViewItem.Tag;
    }

    private void find_form2_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason != CloseReason.UserClosing)
        return;
      e.Cancel = true;
      this.Hide();
    }

    private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      this.button3.PerformClick();
    }

    private void update_spot_button_status()
    {
      GvoDatabase.Find selectedItemTag = this.get_selected_item_tag();
      if (selectedItemTag == null)
        this.button3.Enabled = false;
      else if (selectedItemTag.Type == GvoDatabase.Find.FindType.Database)
        this.button3.Enabled = false;
      else
        this.button3.Enabled = true;
    }

    private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
    {
      this.update_spot_button_status();
    }

    private void find_form2_Activated(object sender, EventArgs e)
    {
      this.ActiveControl = (Control) this.comboBox1;
    }

    private void button3_Click(object sender, EventArgs e)
    {
      this.do_spot(this.get_selected_item_tag());
    }

    private void do_spot(GvoDatabase.Find item)
    {
      if (item == null)
        return;
      this.m_lib.setting.req_spot_item.Request((object) item);
    }

    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.m_lib.setting.find_filter = (_find_filter) this.comboBox2.SelectedIndex;
      this.do_find();
    }

    private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.m_lib.setting.find_filter2 = (_find_filter2) this.comboBox3.SelectedIndex;
      this.do_find();
    }

    private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.m_lib.setting.find_filter3 = (_find_filter3) this.comboBox4.SelectedIndex;
      this.do_find();
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.m_now_find)
        return;
      this.do_find();
    }

    private void listView1_MouseClick(object sender, MouseEventArgs e)
    {
      if ((e.Button & MouseButtons.Right) == MouseButtons.None)
        return;
      GvoDatabase.Find selectedItemTag = this.get_selected_item_tag();
      if (selectedItemTag == null)
        return;
      Point position = new Point(e.X, e.Y);
      ItemDatabase.Data database = selectedItemTag.Database;
      if (database == null)
      {
        this.open_recipe_wiki0_ToolStripMenuItem.Enabled = false;
        this.open_recipe_wiki1_ToolStripMenuItem.Enabled = false;
        this.copy_all_to_clipboardToolStripMenuItem.Enabled = false;
      }
      else
      {
        this.copy_all_to_clipboardToolStripMenuItem.Enabled = true;
        if (database.IsSkill || database.IsReport)
        {
          this.open_recipe_wiki0_ToolStripMenuItem.Enabled = false;
          this.open_recipe_wiki1_ToolStripMenuItem.Enabled = false;
        }
        else if (database.IsRecipe)
        {
          this.open_recipe_wiki0_ToolStripMenuItem.Enabled = true;
          this.open_recipe_wiki1_ToolStripMenuItem.Enabled = false;
        }
        else
        {
          this.open_recipe_wiki0_ToolStripMenuItem.Enabled = false;
          this.open_recipe_wiki1_ToolStripMenuItem.Enabled = true;
        }
      }
      this.contextMenuStrip1.Show((Control) this.listView1, position);
    }

    private void open_recipe_wiki0_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      GvoDatabase.Find selectedItemTag = this.get_selected_item_tag();
      if (selectedItemTag == null || selectedItemTag.Database == null)
        return;
      selectedItemTag.Database.OpenRecipeWiki0();
    }

    private void open_recipe_wiki1_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      GvoDatabase.Find selectedItemTag = this.get_selected_item_tag();
      if (selectedItemTag == null || selectedItemTag.Database == null)
        return;
      selectedItemTag.Database.OpenRecipeWiki1();
    }

    private void copy_name_to_clipboardToolStripMenuItem_Click(object sender, EventArgs e)
    {
      GvoDatabase.Find selectedItemTag = this.get_selected_item_tag();
      if (selectedItemTag == null)
        return;
      Clipboard.SetText(selectedItemTag.NameString);
    }

    private void copy_all_to_clipboardToolStripMenuItem_Click(object sender, EventArgs e)
    {
      GvoDatabase.Find selectedItemTag = this.get_selected_item_tag();
      if (selectedItemTag == null)
        return;
      if (selectedItemTag.Data != null)
      {
        Clipboard.SetText(selectedItemTag.Data.TooltipString);
      }
      else
      {
        if (selectedItemTag.Database == null)
          return;
        Clipboard.SetText(selectedItemTag.Database.GetToolTipString());
      }
    }

    private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      this.m_sorter.Sort(this.listView1, e.Column);
    }

    public void SetFindMode()
    {
      this.tabControl1.SelectedIndex = 0;
    }

    public void UpdateSpotList()
    {
      if (this.m_spot.is_spot)
        this.tabControl1.SelectedIndex = 2;
      else
        this.SetFindMode();
      this.update_spot_list();
    }

    private void update_spot_list()
    {
      this.listView2.BeginUpdate();
      this.listView2.Items.Clear();
      List<spot.spot_once> list = this.m_spot.list;
      this.label1.Text = string.Format("{0}件", (object) list.Count);
      this.label3.Text = this.m_spot.spot_type_str;
      this.listView2.Columns[2].Text = this.m_spot.spot_type_column_str;
      foreach (spot.spot_once spotOnce in list)
        this.listView2.Items.Add(new ListViewItem(spotOnce.info.Name, 0)
        {
          Tag = (object) spotOnce,
          SubItems = {
            spotOnce.name,
            spotOnce.ex
          }
        });
      this.listView2.EndUpdate();
      this.button2.Enabled = list.Count > 0;
      this.update_select_info();
    }

    private void update_select_info()
    {
      GvoWorldInfo.Info info = this.m_item_window.info;
      if (info != null)
      {
        foreach (ListViewItem listViewItem in this.listView2.Items)
        {
          object tag = listViewItem.Tag;
          if (tag != null && !(((spot.spot_once) tag).info.Name != info.Name))
          {
            listViewItem.Selected = true;
            listViewItem.EnsureVisible();
            listViewItem.Focused = true;
            return;
          }
        }
      }
      if (this.listView2.Items.Count <= 0)
        return;
      this.listView2.Items[0].Selected = true;
    }

    private void listView2_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listView2.SelectedIndices.Count <= 0 || this.listView2.SelectedItems[0].Tag == null)
        return;
      spot.spot_once spotOnce = this.listView2.SelectedItems[0].Tag as spot.spot_once;
      if (spotOnce == null)
        return;
      this.m_lib.setting.req_spot_item_changed.Request((object) spotOnce);
    }

    private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      this.m_sorter.Sort(this.listView2, e.Column);
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.m_lib.setting.req_spot_item.Request((object) null);
    }

    private void button4_Click(object sender, EventArgs e)
    {
      this.Hide();
    }

    private void update_cultural_sphere_button()
    {
      this.button5.Enabled = this.listView3.SelectedIndices.Count > 0;
    }

    private void update_cultural_sphere_list()
    {
      List<GvoDatabase.Find> culturalSphereList = this.m_db.GetCulturalSphereList();
      this.listView3.BeginUpdate();
      this.listView3.Items.Clear();
      foreach (GvoDatabase.Find i in culturalSphereList)
        this.add_item(this.listView3, i);
      this.listView3.EndUpdate();
    }

    private void button5_Click(object sender, EventArgs e)
    {
      this.do_spot(this.get_selected_item_tag(this.listView3));
    }

    private void listView3_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.update_cultural_sphere_button();
    }

    private void listView3_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      this.m_sorter.Sort(this.listView3, e.Column);
    }

    private void listView3_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      this.button5.PerformClick();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (find_form2));
      this.button3 = new Button();
      this.toolTip1 = new ToolTip(this.components);
      this.listView1 = new ListView();
      this.comboBox1 = new ComboBox();
      this.listView2 = new ListView();
      this.listView3 = new ListView();
      this.comboBox4 = new ComboBox();
      this.comboBox3 = new ComboBox();
      this.comboBox2 = new ComboBox();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.open_recipe_wiki0_ToolStripMenuItem = new ToolStripMenuItem();
      this.open_recipe_wiki1_ToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.copy_name_to_clipboardToolStripMenuItem = new ToolStripMenuItem();
      this.copy_all_to_clipboardToolStripMenuItem = new ToolStripMenuItem();
      this.tabControl1 = new TabControl();
      this.tabPage1 = new TabPage();
      this.label2 = new Label();
      this.button1 = new Button();
      this.tabPage3 = new TabPage();
      this.button5 = new Button();
      this.tabPage2 = new TabPage();
      this.label3 = new Label();
      this.label1 = new Label();
      this.button2 = new Button();
      this.button4 = new Button();
      this.contextMenuStrip1.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.SuspendLayout();
      this.button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button3.Location = new Point(203, 204);
      this.button3.Name = "button3";
      this.button3.Size = new Size(211, 23);
      this.button3.TabIndex = 6;
      this.button3.Text = "スポット表示";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new EventHandler(this.button3_Click);
      this.toolTip1.AutoPopDelay = 50000;
      this.toolTip1.InitialDelay = 500;
      this.toolTip1.ReshowDelay = 100;
      this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listView1.BorderStyle = BorderStyle.FixedSingle;
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.HideSelection = false;
      this.listView1.Location = new Point(6, 64);
      this.listView1.Name = "listView1";
      this.listView1.ShowItemToolTips = true;
      this.listView1.Size = new Size(408, 134);
      this.listView1.TabIndex = 5;
      this.toolTip1.SetToolTip((Control) this.listView1, "おまじない");
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.listView1.MouseDoubleClick += new MouseEventHandler(this.listView1_MouseDoubleClick);
      this.listView1.MouseClick += new MouseEventHandler(this.listView1_MouseClick);
      this.listView1.ColumnClick += new ColumnClickEventHandler(this.listView1_ColumnClick);
      this.listView1.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.ImeMode = ImeMode.NoControl;
      this.comboBox1.Location = new Point(6, 7);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new Size(250, 20);
      this.comboBox1.TabIndex = 0;
      this.toolTip1.SetToolTip((Control) this.comboBox1, "入力された文字列が含まれるものをできるだけ検索します\r\nxxxx,yyyy 形式で入力することで特定の座標をセンタリングすることができます");
      this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
      this.listView2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listView2.BorderStyle = BorderStyle.FixedSingle;
      this.listView2.FullRowSelect = true;
      this.listView2.GridLines = true;
      this.listView2.HideSelection = false;
      this.listView2.Location = new Point(6, 34);
      this.listView2.Name = "listView2";
      this.listView2.ShowItemToolTips = true;
      this.listView2.Size = new Size(408, 164);
      this.listView2.TabIndex = 29;
      this.toolTip1.SetToolTip((Control) this.listView2, "おまじない");
      this.listView2.UseCompatibleStateImageBehavior = false;
      this.listView2.View = View.Details;
      this.listView2.SelectedIndexChanged += new EventHandler(this.listView2_SelectedIndexChanged);
      this.listView2.ColumnClick += new ColumnClickEventHandler(this.listView2_ColumnClick);
      this.listView3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listView3.BorderStyle = BorderStyle.FixedSingle;
      this.listView3.FullRowSelect = true;
      this.listView3.GridLines = true;
      this.listView3.HideSelection = false;
      this.listView3.Location = new Point(6, 6);
      this.listView3.Name = "listView3";
      this.listView3.ShowItemToolTips = true;
      this.listView3.Size = new Size(408, 192);
      this.listView3.TabIndex = 29;
      this.toolTip1.SetToolTip((Control) this.listView3, "おまじない");
      this.listView3.UseCompatibleStateImageBehavior = false;
      this.listView3.View = View.Details;
      this.listView3.MouseDoubleClick += new MouseEventHandler(this.listView3_MouseDoubleClick);
      this.listView3.SelectedIndexChanged += new EventHandler(this.listView3_SelectedIndexChanged);
      this.listView3.ColumnClick += new ColumnClickEventHandler(this.listView3_ColumnClick);
      this.comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox4.FormattingEnabled = true;
      this.comboBox4.Items.AddRange(new object[4]
      {
        (object) "部分一致",
        (object) "完全一致",
        (object) "前方一致",
        (object) "後方一致"
      });
      this.comboBox4.Location = new Point(262, 35);
      this.comboBox4.Name = "comboBox4";
      this.comboBox4.Size = new Size(101, 20);
      this.comboBox4.TabIndex = 4;
      this.toolTip1.SetToolTip((Control) this.comboBox4, componentResourceManager.GetString("comboBox4.ToolTip"));
      this.comboBox4.SelectedIndexChanged += new EventHandler(this.comboBox4_SelectedIndexChanged);
      this.comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox3.FormattingEnabled = true;
      this.comboBox3.Items.AddRange(new object[2]
      {
        (object) "名称等",
        (object) "種類"
      });
      this.comboBox3.Location = new Point(175, 35);
      this.comboBox3.Name = "comboBox3";
      this.comboBox3.Size = new Size(81, 20);
      this.comboBox3.TabIndex = 3;
      this.toolTip1.SetToolTip((Control) this.comboBox3, "・名称等\r\n　アイテムの名前、街名、使用言語から検索します。\r\n・種類\r\n　アイテムの種類から検索します。\r\n　例:食料品");
      this.comboBox3.SelectedIndexChanged += new EventHandler(this.comboBox3_SelectedIndexChanged);
      this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox2.FormattingEnabled = true;
      this.comboBox2.Items.AddRange(new object[3]
      {
        (object) "街情報",
        (object) "アイテムデ\x30FCタベ\x30FCス",
        (object) "全ての情報"
      });
      this.comboBox2.Location = new Point(6, 35);
      this.comboBox2.Name = "comboBox2";
      this.comboBox2.Size = new Size(163, 20);
      this.comboBox2.TabIndex = 2;
      this.toolTip1.SetToolTip((Control) this.comboBox2, "・街情報\r\n　街情報から検索します。\r\n　検索結果はスポット表示することができます。\r\n・アイテムデ\x30FCタベ\x30FCス\r\n　独自に保持しているアイテムデ\x30FCタベ\x30FCスから検索します。\r\n　検索結果はスポット表示することができません。\r\n・全て情報\r\n　上記のどちらからも検索します。");
      this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox2_SelectedIndexChanged);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.open_recipe_wiki0_ToolStripMenuItem,
        (ToolStripItem) this.open_recipe_wiki1_ToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.copy_name_to_clipboardToolStripMenuItem,
        (ToolStripItem) this.copy_all_to_clipboardToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip3";
      this.contextMenuStrip1.Size = new Size(352, 98);
      this.open_recipe_wiki0_ToolStripMenuItem.Name = "open_recipe_wiki0_ToolStripMenuItem";
      this.open_recipe_wiki0_ToolStripMenuItem.Size = new Size(351, 22);
      this.open_recipe_wiki0_ToolStripMenuItem.Text = "レシピの詳細をレシピ情報wikiで調べる";
      this.open_recipe_wiki0_ToolStripMenuItem.Click += new EventHandler(this.open_recipe_wiki0_ToolStripMenuItem_Click);
      this.open_recipe_wiki1_ToolStripMenuItem.Name = "open_recipe_wiki1_ToolStripMenuItem";
      this.open_recipe_wiki1_ToolStripMenuItem.Size = new Size(351, 22);
      this.open_recipe_wiki1_ToolStripMenuItem.Text = "レシピで作成可能かどうかレシピ情報wikiで調べる";
      this.open_recipe_wiki1_ToolStripMenuItem.Click += new EventHandler(this.open_recipe_wiki1_ToolStripMenuItem_Click);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new Size(348, 6);
      this.copy_name_to_clipboardToolStripMenuItem.Name = "copy_name_to_clipboardToolStripMenuItem";
      this.copy_name_to_clipboardToolStripMenuItem.Size = new Size(351, 22);
      this.copy_name_to_clipboardToolStripMenuItem.Text = "名称をクリップボ\x30FCドにコピ\x30FC";
      this.copy_name_to_clipboardToolStripMenuItem.Click += new EventHandler(this.copy_name_to_clipboardToolStripMenuItem_Click);
      this.copy_all_to_clipboardToolStripMenuItem.Name = "copy_all_to_clipboardToolStripMenuItem";
      this.copy_all_to_clipboardToolStripMenuItem.Size = new Size(351, 22);
      this.copy_all_to_clipboardToolStripMenuItem.Text = "詳細をクリップボ\x30FCドにコピ\x30FC";
      this.copy_all_to_clipboardToolStripMenuItem.Click += new EventHandler(this.copy_all_to_clipboardToolStripMenuItem_Click);
      this.tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControl1.Controls.Add((Control) this.tabPage1);
      this.tabControl1.Controls.Add((Control) this.tabPage3);
      this.tabControl1.Controls.Add((Control) this.tabPage2);
      this.tabControl1.Location = new Point(12, 12);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(428, 259);
      this.tabControl1.TabIndex = 8;
      this.tabPage1.BackColor = Color.Transparent;
      this.tabPage1.Controls.Add((Control) this.comboBox4);
      this.tabPage1.Controls.Add((Control) this.comboBox3);
      this.tabPage1.Controls.Add((Control) this.comboBox2);
      this.tabPage1.Controls.Add((Control) this.button3);
      this.tabPage1.Controls.Add((Control) this.listView1);
      this.tabPage1.Controls.Add((Control) this.label2);
      this.tabPage1.Controls.Add((Control) this.comboBox1);
      this.tabPage1.Controls.Add((Control) this.button1);
      this.tabPage1.Location = new Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(420, 233);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "検索";
      this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(373, 10);
      this.label2.Name = "label2";
      this.label2.Size = new Size(41, 12);
      this.label2.TabIndex = 28;
      this.label2.Text = "0000件";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.button1.Location = new Point(262, 6);
      this.button1.Name = "button1";
      this.button1.Size = new Size(101, 24);
      this.button1.TabIndex = 1;
      this.button1.Text = "検索";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.tabPage3.BackColor = Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.tabPage3.Controls.Add((Control) this.button5);
      this.tabPage3.Controls.Add((Control) this.listView3);
      this.tabPage3.Location = new Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Padding = new Padding(3);
      this.tabPage3.Size = new Size(420, 233);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "文化圏";
      this.button5.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button5.DialogResult = DialogResult.OK;
      this.button5.Location = new Point(203, 204);
      this.button5.Name = "button5";
      this.button5.Size = new Size(211, 23);
      this.button5.TabIndex = 28;
      this.button5.Text = "スポット表示";
      this.button5.UseVisualStyleBackColor = true;
      this.button5.Click += new EventHandler(this.button5_Click);
      this.tabPage2.BackColor = Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.tabPage2.Controls.Add((Control) this.label3);
      this.tabPage2.Controls.Add((Control) this.label1);
      this.tabPage2.Controls.Add((Control) this.button2);
      this.tabPage2.Controls.Add((Control) this.listView2);
      this.tabPage2.Location = new Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new Padding(3);
      this.tabPage2.Size = new Size(420, 233);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "スポット表示";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 10);
      this.label3.Name = "label3";
      this.label3.Size = new Size(35, 12);
      this.label3.TabIndex = 31;
      this.label3.Text = "label3";
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(373, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(41, 12);
      this.label1.TabIndex = 30;
      this.label1.Text = "0000件";
      this.label1.TextAlign = ContentAlignment.MiddleRight;
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button2.DialogResult = DialogResult.OK;
      this.button2.Location = new Point(203, 204);
      this.button2.Name = "button2";
      this.button2.Size = new Size(211, 23);
      this.button2.TabIndex = 28;
      this.button2.Text = "スポット表示解除";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.button4.DialogResult = DialogResult.Cancel;
      this.button4.Location = new Point(177, 241);
      this.button4.Name = "button4";
      this.button4.Size = new Size(37, 15);
      this.button4.TabIndex = 9;
      this.button4.TabStop = false;
      this.button4.Text = "button4";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new EventHandler(this.button4_Click);
      this.AcceptButton = (IButtonControl) this.button1;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.button4;
      this.ClientSize = new Size(452, 283);
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.button4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new Size(468, 320);
      this.Name = "find_form2";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.Manual;
      this.Text = "できるだけ検索";
      this.Activated += new EventHandler(this.find_form2_Activated);
      this.FormClosing += new FormClosingEventHandler(this.find_form2_FormClosing);
      this.contextMenuStrip1.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabPage3.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.tabPage2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
