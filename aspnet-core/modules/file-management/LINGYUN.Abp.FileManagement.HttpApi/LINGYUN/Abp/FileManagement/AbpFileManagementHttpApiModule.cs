using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.FileManagement
{
    [DependsOn(
         typeof(AbpFileManagementApplicationContractsModule),
         typeof(AbpAspNetCoreMvcModule)
         )]
    public class AbpFileManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpFileManagementHttpApiModule).Assembly);
            });
        }
    }
}
