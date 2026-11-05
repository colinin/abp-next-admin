using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;

/// <summary>
/// Cache state implementation of the token bucket algorithm.
/// </summary>
internal static class TokenBucketRateLimitingExecutor
{
    public static async Task<OperationRateLimitingStoreResult> ExecuteAsync(
        IOperationRateLimitingStore store,
        OperationRateLimitingStoreRequest request,
        CancellationToken cancellationToken)
    {
        var maxCount = request.MaxCount;
        var tokensPerPeriod = Math.Max(1, request.TokensPerPeriod);
        var nowMs = request.Now.ToUnixTimeMilliseconds();
        var periodMs = Math.Max(1L, (long)request.Duration.TotalMilliseconds);
        var expiration = TimeSpan.FromMilliseconds(Math.Min(
            periodMs * (Math.Ceiling((double)maxCount / tokensPerPeriod) + 2),
            (double)TimeSpan.FromDays(1).TotalMilliseconds));

        if (!request.Consume)
        {
            var state = await store.GetStateAsync<TokenBucketRateLimitingState>(request.Key, cancellationToken);
            var evaluation = Evaluate(state, nowMs, periodMs, maxCount, tokensPerPeriod, consume: false);

            return evaluation.Allowed
                ? OperationRateLimitingStoreResult.Allow(maxCount, evaluation.CurrentCount, evaluation.RetryAfter, request.Duration)
                : OperationRateLimitingStoreResult.Deny(maxCount, evaluation.CurrentCount, evaluation.RetryAfter, request.Duration);
        }

        var result = default(TokenBucketEvaluation);
        await store.UpdateStateAsync(
            request.Key,
            (TokenBucketRateLimitingState? current) =>
            {
                result = Evaluate(current, nowMs, periodMs, maxCount, tokensPerPeriod, consume: true);
                return result.State;
            },
            expiration,
            cancellationToken);

        return result.Allowed
            ? OperationRateLimitingStoreResult.Allow(maxCount, result.CurrentCount, result.RetryAfter, request.Duration)
            : OperationRateLimitingStoreResult.Deny(maxCount, result.CurrentCount, result.RetryAfter, request.Duration);
    }

    private static TokenBucketEvaluation Evaluate(
        TokenBucketRateLimitingState? state,
        long nowMs,
        long periodMs,
        int tokenLimit,
        int tokensPerPeriod,
        bool consume)
    {
        state ??= new TokenBucketRateLimitingState
        {
            Tokens = tokenLimit,
            LastRefillUnixMs = nowMs
        };

        var elapsedMs = nowMs - state.LastRefillUnixMs;
        if (elapsedMs >= periodMs)
        {
            var periods = elapsedMs / periodMs;
            state.Tokens = Math.Min(tokenLimit, state.Tokens + periods * (double)tokensPerPeriod);
            state.LastRefillUnixMs += periods * periodMs;
        }

        if (state.Tokens > tokenLimit)
        {
            state.Tokens = tokenLimit;
        }

        var allowed = state.Tokens >= 1d;
        if (allowed && consume)
        {
            state.Tokens -= 1d;
        }

        TimeSpan? retryAfter = null;

        if (!allowed)
        {
            var missingTokens = 1d - state.Tokens;
            var periodsNeeded = (long)Math.Ceiling(missingTokens / tokensPerPeriod);
            var retryMs = state.LastRefillUnixMs + periodsNeeded * periodMs - nowMs;

            retryAfter = TimeSpan.FromMilliseconds(Math.Max(1, retryMs));
        }

        return new TokenBucketEvaluation
        {
            Allowed = allowed,
            CurrentCount = (int)Math.Floor(state.Tokens),
            RetryAfter = retryAfter,
            State = state
        };
    }

    private readonly struct TokenBucketEvaluation
    {
        public bool Allowed { get; init; }

        public int CurrentCount { get; init; }

        public TimeSpan? RetryAfter { get; init; }

        public TokenBucketRateLimitingState State { get; init; }
    }
}
