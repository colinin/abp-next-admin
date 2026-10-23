using Elsa.ApprovalFlow.Contracts;
using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.ApprovalFlow.Services;

public sealed class DefaultApproverResolver(IEnumerable<IApproverRuleResolver> ruleResolvers) : IApproverResolver
{
    public async Task<IReadOnlyCollection<ParticipantReference>> ResolveAsync(ApproverResolutionContext context, CancellationToken cancellationToken = default)
	{
		var resolversByKind = ruleResolvers.SelectMany(approverRuleResolver => approverRuleResolver.SupportedKinds.Select(kind => new
		{
			Kind = kind,
			Resolver = approverRuleResolver
		})).ToLookup(x => x.Kind, x => x.Resolver);
		var resolved = new List<ParticipantReference>();
		foreach (var rule in context.Rules)
		{
			var contributed = new List<ParticipantReference>();
			foreach (var resolver in resolversByKind[rule.Kind])
			{
				var list = contributed;
				list.AddRange(await resolver.ResolveAsync(rule, context, cancellationToken));
			}
			foreach (var participant in contributed)
			{
				if ((!rule.ExcludeInitiator || !(context.Initiator != null) || !participant.Matches(context.Initiator)) && (!rule.Deduplicate || !resolved.Any(participant.Matches)))
				{
					resolved.Add(participant);
				}
			}
		}
		return resolved;
	}
}
