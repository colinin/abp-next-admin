using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Work.Authorize;

[Dependency(ServiceLifetime.Singleton, TryRegister = true)]
public class NullWeChatWorkInternalUserFinder : IWeChatWorkInternalUserFinder
{
    public readonly static IWeChatWorkInternalUserFinder Instance = new NullWeChatWorkInternalUserFinder(); 
    public Task<string> FindUserIdentifierAsync(
        string agentId, 
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        string findUserId = null;
        return Task.FromResult(findUserId);
    }

    public Task<List<string>> FindUserIdentifierListAsync(
        string agentId,
        IEnumerable<Guid> userIdList,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<string>());
    }
}
