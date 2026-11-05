using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

public static class OperationRateLimitingCheckerExtensions
{
    public static Task<OperationRateLimitingStatus> CheckAsync(
        this IOperationRateLimitingChecker checker,
        string policyName,
        string? parameter,
        CancellationToken cancellationToken = default)
    {
        return checker.CheckAsync(
            policyName,
            new OperationRateLimitingContext { Parameter = parameter },
            cancellationToken);
    }

    public static Task<bool> IsAllowedAsync(
        this IOperationRateLimitingChecker checker,
        string policyName,
        string? parameter,
        CancellationToken cancellationToken = default)
    {
        return checker.IsAllowedAsync(
            policyName,
            new OperationRateLimitingContext { Parameter = parameter },
            cancellationToken);
    }

    public static Task<OperationRateLimitingStatus> GetStatusAsync(
        this IOperationRateLimitingChecker checker,
        string policyName,
        string? parameter,
        CancellationToken cancellationToken = default)
    {
        return checker.GetStatusAsync(
            policyName,
            new OperationRateLimitingContext { Parameter = parameter },
            cancellationToken);
    }

    public static Task ResetAsync(
        this IOperationRateLimitingChecker checker,
        string policyName,
        string? parameter,
        CancellationToken cancellationToken = default)
    {
        return checker.ResetAsync(
            policyName,
            new OperationRateLimitingContext { Parameter = parameter },
            cancellationToken);
    }

    public static Task ReleaseAsync(
        this IOperationRateLimitingChecker checker,
        string policyName,
        string? parameter,
        CancellationToken cancellationToken = default)
    {
        return checker.ReleaseAsync(
            policyName,
            new OperationRateLimitingContext { Parameter = parameter },
            cancellationToken);
    }

    public static Task<IOperationRateLimitingLease> AcquireAsync(
        this IOperationRateLimitingChecker checker,
        string policyName,
        string? parameter,
        CancellationToken cancellationToken = default)
    {
        return checker.AcquireAsync(
            policyName,
            new OperationRateLimitingContext { Parameter = parameter },
            cancellationToken);
    }
}
