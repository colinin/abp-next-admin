using LINGYUN.Abp.TextTemplating.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.TextTemplating;

[DependsOn(
    typeof(AbpValidationModule),
    typeof(AbpMultiTenancyAbstractionsModule))]
public class AbpTextTemplatingDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpTextTemplatingDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpTextTemplatingResource>("en")
                .AddVirtualJson("/LINGYUN/Abp/TextTemplating/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(AbpTextTemplatingErrorCodes.Namespace, typeof(AbpTextTemplatingResource));
        });
    }
}
