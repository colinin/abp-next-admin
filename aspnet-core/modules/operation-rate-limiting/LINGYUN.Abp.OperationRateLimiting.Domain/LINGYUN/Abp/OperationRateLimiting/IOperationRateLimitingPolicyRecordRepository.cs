using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.OperationRateLimiting;

public interface IOperationRateLimitingPolicyRecordRepository : IRepository<OperationRateLimitingPolicyRecord, Guid>
{
    Task<OperationRateLimitingPolicyRecord?> FindByNameAsync(
        string name,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<OperationRateLimitingPolicyRecord> specification,
        CancellationToken cancellationToken = default);

    Task<List<OperationRateLimitingPolicyRecord>> GetListAsync(
        ISpecification<OperationRateLimitingPolicyRecord> specification,
        string? sorting = $"{nameof(OperationRateLimitingPolicyRecord.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
