using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Resolves the partition (counter) key of a rule.
/// </summary>
public interface IOperationRateLimitingPartitionKeyResolver
{
    Task<string> ResolveAsync(
        OperationRateLimitingRuleDefinition rule,
        OperationRateLimitingContext context,
        CancellationToken cancellationToken = default);
}
