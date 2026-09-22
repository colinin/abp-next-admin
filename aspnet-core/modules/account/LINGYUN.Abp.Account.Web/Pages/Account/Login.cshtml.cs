using LINGYUN.Abp.Account.Dto;
using LINGYUN.Abp.Account.Web.Captcha;
using LINGYUN.Abp.Account.Web.ExternalProviders;
using LINGYUN.Abp.Account.Web.Models;
using LINGYUN.Abp.Identity.QrCode;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCSharp.HttpUserAgentParser.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Reflection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using IIdentityUserRepository = LINGYUN.Abp.Identity.IIdentityUserRepository;

namespace LINGYUN.Abp.Account.Web.Pages.Account;

public class LoginModel : AccountPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrlHash { get; set; }


    [BindProperty(SupportsGet = true)]
    public PasswordLoginInputModel Input { get; set; } = default!;

    public bool ShowCancelButton { get; set; }
    public bool EnableLocalLogin { get; set; }
    public bool EnableQrCodeLogin { get; set; }
    public bool EnableCaptchaLogin { get; set; }
    public CaptchaComponent CaptchaComponent { get; private set; } = default!;
    public ICaptchaComponentProvider CaptchaComponentProvider => LazyServiceProvider.LazyGetRequiredService<ICaptchaComponentProvider>();
    public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
    public IEnumerable<ExternalLoginProviderModel> ExternalProviders { get; set; } = default!;
    public IEnumerable<ExternalLoginProviderModel> VisibleExternalProviders => ExternalProviders.Where(x => !x.DisplayName.IsNullOrWhiteSpace());
    protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache => LazyServiceProvider.LazyGetRequiredService<IdentityDynamicClaimsPrincipalContributorCache>();
    protected IIdentityUserRepository UserRepository => LazyServiceProvider.LazyGetRequiredService<IIdentityUserRepository>();
    protected IQrCodeLoginProvider QrCodeLoginProvider => LazyServiceProvider.LazyGetRequiredService<IQrCodeLoginProvider>();
    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor => LazyServiceProvider.LazyGetRequiredService<ICurrentPrincipalAccessor>();
    protected IHttpUserAgentParserProvider HttpUserAgentParserProvider => LazyServiceProvider.LazyGetRequiredService<IHttpUserAgentParserProvider>();
    public IIdentityLinkUserAppService IdentityLinkUserAppService => LazyServiceProvider.LazyGetRequiredService<IIdentityLinkUserAppService>();
    protected IExternalProviderService ExternalProviderService => LazyServiceProvider.LazyGetRequiredService<IExternalProviderService>();
    protected IAuthenticationSchemeProvider SchemeProvider => LazyServiceProvider.LazyGetRequiredService<IAuthenticationSchemeProvider>();
    protected AbpAccountOptions AccountOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpAccountOptions>>().Value;

    #region LinkUser

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid? LinkUserId { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid? LinkTenantId { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? LinkToken { get; set; }

    public bool IsLinkLogin { get; set; }

    #endregion

    protected async virtual Task InitCaptchaComponent()
    {
        EnableCaptchaLogin = await SettingProvider.IsTrueAsync(Identity.Settings.IdentitySettingNames.SignIn.RequireCaptchaVerification);
        CaptchaComponent = await CaptchaComponentProvider.GetComponentOrDefaultAsync();
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        Input = new PasswordLoginInputModel();

        AllowQrCodeLoginIfNotMobileDevice();

        await InitCaptchaComponent();
        ExternalProviders = await GetExternalProviders();

        EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

        if (IsExternalLoginOnly)
        {
            return await OnPostExternalLogin(ExternalProviders.First().AuthenticationScheme);
        }

        IsLinkLogin = await VerifyLinkTokenAsync();
        if (IsLinkLogin && CurrentUser.IsAuthenticated)
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.Logout
            });

            await SignInManager.SignOutAsync();

            return Redirect(HttpContext.Request.GetDisplayUrl());
        }

        return Page();
    }

    public async virtual Task<IActionResult> OnPostAsync(string action)
    {
        ValidateModel();
        await IdentityOptions.SetAsync();

        await CheckLocalLoginAsync();
        await InitCaptchaComponent();
        ExternalProviders = await GetExternalProviders();
        EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

        if (EnableCaptchaLogin)
        {
            var isValid = await CaptchaComponent.ValidateAsync(
                new CaptchaValidatorContext(
                    LazyServiceProvider, 
                    Input.CaptchaCode!,
                    Input.UserNameOrEmailAddress)
            );
            if (!isValid)
            {
                Alerts.Danger(L["InvalidVerifyCode"]);
                return Page();
            }
        }

        await ReplaceEmailToUsernameOfInputIfNeeds();

        await IdentityOptions.SetAsync();

        IsLinkLogin = await VerifyLinkTokenAsync();

        var result = await SignInManager.PasswordSignInAsync(
            Input.UserNameOrEmailAddress,
            Input.Password,
            Input.RememberMe,
            true
        );

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = result.ToIdentitySecurityLogAction(),
            UserName = Input.UserNameOrEmailAddress
        });

        if (result.RequiresTwoFactor)
        {
            return await TwoFactorLoginResultAsync();
        }

        if (result.IsLockedOut)
        {
            return await HandleUserLockedOut();
        }

        if (result.IsNotAllowed)
        {
            return await HandleUserNotAllowed();
        }

        if (!result.Succeeded)
        {
            return await HandleUserNameOrPasswordInvalid();
        }

        //TODO: Find a way of getting user's id from the logged in user and do not query it again like that!
        var user = await GetIdentityUserAsync(Input.UserNameOrEmailAddress);

        Debug.Assert(user != null, nameof(user) + " != null");

        if (IsLinkLogin)
        {
            return await HandleLinkUserLogin(user);
        }

        // Clear the dynamic claims cache.
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

        return await RedirectSafelyAsync(ReturnUrl!, ReturnUrlHash);
    }

    public virtual async Task<IActionResult> OnPostExternalLogin(string provider)
    {
        var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash });
        var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        properties.Items["scheme"] = provider;

        return await Task.FromResult(Challenge(properties, provider));
    }

    public virtual async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = "", string returnUrlHash = "", string? remoteError = null)
    {
        //TODO: Did not implemented Identity Server 4 sample for this method (see ExternalLoginCallback in Quickstart of IDS4 sample)
        /* Also did not implement these:
         * - Logout(string logoutId)
         */

        if (remoteError != null)
        {
            Logger.LogWarning($"External login callback error: {remoteError}");
            return RedirectToPage("./Login");
        }

        await IdentityOptions.SetAsync();

        var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
        if (loginInfo == null)
        {
            Logger.LogWarning("External login info is not available");
            return RedirectToPage("./Login");
        }

        var result = await SignInManager.ExternalLoginSignInAsync(
            loginInfo.LoginProvider,
            loginInfo.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true
        );

        if (!result.Succeeded)
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                Action = "Login" + result
            });
        }

        if (result.IsLockedOut)
        {
            Logger.LogWarning($"External login callback error: user is locked out!");

            return await HandleUserLockedOut();
        }

        if (result.IsNotAllowed)
        {
            Logger.LogWarning($"External login callback error: user is not allowed!");

            return await HandleExternalLoginNotAllowed(loginInfo);
        }

        IdentityUser? user;
        if (result.Succeeded)
        {
            user = await UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
            if (user != null)
            {
                // Clear the dynamic claims cache.
                await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
            }

            return await RedirectSafelyAsync(returnUrl, returnUrlHash);
        }

        //TODO: Handle other cases for result!

        var email = loginInfo.Principal.FindFirstValue(AbpClaimTypes.Email) ?? loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        if (email.IsNullOrWhiteSpace())
        {
            return RedirectToPage("./Register", new
            {
                IsExternalLogin = true,
                ExternalLoginAuthSchema = loginInfo.LoginProvider,
                ReturnUrl = returnUrl,
                LinkUserId,
                LinkTenantId,
                LinkToken
            });
        }

        user = await UserManager.FindByEmailAsync(email);
        if (user == null)
        {
            return RedirectToPage("./Register", new
            {
                IsExternalLogin = true,
                ExternalLoginAuthSchema = loginInfo.LoginProvider,
                ReturnUrl = returnUrl,
                LinkUserId,
                LinkTenantId,
                LinkToken
            });
        }

        if (await UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey) == null)
        {
            CheckIdentityErrors(await UserManager.AddLoginAsync(user, loginInfo));
        }

        await SignInManager.SignInAsync(user, false);

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
            Action = result.ToIdentitySecurityLogAction(),
            UserName = user.Name
        });

        // Clear the dynamic claims cache.
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

        return await RedirectSafelyAsync(returnUrl, returnUrlHash);
    }

    protected virtual Task<IActionResult> TwoFactorLoginResultAsync()
    {
        // 重定向双因素认证页面
        return Task.FromResult<IActionResult>(RedirectToPage("SendCode", new
        {
            returnUrl = ReturnUrl,
            returnUrlHash = ReturnUrlHash,
            rememberMe = Input.RememberMe,
            linkUserId = LinkUserId,
            linkTenantId = LinkTenantId,
            linkToken = LinkToken,
        }));
    }

    protected virtual async Task<IdentityUser?> GetIdentityUserAsync(string userNameOrEmailAddress)
    {
        return await UserManager.FindByNameAsync(userNameOrEmailAddress) ??
            await UserManager.FindByEmailAsync(userNameOrEmailAddress);
    }

    protected async virtual Task<List<ExternalLoginProviderModel>> GetExternalProviders()
    {
        var schemes = await SchemeProvider.GetAllSchemesAsync();
        var externalProviders = await ExternalProviderService.GetAllAsync();

        var externalProviderModels = new List<ExternalLoginProviderModel>();
        foreach (var scheme in schemes)
        {
            if (TryGetExternalLoginProvider(scheme, externalProviders, out var externalLoginProvider) || 
                scheme.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
            {
                externalProviderModels.Add(new ExternalLoginProviderModel
                {
                    Name = externalLoginProvider!.Name,
                    AuthenticationScheme = scheme.Name,
                    DisplayName = externalLoginProvider.DisplayName,
                    ComponentType = externalLoginProvider.ComponentType,
                });
            }
        }

        return externalProviderModels;
    }

    protected virtual bool TryGetExternalLoginProvider(AuthenticationScheme scheme, List<ExternalLoginProviderModel> externalProviders, out ExternalLoginProviderModel? externalLoginProvider)
    {
        if (ReflectionHelper.IsAssignableToGenericType(scheme.HandlerType, typeof(RemoteAuthenticationHandler<>)))
        {
            externalLoginProvider = externalProviders.FirstOrDefault(x => x.Name == scheme.Name);
            return externalLoginProvider != null;
        }

        externalLoginProvider = null;
        return false;
    }

    protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds()
    {
        if (!ValidationHelper.IsValidEmailAddress(Input.UserNameOrEmailAddress))
        {
            return;
        }

        var userByUsername = await UserManager.FindByNameAsync(Input.UserNameOrEmailAddress);
        if (userByUsername != null)
        {
            return;
        }

        var userByEmail = await UserManager.FindByEmailAsync(Input.UserNameOrEmailAddress);
        if (userByEmail == null)
        {
            return;
        }

        Input.UserNameOrEmailAddress = userByEmail.UserName;
    }

    protected virtual async Task CheckLocalLoginAsync()
    {
        if (!await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
        {
            throw new UserFriendlyException(L["LocalLoginDisabledMessage"]);
        }
    }

    protected virtual Task<IActionResult> HandleUserLockedOut()
    {
        Alerts.Warning(L["UserLockedOutMessage"]);
        return Task.FromResult<IActionResult>(Page());
    }

    protected async virtual Task<IActionResult> HandleUserEmailConfirm(IdentityUser user)
    {
        await StoreConfirmUserAsync(user);
        return RedirectToPage("UserEmailConfirm", new
        {
            returnUrl = ReturnUrl,
            returnUrlHash = ReturnUrlHash,
            rememberMe = Input.RememberMe,
            linkUserId = LinkUserId,
            linkTenantId = LinkTenantId,
            linkToken = LinkToken,
        });
    }

    protected async virtual Task<IActionResult> HandleUserNotAllowed()
    {
        var notAllowedUser = await GetIdentityUserAsync(Input.UserNameOrEmailAddress);
        if (notAllowedUser != null && await UserManager.CheckPasswordAsync(notAllowedUser, Input.Password))
        {
            // 用户必须修改密码
            if (notAllowedUser.ShouldChangePasswordOnNextLogin || await UserManager.ShouldPeriodicallyChangePasswordAsync(notAllowedUser))
            {
                await StoreChangePasswordUserAsync(notAllowedUser);

                return RedirectToPage("ChangePassword", new
                {
                    returnUrl = ReturnUrl,
                    returnUrlHash = ReturnUrlHash,
                    rememberMe = Input.RememberMe,
                });
            }
        }
        if (notAllowedUser != null &&
            !notAllowedUser.EmailConfirmed &&
            await SettingProvider.IsTrueAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail))
        {
            return await HandleUserEmailConfirm(notAllowedUser);
        }
        Alerts.Warning(L["LoginIsNotAllowed"]);
        return Page();
    }

    protected async virtual Task<IActionResult> HandleExternalLoginNotAllowed(ExternalLoginInfo loginInfo)
    {
        Logger.LogWarning("External login callback error: User is Not Allowed!");

        var user = await UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
        if (user == null)
        {
            Logger.LogWarning($"External login callback error: User is Not Found!");
            return RedirectToPage("./Login");
        }

        if (user.ShouldChangePasswordOnNextLogin || await UserManager.ShouldPeriodicallyChangePasswordAsync(user))
        {
            await StoreChangePasswordUserAsync(user);
            return RedirectToPage("./ChangePassword", new
            {
                ReturnUrl,
                ReturnUrlHash,
            });
        }

        Alerts.Warning(L["LoginIsNotAllowed"]);
        return Page();
    }

    protected virtual async Task StoreChangePasswordUserAsync(IdentityUser user)
    {
        var changePwdIdentity = new ClaimsIdentity(AbpAccountAuthenticationTypes.ShouldChangePassword);
        changePwdIdentity.AddClaim(new Claim(AbpClaimTypes.UserId, user.Id.ToString()));
        if (user.TenantId.HasValue)
        {
            changePwdIdentity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.Value.ToString()));
        }

        await HttpContext.SignInAsync(AbpAccountAuthenticationTypes.ShouldChangePassword, new ClaimsPrincipal(changePwdIdentity));
    }

    protected virtual async Task StoreConfirmUserAsync(IdentityUser user)
    {
        var identity = new ClaimsIdentity(AbpAccountAuthenticationTypes.ConfirmUserScheme);
        identity.AddClaim(new Claim(AbpClaimTypes.UserId, user.Id.ToString()));

        if (user.TenantId.HasValue)
        {
            identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.Value.ToString()));
        }

        await HttpContext.SignInAsync(AbpAccountAuthenticationTypes.ConfirmUserScheme, new ClaimsPrincipal(identity));
    }

    protected virtual Task<IActionResult> HandleUserNameOrPasswordInvalid()
    {
        Alerts.Danger(L["InvalidUserNameOrPassword"]);
        return Task.FromResult<IActionResult>(Page());
    }

    protected virtual void AllowQrCodeLoginIfNotMobileDevice()
    {
        if (HttpContext?.Request?.Headers?.UserAgent.IsNullOrEmpty() == false)
        {
            var userAgentInfo = HttpUserAgentParserProvider.Parse(HttpContext.Request.Headers.UserAgent!);
            if (userAgentInfo.MobileDeviceType.IsNullOrWhiteSpace())
            {
                EnableQrCodeLogin = true;
            }
        }
    }

    #region LinkUser

    public async virtual Task<string> GetWithoutLinkReturnUrlAsync(string? returnUrl, string? returnUrlHash = null)
    {
        var redirectUrl = await base.GetRedirectUrlAsync(returnUrl!, returnUrlHash);

        // 使用正则表达式移除 LinkUser 参数
        redirectUrl = Regex.Replace(redirectUrl, @"[&?]LinkToken=[^&]*", "");
        redirectUrl = Regex.Replace(redirectUrl, @"[&?]LinkUserId=[^&]*", "");
        redirectUrl = Regex.Replace(redirectUrl, @"[&?]LinkTenantId=[^&]*", "");
        redirectUrl = Regex.Replace(redirectUrl, @"[&?]linkUserId=[^&]*", "");
        redirectUrl = Regex.Replace(redirectUrl, @"[&?]linkToken=[^&]*", "");

        return redirectUrl;
    }

    protected async virtual Task<bool> VerifyLinkTokenAsync()
    {
        if (LinkToken.IsNullOrWhiteSpace() || !LinkUserId.HasValue)
        {
            if (string.IsNullOrWhiteSpace(ReturnUrl))
            {
                return false;
            }
            var queryString = QueryHelpers.ParseQuery(ReturnUrl);
            queryString.TryGetValue("LinkToken", out var linkTokenVal);
            queryString.TryGetValue("LinkUserId", out var linkUserIdVal);
            queryString.TryGetValue("LinkTenantId", out var linkTenantIdVal);
            if (!linkTokenVal.IsNullOrEmpty() && !linkUserIdVal.IsNullOrEmpty() &&
                Guid.TryParse(linkUserIdVal.ToString(), out var linkUserId))
            {
                LinkToken = linkTokenVal.ToString();
                LinkUserId = linkUserId;
                if (Guid.TryParse(linkTenantIdVal.ToString(), out var linkTenantId))
                {
                    LinkTenantId = linkTenantId;
                }
            }
            else
            {
                return false;
            }
        }
        // TODO: 替换框架对于空格符号的编码错误
        LinkToken = LinkToken.Replace(" ", "+");
        return await IdentityLinkUserAppService.VerifyLinkTokenAsync(new VerifyLinkTokenInput()
        {
            UserId = LinkUserId.Value,
            TenantId = LinkTenantId,
            Token = LinkToken
        });
    }

    protected async virtual Task<IActionResult> HandleLinkUserLogin(IdentityUser user)
    {
        using (CurrentPrincipalAccessor.Change(await SignInManager.CreateUserPrincipalAsync(user)))
        {
            await IdentityLinkUserAppService.LinkAsync(new LinkUserInput
            {
                UserId = LinkUserId!.Value,
                TenantId = LinkTenantId,
                Token = LinkToken!
            });

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                UserName = user.UserName,
                Action = "LinkUser",
                ExtraProperties =
                {
                    { "LinkTenantId",  LinkTenantId },
                    { "LinkUserId", LinkUserId }
                }
            });

            using (CurrentTenant.Change(LinkTenantId))
            {
                user = await UserManager.GetByIdAsync(LinkUserId.Value);
                using (CurrentPrincipalAccessor.Change(await SignInManager.CreateUserPrincipalAsync(user)))
                {
                    await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                    {
                        Identity = IdentitySecurityLogIdentityConsts.Identity,
                        UserName = user.UserName,
                        Action = "LinkUser",
                        ExtraProperties =
                        {
                            { "LinkTenantId",  LinkTenantId },
                            { "LinkUserId", LinkUserId }
                        }
                    });
                }
            }

            return RedirectToPage("./LinkLogged", new
            {
                ReturnUrl,
                ReturnUrlHash,
                LinkUserId,
                LinkTenantId
            });
        }
    }

    #endregion
}

public class PasswordLoginInputModel
{
    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    public string UserNameOrEmailAddress { get; set; } = default!;

    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [DataType(DataType.Password)]
    [DisableAuditing]
    public string Password { get; set; } = default!;

    public string? CaptchaCode { get; set; }

    public bool RememberMe { get; set; }
}

