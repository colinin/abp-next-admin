using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Exporter.Pdf;

[DependsOn(
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpExporterPdfModule),
    typeof(AbpTestsBaseModule))]
public class AbpExporterPdfTestsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpExporterPdfTestsModule>();
        });
    }
}
