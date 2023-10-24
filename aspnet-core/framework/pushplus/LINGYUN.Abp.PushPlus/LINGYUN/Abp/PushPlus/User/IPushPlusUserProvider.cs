using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.User;
/// <summary>
/// 用户接口
/// </summary>
public interface IPushPlusUserProvider
{
    /// <summary>
    /// 获取token
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> GetMyTokenAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// 个人资料详情
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PushPlusUserProfile> GetMyProfileAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取解封剩余时间
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PushPlusUserLimitTime> GetMyLimitTimeAsync(CancellationToken cancellationToken = default);
}
