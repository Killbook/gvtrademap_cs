// Type: gvtrademap_cs.speed_calculator
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using Utility;

namespace gvtrademap_cs
{
  public class speed_calculator
  {
    private const float KM_KNOT_RATE = 1.852f;
    private const float MAP_KNOT_RATE = 0.3029f;
    private const int LIST_INTERVAL_MAX = 120000;
    private const int CALC_INTERVAL_MAX = 20000;
    private const int CALC_ANGLE_MIN_INTERVAL = 20000;
    private const int CALC_ANGLE_STEP_INTERVAL = 5000;
    private const int CALC_ANGLE_MAX_INTERVAL = 60000;
    private const int CALC_ANGLE_MAX_PRECISION_COUNTS = 9;
    private const float ANGLE_GAP_MAX = 2f;
    private List<speed_calculator.data> m_length_list;
    private List<speed_calculator.data> m_angle_list;
    private int m_map_size_x;
    private float m_interval;
    private Point m_old_pos;
    private float m_speed;
    private float m_angle;
    private float m_angle_gap_cos;
    private float m_angle_precision;
    private bool m_req_reset_angle;

    public float speed_map
    {
      get
      {
        return this.m_speed;
      }
    }

    public float speed_knot
    {
      get
      {
        return speed_calculator.MapToKnotSpeed(this.m_speed);
      }
    }

    public float speed_km
    {
      get
      {
        return speed_calculator.MapToKmSpeed(this.m_speed);
      }
    }

    public float angle
    {
      get
      {
        return this.m_angle;
      }
    }

    public float angle_precision
    {
      get
      {
        return this.m_angle_precision;
      }
    }

    public speed_calculator(int map_size_x)
    {
      this.m_map_size_x = map_size_x;
      this.m_length_list = new List<speed_calculator.data>();
      this.m_angle_list = new List<speed_calculator.data>();
      this.m_interval = 0.0f;
      this.m_old_pos = new Point(0, 0);
      this.m_angle = -1f;
      this.m_angle_gap_cos = (float) Math.Cos((double) Useful.ToRadian(2f));
      this.m_angle_precision = 0.0f;
      this.m_req_reset_angle = false;
      this.m_speed = 0.0f;
    }

    public void Add(Point pos, int interval)
    {
      if (this.m_req_reset_angle)
      {
        this.m_angle_list.Clear();
        this.m_angle = -1f;
        this.m_angle_precision = 0.0f;
        this.m_req_reset_angle = false;
      }
      this.m_interval += (float) interval;
      if ((double) this.m_interval < 2000.0)
        return;
      float length = new SeaRoutes.SeaRoutePoint(transform.ToVector2(this.m_old_pos)).Length(new SeaRoutes.SeaRoutePoint(transform.ToVector2(pos)), this.m_map_size_x);
      if ((double) length < 100.0)
        this.m_length_list.Add(new speed_calculator.data(length, this.m_interval));
      this.m_angle_list.Add(new speed_calculator.data(pos, this.m_interval));
      this.ajust_list_size(this.m_length_list);
      this.ajust_list_size(this.m_angle_list);
      this.m_interval = 0.0f;
      this.m_old_pos = pos;
      this.clac_speed();
      this.calc_angle();
    }

    private void ajust_list_size(List<speed_calculator.data> list)
    {
      while (true)
      {
        float num = 0.0f;
        foreach (speed_calculator.data data in list)
          num += data.interval;
        if ((double) num > 120000.0)
          list.RemoveAt(0);
        else
          break;
      }
    }

    public void AddIntervalOnly(int interval)
    {
      this.m_interval += (float) interval;
    }

    private void clac_speed()
    {
      this.m_speed = this.calc_speed_sub(20000);
    }

    private float calc_speed_sub(int calc_interval)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      for (int index = this.m_length_list.Count - 1; index >= 0; --index)
      {
        num1 += this.m_length_list[index].length;
        num2 += this.m_length_list[index].interval;
        if ((double) num2 >= (double) calc_interval)
          break;
      }
      return num1 / (num2 / 1000f);
    }

    public static float MapToKnotSpeed(float speed_map)
    {
      float num = speed_map * 3.30142f;
      if ((double) num < 100.0)
        return num;
      else
        return 0.0f;
    }

    public static float KnotToMapSpeed(float knot)
    {
      return knot * 0.3029f;
    }

    public static float KnotToKmSpeed(float knot)
    {
      return knot * 1.852f;
    }

    public static float KmToKnotSpeed(float km)
    {
      return km * 0.5399568f;
    }

    public static float KmToMapSpeed(float km)
    {
      return speed_calculator.KnotToMapSpeed(speed_calculator.KmToKnotSpeed(km));
    }

    public static float MapToKmSpeed(float speed_map)
    {
      return speed_calculator.KnotToKmSpeed(speed_calculator.MapToKnotSpeed(speed_map));
    }

    private void calc_angle()
    {
      if (this.m_angle_list.Count <= 1)
        return;
      int count = this.m_angle_list.Count;
      Point position = this.m_angle_list[count - 1].position;
      float interval = this.m_angle_list[count - 1].interval;
      List<Point> list1 = new List<Point>();
      float num1 = 20000f;
      for (int index = count - 2; index >= 0; --index)
      {
        interval += this.m_angle_list[index].interval;
        if ((double) interval >= (double) num1)
        {
          list1.Add(this.m_angle_list[index].position);
          num1 += 5000f;
          if ((double) num1 > 60000.0)
            break;
        }
      }
      if (list1.Count <= 0)
        return;
      List<Vector2> list2 = new List<Vector2>();
      foreach (Point v1 in list1)
      {
        Vector2 vector2 = transform.SubVector_LoopX(position, v1, this.m_map_size_x);
        vector2.Normalize();
        if ((double) vector2.LengthSq() >= 0.699999988079071)
          list2.Add(vector2);
      }
      if (list2.Count <= 0)
      {
        this.m_angle = -1f;
        this.m_angle_precision = 0.0f;
      }
      else
      {
        Vector2 left = list2[0];
        int num2 = 1;
        if (list2.Count >= 2)
        {
          Vector2 vector2 = left;
          for (int index = 1; index < list2.Count && (double) Vector2.Dot(left, list2[index]) > (double) this.m_angle_gap_cos; ++index)
          {
            vector2 = list2[index];
            ++num2;
          }
          left = vector2;
        }
        this.m_angle_precision = (float) num2 / 9f;
        this.m_angle = Useful.ToDegree((float) Math.Atan2((double) left.Y, (double) left.X));
        this.m_angle += 90f;
        while ((double) this.m_angle < 0.0)
          this.m_angle += 360f;
        while ((double) this.m_angle >= 360.0)
          this.m_angle -= 360f;
      }
    }

    public void ResetAngle()
    {
      this.m_req_reset_angle = true;
    }

    private class data
    {
      private float m_length;
      private Point m_pos;
      private float m_interval;

      public float length
      {
        get
        {
          return this.m_length;
        }
      }

      public Point position
      {
        get
        {
          return this.m_pos;
        }
      }

      public float interval
      {
        get
        {
          return this.m_interval;
        }
      }

      public data(float length, float interval)
      {
        this.m_length = length;
        this.m_pos = new Point(0, 0);
        this.m_interval = interval;
      }

      public data(Point pos, float interval)
      {
        this.m_length = 0.0f;
        this.m_pos = pos;
        this.m_interval = interval;
      }
    }
  }
}
