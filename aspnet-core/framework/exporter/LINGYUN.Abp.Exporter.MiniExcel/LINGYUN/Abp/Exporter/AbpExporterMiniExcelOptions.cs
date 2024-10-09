using MiniExcelLibs.OpenXml;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Exporter.MiniExcel;
public class AbpExporterMiniExcelOptions
{
    public IDictionary<Type, Action<OpenXmlConfiguration>> ExportSettingMapping { get; }
    public IDictionary<Type, Action<OpenXmlConfiguration>> ImportSettingMapping { get; }
    public AbpExporterMiniExcelOptions()
    {
        ExportSettingMapping = new Dictionary<Type, Action<OpenXmlConfiguration>>();
        ImportSettingMapping = new Dictionary<Type, Action<OpenXmlConfiguration>>();
    }

    public void MapExportSetting(Type dataType, Action<OpenXmlConfiguration> settingMapping)
    {
        ExportSettingMapping[dataType] = settingMapping;
    }

    public void MapImportSetting(Type dataType, Action<OpenXmlConfiguration> settingMapping)
    {
        ImportSettingMapping[dataType] = settingMapping;
    }
}
