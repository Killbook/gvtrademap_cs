// Type: net_base.ReceivedDataEventArgs
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;

namespace net_base
{
  public class ReceivedDataEventArgs : EventArgs
  {
    private string m_received_string;
    private tcp_client_base m_client;

    public string received_string
    {
      get
      {
        return this.m_received_string;
      }
    }

    public tcp_client_base client
    {
      get
      {
        return this.m_client;
      }
    }

    public ReceivedDataEventArgs(tcp_client_base c, string str)
    {
      this.m_client = c;
      this.m_received_string = str;
    }
  }
}
