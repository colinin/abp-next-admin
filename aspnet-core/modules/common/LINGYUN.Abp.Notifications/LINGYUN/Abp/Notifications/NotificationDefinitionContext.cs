using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationDefinitionContext : INotificationDefinitionContext
    {
        internal Dictionary<string, NotificationGroupDefinition> Groups { get; }

        public NotificationDefinitionContext()
        {
            Groups = new Dictionary<string, NotificationGroupDefinition>();
        }

        public NotificationGroupDefinition AddGroup(
            [NotNull] string name, 
            ILocalizableString displayName = null,
            bool allowSubscriptionToClients = true)
        {
            Check.NotNull(name, nameof(name));

            if (Groups.ContainsKey(name))
            {
                throw new AbpException($"There is already an existing notification group with name: {name}");
            }

            return Groups[name] = new NotificationGroupDefinition(name, displayName, allowSubscriptionToClients);
        }

        public NotificationGroupDefinition GetGroupOrNull(string name)
        {
            Check.NotNull(name, nameof(name));

            if (!Groups.ContainsKey(name))
            {
                return null;
            }

            return Groups[name];
        }

        public void RemoveGroup(string name)
        {
            Check.NotNull(name, nameof(name));

            if (!Groups.ContainsKey(name))
            {
                throw new AbpException($"Undefined notification group: '{name}'.");
            }

            Groups.Remove(name);
        }
    }
}
