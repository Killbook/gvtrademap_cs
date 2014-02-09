// Type: Utility.file_ctrl
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.IO;

namespace Utility
{
  public static class file_ctrl
  {
    public static bool RemoveFile(string fname)
    {
      try
      {
        if (!File.Exists(fname))
          return true;
        File.Delete(fname);
      }
      catch
      {
        return false;
      }
      return true;
    }

    public static bool CreatePath(string path)
    {
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        if (!directoryInfo.Exists)
          directoryInfo.Create();
      }
      catch
      {
        return false;
      }
      return true;
    }
  }
}
