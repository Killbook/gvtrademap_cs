﻿/*-------------------------------------------------------------------------

 メモアイコン

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

using Utility;
using System.Drawing;
using System.IO;
using Microsoft.DirectX;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class map_mark
	{
		public enum map_mark_type{
			axis0,		// 風向き
			axis1,		// 風向き
			axis2,		// 風向き
			axis3,		// 風向き
			axis4,		// 風向き
			axis5,		// 風向き
			axis6,		// 風向き
			axis7,		// 風向き
			icon0,		// サメ
			icon1,		// 火災
			icon2,		// 藻
			icon3,		// セイレーン
			icon4,		// 地場異常
			icon5,		// 漁場
			icon6,		// マンボウ
			icon7,		// free
			icon8,		// free
			icon9,		// free
			icon10,		// free
			icon11,		// 目的地
		};

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public class data : hittest
		{
			private gvt_lib					m_lib;

			private string					m_memo;
			private map_mark_type			m_type;

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			// メモ
			public string memo{				get{	return m_memo;			}
											set{	m_memo		= value;	}}
			// 種類
			public map_mark_type type{		get{	return m_type;			}
											set{	m_type		= value;	}}

			// ゲーム座標
			public Point gposition{			get{	return transform.map_pos2_game_pos(position, m_lib.loop_image);		}}
			// ツールチップ用
			public string tooltipstr{		get{
												Point pos	= gposition;
												return String.Format("{0}\n[{1},{2}]", memo, pos.X, pos.Y);
											}}
	
			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public data(gvt_lib lib) : base()
			{
				m_lib				= lib;
				m_memo				= "";
				m_type				= map_mark_type.axis0;
			}
			public data(gvt_lib lib, Point pos, map_mark_type type, string memo) : base()
			{
				m_lib				= lib;
				set_data(pos, type, memo);
			}

			/*-------------------------------------------------------------------------
			 読み込み
			---------------------------------------------------------------------------*/
			public bool Load(string line)
			{
				string[]	tmp		= line.Split(new char[]{'\t'});
				string[]	split	= new string[4];

				for(int i=0; i<split.Length; i++){
					split[i]	= "";
				}

				int	max		= 4;
				if(tmp.Length < max)	max	= tmp.Length;
				for(int i=0; i<max; i++){
					split[i]	= tmp[i];
				}
				try{
					set_data(	new Point(Convert.ToInt32(split[0]), Convert.ToInt32(split[1])),
								map_mark_type.axis0 + Convert.ToInt32(split[2]),
								split[3]);
				}catch{
					return false;
				}
				return true;
			}

			/*-------------------------------------------------------------------------
			 情報の設定
			---------------------------------------------------------------------------*/
			private void set_data(Point pos, map_mark_type type, string memo)
			{
				position		= pos;
				m_type			= type;
				m_memo			= memo;

				base.rect		= new Rectangle(-8, -8, 16, 16);
			}
		}
		
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		private gvt_lib						m_lib;
		private hittest_list				m_datas;

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public map_mark(gvt_lib lib, string fname)
		{
			m_lib			= lib;
			m_datas			= new hittest_list();

			// 読み込み
			load(fname);
		}

		/*-------------------------------------------------------------------------
		 追加
		---------------------------------------------------------------------------*/
		public void Add(Point pos, map_mark_type type, string memo)
		{
			m_datas.Add(new data(m_lib, pos, type, memo));
		}

		/*-------------------------------------------------------------------------
		 読み込み
		---------------------------------------------------------------------------*/
		private void load(string fname)
		{
			if(!File.Exists(fname))		return;

			string line = "";
			try{
				using (StreamReader	sr	= new StreamReader(
					fname, Encoding.GetEncoding("Shift_JIS"))) {

					while((line = sr.ReadLine()) != null){
						if(line == "")		continue;

						data	d	= new data(m_lib);
						if(d.Load(line))	m_datas.Add(d);
					}
				}
			}catch{
				// 読み込み失敗
			}
		}

		/*-------------------------------------------------------------------------
		 書き出し
		---------------------------------------------------------------------------*/
		public bool Write(string fname)
		{
			if(m_datas.Count <= 0){
				// ファイルがあれば削除する
				file_ctrl.RemoveFile(fname);
				return true;
			}
	
			try{
				using (StreamWriter	sw	= new StreamWriter(
					fname, false, Encoding.GetEncoding("Shift_JIS"))) {

					foreach(data d in m_datas){
						string	str		= "";
						str				+= d.position.X + "\t";
						str				+= d.position.Y + "\t";
						str				+= ((int)d.type).ToString() + "\t";
						str				+= d.memo;
						sw.WriteLine(str);
					}
				}
			}catch{
				return false;
			}
			return true;
		}

		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		public void Draw()
		{
			if(!m_lib.setting.draw_icons)	return;
	
			m_lib.device.sprites.BeginDrawSprites(m_lib.icons.texture);
			m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(draw_proc), 32f);
			m_lib.device.sprites.EndDrawSprites();
		}

		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		private void draw_proc(Vector2 offset, LoopXImage image)
		{
			float	size	= image.ImageScale;
			if(size < 0.5)		size	= 0.5f;
			else if(size > 1)	size	= 1;

			Vector2		scale	= new Vector2(size, size);

			// 目的地用
			size	*= 1.5f;
			Vector2		scale2	= new Vector2(size, size);

			foreach(data p in m_datas){
				int		index		= (int)p.type;
				if(index < 0)							index	= 0;
				if(index > (int)map_mark_type.icon11)	index	= (int)map_mark_type.icon11;
				if(!is_draw(index))		continue;

				Vector2		v		= image.GlobalPos2LocalPos(new Vector2(p.position.X, p.position.Y), offset);
				Vector3		pos		= new Vector3(v.X, v.Y, 0.5f);

				m_lib.device.sprites.AddDrawSprites(pos, m_lib.icons.GetIcon(icons.icon_index.memo_icon_0 + index),
											(index == (int)map_mark_type.icon11)? scale2: scale,
											Color.White.ToArgb());
			}
		}

		/*-------------------------------------------------------------------------
		 表示項目チェック
		---------------------------------------------------------------------------*/
		private bool is_draw(int index)
		{
			// 描画フラグ
			DrawSettingMemoIcons	flag	= m_lib.setting.draw_setting_memo_icons;

			switch((map_mark_type)index){
			case map_mark_type.axis0:
			case map_mark_type.axis1:
			case map_mark_type.axis2:
			case map_mark_type.axis3:
			case map_mark_type.axis4:
			case map_mark_type.axis5:
			case map_mark_type.axis6:
			case map_mark_type.axis7:
				if((flag & DrawSettingMemoIcons.wind) == 0)		return false;
				break;
			case map_mark_type.icon0:
				if((flag & DrawSettingMemoIcons.memo_0) == 0)	return false;
				break;
			case map_mark_type.icon1:
				if((flag & DrawSettingMemoIcons.memo_1) == 0)	return false;
				break;
			case map_mark_type.icon2:
				if((flag & DrawSettingMemoIcons.memo_2) == 0)	return false;
				break;
			case map_mark_type.icon3:
				if((flag & DrawSettingMemoIcons.memo_3) == 0)	return false;
				break;
			case map_mark_type.icon4:
				if((flag & DrawSettingMemoIcons.memo_4) == 0)	return false;
				break;
			case map_mark_type.icon5:
				if((flag & DrawSettingMemoIcons.memo_5) == 0)	return false;
				break;
			case map_mark_type.icon6:
				if((flag & DrawSettingMemoIcons.memo_6) == 0)	return false;
				break;
			case map_mark_type.icon7:
				if((flag & DrawSettingMemoIcons.memo_7) == 0)	return false;
				break;
			case map_mark_type.icon8:
				if((flag & DrawSettingMemoIcons.memo_8) == 0)	return false;
				break;
			case map_mark_type.icon9:
				if((flag & DrawSettingMemoIcons.memo_9) == 0)	return false;
				break;
			case map_mark_type.icon10:
				if((flag & DrawSettingMemoIcons.memo_10) == 0)	return false;
				break;
			case map_mark_type.icon11:
				if((flag & DrawSettingMemoIcons.memo_11) == 0)	return false;
				break;
			}
			return true;
		}

		/*-------------------------------------------------------------------------
		 メモ情報を得る
		 座標は地図座標であること
		---------------------------------------------------------------------------*/
		public data FindData(Point pos)
		{
			// 非表示時は矩形判定を行わない
			if(!m_lib.setting.draw_icons)	return null;

			foreach(data p in m_datas){
				if(p.HitTest(pos))		return p;
			}
			return null;
		}

		/*-------------------------------------------------------------------------
		 ツールチップを得る
		 座標は地図座標であること
		---------------------------------------------------------------------------*/
		public string GetToolTip(Point pos)
		{
			data	p	= FindData(pos);
			if(p == null)	return null;
			return p.tooltipstr;
		}

		/*-------------------------------------------------------------------------
		 削除する
		---------------------------------------------------------------------------*/
		public void RemoveData(data d)
		{
			if(d == null)	return;
			try{
				m_datas.Remove(d);
			}catch{
			}
		}

		/*-------------------------------------------------------------------------
		 全て削除する
		---------------------------------------------------------------------------*/
		public void RemoveAllData()
		{
			m_datas.Clear();
		}

		/*-------------------------------------------------------------------------
		 全ての目的地メモアイコンを削除する
		---------------------------------------------------------------------------*/
		public void RemoveAllTargetData()
		{
			while(true){
				data	p	= get_1st_target_memo();
				if(p == null)	break;
				RemoveData(p);
			}
		}

		/*-------------------------------------------------------------------------
		 最初の目的地メモアイコンを得る
		---------------------------------------------------------------------------*/
		private data get_1st_target_memo()
		{
			foreach(data p in m_datas){
				if(p.type == map_mark_type.icon11)	return p;
			}
			return null;
		}
	}
}
