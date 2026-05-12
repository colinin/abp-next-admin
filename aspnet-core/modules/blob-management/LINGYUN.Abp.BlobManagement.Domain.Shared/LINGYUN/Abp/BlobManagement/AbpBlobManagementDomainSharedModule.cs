using LINGYUN.Abp.BlobManagement.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.BlobManagement;

[DependsOn(
    typeof(AbpFeaturesModule),
    typeof(AbpValidationModule))]
public class AbpBlobManagementDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpBlobManagementDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<BlobManagementResource>("en")
                .AddVirtualJson("/LINGYUN/Abp/BlobManagement/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(BlobManagementErrorCodes.Namespace, typeof(BlobManagementResource));
        });
    }
}
