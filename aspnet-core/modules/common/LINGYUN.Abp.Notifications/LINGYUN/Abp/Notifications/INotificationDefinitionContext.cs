using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications
{
    public interface INotificationDefinitionContext
    {
        NotificationGroupDefinition AddGroup(
            [NotNull] string name, 
            ILocalizableString displayName = null,
            bool allowSubscriptionToClients = true);

        NotificationGroupDefinition GetGroupOrNull(string name);

        void RemoveGroup(string name);
    }
}
