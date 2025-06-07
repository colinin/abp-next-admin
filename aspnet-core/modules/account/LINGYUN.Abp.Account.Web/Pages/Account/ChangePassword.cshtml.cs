using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Account.Web.Pages.Account;

public class UserInfoModel : IMultiTenant
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }
}

public class ChangePasswordInputModel
{
    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [Display(Name = "DisplayName:CurrentPassword")]
    [DataType(DataType.Password)]
    [DisableAuditing]
    public string CurrentPassword { get; set; }

    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [Display(Name = "DisplayName:NewPassword")]
    [DataType(DataType.Password)]
    [DisableAuditing]
    public string NewPassword { get; set; }

    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [Display(Name = "DisplayName:NewPasswordConfirm")]
    [DataType(DataType.Password)]
    [DisableAuditing]
    public string NewPasswordConfirm { get; set; }
}

public class ChangePasswordModel : AccountPageModel
{
    [BindProperty]
    public UserInfoModel UserInfo { get; set; }

    [BindProperty]
    public ChangePasswordInputModel Input { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool RememberMe { get; set; }

    public bool HideOldPasswordInput { get; set; }

    public AbpSignInManager AbpSignInManager => LazyServiceProvider.LazyGetRequiredService<AbpSignInManager>();
    public IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache => LazyServiceProvider.LazyGetRequiredService<IdentityDynamicClaimsPrincipalContributorCache>();

    public async virtual Task<IActionResult> OnGetAsync()
    {
        Input = new ChangePasswordInputModel();
        UserInfo = await GetCurrentUser();

        if (UserInfo == null || UserInfo.TenantId != CurrentTenant.Id)
        {
            await HttpContext.SignOutAsync(AbpAccountAuthenticationTypes.ShouldChangePassword);
            return RedirectToPage("/Login", new { ReturnUrl, ReturnUrlHash });
        }

        HideOldPasswordInput = (await UserManager.GetByIdAsync(UserInfo.Id)).PasswordHash == null;
        return Page();
    }

    public async virtual Task<IActionResult> OnPostAsync()
    {
        if (Input.CurrentPassword == Input.NewPassword)
        {
            Alerts.Warning(L["NewPasswordSameAsOld"]);
            return Page();
        }
        if (Input.NewPassword != Input.NewPasswordConfirm)
        {
            Alerts.Warning(L["NewPasswordConfirmFailed"]);
            return Page();
        }

        var userInfo = await GetCurrentUser();
        if (userInfo != null)
        {
            if (userInfo.TenantId == CurrentTenant.Id)
            {
                try
                {
                    await IdentityOptions.SetAsync();
                    var user = await UserManager.GetByIdAsync(userInfo.Id);
                    if (user.PasswordHash == null)
                    {
                        (await UserManager.AddPasswordAsync(user, Input.NewPassword)).CheckErrors();
                    }
                    else
                    {
                        (await UserManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword)).CheckErrors();
                    }

                    await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                    {
                        Identity = IdentitySecurityLogIdentityConsts.Identity,
                        Action = IdentitySecurityLogActionConsts.ChangePassword
                    });
                    user.SetShouldChangePasswordOnNextLogin(false);
                    (await UserManager.UpdateAsync(user)).CheckErrors();

                    await HttpContext.SignOutAsync(AbpAccountAuthenticationTypes.ShouldChangePassword);

                    await SignInManager.SignInAsync(user, RememberMe);

                    await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                    {
                        Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                        Action = IdentitySecurityLogActionConsts.LoginSucceeded,
                        UserName = user.UserName
                    });
                    await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
                    return await RedirectSafelyAsync(ReturnUrl, ReturnUrlHash);
                }
                catch (Exception ex)
                {
                    Alerts.Warning(GetLocalizeExceptionMessage(ex));
                    return Page();
                }
            }
        }

        await HttpContext.SignOutAsync(AbpAccountAuthenticationTypes.ShouldChangePassword);

        return RedirectToPage("/Login", new { ReturnUrl, ReturnUrlHash });
    }

    protected async virtual Task<UserInfoModel> GetCurrentUser()
    {
        var result = await HttpContext.AuthenticateAsync(AbpAccountAuthenticationTypes.ShouldChangePassword);

        var userId = result?.Principal?.FindUserId();
        if (!userId.HasValue)
        {
            return null;
        }

        var tenantId = result.Principal.FindTenantId();
        using (CurrentTenant.Change(tenantId, null))
        {
            var identityUser = await UserManager.FindByIdAsync(userId.Value.ToString());
            return identityUser == null
                ? null
                : new UserInfoModel()
                {
                    Id = identityUser.Id,
                    TenantId = identityUser.TenantId
                };
        }
    }
}
