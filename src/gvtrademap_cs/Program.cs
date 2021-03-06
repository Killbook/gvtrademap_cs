﻿using System;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using Utility;

namespace gvtrademap_cs
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Mutex	m	= new Mutex(false, "mutex_gvtrademap_cs_cookie_Zephyros");
			string	device_info_string	= "";
	
			if(m.WaitOne(0, false)){
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

				// MDX1.1チェック
				AssemblyName	error_ass	= null;
				if(!Useful.LoadReferencedAssembly(Assembly.GetExecutingAssembly(), out error_ass)){
					// Assemblyの読み込みに失敗
					// 主にMDX1.1がインストールされていない
					using(assembly_load_error_form dlg = new assembly_load_error_form(error_ass)){
						dlg.ShowDialog();
					}
					return;
				}
#if !DEBUG
				try{
#endif
					using(gvtrademap_cs_form frm = new gvtrademap_cs_form())
					{
						if(frm.Initialize()){		// 初期化
							device_info_string	= frm.device_info_string;
							frm.Show();				// 表示開始

							int		old_tick_count	= System.Environment.TickCount;
							while(frm.Created){
								// 秒間60フレームを限界とする
								// それほど精度が高くないと思う
								if(System.Environment.TickCount - old_tick_count >= 1000/60){
									old_tick_count	= System.Environment.TickCount;
									frm.update_main_window();
								}
								Application.DoEvents();
								Thread.Sleep(1);		// できるだけCPUを使わない
														// 処理が間に合わなくても必ず休む
							}
						}
					}
#if !DEBUG
				}catch(Exception ex){
					// 想定外のエラー
					using(
						error_form	dlg = new error_form(def.WINDOW_TITLE,
														ex,
														def.WINDOW_TITLE,
														def.URL4,
														(device_info_string != "")? device_info_string: "未取得") ){
						dlg.ShowDialog();
					}
				}
#endif
			}else{
				// すでに起動している
				// アクティブにして終了する
				gvtrademap_cs_form.ActiveGVTradeMap();
			}
			// m が GC によって解放されないようにする
			GC.KeepAlive(m);
		}
	}
}
