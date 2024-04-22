using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using IdentityOptions = Microsoft.AspNetCore.Identity.IdentityOptions;

namespace LY.MicroService.Applications.Single.Pages.Account
{
    /// <summary>
    /// 重写登录模型,实现双因素登录
    /// </summary>
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(LoginModel), typeof(OpenIddictSupportedLoginModel))]
    public class TwoFactorSupportedLoginModel : OpenIddictSupportedLoginModel
    {
        public TwoFactorSupportedLoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions,
            IOptions<IdentityOptions> identityOptions,
            IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
            AbpOpenIddictRequestHelper openIddictRequestHelper)
            : base(schemeProvider, accountOptions, identityOptions, identityDynamicClaimsPrincipalContributorCache, openIddictRequestHelper)
        {

        }

        protected async override Task<List<ExternalProviderModel>> GetExternalProviders()
        {
            var providers = await base.GetExternalProviders();

            foreach (var provider in providers)
            {
                var localizedDisplayName = L[provider.DisplayName];
                if (localizedDisplayName.ResourceNotFound)
                {
                    localizedDisplayName = L["AuthenticationScheme:" + provider.DisplayName];
                }

                if (!localizedDisplayName.ResourceNotFound)
                {
                    provider.DisplayName = localizedDisplayName.Value;
                }
            }

            return providers;
        }

        protected override Task<IActionResult> TwoFactorLoginResultAsync()
        {
            // 重定向双因素认证页面
            return Task.FromResult<IActionResult>(RedirectToPage("SendCode", new
            {
                returnUrl = ReturnUrl,
                returnUrlHash = ReturnUrlHash,
                rememberMe = LoginInput.RememberMe
            })); 
        }
    }
}
