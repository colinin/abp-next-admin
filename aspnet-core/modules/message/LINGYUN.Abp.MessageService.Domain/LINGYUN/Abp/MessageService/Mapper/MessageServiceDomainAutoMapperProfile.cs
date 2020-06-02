using AutoMapper;
using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.Messages;
using LINGYUN.Abp.MessageService.Notifications;
using LINGYUN.Abp.MessageService.Subscriptions;
using LINGYUN.Abp.Notifications;
using Newtonsoft.Json;
using System;

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

            CreateMap<GroupMessage, ChatMessage>()
                .ForMember(dto => dto.Content, map => map.MapFrom(src => src.Content))
                .ForMember(dto => dto.GroupId, map => map.MapFrom(src => src.GroupId.ToString()))
                .ForMember(dto => dto.MessageId, map => map.MapFrom(src => src.MessageId.ToString()))
                .ForMember(dto => dto.FormUserId, map => map.MapFrom(src => src.CreatorId))
                .ForMember(dto => dto.FormUserName, map => map.MapFrom(src => src.SendUserName))
                .ForMember(dto => dto.SendTime, map => map.MapFrom(src => src.CreationTime))
                .ForMember(dto => dto.MessageType, map => map.MapFrom(src => src.Type))
                .ForMember(dto => dto.IsAnonymous, map => map.Ignore())
                .ForMember(dto => dto.ToUserId, map => map.Ignore());

            CreateMap<UserMessage, ChatMessage>()
               .ForMember(dto => dto.Content, map => map.MapFrom(src => src.Content))
               .ForMember(dto => dto.ToUserId, map => map.MapFrom(src => src.ReceiveUserId))
               .ForMember(dto => dto.MessageId, map => map.MapFrom(src => src.MessageId.ToString()))
               .ForMember(dto => dto.FormUserId, map => map.MapFrom(src => src.CreatorId))
               .ForMember(dto => dto.FormUserName, map => map.MapFrom(src => src.SendUserName))
               .ForMember(dto => dto.SendTime, map => map.MapFrom(src => src.CreationTime))
               .ForMember(dto => dto.MessageType, map => map.MapFrom(src => src.Type))
               .ForMember(dto => dto.IsAnonymous, map => map.Ignore())
               .ForMember(dto => dto.GroupId, map => map.Ignore());
        }
    }
}
