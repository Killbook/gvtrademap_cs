﻿/*-------------------------------------------------------------------------

 TCPクライアントベース
 通信プロトコル実装用ベース

---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 using
---------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
namespace net_base
{
	/*-------------------------------------------------------------------------
	 
	---------------------------------------------------------------------------*/
	public class tcp_client_protocol_base : tcp_client_base
	{
		private const string		VERSION_COMMAND		= "VERSION";
	
		public enum client_state{
			init,
			chekc_version,	// バージョン確認中
			error_version,	// バージョンエラー
			ready,			// 通信OK
			disconected,	// 切断されている
		};
	
		// datas[0]はコマンド
		// datas[1]からはデータ
		// データ数はコマンドによって異なる
		public delegate void ReceivedCommandEventHandler(object sender, string[] datas);

		private	string				m_protocol_name;		// プロトコル名
		private int					m_version;				// プロトコルバージョン
		private client_state		m_state;				// ステータス

		public ReceivedCommandEventHandler	ReceivedCommand;

		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		public client_state state		{	get{	return m_state;		}}
	
		/*-------------------------------------------------------------------------
		 クライアント用
		---------------------------------------------------------------------------*/
		public tcp_client_protocol_base(string protocol_name, int version)
			: base()
		{
			init(protocol_name, version);
		}

		/*-------------------------------------------------------------------------
		 サーバ用
		---------------------------------------------------------------------------*/
		public tcp_client_protocol_base(string protocol_name, int version, Socket soc)
			: base(soc)
		{
			init(protocol_name, version);

			// バージョン情報を送る
			send_version();
		}
	
		/*-------------------------------------------------------------------------
		 
		---------------------------------------------------------------------------*/
		private void init(string protocol_name, int version)
		{
			m_protocol_name		= protocol_name;		// プロトコル名
			m_version			= version;				// プロトコルバージョン
			m_state				= client_state.init;	// 初期化中
	
			// データ受信時のハンドラ
			base.ReceivedData	+= new ReceivedDataEventHandler(received_handler);
		}

		/*-------------------------------------------------------------------------
		 データを送信する
		 commandには:を含んではいけない
		 VERSIONという名前のcommandは予約されている
		---------------------------------------------------------------------------*/
		public void SendData(string command, string[] datas)
		{
			if(m_state == client_state.error_version){
				throw new Exception("プロトコルバージョンが異なります\nバージョンを合わせてください");
			}
			if(m_state != client_state.ready){
				// 準備が完了していない
				throw new Exception("バージョンチェックが完了していません");
			}
#if DEBUG
			if(command == VERSION_COMMAND){
				throw new Exception(VERSION_COMMAND + " という名前のコマンド名は指定できません");
			}
#endif
			base.Send(CreatePacket(command, datas));
		}

		/*-------------------------------------------------------------------------
		 パケットを作成する
		---------------------------------------------------------------------------*/
		static public string CreatePacket(string command, string[] datas)
		{
#if DEBUG
			if(command.IndexOf(':') >= 0){
				throw new Exception(": を含むプロトコル名は指定できません");
			}
#endif
			string	packet	= command;
			if(   (datas != null)
				&&(datas.Length > 0) ){
				foreach(string d in datas){
					packet	+= ':' + d;
				}
			}else{
				packet	+= ":";
			}
			return packet;
		}

		/*-------------------------------------------------------------------------
		 データ受信
		 データを分解してハンドラに渡す
		---------------------------------------------------------------------------*/
		private void received_handler(object sender, ReceivedDataEventArgs e)
		{
			string[]	datas	= e.received_string.Split(':');
			if(datas.Length <= 0)	return;		// データエラー

			// バージョン情報
			if(datas[0] == VERSION_COMMAND){
				if(datas.Length != 3){
					// エラー
					m_state		= client_state.error_version;
					return;
				}
				if(   (datas[1] != m_version.ToString())
					||(datas[2] != m_protocol_name) ){
					// エラー
					m_state		= client_state.error_version;
					return;
				}
				// 通信可能
				m_state		= client_state.ready;
				return;
			}
			
			// ハンドラに渡す
			if(ReceivedCommand != null){
				ReceivedCommand(this, datas);
			}
		}

		/*-------------------------------------------------------------------------
		 サーバに接続した
		---------------------------------------------------------------------------*/
		protected override void OnConnected(EventArgs e)
		{
			base.OnConnected(e);

			// バージョン情報を送る
			send_version();
		}
	
		/*-------------------------------------------------------------------------
		 バージョン情報を送る
		---------------------------------------------------------------------------*/
		private void send_version()
		{
			m_state		= client_state.chekc_version;
			base.Send(CreatePacket(VERSION_COMMAND, new string[]{m_version.ToString(), m_protocol_name}));
		}
	
		/*-------------------------------------------------------------------------
		 サーバから切断された
		 サーバから切断した
		---------------------------------------------------------------------------*/
		protected override void OnDisconnected(EventArgs e)
		{
			base.OnDisconnected(e);
			m_state		= client_state.disconected;		// 切断
		}
	}
}
