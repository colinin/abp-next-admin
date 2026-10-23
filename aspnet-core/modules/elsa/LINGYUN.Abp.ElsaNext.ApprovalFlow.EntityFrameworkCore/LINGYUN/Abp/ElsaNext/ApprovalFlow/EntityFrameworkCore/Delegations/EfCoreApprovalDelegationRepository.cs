using LINGYUN.Abp.ElsaNext.ApprovalFlow.Delegations;
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

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.Delegations;

public class EfCoreApprovalDelegationRepository : EfCoreRepository<IElsaNextApprovalFlowDbContext, ApprovalDelegation, Guid>, IApprovalDelegationRepository
{
    public EfCoreApprovalDelegationRepository(IDbContextProvider<IElsaNextApprovalFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<List<ApprovalDelegation>> FindActiveListByDelegatorAsync(string delegatorId, DateTimeOffset now, CancellationToken cancellationToken = default)
    {
        var queryable = Active(await GetQueryableAsync(), now)
            .Where(x => x.DelegatorId == delegatorId)
            .OrderBy(x => x.CreationTime);

        return await queryable.ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<ApprovalDelegation>> FindActiveListByDelegateAsync(string delegateId, DateTimeOffset now, CancellationToken cancellationToken = default)
    {
        var queryable = Active(await GetQueryableAsync(), now)
            .Where(x => x.DelegateId == delegateId)
            .OrderBy(x => x.CreationTime);

        return await queryable.ToListAsync(GetCancellationToken(cancellationToken));
    }
    public async virtual Task<int> GetCountAsync(
        ISpecification<ApprovalDelegation> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<ApprovalDelegation>> GetListAsync(
        ISpecification<ApprovalDelegation> specification,
        string? sorting = $"{nameof(ApprovalDelegation.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : $"{nameof(ApprovalDelegation.CreationTime)} DESC")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    private static IQueryable<ApprovalDelegation> Active(IQueryable<ApprovalDelegation> queryable, DateTimeOffset now)
    {
        return queryable.Where(x => x.IsEnabled && x.EffectiveFrom <= now && (x.EffectiveTo == null || x.EffectiveTo > now));
    }
}
