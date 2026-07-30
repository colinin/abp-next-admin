using LINGYUN.Abp.AspNetCore.WebClientInfo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Net;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AspNetCore.HttpOverrides;

[DependsOn(typeof(AbpAspNetCoreModule))]
public class AbpAspNetCoreHttpOverridesModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<ForwardedHeadersOptions>(options =>
        {
            var forwardedConfig = configuration.GetSection("Forwarded");
            forwardedConfig.Bind(options);

            var knownProxiesConfig = configuration.GetSection("Forwarded:KnownProxies");
            if (knownProxiesConfig.Exists())
            {
                options.KnownProxies.Clear();
                var knownProxies = knownProxiesConfig.Get<List<string>>() ?? [];
                foreach (var knownProxy in knownProxies)
                {
                    if (IPAddress.TryParse(knownProxy, out var iPAddress))
                    {
                        options.KnownProxies.Add(iPAddress);
                    }
                }
            }

            var knownIPNetworksConfig = configuration.GetSection("Forwarded:KnownIPNetworks");
            if (knownIPNetworksConfig.Exists())
            {
                options.KnownIPNetworks.Clear();
                var knownIPNetworks = knownIPNetworksConfig.Get<List<string>>() ?? [];
                foreach (var knownIPNetwork in knownIPNetworks)
                {
                    if (IPNetwork.TryParse(knownIPNetwork, out var iPNetwork))
                    {
                        options.KnownIPNetworks.Add(iPNetwork);
                    }
                }
            }
        });

        context.Services.Replace(ServiceDescriptor.Transient<IWebClientInfoProvider, RequestForwardedHeaderWebClientInfoProvider>());
    }
}
