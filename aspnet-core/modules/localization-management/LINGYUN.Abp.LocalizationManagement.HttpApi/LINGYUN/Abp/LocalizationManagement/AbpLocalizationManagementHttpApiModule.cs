using LINGYUN.Abp.AspNetCore.Mvc.Localization;
using LINGYUN.Abp.LocalizationManagement.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpLocalization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;

namespace LINGYUN.Abp.LocalizationManagement
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcLocalizationModule),
        typeof(AbpLocalizationManagementApplicationContractsModule))]
    public class AbpLocalizationManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            // Dto验证本地化
            PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(LocalizationManagementResource),
                    typeof(AbpLocalizationManagementApplicationContractsModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpLocalizationManagementApplicationContractsModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<LocalizationManagementResource>()
                    .AddBaseTypes(typeof(AbpValidationResource), typeof(AbpLocalizationResource));
            });
        }
    }
}
