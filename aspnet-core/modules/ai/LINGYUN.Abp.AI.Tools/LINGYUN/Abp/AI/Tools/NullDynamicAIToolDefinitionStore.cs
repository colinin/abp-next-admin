using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Tools;

[Dependency(TryRegister = true)]
public class NullDynamicAIToolDefinitionStore : IDynamicAIToolDefinitionStore, ISingletonDependency
{
    private readonly static Task<AIToolDefinition?> CachedNullableAIToolResult = Task.FromResult((AIToolDefinition?)null);
    private readonly static Task<AIToolDefinition> CachedAIToolResult = Task.FromResult((AIToolDefinition)null!);

    private readonly static Task<IReadOnlyList<AIToolDefinition>> CachedAIToolsResult = Task.FromResult(
        (IReadOnlyList<AIToolDefinition>)Array.Empty<AIToolDefinition>().ToImmutableList());

    public Task<AIToolDefinition> GetAsync(string name)
    {
        return CachedAIToolResult;
    }

    public Task<IReadOnlyList<AIToolDefinition>> GetAllAsync()
    {
        return CachedAIToolsResult;
    }

    public Task<AIToolDefinition?> GetOrNullAsync(string name)
    {
        return CachedNullableAIToolResult;
    }
}
