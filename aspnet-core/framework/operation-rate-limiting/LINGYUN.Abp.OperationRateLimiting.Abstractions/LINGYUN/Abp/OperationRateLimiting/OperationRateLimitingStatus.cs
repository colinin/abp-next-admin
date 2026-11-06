using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Detailed status of a policy check.
/// </summary>
public class OperationRateLimitingStatus
{
    public string? PolicyName { get; set; }

    /// <summary>
    /// True when every rule allows the next request.
    /// </summary>
    public bool IsAllowed { get; set; }

    public int MaxCount { get; set; }

    public int CurrentCount { get; set; }

    public int RemainingCount { get; set; }

    /// <summary>
    /// Time until the binding rule resets.
    /// </summary>
    public TimeSpan? RetryAfter { get; set; }

    public TimeSpan WindowDuration { get; set; }

    public IReadOnlyList<OperationRateLimitingRuleStatus> Rules { get; set; } = Array.Empty<OperationRateLimitingRuleStatus>();

    /// <summary>
    /// The status returned when rate limiting is globally disabled.
    /// </summary>
    public static OperationRateLimitingStatus Disabled(string? policyName = null)
    {
        return new OperationRateLimitingStatus
        {
            PolicyName = policyName,
            IsAllowed = true,
            MaxCount = int.MaxValue,
            CurrentCount = 0,
            RemainingCount = int.MaxValue,
            RetryAfter = null,
            WindowDuration = TimeSpan.Zero,
            Rules = Array.Empty<OperationRateLimitingRuleStatus>()
        };
    }
}
