using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.BlobManagement;

public interface IBlobContainerRepository : IBasicRepository<BlobContainer, Guid>
{
    Task<string> GetNameAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<BlobContainer?> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<BlobContainer> specification,
        CancellationToken cancellationToken = default);

    Task<List<BlobContainer>> GetListAsync(
        ISpecification<BlobContainer> specification,
        string? sorting = nameof(BlobContainer.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
