using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.BlobStoring.Extensions;
using LINGYUN.Abp.ElsaNext.Localization;
using Volo.Abp.BlobStoring;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.BlobStoring;

[DependsOn(
    typeof(AbpBlobStoringModule),
    typeof(AbpElsaNextModule))]
public class AbpElsaNextBlobStoringModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseAbpBlobStoring();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextBlobStoringModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaNextResource>()
                .AddVirtualJson("/LINGYUN/Abp/ElsaNext/BlobStoring/Localization/Resources");
        });
    }
}
