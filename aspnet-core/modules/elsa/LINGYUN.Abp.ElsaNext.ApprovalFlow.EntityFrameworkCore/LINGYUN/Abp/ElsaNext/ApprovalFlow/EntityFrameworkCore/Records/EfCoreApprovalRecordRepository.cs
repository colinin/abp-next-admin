using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
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

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.Records;

public class EfCoreApprovalRecordRepository : EfCoreRepository<IElsaNextApprovalFlowDbContext, ApprovalRecord, Guid>, IApprovalRecordRepository
{
    public EfCoreApprovalRecordRepository(IDbContextProvider<IElsaNextApprovalFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<List<ApprovalRecord>> GetTimelineAsync(Guid instanceId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.InstanceId == instanceId)
            .OrderBy(x => x.OccurredAt)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<ApprovalRecord>> GetTimelineListAsync(IReadOnlyCollection<Guid> instanceIds, CancellationToken cancellationToken = default)
    {
        var idList = instanceIds.ToList();
        if (idList.Count == 0)
        {
            return new List<ApprovalRecord>();
        }

        return await (await GetQueryableAsync())
            .Where(x => idList.Contains(x.InstanceId))
            .OrderBy(x => x.InstanceId)
            .ThenBy(x => x.OccurredAt)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<bool> ExistsByActionAsync(Guid instanceId, string taskId, string actionKey, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).AnyAsync(
            x => x.InstanceId == instanceId && x.TaskId == taskId && x.ActionKey == actionKey,
            GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<ApprovalRecord> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<ApprovalRecord>> GetListAsync(
        ISpecification<ApprovalRecord> specification,
        string sorting = $"{nameof(ApprovalRecord.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : $"{nameof(ApprovalRecord.CreationTime)} DESC")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
