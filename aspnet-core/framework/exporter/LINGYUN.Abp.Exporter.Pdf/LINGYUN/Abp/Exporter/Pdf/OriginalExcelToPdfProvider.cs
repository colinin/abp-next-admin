using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Exporter.Pdf;

[Dependency(TryRegister = true)]
public class OriginalExcelToPdfProvider : IExcelToPdfProvider, ISingletonDependency
{
    public virtual Task<Stream> ParseAsync(Stream excelStream, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(excelStream);
    }
}
