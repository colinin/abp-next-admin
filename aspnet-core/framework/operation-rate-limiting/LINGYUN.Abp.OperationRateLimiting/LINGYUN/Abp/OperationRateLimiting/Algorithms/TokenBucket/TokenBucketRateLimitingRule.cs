using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;

/// <summary>
/// Token bucket algorithm: replenishes tokens periodically and consumes one token per request.
/// </summary>
public class TokenBucketRateLimitingRule : OperationRateLimitingStateRuleBase
{
    public TokenBucketRateLimitingRule(IOperationRateLimitingStateAccessor stateAccessor)
        : base(stateAccessor)
    {
    }

    public override async Task<OperationRateLimitingStoreResult> ExecuteAsync(
        OperationRateLimitingRuleContext context)
    {
        var maxCount = context.Rule.MaxCount;
        var tokensPerPeriod = Math.Max(1, context.Rule.GetTokensPerPeriod());
        var nowMs = context.Now.ToUnixTimeMilliseconds();
        var periodMs = Math.Max(1L, (long)context.Rule.Duration.TotalMilliseconds);
        var expiration = TimeSpan.FromMilliseconds(Math.Min(
            periodMs * (Math.Ceiling((double)maxCount / tokensPerPeriod) + 2),
            (double)TimeSpan.FromDays(1).TotalMilliseconds));

        if (!context.Consume)
        {
            var state = await StateAccessor.GetStateAsync<TokenBucketRateLimitingState>(
                context.Key,
                context.CancellationToken);
            var evaluation = Evaluate(state, nowMs, periodMs, maxCount, tokensPerPeriod, consume: false);

            return evaluation.Allowed
                ? OperationRateLimitingStoreResult.Allow(maxCount, evaluation.CurrentCount, evaluation.RetryAfter, context.Rule.Duration)
                : OperationRateLimitingStoreResult.Deny(maxCount, evaluation.CurrentCount, evaluation.RetryAfter, context.Rule.Duration);
        }

        var result = default(TokenBucketEvaluation);
        await StateAccessor.UpdateStateAsync(
            context.Key,
            (TokenBucketRateLimitingState? current) =>
            {
                result = Evaluate(current, nowMs, periodMs, maxCount, tokensPerPeriod, consume: true);
                return result.State;
            },
            expiration,
            context.CancellationToken);

        return result.Allowed
            ? OperationRateLimitingStoreResult.Allow(maxCount, result.CurrentCount, result.RetryAfter, context.Rule.Duration)
            : OperationRateLimitingStoreResult.Deny(maxCount, result.CurrentCount, result.RetryAfter, context.Rule.Duration);
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
