//-------------------------------------------------------------------------
//
// Iniのインターフェイスと管理ベース
// IniBaseを実装したクラスならどれでも設定情報の読み書きが可能となる
//
//-------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace Utility.Ini
{
	//-------------------------------------------------------------------------
	/// <summary>
	/// iniを使用するインターフェイス。
	/// クラス毎にグループ名を変えて名前の衝突を防ぐ。
	/// </summary>
	/// <remarks>
	/// <para>設定を保持するクラスがこのインターフェイスを実装し、</para>
	/// <para>IniSettingBaseに登録すれば設定の読み書きをまとめて行える。</para>
	/// </remarks>
	public interface IIniSaveLoad
	{
		/// <summary>
		/// グループ名の初期値、
		/// グループ名を指定しなかったときに使用される。
		/// 初期値を持つことでグループ名を用意する手間を省く。
		/// </summary>
		string DefaultIniGroupName{	get;	}

		/// <summary>
		/// 読み込み
		/// </summary>
		/// <param name="ini">IIni</param>
		/// <param name="group">グループ名</param>
		void IniLoad(IIni ini, string group);

		/// <summary>
		/// 書き出し
		/// </summary>
		/// <param name="ini">IIni</param>
		/// <param name="group">グループ名</param>
		void IniSave(IIni ini, string group);
	}

	//-------------------------------------------------------------------------
	/// <summary>
	/// IniBaseを使用した設定管理のベース。
	/// 複数のIIniSaveLoadを登録できる。
	/// 初期値ではグループ名の重複をチェックする。
	/// グループ名が重複してもいい場合はEnableDuplicateGroupNameをtrueにすること。
	/// 継承したクラスのコンストラクタでファイル名を指定することを期待している。
	/// </summary>
	public abstract class IniSettingBase
	{
		/// <summary>
		/// ユーザリスト
		/// </summary>
		private List<SaveLoadNode>			m_list;

		/// <summary>
		/// グループ名の重複チェック、trueのとき重複を許可する。
		/// 初期値はfalse
		/// </summary>
		protected bool						m_enable_duplicate_group_name;

		/// <summary>
		/// グループ名の重複チェック、trueのとき重複を許可する。
		/// 初期値はfalse
		/// </summary>
		public bool EnableDuplicateGroupName	{	get{	return m_enable_duplicate_group_name;		}
													set{	m_enable_duplicate_group_name	= value;	}}
	
		//-------------------------------------------------------------------------
		/// <summary>
		/// 構築
		/// </summary>
		public IniSettingBase()
		{
			m_list							= new List<SaveLoadNode>();
			m_enable_duplicate_group_name	= false;	// グループ名の重複チェック有効
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 追加
		/// </summary>
		/// <param name="user">IIniを使用するユーザ</param>
		/// <param name="group">userが使用するグループ名</param>
		public virtual void AddIniSaveLoad(IIniSaveLoad user, string group)
		{
			// グループ名の重複チェック
			if(!m_enable_duplicate_group_name){
				if(is_duplicate_group_name(group)){
					throw new Exception(String.Format("[ {0} ]\r\nグループ名が重複しています。", group));
				}
			}
			m_list.Add(new SaveLoadNode(user, group));
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 追加、グループ名はuser.DefaultGroupNameを使用する
		/// </summary>
		/// <param name="user">IIniを使用するユーザ</param>
		public virtual void AddIniSaveLoad(IIniSaveLoad user)
		{
			AddIniSaveLoad(user, user.DefaultIniGroupName);
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// グループ名が重複しているか調べる。
		/// </summary>
		/// <param name="group">調べるグループ名</param>
		/// <returns>重複していればtrue</returns>
		private bool is_duplicate_group_name(string group)
		{
			foreach(SaveLoadNode i in m_list){
				if(i.Group == group)	return true;
			}
			return false;
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 読み込む。
		/// 継承先でIIniを作成し、Load(IIni ini)を呼び出すこと。
		/// コンストラクタでファイル名等を指定すること。
		/// </summary>
		public abstract void Load();

		//-------------------------------------------------------------------------
		/// <summary>
		/// IIniから読み込む
		/// </summary>
		/// <param name="ini"></param>
		protected virtual void Load(IIni ini)
		{
			if(ini == null)		throw new ArgumentException();
			foreach(SaveLoadNode i in m_list){
				i.Load(ini);
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// 書きだす。
		/// 継承先でIIniを作成し、Save(IIni ini)を呼び出すこと。
		/// コンストラクタでファイル名等を指定すること。
		/// </summary>
		public abstract void Save();

		//-------------------------------------------------------------------------
		/// <summary>
		/// IIniに書きだす。
		/// </summary>
		/// <param name="ini">IIni</param>
		protected virtual void Save(IIni ini)
		{
			if(ini == null)		throw new ArgumentException();
			foreach(SaveLoadNode i in m_list){
				i.Save(ini);
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// IniBase管理ノード
		/// </summary>
		private class SaveLoadNode
		{
			private IIniSaveLoad				m_user;
			private string						m_group;

			/// <summary>
			/// グループ名
			/// </summary>
			public string Group					{	get{	return m_group;		}}
	
			//-------------------------------------------------------------------------
			/// <summary>
			/// 構築
			/// </summary>
			/// <param name="user">ユーザ</param>
			/// <param name="group">ユーザが使用するグループ名</param>
			public SaveLoadNode(IIniSaveLoad user, string group)
			{
				if(user == null)					throw new ArgumentException();
				if(String.IsNullOrEmpty(group))		throw new ArgumentException();

				m_user		= user;
				m_group		= group;
			}

			//-------------------------------------------------------------------------
			/// <summary>
			/// 読み込み
			/// </summary>
			/// <param name="ini">IniBase</param>
			public void Load(IIni ini)
			{
				if(ini == null)						throw new ArgumentException();
				m_user.IniLoad(ini, m_group);
			}

			//-------------------------------------------------------------------------
			/// <summary>
			/// 書き出し
			/// </summary>
			/// <param name="ini">IIni</param>
			public void Save(IIni ini)
			{
				if(ini == null)						throw new ArgumentException();
				m_user.IniSave(ini, m_group);
			}
		}
	}

	//-------------------------------------------------------------------------
	/// <summary>
	/// iniアクセスベース、
	/// 文字列の設定と取得を実装して使用する。
	/// </summary>
	/// <remarks>
	/// <para>iniと同じように使用できる。</para>
	/// <para>配列を扱うことができる。</para>
	/// <para>bool,int,long,double,float,stringに対応</para>
	/// <para>データの取得と設定のインターフェイスのみを実装している。</para>
	/// <para>ファイルへの読み書きは継承先が自由に行う。</para>
	/// <para>使用者はファイルへの読み書きがどう行われるか意識する必要はない。</para>
	/// <para>IniBaseに対してGetProfile()とSetProfile()を呼ぶだけでよい。</para>
	/// <para>ファイルへの読み書きを隠蔽するため、IniSettingBaseを継承して使用すること。</para>
	/// <para>IniBaseを継承したクラスのインスタンスを直接作成するべきではない。</para>
	/// <para>xmlを使ったクラスとしてXmlIniとXmlIniSettingが用意されている。</para>
	/// </remarks>
	public abstract class IniBase : IIni
	{
		//-------------------------------------------------------------------------
		/// <summary>
		/// データがあるかどうかを得る。
		/// 継承先で実装すること
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <returns>データがある場合true</returns>
		protected abstract bool HasProfile(string group_name, string name);

		//-------------------------------------------------------------------------
		/// <summary>
		/// データ取得
		/// データが得られない場合はdefault_valueを返す
		/// 継承先で実装すること
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="default_value">データが得られない場合の初期値</param>
		/// <returns>得られたデータ</returns>
		public abstract string GetProfile(string group_name, string name, string default_value);
		
		//-------------------------------------------------------------------------
		/// <summary>
		/// データ取得
		/// データが得られない場合はdefault_valueを返す
		/// 継承先で実装すること
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="default_value">データが得られない場合の初期値</param>
		/// <returns>
		/// 得られたデータ
		/// 大きさ0の配列の可能性有り
		/// </returns>
		public abstract string[] GetProfile(string group_name, string name, string[] default_value);

		//-------------------------------------------------------------------------
		/// <summary>
		/// データ設定
		/// 継承先で実装すること
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="value">データ</param>
		public abstract void SetProfile(string group_name, string name, string value);

		//-------------------------------------------------------------------------
		/// <summary>
		/// データ設定
		/// 配列の内容はすべて置きかえられる
		/// 継承先で実装すること
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="value">データ</param>
		public abstract void SetProfile(string group_name, string name, string[] value);
	

		//-------------------------------------------------------------------------
		/// <summary>
		/// boolデータ取得
		/// データが得られない場合はdefault_valueを返す
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="default_value">データが得られない場合の初期値</param>
		/// <returns>得られたデータ</returns>
		public bool GetProfile(string group_name, string name, bool default_value)
		{
			if(!HasProfile(group_name, name))	return default_value;
			string	value	= GetProfile(group_name, name, "");
			try{
				return to_bool(value);
			}catch{
				return default_value;
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// intデータ取得
		/// データが得られない場合はdefault_valueを返す
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="default_value">データが得られない場合の初期値</param>
		/// <returns>得られたデータ</returns>
		public int GetProfile(string group_name, string name, int default_value)
		{
			if(!HasProfile(group_name, name))	return default_value;
			string	value	= GetProfile(group_name, name, "");
			try{
				return to_int(value);
			}catch{
				return default_value;
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// longデータ取得
		/// データが得られない場合はdefault_valueを返す
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="default_value">データが得られない場合の初期値</param>
		/// <returns>得られたデータ</returns>
		public long GetProfile(string group_name, string name, long default_value)
		{
			if(!HasProfile(group_name, name))	return default_value;
			string	value	= GetProfile(group_name, name, "");
			try{
				return to_long(value);
			}catch{
				return default_value;
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// doubleデータ取得
		/// データが得られない場合はdefault_valueを返す
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="default_value">データが得られない場合の初期値</param>
		/// <returns>得られたデータ</returns>
		public double GetProfile(string group_name, string name, double default_value)
		{
			if(!HasProfile(group_name, name))	return default_value;
			string	value	= GetProfile(group_name, name, "");
			try{
				return to_double(value);
			}catch{
				return default_value;
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// floatデータ取得
		/// データが得られない場合はdefault_valueを返す
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="default_value">データが得られない場合の初期値</param>
		/// <returns>得られたデータ</returns>
		public float GetProfile(string group_name, string name, float default_value)
		{
			if(!HasProfile(group_name, name))	return default_value;
			string	value	= GetProfile(group_name, name, "");
			try{
				return to_float(value);
			}catch{
				return default_value;
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// bool設定を更新
		/// 設定項目がない場合Elementが作られる
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="value">データ</param>
		public void SetProfile(string group_name, string name, bool value)
		{
			SetProfile(group_name, name, to_string(value));
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// int設定を更新
		/// 設定項目がない場合Elementが作られる
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="value">データ</param>
		public void SetProfile(string group_name, string name, int value)
		{
			SetProfile(group_name, name, to_string(value));
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// long設定を更新
		/// 設定項目がない場合Elementが作られる
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="value">データ</param>
		public void SetProfile(string group_name, string name, long value)
		{
			SetProfile(group_name, name, to_string(value));
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// double設定を更新
		/// 設定項目がない場合Elementが作られる
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="value">データ</param>
		public void SetProfile(string group_name, string name, double value)
		{
			SetProfile(group_name, name, to_string(value));
		}
	
		//-------------------------------------------------------------------------
		/// <summary>
		/// float設定を更新
		/// 設定項目がない場合Elementが作られる
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="value">データ</param>
		public void SetProfile(string group_name, string name, float value)
		{
			SetProfile(group_name, name, to_string(value));
		}


		//-------------------------------------------------------------------------
		/// <summary>
		/// bool[]データ取得
		/// データが得られない場合はdefault_valueを返す
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="default_value">データが得られない場合の初期値</param>
		/// <returns>
		/// 得られたデータ
		/// 大きさ0の配列の可能性有り
		/// </returns>
		public bool[] GetProfile(string group_name, string name, bool[] default_value)
		{
			if(!HasProfile(group_name, name))	return default_value;

			string[]	value	= GetProfile(group_name, name, new string[]{});
			try{
				List<bool>	list	= new List<bool>();
				foreach(string s in value){
					try{
						bool	b	= to_bool(s);
						list.Add(b);
					}catch{
						// 失敗した要素は無視
					}
				}
				return list.ToArray();
			}catch{
				return default_value;
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// int[]データ取得
		/// データが得られない場合はdefault_valueを返す
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="default_value">データが得られない場合の初期値</param>
		/// <returns>
		/// 得られたデータ
		/// 大きさ0の配列の可能性有り
		/// </returns>
		public int[] GetProfile(string group_name, string name, int[] default_value)
		{
			if(!HasProfile(group_name, name))	return default_value;

			string[]	value	= GetProfile(group_name, name, new string[]{});
			try{
				List<int>	list	= new List<int>();
				foreach(string s in value){
					try{
						int	b	= to_int(s);
						list.Add(b);
					}catch{
						// 失敗した要素は無視
					}
				}
				return list.ToArray();
			}catch{
				return default_value;
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// long[]データ取得
		/// データが得られない場合はdefault_valueを返す
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="default_value">データが得られない場合の初期値</param>
		/// <returns>
		/// 得られたデータ
		/// 大きさ0の配列の可能性有り
		/// </returns>
		public long[] GetProfile(string group_name, string name, long[] default_value)
		{
			if(!HasProfile(group_name, name))	return default_value;

			string[]	value	= GetProfile(group_name, name, new string[]{});
			try{
				List<long>	list	= new List<long>();
				foreach(string s in value){
					try{
						long	b	= to_long(s);
						list.Add(b);
					}catch{
						// 失敗した要素は無視
					}
				}
				return list.ToArray();
			}catch{
				return default_value;
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// double[]データ取得
		/// データが得られない場合はdefault_valueを返す
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="default_value">データが得られない場合の初期値</param>
		/// <returns>
		/// 得られたデータ
		/// 大きさ0の配列の可能性有り
		/// </returns>
		public double[] GetProfile(string group_name, string name, double[] default_value)
		{
			if(!HasProfile(group_name, name))	return default_value;

			string[]	value	= GetProfile(group_name, name, new string[]{});
			try{
				List<double>	list	= new List<double>();
				foreach(string s in value){
					try{
						double	b	= to_double(s);
						list.Add(b);
					}catch{
						// 失敗した要素は無視
					}
				}
				return list.ToArray();
			}catch{
				return default_value;
			}
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// float[]データ取得
		/// データが得られない場合はdefault_valueを返す
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="default_value">データが得られない場合の初期値</param>
		/// <returns>
		/// 得られたデータ
		/// 大きさ0の配列の可能性有り
		/// </returns>
		public float[] GetProfile(string group_name, string name, float[] default_value)
		{
			if(!HasProfile(group_name, name))	return default_value;

			string[]	value	= GetProfile(group_name, name, new string[]{});
			try{
				List<float>	list	= new List<float>();
				foreach(string s in value){
					try{
						float	b	= to_float(s);
						list.Add(b);
					}catch{
						// 失敗した要素は無視
					}
				}
				return list.ToArray();
			}catch{
				return default_value;
			}
		}


		//-------------------------------------------------------------------------
		/// <summary>
		/// bool[]設定を更新
		/// 設定項目がない場合Elementが作られる
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="value">データ</param>
		public void SetProfile(string group_name, string name, bool[] value)
		{
			List<string>	list	= new List<string>();
			foreach(bool v in value){
				list.Add(to_string(v));
			}
			SetProfile(group_name, name, list.ToArray());
		}
	
		//-------------------------------------------------------------------------
		/// <summary>
		/// int[]設定を更新
		/// 設定項目がない場合Elementが作られる
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="value">データ</param>
		public void SetProfile(string group_name, string name, int[] value)
		{
			List<string>	list	= new List<string>();
			foreach(int v in value){
				list.Add(to_string(v));
			}
			SetProfile(group_name, name, list.ToArray());
		}
		
		//-------------------------------------------------------------------------
		/// <summary>
		/// long[]設定を更新
		/// 設定項目がない場合Elementが作られる
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="value">データ</param>
		public void SetProfile(string group_name, string name, long[] value)
		{
			List<string>	list	= new List<string>();
			foreach(long v in value){
				list.Add(to_string(v));
			}
			SetProfile(group_name, name, list.ToArray());
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// double[]設定を更新
		/// 設定項目がない場合Elementが作られる
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="value">データ</param>
		public void SetProfile(string group_name, string name, double[] value)
		{
			List<string>	list	= new List<string>();
			foreach(double v in value){
				list.Add(to_string(v));
			}
			SetProfile(group_name, name, list.ToArray());
		}

		//-------------------------------------------------------------------------
		/// <summary>
		/// float[]設定を更新
		/// 設定項目がない場合Elementが作られる
		/// </summary>
		/// <param name="group_name">グループ名</param>
		/// <param name="name">データ名</param>
		/// <param name="value">データ</param>
		public void SetProfile(string group_name, string name, float[] value)
		{
			List<string>	list	= new List<string>();
			foreach(float v in value){
				list.Add(to_string(v));
			}
			SetProfile(group_name, name, list.ToArray());
		}

		//-------------------------------------------------------------------------
		private string to_string(bool value)
		{
			if(value)	return "1";
			return "0";
		}
		private string to_string(int value){	return value.ToString();	}
		private string to_string(long value){	return value.ToString();	}
		private string to_string(double value){	return value.ToString();	}
		private string to_string(float value){	return value.ToString();	}

		//-------------------------------------------------------------------------
		private bool to_bool(string str)
		{
			if(String.IsNullOrEmpty(str))	throw new Exception();
			if(str == "0")					return false;
			return true;
		}
		private int to_int(string str)
		{
			if(String.IsNullOrEmpty(str))	throw new Exception();
			return Convert.ToInt32(str);
		}
		private long to_long(string str)
		{
			if(String.IsNullOrEmpty(str))	throw new Exception();
			return Convert.ToInt64(str);
		}
		private double to_double(string str)
		{
			if(String.IsNullOrEmpty(str))	throw new Exception();
			return Convert.ToDouble(str);
		}
		private float to_float(string str)
		{
			if(String.IsNullOrEmpty(str))	throw new Exception();
			return (float)Convert.ToDouble(str);
		}
	}
}
