using LINGYUN.Abp.AspNetCore.WebClientInfo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
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
            configuration.GetSection("Forwarded").Bind(options);
        });

        context.Services.Replace(ServiceDescriptor.Transient<IWebClientInfoProvider, RequestForwardedHeaderWebClientInfoProvider>());
    }
}
