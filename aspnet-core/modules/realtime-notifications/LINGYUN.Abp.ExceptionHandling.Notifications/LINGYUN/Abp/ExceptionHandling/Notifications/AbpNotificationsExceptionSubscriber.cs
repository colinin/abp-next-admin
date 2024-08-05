using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ExceptionHandling.Notifications;

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
        // 发送错误模板消息
        await notificationSender.SendNofiterAsync(
            NotificationsCommonNotificationNames.ExceptionHandling,
            new NotificationTemplate(
                NotificationsCommonNotificationNames.ExceptionHandling,
                formUser: "System",
                data: new Dictionary<string, object>
                {
                    { "header", "An application exception has occurred" },
                    { "footer", $"Copyright to LY Colin © {DateTime.Now.Year}" },
                    { "loglevel", context.LogLevel.ToString() },
                    { "stackTrace", context.Exception.ToString() },
                }),
            user: null,
            CurrentTenant.Id,
            NotificationSeverity.Error);
    }
}
