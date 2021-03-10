using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement
{
    [DependsOn(
         typeof(AbpOssManagementApplicationContractsModule),
         typeof(AbpAspNetCoreMvcModule)
         )]
    public class AbpOssManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpOssManagementHttpApiModule).Assembly);
            });
        }
    }
}
