// Type: gvo_base.gvo_def
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.IO;
using Utility;

namespace gvo_base
{
  public static class gvo_def
  {
    public const string GVO_CLASS_NAME = "Greate Voyages Online Game MainFrame";
    public const string GVO_WINDOW_NAME = "大航海時代 Online";
    public const string GVO_USERDATA_PATH = "KOEI\\GV Online\\";
    public const string GVO_LOG_PATH = "KOEI\\GV Online\\Log\\Chat\\";
    public const string GVO_MAIL_PATH = "KOEI\\GV Online\\Mail\\";
    public const string GVO_SCREENSHOT_PATH = "KOEI\\GV Online\\ScreenShot\\";
    public const string URL2 = "http://www.umiol.com/db/recipe.php?cmd=recsrc&submit=%B8%A1%BA%F7&recsrckey=";
    public const string URL3 = "http://www.umiol.com/db/recipe.php?cmd=prosrc&submit=%B8%A1%BA%F7&prosrckey=";

    public static string GetGvoLogPath()
    {
      return Path.Combine(Useful.GetMyDocumentPath(), "KOEI\\GV Online\\Log\\Chat\\");
    }

    public static string GetGvoMailPath()
    {
      return Path.Combine(Useful.GetMyDocumentPath(), "KOEI\\GV Online\\Mail\\");
    }

    public static string GetGvoScreenShotPath()
    {
      return Path.Combine(Useful.GetMyDocumentPath(), "KOEI\\GV Online\\ScreenShot\\");
    }
  }
}
