﻿/*-------------------------------------------------------------------------

 Assemblyロードエラー

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
using System.Reflection;
using System.Diagnostics;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace gvtrademap_cs
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public partial class assembly_load_error_form : Form
	{
		private AssemblyName			m_assembly_name;

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public assembly_load_error_form(AssemblyName assembly_name)
		{
			InitializeComponent();

			m_assembly_name		= assembly_name;

			string	str	= def.WINDOW_TITLE + "\n";
			str			+= assembly_name.FullName + "\n";
			str			+= "の読み込みに失敗しました。\n\n";

			str			+= "交易Map C#の起動にはMicrsoft DirectX 9.0C以降、Managed DirectX(MDX1.1) が必要です。\n";
			str			+= "MDX1.1をインストールするには DirectX End-User Runtime Web Installer を実行してください。\n";
			str			+= "DirectX End-User Runtime Web InstallerはMDX1.1をインストールしてくれます。\n";

			str			+= "\n";
			str			+= "MDX1.1をインストールしたにも関わらず起動できない場合はエラー内容を報告してもらえると対応できるかもしれません。";
	
			textBox1.AcceptsReturn	= true;
			textBox1.Lines			= str.Split(new char[]{'\n'});
			textBox1.Select(0, 0);
		}

		/*-------------------------------------------------------------------------
		 DirectX End-User Runtime Web Installer ダウンロードページを開く
		---------------------------------------------------------------------------*/
		private void button3_Click(object sender, EventArgs e)
		{
			Process.Start(def.URL6);
		}

		/*-------------------------------------------------------------------------
		 エラー報告を行うページを開く
		---------------------------------------------------------------------------*/
		private void button2_Click(object sender, EventArgs e)
		{
			Process.Start(def.URL4);
		}
	}
}
