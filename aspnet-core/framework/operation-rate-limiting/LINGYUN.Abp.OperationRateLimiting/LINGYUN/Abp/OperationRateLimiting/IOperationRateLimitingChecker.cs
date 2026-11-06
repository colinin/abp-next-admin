using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Entry point used by application code to check operation rate limits.
/// </summary>
public interface IOperationRateLimitingChecker
{
    /// <summary>
    /// Checks the limit and increments the counters when every rule allows the request.
    /// Throws <see cref="AbpOperationRateLimitingException"/> (HTTP 429) when a limit is exceeded.
    /// </summary>
    Task<OperationRateLimitingStatus> CheckAsync(
        string policyName,
        OperationRateLimitingContext? context = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Read-only check: returns true when the next request would be allowed. Does not increment counters.
    /// </summary>
    Task<bool> IsAllowedAsync(
        string policyName,
        OperationRateLimitingContext? context = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the detailed status without incrementing counters.
    /// </summary>
    Task<OperationRateLimitingStatus> GetStatusAsync(
        string policyName,
        OperationRateLimitingContext? context = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets the counters of the policy for the given context.
    /// </summary>
    Task ResetAsync(
        string policyName,
        OperationRateLimitingContext? context = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Releases a permit acquired by a concurrency rule (no-op for other algorithms).
    /// </summary>
    Task ReleaseAsync(
        string policyName,
        OperationRateLimitingContext? context = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks the limit and returns a lease that releases the concurrency permits when disposed.
    /// </summary>
    Task<IOperationRateLimitingLease> AcquireAsync(
        string policyName,
        OperationRateLimitingContext? context = null,
        CancellationToken cancellationToken = default);
}
