using Elsa.UserTasks.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public interface IApprovalInstanceGuard
{
    Task<ApprovalInstance> GetRunningInstanceAsync(Guid instanceId, CancellationToken cancellationToken = default);

	Task<List<ApprovalPendingTask>> GetPendingTasksAsync(ApprovalInstance instance, UserTaskActor actor, CancellationToken cancellationToken = default);

    Task<bool> IsPendingApproverAsync(ApprovalInstance instance, UserTaskActor actor, CancellationToken cancellationToken = default);
}
