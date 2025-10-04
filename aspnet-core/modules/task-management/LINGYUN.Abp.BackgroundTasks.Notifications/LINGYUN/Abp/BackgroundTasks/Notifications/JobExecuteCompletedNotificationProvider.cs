using JetBrains.Annotations;
using LINGYUN.Abp.BackgroundTasks.Activities;
using LINGYUN.Abp.BackgroundTasks.Localization;
using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.BackgroundTasks.Notifications;

public class JobExecuteCompletedNotificationProvider : NotificationJobExecutedProvider
{
    public const string Name = "JobExecutedCompletedNofiter";
    public override string DefaultNotificationName => BackgroundTasksNotificationNames.JobExecuteCompleted;

    public JobExecuteCompletedNotificationProvider(
        ICurrentTenant currentTenant, 
        INotificationSender notificationSender, 
        ITemplateRenderer templateRenderer, 
        IStringLocalizer<BackgroundTasksResource> stringLocalizer) 
        : base(currentTenant, notificationSender, templateRenderer, stringLocalizer)
    {
    }

    public async override Task NotifyComplateAsync([NotNull] JobActionExecuteContext context)
    {
        var title = StringLocalizer["Notifications:JobExecuteCompleted"].Value;

        await SendNofiterAsync(context, title, NotificationSeverity.Info);
    }
}
