// Type: gvtrademap_cs.myship_info
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using gvo_base;
using gvo_net_base;
using Microsoft.DirectX;
using System.Drawing;
using Utility;

namespace gvtrademap_cs
{
  public class myship_info
  {
    private const int FROM_CLIENT_RECEIVE_TIME_OUT = 5000;
    private const int OUT_OF_SEA_TIME_OUT = 5000;
    private const float STEP_POSITION_SPEED_MIN = 1f;
    private const float STEP_POSITION_SPEED_MIN1 = 2.8f;
    private const int STEP_POSITION_DAYS_MAX = 50;
    private const float ANGLE_LINE_LENGTH = 3000f;
    private gvt_lib m_lib;
    private GvoDatabase m_db;
    private Point m_pos;
    private float m_angle;
    private bool m_is_in_the_sea;
    private gvo_server_service m_server_service;
    private DateTimer m_capture_timer;
    private int m_capture_interval;
    private DateTimer m_expect_pos_timer;
    private DateTimer m_expect_delay_timer;
    private Vector2 m_expect_vector;
    private bool m_is_draw_expect_pos;
    private bool m_capture_sucess;

    public Point pos
    {
      get
      {
        return this.m_pos;
      }
    }

    public float angle
    {
      get
      {
        return this.m_angle;
      }
    }

    public gvo_server_service server_service
    {
      get
      {
        return this.m_server_service;
      }
    }

    public bool is_in_the_sea
    {
      get
      {
        return this.m_is_in_the_sea;
      }
    }

    public bool is_analized_pos
    {
      get
      {
        return this.m_pos.X >= 0 && this.m_pos.Y >= 0;
      }
    }

    private bool is_draw_expect_pos
    {
      get
      {
        return this.m_is_draw_expect_pos && !(this.m_expect_vector == Vector2.Empty);
      }
    }

    public bool capture_sucess
    {
      get
      {
        return this.m_capture_sucess;
      }
    }

    public myship_info(gvt_lib lib, GvoDatabase db)
    {
      this.m_lib = lib;
      this.m_db = db;
      this.m_pos = new Point(-1, -1);
      this.m_angle = -1f;
      this.m_is_in_the_sea = false;
      this.m_server_service = new gvo_server_service();
      this.m_capture_timer = new DateTimer();
      this.m_expect_pos_timer = new DateTimer();
      this.m_expect_delay_timer = new DateTimer();
      this.m_capture_sucess = false;
      this.reset_expect();
    }

    private void reset_expect()
    {
      this.m_expect_vector = Vector2.Empty;
      this.m_is_draw_expect_pos = false;
      this.m_expect_pos_timer.StartSection();
    }

    public void Update()
    {
      this.m_capture_sucess = false;
      this.update_server();
      this.update_capture_interval();
      if (this.m_lib.setting.is_server_mode)
      {
        this.do_receive_client();
      }
      else
      {
        this.do_capture();
        this.m_db.GvoChat.UpdateSeaArea_DoRequest();
      }
    }

    private void update_server()
    {
      if (this.m_lib.setting.is_server_mode)
      {
        if (this.m_server_service.is_listening)
          return;
        this.m_server_service.Listen(this.m_lib.setting.port_index);
      }
      else
        this.m_server_service.Close();
    }

    private void update_capture_interval()
    {
      switch (this.m_lib.setting.capture_interval)
      {
        case CaptureIntervalIndex.Per500ms:
          this.m_capture_interval = 500;
          break;
        case CaptureIntervalIndex.Per2000ms:
          this.m_capture_interval = 2000;
          break;
        case CaptureIntervalIndex.Per250ms:
          this.m_capture_interval = 250;
          break;
        default:
          this.m_capture_interval = 1000;
          break;
      }
    }

    private void do_capture()
    {
      int timeMilliseconds = this.m_capture_timer.GetSectionTimeMilliseconds();
      if (timeMilliseconds < this.m_capture_interval)
        return;
      this.m_capture_timer.StartSection();
      this.update_myship_data(timeMilliseconds, this.get_myship_data());
    }

    private gvo_analized_data get_myship_data()
    {
      if (!this.m_lib.setting.save_searoutes)
        return (gvo_analized_data) null;
      if (this.m_lib.setting.windows_vista_aero)
        this.m_db.Capture.capture_mode = gvo_capture_base.mode.vista;
      else
        this.m_db.Capture.capture_mode = gvo_capture_base.mode.xp;
      this.m_db.Capture.CaptureAll();
      return gvo_analized_data.FromAnalizedData((gvo_capture_base) this.m_db.Capture, (gvo_map_cs_chat_base) this.m_db.GvoChat);
    }

    private void do_receive_client()
    {
      gvo_analized_data receivedCaptureData = this.get_received_capture_data();
      if (receivedCaptureData == null)
      {
        if (this.m_capture_timer.GetSectionTimeMilliseconds() < 5000)
          return;
        this.m_is_in_the_sea = false;
        this.reset_expect();
      }
      else
      {
        int timeMilliseconds = this.m_capture_timer.GetSectionTimeMilliseconds();
        this.m_capture_timer.StartSection();
        this.update_myship_data(timeMilliseconds, receivedCaptureData);
      }
    }

    private gvo_analized_data get_received_capture_data()
    {
      gvo_tcp_client client = this.m_server_service.GetClient();
      if (client == null)
        return (gvo_analized_data) null;
      gvo_map_cs_chat_base.sea_area_type[] seaInfo = client.sea_info;
      if (seaInfo != null)
      {
        foreach (gvo_map_cs_chat_base.sea_area_type seaAreaType in seaInfo)
        {
          switch (seaAreaType.type)
          {
            case gvo_map_cs_chat_base.sea_type.safty:
              this.m_db.SeaArea.SetType(seaAreaType.name, sea_area.sea_area_once.sea_type.safty);
              break;
            case gvo_map_cs_chat_base.sea_type.lawless:
              this.m_db.SeaArea.SetType(seaAreaType.name, sea_area.sea_area_once.sea_type.lawless);
              break;
            default:
              this.m_db.SeaArea.SetType(seaAreaType.name, sea_area.sea_area_once.sea_type.normal);
              break;
          }
        }
      }
      gvo_analized_data captureData = client.capture_data;
      if (captureData.capture_days_success || captureData.capture_success)
        return captureData;
      else
        return (gvo_analized_data) null;
    }

    private void update_myship_data(int sectiontime, gvo_analized_data data)
    {
      if (data == null)
        return;
      if (!this.m_lib.setting.save_searoutes)
      {
        this.m_pos = new Point(-1, -1);
        this.m_is_in_the_sea = false;
        this.reset_expect();
      }
      else
      {
        this.m_db.SpeedCalculator.AddIntervalOnly(sectiontime);
        if (data.is_start_build_ship)
          this.m_db.BuildShipCounter.StartBuildShip(data.build_ship_name);
        if (data.is_finish_build_ship)
          this.m_db.BuildShipCounter.FinishBuildShip();
        if (data.capture_days_success)
        {
          this.m_db.InterestDays.Update(data.days, data.interest);
          this.m_db.BuildShipCounter.Update(data.days);
          if (!data.capture_success)
          {
            this.m_is_draw_expect_pos = true;
            this.m_expect_delay_timer.StartSection();
          }
          else
          {
            this.m_pos = new Point(data.pos_x, data.pos_y);
            this.m_angle = data.angle;
            this.m_is_in_the_sea = true;
            this.m_capture_sucess = true;
            this.m_db.SeaRoute.AddPoint(this.m_pos, data.days, gvo_chat.ToIndex(data.accident));
            this.m_db.SpeedCalculator.Add(this.m_pos, 0);
            this.m_lib.device.SetMustDrawFlag();
            this.m_expect_vector = transform.ToVector2(gvo_capture_base.AngleToVector(this.m_angle));
            this.m_expect_pos_timer.StartSection();
            this.m_expect_delay_timer.StartSection();
            this.m_is_draw_expect_pos = false;
          }
        }
        else
        {
          if (this.m_expect_delay_timer.GetSectionTimeMilliseconds() <= 5000)
            return;
          this.m_is_in_the_sea = false;
          this.reset_expect();
        }
      }
    }

    public void Draw()
    {
      if (!this.is_analized_pos)
        return;
      this.m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(this.draw_myship_proc), 3000f);
    }

    private void draw_myship_proc(Vector2 offset, LoopXImage image)
    {
      Vector2 pos = image.GlobalPos2LocalPos(transform.game_pos2_map_pos(transform.ToVector2(this.m_pos), this.m_lib.loop_image), offset);
      this.draw_angle_line_all(pos, image);
      this.m_lib.device.sprites.BeginDrawSprites(this.m_lib.icons.texture);
      int color = -1;
      if (this.m_lib.setting.draw_setting_myship_expect_pos && this.draw_expect_pos(pos, this.m_db.SpeedCalculator.speed_map))
        color = Color.FromArgb(160, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue).ToArgb();
      this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3(pos.X, pos.Y, 0.301f), this.m_lib.icons.GetIcon(icons.icon_index.myship), color);
      this.m_lib.device.sprites.EndDrawSprites();
    }

    private void draw_angle_line_all(Vector2 pos, LoopXImage image)
    {
      if (!this.m_lib.setting.draw_myship_angle)
        return;
      DrawSettingMyShipAngle settingMyshipAngle = this.m_lib.setting.draw_setting_myship_angle;
      if ((settingMyshipAngle & DrawSettingMyShipAngle.draw_1) != (DrawSettingMyShipAngle) 0)
      {
        int alpha = (int) ((double) byte.MaxValue * ((double) this.m_db.SpeedCalculator.angle_precision * (double) this.m_db.SpeedCalculator.angle_precision));
        this.draw_angle_line(pos, image, this.m_db.SpeedCalculator.angle, Color.FromArgb(alpha, 0, 0, (int) byte.MaxValue));
      }
      if (!this.is_in_the_sea || (settingMyshipAngle & DrawSettingMyShipAngle.draw_0) == (DrawSettingMyShipAngle) 0)
        return;
      Color color = this.is_draw_expect_pos ? Color.Tomato : Color.Black;
      this.draw_angle_line(pos, image, this.m_angle, color);
      if (!this.m_lib.setting.draw_setting_myship_angle_with_speed_pos)
        return;
      this.draw_step_position2(pos, image, this.m_angle, this.m_db.SpeedCalculator.speed_map);
    }

    private void draw_step_position2(Vector2 pos, LoopXImage image, float angle, float speed)
    {
      if ((double) angle < 0.0)
        return;
      float num1 = speed_calculator.MapToKnotSpeed(speed);
      if ((double) num1 < 1.0)
        return;
      speed = this.transform_speed(speed, image);
      this.m_lib.device.device.RenderState.ZBufferEnable = false;
      this.m_lib.device.line.Width = 1f;
      this.m_lib.device.line.Antialias = this.m_lib.setting.enable_line_antialias;
      this.m_lib.device.line.Pattern = -1;
      this.m_lib.device.line.PatternScale = 1f;
      this.m_lib.device.line.Begin();
      float num2 = 0.0f;
      int num3 = 1;
      int num4 = 0;
      int num5 = 0;
      Vector2 vector2_1 = transform.ToVector2(gvo_capture_base.AngleToVector(angle));
      Vector2 vector2_2 = new Vector2(vector2_1.Y, -vector2_1.X);
      Vector2[] vertexList = new Vector2[3];
      float num6 = 3000f * image.ImageScale;
      while ((double) num2 < (double) num6)
      {
        Vector2 vector2_3 = vector2_1 * (speed * (float) num3);
        Vector2 vector2_4 = vector2_1 * (float) ((double) speed * (double) num3 - 4.5);
        Color black = Color.Black;
        float num7 = 2.5f;
        if (++num4 >= 5)
        {
          Color color;
          float num8;
          if (++num5 >= 2)
          {
            color = Color.Red;
            num5 = 0;
            num8 = 4f;
          }
          else
          {
            color = Color.Tomato;
            num8 = 3f;
          }
          num4 = 0;
          vertexList[0] = pos + vector2_4 + vector2_2 * num8;
          vertexList[1] = pos + vector2_3;
          vertexList[2] = pos + vector2_4 + vector2_2 * -num8;
          this.m_lib.device.line.Draw(vertexList, color);
        }
        else if ((double) num1 > 2.79999995231628)
        {
          vertexList[0] = pos + vector2_4 + vector2_2 * num7;
          vertexList[1] = pos + vector2_3;
          vertexList[2] = pos + vector2_4 + vector2_2 * -num7;
          this.m_lib.device.line.Draw(vertexList, black);
        }
        num2 += speed;
        if (++num3 > 50)
          break;
      }
      this.m_lib.device.line.End();
      this.m_lib.device.device.RenderState.ZBufferEnable = true;
    }

    private float transform_speed(float speed, LoopXImage image)
    {
      speed = this.update_step_position_speed(speed);
      speed *= transform.get_rate_to_map_x(image);
      speed *= image.ImageScale;
      return speed;
    }

    private float update_step_position_speed(float speed_map)
    {
      speed_map *= 60f;
      return speed_map;
    }

    private void draw_angle_line(Vector2 pos, LoopXImage image, float angle, Color color)
    {
      if ((double) angle < 0.0)
        return;
      this.m_lib.device.device.RenderState.ZBufferEnable = false;
      this.m_lib.device.line.Width = 1f;
      this.m_lib.device.line.Antialias = this.m_lib.setting.enable_line_antialias;
      this.m_lib.device.line.Pattern = -1;
      this.m_lib.device.line.PatternScale = 1f;
      this.m_lib.device.line.Begin();
      Vector2 vector2 = transform.ToVector2(gvo_capture_base.AngleToVector(angle));
      this.m_lib.device.line.Draw(new Vector2[2]
      {
        pos,
        pos + vector2 * (3000f * image.ImageScale)
      }, color);
      this.m_lib.device.line.End();
      this.m_lib.device.device.RenderState.ZBufferEnable = true;
    }

    private bool draw_expect_pos(Vector2 pos, float speed)
    {
      if (!this.is_draw_expect_pos || (double) speed_calculator.MapToKnotSpeed(speed) < 1.0)
        return false;
      int timeMilliseconds = this.m_expect_pos_timer.GetSectionTimeMilliseconds();
      speed = this.transform_speed(speed, this.m_lib.loop_image);
      speed *= 0.01666667f;
      speed *= 1.0 / 1000.0;
      speed *= (float) timeMilliseconds;
      pos += this.m_expect_vector * speed;
      this.m_lib.device.sprites.AddDrawSpritesNC(new Vector3(pos.X, pos.Y, 0.3f), this.m_lib.icons.GetIcon(icons.icon_index.myship));
      return true;
    }
  }
}
