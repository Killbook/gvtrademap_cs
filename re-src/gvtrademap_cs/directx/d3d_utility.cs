// Type: directx.d3d_utility
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace directx
{
  public static class d3d_utility
  {
    public static Vector2 GetTextureSize(Texture tex)
    {
      if (tex == (Texture) null)
        return new Vector2(0.0f, 0.0f);
      try
      {
        SurfaceDescription levelDescription = tex.GetLevelDescription(0);
        return new Vector2((float) levelDescription.Width, (float) levelDescription.Height);
      }
      catch
      {
        return new Vector2(0.0f, 0.0f);
      }
    }

    public static Texture CreateTextureFromTexture(Device device, Texture src_texture)
    {
      SurfaceDescription levelDescription = src_texture.GetLevelDescription(0);
      return d3d_utility.CreateTextureFromTexture(device, src_texture, levelDescription.Format);
    }

    public static Texture CreateTextureFromTexture(Device device, Texture src_texture, Format format)
    {
      if (device == (Device) null)
        return (Texture) null;
      if (src_texture == (Texture) null)
        return (Texture) null;
      try
      {
        Texture textureSameSize = d3d_utility.CreateTextureSameSize(device, src_texture, format);
        if (textureSameSize == (Texture) null)
          return (Texture) null;
        if (d3d_utility.CopyTexture(device, textureSameSize, src_texture))
          return textureSameSize;
        textureSameSize.Dispose();
        return (Texture) null;
      }
      catch
      {
        return (Texture) null;
      }
    }

    public static Texture CreateTextureSameSize(Device device, Texture src_texture)
    {
      SurfaceDescription levelDescription = src_texture.GetLevelDescription(0);
      return d3d_utility.CreateTextureSameSize(device, src_texture, levelDescription.Format);
    }

    public static Texture CreateTextureSameSize(Device device, Texture src_texture, Pool pool)
    {
      SurfaceDescription levelDescription = src_texture.GetLevelDescription(0);
      return d3d_utility.CreateTextureSameSize(device, src_texture, Usage.None, levelDescription.Format, pool);
    }

    public static Texture CreateTextureSameSize(Device device, Texture src_texture, Format format)
    {
      return d3d_utility.CreateTextureSameSize(device, src_texture, Usage.None, format, Pool.Managed);
    }

    public static Texture CreateTextureSameSize(Device device, Texture src_texture, Usage usage, Format format, Pool pool)
    {
      Vector2 textureSize = d3d_utility.GetTextureSize(src_texture);
      try
      {
        return new Texture(device, (int) textureSize.X, (int) textureSize.Y, 1, usage, format, pool);
      }
      catch
      {
        return (Texture) null;
      }
    }

    public static Texture CreateRenderTargetTextureSameSize(Device device, Texture src_texture)
    {
      SurfaceDescription levelDescription = src_texture.GetLevelDescription(0);
      return d3d_utility.CreateTextureSameSize(device, src_texture, Usage.RenderTarget, levelDescription.Format, Pool.Default);
    }

    public static bool CopyTexture(Device device, Texture dst_texture, Texture src_texture)
    {
      if (device == (Device) null || src_texture == (Texture) null)
        return false;
      if (dst_texture == (Texture) null)
        return false;
      try
      {
        Surface surfaceLevel1 = dst_texture.GetSurfaceLevel(0);
        Surface surfaceLevel2 = src_texture.GetSurfaceLevel(0);
        SurfaceLoader.FromSurface(surfaceLevel1, surfaceLevel2, Filter.None, 0);
        surfaceLevel1.Dispose();
        surfaceLevel2.Dispose();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static Size TextureSizePow2(Size size)
    {
      return new Size()
      {
        Width = d3d_utility.size_pow2(size.Width),
        Height = d3d_utility.size_pow2(size.Height)
      };
    }

    private static int size_pow2(int size)
    {
      int num = 2;
      for (int index = 0; index < 30; ++index)
      {
        if (size <= num)
          return num;
        num <<= 1;
      }
      return size;
    }
  }
}
