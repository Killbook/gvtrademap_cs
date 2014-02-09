/*-------------------------------------------------------------------------

 経過日数カウンタ
 完全な経過日数とはならない可能性があるが、
 許容できる範囲の精度は保てる

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public class gvo_day_counter
	{
		private const int					DEF_COUNTER_MAX	= 99;	// カウンタがカンストする初期値
	
		private int							m_days;					// 確定分
		private int							m_voyage_days_start;	// チェック開始時の航海日数
		private int							m_voyage_days;			// 現在の航海日数
		private int							m_counter_max;			// カウンタがカンストする日数

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		protected int Days{
			get{
				return m_days;
			}
			set{
				m_days	= value;
			}
		}
		protected int VoyageDaysStart{
			get{
				return m_voyage_days_start;
			}
		}
		protected int VoyageDays{
			get{
				return m_voyage_days;
			}
		}
		protected int CounterMax{
			get{
				return m_counter_max;
			}
			set{
				m_counter_max	= value;
				if(m_counter_max < 0)	m_counter_max	= DEF_COUNTER_MAX;
			}
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public gvo_day_counter()
			: this(0)
		{
		}
		public gvo_day_counter(int days)
		{
			Reset();
			m_days					= days;
			m_counter_max			= DEF_COUNTER_MAX;
		}

		/*-------------------------------------------------------------------------
		 更新
		---------------------------------------------------------------------------*/
		protected virtual void UpdateBase(int days)
		{
			if(m_voyage_days_start < 0){
				// 最初の更新
				m_voyage_days_start	= days;
				m_voyage_days		= days;
			}else{
				// 航海日数が前回よりも小さかったら
				// 確定日数を更新する
				if(days < m_voyage_days){
					m_days				+= m_voyage_days - m_voyage_days_start;
					m_voyage_days_start	= days;
					m_voyage_days		= days;
				}
				// 今回の航海日数
				m_voyage_days	= days;
			}
		}
	
		/*-------------------------------------------------------------------------
		 リセット
		---------------------------------------------------------------------------*/
		public void Reset()
		{
			Reset(-1);
		}

		/*-------------------------------------------------------------------------
		 リセット
		 now_daysを基準とする
		---------------------------------------------------------------------------*/
		public void Reset(int now_days)
		{
			m_days					= 0;
			m_voyage_days_start		= now_days;
			m_voyage_days			= now_days;
		}

		/*-------------------------------------------------------------------------
		 経過日数を得る
		 m_counter_max日でカンストする
		---------------------------------------------------------------------------*/
		public int GetDays()
		{
			// 確定分+航海日数
			int	days	= get_true_days();
			return (days <= m_counter_max)? days: m_counter_max;
		}

		/*-------------------------------------------------------------------------
		 経過日数を得る
		---------------------------------------------------------------------------*/
		protected int get_true_days()
		{
			return m_days + (m_voyage_days - m_voyage_days_start);
		}
	}
}
