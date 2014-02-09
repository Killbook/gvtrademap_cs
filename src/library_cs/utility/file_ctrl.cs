﻿/*-------------------------------------------------------------------------

 ファイルに関する操作

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System.IO;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace Utility
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public static class file_ctrl
	{
		/*-------------------------------------------------------------------------
		 ファイルの削除
		---------------------------------------------------------------------------*/
		static public bool RemoveFile(string fname)
		{
			try{
				// ファイルが存在しない
				if(!File.Exists(fname))		return true;

				// 削除する
				File.Delete(fname);
			}catch{
				return false;
			}
			return true;
		}
	
		/*-------------------------------------------------------------------------
		 フォルダの作成
		---------------------------------------------------------------------------*/
		static public bool CreatePath(string path)
		{
			try{
				DirectoryInfo	di	= new DirectoryInfo(path);
				if(!di.Exists){
					di.Create();
				}
			}catch{
				// 作成失敗
				return false;
			}
			return true;
		}
	}
}
