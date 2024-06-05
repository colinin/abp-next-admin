using LINGYUN.Abp.RealTime.Localization;
using LINGYUN.Abp.WxPusher.Features;
using LINGYUN.Abp.WxPusher.Messages;
using LINGYUN.Abp.WxPusher.User;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Notifications.WxPusher;

public class WxPusherNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = "WxPusher";

    public override string Name => ProviderName;

    protected IFeatureChecker FeatureChecker { get; }

    protected IWxPusherUserStore WxPusherUserStore { get; }

    protected IWxPusherMessageSender WxPusherMessageSender { get; }

    protected IStringLocalizerFactory LocalizerFactory { get; }

    protected AbpLocalizationOptions LocalizationOptions { get; }

    protected INotificationDefinitionManager NotificationDefinitionManager { get; }

    public WxPusherNotificationPublishProvider(
        IFeatureChecker featureChecker,
        IWxPusherUserStore wxPusherUserStore,
        IWxPusherMessageSender wxPusherMessageSender, 
        IStringLocalizerFactory localizerFactory, 
        IOptions<AbpLocalizationOptions> localizationOptions, 
        INotificationDefinitionManager notificationDefinitionManager)
    {
        FeatureChecker = featureChecker;
        WxPusherUserStore = wxPusherUserStore;
        WxPusherMessageSender = wxPusherMessageSender;
        LocalizerFactory = localizerFactory;
        LocalizationOptions = localizationOptions.Value;
        NotificationDefinitionManager = notificationDefinitionManager;
    }

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
        NotificationInfo notification, 
        IEnumerable<UserIdentifier> identifiers, 
        CancellationToken cancellationToken = default)
    {
        var subscribeUserIds = identifiers.Select(x => x.UserId);

        var topics = await WxPusherUserStore.GetSubscribeTopicsAsync(subscribeUserIds, cancellationToken);
        var uids = await WxPusherUserStore.GetBindUidsAsync(subscribeUserIds, cancellationToken);

        var notificationDefine = await NotificationDefinitionManager.GetOrNullAsync(notification.Name);
        var url = notification.Data.GetUrlOrNull() ?? notificationDefine?.GetUrlOrNull();
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
                uids: uids,
                url: url,
                cancellationToken: cancellationToken);
        }
        else
        {
            var titleInfo = notification.Data.TryGetData("title").As<LocalizableStringInfo>();
            var titleResource = GetResource(titleInfo.ResourceName);
            var titleLocalizer = await LocalizerFactory.CreateByResourceNameAsync(titleResource.ResourceName);
            var title = titleLocalizer[titleInfo.Name, titleInfo.Values].Value;

            var messageInfo = notification.Data.TryGetData("message").As<LocalizableStringInfo>();
            var messageResource = GetResource(messageInfo.ResourceName);
            var messageLocalizer = await LocalizerFactory.CreateByResourceNameAsync(messageResource.ResourceName);
            var message = messageLocalizer[messageInfo.Name, messageInfo.Values].Value;

            await WxPusherMessageSender.SendAsync(
                content: message,
                summary: title,
                contentType: contentType,
                topicIds: topics,
                uids: uids,
                url: url,
                cancellationToken: cancellationToken);
        }
    }

    private LocalizationResourceBase GetResource(string resourceName)
    {
        return LocalizationOptions.Resources.Values
            .First(x => x.ResourceName.Equals(resourceName));
    }
}
