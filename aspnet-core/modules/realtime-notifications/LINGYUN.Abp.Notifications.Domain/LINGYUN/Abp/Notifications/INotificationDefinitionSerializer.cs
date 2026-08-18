using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications;

public interface INotificationDefinitionSerializer
{
    Task<(NotificationDefinitionGroupRecord[], NotificationDefinitionRecord[])>
        SerializeAsync(IEnumerable<NotificationGroupDefinition> notificationGroups);

    Task<NotificationDefinitionGroupRecord> SerializeAsync(
        NotificationGroupDefinition notificationGroup);

    Task<NotificationDefinitionRecord> SerializeAsync(
        NotificationDefinition notification,
        NotificationGroupDefinition notificationGroup);
}
