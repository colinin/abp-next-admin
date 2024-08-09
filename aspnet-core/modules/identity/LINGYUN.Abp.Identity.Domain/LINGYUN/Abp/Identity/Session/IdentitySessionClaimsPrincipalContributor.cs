using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.Identity.Session.AspNetCore;
public class IdentitySessionClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
{
    public virtual Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var claimsIdentity = context.ClaimsPrincipal.Identities.First();
        if (claimsIdentity.HasClaim(x => x.Type == AbpClaimTypes.SessionId))
        {
            return Task.CompletedTask;
        }
        var sessionId = claimsIdentity.FindSessionId();
        if (!sessionId.IsNullOrWhiteSpace())
        {
            claimsIdentity.AddOrReplace(new Claim(AbpClaimTypes.SessionId, sessionId));
            context.ClaimsPrincipal.AddIdentityIfNotContains(claimsIdentity);
            return Task.CompletedTask;
        }

        var userId = claimsIdentity.FindUserId();
        if (userId.HasValue)
        {
            var sessionInfoProvider = context.GetRequiredService<ISessionInfoProvider>();
            sessionId = sessionInfoProvider.SessionId ?? Guid.NewGuid().ToString("N");

            claimsIdentity.AddOrReplace(new Claim(AbpClaimTypes.SessionId, sessionId));
            context.ClaimsPrincipal.AddIdentityIfNotContains(claimsIdentity);
        }
        return Task.CompletedTask;
    }
}
