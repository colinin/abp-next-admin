using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Optional interface for rules that can release a permit acquired by a previous check
/// (used by the concurrency algorithm).
/// </summary>
public interface IOperationRateLimitingRuleReleaser
{
    Task ReleaseAsync(OperationRateLimitingRuleContext context);
}
