using LINGYUN.Abp.PrintDesigner.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.PrintDesigner;

[DependsOn(
    typeof(AbpValidationModule),
    typeof(AbpMultiTenancyAbstractionsModule))]
public class AbpPrintDesignerDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPrintDesignerDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<PrintDesignerResource>()
                .AddVirtualJson("/LINGYUN/Abp/PrintDesigner/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(PrintDesignerErrorCodes.Namespace, typeof(PrintDesignerResource));
        });
    }
}
