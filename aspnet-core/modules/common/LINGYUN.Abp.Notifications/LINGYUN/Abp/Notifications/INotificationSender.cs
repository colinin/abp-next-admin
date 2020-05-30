using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications
{
    public interface INotificationSender
    {
        Task SendAsync(NotificationData data, Guid userId, Guid? tenantId);
        Task SendAsync(NotificationData data, IEnumerable<Guid> userIds, Guid? tenantId);
    }
}
