// Type: gvtrademap_cs.favoriteroute
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Utility;

namespace gvtrademap_cs
{
  internal static class favoriteroute
  {
    public static bool MixMap(string fname_a, string fname_b, string fname_c, ImageFormat format)
    {
      bool flag = false;
      if (!File.Exists(fname_a))
        return false;
      if (!File.Exists(fname_b))
      {
        file_ctrl.RemoveFile(fname_c);
        return false;
      }
      else
      {
        if (!File.Exists(fname_c))
        {
          flag = true;
        }
        else
        {
          FileInfo fileInfo1 = new FileInfo(fname_a);
          FileInfo fileInfo2 = new FileInfo(fname_b);
          FileInfo fileInfo3 = new FileInfo(fname_c);
          if (fileInfo1.LastWriteTime > fileInfo3.LastWriteTime)
            flag = true;
          if (fileInfo2.LastWriteTime > fileInfo3.LastWriteTime)
            flag = true;
        }
        if (!flag)
          return true;
        try
        {
          Size size1;
          int stride1;
          byte[] numArray1 = favoriteroute.load_image(fname_a, out size1, out stride1);
          Size size2;
          int stride2;
          byte[] numArray2 = favoriteroute.load_image(fname_b, out size2, out stride2);
          if (numArray1.Length != numArray2.Length)
          {
            int num = (int) MessageBox.Show("お気に入り航路の画像サイズが地図と異なります。", "お気に入り航路の合成中", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          else
          {
            for (int index1 = 0; index1 < size1.Height; ++index1)
            {
              int index2 = stride1 * index1;
              int num1 = 0;
              while (num1 < size1.Width)
              {
                byte num2 = numArray2[index2];
                byte num3 = numArray2[index2 + 1];
                byte num4 = numArray2[index2 + 2];
                if ((int) num2 < 250 || (int) num3 < 250 || (int) num4 < 250)
                {
                  byte num5 = numArray1[index2];
                  byte num6 = numArray1[index2 + 1];
                  byte num7 = numArray1[index2 + 2];
                  byte num8 = (byte) ((int) num5 / 2 + (int) num2 / 2);
                  byte num9 = (byte) ((int) num6 / 2 + (int) num3 / 2);
                  byte num10 = (byte) ((int) num7 / 2 + (int) num4 / 2);
                  numArray1[index2] = num8;
                  numArray1[index2 + 1] = num9;
                  numArray1[index2 + 2] = num10;
                }
                ++num1;
                index2 += 3;
              }
            }
            GCHandle gcHandle = GCHandle.Alloc((object) numArray1, GCHandleType.Pinned);
            Bitmap bitmap = new Bitmap(size1.Width, size1.Height, stride1, PixelFormat.Format24bppRgb, gcHandle.AddrOfPinnedObject());
            bitmap.Save(fname_c, format);
            gcHandle.Free();
            bitmap.Dispose();
          }
        }
        catch
        {
        }
        GC.Collect();
        return true;
      }
    }

    private static byte[] load_image(string fname, out Size size, out int stride)
    {
      Bitmap bitmap = new Bitmap(fname);
      size = new Size(bitmap.Width, bitmap.Height);
      BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
      IntPtr scan0 = bitmapdata.Scan0;
      stride = bitmapdata.Stride;
      int length = bitmapdata.Height * stride;
      byte[] destination = new byte[length];
      Marshal.Copy(scan0, destination, 0, length);
      bitmap.UnlockBits(bitmapdata);
      bitmap.Dispose();
      return destination;
    }
  }
}
