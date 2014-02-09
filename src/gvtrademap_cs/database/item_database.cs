/*-------------------------------------------------------------------------

 アイテムデータベース
 できるだけ検索用に継承してる

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Diagnostics;
using System.Drawing;
using System;

using Utility;
using gvo_base;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public class ItemDatabaseCustom : ItemDatabase
	{
		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public ItemDatabaseCustom(string fname)
			: base(fname)
		{
		}
		
		/*-------------------------------------------------------------------------
		 できるだけ検索
		---------------------------------------------------------------------------*/
		public void FindAll(string find_string, List<database.find> list, database.find.FindHandler handler)
		{
			IEnumerator<Data>	e	= base.GetEnumerator();
			while(e.MoveNext()){
				if(handler(e.Current.Name, find_string)){
					list.Add(new database.find(e.Current));
				}
			}
		}

		/*-------------------------------------------------------------------------
		 できるだけ検索
		 種類での検索用
		---------------------------------------------------------------------------*/
		public void FindAll_FromType(string find_string, List<database.find> list, database.find.FindHandler handler)
		{
			IEnumerator<Data>	e	= base.GetEnumerator();
			while(e.MoveNext()){
				if(handler(e.Current.Type, find_string)){
					list.Add(new database.find(e.Current));
				}
			}
		}
	}
}
