using LINGYUN.Abp.Account.Web.ExternalProviders;
using LINGYUN.Abp.Account.Web.Pages.Account;
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
        typeof(OpenIddictLoginModel))]
    public class OpenIddictLoginModel : LINGYUN.Abp.Account.Web.Pages.Account.LoginModel
    {
        protected AbpOpenIddictRequestHelper OpenIddictRequestHelper { get; }
        public OpenIddictLoginModel(
            IExternalProviderService externalProviderService,
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions,
            IOptions<IdentityOptions> identityOptions,
            IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
            AbpOpenIddictRequestHelper openIddictRequestHelper)
            : base(externalProviderService, schemeProvider, accountOptions, identityOptions, identityDynamicClaimsPrincipalContributorCache)
        {
            OpenIddictRequestHelper = openIddictRequestHelper;
        }

        public async override Task<IActionResult> OnGetAsync()
        {
            PasswordLoginInput = new PasswordLoginInputModel();

            var request = await OpenIddictRequestHelper.GetFromReturnUrlAsync(ReturnUrl);
            if (request?.ClientId != null)
            {
                // TODO: Find a proper cancel way.
                // ShowCancelButton = true;

                PasswordLoginInput.UserNameOrEmailAddress = request.LoginHint;

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
