using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace LINGYUN.Abp.Exporter.Pdf;
public abstract class AbpExporterPdfTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
{
}
