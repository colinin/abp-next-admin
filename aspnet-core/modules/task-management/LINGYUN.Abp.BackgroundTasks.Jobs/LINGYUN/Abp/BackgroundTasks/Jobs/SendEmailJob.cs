using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.TextTemplating;
using Volo.Abp.Json;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public class SendEmailJob : IJobRunnable
{
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
    /// 可选, 模板消息中的模型参数
    /// </summary>
    public const string PropertyModel = "model";
    /// <summary>
    /// 可选, 模板消息中的上下文参数
    /// </summary>
    public const string PropertyContext = "context";
    /// <summary>
    /// 可选, 模板消息中的区域性
    /// </summary>
    public const string PropertyCulture = "culture";

    public virtual async Task ExecuteAsync(JobRunnableContext context)
    {
        context.TryGetString(PropertyFrom, out var from);
        var to = context.GetString(PropertyTo);
        var subject = context.GetString(PropertySubject);

        var emailSender = context.GetRequiredService<IEmailSender>();

        if (context.TryGetString(PropertyTemplate, out var template) && !template.IsNullOrWhiteSpace())
        {
            var globalContext = new Dictionary<string, object>();
            context.TryGetString(PropertyCulture, out var culture);
            if (context.TryGetString(PropertyContext, out var globalCtx))
            {
                var jsonSerializer = context.GetRequiredService<IJsonSerializer>();
                try
                {
                    globalContext = jsonSerializer.Deserialize<Dictionary<string, object>>(globalCtx);
                }
                catch { }
            }

            object model = null;
            if (context.TryGetString(PropertyModel, out var modelString) && !modelString.IsNullOrWhiteSpace())
            {
                try
                {
                    model = JsonConvert.DeserializeObject(modelString);
                }
                catch { }
            }

            var templateRenderer = context.GetRequiredService<ITemplateRenderer>();

            var content = await templateRenderer.RenderAsync(
                templateName: template,
                model: model,
                cultureName: culture,
                globalContext: globalContext);

            await QueueEmail(emailSender, from, to, subject, content, true);
            return;
        }

        var body = context.GetString(PropertyBody);

        await QueueEmail(emailSender, from, to, subject, body, false);
    }

    private async Task QueueEmail(IEmailSender emailSender, string from, string to, string subject, string body, bool isBodyHtml = true)
    {
        if (from.IsNullOrWhiteSpace())
        {
            await emailSender.SendAsync(to, subject, body, isBodyHtml);
            return;
        }
        await emailSender.SendAsync(from, to, subject, body, isBodyHtml);
    }
}
