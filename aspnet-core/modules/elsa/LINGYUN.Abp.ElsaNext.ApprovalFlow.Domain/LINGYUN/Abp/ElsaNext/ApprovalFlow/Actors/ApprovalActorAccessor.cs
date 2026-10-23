using Elsa.UserTasks.Contracts;
using Elsa.UserTasks.Models;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Actors;

public class ApprovalActorAccessor : DomainService, IApprovalActorAccessor
{
	protected ICurrentPrincipalAccessor PrincipalAccessor { get; }

	protected IUserTaskIdentityResolver IdentityResolver { get; }

	public ApprovalActorAccessor(
		ICurrentPrincipalAccessor principalAccessor,
		IUserTaskIdentityResolver identityResolver)
	{
		PrincipalAccessor = principalAccessor;
		IdentityResolver = identityResolver;
	}

    public virtual async Task<UserTaskActor?> GetAsync(CancellationToken cancellationToken = default)
	{
		var principal = PrincipalAccessor.Principal;
		if (principal == null || principal.Identity?.IsAuthenticated != true)
		{
			return null;
		}
		var actor = await IdentityResolver.ResolveAsync(principal, cancellationToken);
		if (actor == null)
		{
			return null;
		}

		return actor;
	}
}
