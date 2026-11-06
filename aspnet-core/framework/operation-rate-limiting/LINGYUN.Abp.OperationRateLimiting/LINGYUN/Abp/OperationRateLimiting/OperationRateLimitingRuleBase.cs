using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Base class for rule implementations.
/// </summary>
public abstract class OperationRateLimitingRuleBase : IOperationRateLimitingRule, ITransientDependency
{
    protected IOperationRateLimitingStateAccessor StateAccessor { get; }
    protected OperationRateLimitingRuleBase(IOperationRateLimitingStateAccessor stateAccessor)
    {
        StateAccessor = stateAccessor;
    }

    public abstract Task<OperationRateLimitingStoreResult> ExecuteAsync(OperationRateLimitingRuleContext context);
}
