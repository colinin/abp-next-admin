using Elsa.UserTasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elsa.ApprovalFlow.Models;

public sealed class ApprovalNodeState
{
	public string NodeInstanceId { get; set; } = "";

	public string TenantId { get; set; } = "";

	public ApprovalSignMode SignMode { get; set; }

	public int ExpectedApproverCount { get; set; }

	public List<ApproverSlot> Slots { get; set; } = new List<ApproverSlot>();

	public List<ApproverDecision> Decisions { get; set; } = new List<ApproverDecision>();

	public Dictionary<string, ApprovalActionEffect> ActionEffects { get; set; } = new Dictionary<string, ApprovalActionEffect>(StringComparer.OrdinalIgnoreCase);

	public bool Settled { get; set; }

	public static ApprovalNodeState Create(string nodeInstanceId, string tenantId, ApprovalSignMode signMode, IReadOnlyCollection<ParticipantReference> approvers)
	{
		var approvalNodeState = new ApprovalNodeState
		{
			NodeInstanceId = nodeInstanceId,
			TenantId = tenantId,
			SignMode = signMode
		};
		approvalNodeState.Slots.AddRange(approvers.Select(x => new ApproverSlot
		{
			Approver = x
		}));
		return approvalNodeState;
	}

	public IReadOnlyList<ApproverSlot> PendingSlots()
	{
		return Slots.Where(x => !x.Dispatched && !x.Decided).ToArray();
	}

	public ApproverSlot? Record(ApproverDecision decision)
	{
		var approverSlot = Slots.FirstOrDefault(x => !x.Decided && x.Approver.Matches(decision.Approver)) ?? Slots.FirstOrDefault(x => !x.Decided && x.Dispatched);
		if (approverSlot != null)
		{
			approverSlot.Decided = true;
		}
		Decisions.Add(decision);
		return approverSlot;
	}
}
