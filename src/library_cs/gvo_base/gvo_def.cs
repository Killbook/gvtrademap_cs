/*-------------------------------------------------------------------------

 定数定義

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using Utility;
using System.IO;

/*-------------------------------------------------------------------------

---------------------------------------------------------------------------*/
namespace gvo_base
{
	/*-------------------------------------------------------------------------

	---------------------------------------------------------------------------*/
	static public class gvo_def
	{
		// 大航海時代Onlineのウインドウ検索用
		public const string		GVO_CLASS_NAME				= "Greate Voyages Online Game MainFrame";
		public const string		GVO_WINDOW_NAME				= "大航海時代 Online";

		// ユーザデータパス
		public const string		GVO_USERDATA_PATH			= @"KOEI\GV Online\";
		// ログパス
		public const string		GVO_LOG_PATH				= GVO_USERDATA_PATH + @"Log\Chat\";
		// メールパス
		public const string		GVO_MAIL_PATH				= GVO_USERDATA_PATH + @"Mail\";
		// スクリーンショットパス
		public const string		GVO_SCREENSHOT_PATH			= GVO_USERDATA_PATH + @"ScreenShot\";

		// URL
		public const string		URL2						= @"http://www.umiol.com/db/recipe.php?cmd=recsrc&submit=%B8%A1%BA%F7&recsrckey=";
		public const string		URL3						= @"http://www.umiol.com/db/recipe.php?cmd=prosrc&submit=%B8%A1%BA%F7&prosrckey=";

		/*-------------------------------------------------------------------------
		 大航海時代Onlineのログのフルパスを得る
		---------------------------------------------------------------------------*/
		static public string GetGvoLogPath()
		{
			return Path.Combine(Useful.GetMyDocumentPath(), GVO_LOG_PATH);
		}

		/*-------------------------------------------------------------------------
		 大航海時代Onlineのメールのフルパスを得る
		---------------------------------------------------------------------------*/
		static public string GetGvoMailPath()
		{
			return Path.Combine(Useful.GetMyDocumentPath(), GVO_MAIL_PATH);
		}

		/*-------------------------------------------------------------------------
		 大航海時代Onlineのメールのフルパスを得る
		---------------------------------------------------------------------------*/
		static public string GetGvoScreenShotPath()
		{
			return Path.Combine(Useful.GetMyDocumentPath(), GVO_SCREENSHOT_PATH);
		}
	}
}
