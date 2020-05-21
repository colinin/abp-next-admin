using Microsoft.AspNetCore.Http;
using System.Linq;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Platform.MultiTenancy
{
    public class AuthorizationTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public override string Name => "Authorization";

        protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            if (httpContext.User?.Identity == null)
            {
                return null;
            }
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return null;
            }
            var tenantIdKey = context.GetAbpAspNetCoreMultiTenancyOptions().TenantKey;

            var tenantClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(AbpClaimTypes.TenantId));

            if (tenantClaim == null)
            {
                return null;
            }

            return tenantClaim.Value;
        }
    }
}
