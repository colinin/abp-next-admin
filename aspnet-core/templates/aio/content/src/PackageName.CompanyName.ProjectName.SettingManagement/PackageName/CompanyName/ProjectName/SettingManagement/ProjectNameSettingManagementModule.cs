using LINGYUN.Abp.SettingManagement;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace PackageName.CompanyName.ProjectName.SettingManagement;

[DependsOn(
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpSettingManagementDomainModule))]
public class ProjectNameSettingManagementModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ProjectNameSettingManagementModule).Assembly);
        });
    }
}
