using Elsa.UserTasks.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Decisions;

public interface IApprovalActionResolver
{
	Task<IReadOnlyList<ApprovalAvailableAction>> ResolveAsync(Guid instanceId, UserTaskActor actor, CancellationToken cancellationToken = default);
}
