// Type: gvtrademap_cs.SeaRoutes
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using directx;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Utility;

namespace gvtrademap_cs
{
  public class SeaRoutes
  {
    private const float LINE_ROUTE_MAX = 225f;
    private const float ADD_POINT_MIN = 49f;
    private const float ADD_POINT_ANGLE_GAP_MAX = 16f;
    private const float ADD_POINT_ANGLE_LENGTH_SQ_MAX = 4000000f;
    private const int SCREEN_SHOT_BOUNDING_BOX_GAP_X = 64;
    private const int SCREEN_SHOT_BOUNDING_BOX_GAP_Y = 64;
    private const float SEAROUTES_ALPHA = 0.4f;
    private const float BB_SIZE_MAX = 250f;
    private const float BB_POPUP_SIZE_MAX = 350f;
    private const float BB_OUTSIDESCREEEN_OFFSET = 32f;
    private const int SS_DISTRIBUTION_X = 64;
    private gvt_lib m_lib;
    private List<SeaRoutes.Voyage> m_sea_routes;
    private List<SeaRoutes.Voyage> m_favorite_sea_routes;
    private List<SeaRoutes.Voyage> m_trash_sea_routes;
    private int m_color_index;
    private int m_old_days;
    private Point m_old_day_pos;
    private Point m_old_pos;
    private bool m_is_1st;
    private bool m_is_select_mode;
    private RequestCtrl m_req_update_list;
    private RequestCtrl m_req_redraw_list;

    public List<SeaRoutes.Voyage> searoutes
    {
      get
      {
        return this.m_sea_routes;
      }
    }

    public List<SeaRoutes.Voyage> favorite_sea_routes
    {
      get
      {
        return this.m_favorite_sea_routes;
      }
    }

    public List<SeaRoutes.Voyage> trash_sea_routes
    {
      get
      {
        return this.m_trash_sea_routes;
      }
    }

    public RequestCtrl req_update_list
    {
      get
      {
        return this.m_req_update_list;
      }
    }

    public RequestCtrl req_redraw_list
    {
      get
      {
        return this.m_req_redraw_list;
      }
    }

    public SeaRoutes(gvt_lib lib, string file_name, string favorite_file_name, string trash_file_name)
    {
      this.m_lib = lib;
      this.m_sea_routes = new List<SeaRoutes.Voyage>();
      this.m_favorite_sea_routes = new List<SeaRoutes.Voyage>();
      this.m_trash_sea_routes = new List<SeaRoutes.Voyage>();
      this.m_req_update_list = new RequestCtrl();
      this.m_req_redraw_list = new RequestCtrl();
      this.m_is_select_mode = false;
      this.init_add_points();
      this.load_old_routes("temp\\searoute_temp.txt", "temp\\searoute_temp2.txt");
      this.load_routes(file_name);
      this.load_routes_sub(this.m_favorite_sea_routes, favorite_file_name);
      this.load_routes_sub(this.m_trash_sea_routes, trash_file_name);
    }

    private bool load_old_routes(string file_name1, string file_name2)
    {
      if (!File.Exists(file_name1) && !File.Exists(file_name2))
        return true;
      int num1 = -1;
      try
      {
        using (StreamReader streamReader = new StreamReader(file_name2, Encoding.GetEncoding("Shift_JIS")))
        {
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            try
            {
              string[] strArray = str.Split(new char[1]
              {
                ','
              });
              int num2 = Convert.ToInt32(strArray[0]);
              int num3 = Convert.ToInt32(strArray[1]);
              int color_index = Convert.ToInt32(strArray[2]);
              if (num1 != color_index)
              {
                this.add_sea_routes();
                num1 = color_index;
              }
              this.add_point(new SeaRoutes.SeaRoutePoint((float) num2, (float) num3, color_index));
            }
            catch
            {
            }
          }
        }
      }
      catch
      {
        this.m_sea_routes.Clear();
      }
      try
      {
        using (StreamReader streamReader = new StreamReader(file_name1, Encoding.GetEncoding("Shift_JIS")))
        {
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            try
            {
              string[] strArray = str.Split(new char[1]
              {
                ','
              });
              int num2 = Convert.ToInt32(strArray[0]);
              int num3 = Convert.ToInt32(strArray[1]);
              int days = Convert.ToInt32(strArray[2]);
              int color_index = Convert.ToInt32(strArray[3]);
              if (color_index >= 101 && color_index <= 111)
                this.add_accident(new SeaRoutes.SeaRoutePopupPoint((float) num2, (float) num3, color_index, days));
              else
                this.add_popup(new SeaRoutes.SeaRoutePopupPoint((float) num2, (float) num3, color_index, days));
            }
            catch
            {
            }
          }
        }
      }
      catch
      {
        this.m_sea_routes.Clear();
      }
      file_ctrl.RemoveFile(file_name1);
      file_ctrl.RemoveFile(file_name2);
      return true;
    }

    private bool load_routes(string file_name)
    {
      if (!this.load_routes_sub(this.m_sea_routes, file_name))
        return false;
      if (this.m_sea_routes.Count > 1)
      {
        this.m_color_index = this.get_newest_sea_routes().GetColorIndex();
        if (this.m_color_index < 0)
          this.m_color_index = 0;
        if (++this.m_color_index >= 8)
          this.m_color_index = 0;
      }
      return true;
    }

    private bool load_routes_sub(List<SeaRoutes.Voyage> list, string file_name)
    {
      try
      {
        using (StreamReader streamReader = new StreamReader(file_name, Encoding.GetEncoding("Shift_JIS")))
        {
          string line;
          while ((line = streamReader.ReadLine()) != null)
          {
            if (line == "start routes")
            {
              this.add_sea_routes(list);
              this.get_newest_sea_routes(list).StartLoad();
            }
            else
              this.get_newest_sea_routes(list).LoadFromLine(line);
          }
        }
      }
      catch
      {
        return false;
      }
      return true;
    }

    private SeaRoutes.Voyage get_newest_sea_routes()
    {
      return this.get_newest_sea_routes(this.m_sea_routes);
    }

    private SeaRoutes.Voyage get_newest_sea_routes(List<SeaRoutes.Voyage> list)
    {
      if (list.Count < 1)
        this.add_sea_routes(list);
      return list[list.Count - 1];
    }

    private void add_sea_routes()
    {
      this.add_sea_routes(this.m_sea_routes);
    }

    private void add_sea_routes(List<SeaRoutes.Voyage> list)
    {
      list.Add(new SeaRoutes.Voyage(this.m_lib));
      this.m_req_update_list.Request();
    }

    private void ajust_size()
    {
      if (this.m_sea_routes.Count > 0)
      {
        while (this.m_sea_routes.Count > this.m_lib.setting.searoutes_group_max)
        {
          SeaRoutes.Voyage voyage = this.m_sea_routes[0];
          this.m_sea_routes.RemoveAt(0);
          this.m_trash_sea_routes.Add(voyage);
          this.m_req_update_list.Request();
        }
      }
      while (this.m_trash_sea_routes.Count > this.m_lib.setting.trash_searoutes_group_max)
      {
        this.m_trash_sea_routes.RemoveAt(0);
        this.m_req_update_list.Request();
      }
    }

    public void DrawRoutesLines()
    {
      this.ajust_size();
      this.check_select_mode();
      this.set_alpha();
      this.set_minimum_draw_days();
      this.draw_routes();
    }

    private IEnumerable<SeaRoutes.Voyage> enum_all_sea_routes()
    {
      foreach (SeaRoutes.Voyage voyage in this.m_trash_sea_routes)
        yield return voyage;
      foreach (SeaRoutes.Voyage voyage in this.m_favorite_sea_routes)
        yield return voyage;
      foreach (SeaRoutes.Voyage voyage in this.m_sea_routes)
        yield return voyage;
    }

    private IEnumerable<SeaRoutes.Voyage> enum_sea_routes_without_trash()
    {
      foreach (SeaRoutes.Voyage voyage in this.m_favorite_sea_routes)
        yield return voyage;
      foreach (SeaRoutes.Voyage voyage in this.m_sea_routes)
        yield return voyage;
    }

    private void check_select_mode()
    {
      this.m_is_select_mode = false;
      foreach (SeaRoutes.Voyage voyage in this.enum_all_sea_routes())
      {
        if (voyage.IsSelected)
        {
          this.m_is_select_mode = true;
          break;
        }
      }
    }

    public void DrawPopups()
    {
      this.draw_accident();
      this.draw_popups();
    }

    private void set_alpha()
    {
      if (this.m_lib.setting.enable_sea_routes_aplha)
      {
        foreach (SeaRoutes.Voyage voyage in this.m_sea_routes)
          voyage.Alpha = 0.4f;
        if (this.m_sea_routes.Count > 0)
          this.get_newest_sea_routes().Alpha = 1f;
      }
      else
      {
        foreach (SeaRoutes.Voyage voyage in this.m_sea_routes)
          voyage.Alpha = 1f;
      }
      if (this.m_lib.setting.enable_favorite_sea_routes_alpha)
      {
        foreach (SeaRoutes.Voyage voyage in this.m_favorite_sea_routes)
          voyage.Alpha = 0.4f;
      }
      else
      {
        foreach (SeaRoutes.Voyage voyage in this.m_favorite_sea_routes)
          voyage.Alpha = 1f;
      }
    }

    private void set_minimum_draw_days()
    {
      foreach (SeaRoutes.Voyage voyage in this.m_sea_routes)
        voyage.MinimumDrawDays = this.m_lib.setting.minimum_draw_days;
      if (this.m_sea_routes.Count <= 0)
        return;
      this.get_newest_sea_routes().MinimumDrawDays = 0;
    }

    private void draw_routes()
    {
      if (!this.m_lib.setting.draw_sea_routes)
        return;
      float num = 1f * this.m_lib.loop_image.ImageScale;
      if ((double) num < 1.0)
        num = 1f;
      else if ((double) num > 2.0)
        num = 2f;
      if (this.m_is_select_mode)
        num *= 3f;
      this.m_lib.device.line.Width = num;
      this.m_lib.device.line.Antialias = this.m_lib.setting.enable_line_antialias;
      this.m_lib.device.line.Pattern = -1;
      this.m_lib.device.line.PatternScale = 1f;
      this.m_lib.device.device.RenderState.ZBufferEnable = false;
      this.m_lib.device.line.Begin();
      this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_routes_proc), 8f);
      this.m_lib.device.line.End();
      this.m_lib.device.device.RenderState.ZBufferEnable = true;
    }

    private void draw_routes_proc(Vector2 offset, LoopXImage image)
    {
      foreach (SeaRoutes.Voyage voyage in this.m_is_select_mode ? this.enum_all_sea_routes() : this.enum_sea_routes_without_trash())
        voyage.DrawRoutes(offset, image, this.m_is_select_mode);
    }

    private void draw_routes_bb_proc(Vector2 offset, LoopXImage image)
    {
      foreach (SeaRoutes.Voyage voyage in this.m_is_select_mode ? this.enum_all_sea_routes() : this.enum_sea_routes_without_trash())
        voyage.DrawRoutesBB(offset, image, this.m_is_select_mode);
    }

    private void draw_popups()
    {
      if (this.m_lib.setting.draw_popup_day_interval == 0)
        return;
      this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_popups_proc), 32f);
    }

    private void draw_popups_proc(Vector2 offset, LoopXImage image)
    {
      this.m_lib.device.sprites.BeginDrawSprites(this.m_lib.icons.texture, offset, image.ImageScale, new Vector2(1f, 1f));
      foreach (SeaRoutes.Voyage voyage in this.m_is_select_mode ? this.enum_all_sea_routes() : this.enum_sea_routes_without_trash())
        voyage.DrawPopups(offset, image, this.m_is_select_mode);
      this.m_lib.device.sprites.EndDrawSprites();
    }

    private void draw_accident()
    {
      if (!this.m_lib.setting.draw_accident)
        return;
      this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_accident_proc), 32f);
    }

    private void draw_accident_proc(Vector2 offset, LoopXImage image)
    {
      this.m_lib.device.sprites.BeginDrawSprites(this.m_lib.icons.texture, offset, image.ImageScale, new Vector2(1f, 1f));
      if (this.m_is_select_mode)
      {
        foreach (SeaRoutes.Voyage voyage in this.enum_all_sea_routes())
          voyage.DrawAccidents(offset, image, this.m_is_select_mode);
      }
      else if (this.m_lib.setting.draw_favorite_sea_routes_alpha_popup)
      {
        foreach (SeaRoutes.Voyage voyage in this.enum_sea_routes_without_trash())
          voyage.DrawAccidents(offset, image, this.m_is_select_mode);
      }
      else
      {
        foreach (SeaRoutes.Voyage voyage in this.m_sea_routes)
          voyage.DrawAccidents(offset, image, this.m_is_select_mode);
      }
      this.m_lib.device.sprites.EndDrawSprites();
    }

    private void init_add_points()
    {
      this.m_color_index = 0;
      this.m_old_days = 0;
      this.m_old_day_pos = new Point(0, 0);
      this.m_old_pos = new Point(0, 0);
      this.m_is_1st = true;
    }

    public void AddPoint(Point pos, int days, int accident)
    {
      if (days < 0 || pos.X < 0 || pos.Y < 0)
        return;
      Vector2 pos1 = transform.game_pos2_map_pos(transform.ToVector2(pos), this.m_lib.loop_image);
      if (this.m_is_1st)
      {
        this.add_sea_routes();
        this.m_old_days = days;
        this.m_old_pos = pos;
        if (days > 0)
          this.add_popup(new SeaRoutes.SeaRoutePopupPoint(pos1, this.m_color_index, days));
        this.add_point(new SeaRoutes.SeaRoutePoint(pos1, this.m_color_index));
        this.m_is_1st = false;
      }
      else
      {
        if (this.m_old_days != days)
        {
          if (this.m_old_days > days)
          {
            if (++this.m_color_index >= 8)
              this.m_color_index = 0;
            this.add_sea_routes();
          }
          if (days > 0)
            this.add_popup(new SeaRoutes.SeaRoutePopupPoint(pos1, this.m_color_index, days));
          this.m_old_days = days;
        }
        if (this.m_old_pos != pos && (double) new SeaRoutes.SeaRoutePoint(transform.ToVector2(this.m_old_pos)).LengthSq(new SeaRoutes.SeaRoutePoint(transform.ToVector2(pos)), (int) this.m_lib.loop_image.ImageSize.X) >= (double) (49f * transform.get_rate_to_game_x(this.m_lib.loop_image)))
        {
          this.add_point(new SeaRoutes.SeaRoutePoint(pos1, this.m_color_index));
          this.m_old_pos = pos;
        }
      }
      if (accident < 101 || accident > 111)
        return;
      this.add_accident(new SeaRoutes.SeaRoutePopupPoint(pos1, accident, days));
    }

    private void add_popup(SeaRoutes.SeaRoutePopupPoint p)
    {
      this.get_newest_sea_routes().AddPopup(p);
      this.m_req_redraw_list.Request();
    }

    private void add_accident(SeaRoutes.SeaRoutePopupPoint p)
    {
      this.get_newest_sea_routes().AddAccident(p);
      this.m_req_redraw_list.Request();
    }

    private void add_point(SeaRoutes.SeaRoutePoint p)
    {
      this.get_newest_sea_routes().AddPoint(p);
      this.m_req_redraw_list.Request();
    }

    public bool Write(string file_name)
    {
      return SeaRoutes.write(this.m_sea_routes, file_name);
    }

    public bool WriteFavorite(string file_name)
    {
      return SeaRoutes.write(this.m_favorite_sea_routes, file_name);
    }

    public bool WriteTrash(string file_name)
    {
      return SeaRoutes.write(this.m_trash_sea_routes, file_name);
    }

    private static bool write(List<SeaRoutes.Voyage> list, string file_name)
    {
      try
      {
        using (StreamWriter sw = new StreamWriter(file_name, false, Encoding.GetEncoding("Shift_JIS")))
        {
          foreach (SeaRoutes.Voyage voyage in list)
          {
            if (!voyage.IsEmpty)
            {
              sw.WriteLine("start routes");
              voyage.Write(sw);
            }
          }
        }
      }
      catch
      {
        return false;
      }
      return true;
    }

    private void remove_empty_routes()
    {
      while (true)
      {
        SeaRoutes.Voyage emptyRoutes = this.find_empty_routes();
        if (emptyRoutes != null)
          this.m_sea_routes.Remove(emptyRoutes);
        else
          break;
      }
    }

    private SeaRoutes.Voyage find_empty_routes()
    {
      foreach (SeaRoutes.Voyage voyage in this.m_sea_routes)
      {
        if (voyage.IsEmpty)
          return voyage;
      }
      return (SeaRoutes.Voyage) null;
    }

    public void CalcScreenShotBoundingBox(out Point offset, out Size size)
    {
      offset = new Point(0, 0);
      size = new Size((int) this.m_lib.loop_image.ImageSize.X, (int) this.m_lib.loop_image.ImageSize.Y);
      int length = (int) this.m_lib.loop_image.ImageSize.X / 64;
      if ((int) this.m_lib.loop_image.ImageSize.X % 64 != 0)
        ++length;
      List<Point>[] map = new List<Point>[length];
      for (int index = 0; index < length; ++index)
        map[index] = new List<Point>();
      int min_y = (int) this.m_lib.loop_image.ImageSize.Y;
      int max_y = 0;
      this.check_select_mode();
      foreach (SeaRoutes.Voyage voyage in this.m_is_select_mode ? this.enum_all_sea_routes() : this.enum_sea_routes_without_trash())
        voyage.SS_AddMinMaxList(map, ref min_y, ref max_y, this.m_is_select_mode);
      if (this.is_empty_list(map))
      {
        offset = new Point(0, 0);
        size = new Size(0, 0);
      }
      else
      {
        int start_index;
        int free_count;
        this.calc_bounding_box_x(map, out start_index, out free_count);
        int size_x;
        int offset_x;
        this.calc_screenshot_range(map, start_index, free_count, out size_x, out offset_x);
        size.Width = size_x;
        size.Width = size.Width + 1 & -2;
        offset.X = offset_x;
        size.Height = max_y - min_y;
        size.Height = size.Height + 1 & -2;
        offset.Y = min_y;
        offset.X -= 64;
        offset.Y -= 64;
        size.Width += 128;
        size.Height += 128;
      }
    }

    private void calc_screenshot_range(List<Point>[] map, int start_index_x, int free_count_x, out int size_x, out int offset_x)
    {
      if (free_count_x <= 0)
      {
        offset_x = (int) -((double) this.m_lib.loop_image.ImageSize.X / 2.0);
        size_x = (int) this.m_lib.loop_image.ImageSize.X;
      }
      else
      {
        int num = map.Length - free_count_x;
        int index1 = start_index_x + free_count_x;
        if (index1 >= map.Length)
          index1 -= map.Length;
        if (index1 < 0)
          index1 += map.Length;
        int index2 = index1 + num - 1;
        if (index2 >= map.Length)
          index2 -= map.Length;
        if (index2 < 0)
          index2 += map.Length;
        Point point1 = map[index1][0];
        foreach (Point point2 in map[index1])
        {
          if (point2.X < point1.X)
            point1 = point2;
        }
        Point point3 = map[index2][0];
        foreach (Point point2 in map[index2])
        {
          if (point2.X > point3.X)
            point3 = point2;
        }
        offset_x = point1.X;
        if (index2 < index1)
          size_x = point3.X + (int) this.m_lib.loop_image.ImageSize.X - point1.X;
        else
          size_x = point3.X - point1.X;
      }
    }

    private bool is_empty_list(List<Point>[] map)
    {
      int length = map.Length;
      for (int index = 0; index < length; ++index)
      {
        if (map[index].Count > 0)
          return false;
      }
      return true;
    }

    private void calc_bounding_box_x(List<Point>[] map, out int start_index, out int free_count)
    {
      int length = map.Length;
      int num1 = -1;
      int num2 = 0;
      for (int start = 0; start < length; ++start)
      {
        if (map[start].Count <= 0)
        {
          int num3 = this.calc_bounding_box_x_sub(map, start);
          if (num3 > num2)
          {
            num1 = start;
            num2 = num3;
          }
        }
      }
      start_index = num1;
      free_count = num2;
    }

    private int calc_bounding_box_x_sub(List<Point>[] map, int start)
    {
      int length = map.Length;
      int num1 = 0;
      int num2 = 0;
      for (; num1 < length; ++num1)
      {
        int index = start + num1;
        if (index >= length)
          index -= length;
        if (map[index].Count <= 0)
          ++num2;
        else
          break;
      }
      return num2;
    }

    public void ResetSelectFlag()
    {
      foreach (SeaRoutes.Voyage voyage in this.enum_all_sea_routes())
        voyage.IsSelected = false;
    }

    public void RemoveSeaRoutes(List<SeaRoutes.Voyage> remove_list)
    {
      if (this.is_newest_sea_routes(remove_list))
        this.init_add_points();
      this.remove_sea_routes(this.m_sea_routes, remove_list);
    }

    public void remove_sea_routes(List<SeaRoutes.Voyage> list, List<SeaRoutes.Voyage> remove_list)
    {
      foreach (SeaRoutes.Voyage voyage in remove_list)
      {
        try
        {
          list.Remove(voyage);
        }
        catch
        {
        }
      }
    }

    public void RemoveFavoriteSeaRoutes(List<SeaRoutes.Voyage> remove_list)
    {
      this.remove_sea_routes(this.m_favorite_sea_routes, remove_list);
    }

    public void RemoveTrashSeaRoutes(List<SeaRoutes.Voyage> remove_list)
    {
      this.remove_sea_routes(this.m_trash_sea_routes, remove_list);
    }

    private bool is_newest_sea_routes(List<SeaRoutes.Voyage> list)
    {
      SeaRoutes.Voyage newestSeaRoutes = this.get_newest_sea_routes();
      foreach (SeaRoutes.Voyage voyage in list)
      {
        if (voyage == newestSeaRoutes)
          return true;
      }
      return false;
    }

    public void MoveSeaRoutesToFavoriteSeaRoutes(List<SeaRoutes.Voyage> move_list)
    {
      this.add_sea_routes(this.m_sea_routes, this.m_favorite_sea_routes, move_list);
      this.RemoveSeaRoutes(move_list);
    }

    public void MoveSeaRoutesToTrashSeaRoutes(List<SeaRoutes.Voyage> move_list)
    {
      this.add_sea_routes(this.m_sea_routes, this.m_trash_sea_routes, move_list);
      this.RemoveSeaRoutes(move_list);
    }

    public void MoveFavoriteSeaRoutesToTrashSeaRoutes(List<SeaRoutes.Voyage> move_list)
    {
      this.add_sea_routes(this.m_favorite_sea_routes, this.m_trash_sea_routes, move_list);
      this.RemoveFavoriteSeaRoutes(move_list);
    }

    public void MoveTrashSeaRoutesToFavoriteSeaRoutes(List<SeaRoutes.Voyage> move_list)
    {
      this.add_sea_routes(this.m_trash_sea_routes, this.m_favorite_sea_routes, move_list);
      this.RemoveTrashSeaRoutes(move_list);
    }

    private void add_sea_routes(List<SeaRoutes.Voyage> from_list, List<SeaRoutes.Voyage> to_list, List<SeaRoutes.Voyage> move_list)
    {
      foreach (SeaRoutes.Voyage routes in move_list)
      {
        if (this.has_sea_routes(from_list, routes))
          to_list.Add(routes);
      }
    }

    private bool has_sea_routes(List<SeaRoutes.Voyage> list, SeaRoutes.Voyage routes)
    {
      foreach (SeaRoutes.Voyage voyage in list)
      {
        if (voyage == routes)
          return true;
      }
      return false;
    }

    public class SeaRoutePoint
    {
      private Vector2 m_pos;
      private int m_color_index;
      private int m_color;

      public Vector2 Position
      {
        get
        {
          return this.m_pos;
        }
        set
        {
          this.m_pos = value;
        }
      }

      public int ColorIndex
      {
        get
        {
          return this.m_color_index;
        }
      }

      public int Color
      {
        get
        {
          return this.m_color;
        }
      }

      public SeaRoutePoint(float x, float y, int color_index)
      {
        this.m_pos.X = x;
        this.m_pos.Y = y;
        this.m_color_index = color_index;
        this.m_color = DrawColor.GetColor(color_index);
      }

      public SeaRoutePoint(float x, float y)
      {
        this.m_pos.X = x;
        this.m_pos.Y = y;
        this.m_color_index = 0;
        this.m_color = DrawColor.GetColor(this.ColorIndex);
      }

      public SeaRoutePoint(Vector2 pos, int color_index)
      {
        this.m_pos = pos;
        this.m_color_index = color_index;
        this.m_color = DrawColor.GetColor(color_index);
      }

      public SeaRoutePoint(Vector2 pos)
      {
        this.m_pos = pos;
        this.m_color_index = 0;
        this.m_color = DrawColor.GetColor(this.ColorIndex);
      }

      public SeaRoutePoint(SeaRoutes.SeaRoutePoint p)
      {
        this.m_pos = p.Position;
        this.m_color_index = p.ColorIndex;
        this.m_color = p.Color;
      }

      public float LengthSq(SeaRoutes.SeaRoutePoint p)
      {
        return new Vector2(p.Position.X - this.m_pos.X, p.Position.Y - this.m_pos.Y).LengthSq();
      }

      public float Length(SeaRoutes.SeaRoutePoint p)
      {
        return new Vector2(p.Position.X - this.m_pos.X, p.Position.Y - this.m_pos.Y).Length();
      }

      public float LengthSq(SeaRoutes.SeaRoutePoint p, int size_x)
      {
        SeaRoutes.SeaRoutePoint near_p = (SeaRoutes.SeaRoutePoint) null;
        return this.LengthSq(p, size_x, ref near_p);
      }

      public float LengthSq(SeaRoutes.SeaRoutePoint p, int size_x, ref SeaRoutes.SeaRoutePoint near_p)
      {
        float num1 = this.LengthSq(p);
        SeaRoutes.SeaRoutePoint p1 = new SeaRoutes.SeaRoutePoint(p);
        p1.build_loop_x(size_x);
        SeaRoutes.SeaRoutePoint p2 = new SeaRoutes.SeaRoutePoint(p);
        p2.build_loop_x(-size_x);
        float num2 = this.LengthSq(p1);
        float num3 = this.LengthSq(p2);
        if ((double) num1 < (double) num2)
        {
          if ((double) num1 < (double) num3)
          {
            near_p = p;
            return num1;
          }
          else
          {
            near_p = p2;
            return num3;
          }
        }
        else if ((double) num2 < (double) num3)
        {
          near_p = p1;
          return num2;
        }
        else
        {
          near_p = p2;
          return num3;
        }
      }

      public float Length(SeaRoutes.SeaRoutePoint p, int size_x)
      {
        return (float) Math.Sqrt((double) this.LengthSq(p, size_x));
      }

      public float Length(SeaRoutes.SeaRoutePoint p, int size_x, ref SeaRoutes.SeaRoutePoint near_p)
      {
        SeaRoutes.SeaRoutePoint near_p1 = (SeaRoutes.SeaRoutePoint) null;
        return (float) Math.Sqrt((double) this.LengthSq(p, size_x, ref near_p1));
      }

      private void build_loop_x(int size_x)
      {
        this.m_pos.X += (float) size_x;
      }

      public Vector2 GetVector(SeaRoutes.SeaRoutePoint from)
      {
        return new Vector2(this.Position.X - from.Position.X, this.Position.Y - from.Position.Y);
      }

      public Vector2 GetVectorNormalized(SeaRoutes.SeaRoutePoint from)
      {
        Vector2 vector = this.GetVector(from);
        vector.Normalize();
        return vector;
      }

      public Vector2 GetVector(SeaRoutes.SeaRoutePoint from, int loop_x_size)
      {
        SeaRoutes.SeaRoutePoint near_p = (SeaRoutes.SeaRoutePoint) null;
        double num = (double) from.LengthSq(this, loop_x_size, ref near_p);
        return near_p.GetVector(from);
      }

      public Vector2 GetVectorNormalized(SeaRoutes.SeaRoutePoint from, int loop_x_size)
      {
        Vector2 vector = this.GetVector(from, loop_x_size);
        vector.Normalize();
        return vector;
      }
    }

    public class SeaRoutePopupPoint : SeaRoutes.SeaRoutePoint
    {
      private int m_days;

      public int Days
      {
        get
        {
          return this.m_days;
        }
      }

      public SeaRoutePopupPoint(float x, float y, int color_index, int days)
        : base(x, y, color_index)
      {
        this.m_days = days;
      }

      public SeaRoutePopupPoint(Vector2 pos, int color_index, int days)
        : this(pos.X, pos.Y, color_index, days)
      {
      }
    }

    public class SeaRoutePopupPointsBB : D3dBB2d
    {
      private List<SeaRoutes.SeaRoutePopupPoint> m_points;
      private int m_max_days;

      public List<SeaRoutes.SeaRoutePopupPoint> Points
      {
        get
        {
          return this.m_points;
        }
      }

      public int MaxDays
      {
        get
        {
          return this.m_max_days;
        }
      }

      public SeaRoutePopupPointsBB()
      {
        this.m_points = new List<SeaRoutes.SeaRoutePopupPoint>();
        this.OffsetLT = new Vector2(-32f, -32f);
        this.OffsetRB = new Vector2(32f, 32f);
        this.m_max_days = 0;
      }

      public bool Add(SeaRoutes.SeaRoutePopupPoint p)
      {
        Vector2 size = this.IfUpdate(p.Position).Size;
        if ((double) size.X > 350.0 || (double) size.Y > 350.0)
          return false;
        this.m_points.Add(p);
        this.Update(p.Position);
        foreach (SeaRoutes.SeaRoutePopupPoint seaRoutePopupPoint in this.m_points)
        {
          if (seaRoutePopupPoint.Days > this.m_max_days)
            this.m_max_days = seaRoutePopupPoint.Days;
        }
        return true;
      }
    }

    private class RouteLineBB : D3dBB2d
    {
      private List<Vector2> m_points;
      private int m_color;

      public int Color
      {
        get
        {
          return this.m_color;
        }
      }

      public int Count
      {
        get
        {
          return this.m_points.Count;
        }
      }

      public RouteLineBB(int color)
      {
        this.m_points = new List<Vector2>();
        this.m_color = color;
        this.OffsetLT = new Vector2(-2f, -2f);
        this.OffsetRB = new Vector2(2f, 2f);
      }

      public void AddPoint(Vector2 point)
      {
        this.m_points.Add(point);
        this.Update(point);
      }

      public bool IsCulling(Vector2 offset, LoopXImage image)
      {
        return base.IsCulling(offset, image.ImageScale, new D3dBB2d.CullingRect(image.Device.client_size));
      }

      public void DrawBB(Vector2 offset, LoopXImage image)
      {
        this.Draw(image.Device, 0.5f, offset, image.ImageScale, this.Color | -16777216);
      }

      public Vector2[] BuildLinePoints(Vector2 offset, LoopXImage image)
      {
        Vector2[] vector2Array = new Vector2[this.m_points.Count];
        int num = 0;
        foreach (Vector2 global_pos in this.m_points)
          vector2Array[num++] = image.GlobalPos2LocalPos(global_pos, offset);
        return vector2Array;
      }
    }

    public class Voyage
    {
      private gvt_lib m_lib;
      private List<SeaRoutes.SeaRoutePoint> m_routes;
      private List<SeaRoutes.SeaRoutePopupPointsBB> m_popups;
      private List<SeaRoutes.SeaRoutePopupPointsBB> m_accidents;
      private List<SeaRoutes.RouteLineBB> m_line_routes;
      private bool m_is_build_line_routes;
      private float m_alpha;
      private float m_gap_cos;
      private bool m_is_draw;
      private int m_max_days;
      private int m_minimum_draw_days;
      private DateTime m_date;
      private bool m_is_selected;

      public float Alpha
      {
        get
        {
          return this.m_alpha;
        }
        set
        {
          this.m_alpha = value;
          if ((double) this.m_alpha < 0.0)
            this.m_alpha = 0.0f;
          if ((double) this.m_alpha <= 1.0)
            return;
          this.m_alpha = 1f;
        }
      }

      public bool IsEmpty
      {
        get
        {
          return this.m_routes.Count <= 1 && this.m_popups.Count <= 1 && this.m_accidents.Count <= 1;
        }
      }

      public bool IsEnableDraw
      {
        get
        {
          return this.m_is_draw;
        }
        set
        {
          this.m_is_draw = value;
        }
      }

      public int MaxDays
      {
        get
        {
          return this.m_max_days;
        }
      }

      public string MaxDaysString
      {
        get
        {
          return string.Format("{0}日", (object) this.MaxDays);
        }
      }

      public int MinimumDrawDays
      {
        get
        {
          return this.m_minimum_draw_days;
        }
        set
        {
          this.m_minimum_draw_days = value;
        }
      }

      public Vector2 MapPoint1st
      {
        get
        {
          if (this.m_routes.Count > 0)
            return this.m_routes[0].Position;
          else
            return new Vector2(-1f, -1f);
        }
      }

      public string MapPoint1stString
      {
        get
        {
          return this.get_pos_str(this.MapPoint1st);
        }
      }

      public Point GamePoint1st
      {
        get
        {
          if (this.m_routes.Count > 0)
            return transform.ToPoint(transform.map_pos2_game_pos(this.m_routes[0].Position, this.m_lib.loop_image));
          else
            return new Point(-1, -1);
        }
      }

      public string GamePoint1stStr
      {
        get
        {
          return this.get_pos_str(this.GamePoint1st);
        }
      }

      public Vector2 MapPointLast
      {
        get
        {
          if (this.m_routes.Count > 0)
            return this.m_routes[this.m_routes.Count - 1].Position;
          else
            return new Vector2(-1f, -1f);
        }
      }

      public string MapPointLastString
      {
        get
        {
          return this.get_pos_str(this.MapPointLast);
        }
      }

      public Point GamePointLast
      {
        get
        {
          if (this.m_routes.Count > 0)
            return transform.ToPoint(transform.map_pos2_game_pos(this.m_routes[this.m_routes.Count - 1].Position, this.m_lib.loop_image));
          else
            return new Point(-1, -1);
        }
      }

      public string GamePointLastString
      {
        get
        {
          return this.get_pos_str(this.GamePointLast);
        }
      }

      public DateTime DateTime
      {
        get
        {
          return this.m_date;
        }
      }

      public string DateTimeString
      {
        get
        {
          if (this.m_date.Ticks == 0L)
            return "不明(旧デ\x30FCタ)";
          else
            return Useful.TojbbsDateTimeString(this.m_date);
        }
      }

      public bool IsSelected
      {
        get
        {
          return this.m_is_selected;
        }
        set
        {
          this.m_is_selected = value;
        }
      }

      public Voyage(gvt_lib lib)
      {
        this.m_lib = lib;
        this.m_popups = new List<SeaRoutes.SeaRoutePopupPointsBB>();
        this.m_accidents = new List<SeaRoutes.SeaRoutePopupPointsBB>();
        this.m_routes = new List<SeaRoutes.SeaRoutePoint>();
        this.m_line_routes = new List<SeaRoutes.RouteLineBB>();
        this.m_is_build_line_routes = false;
        this.Alpha = 1f;
        this.m_gap_cos = (float) Math.Cos((double) Useful.ToRadian(16f));
        this.m_is_draw = true;
        this.m_max_days = 0;
        this.m_minimum_draw_days = 0;
        this.m_is_selected = false;
        this.m_date = DateTime.Now;
      }

      public void AddPopup(SeaRoutes.SeaRoutePopupPoint p)
      {
        if (this.m_popups.Count <= 0)
          this.m_popups.Add(new SeaRoutes.SeaRoutePopupPointsBB());
        if (!this.m_popups[this.m_popups.Count - 1].Add(p))
        {
          this.m_popups.Add(new SeaRoutes.SeaRoutePopupPointsBB());
          this.m_popups[this.m_popups.Count - 1].Add(p);
        }
        foreach (SeaRoutes.SeaRoutePopupPointsBB routePopupPointsBb in this.m_popups)
        {
          if (routePopupPointsBb.MaxDays > this.m_max_days)
            this.m_max_days = routePopupPointsBb.MaxDays;
        }
      }

      public void AddAccident(SeaRoutes.SeaRoutePopupPoint p)
      {
        if (this.m_accidents.Count <= 0)
          this.m_accidents.Add(new SeaRoutes.SeaRoutePopupPointsBB());
        if (this.m_accidents[this.m_accidents.Count - 1].Add(p))
          return;
        this.m_accidents.Add(new SeaRoutes.SeaRoutePopupPointsBB());
        this.m_accidents[this.m_accidents.Count - 1].Add(p);
      }

      public void AddPoint(SeaRoutes.SeaRoutePoint p)
      {
        this.m_routes.Add(p);
        this.m_is_build_line_routes = false;
      }

      private void build_line_routes()
      {
        if (this.m_is_build_line_routes)
          return;
        this.m_line_routes.Clear();
        if (this.m_routes.Count < 2)
          return;
        SeaRoutes.SeaRoutePoint old_pos1 = this.m_routes[0];
        SeaRoutes.SeaRoutePoint old_pos2 = (SeaRoutes.SeaRoutePoint) null;
        SeaRoutes.RouteLineBB route = (SeaRoutes.RouteLineBB) null;
        for (int index = 1; index < this.m_routes.Count; ++index)
        {
          if (route == null)
          {
            route = new SeaRoutes.RouteLineBB(old_pos1.Color);
            route.AddPoint(old_pos1.Position);
          }
          SeaRoutes.SeaRoutePoint near_p = (SeaRoutes.SeaRoutePoint) null;
          float num = old_pos1.LengthSq(this.m_routes[index], (int) this.m_lib.loop_image.ImageSize.X, ref near_p);
          if ((double) num > 225.0)
            near_p = (double) num <= 4000000.0 ? this.check_point_sub(old_pos1, old_pos2, near_p) : (SeaRoutes.SeaRoutePoint) null;
          if (near_p != null)
            route.AddPoint(near_p.Position);
          if (near_p != this.m_routes[index] || (double) route.Size.X >= 250.0 || (double) route.Size.Y >= 250.0)
          {
            this.add_route(route);
            route = (SeaRoutes.RouteLineBB) null;
          }
          old_pos2 = old_pos1;
          old_pos1 = this.m_routes[index];
        }
        if (route != null)
          this.add_route(route);
        this.m_is_build_line_routes = true;
      }

      private SeaRoutes.SeaRoutePoint check_point_sub(SeaRoutes.SeaRoutePoint old_pos1, SeaRoutes.SeaRoutePoint old_pos2, SeaRoutes.SeaRoutePoint new_pos)
      {
        if (old_pos1 == null)
          return (SeaRoutes.SeaRoutePoint) null;
        if (old_pos2 == null)
          return (SeaRoutes.SeaRoutePoint) null;
        if ((double) Vector2.Dot(old_pos1.GetVectorNormalized(old_pos2, (int) this.m_lib.loop_image.ImageSize.X), new_pos.GetVectorNormalized(old_pos1, (int) this.m_lib.loop_image.ImageSize.X)) >= (double) this.m_gap_cos)
          return new_pos;
        else
          return (SeaRoutes.SeaRoutePoint) null;
      }

      private void add_route(SeaRoutes.RouteLineBB route)
      {
        if (route.Count < 2)
          return;
        this.m_line_routes.Add(route);
      }

      public void DrawRoutes(Vector2 offset, LoopXImage image, bool is_select_mode)
      {
        if (!this.can_draw(is_select_mode))
          return;
        this.build_line_routes();
        int num1 = (int) ((double) this.m_alpha * (double) byte.MaxValue);
        if (this.m_is_selected)
          num1 = (int) byte.MaxValue;
        int num2 = num1 << 24;
        foreach (SeaRoutes.RouteLineBB routeLineBb in this.m_line_routes)
        {
          if (!routeLineBb.IsCulling(offset, image))
            this.m_lib.device.line.Draw(routeLineBb.BuildLinePoints(offset, image), routeLineBb.Color | num2);
        }
      }

      private bool can_draw(bool is_select_mode)
      {
        return this.m_is_selected || (!is_select_mode || this.m_is_selected) && (this.m_is_draw && this.MaxDays >= this.MinimumDrawDays);
      }

      public void DrawRoutesBB(Vector2 offset, LoopXImage image, bool is_select_mode)
      {
        if (!this.can_draw(is_select_mode))
          return;
        foreach (SeaRoutes.RouteLineBB routeLineBb in this.m_line_routes)
          routeLineBb.DrawBB(offset, image);
      }

      public void DrawPopups(Vector2 offset, LoopXImage image, bool is_select_mode)
      {
        if (!this.can_draw(is_select_mode))
          return;
        float num1 = image.ImageScale;
        if ((double) num1 < 0.5)
          num1 = 0.5f;
        else if ((double) num1 > 1.0)
          num1 = 1f;
        float num2 = this.m_is_selected ? 1f : this.m_alpha;
        int num3 = (int) ((double) num2 * (double) byte.MaxValue) << 24;
        int num4 = (int) ((double) num2 * 64.0) << 24;
        int color1 = 16777215 | num3;
        int color2 = 16777215 | num4;
        D3dBB2d.CullingRect rect = new D3dBB2d.CullingRect(image.Device.client_size);
        foreach (SeaRoutes.SeaRoutePopupPointsBB routePopupPointsBb in this.m_popups)
        {
          if (!routePopupPointsBb.IsCulling(offset, image.ImageScale, rect))
          {
            foreach (SeaRoutes.SeaRoutePopupPoint seaRoutePopupPoint in routePopupPointsBb.Points)
            {
              if (seaRoutePopupPoint.ColorIndex >= 0 && seaRoutePopupPoint.ColorIndex < 8)
              {
                Vector3 pos = new Vector3(seaRoutePopupPoint.Position.X, seaRoutePopupPoint.Position.Y, 0.5f);
                if (seaRoutePopupPoint.Days % this.m_lib.setting.draw_popup_day_interval != 0)
                {
                  this.m_lib.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(icons.icon_index.days_mini_6), new Vector2(num1, num1), num3 | seaRoutePopupPoint.Color);
                }
                else
                {
                  pos.Z = 0.8f;
                  this.m_lib.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(icons.icon_index.days_big_shadow), new Vector2(1f, 1f), color2);
                  pos.Z = 0.5f;
                  icons.icon_index index1 = seaRoutePopupPoint.Days >= 100 ? icons.icon_index.days_big_100 : icons.icon_index.days_big_6;
                  if (this.m_lib.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(index1), num3 | seaRoutePopupPoint.Color))
                  {
                    int num5 = seaRoutePopupPoint.Days;
                    if (num5 > 999)
                      num5 = 999;
                    if (num5 < 0)
                      num5 = 0;
                    int num6 = 1;
                    Vector2 offset2 = new Vector2(0.0f, 0.0f);
                    if (num5 >= 100)
                    {
                      offset2.X += 7f;
                      num6 = 3;
                    }
                    else if (num5 >= 10)
                    {
                      offset2.X += 4f;
                      num6 = 2;
                    }
                    for (int index2 = 0; index2 < num6; ++index2)
                    {
                      this.m_lib.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon((icons.icon_index) (1 + num5 % 10)), color1, offset2);
                      offset2.X -= 7f;
                      num5 /= 10;
                    }
                  }
                }
              }
            }
          }
        }
      }

      public void DrawAccidents(Vector2 offset, LoopXImage image, bool is_select_mode)
      {
        if (!this.can_draw(is_select_mode))
          return;
        float num1 = this.m_is_selected ? 1f : this.m_alpha;
        int num2 = (int) ((double) num1 * (double) byte.MaxValue) << 24;
        int num3 = (int) ((double) num1 * 64.0) << 24;
        int color1 = 16777215 | num2;
        int color2 = 16777215 | num3;
        D3dBB2d.CullingRect rect = new D3dBB2d.CullingRect(image.Device.client_size);
        foreach (SeaRoutes.SeaRoutePopupPointsBB routePopupPointsBb in this.m_accidents)
        {
          if (!routePopupPointsBb.IsCulling(offset, image.ImageScale, rect))
          {
            foreach (SeaRoutes.SeaRoutePopupPoint seaRoutePopupPoint in routePopupPointsBb.Points)
            {
              if (seaRoutePopupPoint.ColorIndex >= 101 && seaRoutePopupPoint.ColorIndex <= 111 && this.is_draw_popups(seaRoutePopupPoint.ColorIndex))
              {
                Vector3 pos = new Vector3(seaRoutePopupPoint.Position.X, seaRoutePopupPoint.Position.Y, 0.5f);
                pos.Z = 0.8f;
                this.m_lib.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(icons.icon_index.accident_popup_shadow), new Vector2(1f, 1f), color2);
                pos.Z = 0.5f;
                this.m_lib.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon(icons.icon_index.accident_popup), color1);
                this.m_lib.device.sprites.AddDrawSpritesNC(pos, this.m_lib.icons.GetIcon((icons.icon_index) (136 + (seaRoutePopupPoint.ColorIndex - 101))), color1);
              }
            }
          }
        }
      }

      private bool is_draw_popups(int index)
      {
        DrawSettingAccidents settingAccidents = this.m_lib.setting.draw_setting_accidents;
        switch (index)
        {
          case 101:
            if ((settingAccidents & DrawSettingAccidents.accident_0) == (DrawSettingAccidents) 0)
              return false;
            else
              break;
          case 102:
            if ((settingAccidents & DrawSettingAccidents.accident_1) == (DrawSettingAccidents) 0)
              return false;
            else
              break;
          case 103:
            if ((settingAccidents & DrawSettingAccidents.accident_2) == (DrawSettingAccidents) 0)
              return false;
            else
              break;
          case 104:
            if ((settingAccidents & DrawSettingAccidents.accident_3) == (DrawSettingAccidents) 0)
              return false;
            else
              break;
          case 105:
            if ((settingAccidents & DrawSettingAccidents.accident_4) == (DrawSettingAccidents) 0)
              return false;
            else
              break;
          case 106:
            if ((settingAccidents & DrawSettingAccidents.accident_5) == (DrawSettingAccidents) 0)
              return false;
            else
              break;
          case 107:
            if ((settingAccidents & DrawSettingAccidents.accident_6) == (DrawSettingAccidents) 0)
              return false;
            else
              break;
          case 108:
            if ((settingAccidents & DrawSettingAccidents.accident_7) == (DrawSettingAccidents) 0)
              return false;
            else
              break;
          case 109:
            if ((settingAccidents & DrawSettingAccidents.accident_8) == (DrawSettingAccidents) 0)
              return false;
            else
              break;
          case 110:
            if ((settingAccidents & DrawSettingAccidents.accident_9) == (DrawSettingAccidents) 0)
              return false;
            else
              break;
          case 111:
            if ((settingAccidents & DrawSettingAccidents.accident_10) == (DrawSettingAccidents) 0)
              return false;
            else
              break;
        }
        return true;
      }

      public void StartLoad()
      {
        this.m_date = new DateTime();
      }

      public void LoadFromLine(string line)
      {
        try
        {
          string[] strArray = line.Split(new char[1]
          {
            ','
          });
          switch (strArray[0])
          {
            case "popup":
            case "accidents":
              int num1 = Convert.ToInt32(strArray[1]);
              int num2 = Convert.ToInt32(strArray[2]);
              int days = Convert.ToInt32(strArray[3]);
              int color_index = Convert.ToInt32(strArray[4]);
              if (color_index >= 101 && color_index <= 111)
              {
                this.AddAccident(new SeaRoutes.SeaRoutePopupPoint((float) num1, (float) num2, color_index, days));
                break;
              }
              else
              {
                this.AddPopup(new SeaRoutes.SeaRoutePopupPoint((float) num1, (float) num2, color_index, days));
                break;
              }
            case "routes":
              this.m_routes.Add(new SeaRoutes.SeaRoutePoint((float) Convert.ToDouble(strArray[1]), (float) Convert.ToDouble(strArray[2]), Convert.ToInt32(strArray[3])));
              break;
            case "draw":
              this.m_is_draw = Convert.ToInt32(strArray[1]) != 0;
              break;
            case "date":
              this.m_date = Useful.ToDateTime(strArray[1]);
              break;
          }
        }
        catch
        {
        }
      }

      public void Write(StreamWriter sw)
      {
        if (this.IsEnableDraw)
          sw.WriteLine("draw,1");
        else
          sw.WriteLine("draw,0");
        sw.WriteLine("date," + Useful.TojbbsDateTimeString(this.m_date));
        foreach (SeaRoutes.SeaRoutePopupPointsBB routePopupPointsBb in this.m_popups)
        {
          foreach (SeaRoutes.SeaRoutePopupPoint seaRoutePopupPoint in routePopupPointsBb.Points)
          {
            string str = "" + "popup," + ((int) seaRoutePopupPoint.Position.X).ToString() + "," + ((int) seaRoutePopupPoint.Position.Y).ToString() + "," + (object) seaRoutePopupPoint.Days + "," + (object) seaRoutePopupPoint.ColorIndex;
            sw.WriteLine(str);
          }
        }
        foreach (SeaRoutes.SeaRoutePopupPointsBB routePopupPointsBb in this.m_accidents)
        {
          foreach (SeaRoutes.SeaRoutePopupPoint seaRoutePopupPoint in routePopupPointsBb.Points)
          {
            string str = "" + "accidents," + ((int) seaRoutePopupPoint.Position.X).ToString() + "," + ((int) seaRoutePopupPoint.Position.Y).ToString() + "," + (object) seaRoutePopupPoint.Days + "," + (object) seaRoutePopupPoint.ColorIndex;
            sw.WriteLine(str);
          }
        }
        foreach (SeaRoutes.SeaRoutePoint seaRoutePoint in this.m_routes)
        {
          string str = "" + "routes," + (object) seaRoutePoint.Position.X + "," + (object) seaRoutePoint.Position.Y + "," + (object) seaRoutePoint.ColorIndex;
          sw.WriteLine(str);
        }
      }

      public int GetColorIndex()
      {
        if (this.m_routes.Count > 0)
          return this.m_routes[0].ColorIndex;
        if (this.m_popups.Count > 0 && this.m_popups[0].Points.Count > 0)
          return this.m_popups[0].Points[0].ColorIndex;
        else
          return 0;
      }

      public void Remove(bool popups, bool accident, bool routes)
      {
        if (popups)
          this.m_popups.Clear();
        if (accident)
          this.m_accidents.Clear();
        if (!routes)
          return;
        this.m_routes.Clear();
        this.m_line_routes.Clear();
      }

      public void SS_AddMinMaxList(List<Point>[] map, ref int min_y, ref int max_y, bool is_select_mode)
      {
        if (!this.can_draw(is_select_mode))
          return;
        foreach (SeaRoutes.SeaRoutePoint seaRoutePoint in this.m_routes)
          this.add_minmax_list(map, transform.ToPoint(seaRoutePoint.Position), ref min_y, ref max_y);
        foreach (SeaRoutes.SeaRoutePopupPointsBB routePopupPointsBb in this.m_popups)
        {
          foreach (SeaRoutes.SeaRoutePopupPoint seaRoutePopupPoint in routePopupPointsBb.Points)
            this.add_minmax_list(map, transform.ToPoint(seaRoutePopupPoint.Position), ref min_y, ref max_y);
        }
        foreach (SeaRoutes.SeaRoutePopupPointsBB routePopupPointsBb in this.m_accidents)
        {
          foreach (SeaRoutes.SeaRoutePopupPoint seaRoutePopupPoint in routePopupPointsBb.Points)
            this.add_minmax_list(map, transform.ToPoint(seaRoutePopupPoint.Position), ref min_y, ref max_y);
        }
      }

      private void add_minmax_list(List<Point>[] map, Point pos, ref int min_y, ref int max_y)
      {
        this.calc_bounding_box_y(pos.Y, ref min_y, ref max_y);
        int index = pos.X / 64;
        if (index < 0 || index >= map.Length)
          return;
        map[index].Add(pos);
      }

      private void calc_bounding_box_y(int y, ref int min, ref int max)
      {
        if (y <= 64)
          return;
        if (y > max)
          max = y;
        if (y >= min)
          return;
        min = y;
      }

      private string get_pos_str(Point pos)
      {
        if (pos.X < 0 || pos.Y < 0)
          return "不明な位置";
        else
          return string.Format("{0},{1}", (object) pos.X, (object) pos.Y);
      }

      private string get_pos_str(Vector2 pos)
      {
        if ((double) pos.X < 0.0 || (double) pos.Y < 0.0)
          return "不明な位置";
        else
          return string.Format("{0},{1}", (object) (int) pos.X, (object) (int) pos.Y);
      }
    }
  }
}
