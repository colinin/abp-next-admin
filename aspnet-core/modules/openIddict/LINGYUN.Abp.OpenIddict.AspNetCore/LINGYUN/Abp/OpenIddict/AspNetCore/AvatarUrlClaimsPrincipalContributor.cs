using LINGYUN.Abp.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.OpenIddict.AspNetCore;
public class AvatarUrlClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
{
    public async virtual Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        if (identity != null)
        {
            if (identity.HasClaim(x => x.Type == IdentityConsts.ClaimType.Avatar.Name))
            {
                return;
            }
            var userManager = context.ServiceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.GetUserAsync(context.ClaimsPrincipal);
            var userClaims = await userManager.GetClaimsAsync(user);
            var userAvatarUrl = userClaims.FirstOrDefault(x => x.Type == IdentityConsts.ClaimType.Avatar.Name);
            if (userAvatarUrl != null)
            {
                identity.AddIfNotContains(new Claim(IdentityConsts.ClaimType.Avatar.Name, userAvatarUrl.Value));
            }
        }
    }
}
