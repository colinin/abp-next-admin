using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.Nodes;

public class EfCoreApprovalNodeTaskRepository : EfCoreRepository<IElsaNextApprovalFlowDbContext, ApprovalNodeTask, Guid>, IApprovalNodeTaskRepository
{
    public EfCoreApprovalNodeTaskRepository(IDbContextProvider<IElsaNextApprovalFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<ApprovalNodeTask?> FindByTaskIdAsync(string taskId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.TaskId == taskId)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<ApprovalNodeTask>> GetListByInstanceAsync(Guid instanceId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.InstanceId == instanceId)
            .OrderBy(x => x.CreationTime)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<ApprovalNodeTask>> GetPendingListByNodeInstanceAsync(string nodeInstanceId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.NodeInstanceId == nodeInstanceId &&
                        (x.Status == ApprovalNodeTaskStatus.Pending || x.Status == ApprovalNodeTaskStatus.Transferred))
            .OrderBy(x => x.CreationTime)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
