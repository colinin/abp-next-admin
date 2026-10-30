using Elsa.ApprovalFlow.Models;
using System.Collections.Generic;

namespace Elsa.ApprovalFlow.Contracts;

public interface IApprovalCompletionPolicy
{
	ApprovalPolicyResult Evaluate(ApprovalSignMode mode, IReadOnlyCollection<ApproverDecision> decisions, int expectedApproverCount);
}
