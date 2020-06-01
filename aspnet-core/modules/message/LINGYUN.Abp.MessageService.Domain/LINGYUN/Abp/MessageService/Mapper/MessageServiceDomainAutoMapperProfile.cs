using AutoMapper;
using LINGYUN.Abp.MessageService.Notifications;
using LINGYUN.Abp.MessageService.Subscriptions;
using LINGYUN.Abp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LINGYUN.Abp.MessageService.Mapper
{
    public class MessageServiceDomainAutoMapperProfile : Profile
    {
        public MessageServiceDomainAutoMapperProfile()
        {
            CreateMap<Notification, NotificationInfo>()
                .ForMember(dto => dto.Id, map => map.MapFrom(src => src.NotificationId))
                .ForMember(dto => dto.Name, map => map.MapFrom(src => src.NotificationName))
                .ForMember(dto => dto.NotificationType, map => map.MapFrom(src => src.Type))
                .ForMember(dto => dto.NotificationSeverity, map => map.MapFrom(src => src.Severity))
                .ForMember(dto => dto.Data, map => map.MapFrom((src, nfi) =>
                {
                    var notificationDataType = Type.GetType(src.NotificationTypeName);
                    var notificationData = JsonConvert.DeserializeObject(src.NotificationData, notificationDataType);
                    if(notificationData != null)
                    {
                        return notificationData as NotificationData;
                    }
                    return new NotificationData();
                }));

            CreateMap<UserSubscribe, NotificationSubscriptionInfo>();
        }
    }
}
