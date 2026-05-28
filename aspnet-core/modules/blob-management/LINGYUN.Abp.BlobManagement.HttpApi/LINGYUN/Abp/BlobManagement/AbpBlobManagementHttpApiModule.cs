using LINGYUN.Abp.BlobManagement.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.VirtualFileSystem;
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

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAspNetCoreContentOptions>(options =>
        {
            options.ContentTypeMaps.TryAdd(".log", "text/plain");
            options.ContentTypeMaps.TryAdd(".text", "text/plain");
        });
        Configure<AbpBlobManagementContentTypeResolveOptions>(options =>
        {
            options.BlobContentTypeResolvers.Add(new FileExtensionBlobContentTypeResolveContributor());
        });
    }
}
