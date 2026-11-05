using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Base class for rule implementations.
/// </summary>
public abstract class OperationRateLimitingRuleBase : IOperationRateLimitingRule, ITransientDependency
{
    public abstract Task<OperationRateLimitingStoreResult> ExecuteAsync(OperationRateLimitingRuleContext context);
}
