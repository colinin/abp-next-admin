using LINGYUN.Abp.PushPlus.Channel;
using LINGYUN.Abp.PushPlus.Features;
using LINGYUN.Abp.PushPlus.Message;
using LINGYUN.Abp.RealTime.Localization;
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

namespace LINGYUN.Abp.Notifications.PushPlus;

public class PushPlusNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = "PushPlus";

    public override string Name => ProviderName;

    protected IFeatureChecker FeatureChecker { get; }

    protected IPushPlusMessageSender PushPlusMessageSender { get; }

    protected IStringLocalizerFactory LocalizerFactory { get; }

    protected AbpLocalizationOptions LocalizationOptions { get; }

    protected INotificationDefinitionManager NotificationDefinitionManager { get; }

    public PushPlusNotificationPublishProvider(
        IFeatureChecker featureChecker,
        IPushPlusMessageSender pushPlusMessageSender, 
        IStringLocalizerFactory localizerFactory, 
        IOptions<AbpLocalizationOptions> localizationOptions, 
        INotificationDefinitionManager notificationDefinitionManager)
    {
        FeatureChecker = featureChecker;
        PushPlusMessageSender = pushPlusMessageSender;
        LocalizerFactory = localizerFactory;
        LocalizationOptions = localizationOptions.Value;
        NotificationDefinitionManager = notificationDefinitionManager;
    }

    protected async override Task<bool> CanPublishAsync(NotificationInfo notification, CancellationToken cancellationToken = default)
    {
        if (!await FeatureChecker.IsEnabledAsync(PushPlusFeatureNames.Message.Enable))
        {
            Logger.LogWarning(
                "{0} cannot push messages because the feature {1} is not enabled",
                Name,
                PushPlusFeatureNames.Message.Enable);
            return false;
        }
        return true;
    }

    protected async override Task PublishAsync(
        NotificationInfo notification, 
        IEnumerable<UserIdentifier> identifiers, 
        CancellationToken cancellationToken = default)
    {
        var topic = "";

        var notificationDefine = await NotificationDefinitionManager.GetOrNullAsync(notification.Name);
        var topicDefine = notificationDefine?.GetTopicOrNull();
        if (!topicDefine.IsNullOrWhiteSpace())
        {
            topic = topicDefine;
        }
        var channel = notificationDefine?.GetChannelOrDefault(PushPlusChannelType.Email)
             ?? PushPlusChannelType.Email;
        var template = notificationDefine?.GetTemplateOrDefault(PushPlusMessageTemplate.Text)
             ?? PushPlusMessageTemplate.Text;
        var webhook = notification.Data.GetWebhookOrNull() ?? "";
        var callbackUrl = notification.Data.GetCallbackUrlOrNull() ?? "";

        if (!notification.Data.NeedLocalizer())
        {
            var title = notification.Data.TryGetData("title").ToString();
            var message = notification.Data.TryGetData("message").ToString();

            await PushPlusMessageSender.SendWithChannelAsync(
                title,
                message,
                topic,
                channelType: channel,
                template: template,
                webhook: webhook,
                callbackUrl: callbackUrl,
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

            await PushPlusMessageSender.SendWithChannelAsync(
                title,
                message,
                topic,
                channelType: channel,
                template: template,
                webhook: webhook,
                callbackUrl: callbackUrl,
                cancellationToken: cancellationToken);
        }
    }

    private LocalizationResourceBase GetResource(string resourceName)
    {
        return LocalizationOptions.Resources.Values
            .First(x => x.ResourceName.Equals(resourceName));
    }
}
