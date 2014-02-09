// Type: net_base.net_useful
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using System.Net;
using System.Net.Sockets;

namespace net_base
{
  public static class net_useful
  {
    public static string GetHostName()
    {
      return Dns.GetHostName();
    }

    public static IPAddress[] GetLocalIpAddress()
    {
      try
      {
        return Dns.GetHostAddresses(Dns.GetHostName());
      }
      catch
      {
        return (IPAddress[]) null;
      }
    }

    public static IPAddress[] GetLocalIpAddress_Ipv4()
    {
      return net_useful.GetAddressListIpv4(net_useful.GetLocalIpAddress());
    }

    public static IPAddress[] GetLocalIpAddress_Ipv6()
    {
      return net_useful.GetAddressListIpv6(net_useful.GetLocalIpAddress());
    }

    public static IPAddress[] GetIpAddress(string host)
    {
      return Dns.GetHostEntry(host).AddressList;
    }

    public static IPAddress[] GetIpAddressIpv4(string host)
    {
      return net_useful.GetAddressListIpv4(net_useful.GetIpAddress(host));
    }

    public static IPAddress[] GetIpAddressIpv6(string host)
    {
      return net_useful.GetAddressListIpv6(net_useful.GetIpAddress(host));
    }

    public static IPAddress[] GetAddressListIpv4(IPAddress[] list)
    {
      return net_useful.GetAddressList(list, AddressFamily.InterNetwork);
    }

    public static IPAddress[] GetAddressListIpv6(IPAddress[] list)
    {
      return net_useful.GetAddressList(list, AddressFamily.InterNetworkV6);
    }

    public static IPAddress[] GetAddressList(IPAddress[] list, AddressFamily family)
    {
      if (list == null)
        return (IPAddress[]) null;
      int length = 0;
      foreach (IPAddress ipAddress in list)
      {
        if (ipAddress.AddressFamily == family)
          ++length;
      }
      if (length <= 0)
        return (IPAddress[]) null;
      IPAddress[] ipAddressArray = new IPAddress[length];
      int num = 0;
      foreach (IPAddress ipAddress in list)
      {
        if (ipAddress.AddressFamily == family)
          ipAddressArray[num++] = ipAddress;
      }
      return ipAddressArray;
    }
  }
}
