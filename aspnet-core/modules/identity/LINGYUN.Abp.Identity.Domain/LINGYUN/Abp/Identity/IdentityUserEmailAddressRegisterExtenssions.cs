using Volo.Abp.Data;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity;

public static class IdentityUserEmailAddressRegisterExtenssions
{
    public static bool IsEmailAddressRegisterUser(this IdentityUser user)
    {
        return user.GetProperty(UserTwoFactorTokenProviderConsts.EmailAddressRegisterTokenProvider, false);
    }

    public static void SetEmailAddressRegisterUser(this IdentityUser user)
    {
        user.SetProperty(UserTwoFactorTokenProviderConsts.EmailAddressRegisterTokenProvider, true);
    }
}
