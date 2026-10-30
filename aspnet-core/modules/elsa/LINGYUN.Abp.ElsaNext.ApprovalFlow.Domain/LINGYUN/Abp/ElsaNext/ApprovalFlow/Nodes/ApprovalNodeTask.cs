using Volo.Abp;
using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;

public class ApprovalNodeTask : CreationAuditedEntity<Guid>, IMultiTenant
{
	public Guid? TenantId { get; private set; }

	public Guid InstanceId { get; private set; }

	public string NodeInstanceId { get; private set; }

	public string? NodeId { get; private set; }

	public string? NodeName { get; private set; }

	public string TaskId { get; private set; }

	public string? ApproverProvider { get; private set; }

	public string? ApproverType { get; private set; }

	public string? ApproverId { get; private set; }

	public string? ApproverDisplayName { get; private set; }

	public ApprovalSignMode SignMode { get; private set; }

	public ApprovalNodeTaskStatus Status { get; private set; }

	public ApprovalNodeTaskOrigin Origin { get; private set; }

	public DateTimeOffset? FirstViewedAt { get; private set; }

	public DateTimeOffset? CompletedAt { get; private set; }

	private ApprovalNodeTask()
	{
		NodeInstanceId = "";
		TaskId = "";
	}

	public ApprovalNodeTask(
        Guid id, 
        Guid instanceId,
        string nodeInstanceId, 
        string taskId,
        ApprovalSignMode signMode,
        ParticipantReference? approver = null, 
        string? nodeId = null, 
        string? nodeName = null,
        ApprovalNodeTaskOrigin origin = ApprovalNodeTaskOrigin.Workflow)
		: base(id)
	{
		InstanceId = Check.NotDefaultOrNull<Guid>(instanceId, nameof(instanceId));
		NodeInstanceId = Check.NotNullOrWhiteSpace(nodeInstanceId, nameof(nodeInstanceId), ApprovalNodeTaskConsts.MaxNodeInstanceIdLength);
		TaskId = Check.NotNullOrWhiteSpace(taskId, nameof(taskId), ApprovalNodeTaskConsts.MaxTaskIdLength);
		SignMode = signMode;
		NodeId = nodeId;
		NodeName = nodeName;
		Status = ApprovalNodeTaskStatus.Pending;
		Origin = origin;
		SetApprover(approver);
	}

	public void MarkFirstViewed(DateTimeOffset at)
	{
		FirstViewedAt ??= at;
	}

	public void MarkDecided(DateTimeOffset at)
	{
		if (Status != ApprovalNodeTaskStatus.Cancelled)
		{
			Status = ApprovalNodeTaskStatus.Decided;
			CompletedAt = at;
		}
	}

	public void MarkCancelled()
	{
		Status = ApprovalNodeTaskStatus.Cancelled;
	}

	public bool IsPending()
	{
		return Status == ApprovalNodeTaskStatus.Pending;
	}

	public bool IsOpen()
	{
		var status = Status;
		if (status == ApprovalNodeTaskStatus.Pending || status == ApprovalNodeTaskStatus.Transferred)
		{
			return true;
		}
		return false;
	}

	public void TransferTo(ParticipantReference transferee)
	{
		if (Status == ApprovalNodeTaskStatus.Pending)
		{
			SetApprover(transferee);
			Status = ApprovalNodeTaskStatus.Transferred;
		}
	}

	public ParticipantReference? GetApprover()
	{
		return ParticipantColumns.ToReference("", ApproverProvider, ApproverType, ApproverId, ApproverDisplayName);
	}

	private void SetApprover(ParticipantReference? approver)
	{
		ApproverProvider = approver?.Provider;
		ApproverType = approver?.Type.ToString();
		ApproverId = approver?.Id;
		ApproverDisplayName = approver?.DisplayName;
	}
}
