using LINGYUN.Abp.Dapr.Client;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WebhooksManagement;

[DependsOn(
    typeof(AbpDaprClientModule),
    typeof(WebhooksManagementApplicationContractsModule))]
public class WebhooksManagementDaprClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddDaprClientProxies(
            typeof(WebhooksManagementApplicationContractsModule).Assembly,
            WebhooksManagementRemoteServiceConsts.RemoteServiceName);
    }
}
