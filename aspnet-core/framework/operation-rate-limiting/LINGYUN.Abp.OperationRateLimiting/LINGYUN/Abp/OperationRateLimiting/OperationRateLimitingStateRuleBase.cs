using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Base class for rules whose counters are stored through
/// <see cref="IOperationRateLimitingStateAccessor"/>.
/// </summary>
public abstract class OperationRateLimitingStateRuleBase :
    OperationRateLimitingRuleBase,
    IOperationRateLimitingRuleResetter
{
    protected IOperationRateLimitingStateAccessor StateAccessor { get; }

    protected OperationRateLimitingStateRuleBase(IOperationRateLimitingStateAccessor stateAccessor)
    {
        StateAccessor = stateAccessor;
    }

    public virtual Task ResetAsync(OperationRateLimitingRuleContext context)
    {
        return StateAccessor.ResetAsync(context.Key, context.CancellationToken);
    }
}
