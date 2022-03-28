using LINGYUN.Abp.WebhooksManagement.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;

namespace LINGYUN.Abp.WebhooksManagement;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(WebhooksManagementApplicationContractsModule))]
public class WebhooksManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(WebhooksManagementHttpApiModule).Assembly);
        });

        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(WebhooksManagementResource),
                typeof(WebhooksManagementApplicationContractsModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<WebhooksManagementResource>()
                .AddBaseTypes(typeof(AbpValidationResource));
        });
    }
}
