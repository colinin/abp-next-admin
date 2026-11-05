using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.SlidingWindow;

/// <summary>
/// Sliding window algorithm: counts the requests of the last window by dividing it into segments.
/// </summary>
public class SlidingWindowRateLimitingRule : OperationRateLimitingRuleBase
{
    public override Task<OperationRateLimitingStoreResult> ExecuteAsync(OperationRateLimitingRuleContext context)
    {
        return context.Store.ExecuteAsync(
            OperationRateLimitingStoreRequestFactory.Create(
                context,
                context.Rule.Duration,
                context.Rule.MaxCount,
                context.Rule.GetSegments()),
            context.CancellationToken);
    }
}
