using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Exporter;

[Dependency(TryRegister = true)]
[Obsolete("This interface will be deprecated in future versions. Please use IExcelImporterProvider instead.")]
public class NullImporterProvider : IImporterProvider, ISingletonDependency
{
    public Task<IReadOnlyCollection<T>> ImportAsync<T>(Stream stream) where T : class, new()
    {
        IReadOnlyCollection<T> empty = Array.Empty<T>();

        return Task.FromResult(empty);
    }
}
