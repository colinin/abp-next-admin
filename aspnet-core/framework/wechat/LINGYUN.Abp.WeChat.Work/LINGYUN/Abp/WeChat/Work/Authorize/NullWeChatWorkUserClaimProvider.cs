using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Work.Authorize;

[Dependency(ServiceLifetime.Singleton, TryRegister = true)]
public class NullWeChatWorkUserClaimProvider : IWeChatWorkUserClaimProvider
{
    public readonly static IWeChatWorkUserClaimProvider Instance = new NullWeChatWorkUserClaimProvider(); 
    public Task<string> FindUserIdentifierAsync(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        string findUserId = null;
        return Task.FromResult(findUserId);
    }

    public Task<List<string>> FindUserIdentifierListAsync(
        IEnumerable<Guid> userIdList,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<string>());
    }
    public Task BindUserAsync(
        Guid userId,
        string weChatUserId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("请使用 AbpIdentityWeChatWorkModule 模块实现企业微信用户绑定!");
    }
}
