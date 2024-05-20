using LINGYUN.Abp.DataProtection.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.DataProtectionManagement;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpDataProtectionManagementApplicationContractsModule))]
public class AbpDataProtectionManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpDataProtectionManagementHttpApiModule).Assembly);
        });

        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(DataProtectionResource),
                typeof(AbpDataProtectionManagementApplicationContractsModule).Assembly);
        });
    }
}
