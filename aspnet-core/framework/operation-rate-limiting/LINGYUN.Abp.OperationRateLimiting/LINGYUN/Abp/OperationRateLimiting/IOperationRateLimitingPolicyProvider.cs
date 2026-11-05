using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Provides the effective policy definitions.
/// Replace this service to load policies from any external source.
/// </summary>
public interface IOperationRateLimitingPolicyProvider
{
    Task<OperationRateLimitingPolicy?> GetOrNullAsync(string name, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OperationRateLimitingPolicy>> GetPoliciesAsync(CancellationToken cancellationToken = default);
}
