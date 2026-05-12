using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;

namespace Microsoft.AspNetCore.Identity;

public static class IdentityUserManagerExtensions
{
    public static async Task<IdentityResult> SetTwoFactorEnabledWithAccountConfirmedAsync<TUser>(
        [NotNull] this UserManager<TUser> userManager, 
        [NotNull] TUser user, 
        bool enabled)
        where TUser : IdentityUser
    {
        Check.NotNull(userManager, nameof(userManager));
        Check.NotNull(user, nameof(user));
        
        if (enabled)
        {
            var hasAuthenticatorEnabled = user.GetProperty(userManager.Options.Tokens.AuthenticatorTokenProvider, false);
            var phoneNumberConfirmed = await userManager.IsPhoneNumberConfirmedAsync(user);
            var emailAddressConfirmed = await userManager.IsEmailConfirmedAsync(user);
            // 如果其中一个安全选项未确认,无法启用双因素验证
            if (!hasAuthenticatorEnabled && !phoneNumberConfirmed && !emailAddressConfirmed)
            {
                var localizer = userManager.ServiceProvider.GetRequiredService<IStringLocalizer<IdentityResource>>();

                //throw new IdentityException(
                //    LINGYUN.Abp.Identity.IdentityErrorCodes.ChangeTwoFactorWithMFANotBound,
                //    details: phoneNumberConfirmed ? "phone number not confirmed" : "email address not confirmed");

                return IdentityResult.Failed(new IdentityError
                {
                    Code = LINGYUN.Abp.Identity.IdentityErrorCodes.ChangeTwoFactorWithMFANotBound,
                    Description = localizer[LINGYUN.Abp.Identity.IdentityErrorCodes.ChangeTwoFactorWithMFANotBound]
                });
            }
        }

        return await userManager.SetTwoFactorEnabledAsync(user, enabled);
    }
}
