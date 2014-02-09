// Type: net_base.tcp_client_base
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace net_base
{
  public class tcp_client_base : IDisposable
  {
    private readonly object m_sync_socket = new object();
    private Socket m_client;
    private IPEndPoint m_local_ep;
    private IPEndPoint m_remote_ep;
    private MemoryStream m_received_bytes;
    private Encoding m_encoding;
    private bool m_disconnected_from_server;

    public bool is_closed
    {
      get
      {
        return this.m_client == null;
      }
    }

    public bool is_connected
    {
      get
      {
        if (this.is_closed)
          return false;
        else
          return this.m_client.Connected;
      }
    }

    public IPEndPoint local_ep
    {
      get
      {
        return this.m_local_ep;
      }
    }

    public IPEndPoint remote_ep
    {
      get
      {
        return this.m_remote_ep;
      }
    }

    protected Encoding encoding
    {
      get
      {
        return this.m_encoding;
      }
      set
      {
        this.m_encoding = value;
      }
    }

    public event EventHandler Connected;

    public event ReceivedDataEventHandler ReceivedData;

    public event EventHandler Disconnected;

    public tcp_client_base()
    {
      this.m_client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      this.m_local_ep = (IPEndPoint) null;
      this.m_remote_ep = (IPEndPoint) null;
      this.initialize();
    }

    public tcp_client_base(Socket soc)
    {
      this.m_client = soc;
      this.m_local_ep = (IPEndPoint) soc.LocalEndPoint;
      this.m_remote_ep = (IPEndPoint) soc.RemoteEndPoint;
      this.initialize();
    }

    private void initialize()
    {
      this.m_encoding = Encoding.UTF8;
      this.m_disconnected_from_server = false;
    }

    public virtual void Dispose()
    {
      this.Close();
    }

    public void Close()
    {
      this.Close(true);
    }

    public void Close(bool is_self)
    {
      if (this.m_client == null)
        return;
      try
      {
        this.m_client.Shutdown(SocketShutdown.Both);
        this.m_client.Close();
      }
      catch
      {
      }
      this.m_client = (Socket) null;
      if (this.m_received_bytes != null)
      {
        this.m_received_bytes.Close();
        this.m_received_bytes = (MemoryStream) null;
      }
      this.m_disconnected_from_server = !is_self;
      this.OnDisconnected(new EventArgs());
    }

    public void Send(string str)
    {
      if (!this.is_connected)
        throw new ApplicationException("接続していません");
      byte[] bytes = this.m_encoding.GetBytes(str + "\x0004");
      lock (this.m_sync_socket)
        this.m_client.Send(bytes);
    }

    public void Connect(string host, int port)
    {
      IPAddress[] ipAddressIpv4 = net_useful.GetIpAddressIpv4(host);
      if (ipAddressIpv4 == null || ipAddressIpv4.Length <= 0)
        throw new ApplicationException("サ\x30FCバ\x30FCが見つかりません");
      this.Connect(ipAddressIpv4[0], port);
    }

    public void Connect(IPAddress host, int port)
    {
      if (this.is_closed)
        throw new ApplicationException("接続していません");
      if (this.is_connected)
        throw new ApplicationException("接続済です");
      this.m_client.Connect((EndPoint) new IPEndPoint(host, port));
      this.m_local_ep = (IPEndPoint) this.m_client.LocalEndPoint;
      this.m_remote_ep = (IPEndPoint) this.m_client.RemoteEndPoint;
      this.OnConnected(new EventArgs());
      this.StartReceive();
    }

    public void StartReceive()
    {
      if (!this.is_connected)
        throw new ApplicationException("接続していません");
      byte[] buffer = new byte[1024];
      this.m_received_bytes = new MemoryStream();
      try
      {
        this.m_client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.receive_data_callback), (object) buffer);
      }
      catch
      {
        this.Close(false);
      }
    }

    private void receive_data_callback(IAsyncResult ar)
    {
      int count = -1;
      try
      {
        lock (this.m_sync_socket)
          count = this.m_client.EndReceive(ar);
      }
      catch
      {
      }
      if (count <= 0)
      {
        this.Close(false);
      }
      else
      {
        byte[] buffer = (byte[]) ar.AsyncState;
        this.m_received_bytes.Write(buffer, 0, count);
        if (this.m_received_bytes.Length > (long) int.MaxValue)
        {
          this.Close(false);
        }
        else
        {
          if (this.m_received_bytes.Length >= 1L)
          {
            this.m_received_bytes.Seek(-1L, SeekOrigin.End);
            if (this.m_received_bytes.ReadByte() == 4)
            {
              string @string = this.m_encoding.GetString(this.m_received_bytes.ToArray());
              this.m_received_bytes.Close();
              int startIndex = 0;
              int num;
              while ((num = @string.IndexOf('\x0004', startIndex)) >= 0)
              {
                string str = @string.Substring(startIndex, num - startIndex);
                startIndex = num + 1;
                this.OnReceivedData(new ReceivedDataEventArgs(this, str));
              }
              this.m_received_bytes = new MemoryStream();
            }
            else
              this.m_received_bytes.Seek(0L, SeekOrigin.End);
          }
          try
          {
            lock (this.m_sync_socket)
              this.m_client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.receive_data_callback), (object) buffer);
          }
          catch
          {
            this.Close(false);
          }
        }
      }
    }

    protected virtual void OnConnected(EventArgs e)
    {
      if (this.Connected == null)
        return;
      this.Connected((object) this, e);
    }

    protected virtual void OnDisconnected(EventArgs e)
    {
      if (this.Disconnected == null)
        return;
      this.Disconnected((object) this, e);
    }

    protected virtual void OnReceivedData(ReceivedDataEventArgs e)
    {
      if (this.ReceivedData == null)
        return;
      this.ReceivedData((object) this, e);
    }
  }
}
