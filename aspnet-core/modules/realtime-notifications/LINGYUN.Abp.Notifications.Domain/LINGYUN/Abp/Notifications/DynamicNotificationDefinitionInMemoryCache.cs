using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Notifications;

[ExposeServices(
    typeof(IDynamicNotificationDefinitionStoreCache),
    typeof(DynamicNotificationDefinitionInMemoryCache))]
public class DynamicNotificationDefinitionInMemoryCache : IDynamicNotificationDefinitionStoreCache, ISingletonDependency
{
    public string CacheStamp { get; set; }

    protected IDictionary<string, NotificationGroupDefinition> NotificationGroupDefinitions { get; }
    protected IDictionary<string, NotificationDefinition> NotificationDefinitions { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }
    protected ITemplateDefinitionManager TemplateDefinitionManager { get; }

    public SemaphoreSlim SyncSemaphore { get; } = new(1, 1);

    public DateTime? LastCheckTime { get; set; }

    public DynamicNotificationDefinitionInMemoryCache(
        ILocalizableStringSerializer localizableStringSerializer,
        ITemplateDefinitionManager templateDefinitionManager)
    {
        LocalizableStringSerializer = localizableStringSerializer;
        TemplateDefinitionManager = templateDefinitionManager;

        NotificationGroupDefinitions = new Dictionary<string, NotificationGroupDefinition>();
        NotificationDefinitions = new Dictionary<string, NotificationDefinition>();
    }

    public virtual Task FillAsync(
        List<NotificationDefinitionGroupRecord> notificationGroupRecords,
        List<NotificationDefinitionRecord> notificationRecords)
    {
        NotificationGroupDefinitions.Clear();
        NotificationDefinitions.Clear();

        var context = new NotificationDefinitionContext();

        foreach (var notificationGroupRecord in notificationGroupRecords)
        {
            ILocalizableString description = null;
            if (!notificationGroupRecord.Description.IsNullOrWhiteSpace())
            {
                description = LocalizableStringSerializer.Deserialize(notificationGroupRecord.Description);
            }
            var notificationGroup = context.AddGroup(
                notificationGroupRecord.Name,
                LocalizableStringSerializer.Deserialize(notificationGroupRecord.DisplayName),
                description,
                notificationGroupRecord.AllowSubscriptionToClients
            );

            NotificationGroupDefinitions[notificationGroup.Name] = notificationGroup;

            foreach (var property in notificationGroupRecord.ExtraProperties)
            {
                notificationGroup[property.Key] = property.Value;
            }

            var notificationRecordsInThisGroup = notificationRecords
                .Where(p => p.GroupName == notificationGroup.Name);

            foreach (var notificationRecord in notificationRecordsInThisGroup)
            {
                AddNotification(notificationGroup, notificationRecord);
            }
        }

        return Task.CompletedTask;
    }

    public virtual NotificationDefinition GetNotificationOrNull(string name)
    {
        return NotificationDefinitions.GetOrDefault(name);
    }

    public virtual IReadOnlyList<NotificationDefinition> GetNotifications()
    {
        return NotificationDefinitions.Values.ToList();
    }

    public virtual NotificationGroupDefinition GetNotificationGroupOrNull(string name)
    {
        return NotificationGroupDefinitions.GetOrDefault(name);
    }

    public virtual IReadOnlyList<NotificationGroupDefinition> GetGroups()
    {
        return NotificationGroupDefinitions.Values.ToList();
    }

    private async void AddNotification(
        NotificationGroupDefinition notificationGroup,
        NotificationDefinitionRecord notificationRecord)
    {
        ILocalizableString description = null;
        if (!notificationRecord.Description.IsNullOrWhiteSpace())
        {
            description = LocalizableStringSerializer.Deserialize(notificationRecord.Description);
        }

        var notification = notificationGroup.AddNotification(
            notificationRecord.Name,
            LocalizableStringSerializer.Deserialize(notificationRecord.DisplayName),
            description,
            notificationRecord.NotificationType,
            notificationRecord.NotificationLifetime,
            notificationRecord.ContentType,
            notificationRecord.AllowSubscriptionToClients
        );

        NotificationDefinitions[notification.Name] = notification;

        if (!notificationRecord.Providers.IsNullOrWhiteSpace())
        {
            notification.Providers.AddRange(notificationRecord.Providers.Split(','));
        }

        if (!notificationRecord.Template.IsNullOrWhiteSpace())
        {
            var templateDefinition = await TemplateDefinitionManager.GetOrNullAsync(notificationRecord.Template);
            notification.WithTemplate(templateDefinition);
        }

        foreach (var property in notificationRecord.ExtraProperties)
        {
            notification[property.Key] = property.Value;
        }
    }
}
