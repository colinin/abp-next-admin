using Magicodes.ExporterAndImporter.Excel;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Exporter.MagicodesIE.Excel;
public class AbpExporterMagicodesIEExcelOptions
{
    /// <summary>
    /// 数据导出设置
    /// </summary>
    public IDictionary<Type, Action<ExcelExporterAttribute>> ExportSettingMapping { get; }
    /// <summary>
    /// 数据导入设置
    /// </summary>
    public IDictionary<Type, Action<ExcelImporterAttribute>> ImportSettingMapping { get; }
    public AbpExporterMagicodesIEExcelOptions()
    {
        ExportSettingMapping = new Dictionary<Type, Action<ExcelExporterAttribute>>();
        ImportSettingMapping = new Dictionary<Type, Action<ExcelImporterAttribute>>();
    }

    public void MapExportSetting(Type dataType, Action<ExcelExporterAttribute> exportSettingsSetup)
    {
        ExportSettingMapping[dataType] = exportSettingsSetup;
    }

    public void MapImportSetting(Type dataType, Action<ExcelImporterAttribute> importSettingsSetup)
    {
        ImportSettingMapping[dataType] = importSettingsSetup;
    }
}
