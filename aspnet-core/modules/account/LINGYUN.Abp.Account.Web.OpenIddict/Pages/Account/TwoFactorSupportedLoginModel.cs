using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Account.Web;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using IdentityOptions = Microsoft.AspNetCore.Identity.IdentityOptions;

namespace LINGYUN.Abp.Account.Web.OpenIddict.Pages.Account
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(
        typeof(LINGYUN.Abp.Account.Web.Pages.Account.LoginModel),
        typeof(TwoFactorSupportedLoginModel))]
    public class TwoFactorSupportedLoginModel : LINGYUN.Abp.Account.Web.Pages.Account.LoginModel
    {
        protected AbpOpenIddictRequestHelper OpenIddictRequestHelper { get; }
        public TwoFactorSupportedLoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions,
            IOptions<IdentityOptions> identityOptions,
            IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
            AbpOpenIddictRequestHelper openIddictRequestHelper)
            : base(schemeProvider, accountOptions, identityOptions, identityDynamicClaimsPrincipalContributorCache)
        {
            OpenIddictRequestHelper = openIddictRequestHelper;
        }

        public async override Task<IActionResult> OnGetAsync()
        {
            LoginInput = new LoginInputModel();

            var request = await OpenIddictRequestHelper.GetFromReturnUrlAsync(ReturnUrl);
            if (request?.ClientId != null)
            {
                // TODO: Find a proper cancel way.
                // ShowCancelButton = true;

                LoginInput.UserNameOrEmailAddress = request.LoginHint;

                //TODO: Reference AspNetCore MultiTenancy module and use options to get the tenant key!
                var tenant = request.GetParameter(TenantResolverConsts.DefaultTenantKey)?.ToString();
                if (!string.IsNullOrEmpty(tenant))
                {
                    CurrentTenant.Change(Guid.Parse(tenant));
                    Response.Cookies.Append(TenantResolverConsts.DefaultTenantKey, tenant);
                }
            }

            return await base.OnGetAsync();
        }
    }
}
