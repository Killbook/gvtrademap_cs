// Type: net_base.tcp_server_protocol_base
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Net.Sockets;

namespace net_base
{
  public class tcp_server_protocol_base : tcp_server_base
  {
    private readonly object m_sync_object = new object();
    private string m_protocol_name;
    private int m_version;

    public tcp_client_protocol_base[] client_list
    {
      get
      {
        lock (this.m_sync_socket)
        {
          tcp_client_protocol_base[] local_1 = new tcp_client_protocol_base[this.m_client_list.Count];
          for (int local_2 = 0; local_2 < this.m_client_list.Count; ++local_2)
            local_1[local_2] = this.m_client_list[local_2] as tcp_client_protocol_base;
          return local_1;
        }
      }
    }

    public tcp_server_protocol_base(string protocol_name, int version)
    {
      this.m_protocol_name = protocol_name;
      this.m_version = version;
      this.ReceivedData += new ReceivedDataEventHandler(this.received_handler);
    }

    private void received_handler(object sender, ReceivedDataEventArgs e)
    {
    }

    protected override tcp_client_base CreateClient(Socket sct)
    {
      return (tcp_client_base) new tcp_client_protocol_base(this.m_protocol_name, this.m_version, sct);
    }

    public void SendDataToAllClients(string command, string[] datas)
    {
      if (this.m_client_list == null)
        return;
      string packet = tcp_client_protocol_base.CreatePacket(command, datas);
      lock (this.m_sync_socket)
      {
        foreach (tcp_client_base item_0 in this.m_client_list)
        {
          tcp_client_protocol_base local_4 = item_0 as tcp_client_protocol_base;
          if (local_4 != null && local_4.state == tcp_client_protocol_base.client_state.ready)
            local_4.Send(packet);
        }
      }
    }
  }
}
