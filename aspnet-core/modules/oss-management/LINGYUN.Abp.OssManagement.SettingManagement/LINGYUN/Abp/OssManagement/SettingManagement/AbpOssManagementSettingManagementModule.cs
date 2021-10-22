using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.SettingManagement
{
    [DependsOn(
        typeof(AbpOssManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class AbpOssManagementSettingManagementModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpOssManagementSettingManagementModule).Assembly);
            });
        }
    }
}
