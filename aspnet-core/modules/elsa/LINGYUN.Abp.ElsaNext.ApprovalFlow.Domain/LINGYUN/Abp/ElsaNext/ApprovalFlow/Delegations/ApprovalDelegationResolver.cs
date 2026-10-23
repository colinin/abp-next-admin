using Elsa.UserTasks.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Delegations;

public class ApprovalDelegationResolver : DomainService, IApprovalDelegationResolver
{
	protected IApprovalDelegationRepository Repository { get; }

	public ApprovalDelegationResolver(IApprovalDelegationRepository repository)
	{
		Repository = repository;
	}

	public virtual async Task<bool> IsDelegatedAsync(ParticipantReference delegator, ParticipantReference actor, CancellationToken cancellationToken = default)
	{
		if (delegator.Matches(actor))
		{
			return false;
		}
		return (await Repository.FindActiveListByDelegateAsync(actor.Id, new DateTimeOffset(Clock.Now), cancellationToken))
            .Any(x => x.IsDelegator(delegator) && x.IsDelegate(actor));
	}

	public virtual async Task<bool> HasActiveDelegateAsync(ParticipantReference delegator, CancellationToken cancellationToken = default)
	{
		return (await Repository.FindActiveListByDelegatorAsync(delegator.Id, new DateTimeOffset(Clock.Now), cancellationToken))
            .Any(x => x.IsDelegator(delegator));
	}
}
