using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobManagement;

public class BlobContainer : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual string Name { get; protected set; }
    public virtual string Provider { get; protected set; }
    protected BlobContainer()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    internal BlobContainer(
        Guid id, 
        [NotNull] string name, 
        Guid? tenantId = null)
        : base(id)
    {
        SetName(name);
        TenantId = tenantId;
    }

    public void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), BlobContainerConsts.MaxNameLength);
    }

    public void SetProvider(string provider)
    {
        Provider = Check.NotNullOrWhiteSpace(provider, nameof(provider), BlobContainerConsts.MaxProviderLength);
    }
}
