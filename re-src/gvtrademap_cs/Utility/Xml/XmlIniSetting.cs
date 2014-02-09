// Type: Utility.Xml.XmlIniSetting
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using Utility.Ini;

namespace Utility.Xml
{
  public class XmlIniSetting : IniSettingBase
  {
    private string m_file_name;
    private string m_id;

    public XmlIniSetting(string file_name, string id)
    {
      if (string.IsNullOrEmpty(file_name))
        throw new ArgumentException();
      if (string.IsNullOrEmpty(id))
        throw new ArgumentException();
      this.m_file_name = file_name;
      this.m_id = id;
    }

    public override void Load()
    {
      this.Load((IIni) new XmlIni(this.m_file_name, this.m_id));
    }

    public override void Save()
    {
      XmlIni xmlIni = new XmlIni(this.m_id);
      this.Save((IIni) xmlIni);
      xmlIni.Save(this.m_file_name);
    }
  }
}
