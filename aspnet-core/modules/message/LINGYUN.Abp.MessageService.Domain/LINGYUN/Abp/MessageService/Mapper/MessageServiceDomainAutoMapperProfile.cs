using AutoMapper;
using LINGYUN.Abp.IM.Groups;
using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.Chat;
using LINGYUN.Abp.MessageService.Groups;
using LINGYUN.Abp.MessageService.Notifications;
using LINGYUN.Abp.MessageService.Subscriptions;
using LINGYUN.Abp.Notifications;
using System;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.MessageService.Mapper
{
    public class MessageServiceDomainAutoMapperProfile : Profile
    {
        public MessageServiceDomainAutoMapperProfile()
        {
            CreateMap<Notification, NotificationInfo>()
                .ForMember(dto => dto.Id, map => map.MapFrom(src => src.NotificationId))
                .ForMember(dto => dto.Name, map => map.MapFrom(src => src.NotificationName))
                .ForMember(dto => dto.Lifetime, map => map.Ignore())
                .ForMember(dto => dto.Type, map => map.MapFrom(src => src.Type))
                .ForMember(dto => dto.Severity, map => map.MapFrom(src => src.Severity))
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

            //CreateMap<Notification, NotificationInfo>()
            //    .ConvertUsing<NotificationTypeConverter>();

            CreateMap<UserSubscribe, NotificationSubscriptionInfo>();

            CreateMessageMap<GroupMessage, ChatMessage>()
                .ForMember(dto => dto.Content, map => map.MapFrom(src => src.Content))
                .ForMember(dto => dto.GroupId, map => map.MapFrom(src => src.GroupId.ToString()))
                .Ignore(dto => dto.ToUserId);

            CreateMessageMap<UserMessage, ChatMessage>()
                .ForMember(dto => dto.ToUserId, map => map.MapFrom(src => src.ReceiveUserId))
                .Ignore(dto => dto.GroupId);

            CreateMap<ChatGroup, Group>()
                .ForMember(g => g.Id, map => map.MapFrom(src => src.GroupId.ToString()))
                .ForMember(g => g.MaxUserLength, map => map.MapFrom(src => src.MaxUserCount))
                .Ignore(g => g.GroupUserCount);
        }

        protected IMappingExpression<TSource, TDestination> CreateMessageMap<TSource, TDestination>()
            where TSource : Message
            where TDestination : ChatMessage
        {
            return CreateMap<TSource, TDestination>()
               .ForMember(dto => dto.Content, map => map.MapFrom(src => src.Content))
               .ForMember(dto => dto.MessageId, map => map.MapFrom(src => src.MessageId.ToString()))
               .ForMember(dto => dto.FormUserId, map => map.MapFrom(src => src.CreatorId))
               .ForMember(dto => dto.FormUserName, map => map.MapFrom(src => src.SendUserName))
               .ForMember(dto => dto.SendTime, map => map.MapFrom(src => src.CreationTime))
               .ForMember(dto => dto.MessageType, map => map.MapFrom(src => src.Type))
               .ForMember(dto => dto.Source, map => map.MapFrom(src => src.Source))
               .ForMember(dto => dto.IsAnonymous, map => map.MapFrom(src => src.GetProperty(nameof(ChatMessage.IsAnonymous), false)))
               .MapExtraProperties(MappingPropertyDefinitionChecks.None);
        }
    }
}
