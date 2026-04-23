using Microsoft.Extensions.Options;
using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Exporter.MiniSoftware;

public class MiniExcelExporterProvider : 
    IExcelExporterProvider,
#pragma warning disable CS0618
    IExporterProvider,
#pragma warning restore CS0618
    ITransientDependency
{
    private readonly AbpExporterMiniExcelOptions _options;

    public MiniExcelExporterProvider(IOptions<AbpExporterMiniExcelOptions> options)
    {
        _options = options.Value;
    }

    public async virtual Task<Stream> ExportAsync(object data, CancellationToken cancellationToken = default)
    {
        var dataType = data.GetType();
        if (dataType.IsGenericType)
        {
            dataType = dataType.GetGenericArguments()[0];
        }
        return await ExportToExcelStreamAsync(dataType, data, cancellationToken);
    }

    public async virtual Task<Stream> ExportAsync<T>(ICollection<T> data, CancellationToken cancellationToken = default) where T : class, new()
    {
        return await ExportToExcelStreamAsync(typeof(T), data, cancellationToken);
    }

    protected async virtual Task<Stream> ExportToExcelStreamAsync(Type dataType, object data, CancellationToken cancellationToken = default)
    {
        var memoryStream = new MemoryStream();
        var xmlConfig = new OpenXmlConfiguration();

        if (_options.ExportSettingMapping.TryGetValue(dataType, out var exportSetting))
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
