using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Identity;
public interface IIdentitySessionAppService
{
    /// <summary>
    /// 获取用户会话列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedResultDto<IdentitySessionDto>> GetSessionsAsync(GetUserSessionsInput input);
    /// <summary>
    /// 撤销用户会话
    /// </summary>
    /// <param name="sessionId">会话id</param>
    /// <returns></returns>
    Task RevokeSessionAsync(string sessionId);
}
