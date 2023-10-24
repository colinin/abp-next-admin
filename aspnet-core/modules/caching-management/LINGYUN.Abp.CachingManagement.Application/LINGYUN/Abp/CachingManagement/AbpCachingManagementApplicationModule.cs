using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.CachingManagement;

[DependsOn(
    typeof(AbpCachingManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule))]
public class AbpCachingManagementApplicationModule : AbpModule
{

}