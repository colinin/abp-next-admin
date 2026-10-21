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
    public virtual string Name { get; protected set; } = default!;
    public virtual string Provider { get; protected set; } = default!;
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

    /// <summary>
    /// 设置容器名称
    /// </summary>
    public void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), BlobContainerConsts.MaxNameLength);

        // 容器名作为一级目录名参与路径组合
        BlobManagementNameValidator.CheckName(Name, BlobManagementErrorCodes.Container.InvalidName);
    }

    public void SetProvider(string provider)
    {
        Provider = Check.NotNullOrWhiteSpace(provider, nameof(provider), BlobContainerConsts.MaxProviderLength);
    }
}
