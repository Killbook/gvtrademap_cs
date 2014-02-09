// Type: Utility.KeyAssign.KeyAssignList
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Utility.Ini;

namespace Utility.KeyAssign
{
  public sealed class KeyAssignList : IEnumerable<KeyAssignList.Assign>, IEnumerable
  {
    private List<KeyAssignList.Assign> m_list;
    private KeyAssignRule m_assign_rule;

    public int Count
    {
      get
      {
        return this.m_list.Count;
      }
    }

    public KeyAssignList.Assign this[int index]
    {
      get
      {
        return this.m_list[index];
      }
    }

    public KeyAssignList()
      : this(new KeyAssignRule())
    {
    }

    public KeyAssignList(KeyAssignRule rule)
    {
      this.m_list = new List<KeyAssignList.Assign>();
      this.m_assign_rule = rule;
      if (this.m_assign_rule == null)
        throw new ArgumentNullException("AssignRuleの指定がnullです。");
    }

    public KeyAssignList(KeyAssignList from)
    {
      this.m_list = new List<KeyAssignList.Assign>();
      this.m_assign_rule = from.m_assign_rule;
      foreach (KeyAssignList.Assign from1 in from.m_list)
        this.m_list.Add(new KeyAssignList.Assign(from1));
    }

    public KeyAssignList DeepClone()
    {
      return new KeyAssignList(this);
    }

    public void AddAssign(string name, string group, Keys default_key, object tag, string ini_name)
    {
      this.m_list.Add(new KeyAssignList.Assign(this.m_assign_rule, name, group, default_key, tag, ini_name));
    }

    public void AddAssign(string name, string group, Keys default_key, object tag)
    {
      this.AddAssign(name, group, default_key, tag, tag.ToString());
    }

    public List<string> GetGroupList()
    {
      List<string> list = new List<string>();
      foreach (KeyAssignList.Assign assign in this.m_list)
      {
        if (!this.is_found(assign.Group, list))
          list.Add(assign.Group);
      }
      if (list.Count <= 0)
        return (List<string>) null;
      else
        return list;
    }

    private bool is_found(string str, List<string> list)
    {
      foreach (string str1 in list)
      {
        if (str1 == str)
          return true;
      }
      return false;
    }

    public List<KeyAssignList.Assign> GetAssignedListFromGroup(string group)
    {
      List<KeyAssignList.Assign> list = new List<KeyAssignList.Assign>();
      foreach (KeyAssignList.Assign assign in this.m_list)
      {
        if (assign.Group == group)
          list.Add(assign);
      }
      if (list.Count <= 0)
        return (List<KeyAssignList.Assign>) null;
      else
        return list;
    }

    public List<KeyAssignList.Assign> GetAssignedList(KeyEventArgs e)
    {
      return this.GetAssignedList(e.KeyData);
    }

    public List<KeyAssignList.Assign> GetAssignedList(Keys key)
    {
      List<KeyAssignList.Assign> list = new List<KeyAssignList.Assign>();
      foreach (KeyAssignList.Assign assign in this.m_list)
      {
        if (assign.IsAssignedKey(key))
          list.Add(assign);
      }
      if (list.Count <= 0)
        return (List<KeyAssignList.Assign>) null;
      else
        return list;
    }

    public void DefaultAll()
    {
      foreach (KeyAssignList.Assign assign in this.m_list)
        assign.Default();
    }

    public KeyAssignList.Assign GetAssign(object tag)
    {
      foreach (KeyAssignList.Assign assign in this.m_list)
      {
        if (assign.Tag.Equals(tag))
          return assign;
      }
      return (KeyAssignList.Assign) null;
    }

    public string GetAssignShortcutText(object tag)
    {
      KeyAssignList.Assign assign = this.GetAssign(tag);
      if (assign == null || assign.Keys == Keys.None)
        return "";
      else
        return assign.KeysString;
    }

    public void IniLoad(IIni p, string group)
    {
      if (string.IsNullOrEmpty(group) || p == null)
        return;
      foreach (KeyAssignList.Assign assign in this.m_list)
        assign.IniLoad(p, group);
    }

    public void IniSave(IIni p, string group)
    {
      if (string.IsNullOrEmpty(group) || p == null)
        return;
      foreach (KeyAssignList.Assign assign in this.m_list)
        assign.IniSave(p, group);
    }

    public IEnumerator<KeyAssignList.Assign> GetEnumerator()
    {
      for (int i = 0; i < this.m_list.Count; ++i)
        yield return this.m_list[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    public sealed class Assign
    {
      private KeyAssignRule m_assign_rule;
      private Keys m_keys;
      private Keys m_default_keys;
      private string m_name;
      private string m_group;
      private object m_tag;
      private string m_ini_name;

      public string KeysString
      {
        get
        {
          return this.GetKeysString(this.m_keys);
        }
      }

      public Keys Keys
      {
        get
        {
          return this.m_keys;
        }
        internal set
        {
          this.m_keys = this.CanAssignKeys(value) ? value : Keys.None;
        }
      }

      public Keys DefaultKeys
      {
        get
        {
          return this.m_default_keys;
        }
      }

      public string DefaultKeysString
      {
        get
        {
          return this.GetKeysString(this.m_default_keys);
        }
      }

      public string Name
      {
        get
        {
          return this.m_name;
        }
      }

      public string Group
      {
        get
        {
          return this.m_group;
        }
      }

      public object Tag
      {
        get
        {
          return this.m_tag;
        }
      }

      public string IniName
      {
        get
        {
          return this.m_ini_name;
        }
      }

      internal Assign(KeyAssignRule rule, string name, string group, Keys default_key, object tag, string ini_name)
        : this(rule, name, group, default_key, default_key, tag, ini_name)
      {
      }

      internal Assign(KeyAssignRule rule, string name, string group, Keys default_key, Keys key, object tag, string ini_name)
      {
        if (rule == null)
          throw new ArgumentNullException("ショ\x30FCトカットに割り当てられるかを決定するためにAssignRuleが必要です。");
        this.m_assign_rule = rule;
        this.m_keys = key;
        this.m_default_keys = default_key;
        this.m_name = name;
        this.m_group = group;
        this.m_tag = tag;
        this.m_ini_name = ini_name;
        if (!this.CanAssignKeys(this.m_keys))
          this.m_keys = Keys.None;
        if (this.m_default_keys != Keys.None && !this.CanAssignKeys(this.m_default_keys))
          throw new Exception("割り当てできないキ\x30FCの組み合わせが初期値に指定されました。");
      }

      internal Assign(KeyAssignList.Assign from)
        : this(from.m_assign_rule, from.Name, from.Group, from.DefaultKeys, from.Keys, from.Tag, from.IniName)
      {
      }

      public bool IsAssignedKey(Keys key)
      {
        if (key == Keys.None)
          return false;
        else
          return this.m_keys == key;
      }

      internal void Default()
      {
        this.Keys = this.DefaultKeys;
      }

      internal void IniLoad(IIni p, string group)
      {
        if (string.IsNullOrEmpty(group) || string.IsNullOrEmpty(this.m_ini_name) || p == null)
          return;
        this.Keys = (Keys) p.GetProfile(group, this.m_ini_name, (int) this.m_keys);
      }

      internal void IniSave(IIni p, string group)
      {
        if (string.IsNullOrEmpty(group) || string.IsNullOrEmpty(this.m_ini_name) || p == null)
          return;
        p.SetProfile(group, this.m_ini_name, (int) this.m_keys);
      }

      public string GetKeysString(Keys key)
      {
        return this.m_assign_rule.GetKeysString(key);
      }

      public bool CanAssignKeys(Keys key)
      {
        return this.m_assign_rule.CanAssignKeys(key);
      }
    }
  }
}
