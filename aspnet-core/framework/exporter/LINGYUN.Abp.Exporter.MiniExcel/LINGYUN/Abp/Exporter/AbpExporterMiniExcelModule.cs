using System;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Exporter.MiniExcel;

[Obsolete("Use AbpExporterMiniSoftwareModule")]
[DependsOn(typeof(AbpExporterCoreModule))]
public class AbpExporterMiniExcelModule : AbpModule
{
}