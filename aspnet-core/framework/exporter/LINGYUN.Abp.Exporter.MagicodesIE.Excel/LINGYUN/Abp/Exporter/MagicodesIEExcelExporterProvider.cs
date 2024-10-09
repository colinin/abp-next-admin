using Magicodes.ExporterAndImporter.Excel;
using Magicodes.ExporterAndImporter.Excel.Utility;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Exporter.MagicodesIE.Excel;
public class MagicodesIEExcelExporterProvider : IExporterProvider, ITransientDependency
{
    private readonly AbpExporterMagicodesIEExcelOptions _options;
    private readonly IExcelExporter _excelExporter;

    public MagicodesIEExcelExporterProvider(IOptions<AbpExporterMagicodesIEExcelOptions> options)
    {
        _options = options.Value;
    }

    public async virtual Task<Stream> ExportAsync<T>(ICollection<T> data, CancellationToken cancellationToken = default)
        where T : class, new()
    {
        var fileBytes = new List<byte>();

        var exportHelper = new ExportHelper<T>();

        // 由于Microsoft.IE.Excel官方此接口未暴露用户配置,做一次转换
        if (_options.ExportSettingMapping.TryGetValue(typeof(T), out var exportSetting))
        {
            exportSetting?.Invoke(exportHelper.ExcelExporterSettings);
        }

        if (exportHelper.ExcelExporterSettings.MaxRowNumberOnASheet > 0 && data.Count > exportHelper.ExcelExporterSettings.MaxRowNumberOnASheet)
        {
            using (exportHelper.CurrentExcelPackage)
            {
                var num = data.Count / exportHelper.ExcelExporterSettings.MaxRowNumberOnASheet + ((data.Count % exportHelper.ExcelExporterSettings.MaxRowNumberOnASheet > 0) ? 1 : 0);
                for (var i = 0; i < num; i++)
                {
                    var dataItems2 = data.Skip(i * exportHelper.ExcelExporterSettings.MaxRowNumberOnASheet).Take(exportHelper.ExcelExporterSettings.MaxRowNumberOnASheet).ToList();
                    exportHelper.AddExcelWorksheet();
                    exportHelper.Export(dataItems2);
                }

                fileBytes.AddRange(await exportHelper.CurrentExcelPackage.GetAsByteArrayAsync());
            }
        }
        else
        {
            using var excelPackage2 = exportHelper.Export(data);
            fileBytes.AddRange(await excelPackage2.GetAsByteArrayAsync());
        }

        var memoryStream = new MemoryStream([.. fileBytes]);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;
    }
}
