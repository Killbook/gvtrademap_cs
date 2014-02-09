// Type: gvtrademap_cs.draw_infonames
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using gvo_base;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace gvtrademap_cs
{
  internal class draw_infonames : IDisposable
  {
    private const int WIND_ANGLE_COLOR2 = 1627389951;
    private gvt_lib m_lib;
    private GvoWorldInfo m_world;
    private bool m_is_create_buffers;
    private bool m_is_error;
    private VertexDeclaration m_decl;
    private d3d_writable_vb_with_index m_icons1_vb;
    private d3d_writable_vb_with_index m_city_names1_vb;
    private d3d_writable_vb_with_index m_icons2_vb;
    private d3d_writable_vb_with_index m_city_names2_vb;
    private d3d_writable_vb_with_index m_sea_names1_vb;
    private d3d_writable_vb_with_index m_sea_names2_vb;

    public draw_infonames(gvt_lib lib, GvoWorldInfo world)
    {
      this.m_lib = lib;
      this.m_world = world;
      this.m_is_create_buffers = false;
      this.m_is_error = false;
      this.m_icons1_vb = (d3d_writable_vb_with_index) null;
      this.m_city_names1_vb = (d3d_writable_vb_with_index) null;
      this.m_icons2_vb = (d3d_writable_vb_with_index) null;
      this.m_city_names2_vb = (d3d_writable_vb_with_index) null;
      this.m_sea_names1_vb = (d3d_writable_vb_with_index) null;
      this.m_sea_names2_vb = (d3d_writable_vb_with_index) null;
    }

    public void Dispose()
    {
      if (this.m_decl != (VertexDeclaration) null)
        this.m_decl.Dispose();
      if (this.m_icons1_vb != null)
        this.m_icons1_vb.Dispose();
      if (this.m_city_names1_vb != null)
        this.m_city_names1_vb.Dispose();
      if (this.m_icons2_vb != null)
        this.m_icons2_vb.Dispose();
      if (this.m_city_names2_vb != null)
        this.m_city_names2_vb.Dispose();
      if (this.m_sea_names1_vb != null)
        this.m_sea_names1_vb.Dispose();
      if (this.m_sea_names2_vb != null)
        this.m_sea_names2_vb.Dispose();
      this.m_decl = (VertexDeclaration) null;
      this.m_icons1_vb = (d3d_writable_vb_with_index) null;
      this.m_city_names1_vb = (d3d_writable_vb_with_index) null;
      this.m_icons2_vb = (d3d_writable_vb_with_index) null;
      this.m_city_names2_vb = (d3d_writable_vb_with_index) null;
      this.m_sea_names1_vb = (d3d_writable_vb_with_index) null;
      this.m_sea_names2_vb = (d3d_writable_vb_with_index) null;
    }

    public void DrawCityName()
    {
      this.create_buffers();
      if (this.m_lib.device.sprites.effect != (Effect) null)
      {
        this.m_lib.device.device.RenderState.ZBufferEnable = false;
        this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_cityname_shader_proc), 64f);
        this.m_lib.device.device.RenderState.ZBufferEnable = true;
      }
      else
      {
        if ((double) this.m_lib.loop_image.ImageScale <= 0.600000023841858)
          return;
        this.m_lib.device.device.RenderState.ZBufferEnable = false;
        this.m_lib.device.sprites.BeginDrawSprites(this.m_lib.infonameimage.texture);
        this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_cityname_proc), 64f);
        this.m_lib.device.sprites.EndDrawSprites();
        this.m_lib.device.device.RenderState.ZBufferEnable = true;
      }
    }

    private void draw_cityname_proc(Vector2 offset, LoopXImage image)
    {
      if (this.m_lib.setting.map_icon == MapIcon.Big)
      {
        foreach (GvoWorldInfo.Info info in this.m_world.NoSeas)
        {
          Vector2 vector2 = image.GlobalPos2LocalPos(transform.ToVector2(info.position), offset);
          if (info.IconRect != null)
            this.m_lib.device.sprites.AddDrawSprites(new Vector3(vector2.X, vector2.Y, 0.3f), info.IconRect);
          if (this.m_lib.setting.map_draw_names != MapDrawNames.Hide && info.NameRect != null)
          {
            vector2.X += (float) info.StringOffset1.X;
            vector2.Y += (float) info.StringOffset1.Y;
            this.m_lib.device.sprites.AddDrawSprites(new Vector3(vector2.X, vector2.Y, 0.3f), info.NameRect);
          }
        }
      }
      else
      {
        foreach (GvoWorldInfo.Info info in this.m_world.NoSeas)
        {
          Vector2 vector2 = image.GlobalPos2LocalPos(transform.ToVector2(info.position), offset);
          if (info.SmallIconRect != null)
            this.m_lib.device.sprites.AddDrawSprites(new Vector3(vector2.X, vector2.Y, 0.3f), info.SmallIconRect);
          if (this.m_lib.setting.map_draw_names != MapDrawNames.Hide && info.NameRect != null)
          {
            vector2.X += (float) info.StringOffset2.X;
            vector2.Y += (float) info.StringOffset2.Y;
            this.m_lib.device.sprites.AddDrawSprites(new Vector3(vector2.X, vector2.Y, 0.3f), info.NameRect);
          }
        }
      }
    }

    private void draw_cityname_shader_proc(Vector2 offset, LoopXImage image)
    {
      this.set_shader_params(this.m_lib.infonameimage.texture, offset, image.ImageScale);
      Effect effect = this.m_lib.device.sprites.effect;
      if (effect == (Effect) null)
        return;
      effect.Begin(FX.None);
      effect.BeginPass(0);
      this.m_lib.device.device.VertexDeclaration = this.m_decl;
      if (this.m_lib.setting.map_icon == MapIcon.Big)
      {
        this.draw_buffer(this.m_icons1_vb, this.m_world.NoSeas.Count);
        if (this.m_lib.setting.map_draw_names == MapDrawNames.Draw)
          this.draw_buffer(this.m_city_names1_vb, this.m_world.NoSeas.Count);
      }
      else
      {
        this.draw_buffer(this.m_icons2_vb, this.m_world.NoSeas.Count);
        if (this.m_lib.setting.map_draw_names == MapDrawNames.Draw)
          this.draw_buffer(this.m_city_names2_vb, this.m_world.NoSeas.Count);
      }
      effect.EndPass();
      effect.End();
    }

    public void DrawSeaName()
    {
      this.create_buffers();
      if (this.m_lib.device.sprites.effect != (Effect) null)
      {
        this.m_lib.device.device.RenderState.ZBufferEnable = false;
        this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_seaname_shader_proc), 64f);
        this.m_lib.device.device.RenderState.ZBufferEnable = true;
      }
      else
      {
        if ((double) this.m_lib.loop_image.ImageScale <= 0.600000023841858)
          return;
        this.m_lib.device.device.RenderState.ZBufferEnable = false;
        this.m_lib.device.sprites.BeginDrawSprites(this.m_lib.seainfonameimage.texture);
        this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_seaname_proc), 64f);
        this.m_lib.device.sprites.EndDrawSprites();
        this.m_lib.device.device.RenderState.ZBufferEnable = true;
      }
    }

    private void draw_seaname_proc(Vector2 offset, LoopXImage image)
    {
      d3d_sprite_rects.rect windArrowIcon = this.m_lib.seainfonameimage.GetWindArrowIcon();
      int color1 = this.m_world.Season.now_season == gvo_season.season.summer ? -1 : 1627389951;
      int color2 = this.m_world.Season.now_season == gvo_season.season.winter ? -1 : 1627389951;
      int index = 0;
      foreach (GvoWorldInfo.Info info in this.m_world.Seas)
      {
        Vector2 vector2 = image.GlobalPos2LocalPos(transform.ToVector2(info.position), offset);
        this.m_lib.device.sprites.AddDrawSprites(new Vector3(vector2.X - 6f, vector2.Y, 0.3f), this.m_lib.seainfonameimage.GetRect(index));
        ++index;
        if (info.SeaInfo != null)
        {
          vector2.X += (float) (info.SeaInfo.WindPos.X - info.position.X);
          vector2.Y += (float) (info.SeaInfo.WindPos.Y - info.position.Y);
          this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3(vector2.X - 6f, vector2.Y, 0.3f), windArrowIcon, info.SeaInfo.SummerAngle, color1);
          this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3(vector2.X + 6f, vector2.Y, 0.3f), windArrowIcon, info.SeaInfo.WinterAngle, color2);
        }
      }
    }

    private void draw_seaname_shader_proc(Vector2 offset, LoopXImage image)
    {
      this.set_shader_params(this.m_lib.seainfonameimage.texture, offset, image.ImageScale);
      Effect effect = this.m_lib.device.sprites.effect;
      if (effect == (Effect) null)
        return;
      effect.Begin(FX.None);
      effect.BeginPass(0);
      this.m_lib.device.device.VertexDeclaration = this.m_decl;
      if (this.m_world.Season.now_season == gvo_season.season.summer)
        this.draw_buffer(this.m_sea_names1_vb, this.m_world.Seas.Count * 3);
      else
        this.draw_buffer(this.m_sea_names2_vb, this.m_world.Seas.Count * 3);
      effect.EndPass();
      effect.End();
    }

    private void set_shader_params(Texture tex, Vector2 offset, float mapscale)
    {
      Effect effect = this.m_lib.device.sprites.effect;
      if (effect == (Effect) null)
        return;
      try
      {
        float num = mapscale;
        if ((double) num >= 0.699999988079071)
          num = 1f;
        if ((double) num < 0.5)
          num = 0.5f;
        float[] f1 = new float[2]
        {
          this.m_lib.device.client_size.X,
          this.m_lib.device.client_size.Y
        };
        float[] f2 = new float[2]
        {
          offset.X,
          offset.Y
        };
        float[] f3 = new float[2]
        {
          num,
          num
        };
        effect.SetValue((EffectHandle) "ViewportSize", f1);
        effect.SetValue((EffectHandle) "Texture", (BaseTexture) tex);
        effect.SetValue((EffectHandle) "MapOffset", f2);
        effect.SetValue((EffectHandle) "MapScale", mapscale);
        effect.SetValue((EffectHandle) "GlobalScale", f3);
        effect.Technique = (EffectHandle) "SpriteWithGlobalParams";
      }
      catch
      {
        this.m_lib.device.sprites.DisposeEffect();
      }
    }

    private void create_buffers()
    {
      if (!this.m_lib.device.is_use_ve1_1_ps1_1 || this.m_is_create_buffers)
        return;
      if (this.m_is_error)
        return;
      try
      {
        this.m_decl = new VertexDeclaration(this.m_lib.device.device, draw_infonames.sprite_vertex.VertexElements);
        this.create_city_buffer();
        this.create_sea_buffer();
        this.m_is_create_buffers = true;
        this.m_is_error = false;
      }
      catch
      {
        this.m_is_create_buffers = false;
        this.m_icons1_vb = (d3d_writable_vb_with_index) null;
        this.m_city_names1_vb = (d3d_writable_vb_with_index) null;
        this.m_icons2_vb = (d3d_writable_vb_with_index) null;
        this.m_city_names2_vb = (d3d_writable_vb_with_index) null;
        this.m_sea_names1_vb = (d3d_writable_vb_with_index) null;
        this.m_sea_names2_vb = (d3d_writable_vb_with_index) null;
        this.m_is_error = true;
      }
    }

    private d3d_writable_vb_with_index create_buffer(int sprite_count)
    {
      return new d3d_writable_vb_with_index(this.m_lib.device.device, typeof (draw_infonames.sprite_vertex), sprite_count * 4, 1);
    }

    private void create_city_buffer()
    {
      this.m_icons1_vb = this.create_buffer(this.m_world.NoSeas.Count);
      this.m_icons2_vb = this.create_buffer(this.m_world.NoSeas.Count);
      this.m_city_names1_vb = this.create_buffer(this.m_world.NoSeas.Count);
      this.m_city_names2_vb = this.create_buffer(this.m_world.NoSeas.Count);
      draw_infonames.sprite_vertex[] tbl1 = new draw_infonames.sprite_vertex[this.m_world.NoSeas.Count * 4];
      draw_infonames.sprite_vertex[] tbl2 = new draw_infonames.sprite_vertex[this.m_world.NoSeas.Count * 4];
      draw_infonames.sprite_vertex[] tbl3 = new draw_infonames.sprite_vertex[this.m_world.NoSeas.Count * 4];
      draw_infonames.sprite_vertex[] tbl4 = new draw_infonames.sprite_vertex[this.m_world.NoSeas.Count * 4];
      int index = 0;
      foreach (GvoWorldInfo.Info info in this.m_world.NoSeas)
      {
        this.set_vbo(ref tbl1, index, info.position, new Point(0, 0), info.IconRect, -1);
        this.set_vbo(ref tbl2, index, info.position, new Point(0, 0), info.SmallIconRect, -1);
        this.set_vbo(ref tbl3, index, info.position, info.StringOffset1, info.NameRect, -1);
        this.set_vbo(ref tbl4, index, info.position, info.StringOffset2, info.NameRect, -1);
        ++index;
      }
      this.m_icons1_vb.SetData<draw_infonames.sprite_vertex>(tbl1);
      this.m_icons2_vb.SetData<draw_infonames.sprite_vertex>(tbl2);
      this.m_city_names1_vb.SetData<draw_infonames.sprite_vertex>(tbl3);
      this.m_city_names2_vb.SetData<draw_infonames.sprite_vertex>(tbl4);
    }

    private void create_sea_buffer()
    {
      this.m_sea_names1_vb = this.create_buffer(this.m_world.Seas.Count * 3);
      this.m_sea_names2_vb = this.create_buffer(this.m_world.Seas.Count * 3);
      draw_infonames.sprite_vertex[] tbl1 = new draw_infonames.sprite_vertex[this.m_world.Seas.Count * 3 * 4];
      draw_infonames.sprite_vertex[] tbl2 = new draw_infonames.sprite_vertex[this.m_world.Seas.Count * 3 * 4];
      int index1 = 0;
      foreach (GvoWorldInfo.Info info in this.m_world.Seas)
      {
        this.set_vbo(ref tbl1, index1, info.position, new Point(-6, 0), this.m_lib.seainfonameimage.GetRect(index1), -1);
        this.set_vbo(ref tbl2, index1, info.position, new Point(-6, 0), this.m_lib.seainfonameimage.GetRect(index1), -1);
        ++index1;
      }
      foreach (GvoWorldInfo.Info info in this.m_world.Seas)
      {
        if (info.SeaInfo != null)
        {
          Point point = new Point(info.SeaInfo.WindPos.X - info.position.X, info.SeaInfo.WindPos.Y - info.position.Y);
          this.set_vbo(ref tbl1, index1, info.position, new Point(point.X - 6, point.Y), this.m_lib.seainfonameimage.GetWindArrowIcon(), info.SeaInfo.SummerAngle, -1);
          this.set_vbo(ref tbl2, index1, info.position, new Point(point.X - 6, point.Y), this.m_lib.seainfonameimage.GetWindArrowIcon(), info.SeaInfo.SummerAngle, 1627389951);
          int index2 = index1 + 1;
          this.set_vbo(ref tbl1, index2, info.position, new Point(point.X + 6, point.Y), this.m_lib.seainfonameimage.GetWindArrowIcon(), info.SeaInfo.WinterAngle, 1627389951);
          this.set_vbo(ref tbl2, index2, info.position, new Point(point.X + 6, point.Y), this.m_lib.seainfonameimage.GetWindArrowIcon(), info.SeaInfo.WinterAngle, -1);
          index1 = index2 + 1;
        }
      }
      this.m_sea_names1_vb.SetData<draw_infonames.sprite_vertex>(tbl1);
      this.m_sea_names2_vb.SetData<draw_infonames.sprite_vertex>(tbl2);
    }

    private void set_vbo(ref draw_infonames.sprite_vertex[] tbl, int index, Point position, Point offset, d3d_sprite_rects.rect _rect, int color)
    {
      this.set_vbo(ref tbl, index, position, offset, _rect, 0.0f, color);
    }

    private void set_vbo(ref draw_infonames.sprite_vertex[] tbl, int index, Point position, Point offset, d3d_sprite_rects.rect _rect, float angle_rad, int color)
    {
      if (_rect == null)
        return;
      Vector3 vector3_1 = new Vector3((float) position.X, (float) position.Y, 0.3f);
      Vector3 vector3_2 = new Vector3(1f, 1f, angle_rad);
      Vector2 vector2 = new Vector2((float) offset.X, (float) offset.Y);
      index *= 4;
      for (int index1 = 0; index1 < 4; ++index1)
      {
        tbl[index + index1].color = color;
        tbl[index + index1].Position = vector3_1;
        tbl[index + index1].offset1 = _rect.offset[index1];
        tbl[index + index1].offset2 = vector2;
        tbl[index + index1].param = vector3_2;
        tbl[index + index1].uv = _rect.uv[index1];
      }
    }

    private void draw_buffer(d3d_writable_vb_with_index vb, int sprite_count)
    {
      this.m_lib.device.device.SetStreamSource(0, vb.vb, 0, draw_infonames.sprite_vertex.SizeInBytes);
      this.m_lib.device.device.Indices = vb.ib;
      this.m_lib.device.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, sprite_count * 4, 0, sprite_count * 2);
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
          return Marshal.SizeOf(typeof (draw_infonames.sprite_vertex));
        }
      }

      static sprite_vertex()
      {
      }
    }
  }
}
