using LINGYUN.Abp.IP.Location;
using Microsoft.Extensions.Options;
using MyCSharp.HttpUserAgentParser;
using MyCSharp.HttpUserAgentParser.Providers;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Identity.Session.AspNetCore;
public class HttpContextDeviceInfoProvider : IDeviceInfoProvider, ITransientDependency
{
    protected IIPLocationResolver IPLocationResolver { get; }
    protected IWebClientInfoProvider WebClientInfoProvider { get; }
    protected IHttpUserAgentParserProvider HttpUserAgentParser { get; }
    protected AbpIdentitySessionAspNetCoreOptions Options { get; }

    public HttpContextDeviceInfoProvider(
        IIPLocationResolver iPLocationResolver,
        IWebClientInfoProvider webClientInfoProvider,
        IHttpUserAgentParserProvider httpUserAgentParserProvider,
        IOptions<AbpIdentitySessionAspNetCoreOptions> options)
    {
        IPLocationResolver = iPLocationResolver;
        WebClientInfoProvider = webClientInfoProvider;
        HttpUserAgentParser = httpUserAgentParserProvider;
        Options = options.Value;
    }

    public string ClientIpAddress => WebClientInfoProvider.ClientIpAddress;

    public async virtual Task<DeviceInfo> GetDeviceInfoAsync()
    {
        var deviceInfo = BrowserDeviceInfo.Parse(HttpUserAgentParser, WebClientInfoProvider.BrowserInfo);
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
        var locationInfo = await IPLocationResolver.ResolveAsync(ipAddress);
        return locationInfo.Location?.Remarks;
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

        public static BrowserDeviceInfo Parse(IHttpUserAgentParserProvider httpUserAgentParserProvider, string browserInfo)
        {
            string device = null;
            string deviceInfo = null;
            if (!browserInfo.IsNullOrWhiteSpace())
            {
                var httpUserAgentInformation = httpUserAgentParserProvider.Parse(browserInfo);
                if (!httpUserAgentInformation.MobileDeviceType.IsNullOrWhiteSpace())
                {
                    device = httpUserAgentInformation.MobileDeviceType;
                }
                else if (!httpUserAgentInformation.Name.IsNullOrWhiteSpace())
                {
                    device = "Web";
                }
                else
                {
                    device = "OAuth";
                }
                deviceInfo = httpUserAgentInformation.Type switch
                {
                    HttpUserAgentType.Browser or HttpUserAgentType.Robot => (httpUserAgentInformation.Platform.HasValue ? httpUserAgentInformation.Platform.Value.Name + " " : string.Empty) + httpUserAgentInformation.Name,
                    _ => httpUserAgentInformation.UserAgent,
                };
            }
            return new BrowserDeviceInfo(device, deviceInfo);
        }
    }
}
