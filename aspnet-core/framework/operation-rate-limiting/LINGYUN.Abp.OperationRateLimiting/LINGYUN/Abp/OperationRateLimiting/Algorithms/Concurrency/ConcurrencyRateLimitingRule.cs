using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.Concurrency;

/// <summary>
/// Concurrency algorithm: allows at most the configured number of in-flight operations.
/// A permit is released explicitly with <see cref="IOperationRateLimitingRuleReleaser.ReleaseAsync"/>
/// or automatically after the configured lease duration.
/// </summary>
public class ConcurrencyRateLimitingRule : OperationRateLimitingRuleBase, IOperationRateLimitingRuleReleaser
{
    public override Task<OperationRateLimitingStoreResult> ExecuteAsync(OperationRateLimitingRuleContext context)
    {
        return context.Store.ExecuteAsync(
            CreateRequest(context),
            context.CancellationToken);
    }

    public virtual Task ReleaseAsync(OperationRateLimitingRuleContext context)
    {
        return context.Store.ReleaseAsync(
            CreateRequest(context),
            context.CancellationToken);
    }

    private static OperationRateLimitingStoreRequest CreateRequest(OperationRateLimitingRuleContext context)
    {
        return OperationRateLimitingStoreRequestFactory.Create(
            context,
            context.Rule.Duration,
            context.Rule.MaxCount);
    }
}
