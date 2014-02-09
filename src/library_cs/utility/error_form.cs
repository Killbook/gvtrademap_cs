﻿/*-------------------------------------------------------------------------

 想定外エラー
 スタックトレースを表示しする
 指定されたURLへのジャンプ可能

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace Utility
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	public partial class error_form : Form
	{
		private string				m_message;
		private string				m_url;

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		public error_form(string window_title, Exception ex, string message_top, string url)
		{
			initialize(window_title, ex, message_top, url, "");
		}	
		public error_form(string window_title, Exception ex, string message_top, string url, string device_info_string)
		{
			initialize(window_title, ex, message_top, url, device_info_string);
		}	

		/*-------------------------------------------------------------------------

		---------------------------------------------------------------------------*/
		private void initialize(string window_title, Exception ex, string message_top, string url, string device_info_string)
		{
			InitializeComponent();

			// エラー内容生成
			string		message		= "";

			if(!String.IsNullOrEmpty(message_top)){
				message		+= message_top + "\n";
			}

			// 日時
			message			+= "DATE:" + Useful.TojbbsDateTimeString(DateTime.Now) + "\n";

			// OSバージョン
			OperatingSystem	os	= Environment.OSVersion;
			message			+= "OS:" + os.VersionString + "\n";
			message			+= "OS:" + Useful.GetOsName(os) + "\n";

			// device info string
			if(!String.IsNullOrEmpty(device_info_string)){
				message		+= "DeviceInfo:" + device_info_string + "\n";
			}
	
			if(ex == null){
				//
				message		+= "エラー内容が不明";
			}else{
				message		+= "Message: " + ex.Message + "\nStackTrace:\n";
				message		+= make_error_message(ex.StackTrace);
			}
			m_message	= message;

			// windowタイトル
			if(!String.IsNullOrEmpty(window_title)){
				this.Text	= window_title;
			}

			// エラー内容
			textBox1.AcceptsReturn	= true;
			textBox1.Lines			= m_message.Split(new char[]{'\n'});
			textBox1.Select(0, 0);

			// エラー報告ボタン
			if(!String.IsNullOrEmpty(url)){
				m_url	= url;
			}else{
				m_url	= "";
				button2.Enabled	= false;
				label2.Text		= "--";
			}
		}

		/*-------------------------------------------------------------------------
		 エラー内容をクリップボードにコピーする
		---------------------------------------------------------------------------*/
		private void button3_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(m_message);
		}

		/*-------------------------------------------------------------------------
		 エラー報告を行うページを開く
		---------------------------------------------------------------------------*/
		private void button2_Click(object sender, EventArgs e)
		{
			if(m_url != ""){
				Process.Start(m_url);
			}
		}

		/*-------------------------------------------------------------------------
		 エラーメッセージを生成する
		 ソースの場所を削除していたが、必要ないようなのでほとんど何もせず返すように変更
		---------------------------------------------------------------------------*/
		static private string make_error_message(string str)
		{
			try{
				str		= str.Replace("\r\n", "\n");
				return str;
			}catch{
				// 何か失敗したときはそのまま返す
				return str;
			}
		}
	}
}
