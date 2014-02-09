// Type: Utility.KeyAssign.KeyAssignRule
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Windows.Forms;
using Utility;

namespace Utility.KeyAssign
{
  public class KeyAssignRule
  {
    public bool CanAssignKeys(KeyEventArgs e)
    {
      return this.CanAssignKeys(e.KeyData);
    }

    public virtual bool CanAssignKeys(Keys key)
    {
      key &= ~(Keys.Shift | Keys.Control | Keys.Alt);
      return key != Keys.None && key != Keys.ShiftKey && (key != Keys.Menu && key != Keys.ControlKey) && (key != (Keys.F1 | Keys.F17) && key != (Keys.F3 | Keys.F17) && (key != Keys.KanaMode && key != Keys.IMEAccept)) && (key != Keys.IMEConvert && key != Keys.IMENonconvert && (key != Keys.IMEAccept && key != Keys.IMEAccept) && (key != Keys.IMEModeChange && key != Keys.LWin && (key != Keys.RWin && key != Keys.Apps))) && (key != Keys.NumLock && key != Keys.Capital && (key != (Keys.F5 | Keys.F17) && key != (Keys.F4 | Keys.F17)));
    }

    public static bool IsShortcut(Keys key)
    {
      return key != Keys.None && Useful.ToEnum(typeof (Shortcut), (object) key) != null;
    }

    public string GetKeysString(KeyEventArgs e)
    {
      return this.GetKeysString(e.KeyData);
    }

    public string GetKeysString(Keys key)
    {
      string str = "";
      if ((key & Keys.Alt) != Keys.None)
        str = str + "Alt+";
      if ((key & Keys.Control) != Keys.None)
        str = str + "Ctrl+";
      if ((key & Keys.Shift) != Keys.None)
        str = str + "Shift+";
      if (this.CanAssignKeys(key))
        str = str + KeyAssignRule.keys_to_string(key);
      else if (str == "")
        return "なし";
      return str;
    }

    protected static string keys_to_string(Keys key)
    {
      key &= ~(Keys.Shift | Keys.Control | Keys.Alt);
      switch (key)
      {
        case Keys.OemSemicolon:
          return ":";
        case Keys.Oemplus:
          return ";";
        case Keys.Oemcomma:
          return ",";
        case Keys.OemMinus:
          return "=";
        case Keys.OemPeriod:
          return ".";
        case Keys.OemQuestion:
          return "/";
        case Keys.Oemtilde:
          return "@";
        case Keys.OemOpenBrackets:
          return "[";
        case Keys.OemPipe:
          return "|";
        case Keys.OemCloseBrackets:
          return "]";
        case Keys.OemQuotes:
          return "^";
        case Keys.OemBackslash:
          return "\\";
        case Keys.D0:
          return "0";
        case Keys.D1:
          return "1";
        case Keys.D2:
          return "2";
        case Keys.D3:
          return "3";
        case Keys.D4:
          return "4";
        case Keys.D5:
          return "5";
        case Keys.D6:
          return "6";
        case Keys.D7:
          return "7";
        case Keys.D8:
          return "8";
        case Keys.D9:
          return "9";
        case Keys.Multiply:
          return "*";
        case Keys.Add:
          return "+";
        case Keys.Subtract:
          return "-";
        case Keys.Divide:
          return "/";
        case Keys.Escape:
          return "ESC";
        case Keys.Prior:
          return "PageUp";
        case Keys.Next:
          return "PageDown";
        case Keys.Back:
          return "BackSpace";
        case Keys.Return:
          return "Enter";
        default:
          return ((object) key).ToString();
      }
    }
  }
}
