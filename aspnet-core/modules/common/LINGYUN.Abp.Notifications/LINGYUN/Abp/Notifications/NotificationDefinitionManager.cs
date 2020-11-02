using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationDefinitionManager : INotificationDefinitionManager, ISingletonDependency
    {
        protected AbpNotificationOptions Options { get; }

        protected IDictionary<string, NotificationGroupDefinition> NotificationGroupDefinitions => _lazyNotificationGroupDefinitions.Value;
        private readonly Lazy<Dictionary<string, NotificationGroupDefinition>> _lazyNotificationGroupDefinitions;

        protected IDictionary<string, NotificationDefinition> NotificationDefinitions => _lazyNotificationDefinitions.Value;
        private readonly Lazy<Dictionary<string, NotificationDefinition>> _lazyNotificationDefinitions;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public NotificationDefinitionManager(
            IOptions<AbpNotificationOptions> options,
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            Options = options.Value;

            _lazyNotificationDefinitions = new Lazy<Dictionary<string, NotificationDefinition>>(
                    CreateNotificationDefinitions,
                    isThreadSafe: true
                );

            _lazyNotificationGroupDefinitions = new Lazy<Dictionary<string, NotificationGroupDefinition>>(
                CreateNotificationGroupDefinitions,
                isThreadSafe: true
            );
        }

        public virtual NotificationDefinition Get(string name)
        {
            Check.NotNull(name, nameof(name));

            var feature = GetOrNull(name);

            if (feature == null)
            {
                throw new AbpException("Undefined notification: " + name);
            }

            return feature;
        }

        public virtual IReadOnlyList<NotificationDefinition> GetAll()
        {
            return NotificationDefinitions.Values.ToImmutableList();
        }

        public virtual NotificationDefinition GetOrNull(string name)
        {
            return NotificationDefinitions.GetOrDefault(name);
        }

        public IReadOnlyList<NotificationGroupDefinition> GetGroups()
        {
            return NotificationGroupDefinitions.Values.ToImmutableList();
        }

        protected virtual Dictionary<string, NotificationDefinition> CreateNotificationDefinitions()
        {
            var notifications = new Dictionary<string, NotificationDefinition>();

            foreach (var groupDefinition in NotificationGroupDefinitions.Values)
            {
                foreach (var notification in groupDefinition.Notifications)
                {
                    if (notifications.ContainsKey(notification.Name))
                    {
                        throw new AbpException("Duplicate notification name: " + notification.Name);
                    }

                    notifications[notification.Name] = notification;
                }
            }

            return notifications;
        }

        protected virtual Dictionary<string, NotificationGroupDefinition> CreateNotificationGroupDefinitions()
        {
            var context = new NotificationDefinitionContext();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var providers = Options
                    .DefinitionProviders
                    .Select(p => scope.ServiceProvider.GetRequiredService(p) as INotificationDefinitionProvider)
                    .ToList();

                foreach (var provider in providers)
                {
                    provider.Define(context);
                }
            }

            return context.Groups;
        }
    }
}
