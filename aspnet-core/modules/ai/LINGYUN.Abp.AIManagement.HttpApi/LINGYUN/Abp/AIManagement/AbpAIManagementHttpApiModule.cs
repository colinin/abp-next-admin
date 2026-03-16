using LINGYUN.Abp.AIManagement.Localization;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AIManagement;

[DependsOn(
    typeof(AbpAIManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpAIManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AIManagementResource), typeof(AbpAIManagementApplicationContractsModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAIManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AIManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });

        context.Services.AddTransient<SseAsyncEnumerableResultFilter>();
    }
}
