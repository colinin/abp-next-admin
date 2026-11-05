using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

public class FakeDynamicOperationRateLimitingPolicyStore : IDynamicOperationRateLimitingPolicyStore
{
    protected FakeOperationRateLimitingPolicyStoreState State { get; }

    public FakeDynamicOperationRateLimitingPolicyStore(FakeOperationRateLimitingPolicyStoreState state)
    {
        State = state;
    }

    public Task<IReadOnlyList<OperationRateLimitingPolicy>> GetPoliciesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<OperationRateLimitingPolicy>>(State.Policies.Values.ToList());
    }

    public Task<OperationRateLimitingPolicy?> GetOrNullAsync(string name, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(State.Policies.TryGetValue(name, out var policy) ? policy : null);
    }

    public Task InvalidateCacheAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
