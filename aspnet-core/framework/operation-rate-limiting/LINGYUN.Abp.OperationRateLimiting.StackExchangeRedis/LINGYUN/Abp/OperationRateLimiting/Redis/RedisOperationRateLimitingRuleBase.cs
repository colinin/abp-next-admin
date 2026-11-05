using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting.Redis;

/// <summary>
/// Base class for Redis Lua rule implementations.
/// </summary>
public abstract class RedisOperationRateLimitingRuleBase :
    OperationRateLimitingRuleBase,
    IOperationRateLimitingRuleResetter
{
    protected StackExchangeRedisOperationRateLimitingExecutor Executor { get; }

    protected IOperationRateLimitingStateAccessor StateAccessor { get; }

    protected RedisOperationRateLimitingRuleBase(
        StackExchangeRedisOperationRateLimitingExecutor executor,
        IOperationRateLimitingStateAccessor stateAccessor)
    {
        Executor = executor;
        StateAccessor = stateAccessor;
    }

    public virtual Task ResetAsync(OperationRateLimitingRuleContext context)
    {
        return StateAccessor.ResetAsync(context.Key, context.CancellationToken);
    }
}
