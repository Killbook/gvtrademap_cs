// Type: gvtrademap_cs.item_window
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Utility;

namespace gvtrademap_cs
{
  public class item_window : d3d_windows.window
  {
    private const int WINDOW_POS_X = 3;
    private const int WINDOW_POS_Y = 3;
    private const float WINDOW_POS_Z = 0.2f;
    private const int WINDOW_SIZE_X = 250;
    private const int WINDOW_SIZE_Y = 200;
    private const int TABS_STEP_X = 20;
    private const int ICONS_STEP_X = 20;
    private const int STEP_Y0 = 18;
    private const int STEP_Y = 20;
    private gvt_lib m_lib;
    private GvoDatabase m_db;
    private spot m_spot;
    private gvtrademap_cs_form m_form;
    private GvoWorldInfo.Info m_info;
    private int m_tab_index;
    private hittest_list m_hittest_list;
    private Vector2 m_window_size;
    private TextBox m_memo_text_box;
    private ListView m_list_view;
    private bool m_is_1st_draw;

    public GvoWorldInfo.Info info
    {
      get
      {
        return this.m_info;
      }
      set
      {
        this.set_info(value, true);
      }
    }

    public item_window(gvt_lib lib, GvoDatabase db, spot _spot, TextBox memo_text_box, ListView list_view, gvtrademap_cs_form form)
      : base(lib.device, new Vector2(3f, 3f), new Vector2(250f, 200f), 0.2f)
    {
      this.title = "アイテムウインドウ";
      this.m_window_size = new Vector2(0.0f, 0.0f);
      this.m_memo_text_box = memo_text_box;
      this.m_list_view = list_view;
      this.m_form = form;
      this.m_lib = lib;
      this.m_db = db;
      this.m_spot = _spot;
      this.m_tab_index = 0;
      this.m_info = (GvoWorldInfo.Info) null;
      this.m_is_1st_draw = true;
      this.m_hittest_list = new hittest_list();
      this.m_hittest_list.Add(new hittest());
      this.m_hittest_list.Add(new hittest());
      this.m_hittest_list.Add(new hittest());
      this.m_hittest_list.Add(new hittest());
      this.m_hittest_list.Add(new hittest());
      this.m_hittest_list.Add(new hittest());
      this.m_hittest_list.Add(new hittest());
      this.m_hittest_list.Add(new hittest());
    }

    private void set_info(GvoWorldInfo.Info _info, bool with_spot_reset)
    {
      if (_info == null)
        return;
      if (with_spot_reset)
      {
        this.m_spot.SetSpot(spot.type.none, "");
        this.m_form.UpdateSpotList();
      }
      if (this.m_info == _info)
        return;
      this.UpdateMemo();
      this.m_info = _info;
      this.m_memo_text_box.Text = this.m_info.Memo;
      this.update_item_list();
    }

    protected override void OnMouseDownClient(Point pos, MouseButtons button)
    {
      if ((button & MouseButtons.Left) != MouseButtons.None)
      {
        this._window_on_mouse_l_down(pos);
      }
      else
      {
        if ((button & MouseButtons.Right) == MouseButtons.None)
          return;
        this._window_on_mouse_r_down(pos);
      }
    }

    private void _window_on_mouse_l_down(Point pos)
    {
      if (this.m_hittest_list.HitTest_Index(pos) != 3)
        return;
      this.on_mouse_l_click_item_tabs(pos);
    }

    private void _window_on_mouse_r_down(Point pos)
    {
      if (this.m_hittest_list.HitTest_Index(pos) != 3)
        return;
      this.on_mouse_l_click_item_tabs(pos);
    }

    protected override void OnMouseClikClient(Point pos, MouseButtons button)
    {
      if ((button & MouseButtons.Left) != MouseButtons.None)
      {
        this._window_on_mouse_l_click(pos);
      }
      else
      {
        if ((button & MouseButtons.Right) == MouseButtons.None)
          return;
        this._window_on_mouse_r_click(pos);
      }
    }

    private void _window_on_mouse_l_click(Point pos)
    {
      switch (this.m_hittest_list.HitTest_Index(pos))
      {
        case 1:
          this.on_mouse_l_click_country(pos);
          break;
        case 2:
          this.on_mouse_l_click_icons(pos);
          break;
        case 4:
          this.on_mouse_l_click_web(pos);
          break;
        case 5:
          this.on_mouse_l_click_country_name(pos);
          break;
        case 6:
          this.on_mouse_l_click_lang(pos, 0);
          break;
        case 7:
          this.on_mouse_l_click_lang(pos, 1);
          break;
      }
    }

    private void _window_on_mouse_r_click(Point pos)
    {
      switch (this.m_hittest_list.HitTest_Index(pos))
      {
        case 1:
          this.on_mouse_r_click_country(pos);
          break;
        case 3:
          this.on_mouse_r_click_item_tabs(pos);
          break;
        case 5:
          this.on_mouse_r_click_country_name(pos);
          break;
      }
    }

    protected override void OnMouseWheelClient(Point pos, int delta)
    {
      if (this.m_hittest_list.HitTest_Index(pos) != 3)
        return;
      this.on_mouse_wheel_item_tabs(pos, delta);
    }

    protected override void OnUpdateClient()
    {
      if (this.window_mode == d3d_windows.window.mode.small)
      {
        this.EnableMemoWindow(false);
        this.EnableItemWindow(false);
      }
      else
      {
        d3d_windows.window window = this.FindWindow("設定ウインドウ");
        if (window != null)
          this.size = new Vector2(window.normal_size.X, this.screen_size.Y - 8f);
        else
          this.size = new Vector2(250f, this.screen_size.Y - 8f);
        this.update_rects();
        if (this.size != this.m_window_size)
        {
          Rectangle rectangle = this.m_hittest_list[0].CalcRect();
          this.m_memo_text_box.Location = new Point(rectangle.Left, rectangle.Top);
          this.m_memo_text_box.Size = new Size(rectangle.Width, rectangle.Height + 1);
          this.m_list_view.Location = this.m_memo_text_box.Location;
          this.m_list_view.Size = this.m_memo_text_box.Size;
          this.ajust_item_columns_width();
        }
        this.update_memo_window();
        this.m_window_size = this.size;
      }
    }

    private void update_rects()
    {
      Point point = transform.ToPoint(this.client_pos);
      foreach (hittest hittest in this.m_hittest_list)
        hittest.position = point;
      int y1 = 0;
      this.m_hittest_list[1].rect = new Rectangle(0, y1, 24, 16);
      int y2 = y1 + 18;
      this.m_hittest_list[2].rect = new Rectangle(2, y2, 80, 16);
      this.m_hittest_list[4].rect = new Rectangle(92, y2, 17, 17);
      this.m_hittest_list[5].rect = new Rectangle(28, 2, 130, 16);
      this.m_hittest_list[6].rect = new Rectangle((int) this.client_size.X - 130, 2, 130, 16);
      this.m_hittest_list[7].rect = new Rectangle((int) this.client_size.X - 130, y2 + 1, 130, 16);
      int y3 = y2 + 18;
      int height = (int) this.client_size.Y - (y3 + 20) - (int) this.pos.Y;
      d3d_windows.window window = this.FindWindow("設定ウインドウ");
      if (window != null)
        height -= (int) window.size.Y;
      this.m_hittest_list[0].rect = new Rectangle(0, y3, (int) this.client_size.X, height);
      int y4 = y3 + (height + 2);
      this.m_hittest_list[3].rect = new Rectangle(2, y4, 240, 16);
      this.client_size = new Vector2(this.client_size.X, (float) (y4 + 20));
    }

    protected override void OnDrawClient()
    {
      if (this.m_is_1st_draw)
      {
        this.update_item_list();
        this.m_is_1st_draw = false;
      }
      this.draw_current_tab();
      this.draw_country_name();
      this.draw_lang();
      this.draw_current_back();
      this.device.sprites.BeginDrawSprites(this.m_lib.icons.texture);
      this.draw_country();
      this.draw_icons();
      this.draw_web();
      this.draw_tabs();
      this.device.sprites.EndDrawSprites();
    }

    private void draw_current_back()
    {
      Point clientMousePosition = this.device.GetClientMousePosition();
      switch (this.m_hittest_list.HitTest_Index(clientMousePosition))
      {
        case 2:
          this.draw_current_back_icons(clientMousePosition);
          break;
        case 3:
          this.draw_current_back_tabs(clientMousePosition);
          break;
        case 4:
          this.draw_current_back_web(clientMousePosition);
          break;
      }
    }

    private void draw_current_back_icons(Point pos)
    {
      Rectangle rectangle = this.m_hittest_list[2].CalcRect();
      pos.X -= rectangle.X;
      pos.X /= 20;
      Vector3 pos1 = new Vector3((float) (rectangle.X + 20 * pos.X), (float) rectangle.Y, this.z);
      --pos1.X;
      --pos1.Y;
      this.DrawCurrentButtonBack(pos1, new Vector2(18f, 18f));
    }

    private void draw_current_back_web(Point pos)
    {
      if (this.info == null || !this.info.IsUrl)
        return;
      Rectangle rectangle = this.m_hittest_list[4].CalcRect();
      Vector3 pos1 = new Vector3((float) rectangle.X, (float) rectangle.Y, this.z);
      --pos1.X;
      --pos1.Y;
      this.DrawCurrentButtonBack(pos1, new Vector2(18f, 18f));
    }

    private void draw_current_back_tabs(Point pos)
    {
      Rectangle rectangle = this.m_hittest_list[3].CalcRect();
      pos.X -= rectangle.X;
      pos.X /= 20;
      Vector3 pos1 = new Vector3((float) (rectangle.X + 20 * pos.X), (float) rectangle.Y, this.z);
      pos1.X -= 2f;
      pos1.Y -= 2f;
      this.DrawCurrentButtonBack(pos1, new Vector2(20f, 21f));
    }

    private void draw_country_name()
    {
      if (this.info == null)
        return;
      Rectangle rectangle = this.m_hittest_list[5].CalcRect();
      Vector3 vector3 = new Vector3((float) rectangle.X, (float) rectangle.Y, this.z);
      this.device.DrawText(font_type.normal, this.info.Name, (int) vector3.X, (int) vector3.Y, Color.Black);
    }

    private void draw_lang()
    {
      if (this.info == null)
        return;
      Rectangle rectangle = this.m_hittest_list[6].CalcRect();
      Vector3 vector3 = new Vector3((float) rectangle.X, (float) rectangle.Y, this.z);
      if (this.info.InfoType == GvoWorldInfo.InfoType.City)
      {
        if (this.info.Lang1 != "")
          this.device.DrawTextR(font_type.normal, this.info.Lang1, (int) vector3.X + rectangle.Width, (int) vector3.Y, Color.Black);
        if (!(this.info.Lang2 != ""))
          return;
        this.device.DrawTextR(font_type.normal, this.info.Lang2, (int) vector3.X + rectangle.Width, (int) vector3.Y + 17, Color.Black);
      }
      else
      {
        if (this.info.InfoType != GvoWorldInfo.InfoType.Sea || this.info.SeaInfo == null)
          return;
        this.device.DrawTextR(font_type.normal, this.info.SeaInfo.SpeedUpRateString, (int) vector3.X + rectangle.Width, (int) vector3.Y, Color.Black);
      }
    }

    private void draw_web()
    {
      if (this.info == null || !this.info.IsUrl)
        return;
      Rectangle rectangle = this.m_hittest_list[4].CalcRect();
      this.device.sprites.AddDrawSpritesNC(new Vector3((float) rectangle.X, (float) rectangle.Y, this.z), this.m_lib.icons.GetIcon(this.info.InfoType == GvoWorldInfo.InfoType.City ? icons.icon_index.web : icons.icon_index.map_icon));
    }

    private void draw_current_tab()
    {
      Rectangle rectangle = this.m_hittest_list[0].CalcRect();
      Vector3 vector3 = new Vector3((float) rectangle.X, (float) rectangle.Y, this.z);
      Vector2 vector2 = new Vector2((float) rectangle.Width, (float) rectangle.Height);
      Vector3 pos = vector3;
      pos.Y += vector2.Y;
      pos.X += (float) (20 * this.m_tab_index);
      this.device.DrawFillRect(pos, new Vector2(20f, 20f), Color.FromArgb(128, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue).ToArgb());
      this.device.DrawLineRect(pos, new Vector2(20f, 21f), Color.Black.ToArgb());
    }

    private int get_item_list_count()
    {
      return this.get_item_list_count(this.m_tab_index);
    }

    private int get_item_list_count(int index)
    {
      if (this.info == null)
        return 0;
      if (index == 11)
        return this.info.GetMemoLines();
      else
        return this.info.GetCount((GvoWorldInfo.Info.GroupIndex) index);
    }

    private void draw_country()
    {
      if (this.info == null)
        return;
      Rectangle rectangle = this.m_hittest_list[1].CalcRect();
      Vector3 pos = new Vector3((float) rectangle.X, (float) rectangle.Y, this.z);
      icons.icon_index index;
      switch (this.info.InfoType)
      {
        case GvoWorldInfo.InfoType.Sea:
          index = icons.icon_index.country_2;
          break;
        case GvoWorldInfo.InfoType.Shore:
        case GvoWorldInfo.InfoType.OutsideCity:
          index = icons.icon_index.country_3;
          break;
        case GvoWorldInfo.InfoType.Shore2:
          index = icons.icon_index.country_1;
          break;
        case GvoWorldInfo.InfoType.PF:
          index = icons.icon_index.country_0;
          break;
        default:
          index = (icons.icon_index) (16 + this.info.MyCountry);
          break;
      }
      this.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(index));
    }

    private void draw_icons()
    {
      Rectangle rectangle = this.m_hittest_list[2].CalcRect();
      Vector3 pos = new Vector3((float) rectangle.X, (float) rectangle.Y, this.z);
      for (int index = 0; index < 4; ++index)
      {
        icons.icon_index iconIndex = icons.icon_index.tab2_gray_0;
        if (this.info != null && (this.info.Sakaba & 1 << index) != 0)
          iconIndex = icons.icon_index.tab2_0;
        this.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(iconIndex + index));
        pos.X += 20f;
      }
    }

    private void draw_tabs()
    {
      Rectangle rectangle = this.m_hittest_list[3].CalcRect();
      Vector3 pos = new Vector3((float) rectangle.X, (float) rectangle.Y, this.z);
      for (int index = 0; index < 12; ++index)
      {
        icons.icon_index iconIndex = icons.icon_index.tab_gray_0;
        if (this.info != null && this.get_item_list_count(index) > 0)
          iconIndex = icons.icon_index.tab_0;
        this.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(iconIndex + index));
        pos.X += 20f;
      }
    }

    private void on_mouse_l_click_item_tabs(Point pos)
    {
      Rectangle rectangle = this.m_hittest_list[3].CalcRect();
      pos.X -= rectangle.X;
      this.ajust_tab_index(pos.X / 20);
    }

    private void on_mouse_r_click_item_tabs(Point pos)
    {
      this.on_mouse_l_click_item_tabs(pos);
      this.m_spot.SetSpot((spot.type) (8 + this.m_tab_index), "");
      this.m_form.UpdateSpotList();
    }

    private void on_mouse_wheel_item_tabs(Point pos, int delta)
    {
      if (delta > 0)
        this.ajust_tab_index(this.m_tab_index - 1);
      else
        this.ajust_tab_index(this.m_tab_index + 1);
    }

    private void ajust_tab_index(int index)
    {
      if (index < 0)
        index = 11;
      if (index > 11)
        index = 0;
      this.m_tab_index = index;
      this.update_item_list();
    }

    public void SpotItem(GvoDatabase.Find select)
    {
      if (select == null)
      {
        this.m_spot.SetSpot(spot.type.none, "");
        this.m_form.UpdateSpotList();
      }
      else
      {
        if (select.Type == GvoDatabase.Find.FindType.Database)
          return;
        GvoWorldInfo.Info info = this.m_db.World.FindInfo(select.InfoName);
        if (info != null)
        {
          this.set_info(info, false);
          this.req_centering_info();
        }
        switch (select.Type)
        {
          case GvoDatabase.Find.FindType.Data:
          case GvoDatabase.Find.FindType.DataPrice:
            this.find_item_for_selected(select.Data.Name);
            this.m_spot.SetSpot(spot.type.has_item, select.Data.Name);
            break;
          case GvoDatabase.Find.FindType.InfoName:
            this.m_spot.SetSpot(spot.type.city_name, select.InfoName);
            break;
          case GvoDatabase.Find.FindType.Lang:
            this.m_spot.SetSpot(spot.type.language, select.Lang);
            break;
          case GvoDatabase.Find.FindType.CulturalSphere:
            this.m_spot.SetSpot(spot.type.cultural_sphere, select.InfoName);
            break;
        }
        this.m_form.UpdateSpotList();
      }
    }

    public void SpotItemChanged(spot.spot_once select)
    {
      if (select == null || select.info == null)
        return;
      GvoWorldInfo.Info info = select.info;
      if (info != null)
      {
        this.set_info(info, false);
        this.req_centering_info();
      }
      this.find_item_for_selected(select.name);
    }

    private void find_item_for_selected(string find_item)
    {
      if (this.info == null)
        return;
      for (GvoWorldInfo.Info.GroupIndex index = GvoWorldInfo.Info.GroupIndex._0; index < GvoWorldInfo.Info.GroupIndex.max; ++index)
      {
        for (int data_index = 0; data_index < this.info.GetCount(index); ++data_index)
        {
          GvoWorldInfo.Info.Group.Data data = this.info.GetData(index, data_index);
          if (data != null && !(data.Name != find_item))
          {
            this.ajust_tab_index((int) index);
            if (this.m_list_view.Items.Count < data_index)
              return;
            this.m_list_view.Items[data_index].Selected = true;
            this.m_list_view.Items[data_index].EnsureVisible();
            this.m_list_view.Items[data_index].Focused = true;
            return;
          }
        }
      }
    }

    private void req_centering_info()
    {
      if (this.info == null)
        return;
      this.m_lib.setting.centering_gpos = transform.map_pos2_game_pos(this.info.position, this.m_lib.loop_image);
      this.m_lib.setting.req_centering_gpos.Request();
    }

    private void on_mouse_l_click_country(Point pos)
    {
      if (this.info == null || this.info.InfoType != GvoWorldInfo.InfoType.City)
        return;
      this.m_spot.SetSpot(spot.type.country_flags, "");
      this.m_form.UpdateSpotList();
    }

    private void on_mouse_r_click_country(Point pos)
    {
      if (this.info == null || this.info.InfoType != GvoWorldInfo.InfoType.City || this.info.AllianceType != GvoWorldInfo.AllianceType.Alliance)
        return;
      this.m_form.ShowChangeDomainsMenuStrip(pos);
    }

    private void on_mouse_r_click_country_name(Point pos)
    {
      if (this.info == null || this.info.InfoType != GvoWorldInfo.InfoType.City || this.info.CulturalSphere == GvoWorldInfo.CulturalSphere.Unknown)
        return;
      this.SpotItem(new GvoDatabase.Find(this.info.CulturalSphere, ""));
    }

    private void on_mouse_l_click_icons(Point pos)
    {
      Rectangle rectangle = this.m_hittest_list[2].CalcRect();
      pos.X -= rectangle.X;
      pos.X /= 20;
      this.m_spot.SetSpot((spot.type) (2 + pos.X), "");
      this.m_form.UpdateSpotList();
    }

    private void on_mouse_l_click_country_name(Point pos)
    {
      if (this.info == null)
        return;
      this.req_centering_info();
      this.m_spot.SetSpot(spot.type.city_name, this.info.Name);
      this.m_form.UpdateSpotList();
    }

    private void on_mouse_l_click_lang(Point pos, int index)
    {
      if (this.info == null)
        return;
      string find_str = index == 0 ? this.info.Lang1 : this.info.Lang2;
      if (find_str == "")
        return;
      this.m_spot.SetSpot(spot.type.language, find_str);
      this.m_form.UpdateSpotList();
    }

    private void on_mouse_l_click_web(Point pos)
    {
      if (this.info == null || !this.info.IsUrl)
        return;
      if (this.info.UrlIndex != -1)
        Process.Start("http://" + (this.info.InfoType != GvoWorldInfo.InfoType.City ? "www2.atwiki.jp/harrington/pages/" + this.info.UrlIndex.ToString() + ".html" : "dol.egret.jp/gtf/?page=city&id=" + this.info.UrlIndex.ToString()));
      else
        Process.Start("http://wiki.livedoor.jp/kristall/d/" + Useful.UrlEncodeEUCJP(this.info.Url));
    }

    public void ChangeTab(bool is_next)
    {
      if (is_next)
        this.ajust_tab_index(this.m_tab_index + 1);
      else
        this.ajust_tab_index(this.m_tab_index - 1);
      this.m_lib.device.SetMustDrawFlag();
    }

    protected override string OnToolTipStringClient(Point pos)
    {
      switch (this.m_hittest_list.HitTest_Index(pos))
      {
        case 2:
          return this.get_tooltip_string_icons(pos);
        case 3:
          return this.get_tooltip_string_tabs(pos);
        default:
          if (this.info == null)
            return (string) null;
          switch (this.m_hittest_list.HitTest_Index(pos))
          {
            case 1:
              switch (this.info.InfoType)
              {
                case GvoWorldInfo.InfoType.City:
                  switch (this.info.AllianceType)
                  {
                    case GvoWorldInfo.AllianceType.Unknown:
                    case GvoWorldInfo.AllianceType.Piratical:
                      return this.info.AllianceTypeStr + "\n左クリックでスポット表示";
                    case GvoWorldInfo.AllianceType.Alliance:
                      return this.info.AllianceTypeStr + " " + this.info.CountryStr + "\n左クリックでスポット表示\n右クリックで同盟国変更";
                    case GvoWorldInfo.AllianceType.Capital:
                    case GvoWorldInfo.AllianceType.Territory:
                      return this.info.CountryStr + " " + this.info.AllianceTypeStr + "\n左クリックでスポット表示";
                  }
                case GvoWorldInfo.InfoType.Sea:
                case GvoWorldInfo.InfoType.Shore:
                case GvoWorldInfo.InfoType.Shore2:
                case GvoWorldInfo.InfoType.OutsideCity:
                case GvoWorldInfo.InfoType.PF:
                  return this.info.InfoTypeStr;
              }
            case 4:
              return this.info.GetToolTipString_HP();
            case 5:
              string str = this.info.TooltipString + "\n左クリックで中心に移動";
              if (this.info.InfoType == GvoWorldInfo.InfoType.City)
                str = str + "\n右クリックで属する文化圏をスポット表示";
              return str;
            case 6:
            case 7:
              if (this.info.Lang1 != "" || this.info.Lang2 != "")
                return "使用言語\n左クリックでスポット表示";
              else
                break;
          }
          return (string) null;
      }
    }

    private string get_tooltip_string_item_list(GvoWorldInfo.Info.Group.Data d)
    {
      if (d == null)
        return (string) null;
      else
        return d.TooltipString + "\n(右クリックでメニュ\x30FCを開きます)";
    }

    private string get_tooltip_string_icons(Point pos)
    {
      Rectangle rectangle = this.m_hittest_list[2].CalcRect();
      pos.X -= rectangle.X;
      pos.X /= 20;
      switch (pos.X)
      {
        case 0:
          return "請負人/酒場娘/販売員\n左クリックでスポット表示";
        case 1:
          return "書庫\n左クリックでスポット表示";
        case 2:
          return "翻訳家\n左クリックでスポット表示";
        case 3:
          return "豪商\n左クリックでスポット表示";
        default:
          return (string) null;
      }
    }

    private string get_tooltip_string_tabs(Point pos)
    {
      Rectangle rectangle = this.m_hittest_list[3].CalcRect();
      pos.X -= rectangle.X;
      pos.X /= 20;
      if (pos.X < 0)
        return (string) null;
      if (pos.X > 11)
        return (string) null;
      else
        return GvoWorldInfo.Info.GetGroupName((GvoWorldInfo.Info.GroupIndex) pos.X) + "\n右クリックでスポット表示";
    }

    private void update_memo_window()
    {
      if (this.m_tab_index == 11)
      {
        this.EnableMemoWindow(true);
        this.EnableItemWindow(false);
      }
      else
      {
        this.EnableMemoWindow(false);
        this.EnableItemWindow(true);
      }
    }

    public void EnableMemoWindow(bool is_enable)
    {
      this.enable_ctrl((Control) this.m_memo_text_box, is_enable);
    }

    public void EnableItemWindow(bool is_enable)
    {
      this.enable_ctrl((Control) this.m_list_view, is_enable);
    }

    private void enable_ctrl(Control ctrl, bool is_enable)
    {
      if (is_enable)
      {
        if (!ctrl.Visible)
          ctrl.Visible = true;
        if (this.m_info == null)
        {
          if (!ctrl.Enabled)
            return;
          ctrl.Enabled = false;
        }
        else
        {
          if (ctrl.Enabled)
            return;
          ctrl.Enabled = true;
        }
      }
      else
      {
        if (!ctrl.Visible)
          return;
        ctrl.Visible = false;
        this.device.form.Focus();
      }
    }

    public void UpdateMemo()
    {
      if (this.info == null)
        return;
      this.info.Memo = this.m_memo_text_box.Text;
    }

    private void update_item_list()
    {
      if (this.info == null)
      {
        this.m_list_view.Clear();
      }
      else
      {
        this.m_list_view.BeginUpdate();
        this.m_list_view.Clear();
        switch (this.m_tab_index)
        {
          case 0:
            this.m_list_view.Columns.Add("名称", 80);
            this.m_list_view.Columns.Add("★", 25, HorizontalAlignment.Center);
            this.m_list_view.Columns.Add("種類", 52, HorizontalAlignment.Center);
            if (this.info.InfoType == GvoWorldInfo.InfoType.City)
            {
              this.m_list_view.Columns.Add("値段", 55, HorizontalAlignment.Right);
              break;
            }
            else
            {
              this.m_list_view.Columns.Add("スキル", 60, HorizontalAlignment.Center);
              break;
            }
          case 3:
            this.m_list_view.Columns.Add("名称", 140);
            this.m_list_view.Columns.Add("人物", 120);
            break;
          case 10:
            this.m_list_view.Columns.Add("名称", 160);
            this.m_list_view.Columns.Add("人物", 60);
            break;
          default:
            this.m_list_view.Columns.Add("名称", 160);
            this.m_list_view.Columns.Add("値段", 80, HorizontalAlignment.Right);
            break;
        }
        int count = this.info.GetCount((GvoWorldInfo.Info.GroupIndex) this.m_tab_index);
        GvoWorldInfo.Info.Group group = this.info.GetGroup((GvoWorldInfo.Info.GroupIndex) this.m_tab_index);
        Font font = this.m_list_view.Font;
        for (int index = 0; index < count; ++index)
        {
          GvoWorldInfo.Info.Group.Data data = group.GetData(index);
          ListViewItem listViewItem = new ListViewItem(data.Name, 0);
          listViewItem.UseItemStyleForSubItems = false;
          listViewItem.ForeColor = data.Color;
          listViewItem.Tag = (object) data;
          string tooltipStringItemList = this.get_tooltip_string_item_list(data);
          if (tooltipStringItemList != null)
            listViewItem.ToolTipText = tooltipStringItemList;
          if (this.m_tab_index == 0)
          {
            listViewItem.SubItems.Add(data.IsBonusItem ? "★" : "", Color.Tomato, listViewItem.BackColor, font);
            listViewItem.SubItems.Add(data.Type, data.CategolyColor, listViewItem.BackColor, font);
          }
          listViewItem.SubItems.Add(data.Price, data.PriceColor, listViewItem.BackColor, font);
          this.m_list_view.Items.Add(listViewItem);
        }
        this.m_list_view.EndUpdate();
        this.ajust_item_columns_width();
      }
    }

    private void ajust_item_columns_width()
    {
      if (this.m_list_view.Columns.Count < 1 || this.m_tab_index == 3)
        return;
      Size clientSize = this.m_list_view.ClientSize;
      int num1 = 0;
      for (int index = 1; index < this.m_list_view.Columns.Count; ++index)
        num1 += this.m_list_view.Columns[index].Width;
      int num2 = clientSize.Width - num1;
      if (num2 <= 0)
        num2 = 20;
      this.m_list_view.Columns[0].Width = num2;
    }

    public bool HitTest_ItemList(Point pos)
    {
      return this.window_mode != d3d_windows.window.mode.small && this.m_hittest_list.HitTest_Index(pos) == 0;
    }

    private enum item_index
    {
      item_list,
      country,
      icons,
      tabs,
      web,
      county_name,
      lang1,
      lang2,
      max,
    }
  }
}
