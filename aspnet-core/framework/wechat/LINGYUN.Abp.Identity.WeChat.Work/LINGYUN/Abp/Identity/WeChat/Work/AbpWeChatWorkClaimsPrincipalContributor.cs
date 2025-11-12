using LINGYUN.Abp.WeChat.Work.Authorize;
using LINGYUN.Abp.WeChat.Work.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.Identity.WeChat.Work;
public class AbpWeChatWorkClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
{
    public async virtual Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var claimsIdentity = context.ClaimsPrincipal.Identities.First();
        if (claimsIdentity.HasClaim(x => x.Type == AbpWeChatWorkClaimTypes.UserId))
        {
            return;
        }
        var userId = claimsIdentity.FindUserId();
        if (userId.HasValue)
        {
            var userClaimProvider = context.ServiceProvider.GetService<IWeChatWorkUserClaimProvider>();

            var weChatWorkUserId = await userClaimProvider?.FindUserIdentifierAsync(userId.Value);
            if (!weChatWorkUserId.IsNullOrWhiteSpace())
            {
                claimsIdentity.AddOrReplace(new Claim(AbpWeChatWorkClaimTypes.UserId, weChatWorkUserId));

                context.ClaimsPrincipal.AddIdentityIfNotContains(claimsIdentity);
            }
        }
    }
}
