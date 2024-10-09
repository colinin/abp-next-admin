using LINGYUN.Abp.Exporter.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Exporter;

[DependsOn(typeof(AbpLocalizationModule))]
public class AbpExporterCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpExporterCoreModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpExporterResource>("en")
                .AddVirtualJson("/LINGYUN/Abp/Exporter/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(ExporterErrorCodes.Namespace, typeof(AbpExporterResource));
        });
    }
}