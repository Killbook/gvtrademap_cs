// Type: Utility.RequestCtrl
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

namespace Utility
{
  public class RequestCtrl
  {
    private bool m_request;
    private object m_arg1;
    private object m_arg2;

    public object Arg1
    {
      get
      {
        return this.m_arg1;
      }
    }

    public object Arg2
    {
      get
      {
        return this.m_arg2;
      }
    }

    public RequestCtrl()
    {
      this.CancelRequest();
      this.m_arg1 = (object) null;
      this.m_arg2 = (object) null;
    }

    public void Request()
    {
      this.m_request = true;
      this.m_arg1 = (object) null;
      this.m_arg2 = (object) null;
    }

    public void Request(object arg1)
    {
      this.m_request = true;
      this.m_arg1 = arg1;
      this.m_arg2 = (object) null;
    }

    public void Request(object arg1, object arg2)
    {
      this.m_request = true;
      this.m_arg1 = arg1;
      this.m_arg2 = arg2;
    }

    public void CancelRequest()
    {
      this.m_request = false;
    }

    public bool IsRequest()
    {
      bool flag = this.m_request;
      this.CancelRequest();
      return flag;
    }
  }
}
