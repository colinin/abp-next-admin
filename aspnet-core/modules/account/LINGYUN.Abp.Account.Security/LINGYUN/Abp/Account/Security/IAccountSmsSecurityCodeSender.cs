using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Security;
/// <summary>
/// 短信安全码发送接口
/// </summary>
public interface IAccountSmsSecurityCodeSender
{
    /// <summary>
    /// 发送短信验证码
    /// </summary>
    /// <param name="phone">手机号</param>
    /// <param name="code">短信验证码</param>
    /// <param name="templateCode">短信验证模板</param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task SendAsync(
        string phone,
        string code,
        string templateCode,
        CancellationToken cancellation = default);
}
