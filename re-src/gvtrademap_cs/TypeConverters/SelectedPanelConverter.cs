// Type: TypeConverters.SelectedPanelConverter
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Controls;
using System.ComponentModel;
using System.Windows.Forms;

namespace TypeConverters
{
  public class SelectedPanelConverter : ReferenceConverter
  {
    public SelectedPanelConverter()
      : base(typeof (ManagedPanel))
    {
    }

    protected override bool IsValueAllowed(ITypeDescriptorContext context, object value)
    {
      if (context != null)
        return ((PanelManager) context.Instance).ManagedPanels.Contains((Control) value);
      else
        return false;
    }
  }
}
