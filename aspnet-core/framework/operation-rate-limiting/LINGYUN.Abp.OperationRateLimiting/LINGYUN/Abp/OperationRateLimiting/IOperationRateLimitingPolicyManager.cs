using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Provides the effective policies by merging the static and dynamic policy stores.
/// </summary>
public interface IOperationRateLimitingPolicyManager
{
    Task<OperationRateLimitingPolicy?> GetOrNullAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OperationRateLimitingPolicy>> GetPoliciesAsync(
        CancellationToken cancellationToken = default);
}
