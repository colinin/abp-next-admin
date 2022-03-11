using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Saas;

[DependsOn(typeof(AbpSaasApplicationContractsModule))]
[DependsOn(typeof(AbpHttpClientModule))]
public class AbpSaasHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(AbpSaasApplicationContractsModule).Assembly,
            AbpSaasRemoteServiceConsts.RemoteServiceName
        );
    }
}
