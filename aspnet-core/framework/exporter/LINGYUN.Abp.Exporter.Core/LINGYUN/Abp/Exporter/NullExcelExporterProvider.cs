using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Exporter;

[Dependency(TryRegister = true)]
public class NullExcelExporterProvider : IExcelExporterProvider, ISingletonDependency
{
    private readonly static Stream _nullStreamCache = Stream.Null;

    public Task<Stream> ExportAsync(object data, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_nullStreamCache);
    }
}
