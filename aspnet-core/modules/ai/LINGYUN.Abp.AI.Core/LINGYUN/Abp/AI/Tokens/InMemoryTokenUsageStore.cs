using LINGYUN.Abp.AI.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Tokens;

[Dependency(ServiceLifetime.Singleton, TryRegister = true)]
public class InMemoryTokenUsageStore : ITokenUsageStore
{
    private static readonly ConcurrentDictionary<string, TokenUsageInfo> _tokenUsageCache = new ConcurrentDictionary<string, TokenUsageInfo>();

    public Task SaveTokenUsagesAsync(IEnumerable<TokenUsageInfo> usageInfos)
    {
        foreach (var usageInfo in usageInfos.GroupBy(x => x.Workspace))
        {
            var tokenUsageInfo = new TokenUsageInfo(usageInfo.Key)
            {
                InputTokenCount = usageInfo.Sum(x => x.InputTokenCount),
                TotalTokenCount = usageInfo.Sum(x => x.TotalTokenCount),
                OutputTokenCount = usageInfo.Sum(x => x.OutputTokenCount),
                ReasoningTokenCount = usageInfo.Sum(x => x.ReasoningTokenCount),
                CachedInputTokenCount = usageInfo.Sum(x => x.CachedInputTokenCount),
            };

            if (!_tokenUsageCache.ContainsKey(usageInfo.Key))
            {
                _tokenUsageCache.TryAdd(usageInfo.Key, tokenUsageInfo);
            }
            else
            {
                _tokenUsageCache[usageInfo.Key] = tokenUsageInfo;
            }
        }

        return Task.CompletedTask;
    }
}
