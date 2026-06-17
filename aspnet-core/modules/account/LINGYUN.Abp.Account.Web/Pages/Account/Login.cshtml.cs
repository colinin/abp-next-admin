using LINGYUN.Abp.Account.Dto;
using LINGYUN.Abp.Account.Web.ExternalProviders;
using LINGYUN.Abp.Account.Web.Models;
using LINGYUN.Abp.Identity.QrCode;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
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
    public string ReturnUrl { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public LoginType LoginType { get; set; }

    [BindProperty(Name = "PasswordLoginInput")]
    public PasswordLoginInputModel PasswordLoginInput { get; set; }

    [BindProperty(Name = "PhoneLoginInput")]
    public PhoneLoginInputModel PhoneLoginInput { get; set; }

    [BindProperty(Name = "QrCodeLoginInput")]
    public QrCodeLoginInputModel QrCodeLoginInput { get; set; }

    public bool EnableLocalLogin { get; set; }

    public bool ShowCancelButton { get; set; }
    public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
    public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

    public IEnumerable<ExternalLoginProviderModel> ExternalProviders { get; set; }
    public IEnumerable<ExternalLoginProviderModel> VisibleExternalProviders => ExternalProviders.Where(x => !x.DisplayName.IsNullOrWhiteSpace());

    protected IIdentityUserRepository UserRepository => LazyServiceProvider.LazyGetRequiredService<IIdentityUserRepository>();
    protected IQrCodeLoginProvider QrCodeLoginProvider => LazyServiceProvider.LazyGetRequiredService<IQrCodeLoginProvider>();
    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor => LazyServiceProvider.LazyGetRequiredService<ICurrentPrincipalAccessor>();
    protected IHttpUserAgentParserProvider HttpUserAgentParserProvider => LazyServiceProvider.LazyGetRequiredService<IHttpUserAgentParserProvider>();

    #region LinkUser

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid? LinkUserId { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid? LinkTenantId { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string LinkToken { get; set; }

    public bool IsLinkLogin { get; set; }

    public IIdentityLinkUserAppService IdentityLinkUserAppService => LazyServiceProvider.LazyGetRequiredService<IIdentityLinkUserAppService>();

    #endregion

    protected IExternalProviderService ExternalProviderService { get; }
    protected IAuthenticationSchemeProvider SchemeProvider { get; }
    protected AbpAccountOptions AccountOptions { get; }
    protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache { get; }
    public LoginModel(
        IExternalProviderService externalProviderService,
        IAuthenticationSchemeProvider schemeProvider,
        IOptions<AbpAccountOptions> accountOptions,
        IOptions<IdentityOptions> identityOptions,
        IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache) 
    {
        ExternalProviderService = externalProviderService;
        SchemeProvider = schemeProvider;
        IdentityOptions = identityOptions;
        AccountOptions = accountOptions.Value;
        IdentityDynamicClaimsPrincipalContributorCache = identityDynamicClaimsPrincipalContributorCache;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        LoginType = LoginType.Password;
        PhoneLoginInput = new PhoneLoginInputModel();
        QrCodeLoginInput = new QrCodeLoginInputModel();
        PasswordLoginInput = new PasswordLoginInputModel();

        AllowQrCodeLoginIfNotMobileDevice();

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

    public async virtual Task<IActionResult> OnPostPasswordLogin(string action)
    {
        LoginType = LoginType.Password;

        await CheckLocalLoginAsync();

        ExternalProviders = await GetExternalProviders();

        EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

        ModelState.RemoveModelErrors(nameof(PhoneLoginInput));
        ModelState.RemoveModelErrors(nameof(QrCodeLoginInput));
        if (!TryValidateModel(PasswordLoginInput, nameof(PasswordLoginInput)))
        {
            return Page();
        }

        await ReplaceEmailToUsernameOfInputIfNeeds();

        await IdentityOptions.SetAsync();

        IsLinkLogin = await VerifyLinkTokenAsync();

        var result = await SignInManager.PasswordSignInAsync(
            PasswordLoginInput.UserNameOrEmailAddress,
            PasswordLoginInput.Password,
            PasswordLoginInput.RememberMe,
            true
        );

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = result.ToIdentitySecurityLogAction(),
            UserName = PasswordLoginInput.UserNameOrEmailAddress
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
        var user = await GetIdentityUserAsync(PasswordLoginInput.UserNameOrEmailAddress);

        Debug.Assert(user != null, nameof(user) + " != null");

        if (IsLinkLogin)
        {
            return await HandleLinkUserLogin(user);
        }

        // Clear the dynamic claims cache.
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

        return await RedirectSafelyAsync(ReturnUrl, ReturnUrlHash);
    }

    public async virtual Task<IActionResult> OnPostPhoneNumberLogin(string action)
    {
        LoginType = LoginType.PhoneNumber;

        await CheckLocalLoginAsync();

        ExternalProviders = await GetExternalProviders();

        EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

        ModelState.RemoveModelErrors(nameof(QrCodeLoginInput));
        ModelState.RemoveModelErrors(nameof(PasswordLoginInput));
        if (!TryValidateModel(PhoneLoginInput, nameof(PhoneLoginInput)))
        {
            return Page();
        }

        var user = await UserRepository.FindByPhoneNumberAsync(PhoneLoginInput.PhoneNumber);
        if (user == null)
        {
            Logger.LogInformation("the user phone number is not registed!");
            Alerts.Danger(L["InvalidPhoneNumber"]);
            return Page();
        }

        var result = await UserManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, PhoneLoginInput.Code);
        if (!result)
        {
            Alerts.Danger(L["InvalidVerifyCode"]);
            return Page();
        }

        if (IsLinkLogin)
        {
            return await HandleLinkUserLogin(user);
        }

        await SignInManager.SignInAsync(user, PhoneLoginInput.RememberMe);

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.IdentityTwoFactor,
            Action = IdentitySecurityLogActionConsts.LoginSucceeded,
            UserName = user.UserName
        });

        // Clear the dynamic claims cache.
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

        return await RedirectSafelyAsync(ReturnUrl, ReturnUrlHash);
    }

    protected virtual void SetTenantCookies(Guid? tenantId = null)
    {
        if (tenantId.HasValue)
        {
            Response.Cookies.Append(
               "__tenant",
               tenantId.ToString(),
               new CookieOptions
               {
                   Path = "/",
                   HttpOnly = false,
                   IsEssential = true,
                   Expires = DateTimeOffset.Now.AddYears(10)
               }
           );
        }
        else
        {
            Response.Cookies.Delete("__tenant");
        }
    }

    public async virtual Task<IActionResult> OnPostQrCodeLogin(string action)
    {
        LoginType = LoginType.QrCode;

        await CheckLocalLoginAsync();

        ExternalProviders = await GetExternalProviders();

        EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

        ModelState.RemoveModelErrors(nameof(PhoneLoginInput));
        ModelState.RemoveModelErrors(nameof(PasswordLoginInput));
        if (!TryValidateModel(QrCodeLoginInput, nameof(QrCodeLoginInput)))
        {
            return Page();
        }

        var qrCodeInfo = await QrCodeLoginProvider.GetCodeAsync(QrCodeLoginInput.Key);
        // 二维码扫描后用户Id不为空
        if (qrCodeInfo == null || qrCodeInfo.Token.IsNullOrWhiteSpace() == true)
        {
            Alerts.Danger(L["QrCode:Invalid"]);
            return Page();
        }

        SetTenantCookies(qrCodeInfo.TenantId);
        using (CurrentTenant.Change(qrCodeInfo.TenantId))
        {
            var user = await UserManager.FindByIdAsync(qrCodeInfo.UserId);
            if (user == null)
            {
                // TODO: 用户验证无效?
                Alerts.Danger(L["QrCode:Invalid"]);
                return Page();
            }

            if (!await UserManager.VerifyUserTokenAsync(user, QrCodeLoginProviderConsts.Name, QrCodeLoginProviderConsts.Purpose, qrCodeInfo.Token))
            {
                Alerts.Danger(L["QrCode:Invalid"]);
                return Page();
            }

            if (IsLinkLogin)
            {
                return await HandleLinkUserLogin(user);
            }

            // TODO: 记住登录
            await SignInManager.SignInAsync(user, true);

            await QrCodeLoginProvider.RemoveAsync(QrCodeLoginInput.Key);

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = QrCodeLoginProviderConsts.Purpose,
                Action = IdentitySecurityLogActionConsts.LoginSucceeded,
                UserName = user.UserName
            });

            // Clear the dynamic claims cache.
            await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

            return await RedirectSafelyAsync(ReturnUrl, ReturnUrlHash);
        }
    }

    public virtual async Task<IActionResult> OnPostExternalLogin(string provider)
    {
        var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash });
        var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        properties.Items["scheme"] = provider;

        return await Task.FromResult(Challenge(properties, provider));
    }

    public virtual async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = "", string returnUrlHash = "", string remoteError = null)
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

        IdentityUser user;
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
            rememberMe = PasswordLoginInput.RememberMe,
            linkUserId = LinkUserId,
            linkTenantId = LinkTenantId,
            linkToken = LinkToken,
        }));
    }


    protected virtual async Task<IdentityUser> GetIdentityUserAsync(string userNameOrEmailAddress)
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
                    Name = externalLoginProvider.Name,
                    AuthenticationScheme = scheme.Name,
                    DisplayName = externalLoginProvider.DisplayName,
                    ComponentType = externalLoginProvider.ComponentType,
                });
            }
        }

        return externalProviderModels;
    }

    protected virtual bool TryGetExternalLoginProvider(AuthenticationScheme scheme, List<ExternalLoginProviderModel> externalProviders, out ExternalLoginProviderModel externalLoginProvider)
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
        if (!ValidationHelper.IsValidEmailAddress(PasswordLoginInput.UserNameOrEmailAddress))
        {
            return;
        }

        var userByUsername = await UserManager.FindByNameAsync(PasswordLoginInput.UserNameOrEmailAddress);
        if (userByUsername != null)
        {
            return;
        }

        var userByEmail = await UserManager.FindByEmailAsync(PasswordLoginInput.UserNameOrEmailAddress);
        if (userByEmail == null)
        {
            return;
        }

        PasswordLoginInput.UserNameOrEmailAddress = userByEmail.UserName;
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

    protected async virtual Task<IActionResult> HandleUserNotAllowed()
    {
        var notAllowedUser = await GetIdentityUserAsync(PasswordLoginInput.UserNameOrEmailAddress);
        if (await UserManager.CheckPasswordAsync(notAllowedUser, PasswordLoginInput.Password))
        {
            // 用户必须修改密码
            if (notAllowedUser.ShouldChangePasswordOnNextLogin || await UserManager.ShouldPeriodicallyChangePasswordAsync(notAllowedUser))
            {
                await StoreChangePasswordUserAsync(notAllowedUser);

                return RedirectToPage("ChangePassword", new
                {
                    returnUrl = ReturnUrl,
                    returnUrlHash = ReturnUrlHash,
                    rememberMe = PasswordLoginInput.RememberMe,
                });
            }
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
            changePwdIdentity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString()));
        }

        await HttpContext.SignInAsync(AbpAccountAuthenticationTypes.ShouldChangePassword, new ClaimsPrincipal(changePwdIdentity));
    }

    protected virtual async Task StoreConfirmUserAsync(IdentityUser user)
    {
        var identity = new ClaimsIdentity(AbpAccountAuthenticationTypes.ConfirmUserScheme);
        identity.AddClaim(new Claim(AbpClaimTypes.UserId, user.Id.ToString()));

        if (user.TenantId.HasValue)
        {
            identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString()));
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
            var userAgentInfo = HttpUserAgentParserProvider.Parse(HttpContext.Request.Headers.UserAgent);
            if (userAgentInfo.MobileDeviceType.IsNullOrWhiteSpace())
            {
                QrCodeLoginInput.IsEnabled = true;
            }
        }
    }

    #region LinkUser

    protected virtual async Task<bool> VerifyLinkTokenAsync()
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
                UserId = LinkUserId.Value,
                TenantId = LinkTenantId,
                Token = LinkToken
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

public abstract class LoginInputModel
{
}

public class PhoneLoginInputModel : LoginInputModel
{
    [Phone]
    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
    public string PhoneNumber { get; set; }

    [Required]
    [StringLength(6)]
    public string Code { get; set; }

    public bool RememberMe { get; set; }
}

public class PasswordLoginInputModel : LoginInputModel
{
    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    public string UserNameOrEmailAddress { get; set; }

    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [DataType(DataType.Password)]
    [DisableAuditing]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}

public class QrCodeLoginInputModel : LoginInputModel
{
    [HiddenInput]
    public string Key { get; set; }

    [HiddenInput]
    public bool IsEnabled { get; set; }
}

public enum LoginType
{
    Password = 0,
    PhoneNumber = 1,
    QrCode = 2
}
