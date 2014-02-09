// Type: Utility.HttpDownload
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.IO;
using System.Net;
using System.Text;

namespace Utility
{
  public static class HttpDownload
  {
    public static bool Download(string url, string write_file_name)
    {
      try
      {
        HttpWebResponse httpWebResponse = (HttpWebResponse) WebRequest.Create(url).GetResponse();
        using (Stream responseStream = httpWebResponse.GetResponseStream())
        {
          using (FileStream fileStream = new FileStream(write_file_name, FileMode.Create, FileAccess.Write))
          {
            byte[] buffer = new byte[8192];
            int count;
            while ((count = responseStream.Read(buffer, 0, buffer.Length)) != 0)
              fileStream.Write(buffer, 0, count);
          }
        }
        httpWebResponse.Close();
      }
      catch
      {
        return false;
      }
      return true;
    }

    public static string Download(string url, Encoding encoder)
    {
      try
      {
        HttpWebResponse httpWebResponse = (HttpWebResponse) WebRequest.Create(url).GetResponse();
        string str;
        using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), encoder))
          str = streamReader.ReadToEnd();
        httpWebResponse.Close();
        return str;
      }
      catch
      {
        return (string) null;
      }
    }
  }
}
