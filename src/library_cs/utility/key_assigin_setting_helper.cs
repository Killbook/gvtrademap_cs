//-------------------------------------------------------------------------
// キーアサインヘルパ
// KeyAssignFormの動作内容
// ダイアログにKeyAssignFormの項目を埋め込んだときに使用する
// ComboBox	グループ選択用
// Button		割り当て変更用
// Button		割り当て解除用
// Button		初期割り当てに戻す
// ListView	割り当てリスト
// の5つのコントロールを渡す
//-------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows.Forms;

//-------------------------------------------------------------------------
namespace Utility.KeyAssign
{
	//-------------------------------------------------------------------------
	/// <summary>
	/// キーアサインヘルパ。
	/// KeyAssignFormの動作内容。
	/// ダイアログにKeyAssignFormの項目を埋め込んだときに使用する。
	/// </summary>
	public sealed class KeyAssiginSettingHelper
	{
		private ComboBox						m_select_group_cbox;
		private Button							m_assign_button;
		private Button							m_remove_assign_button;
		private Button							m_default_all_assign_button;
		private ListView						m_list_view;
		private Form							m_form;

		private KeyAssignList					m_assign_list;

		/// <summary>
		/// OKボタンが押されたときの変更内容
		/// </summary>
		public KeyAssignList List				{	get{	return m_assign_list;	}}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 構築
		/// </summary>
		/// <param name="assign_list">必須、複製して保持される</param>
		/// <param name="form">設定用フォーム、アサイン用ダイアログ表示時に参照される。必須</param>
		/// <param name="cbox">グループ選択用、nullでもよい</param>
		/// <param name="list_view">アサイン一覧表示用ListView、必須</param>
		/// <param name="assign_button">アサインボタン、必須</param>
		/// <param name="remove_assign_button">アサイン削除ボタン、nullでもよい</param>
		/// <param name="default_all_assign_button">すべてを初期値に戻すボタン、nullでもよい</param>
		public KeyAssiginSettingHelper(
					KeyAssignList assign_list,
					Form form,
					ComboBox cbox,
					ListView list_view,
					Button assign_button,
					Button remove_assign_button,
					Button default_all_assign_button)
		{
			m_assign_list				= assign_list.DeepClone();	// リストをコピーして持つ

			m_form						= form;
			m_select_group_cbox			= cbox;
			m_list_view					= list_view;
			m_assign_button				= assign_button;
			m_remove_assign_button		= remove_assign_button;
			m_default_all_assign_button	= default_all_assign_button;

			// コントロールの初期化
			init_ctrl();

			// 初期化
			init();

			// 割り当てボタンの更新
			update_assign_button();
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// コントロールの初期化
		/// </summary>
		private void init_ctrl()
		{
			// グループ選択
			if(m_select_group_cbox != null){
				m_select_group_cbox.DropDownStyle			= System.Windows.Forms.ComboBoxStyle.DropDownList;
				m_select_group_cbox.FormattingEnabled		= true;
				m_select_group_cbox.SelectedIndexChanged	+= new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			}

			// 割り当て変更ボタン
			if(m_assign_button != null){
				m_assign_button.Click += new System.EventHandler(this.button1_Click);
			}

			// 割り当て解除ボタン
			if(m_remove_assign_button != null){
				m_remove_assign_button.Click += new System.EventHandler(this.button2_Click);
			}
	
			// 全て初期化ボタン
			if(m_default_all_assign_button != null){
				m_default_all_assign_button.Click += new System.EventHandler(this.button3_Click);
			}
	
			// リスト
			if(m_list_view != null){
				m_list_view.FullRowSelect = true;
				m_list_view.GridLines = true;
				m_list_view.HideSelection = false;
				m_list_view.MultiSelect = false;
				m_list_view.ShowItemToolTips = true;
				m_list_view.UseCompatibleStateImageBehavior = false;
				m_list_view.View = System.Windows.Forms.View.Details;
				m_list_view.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
				m_list_view.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 初期化
		/// </summary>
		private void init()
		{
			m_list_view.Columns.Add("グループ", 90);
			m_list_view.Columns.Add("機能", 180);
			m_list_view.Columns.Add("割り当て", 100);

			List<string>	glist	= m_assign_list.GetGroupList();
			if(glist == null)	return;

			if(m_select_group_cbox != null){
				m_select_group_cbox.Items.Clear();
				m_select_group_cbox.Items.Add("すべて");
				foreach(string i in glist){
					m_select_group_cbox.Items.Add(i);
				}
				if(m_select_group_cbox.Items.Count > 0){
					m_select_group_cbox.SelectedIndex	= 0;
				}
			}else{
				update_list();
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// リストを更新する
		/// </summary>
		private void update_list()
		{
			m_list_view.BeginUpdate();
			m_list_view.Items.Clear();

			// グループ選択状況によりリスト内容を決める
			List<KeyAssignList.Assign>	list	= null;

			if(m_select_group_cbox != null){
				if(m_select_group_cbox.Text != "すべて"){
					list	= m_assign_list.GetAssignedListFromGroup(m_select_group_cbox.Text);
				}
			}
			
			if(list != null){
				foreach(KeyAssignList.Assign i in list){
					add_item(m_list_view, i);
				}
			}else{
				foreach(KeyAssignList.Assign i in m_assign_list){
					add_item(m_list_view, i);
				}
			}

			m_list_view.EndUpdate();

			// 割り当てボタンの更新
			update_assign_button();
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// ListViewに追加する
		/// </summary>
		/// <param name="listview">対象のListView</param>
		/// <param name="i">キーアサイン</param>
		private void add_item(ListView listview, KeyAssignList.Assign i)
		{
			ListViewItem	item	= new ListViewItem(i.Group, 0);
			item.Tag				= i;
//			item.ToolTipText		= i.tool_tip;
			item.SubItems.Add(i.Name);
			item.SubItems.Add(i.KeysString);

			listview.Items.Add(item);
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 割り当て変更ボタンの更新
		/// </summary>
		private void update_assign_button()
		{
			KeyAssignList.Assign	i	= get_selected_item();
			if(i != null){
				update_button(m_assign_button, true);
				update_button(m_remove_assign_button, (i.Keys == Keys.None)? false: true);
			}else{
				update_button(m_assign_button, false);
				update_button(m_remove_assign_button, false);
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// ボタンの有効、無効の更新。
		/// </summary>
		/// <param name="button">ボタン</param>
		/// <param name="enable">有効にするときtrue</param>
		private void update_button(Button button, bool enable)
		{
			if(button == null)				return;
			if(button.Enabled != enable)	button.Enabled	= enable;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 選択が変更された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			// 割り当てボタンの更新
			update_assign_button();
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// ダブルクリックされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView1_DoubleClick(object sender, EventArgs e)
		{
			button1_Click(sender, e);
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// グループ選択が変更された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			update_list();
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// キー割り当て変更
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			KeyAssignList.Assign	i		= get_selected_item();

			if(i == null)		return;

			using(KeyAssignForm dlg = new KeyAssignForm(i)){
				if(dlg.ShowDialog(m_form) == DialogResult.OK){
					i.Keys		= dlg.NewAssignKey;

					// リストの更新
					m_list_view.SelectedItems[0].SubItems[2].Text	= i.KeysString;

					// ボタン状態の更新
					update_assign_button();
				}
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// キー割り当て解除
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			KeyAssignList.Assign	i		= get_selected_item();

			if(i == null)		return;

			// 割り当てなし
			i.Keys		= Keys.None;

			// リストの更新
			m_list_view.SelectedItems[0].SubItems[2].Text	= i.KeysString;

			// ボタン状態の更新
			update_assign_button();
		}
	
		//-------------------------------------------------------------------------
		/// <summary>
		/// 選択されたアイテムを得る
		/// </summary>
		/// <returns></returns>
		private KeyAssignList.Assign get_selected_item()
		{
			if(m_list_view.SelectedItems.Count <= 0)	return null;

			ListViewItem				item	= m_list_view.SelectedItems[0];
			return item.Tag as KeyAssignList.Assign;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// すべて初期値に戻す
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button3_Click(object sender, EventArgs e)
		{
			// 初期値に戻す
			m_assign_list.DefaultAll();

			foreach(ListViewItem i in m_list_view.Items){
				KeyAssignList.Assign	item	= i.Tag as KeyAssignList.Assign;
				if(item == null)	continue;

				i.SubItems[2].Text	= item.KeysString;
			}
		}
	}
}
