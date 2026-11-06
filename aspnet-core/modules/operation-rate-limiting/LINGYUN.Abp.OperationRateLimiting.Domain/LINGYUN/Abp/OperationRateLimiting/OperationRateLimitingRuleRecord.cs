using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Persisted rule of an operation rate limiting policy.
/// </summary>
public class OperationRateLimitingRuleRecord : Entity<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; internal set; }

    public Guid PolicyId { get; internal set; }

    /// <summary>
    /// Optional stable rule name used as the counter key.
    /// </summary>
    public string? Name { get; set; }

    public string Algorithm { get; set; } = default!;

    public TimeSpan Duration { get; set; }

    public int MaxCount { get; set; } = 1;

    /// <summary>
    /// Order of the rule inside the policy.
    /// </summary>
    public int SortOrder { get; set; }

    public OperationRateLimitingPartitionType PartitionType { get; set; }

    public string? PartitionResolverName { get; set; }

    public bool IsMultiTenancyEnabled { get; set; }

    public string? ErrorCode { get; set; }

    /// <summary>
    /// Algorithm specific values (for example <c>SlidingWindow:Segments</c>).
    /// </summary>
    public Dictionary<string, string> Properties { get; set; } = new();

    protected OperationRateLimitingRuleRecord()
    {
    }

    public OperationRateLimitingRuleRecord(
        Guid id, 
        Guid policyId, 
        string? name,
        string algorithm, 
        Guid? tenantId = null)
        : base(id)
    {
        Name = name;
        PolicyId = policyId;
        Algorithm = algorithm;
        TenantId = tenantId;
    }
}
