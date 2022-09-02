using LINGYUN.Abp.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.MessageService.Notifications;

public interface IDynamicNotificationDefinitionCache
{
    Task<NotificationDefinition> GetOrNullAsync(string name);

    Task<IReadOnlyList<NotificationDefinition>> GetNotificationsAsync();

    Task<IReadOnlyList<NotificationGroupDefinition>> GetGroupsAsync();
}
