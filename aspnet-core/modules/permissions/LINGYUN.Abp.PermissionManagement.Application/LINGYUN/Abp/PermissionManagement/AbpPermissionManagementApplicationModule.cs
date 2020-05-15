using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace LINGYUN.Abp.PermissionManagement
{
    [DependsOn(
        typeof(AbpPermissionManagementDomainModule),
        typeof(AbpPermissionManagementApplicationContractsModule)
        )]
    public class AbpPermissionManagementApplicationModule : AbpModule
    {
    }
}
