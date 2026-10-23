using Volo.Abp;
using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public class ApprovalInstance : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
	public Guid? TenantId { get; private set; }

	public Guid TemplateId { get; private set; }

	public string TemplateName { get; private set; }

	public string? TemplateCategory { get; private set; }

	public string? BusinessKey { get; private set; }

	public string Title { get; private set; }

	public string? InitiatorProvider { get; private set; }

	public string? InitiatorType { get; private set; }

	public string? InitiatorId { get; private set; }

	public string? InitiatorDisplayName { get; private set; }

	public string? ElsaWorkflowInstanceId { get; private set; }

	public ApprovalInstanceStatus Status { get; private set; }

	public string? CurrentNodeId { get; private set; }

	public string? CurrentNodeName { get; private set; }

	public string? FormDataJson { get; private set; }

	public DateTimeOffset StartedAt { get; private set; }

	public DateTimeOffset? FinishedAt { get; private set; }

	public string? Outcome { get; private set; }

	private ApprovalInstance()
	{
		TemplateName = "";
		Title = "";
	}

	public ApprovalInstance(
        Guid id, 
        Guid templateId, 
        string templateName,
        string title, 
        ParticipantReference? initiator, 
        string? templateCategory = null,
        string? businessKey = null, 
        string? formDataJson = null,
        DateTimeOffset? startedAt = null)
		: base(id)
	{
		TemplateId = Check.NotDefaultOrNull<Guid>(templateId, nameof(templateId));
		TemplateName = Check.NotNullOrWhiteSpace(templateName, nameof(templateName), ApprovalInstanceConsts.MaxTemplateNameLength);
		Title = Check.NotNullOrWhiteSpace(title, nameof(title), ApprovalInstanceConsts.MaxTitleLength);
		TemplateCategory = Check.Length(templateCategory, nameof(templateCategory), ApprovalInstanceConsts.MaxTemplateCategoryLength);
		BusinessKey = Check.Length(businessKey, nameof(businessKey), ApprovalInstanceConsts.MaxBusinessKeyLength);
		FormDataJson = formDataJson;
		Status = ApprovalInstanceStatus.Running;
		StartedAt = startedAt ?? throw new ArgumentNullException(nameof(startedAt));
		SetInitiator(initiator);
	}

	public ParticipantReference? GetInitiator()
	{
		return ParticipantColumns.ToReference("", InitiatorProvider, InitiatorType, InitiatorId, InitiatorDisplayName);
	}

	public bool IsFinished()
	{
		return Status != ApprovalInstanceStatus.Running;
	}

	public bool IsInitiatedBy(ParticipantReference participant)
	{
		return ParticipantColumns.Matches(InitiatorProvider, InitiatorType, InitiatorId, participant);
	}

	public void SetElsaWorkflowInstanceId(string elsaWorkflowInstanceId)
	{

		ElsaWorkflowInstanceId = Check.NotNullOrWhiteSpace(elsaWorkflowInstanceId, nameof(elsaWorkflowInstanceId), ApprovalInstanceConsts.MaxElsaWorkflowInstanceIdLength);
	}

	public void ApplyOutcome(ApprovalOutcome outcome, string? actionKey, DateTimeOffset finishedAt)
	{
		var approvalInstanceStatus = outcome switch
		{
			ApprovalOutcome.Approved => (ApprovalInstanceStatus?)ApprovalInstanceStatus.Approved, 
			ApprovalOutcome.Rejected => ApprovalInstanceStatus.Rejected, 
			ApprovalOutcome.Returned => ApprovalInstanceStatus.Returned, 
			ApprovalOutcome.Withdrawn => ApprovalInstanceStatus.Withdrawn, 
			ApprovalOutcome.Cancelled => ApprovalInstanceStatus.Cancelled, 
			_ => null, 
		};
		var approvalInstanceStatus2 = approvalInstanceStatus;
		if (!approvalInstanceStatus2.HasValue)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.OutcomeNotAllowed);
		}
		Complete(approvalInstanceStatus2.Value, actionKey, finishedAt);
	}

	public void SetCurrentNode(string? nodeId, string? nodeName)
	{
		CurrentNodeId = nodeId;
		CurrentNodeName = nodeName;
	}

	public void Complete(ApprovalInstanceStatus status, string? outcome, DateTimeOffset finishedAt)
	{
		if (status == ApprovalInstanceStatus.Running)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.StatusCannotBeRunning);
		}
		if (!IsFinished())
		{
			Status = status;
			Outcome = outcome;
			FinishedAt = finishedAt;
			CurrentNodeId = null;
			CurrentNodeName = null;
		}
	}

	private void SetInitiator(ParticipantReference? initiator)
	{
		InitiatorProvider = initiator?.Provider;
		InitiatorType = initiator?.Type.ToString();
		InitiatorId = initiator?.Id;
		InitiatorDisplayName = initiator?.DisplayName;
	}

}
