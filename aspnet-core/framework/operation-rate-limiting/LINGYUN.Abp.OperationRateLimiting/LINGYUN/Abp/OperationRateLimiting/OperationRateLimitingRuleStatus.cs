using System;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Detailed status of one rule of a policy.
/// </summary>
public class OperationRateLimitingRuleStatus
{
    public string? RuleName { get; set; }

    public bool IsAllowed { get; set; }

    public int MaxCount { get; set; }

    public int CurrentCount { get; set; }

    public int RemainingCount { get; set; }

    public TimeSpan? RetryAfter { get; set; }

    public TimeSpan WindowDuration { get; set; }

    public bool IsPermanent { get; set; }

    /// <summary>
    /// Error code configured on the rule (if any).
    /// </summary>
    public string? ErrorCode { get; set; }
}
