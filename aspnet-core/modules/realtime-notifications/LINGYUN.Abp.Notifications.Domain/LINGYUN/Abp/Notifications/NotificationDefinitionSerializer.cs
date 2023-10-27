using JetBrains.Annotations;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.Notifications;
public class NotificationDefinitionSerializer : INotificationDefinitionSerializer, ITransientDependency
{
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public NotificationDefinitionSerializer(
        IGuidGenerator guidGenerator,
        ISimpleStateCheckerSerializer stateCheckerSerializer,
        ILocalizableStringSerializer localizableStringSerializer)
    {
        StateCheckerSerializer = stateCheckerSerializer;
        LocalizableStringSerializer = localizableStringSerializer;
        GuidGenerator = guidGenerator;
    }

    public async virtual Task<(NotificationDefinitionGroupRecord[], NotificationDefinitionRecord[])>
        SerializeAsync(IEnumerable<NotificationGroupDefinition> notificationGroups)
    {
        var notificationGroupRecords = new List<NotificationDefinitionGroupRecord>();
        var notificationRecords = new List<NotificationDefinitionRecord>();

        foreach (var notificationGroup in notificationGroups)
        {
            notificationGroupRecords.Add(await SerializeAsync(notificationGroup));

            foreach (var notification in notificationGroup.Notifications)
            {
                notificationRecords.Add(await SerializeAsync(notification, notificationGroup));
            }
        }

        return (notificationGroupRecords.ToArray(), notificationRecords.ToArray());
    }

    public virtual Task<NotificationDefinitionGroupRecord> SerializeAsync(NotificationGroupDefinition notificationGroup)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var permissionGroupRecord = new NotificationDefinitionGroupRecord(
                GuidGenerator.Create(),
                notificationGroup.Name,
                LocalizableStringSerializer.Serialize(notificationGroup.DisplayName)
            );

            foreach (var property in notificationGroup.Properties)
            {
                permissionGroupRecord.SetProperty(property.Key, property.Value);
            }

            return Task.FromResult(permissionGroupRecord);
        }
    }

    public virtual Task<NotificationDefinitionRecord> SerializeAsync(
        NotificationDefinition notification, 
        [CanBeNull] NotificationGroupDefinition notificationGroup)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var notificationRecord = new NotificationDefinitionRecord(
                GuidGenerator.Create(),
                notification.Name,
                notificationGroup?.Name,
                LocalizableStringSerializer.Serialize(notification.DisplayName),
                LocalizableStringSerializer.Serialize(notification.Description),
                notification.Template?.Name,
                notification.NotificationLifetime,
                notification.NotificationType,
                notification.ContentType
            );

            foreach (var property in notification.Properties)
            {
                notificationRecord.SetProperty(property.Key, property.Value);
            }

            return Task.FromResult(notificationRecord);
        }
    }

    protected virtual string SerializeProviders(ICollection<string> providers)
    {
        return providers.Any()
            ? providers.JoinAsString(",")
            : null;
    }
}
