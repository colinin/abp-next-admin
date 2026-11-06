using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting;

public class OperationRateLimitingRuleResetter : IOperationRateLimitingRuleResetter, ITransientDependency
{
    protected IOperationRateLimitingStateAccessor StateAccessor { get; }

    public OperationRateLimitingRuleResetter(IOperationRateLimitingStateAccessor stateAccessor)
    {
        StateAccessor = stateAccessor;
    }

    public async virtual Task ResetAsync(OperationRateLimitingRuleContext context)
    {
        await StateAccessor.ResetAsync(context.Key, context.CancellationToken);
    }
}
