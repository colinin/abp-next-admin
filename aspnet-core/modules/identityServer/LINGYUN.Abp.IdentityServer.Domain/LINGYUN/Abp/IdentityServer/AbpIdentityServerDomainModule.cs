using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.IdentityServer;

[DependsOn(typeof(Volo.Abp.IdentityServer.AbpIdentityServerDomainModule))]
public class AbpIdentityServerDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpIdentityServerEventOptions>(options =>
        {
            options.EventServiceHandlers.Add<AbpIdentityServerEventServiceHandler>();
        });

        context.Services.Replace(ServiceDescriptor.Transient<IEventService, AbpEventService>());
    }
}
