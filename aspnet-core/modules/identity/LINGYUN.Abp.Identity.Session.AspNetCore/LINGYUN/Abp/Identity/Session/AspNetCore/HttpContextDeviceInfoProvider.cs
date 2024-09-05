using DeviceDetectorNET;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Identity.Session.AspNetCore;
public class HttpContextDeviceInfoProvider : IDeviceInfoProvider, ITransientDependency
{
    protected IIpLocationInfoProvider IpLocationInfoProvider { get; }
    protected IWebClientInfoProvider WebClientInfoProvider { get; }
    protected AbpIdentitySessionAspNetCoreOptions Options { get; }

    public HttpContextDeviceInfoProvider(
        IIpLocationInfoProvider ipLocationInfoProvider,
        IWebClientInfoProvider webClientInfoProvider,
        IOptions<AbpIdentitySessionAspNetCoreOptions> options)
    {
        IpLocationInfoProvider = ipLocationInfoProvider;
        WebClientInfoProvider = webClientInfoProvider;
        Options = options.Value;
    }

    public string ClientIpAddress => WebClientInfoProvider.ClientIpAddress;

    public async virtual Task<DeviceInfo> GetDeviceInfoAsync()
    {
        var deviceInfo = BrowserDeviceInfo.Parse(WebClientInfoProvider.BrowserInfo);
        var ipAddress = WebClientInfoProvider.ClientIpAddress;
        var ipRegion = "";
        if (!ipAddress.IsNullOrWhiteSpace())
        {
            if (Options.IsParseIpLocation)
            {
                var region = await GetRegion(ipAddress);
                if (!region.IsNullOrWhiteSpace())
                {
                    ipRegion = region;
                }
            }
        }

        return new DeviceInfo(
            deviceInfo.Device,
            deviceInfo.Description,
            ipAddress,
            ipRegion);
    }

    protected async virtual Task<string> GetRegion(string ipAddress)
    {
        var locationInfo = await IpLocationInfoProvider.GetLocationInfoAsync(ipAddress);
        if (locationInfo == null)
        {
            return null;
        }

        if (Options.LocationParser != null)
        {
            return Options.LocationParser(locationInfo);
        }

        return null;
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
