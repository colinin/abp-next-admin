using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Default dynamic store used when no persistent store is registered.
/// </summary>
[Dependency(ServiceLifetime.Singleton, TryRegister = true)]
public class NullDynamicOperationRateLimitingPolicyStore : IDynamicOperationRateLimitingPolicyStore
{
    public Task<IReadOnlyList<OperationRateLimitingPolicy>> GetPoliciesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<OperationRateLimitingPolicy>>(new List<OperationRateLimitingPolicy>());
    }

    public Task<OperationRateLimitingPolicy?> GetOrNullAsync(string name, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<OperationRateLimitingPolicy?>(null);
    }
}
