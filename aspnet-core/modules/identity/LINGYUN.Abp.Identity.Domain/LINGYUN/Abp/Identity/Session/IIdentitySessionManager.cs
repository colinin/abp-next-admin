using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Identity.Session;
/// <summary>
/// 管理用户会话
/// </summary>
public interface IIdentitySessionManager
{
    /// <summary>
    /// 保存会话
    /// </summary>
    /// <param name="claimsPrincipal">用户身份主体</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveSessionAsync(
        ClaimsPrincipal claimsPrincipal, 
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 撤销用户会话
    /// </summary>
    /// <param name="sessionId">会话id</param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task RevokeSessionAsync(
        string sessionId,
        CancellationToken cancellation = default);
}
