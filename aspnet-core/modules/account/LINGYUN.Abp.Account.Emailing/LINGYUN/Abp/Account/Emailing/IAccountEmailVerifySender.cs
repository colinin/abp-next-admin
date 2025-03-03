using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Emailing;

public interface IAccountEmailVerifySender
{
    Task SendMailLoginVerifyCodeAsync(
        string code,
        string userName,
        string emailAddress);
}
