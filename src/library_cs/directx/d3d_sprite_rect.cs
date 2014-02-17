﻿/*-------------------------------------------------------------------------

 矩形切り取り型スプライト

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.IO;
using System.Diagnostics;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace directx
{
	public class d3d_sprite_rects :IDisposable
	{
		/*-------------------------------------------------------------------------
		 矩形1つ
		---------------------------------------------------------------------------*/
		public class rect
		{
			// 
			// 0-1
			// | |
			// 2-3
			private Vector2[]			m_offset;
			private Vector2[]			m_uv;

			private	Vector2				m_lefttop;		// 左上のオフセット
			private Vector2				m_size;			// サイズ

            private Rectangle m_rect;

            /*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public Vector2[] offset	{	get{	return m_offset;	}}
			public Vector2[] uv		{	get{	return m_uv;		}}

			public Vector2 lefttop	{	get{	return m_lefttop;	}}
			public Vector2 size		{	get{	return m_size;		}}

			/*-------------------------------------------------------------------------

			---------------------------------------------------------------------------*/
			public rect(Vector2 tex_size, Vector2 offset, Rectangle _rect)
			{
				m_lefttop		= offset;
				m_size			= new Vector2(_rect.Width, _rect.Height);
                m_rect          = _rect;

                Vector2	uv0		= new Vector2(	(float)_rect.X / tex_size.X,
												(float)_rect.Y / tex_size.Y);
				Vector2	uv1		= new Vector2(	((float)_rect.X + _rect.Width) / tex_size.X,
												((float)_rect.Y + _rect.Height) / tex_size.Y);

				// 頂点情報設定用
				// offset
				m_offset		= new Vector2[4];
				m_offset[0]		= new Vector2(offset.X,				offset.Y);
				m_offset[1]		= new Vector2(offset.X + m_size.X,	offset.Y);
				m_offset[2]		= new Vector2(offset.X,				offset.Y + m_size.Y);
				m_offset[3]		= new Vector2(offset.X + m_size.X,	offset.Y + m_size.Y);
				// uv
				m_uv		= new Vector2[4];
				m_uv[0]		= uv0;
				m_uv[1]		= new Vector2(uv1.X, uv0.Y);
				m_uv[2]		= new Vector2(uv0.X, uv1.Y);
				m_uv[3]		= uv1;
			}

            internal void DumpRects(string bmp_file_name)
            {
                Debug.WriteLine("OBJDT\ticons_0[] = {");
                Debug.WriteLine("\t{\t\"" + bmp_file_name + "\",");
                Debug.WriteLine("\t\t\t0,\t\t// PaletteName");
                Debug.WriteLine(string.Format("\t\t{0}, {1}, {2}, {3},", (object)this.m_rect.Left, (object)this.m_rect.Top, (object)this.m_rect.Right, (object)this.m_rect.Bottom));
                Debug.WriteLine(string.Format("\t\t{0}, {1},", (object)((int)this.m_lefttop.X).ToString(), (object)((int)this.m_lefttop.Y).ToString()));
                Debug.WriteLine("\t\t   0,\t   0,\t\t// Invers V , Invers H ");
                Debug.WriteLine("\t\t   1,\t\t\t\t// Term");
                Debug.WriteLine("\t\t   0,\t\t\t\t// Depth_Index");
                Debug.WriteLine("\t\t   0,\t\t// name");
                Debug.WriteLine("\t\t0,\t\t// bank");
                Debug.WriteLine("\t\t   0,\t\t\t\t// optimize");
                Debug.WriteLine("\t\t   0,\t\t\t\t// drawmode");
                Debug.WriteLine("\t\t   0,\t\t\t\t// lock");
                Debug.WriteLine("\t\t   0,\t\t\t\t// mode");
                Debug.WriteLine("\t}");
                Debug.WriteLine("};");
            }
        }
	
		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		private Texture					m_texture;				// スプライト用テクスチャ
		private	Vector2					m_texture_size;			// テクスチャサイズ
		private List<rect>				m_rects;				// 矩形情報

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public Texture	texture			{	get{	return m_texture;		}}
		public Vector2	texture_size	{	get{	return m_texture_size;	}}
		public List<rect> rects			{	get{	return m_rects;			}}
		public int rect_count			{	get{	return m_rects.Count;	}}

		/*-------------------------------------------------------------------------
		 ファイル名から構築
		 通常DDSファイルを使用すること

		 2のべき乗でない場合強制的に2のべき乗に変換されてしまうので注意
		 2のべき乗で絵を作ること
		---------------------------------------------------------------------------*/
		public d3d_sprite_rects(d3d_device device, string fname)
		{
			try{
				m_texture			= TextureLoader.FromFile(device.device, fname);
				m_texture_size		= d3d_utility.GetTextureSize(m_texture);
			}catch{
				m_texture			= null;
				m_texture_size.X	= 0;
				m_texture_size.Y	= 0;
			}
			m_rects				= new List<rect>();
		}

		/*-------------------------------------------------------------------------
		 Streamから構築
		 組み込みリソースからの構築用
		 通常DDSファイルを使用すること

		 2のべき乗でない場合強制的に2のべき乗に変換されてしまうので注意
		 2のべき乗で絵を作ること
		---------------------------------------------------------------------------*/
		public d3d_sprite_rects(d3d_device device, Stream stream)
		{
			try{
				m_texture			= TextureLoader.FromStream(device.device, stream);
				m_texture_size		= d3d_utility.GetTextureSize(m_texture);
			}catch{
				m_texture			= null;
				m_texture_size.X	= 0;
				m_texture_size.Y	= 0;
			}
			m_rects				= new List<rect>();
		}
	
		/*-------------------------------------------------------------------------
		 矩形を追加
		 矩形番号を返す
		---------------------------------------------------------------------------*/
		public int AddRect(Vector2 offset, Rectangle _rect)
		{
			m_rects.Add(new rect(m_texture_size, offset, _rect));
			return m_rects.Count -1;
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public void Dispose()
		{
			if(m_texture != null){
				m_texture.Dispose();
				m_texture	= null;
			}
			m_texture_size	= new Vector2(0, 0);
		}

		/*-------------------------------------------------------------------------
		 矩形を得る
		---------------------------------------------------------------------------*/
		public virtual d3d_sprite_rects.rect GetRect(int index)
		{
			return m_rects[index];
		}

        protected void DumpRects(string bmp_file_name)
        {
            foreach (d3d_sprite_rects.rect rect in this.m_rects)
                rect.DumpRects(bmp_file_name);
        }
    }
}
