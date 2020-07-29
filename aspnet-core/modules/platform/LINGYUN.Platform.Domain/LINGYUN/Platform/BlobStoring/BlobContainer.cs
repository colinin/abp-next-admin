using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.BlobStoring
{
    public class BlobContainer : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual string Name { get; protected set; }
        public BlobContainer(Guid id, [NotNull] string name, Guid? tenantId = null)
            : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), BlobContainerConsts.MaxNameLength);
            TenantId = tenantId;
        }

    }
}
