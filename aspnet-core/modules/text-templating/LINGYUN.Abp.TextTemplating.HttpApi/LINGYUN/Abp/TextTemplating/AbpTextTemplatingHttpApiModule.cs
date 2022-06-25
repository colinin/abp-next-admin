using LINGYUN.Abp.TextTemplating.Localization;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.TextTemplating;

[DependsOn(
    typeof(AbpTextTemplatingApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpTextTemplatingHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpTextTemplatingHttpApiModule).Assembly);
        });

        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(AbpTextTemplatingResource),
                typeof(AbpTextTemplatingApplicationContractsModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpTextTemplatingResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
