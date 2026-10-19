using LINGYUN.Abp.PushPlus.Message;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.PushPlus;

/// <summary>
/// 捕获式 <see cref="IPushPlusMessageSender"/> 假实现
/// </summary>
public class FakePushPlusMessageSender : IPushPlusMessageSender
{
    public List<SentArgs> SentMessages { get; } = new();

    public Task<string> SendWeChatAsync(string title, string content, string topic = "", PushPlusMessageTemplate template = PushPlusMessageTemplate.Html, string webhook = "", string callbackUrl = "", CancellationToken cancellationToken = default)
        => Capture("WeChat", title, content, topic, template, webhook, callbackUrl);

    public Task<string> SendEmailAsync(string title, string content, string topic = "", PushPlusMessageTemplate template = PushPlusMessageTemplate.Html, string webhook = "", string callbackUrl = "", CancellationToken cancellationToken = default)
        => Capture("Email", title, content, topic, template, webhook, callbackUrl);

    public Task<string> SendWeWorkAsync(string title, string content, string topic = "", PushPlusMessageTemplate template = PushPlusMessageTemplate.Html, string webhook = "", string callbackUrl = "", CancellationToken cancellationToken = default)
        => Capture("WeWork", title, content, topic, template, webhook, callbackUrl);

    public Task<string> SendWebhookAsync(string title, string content, string topic = "", PushPlusMessageTemplate template = PushPlusMessageTemplate.Html, string webhook = "", string callbackUrl = "", CancellationToken cancellationToken = default)
        => Capture("Webhook", title, content, topic, template, webhook, callbackUrl);

    public Task<string> SendSmsAsync(string title, string content, string topic = "", PushPlusMessageTemplate template = PushPlusMessageTemplate.Html, string webhook = "", string callbackUrl = "", CancellationToken cancellationToken = default)
        => Capture("Sms", title, content, topic, template, webhook, callbackUrl);

    public void Clear() => SentMessages.Clear();

    private Task<string> Capture(
        string channel,
        string title,
        string content,
        string topic,
        PushPlusMessageTemplate template,
        string webhook,
        string callbackUrl)
    {
        SentMessages.Add(new SentArgs(channel, title, content, topic, template, webhook, callbackUrl));

        return Task.FromResult(string.Empty);
    }

    public record SentArgs(
        string Channel,
        string Title,
        string Content,
        string Topic,
        PushPlusMessageTemplate Template,
        string Webhook,
        string CallbackUrl);
}
