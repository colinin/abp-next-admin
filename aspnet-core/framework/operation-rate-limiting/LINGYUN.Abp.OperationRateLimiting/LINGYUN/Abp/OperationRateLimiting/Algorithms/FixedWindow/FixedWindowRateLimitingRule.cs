using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.FixedWindow;

/// <summary>
/// Fixed window algorithm: allows the configured maximum count of requests per fixed time window.
/// </summary>
public class FixedWindowRateLimitingRule : OperationRateLimitingRuleBase
{
    public override Task<OperationRateLimitingStoreResult> ExecuteAsync(OperationRateLimitingRuleContext context)
    {
        return context.Store.ExecuteAsync(
            OperationRateLimitingStoreRequestFactory.Create(
                context,
                context.Rule.Duration,
                context.Rule.MaxCount),
            context.CancellationToken);
    }
}
