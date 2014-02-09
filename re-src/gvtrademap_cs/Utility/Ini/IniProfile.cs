// Type: Utility.Ini.IniProfile
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Utility.Ini
{
  public class IniProfile : IniBase
  {
    private const int BUFF_LEN = 256;
    private string m_file_name;

    public IniProfile(string file_name)
    {
      this.m_file_name = file_name;
    }

    [DllImport("kernel32.dll")]
    private static uint GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder StringBuilder, uint nSize, string lpFileName);

    [DllImport("kernel32.dll")]
    private static uint WritePrivateProfileString(string lpApplicationName, string lpEntryName, string lpEntryString, string lpFileName);

    protected override bool HasProfile(string group_name, string name)
    {
      return true;
    }

    public override string GetProfile(string group_name, string name, string default_value)
    {
      StringBuilder StringBuilder = new StringBuilder(256);
      int num = (int) IniProfile.GetPrivateProfileString(group_name, name, default_value, StringBuilder, Convert.ToUInt32(StringBuilder.Capacity), this.m_file_name);
      return ((object) StringBuilder).ToString();
    }

    public override string[] GetProfile(string group_name, string name, string[] default_value)
    {
      throw new NotImplementedException();
    }

    public override void SetProfile(string group_name, string name, string value)
    {
      if ((int) IniProfile.WritePrivateProfileString(group_name, name, value, this.m_file_name) == 0)
        throw new Exception("デ\x30FCタの書き込みに失敗");
    }

    public override void SetProfile(string group_name, string name, string[] value)
    {
      throw new NotImplementedException();
    }
  }
}
