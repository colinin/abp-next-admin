using LINGYUN.Abp.Account.Dto;
using LINGYUN.Abp.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;

using VoloIdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LINGYUN.Abp.Account.Web.Pages.Account;

public class UseRecoveryCodesModel : AccountPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrlHash { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public bool RememberMe { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid? LinkUserId { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid? LinkTenantId { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? LinkToken { get; set; }

    [BindProperty]
    public RestoreRecoveryCodesInput Input { get; set; } = new();

    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }
    protected IIdentityLinkUserAppService IdentityLinkUserAppService { get; }
    protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache { get; }

    public UseRecoveryCodesModel(
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IIdentityLinkUserAppService identityLinkUserAppService,
        IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache)
    {
        CurrentPrincipalAccessor = currentPrincipalAccessor;
        IdentityLinkUserAppService = identityLinkUserAppService;
        IdentityDynamicClaimsPrincipalContributorCache = identityDynamicClaimsPrincipalContributorCache;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            Alerts.Warning(L["TwoFactorAuthenticationInvaidUser"]);
            return Page();
        }

        var result = await SignInManager.TwoFactorRecoveryCodeSignInAsync(Input.RecoveryCode);

        if (!result.Succeeded)
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogExtendActionConsts.LoginRecoveryCodeFailed,
                UserName = user.UserName
            });
            Alerts.Danger(L["InvalidRecoveryCode"]);
            return Page();
        }

        if (Input.RememberBrowser)
        {
            await SignInManager.RememberTwoFactorClientAsync(user);
        }

        if (await VerifyLinkTokenAsync())
        {
            await HandleLinkUserLogin(user);
        }

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogExtendActionConsts.LoginRecoveryCodeSucceeded,
            UserName = user.UserName
        });

        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

        return await RedirectSafelyAsync(ReturnUrl!, ReturnUrlHash);
    }

    #region LinkUser
    protected async virtual Task HandleLinkUserLogin(VoloIdentityUser user)
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

    public class RestoreRecoveryCodesInput
    {
        [Required]
        [DataType(DataType.Text)]
        public string RecoveryCode { get; set; } = default!;

        public bool RememberBrowser { get; set; }
    }
}