using LINGYUN.Abp.WeChat.Work.Messages;
using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;
public class WeChatWorkWebhookNotificationContributor : IWebhookNotificationContributor
{
    public string Name => "WeChat.Work";

    public async virtual Task ContributeAsync(IWebhookNotificationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpNotificationsWebhookWeChatWorkOptions>>().Value;
        var notificationDataSerializer = context.ServiceProvider.GetRequiredService<INotificationDataSerializer>();

        var data = await notificationDataSerializer.ToStandard(context.Notification.Data);
        var notificationContent = data.Message;

        try
        {
            if (context.Notification.ContentType == NotificationContentType.Html)
            {
                notificationContent = CommonMark.CommonMarkConverter.Convert(notificationContent);
            }

            WeChatWorkWebhookMessage message;

            switch (context.Notification.ContentType)
            {
                case NotificationContentType.Text:
                    message = new WeChatWorkWebhookTextMessage(
                        new WebhookTextMessage(notificationContent));
                    break;
                case NotificationContentType.Html:
                case NotificationContentType.Markdown:
                    if (options.UseMarkdownV2)
                    {
                        message = new WeChatWorkWebhookMarkdownV2Message(
                            new WebhookMarkdownV2Message(notificationContent));
                    }
                    else
                    {
                        message = new WeChatWorkWebhookMarkdownMessage(
                            new WebhookMarkdownMessage(notificationContent));
                    }
                    break;
                default:
                    return;
            }

            if (message == null)
            {
                context.ServiceProvider
                    .GetService<ILogger<WeChatWorkWebhookNotificationContributor>>()
                    ?.LogWarning("Unable to send work weixin messages because WeChatWorkMessage is null.");
                return;
            }

            context.Webhook = new WebhookNotificationData(
                context.Notification.Name,
                message);
            context.Handled = true;
        }
        catch (Exception ex)
        {
            context.ServiceProvider
                .GetService<ILogger<WeChatWorkWebhookNotificationContributor>>()
                ?.LogWarning("Failed to parse the content of the Webhook message: {message}", ex.Message);
        }
    }
}
