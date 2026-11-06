using System;
using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Definition of an operation rate limiting policy.
/// </summary>
public class OperationRateLimitingPolicy
{
    /// <summary>
    /// Unique name of the policy.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Optional display name.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Optional error code used when this policy denies a request.
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Global switch of the policy.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Rules of the policy. All rules must allow the request (AND logic).
    /// </summary>
    public List<OperationRateLimitingRuleDefinition> Rules { get; set; } = new();

    public OperationRateLimitingPolicy()
    {
    }

    public OperationRateLimitingPolicy(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Returns true if the policy contains at least one concurrency rule, so a permit
    /// acquired by a check can be released later.
    /// </summary>
    public virtual bool HasConcurrencyRule()
    {
        return Rules.Any(rule => string.Equals(
            rule.Algorithm,
            OperationRateLimitingAlgorithms.Concurrency,
            StringComparison.OrdinalIgnoreCase));
    }

    public virtual OperationRateLimitingPolicy Clone()
    {
        var clone = (OperationRateLimitingPolicy)MemberwiseClone();
        clone.Rules = Rules.Select(rule => rule.Clone()).ToList();
        return clone;
    }
}
