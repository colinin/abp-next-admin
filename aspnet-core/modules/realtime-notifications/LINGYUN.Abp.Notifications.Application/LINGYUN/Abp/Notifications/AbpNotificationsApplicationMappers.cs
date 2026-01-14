using Riok.Mapperly.Abstractions;
using System;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.Notifications;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class UserNotificationInfoToUserNotificationDtoMapper : MapperBase<UserNotificationInfo, UserNotificationDto>
{
    [MapProperty(nameof(UserNotificationInfo.NotificationId), nameof(UserNotificationDto.Id))]
    [MapperIgnoreTarget(nameof(UserNotificationDto.Lifetime))]
    [MapPropertyFromSource(nameof(UserNotificationDto.Data), Use = nameof(TryGetNotificationData))]
    public override partial UserNotificationDto Map(UserNotificationInfo source);

    [MapProperty(nameof(UserNotificationInfo.NotificationId), nameof(UserNotificationDto.Id))]
    [MapperIgnoreTarget(nameof(UserNotificationDto.Lifetime))]
    [MapPropertyFromSource(nameof(UserNotificationDto.Data), Use = nameof(TryGetNotificationData))]
    public override partial void Map(UserNotificationInfo source, UserNotificationDto destination);

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
