using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WxPusher.User;

public interface IWxPusherUserStore
{
    /// <summary>
    /// 获取用户订阅的topic列表
    /// </summary>
    /// <param name="userIds">用户标识列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<int>> GetSubscribeTopicsAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取绑定用户uid列表
    /// </summary>
    /// <param name="userIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<string>> GetBindUidsAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default);
}
