using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications
{
    public interface INotificationPublisher
    {
        Task PublishAsync(NotificationData data, IEnumerable<Guid> userIds, Guid? tenantId);
    }
}
