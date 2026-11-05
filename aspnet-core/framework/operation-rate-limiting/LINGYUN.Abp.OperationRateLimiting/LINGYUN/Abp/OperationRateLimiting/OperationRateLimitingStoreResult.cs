using System;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Result of a single rule check.
/// </summary>
public class OperationRateLimitingStoreResult
{
    public bool IsAllowed { get; set; }

    public int MaxCount { get; set; }

    public int CurrentCount { get; set; }

    public int RemainingCount { get; set; }

    /// <summary>
    /// Time until the limit resets. Null for permanent bans and for algorithms that cannot
    /// estimate it.
    /// </summary>
    public TimeSpan? RetryAfter { get; set; }

    /// <summary>
    /// Total window/lease duration of the rule.
    /// </summary>
    public TimeSpan WindowDuration { get; set; }

    /// <summary>
    /// True when the rule is a permanent ban (<c>maxCount: 0</c>).
    /// </summary>
    public bool IsPermanent { get; set; }

    public static OperationRateLimitingStoreResult Allow(
        int maxCount,
        int currentCount,
        TimeSpan? retryAfter = null,
        TimeSpan? windowDuration = null)
    {
        return new OperationRateLimitingStoreResult
        {
            IsAllowed = true,
            MaxCount = maxCount,
            CurrentCount = currentCount,
            RemainingCount = Math.Max(0, maxCount - currentCount),
            RetryAfter = retryAfter,
            WindowDuration = windowDuration ?? TimeSpan.Zero
        };
    }

    public static OperationRateLimitingStoreResult Deny(
        int maxCount,
        int currentCount,
        TimeSpan? retryAfter,
        TimeSpan? windowDuration = null,
        bool isPermanent = false)
    {
        return new OperationRateLimitingStoreResult
        {
            IsAllowed = false,
            MaxCount = maxCount,
            CurrentCount = currentCount,
            RemainingCount = 0,
            RetryAfter = isPermanent ? null : retryAfter,
            WindowDuration = windowDuration ?? TimeSpan.Zero,
            IsPermanent = isPermanent
        };
    }
}
