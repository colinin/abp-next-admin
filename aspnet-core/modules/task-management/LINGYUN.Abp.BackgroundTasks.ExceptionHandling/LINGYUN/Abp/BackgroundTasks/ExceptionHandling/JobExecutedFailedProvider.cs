using JetBrains.Annotations;
using LINGYUN.Abp.BackgroundTasks.Activities;
using LINGYUN.Abp.BackgroundTasks.ExceptionHandling.Templates;
using LINGYUN.Abp.BackgroundTasks.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.BackgroundTasks.ExceptionHandling;

public class JobExecutedFailedProvider : JobExecutedProvider, ITransientDependency
{
    public const string Name = "JobExecutedFailedProvider";
    public readonly static IList<JobActionParamter> Paramters = new List<JobActionParamter>
    {
        new JobActionParamter(PropertyTo, L("DisplayName:PropertyTo"), L("Description:PropertyTo"), true),

        new JobActionParamter(PropertySubject, L("DisplayName:PropertySubject"), L("Description:PropertySubject")),
        new JobActionParamter(PropertyFrom, L("DisplayName:PropertyFrom"), L("Description:PropertyFrom")),
        new JobActionParamter(PropertyBody, L("DisplayName:PropertyBody"), L("Description:PropertyBody")),
        new JobActionParamter(PropertyTemplate, L("DisplayName:PropertyTemplate"), L("Description:PropertyTemplate")),
        new JobActionParamter(PropertyContext, L("DisplayName:PropertyContext"), L("Description:PropertyContext")),
        new JobActionParamter(PropertyCulture, L("DisplayName:PropertyCulture"), L("Description:PropertyCulture")),
    };

    public const string JobGroup = "ExceptionNotifier";

    public const string PropertyFrom = "from";
    /// <summary>
    /// 接收者
    /// </summary>
    public const string PropertyTo = "to";
    /// <summary>
    /// 必须，邮件主体
    /// </summary>
    public const string PropertySubject = "subject";
    /// <summary>
    /// 消息内容, 文本消息时必须
    /// </summary>
    public const string PropertyBody = "body";
    /// <summary>
    /// 发送模板消息
    /// </summary>
    public const string PropertyTemplate = "template";
    /// <summary>
    /// 可选, 模板消息中的上下文参数
    /// </summary>
    public const string PropertyContext = "context";
    /// <summary>
    /// 可选, 模板消息中的区域性
    /// </summary>
    public const string PropertyCulture = "culture";

    public ILogger<JobExecutedFailedProvider> Logger { protected get; set; }

    protected IEmailSender EmailSender { get; }
    protected ITemplateRenderer TemplateRenderer { get; }
    public JobExecutedFailedProvider(
        IEmailSender emailSender,
        ITemplateRenderer templateRenderer)
    {
        EmailSender = emailSender;
        TemplateRenderer = templateRenderer;

        Logger = NullLogger<JobExecutedFailedProvider>.Instance;
    }

    public override async Task NotifyErrorAsync([NotNull] JobActionExecuteContext context)
    {
        if (context.Action.Paramters.TryGetValue(PropertyTo, out var exceptionTo) &&
            exceptionTo is string to)
        {
            var template = context.Action.Paramters.GetOrDefault(PropertyTemplate)?.ToString() ?? "";
            var subject = context.Action.Paramters.GetOrDefault(PropertySubject)?.ToString() ?? "From job execute exception";
            var from = context.Action.Paramters.GetOrDefault(PropertyFrom)?.ToString() ?? "";
            var errorMessage = context.Event.EventData.Exception.GetBaseException().Message;

            if (template.IsNullOrWhiteSpace())
            {
                // var message = eventData.Args.GetOrDefault(SendEmailJob.PropertyBody)?.ToString() ?? "";
                // await EmailSender.SendAsync(from, to, subject, message, false);
                // return;
                // 默认使用内置模板发送错误
                template = JobExceptionHandlingTemplates.JobExceptionNotifier;
            }

            var footer = context.Action.Paramters.GetOrDefault("footer")?.ToString() ?? $"Copyright to LY Colin © {context.Event.EventData.RunTime.Year}";
            var model = new
            {
                Title = subject,
                Id = context.Event.EventData.Key,
                Group = context.Action.Paramters.GetOrDefault(nameof(JobInfo.Group)) ?? context.Event.EventData.Group,
                Name = context.Action.Paramters.GetOrDefault(nameof(JobInfo.Name)) ?? context.Event.EventData.Name,
                Type = context.Action.Paramters.GetOrDefault(nameof(JobInfo.Type)) ?? context.Event.EventData.Type.Name,
                Triggertime = context.Event.EventData.RunTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Message = errorMessage,
                Tenantname = context.Action.Paramters.GetOrDefault(nameof(IMultiTenant.TenantId)),
                Footer = footer,
            };

            var globalContext = new Dictionary<string, object>();
            if (context.Action.Paramters.TryGetValue(PropertyContext, out var ctx) &&
                ctx is string ctxStr && !ctxStr.IsNullOrWhiteSpace())
            {
                try
                {
                    globalContext = JsonConvert.DeserializeObject<Dictionary<string, object>>(ctxStr);
                }
                catch { }
            }
            globalContext.AddIfNotContains(context.Action.Paramters);

            var culture = context.Action.Paramters.GetOrDefault(PropertyCulture)?.ToString() ?? CultureInfo.CurrentCulture.Name;

            var content = await TemplateRenderer.RenderAsync(
                templateName: template,
                model: model,
                cultureName: culture,
                globalContext: globalContext);

            if (from.IsNullOrWhiteSpace())
            {
                await EmailSender.SendAsync(to, subject, content, true);
                return;
            }
            await EmailSender.SendAsync(from, to, subject, content, true);
        }
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<BackgroundTasksResource>(name);
    }
}
