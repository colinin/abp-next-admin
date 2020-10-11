using Volo.Abp.Modularity;

namespace LINGYUN.Abp.PermissionManagement
{
    [DependsOn(
        typeof(Volo.Abp.PermissionManagement.AbpPermissionManagementDomainModule))]
    public class AbpPermissionManagementDomainModule : AbpModule
    {

    }
}
