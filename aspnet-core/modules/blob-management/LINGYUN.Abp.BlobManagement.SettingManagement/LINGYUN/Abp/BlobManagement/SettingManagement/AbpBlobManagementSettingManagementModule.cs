using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement.SettingManagement;

[DependsOn(
    typeof(AbpBlobManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpBlobManagementSettingManagementModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpBlobManagementSettingManagementModule).Assembly);
        });
    }
}
