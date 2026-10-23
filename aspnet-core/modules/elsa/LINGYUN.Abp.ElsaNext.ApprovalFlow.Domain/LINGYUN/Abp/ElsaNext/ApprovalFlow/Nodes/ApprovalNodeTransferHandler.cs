using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Contracts;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;

public class ApprovalNodeTransferHandler : DomainService, IApprovalNodeTransferHandler
{
	protected IApprovalInstanceGuard InstanceGuard { get; }

	protected IApprovalNodeTaskRepository NodeTaskRepository { get; }

	protected IApprovalRecordRepository RecordRepository { get; }

	protected IUserTaskManager UserTaskManager { get; }

	public ApprovalNodeTransferHandler(IApprovalInstanceGuard instanceGuard, IApprovalNodeTaskRepository nodeTaskRepository, IApprovalRecordRepository recordRepository, IUserTaskManager userTaskManager)
	{
		InstanceGuard = instanceGuard;
		NodeTaskRepository = nodeTaskRepository;
		RecordRepository = recordRepository;
		UserTaskManager = userTaskManager;
	}

	public virtual async Task<ApprovalRecord> TransferAsync(ApprovalNodeTransferRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		Check.NotNull(request, nameof(request));
		Check.NotNull(request.Actor, nameof(request.Actor));
		if (request.Transferee == null || string.IsNullOrWhiteSpace(request.Transferee.Id))
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.TransfereeRequired);
		}
		ApprovalInstance instance = await InstanceGuard.GetRunningInstanceAsync(request.InstanceId, cancellationToken);
		string tenantId = ApprovalInstanceGuard.ToUserTaskTenantId(instance);
		ApprovalNodeTask nodeTask = await ResolveNodeTaskAsync(instance, request, cancellationToken);
		UserTaskDetail userTask = await UserTaskManager.GetAsync(tenantId, nodeTask.TaskId, request.Actor, cancellationToken);
		if (userTask == null || !nodeTask.IsOpen())
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NoPendingTask);
		}
		ParticipantReference transferee = request.Transferee;
		if (!(await UserTaskManager.AssignAsync(tenantId, nodeTask.TaskId, new UserTaskAssignRequest(userTask.Revision, transferee, request.Comment, GuidGenerator.Create().ToString("N")), request.Actor, cancellationToken)).Accepted)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.TransferRejected);
		}
		DateTimeOffset now = new DateTimeOffset(Clock.Now);
		nodeTask.TransferTo(transferee);
		await NodeTaskRepository.UpdateAsync(nodeTask, autoSave: true, cancellationToken);
		ApprovalRecord record = new ApprovalRecord(GuidGenerator.Create(), instance.Id, ApprovalRecordAction.Transfer, ApprovalActionEffect.Neutral, now, request.Actor.Subject, null, nodeTask.NodeId, nodeTask.NodeName, nodeTask.NodeInstanceId, nodeTask.TaskId, request.Comment);
		return await RecordRepository.InsertAsync(record, autoSave: true, cancellationToken);
	}

	protected virtual async Task<ApprovalNodeTask> ResolveNodeTaskAsync(ApprovalInstance instance, ApprovalNodeTransferRequest request, CancellationToken cancellationToken)
	{
		List<ApprovalPendingTask> pending = await InstanceGuard.GetPendingTasksAsync(instance, request.Actor, cancellationToken);
		if (!string.IsNullOrWhiteSpace(request.TaskId))
		{
			ApprovalPendingTask matched = pending.FirstOrDefault((ApprovalPendingTask x) => string.Equals(x.NodeTask.TaskId, request.TaskId, StringComparison.Ordinal));
			if (matched != null)
			{
				return matched.NodeTask;
			}
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NoPendingTask);
		}
		return pending.Count switch
		{
			1 => pending[0].NodeTask, 
			0 => throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NoPendingTask), 
			_ => throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.AmbiguousTask), 
		};
	}
}
