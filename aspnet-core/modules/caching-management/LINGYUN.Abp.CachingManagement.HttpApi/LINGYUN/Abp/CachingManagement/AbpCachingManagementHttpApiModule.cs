using LINGYUN.Abp.CachingManagement.Localization;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.CachingManagement;

[DependsOn(
    typeof(AbpCachingManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpCachingManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpCachingManagementHttpApiModule).Assembly);
        });

        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(CacheResource),
                typeof(AbpCachingManagementApplicationContractsModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CacheResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}