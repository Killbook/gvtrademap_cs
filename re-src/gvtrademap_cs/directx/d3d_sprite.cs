// Type: directx.d3d_sprite
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace directx
{
  public class d3d_sprite : IDisposable
  {
    private const int DRAW_SPRITE_ONCE = 128;
    private const int SWAP_BFFER_MAX = 16;
    private Device m_d3d_device;
    private CustomVertex.TransformedColoredTextured[] m_vertex_list;
    private int m_sprite_count;
    private Texture m_texture;
    private Vector2 m_map_offset;
    private float m_map_scale;
    private Vector2 m_global_scale;
    private int m_draw_sprites_in_frame;
    private Effect m_effect;
    private VertexDeclaration m_decl;
    private d3d_writable_vb_with_index m_vb;
    private d3d_sprite.sprite_vertex[] m_vbo;

    public int draw_sprites_in_frame
    {
      get
      {
        return this.m_draw_sprites_in_frame;
      }
    }

    public Effect effect
    {
      get
      {
        return this.m_effect;
      }
    }

    public d3d_sprite(Device device, bool is_use_ve1_1_ps1_1)
    {
      this.m_d3d_device = device;
      this.m_vertex_list = new CustomVertex.TransformedColoredTextured[768];
      this.m_sprite_count = 0;
      this.m_effect = (Effect) null;
      this.m_decl = (VertexDeclaration) null;
      this.m_vb = (d3d_writable_vb_with_index) null;
      this.m_vbo = (d3d_sprite.sprite_vertex[]) null;
      if (is_use_ve1_1_ps1_1)
        this.initialize_shader(Assembly.GetExecutingAssembly().GetManifestResourceStream("directx.fx.sprite.fxo"));
      this.BeginFrame();
    }

    public void BeginFrame()
    {
      this.m_draw_sprites_in_frame = 0;
    }

    public void Dispose()
    {
      this.DisposeEffect();
    }

    public void DisposeEffect()
    {
      if (this.m_effect != (Effect) null)
        this.m_effect.Dispose();
      if (this.m_decl != (VertexDeclaration) null)
        this.m_decl.Dispose();
      if (this.m_vb != null)
        this.m_vb.Dispose();
      this.m_effect = (Effect) null;
      this.m_decl = (VertexDeclaration) null;
      this.m_vb = (d3d_writable_vb_with_index) null;
    }

    private void initialize_shader(Stream stream)
    {
      try
      {
        string compilationErrors;
        this.m_effect = Effect.FromStream(this.m_d3d_device, stream, (Include) null, (string) null, ShaderFlags.None, (EffectPool) null, out compilationErrors);
        this.initialize_shader();
      }
      catch
      {
        this.m_effect = (Effect) null;
      }
    }

    private void initialize_shader()
    {
      try
      {
        this.m_decl = new VertexDeclaration(this.m_d3d_device, d3d_sprite.sprite_vertex.VertexElements);
        this.m_vb = new d3d_writable_vb_with_index(this.m_d3d_device, typeof (d3d_sprite.sprite_vertex), 512, 16);
        this.m_vbo = new d3d_sprite.sprite_vertex[512];
      }
      catch
      {
        this.m_effect = (Effect) null;
        this.m_decl = (VertexDeclaration) null;
        this.m_vb = (d3d_writable_vb_with_index) null;
        this.m_vbo = (d3d_sprite.sprite_vertex[]) null;
      }
    }

    public void BeginDrawSprites(Texture tex)
    {
      this.begin_draw_sprites_shader(tex);
      if (this.m_effect == (Effect) null)
      {
        this.m_texture = tex;
        this.m_map_offset = new Vector2(0.0f, 0.0f);
        this.m_map_scale = 1f;
        this.m_global_scale = new Vector2(1f, 1f);
      }
      this.m_sprite_count = 0;
    }

    private void begin_draw_sprites_shader(Texture tex)
    {
      if (this.m_effect == (Effect) null)
        return;
      try
      {
        this.m_effect.SetValue((EffectHandle) "ViewportSize", new float[2]
        {
          (float) this.m_d3d_device.Viewport.Width,
          (float) this.m_d3d_device.Viewport.Height
        });
        this.m_effect.SetValue((EffectHandle) "Texture", (BaseTexture) tex);
        this.m_effect.Technique = (EffectHandle) "Sprite";
      }
      catch
      {
        this.DisposeEffect();
      }
    }

    public void BeginDrawSprites(Texture tex, Vector2 map_offset, float map_scale, Vector2 global_scale)
    {
      this.begin_draw_sprites_shader(tex, map_offset, map_scale, global_scale);
      if (this.m_effect == (Effect) null)
      {
        this.m_texture = tex;
        this.m_map_offset = map_offset;
        this.m_map_scale = map_scale;
        this.m_global_scale = global_scale;
      }
      this.m_sprite_count = 0;
    }

    public void begin_draw_sprites_shader(Texture tex, Vector2 map_offset, float map_scale, Vector2 global_scale)
    {
      if (this.m_effect == (Effect) null)
        return;
      try
      {
        float[] f1 = new float[2]
        {
          (float) this.m_d3d_device.Viewport.Width,
          (float) this.m_d3d_device.Viewport.Height
        };
        float[] f2 = new float[2]
        {
          map_offset.X,
          map_offset.Y
        };
        float[] f3 = new float[2]
        {
          global_scale.X,
          global_scale.Y
        };
        this.m_effect.SetValue((EffectHandle) "ViewportSize", f1);
        this.m_effect.SetValue((EffectHandle) "Texture", (BaseTexture) tex);
        this.m_effect.SetValue((EffectHandle) "MapOffset", f2);
        this.m_effect.SetValue((EffectHandle) "MapScale", map_scale);
        this.m_effect.SetValue((EffectHandle) "GlobalScale", f3);
        this.m_effect.Technique = (EffectHandle) "SpriteWithGlobalParams";
      }
      catch
      {
        this.DisposeEffect();
      }
    }

    private Vector3 goffsetscale(Vector3 p)
    {
      p.X += this.m_map_offset.X;
      p.Y += this.m_map_offset.Y;
      p.X *= this.m_map_scale;
      p.Y *= this.m_map_scale;
      return p;
    }

    private Vector2 gscale(Vector2 scale)
    {
      scale.X *= this.m_global_scale.X;
      scale.Y *= this.m_global_scale.Y;
      return scale;
    }

    public bool AddDrawSprites(Vector3 pos, d3d_sprite_rects.rect _rect)
    {
      return this.AddDrawSprites(pos, _rect, -1);
    }

    public bool AddDrawSprites(Vector3 pos, d3d_sprite_rects.rect _rect, int color)
    {
      return this.AddDrawSprites(pos, _rect, color, new Vector2(0.0f, 0.0f));
    }

    public bool AddDrawSprites(Vector3 pos, d3d_sprite_rects.rect _rect, int color, Vector2 offset2)
    {
      if (this.m_effect != (Effect) null)
      {
        this.add_sprite_to_vertex_buffer_shader(pos, _rect, 0.0f, new Vector2(1f, 1f), color, offset2);
      }
      else
      {
        pos = this.goffsetscale(pos);
        Vector2 size = this.gscale(_rect.size);
        pos.X = (float) (int) pos.X;
        pos.Y = (float) (int) pos.Y;
        pos.X += offset2.X;
        pos.Y += offset2.Y;
        pos.X += _rect.lefttop.X;
        pos.Y += _rect.lefttop.Y;
        if ((double) pos.X >= (double) (this.m_d3d_device.Viewport.Width + this.m_d3d_device.Viewport.X) || ((double) pos.Y >= (double) (this.m_d3d_device.Viewport.Height + this.m_d3d_device.Viewport.Y) || (double) pos.X + (double) size.X < 0.0 || (double) pos.Y + (double) size.Y < 0.0))
          return false;
        this.add_sprite_to_vertex_buffer(pos, size, _rect, color);
      }
      if (this.m_sprite_count >= 128)
        this.draw_sprites();
      return true;
    }

    public bool AddDrawSprites(Vector3 pos, d3d_sprite_rects.rect _rect, Vector2 scale)
    {
      return this.AddDrawSprites(pos, _rect, scale, -1);
    }

    public bool AddDrawSprites(Vector3 pos, d3d_sprite_rects.rect _rect, Vector2 scale, int color)
    {
      return this.AddDrawSprites(pos, _rect, scale, color, new Vector2(0.0f, 0.0f));
    }

    public bool AddDrawSprites(Vector3 pos, d3d_sprite_rects.rect _rect, Vector2 scale, int color, Vector2 offset2)
    {
      if (this.m_effect != (Effect) null)
      {
        this.add_sprite_to_vertex_buffer_shader(pos, _rect, 0.0f, scale, color, offset2);
      }
      else
      {
        pos = this.goffsetscale(pos);
        Vector2 lefttop = _rect.lefttop;
        Vector2 size = this.gscale(_rect.size);
        lefttop.X *= scale.X;
        lefttop.Y *= scale.Y;
        size.X *= scale.X;
        size.Y *= scale.Y;
        pos.X = (float) (int) pos.X;
        pos.Y = (float) (int) pos.Y;
        pos.X += offset2.X;
        pos.Y += offset2.Y;
        pos.X += lefttop.X;
        pos.Y += lefttop.Y;
        if ((double) pos.X >= (double) (this.m_d3d_device.Viewport.Width + this.m_d3d_device.Viewport.X) || ((double) pos.Y >= (double) (this.m_d3d_device.Viewport.Height + this.m_d3d_device.Viewport.Y) || (double) pos.X + (double) size.X < 0.0 || (double) pos.Y + (double) size.Y < 0.0))
          return false;
        this.add_sprite_to_vertex_buffer(pos, size, _rect, color);
      }
      if (this.m_sprite_count >= 128)
        this.draw_sprites();
      return true;
    }

    public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect)
    {
      return this.AddDrawSpritesNC(pos, _rect, -1);
    }

    public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, int color)
    {
      return this.AddDrawSpritesNC(pos, _rect, color, new Vector2(0.0f, 0.0f));
    }

    public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, int color, Vector2 offset2)
    {
      if (this.m_effect != (Effect) null)
      {
        this.add_sprite_to_vertex_buffer_shader(pos, _rect, 0.0f, new Vector2(1f, 1f), color, offset2);
      }
      else
      {
        pos = this.goffsetscale(pos);
        Vector2 size = this.gscale(_rect.size);
        pos.X = (float) (int) pos.X;
        pos.Y = (float) (int) pos.Y;
        pos.X += offset2.X;
        pos.Y += offset2.Y;
        pos.X += _rect.lefttop.X;
        pos.Y += _rect.lefttop.Y;
        this.add_sprite_to_vertex_buffer(pos, size, _rect, color);
      }
      if (this.m_sprite_count >= 128)
        this.draw_sprites();
      return true;
    }

    public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, Vector2 scale)
    {
      return this.AddDrawSpritesNC(pos, _rect, scale, -1);
    }

    public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, Vector2 scale, int color)
    {
      return this.AddDrawSpritesNC(pos, _rect, scale, color, new Vector2(0.0f, 0.0f));
    }

    public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, Vector2 scale, int color, Vector2 offset2)
    {
      if (this.m_effect != (Effect) null)
      {
        this.add_sprite_to_vertex_buffer_shader(pos, _rect, 0.0f, scale, color, offset2);
      }
      else
      {
        pos = this.goffsetscale(pos);
        Vector2 lefttop = _rect.lefttop;
        Vector2 size = this.gscale(_rect.size);
        lefttop.X *= scale.X;
        lefttop.Y *= scale.Y;
        size.X *= scale.X;
        size.Y *= scale.Y;
        pos.X += lefttop.X;
        pos.Y += lefttop.Y;
        pos.X = (float) (int) pos.X;
        pos.Y = (float) (int) pos.Y;
        pos.X += offset2.X;
        pos.Y += offset2.Y;
        this.add_sprite_to_vertex_buffer(pos, size, _rect, color);
      }
      if (this.m_sprite_count >= 128)
        this.draw_sprites();
      return true;
    }

    public void EndDrawSprites()
    {
      if (this.m_sprite_count <= 0)
        return;
      this.draw_sprites();
    }

    private void draw_sprites()
    {
      if (this.m_effect != (Effect) null)
      {
        this.m_vb.SetData<d3d_sprite.sprite_vertex>(this.m_vbo);
        this.m_effect.Begin(FX.None);
        this.m_effect.BeginPass(0);
        this.m_d3d_device.VertexDeclaration = this.m_decl;
        this.m_d3d_device.SetStreamSource(0, this.m_vb.vb, 0, d3d_sprite.sprite_vertex.SizeInBytes);
        this.m_d3d_device.Indices = this.m_vb.ib;
        this.m_d3d_device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, this.m_sprite_count * 4, 0, this.m_sprite_count * 2);
        this.m_effect.EndPass();
        this.m_effect.End();
        this.m_vb.Flip();
      }
      else
      {
        this.m_d3d_device.SetTexture(0, (BaseTexture) this.m_texture);
        this.m_d3d_device.VertexFormat = VertexFormats.Texture1 | VertexFormats.Diffuse | VertexFormats.Transformed;
        this.m_d3d_device.DrawUserPrimitives(PrimitiveType.TriangleList, this.m_sprite_count * 2, (object) this.m_vertex_list);
      }
      this.m_sprite_count = 0;
    }

    private void add_sprite_to_vertex_buffer(Vector3 pos, Vector2 size, d3d_sprite_rects.rect _rect, int color)
    {
      int index1 = this.m_sprite_count * 6;
      pos.X -= 0.5f;
      pos.Y -= 0.5f;
      for (int index2 = 0; index2 < 6; ++index2)
      {
        this.m_vertex_list[index1 + index2].X = pos.X;
        this.m_vertex_list[index1 + index2].Y = pos.Y;
        this.m_vertex_list[index1 + index2].Z = pos.Z;
        this.m_vertex_list[index1 + index2].Rhw = 1f;
        this.m_vertex_list[index1 + index2].Color = color;
      }
      this.m_vertex_list[index1 + 1].X += size.X;
      this.m_vertex_list[index1 + 2].Y += size.Y;
      this.m_vertex_list[index1].Tu = _rect.uv[0].X;
      this.m_vertex_list[index1].Tv = _rect.uv[0].Y;
      this.m_vertex_list[index1 + 1].Tu = _rect.uv[1].X;
      this.m_vertex_list[index1 + 1].Tv = _rect.uv[1].Y;
      this.m_vertex_list[index1 + 2].Tu = _rect.uv[2].X;
      this.m_vertex_list[index1 + 2].Tv = _rect.uv[2].Y;
      this.m_vertex_list[index1 + 3].X += size.X;
      this.m_vertex_list[index1 + 4].X += size.X;
      this.m_vertex_list[index1 + 4].Y += size.Y;
      this.m_vertex_list[index1 + 5].Y += size.Y;
      this.m_vertex_list[index1 + 3].Tu = _rect.uv[1].X;
      this.m_vertex_list[index1 + 3].Tv = _rect.uv[1].Y;
      this.m_vertex_list[index1 + 4].Tu = _rect.uv[3].X;
      this.m_vertex_list[index1 + 4].Tv = _rect.uv[3].Y;
      this.m_vertex_list[index1 + 5].Tu = _rect.uv[2].X;
      this.m_vertex_list[index1 + 5].Tv = _rect.uv[2].Y;
      ++this.m_sprite_count;
      ++this.m_draw_sprites_in_frame;
    }

    private void add_sprite_to_vertex_buffer_shader(Vector3 pos, d3d_sprite_rects.rect _rect, float angle_rad, Vector2 scale, int color, Vector2 offset)
    {
      int num = this.m_sprite_count * 4;
      Vector3 vector3 = new Vector3(scale.X, scale.Y, angle_rad);
      for (int index = 0; index < 4; ++index)
      {
        this.m_vbo[num + index].color = color;
        this.m_vbo[num + index].Position = pos;
        this.m_vbo[num + index].offset1 = _rect.offset[index];
        this.m_vbo[num + index].offset2 = offset;
        this.m_vbo[num + index].param = vector3;
        this.m_vbo[num + index].uv = _rect.uv[index];
      }
      ++this.m_sprite_count;
      ++this.m_draw_sprites_in_frame;
    }

    public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, float angle_rad)
    {
      return this.AddDrawSpritesNC(pos, _rect, angle_rad, new Vector2(1f, 1f), -1);
    }

    public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, float angle_rad, Vector2 scale)
    {
      return this.AddDrawSpritesNC(pos, _rect, angle_rad, scale, -1);
    }

    public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, float angle_rad, int color)
    {
      return this.AddDrawSpritesNC(pos, _rect, angle_rad, new Vector2(1f, 1f), color);
    }

    public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, float angle_rad, Vector2 scale, int color)
    {
      return this.AddDrawSpritesNC(pos, _rect, angle_rad, scale, color, new Vector2(0.0f, 0.0f));
    }

    public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, float angle_rad, Vector2 scale, int color, Vector2 offset2)
    {
      if (this.m_effect != (Effect) null)
      {
        this.add_sprite_to_vertex_buffer_shader(pos, _rect, angle_rad, scale, color, offset2);
      }
      else
      {
        pos = this.goffsetscale(pos);
        Vector2 vector2 = this.gscale(scale);
        Matrix matrix = Matrix.RotationZ(angle_rad);
        Matrix sourceMatrix = Matrix.Scaling(vector2.X, vector2.Y, 1f) * matrix;
        Vector3 vector3_1 = new Vector3(_rect.offset[0].X, _rect.offset[0].Y, 0.0f);
        Vector3 vector3_2 = new Vector3(_rect.offset[1].X, _rect.offset[1].Y, 0.0f);
        Vector3 vector3_3 = new Vector3(_rect.offset[2].X, _rect.offset[2].Y, 0.0f);
        Vector3 vector3_4 = new Vector3(_rect.offset[3].X, _rect.offset[3].Y, 0.0f);
        vector3_1.TransformCoordinate(sourceMatrix);
        vector3_2.TransformCoordinate(sourceMatrix);
        vector3_3.TransformCoordinate(sourceMatrix);
        vector3_4.TransformCoordinate(sourceMatrix);
        int index1 = this.m_sprite_count * 6;
        pos.X = (float) (int) pos.X;
        pos.Y = (float) (int) pos.Y;
        pos.X += offset2.X;
        pos.Y += offset2.Y;
        pos.X -= 0.5f;
        pos.Y -= 0.5f;
        for (int index2 = 0; index2 < 6; ++index2)
        {
          this.m_vertex_list[index1 + index2].X = pos.X;
          this.m_vertex_list[index1 + index2].Y = pos.Y;
          this.m_vertex_list[index1 + index2].Z = pos.Z;
          this.m_vertex_list[index1 + index2].Rhw = 1f;
          this.m_vertex_list[index1 + index2].Color = color;
        }
        this.m_vertex_list[index1].X += vector3_1.X;
        this.m_vertex_list[index1].Y += vector3_1.Y;
        this.m_vertex_list[index1 + 1].X += vector3_2.X;
        this.m_vertex_list[index1 + 1].Y += vector3_2.Y;
        this.m_vertex_list[index1 + 2].X += vector3_3.X;
        this.m_vertex_list[index1 + 2].Y += vector3_3.Y;
        this.m_vertex_list[index1].Tu = _rect.uv[0].X;
        this.m_vertex_list[index1].Tv = _rect.uv[0].Y;
        this.m_vertex_list[index1 + 1].Tu = _rect.uv[1].X;
        this.m_vertex_list[index1 + 1].Tv = _rect.uv[1].Y;
        this.m_vertex_list[index1 + 2].Tu = _rect.uv[2].X;
        this.m_vertex_list[index1 + 2].Tv = _rect.uv[2].Y;
        this.m_vertex_list[index1 + 3].X += vector3_2.X;
        this.m_vertex_list[index1 + 3].Y += vector3_2.Y;
        this.m_vertex_list[index1 + 4].X += vector3_4.X;
        this.m_vertex_list[index1 + 4].Y += vector3_4.Y;
        this.m_vertex_list[index1 + 5].X += vector3_3.X;
        this.m_vertex_list[index1 + 5].Y += vector3_3.Y;
        this.m_vertex_list[index1 + 3].Tu = _rect.uv[1].X;
        this.m_vertex_list[index1 + 3].Tv = _rect.uv[1].Y;
        this.m_vertex_list[index1 + 4].Tu = _rect.uv[3].X;
        this.m_vertex_list[index1 + 4].Tv = _rect.uv[3].Y;
        this.m_vertex_list[index1 + 5].Tu = _rect.uv[2].X;
        this.m_vertex_list[index1 + 5].Tv = _rect.uv[2].Y;
        ++this.m_sprite_count;
        ++this.m_draw_sprites_in_frame;
      }
      if (this.m_sprite_count >= 128)
        this.draw_sprites();
      return true;
    }

    protected struct sprite_vertex
    {
      public static VertexElement[] VertexElements = new VertexElement[6]
      {
        new VertexElement((short) 0, (short) 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, (byte) 0),
        new VertexElement((short) 0, (short) 12, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, (byte) 0),
        new VertexElement((short) 0, (short) 20, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, (byte) 1),
        new VertexElement((short) 0, (short) 36, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, (byte) 2),
        new VertexElement((short) 0, (short) 48, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, (byte) 0),
        VertexElement.VertexDeclarationEnd
      };
      public Vector3 Position;
      public Vector2 uv;
      public Vector2 offset1;
      public Vector2 offset2;
      public Vector3 param;
      public int color;

      public static int SizeInBytes
      {
        get
        {
          return Marshal.SizeOf(typeof (d3d_sprite.sprite_vertex));
        }
      }

      static sprite_vertex()
      {
      }
    }
  }
}
