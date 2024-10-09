using Magicodes.ExporterAndImporter.Excel;
using Magicodes.ExporterAndImporter.Excel.Utility;
using System;
using System.IO;

namespace LINGYUN.Abp.Exporter.MagicodesIE.Excel;
public class AbpImportHelper<T> : ImportHelper<T> where T : class, new()
{
    public AbpImportHelper(string? filePath = null, string? labelingFilePath = null)
        : base(filePath, labelingFilePath)
    {
    }

    public AbpImportHelper(Stream stream, Stream? labelingFileStream)
        : base(stream, labelingFileStream)
    {
    }

    internal void ConfigureExcelImportSettings(Action<ExcelImporterAttribute>? excelImportSettingsConfig)
    {
        excelImportSettingsConfig?.Invoke(ExcelImporterSettings);
    }
}
