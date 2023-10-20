using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Authorize;
public interface IWeChatWorkInternalUserFinder
{
    /// <summary>
    /// 通过用户标识查询企业微信用户标识
    /// </summary>
    /// <param name="agentId"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> FindUserIdentifierAsync(
        string agentId,
        Guid userId, 
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 通过用户标识列表查询企业微信用户标识列表
    /// </summary>
    /// <param name="agentId"></param>
    /// <param name="userIdList"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<string>> FindUserIdentifierListAsync(
        string agentId,
        IEnumerable<Guid> userIdList,
        CancellationToken cancellationToken = default);
}
