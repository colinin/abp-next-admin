using System.Threading.Tasks;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Identity;

namespace LY.MicroService.IdentityServer.Emailing;

public interface IAccountEmailVerifySender : IAccountEmailer
{
    Task SendMailLoginVerifyLinkAsync(
        IdentityUser user,
        string code,
        string appName,
        string provider,
        bool rememberMe = false,
        string returnUrl = null,
        string returnUrlHash = null);
}
