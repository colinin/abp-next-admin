using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting.Redis.Algorithms.FixedWindow;

public class RedisFixedWindowRateLimitingRule : StackExchangeRedisOperationRateLimitingRuleBase
{
    protected const string ScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/FixedWindow/fixed-window.lua";

    public RedisFixedWindowRateLimitingRule(
        StackExchangeRedisOperationRateLimitingExecutor executor)
        : base(executor)
    {
    }

    public override async Task<OperationRateLimitingStoreResult> ExecuteAsync(
        OperationRateLimitingRuleContext context)
    {
        var nowMs = context.Now.ToUnixTimeMilliseconds();
        var windowMs = Math.Max(1L, (long)context.Rule.Duration.TotalMilliseconds);
        var windowStartMs = nowMs - nowMs % windowMs;
        var ttlMs = Math.Max(1, windowStartMs + windowMs - nowMs);

        var result = await Executor.EvaluateAsync(
            ScriptPath,
            context.Key,
            new RedisValue[]
            {
                context.Rule.MaxCount,
                context.Consume ? 1 : 0,
                ttlMs,
                ttlMs
            },
            context.CancellationToken);

        return Executor.ParseResult(result, context);
    }
}
