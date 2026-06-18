using Microsoft.Extensions.Options;
using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Exporter.MiniSoftware;

#pragma warning disable CS0618
[ExposeServices(typeof(IImporterProvider))]
#pragma warning restore CS0618
[ExposeServices(typeof(IExcelImporterProvider))]
public class MiniExcelImporterProvider : 
    IExcelImporterProvider,
#pragma warning disable CS0618
    IImporterProvider,
#pragma warning restore CS0618
    ITransientDependency
{
    private readonly AbpExporterMiniExcelOptions _options;

    public MiniExcelImporterProvider(IOptions<AbpExporterMiniExcelOptions> options)
    {
        _options = options.Value;
    }

    public async virtual Task<IReadOnlyCollection<T>> ImportAsync<T>(Stream stream) where T : class, new()
    {
        var xmlConfig = new OpenXmlConfiguration();

        if (_options.ImportSettingMapping.TryGetValue(typeof(T), out var exportSetting))
        {
            exportSetting?.Invoke(xmlConfig);
        }

        var dataList = await stream.QueryAsync<T>(
            excelType: ExcelType.XLSX,
            configuration: xmlConfig);

        return dataList.ToImmutableList();
    }
}
