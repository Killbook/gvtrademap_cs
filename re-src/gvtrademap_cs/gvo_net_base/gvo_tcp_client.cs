// Type: gvo_net_base.gvo_tcp_client
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using gvo_base;
using net_base;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace gvo_net_base
{
  public class gvo_tcp_client : tcp_client_protocol_base
  {
    private readonly object m_sync_object = new object();
    public const string PROTOCOL_NAME = "GVO_NAVIGATION_PROTOCOL";
    public const int PROTOCOL_VERSION = 1;
    private const string COMMAND_CAPALL = "CAPALL";
    private const string COMMAND_CAPDAY = "CAPDAY";
    private const string COMMAND_SEAINFO = "SEAINFO";
    private const string COMMAND_ERROR = "ERROR";
    private gvo_analized_data m_received_data;
    private List<gvo_map_cs_chat_base.sea_area_type> m_sea_info;
    private bool m_enable_receive_data;

    public gvo_analized_data capture_data
    {
      get
      {
        lock (this.m_sync_object)
        {
          gvo_analized_data local_1 = this.m_received_data.Clone();
          this.m_received_data.Clear();
          return local_1;
        }
      }
    }

    public gvo_map_cs_chat_base.sea_area_type[] sea_info
    {
      get
      {
        lock (this.m_sync_object)
        {
          if (this.m_sea_info.Count <= 0)
            return (gvo_map_cs_chat_base.sea_area_type[]) null;
          gvo_map_cs_chat_base.sea_area_type[] local_2 = this.m_sea_info.ToArray();
          this.m_sea_info.Clear();
          return local_2;
        }
      }
    }

    public gvo_tcp_client()
      : base("GVO_NAVIGATION_PROTOCOL", 1)
    {
      this.init();
      this.m_enable_receive_data = false;
    }

    public gvo_tcp_client(Socket sct)
      : base("GVO_NAVIGATION_PROTOCOL", 1, sct)
    {
      this.init();
      this.m_enable_receive_data = true;
    }

    private void init()
    {
      gvo_tcp_client gvoTcpClient = this;
      tcp_client_protocol_base.ReceivedCommandEventHandler commandEventHandler = gvoTcpClient.ReceivedCommand + new tcp_client_protocol_base.ReceivedCommandEventHandler(this.received_command_handler);
      gvoTcpClient.ReceivedCommand = commandEventHandler;
      this.m_received_data = new gvo_analized_data();
      this.m_sea_info = new List<gvo_map_cs_chat_base.sea_area_type>();
    }

    public void SendCaptureAll(int days, int x, int y, float angle, bool interest, gvo_map_cs_chat_base.accident accident)
    {
      string[] datas;
      if (accident != gvo_map_cs_chat_base.accident.none)
        datas = new string[6]
        {
          days.ToString(),
          x.ToString(),
          y.ToString(),
          angle.ToString(),
          interest ? "1" : "0",
          gvo_map_cs_chat_base.ToAccidentString(accident)
        };
      else if (interest)
        datas = new string[5]
        {
          days.ToString(),
          x.ToString(),
          y.ToString(),
          angle.ToString(),
          "1"
        };
      else
        datas = new string[4]
        {
          days.ToString(),
          x.ToString(),
          y.ToString(),
          angle.ToString()
        };
      this.send_data("CAPALL", datas);
    }

    public void SendCaptureDays(int days, bool interest)
    {
      this.send_data("CAPDAY", new string[2]
      {
        days.ToString(),
        interest ? "1" : "0"
      });
    }

    public void SendSeaInfo(gvo_map_cs_chat_base.sea_area_type info)
    {
      if (info == null)
        return;
      string str = gvo_map_cs_chat_base.ToSeaTypeString(info.type);
      this.send_data("SEAINFO", new string[2]
      {
        info.name,
        str
      });
    }

    private void send_data(string command, string[] datas)
    {
      if (this.state != tcp_client_protocol_base.client_state.ready)
        return;
      try
      {
        this.SendData(command, datas);
      }
      catch
      {
      }
    }

    private void received_command_handler(object sender, string[] datas)
    {
      if (!this.m_enable_receive_data)
        return;
      lock (this.m_sync_object)
      {
        switch (datas[0])
        {
          case "CAPALL":
            this.m_received_data.Clear();
            try
            {
              this.m_received_data.days = Convert.ToInt32(datas[1]);
              this.m_received_data.pos_x = Convert.ToInt32(datas[2]);
              this.m_received_data.pos_y = Convert.ToInt32(datas[3]);
              this.m_received_data.angle = Convert.ToSingle(datas[4]);
              if (datas.Length >= 6)
                this.m_received_data.interest = Convert.ToInt32(datas[5]) != 0;
              if (datas.Length < 7)
                break;
              this.m_received_data.accident = gvo_map_cs_chat_base.ToAccident(datas[6]);
              break;
            }
            catch
            {
              this.m_received_data.Clear();
              break;
            }
          case "CAPDAY":
            this.m_received_data.Clear();
            try
            {
              this.m_received_data.days = Convert.ToInt32(datas[1]);
              this.m_received_data.interest = Convert.ToInt32(datas[2]) != 0;
              break;
            }
            catch
            {
              this.m_received_data.Clear();
              break;
            }
          case "SEAINFO":
            try
            {
              this.m_sea_info.Add(new gvo_map_cs_chat_base.sea_area_type(datas[1], gvo_map_cs_chat_base.ToSeaType(datas[2])));
              break;
            }
            catch
            {
              this.m_received_data.Clear();
              break;
            }
        }
      }
    }
  }
}
