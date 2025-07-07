using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Exporter.Pdf.LibreOffice;

[DependsOn(typeof(AbpExporterPdfModule))]
public class AbpExporterPdfLibreOfficeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 解决单元测试问题
        if (context.Services.IsAdded<LibreOfficeTestEnvironment>())
        {
            return;
        }
        if (!LibreOfficeCommands.IsLibreOffliceInstalled())
        {
            throw new Volo.Abp.AbpInitializationException("Libreoffice not installed in the current operation environment of the host, please refer to the document after installation using ` AbpExporterPdfLibreOfficeModule ` module.");
        }
        context.Services.AddTransient<IExcelToPdfProvider, LibreOfficeExcelToPdfProvider>();
    }
}
