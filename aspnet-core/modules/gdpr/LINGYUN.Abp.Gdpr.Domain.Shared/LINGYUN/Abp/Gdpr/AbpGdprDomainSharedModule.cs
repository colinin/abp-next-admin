using LINGYUN.Abp.Gdpr.Localization;
using Volo.Abp.Domain;
using Volo.Abp.Features;
using Volo.Abp.Gdpr;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Gdpr;

[DependsOn(
    typeof(AbpFeaturesModule),
    typeof(AbpDddDomainSharedModule),
    typeof(AbpGdprAbstractionsModule)
    )]
public class AbpGdprDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpGdprDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<GdprResource>("en")
                .AddVirtualJson("/LINGYUN/Abp/Gdpr/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(GdprErrorCodes.Namespace, typeof(GdprResource));
        });
    }
}
