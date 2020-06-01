using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications
{
    public interface INotificationPublisher
    {
        Task PublishAsync(NotificationInfo notification, IEnumerable<Guid> userIds);
    }
}
