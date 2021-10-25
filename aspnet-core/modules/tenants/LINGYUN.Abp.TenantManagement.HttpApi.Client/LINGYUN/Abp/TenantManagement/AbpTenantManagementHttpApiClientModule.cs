using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.TenantManagement
{
    [DependsOn(
        typeof(AbpTenantManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class AbpTenantManagementHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AbpTenantManagementApplicationContractsModule).Assembly,
                TenantManagementRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}
