using Riok.Mapperly.Abstractions;
using System;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.Notifications;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class NotificationToNotificationInfoMapper : MapperBase<Notification, NotificationInfo>
{
    [MapProperty(nameof(Notification.NotificationId), nameof(NotificationInfo.Id))]
    [MapProperty(nameof(Notification.NotificationName), nameof(NotificationInfo.Name))]
    [MapperIgnoreTarget(nameof(NotificationInfo.Lifetime))]
    [MapPropertyFromSource(nameof(NotificationInfo.Data), Use = nameof(TryGetNotificationData))]
    public override partial NotificationInfo Map(Notification source);

    [MapProperty(nameof(Notification.NotificationId), nameof(NotificationInfo.Id))]
    [MapProperty(nameof(Notification.NotificationName), nameof(NotificationInfo.Name))]
    [MapperIgnoreTarget(nameof(NotificationInfo.Lifetime))]
    [MapPropertyFromSource(nameof(NotificationInfo.Data), Use = nameof(TryGetNotificationData))]
    public override partial void Map(Notification source, NotificationInfo destination);

    [UserMapping(Default = false)]
    private static NotificationData TryGetNotificationData(Notification source)
    {
        if (source != null)
        {
            var dataType = Type.GetType(source.NotificationTypeName);
            var data = Activator.CreateInstance(dataType);
            if (data is NotificationData notificationData)
            {
                notificationData.ExtraProperties = source.ExtraProperties;
                return notificationData;
            }
        }
        return new NotificationData();
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class UserNotificationInfoToNotificationInfoMapper : MapperBase<UserNotificationInfo, NotificationInfo>
{
    [MapProperty(nameof(UserNotificationInfo.NotificationId), nameof(NotificationInfo.Id))]
    [MapProperty(nameof(UserNotificationInfo.Name), nameof(NotificationInfo.Name))]
    [MapperIgnoreTarget(nameof(NotificationInfo.Lifetime))]
    [MapPropertyFromSource(nameof(NotificationInfo.Data), Use = nameof(TryGetNotificationData))]
    public override partial NotificationInfo Map(UserNotificationInfo source);

    [MapProperty(nameof(UserNotificationInfo.NotificationId), nameof(NotificationInfo.Id))]
    [MapProperty(nameof(UserNotificationInfo.Name), nameof(NotificationInfo.Name))]
    [MapperIgnoreTarget(nameof(NotificationInfo.Lifetime))]
    [MapPropertyFromSource(nameof(NotificationInfo.Data), Use = nameof(TryGetNotificationData))]
    public override partial void Map(UserNotificationInfo source, NotificationInfo destination);

    [UserMapping(Default = false)]
    private static NotificationData TryGetNotificationData(UserNotificationInfo source)
    {
        if (source != null)
        {
            var dataType = Type.GetType(source.NotificationTypeName);
            var data = Activator.CreateInstance(dataType);
            if (data is NotificationData notificationData)
            {
                notificationData.ExtraProperties = source.ExtraProperties;
                return notificationData;
            }
        }
        return new NotificationData();
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class UserSubscribeToNotificationSubscriptionInfoMapper : MapperBase<UserSubscribe, NotificationSubscriptionInfo>
{
    public override partial NotificationSubscriptionInfo Map(UserSubscribe source);
    public override partial void Map(UserSubscribe source, NotificationSubscriptionInfo destination);
}
