using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Gdpr;

public interface IGdprRequestRepository : IBasicRepository<GdprRequest, Guid>
{
    Task<DateTime?> FindLatestRequestTimeAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<GdprRequest> specification,
        CancellationToken cancellationToken = default);

    Task<List<GdprRequest>> GetListAsync(
        ISpecification<GdprRequest> specification,
        string? sorting = $"{nameof(GdprRequest.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
