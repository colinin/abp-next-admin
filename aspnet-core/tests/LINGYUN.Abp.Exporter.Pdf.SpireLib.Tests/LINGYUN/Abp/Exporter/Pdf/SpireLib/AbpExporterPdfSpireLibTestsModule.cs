using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Exporter.Pdf.SpireLib;

[DependsOn(
    typeof(AbpExporterPdfSpireLibModule),
    typeof(AbpExporterPdfTestsModule),
    typeof(AbpAutofacModule))]
public class AbpExporterPdfSpireLibTestsModule : AbpModule
{
}
