/*-------------------------------------------------------------------------

 航路共有

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 define
---------------------------------------------------------------------------*/
//#define	DRAW_POPUPS_BOUNDINGBOX

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using Utility;
using System.Drawing;
using Microsoft.DirectX;
using directx;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------
	 共有情報
	---------------------------------------------------------------------------*/
	public class ShareRoutes
	{
		// バウンディングボックスサイズ
		private const int					BB_ONCE_SIZE		= 400;
		// 角度表示の長さ
		private const float					ANGLE_LINE_LENGTH	= 48f;
		// テクスチャフォントのリフレッシュ間隔
		private const int					TEXTURED_FONT_REFLESH_INTERVAL	= 30;	// 30分
//		private const int					TEXTURED_FONT_REFLESH_INTERVAL	= 1;
	
		public enum State{
			outof_sea,				// 海上ではない
			in_the_sea,				// 海上
		};
	
		/*-------------------------------------------------------------------------
		 共有1船
		---------------------------------------------------------------------------*/
		public class ShareShip
		{
			private string					m_name;				// 名前
			private Point					m_pos;				// 位置
			private State					m_state;			// 状態
			private Vector2					m_angle_vector;		// 移動ベクトル

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public string Name{				get{	return m_name;				}}
			public Point Position{			get{	return m_pos;				}}
			public State State{				get{	return m_state;				}}
			public Vector2 AngleVector{		get{	return m_angle_vector;		}
											set{	m_angle_vector	= value;	}}

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public ShareShip(string name, Point pos, State _state)
			{
				m_name				= name;
				m_pos				= pos;
				m_state				= _state;
				m_angle_vector		= new Vector2(0, 0);
			}
		}

		/*-------------------------------------------------------------------------
		 共有1船
		 バウンディングボックス付き
		---------------------------------------------------------------------------*/
		public class ShareShipListBB : D3dBB2d
		{
			private List<ShareShip>		m_list;

			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public List<ShareShip>		List		{	get{	return m_list;		}}
	
			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public ShareShipListBB()
			{
				m_list			= new List<ShareShip>();
				// 適当なオフセット
				base.OffsetLT	= new Vector2(-64, -8);
				base.OffsetRB	= new Vector2( 64,  16);
			}
			/*-------------------------------------------------------------------------
			 
			---------------------------------------------------------------------------*/
			public bool Add(ShareShip p, LoopXImage image)
			{
				Vector2	pos		= transform.game_pos2_map_pos(transform.ToVector2(p.Position), image);
				Vector2	size	= base.IfUpdate(pos).Size;
				if(size.X > BB_ONCE_SIZE)	return false;
				if(size.Y > BB_ONCE_SIZE)	return false;

				// 追加
				m_list.Add(p);
				base.Update(pos);
				return true;		// 追加した
			}
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		private gvt_lib						m_lib;

		// 情報取得はスレッドのため、ダブルバッファで行う
		private bool						m_share_side;
		private List<ShareShip>				m_share1;			// 共有情報
		private List<ShareShip>				m_share2;			// 共有情報
		private List<ShareShipListBB>		m_share_bb1;		// 共有情報(BB付き)
		private List<ShareShipListBB>		m_share_bb2;		// 共有情報(BB付き)

		private bool						m_req_textured_font_reflesh;	// テクスチャフォントのリフレッシュリクエスト
		private int							m_network_access_count;			// 共有情報を得たら+1
	
		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		// 描画用
		public List<ShareShip> ShareList{					get{	return (m_share_side)? m_share1 : m_share2;				}}
		public List<ShareShipListBB> ShareListBB{			get{	return (m_share_side)? m_share_bb1 : m_share_bb2;		}}
		// 更新用
		private List<ShareShip> ShareUpdateList{			get{	return (m_share_side)? m_share2 : m_share1;				}}
		private List<ShareShipListBB> ShareUpdateListBB{	get{	return (m_share_side)? m_share_bb2 : m_share_bb1;		}}
	
		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public ShareRoutes(gvt_lib lib)
		{
			m_lib							= lib;

			m_share1						= new List<ShareShip>();
			m_share2						= new List<ShareShip>();
			m_share_bb1						= new List<ShareShipListBB>();
			m_share_bb2						= new List<ShareShipListBB>();
			m_share_side					= false;
			m_req_textured_font_reflesh		= false;
			m_network_access_count			= 0;
		}

		/*-------------------------------------------------------------------------
		 共有情報の取得と設定
		 スレッド対応
		---------------------------------------------------------------------------*/
		public void Share()
		{
			string	data	= HttpDownload.Download(def.URL_HP_ORIGINAL + @"/getallpos.php", Encoding.GetEncoding("shift_jis"));
			if(data == null)	return;

			update_list(data);
		}
			
		/*-------------------------------------------------------------------------
		 共有情報の取得と設定
		 スレッド対応
		---------------------------------------------------------------------------*/
		public void Share(int x, int y, State _state)
		{
			if(m_lib.setting.share_group == "")			return;
			if(m_lib.setting.share_group_myname == "")	return;

			string	url		= "/getposition.php?server=" + GvoDomains.GetServerString(m_lib.setting.server);
			url				+= "&group=" + Useful.UrlEncodeShiftJis(m_lib.setting.share_group);
			url				+= "&name=" + Useful.UrlEncodeShiftJis(m_lib.setting.share_group_myname);
			url				+= "&x=" + x.ToString();
			url				+= "&y=" + y.ToString();
			url				+= "&out=" + ((int)_state).ToString();

			string	data	= HttpDownload.Download(def.URL_HP_ORIGINAL + url, Encoding.GetEncoding("shift_jis"));
			if(data == null)	return;

			update_list(data);
		}

		/*-------------------------------------------------------------------------
		 共有情報の取得と設定
		 スレッド対応
		---------------------------------------------------------------------------*/
		public void update_list(string data)
		{
			List<ShareShip>	update_list	= ShareUpdateList;

			// 削除
			update_list.Clear();

			string[]	split	= data.Split(new string[]{"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);
			foreach(string s in split){
				if(s == "<html>")		continue;
				if(s == "</html>")		break;

				string[] split2	= s.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
				if(split2.Length < 4)							continue;
				if(split2[0] == m_lib.setting.share_group_myname)	continue;	// 自分自身

				try{
					int		x		= Convert.ToInt32(split2[1]);
					int		y		= Convert.ToInt32(split2[2]);
					int		_state	= Convert.ToInt32(split2[3]);
					update_list.Add(new ShareShip(split2[0], new Point(x, y), State.outof_sea + _state));
				}catch{
				}
			}

			// 移動ベクトルを得る
			update_move_angle();

			// BBリストを更新する
			update_bb_list();
	
			// 描画用にフリップ
			flip();

			// テクスチャフォントリフレッシュリクエスト
			if(++m_network_access_count >= TEXTURED_FONT_REFLESH_INTERVAL){
				m_req_textured_font_reflesh		= true;
				m_network_access_count			= 0;
			}
		}

		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		public void Draw()
		{
			// リフレッシュリクエスト
			if(m_req_textured_font_reflesh){
				m_lib.device.textured_font.Clear();
				m_req_textured_font_reflesh	= false;
			}
	
			// 描画フラグが立ってないときは描画しない
			if(!m_lib.setting.draw_share_routes)		return;

			// 角度
			m_lib.device.device.RenderState.ZBufferEnable	= false;
			m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(draw_angle_proc), 64f);
	
			// 名前
			m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(draw_name_proc), 256f);
			m_lib.device.device.RenderState.ZBufferEnable	= true;

			// アイコン
			m_lib.device.sprites.BeginDrawSprites(m_lib.icons.texture);
			m_lib.loop_image.EnumDrawCallBack(new LoopXImage.DrawHandler(draw_proc), 32f);
			m_lib.device.sprites.EndDrawSprites();
		}

		/*-------------------------------------------------------------------------
		 描画
		---------------------------------------------------------------------------*/
		private void draw_proc(Vector2 offset, LoopXImage image)
		{
			List<ShareShipListBB>	list	= ShareListBB;

			D3dBB2d.CullingRect	rect	= new D3dBB2d.CullingRect(image.Device.client_size);
			foreach(ShareShipListBB bb in list){
				// バウンディングボックスで画面外かどうか調べる
				if(bb.IsCulling(offset, image.ImageScale, rect))	continue;

#if DRAW_POPUPS_BOUNDINGBOX
				d3d_bb2d.Draw(bb.bb, image.device, 0.5f, offset, image.scale, Color.Red.ToArgb());
#endif
				foreach(ShareRoutes.ShareShip s in bb.List){
					if(s.Position.X < 0)		continue;
					if(s.Position.Y < 0)		continue;
		
					// 地図座標に変換
					Vector2	pos0	= transform.game_pos2_map_pos(transform.ToVector2(s.Position), image);
					Vector2 pos		= image.GlobalPos2LocalPos(pos0, offset);

					// 船
					icons.icon_index	index	= (s.State == State.in_the_sea)? icons.icon_index.myship: icons.icon_index.share_city;
					m_lib.device.sprites.AddDrawSprites(new Vector3(pos.X, pos.Y, 0.31f), m_lib.icons.GetIcon(index));
				}
			}
		}

		/*-------------------------------------------------------------------------
		 描画 名前
		---------------------------------------------------------------------------*/
		private void draw_name_proc(Vector2 offset, LoopXImage image)
		{
			List<ShareShipListBB>	list		= ShareListBB;

			D3dBB2d.CullingRect	crect	= new D3dBB2d.CullingRect(image.Device.client_size);
			d3d_textured_font	font		= m_lib.device.textured_font;
			foreach(ShareShipListBB bb in list){
				// バウンディングボックスで画面外かどうか調べる
				if(bb.IsCulling(offset, image.ImageScale, crect))	continue;

				foreach(ShareRoutes.ShareShip s in bb.List){
					if(s.Position.X < 0)		continue;
					if(s.Position.Y < 0)		continue;

					// 地図座標に変換
					Vector2	pos0	= transform.game_pos2_map_pos(new Vector2(s.Position.X, s.Position.Y), image);
					Vector2 pos		= image.GlobalPos2LocalPos(pos0, offset);

					// 名前
					Rectangle	rect	= font.MeasureText(s.Name, Color.Black);
					rect.Width	+= 4;
					rect.Height	+= 4;

					int	halh_x	= rect.Width / 2;
					Vector3		p		= new Vector3(pos.X - halh_x - 2, pos.Y - 3, 0.31f);
					Vector2		size	= new Vector2(rect.Width, rect.Height);
					m_lib.device.DrawFillRect(p, size, Color.FromArgb(128, 255, 255, 255).ToArgb());
					m_lib.device.DrawLineRect(p, size, Color.Gray.ToArgb());
					font.DrawTextC(s.Name, new Vector3(pos.X, pos.Y+1, 0.31f), Color.Black);
				}
			}
		}

		/*-------------------------------------------------------------------------
		 描画 角度
		---------------------------------------------------------------------------*/
		private void draw_angle_proc(Vector2 offset, LoopXImage image)
		{
			List<ShareShip>	list	= ShareList;

			m_lib.device.line.Width			= 1;
			m_lib.device.line.Antialias		= m_lib.setting.enable_line_antialias;
			m_lib.device.line.Pattern		= -1;
			m_lib.device.line.PatternScale	= 1.0f;
			m_lib.device.line.Begin();
	
			foreach(ShareRoutes.ShareShip s in list){
				if(s.Position.X < 0)			continue;
				if(s.Position.Y < 0)			continue;
				if(s.AngleVector.LengthSq() < 0.5f)	continue;
	
				// 地図座標に変換
				Vector2	pos0	= transform.game_pos2_map_pos(new Vector2(s.Position.X, s.Position.Y), image);
				Vector2 pos		= image.GlobalPos2LocalPos(pos0, offset);

				// クランプ
				if(pos.X + ANGLE_LINE_LENGTH < 0)							continue;
				if(pos.X - ANGLE_LINE_LENGTH >= image.Device.client_size.X)	continue;
				if(pos.Y + ANGLE_LINE_LENGTH < 0)							continue;
				if(pos.Y - ANGLE_LINE_LENGTH >= image.Device.client_size.Y)	continue;
	
				Vector2[]	points	= new Vector2[2];
				points[0]	= pos;
				points[1]	= pos + (s.AngleVector * ANGLE_LINE_LENGTH);
				m_lib.device.line.Draw(points, Color.FromArgb(200, 64, 64, 64));
			}
			m_lib.device.line.End();
		}

		/*-------------------------------------------------------------------------
		 flip
		---------------------------------------------------------------------------*/
		private void flip()
		{
			m_share_side	= (m_share_side)? false: true;
		}

		/*-------------------------------------------------------------------------
		 移動ベクトルを得る
		 BB用の矩形も更新する
		---------------------------------------------------------------------------*/
		private void update_move_angle()
		{
			List<ShareShip>	update_list	= ShareUpdateList;

			foreach(ShareRoutes.ShareShip s in update_list){
				ShareRoutes.ShareShip	old	= find_ship(s.Name);
				if(old == null)		continue;		// 前回の情報が見当たらない

				// 移動ベクトルを得る
				Vector2	vec	= transform.SubVector_LoopX(s.Position, old.Position, def.GAME_WIDTH);
				vec.Normalize();
				if(vec.LengthSq() > 0.5f)	s.AngleVector		= vec;
				else						s.AngleVector		= new Vector2(0, 0);
			}
		}
	
		/*-------------------------------------------------------------------------
		 BBリストを更新する
		---------------------------------------------------------------------------*/
		private void update_bb_list()
		{
			List<ShareShip>			update_list		= ShareUpdateList;
			List<ShareShipListBB>	update_bb_list	= ShareUpdateListBB;

			// BBリストをクリア
			update_bb_list.Clear();

			List<ShareShip>			free_list		= new List<ShareShip>();
			foreach(ShareShip s in update_list)	free_list.Add(s);
	
			// 適当に距離が近いグループを作る
			while(free_list.Count > 0){
				update_bb_list.Add(create_bb(ref free_list));
			}
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		private ShareShipListBB create_bb(ref List<ShareShip> free_list)
		{
			List<ShareShip>		old		= free_list;
			free_list				= new List<ShareShip>();

			ShareShipListBB	bb	= new ShareShipListBB();
			foreach(ShareShip i in old){
				if(!bb.Add(i, m_lib.loop_image)){
					// 追加されなかったら次回に回す
					free_list.Add(i);
				}
			}
			return bb;
		}

		/*-------------------------------------------------------------------------
		 前回の情報を得る
		 名前が同一の前回の情報を得る
		---------------------------------------------------------------------------*/
		private ShareShip find_ship(string name)
		{
			List<ShareShip>	list	= ShareList;
			foreach(ShareRoutes.ShareShip s in list){
				if(s.Name == name)	return s;
			}
			return null;
		}
	}
}
