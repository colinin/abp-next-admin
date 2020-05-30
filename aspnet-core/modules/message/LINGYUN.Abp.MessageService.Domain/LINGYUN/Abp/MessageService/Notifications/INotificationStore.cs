using LINGYUN.Abp.Notifications;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public interface INotificationStore
    {
        Task InsertUserNotificationAsync(NotificationInfo notification, Guid userId);
    }
}
