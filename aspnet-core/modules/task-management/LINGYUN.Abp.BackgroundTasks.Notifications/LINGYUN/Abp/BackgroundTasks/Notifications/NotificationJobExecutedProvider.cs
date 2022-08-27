using JetBrains.Annotations;
using LINGYUN.Abp.BackgroundTasks.Activities;
using LINGYUN.Abp.BackgroundTasks.Localization;
using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.BackgroundTasks.Notifications;

public abstract class NotificationJobExecutedProvider : JobExecutedProvider, ITransientDependency
{
    public readonly static IList<JobActionParamter> Paramters = new List<JobActionParamter>
    {
        new JobActionParamter(PropertyPushProvider, L("DisplayName:PushProvider"), L("Description:PushProvider")),
        new JobActionParamter(PropertyUseTemplate, L("DisplayName:Template"), L("Description:Template")),
        new JobActionParamter(PropertyContent, L("DisplayName:Content"), L("Description:Content")),
        new JobActionParamter(PropertyCulture, L("DisplayName:Culture"), L("Description:Culture")),
    };

    /// <summary>
    /// 指定发送工具
    /// </summary>
    public const string PropertyPushProvider = "push-provider";
    /// <summary>
    /// 使用通知模板
    /// </summary>
    public const string PropertyUseTemplate = "use-template";
    /// <summary>
    /// 通知内容, 不使用模板时必须
    /// </summary>
    public const string PropertyContent = "content";
    /// <summary>
    /// 可选, 模板消息中的区域性
    /// </summary>
    public const string PropertyCulture = "culture";

    protected ICurrentTenant CurrentTenant { get; }
    protected INotificationSender NotificationSender { get; }
    protected ITemplateRenderer TemplateRenderer { get; }
    protected IStringLocalizer<BackgroundTasksResource> StringLocalizer { get; }

    protected NotificationJobExecutedProvider(
        ICurrentTenant currentTenant,
        INotificationSender notificationSender, 
        ITemplateRenderer templateRenderer, 
        IStringLocalizer<BackgroundTasksResource> stringLocalizer)
    {
        CurrentTenant = currentTenant;
        NotificationSender = notificationSender;
        TemplateRenderer = templateRenderer;
        StringLocalizer = stringLocalizer;
    }

    protected async virtual Task SendNofiterAsync(
        [NotNull] JobActionExecuteContext context,
        [NotNull] string title,
        NotificationSeverity severity = NotificationSeverity.Info)
    {
        var useProvider = context.Action.Paramters.GetOrDefault(PropertyPushProvider)?.ToString() ?? "";
        var content = context.Action.Paramters.GetOrDefault(PropertyContent)?.ToString() ?? "";
        var templateName = context.Action.Paramters.GetOrDefault(PropertyUseTemplate)?.ToString()
            ?? BackgroundTasksNotificationNames.JobExecuteSucceeded;

        if (content.IsNullOrWhiteSpace() && !templateName.IsNullOrWhiteSpace())
        {
            var errorMessage = context.Event.EventData.Exception?.GetBaseException().Message;
            var model = new
            {
                Color = GetTitleColor(severity),
                Error = context.Event.EventData.Exception != null,
                Errormessage = errorMessage,
                Title = title,
                Id = context.Event.EventData.Key,
                Group = context.Event.EventData.Args.GetOrDefault(nameof(JobInfo.Group)) ?? context.Event.EventData.Group,
                Name = context.Event.EventData.Args.GetOrDefault(nameof(JobInfo.Name)) ?? context.Event.EventData.Name,
                Type = context.Event.EventData.Args.GetOrDefault(nameof(JobInfo.Type)) ?? context.Event.EventData.Type.Name,
                Triggertime = context.Event.EventData.RunTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Tenantname = context.Event.EventData.Args.GetOrDefault(nameof(IMultiTenant.TenantId)),
            };
            var culture = context.Action.Paramters.GetOrDefault(PropertyCulture)?.ToString() ?? CultureInfo.CurrentCulture.Name;

            content = await TemplateRenderer.RenderAsync(templateName, model, culture);
        }

        var notificationData = new NotificationData();
        notificationData.WriteStandardData(
            title: title,
            message: content,
            createTime: context.Event.EventData.RunTime,
            formUser: "BackgroundTasks Engine");

        await NotificationSender.SendNofiterAsync(
            BackgroundTasksNotificationNames.JobExecuteSucceeded,
            notificationData,
            tenantId: CurrentTenant.Id,
            severity: severity,
            useProviders: useProvider.Split(';'));
    }

    protected string GetTitleColor(NotificationSeverity severity = NotificationSeverity.Info)
    {
        return severity switch
        {
            NotificationSeverity.Success => "#3CB371",
            NotificationSeverity.Warn => "#FF4500",
            NotificationSeverity.Error => "red",
            NotificationSeverity.Info => "#708090",
            NotificationSeverity.Fatal => "red",
            _ => "#708090"
        };
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<BackgroundTasksResource>(name);
    }
}
