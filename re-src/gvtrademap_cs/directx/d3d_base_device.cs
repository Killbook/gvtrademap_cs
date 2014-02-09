// Type: directx.d3d_base_device
// Assembly: gvtrademap_cs, Version=1.3.2.3, Culture=neutral, PublicKeyToken=null
// MVID: 3D162A44-1A8B-4B7A-9FC3-6379559CB419
// Assembly location: C:\tmp\A\files\gvtrademap_cs.exe

using Microsoft.DirectX.Direct3D;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace directx
{
  public abstract class d3d_base_device : IDisposable
  {
    private Form m_form;
    private int m_adapter_index;
    private Device m_d3d_device;
    private Caps m_caps;
    private CreateFlags m_create_flags;
    private PresentParameters m_present_params;
    private DeviceType m_device_type;
    private d3d_base_device.CreateType m_create_type;

    public Form form
    {
      get
      {
        return this.m_form;
      }
    }

    public int adpter_index
    {
      get
      {
        return this.m_adapter_index;
      }
    }

    public Device device
    {
      get
      {
        return this.m_d3d_device;
      }
    }

    public Caps caps
    {
      get
      {
        return this.m_caps;
      }
    }

    public CreateFlags create_flags
    {
      get
      {
        return this.m_create_flags;
      }
    }

    public PresentParameters present_params
    {
      get
      {
        return this.m_present_params;
      }
    }

    public DeviceType device_type
    {
      get
      {
        return this.m_device_type;
      }
    }

    public d3d_base_device.CreateType create_type
    {
      get
      {
        return this.m_create_type;
      }
    }

    public void Create(Form form, PresentParameters param)
    {
      this.Create(form, param, d3d_base_device.CreateType.BestPerformance, DeviceType.Hardware);
    }

    public void Create(Form form, PresentParameters param, d3d_base_device.CreateType create_type)
    {
      this.Create(form, param, create_type, DeviceType.Hardware);
    }

    public void Create(Form form, PresentParameters param, d3d_base_device.CreateType create_type, DeviceType device_type)
    {
      this.m_form = form;
      this.m_device_type = device_type;
      this.m_create_type = create_type;
      this.m_adapter_index = Manager.Adapters.Default.Adapter;
      this.m_caps = Manager.GetDeviceCaps(this.m_adapter_index, device_type);
      this.m_present_params = param;
      this.m_create_flags = (CreateFlags) 0;
      switch (create_type)
      {
        case d3d_base_device.CreateType.BestPerformance:
          if (this.m_caps.DeviceCaps.SupportsHardwareTransformAndLight)
            this.m_create_flags |= CreateFlags.HardwareVertexProcessing;
          else
            this.m_create_flags |= CreateFlags.SoftwareVertexProcessing;
          if (this.m_caps.DeviceCaps.SupportsPureDevice && this.m_caps.DeviceCaps.SupportsHardwareTransformAndLight)
          {
            this.m_create_flags |= CreateFlags.PureDevice;
            break;
          }
          else
            break;
        case d3d_base_device.CreateType.SoftwareVertexProcessing:
          this.m_create_flags |= CreateFlags.SoftwareVertexProcessing;
          break;
        default:
          throw new Exception("CreateTypeの指定が不正");
      }
      this.m_d3d_device = new Device(this.m_adapter_index, device_type, (Control) form, this.m_create_flags, new PresentParameters[1]
      {
        this.m_present_params
      });
    }

    public virtual void Dispose()
    {
      if (this.m_d3d_device != (Device) null)
        this.m_d3d_device.Dispose();
      this.m_d3d_device = (Device) null;
    }

    public void Clear(Color color)
    {
      this.m_d3d_device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, color, 1f, 0);
    }

    public void Clear(ClearFlags flags, Color color)
    {
      this.m_d3d_device.Clear(flags, color, 1f, 0);
    }

    public virtual bool Begin()
    {
      if (!this.OnDeviceLostException())
        return false;
      this.m_d3d_device.BeginScene();
      return true;
    }

    public virtual void End()
    {
      this.m_d3d_device.EndScene();
    }

    public virtual bool Present()
    {
      if (this.m_d3d_device == (Device) null)
        return true;
      try
      {
        this.m_d3d_device.Present();
      }
      catch (DeviceLostException ex)
      {
        this.OnDeviceLostException();
      }
      catch (DriverInternalErrorException ex)
      {
        return false;
      }
      return true;
    }

    protected virtual bool OnDeviceLostException()
    {
      int result;
      if (!this.m_d3d_device.CheckCooperativeLevel(out result))
      {
        if (result == -2005530519)
          this.m_d3d_device.Reset(new PresentParameters[1]
          {
            this.m_present_params
          });
        else if (result == -2005530520)
        {
          Thread.Sleep(20);
          return false;
        }
      }
      return true;
    }

    public bool CheckDeviceFormat(Usage usage, ResourceType resource_type, DepthFormat format)
    {
      if (this.m_d3d_device == (Device) null)
        return false;
      else
        return Manager.CheckDeviceFormat(this.m_adapter_index, this.m_device_type, this.m_present_params.Windowed ? this.m_d3d_device.DisplayMode.Format : this.m_present_params.BackBufferFormat, usage, resource_type, format);
    }

    public bool CheckDeviceFormat(Usage usage, ResourceType resource_type, Microsoft.DirectX.Direct3D.Format format)
    {
      if (this.m_d3d_device == (Device) null)
        return false;
      else
        return Manager.CheckDeviceFormat(this.m_adapter_index, this.m_device_type, this.m_present_params.Windowed ? this.m_d3d_device.DisplayMode.Format : this.m_present_params.BackBufferFormat, usage, resource_type, format);
    }

    public enum CreateType
    {
      BestPerformance,
      SoftwareVertexProcessing,
    }
  }
}
