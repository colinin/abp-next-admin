using Magicodes.ExporterAndImporter.Core.Models;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Exporter.MagicodesIE.Excel;

public class MagicodesIEExcelImporterProvider : IImporterProvider, ITransientDependency
{
    public ILogger<MagicodesIEExcelImporterProvider> Logger { protected get; set; }

    private readonly AbpExporterMagicodesIEExcelOptions _options;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IExcelImporter _excelImporter;

    public MagicodesIEExcelImporterProvider(
        IJsonSerializer jsonSerializer, 
        IExcelImporter excelImporter,
        IOptions<AbpExporterMagicodesIEExcelOptions> options)
    {
        _jsonSerializer = jsonSerializer;
        _excelImporter = excelImporter;
        _options = options.Value;

        Logger = NullLogger<MagicodesIEExcelImporterProvider>.Instance;
    }

    public async virtual Task<IReadOnlyCollection<T>> ImportAsync<T>(Stream stream) where T : class, new()
    {
        using var importHelper = new AbpImportHelper<T>(stream, null);

        // 由于Microsoft.IE.Excel官方此接口未暴露用户配置,做一次转换
        if (_options.ImportSettingMapping.TryGetValue(typeof(T), out var importSetting))
        {
            importHelper.ConfigureExcelImportSettings(importSetting);
        }

        var importResult = await importHelper.Import(null, null);

        ThrowImportException(importResult);

        return importResult.Data.ToImmutableList();
    }

    protected virtual void ThrowImportException<T>(ImportResult<T> importResult) where T : class, new()
    {
        if (importResult.HasError)
        {
            if (importResult.RowErrors.Count > 0)
            {
                var errorJson = _jsonSerializer.Serialize(importResult.RowErrors);
                Logger.LogWarning("Row validation error occurred during data import process, details: {errorJson}", errorJson);
            }

            if (importResult.TemplateErrors.Count > 0)
            {
                var errorJson = _jsonSerializer.Serialize(importResult.TemplateErrors);
                Logger.LogWarning("Template validation error occurred during data import process, details: {errorJson}", errorJson);
            }

            throw new BusinessException(
                code: ExporterErrorCodes.ImportDataError,
                innerException: importResult.Exception);
        }
    }
}
