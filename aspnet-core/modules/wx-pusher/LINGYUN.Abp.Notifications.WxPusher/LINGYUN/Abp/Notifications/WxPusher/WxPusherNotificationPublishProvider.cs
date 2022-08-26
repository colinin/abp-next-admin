using LINGYUN.Abp.RealTime.Localization;
using LINGYUN.Abp.WxPusher.Messages;
using LINGYUN.Abp.WxPusher.User;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Notifications.WxPusher;

public class WxPusherNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = "WxPusher";

    public override string Name => ProviderName;

    protected IWxPusherUserStore WxPusherUserStore { get; }

    protected IWxPusherMessageSender WxPusherMessageSender { get; }

    protected IStringLocalizerFactory LocalizerFactory { get; }

    protected AbpLocalizationOptions LocalizationOptions { get; }

    protected INotificationDefinitionManager NotificationDefinitionManager { get; }

    public WxPusherNotificationPublishProvider(
        IWxPusherUserStore wxPusherUserStore,
        IWxPusherMessageSender wxPusherMessageSender, 
        IStringLocalizerFactory localizerFactory, 
        IOptions<AbpLocalizationOptions> localizationOptions, 
        INotificationDefinitionManager notificationDefinitionManager)
    {
        WxPusherUserStore = wxPusherUserStore;
        WxPusherMessageSender = wxPusherMessageSender;
        LocalizerFactory = localizerFactory;
        LocalizationOptions = localizationOptions.Value;
        NotificationDefinitionManager = notificationDefinitionManager;
    }

    protected async override Task PublishAsync(
        NotificationInfo notification, 
        IEnumerable<UserIdentifier> identifiers, 
        CancellationToken cancellationToken = default)
    {
        var topics = await WxPusherUserStore
            .GetSubscribeTopicsAsync(
                identifiers.Select(x => x.UserId),
                cancellationToken);

        var notificationDefine = NotificationDefinitionManager.GetOrNull(notification.Name);
        var topicDefine = notificationDefine?.GetTopics();
        if (topicDefine.Any())
        {
            topics = topicDefine;
        }
        var contentType = notificationDefine?.GetContentTypeOrDefault(MessageContentType.Text)
             ?? MessageContentType.Text;

        if (!notification.Data.NeedLocalizer())
        {
            var title = notification.Data.TryGetData("title").ToString();
            var message = notification.Data.TryGetData("message").ToString();

            await WxPusherMessageSender.SendAsync(
                content: message,
                summary: title,
                contentType: contentType,
                topicIds: topics,
                cancellationToken: cancellationToken);
        }
        else
        {
            var titleInfo = notification.Data.TryGetData("title").As<LocalizableStringInfo>();
            var titleResource = GetResource(titleInfo.ResourceName);
            var title = LocalizerFactory.Create(titleResource.ResourceType)[titleInfo.Name, titleInfo.Values].Value;

            var messageInfo = notification.Data.TryGetData("message").As<LocalizableStringInfo>();
            var messageResource = GetResource(messageInfo.ResourceName);
            var message = LocalizerFactory.Create(messageResource.ResourceType)[messageInfo.Name, messageInfo.Values].Value;

            await WxPusherMessageSender.SendAsync(
                content: message,
                summary: title,
                contentType: contentType,
                topicIds: topics,
                cancellationToken: cancellationToken);
        }
    }

    private LocalizationResource GetResource(string resourceName)
    {
        return LocalizationOptions.Resources.Values
            .First(x => x.ResourceName.Equals(resourceName));
    }
}
