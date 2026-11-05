using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Loads the statically defined policies.
/// </summary>
/// <remarks>
/// The default implementation reads the policies from <see cref="AbpOperationRateLimitingOptions"/>.
/// Replace it to load static policies from another source (configuration, a file, ...).
/// </remarks>
public interface IStaticOperationRateLimitingPolicyStore
{
    Task<IReadOnlyList<OperationRateLimitingPolicy>> GetPoliciesAsync(CancellationToken cancellationToken = default);

    Task<OperationRateLimitingPolicy?> GetOrNullAsync(string name, CancellationToken cancellationToken = default);
}
