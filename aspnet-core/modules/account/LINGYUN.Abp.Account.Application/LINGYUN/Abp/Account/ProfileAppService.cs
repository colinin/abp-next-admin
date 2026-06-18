using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Account;

[Dependency(ReplaceServices = true)]
public class ProfileAppService : Volo.Abp.Account.ProfileAppService
{
    public ProfileAppService(
        IdentityUserManager userManager, 
        IOptions<IdentityOptions> identityOptions) 
        : base(userManager, identityOptions)
    {
    }

    public async override Task ChangePasswordAsync(ChangePasswordInput input)
    {
        await IdentityOptions.SetAsync();

        var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());

        if (currentUser.IsExternal)
        {
            throw new BusinessException(code: IdentityErrorCodes.ExternalUserPasswordChange);
        }

        if (currentUser.PasswordHash.IsNullOrWhiteSpace())
        {
            (await UserManager.AddPasswordAsync(currentUser, input.NewPassword)).CheckErrors();
        }
        else
        {
            (await UserManager.ChangePasswordAsync(currentUser, input.CurrentPassword, input.NewPassword)).CheckErrors();
        }

        if (await SettingProvider.IsTrueAsync(IdentitySettingNames.Password.EnablePreventPasswordReuse))
        {
            var preventPasswordReuseCount = await SettingProvider.GetAsync(IdentitySettingNames.Password.PreventPasswordReuseCount, 6);
            currentUser.AddPasswordHistory(input.NewPassword);
            if (currentUser.PasswordHistories.Count > preventPasswordReuseCount)
            {
                var excessCount = currentUser.PasswordHistories.Count - preventPasswordReuseCount;
                var oldestHistories = currentUser.PasswordHistories
                    .OrderBy(x => x.CreatedAt)
                    .Take(excessCount)
                    .ToList();

                foreach (var history in oldestHistories)
                {
                    currentUser.PasswordHistories.Remove(history);
                }
            }
        }
    }
}
