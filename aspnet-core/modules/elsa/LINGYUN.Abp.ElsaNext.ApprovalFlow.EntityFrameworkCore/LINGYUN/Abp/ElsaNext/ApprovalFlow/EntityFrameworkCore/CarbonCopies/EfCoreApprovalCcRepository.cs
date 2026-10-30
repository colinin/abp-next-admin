using LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies;
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

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.CarbonCopies;

public class EfCoreApprovalCcRepository : EfCoreRepository<IElsaNextApprovalFlowDbContext, ApprovalCc, Guid>, IApprovalCcRepository
{
    public EfCoreApprovalCcRepository(IDbContextProvider<IElsaNextApprovalFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<ApprovalCc> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<ApprovalCc>> GetListAsync(
        ISpecification<ApprovalCc> specification,
        string? sorting = nameof(ApprovalCc.NotifiedAt),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : nameof(ApprovalCc.NotifiedAt))
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<ApprovalCc>> GetListByInstanceAsync(
        Guid instanceId, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.InstanceId == instanceId)
            .OrderBy(x => x.NotifiedAt)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
