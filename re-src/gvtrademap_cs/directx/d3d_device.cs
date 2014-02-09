// Type: directx.d3d_device
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace directx
{
  public class d3d_device : d3d_base_device
  {
    private Microsoft.DirectX.Direct3D.Font m_font;
    private Microsoft.DirectX.Direct3D.Font m_font_small;
    private Line m_line;
    private Sprite m_font_sprite;
    private bool m_is_use_font_sprite;
    private bool m_is_use_ve1_1_ps1_1;
    private string m_device_info_string;
    private string m_device_info_string_short;
    private d3d_point m_points;
    private d3d_sprite m_sprite;
    private d3d_systemfont m_systemfont;
    private d3d_textured_font m_textured_font;
    private Vector2 m_client_size;
    private int m_skip_count;
    private int m_skip_max;
    private bool m_is_must_draw;

    public Line line
    {
      get
      {
        return this.m_line;
      }
    }

    public d3d_systemfont systemfont
    {
      get
      {
        return this.m_systemfont;
      }
    }

    public d3d_textured_font textured_font
    {
      get
      {
        return this.m_textured_font;
      }
    }

    public Vector2 client_size
    {
      get
      {
        return this.m_client_size;
      }
    }

    public d3d_point points
    {
      get
      {
        return this.m_points;
      }
    }

    public d3d_sprite sprites
    {
      get
      {
        return this.m_sprite;
      }
    }

    public bool is_use_ve1_1_ps1_1
    {
      get
      {
        return this.m_is_use_ve1_1_ps1_1;
      }
    }

    public string deviec_info_string
    {
      get
      {
        return this.m_device_info_string;
      }
    }

    public string deviec_info_string_short
    {
      get
      {
        return this.m_device_info_string_short;
      }
    }

    public bool now_use_shader
    {
      get
      {
        return this.is_use_ve1_1_ps1_1 && !(this.m_sprite.effect == (Effect) null);
      }
    }

    public int skip_count
    {
      get
      {
        return this.m_skip_count;
      }
    }

    public int skip_max
    {
      get
      {
        return this.m_skip_max;
      }
      set
      {
        this.m_skip_max = value;
      }
    }

    public d3d_device(Form form)
    {
      PresentParameters presentParameters = new PresentParameters();
      presentParameters.Windowed = true;
      presentParameters.SwapEffect = SwapEffect.Discard;
      presentParameters.PresentationInterval = PresentInterval.Immediate;
      presentParameters.EnableAutoDepthStencil = true;
      presentParameters.AutoDepthStencilFormat = DepthFormat.D16;
      presentParameters.BackBufferCount = 1;
      presentParameters.BackBufferFormat = Microsoft.DirectX.Direct3D.Format.Unknown;
      try
      {
        this.Create(form, presentParameters);
      }
      catch
      {
        int num = (int) MessageBox.Show("DirectXの初期化に失敗しました。", "初期化エラ\x30FC");
        return;
      }
      try
      {
        this.device.DeviceReset += new EventHandler(this.device_reset);
        this.check_shader_support();
        this.m_font = new Microsoft.DirectX.Direct3D.Font(this.device, new System.Drawing.Font("MS UI Gothic", 9f));
        this.m_font_small = new Microsoft.DirectX.Direct3D.Font(this.device, new System.Drawing.Font("MS UI Gothic", 8f));
        this.m_font_sprite = new Sprite(this.device);
        this.m_is_use_font_sprite = false;
        this.m_line = new Line(this.device);
        this.device_reset((object) this, (EventArgs) null);
        this.m_points = new d3d_point(this.device);
        this.m_sprite = new d3d_sprite(this.device, this.is_use_ve1_1_ps1_1);
        this.m_systemfont = new d3d_systemfont(this);
        this.m_textured_font = new d3d_textured_font(this, this.m_font);
        this.get_device_information();
        this.UpdateClientSize();
      }
      catch
      {
        int num = (int) MessageBox.Show("DirectXの初期化後の設定に失敗しました。", "初期化エラ\x30FC");
        base.Dispose();
      }
      this.m_skip_count = 0;
      this.m_skip_max = 0;
      this.m_is_must_draw = false;
    }

    public override void Dispose()
    {
      if (this.m_textured_font != null)
        this.m_textured_font.Dispose();
      if (this.m_systemfont != null)
        this.m_systemfont.Dispose();
      if (this.m_sprite != null)
        this.m_sprite.Dispose();
      if (this.m_font_sprite != (Sprite) null)
        this.m_font_sprite.Dispose();
      if (this.m_line != (Line) null)
        this.m_line.Dispose();
      if (this.m_font_small != (Microsoft.DirectX.Direct3D.Font) null)
        this.m_font_small.Dispose();
      if (this.m_font != (Microsoft.DirectX.Direct3D.Font) null)
        this.m_font.Dispose();
      this.m_textured_font = (d3d_textured_font) null;
      this.m_systemfont = (d3d_systemfont) null;
      this.m_sprite = (d3d_sprite) null;
      this.m_font_sprite = (Sprite) null;
      this.m_line = (Line) null;
      this.m_font_small = (Microsoft.DirectX.Direct3D.Font) null;
      this.m_font = (Microsoft.DirectX.Direct3D.Font) null;
      base.Dispose();
    }

    private void check_shader_support()
    {
      Version version = new Version(1, 1);
      Caps caps = this.caps;
      if (caps.VertexShaderVersion >= version && caps.PixelShaderVersion >= version)
        this.m_is_use_ve1_1_ps1_1 = true;
      else
        this.m_is_use_ve1_1_ps1_1 = false;
    }

    private void get_device_information()
    {
      Caps caps = this.caps;
      AdapterDetails adapterDetails = Manager.Adapters[this.adpter_index].Information;
      this.m_device_info_string_short = adapterDetails.Description;
      this.m_device_info_string = adapterDetails.Description + "\n";
      d3d_device d3dDevice1 = this;
      string str1 = d3dDevice1.m_device_info_string + "VertexShader: " + ((object) caps.VertexShaderVersion).ToString() + "  ";
      d3dDevice1.m_device_info_string = str1;
      d3d_device d3dDevice2 = this;
      string str2 = d3dDevice2.m_device_info_string + "PixelShader: " + ((object) caps.PixelShaderVersion).ToString() + "\n";
      d3dDevice2.m_device_info_string = str2;
      d3d_device d3dDevice3 = this;
      string str3 = d3dDevice3.m_device_info_string + "頂点処理:";
      d3dDevice3.m_device_info_string = str3;
      if ((this.create_flags & CreateFlags.HardwareVertexProcessing) != (CreateFlags) 0)
      {
        d3d_device d3dDevice4 = this;
        string str4 = d3dDevice4.m_device_info_string + "HardwareVertexProcessing";
        d3dDevice4.m_device_info_string = str4;
        if ((this.create_flags & CreateFlags.PureDevice) != (CreateFlags) 0)
        {
          d3d_device d3dDevice5 = this;
          string str5 = d3dDevice5.m_device_info_string + "(PureDevice)\n";
          d3dDevice5.m_device_info_string = str5;
        }
        else
        {
          d3d_device d3dDevice5 = this;
          string str5 = d3dDevice5.m_device_info_string + "\n";
          d3dDevice5.m_device_info_string = str5;
        }
      }
      else if ((this.create_flags & CreateFlags.SoftwareVertexProcessing) != (CreateFlags) 0)
      {
        d3d_device d3dDevice4 = this;
        string str4 = d3dDevice4.m_device_info_string + "SoftwareVertexProcessing\n";
        d3dDevice4.m_device_info_string = str4;
      }
      d3d_device d3dDevice6 = this;
      string str6 = d3dDevice6.m_device_info_string + "Vertex/Pixel Shader:";
      d3dDevice6.m_device_info_string = str6;
      d3d_device d3dDevice7 = this;
      string str7 = d3dDevice7.m_device_info_string + (this.now_use_shader ? "有効" : "無効");
      d3dDevice7.m_device_info_string = str7;
    }

    private void device_reset(object sender, EventArgs e)
    {
      this.device.RenderState.CullMode = Cull.None;
      this.device.RenderState.Lighting = false;
      this.device.RenderState.ZBufferEnable = true;
      this.device.RenderState.ZBufferFunction = Compare.LessEqual;
      this.device.RenderState.AlphaBlendEnable = true;
      this.device.RenderState.AlphaTestEnable = true;
      this.device.RenderState.AlphaFunction = Compare.Greater;
      this.device.RenderState.ReferenceAlpha = 0;
      this.device.RenderState.SourceBlend = Blend.SourceAlpha;
      this.device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
      this.device.SamplerState[0].MagFilter = TextureFilter.Linear;
      this.device.SamplerState[0].MinFilter = TextureFilter.Linear;
      this.device.SamplerState[0].AddressU = TextureAddress.Clamp;
      this.device.SamplerState[0].AddressV = TextureAddress.Clamp;
      this.device.TextureState[0].AlphaOperation = TextureOperation.Modulate;
      this.device.TextureState[0].AlphaArgument1 = TextureArgument.TextureColor;
      this.device.TextureState[0].AlphaArgument2 = TextureArgument.Current;
    }

    public void SetMustDrawFlag()
    {
      this.m_is_must_draw = true;
    }

    public bool IsNeedDraw()
    {
      if (this.m_is_must_draw || this.m_skip_count >= this.m_skip_max)
        return true;
      ++this.m_skip_count;
      return false;
    }

    public override bool Begin()
    {
      if (!base.Begin())
        return false;
      this.UpdateClientSize();
      this.m_sprite.BeginFrame();
      this.m_systemfont.BeginFrame();
      return true;
    }

    public override bool Present()
    {
      this.m_skip_count = 0;
      this.m_is_must_draw = false;
      return base.Present();
    }

    public void UpdateClientSize()
    {
      if (this.device != (Device) null)
        this.m_client_size = new Vector2((float) this.device.Viewport.Width, (float) this.device.Viewport.Height);
      else
        this.m_client_size = new Vector2(100f, 100f);
    }

    public void DrawTexture(Texture tex, Vector3 pos, Vector2 size)
    {
      this.DrawTexture(tex, pos, size, Color.White.ToArgb());
    }

    public void DrawTexture(Texture tex, Vector3 pos, Vector2 size, int color)
    {
      CustomVertex.TransformedColoredTextured[] transformedColoredTexturedArray = new CustomVertex.TransformedColoredTextured[4];
      pos.X -= 0.5f;
      pos.Y -= 0.5f;
      for (int index = 0; index < 4; ++index)
      {
        transformedColoredTexturedArray[index].Color = color;
        transformedColoredTexturedArray[index].Rhw = 1f;
        transformedColoredTexturedArray[index].Z = pos.Z;
      }
      transformedColoredTexturedArray[0].X = pos.X;
      transformedColoredTexturedArray[0].Y = pos.Y;
      transformedColoredTexturedArray[0].Tu = 0.0f;
      transformedColoredTexturedArray[0].Tv = 0.0f;
      transformedColoredTexturedArray[1].X = pos.X + size.X;
      transformedColoredTexturedArray[1].Y = pos.Y;
      transformedColoredTexturedArray[1].Tu = 1f;
      transformedColoredTexturedArray[1].Tv = 0.0f;
      transformedColoredTexturedArray[2].X = pos.X;
      transformedColoredTexturedArray[2].Y = pos.Y + size.Y;
      transformedColoredTexturedArray[2].Tu = 0.0f;
      transformedColoredTexturedArray[2].Tv = 1f;
      transformedColoredTexturedArray[3].X = pos.X + size.X;
      transformedColoredTexturedArray[3].Y = pos.Y + size.Y;
      transformedColoredTexturedArray[3].Tu = 1f;
      transformedColoredTexturedArray[3].Tv = 1f;
      this.device.VertexFormat = VertexFormats.Texture1 | VertexFormats.Diffuse | VertexFormats.Transformed;
      this.device.SetTexture(0, (BaseTexture) tex);
      this.device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, (object) transformedColoredTexturedArray);
    }

    public void DrawFillRect(Vector3 pos, Vector2 size, int color)
    {
      CustomVertex.TransformedColored[] transformedColoredArray = new CustomVertex.TransformedColored[4];
      for (int index = 0; index < 4; ++index)
      {
        transformedColoredArray[index].Color = color;
        transformedColoredArray[index].Rhw = 1f;
        transformedColoredArray[index].Z = pos.Z;
      }
      transformedColoredArray[0].X = pos.X;
      transformedColoredArray[0].Y = pos.Y;
      transformedColoredArray[1].X = pos.X + size.X;
      transformedColoredArray[1].Y = pos.Y;
      transformedColoredArray[2].X = pos.X;
      transformedColoredArray[2].Y = pos.Y + size.Y;
      transformedColoredArray[3].X = pos.X + size.X;
      transformedColoredArray[3].Y = pos.Y + size.Y;
      this.device.VertexFormat = VertexFormats.Diffuse | VertexFormats.Transformed;
      this.device.SetTexture(0, (BaseTexture) null);
      this.device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, (object) transformedColoredArray);
    }

    public void DrawLineRect(Vector3 pos, Vector2 size, int color)
    {
      CustomVertex.TransformedColored[] transformedColoredArray = new CustomVertex.TransformedColored[5];
      for (int index = 0; index < 5; ++index)
      {
        transformedColoredArray[index].Color = color;
        transformedColoredArray[index].Rhw = 1f;
        transformedColoredArray[index].Z = pos.Z;
      }
      transformedColoredArray[0].X = pos.X;
      transformedColoredArray[0].Y = pos.Y;
      transformedColoredArray[1].X = pos.X + size.X;
      transformedColoredArray[1].Y = pos.Y;
      transformedColoredArray[2].X = pos.X + size.X;
      transformedColoredArray[2].Y = pos.Y + size.Y;
      transformedColoredArray[3].X = pos.X;
      transformedColoredArray[3].Y = pos.Y + size.Y;
      transformedColoredArray[4].X = pos.X;
      transformedColoredArray[4].Y = pos.Y;
      this.device.VertexFormat = VertexFormats.Diffuse | VertexFormats.Transformed;
      this.device.SetTexture(0, (BaseTexture) null);
      this.device.DrawUserPrimitives(PrimitiveType.LineStrip, 4, (object) transformedColoredArray);
    }

    public void DrawLine(Vector3 pos, Vector2 pos2, int color)
    {
      CustomVertex.TransformedColored[] transformedColoredArray = new CustomVertex.TransformedColored[2];
      transformedColoredArray[0].X = pos.X;
      transformedColoredArray[0].Y = pos.Y;
      transformedColoredArray[0].Rhw = 1f;
      transformedColoredArray[0].Z = pos.Z;
      transformedColoredArray[0].Color = color;
      transformedColoredArray[1].X = pos2.X;
      transformedColoredArray[1].Y = pos2.Y;
      transformedColoredArray[1].Rhw = 1f;
      transformedColoredArray[1].Z = pos.Z;
      transformedColoredArray[1].Color = color;
      this.device.VertexFormat = VertexFormats.Diffuse | VertexFormats.Transformed;
      this.device.SetTexture(0, (BaseTexture) null);
      this.device.DrawUserPrimitives(PrimitiveType.LineList, 1, (object) transformedColoredArray);
    }

    public void DrawLineStrip(Vector3 pos, Vector2[] vec, int color)
    {
      CustomVertex.TransformedColored[] transformedColoredArray = new CustomVertex.TransformedColored[vec.Length];
      for (int index = 0; index < vec.Length; ++index)
      {
        transformedColoredArray[index].X = pos.X + vec[index].X;
        transformedColoredArray[index].Y = pos.Y + vec[index].Y;
        transformedColoredArray[index].Z = pos.Z;
        transformedColoredArray[index].Rhw = 1f;
        transformedColoredArray[index].Color = color;
      }
      this.device.VertexFormat = VertexFormats.Diffuse | VertexFormats.Transformed;
      this.device.SetTexture(0, (BaseTexture) null);
      this.device.DrawUserPrimitives(PrimitiveType.LineStrip, vec.Length - 1, (object) transformedColoredArray);
    }

    public void BeginFont()
    {
      this.m_font_sprite.Begin(SpriteFlags.SortTexture | SpriteFlags.AlphaBlend);
      this.m_is_use_font_sprite = true;
    }

    public void EndFont()
    {
      this.m_font_sprite.End();
      this.m_is_use_font_sprite = false;
    }

    public void DrawText(font_type type, string str, int x, int y, Color color)
    {
      (type != font_type.normal ? this.m_font_small : this.m_font).DrawText(this.m_is_use_font_sprite ? this.m_font_sprite : (Sprite) null, str, new Point(x, y + 1), color);
    }

    public Rectangle MeasureText(font_type type, string str, Color color)
    {
      return (type != font_type.normal ? this.m_font_small : this.m_font).MeasureString(this.m_is_use_font_sprite ? this.m_font_sprite : (Sprite) null, str, DrawTextFormat.None, color);
    }

    public void DrawTextR(font_type type, string str, int x, int y, Color color)
    {
      Microsoft.DirectX.Direct3D.Font font = type != font_type.normal ? this.m_font_small : this.m_font;
      Sprite sprite = this.m_is_use_font_sprite ? this.m_font_sprite : (Sprite) null;
      Rectangle rectangle = font.MeasureString(sprite, str, DrawTextFormat.None, color);
      font.DrawText(sprite, str, new Point(x - rectangle.Width, y + 1), color);
    }

    public void DrawTextC(font_type type, string str, int x, int y, Color color)
    {
      Microsoft.DirectX.Direct3D.Font font = type != font_type.normal ? this.m_font_small : this.m_font;
      Sprite sprite = this.m_is_use_font_sprite ? this.m_font_sprite : (Sprite) null;
      Rectangle rectangle = font.MeasureString(sprite, str, DrawTextFormat.None, color);
      font.DrawText(sprite, str, new Point(x - rectangle.Width / 2, y + 1), color);
    }

    public Point GetClientMousePosition()
    {
      return this.form.PointToClient(Control.MousePosition);
    }
  }
}
