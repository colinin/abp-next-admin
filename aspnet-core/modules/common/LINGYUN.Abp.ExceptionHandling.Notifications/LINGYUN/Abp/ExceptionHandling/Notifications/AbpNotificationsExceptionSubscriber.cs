using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.ExceptionHandling.Notifications
{
    public class AbpNotificationsExceptionSubscriber : AbpExceptionSubscriberBase
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected IClock Clock { get; }
        public AbpNotificationsExceptionSubscriber(
            ICurrentTenant currentTenant,
            IServiceScopeFactory serviceScopeFactory, 
            IOptions<AbpExceptionHandlingOptions> options,
            IClock clock) : base(serviceScopeFactory, options)
        {
            CurrentTenant = currentTenant;
            Clock = clock;
        }

        protected async override Task SendErrorNotifierAsync(ExceptionSendNotifierContext context)
        {
            var notificationSender = context.ServiceProvider.GetRequiredService<INotificationSender>();

            NotificationData notificationData = new NotificationData();
            // 写入通知数据
            //TODO：集成TextTemplate完成格式化的推送
            notificationData.WriteStandardData(
                context.Exception.GetType().FullName, 
                context.Exception.Message,
                Clock.Now, 
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
