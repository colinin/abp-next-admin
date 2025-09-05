using LINGYUN.Abp.Account.Web.ExternalProviders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Account.Web.Areas.Account.Controllers;

[Controller]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route($"api/{AccountRemoteServiceConsts.ModuleName}")]
[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
public class AccountController : AbpController
{
    protected AbpSignInManager SignInManager => LazyServiceProvider.LazyGetRequiredService<AbpSignInManager>();
    protected IdentityUserManager UserManager => LazyServiceProvider.LazyGetRequiredService<IdentityUserManager>();
    protected IExternalProviderService ExternalProviderService => LazyServiceProvider.LazyGetRequiredService<IExternalProviderService>();

    public AccountController()
    {
        LocalizationResource = typeof(AccountResource);
    }

    [HttpGet]
    [Authorize]
    [Route("external-logins")]
    public async virtual Task<ExternalLoginResultDto> GetExternalLoginsAsync()
    {
        var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());
        var userLogins = await UserManager.GetLoginsAsync(currentUser);
        var externalProviders = await ExternalProviderService.GetAllAsync();

        return new ExternalLoginResultDto
        {
            UserLogins = userLogins.Select(x => new UserLoginInfoDto
            {
                ProviderDisplayName = x.ProviderDisplayName,
                ProviderKey = x.ProviderKey,
                LoginProvider = x.LoginProvider,
            }).ToList(),
            ExternalLogins = externalProviders.Select(x =>
            {
                return new ExternalLoginInfoDto
                {
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                };
            }).ToList(),
        };
    }

    [HttpDelete]
    [Authorize]
    [Route("external-logins/remove")]
    public async virtual Task RemoveExternalLoginAsync(RemoveExternalLoginInput input)
    {
        var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());
        var identityResult = await UserManager.RemoveLoginAsync(
            currentUser,
            input.LoginProvider,
            input.ProviderKey);

        if (!identityResult.Succeeded)
        {
            throw new UserFriendlyException("Operation failed: " + identityResult.Errors.Select(e => $"[{e.Code}] {e.Description}").JoinAsString(", "));
        }
        // 解绑的是当前身份认证方案则退出登录
        var amr = CurrentUser.FindClaimValue(ClaimTypes.AuthenticationMethod);
        if (!amr.IsNullOrWhiteSpace() && string.Equals(amr, input.LoginProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            await SignInManager.SignOutAsync();
        }
    }

    [HttpGet]
    [Authorize]
    [Route("external-logins/bind")]
    public virtual async Task<IActionResult> ExternalLoginBindAsync(string provider, string returnUrl)
    {
        if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(returnUrl))
        {
            Logger.LogWarning("The parameter is incorrect");
            return Redirect(QueryHelpers.AddQueryString(returnUrl, new Dictionary<string, string>()
            {
                ["error"] = "The parameter is incorrect"
            }));
        }

        var tenantId = CurrentTenant.Id;
        var userId = CurrentUser.GetId();

        var redirectUrl = Url.Page("/Account/ExternalLoginBind", pageHandler: "BindCallback", values: new { returnUrl, userId, tenantId });

        var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, userId.ToString());
        properties.Items["scheme"] = provider;

        return await Task.FromResult(Challenge(properties, provider));
    }
}
