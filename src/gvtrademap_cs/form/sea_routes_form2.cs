/*-------------------------------------------------------------------------

 航路図一覧
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

using directx;
using gvo_base;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Utility;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public partial class sea_routes_form2 : Form
	{
		private gvt_lib					m_lib;					// よく使う機能をまとめたもの
		private GvoDatabase				m_db;					// データベース

		private bool					m_disable_update_select;

		private list_view_db			m_view1;
		private list_view_db			m_view2;
		private list_view_db			m_view3;

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public sea_routes_form2(gvt_lib lib, GvoDatabase db)
		{
			m_lib		= lib;
			m_db		= db;
	
			InitializeComponent();

			Useful.SetFontMeiryo(this, def.MEIRYO_POINT);

			m_disable_update_select		= false;

			// tooltip
			toolTip1.AutoPopDelay		= 30*1000;		// 30秒表示
			toolTip1.BackColor			= Color.LightYellow;

			// ViewとDBを関連付けておく
			m_view1		= new list_view_db(listView1, m_db.SeaRoute.searoutes);
			m_view2		= new list_view_db(listView2, m_db.SeaRoute.favorite_sea_routes);
			m_view3		= new list_view_db(listView3, m_db.SeaRoute.trash_sea_routes);

			// 各ページの初期化
			init_page1();
			init_page2();
			init_page3();
		}

		/*-------------------------------------------------------------------------
		 ページの初期化
		---------------------------------------------------------------------------*/
		private void init_page1()
		{
			// ヘッダのコラムを追加しておく
			// コラムのサイズは覚えておく必要がある
			listView1.Columns.Add("表示",	50);
			listView1.Columns.Add("開始位置",	160);
			listView1.Columns.Add("終了位置",	160);
			listView1.Columns.Add("航海日数",	60);
			listView1.Columns.Add("航海日時",	170);
		}

		/*-------------------------------------------------------------------------
		 ページの初期化
		---------------------------------------------------------------------------*/
		private void init_page2()
		{
			// ヘッダのコラムを追加しておく
			// コラムのサイズは覚えておく必要がある
			listView2.Columns.Add("表示",	50);
			listView2.Columns.Add("開始位置",	160);
			listView2.Columns.Add("終了位置",	160);
			listView2.Columns.Add("航海日数",	60);
			listView2.Columns.Add("航海日時",	170);
		}

		/*-------------------------------------------------------------------------
		 ページの初期化
		---------------------------------------------------------------------------*/
		private void init_page3()
		{
			// ヘッダのコラムを追加しておく
			// コラムのサイズは覚えておく必要がある
			listView3.Columns.Add("表示",	50);
			listView3.Columns.Add("開始位置",	160);
			listView3.Columns.Add("終了位置",	160);
			listView3.Columns.Add("航海日数",	60);
			listView3.Columns.Add("航海日時",	170);
		}

		/*-------------------------------------------------------------------------
		 ESCが押されたときの閉じる動作
		---------------------------------------------------------------------------*/
		private void button1_Click(object sender, EventArgs e)
		{
			this.Hide();		// 表示を消す
		}

		/*-------------------------------------------------------------------------
		 閉じられようとしている
		---------------------------------------------------------------------------*/
		private void sea_routes_form2_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason == CloseReason.UserClosing){
				// このフォームの x ボタンが押されたときのみキャンセルする
				// Alt + F4 でもここを通る
				e.Cancel	= true;
				this.Hide();	// 非表示にする

				// 選択状態をリセットする
				m_db.SeaRoute.ResetSelectFlag();
			}
		}

		/*-------------------------------------------------------------------------
		 最初に表示されたとき
		---------------------------------------------------------------------------*/
		private void sea_routes_form2_Shown(object sender, EventArgs e)
		{
			UpdateAllList();
		}

		/*-------------------------------------------------------------------------
		 全てのリスト内容を更新する
		 リストの数が変わった場合
		---------------------------------------------------------------------------*/
		public void UpdateAllList()
		{
			update_sea_routes_list();
			update_favorite_sea_routes_list();
			update_trash_sea_routes_list();
		}

		/*-------------------------------------------------------------------------
		 最新の航路図の内容を更新する
		 変更があったであろう箇所のみ再描画
		 全体を再描画するとちらつくため
		---------------------------------------------------------------------------*/
		public void RedrawNewestSeaRoutes()
		{
			// 航路図一覧以外を更新する必要が今のところない
			switch(tabControl1.SelectedIndex){
			case 0:
				{
					// 最新の航路図の内容を更新する
					int count	= m_view1.view.Items.Count;
					if(count <= 0)		return;
					Rectangle	rect	= m_view1.view.GetItemRect(count - 1);
					m_view1.view.Invalidate(rect);
					m_view1.view.Update();
				}
				break;
			case 1:
			case 2:
				break;
			}
		}
	
		/*-------------------------------------------------------------------------
		 航路図一覧リスト内容を更新する
		---------------------------------------------------------------------------*/
		private void update_sea_routes_list()
		{
			tabPage1.Text					= String.Format("航路図({0})", m_db.SeaRoute.searoutes.Count);
			update_list_count(m_view1);
		}

		/*-------------------------------------------------------------------------
		 お気に入り航路図一覧リスト内容を更新する
		---------------------------------------------------------------------------*/
		private void update_favorite_sea_routes_list()
		{
			tabPage2.Text					= String.Format("お気に入り航路({0})", m_db.SeaRoute.favorite_sea_routes.Count);
			update_list_count(m_view2);
		}

		/*-------------------------------------------------------------------------
		 ごみ箱航路図一覧リスト内容を更新する
		---------------------------------------------------------------------------*/
		private void update_trash_sea_routes_list()
		{
			tabPage3.Text					= String.Format("過去の航路図(非表示)({0})", m_db.SeaRoute.trash_sea_routes.Count);
			update_list_count(m_view3);
		}

		/*-------------------------------------------------------------------------
		 リストの数を更新する
		 一番上に表示されていたアイテムを描画範囲内にスクロールさせる機能付き
		---------------------------------------------------------------------------*/
		private void update_list_count(list_view_db view)
		{
			ListViewItem	item	= view.view.TopItem;
			int				index	= -1;
			if(item != null)	index = item.Index;

			// 一番下までスクロールした状態で項目数を減らすと例外がでる
			// ListViewのバグ？
			view.view.VirtualListSize	= 0;				// 例外対策
			view.view.VirtualListSize	= view.db.Count;	// 新しい項目数

/*			// 見える位置にスクロールさせる
			if(   (view.view.Items.Count > 0)
				&&(Index > 0) ){
				// 一度一番下までスクロールさせる
				view.view.EnsureVisible(view.view.Items.Count - 1);
				// 目的のアイテムを一番上にする
				// 一番上が無理なら適当な位置にスクロールする
				view.view.EnsureVisible(Index);
			}
*/

/*			int Count	= m_view1.view.Items.Count;
			if(Count > 0){
				Rectangle	rect	= view.view.GetItemRect(Count - 1);	// もっとも新しいアイテムのRect
				view.view.Invalidate(view.view.RectangleToScreen(rect));
			}
*/	
			// 一番下が見える位置にスクロールさせる	
			// 一番下の情報が最新のもの
			if(view.view.Items.Count > 0){
				// 一度一番下までスクロールさせる
				view.view.EnsureVisible(view.view.Items.Count - 1);
			}
		}

		/*-------------------------------------------------------------------------
		 航路一覧が選択された
		---------------------------------------------------------------------------*/
		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			selected_index_changed(m_view1);
		}
		private void listView2_SelectedIndexChanged(object sender, EventArgs e)
		{
			selected_index_changed(m_view2);
		}
		private void listView3_SelectedIndexChanged(object sender, EventArgs e)
		{
			selected_index_changed(m_view3);
		}

		/*-------------------------------------------------------------------------
		 航路一覧が選択された
		 sub
		---------------------------------------------------------------------------*/
		private void selected_index_changed(list_view_db view)
		{
			if(m_disable_update_select)			return;

			// 選択状態をリセットする
			m_db.SeaRoute.ResetSelectFlag();

			if(view.view.SelectedIndices.Count < 1)	return;

			// 最初の項目をセンタリングする
			SeaRoutes.Voyage	i	= get_route(view.db, view.view.SelectedIndices[0]);
			if(i != null){
				if(i.GamePoint1st.X < 0)	return;
				if(i.GamePoint1st.Y < 0)	return;
				m_lib.setting.centering_gpos	= i.GamePoint1st;
				m_lib.setting.req_centering_gpos.Request();
			}

			// 選択状態にする
			foreach(int index in view.view.SelectedIndices){
				SeaRoutes.Voyage	ii	= get_route(view.db, index);
				if(ii == null)	continue;
				ii.IsSelected	= true;
			}
		}

		/*-------------------------------------------------------------------------
		 指定された番号の航路図を得る
		 範囲外のときはnullを返す
		---------------------------------------------------------------------------*/
		private SeaRoutes.Voyage get_route(List<SeaRoutes.Voyage> list, int index)
		{
			if(list == null)		return null;
			if(index < 0)			return null;
			if(index >= list.Count)	return null;
			return list[index];
		}
	
		/*-------------------------------------------------------------------------
		 表示
		---------------------------------------------------------------------------*/
		private void show_hide_SToolStripMenuItem_Click(object sender, EventArgs e)
		{
			set_draw_flag(m_view1, true);
			update_sea_routes_list();
		}
		private void toolStripMenuItem7_Click(object sender, EventArgs e)
		{
			set_draw_flag(m_view2, true);
			update_favorite_sea_routes_list();
		}

		/*-------------------------------------------------------------------------
		 非表示
		---------------------------------------------------------------------------*/
		private void hide_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			set_draw_flag(m_view1, false);
			update_sea_routes_list();
		}
		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
			set_draw_flag(m_view2, false);
			update_favorite_sea_routes_list();
		}

		/*-------------------------------------------------------------------------
		 表示、非表示
		 sub
		---------------------------------------------------------------------------*/
		private void set_draw_flag(list_view_db view, bool is_show)
		{
			// 選択状態をリセットする
			m_db.SeaRoute.ResetSelectFlag();

			if(view.view.SelectedIndices.Count < 1)	return;

			m_disable_update_select	= true;
			foreach(int index in view.view.SelectedIndices){
				SeaRoutes.Voyage	ii	= get_route(view.db, index);
				if(ii == null)	continue;
				ii.IsEnableDraw	= is_show;
			}
			m_disable_update_select	= false;
		}

		/*-------------------------------------------------------------------------
		 全て選択状態にする
		---------------------------------------------------------------------------*/
		private void all_select_AToolStripMenuItem_Click(object sender, EventArgs e)
		{
			select_all(m_view1);
		}
		private void toolStripMenuItem5_Click(object sender, EventArgs e)
		{
			select_all(m_view2);
		}
		private void toolStripMenuItem9_Click(object sender, EventArgs e)
		{
			select_all(m_view3);
		}

		/*-------------------------------------------------------------------------
		 全て選択状態にする
		 sub
		---------------------------------------------------------------------------*/
		private void select_all(list_view_db view)
		{
			m_disable_update_select	= true;
			for(int i=0; i<view.db.Count; i++){
				view.view.SelectedIndices.Add(i);
			}
			m_disable_update_select	= false;
			selected_index_changed(view);
		}

		/*-------------------------------------------------------------------------
		 選択状態を解除する
		---------------------------------------------------------------------------*/
		private void button3_Click(object sender, EventArgs e)
		{
			// 選択状態をリセットする
			m_db.SeaRoute.ResetSelectFlag();

			m_disable_update_select	= true;
			listView1.SelectedIndices.Clear();
			listView2.SelectedIndices.Clear();
			listView3.SelectedIndices.Clear();
			m_disable_update_select	= false;
		}

		/*-------------------------------------------------------------------------
		 削除
		 選択されている航路図を削除する
		---------------------------------------------------------------------------*/
		private void move_trash_RToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<SeaRoutes.Voyage>	list	= get_selected_routes_list(m_view1);
			if(list == null)	return;
			if(!ask_remove())	return;

			m_db.SeaRoute.RemoveSeaRoutes(list);
			m_db.SeaRoute.ResetSelectFlag();
			update_sea_routes_list();
		}
		private void toolStripMenuItem4_Click(object sender, EventArgs e)
		{
			List<SeaRoutes.Voyage>	list	= get_selected_routes_list(m_view2);
			if(list == null)	return;
			if(!ask_remove())	return;

			m_db.SeaRoute.RemoveFavoriteSeaRoutes(list);
			m_db.SeaRoute.ResetSelectFlag();
			update_favorite_sea_routes_list();
		}
		private void toolStripMenuItem8_Click(object sender, EventArgs e)
		{
			List<SeaRoutes.Voyage>	list	= get_selected_routes_list(m_view3);
			if(list == null)	return;
			if(!ask_remove())	return;

			m_db.SeaRoute.RemoveTrashSeaRoutes(list);
			m_db.SeaRoute.ResetSelectFlag();
			update_trash_sea_routes_list();
		}

		/*-------------------------------------------------------------------------
		 削除確認
		---------------------------------------------------------------------------*/
		private bool ask_remove()
		{
			if(MessageBox.Show("選択された航路図を削除します。\n削除すると元に戻せません。\nよろしいですか？",
								"航路図削除の確認",
								MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)	return false;
			return true;
		}

		/*-------------------------------------------------------------------------
		 選択されている航路図リストを得る
		 リストがないときはnullを返す
		---------------------------------------------------------------------------*/
		private List<SeaRoutes.Voyage> get_selected_routes_list(list_view_db view)
		{
			if(view.view.SelectedIndices.Count < 1)	return null;

			List<SeaRoutes.Voyage>	list	= new List<SeaRoutes.Voyage>();
			foreach(int index in view.view.SelectedIndices){
				SeaRoutes.Voyage	ii	= get_route(view.db, index);
				if(ii == null)	continue;
				list.Add(ii);
			}
			if(list.Count <= 0)	return null;
			return list;
		}

		/*-------------------------------------------------------------------------
		 航路図からお気に入りに移動
		---------------------------------------------------------------------------*/
		private void add_AToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<SeaRoutes.Voyage>	list	= get_selected_routes_list(m_view1);
			if(list == null)	return;
			m_db.SeaRoute.MoveSeaRoutesToFavoriteSeaRoutes(list);
			m_db.SeaRoute.ResetSelectFlag();
			UpdateAllList();
		}

		/*-------------------------------------------------------------------------
		 航路図から過去の航路図に移動
		---------------------------------------------------------------------------*/
		private void delete_DToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<SeaRoutes.Voyage>	list	= get_selected_routes_list(m_view1);
			if(list == null)	return;
			m_db.SeaRoute.MoveSeaRoutesToTrashSeaRoutes(list);
			m_db.SeaRoute.ResetSelectFlag();
			UpdateAllList();
		}

		/*-------------------------------------------------------------------------
		 お気に入りから過去の航路図に移動
		---------------------------------------------------------------------------*/
		private void toolStripMenuItem3_Click(object sender, EventArgs e)
		{
			List<SeaRoutes.Voyage>	list	= get_selected_routes_list(m_view2);
			if(list == null)	return;
			m_db.SeaRoute.MoveFavoriteSeaRoutesToTrashSeaRoutes(list);
			m_db.SeaRoute.ResetSelectFlag();
			UpdateAllList();
		}

		/*-------------------------------------------------------------------------
		 過去の航路図からお気に入りに移動
		---------------------------------------------------------------------------*/
		private void toolStripMenuItem6_Click(object sender, EventArgs e)
		{
			List<SeaRoutes.Voyage>	list	= get_selected_routes_list(m_view3);
			if(list == null)	return;
			m_db.SeaRoute.MoveTrashSeaRoutesToFavoriteSeaRoutes(list);
			m_db.SeaRoute.ResetSelectFlag();
			UpdateAllList();
		}

		/*-------------------------------------------------------------------------
		 項目の設定
		---------------------------------------------------------------------------*/
		private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			set_item(m_view1, e);
		}
		private void listView2_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			set_item(m_view2, e);
		}
		private void listView3_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			set_item(m_view3, e);
		}

		/*-------------------------------------------------------------------------
		 項目の設定
		---------------------------------------------------------------------------*/
		private void set_item(list_view_db view, RetrieveVirtualItemEventArgs e)
		{
			SeaRoutes.Voyage	ii	= get_route(view.db, e.ItemIndex);
			if(ii == null){
				ListViewItem	item	= new ListViewItem("---", 0);
				item.SubItems.Add("---");
				item.SubItems.Add("---");
				item.SubItems.Add("---");
				item.SubItems.Add("---");
				e.Item	= item;
			}else{
				e.Item	= create_item(ii, view != m_view3);
			}
		}

		/*-------------------------------------------------------------------------
		 アイテムを作成する
		---------------------------------------------------------------------------*/
		private ListViewItem create_item(SeaRoutes.Voyage i, bool is_draw_show_flag)
		{
			GvoWorldInfo.Info	info1	= m_db.World.FindInfo_WithoutSea(transform.ToPoint(i.MapPoint1st));
			string	_1st_name		= (info1 != null)? info1.Name: "";
			GvoWorldInfo.Info	info2	= m_db.World.FindInfo_WithoutSea(transform.ToPoint(i.MapPointLast));
			string last_name		= (info2 != null)? info2.Name: "";

			string	show_str		= (i.IsEnableDraw)? "表示": "非表示";
			if(!is_draw_show_flag)	show_str	= "---";

			ListViewItem	item			= new ListViewItem(show_str, 0);
			item.UseItemStyleForSubItems	= false;
			item.Tag						= i;
//			item.ToolTipText				= i.TooltipString;
			item.SubItems.Add(_1st_name + "(" + i.GamePoint1stStr + ")");
			item.SubItems.Add(last_name + "(" + i.GamePointLastString + ")");
			item.SubItems.Add(i.MaxDaysString);
			item.SubItems.Add(i.DateTimeString);

			if(is_draw_show_flag){
				item.SubItems[0].ForeColor	= (i.IsEnableDraw)? Color.Blue: Color.Red;
			}
			return item;
		}

		/*-------------------------------------------------------------------------
		 listViewとdb
		---------------------------------------------------------------------------*/
		private class list_view_db
		{
			private ListView							m_list_view;
			private List<SeaRoutes.Voyage>	m_db_list;

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public ListView	view						{	get{	return m_list_view;		}}
			public List<SeaRoutes.Voyage> db	{	get{	return m_db_list;		}}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public list_view_db(ListView view, List<SeaRoutes.Voyage> db)
			{
				m_list_view		= view;
				m_db_list		= db;
			}
		}

		/*-------------------------------------------------------------------------
		 メニューの有効と無効
		---------------------------------------------------------------------------*/
		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			if(!set_context_menu_state(contextMenuStrip1, listView1, 7))	e.Cancel	= true;
		}
		private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
		{
			if(!set_context_menu_state(contextMenuStrip2, listView2, 6))	e.Cancel	= true;
		}
		private void contextMenuStrip3_Opening(object sender, CancelEventArgs e)
		{
			if(!set_context_menu_state(contextMenuStrip3, listView3, 3))	e.Cancel	= true;
		}

		/*-------------------------------------------------------------------------
		 メニューの有効と無効
		 sub
		---------------------------------------------------------------------------*/
		private bool set_context_menu_state(ContextMenuStrip menu, ListView list_view, int all_select_index)
		{
			if(list_view.Items.Count <= 0)	return false;	// 非表示
	
			if(list_view.SelectedIndices.Count <= 0){
				foreach(ToolStripItem i in menu.Items){
					i.Enabled	= false;
				}
			}else{
				foreach(ToolStripItem i in menu.Items){
					i.Enabled	= true;
				}
			}
			// 全て選択
			if(list_view.Items.Count > 0){
				if(menu.Items.Count > all_select_index){
					menu.Items[all_select_index].Enabled	= true;
				}
			}

			return true;	// 表示
		}
	}
}
