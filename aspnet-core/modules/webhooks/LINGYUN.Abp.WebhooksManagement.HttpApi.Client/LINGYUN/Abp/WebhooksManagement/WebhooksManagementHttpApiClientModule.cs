using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WebhooksManagement;

[DependsOn(
    typeof(AbpHttpClientModule),
    typeof(WebhooksManagementApplicationContractsModule))]
public class WebhooksManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(WebhooksManagementApplicationContractsModule).Assembly,
            WebhooksManagementRemoteServiceConsts.RemoteServiceName);
    }
}
