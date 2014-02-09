// Type: Utility.Ini.IniSettingBase
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Collections.Generic;

namespace Utility.Ini
{
  public abstract class IniSettingBase
  {
    private List<IniSettingBase.SaveLoadNode> m_list;
    protected bool m_enable_duplicate_group_name;

    public bool EnableDuplicateGroupName
    {
      get
      {
        return this.m_enable_duplicate_group_name;
      }
      set
      {
        this.m_enable_duplicate_group_name = value;
      }
    }

    public IniSettingBase()
    {
      this.m_list = new List<IniSettingBase.SaveLoadNode>();
      this.m_enable_duplicate_group_name = false;
    }

    public virtual void AddIniSaveLoad(IIniSaveLoad user, string group)
    {
      if (!this.m_enable_duplicate_group_name && this.is_duplicate_group_name(group))
        throw new Exception(string.Format("[ {0} ]\r\nグル\x30FCプ名が重複しています。", (object) group));
      this.m_list.Add(new IniSettingBase.SaveLoadNode(user, group));
    }

    public virtual void AddIniSaveLoad(IIniSaveLoad user)
    {
      this.AddIniSaveLoad(user, user.DefaultIniGroupName);
    }

    private bool is_duplicate_group_name(string group)
    {
      foreach (IniSettingBase.SaveLoadNode saveLoadNode in this.m_list)
      {
        if (saveLoadNode.Group == group)
          return true;
      }
      return false;
    }

    public abstract void Load();

    protected virtual void Load(IIni ini)
    {
      if (ini == null)
        throw new ArgumentException();
      foreach (IniSettingBase.SaveLoadNode saveLoadNode in this.m_list)
        saveLoadNode.Load(ini);
    }

    public abstract void Save();

    protected virtual void Save(IIni ini)
    {
      if (ini == null)
        throw new ArgumentException();
      foreach (IniSettingBase.SaveLoadNode saveLoadNode in this.m_list)
        saveLoadNode.Save(ini);
    }

    private class SaveLoadNode
    {
      private IIniSaveLoad m_user;
      private string m_group;

      public string Group
      {
        get
        {
          return this.m_group;
        }
      }

      public SaveLoadNode(IIniSaveLoad user, string group)
      {
        if (user == null)
          throw new ArgumentException();
        if (string.IsNullOrEmpty(group))
          throw new ArgumentException();
        this.m_user = user;
        this.m_group = group;
      }

      public void Load(IIni ini)
      {
        if (ini == null)
          throw new ArgumentException();
        this.m_user.IniLoad(ini, this.m_group);
      }

      public void Save(IIni ini)
      {
        if (ini == null)
          throw new ArgumentException();
        this.m_user.IniSave(ini, this.m_group);
      }
    }
  }
}
