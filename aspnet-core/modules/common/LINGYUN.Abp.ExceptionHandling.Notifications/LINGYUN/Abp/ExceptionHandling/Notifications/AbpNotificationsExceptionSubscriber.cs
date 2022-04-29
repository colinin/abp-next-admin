using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ExceptionHandling.Notifications
{
    public class AbpNotificationsExceptionSubscriber : AbpExceptionSubscriberBase
    {
        protected ICurrentTenant CurrentTenant { get; }
        public AbpNotificationsExceptionSubscriber(
            ICurrentTenant currentTenant,
            IServiceScopeFactory serviceScopeFactory, 
            IOptions<AbpExceptionHandlingOptions> options) 
            : base(serviceScopeFactory, options)
        {
            CurrentTenant = currentTenant;
        }

        protected override async Task SendErrorNotifierAsync(ExceptionSendNotifierContext context)
        {
            var notificationSender = context.ServiceProvider.GetRequiredService<INotificationSender>();

            NotificationData notificationData = new NotificationData();
            // 写入通知数据
            notificationData.TrySetData("header", "An application exception has occurred");
            notificationData.TrySetData("footer", $"Copyright to LY Colin © {DateTime.Now.Year}");
            notificationData.TrySetData("loglevel", context.LogLevel.ToString());
            notificationData.TrySetData("stacktrace", context.Exception.ToString());
            notificationData.WriteStandardData(
                context.Exception.GetType().FullName, 
                context.Exception.Message,
                DateTime.Now, 
                "System");
            
            await notificationSender.SendNofiterAsync(
                AbpExceptionHandlingNotificationNames.NotificationName, 
                notificationData, 
                null,
                CurrentTenant.Id, 
                NotificationSeverity.Error);
        }
    }
}
