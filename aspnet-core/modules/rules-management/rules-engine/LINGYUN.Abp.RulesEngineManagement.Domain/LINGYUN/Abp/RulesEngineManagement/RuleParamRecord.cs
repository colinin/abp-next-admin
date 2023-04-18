using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesEngineManagement;
public class RuleParamRecord : Entity<int>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual Guid RuleId { get; protected set; }
    public virtual Guid ParamId { get; protected set; }
    protected RuleParamRecord() { }
    public RuleParamRecord(Guid ruleId, Guid paramId, Guid? tenantId = null)
    {
        TenantId = tenantId;
        RuleId = ruleId;
        ParamId = paramId;
    }
}
