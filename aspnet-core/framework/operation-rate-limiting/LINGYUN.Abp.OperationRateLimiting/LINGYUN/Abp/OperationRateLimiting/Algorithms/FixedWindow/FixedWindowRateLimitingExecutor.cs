using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.FixedWindow;

/// <summary>
/// Cache state implementation of the fixed window algorithm.
/// </summary>
internal static class FixedWindowRateLimitingExecutor
{
    public static async Task<OperationRateLimitingStoreResult> ExecuteAsync(
        IOperationRateLimitingStore store,
        OperationRateLimitingStoreRequest request,
        CancellationToken cancellationToken)
    {
        var maxCount = request.MaxCount;
        var nowMs = request.Now.ToUnixTimeMilliseconds();
        var windowMs = Math.Max(1L, (long)request.Duration.TotalMilliseconds);
        var windowStartMs = nowMs - nowMs % windowMs;
        var retryAfter = TimeSpan.FromMilliseconds(Math.Max(1, windowStartMs + windowMs - nowMs));

        if (!request.Consume)
        {
            var state = await store.GetStateAsync<FixedWindowRateLimitingState>(request.Key, cancellationToken);
            var count = state == null || state.WindowStartUnixMs != windowStartMs ? 0 : (int)state.Count;

            return count < maxCount
                ? OperationRateLimitingStoreResult.Allow(maxCount, count, retryAfter, request.Duration)
                : OperationRateLimitingStoreResult.Deny(maxCount, count, retryAfter, request.Duration);
        }

        var allowed = false;
        var updated = await store.UpdateStateAsync(
            request.Key,
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
            cancellationToken);

        var currentCount = (int)(updated?.Count ?? 0);

        return allowed
            ? OperationRateLimitingStoreResult.Allow(maxCount, currentCount, retryAfter, request.Duration)
            : OperationRateLimitingStoreResult.Deny(maxCount, currentCount, retryAfter, request.Duration);
    }
}
