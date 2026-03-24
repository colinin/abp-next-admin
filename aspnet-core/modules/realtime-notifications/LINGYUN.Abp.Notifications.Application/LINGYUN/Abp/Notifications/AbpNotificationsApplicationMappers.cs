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

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class NotificationSendRecordInfoToNotificationSendRecordDtoMapper : MapperBase<NotificationSendRecordInfo, NotificationSendRecordDto>
{
    private readonly UserNotificationInfoToUserNotificationDtoMapper _userNotificationInfoMapper;
    public NotificationSendRecordInfoToNotificationSendRecordDtoMapper(UserNotificationInfoToUserNotificationDtoMapper userNotificationInfoMapper)
    {
        _userNotificationInfoMapper = userNotificationInfoMapper;
    }

    [MapProperty(nameof(NotificationSendRecordInfo.Id), nameof(NotificationSendRecordDto.Id))]
    public override partial NotificationSendRecordDto Map(NotificationSendRecordInfo source);

    [MapProperty(nameof(NotificationSendRecordInfo.Id), nameof(NotificationSendRecordDto.Id))]
    public override partial void Map(NotificationSendRecordInfo source, NotificationSendRecordDto destination);

    public override void AfterMap(NotificationSendRecordInfo source, NotificationSendRecordDto destination)
    {
        if (source.NotificationInfo != null)
        {
            var userNotificationInfoDto = _userNotificationInfoMapper.Map(source.NotificationInfo);
            _userNotificationInfoMapper.AfterMap(source.NotificationInfo, userNotificationInfoDto);
            destination.Notification = userNotificationInfoDto;
        }
    }
}