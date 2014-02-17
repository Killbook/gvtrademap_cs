/*-------------------------------------------------------------------------

 造船からの経過日数
 完全な経過日数とはならない可能性があるが、
 許容できる範囲の精度は保てる

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using Utility;
using System.Text.RegularExpressions;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public class gvo_build_ship_counter : gvo_day_counter
	{
		private const int					COUNTER_MAX	= 999;	// 999日でカンストする
	
		private GlobalSettings						m_setting;

		private bool						m_is_now_build;		// 造船中ならtrue
		private string						m_ship_name;		// 船名
		private int							m_finish_days;		// 船名から解析した造船終了日数

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public bool	IsNowBuild				{	get{	return m_is_now_build;		}}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public gvo_build_ship_counter(GlobalSettings _setting)
			: base()
		{
			// カンストする日数を設定する
			base.CounterMax			= COUNTER_MAX;

			// 設定項目
			m_setting				= _setting;

			// 
			base.Days				= m_setting.build_ship_days;
			m_is_now_build			= m_setting.is_now_build_ship;
			m_ship_name				= m_setting.build_ship_name;
			m_finish_days			= get_build_ship_days(m_ship_name);	// 日数を得る
		}

		/*-------------------------------------------------------------------------
		 設定項目更新
		---------------------------------------------------------------------------*/
		private void update_settings()
		{
			m_setting.build_ship_days		= base.get_true_days();
			m_setting.is_now_build_ship		= m_is_now_build;
			m_setting.build_ship_name		= m_ship_name;
		}

		/*-------------------------------------------------------------------------
		 造船開始
		---------------------------------------------------------------------------*/
		public void StartBuildShip(string ship_name)
		{
			base.Reset();					// カウンタリセット
			m_is_now_build	= true;			// 造船開始
			m_ship_name		= ship_name;	// 船名
			m_finish_days	= get_build_ship_days(m_ship_name);		// 日数を得る

			// 設定項目更新
			update_settings();
		}
	
		/*-------------------------------------------------------------------------
		 造船終了
		---------------------------------------------------------------------------*/
		public void FinishBuildShip()
		{
			m_is_now_build	= false;		// 造船終了
			base.Reset();					// カウンタリセット
			m_ship_name		= "";
			m_finish_days	= -1;

			// 設定項目更新
			update_settings();
		}
	
		/*-------------------------------------------------------------------------
		 更新
		---------------------------------------------------------------------------*/
		public void Update(int days)
		{
			if(!m_is_now_build)			return;

			// 日数カウントの更新
			base.UpdateBase(days);

			// 設定項目更新
			update_settings();
		}

		/*-------------------------------------------------------------------------
		 ポップアップ用の文字列を得る
		---------------------------------------------------------------------------*/
		public string GetPopupString()
		{
			if(m_is_now_build){
				// 造船中
				if(m_finish_days > 0){
					// 船名から終了日数解析できている
					if(GetDays() > m_finish_days){
						// 完成する日数が経過している
						return String.Format("[ {0} ]を建造中\n{1}日経過\n完成から{2}日経過",
												m_ship_name,
												base.GetDays(),
												GetDays() - m_finish_days);
					}else if(GetDays() == m_finish_days){
						// 丁度完成してる
						return String.Format("[ {0} ]を建造中\n{1}日経過\n完成しました",
												m_ship_name,
												base.GetDays());
					}else{
						// 完成する日数が経過していない
						return String.Format("[ {0} ]を建造中\n{1}日経過\n残り{2}日",
												m_ship_name,
												base.GetDays(),
												m_finish_days - base.GetDays());
					}
				}else{
					// 船名から終了日数が解析できていない
					return String.Format("[ {0} ]を建造中\n{1}日経過\n船名に 14日 のような名前を付けると\n残り日数を計算できます",
											m_ship_name,
											base.GetDays());
				}
			}else{
				// 造船開始待ち
				return "建造中ではありません";
			}
		}

		/*-------------------------------------------------------------------------
		 造船日数を得る
		 船名に 14日 等が含まれていればそれを使う
		 99日でカンスト
		 日数が含まれない場合は-1を返す
		---------------------------------------------------------------------------*/
		private int get_build_ship_days(string name)
		{
			// 全角数字を半角に変換する
			name	= Useful.AdjustNumber(name);

			Match	m	= Useful.match(@"([0-9]+)日", name);
			if(m == null)	return -1;		// 日数が含まれない

			// 日数に変換
			return Useful.ToInt32(m.Groups[1].Value, -1);
		}
	}
}
