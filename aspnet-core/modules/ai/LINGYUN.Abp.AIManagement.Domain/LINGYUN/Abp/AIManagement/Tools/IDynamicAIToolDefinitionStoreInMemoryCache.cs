using LINGYUN.Abp.AI.Tools;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AIManagement.Tools;
public interface IDynamicAIToolDefinitionStoreInMemoryCache
{
    string CacheStamp { get; set; }

    SemaphoreSlim SyncSemaphore { get; }

    DateTime? LastCheckTime { get; set; }

    Task FillAsync(List<AIToolDefinitionRecord> tools);

    AIToolDefinition? GetAIToolOrNull(string name);

    IReadOnlyList<AIToolDefinition> GetAITools();
}
