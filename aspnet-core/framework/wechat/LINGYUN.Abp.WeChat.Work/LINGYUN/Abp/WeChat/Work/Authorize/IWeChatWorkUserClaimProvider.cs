using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Authorize;
/// <summary>
/// 企业微信用户身份提供者
/// </summary>
public interface IWeChatWorkUserClaimProvider
{
    /// <summary>
    /// 通过用户标识查询企业微信用户标识
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> FindUserIdentifierAsync(
        Guid userId, 
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 通过用户标识列表查询企业微信用户标识列表
    /// </summary>
    /// <param name="userIdList"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<string>> FindUserIdentifierListAsync(
        IEnumerable<Guid> userIdList,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 绑定用户企业微信
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="weChatUserId">企业微信用户Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task BindUserAsync(
        Guid userId,
        string weChatUserId,
        CancellationToken cancellationToken = default);
}
