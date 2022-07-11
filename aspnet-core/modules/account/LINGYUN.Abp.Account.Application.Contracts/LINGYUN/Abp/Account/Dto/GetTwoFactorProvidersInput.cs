using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Account;

public class GetTwoFactorProvidersInput
{
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
    public string UserName { get; set; }
}
