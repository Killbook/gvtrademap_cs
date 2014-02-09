// Type: Utility.Useful
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

namespace Utility
{
  public static class Useful
  {
    public static string GetMyDocumentPath()
    {
      return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    }

    public static bool SetFontMeiryo(Control ctrl, float point)
    {
      try
      {
        Font font = new Font("メイリオ", point, FontStyle.Regular, GraphicsUnit.Point, (byte) sbyte.MinValue);
        if (font.Name != "メイリオ")
          return false;
        ctrl.Font = font;
      }
      catch
      {
        return false;
      }
      return true;
    }

    public static void SetFontMeiryo(Form form, float point)
    {
      foreach (Control ctrl in (ArrangedElementCollection) form.Controls)
        Useful.SetFontMeiryo(ctrl, point);
    }

    public static string UrlEncodeUTF8(string str)
    {
      return HttpUtility.UrlEncode(str, Encoding.UTF8);
    }

    public static string UrlEncodeEUCJP(string str)
    {
      return HttpUtility.UrlEncode(str, Encoding.GetEncoding("euc-jp"));
    }

    public static string UrlEncodeShiftJis(string str)
    {
      return HttpUtility.UrlEncode(str, Encoding.GetEncoding("shift_jis"));
    }

    public static float ToDegree(float radian)
    {
      return radian * 57.29578f;
    }

    public static float ToRadian(float degree)
    {
      // ISSUE: unable to decompile the method.
    }

    public static string ToShortDateTimeString(DateTime dt)
    {
      return dt.ToString("yyyy/MM/dd") + dt.ToString(" HH:mm");
    }

    public static string TojbbsDateTimeString(DateTime dt)
    {
      string str = dt.ToString("yyyy/MM/dd", (IFormatProvider) DateTimeFormatInfo.InvariantInfo);
      switch (dt.DayOfWeek)
      {
        case DayOfWeek.Sunday:
          str = str + "(日)";
          break;
        case DayOfWeek.Monday:
          str = str + "(月)";
          break;
        case DayOfWeek.Tuesday:
          str = str + "(火)";
          break;
        case DayOfWeek.Wednesday:
          str = str + "(水)";
          break;
        case DayOfWeek.Thursday:
          str = str + "(木)";
          break;
        case DayOfWeek.Friday:
          str = str + "(金)";
          break;
        case DayOfWeek.Saturday:
          str = str + "(土)";
          break;
      }
      return str + dt.ToString(" HH:mm:ss");
    }

    public static string AdjustNumber(string str)
    {
      str = str.Replace('０', '0');
      str = str.Replace('１', '1');
      str = str.Replace('２', '2');
      str = str.Replace('３', '3');
      str = str.Replace('４', '4');
      str = str.Replace('５', '5');
      str = str.Replace('６', '6');
      str = str.Replace('７', '7');
      str = str.Replace('８', '8');
      str = str.Replace('９', '9');
      return str;
    }

    public static DateTime ToDateTime(string datetime)
    {
      try
      {
        Match match1;
        if ((match1 = Useful.match("([0-9]+)/([0-9]+)/([0-9]+).* ([0-9]+):([0-9]+):([0-9]+)", datetime)) != null)
          return new DateTime(Convert.ToInt32(match1.Groups[1].Value), Convert.ToInt32(match1.Groups[2].Value), Convert.ToInt32(match1.Groups[3].Value), Convert.ToInt32(match1.Groups[4].Value), Convert.ToInt32(match1.Groups[5].Value), Convert.ToInt32(match1.Groups[6].Value), (Calendar) new GregorianCalendar());
        Match match2;
        if ((match2 = Useful.match("([0-9]+)/([0-9]+)/([0-9]+) ([0-9]+):([0-9]+)", datetime)) != null)
          return new DateTime(Convert.ToInt32(match2.Groups[1].Value), Convert.ToInt32(match2.Groups[2].Value), Convert.ToInt32(match2.Groups[3].Value), Convert.ToInt32(match2.Groups[4].Value), Convert.ToInt32(match2.Groups[5].Value), 0, (Calendar) new GregorianCalendar());
        else
          return new DateTime();
      }
      catch
      {
        return new DateTime();
      }
    }

    public static Match match(string regex, string str)
    {
      Match match = Regex.Match(str, regex);
      if (!match.Success)
        return (Match) null;
      else
        return match;
    }

    public static string ToComma3(int num)
    {
      try
      {
        return string.Format("{0:#,0}", (object) num);
      }
      catch
      {
        return "0";
      }
    }

    public static string ToComma3(long num)
    {
      try
      {
        return string.Format("{0:#,0}", (object) num);
      }
      catch
      {
        return "0";
      }
    }

    public static bool LoadReferencedAssembly(Assembly ass, out AssemblyName error_ass)
    {
      error_ass = (AssemblyName) null;
      if (ass == null)
        return false;
      foreach (AssemblyName assemblyRef in ass.GetReferencedAssemblies())
      {
        try
        {
          Assembly.Load(assemblyRef);
        }
        catch
        {
          error_ass = assemblyRef;
          return false;
        }
      }
      return true;
    }

    public static string GetOsName(OperatingSystem os)
    {
      switch (os.Platform)
      {
        case PlatformID.Win32S:
          return "Win32s";
        case PlatformID.Win32Windows:
          if (os.Version.Major >= 4)
          {
            switch (os.Version.Minor)
            {
              case 0:
                return "Windows 95";
              case 10:
                return "Windows 98";
              case 90:
                return "Windows Me";
            }
          }
          else
            break;
        case PlatformID.Win32NT:
          switch (os.Version.Major)
          {
            case 3:
              switch (os.Version.Minor)
              {
                case 0:
                  return "Windows NT 3";
                case 1:
                  return "Windows NT 3.1";
                case 5:
                  return "Windows NT 3.5";
                case 51:
                  return "Windows NT 3.51";
              }
            case 4:
              if (os.Version.Minor == 0)
                return "Windows NT 4.0";
              else
                break;
            case 5:
              switch (os.Version.Minor)
              {
                case 0:
                  return "Windows 2000";
                case 1:
                  return "Windows XP";
                case 2:
                  return "Windows Server 2003";
              }
            case 6:
              switch (os.Version.Minor)
              {
                case 0:
                  return "Windows Vista or Windows Server 2008";
                case 1:
                  return "Windows 7 or Windows Server 2008 R2";
              }
          }
        case PlatformID.WinCE:
          return "Windows CE";
        case PlatformID.Unix:
          return "Unix";
        case PlatformID.Xbox:
          return "Xbox 360";
        case PlatformID.MacOSX:
          return "Macintosh";
      }
      return "Unknown OS";
    }

    public static string ExecCMD(string Argments)
    {
      Process process = Process.Start(new ProcessStartInfo()
      {
        FileName = Environment.GetEnvironmentVariable("ComSpec"),
        RedirectStandardInput = false,
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true,
        Arguments = Argments
      });
      string str = process.StandardOutput.ReadToEnd();
      process.WaitForExit();
      return str;
    }

    public static object ToEnum(System.Type enumType, object value)
    {
      if (value == null)
        return (object) null;
      if (Enum.IsDefined(enumType, value))
        return Enum.Parse(enumType, value.ToString());
      else
        return (object) null;
    }

    public static object ToEnum(System.Type enumType, object value, object default_value)
    {
      return Useful.ToEnum(enumType, value) ?? default_value;
    }

    public static void UpdateListViewSubItemToolTip(ListView listview, ToolTip tooltip, int mouse_x, int mouse_y)
    {
      if (listview == null || tooltip == null)
        return;
      ListViewItem itemAt = listview.GetItemAt(mouse_x, mouse_y);
      ListViewHitTestInfo listViewHitTestInfo = listview.HitTest(mouse_x, mouse_y);
      string str = "";
      if (itemAt != null && listViewHitTestInfo.SubItem != null)
      {
        str = listViewHitTestInfo.SubItem.Tag as string;
        if (string.IsNullOrEmpty(str))
          str = "";
      }
      Useful.set_tooltip(tooltip, (Control) listview, str);
    }

    private static void set_tooltip(ToolTip tooltip, Control control, string str)
    {
      if (!(tooltip.GetToolTip(control) != str))
        return;
      tooltip.SetToolTip(control, str);
    }

    public static int ToInt32(string _from, int defalte_value)
    {
      int result;
      if (!int.TryParse(_from, out result))
        return defalte_value;
      else
        return result;
    }

    public static bool ToBool(string _from, bool defalte_value)
    {
      _from = _from.ToLower();
      if (_from == "true" || _from == "1")
        return true;
      if (_from == "false" || _from == "0")
        return false;
      else
        return defalte_value;
    }

    public static Point ToPoint(string x, string y, Point defalte_value)
    {
      int result1;
      int result2;
      if (!int.TryParse(x, out result1) || !int.TryParse(y, out result2))
        return defalte_value;
      else
        return new Point(result1, result2);
    }

    public static void XmlAddAttribute(XmlNode node, string attri_name, string value)
    {
      if (string.IsNullOrEmpty(value))
        return;
      value = value.Trim();
      if (string.IsNullOrEmpty(value))
        return;
      XmlAttribute attribute = node.OwnerDocument.CreateAttribute(attri_name);
      attribute.Value = value;
      node.Attributes.Append(attribute);
    }

    public static XmlNode XmlAddNode(XmlNode p_node, string node_name, string name)
    {
      if (string.IsNullOrEmpty(name))
        return (XmlNode) null;
      name = name.Trim();
      if (string.IsNullOrEmpty(name))
        return (XmlNode) null;
      XmlNode node = Useful.XmlAddNode(p_node, node_name);
      Useful.XmlAddAttribute(node, "name", name);
      return node;
    }

    public static XmlNode XmlAddNode(XmlNode p_node, string node_name)
    {
      XmlNode newChild = (XmlNode) p_node.OwnerDocument.CreateElement(node_name);
      p_node.AppendChild(newChild);
      return newChild;
    }

    public static XmlNode XmlAddPoint(XmlNode p_node, string node_name, Point p)
    {
      XmlNode node = Useful.XmlAddNode(p_node, node_name);
      Useful.XmlAddAttribute(node, "x", p.X.ToString());
      Useful.XmlAddAttribute(node, "y", p.Y.ToString());
      return node;
    }

    public static Point XmlGetPoint(XmlNode p_node, string node_name, Point default_p)
    {
      if (p_node == null)
        return default_p;
      XmlNode xmlNode = (XmlNode) p_node[node_name];
      if (xmlNode == null)
        return default_p;
      XmlAttribute xmlAttribute1 = xmlNode.Attributes["x"];
      XmlAttribute xmlAttribute2 = xmlNode.Attributes["y"];
      if (xmlAttribute1 == null || xmlAttribute2 == null)
        return default_p;
      else
        return Useful.ToPoint(xmlAttribute1.Value, xmlAttribute2.Value, default_p);
    }

    public static string XmlGetAttribute(XmlNode node, string attri_name, string default_value)
    {
      if (node == null)
        throw new ArgumentNullException();
      if (node.Attributes[attri_name] == null)
        return default_value;
      else
        return node.Attributes[attri_name].Value;
    }

    public static void XmlRemoveNodeWhenEmpty(XmlNode p_node, XmlNode node)
    {
      if (node.Attributes.Count > 0 || node.ChildNodes.Count < 0)
        return;
      p_node.RemoveChild(node);
    }

    public static string XmlGetFirstText(XmlNode node)
    {
      if (node == null)
        return (string) null;
      if (!node.HasChildNodes)
        return (string) null;
      foreach (XmlNode xmlNode in node.ChildNodes)
      {
        if (xmlNode is XmlText)
          return xmlNode.Value;
      }
      return (string) null;
    }

    public static string XmlGetElementText(XmlNode parent, string name)
    {
      if (parent == null)
        return "";
      XmlNode node = (XmlNode) parent[name];
      if (node == null)
        return "";
      string firstText = Useful.XmlGetFirstText(node);
      if (!string.IsNullOrEmpty(firstText))
        return firstText;
      else
        return "";
    }

    public static XmlNode[] XmlGetElements(XmlNode parent, string name)
    {
      if (parent == null)
        return (XmlNode[]) new XmlElement[0];
      List<XmlNode> list = new List<XmlNode>();
      foreach (XmlNode xmlNode in parent.ChildNodes)
      {
        if (xmlNode.Name == name)
          list.Add(xmlNode);
      }
      return list.ToArray();
    }

    public static XmlNode XmlGetElement(XmlNode parent, string name, string name2)
    {
      if (parent == null)
        return (XmlNode) null;
      foreach (XmlNode node in parent.ChildNodes)
      {
        if (node.Name == name && Useful.XmlGetAttribute(node, "name", "") == name2)
          return node;
      }
      return (XmlNode) null;
    }

    public static XmlDocument XmlCreateXml(string root_name, string version)
    {
      if (string.IsNullOrEmpty(root_name))
        return (XmlDocument) null;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.AppendChild((XmlNode) xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", (string) null));
      xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement(root_name));
      if (!string.IsNullOrEmpty(version))
        Useful.XmlAddAttribute((XmlNode) xmlDocument.DocumentElement, "version", version);
      return xmlDocument;
    }

    public static XmlDocument XmlLoadXml(string file_name)
    {
      if (string.IsNullOrEmpty(file_name))
        return (XmlDocument) null;
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(file_name);
        if (xmlDocument.DocumentElement == null || xmlDocument.DocumentElement.ChildNodes.Count <= 0)
          return (XmlDocument) null;
        else
          return xmlDocument;
      }
      catch
      {
        return (XmlDocument) null;
      }
    }

    public static Size CalcClientSizeFromControlSize(Form form, Control ctrl, Size new_ctrl_size)
    {
      if (form == null || new_ctrl_size.Width <= 0 || new_ctrl_size.Height <= 0)
        throw new ArgumentException("引数エラ\x30FC");
      Size size = form.ClientSize - ctrl.Size;
      return new_ctrl_size + size;
    }

    public static void ChangeClientSize_Center(Form form, Size new_client_size)
    {
      Useful.ChangeClientSize_Center(form, new_client_size, false);
    }

    public static void ChangeClientSize_Center(Form form, Size new_client_size, bool is_hide_window_when_update)
    {
      if (form == null || new_client_size.Width <= 0 || new_client_size.Height <= 0)
        throw new ArgumentException("引数エラ\x30FC");
      if (form.ClientSize == new_client_size)
        return;
      Point positionCenter = Useful.GetPositionCenter(form);
      Size size = form.Size - form.ClientSize + new_client_size;
      Point positionFromCenter = Useful.GetPositionFromCenter(positionCenter, size);
      if (is_hide_window_when_update)
      {
        bool visible = form.Visible;
        form.Visible = false;
        form.SetBounds(positionFromCenter.X, positionFromCenter.Y, size.Width, size.Height);
        form.Visible = visible;
      }
      else
        form.SetBounds(positionFromCenter.X, positionFromCenter.Y, size.Width, size.Height);
    }

    public static Point GetPositionCenter(Form form)
    {
      if (form == null)
        return Point.Empty;
      Point location = form.Location;
      location.X += form.Size.Width / 2;
      location.Y += form.Size.Height / 2;
      if (location.X < 0)
        location.X = 0;
      if (location.Y < 0)
        location.Y = 0;
      return location;
    }

    public static Point GetPositionFromCenter(Form form, Point center_pos)
    {
      if (form == null)
        return Point.Empty;
      else
        return Useful.GetPositionFromCenter(center_pos, form.Size);
    }

    public static Point GetPositionFromCenter(Point center_pos, Size size)
    {
      center_pos.X -= size.Width / 2;
      center_pos.Y -= size.Height / 2;
      if (center_pos.X < 0)
        center_pos.X = 0;
      if (center_pos.Y < 0)
        center_pos.Y = 0;
      return center_pos;
    }

    public static Size GetPrimaryScreenSize()
    {
      Rectangle bounds = Screen.PrimaryScreen.Bounds;
      return new Size(bounds.Width, bounds.Height);
    }
  }
}
