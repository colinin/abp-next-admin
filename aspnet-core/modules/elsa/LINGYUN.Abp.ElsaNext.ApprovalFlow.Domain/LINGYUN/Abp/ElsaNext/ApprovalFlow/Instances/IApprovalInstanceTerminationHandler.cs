using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public interface IApprovalInstanceTerminationHandler
{
    Task<ApprovalRecord> WithdrawAsync(Guid instanceId, UserTaskActor actor, string? comment = null, CancellationToken cancellationToken = default);

    Task<ApprovalRecord> ReturnAsync(Guid instanceId, UserTaskActor actor, string? comment = null, CancellationToken cancellationToken = default);
}
