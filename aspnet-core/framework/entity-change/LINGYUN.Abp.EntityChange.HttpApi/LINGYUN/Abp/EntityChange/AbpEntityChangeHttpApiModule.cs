using LINGYUN.Abp.EntityChange.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.EntityChange;

[DependsOn(
    typeof(AbpEntityChangeApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpEntityChangeHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpEntityChangeHttpApiModule).Assembly);
        });

        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AbpEntityChangeResource), typeof(AbpEntityChangeApplicationContractsModule).Assembly);
        });
    }
}
