using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AspNetCore.SignalR.Protocol.Json
{
    [DependsOn(
        typeof(AbpAspNetCoreSignalRModule))]
    public class AbpAspNetCoreSignalRProtocolJsonModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var newtonsoftJsonOptions = new NewtonsoftJsonHubProtocolOptions();
            context.Services.ExecutePreConfiguredActions(newtonsoftJsonOptions);

            context.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IHubProtocol, NewtonsoftJsonHubProtocol>());
        }
    }
}
