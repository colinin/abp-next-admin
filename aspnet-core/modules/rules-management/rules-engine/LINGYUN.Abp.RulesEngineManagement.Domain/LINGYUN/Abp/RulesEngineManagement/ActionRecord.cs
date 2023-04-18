using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesEngineManagement;

public class ActionRecord : Entity<Guid>, IMultiTenant, IHasExtraProperties
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual string Name { get; protected set; }
    public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }
    protected ActionRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }
    public ActionRecord(
        Guid id, 
        string name,
        Guid? tenantId = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), ActionRecordConsts.MaxNameLength);

        TenantId = tenantId;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }
}
