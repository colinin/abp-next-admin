using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.ApprovalFlow.Contracts;

public interface IApproverRuleResolver
{
	IReadOnlySet<ApproverKind> SupportedKinds { get; }

	Task<IReadOnlyCollection<ParticipantReference>> ResolveAsync(ApproverRule rule, ApproverResolutionContext context, CancellationToken cancellationToken = default);
}
