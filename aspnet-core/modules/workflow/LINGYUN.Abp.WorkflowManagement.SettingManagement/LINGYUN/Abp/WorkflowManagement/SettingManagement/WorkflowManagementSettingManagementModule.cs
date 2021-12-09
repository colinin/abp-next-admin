using LINGYUN.Abp.SettingManagement;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace LINGYUN.Abp.WorkflowManagement.SettingManagement
{
    [DependsOn(
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpSettingManagementDomainModule))]
    public class WorkflowManagementSettingManagementModule : AbpModule
    {
    }
}
