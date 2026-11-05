using System;
using System.Threading.Tasks;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;
using StackExchange.Redis;

namespace LINGYUN.Abp.OperationRateLimiting.Redis.Algorithms.TokenBucket;

public class RedisTokenBucketRateLimitingRule : RedisOperationRateLimitingRuleBase
{
    protected const string ScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/TokenBucket/token-bucket.lua";

    public RedisTokenBucketRateLimitingRule(
        StackExchangeRedisOperationRateLimitingExecutor executor,
        IOperationRateLimitingStateAccessor stateAccessor)
        : base(executor, stateAccessor)
    {
    }

    public override async Task<OperationRateLimitingStoreResult> ExecuteAsync(
        OperationRateLimitingRuleContext context)
    {
        var nowMs = context.Now.ToUnixTimeMilliseconds();
        var tokensPerPeriod = Math.Max(1, context.Rule.GetTokensPerPeriod());
        var periodMs = Math.Max(1L, (long)context.Rule.Duration.TotalMilliseconds);
        var ttlMs = (long)Math.Min(
            periodMs * (Math.Ceiling((double)context.Rule.MaxCount / tokensPerPeriod) + 2),
            (double)TimeSpan.FromDays(1).TotalMilliseconds);

        var result = await Executor.EvaluateAsync(
            ScriptPath,
            context.Key,
            new RedisValue[]
            {
                nowMs,
                periodMs,
                context.Rule.MaxCount,
                tokensPerPeriod,
                context.Consume ? 1 : 0,
                ttlMs
            },
            context.CancellationToken);

        return Executor.ParseResult(result, context);
    }
}
