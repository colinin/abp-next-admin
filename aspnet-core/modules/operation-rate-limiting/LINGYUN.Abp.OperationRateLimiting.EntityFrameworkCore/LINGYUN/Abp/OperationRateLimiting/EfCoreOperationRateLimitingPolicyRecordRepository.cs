using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.OperationRateLimiting;

public class EfCoreOperationRateLimitingPolicyRecordRepository :
    EfCoreRepository<IOperationRateLimitingDbContext, OperationRateLimitingPolicyRecord, Guid>,
    IOperationRateLimitingPolicyRecordRepository
{
    public EfCoreOperationRateLimitingPolicyRecordRepository(
        IDbContextProvider<IOperationRateLimitingDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async override Task<IQueryable<OperationRateLimitingPolicyRecord>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).Include(policy => policy.Rules);
    }

    public async virtual Task<OperationRateLimitingPolicyRecord?> FindByNameAsync(
        string name,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await FindAsync(
            policy => policy.Name == name,
            includeDetails,
            cancellationToken);
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<OperationRateLimitingPolicyRecord> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<OperationRateLimitingPolicyRecord>> GetListAsync(
        ISpecification<OperationRateLimitingPolicyRecord> specification,
        string? sorting = $"{nameof(OperationRateLimitingPolicyRecord.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Include(policy => policy.Rules)
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : $"{nameof(OperationRateLimitingPolicyRecord.CreationTime)} DESC")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
