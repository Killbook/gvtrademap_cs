// Type: Controls.PanelManager
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Design;
using Editors;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using TypeConverters;

namespace Controls
{
  [DefaultProperty("SelectedPanel")]
  [Designer(typeof (PanelManagerDesigner))]
  [DefaultEvent("SelectedIndexChanged")]
  public class PanelManager : Control
  {
    private Container components;
    private ManagedPanel m_SelectedPanel;
    private ManagedPanel oldSelection;

    [Editor(typeof (ManagedPanelCollectionEditor), typeof (UITypeEditor))]
    public Control.ControlCollection ManagedPanels
    {
      get
      {
        return this.Controls;
      }
    }

    [TypeConverter(typeof (SelectedPanelConverter))]
    public ManagedPanel SelectedPanel
    {
      get
      {
        return this.m_SelectedPanel;
      }
      set
      {
        if (this.m_SelectedPanel == value)
          return;
        this.m_SelectedPanel = value;
        this.OnSelectedPanelChanged(EventArgs.Empty);
      }
    }

    [Browsable(false)]
    public int SelectedIndex
    {
      get
      {
        return this.ManagedPanels.IndexOf((Control) this.SelectedPanel);
      }
      set
      {
        if (value == -1)
          this.SelectedPanel = (ManagedPanel) null;
        else
          this.SelectedPanel = (ManagedPanel) this.ManagedPanels[value];
      }
    }

    protected override Size DefaultSize
    {
      get
      {
        return new Size(200, 100);
      }
    }

    public event EventHandler SelectedIndexChanged;

    public PanelManager()
    {
      this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = new Container();
    }

    protected void OnSelectedPanelChanged(EventArgs e)
    {
      if (this.oldSelection != null)
        this.oldSelection.Visible = false;
      if (this.m_SelectedPanel != null)
        this.m_SelectedPanel.Visible = true;
      if ((this.m_SelectedPanel != null ? !this.m_SelectedPanel.Equals((object) this.oldSelection) : this.oldSelection != null) && this.Created && this.SelectedIndexChanged != null)
        this.SelectedIndexChanged((object) this, EventArgs.Empty);
      this.oldSelection = this.m_SelectedPanel;
    }

    protected override void OnControlAdded(ControlEventArgs e)
    {
      if (!(e.Control is ManagedPanel))
        throw new ArgumentException("Only Mangel.Controls.ManagedPanels can be added to a Mangel.Controls.PanelManger.");
      if (this.SelectedPanel != null)
        this.SelectedPanel.Visible = false;
      this.SelectedPanel = (ManagedPanel) e.Control;
      e.Control.Visible = true;
      base.OnControlAdded(e);
    }

    protected override void OnControlRemoved(ControlEventArgs e)
    {
      if (e.Control is ManagedPanel)
      {
        if (this.ManagedPanels.Count > 0)
          this.SelectedIndex = 0;
        else
          this.SelectedPanel = (ManagedPanel) null;
      }
      base.OnControlRemoved(e);
    }
  }
}
