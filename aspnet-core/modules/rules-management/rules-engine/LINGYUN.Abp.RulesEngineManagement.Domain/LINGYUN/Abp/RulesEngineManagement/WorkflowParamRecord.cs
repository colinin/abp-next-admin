using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesEngineManagement;
public class WorkflowParamRecord : Entity<int>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual Guid WorkflowId { get; protected set; }
    public virtual Guid ParamId { get; protected set; }
    protected WorkflowParamRecord() { }
    public WorkflowParamRecord(Guid workflowId, Guid paramId, Guid? tenantId = null)
    {
        TenantId = tenantId;
        WorkflowId = workflowId;
        ParamId = paramId;
    }
}
