using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

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

    public SemaphoreSlim SyncSemaphore { get; } = new(1, 1);

    public DateTime? LastCheckTime { get; set; }

    public DynamicNotificationDefinitionInMemoryCache(
        ILocalizableStringSerializer localizableStringSerializer)
    {
        LocalizableStringSerializer = localizableStringSerializer;

        NotificationGroupDefinitions = new Dictionary<string, NotificationGroupDefinition>();
        NotificationDefinitions = new Dictionary<string, NotificationDefinition>();
    }

    public Task FillAsync(
        List<NotificationDefinitionGroupRecord> notificationGroupRecords,
        List<NotificationDefinitionRecord> notificationRecords)
    {
        NotificationGroupDefinitions.Clear();
        NotificationDefinitions.Clear();

        var context = new NotificationDefinitionContext();

        foreach (var notificationGroupRecord in notificationGroupRecords)
        {
            var notificationGroup = context.AddGroup(
                notificationGroupRecord.Name,
                LocalizableStringSerializer.Deserialize(notificationGroupRecord.DisplayName),
                LocalizableStringSerializer.Deserialize(notificationGroupRecord.Description),
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

    public NotificationDefinition GetNotificationOrNull(string name)
    {
        return NotificationDefinitions.GetOrDefault(name);
    }

    public IReadOnlyList<NotificationDefinition> GetNotifications()
    {
        return NotificationDefinitions.Values.ToList();
    }

    public IReadOnlyList<NotificationGroupDefinition> GetGroups()
    {
        return NotificationGroupDefinitions.Values.ToList();
    }

    private void AddNotification(
        NotificationGroupDefinition notificationGroup,
        NotificationDefinitionRecord notificationRecord)
    {
        var notification = notificationGroup.AddNotification(
            notificationRecord.Name,
            LocalizableStringSerializer.Deserialize(notificationRecord.DisplayName),
            LocalizableStringSerializer.Deserialize(notificationRecord.Description),
            notificationRecord.NotificationType,
            notificationRecord.NotificationLifetime,
            notificationRecord.ContentType,
            notificationRecord.AllowSubscriptionToClients
        );

        NotificationDefinitions[notification.Name] = notification;

        if (!notificationRecord.Providers.IsNullOrWhiteSpace())
        {
            notification.Providers.AddRange(notificationRecord.Providers.Split(',', ';'));
        }

        foreach (var property in notificationRecord.ExtraProperties)
        {
            notification[property.Key] = property.Value;
        }
    }
}
