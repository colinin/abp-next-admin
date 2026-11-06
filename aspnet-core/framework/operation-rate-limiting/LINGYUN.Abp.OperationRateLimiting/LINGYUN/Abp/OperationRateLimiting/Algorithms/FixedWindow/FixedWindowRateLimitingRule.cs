using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.FixedWindow;

/// <summary>
/// Fixed window algorithm: allows the configured maximum count of requests per fixed time window.
/// </summary>
public class FixedWindowRateLimitingRule : OperationRateLimitingRuleBase
{
    public FixedWindowRateLimitingRule(IOperationRateLimitingStateAccessor stateAccessor)
        : base(stateAccessor)
    {
    }

    public override async Task<OperationRateLimitingStoreResult> ExecuteAsync(
        OperationRateLimitingRuleContext context)
    {
        var maxCount = context.Rule.MaxCount;
        var nowMs = context.Now.ToUnixTimeMilliseconds();
        var windowMs = Math.Max(1L, (long)context.Rule.Duration.TotalMilliseconds);
        var windowStartMs = nowMs - nowMs % windowMs;
        var retryAfter = TimeSpan.FromMilliseconds(Math.Max(1, windowStartMs + windowMs - nowMs));

        if (!context.Consume)
        {
            var state = await StateAccessor.GetStateAsync<FixedWindowRateLimitingState>(
                context.Key,
                context.CancellationToken);
            var count = state == null || state.WindowStartUnixMs != windowStartMs ? 0 : (int)state.Count;

            return count < maxCount
                ? OperationRateLimitingStoreResult.Allow(maxCount, count, retryAfter, context.Rule.Duration)
                : OperationRateLimitingStoreResult.Deny(maxCount, count, retryAfter, context.Rule.Duration);
        }

        var allowed = false;
        var updated = await StateAccessor.UpdateStateAsync(
            context.Key,
            (FixedWindowRateLimitingState? current) =>
            {
                if (current == null || current.WindowStartUnixMs != windowStartMs)
                {
                    current = new FixedWindowRateLimitingState { WindowStartUnixMs = windowStartMs };
                }

                allowed = current.Count < maxCount;
                if (allowed)
                {
                    current.Count++;
                }

                return current;
            },
            TimeSpan.FromMilliseconds(windowMs * 2),
            context.CancellationToken);

        var currentCount = (int)(updated?.Count ?? 0);

        return allowed
            ? OperationRateLimitingStoreResult.Allow(maxCount, currentCount, retryAfter, context.Rule.Duration)
            : OperationRateLimitingStoreResult.Deny(maxCount, currentCount, retryAfter, context.Rule.Duration);
    }
}
