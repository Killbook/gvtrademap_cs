// Type: Controls.ManagedPanel
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Design;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Controls
{
  [Designer(typeof (ManagedPanelDesigner))]
  [ToolboxItem(false)]
  public class ManagedPanel : ScrollableControl
  {
    [DefaultValue(typeof (DockStyle), "Fill")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public override DockStyle Dock
    {
      get
      {
        return base.Dock;
      }
      set
      {
        base.Dock = DockStyle.Fill;
      }
    }

    [DefaultValue(typeof (AnchorStyles), "None")]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override AnchorStyles Anchor
    {
      get
      {
        return AnchorStyles.None;
      }
      set
      {
        base.Anchor = AnchorStyles.None;
      }
    }

    public ManagedPanel()
    {
      base.Dock = DockStyle.Fill;
      this.SetStyle(ControlStyles.ResizeRedraw, true);
    }

    protected override void OnLocationChanged(EventArgs e)
    {
      base.OnLocationChanged(e);
      this.Location = Point.Empty;
    }

    protected override void OnSizeChanged(EventArgs e)
    {
      base.OnSizeChanged(e);
      if (this.Parent == null)
        this.Size = Size.Empty;
      else
        this.Size = this.Parent.ClientSize;
    }

    protected override void OnParentChanged(EventArgs e)
    {
      if (!(this.Parent is PanelManager) && this.Parent != null)
        throw new ArgumentException("Managed Panels may only be added to a Panel Manager.");
      base.OnParentChanged(e);
    }
  }
}
