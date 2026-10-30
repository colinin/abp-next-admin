using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.ApprovalFlow.Contracts;

public interface IApproverResolver
{
	Task<IReadOnlyCollection<ParticipantReference>> ResolveAsync(ApproverResolutionContext context, CancellationToken cancellationToken = default);
}
