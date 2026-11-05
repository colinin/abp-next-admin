using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace LINGYUN.Abp.OperationRateLimiting.Redis.Algorithms.SlidingWindow;

public class RedisSlidingWindowRateLimitingRule : RedisOperationRateLimitingRuleBase
{
    protected const string ScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/SlidingWindow/sliding-window.lua";

    public RedisSlidingWindowRateLimitingRule(
        StackExchangeRedisOperationRateLimitingExecutor executor,
        IOperationRateLimitingStateAccessor stateAccessor)
        : base(executor, stateAccessor)
    {
    }

    public override async Task<OperationRateLimitingStoreResult> ExecuteAsync(
        OperationRateLimitingRuleContext context)
    {
        var nowMs = context.Now.ToUnixTimeMilliseconds();
        var windowMs = Math.Max(1L, (long)context.Rule.Duration.TotalMilliseconds);
        var ttlMs = windowMs + 1000;

        var result = await Executor.EvaluateAsync(
            ScriptPath,
            context.Key,
            new RedisValue[]
            {
                nowMs,
                windowMs,
                context.Rule.MaxCount,
                context.Consume ? 1 : 0,
                Guid.NewGuid().ToString("N"),
                ttlMs
            },
            context.CancellationToken);

        return Executor.ParseResult(result, context);
    }
}
