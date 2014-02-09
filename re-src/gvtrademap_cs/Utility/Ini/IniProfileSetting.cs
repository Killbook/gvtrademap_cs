// Type: Utility.Ini.IniProfileSetting
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;

namespace Utility.Ini
{
  public class IniProfileSetting : IniSettingBase
  {
    private string m_file_name;

    public IniProfileSetting(string file_name)
    {
      if (string.IsNullOrEmpty(file_name))
        throw new ArgumentException();
      this.m_file_name = file_name;
    }

    public override void Load()
    {
      this.Load((IIni) new IniProfile(this.m_file_name));
    }

    public override void Save()
    {
      this.Save((IIni) new IniProfile(this.m_file_name));
    }
  }
}
