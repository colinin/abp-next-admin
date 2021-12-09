using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.DependencyInjection;
using IdentityOptions = Microsoft.AspNetCore.Identity.IdentityOptions;

namespace LY.MicroService.IdentityServer.Pages.Account
{
    /// <summary>
    /// 重写登录模型,实现双因素登录
    /// </summary>
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(LoginModel), typeof(IdentityServerSupportedLoginModel))]
    public class TwoFactorSupportedLoginModel : IdentityServerSupportedLoginModel
    {
        public TwoFactorSupportedLoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions,
            IOptions<IdentityOptions> identityOptions,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService identityServerEvents)
            : base(schemeProvider, accountOptions, interaction, clientStore, identityServerEvents, identityOptions)
        {

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
