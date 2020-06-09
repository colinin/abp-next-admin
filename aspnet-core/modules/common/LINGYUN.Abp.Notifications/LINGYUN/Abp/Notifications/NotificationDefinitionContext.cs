using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationDefinitionContext : INotificationDefinitionContext
    {
        protected Dictionary<string, NotificationDefinition> Notifications { get; }

        public NotificationDefinitionContext(Dictionary<string, NotificationDefinition> notifications)
        {
            Notifications = notifications;
        }

        public void Add(params NotificationDefinition[] definitions)
        {
            if (definitions.IsNullOrEmpty())
            {
                return;
            }

            foreach (var definition in definitions)
            {
                Notifications[definition.Name] = definition;
            }
        }

        public NotificationDefinition GetOrNull(string name)
        {
            return Notifications.GetOrDefault(name);
        }
    }
}
