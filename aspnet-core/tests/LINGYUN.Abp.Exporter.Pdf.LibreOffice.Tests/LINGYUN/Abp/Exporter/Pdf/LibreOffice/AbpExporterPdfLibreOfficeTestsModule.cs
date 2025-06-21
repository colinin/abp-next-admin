using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Exporter.Pdf.LibreOffice;

[DependsOn(
    typeof(AbpExporterPdfLibreOfficeModule),
    typeof(AbpExporterPdfTestsModule),
    typeof(AbpAutofacModule))]
public class AbpExporterPdfLibreOfficeTestsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        if (!LibreOfficeCommands.IsLibreOffliceInstalled())
        {
            context.Services.AddSingleton<LibreOfficeTestEnvironment>();
        }
    }
}
