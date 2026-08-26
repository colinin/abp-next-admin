using JetBrains.Annotations;
using LINGYUN.Abp.Dynamic.Definitions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications;

public class NotificationDefinitionManager : 
    DynamicDefinitionManager<NotificationGroupDefinition, NotificationDefinition>, 
    INotificationDefinitionManager,
    ITransientDependency
{
    protected IStaticNotificationDefinitionStore StaticStore { get; }
    protected IDynamicNotificationDefinitionStore DynamicStore { get; }

    public NotificationDefinitionManager(
        IStaticNotificationDefinitionStore staticStore,
        IDynamicNotificationDefinitionStore dynamicStore,
        IOptions<AbpDynamicDefinitionsOptions> options) : base(options)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
    }

    public async virtual Task<NotificationDefinition> GetAsync([NotNull] string name)
    {
        return await GetOrNullAsync(name) ?? throw new AbpException("Undefined notification: " + name);
    }

    public async virtual Task<NotificationGroupDefinition?> GetGroupOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var staticGroupDefinition = await StaticStore.GetGroupOrNullAsync(name);
        var dynamicGroupDefinition = await DynamicStore.GetGroupOrNullAsync(name);

        return await GetGroupDefinitionAsync(staticGroupDefinition, dynamicGroupDefinition);
    }

    public async virtual Task<IReadOnlyList<NotificationGroupDefinition>> GetGroupsAsync()
    {
        var staticGroupDefinitions = await StaticStore.GetGroupsAsync();
        var dynamicGroupDefinitions = await DynamicStore.GetGroupsAsync();

        return await GetGroupDefinitionsAsync(staticGroupDefinitions, dynamicGroupDefinitions);
    }

    public async virtual Task<IReadOnlyList<NotificationDefinition>> GetNotificationsAsync()
    {
        var staticDefinitions = await StaticStore.GetNotificationsAsync();
        var dynamicDefinitions = await DynamicStore.GetNotificationsAsync();

        return await GetDefinitionsAsync(staticDefinitions, dynamicDefinitions);
    }

    public async virtual Task<NotificationDefinition?> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var staticDefinition = await StaticStore.GetOrNullAsync(name);
        var dynamicDefinition = await DynamicStore.GetOrNullAsync(name);

        return await GetDefinitionAsync(staticDefinition, dynamicDefinition);
    }

    protected override string GetDefinitionKey(NotificationDefinition definition)
    {
        return definition.Name;
    }

    protected override Task<NotificationDefinition> MergeDefinitionAsync(NotificationDefinition targetDefinition, NotificationDefinition sourceDefinition)
    {
        var displayName = sourceDefinition.DisplayName ?? targetDefinition.DisplayName;
        var description = sourceDefinition.Description ?? targetDefinition.Description;
        var notificationType = sourceDefinition.NotificationType != NotificationType.Application
            ? sourceDefinition.NotificationType
            : targetDefinition.NotificationType;
        var lifetime = sourceDefinition.NotificationLifetime != NotificationLifetime.Persistent
            ? sourceDefinition.NotificationLifetime
            : targetDefinition.NotificationLifetime;
        var contentType = sourceDefinition.ContentType != NotificationContentType.Text
            ? sourceDefinition.ContentType
            : targetDefinition.ContentType;
        var allowSubscriptionToClients = sourceDefinition.AllowSubscriptionToClients || targetDefinition.AllowSubscriptionToClients;

        var mergedNotification = new NotificationDefinition(
            targetDefinition.Name,
            displayName,
            description,
            notificationType,
            lifetime,
            contentType,
            allowSubscriptionToClients
        );

        foreach (var property in targetDefinition.Properties)
        {
            mergedNotification.Properties[property.Key] = property.Value;
        }

        foreach (var property in sourceDefinition.Properties)
        {
            mergedNotification.Properties[property.Key] = property.Value;
        }

        foreach (var provider in targetDefinition.Providers)
        {
            if (!mergedNotification.Providers.Contains(provider))
            {
                mergedNotification.Providers.Add(provider);
            }
        }

        foreach (var provider in sourceDefinition.Providers)
        {
            if (!mergedNotification.Providers.Contains(provider))
            {
                mergedNotification.Providers.Add(provider);
            }
        }

        if (sourceDefinition.Template != null)
        {
            mergedNotification.WithTemplate(sourceDefinition.Template);
        }
        else if (targetDefinition.Template != null)
        {
            mergedNotification.WithTemplate(targetDefinition.Template);
        }

        return Task.FromResult(mergedNotification);
    }

    protected override string GetGroupDefinitionKey(NotificationGroupDefinition groupDefinition)
    {
        return groupDefinition.Name;
    }

    protected override Task MergeGroupDefinitionAsync(NotificationGroupDefinition targetGroupDefinition, NotificationGroupDefinition sourceGroupDefinition)
    {
        foreach (var sourceNotification in sourceGroupDefinition.Notifications)
        {
            var existingNotification = targetGroupDefinition.GetNotificationOrNull(sourceNotification.Name);

            if (existingNotification == null)
            {
                var newNotification = targetGroupDefinition.AddNotification(
                    sourceNotification.Name,
                    sourceNotification.DisplayName,
                    sourceNotification.Description,
                    sourceNotification.NotificationType,
                    sourceNotification.NotificationLifetime,
                    sourceNotification.ContentType,
                    sourceNotification.AllowSubscriptionToClients
                );

                foreach (var provider in sourceNotification.Providers)
                {
                    if (!newNotification.Providers.Contains(provider))
                    {
                        newNotification.Providers.Add(provider);
                    }
                }

                foreach (var property in sourceNotification.Properties)
                {
                    newNotification.Properties[property.Key] = property.Value;
                }

                if (sourceNotification.Template != null)
                {
                    newNotification.WithTemplate(sourceNotification.Template);
                }
            }
            else
            {
                foreach (var property in sourceNotification.Properties)
                {
                    existingNotification.Properties[property.Key] = property.Value;
                }

                foreach (var provider in sourceNotification.Providers)
                {
                    if (!existingNotification.Providers.Contains(provider))
                    {
                        existingNotification.Providers.Add(provider);
                    }
                }

                if (sourceNotification.DisplayName != null)
                {
                    existingNotification.DisplayName = sourceNotification.DisplayName;
                }

                if (sourceNotification.Description != null)
                {
                    existingNotification.Description = sourceNotification.Description;
                }

                if (sourceNotification.Template != null)
                {
                    existingNotification.WithTemplate(sourceNotification.Template);
                }

                existingNotification.AllowSubscriptionToClients =
                    existingNotification.AllowSubscriptionToClients || sourceNotification.AllowSubscriptionToClients;
            }
        }

        return Task.CompletedTask;
    }
}
