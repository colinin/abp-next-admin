using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Platform.Packages;

public interface IPackageRepository : IBasicRepository<Package, Guid>
{
    Task<Package> FindByNameAsync(
        string name,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<Package> FindLatestAsync(
        string name,
        string version = null,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        Specification<Package> specification,
        CancellationToken cancellationToken = default);

    Task<List<Package>> GetListAsync(
        Specification<Package> specification,
        string sorting = $"{nameof(Package.Version)} DESC",
        int skipCount = 0,
        int maxResultCount = 10,
        CancellationToken cancellationToken = default);
}
