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
            var notificationDispatcher = context.ServiceProvider.GetRequiredService<INotificationDispatcher>();
            var notificationName = NotificationNameNormalizer
                .NormalizerName(AbpExceptionHandlingNotificationNames.NotificationName);
            NotificationData notificationData;
            if (CurrentTenant.IsAvailable)
            {
                notificationData = NotificationData.CreateTenantNotificationData(CurrentTenant.Id.Value);
            }
            else
            {
                notificationData = NotificationData.CreateNotificationData();
            }
            // 写入通知数据
            //TODO：集成TextTemplate完成格式化的推送
            notificationData.WriteStandardData(
                context.Exception.GetType().FullName, context.Exception.Message,
                DateTime.Now, "System");
            
            await notificationDispatcher.DispatchAsync(notificationName, notificationData, 
                CurrentTenant.Id, NotificationSeverity.Error);
        }
    }
}
