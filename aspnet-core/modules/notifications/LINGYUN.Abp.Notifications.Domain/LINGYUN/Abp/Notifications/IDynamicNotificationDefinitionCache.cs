using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications;

public interface IDynamicNotificationDefinitionCache
{
    Task<NotificationDefinition> GetOrNullAsync(string name);

    Task<IReadOnlyList<NotificationDefinition>> GetNotificationsAsync();

    Task<IReadOnlyList<NotificationGroupDefinition>> GetGroupsAsync();
}
