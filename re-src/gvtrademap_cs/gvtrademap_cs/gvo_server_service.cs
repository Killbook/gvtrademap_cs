// Type: gvtrademap_cs.gvo_server_service
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using gvo_net_base;
using net_base;
using System;
using System.Windows.Forms;

namespace gvtrademap_cs
{
  public class gvo_server_service : IDisposable
  {
    private gvo_tcp_server m_server;
    private bool m_is_error;

    public bool is_listening
    {
      get
      {
        return this.m_server != null && this.m_server.state == tcp_server_base.server_state.listening;
      }
    }

    public gvo_server_service()
    {
      this.m_server = (gvo_tcp_server) null;
      this.m_is_error = false;
    }

    public void Dispose()
    {
      this.Close();
    }

    public void Close()
    {
      if (this.m_server == null)
        return;
      this.m_server.Close();
      this.m_server = (gvo_tcp_server) null;
      this.m_is_error = false;
    }

    public void Listen(int port_index)
    {
      if (this.m_is_error)
        return;
      this.Close();
      try
      {
        this.m_server = new gvo_tcp_server();
        this.m_server.Listen(port_index);
      }
      catch
      {
        this.Close();
        this.m_is_error = true;
        int num = (int) MessageBox.Show("TCPサ\x30FCバの起動に失敗しました。", "TCPサ\x30FCバ起動エラ\x30FC", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public gvo_tcp_client GetClient()
    {
      if (!this.is_listening)
        return (gvo_tcp_client) null;
      gvo_tcp_client[] clientList = this.m_server.client_list;
      if (clientList == null)
        return (gvo_tcp_client) null;
      if (clientList.Length <= 0)
        return (gvo_tcp_client) null;
      else
        return clientList[0];
    }
  }
}
