using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Localization;

namespace LINGYUN.Abp.SettingManagement;

[DependsOn(
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpAspNetCoreMvcModule)
    )]
public class AbpSettingManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpSettingManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpSettingManagementResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource),
                    typeof(IdentityResource),
                    typeof(AccountResource)
                );
        });
    }
}
