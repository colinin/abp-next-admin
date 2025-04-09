using LINGYUN.Abp.DataProtection;
using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.DataProtectionManagement;

public class SubjectStrategy : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual bool IsEnabled { get; set; }
    public virtual Guid? TenantId { get; protected set; }
    public virtual string SubjectName { get; protected set; }
    public virtual string SubjectId { get; protected set; }
    public virtual DataAccessStrategy Strategy { get; set; }
    protected SubjectStrategy()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public SubjectStrategy(
        Guid id,
        string subjectName, 
        string subjectId, 
        DataAccessStrategy strategy,
        Guid? tenantId = null)
        : base(id)
    {
        IsEnabled = true;

        SubjectName = Check.NotNullOrWhiteSpace(subjectName, nameof(subjectName), SubjectStrategyConsts.MaxSubjectNameLength);
        SubjectId = Check.NotNullOrWhiteSpace(subjectId, nameof(subjectId), SubjectStrategyConsts.MaxSubjectIdLength);
        Strategy = strategy;
        TenantId = tenantId;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }
}
