﻿/*-------------------------------------------------------------------------

 Direct3D
 スプライト
 1枚のテクスチャから大量に描画する目的用
 シェーダが使えるときはシェーダを使用する

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace directx
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class d3d_sprite : IDisposable
	{
		private const int				DRAW_SPRITE_ONCE	= 128;
		private const int				SWAP_BFFER_MAX		= 16;		// シェーダ時のバッファ数

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		private Device								m_d3d_device;

		private CustomVertex.TransformedColoredTextured[]	m_vertex_list;
		private int									m_sprite_count;
		private Texture								m_texture;			// 一時的に保持するテクスチャ

		private Vector2								m_map_offset;
		private float								m_map_scale;
		private Vector2								m_global_scale;

		// 現在のフレームに描画したスプライト数
		private int									m_draw_sprites_in_frame;

		// シェーダ使用時
		private Effect								m_effect;
		private VertexDeclaration					m_decl;
		private d3d_writable_vb_with_index			m_vb;
		private sprite_vertex[]						m_vbo;
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public int draw_sprites_in_frame		{	get{	return m_draw_sprites_in_frame;	}}
		// 外部で特別にシェーダを使用するとき用
		// 通常は得る必要がない
		public Effect effect					{	get{	return m_effect;				}}

		/*-------------------------------------------------------------------------
		 フレームの開始
		---------------------------------------------------------------------------*/
		public void BeginFrame()
		{
			m_draw_sprites_in_frame		= 0;
		}

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public d3d_sprite(Device device, bool is_use_ve1_1_ps1_1)
		{
			m_d3d_device	= device;

			// バッファ
			m_vertex_list	= new CustomVertex.TransformedColoredTextured[DRAW_SPRITE_ONCE * 6];
			m_sprite_count	= 0;

			// シェーダ使用時
			m_effect		= null;
			m_decl			= null;
			m_vb			= null;
			m_vbo			= null;

			// シェーダが使えるなら使う
			if(is_use_ve1_1_ps1_1){
				Assembly	ass		= Assembly.GetExecutingAssembly();
				initialize_shader(ass.GetManifestResourceStream("directx.fx.sprite.fxo"));
			}
			BeginFrame();
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public void Dispose()
		{
			DisposeEffect();
		}

		/*-------------------------------------------------------------------------
		 エフェクトの解放
		---------------------------------------------------------------------------*/
		public void DisposeEffect()
		{
			if(m_effect != null)	m_effect.Dispose();
			if(m_decl != null)		m_decl.Dispose();
			if(m_vb != null)		m_vb.Dispose();
			m_effect	= null;
			m_decl		= null;
			m_vb		= null;
		}
	
		/*-------------------------------------------------------------------------
		 シェーダ使用初期化
		 スプライト用エフェクトを読み込んで初期化される
		 .fxでも.fxoでもよい
		---------------------------------------------------------------------------*/
/*
		private void initialize_shader(string fname)
		{
			string	error;
			try{
				m_effect	= Effect.FromFile(	m_d3d_device, fname,
												null, null,
												ShaderFlags.None,
												null, out error);
				initialize_shader();
			}catch{
				m_effect	= null;
			}
		}
*/	
		/*-------------------------------------------------------------------------
		 シェーダ使用初期化
		 スプライト用エフェクトを読み込んで初期化される
		 埋め込みリソースからの読み込み
		 .fxoであること
		---------------------------------------------------------------------------*/
		private void initialize_shader(Stream stream)
		{
			string	error;
			try{
				m_effect	= Effect.FromStream(m_d3d_device, stream,
												null, null,
												ShaderFlags.None,
												null, out error);
				initialize_shader();
			}catch{
				m_effect	= null;
			}
		}
	
		/*-------------------------------------------------------------------------
		 シェーダ使用用初期化
		 Effectは設定済みであること
		---------------------------------------------------------------------------*/
		private void initialize_shader()
		{
			try{
				m_decl				= new VertexDeclaration(m_d3d_device, sprite_vertex.VertexElements);
				m_vb				= new d3d_writable_vb_with_index(
													m_d3d_device,
													typeof(sprite_vertex),
													DRAW_SPRITE_ONCE * 4,
													SWAP_BFFER_MAX);
				m_vbo				= new sprite_vertex[DRAW_SPRITE_ONCE * 4];
			}catch{
				// 失敗
				m_effect			= null;
				m_decl				= null;
				m_vb				= null;
				m_vbo				= null;
			}
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画開始
		 同じテクスチャを使用するスプライト描画用
		---------------------------------------------------------------------------*/
		public void BeginDrawSprites(Texture tex)
		{
			// シェーダー使用用設定
			begin_draw_sprites_shader(tex);

			if(m_effect == null){
				// シェーダー未使用用設定
				m_texture		= tex;
				m_map_offset	= new Vector2(0,0);
				m_map_scale		= 1;
				m_global_scale	= new Vector2(1,1);
			}
			m_sprite_count	= 0;
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画開始
		 同じテクスチャを使用するスプライト描画用
		---------------------------------------------------------------------------*/
		private void begin_draw_sprites_shader(Texture tex)
		{
			if(m_effect == null)	return;

			try{
				// シェーダ定数を設定します。
				float[]	vector	= {	m_d3d_device.Viewport.Width, m_d3d_device.Viewport.Height };
				m_effect.SetValue("ViewportSize", vector);
				m_effect.SetValue("Texture", tex);

				// Technique
				// 通常のスプライト
				m_effect.Technique	= "Sprite";
			}catch{
				// 失敗
				// シェーダーの使用をやめる
				DisposeEffect();
			}
		}
	
		/*-------------------------------------------------------------------------
		 スプライトの描画開始
		 同じテクスチャを使用するスプライト描画用
		 グローバルパラメータ指定
		---------------------------------------------------------------------------*/
		public void BeginDrawSprites(Texture tex, Vector2 map_offset, float map_scale, Vector2 global_scale)
		{
			// シェーダー使用用設定
			begin_draw_sprites_shader(tex, map_offset, map_scale, global_scale);

			if(m_effect == null){
				// シェーダー未使用用設定
				m_texture		= tex;
				m_map_offset	= map_offset;
				m_map_scale		= map_scale;
				m_global_scale	= global_scale;
			}
			m_sprite_count	= 0;
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画開始
		 同じテクスチャを使用するスプライト描画用
		 グローバルパラメータ指定
		---------------------------------------------------------------------------*/
		public void begin_draw_sprites_shader(Texture tex, Vector2 map_offset, float map_scale, Vector2 global_scale)
		{
			if(m_effect == null)	return ;

			try{
				// シェーダ定数を設定します。
				float[]	vector	= {	m_d3d_device.Viewport.Width, m_d3d_device.Viewport.Height };
				float[] _offset	= { map_offset.X, map_offset.Y };
				float[] gscale	= { global_scale.X, global_scale.Y };
				m_effect.SetValue("ViewportSize", vector);
				m_effect.SetValue("Texture", tex);
				m_effect.SetValue("MapOffset", _offset);
				m_effect.SetValue("MapScale", map_scale);
				m_effect.SetValue("GlobalScale", gscale);

				// technique
				m_effect.Technique	= "SpriteWithGlobalParams";		// グローバルパラメータと拡縮と回転付き
			}catch{
				// 失敗
				// シェーダーの使用をやめる
				DisposeEffect();
			}
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		private Vector3 goffsetscale(Vector3 p)
		{
			p.X		+= m_map_offset.X;
			p.Y		+= m_map_offset.Y;
			p.X		*= m_map_scale;
			p.Y		*= m_map_scale;
			return p;
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		private Vector2 gscale(Vector2 scale)
		{
			scale.X		*= m_global_scale.X;
			scale.Y		*= m_global_scale.Y;
			return scale;
		}

//#if aaa
		/*-------------------------------------------------------------------------
		 スプライトの描画 追加
		 等倍、白、不透明
		---------------------------------------------------------------------------*/
		public bool AddDrawSprites(Vector3 pos, d3d_sprite_rects.rect _rect)
		{
			return AddDrawSprites(pos, _rect, -1);
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画 追加
		 等倍、色指定
		---------------------------------------------------------------------------*/
		public bool AddDrawSprites(Vector3 pos, d3d_sprite_rects.rect _rect, int color)
		{
			return AddDrawSprites(pos, _rect, color, new Vector2(0,0));
		}
		public bool AddDrawSprites(Vector3 pos, d3d_sprite_rects.rect _rect, int color, Vector2 offset2)
		{
			if(m_effect != null){
				// シェーダ使用
				add_sprite_to_vertex_buffer_shader(pos, _rect, 0, new Vector2(1,1), color, offset2);
			}else{
				pos					= goffsetscale(pos);
				Vector2 size		= gscale(_rect.size);

				// 整数化
				pos.X				= (float)(int)pos.X;
				pos.Y				= (float)(int)pos.Y;
				pos.X				+= offset2.X;
				pos.Y				+= offset2.Y;

				pos.X				+= _rect.lefttop.X;
				pos.Y				+= _rect.lefttop.Y;

				// カリング
				if(pos.X >= m_d3d_device.Viewport.Width+m_d3d_device.Viewport.X)	return false;
				if(pos.Y >= m_d3d_device.Viewport.Height+m_d3d_device.Viewport.Y)	return false;
				if(pos.X + size.X < 0)												return false;
				if(pos.Y + size.Y < 0)												return false;

				// 追加
				add_sprite_to_vertex_buffer(pos, size, _rect, color);
			}

			if(m_sprite_count >= DRAW_SPRITE_ONCE){
				draw_sprites();
			}
			return true;
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画 追加
		 スケール指定
		---------------------------------------------------------------------------*/
		public bool AddDrawSprites(Vector3 pos, d3d_sprite_rects.rect _rect, Vector2 scale)
		{
			return AddDrawSprites(pos, _rect, scale, -1);
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画 追加
		 スケール指定、色指定
		---------------------------------------------------------------------------*/
		public bool AddDrawSprites(Vector3 pos, d3d_sprite_rects.rect _rect, Vector2 scale, int color)
		{
			return AddDrawSprites(pos, _rect, scale, color, new Vector2(0,0));
		}
		public bool AddDrawSprites(Vector3 pos, d3d_sprite_rects.rect _rect, Vector2 scale, int color, Vector2 offset2)
		{
			if(m_effect != null){
				// シェーダ使用
				add_sprite_to_vertex_buffer_shader(pos, _rect, 0, scale, color, offset2);
			}else{
				pos					= goffsetscale(pos);

				Vector2		offset	= _rect.lefttop;
				Vector2		size	= gscale(_rect.size);

				offset.X			*= scale.X;
				offset.Y			*= scale.Y;
				size.X				*= scale.X;
				size.Y				*= scale.Y;

				// 整数化
				pos.X				= (float)(int)pos.X;
				pos.Y				= (float)(int)pos.Y;
				pos.X				+= offset2.X;
				pos.Y				+= offset2.Y;

				pos.X				+= offset.X;
				pos.Y				+= offset.Y;

				// カリング
				if(pos.X >= m_d3d_device.Viewport.Width+m_d3d_device.Viewport.X)	return false;
				if(pos.Y >= m_d3d_device.Viewport.Height+m_d3d_device.Viewport.Y)	return false;
				if(pos.X + size.X < 0)												return false;
				if(pos.Y + size.Y < 0)												return false;

				// 追加
				add_sprite_to_vertex_buffer(pos, size, _rect, color);
			}
			if(m_sprite_count >= DRAW_SPRITE_ONCE){
				draw_sprites();
			}
			return true;
		}
//#endif
		/*-------------------------------------------------------------------------
		 スプライトの描画 追加
		 等倍、白、不透明
		 画面外カリングなし
		---------------------------------------------------------------------------*/
		public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect)
		{
			return AddDrawSpritesNC(pos, _rect, -1);
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画 追加
		 等倍、色指定
		 画面外カリングなし
		---------------------------------------------------------------------------*/
		public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, int color)
		{
			return AddDrawSpritesNC(pos, _rect, color, new Vector2(0,0));
		}
		public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, int color, Vector2 offset2)
		{
			if(m_effect != null){
				// シェーダ使用
				add_sprite_to_vertex_buffer_shader(pos, _rect, 0, new Vector2(1,1), color, offset2);
			}else{
				pos					= goffsetscale(pos);
				Vector2	size		= gscale(_rect.size);

				// 整数化
				pos.X				= (float)(int)pos.X;
				pos.Y				= (float)(int)pos.Y;
				pos.X				+= offset2.X;
				pos.Y				+= offset2.Y;

				pos.X				+= _rect.lefttop.X;
				pos.Y				+= _rect.lefttop.Y;

				// 追加
				add_sprite_to_vertex_buffer(pos, size, _rect, color);
			}
			if(m_sprite_count >= DRAW_SPRITE_ONCE){
				draw_sprites();
			}
			return true;
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画 追加
		 スケール指定
		 画面外カリングなし
		---------------------------------------------------------------------------*/
		public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, Vector2 scale)
		{
			return AddDrawSpritesNC(pos, _rect, scale, -1);
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画 追加
		 スケール指定、色指定
		 画面外カリングなし
		---------------------------------------------------------------------------*/
		public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, Vector2 scale, int color)
		{
			return AddDrawSpritesNC(pos, _rect, scale, color, new Vector2(0,0));
		}
		public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, Vector2 scale, int color, Vector2 offset2)
		{
			if(m_effect != null){
				// シェーダ使用
				add_sprite_to_vertex_buffer_shader(pos, _rect, 0, scale, color, offset2);
			}else{
				pos					= goffsetscale(pos);

				Vector2		offset	= _rect.lefttop;
				Vector2		size	= gscale(_rect.size);

				offset.X			*= scale.X;
				offset.Y			*= scale.Y;
				size.X				*= scale.X;
				size.Y				*= scale.Y;

				pos.X				+= offset.X;
				pos.Y				+= offset.Y;
				// 整数化
				pos.X				= (float)(int)pos.X;
				pos.Y				= (float)(int)pos.Y;
				pos.X				+= offset2.X;
				pos.Y				+= offset2.Y;

				// 追加
				add_sprite_to_vertex_buffer(pos, size, _rect, color);
			}

			if(m_sprite_count >= DRAW_SPRITE_ONCE){
				draw_sprites();
			}
			return true;
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画終了
		---------------------------------------------------------------------------*/
		public void EndDrawSprites()
		{
			if(m_sprite_count > 0){
				draw_sprites();
			}
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画
		 ある程度まとまった数を描く
		 テクスチャは設定済みであること
		---------------------------------------------------------------------------*/
		private void draw_sprites()
		{
			if(m_effect != null){
				// シェーダ使用
				m_vb.SetData<sprite_vertex>(m_vbo);		// 頂点データ設定
//				m_vb.SetData<sprite_vertex>(m_vbo, m_sprite_count * 4);		// なぜか重いのでやめ

				m_effect.Begin(0);
				m_effect.BeginPass(0);

				m_d3d_device.VertexDeclaration	= m_decl;
				m_d3d_device.SetStreamSource(0, m_vb.vb, 0, sprite_vertex.SizeInBytes);
				m_d3d_device.Indices			= m_vb.ib;
				m_d3d_device.DrawIndexedPrimitives(PrimitiveType.TriangleList,
											0, 0, m_sprite_count * 4, 0, m_sprite_count*2);
				m_effect.EndPass();
				m_effect.End();
				m_vb.Flip();			// 次のバッファへ
			}else{
				// 通常
				// draw primitives
				m_d3d_device.SetTexture(0, m_texture);
				m_d3d_device.VertexFormat	= CustomVertex.TransformedColoredTextured.Format;
				m_d3d_device.DrawUserPrimitives(PrimitiveType.TriangleList, m_sprite_count * 2, m_vertex_list);
			}
			m_sprite_count	= 0;
		}

		/*-------------------------------------------------------------------------
		 スプライトの追加
		 vertex bufferに追加
		---------------------------------------------------------------------------*/
		private void add_sprite_to_vertex_buffer(Vector3 pos, Vector2 size, d3d_sprite_rects.rect _rect, int color)
		{
			int			index	= m_sprite_count * 6;

			pos.X			-= 0.5f;
			pos.Y			-= 0.5f;

			for(int i=0; i<6; i++){
				m_vertex_list[index + i].X		= pos.X;
				m_vertex_list[index + i].Y		= pos.Y;
				m_vertex_list[index + i].Z		= pos.Z;
				m_vertex_list[index + i].Rhw	= 1;
				m_vertex_list[index + i].Color	= color;
			}

			// 左上三角形
			m_vertex_list[index + 1].X		+= size.X;
			m_vertex_list[index + 2].Y		+= size.Y;

			// 
			m_vertex_list[index + 0].Tu		= _rect.uv[0].X;
			m_vertex_list[index + 0].Tv		= _rect.uv[0].Y;
			m_vertex_list[index + 1].Tu		= _rect.uv[1].X;
			m_vertex_list[index + 1].Tv		= _rect.uv[1].Y;
			m_vertex_list[index + 2].Tu		= _rect.uv[2].X;
			m_vertex_list[index + 2].Tv		= _rect.uv[2].Y;

			// 右下三角形
			m_vertex_list[index + 3].X		+= size.X;
			m_vertex_list[index + 4].X		+= size.X;
			m_vertex_list[index + 4].Y		+= size.Y;
			m_vertex_list[index + 5].Y		+= size.Y;

			// 
			m_vertex_list[index + 3].Tu		= _rect.uv[1].X;
			m_vertex_list[index + 3].Tv		= _rect.uv[1].Y;
			m_vertex_list[index + 4].Tu		= _rect.uv[3].X;
			m_vertex_list[index + 4].Tv		= _rect.uv[3].Y;
			m_vertex_list[index + 5].Tu		= _rect.uv[2].X;
			m_vertex_list[index + 5].Tv		= _rect.uv[2].Y;

			m_sprite_count++;
			m_draw_sprites_in_frame++;
		}
	
		/*-------------------------------------------------------------------------
		 スプライトの追加
		 シェーダ使用
		---------------------------------------------------------------------------*/
		private void add_sprite_to_vertex_buffer_shader(Vector3 pos, d3d_sprite_rects.rect _rect, float angle_rad, Vector2 scale, int color, Vector2 offset)
		{
			int	index	= m_sprite_count * 4;

			Vector3		param			= new Vector3(scale.X, scale.Y, angle_rad);
			for(int i=0; i<4; i++){
				m_vbo[index + i].color		= color;
				m_vbo[index + i].Position	= pos;
				m_vbo[index + i].offset1	= _rect.offset[i];
				m_vbo[index + i].offset2	= offset;
				m_vbo[index + i].param		= param;
				m_vbo[index + i].uv			= _rect.uv[i];
			}

			m_sprite_count++;
			m_draw_sprites_in_frame++;
		}
		
		/*-------------------------------------------------------------------------
		 スプライトの描画 追加
		 回転指定
		 角度はラジアン
		 画面外カリングなし
		 回転指定は画面外カリングなしのみ
		---------------------------------------------------------------------------*/
		public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, float angle_rad)
		{
			return AddDrawSpritesNC(pos, _rect, angle_rad, new Vector2(1,1), -1);
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画 追加
		 スケール指定、回転指定
		 角度はラジアン
		 画面外カリングなし
		 回転指定は画面外カリングなしのみ
		---------------------------------------------------------------------------*/
		public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, float angle_rad, Vector2 scale)
		{
			return AddDrawSpritesNC(pos, _rect, angle_rad, scale, -1);
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画 追加
		 色指定、回転指定
		 角度はラジアン
		 画面外カリングなし
		 回転指定は画面外カリングなしのみ
		---------------------------------------------------------------------------*/
		public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, float angle_rad, int color)
		{
			return AddDrawSpritesNC(pos, _rect, angle_rad, new Vector2(1,1), color);
		}

		/*-------------------------------------------------------------------------
		 スプライトの描画 追加
		 スケール指定、色指定、回転指定
		 角度はラジアン
		 画面外カリングなし
		 回転指定は画面外カリングなしのみ
		---------------------------------------------------------------------------*/
		public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, float angle_rad, Vector2 scale, int color)
		{
			return AddDrawSpritesNC(pos, _rect, angle_rad, scale, color, new Vector2(0,0));
		}
		public bool AddDrawSpritesNC(Vector3 pos, d3d_sprite_rects.rect _rect, float angle_rad, Vector2 scale, int color, Vector2 offset2)
		{
			if(m_effect != null){
				// シェーダ使用
				add_sprite_to_vertex_buffer_shader(pos, _rect, angle_rad, scale, color, offset2);
			}else{
				pos					= goffsetscale(pos);
				Vector2	scale2		= gscale(scale);

				Matrix		mat		= Matrix.RotationZ(angle_rad);			// Z軸回転
				mat			= Matrix.Scaling(scale2.X, scale2.Y, 1) * mat;	// 拡大縮小

				Vector3		p0		= new Vector3(_rect.offset[0].X, _rect.offset[0].Y, 0);
				Vector3		p1		= new Vector3(_rect.offset[1].X, _rect.offset[1].Y, 0);
				Vector3		p2		= new Vector3(_rect.offset[2].X, _rect.offset[2].Y, 0);
				Vector3		p3		= new Vector3(_rect.offset[3].X, _rect.offset[3].Y, 0);

				// 座標変換
				p0.TransformCoordinate(mat);
				p1.TransformCoordinate(mat);
				p2.TransformCoordinate(mat);
				p3.TransformCoordinate(mat);

				int			index	= m_sprite_count * 6;

				pos.X			= (int)pos.X;
				pos.Y			= (int)pos.Y;
				pos.X			+= offset2.X;
				pos.Y			+= offset2.Y;

				pos.X			-= 0.5f;
				pos.Y			-= 0.5f;

				for(int i=0; i<6; i++){
					m_vertex_list[index + i].X		= pos.X;
					m_vertex_list[index + i].Y		= pos.Y;
					m_vertex_list[index + i].Z		= pos.Z;
					m_vertex_list[index + i].Rhw	= 1;
					m_vertex_list[index + i].Color	= color;
				}

				// 左上三角形
				m_vertex_list[index + 0].X		+= p0.X;
				m_vertex_list[index + 0].Y		+= p0.Y;
				m_vertex_list[index + 1].X		+= p1.X;
				m_vertex_list[index + 1].Y		+= p1.Y;
				m_vertex_list[index + 2].X		+= p2.X;
				m_vertex_list[index + 2].Y		+= p2.Y;

				// 
				m_vertex_list[index + 0].Tu		= _rect.uv[0].X;
				m_vertex_list[index + 0].Tv		= _rect.uv[0].Y;
				m_vertex_list[index + 1].Tu		= _rect.uv[1].X;
				m_vertex_list[index + 1].Tv		= _rect.uv[1].Y;
				m_vertex_list[index + 2].Tu		= _rect.uv[2].X;
				m_vertex_list[index + 2].Tv		= _rect.uv[2].Y;

				// 右下三角形
				m_vertex_list[index + 3].X		+= p1.X;
				m_vertex_list[index + 3].Y		+= p1.Y;
				m_vertex_list[index + 4].X		+= p3.X;
				m_vertex_list[index + 4].Y		+= p3.Y;
				m_vertex_list[index + 5].X		+= p2.X;
				m_vertex_list[index + 5].Y		+= p2.Y;

				// 
				m_vertex_list[index + 3].Tu		= _rect.uv[1].X;
				m_vertex_list[index + 3].Tv		= _rect.uv[1].Y;
				m_vertex_list[index + 4].Tu		= _rect.uv[3].X;
				m_vertex_list[index + 4].Tv		= _rect.uv[3].Y;
				m_vertex_list[index + 5].Tu		= _rect.uv[2].X;
				m_vertex_list[index + 5].Tv		= _rect.uv[2].Y;

				m_sprite_count++;
				m_draw_sprites_in_frame++;
			}

			if(m_sprite_count >= DRAW_SPRITE_ONCE){
				draw_sprites();
			}
			return true;
		}

		/*-------------------------------------------------------------------------
		 シェーダ用頂点
		---------------------------------------------------------------------------*/
		[StructLayout( LayoutKind.Sequential )]
		protected struct sprite_vertex
		{
			public Vector3  Position;   // 座標					12
			public Vector2	uv;			// uv					8
			public Vector2	offset1;	// x, y		offset1		8
			public Vector2	offset2;	// x, y		offset2		8
			public Vector3	param;		// x, y		scale		12
										// z		angle_rad
			public int		color;		// color				4

			/*-------------------------------------------------------------------------
			 頂点情報
			---------------------------------------------------------------------------*/
			public static VertexElement[] VertexElements = {
				// Position (12バイト)
				new VertexElement(0, 0, DeclarationType.Float3,
										DeclarationMethod.Default,
										DeclarationUsage.Position, 0),
				// uv (8バイト)
				new VertexElement(0, 12, DeclarationType.Float2,
										DeclarationMethod.Default,
										DeclarationUsage.TextureCoordinate, 0),
				// offset1 offset2
				new VertexElement(0, 12+8, DeclarationType.Float4,
										DeclarationMethod.Default,
										DeclarationUsage.TextureCoordinate, 1),
				// scale angle_rad
				new VertexElement(0, 12+8+16, DeclarationType.Float3,
										DeclarationMethod.Default,
										DeclarationUsage.TextureCoordinate, 2),

				// Color  (4バイト)
				new VertexElement(0, 12+8+16+12, DeclarationType.Color,
										DeclarationMethod.Default,
										DeclarationUsage.Color, 0),
				VertexElement.VertexDeclarationEnd,
			};

			/*-------------------------------------------------------------------------
			 頂点1つのサイズ
			---------------------------------------------------------------------------*/
			public static int SizeInBytes
			{
				get { return Marshal.SizeOf( typeof( sprite_vertex ) ); }
			}
		}
	}
}
