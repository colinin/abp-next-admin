using Elsa.ApprovalFlow.Activities;
using Elsa.ApprovalFlow;
using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Contracts;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;

public class ApprovalNodeAdditionHandler : DomainService, IApprovalNodeAdditionHandler
{
	public const string AddedNodeBookmarkPrefix = "approval-added:";

	protected IApprovalInstanceGuard InstanceGuard { get; }

	protected IApprovalNodeTaskRepository NodeTaskRepository { get; }

	protected IApprovalRecordRepository RecordRepository { get; }

	protected IUserTaskManager UserTaskManager { get; }

	public ApprovalNodeAdditionHandler(
        IApprovalInstanceGuard instanceGuard, 
        IApprovalNodeTaskRepository nodeTaskRepository, 
        IApprovalRecordRepository recordRepository,
        IUserTaskManager userTaskManager)
	{
		InstanceGuard = instanceGuard;
		NodeTaskRepository = nodeTaskRepository;
		RecordRepository = recordRepository;
		UserTaskManager = userTaskManager;
	}

	public virtual async Task<ApprovalRecord> AddNodeAsync(ApprovalNodeAdditionRequest request, CancellationToken cancellationToken = default)
	{
		Check.NotNull(request, nameof(request));
		Check.NotNull(request.Actor, nameof(request.Actor));
		if (request.Approver == null || string.IsNullOrWhiteSpace(request.Approver.Id))
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.ApproverRequired);
		}
		var instance = await InstanceGuard.GetRunningInstanceAsync(request.InstanceId, cancellationToken);
		var source = await GetSourceTaskAsync(instance, request, cancellationToken);
		var now = new DateTimeOffset(Clock.Now);
		var nodeInstanceId = GuidGenerator.Create().ToString("N");
		var taskId = GuidGenerator.Create().ToString("N");
		var title = (string.IsNullOrWhiteSpace(request.Title) ? source.Task.Title : request.Title);
		var approver = request.Approver;
		var actions = ReadActionDefinitions(source.Task);
		await UserTaskManager.ProjectAsync(
            new UserTaskMaterialization(
                ApprovalInstanceGuard.ToUserTaskTenantId(instance), 
                source.Task.WorkflowDefinitionId ?? "", 
                source.Task.WorkflowInstanceId ?? "", 
                GuidGenerator.Create().ToString("N"),
                AddedNodeBookmarkPrefix + nodeInstanceId, 
                new UserTaskDefinitionSnapshot
		        {
			        Title = title,
			        Summary = source.Task.Summary,
			        Reference = instance.Id.ToString("D"),
			        Tags = BuildTags(nodeInstanceId, actions),
			        TaskType = ApprovalTask.TaskTypeMarker,
			        Assignee = approver,
			        Priority = source.Task.Priority,
			        DueAt = request.DueAt,
			        Instructions = source.Task.Instructions,
			        TaskData = source.Task.Data,
			        Actions = actions.Select(x => new UserTaskAction(x.Key.ToString(), x.Label)).ToArray(),
			        EnableTimeoutOutcome = false,
			        EnableCancellationOutcome = false
		        }, 
                Array.Empty<ParticipantReference>(), 
                Array.Empty<ParticipantReference>(), 
                now, 
                taskId, 
                source.Task.WorkflowDefinitionName, 
                source.Task.WorkflowDefinitionVersion, 
                source.Task.WorkflowInstanceReference),
            cancellationToken);

		var record = new ApprovalRecord(
            GuidGenerator.Create(), 
            instance.Id, 
            ApprovalRecordAction.AddSign, 
            ApprovalActionEffect.Neutral,
            now, 
            request.Actor.Subject, 
            null,
            null, 
            null, 
            nodeInstanceId,
            taskId, 
            request.Comment);

		return await RecordRepository.InsertAsync(record, true, cancellationToken);
	}

	public virtual async Task<ApprovalRecord> RemoveNodeAsync(ApprovalNodeRemovalRequest request, CancellationToken cancellationToken = default)
	{
		Check.NotNull(request, nameof(request));
		Check.NotNull(request.Actor, nameof(request.Actor));
		var instance = await InstanceGuard.GetRunningInstanceAsync(request.InstanceId, cancellationToken);
		if (!(await InstanceGuard.IsPendingApproverAsync(instance, request.Actor, cancellationToken)))
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NoPendingTask);
		}
		var nodeTask = await FindNodeTaskAsync(instance, request.NodeInstanceId, cancellationToken);
		if (nodeTask.Origin != ApprovalNodeTaskOrigin.Added)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NodeNotAdded);
		}
		if (!nodeTask.IsPending())
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NodeNotPending);
		}
		var now = new DateTimeOffset(Clock.Now);
		nodeTask.MarkCancelled();
		await NodeTaskRepository.UpdateAsync(nodeTask, true, cancellationToken);
		var record = new ApprovalRecord(
            GuidGenerator.Create(),
            instance.Id, 
            ApprovalRecordAction.RemoveSign, 
            ApprovalActionEffect.Neutral, 
            now, 
            request.Actor.Subject, 
            null, 
            nodeTask.NodeId, 
            nodeTask.NodeName,
            nodeTask.NodeInstanceId,
            nodeTask.TaskId, 
            request.Comment);

		return await RecordRepository.InsertAsync(record, true, cancellationToken);
	}

	protected virtual async Task<ApprovalPendingTask> GetSourceTaskAsync(ApprovalInstance instance, ApprovalNodeAdditionRequest request, CancellationToken cancellationToken)
	{
		var pending = await InstanceGuard.GetPendingTasksAsync(instance, request.Actor, cancellationToken);
		if (!string.IsNullOrWhiteSpace(request.TaskId))
		{
			var matched = pending.FirstOrDefault(x => string.Equals(x.NodeTask.TaskId, request.TaskId, StringComparison.Ordinal));
			if (matched != null)
			{
				return matched;
			}
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NoPendingTask);
		}
		var source = pending.FirstOrDefault();
		if (source == null)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NoPendingTask);
		}
		return source;
	}

	protected virtual async Task<ApprovalNodeTask> FindNodeTaskAsync(ApprovalInstance instance, string nodeInstanceId, CancellationToken cancellationToken)
	{
		var nodeTask = (await NodeTaskRepository.GetListByInstanceAsync(instance.Id, cancellationToken))
            .FirstOrDefault(x => string.Equals(x.NodeInstanceId, nodeInstanceId, StringComparison.Ordinal));
		if (nodeTask == null)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NodeNotFound);
		}
		return nodeTask;
	}

	protected virtual List<ApprovalActionDefinition> ReadActionDefinitions(UserTaskDetail source)
	{
		var effects = ApprovalTaskTags.ReadActionEffects(source.Tags);
		return source.Actions.Select(x => new ApprovalActionDefinition
		{
			Key = Enum.TryParse<ApprovalActionKey>(x.Key, true, out var key) ? key : ApprovalActionKey.Approve,
			Effect = (effects.TryGetValue(x.Key, out var value) ? value : ApprovalActionEffect.Neutral)
		}).ToList();
	}

	protected virtual IReadOnlyCollection<string> BuildTags(string nodeInstanceId, IReadOnlyCollection<ApprovalActionDefinition> actions)
	{
		return [
			ApprovalTask.TaskTypeMarker,
			ApprovalTaskTags.NodeInstance(nodeInstanceId),
			ApprovalTaskTags.SignMode(ApprovalSignMode.Any),
			ApprovalTaskTags.ActionEffects(actions),
			ApprovalTaskTags.Origin(ApprovalTaskTags.OriginAdded)
		];
	}
}
