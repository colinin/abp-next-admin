using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement
{
    [DependsOn(
        typeof(AbpOssManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class AbpOssManagementHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                   typeof(AbpOssManagementApplicationContractsModule).Assembly,
                   OssManagementRemoteServiceConsts.RemoteServiceName
               );
        }
    }
}
