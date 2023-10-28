using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications;

public class StaticNotificationDefinitionStore : IStaticNotificationDefinitionStore, ISingletonDependency
{
    protected IDictionary<string, NotificationGroupDefinition> NotificationGroupDefinitions => _lazyNotificationGroupDefinitions.Value;
    private readonly Lazy<Dictionary<string, NotificationGroupDefinition>> _lazyNotificationGroupDefinitions;

    protected IDictionary<string, NotificationDefinition> NotificationDefinitions => _lazyNotificationDefinitions.Value;
    private readonly Lazy<Dictionary<string, NotificationDefinition>> _lazyNotificationDefinitions;

    protected AbpNotificationsOptions Options { get; }

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StaticNotificationDefinitionStore(
        IServiceScopeFactory serviceScopeFactory,
        IOptions<AbpNotificationsOptions> options)
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

    public virtual Task<NotificationDefinition> GetOrNullAsync(string name)
    {
        return Task.FromResult(NotificationDefinitions.GetOrDefault(name));
    }

    public virtual Task<IReadOnlyList<NotificationDefinition>> GetNotificationsAsync()
    {
        return Task.FromResult<IReadOnlyList<NotificationDefinition>>(
            NotificationDefinitions.Values.ToImmutableList()
        );
    }

    public virtual Task<NotificationGroupDefinition> GetGroupOrNullAsync(string name)
    {
        return Task.FromResult(NotificationGroupDefinitions.GetOrDefault(name));
    }

    public virtual Task<IReadOnlyList<NotificationGroupDefinition>> GetGroupsAsync()
    {
        return Task.FromResult<IReadOnlyList<NotificationGroupDefinition>>(
            NotificationGroupDefinitions.Values.ToImmutableList()
        );
    }
}
