using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public class ApprovalInstanceTerminationHandler : DomainService, IApprovalInstanceTerminationHandler
{
	protected IApprovalInstanceGuard InstanceGuard { get; }

	protected IApprovalInstanceRepository InstanceRepository { get; }

	protected IApprovalNodeTaskRepository NodeTaskRepository { get; }

	protected IApprovalRecordRepository RecordRepository { get; }

	public ApprovalInstanceTerminationHandler(IApprovalInstanceGuard instanceGuard, IApprovalInstanceRepository instanceRepository, IApprovalNodeTaskRepository nodeTaskRepository, IApprovalRecordRepository recordRepository)
	{
		InstanceGuard = instanceGuard;
		InstanceRepository = instanceRepository;
		NodeTaskRepository = nodeTaskRepository;
		RecordRepository = recordRepository;
	}

	public virtual async Task<ApprovalRecord> WithdrawAsync(Guid instanceId, UserTaskActor actor, string? comment = null, CancellationToken cancellationToken = default)
	{
		Check.NotNull<UserTaskActor>(actor, nameof(actor));
		var instance = await InstanceGuard.GetRunningInstanceAsync(instanceId, cancellationToken);
		if (!instance.IsInitiatedBy(actor.Subject))
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NotInitiator, logLevel: LogLevel.Warning);
		}
		return await TerminateAsync(instance, ApprovalRecordAction.Withdraw, ApprovalOutcome.Withdrawn, actor.Subject, comment, cancellationToken);
	}

	public virtual async Task<ApprovalRecord> ReturnAsync(Guid instanceId, UserTaskActor actor, string? comment = null, CancellationToken cancellationToken = default)
	{
		Check.NotNull<UserTaskActor>(actor, nameof(actor));
		var instance = await InstanceGuard.GetRunningInstanceAsync(instanceId, cancellationToken);
		if (!(await InstanceGuard.IsPendingApproverAsync(instance, actor, cancellationToken)))
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NoPendingTask, logLevel: LogLevel.Warning);
		}
		return await TerminateAsync(instance, ApprovalRecordAction.Return, ApprovalOutcome.Returned, actor.Subject, comment, cancellationToken);
	}

	protected virtual async Task<ApprovalRecord> TerminateAsync(ApprovalInstance instance, ApprovalRecordAction action, ApprovalOutcome outcome, ParticipantReference actor, string? comment, CancellationToken cancellationToken = default)
	{
		var now = new DateTimeOffset(Clock.Now);
		foreach (var nodeTask in (await NodeTaskRepository.GetListByInstanceAsync(instance.Id, cancellationToken)).Where((ApprovalNodeTask x) => x.IsOpen()))
		{
			nodeTask.MarkCancelled();
			await NodeTaskRepository.UpdateAsync(nodeTask, true, cancellationToken);
		}
		instance.ApplyOutcome(outcome, action.ToString(), now);
		await InstanceRepository.UpdateAsync(instance, true, cancellationToken);
		var record = new ApprovalRecord(GuidGenerator.Create(), instance.Id, action, ApprovalActionEffect.Neutral, now, actor, null, null, null, null, null, comment);
		return await RecordRepository.InsertAsync(record, true, cancellationToken);
	}
}
