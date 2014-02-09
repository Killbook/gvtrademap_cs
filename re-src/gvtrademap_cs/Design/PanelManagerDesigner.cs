// Type: Design.PanelManagerDesigner
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Controls;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Design
{
  public class PanelManagerDesigner : ParentControlDesigner
  {
    private DesignerVerbCollection m_verbs = new DesignerVerbCollection();
    private IDesignerHost m_DesignerHost;
    private ISelectionService m_SelectionService;

    private PanelManager HostControl
    {
      get
      {
        return (PanelManager) this.Control;
      }
    }

    public override DesignerVerbCollection Verbs
    {
      get
      {
        if (this.m_verbs.Count == 2)
          this.m_verbs[1].Enabled = this.HostControl.ManagedPanels.Count > 0;
        return this.m_verbs;
      }
    }

    public IDesignerHost DesignerHost
    {
      get
      {
        if (this.m_DesignerHost == null)
          this.m_DesignerHost = (IDesignerHost) this.GetService(typeof (IDesignerHost));
        return this.m_DesignerHost;
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

    public PanelManagerDesigner()
    {
      this.m_verbs.AddRange(new DesignerVerb[2]
      {
        new DesignerVerb("ペ\x30FCジの追加", new EventHandler(this.OnAddPanel)),
        new DesignerVerb("ペ\x30FCジの削除", new EventHandler(this.OnRemovePanel))
      });
    }

    protected override void OnPaintAdornments(PaintEventArgs pe)
    {
    }

    private void OnAddPanel(object sender, EventArgs e)
    {
      Control.ControlCollection controls = this.HostControl.Controls;
      this.RaiseComponentChanging((MemberDescriptor) TypeDescriptor.GetProperties((object) this.HostControl)["ManagedPanels"]);
      ManagedPanel managedPanel = (ManagedPanel) this.DesignerHost.CreateComponent(typeof (ManagedPanel));
      managedPanel.Text = managedPanel.Name;
      this.HostControl.ManagedPanels.Add((Control) managedPanel);
      this.RaiseComponentChanged((MemberDescriptor) TypeDescriptor.GetProperties((object) this.HostControl)["ManagedPanels"], (object) controls, (object) this.HostControl.ManagedPanels);
      this.HostControl.SelectedPanel = managedPanel;
      this.SetVerbs();
    }

    private void OnRemovePanel(object sender, EventArgs e)
    {
      Control.ControlCollection controls = this.HostControl.Controls;
      if (this.HostControl.SelectedIndex < 0)
        return;
      this.RaiseComponentChanging((MemberDescriptor) TypeDescriptor.GetProperties((object) this.HostControl)["TabPages"]);
      this.DesignerHost.DestroyComponent((IComponent) this.HostControl.ManagedPanels[this.HostControl.SelectedIndex]);
      this.RaiseComponentChanged((MemberDescriptor) TypeDescriptor.GetProperties((object) this.HostControl)["ManagedPanels"], (object) controls, (object) this.HostControl.ManagedPanels);
      this.SelectionService.SetSelectedComponents((ICollection) new IComponent[1]
      {
        (IComponent) this.HostControl
      }, SelectionTypes.Auto);
      this.SetVerbs();
    }

    private void SetVerbs()
    {
      this.Verbs[1].Enabled = this.HostControl.ManagedPanels.Count == 1;
    }

    protected override void PostFilterProperties(IDictionary properties)
    {
      properties.Remove((object) "AutoScroll");
      properties.Remove((object) "AutoScrollMargin");
      properties.Remove((object) "AutoScrollMinSize");
      properties.Remove((object) "Text");
      base.PostFilterProperties(properties);
    }

    public override void InitializeNewComponent(IDictionary defaultValues)
    {
      base.InitializeNewComponent(defaultValues);
      this.HostControl.ManagedPanels.Add((Control) this.DesignerHost.CreateComponent(typeof (ManagedPanel)));
      this.HostControl.ManagedPanels.Add((Control) this.DesignerHost.CreateComponent(typeof (ManagedPanel)));
      PanelManager panelManager = (PanelManager) this.Control;
      panelManager.ManagedPanels[0].Text = panelManager.ManagedPanels[0].Name;
      panelManager.ManagedPanels[1].Text = panelManager.ManagedPanels[1].Name;
      this.HostControl.SelectedIndex = 0;
    }
  }
}
