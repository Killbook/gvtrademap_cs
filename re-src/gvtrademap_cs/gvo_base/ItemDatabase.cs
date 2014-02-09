// Type: gvo_base.ItemDatabase
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using Utility;

namespace gvo_base
{
  public class ItemDatabase : MultiDictionary<string, ItemDatabase.Data>
  {
    private Dictionary<string, string> m_ajust_name_list;

    public ItemDatabase()
    {
      this.create_ajust_name_list();
    }

    public ItemDatabase(string fname)
      : this()
    {
      this.Load(fname);
    }

    public void Load(string fname)
    {
      this.Clear();
      try
      {
        using (StreamReader streamReader = new StreamReader(fname, Encoding.GetEncoding("Shift_JIS")))
        {
          string line;
          while ((line = streamReader.ReadLine()) != null)
          {
            ItemDatabase.Data t = new ItemDatabase.Data();
            if (t.CreateFromString(line))
              this.Add(t);
          }
        }
      }
      catch
      {
        this.Clear();
      }
    }

    private void create_ajust_name_list()
    {
      this.m_ajust_name_list = new Dictionary<string, string>();
      this.m_ajust_name_list.Add("ロット(NO.1)", "ロット（NO.1）");
      this.m_ajust_name_list.Add("ロット(NO.2)", "ロット（NO.2）");
      this.m_ajust_name_list.Add("ロット(NO.3)", "ロット（NO.3）");
      this.m_ajust_name_list.Add("ロット(NO.4)", "ロット（NO.4）");
      this.m_ajust_name_list.Add("ロット(NO.5)", "ロット（NO.5）");
      this.m_ajust_name_list.Add("ロット(NO.6)", "ロット（NO.6）");
      this.m_ajust_name_list.Add("ロット(NO.7)", "ロット（NO.7）");
      this.m_ajust_name_list.Add("ロット(NO.8)", "ロット（NO.8）");
      this.m_ajust_name_list.Add("ロット(NO.9)", "ロット（NO.9）");
      this.m_ajust_name_list.Add("ロット(NO.10)", "ロット（NO.10）");
      this.m_ajust_name_list.Add("ロット(NO.11)", "ロット（NO.11）");
      this.m_ajust_name_list.Add("ロット(NO.12)", "ロット（NO.12）");
      this.m_ajust_name_list.Add("ロット(NO.13)", "ロット（NO.13）");
      this.m_ajust_name_list.Add("ロット(NO.14)", "ロット（NO.14）");
      this.m_ajust_name_list.Add("ロット(No.14)", "ロット（NO.14）");
      this.m_ajust_name_list.Add("鉱石精錬の書", "鉱石製錬の書");
      this.m_ajust_name_list.Add("合金製錬の書", "合金精錬の書");
      this.m_ajust_name_list.Add("貴金属の精錬法", "貴金属の製錬法");
      this.m_ajust_name_list.Add("アラブ神獣の像彫刻術", "アラブの神獣の像彫刻術");
      this.m_ajust_name_list.Add("ボンバルタ", "ボンバルダ");
      this.m_ajust_name_list.Add("牛革製ベスト", "牛皮製ベスト");
      this.m_ajust_name_list.Add("花嫁衣装の縫製書", "花嫁衣裳の縫製法");
      this.m_ajust_name_list.Add("小型船用高級上納品の梱包", "小型高級上納品の梱包");
      this.m_ajust_name_list.Add("中型船用高級上納品の梱包", "中型高級上納品の梱包");
      this.m_ajust_name_list.Add("大型船用高級上納品の梱包", "大型高級上納品の梱包");
      this.m_ajust_name_list.Add("高級上納品(小型船用)", "高級上納品（小型船用）");
      this.m_ajust_name_list.Add("高級上納品(中型船用)", "高級上納品（中型船用）");
      this.m_ajust_name_list.Add("高級上納品(大型船用)", "高級上納品（大型船用）");
      this.m_ajust_name_list.Add("全艤装補助帆縫製法", "全艤装補助帆組立法");
      this.m_ajust_name_list.Add("ペットの育て方初級編", "ペットの育て方　初級編");
      this.m_ajust_name_list.Add("セット料理集第1集", "セット料理集第1巻");
      this.m_ajust_name_list.Add("フォルダンミルクレ\x30FCプ", "フォンダン・ミルクレ\x30FCプ");
      this.m_ajust_name_list.Add("果実を使ったお菓子", "果物を使ったお菓子");
      this.m_ajust_name_list.Add("防御職人の工芸技法", "防具職人の工芸技法");
      this.m_ajust_name_list.Add("きのこバタ\x30FCソテ\x30FC", "きのこのバタ\x30FCソテ\x30FC");
      this.m_ajust_name_list.Add("フル\x30FCツ盛り合わせ", "フル\x30FCツの盛り合わせ");
      this.m_ajust_name_list.Add("ブッシュドノエル", "ブッシュ・ド・ノエル");
      this.m_ajust_name_list.Add("実用衣装裁縫術・第1巻", "実用衣装縫製術・第1巻");
      this.m_ajust_name_list.Add("ロ\x30FCマ神話の像彫刻術", "ロ\x30FCマ神話の彫刻術");
      this.m_ajust_name_list.Add("一味違う！手作り小物", "一味違う！　手作り小物");
      this.m_ajust_name_list.Add("ゲルマン諸語", "ゲルマン諸語翻訳メモ");
      this.m_ajust_name_list.Add("東欧諸語", "東欧諸語翻訳メモ");
      this.m_ajust_name_list.Add("ロマンス諸語", "ロマンス諸語翻訳メモ");
      this.m_ajust_name_list.Add("アルタイ諸語", "アルタイ諸語翻訳メモ");
      this.m_ajust_name_list.Add("セム・ハム諸語", "セム・ハム諸語翻訳メモ");
      this.m_ajust_name_list.Add("アメリカ諸語", "アメリカ諸語翻訳メモ");
      this.m_ajust_name_list.Add("アフリカ諸語", "アフリカ諸語翻訳メモ");
      this.m_ajust_name_list.Add("インド洋諸語", "インド洋諸語翻訳メモ");
      this.m_ajust_name_list.Add("デミ・カルヴァリン砲10門", "デミ・カルヴァリン10門");
      this.m_ajust_name_list.Add("デミ・カルヴァリン砲12門", "デミ・カルヴァリン12門");
      this.m_ajust_name_list.Add("デミ・カルヴァリン砲14門", "デミ・カルヴァリン14門");
      this.m_ajust_name_list.Add("デミ・カルヴァリン砲16門", "デミ・カルヴァリン16門");
      this.m_ajust_name_list.Add("漁師の心得・鮮魚保存法", "漁師の心得　鮮魚保存法");
      this.m_ajust_name_list.Add("ファルコン2砲", "ファルコン砲2門");
      this.m_ajust_name_list.Add("ファルコン4砲", "ファルコン砲4門");
      this.m_ajust_name_list.Add("ファルコン6砲", "ファルコン砲6門");
      this.m_ajust_name_list.Add("ファルコン8砲", "ファルコン砲8門");
      this.m_ajust_name_list.Add("マクラジャボトル", "マラクジャボトル");
      this.m_ajust_name_list.Add("マクラジャジュ\x30FCス", "マラクジャジュ\x30FCス");
      this.m_ajust_name_list.Add("軽量シ\x30FCダ板", "軽量シ\x30FCダ\x30FC板");
      this.m_ajust_name_list.Add("小袖♂", "小袖");
      this.m_ajust_name_list.Add("小袖♀", "小袖");
      this.m_ajust_name_list.Add("折鳥帽子♂", "折烏帽子");
      this.m_ajust_name_list.Add("かんざし♀", "かんざし");
      this.m_ajust_name_list.Add("通天冠♂", "通天冠");
      this.m_ajust_name_list.Add("歩揺♀", "歩揺");
      this.m_ajust_name_list.Add("四方平定巾♂", "四方平定巾");
      this.m_ajust_name_list.Add("窄袖衫襦♀", "窄袖衫襦");
      this.m_ajust_name_list.Add("直裾深衣♂", "直裾深衣");
    }

    public ItemDatabase.Data Find(string name)
    {
      name = this.adjust_name(name);
      return this.GetValue(name);
    }

    public ItemDatabase.Data Find(int id)
    {
      IEnumerator<ItemDatabase.Data> enumerator = this.GetEnumerator();
      while (enumerator.MoveNext())
      {
        if (enumerator.Current.Id == id)
          return enumerator.Current;
      }
      return (ItemDatabase.Data) null;
    }

    private string adjust_name(string name)
    {
      int length = name.IndexOf("★");
      if (length >= 0)
        name = name.Substring(0, length);
      string str;
      if (this.m_ajust_name_list.TryGetValue(name, out str))
        name = str;
      return name;
    }

    public static ItemDatabase.Categoly GetCategolyFromType(string name)
    {
      switch (name)
      {
        case "食料品":
        case "調味料":
        case "雑貨":
        case "医薬品":
        case "家畜":
          return ItemDatabase.Categoly.Categoly1;
        case "酒類":
        case "鉱石":
        case "染料":
        case "工業品":
        case "嗜好品":
          return ItemDatabase.Categoly.Categoly2;
        case "繊維":
        case "織物":
        case "武具":
        case "火器":
        case "工芸品":
        case "美術品":
          return ItemDatabase.Categoly.Categoly3;
        case "香辛料":
        case "貴金属":
        case "香料":
        case "宝石":
          return ItemDatabase.Categoly.Categoly4;
        default:
          return ItemDatabase.Categoly.Unknown;
      }
    }

    public static Color GetCategolyColor(ItemDatabase.Categoly cate)
    {
      switch (cate)
      {
        case ItemDatabase.Categoly.Categoly1:
          return Color.Gray;
        case ItemDatabase.Categoly.Categoly2:
          return Color.OrangeRed;
        case ItemDatabase.Categoly.Categoly3:
          return Color.Green;
        case ItemDatabase.Categoly.Categoly4:
          return Color.Blue;
        default:
          return Color.Black;
      }
    }

    public static ItemDatabase.TypeGroup GetTypeGroupFromType(string name)
    {
      switch (name)
      {
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
          return ItemDatabase.TypeGroup.Trade;
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
          return ItemDatabase.TypeGroup.Item;
        case "小型帆船":
        case "中小型帆船":
        case "中型帆船":
        case "中大型帆船":
        case "大型帆船":
        case "中小型ガレ\x30FC":
        case "中型ガレ\x30FC":
        case "中大型ガレ\x30FC":
        case "大型ガレ\x30FC":
          return ItemDatabase.TypeGroup.Ship;
        case "船首像":
        case "追加装甲":
        case "特殊兵装":
        case "補助帆":
        case "舷側砲":
        case "船首砲":
        case "船尾砲":
        case "紋章":
          return ItemDatabase.TypeGroup.Rigging;
        case "頭装備品":
        case "体装備品":
        case "足装備品":
        case "手装備品":
        case "武器・道具":
        case "装身具":
          return ItemDatabase.TypeGroup.Equip;
        case "冒険スキル":
        case "交易スキル":
        case "海事スキル":
        case "言語スキル":
        case "アイテム効果スキル":
        case "副官スキル":
        case "船スキルスキル":
          return ItemDatabase.TypeGroup.Skill;
        case "報告":
          return ItemDatabase.TypeGroup.Report;
        case "陸戦テクニック":
          return ItemDatabase.TypeGroup.Technic;
        default:
          return ItemDatabase.TypeGroup.Unknown;
      }
    }

    public static ItemDatabase.TypeGroup2 GetTypeGroupFromType2(string name)
    {
      switch (name)
      {
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
          return ItemDatabase.TypeGroup2.Trade;
        case "消耗品":
        case "推薦状":
        case "レシピ帳":
        case "宝箱":
        case "ロット":
        case "素材":
        case "ペット権利書":
        case "家具":
          return ItemDatabase.TypeGroup2.Item;
        case "小型帆船":
        case "中小型帆船":
        case "中型帆船":
        case "中大型帆船":
        case "大型帆船":
        case "中小型ガレ\x30FC":
        case "中型ガレ\x30FC":
        case "中大型ガレ\x30FC":
        case "大型ガレ\x30FC":
          return ItemDatabase.TypeGroup2.Ship;
        case "船首像":
        case "追加装甲":
        case "特殊兵装":
        case "補助帆":
        case "舷側砲":
        case "船首砲":
        case "船尾砲":
        case "紋章":
          return ItemDatabase.TypeGroup2.Item;
        case "頭装備品":
        case "体装備品":
        case "足装備品":
        case "手装備品":
        case "武器・道具":
        case "装身具":
          return ItemDatabase.TypeGroup2.Item;
        default:
          return ItemDatabase.TypeGroup2.Unknown;
      }
    }

    public static string ToString(ItemDatabase.TypeGroup tg)
    {
      switch (tg)
      {
        case ItemDatabase.TypeGroup.All:
          return "全ての種類";
        case ItemDatabase.TypeGroup.CityName:
          return "街名等";
        case ItemDatabase.TypeGroup.UseLang:
          return "使用言語";
        case ItemDatabase.TypeGroup.Trade:
          return "交易品";
        case ItemDatabase.TypeGroup.Item:
          return "アイテム";
        case ItemDatabase.TypeGroup.Equip:
          return "装備";
        case ItemDatabase.TypeGroup.Ship:
          return "船";
        case ItemDatabase.TypeGroup.Rigging:
          return "艤装";
        case ItemDatabase.TypeGroup.Skill:
          return "スキル";
        case ItemDatabase.TypeGroup.Report:
          return "報告";
        case ItemDatabase.TypeGroup.Technic:
          return "陸戦テクニック";
        default:
          return "不明";
      }
    }

    private void technic_cnv()
    {
      string path = "database\\tec_wiki.txt";
      try
      {
        using (StreamReader streamReader = new StreamReader(path, Encoding.GetEncoding("Shift_JIS")))
        {
          string str1;
          while ((str1 = streamReader.ReadLine()) != null)
          {
            if (!(str1 == ""))
            {
              if (str1.IndexOf("**") == 0)
              {
                str1.Substring(2);
              }
              else
              {
                string[] split = str1.Replace("&br;", "").Split(new char[1]
                {
                  '|'
                }, StringSplitOptions.None);
                if (split.Length >= 1 && !(split[1] == "CENTER:") && (!(split[1] == "スキル名") && !(split[1] == "BGCOLOR(#FFE9DD):")) && !(split[1] == ""))
                {
                  if (split.Length == 10)
                  {
                    foreach (string str2 in this.create_document0(split))
                      ;
                  }
                  else if (split.Length == 11)
                  {
                    foreach (string str2 in this.create_document0(split))
                      ;
                  }
                }
              }
            }
          }
        }
      }
      catch
      {
        this.Clear();
      }
    }

    private string[] create_document0(string[] split)
    {
      List<string> list = new List<string>();
      string str = "Rank:" + split[2] + " " + "消費:" + split[4] + " " + "射程:" + split[5] + " " + "範囲:" + split[6] + " ";
      list.Add(str);
      return list.ToArray();
    }

    public static string ToString(ItemDatabase.TypeGroup2 tg)
    {
      switch (tg)
      {
        case ItemDatabase.TypeGroup2.Trade:
          return "交易品";
        case ItemDatabase.TypeGroup2.Item:
          return "アイテム";
        case ItemDatabase.TypeGroup2.Ship:
          return "船";
        default:
          return "不明";
      }
    }

    public bool MergeShipPartsDatabase(ShipPartsDataBase db)
    {
      if (db == null)
        return false;
      foreach (ShipPartsDataBase.ShipPart i in db.PartsList)
      {
        ItemDatabase.Data data = this.GetValue(i.Name);
        if (data != null)
          data.MergeShipPartsDatabase(i);
      }
      return true;
    }

    public enum Categoly
    {
      Categoly1,
      Categoly2,
      Categoly3,
      Categoly4,
      Unknown,
    }

    public enum TypeGroup
    {
      All,
      CityName,
      UseLang,
      Trade,
      Item,
      Equip,
      Ship,
      Rigging,
      Skill,
      Report,
      Technic,
      Unknown,
    }

    public enum TypeGroup2
    {
      Trade,
      Item,
      Ship,
      Unknown,
    }

    public class Data : IDictionaryNode<string>
    {
      private int m_id;
      private string m_name;
      private string m_type;
      private string m_document;
      private ItemDatabase.Categoly m_categoly;
      private ItemDatabase.TypeGroup m_type_group;
      private ItemDatabase.TypeGroup2 m_type_group2;
      private bool m_is_combat_item;

      public string Key
      {
        get
        {
          return this.m_name;
        }
      }

      public int Id
      {
        get
        {
          return this.m_id;
        }
      }

      public string Name
      {
        get
        {
          return this.m_name;
        }
      }

      public string Type
      {
        get
        {
          return this.m_type;
        }
      }

      public string Document
      {
        get
        {
          return this.m_document;
        }
      }

      public bool IsRecipe
      {
        get
        {
          return this.Type == "レシピ帳";
        }
      }

      public bool IsSkill
      {
        get
        {
          return this.Type == "冒険スキル" || this.Type == "交易スキル" || (this.Type == "海事スキル" || this.Type == "言語スキル");
        }
      }

      public bool IsReport
      {
        get
        {
          return this.Type == "報告";
        }
      }

      public ItemDatabase.Categoly Categoly
      {
        get
        {
          return this.m_categoly;
        }
      }

      public Color CategolyColor
      {
        get
        {
          return ItemDatabase.GetCategolyColor(this.m_categoly);
        }
      }

      public ItemDatabase.TypeGroup TypeGroup
      {
        get
        {
          return this.m_type_group;
        }
      }

      public bool IsCombatItem
      {
        get
        {
          return this.m_is_combat_item;
        }
      }

      public ItemDatabase.TypeGroup2 TypeGroup2
      {
        get
        {
          return this.m_type_group2;
        }
      }

      public string TypeGroup2Str
      {
        get
        {
          return ItemDatabase.ToString(this.m_type_group2);
        }
      }

      public bool CreateFromString(string line)
      {
        string[] strArray = line.Split(new char[1]
        {
          ','
        });
        if (strArray.Length < 4)
          return false;
        try
        {
          this.m_id = Useful.ToInt32(strArray[0].Trim(), 0);
          this.m_type = strArray[1].Trim();
          this.m_name = strArray[2].Trim();
          this.m_document = "";
          for (int index = 3; index < strArray.Length; ++index)
          {
            ItemDatabase.Data data = this;
            string str = data.m_document + strArray[index].Trim() + "\n";
            data.m_document = str;
          }
          if (this.m_document.IndexOf("再使用時間：") >= 0)
            this.m_is_combat_item = true;
          this.m_categoly = ItemDatabase.GetCategolyFromType(this.m_type);
          this.m_type_group = ItemDatabase.GetTypeGroupFromType(this.m_type);
          this.m_type_group2 = ItemDatabase.GetTypeGroupFromType2(this.m_type);
        }
        catch
        {
          return false;
        }
        return true;
      }

      public virtual string GetToolTipString()
      {
        return "名称:" + this.Name + "\n" + "種類:" + this.Type + "\n" + "説明:\n" + this.Document;
      }

      public void OpenRecipeWiki0()
      {
        Process.Start("http://www.umiol.com/db/recipe.php?cmd=recsrc&submit=%B8%A1%BA%F7&recsrckey=" + Useful.UrlEncodeEUCJP(this.Name));
      }

      public void OpenRecipeWiki1()
      {
        Process.Start("http://www.umiol.com/db/recipe.php?cmd=prosrc&submit=%B8%A1%BA%F7&prosrckey=" + Useful.UrlEncodeEUCJP(this.Name));
      }

      internal void MergeShipPartsDatabase(ShipPartsDataBase.ShipPart i)
      {
        if (i == null)
          return;
        ItemDatabase.Data data = this;
        string str = data.m_document + i.ToStringParamsOnly();
        data.m_document = str;
      }
    }
  }
}
