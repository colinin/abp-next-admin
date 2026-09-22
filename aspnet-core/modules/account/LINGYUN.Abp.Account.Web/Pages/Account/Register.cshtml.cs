using LINGYUN.Abp.Account.Dto;
using LINGYUN.Abp.Account.Web.Captcha;
using LINGYUN.Abp.Account.Web.ExternalProviders;
using LINGYUN.Abp.Account.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Reflection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using IAbpAccountAppService = Volo.Abp.Account.IAccountAppService;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using ILAbpAccountAppService = LINGYUN.Abp.Account.IAccountAppService;

namespace LINGYUN.Abp.Account.Web.Pages.Account;

public class RegisterModel : AccountPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrlHash { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public int SendEmailVerifyCodeInternal { get; set; }

    [BindProperty]
    public PostInput Input { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public bool IsExternalLogin { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ExternalLoginAuthSchema { get; set; }

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

    public bool EnableCaptcha { get; set; }
    public CaptchaComponent CaptchaComponent { get; private set; } = default!;
    public ICaptchaComponentProvider CaptchaComponentProvider => LazyServiceProvider.LazyGetRequiredService<ICaptchaComponentProvider>();

    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor => LazyServiceProvider.LazyGetRequiredService<ICurrentPrincipalAccessor>();

    public IIdentityLinkUserAppService IdentityLinkUserAppService => LazyServiceProvider.LazyGetRequiredService<IIdentityLinkUserAppService>();

    #endregion

    public IEnumerable<ExternalLoginProviderModel> ExternalProviders { get; set; } = default!;
    public IEnumerable<ExternalLoginProviderModel> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));
    public bool EnableLocalRegister { get; set; }
    public bool RequireEmailVerificationToRegister { get; set; }
    public bool IsExternalLoginOnly => EnableLocalRegister == false && ExternalProviders?.Count() == 1;
    public string? ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

    protected IExternalProviderService ExternalProviderService { get; }
    protected IAuthenticationSchemeProvider SchemeProvider { get; }
    protected ILAbpAccountAppService LAbpAccountAppService { get; }

    protected AbpAccountOptions AccountOptions { get; }
    protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache { get; }

    public RegisterModel(
        IExternalProviderService externalProviderService,
        IAbpAccountAppService abpAccountAppService,
        ILAbpAccountAppService lAbpAccountAppService,
        IAuthenticationSchemeProvider schemeProvider,
        IOptions<AbpAccountOptions> accountOptions,
        IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache)
    {
        ExternalProviderService = externalProviderService;
        SchemeProvider = schemeProvider;
        IdentityDynamicClaimsPrincipalContributorCache = identityDynamicClaimsPrincipalContributorCache;
        AccountAppService = abpAccountAppService;
        LAbpAccountAppService = lAbpAccountAppService;
        AccountOptions = accountOptions.Value;
    }

    protected async virtual Task InitCaptchaComponent()
    {
        EnableCaptcha = await SettingProvider.IsTrueAsync(Identity.Settings.IdentitySettingNames.SignIn.RequireCaptchaVerification);
        CaptchaComponent = await CaptchaComponentProvider.GetComponentOrDefaultAsync();
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        await InitCaptchaComponent();
        ExternalProviders = await GetExternalProviders();
        RequireEmailVerificationToRegister = await SettingProvider.IsTrueAsync(IdentitySettingNames.SignIn.RequireEmailVerificationToRegister);
        if (RequireEmailVerificationToRegister)
        {
            SendEmailVerifyCodeInternal = await SettingProvider.GetAsync(Identity.Settings.IdentitySettingNames.User.EmailRegisterRepetInterval, 1);
        }

        if (!await CheckSelfRegistrationAsync())
        {
            if (IsExternalLoginOnly)
            {
                return await OnPostExternalLogin(ExternalLoginScheme!);
            }

            Alerts.Warning(L["SelfRegistrationDisabledMessage"]);
        }

        await TrySetEmailAsync();

        return Page();
    }

    protected virtual async Task TrySetEmailAsync()
    {
        if (IsExternalLogin)
        {
            var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return;
            }

            if (!externalLoginInfo.Principal.Identities.Any())
            {
                return;
            }

            var identity = externalLoginInfo.Principal.Identities.First();
            var emailClaim = identity.FindFirst(AbpClaimTypes.Email) ?? identity.FindFirst(ClaimTypes.Email);

            if (emailClaim == null)
            {
                return;
            }

            var userName = await UserManager.GetUserNameFromEmailAsync(emailClaim.Value);
            Input = new PostInput { UserName = userName, EmailAddress = emailClaim.Value };
        }
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await InitCaptchaComponent();
            ExternalProviders = await GetExternalProviders();

            if (!await CheckSelfRegistrationAsync())
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }

            if (IsExternalLogin)
            {
                var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
                if (externalLoginInfo == null)
                {
                    Logger.LogWarning("External login info is not available");
                    return RedirectToPage("./Login");
                }
                if (Input.UserName.IsNullOrWhiteSpace())
                {
                    Input.UserName = await UserManager.GetUserNameFromEmailAsync(Input.EmailAddress);
                }
                await RegisterExternalUserAsync(externalLoginInfo, Input.UserName, Input.EmailAddress);
            }
            else
            {
                if (EnableCaptcha)
                {
                    var isValid = await CaptchaComponent.ValidateAsync(
                        new CaptchaValidatorContext(
                            LazyServiceProvider,
                            Input.CaptchaCode!,
                            Input.UserName)
                    );
                    if (!isValid)
                    {
                        Alerts.Danger(L["InvalidVerifyCode"]);
                        return Page();
                    }
                }

                RequireEmailVerificationToRegister = await SettingProvider.IsTrueAsync(IdentitySettingNames.SignIn.RequireEmailVerificationToRegister);

                if (RequireEmailVerificationToRegister)
                {
                    if (Input.VerifyCode.IsNullOrWhiteSpace())
                    {
                        Alerts.Danger(L["EmailVerifyCodeIsRequired"]);
                        return Page();
                    }
                    var isVerifyCodeValid = await LAbpAccountAppService.VerifyEmailRegisterCodeAsync(new VerifyEmailRegisterCodeInput
                    {
                        EmailAddress = Input.EmailAddress,
                        VerifyCode = Input.VerifyCode,
                    });
                    if (!isVerifyCodeValid)
                    {
                        Alerts.Danger(L["InvalidVerifyCode"]);
                        return Page();
                    }
                }

                var user = await RegisterLocalUserAsync();

                if (RequireEmailVerificationToRegister)
                {
                    var emailConfirmationToken = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    await UserManager.ConfirmEmailAsync(user, emailConfirmationToken);
                }

                if (await VerifyLinkTokenAsync())
                {
                    await HandleLinkUserLogin(user);
                }

                if (await UserManager.GetTwoFactorEnabledAsync(user))
                {
                    var result = await SignInManager.PasswordSignInAsync(
                        Input.UserName,
                        Input.Password,
                        false,
                        true
                    );

                    if (result.Succeeded)
                    {
                        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
                        return Redirect(ReturnUrl ?? "~/");
                    }

                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("SendCode", new
                        {
                            returnUrl = ReturnUrl,
                            returnUrlHash = ReturnUrlHash,
                            linkUserId = LinkUserId,
                            linkTenantId = LinkTenantId,
                            linkToken = LinkToken,
                        });
                    }

                    return RedirectToPage("Login", new
                    {
                        returnUrl = ReturnUrl,
                        returnUrlHash = ReturnUrlHash,
                        linkUserId = LinkUserId,
                        linkTenantId = LinkTenantId,
                        linkToken = LinkToken,
                    });
                }

                await SignInManager.SignInAsync(user, isPersistent: true);

                // Clear the dynamic claims cache.
                await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
            }

            return Redirect(ReturnUrl ?? "~/");
        }
        catch (BusinessException e)
        {
            Alerts.Danger(GetLocalizeExceptionMessage(e));
            return Page();
        }
    }

    protected virtual async Task<IdentityUser> RegisterLocalUserAsync()
    {
        ValidateModel();

        var userDto = await AccountAppService.RegisterAsync(
            new RegisterDto
            {
                AppName = "MVC",
                EmailAddress = Input.EmailAddress,
                Password = Input.Password,
                UserName = Input.UserName
            }
        );

        return await UserManager.GetByIdAsync(userDto.Id);
    }

    protected virtual async Task RegisterExternalUserAsync(ExternalLoginInfo externalLoginInfo, string userName, string emailAddress)
    {
        await IdentityOptions.SetAsync();

        var user = new IdentityUser(GuidGenerator.Create(), userName, emailAddress, CurrentTenant.Id);

        (await UserManager.CreateAsync(user)).CheckErrors();
        (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();

        var userLoginAlreadyExists = user.Logins.Any(x =>
            x.TenantId == user.TenantId &&
            x.LoginProvider == externalLoginInfo.LoginProvider &&
            x.ProviderKey == externalLoginInfo.ProviderKey);

        if (!userLoginAlreadyExists)
        {
            (await UserManager.AddLoginAsync(user, new UserLoginInfo(
                externalLoginInfo.LoginProvider,
                externalLoginInfo.ProviderKey,
                externalLoginInfo.ProviderDisplayName
            ))).CheckErrors();
        }

        await SignInManager.SignInAsync(user, isPersistent: true, ExternalLoginAuthSchema);

        // Clear the dynamic claims cache.
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
    }

    protected virtual async Task<bool> CheckSelfRegistrationAsync()
    {
        EnableLocalRegister = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin) &&
                              await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled);

        if (IsExternalLogin)
        {
            return true;
        }

        if (!EnableLocalRegister)
        {
            return false;
        }

        return true;
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

    protected virtual async Task<IActionResult> OnPostExternalLogin(string provider)
    {
        var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash });
        var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        properties.Items["scheme"] = provider;

        return await Task.FromResult(Challenge(properties, provider));
    }

    #region LinkUser
    protected async virtual Task HandleLinkUserLogin(IdentityUser user)
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
                var targetUser = await UserManager.GetByIdAsync(LinkUserId.Value);
                using (CurrentPrincipalAccessor.Change(await SignInManager.CreateUserPrincipalAsync(targetUser)))
                {
                    await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                    {
                        Identity = IdentitySecurityLogIdentityConsts.Identity,
                        UserName = targetUser.UserName,
                        Action = "LinkUser",
                        ExtraProperties =
                        {
                            { "LinkTenantId",  LinkTenantId },
                            { "LinkUserId", LinkUserId }
                        }
                    });
                }
            }
        }
    }

    protected virtual async Task<bool> VerifyLinkTokenAsync()
    {
        return !LinkToken.IsNullOrWhiteSpace() && LinkUserId != null
            && await IdentityLinkUserAppService.VerifyLinkTokenAsync(new VerifyLinkTokenInput
            {
                UserId = LinkUserId.Value,
                TenantId = LinkTenantId,
                Token = LinkToken
            });
    }
    #endregion

    public class PostInput
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName { get; set; } = default!;

        [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string EmailAddress { get; set; } = default!;

        [StringLength(10)]
        public string? VerifyCode { get; set; }

        public string? CaptchaCode { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string Password { get; set; } = default!;
    }
}

