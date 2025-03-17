using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Identity.QrCode;

[DependsOn(typeof(AbpIdentityDomainModule))]
public class AbpIdentityQrCodeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpIdentityQrCodeModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<IdentityResource>()
                .AddVirtualJson("/LINGYUN/Abp/Identity/QrCode/Localization/Resources");
        });
    }
}
