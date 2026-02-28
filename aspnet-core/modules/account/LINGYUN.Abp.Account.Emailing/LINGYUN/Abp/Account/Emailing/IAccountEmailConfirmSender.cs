using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Emailing;

[Obsolete("This interface has been deprecated. Please use LINGYUN.Abp.Account.Security.IAccountEmailSecurityCodeSender.")]
public interface IAccountEmailConfirmSender
{
    Task SendEmailConfirmLinkAsync(
        Guid userId,
        string userEmail,
        string confirmToken,
        string appName,
        string returnUrl = null,
        string returnUrlHash = null,
        Guid? userTenantId = null
    );
}
