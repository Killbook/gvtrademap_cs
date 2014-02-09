// Type: gvtrademap_cs.GvoItemTypeDatabase
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Xml;
using Utility;

namespace gvtrademap_cs
{
  public class GvoItemTypeDatabase
  {
    private MultiDictionary<string, GvoItemTypeDatabase.ItemRank> m_bonus_items;
    private MultiDictionary<string, GvoItemTypeDatabase.ItemRank> m_nanban_trade_items;
    private MultiDictionary<string, GvoItemTypeDatabase.ItemRank> m_fishting_ranks;
    private MultiDictionary<string, GvoItemTypeDatabase.ItemRank> m_collect_ranks;
    private MultiDictionary<string, GvoItemTypeDatabase.ItemRank> m_supply_ranks;

    public GvoItemTypeDatabase()
    {
      this.m_bonus_items = new MultiDictionary<string, GvoItemTypeDatabase.ItemRank>();
      this.m_nanban_trade_items = new MultiDictionary<string, GvoItemTypeDatabase.ItemRank>();
      this.m_fishting_ranks = new MultiDictionary<string, GvoItemTypeDatabase.ItemRank>();
      this.m_collect_ranks = new MultiDictionary<string, GvoItemTypeDatabase.ItemRank>();
      this.m_supply_ranks = new MultiDictionary<string, GvoItemTypeDatabase.ItemRank>();
    }

    public void Load()
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load("database\\itemtype_db.xml");
        if (xmlDocument.DocumentElement.ChildNodes == null)
          return;
        foreach (XmlNode node in xmlDocument.DocumentElement.ChildNodes)
        {
          if (node.Attributes["name"] != null)
          {
            switch (node.Attributes["name"].Value)
            {
              case "fish_rank":
                this.load_sub(node, this.m_fishting_ranks);
                continue;
              case "collect_ranks":
                this.load_sub(node, this.m_collect_ranks);
                continue;
              case "supply_ranks":
                this.load_sub(node, this.m_supply_ranks);
                continue;
              case "bonus_items":
                this.load_sub(node, this.m_bonus_items);
                continue;
              case "nanban_trade_items":
                this.load_sub(node, this.m_nanban_trade_items);
                continue;
              default:
                continue;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("採取ランク等の読み込みで問題発生");
        Console.Write(ex.Message);
      }
    }

    private void load_sub(XmlNode node, MultiDictionary<string, GvoItemTypeDatabase.ItemRank> list)
    {
      list.Clear();
      if (node == null || node.ChildNodes == null)
        return;
      foreach (XmlNode n in node.ChildNodes)
      {
        GvoItemTypeDatabase.ItemRank t = GvoItemTypeDatabase.ItemRank.FromXml(n);
        if (t != null)
          list.Add(t);
      }
    }

    private void write_xml(string file_name)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.AppendChild((XmlNode) xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", (string) null));
      xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement("itemtype_db_root"));
      this.write_item_ranks(xmlDocument.DocumentElement, "fish_rank", this.m_fishting_ranks);
      this.write_item_ranks(xmlDocument.DocumentElement, "collect_ranks", this.m_collect_ranks);
      this.write_item_ranks(xmlDocument.DocumentElement, "supply_ranks", this.m_supply_ranks);
      this.write_item_ranks(xmlDocument.DocumentElement, "bonus_items", this.m_bonus_items);
      this.write_item_ranks(xmlDocument.DocumentElement, "nanban_trade_items", this.m_nanban_trade_items);
      xmlDocument.Save(file_name);
    }

    private void write_item_ranks(XmlElement p_node, string name, MultiDictionary<string, GvoItemTypeDatabase.ItemRank> list)
    {
      XmlNode node = Useful.XmlAddNode((XmlNode) p_node, "group", name);
      foreach (GvoItemTypeDatabase.ItemRank itemRank in list)
        itemRank.WriteXml(node);
    }

    public bool IsBonusItem(string name)
    {
      if (this.m_bonus_items.GetValue(name) != null)
        return true;
      else
        return this.IsNanbanTradeItem(name);
    }

    public bool IsNanbanTradeItem(string name)
    {
      return this.m_nanban_trade_items.GetValue(name) != null;
    }

    private int get_rank(MultiDictionary<string, GvoItemTypeDatabase.ItemRank> list, string name)
    {
      GvoItemTypeDatabase.ItemRank itemRank = list.GetValue(name);
      if (itemRank == null)
        return 0;
      else
        return itemRank.Rank;
    }

    public int GetFishingRank(string name)
    {
      return this.get_rank(this.m_fishting_ranks, name);
    }

    public int GetSaisyuRank(string name)
    {
      return this.get_rank(this.m_collect_ranks, name);
    }

    public int GetChotatuRank(string name)
    {
      return this.get_rank(this.m_supply_ranks, name);
    }

    public class ItemRank : IDictionaryNode<string>
    {
      private string m_name;
      private int m_rank;

      public string Key
      {
        get
        {
          return this.m_name;
        }
      }

      public string Name
      {
        get
        {
          return this.m_name;
        }
      }

      public int Rank
      {
        get
        {
          return this.m_rank;
        }
      }

      public ItemRank()
      {
        this.m_name = "";
        this.m_rank = 0;
      }

      internal static GvoItemTypeDatabase.ItemRank FromXml(XmlNode n)
      {
        if (n == null)
          return (GvoItemTypeDatabase.ItemRank) null;
        GvoItemTypeDatabase.ItemRank itemRank = new GvoItemTypeDatabase.ItemRank();
        itemRank.m_name = Useful.XmlGetAttribute(n, "name", itemRank.m_name);
        itemRank.m_rank = Useful.ToInt32(Useful.XmlGetAttribute(n, "rank", "0"), 0);
        if (string.IsNullOrEmpty(itemRank.m_name))
          return (GvoItemTypeDatabase.ItemRank) null;
        else
          return itemRank;
      }

      internal void WriteXml(XmlNode node)
      {
        Useful.XmlAddAttribute(Useful.XmlAddNode(node, "item", this.m_name), "rank", this.m_rank.ToString());
      }
    }
  }
}
