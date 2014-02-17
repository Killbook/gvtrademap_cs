/*-------------------------------------------------------------------------

 できるだけ検索
 モードレスダイアログとして使用すること

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
using System.Collections;

using Utility;
using Utility.Ctrl;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public partial class find_form2 : Form
	{
		private const int				LIST_MAX = 2000;		// リストに表示する最大
	
		private gvt_lib					m_lib;					// よく使う機能をまとめたもの
		private GvoDatabase				m_db;					// データベース
		private spot					m_spot;					// スポット一覧
		private item_window				m_item_window;			// アイテムウインドウ
		private Point					m_gpos;

		private ListViewItemSorter		m_sorter;				// ソート
		private bool					m_now_find;
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public find_form2(gvt_lib lib, GvoDatabase db, spot _spot, item_window _item_window)
		{
			m_lib				= lib;
			m_db				= db;
			m_spot				= _spot;
			m_item_window		= _item_window;
			m_gpos				= new Point(-1, -1);

			m_sorter			= new ListViewItemSorter();

			InitializeComponent();

			Useful.SetFontMeiryo(this, def.MEIRYO_POINT);

			m_now_find			= false;

			// tooltip
			toolTip1.AutoPopDelay		= 30*1000;		// 30秒表示
			toolTip1.BackColor			= Color.LightYellow;

			// 各ページの初期化
			init_page1();
			init_page2();
			init_page3();
		}

		/*-------------------------------------------------------------------------
		 検索ページの初期化
		---------------------------------------------------------------------------*/
		private void init_page1()
		{
			// フィルタ
			comboBox2.SelectedIndex		= (int)m_lib.setting.find_filter;
			comboBox3.SelectedIndex		= (int)m_lib.setting.find_filter2;
			comboBox4.SelectedIndex		= (int)m_lib.setting.find_filter3;
	
			// ヘッダのコラムを追加しておく
			// コラムのサイズは覚えておく必要がある
			listView1.Columns.Add("一致名",	180);
			listView1.Columns.Add("種類",	80);
			listView1.Columns.Add("場所",	120);
	
			// 検索文字列を更新
			update_find_strings();
			// 検索結果を更新
			update_find_result(null);
			// スポットボタンの状態更新
			update_spot_button_status();
		}

		/*-------------------------------------------------------------------------
		 スポット表示一覧ページの初期化
		---------------------------------------------------------------------------*/
		private void init_page2()
		{
			// ヘッダのコラムを追加しておく
			// コラムのサイズは覚えておく必要がある
			listView2.Columns.Add("場所",	120);
			listView2.Columns.Add("種類",	150);
			listView2.Columns.Add("",	100);

			// スポット一覧の更新
			update_spot_list();
		}
		
		/*-------------------------------------------------------------------------
		 文化圏ページの初期化
		---------------------------------------------------------------------------*/
		private void init_page3()
		{
			// ヘッダのコラムを追加しておく
			// コラムのサイズは覚えておく必要がある
			listView3.Columns.Add("名前",	180);
			listView3.Columns.Add("種類",	80);
			listView3.Columns.Add(" ",	120);

			// リストの更新
			update_cultural_sphere_list();

			// ボタンの更新
			update_cultural_sphere_button();
		}

		/*-------------------------------------------------------------------------
		 検索ボタンをクリック
		---------------------------------------------------------------------------*/
		private void button1_Click(object sender, EventArgs e)
		{
			do_find();
		}

		/*-------------------------------------------------------------------------
		 検索
		---------------------------------------------------------------------------*/
		private void do_find()
		{
			string		find_string	= comboBox1.Text;
	
			// 空白チェック
			if(find_string == "")	return;

			m_now_find		= true;

			// 履歴に追加
			m_lib.setting.find_strings.Add(find_string);

			// 座標検索判定
			if(do_centering_gpos(find_string)){
				// 検索結果をクリア
				listView1.Items.Clear();		// アイテム内容をクリア
												// ヘッダコラムはクリアされない

				// センタリングリクエスト
				m_lib.setting.centering_gpos	= m_gpos;
				m_lib.setting.req_centering_gpos.Request();
			}else{
				this.Cursor	= Cursors.WaitCursor;
				// 検索
				List<GvoDatabase.Find>	results	= m_db.FindAll(find_string);

				// 検索文字列を更新
				update_find_strings();
				// 検索結果を更新
				update_find_result(results);
				// スポットボタンの状態更新
				update_spot_button_status();
				this.Cursor	= Cursors.Default;
			}

			m_now_find		= false;
		}
	
		/*-------------------------------------------------------------------------
		 検索履歴を更新
		---------------------------------------------------------------------------*/
		private void update_find_strings()
		{
			// 検索ボックス
			comboBox1.DropDownHeight		= 200;
			// 履歴をクリア
			comboBox1.Items.Clear();
			if(m_lib.setting.find_strings.Count > 0){
				foreach(string s in m_lib.setting.find_strings){
					comboBox1.Items.Add(s);
				}
				// 一番上を選択
				comboBox1.SelectedIndex		= 0;
			}
		}

		/*-------------------------------------------------------------------------
		 検索結果を更新
		---------------------------------------------------------------------------*/
		private void update_find_result(List<GvoDatabase.Find> results)
		{
			listView1.BeginUpdate();
			listView1.Items.Clear();		// アイテム内容をクリア
											// ヘッダコラムはクリアされない

			bool	is_overflow	= false;
			if(results != null){
				foreach(GvoDatabase.Find i in results){
					add_item(listView1, i);
					if(listView1.Items.Count >= LIST_MAX){
						is_overflow		= true;
						break;
					}
				}
			}
			listView1.EndUpdate();

			if(results != null){
//				label2.Text				= String.Format("{0}件", listView1.Items.Count);
				label2.Text				= String.Format("{0}件", results.Count);
			}else{
				label2.Text				= "0件";
			}

			if(is_overflow){
				MessageBox.Show(this, LIST_MAX.ToString() + "件以上はスキップされました。", "検索結果が多すぎます");
			}
		}

		/*-------------------------------------------------------------------------
		 追加する
		---------------------------------------------------------------------------*/
		private void add_item(ListView listview, GvoDatabase.Find i)
		{
			// フィルタ
			if(i.Type != GvoDatabase.Find.FindType.CulturalSphere){
				// 文化圏以外のとき
				switch(m_lib.setting.find_filter){
				case _find_filter.world_info:
					if(i.Type == GvoDatabase.Find.FindType.Database)	return;
					break;
				case _find_filter.item_database:
					if(i.Type != GvoDatabase.Find.FindType.Database)	return;
					break;
				case _find_filter.both:
				default:
					break;
				}
			}
			
			ListViewItem	item	= new ListViewItem(i.NameString, 0);
			item.Tag				= i;
			item.ToolTipText		= i.TooltipString;
			item.SubItems.Add(i.TypeString);
			item.SubItems.Add(i.SpotString);

			listview.Items.Add(item);
		}

		/*-------------------------------------------------------------------------
		 座標の入力をチェックする
		 座標が入力されていればセンタリングする
		---------------------------------------------------------------------------*/
		private bool do_centering_gpos(string str)
		{
			try{
				str	= str.Replace('０', '0');
				str	= str.Replace('１', '1');
				str	= str.Replace('２', '2');
				str	= str.Replace('３', '3');
				str	= str.Replace('４', '4');
				str	= str.Replace('５', '5');
				str	= str.Replace('６', '6');
				str	= str.Replace('７', '7');
				str	= str.Replace('８', '8');
				str	= str.Replace('９', '9');
				str	= str.Replace('、', ',');
				str	= str.Replace('，', ',');
				str	= str.Replace('.', ',');
				str	= str.Replace('。', ',');
				str	= str.Replace('．', ',');

				string[] split	= str.Split(new char[]{','});
				if(split.Length != 2)	return false;

				Point	gpos	= new Point(Convert.ToInt32(split[0]),
											Convert.ToInt32(split[1]));
				if(gpos.X < 0)			return false;
				if(gpos.Y < 0)			return false;

				// 中心にする座標
				m_gpos			= gpos;
				return true;
			}catch{
				return false;
			}
		}

		/*-------------------------------------------------------------------------
		 選択されているアイテムのTAGを得る
		---------------------------------------------------------------------------*/
		private GvoDatabase.Find get_selected_item_tag()
		{
			return get_selected_item_tag(listView1);
		}

		/*-------------------------------------------------------------------------
		 選択されているアイテムのTAGを得る
		---------------------------------------------------------------------------*/
		private GvoDatabase.Find get_selected_item_tag(ListView view)
		{
			if(view.SelectedItems.Count <= 0)		return null;
			ListViewItem	item	= view.SelectedItems[0];
			if(item.Tag == null)					return null;
			return (GvoDatabase.Find)item.Tag;
		}
	
		/*-------------------------------------------------------------------------
		 閉じられようとしている
		---------------------------------------------------------------------------*/
		private void find_form2_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason == CloseReason.UserClosing){
				// このフォームの x ボタンが押されたときのみキャンセルする
				// Alt + F4 でもここを通る
				e.Cancel	= true;
				this.Hide();	// 非表示にする
			}
		}

		/*-------------------------------------------------------------------------
		 ダブルクリックされた
		---------------------------------------------------------------------------*/
		private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			button3.PerformClick();
		}

		/*-------------------------------------------------------------------------
		 スポットボタンの状態更新
		---------------------------------------------------------------------------*/
		private void update_spot_button_status()
		{
			GvoDatabase.Find	tag		= get_selected_item_tag();
			if(tag == null){
				button3.Enabled		= false;
			}else{
				if(tag.Type == GvoDatabase.Find.FindType.Database){
					button3.Enabled		= false;
				}else{
					button3.Enabled		= true;
				}
			}
		}
	
		/*-------------------------------------------------------------------------
		 選択状態が更新された
		---------------------------------------------------------------------------*/
		private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			update_spot_button_status();
		}
	
		/*-------------------------------------------------------------------------
		 アクティブにされた
		 フォーカスを検索ボックスに移す
		---------------------------------------------------------------------------*/
		private void find_form2_Activated(object sender, EventArgs e)
		{
			ActiveControl	= comboBox1;
		}
	
		/*-------------------------------------------------------------------------
		 選択されたアイテムをスポット表示ボタンが押された
		---------------------------------------------------------------------------*/
		private void button3_Click(object sender, EventArgs e)
		{
			do_spot(get_selected_item_tag());
		}

		/*-------------------------------------------------------------------------
		 スポット表示する
		---------------------------------------------------------------------------*/
		private void do_spot(GvoDatabase.Find item)
		{
			if(item == null)		return;

			// スポット
			m_lib.setting.req_spot_item.Request(item);
		}
	
		/*-------------------------------------------------------------------------
		 フィルタ変更
		---------------------------------------------------------------------------*/
		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_lib.setting.find_filter	= _find_filter.world_info + comboBox2.SelectedIndex;
			do_find();
		}
		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_lib.setting.find_filter2	= _find_filter2.name + comboBox3.SelectedIndex;
			do_find();
		}
		private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_lib.setting.find_filter3	= _find_filter3.full_text_search + comboBox4.SelectedIndex;
			do_find();
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(m_now_find)	return;
			do_find();
		}

		/*-------------------------------------------------------------------------
		 リスト内で右クリックされた
		---------------------------------------------------------------------------*/
		private void listView1_MouseClick(object sender, MouseEventArgs e)
		{
			if((e.Button & MouseButtons.Right) == 0)	return;		// 右クリックのみ

			GvoDatabase.Find	tag		= get_selected_item_tag();
			if(tag == null)								return;

			// 表示位置調整
			Point		pos	= new Point(e.X, e.Y);

			ItemDatabaseCustom.Data			db		= tag.Database;
			if(db == null){
				// アイテムデータベースと一致しない
				open_recipe_wiki0_ToolStripMenuItem.Enabled		= false;
				open_recipe_wiki1_ToolStripMenuItem.Enabled		= false;
				copy_all_to_clipboardToolStripMenuItem.Enabled	= false;
			}else{
				copy_all_to_clipboardToolStripMenuItem.Enabled	= true;
				if(db.IsSkill || db.IsReport){
					// スキルか報告
					open_recipe_wiki0_ToolStripMenuItem.Enabled	= false;
					open_recipe_wiki1_ToolStripMenuItem.Enabled	= false;
				}else{
					if(db.IsRecipe){
						// レシピ
						open_recipe_wiki0_ToolStripMenuItem.Enabled	= true;
						open_recipe_wiki1_ToolStripMenuItem.Enabled	= false;
					}else{
						// レシピ以外
						open_recipe_wiki0_ToolStripMenuItem.Enabled	= false;
						open_recipe_wiki1_ToolStripMenuItem.Enabled	= true;
					}
				}
			}
			contextMenuStrip1.Show(listView1, pos);
		}

		/*-------------------------------------------------------------------------
		 レシピ情報wikiを開く
		 レシピ検索
		---------------------------------------------------------------------------*/
		private void open_recipe_wiki0_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GvoDatabase.Find	tag		= get_selected_item_tag();
			if(tag == null)										return;
			if(tag.Database == null)							return;
			tag.Database.OpenRecipeWiki0();
		}

		/*-------------------------------------------------------------------------
		 レシピ情報wikiを開く
		 作成可能かどうか検索
		---------------------------------------------------------------------------*/
		private void open_recipe_wiki1_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GvoDatabase.Find	tag		= get_selected_item_tag();
			if(tag == null)										return;
			if(tag.Database == null)							return;
			tag.Database.OpenRecipeWiki1();
		}

		/*-------------------------------------------------------------------------
		 名称をクリップボードにコピーする
		---------------------------------------------------------------------------*/
		private void copy_name_to_clipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GvoDatabase.Find	tag		= get_selected_item_tag();
			if(tag == null)										return;
			Clipboard.SetText(tag.NameString);
		}

		/*-------------------------------------------------------------------------
		 詳細をクリップボードにコピーする
		---------------------------------------------------------------------------*/
		private void copy_all_to_clipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GvoDatabase.Find	tag		= get_selected_item_tag();
			if(tag == null)										return;
			if(tag.Data != null){
				Clipboard.SetText(tag.Data.TooltipString);
			}else if(tag.Database != null){
				Clipboard.SetText(tag.Database.GetToolTipString());
			}
		}

		/*-------------------------------------------------------------------------
		 ヘッダをクリックされた
		---------------------------------------------------------------------------*/
		private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			// ソートする
			m_sorter.Sort(listView1, e.Column);
		}

		/*-------------------------------------------------------------------------
		 検索モードにする
		---------------------------------------------------------------------------*/
		public void SetFindMode()
		{
			// 検索をアクティブにする
			tabControl1.SelectedIndex	= 0;
		}

		/*-------------------------------------------------------------------------
		 スポット結果の更新
		 スポット一覧をアクティブにする
		---------------------------------------------------------------------------*/
		public void UpdateSpotList()
		{
			if(m_spot.is_spot){
				// スポット一覧をアクティブにする
				tabControl1.SelectedIndex	= 2;
			}else{
				// 検索をアクティブにする
				SetFindMode();
			}
	
			// 一覧の更新
			update_spot_list();
		}

		/*-------------------------------------------------------------------------
		 スポット結果の更新
		---------------------------------------------------------------------------*/
		private void update_spot_list()
		{
			listView2.BeginUpdate();
			listView2.Items.Clear();

			List<spot.spot_once>	list	= m_spot.list;

			label1.Text					= String.Format("{0}件", list.Count);
			label3.Text					= m_spot.spot_type_str;
			listView2.Columns[2].Text	= m_spot.spot_type_column_str;
			
			// リストの追加
			foreach(spot.spot_once s in list){
				ListViewItem	item	= new ListViewItem(s.info.Name, 0);
				item.Tag				= s;
				item.SubItems.Add(s.name);
				item.SubItems.Add(s.ex);

				listView2.Items.Add(item);
			}
			listView2.EndUpdate();

			// ボタンの有効無効
			button2.Enabled			= (list.Count <= 0)? false: true;

			// 選択を設定する
			update_select_info();
		}

		/*-------------------------------------------------------------------------
		 選択されているinfoを選択する
		---------------------------------------------------------------------------*/
		private void update_select_info()
		{
			GvoWorldInfo.Info info	= m_item_window.info;

			if(info != null){
				foreach(ListViewItem i in listView2.Items){
					object tag			= i.Tag;
					if(tag == null)		continue;

					spot.spot_once	s	= (spot.spot_once)tag;
					if(s.info.Name != info.Name)	continue;

					// 選択状態にし、見える位置にスクロールさせる
					i.Selected	= true;
					i.EnsureVisible();
					i.Focused	= true;
					return;
				}
			}

			// 選択中のinfoが含まれない場合は1番目を選択状態にする	
			// 文化圏のスポット時
			if(listView2.Items.Count > 0){
				listView2.Items[0].Selected		= true;
			}
		}

		/*-------------------------------------------------------------------------
		 スポット結果の選択が更新された
		---------------------------------------------------------------------------*/
		private void listView2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(listView2.SelectedIndices.Count <= 0)	return;
			if(listView2.SelectedItems[0].Tag == null)	return;
			spot.spot_once	s	= listView2.SelectedItems[0].Tag as spot.spot_once;
			if(s == null)								return;

			m_lib.setting.req_spot_item_changed.Request(s);
		}

		/*-------------------------------------------------------------------------
		 スポット結果のコラムがクリックされた
		 ソートする
		---------------------------------------------------------------------------*/
		private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			// ソートする
			m_sorter.Sort(listView2, e.Column);
		}

		/*-------------------------------------------------------------------------
		 スポット表示解除
		---------------------------------------------------------------------------*/
		private void button2_Click(object sender, EventArgs e)
		{
			m_lib.setting.req_spot_item.Request(null);
		}

		/*-------------------------------------------------------------------------
		 ESCが押されたときの閉じる動作
		---------------------------------------------------------------------------*/
		private void button4_Click(object sender, EventArgs e)
		{
			this.Hide();		// 表示を消す
		}


		/*-------------------------------------------------------------------------
		 文化圏をスポット表示するボタンを更新する
		---------------------------------------------------------------------------*/
		private void update_cultural_sphere_button()
		{
			button5.Enabled	= (listView3.SelectedIndices.Count <= 0)? false: true;
		}

		/*-------------------------------------------------------------------------
		 文化圏リストの更新
		 途中で更新されないため、初期化時のみ
		---------------------------------------------------------------------------*/
		private void update_cultural_sphere_list()
		{
			List<GvoDatabase.Find>	results	= m_db.GetCulturalSphereList();

			listView3.BeginUpdate();
			listView3.Items.Clear();

			// リストの追加
			foreach(GvoDatabase.Find i in results){
				add_item(listView3, i);
			}
			listView3.EndUpdate();
		}

		/*-------------------------------------------------------------------------
		 文化圏をスポット表示する
		---------------------------------------------------------------------------*/
		private void button5_Click(object sender, EventArgs e)
		{
			do_spot(get_selected_item_tag(listView3));
		}

		/*-------------------------------------------------------------------------
		 文化圏の選択更新
		---------------------------------------------------------------------------*/
		private void listView3_SelectedIndexChanged(object sender, EventArgs e)
		{
			// ボタンの更新
			update_cultural_sphere_button();
		}

		/*-------------------------------------------------------------------------
		 カラムクリック
		 ソートする
		---------------------------------------------------------------------------*/
		private void listView3_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			// ソートする
			m_sorter.Sort(listView3, e.Column);
		}

		/*-------------------------------------------------------------------------
		 文化圏リストがダブルクリックされた
		---------------------------------------------------------------------------*/
		private void listView3_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			button5.PerformClick();
		}
	}
}
