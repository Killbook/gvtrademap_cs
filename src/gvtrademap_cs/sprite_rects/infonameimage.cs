/*-------------------------------------------------------------------------

 街名などの絵情報

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
	public class infonameimage : d3d_sprite_rects
	{
		private const int			ICON_START_INDEX		= 0;
		private const int			CITY_START_INDEX		= 12;
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public infonameimage(d3d_device device, string infoimage_fname)
			: base(device, infoimage_fname)
		{
			if(device.device == null)	return;
			add_rects();
		}

		/*-------------------------------------------------------------------------
		 アイコンの矩形を得る
		---------------------------------------------------------------------------*/
		public d3d_sprite_rects.rect GetIcon(int index)
		{
			if(index < 0)									return null;
			if((ICON_START_INDEX + index) >= rect_count)	return null;
			return GetRect(ICON_START_INDEX + index);
		}

		/*-------------------------------------------------------------------------
		 街名の矩形を得る
		---------------------------------------------------------------------------*/
		public d3d_sprite_rects.rect GetCityName(int index)
		{
			if(index < 0)									return null;
			if((CITY_START_INDEX + index) >= rect_count)	return null;
			return GetRect(CITY_START_INDEX + index);
		}

		/*-------------------------------------------------------------------------
		 切り取り矩形の追加
		---------------------------------------------------------------------------*/
		private void add_rects()
		{
			// スクリプトで生成されたもの
//from tool 
//infonameimage.cs 
//infoicon 
			AddRect(new Vector2(-12, -22), new Rectangle(168, 12, 192 - 168, 38 - 12));	// 0
			AddRect(new Vector2(-11, -14), new Rectangle(168, 40, 192 - 168, 60 - 40));	// 1
			AddRect(new Vector2(-7, -11), new Rectangle(72, 60, 86 - 72, 76 - 60));	// 2
			AddRect(new Vector2(-5, -9), new Rectangle(156, 0, 166 - 156, 12 - 0));	// 3
			AddRect(new Vector2(-7, -11), new Rectangle(72, 36, 86 - 72, 52 - 36));	// 4
			AddRect(new Vector2(-5, -9), new Rectangle(168, 0, 178 - 168, 12 - 0));	// 5
			AddRect(new Vector2(-2, -2), new Rectangle(176, 64, 182 - 176, 70 - 64));	// 6
			AddRect(new Vector2(-2, -2), new Rectangle(176, 72, 182 - 176, 78 - 72));	// 7
			AddRect(new Vector2(-2, -2), new Rectangle(176, 80, 182 - 176, 86 - 80));	// 8
			AddRect(new Vector2(-2, -2), new Rectangle(176, 88, 182 - 176, 94 - 88));	// 9
			AddRect(new Vector2(-2, -2), new Rectangle(176, 96, 182 - 176, 102 - 96));	// 10
			AddRect(new Vector2(-2, -2), new Rectangle(176, 104, 182 - 176, 110 - 104));	// 11
//Info 
			AddRect(new Vector2(0, 0), new Rectangle(0, 0, 76 - 0, 16 - 0));	// 0
			AddRect(new Vector2(-12, 0), new Rectangle(0, 16, 64 - 0, 32 - 16));	// 1
			AddRect(new Vector2(-28, -16), new Rectangle(0, 32, 56 - 0, 48 - 32));	// 2
			AddRect(new Vector2(0, -16), new Rectangle(0, 48, 40 - 0, 64 - 48));	// 3
			AddRect(new Vector2(-34, -16), new Rectangle(0, 64, 68 - 0, 80 - 64));	// 4
			AddRect(new Vector2(-76, -8), new Rectangle(0, 80, 76 - 0, 96 - 80));	// 5
			AddRect(new Vector2(0, 0), new Rectangle(0, 96, 60 - 0, 112 - 96));	// 6
			AddRect(new Vector2(-57, -16), new Rectangle(0, 112, 60 - 0, 128 - 112));	// 7
			AddRect(new Vector2(0, 0), new Rectangle(0, 128, 56 - 0, 144 - 128));	// 8
			AddRect(new Vector2(0, 0), new Rectangle(0, 144, 76 - 0, 160 - 144));	// 9
			AddRect(new Vector2(0, -8), new Rectangle(0, 160, 52 - 0, 176 - 160));	// 10
			AddRect(new Vector2(-22, -16), new Rectangle(0, 176, 44 - 0, 192 - 176));	// 11
			AddRect(new Vector2(-14, 0), new Rectangle(0, 192, 28 - 0, 208 - 192));	// 12
			AddRect(new Vector2(-24, 0), new Rectangle(0, 208, 60 - 0, 224 - 208));	// 13
			AddRect(new Vector2(0, -8), new Rectangle(0, 224, 68 - 0, 240 - 224));	// 14
			AddRect(new Vector2(-41, -15), new Rectangle(0, 240, 44 - 0, 256 - 240));	// 15
			AddRect(new Vector2(-42, -1), new Rectangle(0, 256, 44 - 0, 272 - 256));	// 16
            AddRect(new Vector2(-49, -10), new Rectangle(0, 272, 48, 16)); 	// 17
			AddRect(new Vector2(-41, -16), new Rectangle(0, 288, 48 - 0, 304 - 288));	// 18
			AddRect(new Vector2(-18, 0), new Rectangle(0, 304, 36 - 0, 320 - 304));	// 19
			AddRect(new Vector2(0, -16), new Rectangle(0, 320, 36 - 0, 336 - 320));	// 20
			AddRect(new Vector2(0, 0), new Rectangle(0, 336, 48 - 0, 352 - 336));	// 21
			AddRect(new Vector2(0, -8), new Rectangle(0, 352, 48 - 0, 368 - 352));	// 22
			AddRect(new Vector2(-11, 0), new Rectangle(0, 368, 68 - 0, 384 - 368));	// 23
			AddRect(new Vector2(-20, 0), new Rectangle(0, 384, 40 - 0, 400 - 384));	// 24
			AddRect(new Vector2(0, -16), new Rectangle(0, 400, 48 - 0, 416 - 400));	// 25
			AddRect(new Vector2(-28, -16), new Rectangle(0, 416, 56 - 0, 432 - 416));	// 26
			AddRect(new Vector2(-18, -16), new Rectangle(0, 432, 40 - 0, 448 - 432));	// 27
			AddRect(new Vector2(-28, -16), new Rectangle(0, 448, 56 - 0, 464 - 448));	// 28
			AddRect(new Vector2(-18, -1), new Rectangle(0, 464, 36 - 0, 480 - 464));	// 29
			AddRect(new Vector2(-2, -14), new Rectangle(0, 480, 36 - 0, 496 - 480));	// 30
			AddRect(new Vector2(-22, 0), new Rectangle(0, 496, 44 - 0, 512 - 496));	// 31
			AddRect(new Vector2(-1, -15), new Rectangle(0, 512, 36 - 0, 528 - 512));	// 32
			AddRect(new Vector2(-28, -16), new Rectangle(0, 528, 56 - 0, 544 - 528));	// 33
			AddRect(new Vector2(-30, -1), new Rectangle(0, 544, 60 - 0, 560 - 544));	// 34
			AddRect(new Vector2(-1, -15), new Rectangle(0, 560, 48 - 0, 576 - 560));	// 35
			AddRect(new Vector2(-44, -1), new Rectangle(0, 576, 48 - 0, 592 - 576));	// 36
			AddRect(new Vector2(-34, -15), new Rectangle(0, 592, 68 - 0, 608 - 592));	// 37
			AddRect(new Vector2(-1, -8), new Rectangle(0, 608, 48 - 0, 624 - 608));	// 38
			AddRect(new Vector2(0, 0), new Rectangle(0, 624, 40 - 0, 640 - 624));	// 39
			AddRect(new Vector2(0, -8), new Rectangle(0, 640, 48 - 0, 656 - 640));	// 40
			AddRect(new Vector2(-45, -8), new Rectangle(0, 656, 48 - 0, 672 - 656));	// 41
			AddRect(new Vector2(-26, -16), new Rectangle(0, 672, 56 - 0, 688 - 672));	// 42
			AddRect(new Vector2(0, 0), new Rectangle(0, 688, 48 - 0, 704 - 688));	// 43
			AddRect(new Vector2(0, -8), new Rectangle(0, 704, 56 - 0, 720 - 704));	// 44
			AddRect(new Vector2(-30, 0), new Rectangle(0, 720, 36 - 0, 736 - 720));	// 45
			AddRect(new Vector2(-14, -15), new Rectangle(0, 736, 28 - 0, 752 - 736));	// 46
			AddRect(new Vector2(-28, -16), new Rectangle(0, 752, 56 - 0, 768 - 752));	// 47
			AddRect(new Vector2(-1, -8), new Rectangle(0, 768, 48 - 0, 784 - 768));	// 48
			AddRect(new Vector2(-1, -2), new Rectangle(0, 784, 32 - 0, 800 - 784));	// 49
			AddRect(new Vector2(-26, -1), new Rectangle(0, 800, 52 - 0, 816 - 800));	// 50
			AddRect(new Vector2(-41, -14), new Rectangle(0, 816, 44 - 0, 832 - 816));	// 51
			AddRect(new Vector2(-50, -1), new Rectangle(0, 832, 68 - 0, 848 - 832));	// 52
			AddRect(new Vector2(-23, 0), new Rectangle(0, 848, 48 - 0, 864 - 848));	// 53
			AddRect(new Vector2(-83, -1), new Rectangle(0, 864, 88 - 0, 880 - 864));	// 54
			AddRect(new Vector2(-67, -8), new Rectangle(0, 880, 68 - 0, 896 - 880));	// 55
			AddRect(new Vector2(-49, -8), new Rectangle(0, 896, 52 - 0, 912 - 896));	// 56
			AddRect(new Vector2(0, -8), new Rectangle(0, 912, 40 - 0, 928 - 912));	// 57
			AddRect(new Vector2(0, -8), new Rectangle(0, 928, 48 - 0, 944 - 928));	// 58
			AddRect(new Vector2(-34, -15), new Rectangle(0, 944, 68 - 0, 960 - 944));	// 59
			AddRect(new Vector2(-44, -2), new Rectangle(0, 960, 48 - 0, 976 - 960));	// 60
			AddRect(new Vector2(-22, 0), new Rectangle(0, 976, 44 - 0, 992 - 976));	// 61
			AddRect(new Vector2(-36, 0), new Rectangle(0, 992, 72 - 0, 1008 - 992));	// 62
			AddRect(new Vector2(-1, -8), new Rectangle(0, 1008, 64 - 0, 1024 - 1008));	// 63
			AddRect(new Vector2(-1, -3), new Rectangle(96, 0, 148 - 96, 16 - 0));	// 64
			AddRect(new Vector2(-1, -8), new Rectangle(96, 16, 144 - 96, 32 - 16));	// 65
			AddRect(new Vector2(-24, -15), new Rectangle(96, 32, 144 - 96, 48 - 32));	// 66
			AddRect(new Vector2(-32, -1), new Rectangle(96, 48, 160 - 96, 64 - 48));	// 67
			AddRect(new Vector2(-38, -15), new Rectangle(96, 64, 172 - 96, 80 - 64));	// 68
			AddRect(new Vector2(-9, -15), new Rectangle(96, 80, 164 - 96, 96 - 80));	// 69
			AddRect(new Vector2(-26, -1), new Rectangle(96, 96, 148 - 96, 112 - 96));	// 70
			AddRect(new Vector2(-18, -15), new Rectangle(96, 112, 132 - 96, 128 - 112));	// 71
			AddRect(new Vector2(-51, -8), new Rectangle(96, 128, 152 - 96, 144 - 128));	// 72
			AddRect(new Vector2(-7, -13), new Rectangle(96, 144, 180 - 96, 160 - 144));	// 73
			AddRect(new Vector2(-26, 0), new Rectangle(96, 160, 148 - 96, 176 - 160));	// 74
			AddRect(new Vector2(-30, 0), new Rectangle(96, 176, 156 - 96, 192 - 176));	// 75
			AddRect(new Vector2(-2, -8), new Rectangle(96, 192, 148 - 96, 208 - 192));	// 76
			AddRect(new Vector2(-56, -14), new Rectangle(96, 208, 156 - 96, 224 - 208));	// 77
			AddRect(new Vector2(-34, -14), new Rectangle(96, 224, 164 - 96, 240 - 224));	// 78
			AddRect(new Vector2(-18, -15), new Rectangle(96, 240, 140 - 96, 256 - 240));	// 79
			AddRect(new Vector2(-38, -2), new Rectangle(96, 256, 180 - 96, 272 - 256));	// 80
			AddRect(new Vector2(-57, -1), new Rectangle(96, 272, 160 - 96, 288 - 272));	// 81
			AddRect(new Vector2(-51, -8), new Rectangle(96, 288, 152 - 96, 304 - 288));	// 82
			AddRect(new Vector2(-72, -8), new Rectangle(96, 304, 172 - 96, 320 - 304));	// 83
			AddRect(new Vector2(-86, -13), new Rectangle(96, 320, 196 - 96, 336 - 320));	// 84
			AddRect(new Vector2(-48, -15), new Rectangle(96, 336, 192 - 96, 352 - 336));	// 85
			AddRect(new Vector2(-83, -8), new Rectangle(96, 352, 184 - 96, 368 - 352));	// 86
			AddRect(new Vector2(-30, -15), new Rectangle(96, 368, 156 - 96, 384 - 368));	// 87
			AddRect(new Vector2(-1, -8), new Rectangle(96, 384, 152 - 96, 400 - 384));	// 88
			AddRect(new Vector2(-36, -1), new Rectangle(96, 400, 168 - 96, 416 - 400));	// 89
			AddRect(new Vector2(-1, -8), new Rectangle(96, 416, 152 - 96, 432 - 416));	// 90
			AddRect(new Vector2(-1, -8), new Rectangle(96, 432, 136 - 96, 448 - 432));	// 91
			AddRect(new Vector2(-44, -15), new Rectangle(96, 448, 184 - 96, 464 - 448));	// 92
			AddRect(new Vector2(-26, -1), new Rectangle(96, 464, 148 - 96, 480 - 464));	// 93
			AddRect(new Vector2(-1, -8), new Rectangle(96, 480, 172 - 96, 496 - 480));	// 94
			AddRect(new Vector2(-2, -8), new Rectangle(96, 496, 148 - 96, 512 - 496));	// 95
			AddRect(new Vector2(-1, -8), new Rectangle(96, 512, 140 - 96, 528 - 512));	// 96
			AddRect(new Vector2(-1, -8), new Rectangle(96, 528, 152 - 96, 544 - 528));	// 97
			AddRect(new Vector2(-1, -8), new Rectangle(96, 544, 148 - 96, 560 - 544));	// 98
			AddRect(new Vector2(-39, -8), new Rectangle(96, 560, 136 - 96, 576 - 560));	// 99
			AddRect(new Vector2(-1, -8), new Rectangle(96, 576, 160 - 96, 592 - 576));	// 100
			AddRect(new Vector2(-50, -8), new Rectangle(96, 592, 148 - 96, 608 - 592));	// 101
			AddRect(new Vector2(-49, -8), new Rectangle(96, 608, 152 - 96, 624 - 608));	// 102
			AddRect(new Vector2(-49, -15), new Rectangle(96, 624, 152 - 96, 640 - 624));	// 103
			AddRect(new Vector2(-44, -15), new Rectangle(96, 640, 160 - 96, 656 - 640));	// 104
			AddRect(new Vector2(-70, -8), new Rectangle(96, 656, 168 - 96, 672 - 656));	// 105
			AddRect(new Vector2(-2, -8), new Rectangle(96, 672, 160 - 96, 688 - 672));	// 106
			AddRect(new Vector2(-49, -8), new Rectangle(96, 688, 152 - 96, 704 - 688));	// 107
			AddRect(new Vector2(-20, -15), new Rectangle(96, 704, 136 - 96, 720 - 704));	// 108
			AddRect(new Vector2(-1, -8), new Rectangle(96, 720, 148 - 96, 736 - 720));	// 109
			AddRect(new Vector2(-62, -14), new Rectangle(96, 736, 172 - 96, 752 - 736));	// 110
			AddRect(new Vector2(0, -8), new Rectangle(96, 752, 136 - 96, 768 - 752));	// 111
			AddRect(new Vector2(-37, -8), new Rectangle(96, 768, 136 - 96, 784 - 768));	// 112
			AddRect(new Vector2(0, -15), new Rectangle(96, 784, 152 - 96, 800 - 784));	// 113
			AddRect(new Vector2(-57, -3), new Rectangle(96, 800, 156 - 96, 816 - 800));	// 114
			AddRect(new Vector2(-48, -8), new Rectangle(96, 816, 148 - 96, 832 - 816));	// 115
			AddRect(new Vector2(-39, -8), new Rectangle(96, 832, 140 - 96, 848 - 832));	// 116
			AddRect(new Vector2(-2, -15), new Rectangle(96, 848, 152 - 96, 864 - 848));	// 117
			AddRect(new Vector2(-2, -14), new Rectangle(96, 864, 152 - 96, 880 - 864));	// 118
			AddRect(new Vector2(-58, -12), new Rectangle(96, 880, 156 - 96, 896 - 880));	// 119
			AddRect(new Vector2(0, -8), new Rectangle(96, 896, 124 - 96, 912 - 896));	// 120
			AddRect(new Vector2(-2, -14), new Rectangle(96, 912, 136 - 96, 928 - 912));	// 121
			AddRect(new Vector2(-50, -4), new Rectangle(96, 928, 152 - 96, 944 - 928));	// 122
			AddRect(new Vector2(-14, -15), new Rectangle(96, 944, 140 - 96, 960 - 944));	// 123
			AddRect(new Vector2(-1, -8), new Rectangle(96, 960, 184 - 96, 976 - 960));	// 124
			AddRect(new Vector2(-53, -14), new Rectangle(96, 976, 172 - 96, 992 - 976));	// 125
			AddRect(new Vector2(-1, -1), new Rectangle(96, 992, 128 - 96, 1008 - 992));	// 126
			AddRect(new Vector2(-25, -14), new Rectangle(96, 1008, 152 - 96, 1024 - 1008));	// 127
			AddRect(new Vector2(-21, -14), new Rectangle(200, 0, 244 - 200, 16 - 0));	// 128
			AddRect(new Vector2(-20, -2), new Rectangle(200, 16, 240 - 200, 32 - 16));	// 129
			AddRect(new Vector2(-51, -8), new Rectangle(200, 32, 256 - 200, 48 - 32));	// 130
			AddRect(new Vector2(-45, -14), new Rectangle(200, 48, 280 - 200, 64 - 48));	// 131
			AddRect(new Vector2(-2, -12), new Rectangle(200, 64, 244 - 200, 80 - 64));	// 132
			AddRect(new Vector2(-30, -1), new Rectangle(200, 80, 260 - 200, 96 - 80));	// 133
			AddRect(new Vector2(-16, -14), new Rectangle(200, 96, 244 - 200, 112 - 96));	// 134
			AddRect(new Vector2(-30, -15), new Rectangle(200, 112, 260 - 200, 128 - 112));	// 135
			AddRect(new Vector2(-2, -8), new Rectangle(200, 128, 232 - 200, 144 - 128));	// 136
			AddRect(new Vector2(-35, -9), new Rectangle(200, 144, 236 - 200, 160 - 144));	// 137
			AddRect(new Vector2(-1, -2), new Rectangle(200, 160, 244 - 200, 176 - 160));	// 138
			AddRect(new Vector2(-2, -1), new Rectangle(200, 176, 236 - 200, 192 - 176));	// 139
			AddRect(new Vector2(-30, -16), new Rectangle(200, 192, 260 - 200, 208 - 192));	// 140
			AddRect(new Vector2(-22, -1), new Rectangle(200, 208, 244 - 200, 224 - 208));	// 141
			AddRect(new Vector2(-3, -15), new Rectangle(200, 224, 236 - 200, 240 - 224));	// 142
			AddRect(new Vector2(-39, -2), new Rectangle(200, 240, 244 - 200, 256 - 240));	// 143
			AddRect(new Vector2(-30, -1), new Rectangle(200, 256, 260 - 200, 272 - 256));	// 144
			AddRect(new Vector2(-47, -3), new Rectangle(200, 272, 256 - 200, 288 - 272));	// 145
			AddRect(new Vector2(-16, -14), new Rectangle(200, 288, 232 - 200, 304 - 288));	// 146
			AddRect(new Vector2(-26, 0), new Rectangle(200, 304, 252 - 200, 320 - 304));	// 147
			AddRect(new Vector2(0, -8), new Rectangle(200, 320, 252 - 200, 336 - 320));	// 148
			AddRect(new Vector2(-13, -15), new Rectangle(200, 336, 256 - 200, 352 - 336));	// 149
			AddRect(new Vector2(-37, -8), new Rectangle(200, 352, 240 - 200, 368 - 352));	// 150
			AddRect(new Vector2(-1, -8), new Rectangle(200, 368, 260 - 200, 384 - 368));	// 151
			AddRect(new Vector2(-1, -8), new Rectangle(200, 384, 272 - 200, 400 - 384));	// 152
			AddRect(new Vector2(-32, -15), new Rectangle(200, 400, 264 - 200, 416 - 400));	// 153
			AddRect(new Vector2(-52, -14), new Rectangle(200, 416, 256 - 200, 432 - 416));	// 154
			AddRect(new Vector2(-33, -12), new Rectangle(200, 432, 240 - 200, 448 - 432));	// 155
			AddRect(new Vector2(-2, -8), new Rectangle(200, 448, 264 - 200, 464 - 448));	// 156
			AddRect(new Vector2(-2, -8), new Rectangle(200, 464, 276 - 200, 480 - 464));	// 157
			AddRect(new Vector2(-2, -1), new Rectangle(200, 480, 228 - 200, 496 - 480));	// 158
			AddRect(new Vector2(-2, -8), new Rectangle(200, 496, 252 - 200, 512 - 496));	// 159
			AddRect(new Vector2(-2, -8), new Rectangle(200, 512, 276 - 200, 528 - 512));	// 160
			AddRect(new Vector2(-20, -1), new Rectangle(200, 528, 240 - 200, 544 - 528));	// 161
			AddRect(new Vector2(-26, -1), new Rectangle(200, 544, 252 - 200, 560 - 544));	// 162
			AddRect(new Vector2(-28, -1), new Rectangle(200, 560, 256 - 200, 576 - 560));	// 163
			AddRect(new Vector2(-25, -1), new Rectangle(200, 576, 232 - 200, 592 - 576));	// 164
			AddRect(new Vector2(-25, -11), new Rectangle(200, 592, 228 - 200, 608 - 592));	// 165
			AddRect(new Vector2(-3, -14), new Rectangle(200, 608, 216 - 200, 624 - 608));	// 166
			AddRect(new Vector2(0, -8), new Rectangle(200, 624, 232 - 200, 640 - 624));	// 167
			AddRect(new Vector2(-25, -8), new Rectangle(200, 640, 232 - 200, 656 - 640));	// 168
			AddRect(new Vector2(-13, -14), new Rectangle(200, 656, 232 - 200, 672 - 656));	// 169
			AddRect(new Vector2(-38, -12), new Rectangle(200, 672, 240 - 200, 688 - 672));	// 170
			AddRect(new Vector2(-27, -8), new Rectangle(200, 688, 232 - 200, 704 - 688));	// 171
			AddRect(new Vector2(-27, -14), new Rectangle(200, 704, 232 - 200, 720 - 704));	// 172
			AddRect(new Vector2(-26, -8), new Rectangle(200, 720, 228 - 200, 736 - 720));	// 173
			AddRect(new Vector2(-14, -16), new Rectangle(200, 736, 228 - 200, 752 - 736));	// 174
			AddRect(new Vector2(-16, -16), new Rectangle(200, 752, 232 - 200, 768 - 752));	// 175
			AddRect(new Vector2(-16, -2), new Rectangle(200, 768, 232 - 200, 784 - 768));	// 176
			AddRect(new Vector2(-101, 6), new Rectangle(168, 784, 266 - 168, 800 - 784));	// 177
			AddRect(new Vector2(-114, -8), new Rectangle(168, 800, 272 - 168, 816 - 800));	// 178
			AddRect(new Vector2(-124, -17), new Rectangle(168, 816, 284 - 168, 832 - 816));	// 179
			AddRect(new Vector2(-72, -6), new Rectangle(168, 832, 230 - 168, 848 - 832));	// 180
			AddRect(new Vector2(-70, -20), new Rectangle(168, 848, 230 - 168, 864 - 848));	// 181
			AddRect(new Vector2(-65, -18), new Rectangle(168, 864, 230 - 168, 880 - 864));	// 182

            AddRect(new Vector2(-58f, -31f), new Rectangle(168, 880, 48, 16));
            AddRect(new Vector2(-50f, -23f), new Rectangle(136, 896, 64, 16));
            AddRect(new Vector2(-14f, -23f), new Rectangle(240, 768, 32, 16));
            AddRect(new Vector2(-45f, -20f), new Rectangle(224, 880, 64, 16));
            AddRect(new Vector2(-45f, 5f), new Rectangle(240, 752, 40, 16));
            AddRect(new Vector2(19f, -13f), new Rectangle(208, 896, 80, 16));

            //info2 
            AddRect(new Vector2(-19, 0), new Rectangle(288, 0, 344 - 288, 10 - 0));	// 0
			AddRect(new Vector2(0, -5), new Rectangle(288, 10, 344 - 288, 20 - 10));	// 1
			AddRect(new Vector2(-44, -10), new Rectangle(288, 20, 352 - 288, 30 - 20));	// 2
			AddRect(new Vector2(0, -5), new Rectangle(288, 30, 370 - 288, 40 - 30));	// 3
			AddRect(new Vector2(-5, -10), new Rectangle(288, 40, 334 - 288, 50 - 40));	// 4
			AddRect(new Vector2(-28, -10), new Rectangle(288, 50, 344 - 288, 60 - 50));	// 5
			AddRect(new Vector2(0, -5), new Rectangle(288, 60, 344 - 288, 70 - 60));	// 6
			AddRect(new Vector2(0, -5), new Rectangle(288, 70, 344 - 288, 80 - 70));	// 7
			AddRect(new Vector2(-31, 0), new Rectangle(288, 80, 350 - 288, 90 - 80));	// 8
			AddRect(new Vector2(0, -5), new Rectangle(288, 90, 342 - 288, 100 - 90));	// 9
			AddRect(new Vector2(0, -5), new Rectangle(288, 100, 352 - 288, 110 - 100));	// 10
			AddRect(new Vector2(-27, 0), new Rectangle(288, 110, 342 - 288, 120 - 110));	// 11
			AddRect(new Vector2(-9, 0), new Rectangle(288, 120, 350 - 288, 130 - 120));	// 12
			AddRect(new Vector2(-59, 0), new Rectangle(288, 130, 378 - 288, 140 - 130));	// 13
			AddRect(new Vector2(-15, -1), new Rectangle(288, 140, 336 - 288, 150 - 140));	// 14
			AddRect(new Vector2(0, -5), new Rectangle(288, 150, 330 - 288, 160 - 150));	// 15
			AddRect(new Vector2(-39, -2), new Rectangle(288, 160, 328 - 288, 170 - 160));	// 16
			AddRect(new Vector2(-24, -10), new Rectangle(288, 170, 336 - 288, 180 - 170));	// 17
			AddRect(new Vector2(-54, -5), new Rectangle(288, 180, 342 - 288, 190 - 180));	// 18
			AddRect(new Vector2(-45, -5), new Rectangle(288, 190, 334 - 288, 200 - 190));	// 19
			AddRect(new Vector2(0, -5), new Rectangle(288, 200, 330 - 288, 210 - 200));	// 20
			AddRect(new Vector2(0, -5), new Rectangle(288, 210, 324 - 288, 220 - 210));	// 21
			AddRect(new Vector2(-72, -5), new Rectangle(288, 220, 360 - 288, 230 - 220));	// 22
			AddRect(new Vector2(-33, 0), new Rectangle(288, 230, 344 - 288, 240 - 230));	// 23
			AddRect(new Vector2(-64, -10), new Rectangle(288, 240, 352 - 288, 250 - 240));	// 24
			AddRect(new Vector2(-23, 0), new Rectangle(288, 250, 334 - 288, 260 - 250));	// 25
			AddRect(new Vector2(-46, -5), new Rectangle(288, 260, 334 - 288, 270 - 260));	// 26
			AddRect(new Vector2(0, 0), new Rectangle(288, 270, 342 - 288, 280 - 270));	// 27
			AddRect(new Vector2(-32, -10), new Rectangle(288, 280, 352 - 288, 290 - 280));	// 28
			AddRect(new Vector2(0, -5), new Rectangle(288, 290, 370 - 288, 300 - 290));	// 29
			AddRect(new Vector2(0, -5), new Rectangle(288, 300, 352 - 288, 310 - 300));	// 30
			AddRect(new Vector2(0, -5), new Rectangle(288, 310, 360 - 288, 320 - 310));	// 31
			AddRect(new Vector2(-32, -10), new Rectangle(288, 320, 342 - 288, 330 - 320));	// 32
			AddRect(new Vector2(-72, -5), new Rectangle(288, 330, 360 - 288, 340 - 330));	// 33
			AddRect(new Vector2(-64, -5), new Rectangle(288, 340, 352 - 288, 350 - 340));	// 34
			AddRect(new Vector2(0, -4), new Rectangle(288, 350, 342 - 288, 370 - 350));	// 35
			AddRect(new Vector2(-90, -5), new Rectangle(288, 370, 378 - 288, 380 - 370));	// 36
			AddRect(new Vector2(-36, -5), new Rectangle(288, 380, 324 - 288, 390 - 380));	// 37
			AddRect(new Vector2(0, -5), new Rectangle(288, 390, 324 - 288, 400 - 390));	// 38
			AddRect(new Vector2(-72, -10), new Rectangle(288, 400, 360 - 288, 410 - 400));	// 39
			AddRect(new Vector2(-29, -10), new Rectangle(288, 410, 346 - 288, 420 - 410));	// 40
			AddRect(new Vector2(0, -5), new Rectangle(288, 420, 360 - 288, 430 - 420));	// 41
			AddRect(new Vector2(0, -5), new Rectangle(288, 430, 334 - 288, 440 - 430));	// 42
			AddRect(new Vector2(0, -10), new Rectangle(288, 440, 360 - 288, 450 - 440));	// 43
			AddRect(new Vector2(0, -10), new Rectangle(288, 450, 334 - 288, 470 - 450));	// 44
			AddRect(new Vector2(-25, -10), new Rectangle(288, 470, 338 - 288, 480 - 470));	// 45
			AddRect(new Vector2(0, 0), new Rectangle(288, 480, 346 - 288, 490 - 480));	// 46
			AddRect(new Vector2(0, -5), new Rectangle(288, 490, 378 - 288, 500 - 490));	// 47
			AddRect(new Vector2(-32, 0), new Rectangle(288, 500, 352 - 288, 510 - 500));	// 48
			AddRect(new Vector2(0, -5), new Rectangle(288, 510, 334 - 288, 520 - 510));	// 49
			AddRect(new Vector2(-41, -10), new Rectangle(288, 520, 370 - 288, 530 - 520));	// 50
			AddRect(new Vector2(0, -5), new Rectangle(288, 530, 334 - 288, 540 - 530));	// 51
			AddRect(new Vector2(0, -5), new Rectangle(288, 540, 326 - 288, 550 - 540));	// 52
			AddRect(new Vector2(-26, 0), new Rectangle(288, 550, 360 - 288, 560 - 550));	// 53
			AddRect(new Vector2(-46, -5), new Rectangle(288, 560, 334 - 288, 570 - 560));	// 54
			AddRect(new Vector2(0, -5), new Rectangle(288, 570, 324 - 288, 580 - 570));	// 55
			AddRect(new Vector2(0, -5), new Rectangle(288, 580, 326 - 288, 590 - 580));	// 56
            AddRect(new Vector2(-24f, -16f), new Rectangle(288, 590, 64, 10));	// 57
			AddRect(new Vector2(0, -5), new Rectangle(288, 600, 330 - 288, 610 - 600));	// 58
			AddRect(new Vector2(0, -5), new Rectangle(288, 610, 344 - 288, 620 - 610));	// 59
			AddRect(new Vector2(0, -5), new Rectangle(288, 620, 352 - 288, 630 - 620));	// 60
			AddRect(new Vector2(-42, -5), new Rectangle(288, 630, 330 - 288, 640 - 630));	// 61
			AddRect(new Vector2(-34, -5), new Rectangle(288, 640, 322 - 288, 650 - 640));	// 62
			AddRect(new Vector2(0, -5), new Rectangle(288, 650, 338 - 288, 660 - 650));	// 63
			AddRect(new Vector2(0, -5), new Rectangle(288, 660, 344 - 288, 670 - 660));	// 64
			AddRect(new Vector2(0, -5), new Rectangle(288, 670, 344 - 288, 680 - 670));	// 65
			AddRect(new Vector2(0, 0), new Rectangle(288, 680, 330 - 288, 690 - 680));	// 66
			AddRect(new Vector2(0, -5), new Rectangle(288, 690, 332 - 288, 700 - 690));	// 67
			AddRect(new Vector2(0, -5), new Rectangle(288, 700, 352 - 288, 710 - 700));	// 68
			AddRect(new Vector2(-64, -5), new Rectangle(288, 710, 352 - 288, 720 - 710));	// 69
			AddRect(new Vector2(-37, -10), new Rectangle(288, 720, 362 - 288, 730 - 720));	// 70
			AddRect(new Vector2(0, -10), new Rectangle(288, 730, 326 - 288, 740 - 730));	// 71
			AddRect(new Vector2(0, -5), new Rectangle(288, 740, 334 - 288, 750 - 740));	// 72
			AddRect(new Vector2(-46, -5), new Rectangle(288, 750, 334 - 288, 760 - 750));	// 73
			AddRect(new Vector2(-56, -5), new Rectangle(288, 760, 344 - 288, 770 - 760));	// 74
			AddRect(new Vector2(0, -5), new Rectangle(288, 770, 334 - 288, 780 - 770));	// 75
			AddRect(new Vector2(-8, -10), new Rectangle(288, 780, 338 - 288, 790 - 780));	// 76
			AddRect(new Vector2(0, -5), new Rectangle(288, 790, 336 - 288, 800 - 790));	// 77
			AddRect(new Vector2(0, -5), new Rectangle(288, 800, 362 - 288, 810 - 800));	// 78
			AddRect(new Vector2(-64, -5), new Rectangle(288, 810, 352 - 288, 820 - 810));	// 79
			AddRect(new Vector2(-1, -15), new Rectangle(288, 820, 318 - 288, 840 - 820));	// 80
			AddRect(new Vector2(0, -5), new Rectangle(288, 840, 326 - 288, 850 - 840));	// 81
			AddRect(new Vector2(0, -5), new Rectangle(288, 850, 346 - 288, 860 - 850));	// 82
			AddRect(new Vector2(-28, -1), new Rectangle(288, 860, 344 - 288, 870 - 860));	// 83
			AddRect(new Vector2(-23, -10), new Rectangle(288, 870, 344 - 288, 880 - 870));	// 84
			AddRect(new Vector2(0, -10), new Rectangle(288, 880, 342 - 288, 890 - 880));	// 85
			AddRect(new Vector2(-34, -10), new Rectangle(288, 890, 334 - 288, 900 - 890));	// 86
			AddRect(new Vector2(0, -5), new Rectangle(288, 900, 344 - 288, 910 - 900));	// 87
			AddRect(new Vector2(-28, -5), new Rectangle(288, 910, 316 - 288, 920 - 910));	// 88
			AddRect(new Vector2(0, -5), new Rectangle(288, 920, 324 - 288, 930 - 920));	// 89
			AddRect(new Vector2(-58, -5), new Rectangle(288, 930, 346 - 288, 940 - 930));	// 90
			AddRect(new Vector2(-15, -2), new Rectangle(288, 940, 318 - 288, 960 - 940));	// 91
			AddRect(new Vector2(-72, -5), new Rectangle(380, 0, 452 - 380, 10 - 0));	// 92
			AddRect(new Vector2(-20, 0), new Rectangle(380, 10, 420 - 380, 20 - 10));	// 93
			AddRect(new Vector2(0, -5), new Rectangle(380, 20, 452 - 380, 30 - 20));	// 94
			AddRect(new Vector2(-42, -16), new Rectangle(380, 30, 428 - 380, 50 - 30));	// 95
			AddRect(new Vector2(0, -5), new Rectangle(380, 50, 436 - 380, 60 - 50));	// 96
			AddRect(new Vector2(-56, -2), new Rectangle(380, 60, 436 - 380, 70 - 60));	// 97
			AddRect(new Vector2(-31, -10), new Rectangle(380, 70, 428 - 380, 80 - 70));	// 98
			AddRect(new Vector2(0, -5), new Rectangle(380, 80, 434 - 380, 90 - 80));	// 99
			AddRect(new Vector2(0, -5), new Rectangle(380, 90, 436 - 380, 100 - 90));	// 100
			AddRect(new Vector2(-27, -1), new Rectangle(380, 100, 434 - 380, 110 - 100));	// 101
			AddRect(new Vector2(0, -1), new Rectangle(380, 110, 424 - 380, 120 - 110));	// 102
			AddRect(new Vector2(0, -5), new Rectangle(380, 120, 442 - 380, 130 - 120));	// 103
			AddRect(new Vector2(0, -5), new Rectangle(380, 130, 434 - 380, 140 - 130));	// 104
			AddRect(new Vector2(-18, 0), new Rectangle(380, 140, 416 - 380, 150 - 140));	// 105
			AddRect(new Vector2(-56, -5), new Rectangle(380, 150, 436 - 380, 160 - 150));	// 106
			AddRect(new Vector2(-56, -5), new Rectangle(380, 160, 436 - 380, 170 - 160));	// 107
			AddRect(new Vector2(-6, -2), new Rectangle(380, 170, 444 - 380, 180 - 170));	// 108
			AddRect(new Vector2(-64, -5), new Rectangle(380, 180, 444 - 380, 190 - 180));	// 109
			AddRect(new Vector2(-1, -2), new Rectangle(380, 190, 446 - 380, 200 - 190));	// 110
			AddRect(new Vector2(-36, -5), new Rectangle(380, 200, 416 - 380, 210 - 200));	// 111
			AddRect(new Vector2(0, -5), new Rectangle(380, 210, 444 - 380, 220 - 210));	// 112
			AddRect(new Vector2(-64, -5), new Rectangle(380, 220, 444 - 380, 230 - 220));	// 113
			AddRect(new Vector2(-64, -5), new Rectangle(380, 230, 444 - 380, 240 - 230));	// 114
			AddRect(new Vector2(-36, -10), new Rectangle(380, 240, 452 - 380, 250 - 240));	// 115
			AddRect(new Vector2(0, -5), new Rectangle(380, 250, 426 - 380, 260 - 250));	// 116
			AddRect(new Vector2(0, -5), new Rectangle(380, 260, 416 - 380, 270 - 260));	// 117
			AddRect(new Vector2(0, -5), new Rectangle(380, 270, 470 - 380, 280 - 270));	// 118
			AddRect(new Vector2(-25, 0), new Rectangle(380, 280, 428 - 380, 290 - 280));	// 119
			AddRect(new Vector2(0, -5), new Rectangle(380, 290, 460 - 380, 300 - 290));	// 120
			AddRect(new Vector2(0, -5), new Rectangle(380, 300, 426 - 380, 310 - 300));	// 121
			AddRect(new Vector2(-31, -10), new Rectangle(380, 310, 442 - 380, 320 - 310));	// 122
			AddRect(new Vector2(-26, -5), new Rectangle(380, 320, 406 - 380, 330 - 320));	// 123
			AddRect(new Vector2(0, -5), new Rectangle(380, 330, 426 - 380, 340 - 330));	// 124
			AddRect(new Vector2(0, -5), new Rectangle(380, 340, 434 - 380, 350 - 340));	// 125
			AddRect(new Vector2(-21, -1), new Rectangle(380, 350, 422 - 380, 360 - 350));	// 126
			AddRect(new Vector2(0, -5), new Rectangle(380, 360, 426 - 380, 370 - 360));	// 127
			AddRect(new Vector2(-37, 0), new Rectangle(380, 370, 454 - 380, 380 - 370));	// 128
			AddRect(new Vector2(-37, -10), new Rectangle(380, 380, 454 - 380, 390 - 380));	// 129
			AddRect(new Vector2(0, 0), new Rectangle(380, 390, 444 - 380, 400 - 390));	// 130
			AddRect(new Vector2(0, -10), new Rectangle(380, 400, 444 - 380, 410 - 400));	// 131
			AddRect(new Vector2(0, -5), new Rectangle(380, 410, 438 - 380, 420 - 410));	// 132
			AddRect(new Vector2(0, -5), new Rectangle(380, 420, 428 - 380, 430 - 420));	// 133
			AddRect(new Vector2(0, 0), new Rectangle(380, 430, 442 - 380, 440 - 430));	// 134
			AddRect(new Vector2(-27, -10), new Rectangle(380, 440, 434 - 380, 450 - 440));	// 135
			AddRect(new Vector2(0, -10), new Rectangle(380, 450, 396 - 380, 460 - 450));	// 136
			AddRect(new Vector2(-27, -10), new Rectangle(380, 460, 434 - 380, 470 - 460));	// 137
			AddRect(new Vector2(-54, -1), new Rectangle(380, 470, 434 - 380, 480 - 470));	// 138
			AddRect(new Vector2(-18, -10), new Rectangle(380, 480, 416 - 380, 490 - 480));	// 139
			AddRect(new Vector2(-46, -5), new Rectangle(380, 490, 426 - 380, 500 - 490));	// 140
			AddRect(new Vector2(0, -5), new Rectangle(380, 500, 434 - 380, 510 - 500));	// 141
			AddRect(new Vector2(-23, -10), new Rectangle(380, 510, 408 - 380, 520 - 510));	// 142
			AddRect(new Vector2(-12, -10), new Rectangle(380, 520, 404 - 380, 530 - 520));	// 143
			AddRect(new Vector2(-41, -5), new Rectangle(380, 530, 418 - 380, 540 - 530));	// 144
			AddRect(new Vector2(-41, -5), new Rectangle(380, 540, 418 - 380, 550 - 540));	// 145
			AddRect(new Vector2(0, -10), new Rectangle(380, 550, 408 - 380, 560 - 550));	// 146
			AddRect(new Vector2(-27, -5), new Rectangle(380, 560, 408 - 380, 570 - 560));	// 147
			AddRect(new Vector2(-36, -5), new Rectangle(380, 570, 418 - 380, 580 - 570));	// 148
			AddRect(new Vector2(-36, -5), new Rectangle(380, 580, 418 - 380, 590 - 580));	// 149
			AddRect(new Vector2(-36, -5), new Rectangle(380, 590, 418 - 380, 600 - 590));	// 150
			AddRect(new Vector2(-19, -10), new Rectangle(380, 600, 418 - 380, 610 - 600));	// 151
			AddRect(new Vector2(5, -16), new Rectangle(380, 610, 454 - 380, 620 - 610));	// 152
			AddRect(new Vector2(-61, -5), new Rectangle(380, 620, 436 - 380, 630 - 620));	// 153
			AddRect(new Vector2(-70, -5), new Rectangle(380, 630, 444 - 380, 640 - 630));	// 154
		// 訳あって手打ち
		//
            AddRect(new Vector2(-5f, -5f), new Rectangle(376, 640, 80, 10));
            AddRect(new Vector2(-28f, -16f), new Rectangle(380, 650, 60, 10));
            AddRect(new Vector2(-42f, -16f), new Rectangle(380, 660, 90, 10));

			AddRect(new Vector2(-52, 0), new Rectangle(184, 960, 288 - 184, 976 - 960));	// 155
			AddRect(new Vector2(-31, -16), new Rectangle(184, 976, 246 - 184, 992 - 976));	// 156
			AddRect(new Vector2(0, -8), new Rectangle(184, 992, 260 - 184, 1008 - 992));	// 157
			AddRect(new Vector2(-38, -16), new Rectangle(184, 1008, 260 - 184, 1024 - 1008));	// 158
		}
	}
}
