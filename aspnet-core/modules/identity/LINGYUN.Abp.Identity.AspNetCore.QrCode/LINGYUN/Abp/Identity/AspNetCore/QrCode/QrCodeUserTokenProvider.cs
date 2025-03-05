using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LINGYUN.Abp.Identity.QrCode;

public class QrCodeUserTokenProvider : DataProtectorTokenProvider<IdentityUser>
{
    public static string ProviderName => QrCodeLoginProviderConsts.Name;
    public QrCodeUserTokenProvider(
        IDataProtectionProvider dataProtectionProvider, 
        IOptions<DataProtectionTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<IdentityUser>> logger)
        : base(dataProtectionProvider, options, logger)
    {
    }
}
