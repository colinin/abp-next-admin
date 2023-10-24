using LINGYUN.Abp.PushPlus.Channel;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.Message;

public static class IPushPlusMessageSenderExtensions
{
    public static Task<string> SendWithChannelAsync(
        this IPushPlusMessageSender pushPlusMessageSender,
        string title,
        string content,
        string topic = "",
        PushPlusChannelType channelType = PushPlusChannelType.WeChat,
        PushPlusMessageTemplate template = PushPlusMessageTemplate.Html,
        string webhook = "",
        string callbackUrl = "",
        CancellationToken cancellationToken = default)
    {
        return channelType switch
        {
            PushPlusChannelType.WeChat => 
                pushPlusMessageSender.SendWeChatAsync(
                    title,
                    content,
                    topic,
                    template,
                    webhook,
                    callbackUrl,
                    cancellationToken),
            PushPlusChannelType.WeWork =>
                pushPlusMessageSender.SendWeWorkAsync(
                    title,
                    content,
                    topic,
                    template,
                    webhook,
                    callbackUrl,
                    cancellationToken),
            PushPlusChannelType.Webhook =>
                pushPlusMessageSender.SendWebhookAsync(
                    title,
                    content,
                    topic,
                    template,
                    webhook,
                    callbackUrl,
                    cancellationToken),
            PushPlusChannelType.Email =>
                pushPlusMessageSender.SendEmailAsync(
                    title,
                    content,
                    topic,
                    template,
                    webhook,
                    callbackUrl,
                    cancellationToken),
            PushPlusChannelType.Sms =>
                pushPlusMessageSender.SendSmsAsync(
                    title,
                    content,
                    topic,
                    template,
                    webhook,
                    callbackUrl,
                    cancellationToken),
            _ =>
                pushPlusMessageSender.SendWeChatAsync(
                    title,
                    content,
                    topic,
                    template,
                    webhook,
                    callbackUrl,
                    cancellationToken),
        };
    }
}
