using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AspNetCore.Mvc.Client
{
    [DependsOn(
       typeof(AbpAspNetCoreMvcClientCommonModule),
       typeof(AbpEventBusModule)
       )]
    public class AbpAspNetCoreMvcClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpAspNetCoreMvcClientCacheOptions>(configuration.GetSection("AbpMvcClient:Cache"));
        }
    }
}
