using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public class Subscribe : CreationAuditedEntity<long>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual string EventType { get; protected set; }
        public virtual string RoleName { get; protected set; }
    }
}
