using Elsa.ApprovalFlow.Models;
using Elsa.Mediator.Contracts;
using Elsa.Workflows;
using Elsa.Workflows.Notifications;
using Elsa.Workflows.State;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Handlers;

/// <summary>
/// Closes the approval instance once the Elsa workflow that drives it has finished.
/// </summary>
/// <remarks>
/// The instance status belongs to the workflow, not to the last task: a rejection may end the process early and an
/// approval only ends it after the final node. Workflows can therefore publish the outcome through the
/// "ApprovalOutcome" output; when they do not, a successfully finished workflow is treated as approved.
/// </remarks>
public class WorkflowFinishedHandler : DomainService, INotificationHandler<WorkflowFinished>, INotificationHandler
{
    public const string OutcomeOutputKey = ElsaNextApprovalFlowConsts.OutcomeOutputKey;

    protected IApprovalInstanceRepository InstanceRepository { get; }

    protected IApprovalNodeTaskRepository NodeTaskRepository { get; }

    protected IApprovalRecordRepository RecordRepository { get; }

    public WorkflowFinishedHandler(
        IApprovalInstanceRepository instanceRepository,
        IApprovalNodeTaskRepository nodeTaskRepository,
        IApprovalRecordRepository recordRepository)
    {
        InstanceRepository = instanceRepository;
        NodeTaskRepository = nodeTaskRepository;
        RecordRepository = recordRepository;
    }

    public virtual async Task HandleAsync(WorkflowFinished notification, CancellationToken cancellationToken)
    {
        var state = notification.WorkflowState;
        if (state == null || string.IsNullOrWhiteSpace(state.Id))
        {
            return;
        }

        var instance = await InstanceRepository.FindByElsaWorkflowInstanceIdAsync(state.Id, cancellationToken);
        if (instance == null || instance.IsFinished())
        {
            return;
        }

        var status = await ResolveStatusAsync(state, instance.Id, cancellationToken);
        if (!status.HasValue)
        {
            return;
        }

        var now = new DateTimeOffset(Clock.Now);
        var openNodeTasks = (await NodeTaskRepository.GetListByInstanceAsync(instance.Id, cancellationToken))
            .Where(x => x.IsOpen())
            .ToList();

        foreach (var nodeTask in openNodeTasks)
        {
            nodeTask.MarkCancelled();
            await NodeTaskRepository.UpdateAsync(nodeTask, autoSave: true, cancellationToken: cancellationToken);
        }

        instance.Complete(status.Value, status.Value.ToString(), now);
        await InstanceRepository.UpdateAsync(instance, autoSave: true, cancellationToken: cancellationToken);

        await RecordRepository.InsertAsync(
            new ApprovalRecord(
                GuidGenerator.Create(),
                instance.Id,
                ApprovalRecordAction.End,
                ApprovalActionEffect.Neutral,
                now),
            autoSave: true,
            cancellationToken: cancellationToken);
    }

    protected virtual async Task<ApprovalInstanceStatus?> ResolveStatusAsync(WorkflowState state, Guid instanceId, CancellationToken cancellationToken)
    {
        if (state.SubStatus == WorkflowSubStatus.Cancelled)
        {
            return ApprovalInstanceStatus.Cancelled;
        }

        if (state.SubStatus != WorkflowSubStatus.Finished)
        {
            // Faulted workflows keep the instance running so an operator can inspect and withdraw it.
            return null;
        }

        return ReadOutcome(state)
            ?? await ReadOutcomeFromRecordsAsync(instanceId, cancellationToken)
            ?? ApprovalInstanceStatus.Approved;
    }

    protected virtual async Task<ApprovalInstanceStatus?> ReadOutcomeFromRecordsAsync(Guid instanceId, CancellationToken cancellationToken)
    {
        // Older templates do not publish ApprovalOutcome. In that case the approval decision
        // records are the source of truth: the last non-neutral decision decides the instance.
        var records = await RecordRepository.GetTimelineAsync(instanceId, cancellationToken);
        var lastDecision = records.LastOrDefault(x => x.Effect != ApprovalActionEffect.Neutral);
        return lastDecision?.Effect == ApprovalActionEffect.Reject
            ? ApprovalInstanceStatus.Rejected
            : null;
    }

    protected virtual ApprovalInstanceStatus? ReadOutcome(WorkflowState state)
    {
        if (!state.Output.TryGetValue(OutcomeOutputKey, out var value) || value == null)
        {
            return null;
        }

        var text = value.ToString();
        if (Enum.TryParse<ApprovalInstanceStatus>(text, ignoreCase: true, out var status) &&
            status != ApprovalInstanceStatus.Running)
        {
            return status;
        }

        if (!Enum.TryParse<ApprovalOutcome>(text, ignoreCase: true, out var outcome))
        {
            return null;
        }

        return outcome switch
        {
            ApprovalOutcome.Approved => ApprovalInstanceStatus.Approved,
            ApprovalOutcome.Rejected => ApprovalInstanceStatus.Rejected,
            ApprovalOutcome.Returned => ApprovalInstanceStatus.Returned,
            ApprovalOutcome.Withdrawn => ApprovalInstanceStatus.Withdrawn,
            ApprovalOutcome.Cancelled => ApprovalInstanceStatus.Cancelled,
            _ => null,
        };
    }
}
