using Volo.Abp.Data;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity;

public static class IdentityUserPhoneNumberRegisterExtenssions
{
    public static bool IsPhoneNumberRegisterUser(this IdentityUser user)
    {
        return user.GetProperty(UserTwoFactorTokenProviderConsts.PhoneNumberRegisterTokenProvider, false);
    }

    public static void SetPhoneNumberRegisterUser(this IdentityUser user)
    {
        user.SetProperty(UserTwoFactorTokenProviderConsts.PhoneNumberRegisterTokenProvider, true);
    }
}
