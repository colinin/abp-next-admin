using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.Concurrency;

/// <summary>
/// Cache state implementation of the concurrency algorithm.
/// </summary>
internal static class ConcurrencyRateLimitingExecutor
{
    public static async Task<OperationRateLimitingStoreResult> ExecuteAsync(
        IOperationRateLimitingStore store,
        OperationRateLimitingStoreRequest request,
        CancellationToken cancellationToken)
    {
        var maxCount = request.MaxCount;
        var nowMs = request.Now.ToUnixTimeMilliseconds();
        var leaseMs = Math.Max(1L, (long)request.Duration.TotalMilliseconds);
        var expiration = TimeSpan.FromMilliseconds(leaseMs * 2);

        if (!request.Consume)
        {
            var state = await store.GetStateAsync<ConcurrencyRateLimitingState>(request.Key, cancellationToken);
            var evaluation = Evaluate(state, nowMs, leaseMs, maxCount, consume: false);

            return evaluation.Allowed
                ? OperationRateLimitingStoreResult.Allow(maxCount, evaluation.CurrentCount, evaluation.RetryAfter, request.Duration)
                : OperationRateLimitingStoreResult.Deny(maxCount, evaluation.CurrentCount, evaluation.RetryAfter, request.Duration);
        }

        var result = default(ConcurrencyEvaluation);
        await store.UpdateStateAsync(
            request.Key,
            (ConcurrencyRateLimitingState? current) =>
            {
                result = Evaluate(current, nowMs, leaseMs, maxCount, consume: true);
                return result.State;
            },
            expiration,
            cancellationToken);

        return result.Allowed
            ? OperationRateLimitingStoreResult.Allow(maxCount, result.CurrentCount, result.RetryAfter, request.Duration)
            : OperationRateLimitingStoreResult.Deny(maxCount, result.CurrentCount, result.RetryAfter, request.Duration);
    }

    public static async Task ReleaseAsync(
        IOperationRateLimitingStore store,
        OperationRateLimitingStoreRequest request,
        CancellationToken cancellationToken)
    {
        var nowMs = request.Now.ToUnixTimeMilliseconds();
        var leaseMs = Math.Max(1L, (long)request.Duration.TotalMilliseconds);

        await store.UpdateStateAsync(
            request.Key,
            (ConcurrencyRateLimitingState? current) =>
            {
                if (current == null)
                {
                    return null;
                }

                current.LeaseStartUnixMs.RemoveAll(start => start > nowMs || nowMs - start >= leaseMs);

                if (current.LeaseStartUnixMs.Count == 0)
                {
                    return null;
                }

                current.LeaseStartUnixMs.Sort();
                current.LeaseStartUnixMs.RemoveAt(0);

                return current.LeaseStartUnixMs.Count == 0 ? null : current;
            },
            TimeSpan.FromMilliseconds(leaseMs * 2),
            cancellationToken);
    }

    private static ConcurrencyEvaluation Evaluate(
        ConcurrencyRateLimitingState? state,
        long nowMs,
        long leaseMs,
        int permitLimit,
        bool consume)
    {
        state ??= new ConcurrencyRateLimitingState();
        state.LeaseStartUnixMs.RemoveAll(start => start > nowMs || nowMs - start >= leaseMs);

        var allowed = state.LeaseStartUnixMs.Count < permitLimit;

        if (allowed && consume)
        {
            state.LeaseStartUnixMs.Add(nowMs);
        }

        TimeSpan? retryAfter = null;

        if (!allowed && state.LeaseStartUnixMs.Count > 0)
        {
            var oldestLease = state.LeaseStartUnixMs.Min();
            retryAfter = TimeSpan.FromMilliseconds(Math.Max(1, oldestLease + leaseMs - nowMs));
        }

        return new ConcurrencyEvaluation
        {
            Allowed = allowed,
            CurrentCount = state.LeaseStartUnixMs.Count,
            RetryAfter = retryAfter,
            State = state
        };
    }

    private readonly struct ConcurrencyEvaluation
    {
        public bool Allowed { get; init; }

        public int CurrentCount { get; init; }

        public TimeSpan? RetryAfter { get; init; }

        public ConcurrencyRateLimitingState State { get; init; }
    }
}
