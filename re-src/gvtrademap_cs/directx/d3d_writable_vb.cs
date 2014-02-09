// Type: directx.d3d_writable_vb
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX.Direct3D;
using System;

namespace directx
{
  public class d3d_writable_vb : IDisposable
  {
    private VertexBuffer[] m_vb;
    private int m_index;

    public VertexBuffer vb
    {
      get
      {
        return this.m_vb[this.m_index];
      }
    }

    public d3d_writable_vb(Device device, Type type, int element_count, int buffer_count)
    {
      this.m_vb = new VertexBuffer[buffer_count];
      this.m_index = 0;
      for (int index = 0; index < buffer_count; ++index)
        this.m_vb[index] = new VertexBuffer(type, element_count, device, Usage.WriteOnly, VertexFormats.Texture0, Pool.Managed);
    }

    public void SetData<T>(T[] _object) where T : struct
    {
      this.m_vb[this.m_index].SetData((object) _object, 0, LockFlags.None);
    }

    public void Flip()
    {
      if (++this.m_index < this.m_vb.Length)
        return;
      this.m_index = 0;
    }

    public virtual void Dispose()
    {
      if (this.m_vb == null || this.m_vb.Length <= 0)
        return;
      for (int index = 0; index < this.m_vb.Length; ++index)
        this.m_vb[index].Dispose();
      this.m_vb = (VertexBuffer[]) null;
      this.m_index = 0;
    }
  }
}
