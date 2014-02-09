// Type: net_base.tcp_server_base
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace net_base
{
  public class tcp_server_base : IDisposable
  {
    private int MAX_CLIENT = 16;
    private int DEF_BACKLOG = 100;
    protected readonly object m_sync_socket = new object();
    private Socket m_server;
    private IPEndPoint m_socket_ep;
    private tcp_server_base.server_state m_state;
    private int m_max_client;
    protected List<tcp_client_base> m_client_list;

    public IPEndPoint socket_ep
    {
      get
      {
        return this.m_socket_ep;
      }
    }

    public int max_client
    {
      get
      {
        return this.m_max_client;
      }
      set
      {
        this.m_max_client = value;
      }
    }

    public tcp_server_base.server_state state
    {
      get
      {
        return this.m_state;
      }
    }

    public tcp_client_base[] client_list
    {
      get
      {
        lock (this.m_sync_socket)
          return this.m_client_list.ToArray();
      }
    }

    public event ServerEventHandler AcceptedClient;

    public event ReceivedDataEventHandler ReceivedData;

    public event ServerEventHandler DisconnectedClient;

    public tcp_server_base()
    {
      this.m_max_client = this.MAX_CLIENT;
      this.m_server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      this.m_state = tcp_server_base.server_state.init;
      this.m_client_list = new List<tcp_client_base>();
    }

    public virtual void Dispose()
    {
      this.Close();
    }

    public void Close()
    {
      lock (this.m_sync_socket)
      {
        if (this.m_state == tcp_server_base.server_state.listening)
        {
          try
          {
            this.m_server.Close();
          }
          catch
          {
          }
          this.m_server = (Socket) null;
          this.m_state = tcp_server_base.server_state.stoped;
        }
        if (this.m_client_list == null)
          return;
        while (this.m_client_list.Count > 0)
          this.m_client_list[0].Dispose();
        this.m_client_list = (List<tcp_client_base>) null;
      }
    }

    public void Send(tcp_client_base client, string str)
    {
      if (client == null)
        return;
      lock (this.m_sync_socket)
        client.Send(str);
    }

    public void SendToAllClients(string str)
    {
      if (this.m_client_list == null)
        return;
      lock (this.m_sync_socket)
      {
        foreach (tcp_client_base item_0 in this.m_client_list)
          item_0.Send(str);
      }
    }

    public void Listen(int portNum)
    {
      IPAddress[] localIpAddressIpv4 = net_useful.GetLocalIpAddress_Ipv4();
      if (localIpAddressIpv4 == null || localIpAddressIpv4.Length <= 0)
        throw new ApplicationException("PCのIPアドレス取得に失敗");
      this.Listen(localIpAddressIpv4[0], portNum);
    }

    public void Listen(IPAddress host, int portNum)
    {
      this.Listen(host, portNum, this.DEF_BACKLOG);
    }

    public void Listen(IPAddress host, int portNum, int backlog)
    {
      if (this.m_server == null)
        throw new ApplicationException("サ\x30FCバが閉じています");
      if (this.m_state != tcp_server_base.server_state.init)
        throw new ApplicationException("初期化状態ではありません");
      this.m_socket_ep = new IPEndPoint(host, portNum);
      this.m_server.Bind((EndPoint) this.m_socket_ep);
      this.m_server.Listen(backlog);
      this.m_state = tcp_server_base.server_state.listening;
      this.m_server.BeginAccept(new AsyncCallback(this.accept_callback), (object) null);
    }

    protected virtual tcp_client_base CreateClient(Socket sct)
    {
      return new tcp_client_base(sct);
    }

    private void accept_callback(IAsyncResult ar)
    {
      Socket sct = (Socket) null;
      try
      {
        lock (this.m_sync_socket)
          sct = this.m_server.EndAccept(ar);
      }
      catch
      {
        this.Close();
        return;
      }
      tcp_client_base client = this.CreateClient(sct);
      if (this.m_client_list.Count >= this.m_max_client)
      {
        client.Close();
      }
      else
      {
        lock (this.m_sync_socket)
          this.m_client_list.Add(client);
        client.Disconnected += new EventHandler(this.client_disconnected);
        client.ReceivedData += new ReceivedDataEventHandler(this.client_received_data);
        this.OnAcceptedClient(new ServerEventArgs(client));
        if (!client.is_closed)
          client.StartReceive();
      }
      this.m_server.BeginAccept(new AsyncCallback(this.accept_callback), (object) null);
    }

    private void client_disconnected(object sender, EventArgs e)
    {
      lock (this.m_sync_socket)
        this.m_client_list.Remove((tcp_client_base) sender);
      this.OnDisconnectedClient(new ServerEventArgs((tcp_client_base) sender));
    }

    private void client_received_data(object sender, ReceivedDataEventArgs e)
    {
      this.OnReceivedData(new ReceivedDataEventArgs((tcp_client_base) sender, e.received_string));
    }

    protected virtual void OnAcceptedClient(ServerEventArgs e)
    {
      if (this.AcceptedClient == null)
        return;
      this.AcceptedClient((object) this, e);
    }

    protected virtual void OnReceivedData(ReceivedDataEventArgs e)
    {
      if (this.ReceivedData == null)
        return;
      this.ReceivedData((object) this, e);
    }

    protected virtual void OnDisconnectedClient(ServerEventArgs e)
    {
      if (this.DisconnectedClient == null)
        return;
      this.DisconnectedClient((object) this, e);
    }

    public enum server_state
    {
      init,
      listening,
      stoped,
    }
  }
}
