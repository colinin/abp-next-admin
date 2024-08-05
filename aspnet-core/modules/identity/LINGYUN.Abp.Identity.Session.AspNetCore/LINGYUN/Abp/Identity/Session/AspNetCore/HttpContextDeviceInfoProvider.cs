using DeviceDetectorNET;
using System;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Identity.Session.AspNetCore;
public class HttpContextDeviceInfoProvider : IDeviceInfoProvider, ITransientDependency
{
    protected IWebClientInfoProvider WebClientInfoProvider { get; }

    public HttpContextDeviceInfoProvider(
        IWebClientInfoProvider webClientInfoProvider)
    {
        WebClientInfoProvider = webClientInfoProvider;
    }

    public DeviceInfo DeviceInfo => GetDeviceInfo();

    public string ClientIpAddress => WebClientInfoProvider.ClientIpAddress;

    protected virtual DeviceInfo GetDeviceInfo()
    {
        var deviceInfo = BrowserDeviceInfo.Parse(WebClientInfoProvider.BrowserInfo);

        return new DeviceInfo(
            deviceInfo.Device,
            deviceInfo.Description,
            WebClientInfoProvider.ClientIpAddress);
    }

    private class BrowserDeviceInfo
    {
        public string Device { get; }
        public string Description { get; }

        public BrowserDeviceInfo(string device, string description)
        {
            Device = device;
            Description = description;
        }

        public static BrowserDeviceInfo Parse(string browserInfo)
        {
            string device = null;
            string deviceInfo = null;
            if (!browserInfo.IsNullOrWhiteSpace())
            {
                var deviceDetector = new DeviceDetector(browserInfo);
                deviceDetector.Parse();
                if (deviceDetector.IsParsed())
                {
                    var osInfo = deviceDetector.GetOs();
                    if (deviceDetector.IsMobile())
                    {
                        // IdentitySessionDevices.Mobile
                        device = osInfo.Success ? osInfo.Match.Name : "Mobile";
                    }
                    else if (deviceDetector.IsBrowser())
                    {
                        // IdentitySessionDevices.Web
                        device = "Web";
                    }
                    else if (deviceDetector.IsDesktop())
                    {
                        // TODO: PC
                        device = "Desktop";
                    }

                    if (osInfo.Success)
                    {
                        deviceInfo = osInfo.Match.Name + " " + osInfo.Match.Version;
                    }

                    var clientInfo = deviceDetector.GetClient();
                    if (clientInfo.Success)
                    {
                        deviceInfo = deviceInfo.IsNullOrWhiteSpace() ? clientInfo.Match.Name : deviceInfo + " " + clientInfo.Match.Name;
                    }
                }
            }
            return new BrowserDeviceInfo(device, deviceInfo);
        }
    }
}
