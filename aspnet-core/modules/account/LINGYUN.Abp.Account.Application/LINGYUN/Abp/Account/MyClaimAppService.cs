using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Account;

[Authorize]
public class MyClaimAppService : AccountApplicationServiceBase, IMyClaimAppService
{
    public async virtual Task<GetUserClaimStateDto> GetStateAsync(string claimType)
    {
        var user = await GetCurrentUserAsync();

        var userClaim = user.Claims.FirstOrDefault(x => x.ClaimType == claimType);

        return new GetUserClaimStateDto
        {
            IsBound = userClaim != null,
            Value = userClaim?.ClaimValue,
        };
    }

    public async virtual Task ResetAsync(string claimType)
    {
        var user = await GetCurrentUserAsync();

        var seeyonLoginClaim = user.Claims.FirstOrDefault(x => x.ClaimType == claimType);
        if (seeyonLoginClaim != null)
        {
            (await UserManager.RemoveClaimAsync(user, seeyonLoginClaim.ToClaim())).CheckErrors();
        }
    }
}
