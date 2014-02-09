/*-------------------------------------------------------------------------

 DirectX用ユーティリティ
 全てstaticメソッド

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace directx
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	static public class d3d_utility
	{
		/*-------------------------------------------------------------------------
		 テクスチャのサイズを得る
		 LevelDescription 0 のサイズを返す 
		 tex==null のとき Vector2(0,0) を返す
		---------------------------------------------------------------------------*/
		static public Vector2 GetTextureSize(Texture tex)
		{
			if(tex == null)	return new Vector2(0, 0);

			try{
				SurfaceDescription	dsc	= tex.GetLevelDescription(0);
				return new Vector2(dsc.Width, dsc.Height);
			}catch{
				return new Vector2(0, 0);
			}
		}

		/*-------------------------------------------------------------------------
		 テクスチャを作成する
		 テクスチャはManagedで作成される
		 システムメモリに作成したテクスチャを使用できるテクスチャにするのに使用する
		 bmp等から切り出したテクスチャを作成する場合は、
		 システムメモリにテクスチャを作成し、この関数でManagedに転送する
		 Managedで作成したテクスチャをロックしてはいけない
		 
		 テクスチャはできるだけDDSで作成し、TextureLoaderを使って読み込むこと
		 TextureLoaderはManagedなテクスチャを作成してくれる
		---------------------------------------------------------------------------*/
		static public Texture CreateTextureFromTexture(Device device, Texture src_texture)
		{
			SurfaceDescription	d	= src_texture.GetLevelDescription(0);
			return CreateTextureFromTexture(device, src_texture, d.Format);
		}

		/*-------------------------------------------------------------------------
		 フォーマット指定版
		 フォーマット変換は基本的には時間が掛かるので注意
		 フォーマット変換に失敗するかもしれない
		---------------------------------------------------------------------------*/
		static public Texture CreateTextureFromTexture(Device device, Texture src_texture, Format format)
		{
			if(device	== null)		return null;
			if(src_texture == null)		return null;

			try{
				// 同じサイズのテクスチャを作成する
				Texture dst_texture		= CreateTextureSameSize(device, src_texture, format);
				if(dst_texture == null){
					// 失敗
					return null;
				}

				// コピー
				if(CopyTexture(device, dst_texture, src_texture)){
					return dst_texture;
				}else{
					dst_texture.Dispose();
					return null;
				}
			}catch{
				return null;
			}
		}

		/*-------------------------------------------------------------------------
		 渡されたテクスチャと同じサイズのテクスチャを作成する
		---------------------------------------------------------------------------*/
		static public Texture CreateTextureSameSize(Device device, Texture src_texture)
		{
			SurfaceDescription	d	= src_texture.GetLevelDescription(0);
			return CreateTextureSameSize(device, src_texture, d.Format);
		}
		static public Texture CreateTextureSameSize(Device device, Texture src_texture, Pool pool)
		{
			SurfaceDescription	d	= src_texture.GetLevelDescription(0);
			return CreateTextureSameSize(device, src_texture, Usage.None, d.Format, pool);
		}
		static public Texture CreateTextureSameSize(Device device, Texture src_texture, Format format)
		{
			// Managedで作成する
			return CreateTextureSameSize(device, src_texture, Usage.None, format, Pool.Managed);
		}
		static public Texture CreateTextureSameSize(Device device, Texture src_texture, Usage usage, Format format, Pool pool)
		{
			Vector2	size	= d3d_utility.GetTextureSize(src_texture);
			try{
				Texture tex	= new Texture(device, (int)size.X, (int)size.Y,
											1, usage, format, pool);
				return tex;
			}catch{
				// 作成失敗
				return null;
			}
		}
		/*-------------------------------------------------------------------------
		 渡されたテクスチャと同じサイズのテクスチャを作成する
		 レンダーターゲットとして作成する
		---------------------------------------------------------------------------*/
		static public Texture CreateRenderTargetTextureSameSize(Device device, Texture src_texture)
		{
			SurfaceDescription	d	= src_texture.GetLevelDescription(0);
			return CreateTextureSameSize(device, src_texture, Usage.RenderTarget, d.Format, Pool.Default);
		}
	
		/*-------------------------------------------------------------------------
		 テクスチャコピー
		 例えばシステムメモリ上のテクスチャをVRAMに転送する等
		 サイズ等はチェックしないので注意
		---------------------------------------------------------------------------*/
		static public bool CopyTexture(Device device, Texture dst_texture, Texture src_texture)
		{
			if(device	== null)		return false;
			if(src_texture == null)		return false;
			if(dst_texture == null)		return false;

			try{
				// 単純にコピー
				Surface		dst	= dst_texture.GetSurfaceLevel(0);
				Surface		src	= src_texture.GetSurfaceLevel(0);
				SurfaceLoader.FromSurface(dst, src, Filter.None, 0);
				dst.Dispose();
				src.Dispose();
				return true;
			}catch{
				return false;
			}
		}

		/*-------------------------------------------------------------------------
		 テクスチャサイズを2のべき乗に調整する
		---------------------------------------------------------------------------*/
		static public Size TextureSizePow2(Size size)
		{
			Size ret	= new Size();
			ret.Width	= size_pow2(size.Width);
			ret.Height	= size_pow2(size.Height);
			return ret;
		}

		/*-------------------------------------------------------------------------
		 サイズを2のべき乗に調整する
		---------------------------------------------------------------------------*/
		static private int size_pow2(int size)
		{
			int	pow2	= 2;

			for(int i=0; i<32-2; i++){
				if(size <= pow2)	return pow2;
				pow2	<<= 1;
			}
			return size;
		}
	}
}
