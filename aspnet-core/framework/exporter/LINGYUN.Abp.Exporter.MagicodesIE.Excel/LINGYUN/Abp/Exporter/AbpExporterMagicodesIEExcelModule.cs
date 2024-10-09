using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Exporter.MagicodesIE.Excel;

[DependsOn(typeof(AbpExporterCoreModule))]
public class AbpExporterMagicodesIEExcelModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddScoped<IExcelExporter, ExcelExporter>();

        context.Services.AddScoped<IExcelImporter, ExcelImporter>();

        context.Services.AddScoped<IExportFileByTemplate, ExcelExporter>();

        context.Services.Replace(
            ServiceDescriptor.Transient(
                typeof(IExporterProvider),
                typeof(MagicodesIEExcelExporterProvider)));

        context.Services.Replace(
            ServiceDescriptor.Transient(
                typeof(IImporterProvider),
                typeof(MagicodesIEExcelImporterProvider)));
    }
}