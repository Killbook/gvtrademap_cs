// Type: gvtrademap_cs.GlobalSettings
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Drawing;
using Utility;
using Utility.Ini;

namespace gvtrademap_cs
{
  public class GlobalSettings : IIniSaveLoad
  {
    private const int DEF_SEAROUTES_GROUP_MAX = 20;
    private const int DEF_TRASH_SEAROUTES_GROUP_MAX = 200;
    private Point m_window_location;
    private Size m_window_size;
    private Point m_find_window_location;
    private Size m_find_window_size;
    private bool m_find_window_visible;
    private Point m_sea_routes_window_location;
    private Size m_sea_routes_window_size;
    private bool m_sea_routes_window_visible;
    private bool m_save_searoutes;
    private bool m_draw_share_routes;
    private bool m_draw_icons;
    private bool m_draw_sea_routes;
    private int m_draw_popup_day_interval;
    private bool m_draw_accident;
    private bool m_center_myship;
    private bool m_draw_myship_angle;
    private bool m_draw_web_icons;
    private GvoWorldInfo.Server m_server;
    private GvoWorldInfo.Country m_country;
    private MapIndex m_map_index;
    private MapIcon m_map_icon;
    private MapDrawNames m_map_draw_names;
    private string m_share_group;
    private string m_share_group_myname;
    private int m_searoutes_group_max;
    private int m_trash_searoutes_group_max;
    private bool m_connect_network;
    private bool m_hook_mouse;
    private bool m_windows_vista_aero;
    private bool m_enable_analize_log_chat;
    private bool m_is_share_routes;
    private CaptureIntervalIndex m_capture_interval;
    private bool m_connect_web_icon;
    private bool m_compatible_windows_rclick;
    private TudeInterval m_tude_interval;
    private bool m_use_mixed_map;
    private bool m_window_top_most;
    private bool m_enable_line_antialias;
    private bool m_enable_sea_routes_aplha;
    private SSFormat m_ss_format;
    private bool m_remove_near_web_icons;
    private bool m_draw_capture_info;
    private bool m_is_server_mode;
    private int m_port_index;
    private bool m_is_mixed_info_names;
    private int m_minimum_draw_days;
    private bool m_enable_favorite_sea_routes_alpha;
    private bool m_draw_favorite_sea_routes_alpha_popup;
    private bool m_debug_flag_show_potision;
    private string m_select_info;
    private float m_map_pos_x;
    private float m_map_pos_y;
    private float m_map_scale;
    private bool m_is_item_window_normal_size;
    private bool m_is_setting_window_normal_size;
    private bool m_is_border_style_none;
    private UniqueString m_find_strings;
    private _find_filter m_find_filter;
    private _find_filter2 m_find_filter2;
    private _find_filter3 m_find_filter3;
    private int m_interest_days;
    private bool m_force_show_build_ship;
    private bool m_is_now_build_ship;
    private string m_build_ship_name;
    private int m_build_ship_days;
    private DrawSettingWebIcons m_draw_setting_web_icons;
    private DrawSettingMemoIcons m_draw_setting_memo_icons;
    private DrawSettingAccidents m_draw_setting_accidents;
    private DrawSettingMyShipAngle m_draw_setting_myship_angle;
    private bool m_draw_setting_myship_angle_with_speed_pos;
    private bool m_draw_setting_myship_expect_pos;
    private RequestCtrl m_req_screen_shot;
    private RequestCtrl m_req_update_map;
    private RequestCtrl m_req_centering_gpos;
    private Point m_centering_gpos;
    private RequestCtrl m_req_spot_item;
    private RequestCtrl m_req_spot_item_changed;

    public Point window_location
    {
      get
      {
        return this.m_window_location;
      }
      set
      {
        this.m_window_location = value;
      }
    }

    public Size window_size
    {
      get
      {
        return this.m_window_size;
      }
      set
      {
        this.m_window_size = value;
      }
    }

    public Point find_window_location
    {
      get
      {
        return this.m_find_window_location;
      }
      set
      {
        this.m_find_window_location = value;
      }
    }

    public Size find_window_size
    {
      get
      {
        return this.m_find_window_size;
      }
      set
      {
        this.m_find_window_size = value;
      }
    }

    public bool find_window_visible
    {
      get
      {
        return this.m_find_window_visible;
      }
      set
      {
        this.m_find_window_visible = value;
      }
    }

    public Point sea_routes_window_location
    {
      get
      {
        return this.m_sea_routes_window_location;
      }
      set
      {
        this.m_sea_routes_window_location = value;
      }
    }

    public Size sea_routes_window_size
    {
      get
      {
        return this.m_sea_routes_window_size;
      }
      set
      {
        this.m_sea_routes_window_size = value;
      }
    }

    public bool sea_routes_window_visible
    {
      get
      {
        return this.m_sea_routes_window_visible;
      }
      set
      {
        this.m_sea_routes_window_visible = value;
      }
    }

    public bool save_searoutes
    {
      get
      {
        return this.m_save_searoutes;
      }
      set
      {
        this.m_save_searoutes = value;
      }
    }

    public bool draw_share_routes
    {
      get
      {
        return this.m_draw_share_routes;
      }
      set
      {
        this.m_draw_share_routes = value;
      }
    }

    public bool draw_icons
    {
      get
      {
        return this.m_draw_icons;
      }
      set
      {
        this.m_draw_icons = value;
      }
    }

    public bool draw_sea_routes
    {
      get
      {
        return this.m_draw_sea_routes;
      }
      set
      {
        this.m_draw_sea_routes = value;
      }
    }

    public int draw_popup_day_interval
    {
      get
      {
        return this.m_draw_popup_day_interval;
      }
      set
      {
        this.m_draw_popup_day_interval = value;
        switch (this.m_draw_popup_day_interval)
        {
          case 1:
            break;
          case 5:
            break;
          default:
            this.m_draw_popup_day_interval = 0;
            break;
        }
      }
    }

    public bool draw_accident
    {
      get
      {
        return this.m_draw_accident;
      }
      set
      {
        this.m_draw_accident = value;
      }
    }

    public bool center_myship
    {
      get
      {
        return this.m_center_myship;
      }
      set
      {
        this.m_center_myship = value;
      }
    }

    public bool draw_myship_angle
    {
      get
      {
        return this.m_draw_myship_angle;
      }
      set
      {
        this.m_draw_myship_angle = value;
      }
    }

    public MapIndex map
    {
      get
      {
        return this.m_map_index;
      }
      set
      {
        this.m_map_index = value;
      }
    }

    public MapIcon map_icon
    {
      get
      {
        return this.m_map_icon;
      }
      set
      {
        this.m_map_icon = value;
      }
    }

    public MapDrawNames map_draw_names
    {
      get
      {
        return this.m_map_draw_names;
      }
      set
      {
        this.m_map_draw_names = value;
      }
    }

    public GvoWorldInfo.Server server
    {
      get
      {
        return this.m_server;
      }
      set
      {
        this.m_server = value;
      }
    }

    public GvoWorldInfo.Country country
    {
      get
      {
        return this.m_country;
      }
      set
      {
        this.m_country = value;
      }
    }

    public string share_group
    {
      get
      {
        return this.m_share_group;
      }
      set
      {
        this.m_share_group = value;
      }
    }

    public string share_group_myname
    {
      get
      {
        return this.m_share_group_myname;
      }
      set
      {
        this.m_share_group_myname = value;
      }
    }

    public string select_info
    {
      get
      {
        return this.m_select_info;
      }
      set
      {
        this.m_select_info = value;
      }
    }

    public float map_pos_x
    {
      get
      {
        return this.m_map_pos_x;
      }
      set
      {
        this.m_map_pos_x = value;
      }
    }

    public float map_pos_y
    {
      get
      {
        return this.m_map_pos_y;
      }
      set
      {
        this.m_map_pos_y = value;
      }
    }

    public float map_scale
    {
      get
      {
        return this.m_map_scale;
      }
      set
      {
        this.m_map_scale = value;
      }
    }

    public UniqueString find_strings
    {
      get
      {
        return this.m_find_strings;
      }
    }

    public _find_filter find_filter
    {
      get
      {
        return this.m_find_filter;
      }
      set
      {
        this.m_find_filter = value;
      }
    }

    public _find_filter2 find_filter2
    {
      get
      {
        return this.m_find_filter2;
      }
      set
      {
        this.m_find_filter2 = value;
      }
    }

    public _find_filter3 find_filter3
    {
      get
      {
        return this.m_find_filter3;
      }
      set
      {
        this.m_find_filter3 = value;
      }
    }

    public int searoutes_group_max
    {
      get
      {
        return this.m_searoutes_group_max;
      }
      set
      {
        this.m_searoutes_group_max = value;
        if (this.m_searoutes_group_max > 0)
          return;
        this.m_searoutes_group_max = this.def_searoutes_group_max;
      }
    }

    public int def_searoutes_group_max
    {
      get
      {
        return 20;
      }
    }

    public int trash_searoutes_group_max
    {
      get
      {
        return this.m_trash_searoutes_group_max;
      }
      set
      {
        this.m_trash_searoutes_group_max = value;
        if (this.m_trash_searoutes_group_max > 0)
          return;
        this.m_trash_searoutes_group_max = this.def_trash_searoutes_group_max;
      }
    }

    public int def_trash_searoutes_group_max
    {
      get
      {
        return 200;
      }
    }

    public RequestCtrl req_screen_shot
    {
      get
      {
        return this.m_req_screen_shot;
      }
    }

    public RequestCtrl req_update_map
    {
      get
      {
        return this.m_req_update_map;
      }
    }

    public bool connect_network
    {
      get
      {
        return this.m_connect_network;
      }
      set
      {
        this.m_connect_network = value;
      }
    }

    public bool hook_mouse
    {
      get
      {
        return this.m_hook_mouse;
      }
      set
      {
        this.m_hook_mouse = value;
      }
    }

    public bool windows_vista_aero
    {
      get
      {
        return this.m_windows_vista_aero;
      }
      set
      {
        this.m_windows_vista_aero = value;
      }
    }

    public bool enable_analize_log_chat
    {
      get
      {
        return this.m_enable_analize_log_chat;
      }
      set
      {
        this.m_enable_analize_log_chat = value;
      }
    }

    public bool is_share_routes
    {
      get
      {
        return this.m_is_share_routes;
      }
      set
      {
        this.m_is_share_routes = value;
      }
    }

    public SSFormat ss_format
    {
      get
      {
        return this.m_ss_format;
      }
      set
      {
        this.m_ss_format = value;
      }
    }

    public bool enable_share_routes
    {
      get
      {
        return this.connect_network && this.is_share_routes && (!(this.share_group == "") && !(this.share_group_myname == ""));
      }
    }

    public int interest_days
    {
      get
      {
        return this.m_interest_days;
      }
      set
      {
        this.m_interest_days = value;
      }
    }

    public bool force_show_build_ship
    {
      get
      {
        return this.m_force_show_build_ship;
      }
      set
      {
        this.m_force_show_build_ship = value;
      }
    }

    public bool is_now_build_ship
    {
      get
      {
        return this.m_is_now_build_ship;
      }
      set
      {
        this.m_is_now_build_ship = value;
      }
    }

    public string build_ship_name
    {
      get
      {
        return this.m_build_ship_name;
      }
      set
      {
        this.m_build_ship_name = value;
      }
    }

    public int build_ship_days
    {
      get
      {
        return this.m_build_ship_days;
      }
      set
      {
        this.m_build_ship_days = value;
      }
    }

    public CaptureIntervalIndex capture_interval
    {
      get
      {
        return this.m_capture_interval;
      }
      set
      {
        this.m_capture_interval = value;
      }
    }

    public bool connect_web_icon
    {
      get
      {
        return this.m_connect_web_icon;
      }
      set
      {
        this.m_connect_web_icon = value;
      }
    }

    public bool is_load_web_icon
    {
      get
      {
        if (!this.connect_network)
          return false;
        else
          return this.connect_web_icon;
      }
    }

    public bool compatible_windows_rclick
    {
      get
      {
        return this.m_compatible_windows_rclick;
      }
      set
      {
        this.m_compatible_windows_rclick = value;
      }
    }

    public bool is_item_window_normal_size
    {
      get
      {
        return this.m_is_item_window_normal_size;
      }
      set
      {
        this.m_is_item_window_normal_size = value;
      }
    }

    public bool is_setting_window_normal_size
    {
      get
      {
        return this.m_is_setting_window_normal_size;
      }
      set
      {
        this.m_is_setting_window_normal_size = value;
      }
    }

    public bool is_border_style_none
    {
      get
      {
        return this.m_is_border_style_none;
      }
      set
      {
        this.m_is_border_style_none = value;
      }
    }

    public TudeInterval tude_interval
    {
      get
      {
        return this.m_tude_interval;
      }
      set
      {
        this.m_tude_interval = value;
      }
    }

    public bool draw_web_icons
    {
      get
      {
        return this.m_draw_web_icons;
      }
      set
      {
        this.m_draw_web_icons = value;
      }
    }

    public bool use_mixed_map
    {
      get
      {
        return this.m_use_mixed_map;
      }
      set
      {
        this.m_use_mixed_map = value;
      }
    }

    public RequestCtrl req_centering_gpos
    {
      get
      {
        return this.m_req_centering_gpos;
      }
    }

    public Point centering_gpos
    {
      get
      {
        return this.m_centering_gpos;
      }
      set
      {
        this.m_centering_gpos = value;
      }
    }

    public RequestCtrl req_spot_item
    {
      get
      {
        return this.m_req_spot_item;
      }
    }

    public RequestCtrl req_spot_item_changed
    {
      get
      {
        return this.m_req_spot_item_changed;
      }
    }

    public bool window_top_most
    {
      get
      {
        return this.m_window_top_most;
      }
      set
      {
        this.m_window_top_most = value;
      }
    }

    public bool enable_line_antialias
    {
      get
      {
        return this.m_enable_line_antialias;
      }
      set
      {
        this.m_enable_line_antialias = value;
      }
    }

    public bool enable_sea_routes_aplha
    {
      get
      {
        return this.m_enable_sea_routes_aplha;
      }
      set
      {
        this.m_enable_sea_routes_aplha = value;
      }
    }

    public bool remove_near_web_icons
    {
      get
      {
        return this.m_remove_near_web_icons;
      }
      set
      {
        this.m_remove_near_web_icons = value;
      }
    }

    public bool draw_capture_info
    {
      get
      {
        return this.m_draw_capture_info;
      }
      set
      {
        this.m_draw_capture_info = value;
      }
    }

    public bool is_server_mode
    {
      get
      {
        return this.m_is_server_mode;
      }
      set
      {
        this.m_is_server_mode = value;
      }
    }

    public int port_index
    {
      get
      {
        return this.m_port_index;
      }
      set
      {
        this.m_port_index = value;
      }
    }

    public bool is_mixed_info_names
    {
      get
      {
        return this.m_is_mixed_info_names;
      }
      set
      {
        this.m_is_mixed_info_names = value;
      }
    }

    public int minimum_draw_days
    {
      get
      {
        return this.m_minimum_draw_days;
      }
      set
      {
        this.m_minimum_draw_days = value;
        if (this.m_minimum_draw_days >= 0)
          return;
        this.m_minimum_draw_days = 0;
      }
    }

    public bool enable_favorite_sea_routes_alpha
    {
      get
      {
        return this.m_enable_favorite_sea_routes_alpha;
      }
      set
      {
        this.m_enable_favorite_sea_routes_alpha = value;
      }
    }

    public bool draw_favorite_sea_routes_alpha_popup
    {
      get
      {
        return this.m_draw_favorite_sea_routes_alpha_popup;
      }
      set
      {
        this.m_draw_favorite_sea_routes_alpha_popup = value;
      }
    }

    public bool debug_flag_show_potision
    {
      get
      {
        return this.m_debug_flag_show_potision;
      }
      set
      {
        this.m_debug_flag_show_potision = value;
      }
    }

    public DrawSettingWebIcons draw_setting_web_icons
    {
      get
      {
        return this.m_draw_setting_web_icons;
      }
      set
      {
        this.m_draw_setting_web_icons = value;
      }
    }

    public DrawSettingMemoIcons draw_setting_memo_icons
    {
      get
      {
        return this.m_draw_setting_memo_icons;
      }
      set
      {
        this.m_draw_setting_memo_icons = value;
      }
    }

    public DrawSettingAccidents draw_setting_accidents
    {
      get
      {
        return this.m_draw_setting_accidents;
      }
      set
      {
        this.m_draw_setting_accidents = value;
      }
    }

    public DrawSettingMyShipAngle draw_setting_myship_angle
    {
      get
      {
        return this.m_draw_setting_myship_angle;
      }
      set
      {
        this.m_draw_setting_myship_angle = value;
      }
    }

    public bool draw_setting_myship_angle_with_speed_pos
    {
      get
      {
        return this.m_draw_setting_myship_angle_with_speed_pos;
      }
      set
      {
        this.m_draw_setting_myship_angle_with_speed_pos = value;
      }
    }

    public bool draw_setting_myship_expect_pos
    {
      get
      {
        return this.m_draw_setting_myship_expect_pos;
      }
      set
      {
        this.m_draw_setting_myship_expect_pos = value;
      }
    }

    public string DefaultIniGroupName
    {
      get
      {
        return "GlobalSettings";
      }
    }

    public GlobalSettings()
    {
      this.init();
    }

    public void CancelAllRequests()
    {
      this.req_screen_shot.CancelRequest();
      this.req_update_map.CancelRequest();
      this.req_centering_gpos.CancelRequest();
      this.req_spot_item.CancelRequest();
      this.req_spot_item_changed.CancelRequest();
    }

    private void init()
    {
      this.window_location = new Point(10, 10);
      this.window_size = new Size(640, 400);
      this.find_window_size = new Size(468, 320);
      this.find_window_location = new Point(this.window_location.X + (this.window_size.Width / 2 - this.find_window_size.Width / 2), this.window_location.Y + (this.window_size.Height / 2 - this.find_window_size.Height / 2));
      this.find_window_visible = false;
      this.sea_routes_window_size = new Size(690, 320);
      this.sea_routes_window_location = new Point(this.window_location.X + (this.window_size.Width / 2 - this.sea_routes_window_size.Width / 2), this.window_location.Y + (this.window_size.Height / 2 - this.sea_routes_window_size.Height / 2));
      this.sea_routes_window_visible = false;
      this.save_searoutes = true;
      this.draw_share_routes = false;
      this.draw_web_icons = false;
      this.draw_icons = false;
      this.draw_sea_routes = true;
      this.draw_popup_day_interval = 5;
      this.draw_accident = false;
      this.center_myship = false;
      this.draw_myship_angle = true;
      this.server = GvoWorldInfo.Server.Euros;
      this.country = GvoWorldInfo.Country.England;
      this.m_map_index = MapIndex.Map1;
      this.m_map_icon = MapIcon.Big;
      this.m_map_draw_names = MapDrawNames.Draw;
      this.share_group = "";
      this.share_group_myname = "";
      this.searoutes_group_max = 20;
      this.trash_searoutes_group_max = 200;
      this.connect_network = true;
      this.hook_mouse = false;
      this.windows_vista_aero = false;
      this.enable_analize_log_chat = true;
      this.is_share_routes = false;
      this.capture_interval = CaptureIntervalIndex.Per1000ms;
      this.connect_web_icon = false;
      this.compatible_windows_rclick = false;
      this.tude_interval = TudeInterval.OnlyPoints;
      this.use_mixed_map = true;
      this.window_top_most = false;
      this.enable_line_antialias = true;
      this.enable_sea_routes_aplha = false;
      this.m_ss_format = SSFormat.Bmp;
      this.remove_near_web_icons = true;
      this.draw_capture_info = false;
      this.is_server_mode = false;
      this.port_index = 16612;
      this.is_mixed_info_names = false;
      this.minimum_draw_days = 0;
      this.enable_favorite_sea_routes_alpha = true;
      this.draw_favorite_sea_routes_alpha_popup = false;
      this.debug_flag_show_potision = false;
      this.select_info = "";
      this.map_pos_x = 0.0f;
      this.map_pos_y = 0.0f;
      this.map_scale = 1f;
      this.is_item_window_normal_size = true;
      this.is_setting_window_normal_size = true;
      this.is_border_style_none = false;
      this.m_find_strings = new UniqueString();
      this.find_filter = _find_filter.both;
      this.find_filter2 = _find_filter2.name;
      this.find_filter3 = _find_filter3.full_text_search;
      this.interest_days = 0;
      this.force_show_build_ship = false;
      this.is_now_build_ship = false;
      this.build_ship_name = "";
      this.build_ship_days = 0;
      this.m_req_screen_shot = new RequestCtrl();
      this.m_req_update_map = new RequestCtrl();
      this.m_req_centering_gpos = new RequestCtrl();
      this.m_req_spot_item = new RequestCtrl();
      this.m_req_spot_item_changed = new RequestCtrl();
      this.m_centering_gpos = new Point(-1, -1);
      this.draw_setting_web_icons = DrawSettingWebIcons.wind | DrawSettingWebIcons.accident_0 | DrawSettingWebIcons.accident_1 | DrawSettingWebIcons.accident_2 | DrawSettingWebIcons.accident_3 | DrawSettingWebIcons.accident_4;
      this.draw_setting_memo_icons = DrawSettingMemoIcons.wind | DrawSettingMemoIcons.memo_0 | DrawSettingMemoIcons.memo_1 | DrawSettingMemoIcons.memo_2 | DrawSettingMemoIcons.memo_3 | DrawSettingMemoIcons.memo_4 | DrawSettingMemoIcons.memo_5 | DrawSettingMemoIcons.memo_6 | DrawSettingMemoIcons.memo_7 | DrawSettingMemoIcons.memo_8 | DrawSettingMemoIcons.memo_9 | DrawSettingMemoIcons.memo_10 | DrawSettingMemoIcons.memo_11;
      this.draw_setting_accidents = DrawSettingAccidents.accident_0 | DrawSettingAccidents.accident_1 | DrawSettingAccidents.accident_2 | DrawSettingAccidents.accident_3 | DrawSettingAccidents.accident_4 | DrawSettingAccidents.accident_5 | DrawSettingAccidents.accident_6 | DrawSettingAccidents.accident_7 | DrawSettingAccidents.accident_8 | DrawSettingAccidents.accident_9 | DrawSettingAccidents.accident_10;
      this.draw_setting_myship_angle = DrawSettingMyShipAngle.draw_0 | DrawSettingMyShipAngle.draw_1;
      this.draw_setting_myship_angle_with_speed_pos = true;
      this.draw_setting_myship_expect_pos = true;
    }

    public GlobalSettings Clone()
    {
      GlobalSettings globalSettings = new GlobalSettings();
      globalSettings.window_location = this.window_location;
      globalSettings.window_size = this.window_size;
      globalSettings.find_window_location = this.find_window_location;
      globalSettings.find_window_size = this.find_window_size;
      globalSettings.find_window_visible = this.find_window_visible;
      globalSettings.sea_routes_window_size = this.sea_routes_window_size;
      globalSettings.sea_routes_window_location = this.sea_routes_window_location;
      globalSettings.sea_routes_window_visible = this.sea_routes_window_visible;
      globalSettings.save_searoutes = this.save_searoutes;
      globalSettings.draw_share_routes = this.draw_share_routes;
      globalSettings.draw_icons = this.draw_icons;
      globalSettings.draw_sea_routes = this.draw_sea_routes;
      globalSettings.draw_popup_day_interval = this.draw_popup_day_interval;
      globalSettings.draw_accident = this.draw_accident;
      globalSettings.center_myship = this.center_myship;
      globalSettings.draw_myship_angle = this.draw_myship_angle;
      globalSettings.server = this.server;
      globalSettings.country = this.country;
      globalSettings.map = this.map;
      globalSettings.map_icon = this.map_icon;
      globalSettings.map_draw_names = this.map_draw_names;
      globalSettings.share_group = this.share_group;
      globalSettings.share_group_myname = this.share_group_myname;
      globalSettings.select_info = this.select_info;
      globalSettings.map_pos_x = this.map_pos_x;
      globalSettings.map_pos_y = this.map_pos_y;
      globalSettings.map_scale = this.map_scale;
      globalSettings.searoutes_group_max = this.searoutes_group_max;
      globalSettings.trash_searoutes_group_max = this.trash_searoutes_group_max;
      globalSettings.m_find_strings.Clone(this.m_find_strings);
      globalSettings.find_filter = this.find_filter;
      globalSettings.find_filter2 = this.find_filter2;
      globalSettings.find_filter3 = this.find_filter3;
      globalSettings.connect_network = this.connect_network;
      globalSettings.hook_mouse = this.hook_mouse;
      globalSettings.windows_vista_aero = this.windows_vista_aero;
      globalSettings.enable_analize_log_chat = this.enable_analize_log_chat;
      globalSettings.is_share_routes = this.is_share_routes;
      globalSettings.interest_days = this.interest_days;
      globalSettings.force_show_build_ship = this.force_show_build_ship;
      globalSettings.is_now_build_ship = this.is_now_build_ship;
      globalSettings.build_ship_name = this.build_ship_name;
      globalSettings.build_ship_days = this.build_ship_days;
      globalSettings.capture_interval = this.capture_interval;
      globalSettings.connect_web_icon = this.connect_web_icon;
      globalSettings.compatible_windows_rclick = this.compatible_windows_rclick;
      globalSettings.is_item_window_normal_size = this.is_item_window_normal_size;
      globalSettings.is_setting_window_normal_size = this.is_setting_window_normal_size;
      globalSettings.is_border_style_none = this.is_border_style_none;
      globalSettings.tude_interval = this.tude_interval;
      globalSettings.draw_web_icons = this.draw_web_icons;
      globalSettings.use_mixed_map = this.use_mixed_map;
      globalSettings.window_top_most = this.window_top_most;
      globalSettings.enable_line_antialias = this.enable_line_antialias;
      globalSettings.enable_sea_routes_aplha = this.enable_sea_routes_aplha;
      globalSettings.draw_setting_web_icons = this.draw_setting_web_icons;
      globalSettings.draw_setting_memo_icons = this.draw_setting_memo_icons;
      globalSettings.draw_setting_accidents = this.draw_setting_accidents;
      globalSettings.draw_setting_myship_angle = this.draw_setting_myship_angle;
      globalSettings.draw_setting_myship_angle_with_speed_pos = this.draw_setting_myship_angle_with_speed_pos;
      globalSettings.draw_setting_myship_expect_pos = this.draw_setting_myship_expect_pos;
      globalSettings.ss_format = this.ss_format;
      globalSettings.remove_near_web_icons = this.remove_near_web_icons;
      globalSettings.draw_capture_info = this.draw_capture_info;
      globalSettings.is_server_mode = this.is_server_mode;
      globalSettings.port_index = this.port_index;
      globalSettings.is_mixed_info_names = this.is_mixed_info_names;
      globalSettings.minimum_draw_days = this.minimum_draw_days;
      globalSettings.enable_favorite_sea_routes_alpha = this.enable_favorite_sea_routes_alpha;
      globalSettings.draw_favorite_sea_routes_alpha_popup = this.draw_favorite_sea_routes_alpha_popup;
      globalSettings.debug_flag_show_potision = this.debug_flag_show_potision;
      globalSettings.CancelAllRequests();
      return globalSettings;
    }

    public void Clone(GlobalSettings s)
    {
      bool flag = false;
      if (this.map != s.map || this.map_icon != s.map_icon || (this.map_draw_names != s.map_draw_names || this.is_mixed_info_names != s.is_mixed_info_names))
        flag = true;
      this.window_location = s.window_location;
      this.window_size = s.window_size;
      this.find_window_location = s.find_window_location;
      this.find_window_size = s.find_window_size;
      this.find_window_visible = s.find_window_visible;
      this.sea_routes_window_size = s.sea_routes_window_size;
      this.sea_routes_window_location = s.sea_routes_window_location;
      this.sea_routes_window_visible = s.sea_routes_window_visible;
      this.save_searoutes = s.save_searoutes;
      this.draw_share_routes = s.draw_share_routes;
      this.draw_icons = s.draw_icons;
      this.draw_sea_routes = s.draw_sea_routes;
      this.draw_popup_day_interval = s.draw_popup_day_interval;
      this.draw_accident = s.draw_accident;
      this.center_myship = s.center_myship;
      this.draw_myship_angle = s.draw_myship_angle;
      this.server = s.server;
      this.country = s.country;
      this.map = s.map;
      this.map_icon = s.map_icon;
      this.map_draw_names = s.map_draw_names;
      this.share_group = s.share_group;
      this.share_group_myname = s.share_group_myname;
      this.select_info = s.select_info;
      this.map_pos_x = s.map_pos_x;
      this.map_pos_y = s.map_pos_y;
      this.map_scale = s.map_scale;
      this.searoutes_group_max = s.searoutes_group_max;
      this.trash_searoutes_group_max = s.trash_searoutes_group_max;
      this.m_find_strings.Clone(s.m_find_strings);
      this.find_filter = s.find_filter;
      this.find_filter2 = s.find_filter2;
      this.find_filter3 = s.find_filter3;
      this.connect_network = s.connect_network;
      this.hook_mouse = s.hook_mouse;
      this.windows_vista_aero = s.windows_vista_aero;
      this.enable_analize_log_chat = s.enable_analize_log_chat;
      this.is_share_routes = s.is_share_routes;
      this.interest_days = s.interest_days;
      this.force_show_build_ship = s.force_show_build_ship;
      this.is_now_build_ship = s.is_now_build_ship;
      this.build_ship_name = s.build_ship_name;
      this.build_ship_days = s.build_ship_days;
      this.capture_interval = s.capture_interval;
      this.connect_web_icon = s.connect_web_icon;
      this.compatible_windows_rclick = s.compatible_windows_rclick;
      this.is_item_window_normal_size = s.is_item_window_normal_size;
      this.is_setting_window_normal_size = s.is_setting_window_normal_size;
      this.is_border_style_none = s.is_border_style_none;
      this.tude_interval = s.tude_interval;
      this.draw_web_icons = s.draw_web_icons;
      this.use_mixed_map = s.use_mixed_map;
      this.window_top_most = s.window_top_most;
      this.enable_line_antialias = s.enable_line_antialias;
      this.enable_sea_routes_aplha = s.enable_sea_routes_aplha;
      this.draw_setting_web_icons = s.draw_setting_web_icons;
      this.draw_setting_memo_icons = s.draw_setting_memo_icons;
      this.draw_setting_accidents = s.draw_setting_accidents;
      this.draw_setting_myship_angle = s.draw_setting_myship_angle;
      this.draw_setting_myship_angle_with_speed_pos = s.draw_setting_myship_angle_with_speed_pos;
      this.draw_setting_myship_expect_pos = s.draw_setting_myship_expect_pos;
      this.ss_format = s.ss_format;
      this.remove_near_web_icons = s.remove_near_web_icons;
      this.draw_capture_info = s.draw_capture_info;
      this.is_server_mode = s.is_server_mode;
      this.port_index = s.port_index;
      this.is_mixed_info_names = s.is_mixed_info_names;
      this.minimum_draw_days = s.minimum_draw_days;
      this.enable_favorite_sea_routes_alpha = s.enable_favorite_sea_routes_alpha;
      this.draw_favorite_sea_routes_alpha_popup = s.draw_favorite_sea_routes_alpha_popup;
      this.debug_flag_show_potision = s.debug_flag_show_potision;
      this.CancelAllRequests();
      if (!flag)
        return;
      this.req_update_map.Request();
    }

    public void IniLoad(IIni p, string group)
    {
      this.m_window_location.X = p.GetProfile("window", "pos_x", this.window_location.X);
      this.m_window_location.Y = p.GetProfile("window", "pos_y", this.window_location.Y);
      this.m_window_size.Width = p.GetProfile("window", "size_x", this.window_size.Width);
      this.m_window_size.Height = p.GetProfile("window", "size_y", this.window_size.Height);
      this.m_find_window_location.X = p.GetProfile("find_window", "pos_x", this.find_window_location.X);
      this.m_find_window_location.Y = p.GetProfile("find_window", "pos_y", this.find_window_location.Y);
      this.m_find_window_size.Width = p.GetProfile("find_window", "size_x", this.find_window_size.Width);
      this.m_find_window_size.Height = p.GetProfile("find_window", "size_y", this.find_window_size.Height);
      this.find_window_visible = p.GetProfile("find_window", "visible", this.find_window_visible);
      this.m_sea_routes_window_location.X = p.GetProfile("sea_routes_window", "pos_x", this.sea_routes_window_location.X);
      this.m_sea_routes_window_location.Y = p.GetProfile("sea_routes_window", "pos_y", this.sea_routes_window_location.Y);
      this.m_sea_routes_window_size.Width = p.GetProfile("sea_routes_window", "size_x", this.sea_routes_window_size.Width);
      this.m_sea_routes_window_size.Height = p.GetProfile("sea_routes_window", "size_y", this.sea_routes_window_size.Height);
      this.sea_routes_window_visible = p.GetProfile("sea_routes_window", "visible", this.sea_routes_window_visible);
      this.save_searoutes = p.GetProfile("icon", "save_searoutes", this.save_searoutes);
      this.draw_share_routes = p.GetProfile("icon", "draw_share_routes", this.draw_share_routes);
      this.draw_icons = p.GetProfile("icon", "draw_icons", this.draw_icons);
      this.draw_sea_routes = p.GetProfile("icon", "draw_sea_routes", this.draw_sea_routes);
      this.draw_popup_day_interval = p.GetProfile("icon", "draw_popup_day_interval", this.draw_popup_day_interval);
      this.draw_accident = p.GetProfile("icon", "draw_accident", this.draw_accident);
      this.center_myship = p.GetProfile("icon", "center_myship", this.center_myship);
      this.draw_myship_angle = p.GetProfile("icon", "draw_myship_angle", this.draw_myship_angle);
      this.draw_web_icons = p.GetProfile("icon", "draw_web_icons", this.draw_web_icons);
      this.server = GvoWorldInfo.GetServerFromString(p.GetProfile("dialog", "server", ((object) this.server).ToString()));
      this.country = GvoWorldInfo.GetCountryFromString(p.GetProfile("dialog", "country", ((object) this.country).ToString()));
      this.map = (MapIndex) p.GetProfile("dialog", "map_index_new", (int) this.map);
      this.map_icon = (MapIcon) p.GetProfile("dialog", "map_icon", (int) this.map_icon);
      this.map_draw_names = (MapDrawNames) p.GetProfile("dialog", "map_draw_names", (int) this.map_draw_names);
      this.share_group = p.GetProfile("dialog", "share_group", this.share_group);
      this.share_group_myname = p.GetProfile("dialog", "share_group_myname", this.share_group_myname);
      this.searoutes_group_max = p.GetProfile("dialog", "searoutes_group_max", this.searoutes_group_max);
      this.trash_searoutes_group_max = p.GetProfile("dialog", "trash_searoutes_group_max", this.trash_searoutes_group_max);
      this.connect_network = p.GetProfile("dialog", "connect_network", this.connect_network);
      this.hook_mouse = p.GetProfile("dialog", "hook_mouse", this.hook_mouse);
      this.windows_vista_aero = p.GetProfile("dialog", "windows_vista_aero", this.windows_vista_aero);
      this.enable_analize_log_chat = p.GetProfile("dialog", "enable_analize_log_chat", this.enable_analize_log_chat);
      this.is_share_routes = p.GetProfile("dialog", "is_share_routes", this.is_share_routes);
      this.capture_interval = (CaptureIntervalIndex) p.GetProfile("dialog", "capture_interval", (int) this.capture_interval);
      this.connect_web_icon = p.GetProfile("dialog", "connect_web_icon", this.connect_web_icon);
      this.compatible_windows_rclick = p.GetProfile("dialog", "compatible_windows_rclick", this.compatible_windows_rclick);
      this.tude_interval = (TudeInterval) p.GetProfile("dialog", "tude_interval", (int) this.tude_interval);
      this.use_mixed_map = p.GetProfile("dialog", "use_mixed_map", this.use_mixed_map);
      this.window_top_most = p.GetProfile("dialog", "window_top_most", this.window_top_most);
      this.enable_line_antialias = p.GetProfile("dialog", "enable_line_antialias", this.enable_line_antialias);
      this.enable_sea_routes_aplha = p.GetProfile("dialog", "enable_sea_routes_aplha", this.enable_sea_routes_aplha);
      this.ss_format = (SSFormat) p.GetProfile("dialog", "ss_format", (int) this.ss_format);
      this.remove_near_web_icons = p.GetProfile("dialog", "remove_near_web_icons", this.remove_near_web_icons);
      this.draw_capture_info = p.GetProfile("dialog", "draw_capture_info", this.draw_capture_info);
      this.is_server_mode = p.GetProfile("dialog", "is_server_mode", this.is_server_mode);
      this.port_index = p.GetProfile("dialog", "port_index", this.port_index);
      this.is_mixed_info_names = p.GetProfile("dialog", "is_mixed_info_names", this.is_mixed_info_names);
      this.minimum_draw_days = p.GetProfile("dialog", "minimum_draw_days", this.minimum_draw_days);
      this.enable_favorite_sea_routes_alpha = p.GetProfile("dialog", "enable_favorite_sea_routes_alpha", this.enable_favorite_sea_routes_alpha);
      this.draw_favorite_sea_routes_alpha_popup = p.GetProfile("dialog", "draw_favorite_sea_routes_alpha_popup", this.draw_favorite_sea_routes_alpha_popup);
      this.debug_flag_show_potision = p.GetProfile("dialog", "debug_flag_show_potision", this.debug_flag_show_potision);
      this.m_select_info = p.GetProfile("map", "select_info", this.m_select_info);
      this.map_pos_x = p.GetProfile("map", "map_pos_x", this.map_pos_x);
      this.map_pos_y = p.GetProfile("map", "map_pos_y", this.map_pos_y);
      this.map_scale = p.GetProfile("map", "map_scale", this.map_scale);
      this.is_item_window_normal_size = p.GetProfile("item_window", "is_normal_size", this.is_item_window_normal_size);
      this.is_setting_window_normal_size = p.GetProfile("setting_window", "is_normal_size", this.is_setting_window_normal_size);
      this.is_border_style_none = p.GetProfile("setting_window", "is_border_style_none", this.is_border_style_none);
      this.interest_days = p.GetProfile("interest", "interest_days", this.interest_days);
      this.force_show_build_ship = p.GetProfile("build_ship", "force_show_build_ship", this.force_show_build_ship);
      this.is_now_build_ship = p.GetProfile("build_ship", "is_now_build_ship", this.is_now_build_ship);
      this.build_ship_name = p.GetProfile("build_ship", "build_ship_name", this.build_ship_name);
      this.build_ship_days = p.GetProfile("build_ship", "build_ship_days", this.build_ship_days);
      int num = 0;
      this.m_find_strings.Clear();
      while (true)
      {
        string profile = p.GetProfile("find", string.Format("list{0}", (object) num), "");
        if (!(profile == ""))
        {
          this.m_find_strings.AddLast(profile);
          ++num;
        }
        else
          break;
      }
      this.find_filter = (_find_filter) p.GetProfile("find", "find_filter", (int) this.find_filter);
      this.find_filter2 = (_find_filter2) p.GetProfile("find", "find_filter2", (int) this.find_filter2);
      this.find_filter3 = (_find_filter3) p.GetProfile("find", "find_filter3", (int) this.find_filter3);
      this.draw_setting_web_icons = (DrawSettingWebIcons) p.GetProfile("draw_setting", "draw_setting_web_icons", (int) this.draw_setting_web_icons);
      this.draw_setting_memo_icons = (DrawSettingMemoIcons) p.GetProfile("draw_setting", "draw_setting_memo_icons", (int) this.draw_setting_memo_icons);
      this.draw_setting_accidents = (DrawSettingAccidents) p.GetProfile("draw_setting", "draw_setting_accidents", (int) this.draw_setting_accidents);
      this.draw_setting_myship_angle = (DrawSettingMyShipAngle) p.GetProfile("draw_setting", "draw_setting_myship_angle", (int) this.draw_setting_myship_angle);
      this.draw_setting_myship_angle_with_speed_pos = p.GetProfile("draw_setting", "draw_setting_myship_angle_with_speed_pos", this.draw_setting_myship_angle_with_speed_pos);
      this.draw_setting_myship_expect_pos = p.GetProfile("draw_setting", "draw_setting_myship_expect_pos", this.draw_setting_myship_expect_pos);
    }

    public void IniSave(IIni p, string group)
    {
      p.SetProfile("window", "pos_x", this.window_location.X);
      p.SetProfile("window", "pos_y", this.window_location.Y);
      p.SetProfile("window", "size_x", this.window_size.Width);
      p.SetProfile("window", "size_y", this.window_size.Height);
      p.SetProfile("find_window", "pos_x", this.find_window_location.X);
      p.SetProfile("find_window", "pos_y", this.find_window_location.Y);
      p.SetProfile("find_window", "size_x", this.find_window_size.Width);
      p.SetProfile("find_window", "size_y", this.find_window_size.Height);
      p.SetProfile("find_window", "visible", this.find_window_visible);
      p.SetProfile("sea_routes_window", "pos_x", this.sea_routes_window_location.X);
      p.SetProfile("sea_routes_window", "pos_y", this.sea_routes_window_location.Y);
      p.SetProfile("sea_routes_window", "size_x", this.sea_routes_window_size.Width);
      p.SetProfile("sea_routes_window", "size_y", this.sea_routes_window_size.Height);
      p.SetProfile("sea_routes_window", "visible", this.sea_routes_window_visible);
      p.SetProfile("icon", "save_searoutes", this.save_searoutes);
      p.SetProfile("icon", "draw_share_routes", this.draw_share_routes);
      p.SetProfile("icon", "draw_icons", this.draw_icons);
      p.SetProfile("icon", "draw_sea_routes", this.draw_sea_routes);
      p.SetProfile("icon", "draw_popup_day_interval", this.draw_popup_day_interval);
      p.SetProfile("icon", "draw_accident", this.draw_accident);
      p.SetProfile("icon", "center_myship", this.center_myship);
      p.SetProfile("icon", "draw_myship_angle", this.draw_myship_angle);
      p.SetProfile("icon", "draw_web_icons", this.draw_web_icons);
      p.SetProfile("dialog", "server", ((object) this.server).ToString());
      p.SetProfile("dialog", "country", ((object) this.country).ToString());
      p.SetProfile("dialog", "map_index_new", (int) this.map);
      p.SetProfile("dialog", "map_icon", (int) this.map_icon);
      p.SetProfile("dialog", "map_draw_names", (int) this.map_draw_names);
      p.SetProfile("dialog", "share_group", this.share_group);
      p.SetProfile("dialog", "share_group_myname", this.share_group_myname);
      p.SetProfile("dialog", "searoutes_group_max", this.searoutes_group_max);
      p.SetProfile("dialog", "trash_searoutes_group_max", this.trash_searoutes_group_max);
      p.SetProfile("dialog", "connect_network", this.connect_network);
      p.SetProfile("dialog", "hook_mouse", this.hook_mouse);
      p.SetProfile("dialog", "windows_vista_aero", this.windows_vista_aero);
      p.SetProfile("dialog", "enable_analize_log_chat", this.enable_analize_log_chat);
      p.SetProfile("dialog", "is_share_routes", this.is_share_routes);
      p.SetProfile("dialog", "capture_interval", (int) this.capture_interval);
      p.SetProfile("dialog", "connect_web_icon", this.connect_web_icon);
      p.SetProfile("dialog", "compatible_windows_rclick", this.compatible_windows_rclick);
      p.SetProfile("dialog", "tude_interval", (int) this.tude_interval);
      p.SetProfile("dialog", "use_mixed_map", this.use_mixed_map);
      p.SetProfile("dialog", "window_top_most", this.window_top_most);
      p.SetProfile("dialog", "enable_line_antialias", this.enable_line_antialias);
      p.SetProfile("dialog", "enable_sea_routes_aplha", this.enable_sea_routes_aplha);
      p.SetProfile("dialog", "ss_format", (int) this.ss_format);
      p.SetProfile("dialog", "remove_near_web_icons", this.remove_near_web_icons);
      p.SetProfile("dialog", "draw_capture_info", this.draw_capture_info);
      p.SetProfile("dialog", "is_server_mode", this.is_server_mode);
      p.SetProfile("dialog", "port_index", this.port_index);
      p.SetProfile("dialog", "is_mixed_info_names", this.is_mixed_info_names);
      p.SetProfile("dialog", "minimum_draw_days", this.minimum_draw_days);
      p.SetProfile("dialog", "enable_favorite_sea_routes_alpha", this.enable_favorite_sea_routes_alpha);
      p.SetProfile("dialog", "draw_favorite_sea_routes_alpha_popup", this.draw_favorite_sea_routes_alpha_popup);
      p.SetProfile("dialog", "debug_flag_show_potision", this.debug_flag_show_potision);
      p.SetProfile("map", "select_info", this.m_select_info);
      p.SetProfile("map", "map_pos_x", this.map_pos_x);
      p.SetProfile("map", "map_pos_y", this.map_pos_y);
      p.SetProfile("map", "map_scale", this.map_scale);
      p.SetProfile("item_window", "is_normal_size", this.is_item_window_normal_size);
      p.SetProfile("setting_window", "is_normal_size", this.is_setting_window_normal_size);
      p.SetProfile("setting_window", "is_border_style_none", this.is_border_style_none);
      p.SetProfile("interest", "interest_days", this.interest_days);
      p.SetProfile("build_ship", "force_show_build_ship", this.force_show_build_ship);
      p.SetProfile("build_ship", "is_now_build_ship", this.is_now_build_ship);
      p.SetProfile("build_ship", "build_ship_name", this.build_ship_name);
      p.SetProfile("build_ship", "build_ship_days", this.build_ship_days);
      int num = 0;
      foreach (string str in this.m_find_strings)
      {
        p.SetProfile("find", string.Format("list{0}", (object) num), str);
        ++num;
      }
      p.SetProfile("find", "find_filter", (int) this.find_filter);
      p.SetProfile("find", "find_filter2", (int) this.find_filter2);
      p.SetProfile("find", "find_filter3", (int) this.find_filter3);
      p.SetProfile("draw_setting", "draw_setting_web_icons", (int) this.draw_setting_web_icons);
      p.SetProfile("draw_setting", "draw_setting_memo_icons", (int) this.draw_setting_memo_icons);
      p.SetProfile("draw_setting", "draw_setting_accidents", (int) this.draw_setting_accidents);
      p.SetProfile("draw_setting", "draw_setting_myship_angle", (int) this.draw_setting_myship_angle);
      p.SetProfile("draw_setting", "draw_setting_myship_angle_with_speed_pos", this.draw_setting_myship_angle_with_speed_pos);
      p.SetProfile("draw_setting", "draw_setting_myship_expect_pos", this.draw_setting_myship_expect_pos);
    }
  }
}
