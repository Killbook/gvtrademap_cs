// Type: gvo_base.gvo_capture_base
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Drawing;
using Utility;
using win32;

namespace gvo_base
{
  public class gvo_capture_base : IDisposable
  {
    protected const int COMPASS_DIV = 360;
    protected const int COMPASS_1ST_STEP = 2;
    protected const int COMPASS_2ND_RANGE = 8;
    protected const int COMPASS_DIV_90 = 90;
    protected const int COMPASS_DIV_45 = 45;
    private gvo_capture_base.mode m_capture_mode;
    private ScreenCapture m_capture1;
    private ScreenCapture m_capture2;
    private Point m_point;
    private int m_days;
    private float m_angle;
    protected Point[] m_compass_pos;
    protected float[] m_ajust_compass;
    protected int m_1st_com_index;
    protected float m_com_index;
    protected float m_com_index2;
    protected int m_an_index;
    private bool m_enable_point;
    private bool m_enable_days;
    private bool m_enable_angle;

    public Point point
    {
      get
      {
        return this.m_point;
      }
    }

    public int days
    {
      get
      {
        return this.m_days;
      }
    }

    public float angle
    {
      get
      {
        return this.m_angle;
      }
    }

    public bool capture_point_success
    {
      get
      {
        return this.point.X >= 0 && this.point.Y >= 0;
      }
    }

    public bool capture_days_success
    {
      get
      {
        return this.days >= 0;
      }
    }

    public bool capture_success
    {
      get
      {
        if (!this.capture_days_success)
          return false;
        else
          return this.capture_point_success;
      }
    }

    public gvo_capture_base.mode capture_mode
    {
      get
      {
        return this.m_capture_mode;
      }
      set
      {
        this.m_capture_mode = value;
      }
    }

    protected ScreenCapture capture1
    {
      get
      {
        return this.m_capture1;
      }
    }

    protected ScreenCapture capture2
    {
      get
      {
        return this.m_capture2;
      }
    }

    protected bool enable_point
    {
      get
      {
        return this.m_enable_point;
      }
      set
      {
        this.m_enable_point = value;
      }
    }

    protected bool enable_days
    {
      get
      {
        return this.m_enable_days;
      }
      set
      {
        this.m_enable_days = value;
      }
    }

    protected bool enable_angle
    {
      get
      {
        return this.m_enable_angle;
      }
      set
      {
        this.m_enable_angle = value;
      }
    }

    public gvo_capture_base()
    {
      this.m_capture1 = this.CreateScreenCapture(64, 24);
      this.m_capture2 = this.CreateScreenCapture(128, 128);
      this.m_enable_point = true;
      this.m_enable_days = true;
      this.m_enable_angle = true;
      this.m_point = new Point(-1, -1);
      this.m_days = -1;
      this.m_angle = -1f;
      this.capture_mode = gvo_capture_base.mode.xp;
      this.create_compass_tbl();
      this.create_ajust_compass_tbl();
    }

    protected virtual ScreenCapture CreateScreenCapture(int size_x, int size_y)
    {
      return new ScreenCapture(size_x, size_y);
    }

    public virtual void Dispose()
    {
    }

    public static bool IsFoundGvoWindow()
    {
      return !(gvo_capture_base.FindGvoWindow() == IntPtr.Zero);
    }

    public static IntPtr FindGvoWindow()
    {
      return user32.FindWindowA("Greate Voyages Online Game MainFrame", "大航海時代 Online");
    }

    public static IntPtr FindGvoWindow(out Rectangle rect)
    {
      rect = new Rectangle();
      IntPtr gvoWindow = gvo_capture_base.FindGvoWindow();
      if (gvoWindow == IntPtr.Zero)
        return IntPtr.Zero;
      Point p = new Point();
      user32.GetClientRect(gvoWindow, ref rect);
      user32.ClientToScreen(gvoWindow, ref p);
      if (rect.Width <= 0 || rect.Height <= 0)
        return IntPtr.Zero;
      rect.X = p.X;
      rect.Y = p.Y;
      return gvoWindow;
    }

    public virtual bool CaptureAll()
    {
      if (!this.capture_dol_window())
      {
        this.m_point = new Point(-1, -1);
        this.m_days = -1;
        this.m_angle = -1f;
        return false;
      }
      else
      {
        if (this.m_enable_point || this.m_enable_days)
          this.m_capture1.CreateImage();
        if (this.m_enable_angle)
          this.m_capture2.CreateImage();
        if (this.m_enable_days)
          this.analize_days();
        if (this.m_enable_point)
          this.analize_point();
        if (this.m_enable_angle)
          this.analize_angle();
        return true;
      }
    }

    private bool capture_dol_window()
    {
      Rectangle rect;
      IntPtr hWnd = gvo_capture_base.FindGvoWindow(out rect);
      if (hWnd == IntPtr.Zero)
        return false;
      if (this.capture_mode == gvo_capture_base.mode.xp)
      {
        rect.X = 0;
        rect.Y = 0;
      }
      else
        hWnd = IntPtr.Zero;
      IntPtr dc = user32.GetDC(hWnd);
      if (dc == IntPtr.Zero)
        return false;
      this.DoCapture(dc, rect);
      user32.ReleaseDC(hWnd, dc);
      return true;
    }

    protected virtual void DoCapture(IntPtr hdc, Rectangle rect)
    {
      if (this.m_enable_point)
        this.m_capture1.DoCapture(hdc, new Point(0, 0), new Point(rect.X + (rect.Width - 72), rect.Y + (rect.Height - 272)), new Size(60, 11));
      if (this.m_enable_days)
        this.m_capture1.DoCapture(hdc, new Point(0, 12), new Point(rect.X + 14, rect.Y + 19), new Size(21, 11));
      if (!this.m_enable_angle)
        return;
      this.m_capture2.DoCapture(hdc, new Point(0, 0), new Point(rect.X + (rect.Width - 148), rect.Y + (rect.Height - 108)), new Size(113, 107));
    }

    private bool analize_number(int index, out int num)
    {
      string[] strArray = new string[11]
      {
        "011111110100000001100000001011111110",
        "000000000010000000111111111000000000",
        "011000011100001101100010001011100001",
        "011000110100010001100010001011101110",
        "000001100000110100011000100111111111",
        "111110110100100001100100001100011110",
        "011111110100010001100010001011001110",
        "100000000100000111100111000111000000",
        "011101110100010001100010001011101110",
        "011100110100010001100010001011111110",
        "000000010000000010000000011000000000"
      };
      byte[] image = this.m_capture1.Image;
      int stride = this.m_capture1.Stride;
      int num1 = index * 6 * 3;
      string str = "";
      for (int index1 = 1; index1 < 5; ++index1)
      {
        int num2 = 0;
        for (int index2 = 0; index2 < 9; ++index2)
        {
          int index3 = num2 + num1 + index1 * 3;
          str = (int) image[index3] + (int) image[index3 + 1] + (int) image[index3 + 2] <= 720 ? str + (object) '0' : str + (object) '1';
          num2 += stride;
        }
      }
      num = 0;
      for (int index1 = 0; index1 < 10; ++index1)
      {
        if (str == strArray[index1])
        {
          num = index1;
          return true;
        }
      }
      return false;
    }

    private void analize_point()
    {
      bool flag1 = false;
      this.m_point.X = 0;
      for (int index = 0; index < 5; ++index)
      {
        int num;
        if (this.analize_number(index, out num))
        {
          flag1 = true;
          this.m_point.X *= 10;
          this.m_point.X += num;
        }
        else if (flag1)
        {
          flag1 = false;
          break;
        }
      }
      if (!flag1)
      {
        this.m_point = new Point(-1, -1);
      }
      else
      {
        bool flag2 = false;
        this.m_point.Y = 0;
        for (int index = 6; index < 10; ++index)
        {
          int num;
          if (this.analize_number(index, out num))
          {
            flag2 = true;
            this.m_point.Y *= 10;
            this.m_point.Y += num;
          }
          else if (flag2)
            break;
        }
        if (flag2)
          return;
        this.m_point = new Point(-1, -1);
      }
    }

    private void analize_days()
    {
      string[] strArray = new string[10]
      {
        "111110000001011100",
        "100001111111000000",
        "110011001011110011",
        "110011001001110111",
        "001000011111000001",
        "101011101001000110",
        "111111001001100110",
        "100011101110100000",
        "110110001001110110",
        "111011000001011110"
      };
      byte[] image = this.m_capture1.Image;
      int stride = this.m_capture1.Stride;
      int index1 = stride * 16 + 30;
      int num1 = (int) image[index1] + (int) image[index1 + 1] + (int) image[index1 + 2];
      int index2 = stride * 22 + 30;
      int num2 = (int) image[index2] + (int) image[index2 + 1] + (int) image[index2 + 2];
      int num3;
      int num4;
      if (num1 > 384 || num2 > 384)
      {
        int index3 = stride * 16 + 6;
        int num5 = (int) image[index3] + (int) image[index3 + 1] + (int) image[index3 + 2];
        int index4 = stride * 22 + 6;
        int num6 = (int) image[index4] + (int) image[index4 + 1] + (int) image[index4 + 2];
        if (num5 > 384 || num6 > 384)
        {
          num3 = 3;
          num4 = 0;
        }
        else
        {
          num3 = 1;
          num4 = 8;
        }
      }
      else
      {
        num3 = 2;
        num4 = 4;
      }
      this.m_days = 0;
      for (int index3 = 0; index3 < num3; ++index3)
      {
        string str = "";
        int num5 = (num4 + index3 * 8) * 3 + 12 * stride;
        int num6 = 0;
        while (num6 < 6)
        {
          int num7 = 0;
          int num8 = 0;
          while (num8 < 12)
          {
            int index4 = num7 + num5 + num6 * 3;
            str = (int) image[index4] + (int) image[index4 + 1] + (int) image[index4 + 2] <= 384 ? str + (object) '0' : str + (object) '1';
            num7 += stride * 2;
            num8 += 2;
          }
          num6 += 2;
        }
        bool flag = false;
        for (int index4 = 0; index4 < 10; ++index4)
        {
          if (str == strArray[index4])
          {
            this.m_days *= 10;
            this.m_days += index4;
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          this.m_days = -1;
          break;
        }
      }
    }

    private void analize_angle()
    {
      this.m_angle = -1f;
      if (!this.capture_success)
        return;
      int num1 = this.analize_angle_1st_step();
      if (num1 < 0)
        return;
      this.m_1st_com_index = num1;
      float angle_index;
      float num2;
      if (num1 > 272)
      {
        angle_index = this.analize_angle_2nd_step2(num1 + 180) - 180f;
        num2 = this.analize_angle_2nd_step2(num1 - 90) + 90f;
        this.m_an_index = 0;
      }
      else if (num1 <= 90)
      {
        angle_index = this.analize_angle_2nd_step2(num1 + 180) - 180f;
        num2 = this.analize_angle_2nd_step2(num1 + 90) - 90f;
        this.m_an_index = 1;
      }
      else if (num1 > 90 && num1 <= 180)
      {
        angle_index = this.analize_angle_2nd_step2(num1 + 90) - 90f;
        num2 = this.analize_angle_2nd_step2(num1 - 90) + 90f;
        this.m_an_index = 2;
      }
      else
      {
        angle_index = this.analize_angle_2nd_step2(num1 - 90) + 90f;
        num2 = this.analize_angle_2nd_step2(num1 + 90) - 90f;
        this.m_an_index = 3;
      }
      this.m_com_index = angle_index;
      this.m_com_index2 = num2;
      this.m_angle = this.update_angle_with_ajust(angle_index);
    }

    private int analize_angle_1st_step()
    {
      int i = 0;
      while (i < 360)
      {
        if (this.analize_angle_sub(i))
          return i;
        i += 2;
      }
      return -1;
    }

    private float analize_angle_2nd_step2(int index)
    {
      int num1 = index;
      for (int index1 = 0; index1 < 8; ++index1)
      {
        int i = index - (index1 + 1);
        if (this.analize_angle_sub2(i))
          num1 = i;
      }
      int num2 = index;
      for (int index1 = 0; index1 < 8; ++index1)
      {
        int i = index + (index1 + 1);
        if (this.analize_angle_sub2(i))
          num2 = i;
      }
      return (float) (num1 + num2) * 0.5f;
    }

    private bool analize_angle_sub(int i)
    {
      byte[] image = this.m_capture2.Image;
      int stride = this.m_capture2.Stride;
      if (i >= 360)
        i -= 360;
      if (i < 0)
        i += 360;
      int index = stride * this.m_compass_pos[i].Y + this.m_compass_pos[i].X * 3;
      int num1 = (int) image[index];
      int num2 = (int) image[index + 1];
      if ((int) image[index + 2] <= num1 + 10 || num2 <= num1 + 10)
        return false;
      image[index] = (byte) 0;
      image[index + 1] = (byte) 0;
      image[index + 2] = byte.MaxValue;
      return true;
    }

    private bool analize_angle_sub2(int i)
    {
      byte[] image = this.m_capture2.Image;
      int stride = this.m_capture2.Stride;
      if (i >= 360)
        i -= 360;
      if (i < 0)
        i += 360;
      int index = stride * this.m_compass_pos[i].Y + this.m_compass_pos[i].X * 3;
      int num1 = (int) image[index];
      int num2 = (int) image[index + 1];
      int num3 = (int) image[index + 2];
      if (num3 < 137 || num3 != num2 || num2 != num1)
        return false;
      image[index] = (byte) 0;
      image[index + 1] = (byte) 0;
      image[index + 2] = byte.MaxValue;
      return true;
    }

    private void create_compass_tbl()
    {
      this.m_compass_pos = new Point[360]
      {
        new Point(56, 1),
        new Point(56, 1),
        new Point(57, 1),
        new Point(58, 1),
        new Point(59, 1),
        new Point(59, 1),
        new Point(60, 1),
        new Point(61, 1),
        new Point(62, 1),
        new Point(63, 1),
        new Point(63, 1),
        new Point(64, 1),
        new Point(65, 1),
        new Point(66, 2),
        new Point(67, 2),
        new Point(67, 2),
        new Point(68, 2),
        new Point(69, 2),
        new Point(70, 2),
        new Point(70, 3),
        new Point(71, 3),
        new Point(72, 3),
        new Point(73, 3),
        new Point(73, 3),
        new Point(74, 4),
        new Point(75, 4),
        new Point(76, 4),
        new Point(76, 4),
        new Point(77, 5),
        new Point(78, 5),
        new Point(79, 5),
        new Point(79, 6),
        new Point(80, 6),
        new Point(81, 6),
        new Point(82, 6),
        new Point(82, 7),
        new Point(83, 7),
        new Point(84, 8),
        new Point(84, 8),
        new Point(85, 8),
        new Point(86, 9),
        new Point(87, 9),
        new Point(87, 9),
        new Point(88, 10),
        new Point(89, 10),
        new Point(89, 11),
        new Point(90, 11),
        new Point(91, 12),
        new Point(91, 12),
        new Point(92, 13),
        new Point(92, 13),
        new Point(93, 14),
        new Point(94, 14),
        new Point(94, 15),
        new Point(95, 15),
        new Point(95, 16),
        new Point(96, 16),
        new Point(97, 17),
        new Point(97, 17),
        new Point(98, 18),
        new Point(98, 18),
        new Point(99, 19),
        new Point(99, 20),
        new Point(100, 20),
        new Point(100, 21),
        new Point(101, 21),
        new Point(101, 22),
        new Point(102, 23),
        new Point(102, 23),
        new Point(103, 24),
        new Point(103, 25),
        new Point(104, 25),
        new Point(104, 26),
        new Point(104, 27),
        new Point(105, 27),
        new Point(105, 28),
        new Point(106, 29),
        new Point(106, 30),
        new Point(106, 30),
        new Point(107, 31),
        new Point(107, 32),
        new Point(107, 33),
        new Point(108, 33),
        new Point(108, 34),
        new Point(108, 35),
        new Point(108, 36),
        new Point(109, 36),
        new Point(109, 37),
        new Point(109, 38),
        new Point(109, 39),
        new Point(110, 39),
        new Point(110, 39),
        new Point(110, 40),
        new Point(110, 41),
        new Point(110, 42),
        new Point(110, 43),
        new Point(110, 43),
        new Point(110, 44),
        new Point(110, 45),
        new Point(110, 46),
        new Point(110, 47),
        new Point(110, 48),
        new Point(110, 48),
        new Point(110, 49),
        new Point(110, 50),
        new Point(110, 51),
        new Point(110, 52),
        new Point(110, 53),
        new Point(110, 54),
        new Point(110, 54),
        new Point(110, 55),
        new Point(110, 56),
        new Point(109, 57),
        new Point(109, 58),
        new Point(109, 59),
        new Point(109, 60),
        new Point(108, 60),
        new Point(108, 61),
        new Point(108, 62),
        new Point(107, 63),
        new Point(107, 64),
        new Point(107, 65),
        new Point(106, 66),
        new Point(106, 66),
        new Point(106, 67),
        new Point(105, 68),
        new Point(105, 69),
        new Point(104, 70),
        new Point(104, 71),
        new Point(103, 71),
        new Point(103, 72),
        new Point(102, 73),
        new Point(101, 74),
        new Point(101, 75),
        new Point(100, 75),
        new Point(100, 76),
        new Point(99, 77),
        new Point(98, 78),
        new Point(98, 78),
        new Point(97, 79),
        new Point(96, 80),
        new Point(95, 80),
        new Point(95, 81),
        new Point(94, 82),
        new Point(93, 82),
        new Point(92, 83),
        new Point(91, 84),
        new Point(90, 84),
        new Point(90, 85),
        new Point(89, 86),
        new Point(88, 86),
        new Point(87, 87),
        new Point(86, 87),
        new Point(85, 88),
        new Point(84, 88),
        new Point(83, 89),
        new Point(82, 89),
        new Point(81, 90),
        new Point(80, 90),
        new Point(79, 91),
        new Point(78, 91),
        new Point(77, 91),
        new Point(76, 92),
        new Point(75, 92),
        new Point(74, 93),
        new Point(73, 93),
        new Point(71, 93),
        new Point(70, 93),
        new Point(69, 94),
        new Point(68, 94),
        new Point(67, 94),
        new Point(66, 94),
        new Point(65, 95),
        new Point(64, 95),
        new Point(62, 95),
        new Point(61, 95),
        new Point(60, 95),
        new Point(59, 95),
        new Point(58, 95),
        new Point(57, 95),
        new Point(56, 95),
        new Point(55, 95),
        new Point(54, 95),
        new Point(53, 95),
        new Point(52, 95),
        new Point(51, 95),
        new Point(50, 95),
        new Point(48, 95),
        new Point(47, 95),
        new Point(46, 94),
        new Point(45, 94),
        new Point(44, 94),
        new Point(43, 94),
        new Point(42, 93),
        new Point(41, 93),
        new Point(39, 93),
        new Point(38, 93),
        new Point(37, 92),
        new Point(36, 92),
        new Point(35, 91),
        new Point(34, 91),
        new Point(33, 91),
        new Point(32, 90),
        new Point(31, 90),
        new Point(30, 89),
        new Point(29, 89),
        new Point(28, 88),
        new Point(27, 88),
        new Point(26, 87),
        new Point(25, 87),
        new Point(24, 86),
        new Point(23, 86),
        new Point(22, 85),
        new Point(22, 84),
        new Point(21, 84),
        new Point(20, 83),
        new Point(19, 82),
        new Point(18, 82),
        new Point(17, 81),
        new Point(17, 80),
        new Point(16, 80),
        new Point(15, 79),
        new Point(14, 78),
        new Point(14, 78),
        new Point(13, 77),
        new Point(12, 76),
        new Point(12, 75),
        new Point(11, 75),
        new Point(11, 74),
        new Point(10, 73),
        new Point(9, 72),
        new Point(9, 71),
        new Point(8, 71),
        new Point(8, 70),
        new Point(7, 69),
        new Point(7, 68),
        new Point(6, 67),
        new Point(6, 66),
        new Point(6, 66),
        new Point(5, 65),
        new Point(5, 64),
        new Point(5, 63),
        new Point(4, 62),
        new Point(4, 61),
        new Point(4, 60),
        new Point(3, 60),
        new Point(3, 59),
        new Point(3, 58),
        new Point(3, 57),
        new Point(2, 56),
        new Point(2, 55),
        new Point(2, 54),
        new Point(2, 54),
        new Point(2, 53),
        new Point(2, 52),
        new Point(2, 51),
        new Point(2, 50),
        new Point(2, 49),
        new Point(2, 48),
        new Point(2, 48),
        new Point(2, 47),
        new Point(2, 46),
        new Point(2, 45),
        new Point(2, 44),
        new Point(2, 43),
        new Point(2, 43),
        new Point(2, 42),
        new Point(2, 41),
        new Point(2, 40),
        new Point(2, 39),
        new Point(2, 39),
        new Point(3, 39),
        new Point(3, 38),
        new Point(3, 37),
        new Point(3, 36),
        new Point(4, 36),
        new Point(4, 35),
        new Point(4, 34),
        new Point(4, 33),
        new Point(5, 33),
        new Point(5, 32),
        new Point(5, 31),
        new Point(6, 30),
        new Point(6, 30),
        new Point(6, 29),
        new Point(7, 28),
        new Point(7, 27),
        new Point(8, 27),
        new Point(8, 26),
        new Point(8, 25),
        new Point(9, 25),
        new Point(9, 24),
        new Point(10, 23),
        new Point(10, 23),
        new Point(11, 22),
        new Point(11, 21),
        new Point(12, 21),
        new Point(12, 20),
        new Point(13, 20),
        new Point(13, 19),
        new Point(14, 18),
        new Point(14, 18),
        new Point(15, 17),
        new Point(15, 17),
        new Point(16, 16),
        new Point(17, 16),
        new Point(17, 15),
        new Point(18, 15),
        new Point(18, 14),
        new Point(19, 14),
        new Point(20, 13),
        new Point(20, 13),
        new Point(21, 12),
        new Point(21, 12),
        new Point(22, 11),
        new Point(23, 11),
        new Point(23, 10),
        new Point(24, 10),
        new Point(25, 9),
        new Point(25, 9),
        new Point(26, 9),
        new Point(27, 8),
        new Point(28, 8),
        new Point(28, 8),
        new Point(29, 7),
        new Point(30, 7),
        new Point(30, 6),
        new Point(31, 6),
        new Point(32, 6),
        new Point(33, 6),
        new Point(33, 5),
        new Point(34, 5),
        new Point(35, 5),
        new Point(36, 4),
        new Point(36, 4),
        new Point(37, 4),
        new Point(38, 4),
        new Point(39, 3),
        new Point(39, 3),
        new Point(40, 3),
        new Point(41, 3),
        new Point(42, 3),
        new Point(42, 2),
        new Point(43, 2),
        new Point(44, 2),
        new Point(45, 2),
        new Point(45, 2),
        new Point(46, 2),
        new Point(47, 1),
        new Point(48, 1),
        new Point(49, 1),
        new Point(49, 1),
        new Point(50, 1),
        new Point(51, 1),
        new Point(52, 1),
        new Point(53, 1),
        new Point(53, 1),
        new Point(54, 1),
        new Point(55, 1),
        new Point(56, 1)
      };
    }

    private void create_ajust_compass_tbl()
    {
      this.m_ajust_compass = new float[360]
      {
        0.0f,
        0.0f,
        358f,
        358f,
        356f,
        354f,
        354f,
        354f,
        352f,
        352f,
        350f,
        350f,
        348f,
        348f,
        346f,
        346f,
        344f,
        344f,
        342f,
        342f,
        340f,
        340f,
        338f,
        338f,
        336f,
        336f,
        334f,
        334f,
        332f,
        332f,
        330f,
        330f,
        328f,
        328f,
        326f,
        326f,
        324f,
        324f,
        322f,
        322f,
        320f,
        320f,
        318f,
        318f,
        316f,
        316f,
        314f,
        314f,
        312f,
        310f,
        310f,
        308f,
        308f,
        306f,
        306f,
        304f,
        304f,
        302f,
        302f,
        300f,
        300f,
        298f,
        298f,
        296f,
        296f,
        294f,
        294f,
        292f,
        292f,
        290f,
        290f,
        288f,
        288f,
        286f,
        286f,
        284f,
        284f,
        282f,
        282f,
        280f,
        280f,
        278f,
        276f,
        276f,
        276f,
        274f,
        274f,
        272f,
        272f,
        270f,
        270f,
        268f,
        268f,
        266f,
        266f,
        264f,
        264f,
        262f,
        262f,
        260f,
        260f,
        258f,
        258f,
        256f,
        256f,
        254f,
        254f,
        252f,
        252f,
        250f,
        250f,
        248f,
        248f,
        246f,
        246f,
        244f,
        244f,
        242f,
        242f,
        240f,
        240f,
        238f,
        238f,
        236f,
        236f,
        234f,
        234f,
        232f,
        232f,
        230f,
        230f,
        228f,
        228f,
        226f,
        226f,
        224f,
        224f,
        222f,
        222f,
        220f,
        220f,
        218f,
        218f,
        216f,
        216f,
        214f,
        214f,
        212f,
        212f,
        210f,
        210f,
        208f,
        208f,
        206f,
        206f,
        204f,
        204f,
        202f,
        202f,
        200f,
        200f,
        198f,
        198f,
        196f,
        196f,
        194f,
        194f,
        192f,
        192f,
        190f,
        190f,
        188f,
        188f,
        186f,
        186f,
        184f,
        184f,
        182f,
        182f,
        180f,
        180f,
        178f,
        178f,
        176f,
        174f,
        174f,
        172f,
        172f,
        170f,
        170f,
        168f,
        168f,
        166f,
        166f,
        164f,
        164f,
        162f,
        162f,
        160f,
        160f,
        158f,
        158f,
        156f,
        156f,
        154f,
        154f,
        152f,
        152f,
        150f,
        150f,
        148f,
        148f,
        146f,
        146f,
        144f,
        144f,
        142f,
        142f,
        140f,
        140f,
        138f,
        138f,
        136f,
        136f,
        134f,
        134f,
        132f,
        132f,
        130f,
        130f,
        128f,
        128f,
        126f,
        126f,
        124f,
        124f,
        122f,
        122f,
        120f,
        120f,
        120f,
        118f,
        118f,
        116f,
        116f,
        114f,
        114f,
        112f,
        112f,
        110f,
        110f,
        108f,
        108f,
        106f,
        106f,
        104f,
        104f,
        102f,
        102f,
        100f,
        100f,
        98f,
        98f,
        96f,
        96f,
        94f,
        94f,
        92f,
        92f,
        90f,
        90f,
        88f,
        88f,
        86f,
        86f,
        84f,
        84f,
        82f,
        82f,
        80f,
        78f,
        78f,
        76f,
        76f,
        74f,
        74f,
        72f,
        72f,
        70f,
        70f,
        68f,
        68f,
        66f,
        66f,
        64f,
        64f,
        62f,
        62f,
        60f,
        60f,
        58f,
        58f,
        56f,
        56f,
        54f,
        54f,
        52f,
        52f,
        50f,
        50f,
        48f,
        48f,
        46f,
        46f,
        44f,
        44f,
        42f,
        42f,
        40f,
        40f,
        38f,
        38f,
        36f,
        36f,
        34f,
        34f,
        32f,
        32f,
        30f,
        30f,
        30f,
        28f,
        28f,
        26f,
        26f,
        24f,
        24f,
        22f,
        22f,
        20f,
        20f,
        18f,
        18f,
        16f,
        16f,
        14f,
        14f,
        12f,
        12f,
        10f,
        10f,
        8f,
        8f,
        6f,
        6f,
        4f,
        4f,
        2f,
        2f,
        0.0f
      };
    }

    private float update_angle_with_ajust(float angle_index)
    {
      int index = (int) angle_index;
      if (index >= 360)
        index -= 360;
      if (index < 0)
        index += 360;
      if (index >= 360 || index < 0)
        return -1f;
      else
        return this.m_ajust_compass[index];
    }

    public static PointF AngleToVector(float angle)
    {
      if ((double) angle < 0.0)
        return new PointF(0.0f, 0.0f);
      PointF pointF = new PointF();
      angle = Useful.ToRadian(angle - 90f);
      pointF.X = (float) Math.Cos((double) angle);
      pointF.Y = (float) Math.Sin((double) angle);
      return pointF;
    }

    public enum mode
    {
      xp,
      vista,
    }
  }
}
