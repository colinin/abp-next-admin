using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using VoloAbpFeatureManagementHttpApiModule = Volo.Abp.FeatureManagement.AbpFeatureManagementHttpApiModule;

namespace LINGYUN.Abp.FeatureManagement.HttpApi;

[DependsOn(
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(VoloAbpFeatureManagementHttpApiModule))]
public class AbpFeatureManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure(delegate (IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpFeatureManagementHttpApiModule)!.Assembly);
        });

        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(AbpFeatureManagementResource),
                typeof(AbpFeatureManagementApplicationContractsModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Get<AbpFeatureManagementResource>()
                .AddBaseTypes(typeof(AbpValidationResource));
        });
    }
}
