using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Identity.Session;
public class AllowAnonymousIdentitySessionChecker : IIdentitySessionChecker
{
    public Task<bool> ValidateSessionAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}
