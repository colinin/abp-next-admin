using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace LINGYUN.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(AbpDddApplicationModule)
        )]
    public class AbpSettingManagementApplicationModule : AbpModule
    {
    }
}
