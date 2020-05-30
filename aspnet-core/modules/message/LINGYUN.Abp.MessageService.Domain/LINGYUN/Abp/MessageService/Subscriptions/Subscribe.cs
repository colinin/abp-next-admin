using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public abstract class Subscribe : Entity<long>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual string NotificationName { get; protected set; }

        protected Subscribe() { }

        protected Subscribe(string notificationName)
        {
            NotificationName = notificationName;
        }
    }
}
