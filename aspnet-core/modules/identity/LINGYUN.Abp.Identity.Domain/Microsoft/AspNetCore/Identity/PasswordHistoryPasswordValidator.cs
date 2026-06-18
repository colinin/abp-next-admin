using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Microsoft.AspNetCore.Identity;

public class PasswordHistoryPasswordValidator : IPasswordValidator<IdentityUser>
{
    public async virtual Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user, string password)
    {
        var settingProvider = manager.ServiceProvider.GetRequiredService<ISettingProvider>();
        if (await settingProvider.IsTrueAsync(IdentitySettingNames.Password.EnablePreventPasswordReuse))
        {
            var preventPasswordReuseCount = await settingProvider.GetAsync(IdentitySettingNames.Password.PreventPasswordReuseCount, 6);
            if (preventPasswordReuseCount > 0 && user.PasswordHistories.Any(x => x.Password == password))
            {
                var localizer = manager.ServiceProvider.GetRequiredService<IStringLocalizer<IdentityResource>>();

                return IdentityResult.Failed(new IdentityError
                {
                    Code = LINGYUN.Abp.Identity.IdentityErrorCodes.PasswordInHistoryInValid,
                    Description = localizer["Volo.Abp.Identity:PasswordInHistory", preventPasswordReuseCount]
                });
            }
        }
        return IdentityResult.Success;
    }
}
