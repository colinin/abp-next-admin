using Elsa.ApprovalFlow.Contracts;
using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Approvers;

/// <summary>
/// Resolves <see cref="ApproverKind.Role"/> rules to ABP role groups. Roles stay as candidate groups so they match
/// the actor's role groups, and the participant directory expands them to members when a concrete user is needed.
/// </summary>
public class RoleApproverRuleResolver : IApproverRuleResolver, ITransientDependency
{
    public IReadOnlySet<ApproverKind> SupportedKinds { get; } = new HashSet<ApproverKind>
    {
        ApproverKind.Role,
    };

    public virtual Task<IReadOnlyCollection<ParticipantReference>> ResolveAsync(
        ApproverRule rule,
        ApproverResolutionContext context,
        CancellationToken cancellationToken = default)
    {
        var roles = rule.Participants
            .Select(participant => participant.Provider == ParticipantReferenceFactory.RoleProvider
                ? participant
                : ParticipantReferenceFactory.Role(context.TenantId, participant.Id, participant.DisplayName))
            .ToArray();

        return Task.FromResult<IReadOnlyCollection<ParticipantReference>>(roles);
    }
}
