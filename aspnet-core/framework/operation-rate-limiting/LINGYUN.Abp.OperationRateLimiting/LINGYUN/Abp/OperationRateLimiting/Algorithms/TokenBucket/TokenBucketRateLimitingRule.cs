using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;

/// <summary>
/// Token bucket algorithm: allows bursts up to the configured token limit and replenishes
/// the configured number of tokens every period.
/// </summary>
public class TokenBucketRateLimitingRule : OperationRateLimitingRuleBase
{
    public override Task<OperationRateLimitingStoreResult> ExecuteAsync(OperationRateLimitingRuleContext context)
    {
        return context.Store.ExecuteAsync(
            OperationRateLimitingStoreRequestFactory.Create(
                context,
                context.Rule.Duration,
                context.Rule.MaxCount,
                tokensPerPeriod: context.Rule.GetTokensPerPeriod()),
            context.CancellationToken);
    }
}
