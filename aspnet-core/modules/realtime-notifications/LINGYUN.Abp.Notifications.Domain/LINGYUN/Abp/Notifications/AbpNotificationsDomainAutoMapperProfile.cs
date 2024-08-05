using AutoMapper;
using System;

namespace LINGYUN.Abp.Notifications;

public class AbpNotificationsDomainAutoMapperProfile : Profile
{
    public AbpNotificationsDomainAutoMapperProfile()
    {
        CreateMap<Notification, NotificationInfo>()
            .ForMember(dto => dto.Id, map => map.MapFrom(src => src.NotificationId.ToString()))
            .ForMember(dto => dto.Name, map => map.MapFrom(src => src.NotificationName))
            .ForMember(dto => dto.Lifetime, map => map.Ignore())
            .ForMember(dto => dto.Type, map => map.MapFrom(src => src.Type))
            .ForMember(dto => dto.ContentType, map => map.MapFrom(src => src.ContentType))
            .ForMember(dto => dto.Severity, map => map.MapFrom(src => src.Severity))
            .ForMember(dto => dto.CreationTime, map => map.MapFrom(src => src.CreationTime))
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

        CreateMap<UserNotificationInfo, NotificationInfo>()
            .ForMember(dto => dto.Id, map => map.MapFrom(src => src.Id.ToString()))
            .ForMember(dto => dto.Name, map => map.MapFrom(src => src.Name))
            .ForMember(dto => dto.Lifetime, map => map.Ignore())
            .ForMember(dto => dto.Type, map => map.MapFrom(src => src.Type))
            .ForMember(dto => dto.ContentType, map => map.MapFrom(src => src.ContentType))
            .ForMember(dto => dto.Severity, map => map.MapFrom(src => src.Severity))
            .ForMember(dto => dto.CreationTime, map => map.MapFrom(src => src.CreationTime))
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

        CreateMap<UserSubscribe, NotificationSubscriptionInfo>();
    }
}
