using Volo.Abp;
using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;

public class ApprovalRecord : CreationAuditedEntity<Guid>, IMultiTenant
{
	public Guid? TenantId { get; private set; }

	public Guid InstanceId { get; private set; }

	public string? NodeId { get; private set; }

	public string? NodeName { get; private set; }

	public string? NodeInstanceId { get; private set; }

	public string? TaskId { get; private set; }

	public string? ActorProvider { get; private set; }

	public string? ActorType { get; private set; }

	public string? ActorId { get; private set; }

	public string? ActorDisplayName { get; private set; }

	public string ActionKey { get; private set; }

	public string? ActionLabel { get; private set; }

	public ApprovalActionEffect Effect { get; private set; }

	public string? Comment { get; private set; }

	public string? DataJson { get; private set; }

	public DateTimeOffset OccurredAt { get; private set; }

	private ApprovalRecord()
	{
		ActionKey = "";
	}

	public ApprovalRecord(
        Guid id,
        Guid instanceId,
        string actionKey,
        ApprovalActionEffect effect,
        DateTimeOffset occurredAt,
        ParticipantReference? actor = null,
        string? actionLabel = null,
        string? nodeId = null,
        string? nodeName = null, 
        string? nodeInstanceId = null,
        string? taskId = null, 
        string? comment = null, 
        string? dataJson = null)
		: base(id)
	{
		InstanceId = Check.NotDefaultOrNull<Guid>(instanceId, nameof(instanceId));
		ActionKey = Check.NotNullOrWhiteSpace(actionKey, nameof(actionKey), ApprovalRecordConsts.MaxActionKeyLength);
		Effect = effect;
		OccurredAt = occurredAt;
		ActionLabel = actionLabel;
		NodeId = nodeId;
		NodeName = nodeName;
		NodeInstanceId = nodeInstanceId;
		TaskId = taskId;
		Comment = Check.Length(comment, nameof(comment), ApprovalRecordConsts.MaxCommentLength);
		DataJson = dataJson;
		SetActor(actor);
	}

	public ApprovalRecord(
        Guid id,
        Guid instanceId,
        ApprovalRecordAction action,
        ApprovalActionEffect effect,
        DateTimeOffset occurredAt,
        ParticipantReference? actor = null,
        string? nodeId = null,
        string? nodeName = null,
        string? nodeInstanceId = null,
        string? taskId = null,
        string? comment = null,
        string? dataJson = null)
		: this(
            id,
            instanceId,
            action.ToString(),
            effect,
            occurredAt,
            actor,
            actionLabel: null,
            nodeId,
            nodeName,
            nodeInstanceId,
            taskId,
            comment,
            dataJson)
	{
	}
	public ParticipantReference? GetActor()
	{
		return ParticipantColumns.ToReference("", ActorProvider, ActorType, ActorId, ActorDisplayName);
	}

	private void SetActor(ParticipantReference? actor)
	{
		ActorProvider = actor?.Provider;
		ActorType = actor?.Type.ToString();
		ActorId = actor?.Id;
		ActorDisplayName = actor?.DisplayName;
	}
}
