using JetBrains.Annotations;
using LINGYUN.Abp.BackgroundTasks.Activities;
using LINGYUN.Abp.BackgroundTasks.Localization;
using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.BackgroundTasks.Notifications;

public class JobExecuteFailedNotificationProvider : NotificationJobExecutedProvider
{
    public const string Name = "JobExecutedFailedNofiter";

    public JobExecuteFailedNotificationProvider(
        ICurrentTenant currentTenant, 
        INotificationSender notificationSender, 
        ITemplateRenderer templateRenderer, 
        IStringLocalizer<BackgroundTasksResource> stringLocalizer) 
        : base(currentTenant, notificationSender, templateRenderer, stringLocalizer)
    {
    }

    public async override Task NotifyErrorAsync([NotNull] JobActionExecuteContext context)
    {
        var title = StringLocalizer["JobExecutedFailed"].Value;

        await SendNofiterAsync(context, title, NotificationSeverity.Error);
    }
}
