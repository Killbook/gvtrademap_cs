/*-------------------------------------------------------------------------

 チャット解析
 リクエスト付き
 預金の利息は災害とは独立して解析される
 危険海域変動システムも独立して解析される
 アクシデントは解析時の最後のもののみ

 ログの更新チェックに時間が掛かるため、スレッド推奨
 海域変動は専用のメソッドUpdateSeaArea_DoRequest()でメインスレッドから行うこと

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

using Utility;
using gvo_base;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class gvo_chat : gvo_map_cs_chat_base
	{
		private sea_area					m_sea_area;

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public gvo_chat(sea_area _sea_area)
			: base()
		{
			m_sea_area		= _sea_area;
		}
		public gvo_chat(string path, sea_area _sea_area)
			: base(path)
		{
			m_sea_area		= _sea_area;
		}
		
		/*-------------------------------------------------------------------------
		 海域変動の更新
		 リクエストがあるときのみ解析する
		---------------------------------------------------------------------------*/
		public void UpdateSeaArea_DoRequest()
		{
			if(IsRequest()){
				update_sea_area();
			}
		}
	
		/*-------------------------------------------------------------------------
		 災害情報から保存用の値に変換する
		---------------------------------------------------------------------------*/
		static public int ToIndex(accident a)
		{
			switch(a){
			case accident.shark1:				// サメ1
			case accident.shark2:				// サメ2
				return 101;
			case accident.fire:					// 火災
				return 102;
			case accident.seaweed:				// 藻
				return 103;
			case accident.seiren:				// セイレーン
				return 104;
			case accident.compass:				// 羅針盤
				return 105;
			case accident.storm:				// 嵐
				return 106;
			case accident.blizzard:				// 吹雪
				return 107;
			case accident.mouse:				// ネズミ
				return 108;
			case accident.UMA:					// 得体の知れない怪物
				return 109;
			case accident.treasure1:			// 何かいい物
			case accident.treasure2:			// 何か見つかるかも
			case accident.treasure3:			// 高価なもの
				return 111;
			case accident.escape_battle:		// 全船が戦場を離れました
			case accident.win_battle:			// 勝利
			case accident.lose_battle:			// 敗北
				return 110;
			}
			return -1;		// unknown
		}

		/*-------------------------------------------------------------------------
		 海域変動システムを更新する
		---------------------------------------------------------------------------*/
		private void update_sea_area()
		{
			sea_area_type[]	list	= base.sea_area_type_list;

			// 反映させる
			if(list != null){
				foreach(sea_area_type d in list){
					switch(d.type){
					case sea_type.normal:
						m_sea_area.SetType(d.name, sea_area.sea_area_once.sea_type.normal);
						break;
					case sea_type.safty:
						m_sea_area.SetType(d.name, sea_area.sea_area_once.sea_type.safty);
						break;
					case sea_type.lawless:
						m_sea_area.SetType(d.name, sea_area.sea_area_once.sea_type.lawless);
						break;
					}
				}
			}

			// リセット
			base.ResetSeaArea();
		}
	}
}
