using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.LocalizationManagement;

public interface IResourceRepository : IRepository<Resource, Guid>
{
    Task<bool> ExistsAsync(
        string name,
        CancellationToken cancellationToken = default);

    Resource FindByName(string name);

    Task<Resource> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<List<Resource>> GetActivedListAsync(
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<Resource> specification,
        CancellationToken cancellationToken = default);

    Task<List<Resource>> GetListAsync(
        ISpecification<Resource> specification,
        string sorting = nameof(Resource.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
