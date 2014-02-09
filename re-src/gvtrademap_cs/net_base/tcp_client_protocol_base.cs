// Type: net_base.tcp_client_protocol_base
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Net.Sockets;

namespace net_base
{
  public class tcp_client_protocol_base : tcp_client_base
  {
    private const string VERSION_COMMAND = "VERSION";
    private string m_protocol_name;
    private int m_version;
    private tcp_client_protocol_base.client_state m_state;
    public tcp_client_protocol_base.ReceivedCommandEventHandler ReceivedCommand;

    public tcp_client_protocol_base.client_state state
    {
      get
      {
        return this.m_state;
      }
    }

    public tcp_client_protocol_base(string protocol_name, int version)
    {
      this.init(protocol_name, version);
    }

    public tcp_client_protocol_base(string protocol_name, int version, Socket soc)
      : base(soc)
    {
      this.init(protocol_name, version);
      this.send_version();
    }

    private void init(string protocol_name, int version)
    {
      this.m_protocol_name = protocol_name;
      this.m_version = version;
      this.m_state = tcp_client_protocol_base.client_state.init;
      this.ReceivedData += new ReceivedDataEventHandler(this.received_handler);
    }

    public void SendData(string command, string[] datas)
    {
      if (this.m_state == tcp_client_protocol_base.client_state.error_version)
        throw new Exception("プロトコルバ\x30FCジョンが異なります\nバ\x30FCジョンを合わせてください");
      if (this.m_state != tcp_client_protocol_base.client_state.ready)
        throw new Exception("バ\x30FCジョンチェックが完了していません");
      this.Send(tcp_client_protocol_base.CreatePacket(command, datas));
    }

    public static string CreatePacket(string command, string[] datas)
    {
      string str1 = command;
      if (datas != null && datas.Length > 0)
      {
        foreach (string str2 in datas)
          str1 = str1 + (object) ':' + str2;
      }
      else
        str1 = str1 + ":";
      return str1;
    }

    private void received_handler(object sender, ReceivedDataEventArgs e)
    {
      string[] datas = e.received_string.Split(new char[1]
      {
        ':'
      });
      if (datas.Length <= 0)
        return;
      if (datas[0] == "VERSION")
      {
        if (datas.Length != 3)
          this.m_state = tcp_client_protocol_base.client_state.error_version;
        else if (datas[1] != this.m_version.ToString() || datas[2] != this.m_protocol_name)
          this.m_state = tcp_client_protocol_base.client_state.error_version;
        else
          this.m_state = tcp_client_protocol_base.client_state.ready;
      }
      else
      {
        if (this.ReceivedCommand == null)
          return;
        this.ReceivedCommand((object) this, datas);
      }
    }

    protected override void OnConnected(EventArgs e)
    {
      base.OnConnected(e);
      this.send_version();
    }

    private void send_version()
    {
      this.m_state = tcp_client_protocol_base.client_state.chekc_version;
      this.Send(tcp_client_protocol_base.CreatePacket("VERSION", new string[2]
      {
        this.m_version.ToString(),
        this.m_protocol_name
      }));
    }

    protected override void OnDisconnected(EventArgs e)
    {
      base.OnDisconnected(e);
      this.m_state = tcp_client_protocol_base.client_state.disconected;
    }

    public enum client_state
    {
      init,
      chekc_version,
      error_version,
      ready,
      disconected,
    }

    public delegate void ReceivedCommandEventHandler(object sender, string[] datas);
  }
}
