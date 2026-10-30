using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
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

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.Instances;

public class EfCoreApprovalInstanceRepository : EfCoreRepository<IElsaNextApprovalFlowDbContext, ApprovalInstance, Guid>, IApprovalInstanceRepository
{
    public EfCoreApprovalInstanceRepository(IDbContextProvider<IElsaNextApprovalFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<ApprovalInstance> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<ApprovalInstance>> GetListAsync(
        ISpecification<ApprovalInstance> specification,
        string? sorting = $"{nameof(ApprovalInstance.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : $"{nameof(ApprovalInstance.CreationTime)} DESC")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<ApprovalInstance?> FindByElsaWorkflowInstanceIdAsync(
        string elsaWorkflowInstanceId, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.ElsaWorkflowInstanceId == elsaWorkflowInstanceId)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}
