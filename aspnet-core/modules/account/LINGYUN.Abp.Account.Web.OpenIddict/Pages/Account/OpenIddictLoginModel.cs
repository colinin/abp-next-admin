using LINGYUN.Abp.Account.Web.Pages.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;

namespace LINGYUN.Abp.Account.Web.OpenIddict.Pages.Account
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(
        typeof(LINGYUN.Abp.Account.Web.Pages.Account.LoginModel),
        typeof(OpenIddictLoginModel))]
    public class OpenIddictLoginModel : LINGYUN.Abp.Account.Web.Pages.Account.LoginModel
    {
        protected AbpOpenIddictRequestHelper OpenIddictRequestHelper { get; }
        public OpenIddictLoginModel(AbpOpenIddictRequestHelper openIddictRequestHelper)
        {
            OpenIddictRequestHelper = openIddictRequestHelper;
        }

        public async override Task<IActionResult> OnGetAsync()
        {
            Input = new PasswordLoginInputModel();

            var request = await OpenIddictRequestHelper.GetFromReturnUrlAsync(ReturnUrl);
            if (request?.ClientId != null)
            {
                // TODO: Find a proper cancel way.
                // ShowCancelButton = true;

                Input.UserNameOrEmailAddress = request.LoginHint!;

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

        public async override Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "Cancel")
            {
                return await OnCancelLogin(action);
            }

            return await base.OnPostAsync(action);
        }

        public async override Task<IActionResult> OnPostExternalLogin(string provider)
        {
            if (AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                return await ProcessWindowsLoginAsync();
            }

            return await base.OnPostExternalLogin(provider);
        }

        protected async virtual Task<IActionResult> OnCancelLogin(string action)
        {
            var request = await OpenIddictRequestHelper.GetFromReturnUrlAsync(ReturnUrl);

            var transaction = HttpContext.GetOpenIddictServerTransaction();
            if (request?.ClientId != null && transaction != null)
            {
                transaction.EndpointType = OpenIddictServerEndpointType.Authorization;
                transaction.Request = request;

                var notification = new OpenIddictServerEvents.ValidateAuthorizationRequestContext(transaction);
                transaction.SetProperty(typeof(OpenIddictServerEvents.ValidateAuthorizationRequestContext).FullName!, notification);

                return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            return Redirect("~/");
        }

        protected virtual async Task<IActionResult> ProcessWindowsLoginAsync()
        {
            var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
            if (result.Succeeded)
            {
                var props = new AuthenticationProperties()
                {
                    RedirectUri = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash }),
                    Items =
                {
                    {
                        "LoginProvider", AccountOptions.WindowsAuthenticationSchemeName
                    }
                }
                };

                var id = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
                id.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.Principal.FindFirstValue(ClaimTypes.PrimarySid)!));
                id.AddClaim(new Claim(ClaimTypes.Name, result.Principal.FindFirstValue(ClaimTypes.Name)!));

                await HttpContext.SignInAsync(IdentityConstants.ExternalScheme, new ClaimsPrincipal(id), props);

                return Redirect(props.RedirectUri!);
            }

            return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
        }
    }
}
