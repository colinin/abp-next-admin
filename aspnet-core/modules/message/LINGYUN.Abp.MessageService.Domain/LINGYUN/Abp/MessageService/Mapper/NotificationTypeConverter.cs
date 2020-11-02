using AutoMapper;
using LINGYUN.Abp.MessageService.Notifications;
using LINGYUN.Abp.Notifications;
using Newtonsoft.Json;
using System;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.MessageService.Mapper
{
    public class NotificationTypeConverter : ITypeConverter<Notification, NotificationInfo>, ISingletonDependency
    {
        public NotificationInfo Convert(Notification source, NotificationInfo destination, ResolutionContext context)
        {
            destination = new NotificationInfo
            {
                Name = source.NotificationName,
                Type = source.Type,
                Severity = source.Severity
            };
            destination.SetId(source.NotificationId);

            var dataType = Type.GetType(source.NotificationTypeName);
            Check.NotNull(dataType, source.NotificationTypeName);

            var data = JsonConvert.DeserializeObject(source.NotificationData, dataType);
            if (data != null && data is NotificationData notificationData)
            {
                destination.Data = NotificationDataConverter.Convert(notificationData);
            }
            else
            {
                destination.Data = new NotificationData();
                destination.Data.TrySetData("data", source.NotificationData);
            }
            return destination;
        }
    }
}
