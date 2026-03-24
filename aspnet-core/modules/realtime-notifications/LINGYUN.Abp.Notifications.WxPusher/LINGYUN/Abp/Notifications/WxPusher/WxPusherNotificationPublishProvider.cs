using LINGYUN.Abp.WxPusher.Features;
using LINGYUN.Abp.WxPusher.Messages;
using LINGYUN.Abp.WxPusher.User;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Notifications.WxPusher;

public class WxPusherNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = "WxPusher";
    public override string Name => ProviderName;
    protected IFeatureChecker FeatureChecker => ServiceProvider.LazyGetRequiredService<IFeatureChecker>();
    protected IWxPusherUserStore WxPusherUserStore => ServiceProvider.LazyGetRequiredService<IWxPusherUserStore>();
    protected IWxPusherMessageSender WxPusherMessageSender => ServiceProvider.LazyGetRequiredService<IWxPusherMessageSender>();
    protected INotificationDataSerializer NotificationDataSerializer => ServiceProvider.LazyGetRequiredService<INotificationDataSerializer>();
    protected INotificationDefinitionManager NotificationDefinitionManager => ServiceProvider.LazyGetRequiredService<INotificationDefinitionManager>();

    protected async override Task<bool> CanPublishAsync(NotificationInfo notification, CancellationToken cancellationToken = default)
    {
        if (!await FeatureChecker.IsEnabledAsync(true,
            WxPusherFeatureNames.Enable,
            WxPusherFeatureNames.Message.Enable))
        {
            Logger.LogWarning(
                "{0} cannot push messages because the feature {1} is not enabled",
                Name,
                WxPusherFeatureNames.Message.Enable);
            return false;
        }
        return true;
    }

    protected async override Task PublishAsync(
        NotificationPublishContext context,
        CancellationToken cancellationToken = default)
    {
        var subscribeUserIds = context.Users.Select(x => x.UserId);

        var topics = await WxPusherUserStore.GetSubscribeTopicsAsync(subscribeUserIds, cancellationToken);
        var uids = await WxPusherUserStore.GetBindUidsAsync(subscribeUserIds, cancellationToken);

        var notificationDefine = await NotificationDefinitionManager.GetOrNullAsync(context.Notification.Name);
        var url = context.Notification.Data.GetUrlOrNull() ?? notificationDefine?.GetUrlOrNull();
        var topicDefine = notificationDefine?.GetTopics();
        if (topicDefine.Any())
        {
            topics = topicDefine;
        }
        var contentType = notificationDefine?.GetContentTypeOrDefault(MessageContentType.Text)
             ?? MessageContentType.Text;
        var notificationData = await NotificationDataSerializer.ToStandard(context.Notification.Data);

        await WxPusherMessageSender.SendAsync(
            content: notificationData.Message,
            summary: notificationData.Title,
            contentType: contentType,
            topicIds: topics,
            uids: uids,
            url: url,
            cancellationToken: cancellationToken);

        Logger.LogDebug("The notification: {0} with provider: {1} has successfully published!", context.Notification.Name, Name);
    }
}
