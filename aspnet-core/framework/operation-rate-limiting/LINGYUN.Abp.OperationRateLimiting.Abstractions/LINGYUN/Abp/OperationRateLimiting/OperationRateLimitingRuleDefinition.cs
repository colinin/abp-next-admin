using System;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Definition of a rate limiting rule.
/// </summary>
/// <remarks>
/// The definition only contains algorithm independent properties. <see cref="Duration"/> and
/// <see cref="MaxCount"/> are common to every algorithm; other algorithm specific values
/// (segments, tokens per period, ...) are stored in <see cref="Properties"/> with the algorithm
/// name as prefix (for example <c>SlidingWindow:Segments</c>) and are read/written through the
/// extension methods of the corresponding algorithm namespace.
/// </remarks>
public class OperationRateLimitingRuleDefinition
{
    /// <summary>
    /// Optional unique (per policy) name of the rule.
    /// When set, it is used as the counter key, so changing the rule parameters
    /// does not reset existing counters.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Name of the algorithm executed by this rule. See <see cref="OperationRateLimitingAlgorithms"/>.
    /// </summary>
    public string Algorithm { get; set; } = default!;

    /// <summary>
    /// Window duration (fixed/sliding window), replenishment period (token bucket)
    /// or lease duration (concurrency), depending on the algorithm.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Maximum number of requests (or permits). A value of 0 creates a permanent ban policy.
    /// </summary>
    public int MaxCount { get; set; } = 1;

    /// <summary>
    /// Partition strategy of the rule.
    /// </summary>
    public OperationRateLimitingPartitionType PartitionType { get; set; }

    /// <summary>
    /// Name of the custom partition key resolver (only when <see cref="PartitionType"/> is
    /// <see cref="OperationRateLimitingPartitionType.Custom"/>).
    /// </summary>
    public string? PartitionResolverName { get; set; }

    /// <summary>
    /// When true, the counter key includes the current tenant identifier.
    /// </summary>
    public bool IsMultiTenancyEnabled { get; set; }

    /// <summary>
    /// Optional error code used when this rule denies a request.
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Algorithm specific values, keyed with the algorithm name as prefix
    /// (for example <c>FixedWindow:Duration</c>). Custom algorithms can use their own keys.
    /// </summary>
    public Dictionary<string, string> Properties { get; set; } = new();

    public OperationRateLimitingRuleDefinition()
        : this(null!)
    {
    }

    public OperationRateLimitingRuleDefinition(
        string name, 
        OperationRateLimitingPartitionType partitionType = OperationRateLimitingPartitionType.Parameter)
    {
        Name = name;
        PartitionType = partitionType;

        Properties = new Dictionary<string, string>(StringComparer.Ordinal);
    }



    /// <summary>
    /// Adds or updates a property.
    /// </summary>
    public virtual OperationRateLimitingRuleDefinition WithProperty(string key, string value)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));
        Check.NotNull(value, nameof(value));

        Properties[key] = value;

        return this;
    }

    /// <summary>
    /// Gets a property value or null.
    /// </summary>
    public virtual string? GetPropertyOrNull(string key)
    {
        return Properties.TryGetValue(key, out var value) ? value : null;
    }

    /// <summary>
    /// Creates a copy of the definition.
    /// </summary>
    public virtual OperationRateLimitingRuleDefinition Clone()
    {
        var clone = (OperationRateLimitingRuleDefinition)MemberwiseClone();
        clone.Properties = new Dictionary<string, string>(Properties, StringComparer.Ordinal);
        return clone;
    }
}
