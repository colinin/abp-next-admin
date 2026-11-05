using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Executes one rule of a policy. Implement this interface to provide a custom algorithm.
/// </summary>
/// <remarks>
/// Implementations are resolved from dependency injection for every check, so they may use
/// scoped services. Register the implementation with <see cref="AbpOperationRateLimitingOptions.AddAlgorithm{TAlgorithm}"/>
/// or add it to a policy with <c>policy.AddRule&lt;TAlgorithm&gt;()</c>.
/// </remarks>
public interface IOperationRateLimitingRule
{
    Task<OperationRateLimitingStoreResult> ExecuteAsync(OperationRateLimitingRuleContext context);
}
