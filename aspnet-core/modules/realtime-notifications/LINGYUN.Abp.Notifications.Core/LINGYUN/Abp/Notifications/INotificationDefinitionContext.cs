using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Notifications;

public interface INotificationDefinitionContext
{
    NotificationGroupDefinition AddGroup(
        [NotNull] string name, 
        ILocalizableString displayName = null,
        ILocalizableString description = null,
        bool allowSubscriptionToClients = true);

    NotificationGroupDefinition GetGroupOrNull(string name);

    void RemoveGroup(string name);
}
