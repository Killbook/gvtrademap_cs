// Type: directx.d3d_writable_vb_with_index
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX.Direct3D;
using System;

namespace directx
{
  public class d3d_writable_vb_with_index : d3d_writable_vb
  {
    private IndexBuffer m_ib;

    public IndexBuffer ib
    {
      get
      {
        return this.m_ib;
      }
    }

    public d3d_writable_vb_with_index(Device device, Type type, int element_count, int buffer_count)
      : base(device, type, element_count, buffer_count)
    {
      this.m_ib = d3d_writable_vb_with_index.CreateSpriteIndexBuffer(device, element_count);
    }

    public static IndexBuffer CreateSpriteIndexBuffer(Device device, int element_count)
    {
      int length = element_count / 4 * 6;
      if (length >= (int) ushort.MaxValue)
        throw new Exception();
      IndexBuffer indexBuffer = new IndexBuffer(device, length * 2, Usage.WriteOnly, Pool.Managed, true);
      ushort[] numArray = new ushort[length];
      ushort num = (ushort) 0;
      int index = 0;
      while (index < length)
      {
        numArray[index] = num;
        numArray[index + 1] = (ushort) ((uint) num + 1U);
        numArray[index + 2] = (ushort) ((uint) num + 2U);
        numArray[index + 3] = (ushort) ((uint) num + 1U);
        numArray[index + 4] = (ushort) ((uint) num + 3U);
        numArray[index + 5] = (ushort) ((uint) num + 2U);
        num += (ushort) 4;
        index += 6;
      }
      indexBuffer.SetData((object) numArray, 0, LockFlags.None);
      return indexBuffer;
    }

    public override void Dispose()
    {
      if ((Resource) this.m_ib != (Resource) null)
      {
        this.m_ib.Dispose();
        this.m_ib = (IndexBuffer) null;
      }
      base.Dispose();
    }
  }
}
