using System;
using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.Concurrency;

/// <summary>
/// Concurrency algorithm: allows at most the configured number of in-flight operations.
/// A permit is released explicitly with <see cref="IOperationRateLimitingRuleReleaser.ReleaseAsync"/>
/// or automatically after the configured lease duration.
/// </summary>
public class ConcurrencyRateLimitingRule :
    OperationRateLimitingStateRuleBase,
    IOperationRateLimitingRuleReleaser
{
    public ConcurrencyRateLimitingRule(IOperationRateLimitingStateAccessor stateAccessor)
        : base(stateAccessor)
    {
    }

    public override async Task<OperationRateLimitingStoreResult> ExecuteAsync(
        OperationRateLimitingRuleContext context)
    {
        var maxCount = context.Rule.MaxCount;
        var nowMs = context.Now.ToUnixTimeMilliseconds();
        var leaseMs = Math.Max(1L, (long)context.Rule.Duration.TotalMilliseconds);
        var expiration = TimeSpan.FromMilliseconds(leaseMs * 2);

        if (!context.Consume)
        {
            var state = await StateAccessor.GetStateAsync<ConcurrencyRateLimitingState>(
                context.Key,
                context.CancellationToken);
            var evaluation = Evaluate(state, nowMs, leaseMs, maxCount, consume: false);

            return evaluation.Allowed
                ? OperationRateLimitingStoreResult.Allow(maxCount, evaluation.CurrentCount, evaluation.RetryAfter, context.Rule.Duration)
                : OperationRateLimitingStoreResult.Deny(maxCount, evaluation.CurrentCount, evaluation.RetryAfter, context.Rule.Duration);
        }

        var result = default(ConcurrencyEvaluation);
        await StateAccessor.UpdateStateAsync(
            context.Key,
            (ConcurrencyRateLimitingState? current) =>
            {
                result = Evaluate(current, nowMs, leaseMs, maxCount, consume: true);
                return result.State;
            },
            expiration,
            context.CancellationToken);

        return result.Allowed
            ? OperationRateLimitingStoreResult.Allow(maxCount, result.CurrentCount, result.RetryAfter, context.Rule.Duration)
            : OperationRateLimitingStoreResult.Deny(maxCount, result.CurrentCount, result.RetryAfter, context.Rule.Duration);
    }

    public virtual async Task ReleaseAsync(OperationRateLimitingRuleContext context)
    {
        var nowMs = context.Now.ToUnixTimeMilliseconds();
        var leaseMs = Math.Max(1L, (long)context.Rule.Duration.TotalMilliseconds);

        await StateAccessor.UpdateStateAsync(
            context.Key,
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
            context.CancellationToken);
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
