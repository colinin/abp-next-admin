using Magicodes.ExporterAndImporter.Excel.Utility;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Exporter.MagicodesIE.Excel;

#pragma warning disable CS0618
[ExposeServices(typeof(IExporterProvider))]
#pragma warning restore CS0618
[ExposeServices(typeof(IExcelExporterProvider))]
public class MagicodesIEExcelExporterProvider :
    IExcelExporterProvider,
#pragma warning disable CS0618
    IExporterProvider,
#pragma warning restore CS0618
    ITransientDependency
{
    private readonly AbpExporterMagicodesIEExcelOptions _options;

    public MagicodesIEExcelExporterProvider(IOptions<AbpExporterMagicodesIEExcelOptions> options)
    {
        _options = options.Value;
    }

    public async virtual Task<Stream> ExportAsync(object data, CancellationToken cancellationToken = default)
    {
        var dataType = data.GetType();

        var collectionType = dataType.GetInterface(typeof(ICollection<>).Name);
        if (collectionType != null)
        {
            var itemType = collectionType.GetGenericArguments()[0];

            var exportMethod = GetType().GetMethod(
                nameof(ExportAsync), 
                new[] { 
                    typeof(ICollection<>).MakeGenericType(itemType), 
                    typeof(CancellationToken)
                });

            if (exportMethod != null)
            {
                var genericMethod = exportMethod.MakeGenericMethod(itemType);
                var result = genericMethod.Invoke(this, new[] { data, cancellationToken });

                if (result is Task<Stream> task)
                {
                    return await task;
                }
            }
        }

        throw new NotSupportedException($"Type {dataType.Name} is not supported. Expected ICollection<T>.");
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
