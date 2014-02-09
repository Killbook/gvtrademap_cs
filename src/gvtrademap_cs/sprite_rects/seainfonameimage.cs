/*-------------------------------------------------------------------------

 海域名などの絵情報

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
	
using directx;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	public class seainfonameimage : d3d_sprite_rects
	{
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public seainfonameimage(d3d_device device, string infoimage_fname)
			: base(device, infoimage_fname)
		{
			if(device.device == null)	return;
			add_rects();
		}

		/*-------------------------------------------------------------------------
		 風向き用矩形を得る
		---------------------------------------------------------------------------*/
		public d3d_sprite_rects.rect GetWindArrowIcon()
		{
			return GetRect(rect_count - 2);
		}
		/*-------------------------------------------------------------------------
		 街の位置調整用
		---------------------------------------------------------------------------*/
		public d3d_sprite_rects.rect GetCityIcon()
		{
			return GetRect(rect_count - 1);
		}

		/*-------------------------------------------------------------------------
		 切り取り矩形の追加
		---------------------------------------------------------------------------*/
		private void add_rects()
		{
//seainfonameimage.cs 
//GvoSeaInfo 
			AddRect(new Vector2(-5, -6), new Rectangle(0, 0, 60 - 0, 12 - 0));	// 0
			AddRect(new Vector2(-5, -6), new Rectangle(0, 12, 74 - 0, 24 - 12));	// 1
			AddRect(new Vector2(-5, -6), new Rectangle(0, 24, 22 - 0, 36 - 24));	// 2
			AddRect(new Vector2(-5, -6), new Rectangle(0, 36, 38 - 0, 48 - 36));	// 3
			AddRect(new Vector2(-5, -6), new Rectangle(0, 48, 70 - 0, 60 - 48));	// 4
			AddRect(new Vector2(-5, -6), new Rectangle(0, 60, 70 - 0, 72 - 60));	// 5
			AddRect(new Vector2(-5, -6), new Rectangle(0, 72, 50 - 0, 84 - 72));	// 6
			AddRect(new Vector2(-5, -6), new Rectangle(0, 84, 52 - 0, 96 - 84));	// 7
			AddRect(new Vector2(-5, -6), new Rectangle(0, 96, 42 - 0, 108 - 96));	// 8
			AddRect(new Vector2(-5, -6), new Rectangle(0, 108, 50 - 0, 120 - 108));	// 9
			AddRect(new Vector2(-5, -6), new Rectangle(0, 120, 52 - 0, 132 - 120));	// 10
			AddRect(new Vector2(-5, -6), new Rectangle(0, 132, 50 - 0, 156 - 132));	// 11
			AddRect(new Vector2(-5, -5), new Rectangle(0, 156, 42 - 0, 180 - 156));	// 12
			AddRect(new Vector2(-5, -6), new Rectangle(0, 180, 46 - 0, 192 - 180));	// 13
			AddRect(new Vector2(-5, -6), new Rectangle(0, 192, 56 - 0, 204 - 192));	// 14
			AddRect(new Vector2(-5, -6), new Rectangle(0, 204, 48 - 0, 216 - 204));	// 15
			AddRect(new Vector2(-5, -6), new Rectangle(0, 216, 46 - 0, 228 - 216));	// 16
			AddRect(new Vector2(-5, -6), new Rectangle(0, 228, 42 - 0, 240 - 228));	// 17
			AddRect(new Vector2(-5, -6), new Rectangle(0, 240, 22 - 0, 252 - 240));	// 18
			AddRect(new Vector2(-5, -6), new Rectangle(0, 252, 50 - 0, 264 - 252));	// 19
			AddRect(new Vector2(-5, -6), new Rectangle(0, 264, 52 - 0, 276 - 264));	// 20
			AddRect(new Vector2(-5, -6), new Rectangle(0, 276, 52 - 0, 288 - 276));	// 21
			AddRect(new Vector2(-5, -6), new Rectangle(0, 288, 42 - 0, 300 - 288));	// 22
			AddRect(new Vector2(-5, -6), new Rectangle(0, 300, 42 - 0, 312 - 300));	// 23
			AddRect(new Vector2(-5, -6), new Rectangle(0, 312, 52 - 0, 324 - 312));	// 24
			AddRect(new Vector2(-5, -6), new Rectangle(0, 324, 40 - 0, 336 - 324));	// 25
			AddRect(new Vector2(-5, -6), new Rectangle(0, 336, 62 - 0, 348 - 336));	// 26
			AddRect(new Vector2(-5, -6), new Rectangle(0, 348, 52 - 0, 360 - 348));	// 27
			AddRect(new Vector2(-5, -6), new Rectangle(0, 360, 62 - 0, 372 - 360));	// 28
			AddRect(new Vector2(-5, -6), new Rectangle(0, 372, 82 - 0, 384 - 372));	// 29
			AddRect(new Vector2(-5, -6), new Rectangle(0, 384, 62 - 0, 396 - 384));	// 30
			AddRect(new Vector2(-5, -6), new Rectangle(0, 396, 70 - 0, 408 - 396));	// 31
			AddRect(new Vector2(-5, -6), new Rectangle(0, 408, 62 - 0, 420 - 408));	// 32
			AddRect(new Vector2(-5, -6), new Rectangle(0, 420, 52 - 0, 432 - 420));	// 33
			AddRect(new Vector2(-5, -6), new Rectangle(0, 432, 22 - 0, 444 - 432));	// 34
			AddRect(new Vector2(-5, -6), new Rectangle(0, 444, 52 - 0, 456 - 444));	// 35
			AddRect(new Vector2(-5, -6), new Rectangle(0, 456, 62 - 0, 468 - 456));	// 36
			AddRect(new Vector2(-5, -6), new Rectangle(0, 468, 60 - 0, 480 - 468));	// 37
			AddRect(new Vector2(-5, -6), new Rectangle(0, 480, 62 - 0, 492 - 480));	// 38
			AddRect(new Vector2(-5, -6), new Rectangle(0, 492, 62 - 0, 504 - 492));	// 39
			AddRect(new Vector2(-5, -6), new Rectangle(84, 0, 138 - 84, 12 - 0));	// 40
			AddRect(new Vector2(-5, -6), new Rectangle(84, 12, 146 - 84, 24 - 12));	// 41
			AddRect(new Vector2(-5, -6), new Rectangle(84, 24, 164 - 84, 36 - 24));	// 42
			AddRect(new Vector2(-5, -6), new Rectangle(84, 36, 136 - 84, 48 - 36));	// 43
			AddRect(new Vector2(-5, -6), new Rectangle(84, 48, 166 - 84, 60 - 48));	// 44
			AddRect(new Vector2(-5, -6), new Rectangle(84, 60, 136 - 84, 72 - 60));	// 45
			AddRect(new Vector2(-5, -6), new Rectangle(84, 72, 136 - 84, 84 - 72));	// 46
			AddRect(new Vector2(-5, -6), new Rectangle(84, 84, 136 - 84, 96 - 84));	// 47
			AddRect(new Vector2(-5, -6), new Rectangle(84, 96, 136 - 84, 108 - 96));	// 48
			AddRect(new Vector2(-5, -6), new Rectangle(84, 108, 156 - 84, 120 - 108));	// 49
			AddRect(new Vector2(-5, -6), new Rectangle(84, 120, 146 - 84, 132 - 120));	// 50
			AddRect(new Vector2(-5, -6), new Rectangle(84, 132, 136 - 84, 144 - 132));	// 51
			AddRect(new Vector2(-5, -6), new Rectangle(84, 144, 174 - 84, 156 - 144));	// 52
			AddRect(new Vector2(-5, -6), new Rectangle(84, 156, 166 - 84, 168 - 156));	// 53
			AddRect(new Vector2(-5, -6), new Rectangle(84, 168, 156 - 84, 180 - 168));	// 54
			AddRect(new Vector2(-5, -6), new Rectangle(84, 180, 136 - 84, 192 - 180));	// 55
			AddRect(new Vector2(-5, -6), new Rectangle(84, 192, 146 - 84, 204 - 192));	// 56
			AddRect(new Vector2(-5, -6), new Rectangle(84, 204, 156 - 84, 216 - 204));	// 57
			AddRect(new Vector2(-5, -6), new Rectangle(84, 216, 124 - 84, 228 - 216));	// 58
			AddRect(new Vector2(-5, -6), new Rectangle(84, 228, 126 - 84, 240 - 228));	// 59
			AddRect(new Vector2(-5, -6), new Rectangle(84, 240, 136 - 84, 252 - 240));	// 60
			AddRect(new Vector2(-5, -6), new Rectangle(84, 252, 126 - 84, 264 - 252));	// 61
			AddRect(new Vector2(-5, -6), new Rectangle(84, 264, 156 - 84, 276 - 264));	// 62
			AddRect(new Vector2(-5, -6), new Rectangle(84, 276, 156 - 84, 288 - 276));	// 63
			AddRect(new Vector2(-5, -6), new Rectangle(84, 288, 186 - 84, 300 - 288));	// 64
			AddRect(new Vector2(-5, -6), new Rectangle(84, 300, 136 - 84, 312 - 300));	// 65
			AddRect(new Vector2(-5, -6), new Rectangle(84, 312, 136 - 84, 324 - 312));	// 66
			AddRect(new Vector2(-5, -6), new Rectangle(84, 324, 186 - 84, 336 - 324));	// 67
			AddRect(new Vector2(-5, -6), new Rectangle(84, 336, 136 - 84, 348 - 336));	// 68
			AddRect(new Vector2(-5, -6), new Rectangle(84, 348, 136 - 84, 360 - 348));	// 69
			AddRect(new Vector2(-5, -6), new Rectangle(84, 360, 136 - 84, 372 - 360));	// 70
			AddRect(new Vector2(-5, -6), new Rectangle(84, 372, 156 - 84, 384 - 372));	// 71
			AddRect(new Vector2(-5, -6), new Rectangle(84, 384, 156 - 84, 396 - 384));	// 72
			AddRect(new Vector2(-5, -6), new Rectangle(84, 396, 146 - 84, 408 - 396));	// 73
			AddRect(new Vector2(-5, -6), new Rectangle(84, 408, 156 - 84, 420 - 408));	// 74
			AddRect(new Vector2(-5, -6), new Rectangle(84, 420, 136 - 84, 432 - 420));	// 75
			AddRect(new Vector2(-5, -6), new Rectangle(84, 432, 126 - 84, 444 - 432));	// 76
			AddRect(new Vector2(-5, -6), new Rectangle(84, 444, 156 - 84, 456 - 444));	// 77
			AddRect(new Vector2(-5, -6), new Rectangle(84, 456, 166 - 84, 468 - 456));	// 78
			AddRect(new Vector2(-5, -6), new Rectangle(84, 468, 156 - 84, 480 - 468));	// 79
			AddRect(new Vector2(-5, -6), new Rectangle(84, 480, 166 - 84, 492 - 480));	// 80
			AddRect(new Vector2(-5, -6), new Rectangle(84, 492, 156 - 84, 504 - 492));	// 81
			AddRect(new Vector2(-5, -6), new Rectangle(180, 0, 222 - 180, 12 - 0));	// 82
			AddRect(new Vector2(-5, -6), new Rectangle(168, 12, 250 - 168, 24 - 12));	// 83
			AddRect(new Vector2(-5, -6), new Rectangle(180, 24, 242 - 180, 36 - 24));	// 84
			AddRect(new Vector2(-5, -6), new Rectangle(180, 36, 242 - 180, 48 - 36));	// 85
			AddRect(new Vector2(-5, -6), new Rectangle(180, 48, 242 - 180, 60 - 48));	// 86
			AddRect(new Vector2(-5, -6), new Rectangle(180, 60, 252 - 180, 72 - 60));	// 87
			AddRect(new Vector2(-6, -6), new Rectangle(180, 72, 252 - 180, 84 - 72));	// 88
		// 訳あって手打ち
			AddRect(new Vector2(-6, -6), new Rectangle(180-12, 72+12*1, 252 - (180-12), 72 - 60));	// 87
			AddRect(new Vector2(-6, -6), new Rectangle(180, 72+12*2, 60, 84 - 72));	// 88
		//

			AddRect(new Vector2(-4, -7), new Rectangle(240, 492, 250 - 240, 504 - 492));	// 89
			AddRect(new Vector2(-2, -2), new Rectangle(240, 480, 246 - 240, 486 - 480));	// 90
		}
	}
}
