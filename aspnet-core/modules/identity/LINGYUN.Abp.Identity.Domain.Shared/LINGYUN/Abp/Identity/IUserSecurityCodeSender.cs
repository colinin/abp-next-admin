using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Identity
{
    public interface IUserSecurityCodeSender
    {
        Task SendPhoneConfirmedCodeAsync(
            string phone,
            string token,
            string template, // 传递模板号
            CancellationToken cancellation = default);

        Task SendEmailConfirmedCodeAsync(
            string userName,
            string email,
            string token,
            CancellationToken cancellation = default);
    }
}
