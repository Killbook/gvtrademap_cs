// Type: gvo_net_base.gvo_tcp_server
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using net_base;
using System.Net.Sockets;

namespace gvo_net_base
{
  public class gvo_tcp_server : tcp_server_protocol_base
  {
    public gvo_tcp_client[] client_list
    {
      get
      {
        lock (this.m_sync_socket)
        {
          gvo_tcp_client[] local_1 = new gvo_tcp_client[this.m_client_list.Count];
          for (int local_2 = 0; local_2 < this.m_client_list.Count; ++local_2)
            local_1[local_2] = this.m_client_list[local_2] as gvo_tcp_client;
          return local_1;
        }
      }
    }

    public gvo_tcp_server()
      : base("GVO_NAVIGATION_PROTOCOL", 1)
    {
      this.max_client = 1;
    }

    protected override tcp_client_base CreateClient(Socket sct)
    {
      return (tcp_client_base) new gvo_tcp_client(sct);
    }
  }
}
