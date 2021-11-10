using LINGYUN.Abp.MessageService.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MessageService
{
    [DependsOn(
        typeof(AbpMessageServiceApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
        )]
    public class AbpMessageServiceHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpMessageServiceHttpApiModule).Assembly);
            });

            PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(MessageServiceResource), // 用于本地化的资源
                    typeof(AbpMessageServiceApplicationContractsModule).Assembly);  // Dto所在程序集
            });
        }
    }
}
