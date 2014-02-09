// Type: Design.ManagedPanelDesigner
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Controls;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Design
{
  public class ManagedPanelDesigner : ScrollableControlDesigner
  {
    private DesignerVerbCollection m_verbs = new DesignerVerbCollection();
    private ISelectionService m_SelectionService;

    private ManagedPanel HostControl
    {
      get
      {
        return (ManagedPanel) this.Control;
      }
    }

    public ISelectionService SelectionService
    {
      get
      {
        if (this.m_SelectionService == null)
          this.m_SelectionService = (ISelectionService) this.GetService(typeof (ISelectionService));
        return this.m_SelectionService;
      }
    }

    public override SelectionRules SelectionRules
    {
      get
      {
        return SelectionRules.Visible;
      }
    }

    public override DesignerVerbCollection Verbs
    {
      get
      {
        return this.m_verbs;
      }
    }

    public ManagedPanelDesigner()
    {
      this.m_verbs.Add(new DesignerVerb("PanelManagerを選択", new EventHandler(this.OnSelectManager)));
    }

    private void OnSelectManager(object sender, EventArgs e)
    {
      if (this.HostControl.Parent == null)
        return;
      this.SelectionService.SetSelectedComponents((ICollection) new Component[1]
      {
        (Component) this.HostControl.Parent
      });
    }

    protected override void OnPaintAdornments(PaintEventArgs pe)
    {
      base.OnPaintAdornments(pe);
      Pen pen = new Pen((double) this.Control.BackColor.GetBrightness() < 0.5 ? Color.White : ControlPaint.Dark(this.Control.BackColor));
      Rectangle clientRectangle = this.Control.ClientRectangle;
      --clientRectangle.Width;
      --clientRectangle.Height;
      pen.DashStyle = DashStyle.Dash;
      pe.Graphics.DrawRectangle(pen, clientRectangle);
      pen.Dispose();
    }

    protected override void PostFilterProperties(IDictionary properties)
    {
      properties.Remove((object) "Anchor");
      properties.Remove((object) "TabStop");
      properties.Remove((object) "TabIndex");
      base.PostFilterProperties(properties);
    }

    public override void InitializeNewComponent(IDictionary defaultValues)
    {
      base.InitializeNewComponent(defaultValues);
      this.Control.Visible = true;
    }
  }
}
