using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AspNetCore.SignalR.Protocol.Json
{
    [DependsOn(
        typeof(AbpAspNetCoreSignalRModule))]
    public class AbpAspNetCoreSignalRProtocolJsonModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<ISignalRServerBuilder>(builder =>
            {
                builder.AddJsonProtocol();
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.TryAddEnumerable(ServiceDescriptor
                .Transient<IConfigureOptions<JsonHubProtocolOptions>, JsonHubProtocolOptionsSetup>());
        }
    }
}
