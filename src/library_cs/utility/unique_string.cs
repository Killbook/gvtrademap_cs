//-------------------------------------------------------------------------
// ユニークな文字列
// 空な文字列は追加されない
// 検索履歴等で使用する
// 追加された文字列は先頭に追加される
//-------------------------------------------------------------------------
using System.Collections.Generic;
using System.Collections;

//-------------------------------------------------------------------------
namespace Utility
{
	//-------------------------------------------------------------------------
	/// <summary>
	/// ユニークな文字列
	/// 空な文字列は追加されない
	/// 検索履歴等で使用する
	/// 追加された文字列は先頭に追加される
	/// </summary>
	public sealed class UniqueString : IEnumerable<string>
	{
		#region Fields
		private const int				MAX		= 20;
		private const int				MIN		= 1;

		private List<string>			m_strings;			// 重複しない文字列リスト
		private int						m_max;				// 保持する最大
		#endregion

		#region Properties
		//-------------------------------------------------------------------------
		/// <summary>
		/// 内容の取得
		/// </summary>
		/// <param name="i">インデックス</param>
		/// <returns>指定された内容</returns>
		public string this[int i]		{		get{	return m_strings[i];	}}
		/// <summary>
		/// 保持数の取得
		/// </summary>
		public int Count				{		get{	return m_strings.Count;	}}
		/// <summary>
		/// 先頭の内容の取得
		/// </summary>
		public string Top				{		get{	if(m_strings.Count <= 0)	return "";
														return m_strings[0];	}}
		/// <summary>
		/// 最大保持数の取得と設定
		/// </summary>
		public int Max					{		get{	return m_max;			}
												set{	m_max	= value;
														if(m_max < MIN)	m_max	= MIN;
														ajust_count();
												}
										}
		#endregion

		#region Constructors
		//-------------------------------------------------------------------------
		/// <summary>
		/// 構築
		/// </summary>
		public UniqueString()
		{
			m_strings		= new List<string>();
			m_max			= MAX;
		}
		#endregion

		//-------------------------------------------------------------------------
		/// <summary>
		/// 配列に変換
		/// </summary>
		/// <returns>変換された配列</returns>
		public string[] ToArray()
		{
			return m_strings.ToArray();
		}

		//-------------------------------------------------------------------------
		/// 列挙
		public IEnumerator<string> GetEnumerator()
		{
			for(int i=0; i<m_strings.Count; i++){
				yield return m_strings[i];
			}
		}
		IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

		//-------------------------------------------------------------------------
		/// <summary>
		/// 追加
		/// 追加されたらtrueを返す
		/// 先頭に追加される
		/// </summary>
		/// <param name="str">追加する文字列</param>
		/// <returns>追加されたらtrue</returns>
		public bool Add(string str)
		{
			if(string.IsNullOrEmpty(str))	return false;

			// 削除する
			Remove(str);
			// 先頭に追加
			m_strings.Insert(0, str);

			// 最大数に収まるように調整する
			ajust_count();
			return true;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 配列で設定
		/// 設定ファイルからの読み込み用
		/// </summary>
		/// <param name="list">リスト</param>
		public void SetRange(string[] list)
		{
			m_strings.Clear();
			foreach(string s in list){
				AddLast(s);
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 追加
		/// 最後に追加される
		/// 追加されたらtrueを返す
		/// 設定ファイルからの読み込み用
		/// </summary>
		/// <param name="str">追加する文字列</param>
		/// <returns>追加されたらtrue</returns>
		public bool AddLast(string str)
		{
			if(string.IsNullOrEmpty(str))	return false;

			// 削除する
			Remove(str);
			// 最後に追加
			m_strings.Add(str);

			// 最大数に収まるように調整する
			ajust_count();
			return true;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 全て削除
		/// </summary>
		public void Clear()
		{
			m_strings.Clear();
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 一致する文字列を削除する
		/// 削除した場合はtrueを返す
		/// </summary>
		/// <param name="str">削除する文字列</param>
		/// <returns>削除したらtrue</returns>
		public bool Remove(string str)
		{
			if(has_string(str)){
				// 削除する
				m_strings.Remove(str);
				return true;
			}
			return false;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 複製
		/// </summary>
		/// <param name="list">複製元</param>
		public void Clone(UniqueString list)
		{
			Clear();
			Max		= list.Max;
			foreach(string str in list){
				m_strings.Add(str);
			}
		}

		#region Private Methods
		//-------------------------------------------------------------------------
		/// 含まれるかどうかを得る
		private bool has_string(string str)
		{
			foreach(string s in m_strings){
				if(s == str)	return true;	// 含まれる
			}
			return false;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 保持数調整
		/// </summary>
		private void ajust_count()
		{
			// 最大数に収まるように調整する
			while(m_strings.Count > m_max){
				m_strings.RemoveAt(m_strings.Count -1);
			}
		}
		#endregion
	}
}
