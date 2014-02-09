// Type: Utility.KeyAssign.KeyAssignRuleOnlyShortcut
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Windows.Forms;

namespace Utility.KeyAssign
{
  public class KeyAssignRuleOnlyShortcut : KeyAssignRule
  {
    public override bool CanAssignKeys(Keys key)
    {
      if (!KeyAssignRule.IsShortcut(key))
        return false;
      else
        return base.CanAssignKeys(key);
    }
  }
}
