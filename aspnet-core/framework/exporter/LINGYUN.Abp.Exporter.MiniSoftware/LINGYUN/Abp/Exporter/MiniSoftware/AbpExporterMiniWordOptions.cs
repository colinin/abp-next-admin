using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Exporter.MiniSoftware;

public class AbpExporterMiniWordOptions
{
    public IDictionary<Type, string> ExportTemplateMapping { get; }
    public AbpExporterMiniWordOptions()
    {
        ExportTemplateMapping = new Dictionary<Type, string>();
    }

    public void MapExportTemplate(Type dataType, string templateFilePath)
    {
        ExportTemplateMapping[dataType] = templateFilePath;
    }
}
