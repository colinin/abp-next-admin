using System.Collections.Generic;

namespace Elsa.ApprovalFlow.Models;

public sealed record ApprovalPolicyResult(bool IsFinal, bool IsApproved, string ActionKey, IReadOnlyCollection<ApproverDecision> Decisions);
