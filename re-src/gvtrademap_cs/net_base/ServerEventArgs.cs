// Type: net_base.ServerEventArgs
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

namespace net_base
{
  public class ServerEventArgs
  {
    private tcp_client_base m_client;

    public tcp_client_base client
    {
      get
      {
        return this.m_client;
      }
    }

    public ServerEventArgs(tcp_client_base c)
    {
      this.m_client = c;
    }
  }
}
