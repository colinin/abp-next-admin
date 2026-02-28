using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Caching;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement;

[DependsOn(
    typeof(AbpOssManagementDomainModule),
    typeof(AbpOssManagementApplicationContractsModule),
    typeof(AbpCachingModule),
    typeof(AbpMapperlyModule),
    typeof(AbpDddApplicationModule))]
public class AbpOssManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpOssManagementApplicationModule>();
    }
}
