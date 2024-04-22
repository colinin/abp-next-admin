using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.Message;

public interface IPushPlusMessageSender
{
    Task<string> SendWeChatAsync(
        string title,
        string content,
        string topic = "",
        PushPlusMessageTemplate template = PushPlusMessageTemplate.Html,
        string webhook = "",
        string callbackUrl = "",
        CancellationToken cancellationToken = default);

    Task<string> SendEmailAsync(
        string title,
        string content,
        string topic = "",
        PushPlusMessageTemplate template = PushPlusMessageTemplate.Html,
        string webhook = "",
        string callbackUrl = "",
        CancellationToken cancellationToken = default);

    Task<string> SendWeWorkAsync(
        string title,
        string content,
        string topic = "",
        PushPlusMessageTemplate template = PushPlusMessageTemplate.Html,
        string webhook = "",
        string callbackUrl = "",
        CancellationToken cancellationToken = default);

    Task<string> SendWebhookAsync(
        string title,
        string content,
        string topic = "",
        PushPlusMessageTemplate template = PushPlusMessageTemplate.Html,
        string webhook = "",
        string callbackUrl = "",
        CancellationToken cancellationToken = default);

    Task<string> SendSmsAsync(
        string title,
        string content,
        string topic = "",
        PushPlusMessageTemplate template = PushPlusMessageTemplate.Html,
        string webhook = "",
        string callbackUrl = "",
        CancellationToken cancellationToken = default);
}
