using LINGYUN.Abp.PushPlus.Message;
using LINGYUN.Abp.RealTime.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Notifications.PushPlus;

public class PushPlusNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = "PushPlus";

    public override string Name => ProviderName;

    protected IPushPlusMessageSender PushPlusMessageSender { get; }

    protected IStringLocalizerFactory LocalizerFactory { get; }

    protected AbpLocalizationOptions LocalizationOptions { get; }

    protected INotificationDefinitionManager NotificationDefinitionManager { get; }

    public PushPlusNotificationPublishProvider(
        IPushPlusMessageSender pushPlusMessageSender, 
        IStringLocalizerFactory localizerFactory, 
        IOptions<AbpLocalizationOptions> localizationOptions, 
        INotificationDefinitionManager notificationDefinitionManager)
    {
        PushPlusMessageSender = pushPlusMessageSender;
        LocalizerFactory = localizerFactory;
        LocalizationOptions = localizationOptions.Value;
        NotificationDefinitionManager = notificationDefinitionManager;
    }

    protected async override Task PublishAsync(
        NotificationInfo notification, 
        IEnumerable<UserIdentifier> identifiers, 
        CancellationToken cancellationToken = default)
    {
        var topic = "";

        var notificationDefine = NotificationDefinitionManager.GetOrNull(notification.Name);
        var topicDefine = notificationDefine?.GetTopicOrNull();
        if (!topicDefine.IsNullOrWhiteSpace())
        {
            topic = topicDefine;
        }

        if (!notification.Data.NeedLocalizer())
        {
            var title = notification.Data.TryGetData("title").ToString();
            var message = notification.Data.TryGetData("message").ToString();

            await PushPlusMessageSender.SendAsync(
                title,
                message,
                topic,
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

            await PushPlusMessageSender.SendAsync(
                title,
                message,
                topic,
                cancellationToken: cancellationToken);
        }
    }

    private LocalizationResource GetResource(string resourceName)
    {
        return LocalizationOptions.Resources.Values
            .First(x => x.ResourceName.Equals(resourceName));
    }
}
