using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace LINGYUN.Abp.OperationRateLimiting.Redis.Algorithms.Concurrency;

public class RedisConcurrencyRateLimitingRule :
    RedisOperationRateLimitingRuleBase,
    IOperationRateLimitingRuleReleaser
{
    protected const string ScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/Concurrency/concurrency.lua";
    protected const string ReleaseScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/Concurrency/concurrency-release.lua";

    public RedisConcurrencyRateLimitingRule(
        StackExchangeRedisOperationRateLimitingExecutor executor,
        IOperationRateLimitingStateAccessor stateAccessor)
        : base(executor, stateAccessor)
    {
    }

    public override async Task<OperationRateLimitingStoreResult> ExecuteAsync(
        OperationRateLimitingRuleContext context)
    {
        var nowMs = context.Now.ToUnixTimeMilliseconds();
        var leaseMs = Math.Max(1L, (long)context.Rule.Duration.TotalMilliseconds);

        var result = await Executor.EvaluateAsync(
            ScriptPath,
            context.Key,
            new RedisValue[]
            {
                nowMs,
                leaseMs,
                context.Rule.MaxCount,
                context.Consume ? 1 : 0,
                Guid.NewGuid().ToString("N"),
                leaseMs * 2
            },
            context.CancellationToken);

        return Executor.ParseResult(result, context);
    }

    public virtual async Task ReleaseAsync(OperationRateLimitingRuleContext context)
    {
        var leaseMs = Math.Max(1L, (long)context.Rule.Duration.TotalMilliseconds);

        await Executor.EvaluateAsync(
            ReleaseScriptPath,
            context.Key,
            new RedisValue[]
            {
                context.Now.ToUnixTimeMilliseconds(),
                leaseMs
            },
            context.CancellationToken);
    }
}
