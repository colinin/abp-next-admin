using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesEngineManagement;
public class RuleActionRecord : Entity<int>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual Guid ActionId { get; protected set; }
    public virtual Guid RuleId { get; protected set; }
    public virtual ActionType ActionType { get; protected set; }
    protected RuleActionRecord() { }
    public RuleActionRecord(
        Guid ruleId,
        Guid actionId,
        ActionType actionType,
        Guid? tenantId = null)
    {
        RuleId = ruleId;
        ActionId = actionId;
        ActionType = actionType;
        TenantId = tenantId;
    }
}
