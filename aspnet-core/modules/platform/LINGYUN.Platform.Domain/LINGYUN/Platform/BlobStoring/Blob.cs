using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.BlobStoring
{
    public class Blob : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid ContainerId { get; protected set; }
        public virtual Guid? TenantId { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Sha256 { get; protected set; }
        public virtual long Size { get; protected set; }
        protected Blob() { }
        public Blob(Guid id, Guid containerId, [NotNull] string name, [NotNull] string sha256, long size, Guid? tenantId)
        {
            Id = id;
            ContainerId = containerId;
            ChangeFile(name, sha256, size);
            TenantId = tenantId;
        }

        public void ChangeFile(string name, string sha256, long size)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), BlobConsts.MaxNameLength);
            Sha256 = Check.NotNullOrWhiteSpace(sha256, nameof(sha256), BlobConsts.MaxSha256Length);
            Size = size;
        }
    }
}
