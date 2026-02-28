using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Emailing;

[Obsolete("This interface has been deprecated. Please use LINGYUN.Abp.Account.Security.IAccountEmailSecurityCodeSender.")]
public interface IAccountEmailVerifySender
{
    Task SendMailLoginVerifyCodeAsync(
        string code,
        string userName,
        string emailAddress);
}
