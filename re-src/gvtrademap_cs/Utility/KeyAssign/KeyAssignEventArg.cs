// Type: Utility.KeyAssign.KeyAssignEventArg
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;

namespace Utility.KeyAssign
{
  public sealed class KeyAssignEventArg : EventArgs
  {
    private object m_tag;

    public object Tag
    {
      get
      {
        return this.m_tag;
      }
    }

    public KeyAssignEventArg(object tag)
    {
      this.m_tag = tag;
    }
  }
}
