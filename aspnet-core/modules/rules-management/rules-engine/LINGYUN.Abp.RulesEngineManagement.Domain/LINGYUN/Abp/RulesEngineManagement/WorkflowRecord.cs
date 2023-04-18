using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesEngineManagement;

public class WorkflowRecord : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual string Name { get; protected set; }
    public virtual string TypeFullName { get; protected set; }
    public virtual string InjectWorkflows { get; protected set; }
    public virtual ICollection<WorkflowParamRecord> GlobalParams { get; protected set; }
    public virtual ICollection<WorkflowRuleRecord> Rules { get; protected set; }
    protected WorkflowRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public WorkflowRecord(
        Guid id,
        string name, 
        string typeFullName, 
        string injectWorkflows = null,
        Guid? tenantId = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), WorkflowRecordConsts.MaxNameLength);
        TypeFullName = Check.NotNullOrWhiteSpace(typeFullName, nameof(typeFullName), WorkflowRecordConsts.MaxTypeFullNameLength);
        InjectWorkflows = Check.Length(injectWorkflows, nameof(injectWorkflows), WorkflowRecordConsts.MaxInjectWorkflowsLength);

        TenantId = tenantId;

        GlobalParams = new Collection<WorkflowParamRecord>();
        Rules = new Collection<WorkflowRuleRecord>();

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }
}
