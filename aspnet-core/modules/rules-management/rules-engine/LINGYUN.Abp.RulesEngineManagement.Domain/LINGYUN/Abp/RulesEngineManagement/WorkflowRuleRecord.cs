using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesEngineManagement;
public class WorkflowRuleRecord : Entity<int>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual Guid RuleId { get; protected set; }
    public virtual Guid WorkflowId { get; protected set; }
    protected WorkflowRuleRecord() { }
    public WorkflowRuleRecord(Guid workflowId, Guid ruleId, Guid? tenantId = null)
    {
        WorkflowId = workflowId;
        RuleId = ruleId;
        TenantId = tenantId;
    }
}
