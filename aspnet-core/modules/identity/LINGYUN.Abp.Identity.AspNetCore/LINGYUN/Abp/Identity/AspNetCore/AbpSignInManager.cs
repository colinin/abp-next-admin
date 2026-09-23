using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Roles;
using Volo.Abp.Settings;

using VoloAbpSignInManager = Volo.Abp.Identity.AspNetCore.AbpSignInManager;

namespace LINGYUN.Abp.Identity.AspNetCore;

public class AbpSignInManager : VoloAbpSignInManager
{
    protected IdentityTwoFactorManager IdentityTwoFactorManager { get; }
    public AbpSignInManager(
        IdentityUserManager userManager, 
        IHttpContextAccessor contextAccessor, 
        IUserClaimsPrincipalFactory<Volo.Abp.Identity.IdentityUser> claimsFactory, 
        IOptions<IdentityOptions> optionsAccessor, 
        ILogger<SignInManager<Volo.Abp.Identity.IdentityUser>> logger, 
        IAuthenticationSchemeProvider schemes, 
        IUserConfirmation<Volo.Abp.Identity.IdentityUser> confirmation, 
        IOptions<AbpIdentityOptions> options, 
        ISettingProvider settingProvider,
        ICurrentTenant currentTenant,
        IdentityTwoFactorManager identityTwoFactorManager) 
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation, options, settingProvider, currentTenant)
    {
        IdentityTwoFactorManager = identityTwoFactorManager;
    }

    public async override Task<bool> IsTwoFactorEnabledAsync(Volo.Abp.Identity.IdentityUser user)
    {
        if (await IdentityTwoFactorManager.IsForcedEnableAsync())
        {
            return true;
        }

        var isInAdminRole = await UserManager.IsInRoleAsync(user, AbpRoleConsts.AdminRoleName);
        if (await IdentityTwoFactorManager.GetTwoFactorEnabledAsync(user, isInAdminRole))
        {
            return true;
        }

        return await base.IsTwoFactorEnabledAsync(user);
    }
}
