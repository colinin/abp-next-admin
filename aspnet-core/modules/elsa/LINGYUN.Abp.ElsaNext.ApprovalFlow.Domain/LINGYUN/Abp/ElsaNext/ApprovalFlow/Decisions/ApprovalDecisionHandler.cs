using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Contracts;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Delegations;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Decisions;

public class ApprovalDecisionHandler : DomainService, IApprovalDecisionHandler
{
	private const string TerminalConflictCode = "terminal";

	private const string RevisionConflictCode = "revision-conflict";

	private const string TransitionInProgressConflictCode = "transition-in-progress";

	protected IApprovalInstanceRepository InstanceRepository { get; }

	protected IApprovalNodeTaskRepository NodeTaskRepository { get; }

	protected IApprovalRecordRepository RecordRepository { get; }

	protected IUserTaskManager UserTaskManager { get; }

	protected IUserTaskRepository UserTaskRepository { get; }

	protected IApprovalDelegationResolver DelegationResolver { get; }

	protected IUserTaskProjectionService ProjectionService { get; }

	protected ILogger<ApprovalDecisionHandler> Logger { get; }

	public ApprovalDecisionHandler(
        IApprovalInstanceRepository instanceRepository, 
        IApprovalNodeTaskRepository nodeTaskRepository, 
        IApprovalRecordRepository recordRepository,
        IUserTaskManager userTaskManager, 
        IUserTaskRepository userTaskRepository, 
        IApprovalDelegationResolver delegationResolver, 
        IUserTaskProjectionService projectionService,
		ILogger<ApprovalDecisionHandler> logger)
	{
		InstanceRepository = instanceRepository;
		NodeTaskRepository = nodeTaskRepository;
		RecordRepository = recordRepository;
		UserTaskManager = userTaskManager;
		UserTaskRepository = userTaskRepository;
		DelegationResolver = delegationResolver;
		ProjectionService = projectionService;
		Logger = logger;
	}

	public virtual async Task<ApprovalDecisionResult> DecideAsync(ApprovalDecisionRequest request, CancellationToken cancellationToken = default)
	{
		Check.NotNull(request, nameof(request));
		var instance = await InstanceRepository.GetAsync(request.InstanceId, true, cancellationToken);
		if (instance.IsFinished())
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.AlreadyFinished);
		}
		var nodeTask = await NodeTaskRepository.FindByTaskIdAsync(request.TaskId, cancellationToken);
		if (nodeTask == null || !nodeTask.IsOpen())
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NoPendingTask);
		}
		var tenantId = ApprovalInstanceGuard.ToUserTaskTenantId(instance);
		var userTask = await UserTaskManager.GetAsync(tenantId, request.TaskId, request.Actor, cancellationToken);
		if (userTask == null)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NoPendingTask);
		}
		// Candidate tasks are not assigned yet; claim before completing so the User Tasks access policy
		// (which requires an assignee for completion) accepts the decision.
		if (string.Equals(userTask.Status, nameof(UserTaskStatus.Available), StringComparison.OrdinalIgnoreCase) && userTask.Assignee == null)
		{
			var claimResult = await UserTaskManager.ClaimAsync(
				tenantId,
				request.TaskId,
				new UserTaskMutationRequest(userTask.Revision, request.OperationId + "-claim"),
				request.Actor,
				cancellationToken);
			if (!claimResult.Accepted)
			{
				throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NoPendingTask)
					
					;
			}

			userTask = await UserTaskManager.GetAsync(tenantId, request.TaskId, request.Actor, cancellationToken) ?? userTask;
		}
		// Mark the approval node task decided before completing the User Task: completion resumes the
		// workflow and a finishing workflow cancels any node task that is still open.
		var decidedAt = new DateTimeOffset(Clock.Now);
		nodeTask.MarkDecided(decidedAt);
		await NodeTaskRepository.UpdateAsync(nodeTask, true, cancellationToken);

		// A rejection is final by policy. Finish the instance before resuming the workflow so the
		// workflow-finished notification cannot race the decision record and default to Approved.
		if (request.Effect == ApprovalActionEffect.Reject)
		{
			instance.ApplyOutcome(ApprovalOutcome.Rejected, request.ActionKey, decidedAt);
			await InstanceRepository.UpdateAsync(instance, autoSave: true, cancellationToken);
		}

		// Re-read immediately before completion so the revision reflects any concurrent
		// claim/update made while the detail page was open.
		userTask = await UserTaskManager.GetAsync(tenantId, request.TaskId, request.Actor, cancellationToken) ?? userTask;

		var result = await UserTaskManager.CompleteAsync(
            tenantId, 
            request.TaskId, 
            new UserTaskCompletionRequest(
                userTask.Revision,
                request.OperationId,
                request.ActionKey, 
                ParseData(request.DataJson)), 
            request.Actor, 
            cancellationToken);
		if (!result.Accepted)
		{
			Logger.LogWarning(
				"Approval user task completion rejected. TaskId={TaskId}, ActionKey={ActionKey}, Revision={Revision}, ConflictCode={ConflictCode}",
				request.TaskId,
				request.ActionKey,
				userTask.Revision,
				result.ConflictCode);

			if (IsAlreadyDecided(result.ConflictCode) &&
                await IsDelegatedTaskAsync(tenantId, request.TaskId, cancellationToken))
			{
				return new ApprovalDecisionResult(null, Ignored: true);
			}
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NoPendingTask);
		}
		await FinalizeAddedNodeAsync(nodeTask, tenantId, request.TaskId, cancellationToken);
		var action = Enum.TryParse<ApprovalRecordAction>(request.ActionKey, true, out var parsedAction)
			? parsedAction
			: ApprovalRecordAction.Approve;
		var record = new ApprovalRecord(
            GuidGenerator.Create(), 
            instance.Id,
            action, 
            request.Effect, 
            decidedAt,
            request.Actor.Subject,
            nodeTask.NodeId, 
            nodeTask.NodeName, 
            nodeTask.NodeInstanceId, 
            request.TaskId, 
            request.Comment, 
            request.DataJson);

		return new ApprovalDecisionResult(await RecordRepository.InsertAsync(record, true, cancellationToken), Ignored: false);
	}

	private static bool IsAlreadyDecided(string? conflictCode)
	{
		return string.Equals(conflictCode, TerminalConflictCode, StringComparison.Ordinal) || 
            string.Equals(conflictCode, RevisionConflictCode, StringComparison.Ordinal) || 
            string.Equals(conflictCode, TransitionInProgressConflictCode, StringComparison.Ordinal);
	}

	protected virtual async Task<bool> IsDelegatedTaskAsync(string tenantId, string taskId, CancellationToken cancellationToken = default)
	{
		var task = await UserTaskRepository.GetAsync(tenantId, taskId, cancellationToken);
		if (task?.Assignee != null)
		{
			return await DelegationResolver.HasActiveDelegateAsync(task.Assignee, cancellationToken);
		}
		return false;
	}

	protected virtual async Task FinalizeAddedNodeAsync(ApprovalNodeTask nodeTask, string tenantId, string taskId, CancellationToken cancellationToken = default)
	{
		if (nodeTask.Origin == ApprovalNodeTaskOrigin.Added)
		{
			var task = await UserTaskRepository.GetAsync(tenantId, taskId, cancellationToken);
			if (task != null)
			{
				await ProjectionService.FinalizeBookmarkRemovalAsync(new UserTaskBookmarkRemoval(tenantId, taskId, task.BookmarkId, new DateTimeOffset(Clock.Now)), cancellationToken);
			}
		}
	}

	private static JsonElement? ParseData(string? dataJson)
	{
		if (string.IsNullOrWhiteSpace(dataJson))
		{
			return null;
		}
		try
		{
			using var jsonDocument = JsonDocument.Parse(dataJson);
			return jsonDocument.RootElement.Clone();
		}
		catch (JsonException)
		{
			return null;
		}
	}
}
