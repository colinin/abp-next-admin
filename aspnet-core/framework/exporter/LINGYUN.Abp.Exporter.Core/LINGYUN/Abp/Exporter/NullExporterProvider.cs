using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Exporter;

[Obsolete("This interface will be deprecated in future versions. Please use IExcelExporterProvider instead.")]
[Dependency(TryRegister = true)]
public class NullExporterProvider : IExporterProvider, ISingletonDependency
{
    private readonly static Stream _nullStreamCache = Stream.Null;
    public Task<Stream> ExportAsync<T>(ICollection<T> data, CancellationToken cancellationToken = default) where T : class, new()
    {
        return Task.FromResult(_nullStreamCache);
    }
}
