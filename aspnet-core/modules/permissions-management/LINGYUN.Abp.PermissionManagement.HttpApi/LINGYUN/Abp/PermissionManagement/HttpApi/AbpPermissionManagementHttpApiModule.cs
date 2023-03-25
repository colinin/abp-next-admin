using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Localization;
using VoloAbpPermissionManagementHttpApiModule = Volo.Abp.PermissionManagement.HttpApi.AbpPermissionManagementHttpApiModule;

namespace LINGYUN.Abp.PermissionManagement.HttpApi;

[DependsOn(
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(VoloAbpPermissionManagementHttpApiModule))]
public class AbpPermissionManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure(delegate (IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpPermissionManagementHttpApiModule)!.Assembly);
        });

        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(AbpPermissionManagementResource),
                typeof(AbpPermissionManagementApplicationContractsModule).Assembly);
        });
    }
}
