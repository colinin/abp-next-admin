using Elsa.ApprovalFlow.Contracts;
using Elsa.ApprovalFlow.Models;
using System.Collections.Generic;
using System.Linq;

namespace Elsa.ApprovalFlow.Services;

public sealed class DefaultApprovalCompletionPolicy : IApprovalCompletionPolicy
{
	public ApprovalPolicyResult Evaluate(ApprovalSignMode mode, IReadOnlyCollection<ApproverDecision> decisions, int expectedApproverCount)
	{
		var array = decisions.Where(x => x.Effect != ApprovalActionEffect.Neutral).ToArray();
		if (array.Length == 0)
		{
			return new ApprovalPolicyResult(IsFinal: false, IsApproved: false, "", decisions);
		}
		var approverDecision = array.FirstOrDefault(x => x.Effect == ApprovalActionEffect.Reject);
		if (approverDecision != null)
		{
			return new ApprovalPolicyResult(IsFinal: true, IsApproved: false, approverDecision.ActionKey, decisions);
		}
		if (mode == ApprovalSignMode.Any)
		{
			var approverDecision2 = array[0];
			return new ApprovalPolicyResult(IsFinal: true, IsApproved: true, approverDecision2.ActionKey, decisions);
		}
		var num = array.Count(x => x.Effect == ApprovalActionEffect.Approve);
		if (num >= expectedApproverCount)
		{
			var approverDecision3 = array.Last(x => x.Effect == ApprovalActionEffect.Approve);
			return new ApprovalPolicyResult(IsFinal: true, IsApproved: true, approverDecision3.ActionKey, decisions);
		}
		return new ApprovalPolicyResult(IsFinal: false, IsApproved: false, "", decisions);
	}
}
