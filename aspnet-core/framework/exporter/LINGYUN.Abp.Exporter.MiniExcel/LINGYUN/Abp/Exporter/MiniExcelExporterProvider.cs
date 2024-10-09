using Microsoft.Extensions.Options;
using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Exporter.MiniExcel;
public class MiniExcelExporterProvider : IExporterProvider, ITransientDependency
{
    private readonly AbpExporterMiniExcelOptions _options;

    public MiniExcelExporterProvider(IOptions<AbpExporterMiniExcelOptions> options)
    {
        _options = options.Value;
    }

    public async virtual Task<Stream> ExportAsync<T>(ICollection<T> data, CancellationToken cancellationToken = default)
        where T : class, new()
    {
        var memoryStream = new MemoryStream();
        var xmlConfig = new OpenXmlConfiguration();

        if (_options.ExportSettingMapping.TryGetValue(typeof(T), out var exportSetting))
        {
            exportSetting?.Invoke(xmlConfig);
        }

        await memoryStream.SaveAsAsync(
            value: data,
            excelType: ExcelType.XLSX,
            configuration: xmlConfig,
            cancellationToken: cancellationToken);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;
    }
}
