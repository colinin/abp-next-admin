using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Default dynamic store used when no persistent store is registered.
/// </summary>
public class NullDynamicOperationRateLimitingPolicyStore : IDynamicOperationRateLimitingPolicyStore
{
    public static NullDynamicOperationRateLimitingPolicyStore Instance { get; } = new();

    public Task<IReadOnlyList<OperationRateLimitingPolicy>> GetPoliciesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<OperationRateLimitingPolicy>>(new List<OperationRateLimitingPolicy>());
    }

    public Task<OperationRateLimitingPolicy?> GetOrNullAsync(string name, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<OperationRateLimitingPolicy?>(null);
    }

    public Task InvalidateCacheAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
