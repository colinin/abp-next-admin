using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Persisted definition of an operation rate limiting policy.
/// </summary>
/// <remarks>
/// Rules are stored in the <see cref="OperationRateLimitingRuleRecord"/> child table and merged
/// with the statically defined policies by <see cref="IOperationRateLimitingPolicyManager"/>.
/// </remarks>
public class OperationRateLimitingPolicyRecord : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Unique (per tenant) name of the policy.
    /// </summary>
    public virtual string Name { get; protected set; } = default!;

    public virtual string? DisplayName { get; set; }

    public virtual bool IsEnabled { get; set; } = true;

    public virtual string? ErrorCode { get; set; }

    /// <summary>
    /// Rules of the policy.
    /// </summary>
    public virtual ICollection<OperationRateLimitingRuleRecord> Rules { get; protected set; }

    protected OperationRateLimitingPolicyRecord()
    {
        Rules = new Collection<OperationRateLimitingRuleRecord>();
    }

    public OperationRateLimitingPolicyRecord(Guid id, string name, Guid? tenantId = null)
        : base(id)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        Name = name;
        TenantId = tenantId;

        Rules = new Collection<OperationRateLimitingRuleRecord>();
    }

    /// <summary>
    /// Replaces all rules of the policy. The caller owns the new rule instances.
    /// </summary>
    public virtual void SetRules(IEnumerable<OperationRateLimitingRuleRecord> rules)
    {
        Rules.Clear();

        foreach (var rule in rules)
        {
            rule.PolicyId = Id;
            Rules.Add(rule);
        }
    }
}
