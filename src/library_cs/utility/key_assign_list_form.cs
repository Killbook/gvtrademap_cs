//-------------------------------------------------------------------------
// キーアサイン
// 設定
//-------------------------------------------------------------------------
using System.Windows.Forms;

//-------------------------------------------------------------------------
namespace Utility.KeyAssign
{
	//-------------------------------------------------------------------------
	/// <summary>
	/// キーアサイン設定フォーム
	/// </summary>
	public partial class KeyAssignListForm : Form
	{
		private KeyAssiginSettingHelper		m_helper;

		/// <summary>
		/// OKボタンが押されたときの変更内容
		/// </summary>
		public KeyAssignList List			{	get{	return m_helper.List;	}}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 構築
		/// </summary>
		/// <param name="assign_list">キーアサインリスト</param>
		public KeyAssignListForm(KeyAssignList assign_list)
		{
			InitializeComponent();

			// カラムでのソート有効
			listView1.EnableSort(true);

			// ヘルパに任せる
			m_helper	= new KeyAssiginSettingHelper(	assign_list,
														this,
														comboBox1,
														listView1,
														button1, button4, button5);
		}
	}
}
