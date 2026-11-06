using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting.Redis;

[Dependency(ReplaceServices = true)]
public class StackExchangeRedisOperationRateLimitingRuleReleaser : IOperationRateLimitingRuleResetter, ITransientDependency
{
    protected StackExchangeRedisOperationRateLimitingExecutor Executor { get; }

    public StackExchangeRedisOperationRateLimitingRuleReleaser(
        StackExchangeRedisOperationRateLimitingExecutor executor)
    {
        Executor = executor;
    }

    public async virtual Task ResetAsync(OperationRateLimitingRuleContext context)
    {
        await Executor.KeyDeleteAsync(context.Key, context.CancellationToken);
    }
}
