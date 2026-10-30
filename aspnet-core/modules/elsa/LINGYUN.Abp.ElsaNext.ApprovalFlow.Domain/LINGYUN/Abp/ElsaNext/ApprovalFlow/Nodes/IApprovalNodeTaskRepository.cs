using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;

public interface IApprovalNodeTaskRepository : IBasicRepository<ApprovalNodeTask, Guid>
{
	Task<ApprovalNodeTask?> FindByTaskIdAsync(
        string taskId, 
        CancellationToken cancellationToken = default);

	Task<List<ApprovalNodeTask>> GetListByInstanceAsync(
        Guid instanceId, 
        CancellationToken cancellationToken = default);

    Task<List<ApprovalNodeTask>> GetPendingListByNodeInstanceAsync(
        string nodeInstanceId, 
        CancellationToken cancellationToken = default);
}
