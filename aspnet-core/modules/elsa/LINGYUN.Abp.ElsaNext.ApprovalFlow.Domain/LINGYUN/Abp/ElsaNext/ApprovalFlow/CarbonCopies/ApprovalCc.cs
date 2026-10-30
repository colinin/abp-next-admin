using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies;

public class ApprovalCc : CreationAuditedEntity<Guid>, IMultiTenant
{
	public Guid? TenantId { get; private set; }

	public Guid? InstanceId { get; private set; }

	public string? InstanceReference { get; private set; }

	public string? NodeId { get; private set; }

	public string? NodeInstanceId { get; private set; }

	public string Title { get; private set; }

	public string? Summary { get; private set; }

	public string? RecipientProvider { get; private set; }

	public string? RecipientType { get; private set; }

	public string? RecipientId { get; private set; }

	public string? RecipientDisplayName { get; private set; }

	public string? SenderProvider { get; private set; }

	public string? SenderType { get; private set; }

	public string? SenderId { get; private set; }

	public string? SenderDisplayName { get; private set; }

	public string? DataJson { get; private set; }

	public DateTimeOffset NotifiedAt { get; private set; }

	public bool IsRead { get; private set; }

	public DateTimeOffset? ReadAt { get; private set; }

	private ApprovalCc()
	{
		Title = "";
	}

	public ApprovalCc(Guid id, string title, ParticipantReference recipient, DateTimeOffset notifiedAt, Guid? instanceId = null, string? instanceReference = null, string? nodeId = null, string? nodeInstanceId = null, string? summary = null, ParticipantReference? sender = null, string? dataJson = null)
		: base(id)
	{
		Title = Check.NotNullOrWhiteSpace(title, nameof(title), ApprovalCcConsts.MaxTitleLength);
		InstanceId = instanceId;
		InstanceReference = instanceReference;
		NodeId = nodeId;
		NodeInstanceId = nodeInstanceId;
		Summary = summary;
		DataJson = dataJson;
		NotifiedAt = notifiedAt;
		SetRecipient(recipient);
		SetSender(sender);
	}

	public void MarkRead(DateTimeOffset at)
	{
		if (!IsRead)
		{
			IsRead = true;
			ReadAt = at;
		}
	}

	public ParticipantReference? GetRecipient()
	{
		return ParticipantColumns.ToReference("", RecipientProvider, RecipientType, RecipientId, RecipientDisplayName);
	}

	public ParticipantReference? GetSender()
	{
		return ParticipantColumns.ToReference("", SenderProvider, SenderType, SenderId, SenderDisplayName);
	}

	private void SetRecipient(ParticipantReference recipient)
{
		Check.NotNull(recipient, nameof(recipient));
		Check.NotNullOrWhiteSpace(recipient.Id, nameof(recipient), ApprovalCcConsts.MaxParticipantIdLength);
		RecipientProvider = recipient.Provider;
		RecipientType = recipient.Type.ToString();
		RecipientId = recipient.Id;
		RecipientDisplayName = recipient.DisplayName;
	}

	private void SetSender(ParticipantReference? sender)
	{
		SenderProvider = sender?.Provider;
		SenderType = sender?.Type.ToString();
		SenderId = sender?.Id;
		SenderDisplayName = sender?.DisplayName;
	}
}
