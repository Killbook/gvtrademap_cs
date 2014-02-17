//-------------------------------------------------------------------------
// キーアサイン管理
//-------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Utility.Ini;
using System.Collections;

//-------------------------------------------------------------------------
namespace Utility.KeyAssign
{
	//-------------------------------------------------------------------------
	/// <summary>
	/// ショートカット実行用デリゲート
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="arg"></param>
	public delegate void OnProcessCmdKey(object sender, KeyAssignEventArg arg);

	//-------------------------------------------------------------------------
	/// <summary>
	/// ショートカット実行時の引数
	/// </summary>
	public sealed class KeyAssignEventArg : EventArgs
	{
		private object							m_tag;

		/// <summary>
		/// ショートカットに関連付けられたobject、
		/// 通常Enumを使用する
		/// </summary>
		public object Tag						{	get{	return m_tag;		}}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 構築
		/// </summary>
		/// <param name="tag">tag</param>
		public KeyAssignEventArg(object tag)
		{
			m_tag		= tag;
		}
	}

	//-------------------------------------------------------------------------
	/// <summary>
	/// アサインルール、
	/// System.Windows.Forms.Shortcutのみを割り当て可能にする版
	/// </summary>
	public class KeyAssignRuleOnlyShortcut : KeyAssignRule
	{
		//-------------------------------------------------------------------------
		/// <summary>
		/// System.Windows.Forms.Shortcutの場合のみtrueを返す
		/// </summary>
		/// <param name="key">Keys</param>
		/// <returns>アサイン可能ならtrue</returns>
		public override bool CanAssignKeys(Keys key)
		{
			// System.Windows.Forms.Shortcutかどうかチェック
			if(!KeyAssignRule.IsShortcut(key))	return false;

			// base
			return base.CanAssignKeys(key);
		}
	}

	//-------------------------------------------------------------------------
	/// <summary>
	/// アサインルール、
	/// 割り当て可能かどうかとキーの文字列への変換を行う。
	/// 継承してルールを変更することができる。
	/// 動的にルールを変更するとユーザを混乱させる原因となるので注意。
	/// </summary>
	public class KeyAssignRule
	{
		//-------------------------------------------------------------------------
		/// <summary>
		/// 構築
		/// </summary>
		public KeyAssignRule()
		{
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// アサイン可能かどうかを得る。
		/// </summary>
		/// <param name="e">KeyEventArgs</param>
		/// <returns>アサイン可能ならtrue</returns>
		public bool CanAssignKeys(KeyEventArgs e)
		{
			return CanAssignKeys(e.KeyData);
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// アサイン可能かどうかを得る。
		/// ESC、TAB、英字単体等が可能。
		/// このメソッドをオーバーライドしてアサイン可能なキーの組み合わせを
		/// カスタマイズできる。
		/// このメソッドは最低限割り当てられないもののみfalseを返す。
		/// </summary>
		/// <param name="key">Keys</param>
		/// <returns>アサイン可能ならtrue</returns>
		public virtual bool CanAssignKeys(Keys key)
		{
			key	&= ~(Keys.Alt | Keys.Control | Keys.Shift);

			// なし
			if(key == Keys.None)				return false;

			// Ctrl等
			if(key == Keys.ShiftKey)			return false;
			if(key == Keys.Menu)				return false;
			if(key == Keys.ControlKey)			return false;

			// CapsLock
			if(key == (Keys.Oemtilde | Keys.D0))				return false;
			// ひらがな
			if(key == (Keys.OemBackslash | Keys.ShiftKey))		return false;
			if(key == Keys.KanaMode)							return false;
			// IME
			if(key == Keys.IMEAceept)							return false;
			if(key == Keys.IMEConvert)							return false;
			if(key == Keys.IMENonconvert)						return false;
			if(key == Keys.IMEAceept)							return false;
			if(key == Keys.IMEAccept)							return false;
			if(key == Keys.IMEModeChange)						return false;
			// Win
			if(key == Keys.LWin)								return false;
			if(key == Keys.RWin)								return false;

			if(key == Keys.Apps)								return false;
			if(key == Keys.NumLock)								return false;

			// CapsLock
			if(key == Keys.Capital)								return false;

			// 半角、全角
			if(key == (Keys.Oemtilde | Keys.D4))				return false;
			if(key == (Keys.OemBackslash | Keys.ControlKey))	return false;
	
			// アサイン可能
			return true;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// System.Windows.Forms.Shortcutかどうかを得る。
		/// System.Windows.Forms.Shortcutのみをアサイン可能にするヘルパ。
		/// CanAssignKeys()をオーバーライドしたときに使えるかもしれない。
		/// </summary>
		/// <param name="key">key</param>
		/// <returns>アサイン可能ならtrue</returns>
		static public bool IsShortcut(Keys key)
		{
			// Noneはfalseを返す
			if(key == Keys.None)	return false;

			// 変換できればアサイン可能
			object	a = Useful.ToEnum(typeof(Shortcut), (object)(int)key);
			if(a == null)	return false;
			return true;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// キーの文字列を得る。
		/// </summary>
		/// <param name="e">KeyEventArgs</param>
		/// <returns>キー内容の文字列</returns>
		/// <remarks>
		/// <para>キーの文字列を得る。</para>
		/// <para>何も押されていないときは"なし"を返す。</para>
		/// <para>内部でCanAssignKeys()を呼び出し、アサイン可能なキーかどうかを判断する。</para>
		/// <para>アサイン可能でなければ"なし"を返す。</para>
		/// <para>Ctrl、Shift、Altは特別でこれらのキーのみが押された場合は</para>
		/// <para>"Ctrl+"等を返す。</para>
		/// </remarks>
		public string GetKeysString(KeyEventArgs e)
		{
			return GetKeysString(e.KeyData);
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// キーの文字列を得る
		/// </summary>
		/// <param name="key">Keys</param>
		/// <returns>キー内容の文字列</returns>
		/// <remarks>
		/// <para>キーの文字列を得る。</para>
		/// <para>KeyEventArgs.KeyDataを渡すこと</para>
		/// <para>何も押されていないときは"なし"を返す。</para>
		/// <para>内部でCanAssignKeys()を呼び出し、アサイン可能なキーかどうかを判断する。</para>
		/// <para>アサイン可能でなければ"なし"を返す。</para>
		/// <para>Ctrl、Shift、Altは特別でこれらのキーのみが押された場合は</para>
		/// <para>"Ctrl+"等を返す。</para>
		/// </remarks>
		public string GetKeysString(Keys key)
		{
			string	str	= "";
			if((key & Keys.Alt) != 0)		str	+= "Alt+";
			if((key & Keys.Control) != 0)	str	+= "Ctrl+";
			if((key & Keys.Shift) != 0)		str += "Shift+";

			// 割り当てられるキーかチェック
			if(CanAssignKeys(key)){
				str	+= keys_to_string(key);
			}else{
				if(str == "")	return "なし";	// 無効キー
			}
			return str;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// キーを文字列で得る
		/// Oem1とかを少しマシにする
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		static protected string keys_to_string(Keys key)
		{
			key	&= ~(Keys.Alt | Keys.Control | Keys.Shift);

			switch(key){
			case Keys.Back:			return "BackSpace";
			case Keys.D0:			return "0";
			case Keys.D1:			return "1";
			case Keys.D2:			return "2";
			case Keys.D3:			return "3";
			case Keys.D4:			return "4";
			case Keys.D5:			return "5";
			case Keys.D6:			return "6";
			case Keys.D7:			return "7";
			case Keys.D8:			return "8";
			case Keys.D9:			return "9";
			case Keys.OemMinus:		return "=";
			case Keys.Oem7:			return "^";
			case Keys.Oem5:			return "|";
			case Keys.Oem3:			return "@";
			case Keys.Oem4:			return "[";
			case Keys.Oemplus:		return ";";
			case Keys.Oem1:			return ":";
			case Keys.Oem6:			return "]";
			case Keys.Oemcomma:		return ",";
			case Keys.OemPeriod:	return ".";
			case Keys.Oem2:			return "/";
			case Keys.Oem102:		return "\\";
			case Keys.Enter:		return "Enter";
			case Keys.PageUp:		return "PageUp";
			case Keys.PageDown:		return "PageDown";
			case Keys.Escape:		return "ESC";
			case Keys.Add:			return "+";
			case Keys.Subtract:		return "-";
			case Keys.Divide:		return "/";
			case Keys.Multiply:		return "*";
//			case Keys.None:			return "なし";
			}
			// その他
			return key.ToString();
		}
	}
	
	//-------------------------------------------------------------------------
	/// <summary>
	/// キーアサイン管理
	/// </summary>
	/// <remarks>
	/// <para>キーアサインを管理する。</para>
	/// <para>1つのキーアサインに割り振れるキーは1つ。(Ctrl+C等)</para>
	/// <para>List.AddAssign()を呼び、キーアサインテーブルを初期化する。</para>
	/// <para>このクラスはIIniSaveLoadを継承しているため、IniBaseを使用した設定ファイルの読み書きが可能。</para>
	/// <para>ShowSettingDialog()でKeyAssignFormによる設定が可能</para>
	/// <para>アサインを実行するにはProcessCmdKey()を呼ぶ。</para>
	/// <para>ProcessCmdKey()は通常FormのProcessCmdKey()内で呼ぶ。</para>
	/// <para>割り当て可能かどうかの判定にはAssignRuleが使用される。</para>
	/// <para>独自の割り当てルールを指定する場合は、AssignRuleを継承しコンストラクタに渡す。</para>
	/// <para>引数なしのコンストラクタを使用すると、AssignRuleを使用する。</para>
	/// <para>AssignRuleを継承したKeyAssignRuleOnlyShortcutはSystem.Windows.Forms.Shortcutのみを割り当て可能にする。</para>
	/// <para>メニューやコンテキストメニューと同期させるには、</para>
	/// <para>BindTagForMenuItem()でタグを関連付け、UpdateMenuShortcutKeys()でショートカットキーの表示を更新する。</para>
	/// <para>同期させるとメニュークリックのアサイン実行が自動で行われる。</para>
	/// </remarks>
	public class KeyAssignManager : IIniSaveLoad
	{
		private KeyAssignList				m_list;						// assign list
		private OnProcessCmdKey				m_on_processcmdkey;			// ショートカットの実行
		private EventHandler				m_on_update_assign_list;	// アサインリストが更新されたときに呼び出されるデリゲート

		//-------------------------------------------------------------------------
		/// <summary>
		/// アサインリストを得る。
		/// 設定時はOnUpdateAssignList()が呼ばれる。
		/// </summary>
		public KeyAssignList List			{	get{	return m_list;		}
												set{	
													m_list	= value;
													if(m_on_update_assign_list != null){
														m_on_update_assign_list(this, EventArgs.Empty);
													}
												}
											}

		/// <summary>
		/// キーが押されたときのデリゲート
		/// </summary>
		public event OnProcessCmdKey OnProcessCmdKey{
			add{	m_on_processcmdkey	+= value;	}
			remove{	m_on_processcmdkey	-= value;	}
		}

		/// <summary>
		/// アサインリストが更新されたときに呼び出されるデリゲート
		/// </summary>
		public event EventHandler OnUpdateAssignList{
			add{	m_on_update_assign_list	+= value;	}
			remove{	m_on_update_assign_list	-= value;	}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// IniBase読み書き用グループ名の初期値
		/// </summary>
		public string DefaultIniGroupName		{	get{	return "KeyAssignManagerSettings";	}}
	
		//-------------------------------------------------------------------------
		/// <summary>
		/// 構築、割り当てルールにはAssignRuleが使用される
		/// </summary>
		public KeyAssignManager()
			: this(new KeyAssignRule())
		{
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 構築、割り当てルール指定版
		/// </summary>
		/// <param name="rule"></param>
		public KeyAssignManager(KeyAssignRule rule)
		{
			m_list			= new KeyAssignList(rule);
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// ショートカットの実行。
		/// 通常メインformのProcessCmdKey内で呼び出す。
		/// Assignされたキーが押された場合、OnProcessCmdKeyに登録された
		/// デリゲートが呼び出される。
		/// 意図しない呼び出しをチェックするため、OnProcessCmdKeyがnullの場合例外を投げる
		/// </summary>
		/// <param name="keyData">keyData</param>
		/// <returns>ショートカットを実行した場合true</returns>
		public bool ProcessCmdKey(Keys keyData)
		{
			if(m_on_processcmdkey == null)	throw new Exception("OnProcessCmdKeyにデリゲートを登録してから使用してください。");

			List<KeyAssignList.Assign>	alist	= m_list.GetAssignedList(keyData);
			if(alist == null)	return false;		// 割り当てられた機能はない

			// 対応するアサイン分イベントを発生させる
			foreach(KeyAssignList.Assign i in alist){
				m_on_processcmdkey(this, new KeyAssignEventArg(i.Tag));
			}
			return true;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// メニューをクリックされたときのショートカットの実行。
		/// メニューのClickイベント内で呼ぶ。
		/// BindTagForMenuItem()でタグを設定しておけば自動で呼ばれる。
		/// </summary>
		/// <param name="sender">Clickイベントの引数そのまま</param>
		/// <param name="e">Clickイベントの引数そのまま</param>
		public void OnClickToolStripMenu(object sender, EventArgs e)
		{
			if(m_on_processcmdkey == null)	throw new Exception("OnProcessCmdKeyにデリゲートを登録してから使用してください。");

			// ToolStripMenuItem以外は無視
			if(!(sender is ToolStripMenuItem))	return;
			ToolStripMenuItem	ts	= (ToolStripMenuItem)sender;

			// Tagがnullかどうかだけチェック
			if(ts.Tag == null)					return;

			// ショートカット実行
			m_on_processcmdkey(this, new KeyAssignEventArg(ts.Tag));
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// メニューアイテムのタグを設定する。
		/// クリック時のデリゲートの登録も行う。
		/// デリゲートにはOnClickToolStripMenu()を割り当てる。
		/// このメソッドでメニューアイテムにTagを割り当て、
		/// UpdateMenuShortcutKeys()でショートカットの表示を更新する。
		/// クリックされた場合は自動でOnClickToolStripMenu()が呼ばれる。
		/// </summary>
		/// <param name="item">メニューアイテム</param>
		/// <param name="tag">タグ</param>
		public void BindTagForMenuItem(ToolStripMenuItem item, object tag)
		{
			if(item == null)	throw new ArgumentNullException();
			if(tag == null)		throw new ArgumentNullException();

			item.Tag	= tag;
			item.Click	+= new EventHandler(OnClickToolStripMenu);
		}
	
		//-------------------------------------------------------------------------
		/// <summary>
		/// メニューのショートカットキーを更新する。
		/// メニューに表示されるショートカットと管理されているアサインを一致させる。
		/// ToolStripItem.TagがAssign.Tagと一致するものを検索し、ショートカットを更新する。
		/// 表示のみ設定するため、メニューのショートカットの機能は使わない。
		/// ContextMenuStrip版
		/// </summary>
		/// <param name="menu">ContextMenuStrip</param>
		public void UpdateMenuShortcutKeys(ContextMenuStrip menu)
		{
			foreach(ToolStripItem i in menu.Items){
				update_shortcut_keys(i);
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// メニューのショートカットキーを更新する。
		/// メニューに表示されるショートカットと管理されているアサインを一致させる。
		/// ToolStripItem.TagがAssign.Tagと一致するものを検索し、ショートカットを更新する。
		/// 表示のみ設定するため、メニューのショートカットの機能は使わない。
		/// MenuStrip版
		/// </summary>
		/// <param name="menu">MenuStrip</param>
		public void UpdateMenuShortcutKeys(MenuStrip menu)
		{
			foreach(ToolStripItem i in menu.Items){
				update_shortcut_keys(i);
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// ショートカットキーを更新する。
		/// Tagが一致する場合、現在のショートカットキーを割り当てる。
		/// 表示のみ設定するため、メニューのショートカットの機能は使わない。
		/// </summary>
		/// <param name="item">対象</param>
		private void update_shortcut_keys(ToolStripItem item)
		{
			// ToolStripMenuItem以外は無視
			if(item is ToolStripMenuItem){
				ToolStripMenuItem	smi = (ToolStripMenuItem)item;
				foreach(ToolStripItem i in smi.DropDownItems){
					// 子がいれば再帰で潜る
					update_shortcut_keys(i);
				}

				// Tagが一致するAssignに更新
				if(smi.Tag != null){
					KeyAssignList.Assign	a	= m_list.GetAssign(smi.Tag);
					if(a != null){
						// smi.ShortcutKeysにはShortcutのみしか指定できないため、
						// 表示の設定のみ行う
						smi.ShortcutKeys				= Keys.None;	// ショートカットキーとしてはなし
						smi.ShowShortcutKeys			= true;
						smi.ShortcutKeyDisplayString	= (a.Keys == Keys.None)? "": a.KeysString;
					}
				}
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 設定ファイルからの読み込み
		/// </summary>
		/// <param name="p">IniBase</param>
		/// <param name="group">グループ名</param>
		public void IniLoad(IIni p, string group)
		{
			if(String.IsNullOrEmpty(group))		return;
			if(p == null)						return;

			m_list.IniLoad(p, group);
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 設定ファイルに書き出し
		/// </summary>
		/// <param name="p">IniBase</param>
		/// <param name="group">グループ名</param>
		public void IniSave(IIni p, string group)
		{
			if(String.IsNullOrEmpty(group))		return;
			if(p == null)						return;

			m_list.IniSave(p, group);
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// キーアサインの設定
		/// 設定ダイアログを開きアサインを設定する。
		/// 独自のダイアログを使用するときは
		/// KeyAssiginSettingHelperを利用する。
		/// </summary>
		/// <param name="form">親となるフォーム</param>
		/// <returns>変更が行われたときtrue</returns>
		public bool ShowSettingDialog(Form form)
		{
			using(KeyAssignListForm dlg = new KeyAssignListForm(m_list)){
				if(dlg.ShowDialog(form) == DialogResult.OK){
					// 設定変更を反映させる
					// OnUpdateAssignList()を呼ぶ
					this.List	= dlg.List;
					return true;
				}
			}
			return false;
		}
	}

	//-------------------------------------------------------------------------
	/// <summary>
	/// アサインリスト。
	/// 設定変更用にDeepClone()できる。
	/// </summary>
	public sealed class KeyAssignList : IEnumerable<KeyAssignList.Assign>
	{
		private List<Assign>				m_list;
		private KeyAssignRule				m_assign_rule;			// 割り当てルール

		/// <summary>
		/// リスト数
		/// </summary>
		public int Count					{	get{	return m_list.Count;	}}

		/// <summary>
		/// index参照
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Assign this[int index]		{	get{	return m_list[index];	}}
		

		//-------------------------------------------------------------------------
		/// <summary>
		/// 構築、
		/// KeyAssignRuleを使用する
		/// </summary>
		public KeyAssignList()
			: this(new KeyAssignRule())
		{
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 構築、
		/// KeyAssignRuleを指定する
		/// </summary>
		/// <param name="rule"></param>
		public KeyAssignList(KeyAssignRule rule)
		{
			m_list			= new List<Assign>();
			m_assign_rule	= rule;

			if(m_assign_rule == null){
				throw new ArgumentNullException("AssignRuleの指定がnullです。");
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 構築、
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="from"></param>
		public KeyAssignList(KeyAssignList from)
		{
			m_list			= new List<Assign>();
			m_assign_rule	= from.m_assign_rule;	// ルールは参照をコピー
			foreach(Assign i in from.m_list){
				m_list.Add(new Assign(i));
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// コピー
		/// KeyAssignRuleは参照のコピーなので注意
		/// </summary>
		/// <returns></returns>
		public KeyAssignList DeepClone()
		{
			return new KeyAssignList(this);
		}
	
		//-------------------------------------------------------------------------
		/// <summary>
		/// 追加、
		/// 初期値なしの場合はKeys.Noneを渡すこと
		/// </summary>
		/// <param name="name">アサイン名</param>
		/// <param name="group">グループ名</param>
		/// <param name="default_key">初期アサイン</param>
		/// <param name="tag">タグ</param>
		/// <param name="ini_name">設定ファイル書き出し時の名前(英数が望ましい)</param>
		public void AddAssign(string name, string group, Keys default_key, object tag, string ini_name)
		{
			m_list.Add(new Assign(m_assign_rule, name, group, default_key, tag, ini_name));
		}
		
		//-------------------------------------------------------------------------
		/// <summary>
		/// 追加、
		/// public void AddAssign(string name, string group, Keys default_key, object tag, string ini_name)
		/// の少し引数が少ない版
		/// </summary>
		/// <param name="name">アサイン名</param>
		/// <param name="group">グループ名</param>
		/// <param name="default_key">初期アサイン</param>
		/// <param name="tag">タグ</param>
		/// <remarks>
		/// <para>AddAssign(name, group, default_key, tag, tag.ToString());</para>
		/// <para>と呼んだのと同じ。</para>
		/// <para>ini_nameを個別に設定しないならこのメソッドを使用する。</para>
		/// </remarks>
		public void AddAssign(string name, string group, Keys default_key, object tag)
		{
			AddAssign(name, group, default_key, tag, tag.ToString());
		}
	
		//-------------------------------------------------------------------------
		/// <summary>
		/// グループのリストを得る
		/// ない場合はnullを返す
		/// </summary>
		/// <returns>グループリスト</returns>
		public List<string> GetGroupList()
		{
			List<string>	list	= new List<string>();

			// 一致するものを検索
			foreach(Assign a in m_list){
				if(!is_found(a.Group, list)){
					list.Add(a.Group);
				}
			}
			if(list.Count <= 0)		return null;
			return list;
		}

		//-------------------------------------------------------------------------
		private bool is_found(string str, List<string> list)
		{
			foreach(string i in list){
				if(i == str)	return true;
			}
			return false;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 指定されたグループに属するリストを得る
		/// ない場合はnullを返す
		/// </summary>
		/// <param name="group">グループ名</param>
		/// <returns>グループに属するキーアサインリスト</returns>
		public List<Assign> GetAssignedListFromGroup(string group)
		{
			List<Assign>	list	= new List<Assign>();
			// 一致するものを検索
			foreach(Assign a in m_list){
				if(a.Group == group){
					list.Add(a);
				}
			}
			if(list.Count <= 0)		return null;
			return list;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 渡されたキーがアサインされたリストを返す
		/// アサインされていない場合はnullを返す
		/// </summary>
		/// <param name="e">KeyEventArgs</param>
		/// <returns>アサインされたキーのリスト</returns>
		public List<Assign> GetAssignedList(KeyEventArgs e)
		{
			return GetAssignedList(e.KeyData);
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 渡されたキーがアサインされたリストを返す
		/// アサインされていない場合はnullを返す
		/// KeyEventArgs.KeyCodeを渡すこと
		/// </summary>
		/// <param name="key">キー</param>
		/// <returns>アサインされたキーのリスト</returns>
		public List<Assign> GetAssignedList(Keys key)
		{
			List<Assign>	list	= new List<Assign>();

			// 一致するものを検索
			foreach(Assign a in m_list){
				if(a.IsAssignedKey(key)){
					list.Add(a);
				}
			}

			if(list.Count <= 0)	return null;	// アサインされていない
			return list;			
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// キーアサインをすべて初期値に戻す
		/// </summary>
		public void DefaultAll()
		{
			foreach(Assign a in m_list){
				a.Default();
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// Tagに関連付けられたキーアサインを得る
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <returns>キーアサイン</returns>
		public Assign GetAssign(object tag)
		{
			// 一致するものを検索
			foreach(Assign a in m_list){
				if(a.Tag.Equals(tag))	return a;
			}
			return null;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// Tagに関連付けられたキーアサインのショートカットキーを文字列で得る。
		/// 割り当てなし、関連付けられたキーアサインがない場合は""を返す。
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <returns>ショートカットキーの文字列</returns>
		public string GetAssignShortcutText(object tag)
		{
			Assign	a	= GetAssign(tag);
			if(a == null)				return "";
			if(a.Keys == Keys.None)		return "";
			return a.KeysString;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 設定ファイルからの読み込み
		/// IniBaseを対象としている
		/// </summary>
		/// <param name="p">IniBase</param>
		/// <param name="group">グループ名</param>
		public void IniLoad(IIni p, string group)
		{
			if(String.IsNullOrEmpty(group))		return;
			if(p == null)						return;

			foreach(Assign a in m_list){
				a.IniLoad(p, group);
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 設定ファイルに書き出し
		/// IniBaseを対象としている
		/// </summary>
		/// <param name="p">IniBase</param>
		/// <param name="group">グループ名</param>
		public void IniSave(IIni p, string group)
		{
			if(String.IsNullOrEmpty(group))		return;
			if(p == null)						return;

			foreach(Assign a in m_list){
				a.IniSave(p, group);
			}
		}

		//-------------------------------------------------------------------------
		/// 列挙
		public IEnumerator<Assign> GetEnumerator()
		{
			for(int i=0; i<m_list.Count; i++){
				yield return m_list[i];
			}
		}
		IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

		//-------------------------------------------------------------------------
		/// <summary>
		/// キーアサイン
		/// </summary>
		public sealed class Assign
		{
			private KeyAssignRule			m_assign_rule;	// 割り当てルール

			private Keys					m_keys;			// KeyEventArgs.KeyData
			private Keys					m_default_keys;	// KeyEventArgs.KeyData
			private string					m_name;			// function name
			private string					m_group;		// group name
			private object					m_tag;			// tag
			private string					m_ini_name;		// 設定保存用名

			//-------------------------------------------------------------------------
			/// <summary>
			/// キーアサインを文字列で得る
			/// </summary>
			public string KeysString		{	get{	return GetKeysString(m_keys);	}}
			/// <summary>
			/// キーアサインの設定と取得
			/// </summary>
			public Keys Keys				{	get{	return m_keys;					}
									internal	set{	m_keys	= (CanAssignKeys(value))? value: Keys.None;	}}

			/// <summary>
			/// 初期値のキーアサインを得る
			/// </summary>
			public Keys DefaultKeys			{	get{	return m_default_keys;			}}
			/// <summary>
			/// 初期値のキーアサインを文字列で得る
			/// </summary>
			public string DefaultKeysString	{	get{	return GetKeysString(m_default_keys);	}}
			/// <summary>
			/// アサイン名を得る
			/// </summary>
			public string Name				{	get{	return m_name;					}}
			/// <summary>
			/// グループ名を得る
			/// </summary>
			public string Group				{	get{	return m_group;					}}
			/// <summary>
			/// Tagを得る
			/// </summary>
			public object Tag				{	get{	return m_tag;					}}
			/// <summary>
			/// 設定ファイル書き出し時の名前を得る
			/// </summary>
			public string IniName			{	get{	return m_ini_name;				}}

			//-------------------------------------------------------------------------
			/// <summary>
			/// 構築
			/// 初期アサインを最初のアサインとする
			/// </summary>
			/// <param name="rule">割り当てルール</param>
			/// <param name="name">アサイン名</param>
			/// <param name="group">グループ名</param>
			/// <param name="default_key">初期アサイン</param>
			/// <param name="tag">タグ</param>
			/// <param name="ini_name">設定ファイル書き出し時の名前(英数が望ましい)</param>
			internal Assign(KeyAssignRule rule, string name, string group, Keys default_key, object tag, string ini_name)
				: this(rule, name, group, default_key, default_key, tag, ini_name)
			{
			}

			//-------------------------------------------------------------------------
			/// <summary>
			/// 構築
			/// 初期アサインとアサインを別々に設定する版
			/// </summary>
			/// <param name="rule">割り当てルール</param>
			/// <param name="name">アサイン名</param>
			/// <param name="group">グループ名</param>
			/// <param name="default_key">初期アサイン</param>
			/// <param name="key">アサイン</param>
			/// <param name="tag">タグ</param>
			/// <param name="ini_name">設定ファイル書き出し時の名前(英数が望ましい)</param>
			internal Assign(KeyAssignRule rule, string name, string group, Keys default_key, Keys key, object tag, string ini_name)
			{
				if(rule == null){
					throw new ArgumentNullException("ショートカットに割り当てられるかを決定するためにAssignRuleが必要です。");
				}
				m_assign_rule	= rule;

				m_keys			= key;
				m_default_keys	= default_key;
				m_name			= name;
				m_group			= group;
				m_tag			= tag;
				m_ini_name		= ini_name;

				// アサイン不可能ならNoneにする
				if(!CanAssignKeys(m_keys)){
					m_keys			= Keys.None;
				}
				// 初期値で割り当て不可なら例外を投げる
				if(   (m_default_keys != Keys.None)
					&&(!CanAssignKeys(m_default_keys)) ){
					throw new Exception("割り当てできないキーの組み合わせが初期値に指定されました。");
//					m_default_keys	= Keys.None;
				}
			}

			//-------------------------------------------------------------------------
			/// <summary>
			/// 構築、コピーコンストラクタ、
			/// </summary>
			/// <param name="from">コピー元のAssign</param>
			internal Assign(Assign from)
				: this(	from.m_assign_rule,
						from.Name,
						from.Group,
						from.DefaultKeys,
						from.Keys,
						from.Tag,
						from.IniName)
			{
			}

			//-------------------------------------------------------------------------
			/// <summary>
			/// アサインされたキーかどうかを得る
			/// Keys.Noneが渡された場合はfalseを返す
			/// </summary>
			/// <param name="key">Keys</param>
			/// <returns>アサインされていればtrue</returns>
			public bool IsAssignedKey(Keys key)
			{
				if(key == Keys.None)	return false;
				return m_keys == key;
			}

			//-------------------------------------------------------------------------
			/// <summary>
			/// 初期値に設定する
			/// </summary>
			internal void Default()
			{
				this.Keys	= this.DefaultKeys;
			}

			//-------------------------------------------------------------------------
			/// <summary>
			/// 設定ファイルからの読み込み
			/// </summary>
			/// <param name="p">IniBase</param>
			/// <param name="group">グループ名</param>
			internal void IniLoad(IIni p, string group)
			{
				if(String.IsNullOrEmpty(group))			return;
				if(String.IsNullOrEmpty(m_ini_name))	return;
				if(p == null)							return;

				// 割り当て不可ならNone
				this.Keys		= (Keys)p.GetProfile(group, m_ini_name, (int)m_keys);
			}

			//-------------------------------------------------------------------------
			/// <summary>
			/// 設定ファイルに書き出し
			/// </summary>
			/// <param name="p">IniBase</param>
			/// <param name="group"></param>
			internal void IniSave(IIni p, string group)
			{
				if(String.IsNullOrEmpty(group))			return;
				if(String.IsNullOrEmpty(m_ini_name))	return;
				if(p == null)							return;

				p.SetProfile(group, m_ini_name, (int)m_keys);
			}

			//-------------------------------------------------------------------------
			/// <summary>
			/// キーの文字列を得る。
			/// 何も押されていないときは"なし"を返す。
			/// 割り当て可能でないものは"なし"を返す。
			/// 割り当て可能かどうかはKeyAssignRule.CanAssignKeys()の実装による。
			/// </summary>
			/// <param name="key"></param>
			/// <returns></returns>
			public string GetKeysString(Keys key)
			{
				return m_assign_rule.GetKeysString(key);
			}

			//-------------------------------------------------------------------------
			/// <summary>
			/// 割り当て可能かどうかを得る。
			/// 割り当て可能かどうかはKeyAssignRule.CanAssignKeys()の実装による。
			/// </summary>
			/// <param name="key">key</param>
			/// <returns>割り当て可能ならtrue</returns>
			public bool CanAssignKeys(Keys key)
			{
				return m_assign_rule.CanAssignKeys(key);
			}
		}
	}
}
