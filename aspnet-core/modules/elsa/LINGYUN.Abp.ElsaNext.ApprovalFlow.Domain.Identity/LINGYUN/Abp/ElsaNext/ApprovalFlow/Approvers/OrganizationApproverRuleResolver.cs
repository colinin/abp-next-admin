using Elsa.ApprovalFlow.Contracts;
using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.OrganizationUnits;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Approvers;

public class OrganizationApproverRuleResolver : IApproverRuleResolver, ITransientDependency
{
	protected const int MaxDepartmentMembers = 200;

	public IReadOnlySet<ApproverKind> SupportedKinds { get; } = new HashSet<ApproverKind>
	{
		ApproverKind.OrganizationUnit,
		ApproverKind.Supervisor
	};

	protected IOrganizationHierarchy OrganizationHierarchy { get; }

	public OrganizationApproverRuleResolver(IOrganizationHierarchy organizationHierarchy)
	{
		OrganizationHierarchy = organizationHierarchy;
	}

	public virtual async Task<IReadOnlyCollection<ParticipantReference>> ResolveAsync(ApproverRule rule, ApproverResolutionContext context, CancellationToken cancellationToken = default)
	{
		return rule.Kind switch
		{
			ApproverKind.OrganizationUnit => await ResolveOrganizationUnitAsync(rule, context, cancellationToken),
			ApproverKind.Supervisor => await ResolveSupervisorAsync(rule, context, cancellationToken),
			_ => Array.Empty<ParticipantReference>(),
		};
	}

	protected virtual async Task<IReadOnlyCollection<ParticipantReference>> ResolveOrganizationUnitAsync(ApproverRule rule, ApproverResolutionContext context, CancellationToken cancellationToken)
	{
		var target = rule.Participants.FirstOrDefault(x => x.Provider == ParticipantReferenceFactory.GroupProvider) ?? rule.Participants.FirstOrDefault();
		var organizationUnitId = target != null ? ParticipantReferenceFactory.TryGetId(target) : null;
		if (!organizationUnitId.HasValue)
		{
			return Array.Empty<ParticipantReference>();
		}

		return (await OrganizationHierarchy.GetMemberUserIdsAsync(organizationUnitId.Value, 0, MaxDepartmentMembers, null, cancellationToken))
			.Select(userId => ParticipantReferenceFactory.User(context.TenantId, userId))
			.ToArray();
	}

	protected virtual async Task<IReadOnlyCollection<ParticipantReference>> ResolveSupervisorAsync(ApproverRule rule, ApproverResolutionContext context, CancellationToken cancellationToken)
	{
		var initiatorUserId = context.Initiator != null ? ParticipantReferenceFactory.TryGetId(context.Initiator) : null;
		if (!initiatorUserId.HasValue)
		{
			return Array.Empty<ParticipantReference>();
		}

		var organizationUnitId = await OrganizationHierarchy.FindPrimaryOrganizationUnitIdAsync(initiatorUserId.Value, cancellationToken);
		if (!organizationUnitId.HasValue)
		{
			return Array.Empty<ParticipantReference>();
		}

		var chain = await OrganizationHierarchy.GetSupervisorChainAsync(
			organizationUnitId.Value,
			Math.Max(rule.Level, OrganizationUnitConsts.MaxSupervisorLevels),
			cancellationToken);
		var organizationUnitSupervisor = rule.SupervisorLevel switch
		{
			SupervisorLevelKind.TopMost => chain.LastOrDefault(),
			SupervisorLevelKind.LevelN => chain.FirstOrDefault(x => x.Level == Math.Max(rule.Level, 1)),
			_ => chain.FirstOrDefault(),
		};
		if (organizationUnitSupervisor == null)
		{
			return Array.Empty<ParticipantReference>();
		}

		return organizationUnitSupervisor.ManagerUserIds
			.Select(userId => ParticipantReferenceFactory.User(context.TenantId, userId))
			.ToArray();
	}
}
