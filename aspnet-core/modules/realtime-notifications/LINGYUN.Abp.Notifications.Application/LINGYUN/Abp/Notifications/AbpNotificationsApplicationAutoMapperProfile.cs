using AutoMapper;
using System;

namespace LINGYUN.Abp.Notifications;

public class AbpNotificationsApplicationAutoMapperProfile : Profile
{
    public AbpNotificationsApplicationAutoMapperProfile()
    {
        CreateMap<UserNotificationInfo, UserNotificationDto>()
            .ForMember(dto => dto.Id, map => map.MapFrom(src => src.Id.ToString()))
            .ForMember(dto => dto.Lifetime, map => map.Ignore())
            .ForMember(dto => dto.Data, map => map.MapFrom((src, nfi) =>
            {
                var dataType = Type.GetType(src.NotificationTypeName);
                var data = Activator.CreateInstance(dataType);
                if (data is NotificationData notificationData)
                {
                    notificationData.ExtraProperties = src.ExtraProperties;
                    return notificationData;
                }
                return new NotificationData();
            }));
    }
}
