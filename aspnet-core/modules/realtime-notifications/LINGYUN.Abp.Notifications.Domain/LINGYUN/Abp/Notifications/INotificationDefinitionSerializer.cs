using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications;

public interface INotificationDefinitionSerializer
{
    Task<(NotificationDefinitionGroupRecord[], NotificationDefinitionRecord[])>
        SerializeAsync(IEnumerable<NotificationGroupDefinition> NotificationGroups);

    Task<NotificationDefinitionGroupRecord> SerializeAsync(
        NotificationGroupDefinition NotificationGroup);

    Task<NotificationDefinitionRecord> SerializeAsync(
        NotificationDefinition Notification,
        [CanBeNull] NotificationGroupDefinition NotificationGroup);
}
