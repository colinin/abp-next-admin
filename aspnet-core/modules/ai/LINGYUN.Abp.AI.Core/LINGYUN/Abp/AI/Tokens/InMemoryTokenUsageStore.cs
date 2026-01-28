using LINGYUN.Abp.AI.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Tokens;

[Dependency(ServiceLifetime.Singleton, TryRegister = true)]
public class InMemoryTokenUsageStore : ITokenUsageStore
{
    private static readonly ConcurrentDictionary<string, List<TokenUsageInfo>> _tokenUsageCache = new ConcurrentDictionary<string, List<TokenUsageInfo>>();

    public Task SaveTokenUsageAsync(TokenUsageInfo tokenUsageInfo)
    {
        if (_tokenUsageCache.TryGetValue(tokenUsageInfo.Workspace, out var tokenUsageInfos))
        {
            tokenUsageInfos.Add(tokenUsageInfo);
        }
        else
        {
            _tokenUsageCache.TryAdd(tokenUsageInfo.Workspace, [tokenUsageInfo]);
        }
        
        return Task.CompletedTask;
    }
}
