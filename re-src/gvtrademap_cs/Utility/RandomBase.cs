// Type: Utility.RandomBase
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Diagnostics;

namespace Utility
{
  public abstract class RandomBase
  {
    public abstract uint NextUInt32();

    public virtual int NextInt32()
    {
      return (int) this.NextUInt32();
    }

    public virtual ulong NextUInt64()
    {
      return (ulong) this.NextUInt32() << 32 | (ulong) this.NextUInt32();
    }

    public virtual long NextInt64()
    {
      return (long) this.NextUInt32() << 32 | (long) this.NextUInt32();
    }

    public virtual void NextBytes(byte[] buffer)
    {
      int num1 = 0;
      while (num1 + 4 <= buffer.Length)
      {
        uint num2 = this.NextUInt32();
        byte[] numArray1 = buffer;
        int index1 = num1;
        int num3 = 1;
        int num4 = index1 + num3;
        int num5 = (int) (byte) num2;
        numArray1[index1] = (byte) num5;
        byte[] numArray2 = buffer;
        int index2 = num4;
        int num6 = 1;
        int num7 = index2 + num6;
        int num8 = (int) (byte) (num2 >> 8);
        numArray2[index2] = (byte) num8;
        byte[] numArray3 = buffer;
        int index3 = num7;
        int num9 = 1;
        int num10 = index3 + num9;
        int num11 = (int) (byte) (num2 >> 16);
        numArray3[index3] = (byte) num11;
        byte[] numArray4 = buffer;
        int index4 = num10;
        int num12 = 1;
        num1 = index4 + num12;
        int num13 = (int) (byte) (num2 >> 24);
        numArray4[index4] = (byte) num13;
      }
      if (num1 >= buffer.Length)
        return;
      uint num14 = this.NextUInt32();
      byte[] numArray5 = buffer;
      int index5 = num1;
      int num15 = 1;
      int num16 = index5 + num15;
      int num17 = (int) (byte) num14;
      numArray5[index5] = (byte) num17;
      if (num16 >= buffer.Length)
        return;
      byte[] numArray6 = buffer;
      int index6 = num16;
      int num18 = 1;
      int num19 = index6 + num18;
      int num20 = (int) (byte) (num14 >> 8);
      numArray6[index6] = (byte) num20;
      if (num19 >= buffer.Length)
        return;
      byte[] numArray7 = buffer;
      int index7 = num19;
      int num21 = 1;
      int num22 = index7 + num21;
      int num23 = (int) (byte) (num14 >> 16);
      numArray7[index7] = (byte) num23;
    }

    public virtual double NextDouble()
    {
      return 0.0 * Math.PI * (double) this.NextUInt32();
    }

    public virtual double NextDouble2()
    {
      return 0.0 * Math.PI * (double) this.NextUInt32();
    }

    public virtual int Next()
    {
      return (int) this.NextUInt32();
    }

    public virtual int Next(int max_value)
    {
      Trace.Assert(max_value >= 0, "RandomBase.Next()", "max_value は 0 以上にする必要があります。 ");
      return (int) (this.NextDouble() * (double) max_value);
    }

    public virtual int Next(int min_value, int max_value)
    {
      Trace.Assert(max_value >= min_value, "RandomBase.Next()", "max_value は min_value 以上にする必要があります。");
      max_value -= min_value;
      return min_value + (int) (this.NextDouble() * (double) max_value);
    }
  }
}
