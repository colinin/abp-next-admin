using LINGYUN.Abp.BlobManagement.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement;

[DependsOn(
    typeof(AbpBlobManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule)
    )]
public class AbpBlobManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpBlobManagementHttpApiModule).Assembly);
        });

        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(BlobManagementResource),
                typeof(AbpBlobManagementApplicationContractsModule).Assembly);
        });
    }
}
