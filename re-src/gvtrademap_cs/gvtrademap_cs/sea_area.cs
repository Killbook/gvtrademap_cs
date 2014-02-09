// Type: gvtrademap_cs.sea_area
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Utility;

namespace gvtrademap_cs
{
  public class sea_area : IDisposable
  {
    private const int ALPHA_MIN = 35;
    private const int ALPHA_MAX = 60;
    private const int ALPHA_CENTER = 12;
    private const int ANGLE_STEP = 2;
    private const int ANGLE_STEP2 = 4;
    private gvt_lib m_lib;
    private float m_angle;
    private float m_angle2;
    private int m_alpha;
    private int m_alpha2;
    private List<sea_area.sea_area_once> m_groups;
    private sea_area.color_type m_color_type;
    private int m_progress_max;
    private int m_progress_current;
    private string m_progress_info_str;
    private bool m_is_loaded_mask;

    public List<sea_area.sea_area_once> groups
    {
      get
      {
        return this.m_groups;
      }
    }

    public sea_area.color_type color
    {
      get
      {
        return this.m_color_type;
      }
      set
      {
        this.m_color_type = value;
      }
    }

    public int progress_max
    {
      get
      {
        return this.m_progress_max;
      }
    }

    public int progress_current
    {
      get
      {
        return this.m_progress_current;
      }
    }

    public string progress_info_str
    {
      get
      {
        return this.m_progress_info_str;
      }
    }

    public bool IsLoadedMask
    {
      get
      {
        return this.m_is_loaded_mask;
      }
    }

    public sea_area(gvt_lib lib, string fname)
    {
      this.m_lib = lib;
      this.m_angle = 0.0f;
      this.m_angle2 = 0.0f;
      this.m_color_type = sea_area.color_type.type1;
      this.m_groups = new List<sea_area.sea_area_once>();
      this.m_progress_max = 0;
      this.m_progress_current = 0;
      this.m_progress_info_str = "";
      this.m_is_loaded_mask = false;
      sea_area.sea_area_once seaAreaOnce1 = new sea_area.sea_area_once("カリブ海");
      seaAreaOnce1.Add("サンフアン沖", new Vector2(3970f, 1049f), new Vector2(73f, 149f), true);
      seaAreaOnce1.Add("アンティル諸島沖", new Vector2(3819f, 1049f), new Vector2(150f, 149f), true);
      seaAreaOnce1.Add("中央大西洋", new Vector2(4044f, 1199f), new Vector2(373f, 148f), false);
      seaAreaOnce1.Add("西カリブ海", new Vector2(3670f, 1049f), new Vector2(148f, 136f), true);
      seaAreaOnce1.Add("", new Vector2(3753f, 1185f), new Vector2(65f, 124f), true);
      seaAreaOnce1.Add("コッド岬沖", new Vector2(3819f, 750f), new Vector2(224f, 148f), true);
      seaAreaOnce1.Add("バミュ\x30FCダ諸島沖", new Vector2(3717f, 899f), new Vector2(326f, 149f), true);
      seaAreaOnce1.Add("テラ・ノヴァ海", new Vector2(3789f, 600f), new Vector2(254f, 149f), true);
      this.m_groups.Add(seaAreaOnce1);
      sea_area.sea_area_once seaAreaOnce2 = new sea_area.sea_area_once("アフリカ西岸");
      seaAreaOnce2.Add("穀物海岸沖", new Vector2(4418f, 1199f), new Vector2(224f, 299f), true);
      seaAreaOnce2.Add("黄金海岸沖", new Vector2(4643f, 1325f), new Vector2(149f, 173f), true);
      seaAreaOnce2.Add("ギニア湾", new Vector2(1f, 1322f), new Vector2(164f, 176f), true);
      this.m_groups.Add(seaAreaOnce2);
      sea_area.sea_area_once seaAreaOnce3 = new sea_area.sea_area_once("南大西洋");
      seaAreaOnce3.Add("ナミビア沖", new Vector2(1f, 1499f), new Vector2(180f, 149f), true);
      seaAreaOnce3.Add("喜望峰沖", new Vector2(1f, 1649f), new Vector2(297f, 223f), true);
      seaAreaOnce3.Add("ケ\x30FCプ海盆", new Vector2(1f, 1873f), new Vector2(297f, 224f), false);
      seaAreaOnce3.Add("南大西洋", new Vector2(4494f, 1499f), new Vector2(298f, 599f), false);
      this.m_groups.Add(seaAreaOnce3);
      sea_area.sea_area_once seaAreaOnce4 = new sea_area.sea_area_once("アフリカ東岸");
      seaAreaOnce4.Add("アガラス岬沖", new Vector2(299f, 1683f), new Vector2(149f, 189f), true);
      seaAreaOnce4.Add("アガラス海盆", new Vector2(299f, 1873f), new Vector2(299f, 225f), false);
      seaAreaOnce4.Add("モザンビ\x30FCク海峡", new Vector2(449f, 1649f), new Vector2(149f, 223f), true);
      seaAreaOnce4.Add("マダガスカル沖", new Vector2(458f, 1500f), new Vector2(440f, 149f), true);
      seaAreaOnce4.Add("南西インド洋", new Vector2(599f, 1649f), new Vector2(299f, 449f), true);
      this.m_groups.Add(seaAreaOnce4);
      sea_area.sea_area_once seaAreaOnce5 = new sea_area.sea_area_once("紅海");
      seaAreaOnce5.Add("ザンジバル沖", new Vector2(513f, 1348f), new Vector2(385f, 151f), true);
      seaAreaOnce5.Add("アラビア海", new Vector2(600f, 1199f), new Vector2(298f, 149f), true);
      seaAreaOnce5.Add("紅海", new Vector2(457f, 1076f), new Vector2(142f, 202f), true);
      seaAreaOnce5.Add("ペルシャ湾", new Vector2(638f, 1086f), new Vector2(260f, 112f), true);
      this.m_groups.Add(seaAreaOnce5);
      sea_area.sea_area_once seaAreaOnce6 = new sea_area.sea_area_once("インド洋");
      seaAreaOnce6.Add("インド西岸沖", new Vector2(899f, 1125f), new Vector2(109f, 149f), true);
      seaAreaOnce6.Add("インド南岸沖", new Vector2(899f, 1274f), new Vector2(224f, 149f), true);
      seaAreaOnce6.Add("ベンガル湾", new Vector2(1067f, 1134f), new Vector2(272f, 139f), true);
      seaAreaOnce6.Add("中部インド洋", new Vector2(899f, 1424f), new Vector2(449f, 224f), true);
      seaAreaOnce6.Add("南インド洋", new Vector2(899f, 1649f), new Vector2(301f, 449f), false);
      seaAreaOnce6.Add("南東インド洋", new Vector2(1201f, 1649f), new Vector2(296f, 449f), false);
      this.m_groups.Add(seaAreaOnce6);
      sea_area.sea_area_once seaAreaOnce7 = new sea_area.sea_area_once("中南米東岸");
      seaAreaOnce7.Add("南カリブ海", new Vector2(3819f, 1199f), new Vector2(224f, 148f), true);
      seaAreaOnce7.Add("メキシコ湾", new Vector2(3497f, 1055f), new Vector2(173f, 132f), true);
      seaAreaOnce7.Add("サンロケ岬沖", new Vector2(4194f, 1348f), new Vector2(223f, 150f), true);
      seaAreaOnce7.Add("アマゾン川流域", new Vector2(3900f, 1348f), new Vector2(293f, 117f), true);
      seaAreaOnce7.Add("南西大西洋", new Vector2(4194f, 1499f), new Vector2(299f, 373f), true);
      seaAreaOnce7.Add("ブエノスアイレス沖", new Vector2(3946f, 1678f), new Vector2(248f, 195f), true);
      seaAreaOnce7.Add("アルゼンチン海盆", new Vector2(3894f, 1873f), new Vector2(374f, 225f), true);
      seaAreaOnce7.Add("ジョ\x30FCジア海盆", new Vector2(4269f, 1873f), new Vector2(224f, 225f), false);
      this.m_groups.Add(seaAreaOnce7);
      sea_area.sea_area_once seaAreaOnce8 = new sea_area.sea_area_once("東南アジア");
      seaAreaOnce8.Add("アンダマン海", new Vector2(1124f, 1274f), new Vector2(224f, 149f), true);
      seaAreaOnce8.Add("ジャワ海", new Vector2(1349f, 1348f), new Vector2(224f, 150f), true);
      seaAreaOnce8.Add("ジャワ島南方沖", new Vector2(1349f, 1500f), new Vector2(224f, 73f), false);
      seaAreaOnce8.Add("", new Vector2(1349f, 1572f), new Vector2(148f, 76f), false);
      seaAreaOnce8.Add("シャム湾", new Vector2(1349f, 1199f), new Vector2(149f, 148f), true);
      seaAreaOnce8.Add("バンダ海", new Vector2(1574f, 1423f), new Vector2(298f, 150f), true);
      seaAreaOnce8.Add("セレベス海", new Vector2(1499f, 1199f), new Vector2(223f, 148f), true);
      seaAreaOnce8.Add("", new Vector2(1574f, 1347f), new Vector2(148f, 76f), true);
      seaAreaOnce8.Add("西カロリン海盆", new Vector2(1723f, 1199f), new Vector2(149f, 223f), true);
      this.m_groups.Add(seaAreaOnce8);
      sea_area.sea_area_once seaAreaOnce9 = new sea_area.sea_area_once("南太平洋");
      seaAreaOnce9.Add("チリ海盆", new Vector2(3595f, 1797f), new Vector2(299f, 301f), true);
      seaAreaOnce9.Add("西オ\x30FCストラリア海盆", new Vector2(1498f, 1573f), new Vector2(224f, 224f), true);
      seaAreaOnce9.Add("パ\x30FCス海盆", new Vector2(1498f, 1798f), new Vector2(224f, 300f), true);
      seaAreaOnce9.Add("南オ\x30FCストラリア海盆", new Vector2(1723f, 1893f), new Vector2(299f, 205f), true);
      seaAreaOnce9.Add("アラフラ海", new Vector2(1723f, 1573f), new Vector2(373f, 211f), true);
      seaAreaOnce9.Add("", new Vector2(1873f, 1497f), new Vector2(223f, 76f), true);
      seaAreaOnce9.Add("東カロリン海盆", new Vector2(1873f, 1199f), new Vector2(223f, 297f), true);
      seaAreaOnce9.Add("メラネシア海盆", new Vector2(2097f, 1199f), new Vector2(223f, 298f), false);
      seaAreaOnce9.Add("コ\x30FCラル海", new Vector2(2097f, 1498f), new Vector2(299f, 299f), true);
      seaAreaOnce9.Add("タスマン海", new Vector2(2023f, 1798f), new Vector2(448f, 300f), true);
      seaAreaOnce9.Add("中央太平洋海盆西", new Vector2(2321f, 1199f), new Vector2(373f, 298f), false);
      seaAreaOnce9.Add("サモア海盆", new Vector2(2397f, 1498f), new Vector2(224f, 299f), true);
      seaAreaOnce9.Add("南太平洋海盆西", new Vector2(2472f, 1798f), new Vector2(298f, 300f), false);
      seaAreaOnce9.Add("中央太平洋海盆", new Vector2(2695f, 1199f), new Vector2(449f, 298f), false);
      seaAreaOnce9.Add("南太平洋海盆北", new Vector2(2622f, 1498f), new Vector2(523f, 299f), false);
      seaAreaOnce9.Add("南太平洋海盆", new Vector2(2771f, 1798f), new Vector2(374f, 300f), false);
      seaAreaOnce9.Add("南太平洋海盆東", new Vector2(3146f, 1498f), new Vector2(448f, 600f), false);
      this.m_groups.Add(seaAreaOnce9);
      sea_area.sea_area_once seaAreaOnce10 = new sea_area.sea_area_once("中南米西岸");
      seaAreaOnce10.Add("ペル\x30FC海盆", new Vector2(3595f, 1498f), new Vector2(260f, 298f), true);
      seaAreaOnce10.Add("グアヤキル湾", new Vector2(3595f, 1350f), new Vector2(126f, 147f), true);
      seaAreaOnce10.Add("パナマ湾", new Vector2(3595f, 1234f), new Vector2(136f, 114f), true);
      seaAreaOnce10.Add("中央太平洋海盆東", new Vector2(3145f, 1199f), new Vector2(299f, 298f), true);
      seaAreaOnce10.Add("ガラパゴス諸島沖", new Vector2(3445f, 1350f), new Vector2(149f, 147f), true);
      seaAreaOnce10.Add("テワンテペク湾", new Vector2(3445f, 1234f), new Vector2(149f, 114f), true);
      this.m_groups.Add(seaAreaOnce10);
      sea_area.sea_area_once seaAreaOnce11 = new sea_area.sea_area_once("東アジア");
      seaAreaOnce11.Add("東アジア西部", new Vector2(1393f, 912f), new Vector2(254f, 286f), true);
      seaAreaOnce11.Add("東アジア東部", new Vector2(1649f, 751f), new Vector2(373f, 447f), true);
      seaAreaOnce11.Add("北西太平洋海盆", new Vector2(2024f, 751f), new Vector2(297f, 447f), true);
      this.m_groups.Add(seaAreaOnce11);
      this.load(fname);
    }

    public void Dispose()
    {
      if (this.m_groups == null)
        return;
      foreach (sea_area.sea_area_once seaAreaOnce in this.m_groups)
        seaAreaOnce.Dispose();
      this.m_groups.Clear();
    }

    private void load(string fname)
    {
      if (!File.Exists(fname))
        return;
      try
      {
        using (StreamReader streamReader = new StreamReader(fname, Encoding.GetEncoding("Shift_JIS")))
        {
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            if (!(str == ""))
            {
              string[] strArray = str.Split(new char[1]
              {
                ','
              });
              if (strArray.Length >= 2)
                this.SetType(strArray[0], (sea_area.sea_area_once.sea_type) Useful.ToInt32(strArray[1], 0));
            }
          }
        }
      }
      catch
      {
      }
    }

    public void WriteSetting(string fname)
    {
      try
      {
        using (StreamWriter streamWriter = new StreamWriter(fname, false, Encoding.GetEncoding("Shift_JIS")))
        {
          foreach (sea_area.sea_area_once seaAreaOnce in this.m_groups)
          {
            string str = seaAreaOnce.name + "," + (object) seaAreaOnce.type;
            streamWriter.WriteLine(str);
          }
        }
      }
      catch
      {
      }
    }

    public void InitializeFromMaskInfo()
    {
      this.m_progress_max = 0;
      this.m_progress_current = 0;
      this.m_progress_info_str = "読み込み中...";
    }

    public bool CreateFromMask(string fname)
    {
      try
      {
        this.InitializeFromMaskInfo();
        Bitmap bitmap = new Bitmap(fname);
        Size size = new Size(bitmap.Width, bitmap.Height);
        BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppRgb565);
        IntPtr scan0 = bitmapdata.Scan0;
        int length = bitmapdata.Height * bitmapdata.Stride;
        int stride = bitmapdata.Stride;
        byte[] image = new byte[length];
        Marshal.Copy(scan0, image, 0, length);
        bitmap.UnlockBits(bitmapdata);
        bitmap.Dispose();
        this.m_progress_max = this.m_groups.Count;
        this.m_progress_current = 0;
        this.m_progress_info_str = "";
        foreach (sea_area.sea_area_once seaAreaOnce in this.m_groups)
        {
          this.m_progress_info_str = "テクスチャ転送中... " + seaAreaOnce.name;
          seaAreaOnce.CreateFromMask(this.m_lib.device, ref image, size, stride);
          ++this.m_progress_current;
        }
        this.m_progress_info_str = "完了";
        this.m_is_loaded_mask = true;
        GC.Collect();
      }
      catch
      {
        return false;
      }
      return true;
    }

    public void Update()
    {
      this.m_alpha = 47 + (int) (Math.Sin((double) Useful.ToRadian(this.m_angle)) * 12.0);
      this.m_angle += 2f;
      if ((double) this.m_angle >= 360.0)
        this.m_angle -= 360f;
      this.m_alpha2 = 47 + (int) (Math.Sin((double) Useful.ToRadian(this.m_angle2)) * 12.0);
      this.m_angle2 += 4f;
      if ((double) this.m_angle2 >= 360.0)
        this.m_angle2 -= 360f;
      this.m_angle = 30f;
      this.m_angle2 = 30f;
    }

    public void Draw()
    {
      this.m_lib.device.device.RenderState.ZBufferEnable = false;
      this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_proc), 0.0f);
      this.m_lib.device.device.RenderState.ZBufferEnable = true;
    }

    private void draw_proc(Vector2 offset, LoopXImage image)
    {
      foreach (sea_area.sea_area_once seaAreaOnce in this.m_groups)
        seaAreaOnce.Draw(offset, image, this.m_alpha, this.m_alpha2, this.m_color_type);
    }

    public bool SetType(string name, sea_area.sea_area_once.sea_type type)
    {
      sea_area.sea_area_once seaAreaOnce = this.find(name);
      if (seaAreaOnce == null || seaAreaOnce.name == null)
        return false;
      if (type < sea_area.sea_area_once.sea_type.normal)
        type = sea_area.sea_area_once.sea_type.normal;
      if (type > sea_area.sea_area_once.sea_type.lawless)
        type = sea_area.sea_area_once.sea_type.normal;
      if (seaAreaOnce.type != type)
      {
        seaAreaOnce.type = type;
        this.m_lib.setting.req_update_map.Request();
      }
      return true;
    }

    public void ResetSeaType()
    {
      foreach (sea_area.sea_area_once seaAreaOnce in this.m_groups)
        seaAreaOnce.type = sea_area.sea_area_once.sea_type.normal;
    }

    public string Find(Vector2 pos)
    {
      return this.Find(transform.ToPoint(pos));
    }

    public string Find(Point pos)
    {
      foreach (sea_area.sea_area_once seaAreaOnce in this.m_groups)
      {
        if (seaAreaOnce.HitTest(pos))
          return seaAreaOnce.name;
      }
      return (string) null;
    }

    private sea_area.sea_area_once find(string name)
    {
      if (name == null)
        return (sea_area.sea_area_once) null;
      foreach (sea_area.sea_area_once seaAreaOnce in this.m_groups)
      {
        if (seaAreaOnce.name == name)
          return seaAreaOnce;
      }
      return (sea_area.sea_area_once) null;
    }

    public static List<sea_area_once_from_dd> AnalizeFromDD(string str)
    {
      List<sea_area_once_from_dd> list = new List<sea_area_once_from_dd>();
      string str1 = str;
      string[] separator = new string[2]
      {
        "\r\n",
        "\n"
      };
      int num = 1;
      foreach (string line in str1.Split(separator, (StringSplitOptions) num))
      {
        sea_area_once_from_dd seaAreaOnceFromDd = new sea_area_once_from_dd();
        if (seaAreaOnceFromDd.Analize(line))
          list.Add(seaAreaOnceFromDd);
      }
      return list;
    }

    public void UpdateFromDD(List<sea_area_once_from_dd> list, bool is_clear)
    {
      if (list == null)
        return;
      if (is_clear)
        this.ResetSeaType();
      for (int index = list.Count - 1; index >= 0; --index)
        this.SetType(list[index].name, list[index]._sea_type);
    }

    public class sea_area_once : IDisposable
    {
      private List<sea_area.sea_area_once.data> m_list;
      private string m_name;
      private sea_area.sea_area_once.sea_type m_type;

      public string name
      {
        get
        {
          return this.m_name;
        }
      }

      public sea_area.sea_area_once.sea_type type
      {
        get
        {
          return this.m_type;
        }
        set
        {
          this.m_type = value;
        }
      }

      public string type_str
      {
        get
        {
          return sea_area.sea_area_once.ToString(this.m_type);
        }
      }

      public sea_area_once(string name)
      {
        this.m_name = name;
        this.m_type = sea_area.sea_area_once.sea_type.normal;
        this.m_list = new List<sea_area.sea_area_once.data>();
      }

      public void Dispose()
      {
        if (this.m_list == null)
          return;
        foreach (TextureUnit textureUnit in this.m_list)
          textureUnit.Dispose();
        this.m_list.Clear();
      }

      public void Add(string name, Vector2 pos, Vector2 size, bool is_create_from_mask)
      {
        this.m_list.Add(new sea_area.sea_area_once.data(name, pos, size, is_create_from_mask));
      }

      public void CreateFromMask(d3d_device device, ref byte[] image, Size size, int stride)
      {
        foreach (sea_area.sea_area_once.data data in this.m_list)
          data.CreateFromMask(device, ref image, size, stride);
      }

      public string GetToolTipString()
      {
        string str = "";
        foreach (sea_area.sea_area_once.data data in this.m_list)
        {
          if (data.name != "")
          {
            if (str != "")
              str = str + "\n";
            str = str + data.name;
          }
        }
        return str;
      }

      public void Draw(Vector2 offset, LoopXImage image, int alpha, int alpha2, sea_area.color_type type)
      {
        if (this.m_type == sea_area.sea_area_once.sea_type.normal)
          return;
        int color = this.m_type != sea_area.sea_area_once.sea_type.safty ? (type != sea_area.color_type.type1 ? Color.FromArgb(alpha2, 200, 0, 0).ToArgb() : Color.FromArgb(alpha2, 200, 0, 0).ToArgb()) : (type != sea_area.color_type.type1 ? Color.FromArgb(alpha, 0, 64, 200).ToArgb() : Color.FromArgb(alpha, 0, 128, 220).ToArgb());
        foreach (sea_area.sea_area_once.data data in this.m_list)
          data.Draw(offset, image, color);
      }

      public bool HitTest(Point pos)
      {
        foreach (sea_area.sea_area_once.data data in this.m_list)
        {
          if (data.HitTest(pos))
            return true;
        }
        return false;
      }

      public static string ToString(sea_area.sea_area_once.sea_type t)
      {
        switch (t)
        {
          case sea_area.sea_area_once.sea_type.normal:
            return "危険海域";
          case sea_area.sea_area_once.sea_type.safty:
            return "安全海域";
          case sea_area.sea_area_once.sea_type.lawless:
            return "無法海域";
          default:
            return "unknown";
        }
      }

      private class data : TextureUnit
      {
        private string m_name;
        private Vector2 m_pos;
        private Vector2 m_size;
        private bool m_is_create_from_mask;
        private hittest m_hittest;

        public string name
        {
          get
          {
            return this.m_name;
          }
        }

        public Vector2 position
        {
          get
          {
            return this.m_pos;
          }
        }

        public Vector2 size
        {
          get
          {
            return this.m_size;
          }
        }

        public bool is_create_from_mask
        {
          get
          {
            return this.m_is_create_from_mask;
          }
        }

        public data(string name, Vector2 pos, Vector2 size, bool is_create_from_mask)
        {
          this.m_name = name;
          this.m_pos = pos;
          this.m_size = size;
          this.m_is_create_from_mask = is_create_from_mask;
          this.m_hittest = new hittest(new Rectangle(0, 0, (int) size.X, (int) size.Y), transform.ToPoint(pos));
        }

        public void CreateFromMask(d3d_device device, ref byte[] image, Size size, int stride)
        {
          if (!this.m_is_create_from_mask)
            return;
          Rectangle rect = new Rectangle((int) this.m_pos.X, (int) this.m_pos.Y, (int) this.m_size.X & -2, (int) this.m_size.Y & -2);
          base.CreateFromMask(device, ref image, size, stride, rect);
        }

        public void Draw(Vector2 offset, LoopXImage image, int color)
        {
          if (this.IsCreate)
          {
            base.Draw(new Vector3(offset.X, offset.Y, 0.79f), image.ImageScale, color);
          }
          else
          {
            Vector2 global_pos = this.m_pos;
            Vector2 vector2 = image.GlobalPos2LocalPos(global_pos, offset);
            Vector2 size = this.m_size;
            --size.X;
            --size.Y;
            size *= image.ImageScale;
            if ((double) vector2.X + (double) size.X < 0.0 || (double) vector2.Y + (double) size.Y < 0.0)
              return;
            Vector2 clientSize = image.Device.client_size;
            if ((double) vector2.X >= (double) clientSize.X || (double) vector2.Y >= (double) clientSize.Y)
              return;
            image.Device.DrawFillRect(new Vector3(vector2.X, vector2.Y, 0.79f), size, color);
          }
        }

        public bool HitTest(Point pos)
        {
          return this.m_hittest.HitTest(pos);
        }
      }

      public enum sea_type
      {
        normal,
        safty,
        lawless,
      }
    }

    public enum name
    {
      carib,
      west_africa,
      south_atlantic,
      east_africa,
      red_sea,
      indian,
      east_latin_america,
      southeast_asia,
      south_pacific,
      west_latin_america,
      east_asia,
      max,
    }

    public enum color_type
    {
      type1,
      type2,
    }
  }
}
