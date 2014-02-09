// Type: gvo_base.gvo_chat_base
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Utility;

namespace gvo_base
{
  public abstract class gvo_chat_base : RequestCtrl
  {
    private readonly object m_syncobject = new object();
    private string m_path;
    private FileInfo m_newest_chat_file_info;
    private FileInfo m_chat_file_info;
    private int m_analyze_lines;
    private List<gvo_chat_base.analize_data> m_analize_list;
    private List<gvo_chat_base.analized_data> m_analized_list;

    public List<gvo_chat_base.analized_data> analized_list
    {
      get
      {
        lock (this.m_syncobject)
          return this.m_analized_list;
      }
      private set
      {
        lock (this.m_syncobject)
          this.m_analized_list = value;
      }
    }

    public string current_log_fullname
    {
      get
      {
        if (this.m_chat_file_info == null)
          return "";
        else
          return this.m_chat_file_info.FullName;
      }
    }

    public string current_log_name
    {
      get
      {
        if (this.m_chat_file_info == null)
          return "";
        else
          return this.m_chat_file_info.Name;
      }
    }

    public bool is_update
    {
      get
      {
        return this.analized_list != null && this.analized_list.Count > 0;
      }
    }

    public string path
    {
      get
      {
        return this.m_path;
      }
      set
      {
        this.m_path = value;
        this.ResetNewestLogInfo();
      }
    }

    public gvo_chat_base()
    {
      this.init(gvo_def.GetGvoLogPath());
    }

    public gvo_chat_base(string path)
    {
      this.init(path);
    }

    private void init(string path)
    {
      this.m_path = path;
      this.m_newest_chat_file_info = (FileInfo) null;
      this.m_chat_file_info = (FileInfo) null;
      this.m_analyze_lines = 0;
      this.m_analize_list = new List<gvo_chat_base.analize_data>();
      this.m_analized_list = (List<gvo_chat_base.analized_data>) null;
    }

    protected void AddAnalizeList(string analize, gvo_chat_base.type _type, object tag)
    {
      this.m_analize_list.Add(new gvo_chat_base.analize_data(analize, _type, tag));
    }

    protected void AddAnalizeList_Interest(object tag)
    {
      this.m_analize_list.Add(new gvo_chat_base.analize_data("預金の利息を", gvo_chat_base.type.index0, tag));
    }

    protected void ClearAnalizeList()
    {
      this.m_analize_list.Clear();
    }

    private FileInfo get_newest_log_file()
    {
      FileInfo[] logFiles = this.GetLogFiles();
      if (logFiles == null)
        return (FileInfo) null;
      if (logFiles.Length <= 0)
        return (FileInfo) null;
      else
        return logFiles[logFiles.Length - 1];
    }

    public FileInfo[] GetLogFiles()
    {
      return gvo_chat_base.GetLogFiles(this.m_path);
    }

    public static FileInfo[] GetLogFiles(string path)
    {
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        if (!directoryInfo.Exists)
          return (FileInfo[]) null;
        FileInfo[] files1 = directoryInfo.GetFiles("*.html");
        FileInfo[] files2 = directoryInfo.GetFiles("*.txt");
        int length = files1.Length;
        Array.Resize<FileInfo>(ref files1, length + files2.Length);
        Array.Copy((Array) files2, 0, (Array) files1, length, files2.Length);
        if (files1.Length <= 0)
          return (FileInfo[]) null;
        gvo_chat_base.SortFileInfo_LastWriteTime(files1);
        return files1;
      }
      catch
      {
        return (FileInfo[]) null;
      }
    }

    public static void SortFileInfo_LastWriteTime(FileInfo[] list)
    {
      if (list == null)
        return;
      Array.Sort<FileInfo>(list, (IComparer<FileInfo>) new gvo_chat_base.file_info_compare());
    }

    public virtual bool AnalyzeNewestChatLog()
    {
      this.ResetAnalizedList();
      if (!this.check_update_log())
        return false;
      List<gvo_chat_base.analized_data> list = new List<gvo_chat_base.analized_data>();
      if (!this.do_analize(this.m_chat_file_info, list, ref this.m_analyze_lines))
        return false;
      this.analized_list = list;
      return true;
    }

    public List<gvo_chat_base.analized_data> AnalyzeChatLog(string fname)
    {
      try
      {
        return this.AnalyzeChatLog(new FileInfo(fname));
      }
      catch
      {
        return (List<gvo_chat_base.analized_data>) null;
      }
    }

    public List<gvo_chat_base.analized_data> AnalyzeChatLog(FileInfo info)
    {
      List<gvo_chat_base.analized_data> list = new List<gvo_chat_base.analized_data>();
      int analyze_lines = 0;
      if (!this.do_analize(info, list, ref analyze_lines))
        list = (List<gvo_chat_base.analized_data>) null;
      return list;
    }

    public void ResetNewestLogInfo()
    {
      this.m_chat_file_info = (FileInfo) null;
    }

    private bool check_update_log()
    {
      if (!this.is_locked_log_file())
        this.m_newest_chat_file_info = this.get_newest_log_file();
      if (this.m_newest_chat_file_info == null)
        return false;
      if (this.m_chat_file_info == null)
      {
        this.m_chat_file_info = this.m_newest_chat_file_info;
        this.m_analyze_lines = 0;
      }
      else if (this.m_newest_chat_file_info.FullName != this.m_chat_file_info.FullName)
      {
        this.m_chat_file_info = this.m_newest_chat_file_info;
        this.m_analyze_lines = 0;
      }
      else
      {
        if (this.m_newest_chat_file_info.Length <= this.m_chat_file_info.Length)
          return false;
        this.m_chat_file_info = this.m_newest_chat_file_info;
      }
      return true;
    }

    private bool is_locked_log_file()
    {
      if (this.m_chat_file_info == null || !this.m_chat_file_info.Exists)
        return false;
      this.m_newest_chat_file_info = new FileInfo(this.m_chat_file_info.FullName);
      try
      {
        this.m_newest_chat_file_info.Refresh();
      }
      catch
      {
        return false;
      }
      try
      {
        using (new FileStream(this.m_newest_chat_file_info.FullName, FileMode.Open, FileAccess.Read, FileShare.None))
          ;
      }
      catch
      {
        return true;
      }
      this.m_newest_chat_file_info = (FileInfo) null;
      return false;
    }

    private bool do_analize(FileInfo file_info, List<gvo_chat_base.analized_data> list, ref int analyze_lines)
    {
      if (file_info == null)
        return false;
      try
      {
        using (FileStream fileStream = new FileStream(file_info.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
          int num = 0;
          bool flag;
          Encoding encoding;
          if (Path.GetExtension(file_info.FullName) == ".txt")
          {
            flag = true;
            encoding = Encoding.GetEncoding("Shift_JIS");
          }
          else
          {
            flag = false;
            encoding = Encoding.UTF8;
          }
          try
          {
            using (StreamReader streamReader = new StreamReader((Stream) fileStream, encoding))
            {
              string line;
              while ((line = streamReader.ReadLine()) != null)
              {
                if (flag)
                {
                  if (num++ < analyze_lines)
                    continue;
                }
                else if (line.Length >= 22 && num++ >= analyze_lines)
                  line = line.Substring(22);
                else
                  continue;
                this.AnalyzeLine(line, list);
              }
              analyze_lines = num;
            }
          }
          catch
          {
            return false;
          }
        }
      }
      catch
      {
        return false;
      }
      return true;
    }

    protected virtual void AnalyzeLine(string line, List<gvo_chat_base.analized_data> list)
    {
      using (List<gvo_chat_base.analize_data>.Enumerator enumerator = this.m_analize_list.GetEnumerator())
      {
        do
          ;
        while (enumerator.MoveNext() && !enumerator.Current.Analize(line, list));
      }
    }

    public void ResetAnalizedList()
    {
      this.analized_list = (List<gvo_chat_base.analized_data>) null;
    }

    public enum type
    {
      index0,
      any_index,
      regex,
    }

    public class analize_data
    {
      private string m_analize;
      private gvo_chat_base.type m_type;
      private Regex m_regex;
      private object m_tag;

      public string analize
      {
        get
        {
          return this.m_analize;
        }
      }

      public gvo_chat_base.type type
      {
        get
        {
          return this.m_type;
        }
      }

      public object tag
      {
        get
        {
          return this.m_tag;
        }
      }

      public analize_data(string analize, gvo_chat_base.type type)
      {
        this.init(analize, type);
      }

      public analize_data(string analize, gvo_chat_base.type type, object tag)
      {
        this.init(analize, type);
        this.m_tag = tag;
      }

      private void init(string analize, gvo_chat_base.type type)
      {
        this.m_analize = analize;
        this.m_type = type;
        this.m_regex = (Regex) null;
        this.m_tag = (object) null;
        if (type != gvo_chat_base.type.regex)
          return;
        this.m_regex = new Regex(analize);
      }

      public bool Analize(string line, List<gvo_chat_base.analized_data> list)
      {
        switch (this.m_type)
        {
          case gvo_chat_base.type.index0:
            if (line.IndexOf(this.m_analize) == 0)
            {
              list.Add(new gvo_chat_base.analized_data(this, line));
              return true;
            }
            else
              break;
          case gvo_chat_base.type.any_index:
            if (line.IndexOf(this.m_analize) >= 0)
            {
              list.Add(new gvo_chat_base.analized_data(this, line));
              return true;
            }
            else
              break;
          case gvo_chat_base.type.regex:
            Match match = this.m_regex.Match(line);
            if (match.Success)
            {
              list.Add(new gvo_chat_base.analized_data(this, line, match));
              return true;
            }
            else
              break;
        }
        return false;
      }
    }

    public class analized_data
    {
      private gvo_chat_base.analize_data m_analize_data;
      private string m_line;
      private Match m_match;

      public string line
      {
        get
        {
          return this.m_line;
        }
      }

      public Match match
      {
        get
        {
          return this.m_match;
        }
      }

      public object tag
      {
        get
        {
          return this.m_analize_data.tag;
        }
      }

      public analized_data(gvo_chat_base.analize_data analize, string line)
      {
        this.m_analize_data = analize;
        this.m_line = line;
        this.m_match = (Match) null;
      }

      public analized_data(gvo_chat_base.analize_data analize, string line, Match match)
      {
        this.m_analize_data = analize;
        this.m_line = line;
        this.m_match = match;
      }
    }

    public class file_info_compare : IComparer<FileInfo>
    {
      public int Compare(FileInfo x, FileInfo y)
      {
        if (x.LastWriteTime < y.LastWriteTime)
          return -1;
        return x.LastWriteTime > y.LastWriteTime ? 1 : 0;
      }
    }
  }
}
