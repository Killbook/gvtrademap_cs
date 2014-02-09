/*-------------------------------------------------------------------------

 街とか海域とか上陸地点情報

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using directx;
using Utility;
using gvo_base;
using System.Diagnostics;
using Microsoft.DirectX;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class GvoWorldInfo : IDisposable
	{
		public enum InfoType{
			CITY,			// 街
			SEA,			// 海域
			SHORE,			// 上陸地点
			SHORE2,			// 上陸地点 奥地
			OUTSIDE_CITY,	// 郊外
			PF,				// プライベートファーム
			MAX
		};

		/*-------------------------------------------------------------------------
		 税率を考慮した価格を得る
		---------------------------------------------------------------------------*/
		static public int GetTaxPrice(int price)
		{
			return price + (int)(def.TAX * price);
		}

		/*-------------------------------------------------------------------------
		 1つの街、上陸地点、又は海域
		---------------------------------------------------------------------------*/
		public class Info : hittest
		{
			// 判定用マージン
			const int							CHECK_MARGIN	= 10;

			private string						m_name;				// 名前
			private InfoType					m_info_type;		// 種類(街等)
			private string						m_lang1;			// 言語1
			private string						m_lang2;			// 言語2
			private int							m_url_index;		// URL番号
			private string						m_url;				// URLそのもの
																	// 直接リンク
//			private int							m_area_index;		// ??
			private int							m_sakaba_index;		// & 1	酒場娘,請負人,販売員
																	// & 2	書庫
																	// & 4	豪商
																	// & 8	翻訳家
			private GvoDomains.Server			m_server;			// プレイしているサーバ
			private GvoDomains.Data				m_domains;			// 同盟国などの情報
			private GvoSeaInfo.Data				m_seainfo;			// 海域情報(海域のときのみ)

			private string						m_memo;				// メモ
			private string[]					m_memo_div_lines;	// 行単位に分割されたメモ

			private Point						m_string_offset1;	// 街名用オフセット(大きいアイコン用)
			private Point						m_string_offset2;	// 街名用オフセット(小さいアイコン用)
			private d3d_sprite_rects.rect		m_icon_rect;		// アイコン用矩形
			private d3d_sprite_rects.rect		m_small_icon_rect;	// アイコン用矩形(小)
			private d3d_sprite_rects.rect		m_string_rect;		// 文字用矩形
	
			public enum GroupIndex{
				_0,		// 交易
				_1,		// 道具
				_2,		// 工房
				_3,		// 人物
				_4,		// 船
				_5,		// 大砲
				_6,		// 板
				_7,		// 帆
				_8,		// 像
				_9,		// 行商人
				max
			};
	
			private string[]				m_group_name_tbl	= new string[]{
												"[交易]", "[道具]", "[工房]", "[人物]",
												"[船]", "[大砲]", "[板]", "[帆]",
												"[像]", "[行商人]",
												"",
											};
			private List<Group>				m_groups;		// group_index分のデータ

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public string		Name{						get{	return m_name;			}}
			public InfoType		InfoType{					get{	return m_info_type;		}}
			public string		InfoTypeStr{				get{	return GvoWorldInfo.GetInfoTypeString(m_info_type);	}}
			public string		Lang1{						get{	return m_lang1;			}}
			public string		Lang2{						get{	return m_lang2;			}}
			public int			UrlIndex{					get{	return m_url_index;		}}
			public string		Url{						get{	return m_url;			}}
			public int			Sakaba{						get{	return m_sakaba_index;	}}

			public GvoDomains.Server Server{				get{	return m_server;		}}
			// 同盟国
			public GvoDomains.Country Country{
				get{
					if(m_domains == null)	return GvoDomains.Country.unknown;
					return m_domains.GetDomain(m_server);
				}
			}
			public string		CountryStr{					get{	return GvoDomains.GetCountryString(this.Country);	}}
			public GvoDomains.CityType CityType{
				get{
					if(m_domains == null)	return GvoDomains.CityType.city;
					return m_domains.CityType;
				}
			}
			public string CityTypeStr{						get{	return GvoDomains.GetCityTypeString(this.CityType);	}}
			public GvoDomains.AllianceType AllianceType{
				get{
					if(m_domains == null)	return GvoDomains.AllianceType.unknown;
					return m_domains.AllianceType;
				}
			}
			public string AllianceTypeStr{					get{	return GvoDomains.GetAllianceString(this.AllianceType);	}}
			public GvoDomains.CulturalSphere CulturalSphere{
				get{
					if(m_domains == null)	return GvoDomains.CulturalSphere.unknown;
					return m_domains.CulturalSphere;
				}
			}
			public string CulturalSphereStr{				get{	return GvoDomains.GetCulturalSphereString(this.CulturalSphere);	}}

			public GvoSeaInfo.Data SeaInfo{					get{	return m_seainfo;		}
												internal	set{	m_seainfo	= value;	}}
			public GvoDomains.Data DomainInfo{				get{	return m_domains;		}
												internal	set{	m_domains	= value;	}}

			// メモ全体
			// 行単位は専用の関数を使うこと
			public string Memo{						get{	return m_memo;			}
													set{	m_memo	= value;
															div_memo_lines();		}}
			public bool IsUrl{						get{	if(UrlIndex >= 0)	return true;
															if(Url != "")		return true;
															return false;
													}}

			public Point StringOffset1{				get{	return m_string_offset1;	}}
			public Point StringOffset2{				get{	return m_string_offset2;	}}
			public d3d_sprite_rects.rect IconRect{			get{	return m_icon_rect;			}}
			public d3d_sprite_rects.rect SmallIconRect{	get{	return m_small_icon_rect;	}}
			public d3d_sprite_rects.rect NameRect{		get{	return m_string_rect;		}}

			// ツールチップ
			public string TooltipString{				get{	return __get_tool_tip_string();	}}
			
			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public Info() : base()
			{
				m_groups	= new List<Group>();
				foreach(string s in m_group_name_tbl){
					m_groups.Add(new Group(s));
				}

				m_memo				= "";
				div_memo_lines();

				// 判定用矩形を指定しておく
				base.rect			= new Rectangle(-CHECK_MARGIN,
													-CHECK_MARGIN,
													CHECK_MARGIN*2,
													CHECK_MARGIN*2);

				m_server			= GvoDomains.Server.Euros;	// 初期値はユーロス
				m_domains			= null;
				m_seainfo			= null;

				m_icon_rect			= null;
				m_small_icon_rect	= null;
				m_string_rect		= null;
			}

			/*-------------------------------------------------------------------------
			 worldmap.txtから構築
			---------------------------------------------------------------------------*/
			public bool CreateFromString(string line)
			{
				string[]	tmp		= line.Split(new char[]{','});
				string[]	split	= new string[8+4];

				for(int i=0; i<split.Length; i++){
					split[i]	= "";
				}

				int	max		= 8+4;
				if(tmp.Length < max)	max	= tmp.Length;
	
				// データが少ないことがあるので確定させておく
				for(int i=0; i<max; i++){
					split[i]	= tmp[i];
				}

				m_url		= "";
				try{
					m_name			= split[0];
					base.position	= new Point(Convert.ToInt32(split[1]), Convert.ToInt32(split[2]));

					// m_area_index = split[3]は未使用のため解析しない
//					m_area_index		= -1;

					m_url_index			= Useful.ToInt32(split[4], -1);
					if(m_url_index == -1){
						// 直接リンク
						m_url		= split[4];
					}

					// 街以外用初期値
					m_lang1			= "";
					m_lang2			= "";
					m_sakaba_index	= 0;

					switch(split[5]){
					case "上陸地点":
						m_info_type		= InfoType.SHORE;
						break;
					case "郊外":
						m_info_type		= InfoType.OUTSIDE_CITY;
						break;
					case "海域":
						m_info_type		= InfoType.SEA;
						break;
					case "奥地":
						m_info_type		= InfoType.SHORE2;
						break;
					case "PF":
						m_info_type		= InfoType.PF;
						break;
					default:
						// 街
						m_info_type		= InfoType.CITY;
						m_lang1			= split[5];
						m_lang2			= split[6];
						m_sakaba_index	= Useful.ToInt32(split[7], 0);
						break;
					}

					m_string_offset1	= Useful.ToPoint(split[8], split[9], Point.Empty);
					m_string_offset2	= Useful.ToPoint(split[10], split[11], Point.Empty);
				}catch{
					// 失敗
					return false;
				}
				return true;
			}

			/*-------------------------------------------------------------------------
			 指定されたアイテムがあるかを得る
			 完全一致のみで調べる
			---------------------------------------------------------------------------*/
			public Group.Data HasItem(string find_item)
			{
				foreach(Group g in m_groups){
					Group.Data	d	= g.HasItem(find_item);
					if(d != null)	return d;
				}
				return null;
			}

			/*-------------------------------------------------------------------------
			 詳細情報の読み込み
			---------------------------------------------------------------------------*/
			public void Load(string path, string local_path)
			{
				load_sub(path + m_name + ".txt");
				load_sub(local_path + m_name + ".txt");

				// 翻訳家と豪商がいるかどうか調べる
				m_sakaba_index	|= m_groups[(int)GroupIndex._9].GetSakabaFlag();
			}

			/*-------------------------------------------------------------------------
			 詳細情報の読み込み
			---------------------------------------------------------------------------*/
			private void load_sub(string path)
			{
				if(!File.Exists(path))	return;		// ファイルが見つからない

				string line = "";
				try{
					using(StreamReader	sr	= new StreamReader(
						path, Encoding.GetEncoding("Shift_JIS"))) {

						int		index	= -1;
						while((line = sr.ReadLine()) != null){
							if(line == "")	continue;

							int tmp	= get_group_index(line);
							if(tmp != -1){
								index	= tmp;
							}else{
								if(index != -1){
									// データ追加
									m_groups[index].Add(line, this, index);
								}
							}
						}
					}
				}catch{
					// 読み込み失敗
				}
			}

			/*-------------------------------------------------------------------------
			 メモの読み込み
			---------------------------------------------------------------------------*/
			public void LoadMemo(string path)
			{
				// ファイル名作成
				path	+= m_name + "_memo.txt";
	
				if(!File.Exists(path))	return;		// ファイルが見つからない

				try{
					using (StreamReader	sr	= new StreamReader(
						path, Encoding.GetEncoding("Shift_JIS"))) {

						// 全部読み込む
						m_memo	= sr.ReadToEnd();
					}
					div_memo_lines();
				}catch{
					// 読み込み失敗
					m_memo	= "";
				}
			}

			/*-------------------------------------------------------------------------
			 メモの書き出し
			---------------------------------------------------------------------------*/
			public void WriteMemo(string path)
			{
				// ファイル名作成
				path	+= m_name + "_memo.txt";
	
				// メモ情報がなければ書き出さない
				if(m_memo == ""){
					// ファイルがあれば削除する
					file_ctrl.RemoveFile(path);
					return;
				}

				try{
					using (StreamWriter	sw	= new StreamWriter(
						path, false, Encoding.GetEncoding("Shift_JIS"))) {
						sw.Write(Memo);
					}
				}catch{
					// 書き出し失敗
				}
			}

			/*-------------------------------------------------------------------------
			 グループ番号を得る
			 -1が返った場合、グループの内容となる
			---------------------------------------------------------------------------*/
			public int get_group_index(string line)
			{
				for(int i=0; i<m_group_name_tbl.Length; i++){
					if(line == m_group_name_tbl[i])		return i;
				}
				return -1;
			}

			/*-------------------------------------------------------------------------
			 教えてくれる人を調べる
			 教えてくれる人が居なければnullを返す
			---------------------------------------------------------------------------*/
			public string LearnPerson(string find_str)
			{
				Group.Data d	= m_groups[(int)GroupIndex._3].HasItem(find_str);
				if(d == null)	return null;
				return d.Price;
			}

			/*-------------------------------------------------------------------------
			 数を得る
			---------------------------------------------------------------------------*/
			public int GetCount(GroupIndex index)
			{
				if(index < 0)					return 0;
				if(index >= GroupIndex.max)	return 0;
				return m_groups[(int)index].GetCount();
			}
	
			/*-------------------------------------------------------------------------
			 グループを得る
			---------------------------------------------------------------------------*/
			public Group GetGroup(GroupIndex index)
			{
				if(index < 0)					return null;
				if(index >= GroupIndex.max)	return null;
				return m_groups[(int)index];
			}

			/*-------------------------------------------------------------------------
			 データを得る
			---------------------------------------------------------------------------*/
			public Group.Data GetData(GroupIndex index, int data_index)
			{
				Group	g	= GetGroup(index);
				if(g == null)	return null;
				return g.GetData(data_index);
			}

			/*-------------------------------------------------------------------------
			 参照用にデータ更新
			---------------------------------------------------------------------------*/
			public void UpdateDomains(GvoItemTypeDatabase type, GvoDomains.Server server, GvoDomains.Country my_country)
			{
				m_server	= server;	// プレイしているサーバの更新
				foreach(Group g in m_groups){
					g.UpdateDomains(type, this.Country != my_country);
				}
			}

			/*-------------------------------------------------------------------------
			 描画用の矩形を更新する
			---------------------------------------------------------------------------*/
			public void UpdateDrawRects(gvt_lib lib, int index)
			{
				if(m_info_type == InfoType.SEA)	return;
	
				if(m_info_type == InfoType.CITY){
					switch(this.CityType){
					case GvoDomains.CityType.capital:
						m_icon_rect	= lib.infonameimage.GetIcon(0);
						break;
					case GvoDomains.CityType.capital_islam:
						m_icon_rect	= lib.infonameimage.GetIcon(1);
						break;
					case GvoDomains.CityType.city:
						m_icon_rect	= lib.infonameimage.GetIcon(
												(this.AllianceType == GvoDomains.AllianceType.territory)? 2: 3);
						break;
					case GvoDomains.CityType.city_islam:
						m_icon_rect	= lib.infonameimage.GetIcon(
												(this.AllianceType == GvoDomains.AllianceType.territory)? 4: 5);
						break;
					}
					m_small_icon_rect	= lib.infonameimage.GetIcon(8);
					m_string_rect		= lib.infonameimage.GetCityName(index);
				}else{
					switch(InfoType){
					case InfoType.SHORE:
						m_icon_rect		= lib.infonameimage.GetIcon(9);
						break;
					case InfoType.SHORE2:
						m_icon_rect		= lib.infonameimage.GetIcon(6);
						break;
					case InfoType.OUTSIDE_CITY:
						m_icon_rect		= lib.infonameimage.GetIcon(7);
						break;
					case InfoType.PF:
						m_icon_rect		= lib.infonameimage.GetIcon(11);
						break;
					}
					m_small_icon_rect	= m_icon_rect;		// 同じアイコンを使用する
					m_string_rect		= lib.infonameimage.GetCityName(index);
				}
			}

			/*-------------------------------------------------------------------------
			 アイテムのグループ名を得る
			---------------------------------------------------------------------------*/
			static public string GetGroupName(GroupIndex index)
			{
				switch(index){
				case Info.GroupIndex._0:	return "交易品";
				case Info.GroupIndex._1:	return "道具";
				case Info.GroupIndex._2:	return "工房";
				case Info.GroupIndex._3:	return "人物";
				case Info.GroupIndex._4:	return "船";
				case Info.GroupIndex._5:	return "大砲";
				case Info.GroupIndex._6:	return "板";
				case Info.GroupIndex._7:	return "帆";
				case Info.GroupIndex._8:	return "像";
				case Info.GroupIndex._9:	return "行商人";
				case Info.GroupIndex.max:	return "メモ";
				}
				return "不明";
			}

			/*-------------------------------------------------------------------------
			 メモの行数を得る
			---------------------------------------------------------------------------*/
			public int GetMemoLines()
			{
				if(m_memo_div_lines == null)	return 0;
				return m_memo_div_lines.Length;
			}

			/*-------------------------------------------------------------------------
			 メモを得る
			 行指定
			---------------------------------------------------------------------------*/
			public string GetMemo(int line_index)
			{
				if(line_index < 0)					return "";
				if(line_index >= GetMemoLines())	return "";
				return m_memo_div_lines[line_index];
			}

			/*-------------------------------------------------------------------------
			 メモを行毎に分割する
			---------------------------------------------------------------------------*/
			private void div_memo_lines()
			{
				if(m_memo == ""){
					m_memo_div_lines	= null;
				}else{
					m_memo_div_lines	= m_memo.Split(new char[]{'\n'});
				}
			}

			/*-------------------------------------------------------------------------
			 アイテムデータベースとリンクさせる
			---------------------------------------------------------------------------*/
			internal void LinkItemDatabase(ItemDatabaseCustom item_db)
			{
				foreach(Group g in m_groups){
					g.LinkItemDatabase(item_db, Name);
				}
			}
	
			/*-------------------------------------------------------------------------
			 HPを開くtooltipを得る
			---------------------------------------------------------------------------*/
			public string GetToolTipString_HP()
			{
				if(!IsUrl)		return null;		// URLが登録されていない
	
				if(UrlIndex != -1){
					if(InfoType == GvoWorldInfo.InfoType.CITY){
						// 大商戦
						return "大商戦へ\n左クリックでブラウザを開く";
					}else{
						// DKKmap
						return "D.K.K_mapへ\n左クリックでブラウザを開く";
					}
				}else{
					// クリスタル商会
//					return "直接リンク\n左クリックでブラウザを開く";
					return "大航海時代Online上陸地点地図(仮)へ\n左クリックでブラウザを開く";
				}
			}

			/*-------------------------------------------------------------------------
			 できるだけ検索
			---------------------------------------------------------------------------*/
			public void FindAll(string find_string, List<database.find> list, database.find.FindHandler handler)
			{
				// 名前
				if(handler(Name, find_string)){
					list.Add(new database.find(Name));
				}

				// 言語
				if(handler(Lang1, find_string)){
					list.Add(new database.find(Name, Lang1));
				}
				if(handler(Lang2, find_string)){
					list.Add(new database.find(Name, Lang2));
				}

				// アイテムから検索
				foreach(Group g in m_groups){
					g.FindAll(find_string, list, Name, handler);
				}
			}

			/*-------------------------------------------------------------------------
			 できるだけ検索
			 種類からの検索用
			---------------------------------------------------------------------------*/
			public void FindAll_FromType(string find_string, List<database.find> list, database.find.FindHandler handler)
			{
				// アイテムから検索
				foreach(Group g in m_groups){
					g.FindAll_FromType(find_string, list, Name, handler);
				}
			}

			/*-------------------------------------------------------------------------
			 ツールチップ用文字列の取得
			---------------------------------------------------------------------------*/
			private string __get_tool_tip_string()
			{
				string	tip		= Name + "\n";
				if(SeaInfo != null){
					// 海域
					return tip + SeaInfo.TooltipString;
				}else{
					// その他
					switch(InfoType){
					case GvoWorldInfo.InfoType.CITY:
						switch(AllianceType){
						case GvoDomains.AllianceType.piratical:
						case GvoDomains.AllianceType.unknown:
							tip	+= AllianceTypeStr;
							break;
						case GvoDomains.AllianceType.alliance:
							tip	+= AllianceTypeStr + " " + CountryStr;
							break;
						case GvoDomains.AllianceType.capital:
						case GvoDomains.AllianceType.territory:
							tip	+= CountryStr + " " + AllianceTypeStr;
							break;
						}
						tip	+= "\n種類:" + CityTypeStr;
						tip	+= "\n文化圏:" + CulturalSphereStr;

						// 使用言語
						if(Lang1 != "")	tip	+= "\n使用言語:" + Lang1;
						if(Lang2 != "")	tip	+= "\n使用言語:" + Lang2;
						break;
					case GvoWorldInfo.InfoType.OUTSIDE_CITY:
					case GvoWorldInfo.InfoType.PF:
					case GvoWorldInfo.InfoType.SEA:
					case GvoWorldInfo.InfoType.SHORE:
					case GvoWorldInfo.InfoType.SHORE2:
						tip	+= InfoTypeStr;
						break;
					}
					return tip;
				}
			}
	

			/*-------------------------------------------------------------------------
			 データグループ
			---------------------------------------------------------------------------*/
			public class Group
			{
				/*-------------------------------------------------------------------------
				 データ
				---------------------------------------------------------------------------*/
				public class Data
				{
					private Info				m_info;					// 属している街など
					private string				m_name;					// 名前
					private string				m_tag;					// タグ
					private string				m_tag2;					// タグ2
					private int					m_group_index;			// 属しているグループ番号

					// 外からの参照用
					private bool				m_is_bonus_item;		// 名産品
					private string				m_price;				// 値段、人物名など
					private Color				m_color;				// 表示色
					private Color				m_price_color;			// 値段用色
					private string				m_investment;			// 必要な投資額
					private ItemDatabaseCustom.Data	m_item_db;				// アイテムデータベースの詳細
					
					/*-------------------------------------------------------------------------
					 
					---------------------------------------------------------------------------*/
					public string Name				{		get{	return m_name;					}}
					public Info Info				{		get{	return m_info;					}}
					public bool IsBonusItem			{		get{	return m_is_bonus_item;			}}
					public string Price				{		get{	return m_price;					}}
					public string GroupIndexString	{		get{	return GetGroupName(GroupIndex._0 + m_group_index);	}}
					public Color Color				{		get{	return m_color;					}}
					public Color PriceColor			{		get{	return m_price_color;			}}
					public string Investment		{		get{	return m_investment;			}}
					public ItemDatabaseCustom.Data ItemDb	{		get{	return m_item_db;				}}
					public bool HasTooltip			{		get{	return (ItemDb == null)? false: true;	}}
					public string TooltipString		{
						get{
							if(ItemDb == null){
								string str;
								str		= "名称:" + Name;
								str		+= "\nアイテムデータベースに詳細が見つかりませんでした。\n";
								str		+= "名称が微妙に間違っているか未知のデータです。\n";
								return str;
							}else{
								return getMixedToolTipString();
							}
						}
					}
					public string Type
					{
						get{
							if(ItemDb == null)		return "不明";
							return ItemDb.Type;
						}
					}
					public ItemDatabaseCustom.Categoly Categoly
					{
						get{
							if(ItemDb == null)		return ItemDatabaseCustom.Categoly.unknown;
							return ItemDb.Categoly;
						}
					}
					public Color CategolyColor
					{
						get{
							if(ItemDb == null)		return Color.Black;
							return ItemDb.CategolyColor;
						}
					}

					internal string Tag		{		get{	return m_tag;		}
													set{	m_tag	= value;	}} 
					internal string Tag2	{		get{	return m_tag2;		}}

					/*-------------------------------------------------------------------------
					 
					---------------------------------------------------------------------------*/
					public Data(string line, Info info, int index)
					{
						string[]	tmp		= line.Split(new char[]{','});
						string[]	split	= new string[4];

						for(int i=0; i<split.Length; i++){
							split[i]	= "";
						}

						int	max		= 4;
						if(tmp.Length < max)	max	= tmp.Length;
						for(int i=0; i<max; i++){
							split[i]	= tmp[i];
						}

						try{
							m_name			= split[0];
							m_tag			= split[1];
							if(String.IsNullOrEmpty(split[2])){
								m_tag2		= "0";
							}else{
								m_tag2		= split[2];
							}
							m_investment	= split[3];
						}catch{
							m_name			= "unknown";
							m_tag			= "";
							m_tag2			= "0";
							m_investment	= "";
						}

						m_info				= info;
						m_group_index		= index;

						// 外からの参照用
						m_is_bonus_item		= false;
						m_price				= "";
						m_color				= Color.Black;
						m_price_color		= Color.Black;
						m_item_db			= null;

						// 名称変更に対応する
						update_rename(this);
					}

					/*-------------------------------------------------------------------------
					 描画用の色を得る
					---------------------------------------------------------------------------*/
					private Color _get_color()
					{
						switch(m_tag){
						//スキル
						case "s":		return Color.Black;
						//報告
						case "h":		return Color.DarkRed;
						//レシピ
//						case "%":		return Color.DarkGreen;

						// 投資後リスト入り
						case "*":		return Color.Gray;
						// 投資で得られる
						case "$":		return Color.Blue;
						// 備え付け
						//レシピ
						case "%":		return Color.DarkGreen;
						// 翻訳家
						case "@":		return Color.DarkRed;
						// 販売員
						case "+":		return Color.MediumPurple;
						// 豪商
						case "#":		return Color.Green;
						// 行商人
						case "-":		return Color.Gray;
						// その他
						default:		return Color.Black;
						}
					}
					
					/*-------------------------------------------------------------------------
					 描画用の値段を得る
					 種類によっては値段ではないものが返る
					---------------------------------------------------------------------------*/
					private string _get_price()
					{
						switch(m_tag){
						// 投資で得られる
						case "$":		return "投資";
						// 備え付け
						case "%":		return "備え付け";
						// 翻訳家
						case "@":		return "翻訳家";
						// 販売員
						case "+":		return "販売員";
						// 豪商
						case "#":		return "豪商";
						// 行商人
						case "-":		return "行商人";

						// 投資後リスト入り
						case "*":
						// その他
						default:
							return calc_price(false);
						}
					}

					/*-------------------------------------------------------------------------
					 値段を得る
					 税率を考慮する
					---------------------------------------------------------------------------*/
					private string calc_price(bool is_tax)
					{
						int			p;
						if(!Int32.TryParse(m_tag2, out p))	p	= 0;
						if(is_tax)	p	= GetTaxPrice(p);
						return String.Format("{0:#,0}", p);
					}

					/*-------------------------------------------------------------------------
					 更新
					---------------------------------------------------------------------------*/
					internal void UpdateDomains(GvoItemTypeDatabase item_type_db, bool is_tax)
					{
						// 色
						m_color					= _get_color();
						m_price_color			= m_color;

						// 交易品専用の情報
						if(m_group_index == 0){
							// 交易品
							// 名産品かどうかを調べる
							m_is_bonus_item		= item_type_db.IsBonusItem(Name);

							// 値段
							if(m_info.InfoType == InfoType.CITY){
								if(item_type_db.IsNanbanTradeItem(Name))	m_price	= "南蛮";
								else								m_price	= calc_price(is_tax);
							}else{
								update_rank(item_type_db);
							}
						}else{
							// その他
							m_is_bonus_item		= false;

							// 値段
							if(m_group_index != 3)	m_price	= _get_price();
							else					m_price	= Tag2;				// 人物
						}
					}

					/*-------------------------------------------------------------------------
					 調達などのランクを更新する
					---------------------------------------------------------------------------*/
					private void update_rank(GvoItemTypeDatabase type)
					{
						int		index		= Useful.ToInt32(m_tag2, 0);
		
						switch(index){
						case -1:
							m_price			= "採集R" + rank_to_str(type.GetSaisyuRank(Name));
							m_price_color	= Color.DarkCyan;
							break;
						case -2:
							m_price			= "調達R" + rank_to_str(type.GetChotatuRank(Name));
							m_price_color	= Color.Olive;
							break;	
						case -3:
							// -3は予約されているが、データには含まれない
							m_price			= "探索";
							m_price_color	= Color.DarkViolet;
							break;
						case 0:
						default:
							m_price			= "釣りR" + rank_to_str(type.GetFishingRank(Name));
							m_price_color	= Color.Gray;
							break;
						}
					}

					/*-------------------------------------------------------------------------
					 ランクを文字列に変換する
					 R1～R19以外は??に変換される
					---------------------------------------------------------------------------*/
					private string rank_to_str(int rank)
					{
						if(rank < 1)	return "??";
						if(rank >= 20)	return "??";
						return rank.ToString();
					}

					/*-------------------------------------------------------------------------
					 アイテムデータベースとリンクさせる
					---------------------------------------------------------------------------*/
					internal void LinkItemDatabase(ItemDatabaseCustom db, string info_name)
					{
						if(String.IsNullOrEmpty(Name))	return;

						ItemDatabaseCustom.Data	d	= db.Find(Name);
#if DEBUG
//						if(d == null){
//							Debug.WriteLine(String.Format("{0} {1}", info_name, name));
//						}
#endif
						m_item_db		= d;
					}

					/*-------------------------------------------------------------------------
					 アイテム名変更に対応する
					---------------------------------------------------------------------------*/
					static private void update_rename(Data d)
					{
						switch(d.m_name){
						case "仕立て道具":
							{
								d.m_name	= "裁縫道具";
								d.m_tag2	= "10000";
							}
							break;
						case "回避指南書第1巻":
							d.m_name	= "連撃指南書第1巻";
							break;
						case "攻撃指南書第1巻":
							d.m_name	= "猛攻指南書第1巻";
							break;
						case "回復指南書第1巻":
							d.m_name	= "活用指南書第1巻";
							break;
						case "防御指南書第1巻":
							d.m_name	= "奇手指南書第1巻";
							break;
						case "パデットロール":
							d.m_name	= "パデッドロール";
							break;
						case "診察室の製法":
							d.m_name	= "造船素材・診察室";
							break;
						case "天子の像":
							d.m_name	= "天使の像";
							break;
						case "海域":
							d.m_name	= "地理";
							break;
						}
					}

					/*-------------------------------------------------------------------------
					 詳細とミックスしたツールチップ用の文字列を得る
					---------------------------------------------------------------------------*/
					private string getMixedToolTipString()
					{
						if(ItemDb == null)		return "";

						string	str		= "名称:" + this.ItemDb.Name + "\n";
						if(Categoly != ItemDatabaseCustom.Categoly.unknown){
							str				+= "種類:" + Type + "(カテゴリ" + ((int)Categoly + 1).ToString() + ")\n";
						}else{
							str				+= "種類:" + Type + "\n";
						}
						if(this.Investment != ""){
							str				+= "投資:" + this.Investment + "\n";
						}
						str				+= "説明:\n" + ItemDb.Document;
						return str;
					}
				}

				/*-------------------------------------------------------------------------
				 
				---------------------------------------------------------------------------*/
				private string					m_name;				// 名前
				private List<Data>				m_datas;			// データ

				/*-------------------------------------------------------------------------
				 
				---------------------------------------------------------------------------*/
				public string Name				{	get{	return m_name;		}}

				/*-------------------------------------------------------------------------
				 
				---------------------------------------------------------------------------*/
				public Group(string name)
				{
					m_name		= name;
					m_datas		= new List<Data>();
				}

				/*-------------------------------------------------------------------------
				 追加
				---------------------------------------------------------------------------*/
				public void Add(string line, Info info, int index)
				{
					m_datas.Add(new Data(line, info, index));
				}

				/*-------------------------------------------------------------------------
				 指定されたアイテムがあるかを得る
				 完全一致のみで調べる
				---------------------------------------------------------------------------*/
				public Data HasItem(string find_item)
				{
					foreach(Data d in m_datas){
						if(d.Name == find_item)		return d;
					}
					return null;
				}

				/*-------------------------------------------------------------------------
				 豪商と翻訳家フラグを得る
				 ついでに行商人フラグを設定する
				---------------------------------------------------------------------------*/
				public int GetSakabaFlag()
				{
					int	flag	= 0;
					foreach(Data d in m_datas){
						if(d.Tag == "@"){			// 翻訳家
							flag	|= 4;
						}else if(d.Tag == "#"){		// 豪商
							flag	|= 8;
						}else if(d.Tag == ""){
							// 行商人扱い
							d.Tag	= "-";
						}
					}
					return flag;
				}

				/*-------------------------------------------------------------------------
				 数を得る
				---------------------------------------------------------------------------*/
				public int GetCount()
				{
					return m_datas.Count;
				}

				/*-------------------------------------------------------------------------
				 データを得る
				---------------------------------------------------------------------------*/
				public Data GetData(int index)
				{
					if(index < 0)			return null;
					if(index >= GetCount())	return null;
					return m_datas[index];
				}

				/*-------------------------------------------------------------------------
				 データを更新する
				---------------------------------------------------------------------------*/
				internal void UpdateDomains(GvoItemTypeDatabase item_type_db, bool is_tax)
				{
					foreach(Data d in m_datas){
						d.UpdateDomains(item_type_db, is_tax);
					}
				}

				/*-------------------------------------------------------------------------
				 アイテムデータベースとリンクさせる
				---------------------------------------------------------------------------*/
				internal void LinkItemDatabase(ItemDatabaseCustom item_db, string name)
				{
					foreach(Data d in m_datas){
						d.LinkItemDatabase(item_db, name);
					}
				}

				/*-------------------------------------------------------------------------
				 できるだけ検索
				---------------------------------------------------------------------------*/
				public void FindAll(string find_string, List<database.find> list, string info_name, database.find.FindHandler handler)
				{
					foreach(Data d in m_datas){
						if(handler(d.Name, find_string)){
							list.Add(new database.find(database.find.find_type.data, info_name, d));
						}
						if(handler(d.Price, find_string)){
							list.Add(new database.find(database.find.find_type.data_price, info_name, d));
						}
					}
				}

				/*-------------------------------------------------------------------------
				 できるだけ検索
				 種類からの検索用
				---------------------------------------------------------------------------*/
				public void FindAll_FromType(string find_string, List<database.find> list, string info_name, database.find.FindHandler handler)
				{
					foreach(Data d in m_datas){
						if(handler(d.Type, find_string)){
							list.Add(new database.find(database.find.find_type.data, info_name, d));
						}
					}
				}
			}
		}

		/*-------------------------------------------------------------------------
		 GvoWorldInfo
		---------------------------------------------------------------------------*/
		private gvt_lib					m_lib;
		private gvo_season				m_season;
	
		private draw_infonames			m_draw_infonames;		// 街名などの描画
	
		private hittest_list			m_world;				// 世界の情報
		private GvoItemTypeDatabase		m_item_type_db;			// アイテムの種類情報
		private GvoDomains				m_domains;				// 同盟状況
		private GvoSeaInfo				m_seainfo;				// 海域情報

		private hittest_list			m_nonseas;				// 海域以外の一覧
		private hittest_list			m_seas;					// 海域一覧

		private int						m_count_of_city;		// 街の数
		private int						m_count_of_outof_city;	// 郊外の数
		private int						m_count_of_sea;			// 海域の数
		private int						m_count_of_pf;			// プライベートファームの数
		private int						m_count_of_shore;		// 上陸地点の数
		private int						m_count_of_shore2;		// 奥地の数

		// スレッドでの読み込みを行った場合の詳細情報読み込み場所
		private int						m_load_info_current;
		private string					m_load_info_str;

		private GvoDomains.Server		m_server;				// カレントサーバ
		private GvoDomains.Country		m_my_country;			// カレント国

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public hittest_list World			{	get{	return m_world;					}}

		//
		public int CountOfCity				{	get{	return m_count_of_city;			}}
		public int CountOfOutofCity			{	get{	return m_count_of_outof_city;	}}
		public int CountOfSea				{	get{	return m_count_of_sea;			}}
		public int CountOfPf				{	get{	return m_count_of_pf;			}}
		public int CountOfShore				{	get{	return m_count_of_shore;		}}
		public int CountOfShore2			{	get{	return m_count_of_shore2;		}}
	
		// スレッド読み込み用
		public int LoadInfoMax				{	get{	return m_world.Count;			}}
		public int LoadInfoCurrent			{	get{	return m_load_info_current;		}}
		public string LoadInfoStr			{	get{	return m_load_info_str;			}}

		public GvoDomains.Server Server		{	get{	return m_server;				}}
		public GvoDomains.Country MyCountry	{	get{	return m_my_country;			}}

		public hittest_list NoSeas			{	get{	return m_nonseas;				}}
		public hittest_list Seas			{	get{	return m_seas;					}}
		public gvo_season Season			{	get{	return m_season;				}}

		/*-------------------------------------------------------------------------
		 世界の情報管理
		---------------------------------------------------------------------------*/
		public GvoWorldInfo(gvt_lib lib, gvo_season season, string file_name, string domains_file_name, string memo_path)
		{
			m_lib				= lib;
			m_season			= season;

			// スレッド用読み込み場所
			m_load_info_current	= 0;
			m_load_info_str		= "";
	
			m_world				= new hittest_list();
			m_item_type_db		= new GvoItemTypeDatabase();
			m_domains			= new GvoDomains();
			m_seainfo			= new GvoSeaInfo();
			m_nonseas			= new hittest_list();
			m_seas				= new hittest_list();
	
			m_draw_infonames	= new draw_infonames(lib, this);
	
			// サーバと国を初期化
			// エラー番号としておく
			m_server	= GvoDomains.Server.max;
			m_my_country	= GvoDomains.Country.max;
	
			// 同盟状況の順番を読み込む
			m_domains.LoadIndex(domains_file_name);

			// 海域情報を読み込む
			m_seainfo.Load();

			// 街情報を読み込む
			load_city_info(file_name);

			// メモを読み込む
			load_memo(memo_path);

			// GvoDomains、GvoItemTypeDatabase等のデータベースとリンクする
			link_database();

			// 海域一覧とその他一覧を構築する
			create_seas_list();
		}

		/*-------------------------------------------------------------------------
		 街情報を読み込む
		---------------------------------------------------------------------------*/
		private void load_city_info(string file_name)
		{
			string line = "";
			try{
				int index	= 0;
				using (StreamReader	sr	= new StreamReader(
					file_name, Encoding.GetEncoding("Shift_JIS"))) {

					while((line = sr.ReadLine()) != null){
						Info	_info	= new Info();

						if(_info.CreateFromString(line)){
							// 街情報追加
							m_world.Add(_info);
						}else{
							// 解析失敗
						}
						index++;
					}
				}
				// それぞれの数を数えておく
				count_info_type();
			}catch{
				// 読み込み失敗
				m_world.Clear();
			}
		}
	
		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public void Dispose()
		{
			if(m_draw_infonames != null)	m_draw_infonames.Dispose();
			m_draw_infonames	= null;
		}

		/*-------------------------------------------------------------------------
		 GvoDomains、GvoItemTypeDatabase等のデータベースとリンクする
		---------------------------------------------------------------------------*/
		private void link_database()
		{
			int index	= 0;
			foreach(Info i in m_world){
				if(i.InfoType == InfoType.SEA){
					// 海域
					i.SeaInfo	= m_seainfo.Find(i.Name);
				}else{
					// 海域以外
					i.DomainInfo	= m_domains.GetCity(i.Name);
					bool	has_name_image	= true;

					if(i.DomainInfo != null){
						has_name_image		= i.DomainInfo.HasNameImage;
					}

					// 描画用の矩形を更新する
					i.UpdateDrawRects(m_lib, has_name_image? index: -1);
					if(has_name_image)	index++;
				}
			}
		}

		/*-------------------------------------------------------------------------
		 海域一覧とその他一覧を構築する
		---------------------------------------------------------------------------*/
		private void create_seas_list()
		{
			m_nonseas.Clear();
			m_seas.Clear();

			foreach(Info i in m_world){
				if(i.InfoType == InfoType.SEA){
					// 海域
					if(i.SeaInfo != null)	m_seas.Add(i);
				}else{
					// 海域以外
					m_nonseas.Add(i);
				}
			}
		}

		/*-------------------------------------------------------------------------
		 メモを読み込む
		---------------------------------------------------------------------------*/
		private void load_memo(string path)
		{
			foreach(Info i in m_world){
				i.LoadMemo(path);
			}
		}

		/*-------------------------------------------------------------------------
		 メモを書き出す
		---------------------------------------------------------------------------*/
		public void WriteMemo(string path)
		{
			foreach(Info i in m_world){
				i.WriteMemo(path);
			}
		}
	
		/*-------------------------------------------------------------------------
		 それぞれの数を数えておく
		---------------------------------------------------------------------------*/
		private void count_info_type()
		{
			foreach(Info _info in m_world){
				switch(_info.InfoType){
				case InfoType.CITY:			// 街
					m_count_of_city++;
					break;
				case InfoType.SEA:				// 海域
					m_count_of_sea++;
					break;
				case InfoType.SHORE:			// 上陸地点
					m_count_of_shore++;
					break;
				case InfoType.SHORE2:			// 上陸地点 奥地
					m_count_of_shore2++;
					break;
				case InfoType.OUTSIDE_CITY:	// 郊外
					m_count_of_outof_city++;
					break;
				case InfoType.PF:				// プライベートファーム
					m_count_of_pf++;
					break;
				}
			}
		}

		/*-------------------------------------------------------------------------
		 マップ座標から検索
		---------------------------------------------------------------------------*/
		public Info FindInfo(Point map_pos)
		{
			return (Info)m_world.HitTest(map_pos);
		}

		/*-------------------------------------------------------------------------
		 マップ座標から検索
		 海域を含まない
		---------------------------------------------------------------------------*/
		public Info FindInfo_WithoutSea(Point map_pos)
		{
			Info	i	= (Info)m_world.HitTest(map_pos);
			if(i == null)						return null;
			if(i.InfoType == InfoType.SEA)	return null;
			return (Info)m_world.HitTest(map_pos);
		}

		/*-------------------------------------------------------------------------
		 名前から検索
		---------------------------------------------------------------------------*/
		public Info FindInfo(string name)
		{
			if(name == null)	return null;
			if(name == "")		return null;
			foreach(Info _info in m_world){
				if(_info.Name == name){
					return _info;	// 見つかった
				}
			}
			return null;			// 見つからない
		}

		/*-------------------------------------------------------------------------
		 できるだけ検索
		 検索する文字を含むものをできるだけ検索する
		---------------------------------------------------------------------------*/
		public void FindAll(string find_string, List<database.find> list, database.find.FindHandler handler)
		{
			foreach(Info _info in m_world){
				_info.FindAll(find_string, list, handler);
			}
		}

		/*-------------------------------------------------------------------------
		 種類で検索
		 検索する文字を含むものをできるだけ検索する
		 例えば 食料品 等での検索用
		---------------------------------------------------------------------------*/
		public void FindAll_FromType(string find_string, List<database.find> list, database.find.FindHandler handler)
		{
			foreach(Info _info in m_world){
				_info.FindAll_FromType(find_string, list, handler);
			}
		}

		/*-------------------------------------------------------------------------
		 文化圏リストを作成する
		---------------------------------------------------------------------------*/
		public void CulturalSphereList(List<database.find> list)
		{
			for(int i=1; i<(int)GvoDomains.CulturalSphere.MAX; i++){
				string	tooltip	= m_domains.GetCulturalSphereTooltip(GvoDomains.CulturalSphere.unknown + i);
				list.Add(new database.find(GvoDomains.CulturalSphere.unknown + i, tooltip));
			}
		}

		/*-------------------------------------------------------------------------
		 詳細情報の読み込み
		 スレッド読みに対応
		---------------------------------------------------------------------------*/
		public bool Load(string path, string local_path)
		{
			// 同盟状況読み込み
			m_domains.Load();

			// アイテム分類情報読み込み
			m_item_type_db.Load();

			// 街等の詳細読み込み
			m_load_info_current	= 0;
			foreach(Info _info in m_world){
				m_load_info_str	= _info.Name;
				_info.Load(path, local_path);
				m_load_info_current++;
			}
			m_load_info_str		= "完了";

			// サーバと自国を反映させる
			SetServerAndCountry(GvoDomains.Server.Euros, GvoDomains.Country.England);
			return true;
		}

		/*-------------------------------------------------------------------------
		 アイテムデータベースとリンクさせる
		---------------------------------------------------------------------------*/
		public void LinkItemDatabase(ItemDatabaseCustom item_db)
		{
			foreach(Info _info in m_world){
				_info.LinkItemDatabase(item_db);
			}
		}
			
		/*-------------------------------------------------------------------------
		 詳細情報の書き出し
		 同盟国のみ書き出す
		---------------------------------------------------------------------------*/
		public bool Write(string path)
		{
			// 同盟状況書き出し
			m_domains.Write();

			return true;
		}

		/*-------------------------------------------------------------------------
		 サーバと自国を設定する
		---------------------------------------------------------------------------*/
		public void SetServerAndCountry(GvoDomains.Server server, GvoDomains.Country country)
		{
			if(   (m_server == server)
				&&(m_my_country == country)){
				// 前回と同じならなにもしない
				return;
			}
	
			// 設定更新
			m_server	= server;
			m_my_country	= country;

			// 設定を反映させておく
			update_domains();
		}

		/*-------------------------------------------------------------------------
		 サーバと自国を設定する
		---------------------------------------------------------------------------*/
		private void update_domains()
		{
			foreach(Info _info in m_world){
				// データベースを更新する
				_info.UpdateDomains(m_item_type_db, m_server, m_my_country);
			}
		}

		/*-------------------------------------------------------------------------
		 同盟国を変更する
		---------------------------------------------------------------------------*/
		public bool SetDomain(string country_name, GvoDomains.Country country)
		{
			// 変更
			bool ret	= m_domains.SetDomain(m_server, country_name, country);

			// 設定を反映させておく
			update_domains();
			return ret;
		}

		/*-------------------------------------------------------------------------
		 同盟国更新用の文字列を得る
		 "サーバ番号"+"街番号"+"同盟国番号"
		---------------------------------------------------------------------------*/
		public string GetNetUpdateString(string country_name)
		{
			return m_domains.GetNetUpdateString(m_server, country_name);
		}

		/*-------------------------------------------------------------------------
		 InfoType
		---------------------------------------------------------------------------*/
		static public string GetInfoTypeString(InfoType __type)
		{
			switch(__type){
			case InfoType.CITY:			return "街";
			case InfoType.SEA:				return "海域";
			case InfoType.SHORE:			return "上陸地点";
			case InfoType.SHORE2:			return "上陸地点 奥地";
			case InfoType.OUTSIDE_CITY:	return "郊外";
			case InfoType.PF:				return "プライベートファーム";
			}
			return "不明";
		}

		/*-------------------------------------------------------------------------
		 街名描画
		 上陸地点も含む
		---------------------------------------------------------------------------*/
		public void DrawCityName()
		{
			m_draw_infonames.DrawCityName();
		}
	
		/*-------------------------------------------------------------------------
		 海域名描画
		---------------------------------------------------------------------------*/
		public void DrawSeaName()
		{
			m_draw_infonames.DrawSeaName();
		}
	}

	/*-------------------------------------------------------------------------
	 アイテムの種類
	 名産、採集、探索ランク
	 カテゴリはアイテムデータベースに移動した
	---------------------------------------------------------------------------*/
	public class GvoItemTypeDatabase
	{
		/*-------------------------------------------------------------------------
		 釣り、採集、調達用ランク取得
		---------------------------------------------------------------------------*/
		public class ItemRank
		{
			private string					m_name;				// アイテム名
			private int						m_rank;				// 必要ランク

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public string Name			{	get{	return m_name;		}}
			public int Rank				{	get{	return m_rank;		}}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public ItemRank(string line)
			{
				string[]	tmp		= line.Split(new char[]{','});
				string[]	split	= new string[2];

				for(int i=0; i<split.Length; i++){
					split[i]	= "";
				}

				int	max		= 2;
				if(tmp.Length < max)	max	= tmp.Length;
				for(int i=0; i<max; i++){
					split[i]	= tmp[i];
				}

				m_name			= split[0];
				m_rank			= Useful.ToInt32(split[1], 0);
			}
		}
	
		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		private List<string>				m_bonus_items;			// 名産品扱いのアイテム
		private List<string>				m_nanban_trade_items;	// 南蛮品扱いのアイテム
		private List<ItemRank>				m_fishting_ranks;		// 釣りランク
		private List<ItemRank>				m_collect_ranks;		// 採集ランク
		private List<ItemRank>				m_supply_ranks;			// 調達ランク

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public GvoItemTypeDatabase()
		{
			m_bonus_items			= new List<string>();
			m_nanban_trade_items	= new List<string>();

			m_fishting_ranks		= new List<ItemRank>();
			m_collect_ranks			= new List<ItemRank>();
			m_supply_ranks			= new List<ItemRank>();
		}
	
		/*-------------------------------------------------------------------------
		 情報を読み込む
		---------------------------------------------------------------------------*/
		public void Load()
		{
			// 名産品扱いのアイテム一覧読み込み
			load_bonus_items(def.BONUS_ITEMS_FULLFNAME, m_bonus_items);
			// 南蛮品扱いのアイテム一覧読み込み
			load_bonus_items(def.NANBAN_TRADE_ITEMS_FULLFNAME, m_nanban_trade_items);

			// 釣りランク読み込み
			load_bonus_ranks(def.FISHING_RANKS_FULLFNAME, m_fishting_ranks);
			// 採集ランク読み込み
			load_bonus_ranks(def.COLLECT_RANKS_FULLFNAME, m_collect_ranks);
			// 調達ランク読み込み
			load_bonus_ranks(def.SUPPLY_RANKS_FULLFNAME, m_supply_ranks);
		}

		/*-------------------------------------------------------------------------
		 名産品かどうか調べる
		---------------------------------------------------------------------------*/
		public bool IsBonusItem(string name)
		{
			foreach(string s in m_bonus_items){
				if(s == name)	return true;		// 名産品
			}
			foreach(string s in m_nanban_trade_items){
				if(s == name)	return true;		// 南蛮品
			}
			return false;
		}

		/*-------------------------------------------------------------------------
		 南蛮品かどうか調べる
		---------------------------------------------------------------------------*/
		public bool IsNanbanTradeItem(string name)
		{
			foreach(string s in m_nanban_trade_items){
				if(s == name)	return true;		// 南蛮品
			}
			return false;
		}

		/*-------------------------------------------------------------------------
		 釣りランクを得る
		---------------------------------------------------------------------------*/
		public int GetFishingRank(string name)
		{
			foreach(ItemRank i in m_fishting_ranks){
				if(i.Name == name)	return i.Rank;
			}
			return 0;
		}
	
		/*-------------------------------------------------------------------------
		 採集ランクを得る
		---------------------------------------------------------------------------*/
		public int GetSaisyuRank(string name)
		{
			foreach(ItemRank i in m_collect_ranks){
				if(i.Name == name)	return i.Rank;
			}
			return 0;
		}

		/*-------------------------------------------------------------------------
		 調達ランクを得る
		---------------------------------------------------------------------------*/
		public int GetChotatuRank(string name)
		{
			foreach(ItemRank i in m_supply_ranks){
				if(i.Name == name)	return i.Rank;
			}
			return 0;
		}
		
		/*-------------------------------------------------------------------------
		 名産品扱いのアイテム情報を読み込む
		 南蛮交易品含む
		---------------------------------------------------------------------------*/
		private void load_bonus_items(string fname, List<string> list)
		{
			list.Clear();

			string line = "";
			try{
				using (StreamReader	sr	= new StreamReader(
					fname, Encoding.GetEncoding("Shift_JIS"))) {
						while((line = sr.ReadLine()) != null){
						if(line != "")	list.Add(line);
					}
				}
			}catch{
				// 読み込み失敗
				list.Clear();
			}
		}

		/*-------------------------------------------------------------------------
		 釣り、採集、調達 ランク読み込み
		---------------------------------------------------------------------------*/
		private void load_bonus_ranks(string fname, List<ItemRank> list)
		{
			list.Clear();

			string line = "";
			try{
				using (StreamReader	sr	= new StreamReader(
					fname, Encoding.GetEncoding("Shift_JIS"))) {
						while((line = sr.ReadLine()) != null){
						if(line != ""){
							list.Add(new ItemRank(line));
						}
					}
				}
			}catch{
				// 読み込み失敗
			}
		}
	}

	/*-------------------------------------------------------------------------
	 街の同盟状況
	 データの並びは domainsindex.txt	から読み込む
	---------------------------------------------------------------------------*/
	public class GvoDomains : SequentialDictionary<string, GvoDomains.Data>
	{
		// サーバ
		public enum Server{
			Euros,
			Zephyros,
			Notos,
			Boreas,
			max,
		};
		// 国
		public enum Country{
			unknown,			// 所属無(補給港など)
			England,
			Spain,
			Portugal,
			Netherlands,
			France,
			Venezia,
			Turkey,
			max
		};

		// 街の種類
		public enum CityType{
			capital,			// 首都
			city,				// 街
			capital_islam,		// 首都(イスラム)
			city_islam,			// 街(イスラム)
		};
		// 同盟の種類
		public enum AllianceType{
			unknown,			// なし(補給港など)
			alliance,			// 同盟
			capital,			// 首都
			territory,			// 領土
			piratical,			// 海賊島
		};

		// 文化圏
		public enum CulturalSphere{
			unknown,			// 不明
			north_europe,		// 北欧
			germany,			// ドイツ
			netherlands,		// ネーデルランド
			britain,			// ブリテン島
			north_france,		// フランス北部
			iberian,			// イベリア
			atlantic,			// 大西洋
			italy_south_france,	// イタリア・南仏
			balkan,				// バルカン
			turkey,				// トルコ
			near_east,			// 近東
			north_africa,		// 北アフリカ
			west_africa,		// 西アフリカ
			east_africa,		// 東アフリカ
			arab,				// アラブ
			persia,				// ペルシャ
			india,				// インド
			indochina,			// インドシナ
			southeast_asia,		// 東南アジア
			oceania,			// オセアニア
			caribbean,			// カリブ
			east_latin_america,	// 中南米東岸
			west_latin_america,	// 中南米西岸
			china,				// 華南
			japan,				// 日本
			taiwan,				// 台湾
			korea,				// 朝鮮
			north_america,		// 北米
			MAX
		};

		/*-------------------------------------------------------------------------
		 街の同盟状況 1つ
		---------------------------------------------------------------------------*/
		public class Data : IDictionaryNode<string>
		{
			private string					m_name;				// 名前
			private Country[]				m_domains;			// 同盟状況
			private int						m_index;			// 街番号
			private CityType				m_city_type;		// 街の種類
			private AllianceType			m_alliance_type;	// 同盟の種類
			private CulturalSphere			m_cultural_sphere;	// 文化圏
			private bool					m_has_name_image;	// 名前の絵情報を持つときtrue
	
			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public string Key					{	get{	return m_name;				}}
			public string Name					{	get{	return m_name;				}}
			public int Index					{	get{	return m_index;				}}
			public CityType CityType			{	get{	return m_city_type;			}}
			public AllianceType AllianceType	{	get{	return m_alliance_type;		}}
			public CulturalSphere CulturalSphere{	get{	return m_cultural_sphere;	}}
			public bool HasNameImage			{	get{	return m_has_name_image;	}}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public Data(string name, CityType type, AllianceType alliance, CulturalSphere cs, int index, bool _has_name_image)
			{
				m_name				= name;
				m_domains			= new Country[(int)Server.max];
				m_index				= index;
				m_city_type			= type;
				m_alliance_type		= alliance;
				m_cultural_sphere	= cs;
				m_has_name_image	= _has_name_image;

				// すべて不明
				for(int i=0; i<m_domains.Length; i++){
					m_domains[i]	= Country.unknown;
				}
			}

			/*-------------------------------------------------------------------------
			 同盟状況を設定
			---------------------------------------------------------------------------*/
			public void SetDomain(Server server_index, Country country_index)
			{
				// 同盟国のときのみ設定する
				if(AllianceType != AllianceType.alliance)	return;

				m_domains[(int)server_index]	= country_index;
			}

			/*-------------------------------------------------------------------------
			 同盟状況を設定
			 全サーバに同じ値を設定する
			 同盟国を指定できない街用
			---------------------------------------------------------------------------*/
			public void SetDomain(Country country_index)
			{
				// 同盟国のときは一括設定無効
				if(AllianceType == AllianceType.alliance)	return;

				for(int i=0; i<(int)Server.max; i++){
					m_domains[i]	= country_index;
				}
			}

			/*-------------------------------------------------------------------------
			 同盟状況を得る
			---------------------------------------------------------------------------*/
			public Country GetDomain(Server server_index)
			{
				return m_domains[(int)server_index];
			}
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public GvoDomains()
			: base()
		{
		}

		/*-------------------------------------------------------------------------
		 サーバ名を得る
		---------------------------------------------------------------------------*/
		public static string GetServerString(Server _server)
		{
			switch(_server){
			case Server.Euros:			return "euros";
			case Server.Zephyros:		return "zephyros";
			case Server.Notos:			return "notos";
			case Server.Boreas:			return "boreas";
			}
			return "不明なサーバ";
		}

		/*-------------------------------------------------------------------------
		 サーバ名から Server を得る
		---------------------------------------------------------------------------*/
		public static Server GetServerFromString(string str)
		{
			switch(str){
			case "euros":
			case "Euros":
				return Server.Euros;
			case "zephyros":
			case "Zephyros":
				return Server.Zephyros;
			case "notos":
			case "Notos":
				return Server.Notos;
			case "boreas":
			case "Boreas":
				return Server.Boreas;
			}
			return Server.max;
		}

		/*-------------------------------------------------------------------------
		 国名を得る
		---------------------------------------------------------------------------*/
		public static string GetCountryString(Country _country)
		{
			switch(_country){
			case Country.England:		return "イングランド";
			case Country.Spain:			return "イスパニア";
			case Country.Portugal:		return "ポルトガル";
			case Country.Netherlands:	return "ネーデルランド";
			case Country.France:		return "フランス";
			case Country.Venezia:		return "ヴェネツィア";
			case Country.Turkey:		return "オスマントルコ";
			}
			return "所属無";
		}

		/*-------------------------------------------------------------------------
		 街の種類を得る
		---------------------------------------------------------------------------*/
		public static string GetCityTypeString(CityType _city_type)
		{
			switch(_city_type){
			case CityType.capital:			return "首都";
			case CityType.city:				return "街";
			case CityType.capital_islam:	return "首都(イスラム)";
			case CityType.city_islam:		return "街(イスラム)";
			}
			return "不明";
		}

		/*-------------------------------------------------------------------------
		 同盟状況を得る
		---------------------------------------------------------------------------*/
		public static string GetAllianceString(AllianceType _alliance)
		{
			switch(_alliance){
			case AllianceType.unknown:		return "同盟なし";
			case AllianceType.alliance:		return "同盟国";
			case AllianceType.capital:		return "本拠地";
			case AllianceType.territory:	return "領地";
			case AllianceType.piratical:	return "海賊島";
			}
			return "なし";
		}

		/*-------------------------------------------------------------------------
		 文化圏を得る
		---------------------------------------------------------------------------*/
		public static string GetCulturalSphereString(CulturalSphere cs)
		{
			switch(cs){
			case CulturalSphere.unknown:				return "不明";
			case CulturalSphere.north_europe:			return "北欧";
			case CulturalSphere.germany:				return "ドイツ";
			case CulturalSphere.netherlands:			return "ネーデルランド";
			case CulturalSphere.britain:				return "ブリテン島";
			case CulturalSphere.north_france:			return "フランス北部";
			case CulturalSphere.iberian:				return "イベリア";
			case CulturalSphere.atlantic:				return "大西洋";
			case CulturalSphere.italy_south_france:		return "イタリア・南仏";
			case CulturalSphere.balkan:					return "バルカン";
			case CulturalSphere.turkey:					return "トルコ";
			case CulturalSphere.near_east:				return "近東";
			case CulturalSphere.north_africa:			return "北アフリカ";
			case CulturalSphere.west_africa:			return "西アフリカ";
			case CulturalSphere.east_africa:			return "東アフリカ";
			case CulturalSphere.arab:					return "アラブ";
			case CulturalSphere.persia:					return "ペルシャ";
			case CulturalSphere.india:					return "インド";
			case CulturalSphere.indochina:				return "インドシナ";
			case CulturalSphere.southeast_asia:			return "東南アジア";
			case CulturalSphere.oceania:				return "オセアニア";
			case CulturalSphere.caribbean:				return "カリブ";
			case CulturalSphere.east_latin_america:		return "中南米東岸";
			case CulturalSphere.west_latin_america:		return "中南米西岸";
			case CulturalSphere.china:					return "華南";
			case CulturalSphere.japan:					return "日本";
			case CulturalSphere.taiwan:					return "台湾島";
			case CulturalSphere.korea:					return "朝鮮";
			case CulturalSphere.north_america:			return "北米";
			}
			return "不明";
		}

		/*-------------------------------------------------------------------------
		 文化圏用のツールチップを得る
		---------------------------------------------------------------------------*/
		public string GetCulturalSphereTooltip(CulturalSphere cs)
		{
			string	tooltip		= "";
			foreach(Data i in base.m_sequential_database){
				if(i.CulturalSphere == cs){
					tooltip		+= i.Name + "\n";
				}
			}
			return tooltip;
		}

		/*-------------------------------------------------------------------------
		 街を得る
		---------------------------------------------------------------------------*/
		public Data GetCity(string country_name)
		{
			return base.GetValue(country_name);
		}

		/*-------------------------------------------------------------------------
		 同盟国を得る
		---------------------------------------------------------------------------*/
		public Country GetDomain(Server server_index, string country_name)
		{
			Data	d	= GetCity(country_name);
			if(d == null)	return Country.unknown;
			return d.GetDomain(server_index);
		}

		/*-------------------------------------------------------------------------
		 同盟国を変更する
		 変更した場合 true を返す
		---------------------------------------------------------------------------*/
		public bool SetDomain(Server server_index, string country_name, Country _country)
		{
			Data	d	= GetCity(country_name);
			if(d == null)	return false;

			// 同じ場合なにもしない
			if(d.GetDomain(server_index) == _country)		return false;

			// 更新
			d.SetDomain(server_index, _country);
			return true;
		}

		/*-------------------------------------------------------------------------
		 同盟国更新用の文字列を得る
		 "サーバ番号"+"街番号"+"同盟国番号"
		---------------------------------------------------------------------------*/
		public string GetNetUpdateString(Server server_index, string country_name)
		{
			Data	d	= GetCity(country_name);
			if(d == null)	return null;
	
			return ((int)server_index).ToString()
						+ "+" + d.Index.ToString()
						+ "+" + ((int)d.GetDomain(server_index)).ToString();
		}

		/*-------------------------------------------------------------------------
		 順番を読み込む
		 domainsindex.txt
		---------------------------------------------------------------------------*/
		public void LoadIndex(string fname)
		{
			base.Clear();

			string	line	= "";
			int		index	= 0;
			try{
				using (StreamReader	sr	= new StreamReader(
					fname, Encoding.GetEncoding("Shift_JIS"))) {
					while((line = sr.ReadLine()) != null){
						if(line == "")		continue;

						string[]	tmp		= line.Split(new char[]{','});
						string[]	split	= new string[6];

						for(int i=0; i<split.Length; i++){
							split[i]	= "";
						}

						int	max		= split.Length;
						if(tmp.Length < max)	max	= tmp.Length;
						for(int i=0; i<max; i++){
							split[i]	= tmp[i];
						}
						CityType		type			= CityType.city;			// 普通の街
						AllianceType	alliance		= AllianceType.alliance;	// 同盟を結んでる街
						Country			co				= Country.unknown;			// 同盟国不明
						CulturalSphere	cs				= CulturalSphere.unknown;	// 文化圏不明
						bool			has_name_image	= true;

						try{
							type			= CityType.capital + Convert.ToInt32(split[1]);
							alliance		= AllianceType.alliance + Convert.ToInt32(split[2]);
							co				= Country.unknown + Convert.ToInt32(split[3]);
							cs				= CulturalSphere.unknown + Convert.ToInt32(split[4]);
							has_name_image	= (Convert.ToInt32(split[5]) == 0)? false: true;
						}catch{
						}

						Data	d		= new Data(split[0], type, alliance, cs, index++, has_name_image);
						// 同盟国でない場合は旗を指定する
						if(alliance != AllianceType.alliance){
							d.SetDomain(co);
						}
						// 追加
						try{
							base.Add(d);
						}catch{
						}
					}
				}
			}catch{
				// 読み込み失敗
			}
		}

		/*-------------------------------------------------------------------------
		 読み込み
		 サーバからの同盟国受信情報
		---------------------------------------------------------------------------*/
		public void Load()
		{
			string line = "";
			try{
				using (StreamReader	sr	= new StreamReader(
					def.DOMAINS_INFO_FULLFNAME, Encoding.GetEncoding("Shift_JIS"))) {

					int		server_index	= 0;
					while((line = sr.ReadLine()) != null){
						if(line != ""){
							int		max	= line.Length;
							if(max > m_database.Count)	max	= m_database.Count;

							for(int i=0; i<max; i++){
								base.m_sequential_database[i].SetDomain(Server.Euros + server_index,
														Country.unknown + (int)(line[i] - '0'));
							}
							
							if(++server_index >= (int)Server.max){
								break;
							}
						}
					}
				}
			}catch{
				// 読み込み失敗
			}
		}

		/*-------------------------------------------------------------------------
		 書き出し
		---------------------------------------------------------------------------*/
		public void Write()
		{
			string line;
			try{
				using (StreamWriter	sw	= new StreamWriter(
					def.DOMAINS_INFO_FULLFNAME, false, Encoding.GetEncoding("Shift_JIS"))) {

					for(Server	i=Server.Euros; i<Server.max; i++){
						line	= "";
						foreach(Data d in base.m_sequential_database){
							line	+= ((int)d.GetDomain(i)).ToString();
						}
						sw.WriteLine(line);
					}
				}
			}catch{
				// 失敗
			}
		}
	}

	/*-------------------------------------------------------------------------
	 海域情報
	 主に風向き
	 データの並びは SeaInfo.txt	から読み込む
	---------------------------------------------------------------------------*/
	public class GvoSeaInfo : MultiDictionary<string, GvoSeaInfo.Data>
	{
		/*-------------------------------------------------------------------------
		 海域1つ
		---------------------------------------------------------------------------*/
		public class Data : IDictionaryNode<string>
		{
			private string					m_name;
			private Point					m_wind_pos;			// 風向きを描画する位置の中心
			private float					m_summer_angle;		// 夏の風向き
			private float					m_winter_angle;		// 冬の風向き
			private int						m_speedup_rate;		// 最大速度上昇
			private int						m_summer_angle_deg;	// 夏の風向き
			private int						m_winter_angle_deg;	// 夏の風向き
			private string					m_summer_angle_string;	// 16段階の風向き
			private string					m_winter_angle_string;	// 16段階の風向き

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public string Key					{	get{	return m_name;			}}
			public string Name					{	get{	return m_name;			}}
			public Point WindPos				{	get{	return m_wind_pos;		}}
			public float SummerAngle			{	get{	return m_summer_angle;	}}
			public string SummerAngleString		{	get{	return "夏:" + m_summer_angle_string;	}}
			public float WinterAngle			{	get{	return m_winter_angle;	}}
			public string WinterAngleString		{	get{	return "冬:" + m_winter_angle_string;	}}
			public int SpeedUpRate				{	get{	return m_speedup_rate;	}}
			public string SpeedUpRateString		{	get{	return "速度上昇:" + ((SpeedUpRate==0)?"未調査":SpeedUpRate.ToString() + "%");	}}
			public string TooltipString			{
				get{
					string	tip	= "";
					tip			+= SummerAngleString + "\n";
					tip			+= WinterAngleString + "\n";
					tip			+= SpeedUpRateString;
					return tip;
				}
			}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public Data(string name, int x, int y, int summer_angle, int winter_angle, int speedup_rate)
			{
				m_name				= name;
				m_wind_pos			= new Point(x, y);
				m_speedup_rate		= speedup_rate;
				m_summer_angle		= Useful.ToRadian(summer_angle);
				m_winter_angle		= Useful.ToRadian(winter_angle);
				m_summer_angle_deg	= summer_angle;
				m_winter_angle_deg	= winter_angle;

				m_summer_angle_string	= angle_to_string(summer_angle);
				m_winter_angle_string	= angle_to_string(winter_angle);
			}

			/*-------------------------------------------------------------------------
			 角度から文字列に変換する
			 与える角度が逆のため、
			   0度=南からの風
			   90度=西からの風
			   180度=北からの風
			   270度=東からの風
			---------------------------------------------------------------------------*/
			private string angle_to_string(int angle)
			{
				float	tmp	= (float)angle + ((360f/16f)/2);
				if(tmp < 0)	tmp	= 0;
				angle	= (int)(tmp / (360f/16f));
				switch(angle){
				case 0:		return "南の風";
				case 1:		return "南南西の風";
				case 2:		return "南西の風";
				case 3:		return "西南西の風";
				case 4:		return "西の風";
				case 5:		return "西北西の風";
				case 6:		return "北西の風";
				case 7:		return "北北西の風";
				case 8:		return "北の風";
				case 9:		return "北北東の風";
				case 10:	return "北東の風";
				case 11:	return "東北東の風";
				case 12:	return "東の風";
				case 13:	return "東南東の風";
				case 14:	return "南東の風";
				default:	return "南南東の風";
				}
			}
		}
	
		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public GvoSeaInfo()
			: base()
		{
		}

		/*-------------------------------------------------------------------------
		 検索
		---------------------------------------------------------------------------*/
		public Data Find(string name)
		{
			return base.GetValue(name);
		}

		/*-------------------------------------------------------------------------
		 読み込み
		---------------------------------------------------------------------------*/
		public void Load()
		{
			base.Clear();

			string	line	= "";
			try{
				using (StreamReader	sr	= new StreamReader(
					def.SEA_INFO_FULL_NAME, Encoding.GetEncoding("Shift_JIS"))) {
					while((line = sr.ReadLine()) != null){
						if(line == "")		continue;

						string[]	tmp		= line.Split(new char[]{','});
						string[]	split	= new string[6];

						for(int i=0; i<split.Length; i++){
							split[i]	= "";
						}

						int	max		= 6;
						if(tmp.Length < max)	max	= tmp.Length;
						for(int i=0; i<max; i++){
							split[i]	= tmp[i];
						}

						Data	d		= new Data(	split[0],
													Useful.ToInt32(split[1], 0),
													Useful.ToInt32(split[2], 0),
													Useful.ToInt32(split[3], 0),
													Useful.ToInt32(split[4], 0),
													Useful.ToInt32(split[5], 0));
						// 追加
						base.Add(d);
					}
				}
			}catch{
				// 読み込み失敗
			}
		}	
	}
}
