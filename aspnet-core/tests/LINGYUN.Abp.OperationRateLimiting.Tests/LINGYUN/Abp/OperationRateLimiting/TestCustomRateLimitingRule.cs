using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Custom rule used to verify that user defined algorithms can be added to a policy.
/// </summary>
public class TestCustomRateLimitingRule : OperationRateLimitingRuleBase
{
    public override async Task<OperationRateLimitingStoreResult> ExecuteAsync(OperationRateLimitingRuleContext context)
    {
        var maxCount = context.Rule.MaxCount;
        var duration = context.Rule.Duration;
        var allowed = false;

        var state = await context.Store.UpdateStateAsync(
            context.Key,
            (TestCustomRateLimitingState? current) =>
            {
                current ??= new TestCustomRateLimitingState();

                allowed = current.Count < maxCount;
                if (context.Consume && allowed)
                {
                    current.Count++;
                }

                return current;
            },
            TimeSpan.FromMinutes(1),
            context.CancellationToken);

        var count = state?.Count ?? 0;

        return allowed
            ? OperationRateLimitingStoreResult.Allow(maxCount, count, duration, duration)
            : OperationRateLimitingStoreResult.Deny(maxCount, count, duration, duration);
    }
}
