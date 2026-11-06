using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Implemented by rules that own a counter and can remove it.
/// </summary>
public interface IOperationRateLimitingRuleResetter
{
    Task ResetAsync(OperationRateLimitingRuleContext context);
}
