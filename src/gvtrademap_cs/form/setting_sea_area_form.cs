/*-------------------------------------------------------------------------

 危険海域変動システム設定

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Utility;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public partial class setting_sea_area_form : Form
	{
		const string						TEXT0	= "危険海域(通常状態)";
		const string						TEXT1	= "安全海域";
		const string						TEXT2	= "無法海域";
	
		private sea_area					m_sea_area;
	
		private ListViewItem				m_li;
		private int							m_X=0;
		private int							m_Y=0;

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public setting_sea_area_form(sea_area area)
		{
			m_sea_area		= area;
	
			InitializeComponent();
			Useful.SetFontMeiryo(this, def.MEIRYO_POINT);

			// tooltip
			toolTip1.AutoPopDelay		= 30*1000;		// 30秒表示
			toolTip1.BackColor			= Color.LightYellow;

			comboBox1.Hide();

			listView1.Columns.Add("海域群",	200);
			listView1.Columns.Add("状態",	150);

			foreach(sea_area.sea_area_once g in m_sea_area.groups){
				string	str;
				if(g.type == sea_area.sea_area_once.sea_type.normal){
					str		= TEXT0;
				}else if(g.type == sea_area.sea_area_once.sea_type.safty){
					str		= TEXT1;
				}else{
					str		= TEXT2;
				}
				add_item(g.name, str, g.GetToolTipString());
			}
		}

		/*-------------------------------------------------------------------------
		 追加する
		---------------------------------------------------------------------------*/
		private void add_item(string str, string type, string tooltipstring)
		{
			ListViewItem	item	= new ListViewItem(str, 0);
			item.SubItems.Add(type);
			item.ToolTipText		= tooltipstring;
			listView1.Items.Add(item);
		}

		/*-------------------------------------------------------------------------
		 選択状態が変更された
		---------------------------------------------------------------------------*/
		private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			ajust_combobox_size();

			if(!e.IsSelected){
				comboBox1.Hide();
				return;
			}
			comboBox1.Location	= new Point(2 + listView1.Columns[0].Width + listView1.Location.X, m_li.Position.Y + listView1.Location.Y);
			comboBox1.Size		= new Size(listView1.Columns[1].Width, m_li.Bounds.Height);
			string	str			= m_li.SubItems[1].Text;
			comboBox1.Text		= str;
			comboBox1.Show();
		}

		/*-------------------------------------------------------------------------
		 コンボボックスのサイズを調整する
		 選択されていないときは非表示にされる
		---------------------------------------------------------------------------*/
		private void ajust_combobox_size()
		{
			if(listView1.SelectedItems.Count <= 0){
				comboBox1.Hide();
				return;
			}
			comboBox1.Location	= new Point(2 + listView1.Columns[0].Width + listView1.Location.X, m_li.Position.Y + listView1.Location.Y);
			comboBox1.Size		= new Size(listView1.Columns[1].Width, m_li.Bounds.Height);
		}

		/*-------------------------------------------------------------------------
		 マウスが押された
		---------------------------------------------------------------------------*/
		private void listView1_MouseDown(object sender, MouseEventArgs e)
		{
			m_li	= listView1.GetItemAt(e.X , e.Y);
			m_X		= e.X;
			m_Y		= e.Y;
		}

		/*-------------------------------------------------------------------------
		 コンボボックス
		 選択された
		---------------------------------------------------------------------------*/
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_li.SubItems[1].Text	= comboBox1.Text;
		}

		/*-------------------------------------------------------------------------
		 閉じられた
		---------------------------------------------------------------------------*/
		private void setting_sea_area_form_FormClosed(object sender, FormClosedEventArgs e)
		{
		}

		/*-------------------------------------------------------------------------
		 更新する
		---------------------------------------------------------------------------*/
		public void Update(sea_area area)
		{
			foreach(ListViewItem d in listView1.Items){
				sea_area.sea_area_once.sea_type		type;
				if(d.SubItems[1].Text == TEXT0){
					type	= sea_area.sea_area_once.sea_type.normal;
				}else if(d.SubItems[1].Text == TEXT1){
					type	= sea_area.sea_area_once.sea_type.safty;
				}else{
					type	= sea_area.sea_area_once.sea_type.lawless;
				}
				m_sea_area.SetType(d.SubItems[0].Text, type);
			}
		}

		/*-------------------------------------------------------------------------
		 初期化ボタンが押された
		---------------------------------------------------------------------------*/
		private void button3_Click(object sender, EventArgs e)
		{
			foreach(ListViewItem d in listView1.Items){
				d.SubItems[1].Text	= TEXT0;
			}
			if(comboBox1.Visible){
				comboBox1.Text		= TEXT0;
			}
		}


		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
		{
			ajust_combobox_size();
		}
		private void listView1_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			ajust_combobox_size();
		}

		/*-------------------------------------------------------------------------
		 海域情報収集を起動する
		---------------------------------------------------------------------------*/
		private void button4_Click(object sender, EventArgs e)
		{
			try{
				Process.Start(def.SEAAREA_APP_FNAME);
				button2.PerformClick();		// キャンセル扱い
			}catch{
				MessageBox.Show("海域情報収集の起動に失敗しました。", "起動エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
