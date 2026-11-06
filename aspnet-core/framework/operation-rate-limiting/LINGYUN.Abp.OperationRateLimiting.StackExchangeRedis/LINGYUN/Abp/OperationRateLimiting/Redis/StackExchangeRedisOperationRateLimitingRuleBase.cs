using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting.Redis;

public abstract class StackExchangeRedisOperationRateLimitingRuleBase : IOperationRateLimitingRule, ITransientDependency
{
    protected StackExchangeRedisOperationRateLimitingExecutor Executor { get; }
    protected StackExchangeRedisOperationRateLimitingRuleBase(
        StackExchangeRedisOperationRateLimitingExecutor executor)
    {
        Executor = executor;
    }

    public abstract Task<OperationRateLimitingStoreResult> ExecuteAsync(OperationRateLimitingRuleContext context);
}
