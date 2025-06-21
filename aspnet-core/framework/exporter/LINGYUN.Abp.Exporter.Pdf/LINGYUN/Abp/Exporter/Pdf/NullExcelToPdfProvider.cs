using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Exporter.Pdf;

[Dependency(TryRegister = true)]
public class NullExcelToPdfProvider : IExcelToPdfProvider, ISingletonDependency
{
    private readonly static Stream _nullStreamCache = Stream.Null;
    public virtual Task<Stream> ParseAsync(Stream excelStream, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_nullStreamCache);
    }
}
