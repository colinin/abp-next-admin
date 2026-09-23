using Volo.Abp.Modularity;
using VoloAbpPermissionManagementApplicationModule = Volo.Abp.PermissionManagement.AbpPermissionManagementApplicationModule;

namespace LINGYUN.Abp.PermissionManagement;

[DependsOn(
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpPermissionManagementDomainModule),
    typeof(VoloAbpPermissionManagementApplicationModule))]
public class AbpPermissionManagementApplicationModule : AbpModule
{

}
