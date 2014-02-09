﻿/*-------------------------------------------------------------------------

 mouse hook

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using win32;
using System.Windows.Forms;
using System.Runtime.InteropServices;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------
 
	---------------------------------------------------------------------------*/
	class globalmouse_hook : IDisposable
	{
		public enum SendKeyType{
			TOGGLE_SKILL	= 1,	// スキルパネル開閉			Ctrl+Q
			TOGGLE_CUSTOM_SLOT,		// カスタムスロットを開く		Ctrl+Z
			OPEN_ITEM_WINDOW,		// アイテムウインドウを開く	Ctrl+W
		};
	
		private IntPtr					m_handle;		// DLLのハンドル

		// DLL内の関数
		private delegate int SetDolMouseHookEx(int xbutton1_keytype, int xbutton2_keytype);
		private delegate int UnhookDolMouseHook();
	
		/*-------------------------------------------------------------------------
		 初期化
		 マウスフックを開始する
		---------------------------------------------------------------------------*/
		public globalmouse_hook()
			: this(SendKeyType.TOGGLE_SKILL, SendKeyType.OPEN_ITEM_WINDOW)
		{
		}
		public globalmouse_hook(SendKeyType xbutton1, SendKeyType xbutton2)
		{
			m_handle		= kernel32.LoadLibrary("mousehook.dll");
			if(m_handle == IntPtr.Zero){
				MessageBox.Show("mousehook.dll の読み込みに失敗");
				return;
			}

			IntPtr	func	= kernel32.GetProcAddress(m_handle, "SetDolMouseHookEx");
			if(func == IntPtr.Zero){
				MessageBox.Show("SetDolMouseHookEx() のアドレス取得に失敗");
				return;
			}

			SetDolMouseHookEx	setDolMouseHook = (SetDolMouseHookEx)Marshal.GetDelegateForFunctionPointer(func, typeof(SetDolMouseHookEx));
			setDolMouseHook((int)xbutton1, (int)xbutton2);
		}

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		~globalmouse_hook()
		{
			Dispose();
		}

		/*-------------------------------------------------------------------------
		 フックされていれば終了させる
		---------------------------------------------------------------------------*/
		public void Dispose()
		{
			if(m_handle == IntPtr.Zero){
				return;
			}

			IntPtr	func	= kernel32.GetProcAddress(m_handle, "UnhookDolMouseHook");
			if(func == IntPtr.Zero){
				MessageBox.Show("UnhookDolMouseHook() のアドレス取得に失敗");
				return;
			}

			UnhookDolMouseHook	unhookDolMouseHook = (UnhookDolMouseHook)Marshal.GetDelegateForFunctionPointer(func, typeof(UnhookDolMouseHook));
			unhookDolMouseHook();

			kernel32.FreeLibrary(m_handle);
			m_handle		= IntPtr.Zero;
		}
	}
}
