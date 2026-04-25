using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LINGYUN.Abp.Identity.AspNetCore;

public class AbpPhoneNumberRegisterTokenProvider : TotpSecurityStampBasedTokenProvider<IdentityUser>
{
    public const string ProviderName = UserTwoFactorTokenProviderConsts.PhoneNumberRegisterTokenProvider;

    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<IdentityUser> manager, IdentityUser user)
    {
        return Task.FromResult(user.IsPhoneNumberRegisterUser());
    }
}
