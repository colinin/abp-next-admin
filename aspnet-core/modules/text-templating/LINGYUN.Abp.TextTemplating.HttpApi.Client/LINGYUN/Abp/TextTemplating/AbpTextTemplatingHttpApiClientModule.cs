using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.TextTemplating;

[DependsOn(
    typeof(AbpTextTemplatingApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpTextTemplatingHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(AbpTextTemplatingApplicationContractsModule).Assembly,
            AbpTextTemplatingRemoteServiceConsts.RemoteServiceName
        );
    }
}
