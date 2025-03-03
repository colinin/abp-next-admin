using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Account;

public interface IMyClaimAppService : IApplicationService
{
    /// <summary>
    /// 变更头像
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task ChangeAvatarAsync(ChangeAvatarInput input);
    /// <summary>
    /// 查询绑定状态
    /// </summary>
    /// <param name="claimType"></param>
    /// <returns></returns>
    Task<GetUserClaimStateDto> GetStateAsync(string claimType);
    /// <summary>
    /// 重置绑定状态
    /// </summary>
    /// <param name="claimType"></param>
    /// <returns></returns>
    Task ResetAsync(string claimType);
}
