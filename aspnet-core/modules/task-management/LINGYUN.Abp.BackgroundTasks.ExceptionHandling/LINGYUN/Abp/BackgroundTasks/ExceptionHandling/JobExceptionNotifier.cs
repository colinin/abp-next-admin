using JetBrains.Annotations;
using LINGYUN.Abp.BackgroundTasks.Jobs;
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
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BackgroundTasks.ExceptionHandling;

[Dependency(ReplaceServices = true)]
public class JobExceptionNotifier : IJobExceptionNotifier, ITransientDependency
{
    public const string Prefix = "exception.";
    public const string JobGroup = "ExceptionNotifier";

    public ILogger<JobExceptionNotifier> Logger { protected get; set; }

    protected IClock Clock { get; }
    protected IEmailSender EmailSender { get; }
    protected ITemplateRenderer TemplateRenderer { get; }

    public JobExceptionNotifier(
        IClock clock,
        IEmailSender emailSender,
        ITemplateRenderer templateRenderer)
    {
        Clock = clock;
        EmailSender = emailSender;
        TemplateRenderer = templateRenderer;

        Logger = NullLogger<JobExceptionNotifier>.Instance;
    }

    public virtual async Task NotifyAsync([NotNull] JobExceptionNotificationContext context)
    {
        // 异常所属分组不处理, 防止死循环
        if (string.Equals(context.JobInfo.Group, JobGroup))
        {
            Logger.LogWarning($"There is a problem executing the job, reason: {context.Exception.Message}");
            return;
        }
        var notifyKey = Prefix + SendEmailJob.PropertyTo;
        if (context.JobInfo.Args.TryGetValue(notifyKey, out var exceptionTo) &&
            exceptionTo is string to)
        {
            var template = context.JobInfo.Args.GetOrDefault(Prefix + SendEmailJob.PropertyTemplate)?.ToString() ?? "";
            var content = context.JobInfo.Args.GetOrDefault(Prefix + SendEmailJob.PropertyBody)?.ToString() ?? "";
            var subject = context.JobInfo.Args.GetOrDefault(Prefix + SendEmailJob.PropertySubject)?.ToString() ?? "From job execute exception";
            var from = context.JobInfo.Args.GetOrDefault(Prefix + SendEmailJob.PropertyFrom)?.ToString() ?? "";
            var errorMessage = context.Exception.GetBaseException().Message;

            if (template.IsNullOrWhiteSpace())
            {
                await EmailSender.SendAsync(from, to, subject, content, false);
                return;
            }

            var footer = context.JobInfo.Args.GetOrDefault("footer")?.ToString() ?? $"Copyright to LY Colin © {Clock.Now.Year}";
            var model = new
            {
                Title = subject,
                Group = context.JobInfo.Group,
                Name = context.JobInfo.Name,
                Id = context.JobInfo.Id,
                Type = context.JobInfo.Type,
                Triggertime = Clock.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Message = errorMessage,
                Tenantname = context.JobInfo.Args.GetOrDefault(nameof(IMultiTenant.TenantId)),
                Footer = footer,
            };

            var globalContext = new Dictionary<string, object>();
            if (context.JobInfo.Args.TryGetValue(Prefix + SendEmailJob.PropertyContext, out var ctx) &&
                ctx is string ctxStr && !ctxStr.IsNullOrWhiteSpace())
            {
                try
                {
                    globalContext = JsonConvert.DeserializeObject<Dictionary<string, object>>(ctxStr);
                }
                catch { }
            }

            var culture = context.JobInfo.Args.GetOrDefault(Prefix + SendEmailJob.PropertyCulture)?.ToString() ?? CultureInfo.CurrentCulture.Name;

            content = await TemplateRenderer.RenderAsync(
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
