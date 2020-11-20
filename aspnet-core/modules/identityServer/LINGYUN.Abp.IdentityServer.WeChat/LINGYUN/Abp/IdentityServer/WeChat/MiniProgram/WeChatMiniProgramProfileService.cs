using IdentityServer4.AspNetIdentity;
using IdentityServer4.Models;
using LINGYUN.Abp.WeChat.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LINGYUN.Abp.IdentityServer.WeChat.MiniProgram
{
    public class WeChatMiniProgramProfileService : ProfileService<IdentityUser>
    {
        protected ICurrentTenant CurrentTenant { get; }
        public WeChatMiniProgramProfileService(
            IdentityUserManager userManager,
            IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
            ICurrentTenant currentTenant)
            : base(userManager, claimsFactory)
        {
            CurrentTenant = currentTenant;
        }

        [UnitOfWork]
        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            using (CurrentTenant.Change(context.Subject.FindTenantId()))
            {
                await base.GetProfileDataAsync(context);

                // TODO: 可以从令牌获取openid, 安全性呢?
                TryAddWeChatClaim(context, AbpWeChatClaimTypes.OpenId);
                TryAddWeChatClaim(context, AbpWeChatClaimTypes.UnionId);
            }
        }

        [UnitOfWork]
        public override async Task IsActiveAsync(IsActiveContext context)
        {
            using (CurrentTenant.Change(context.Subject.FindTenantId()))
            {
                await base.IsActiveAsync(context);
            }
        }

        protected virtual void TryAddWeChatClaim(ProfileDataRequestContext context, string weChatClaimType)
        {
            if (context.RequestedClaimTypes.Any(rc => rc.Contains(weChatClaimType)))
            {
                var weChatClaim = context.Subject.FindFirst(weChatClaimType);
                if (weChatClaim != null)
                {
                    context.IssuedClaims.Add(weChatClaim);
                }
            }
        }
    }
}
