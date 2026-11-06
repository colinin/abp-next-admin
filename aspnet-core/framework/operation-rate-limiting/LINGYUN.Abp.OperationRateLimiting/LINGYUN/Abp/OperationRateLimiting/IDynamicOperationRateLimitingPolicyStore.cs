using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Loads the dynamic (for example database stored) policy definitions.
/// </summary>
/// <remarks>
/// The module merges these definitions with the static policies of
/// <see cref="IStaticOperationRateLimitingPolicyStore"/>. The merge strategy is configured with
/// <c>AbpDynamicDefinitionsOptions.MapStrategy&lt;OperationRateLimitingPolicy&gt;()</c> and
/// defaults to <c>DynamicDefinitionStrategy.Merge</c>.
/// </remarks>
public interface IDynamicOperationRateLimitingPolicyStore
{
    Task<IReadOnlyList<OperationRateLimitingPolicy>> GetPoliciesAsync(CancellationToken cancellationToken = default);

    Task<OperationRateLimitingPolicy?> GetOrNullAsync(string name, CancellationToken cancellationToken = default);
}
