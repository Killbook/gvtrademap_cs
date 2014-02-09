// Type: Editors.ManagedPanelCollectionEditor
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Controls;
using System;
using System.ComponentModel.Design;

namespace Editors
{
  public class ManagedPanelCollectionEditor : CollectionEditor
  {
    public ManagedPanelCollectionEditor(Type type)
      : base(type)
    {
    }

    protected override Type CreateCollectionItemType()
    {
      return typeof (ManagedPanel);
    }
  }
}
