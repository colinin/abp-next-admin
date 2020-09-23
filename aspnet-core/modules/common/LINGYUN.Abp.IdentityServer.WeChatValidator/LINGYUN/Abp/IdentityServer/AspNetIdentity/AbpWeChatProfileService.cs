using IdentityServer4.AspNetIdentity;
using IdentityServer4.Models;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.IdentityServer.AspNetIdentity
{
    public class AbpWeChatProfileServicee : ProfileService<IdentityUser>
    {
        protected ICurrentTenant CurrentTenant { get; }
        public AbpWeChatProfileServicee(
            IdentityUserManager userManager,
            Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
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
                if (context.RequestedClaimTypes.Any(rc => rc.Contains(WeChatClaimTypes.OpenId)))
                {
                    context.IssuedClaims.Add(context.Subject.FindFirst(WeChatClaimTypes.OpenId));
                }
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
    }
}
