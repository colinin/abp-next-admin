using JetBrains.Annotations;
using LINGYUN.Abp.BackgroundTasks.ExceptionHandling.Templates;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.BackgroundTasks.ExceptionHandling;

public class JobFailedNotifierProvider : IJobFailedNotifierProvider, ITransientDependency
{
    public const string Prefix = "exception.";
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

    public ILogger<JobFailedNotifierProvider> Logger { protected get; set; }

    protected IEmailSender EmailSender { get; }
    protected ITemplateRenderer TemplateRenderer { get; }

    public JobFailedNotifierProvider(
        IEmailSender emailSender,
        ITemplateRenderer templateRenderer)
    {
        EmailSender = emailSender;
        TemplateRenderer = templateRenderer;

        Logger = NullLogger<JobFailedNotifierProvider>.Instance;
    }

    public virtual async Task NotifyErrorAsync([NotNull] JobEventContext context)
    {
        var eventData = context.EventData;
        // 异常所属分组不处理, 防止死循环
        if (string.Equals(eventData.Group, JobGroup))
        {
            Logger.LogWarning($"There is a problem executing the job, reason: {eventData.Exception.Message}");
            return;
        }
        var notifyKey = Prefix + PropertyTo;
        if (eventData.Args.TryGetValue(notifyKey, out var exceptionTo) &&
            exceptionTo is string to)
        {
            var template = eventData.Args.GetOrDefault(Prefix + PropertyTemplate)?.ToString() ?? "";
            var subject = eventData.Args.GetOrDefault(Prefix + PropertySubject)?.ToString() ?? "From job execute exception";
            var from = eventData.Args.GetOrDefault(Prefix + PropertyFrom)?.ToString() ?? "";
            var errorMessage = eventData.Exception.GetBaseException().Message;

            if (template.IsNullOrWhiteSpace())
            {
                // var message = eventData.Args.GetOrDefault(Prefix + SendEmailJob.PropertyBody)?.ToString() ?? "";
                // await EmailSender.SendAsync(from, to, subject, message, false);
                // return;
                // 默认使用内置模板发送错误
                template = JobExceptionHandlingTemplates.JobExceptionNotifier;
            }

            var footer = eventData.Args.GetOrDefault("footer")?.ToString() ?? $"Copyright to LY Colin © {eventData.RunTime.Year}";
            var model = new
            {
                Title = subject,
                Id = eventData.Key,
                Group = eventData.Args.GetOrDefault(nameof(JobInfo.Group)) ?? eventData.Group,
                Name = eventData.Args.GetOrDefault(nameof(JobInfo.Name)) ?? eventData.Name,
                Type = eventData.Args.GetOrDefault(nameof(JobInfo.Type)) ?? eventData.Type.Name,
                Triggertime = eventData.RunTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Message = errorMessage,
                Tenantname = eventData.Args.GetOrDefault(nameof(IMultiTenant.TenantId)),
                Footer = footer,
            };

            var globalContext = new Dictionary<string, object>();
            if (eventData.Args.TryGetValue(Prefix + PropertyContext, out var ctx) &&
                ctx is string ctxStr && !ctxStr.IsNullOrWhiteSpace())
            {
                try
                {
                    globalContext = JsonConvert.DeserializeObject<Dictionary<string, object>>(ctxStr);
                }
                catch { }
            }
            globalContext.AddIfNotContains(eventData.Args);

            var culture = eventData.Args.GetOrDefault(Prefix + PropertyCulture)?.ToString() ?? CultureInfo.CurrentCulture.Name;

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
}
