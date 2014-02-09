/*-------------------------------------------------------------------------

 アイテムデータベース

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Diagnostics;
using System.Drawing;
using System;

using Utility;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvo_base
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public class ItemDatabase : MultiDictionary<string, ItemDatabase.Data>
	{
		// カテゴリ
		public enum Categoly{
			_1,
			_2,
			_3,
			_4,
			unknown
		};
		// 種類のグループ
		public enum TypeGroup{
			all,		// 全ての種類
			city_name,	// 街名等
			use_lang,	// 使用言語
			trade,		// 交易品
			item,		// アイテム
			equip,		// 装備
			ship,		// 船
			rigging,	// 艤装
			skill,		// スキル
			report,		// 報告
			technic,	// 陸戦テクニック
			unknown,	// 不明
		};

		/*-------------------------------------------------------------------------
		 アイテム
		---------------------------------------------------------------------------*/
		public class Data : IDictionaryNode<string>
		{
			private int						m_id;
			private string					m_name;
			private string					m_type;
			private string					m_document;
			private Categoly				m_categoly;				// 交易品時のカテゴリ
			private TypeGroup				m_type_group;			// 種類のグループ
			private bool					m_is_combat_item;		// 陸戦アイテムのときtreu

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public string Key{				get{	return m_name;			}}
			public int Id{					get{	return m_id;			}}
			public string Name{				get{	return m_name;			}}
			public string Type{				get{	return m_type;			}}
			public string Document{			get{	return m_document;		}}
			public bool IsRecipe{			get{	return (Type == "レシピ帳")? true: false;	}}
			public bool IsSkill{			get{
													if(Type == "冒険スキル")	return true;
													if(Type == "交易スキル")	return true;
													if(Type == "海事スキル")	return true;
													if(Type == "言語スキル")	return true;
													return false;
											}
								}
			public bool IsReport{			get{	return (Type == "報告")? true: false;		}}
			public Categoly Categoly{		get{	return m_categoly;		}}
			public Color CategolyColor{		get{	return ItemDatabase.GetCategolyColor(m_categoly);	}}
			public TypeGroup TypeGroup{		get{	return m_type_group;	}}
			public bool IsCombatItem{		get{	return m_is_combat_item;	}}
	
			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public Data()
			{
			}

			/*-------------------------------------------------------------------------
			 ItemDb.txt から構築
			---------------------------------------------------------------------------*/
			public bool CreateFromString(string line)
			{
				string[]	tmp		= line.Split(new char[]{','});
				if(tmp.Length < 4)	return false;

				try{
					m_id			= Useful.ToInt32(tmp[0].Trim(), 0);
					m_type			= tmp[1].Trim();
					m_name			= tmp[2].Trim();
					m_document		= "";
					for(int i=3; i<tmp.Length; i++){
						m_document	+= tmp[i].Trim() + "\n";
					}

					if(m_document.IndexOf("再使用時間：") >= 0){
						// 再使用時間：が含まれれば陸戦アイテムとする
						m_is_combat_item	= true;
					}

					// 交易品時のカテゴリ
					m_categoly		= ItemDatabase.GetCategolyFromType(m_type);
					// 種類のグループ
					m_type_group	= ItemDatabase.GetTypeGroupFromType(m_type);
				}catch{
					return false;
				}
				return true;
			}

			/*-------------------------------------------------------------------------
			 ツールチップ用の文字列を得る
			---------------------------------------------------------------------------*/
			public virtual string GetToolTipString()
			{
				string	str		= "名称:" + Name + "\n";
				str				+= "種類:" + Type + "\n";
				str				+= "説明:\n" + Document;
				return str;
			}

			/*-------------------------------------------------------------------------
			 レシピ情報wikiを開く
			 レシピ検索
			---------------------------------------------------------------------------*/
			public void OpenRecipeWiki0()
			{
				// EUCでURLエンコード
				string	urlenc	= Useful.UrlEncodeEUCJP(this.Name);

				// 検索結果を開く
				Process.Start(gvo_def.URL2 + urlenc);	// レシピ検索
			}

			/*-------------------------------------------------------------------------
			 レシピ情報wikiを開く
			 作成可能かどうか検索
			---------------------------------------------------------------------------*/
			public void OpenRecipeWiki1()
			{
				// EUCでURLエンコード
				string	urlenc	= Useful.UrlEncodeEUCJP(this.Name);

				// 検索結果を開く
				Process.Start(gvo_def.URL3 + urlenc);	// レシピで作成可能か検索
			}
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		private Dictionary<string, string>		m_ajust_name_list;		// 微妙な名前の間違い調整用

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public ItemDatabase()
			: base()
		{
			// 名前修正リストの作成
			create_ajust_name_list();
		}
		public ItemDatabase(string fname)
			: this()
		{
			// データベースの構築
			Load(fname);
		}

		/*-------------------------------------------------------------------------
		 データベースの構築
		---------------------------------------------------------------------------*/
		public void Load(string fname)
		{
			string line = "";
			try{
				using (StreamReader	sr	= new StreamReader(
					fname, Encoding.GetEncoding("Shift_JIS"))) {

					while((line = sr.ReadLine()) != null){
						Data	_data	= new Data();

						if(_data.CreateFromString(line)){
							base.Add(_data);
						}else{
							// 解析失敗
						}
					}
				}
			}catch{
				// 読み込み失敗
				base.Clear();
			}

//			technic_cnv();
		}

		/*-------------------------------------------------------------------------
		 名前調整用リストを作成する
		---------------------------------------------------------------------------*/
		private void create_ajust_name_list()
		{
			m_ajust_name_list	= new Dictionary<string,string>();

			m_ajust_name_list.Add("ロット(NO.1)", "ロット（NO.1）");
			m_ajust_name_list.Add("ロット(NO.2)", "ロット（NO.2）");
			m_ajust_name_list.Add("ロット(NO.3)", "ロット（NO.3）");
			m_ajust_name_list.Add("ロット(NO.4)", "ロット（NO.4）");
			m_ajust_name_list.Add("ロット(NO.5)", "ロット（NO.5）");
			m_ajust_name_list.Add("ロット(NO.6)", "ロット（NO.6）");
			m_ajust_name_list.Add("ロット(NO.7)", "ロット（NO.7）");
			m_ajust_name_list.Add("ロット(NO.8)", "ロット（NO.8）");
			m_ajust_name_list.Add("ロット(NO.9)", "ロット（NO.9）");
			m_ajust_name_list.Add("ロット(NO.10)", "ロット（NO.10）");
			m_ajust_name_list.Add("ロット(NO.11)", "ロット（NO.11）");
			m_ajust_name_list.Add("ロット(NO.12)", "ロット（NO.12）");
			m_ajust_name_list.Add("ロット(NO.13)", "ロット（NO.13）");
			m_ajust_name_list.Add("ロット(NO.14)", "ロット（NO.14）");
			m_ajust_name_list.Add("ロット(No.14)", "ロット（NO.14）");
			m_ajust_name_list.Add("鉱石精錬の書", "鉱石製錬の書");
			m_ajust_name_list.Add("合金製錬の書", "合金精錬の書");
			m_ajust_name_list.Add("貴金属の精錬法", "貴金属の製錬法");
			m_ajust_name_list.Add("アラブ神獣の像彫刻術", "アラブの神獣の像彫刻術");
			m_ajust_name_list.Add("ボンバルタ", "ボンバルダ");
			m_ajust_name_list.Add("牛革製ベスト", "牛皮製ベスト");
			m_ajust_name_list.Add("花嫁衣装の縫製書", "花嫁衣裳の縫製法");
			m_ajust_name_list.Add("小型船用高級上納品の梱包", "小型高級上納品の梱包");
			m_ajust_name_list.Add("中型船用高級上納品の梱包", "中型高級上納品の梱包");
			m_ajust_name_list.Add("大型船用高級上納品の梱包", "大型高級上納品の梱包");
			m_ajust_name_list.Add("高級上納品(小型船用)", "高級上納品（小型船用）");
			m_ajust_name_list.Add("高級上納品(中型船用)", "高級上納品（中型船用）");
			m_ajust_name_list.Add("高級上納品(大型船用)", "高級上納品（大型船用）");
			m_ajust_name_list.Add("全艤装補助帆縫製法", "全艤装補助帆組立法");
			m_ajust_name_list.Add("ペットの育て方初級編", "ペットの育て方　初級編");
			m_ajust_name_list.Add("セット料理集第1集", "セット料理集第1巻");
			m_ajust_name_list.Add("フォルダンミルクレープ", "フォンダン・ミルクレープ");
			m_ajust_name_list.Add("果実を使ったお菓子", "果物を使ったお菓子");
			m_ajust_name_list.Add("防御職人の工芸技法", "防具職人の工芸技法");
			m_ajust_name_list.Add("きのこバターソテー", "きのこのバターソテー");
			m_ajust_name_list.Add("フルーツ盛り合わせ", "フルーツの盛り合わせ");
			m_ajust_name_list.Add("ブッシュドノエル", "ブッシュ・ド・ノエル");
			m_ajust_name_list.Add("実用衣装裁縫術・第1巻", "実用衣装縫製術・第1巻");
			m_ajust_name_list.Add("ローマ神話の像彫刻術", "ローマ神話の彫刻術");
			m_ajust_name_list.Add("一味違う！手作り小物", "一味違う！　手作り小物");
			m_ajust_name_list.Add("ゲルマン諸語", "ゲルマン諸語翻訳メモ");
			m_ajust_name_list.Add("東欧諸語", "東欧諸語翻訳メモ");
			m_ajust_name_list.Add("ロマンス諸語", "ロマンス諸語翻訳メモ");
			m_ajust_name_list.Add("アルタイ諸語", "アルタイ諸語翻訳メモ");
			m_ajust_name_list.Add("セム・ハム諸語", "セム・ハム諸語翻訳メモ");
			m_ajust_name_list.Add("アメリカ諸語", "アメリカ諸語翻訳メモ");
			m_ajust_name_list.Add("アフリカ諸語", "アフリカ諸語翻訳メモ");
			m_ajust_name_list.Add("インド洋諸語", "インド洋諸語翻訳メモ");
			m_ajust_name_list.Add("デミ・カルヴァリン砲10門", "デミ・カルヴァリン10門");
			m_ajust_name_list.Add("デミ・カルヴァリン砲12門", "デミ・カルヴァリン12門");
			m_ajust_name_list.Add("デミ・カルヴァリン砲14門", "デミ・カルヴァリン14門");
			m_ajust_name_list.Add("デミ・カルヴァリン砲16門", "デミ・カルヴァリン16門");
			m_ajust_name_list.Add("漁師の心得・鮮魚保存法", "漁師の心得　鮮魚保存法");
			m_ajust_name_list.Add("ファルコン2砲", "ファルコン砲2門");
			m_ajust_name_list.Add("ファルコン4砲", "ファルコン砲4門");
			m_ajust_name_list.Add("ファルコン6砲", "ファルコン砲6門");
			m_ajust_name_list.Add("ファルコン8砲", "ファルコン砲8門");
			m_ajust_name_list.Add("マクラジャボトル", "マラクジャボトル");
			m_ajust_name_list.Add("マクラジャジュース", "マラクジャジュース");
			m_ajust_name_list.Add("軽量シーダ板", "軽量シーダー板");
			m_ajust_name_list.Add("小袖♂", "小袖");
			m_ajust_name_list.Add("小袖♀", "小袖");
			m_ajust_name_list.Add("折鳥帽子♂", "折烏帽子");
			m_ajust_name_list.Add("かんざし♀", "かんざし");
			m_ajust_name_list.Add("通天冠♂", "通天冠");
			m_ajust_name_list.Add("歩揺♀","歩揺");
			m_ajust_name_list.Add("四方平定巾♂","四方平定巾");
			m_ajust_name_list.Add("窄袖衫襦♀","窄袖衫襦");
			m_ajust_name_list.Add("直裾深衣♂","直裾深衣");
		}
	
		/*-------------------------------------------------------------------------
		 検索
		 完全一致のみ
		 アイテムとのリンク用
		---------------------------------------------------------------------------*/
		public Data Find(string name)
		{
			// 名前の間違いを修正する
			name	= adjust_name(name);

			return base.GetValue(name);
		}

		/*-------------------------------------------------------------------------
		 検索
		 IDで検索
		---------------------------------------------------------------------------*/
		public Data Find(int id)
		{
			IEnumerator<Data>	e	= base.GetEnumerator();
			while(e.MoveNext()){
				if(e.Current.Id == id)	return e.Current;
			}
			return null;
		}
		
		/*-------------------------------------------------------------------------
		 検索アイテム名の名前修正
		---------------------------------------------------------------------------*/
		private string adjust_name(string name)
		{
			// ★ を省く
			int	start_index	= name.IndexOf("★");
			if(start_index >= 0){
				name	= name.Substring(0, start_index);
			}

			// 修正
			string	ajust;
			if(m_ajust_name_list.TryGetValue(name, out ajust)){
				name	= ajust;
			}
			return name;
		}

		/*-------------------------------------------------------------------------
		 タイプ名からカテゴリを得る
		 食料品 などの名前からカテゴリを得る
		---------------------------------------------------------------------------*/
		static public Categoly GetCategolyFromType(string name)
		{
			switch(name){
			case "食料品":
			case "調味料":
			case "雑貨":
			case "医薬品":
			case "家畜":
				return Categoly._1;
			case "酒類":
			case "鉱石":
			case "染料":
			case "工業品":
			case "嗜好品":
				return Categoly._2;
			case "繊維":
			case "織物":
			case "武具":
			case "火器":
			case "工芸品":
			case "美術品":
				return Categoly._3;
			case "香辛料":
			case "貴金属":
			case "香料":
			case "宝石":
				return Categoly._4;
			}
			return Categoly.unknown;
		}

		/*-------------------------------------------------------------------------
		 カテゴリ描画用の色を得る
		---------------------------------------------------------------------------*/
		static public Color GetCategolyColor(Categoly cate)
		{
			switch(cate){
			case Categoly._1:	return Color.Gray;
			case Categoly._2:	return Color.OrangeRed;
			case Categoly._3:	return Color.Green;
			case Categoly._4:	return Color.Blue;
			}
			return Color.Black;
		}

		/*-------------------------------------------------------------------------
		 タイプ名からタイプのグループを得る
		 食料品 などの名前から 交易品 等のグループを得る
		---------------------------------------------------------------------------*/
		static public TypeGroup GetTypeGroupFromType(string name)
		{
			switch(name){
			case "食料品":
			case "家畜":
			case "酒類":
			case "調味料":
			case "嗜好品":
			case "香辛料":
			case "香料":
			case "医薬品":
			case "繊維":
			case "染料":
			case "織物":
			case "貴金属":
			case "鉱石":
			case "宝石":
			case "工芸品":
			case "美術品":
			case "雑貨":
			case "武具":
			case "火器":
			case "工業品":
				return TypeGroup.trade;
			case "消耗品":
			case "推薦状":
			case "レシピ帳":
			case "宝箱":
			case "ロット":
			case "素材":
			case "ペット権利書":
			case "不動産権利書":
			case "船権利書":
			case "家具":
			case "物資":
				return TypeGroup.item;
			case "小型帆船":
			case "中小型帆船":
			case "中型帆船":
			case "中大型帆船":
			case "大型帆船":
			case "中小型ガレー":
			case "中型ガレー":
			case "中大型ガレー":
			case "大型ガレー":
				return TypeGroup.ship;
			case "船首像":
			case "追加装甲":
			case "特殊兵装":
			case "補助帆":
			case "舷側砲":
			case "船首砲":
			case "船尾砲":
			case "紋章":
				return TypeGroup.rigging;
			case "頭装備品":
			case "体装備品":
			case "足装備品":
			case "手装備品":
			case "武器・道具":
			case "装身具":
				return TypeGroup.equip;
			case "冒険スキル":
			case "交易スキル":
			case "海事スキル":
			case "言語スキル":
			case "アイテム効果スキル":
			case "副官スキル":
			case "船スキルスキル":
				return TypeGroup.skill;
			case "報告":
				return TypeGroup.report;
			case "陸戦テクニック":
				return TypeGroup.technic;	// 陸戦テクニック
			default:
				return TypeGroup.unknown;
			}
		}

		/*-------------------------------------------------------------------------
		 タイプのグループを文字列に変換する
		---------------------------------------------------------------------------*/
		static public string ToString(TypeGroup tg)
		{
			switch(tg){
			case TypeGroup.all:			return "全ての種類";
			case TypeGroup.city_name:	return "街名等";
			case TypeGroup.use_lang:	return "使用言語";
			case TypeGroup.trade:		return "交易品";
			case TypeGroup.item:		return "アイテム";
			case TypeGroup.equip:		return "装備";
			case TypeGroup.ship:		return "船";
			case TypeGroup.rigging:		return "艤装";
			case TypeGroup.skill:		return "スキル";
			case TypeGroup.report:		return "報告";
			case TypeGroup.technic:		return "陸戦テクニック";
			default:
				return "不明";
			}
		}

		/// <summary>
		/// テクニック情報のコンバート
		/// </summary>
		private void technic_cnv()
		{
			string	fname	= @"database\tec_wiki.txt";
	
			string line = "";
			try{
				using (StreamReader	sr	= new StreamReader(
					fname, Encoding.GetEncoding("Shift_JIS"))) {

					string	type	= "";
					while((line = sr.ReadLine()) != null){
						if(line == "")		continue;

						if(line.IndexOf("**") == 0){
							type = line.Substring(2);
						}else{
							line	= line.Replace(@"&br;", "");

							string[]	split	= line.Split(new char[]{'|'}, StringSplitOptions.None);

							if(split.Length < 1)					continue;
							if(split[1] == @"CENTER:")				continue;
							if(split[1] == @"スキル名")			continue;
							if(split[1] == @"BGCOLOR(#FFE9DD):")	continue;
							if(split[1] == "")						continue;
		
							if(split.Length == 10){
								// 冒険、商人
								Debug.WriteLine("ID:1");
								Debug.WriteLine("名称:" + split[1]);
								Debug.WriteLine("説明:" + type + "系");
								Debug.WriteLine(split[3]);
								foreach(string i in create_document0(split)){
									Debug.WriteLine(i);
								}
								Debug.WriteLine("種類:陸戦テクニック");
							}else if(split.Length == 11){
								// 戦闘系
								Debug.WriteLine("ID:1");
								Debug.WriteLine("名称:" + split[1]);
								Debug.WriteLine("説明:" + type + " " + split[7]);
								Debug.WriteLine(split[3]);
								foreach(string i in create_document0(split)){
									Debug.WriteLine(i);
								}
								Debug.WriteLine("種類:陸戦テクニック");
							}
							Debug.WriteLine("");
						}
					}
				}
			}catch{
				// 読み込み失敗
				base.Clear();
			}
		}

		private string[] create_document0(string[] split)
		{
			List<string>	list	= new List<string>();
			string	tmp;
			tmp		= "Rank:" + split[2] + " ";
			tmp		+= "消費:" + split[4] + " ";
			tmp		+= "射程:" + split[5] + " ";
			tmp		+= "範囲:" + split[6] + " ";
			list.Add(tmp);
			return list.ToArray();
		}
	}
}
