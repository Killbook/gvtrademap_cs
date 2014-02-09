// Type: Utility.Ini.IIniSaveLoad
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

namespace Utility.Ini
{
  public interface IIniSaveLoad
  {
    string DefaultIniGroupName { get; }

    void IniLoad(IIni ini, string group);

    void IniSave(IIni ini, string group);
  }
}
