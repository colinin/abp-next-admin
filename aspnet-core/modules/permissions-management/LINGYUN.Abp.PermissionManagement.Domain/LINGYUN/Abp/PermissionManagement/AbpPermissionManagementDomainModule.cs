using Volo.Abp.Modularity;
using VoloAbpPermissionManagementDomainModule = Volo.Abp.PermissionManagement.AbpPermissionManagementDomainModule;

namespace LINGYUN.Abp.PermissionManagement;

[DependsOn(typeof(VoloAbpPermissionManagementDomainModule))]
public class AbpPermissionManagementDomainModule : AbpModule
{
}
