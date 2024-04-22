using LINGYUN.Abp.RealTime.Localization;
using LINGYUN.Abp.TuiJuhe.Features;
using LINGYUN.Abp.TuiJuhe.Messages;
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

namespace LINGYUN.Abp.Notifications.TuiJuhe;

public class TuiJuheNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = "TuiJuhe";

    public override string Name => ProviderName;

    protected IFeatureChecker FeatureChecker { get; }

    protected ITuiJuheMessageSender TuiJuheMessageSender { get; }

    protected IStringLocalizerFactory LocalizerFactory { get; }

    protected AbpLocalizationOptions LocalizationOptions { get; }

    protected INotificationDefinitionManager NotificationDefinitionManager { get; }

    public TuiJuheNotificationPublishProvider(
        IFeatureChecker featureChecker,
        ITuiJuheMessageSender tuiJuheMessageSender,
        IStringLocalizerFactory localizerFactory,
        IOptions<AbpLocalizationOptions> localizationOptions,
        INotificationDefinitionManager notificationDefinitionManager)
    {
        FeatureChecker = featureChecker;
        TuiJuheMessageSender = tuiJuheMessageSender;
        LocalizerFactory = localizerFactory;
        LocalizationOptions = localizationOptions.Value;
        NotificationDefinitionManager = notificationDefinitionManager;
    }

    protected async override Task<bool> CanPublishAsync(NotificationInfo notification, CancellationToken cancellationToken = default)
    {
        if (!await FeatureChecker.IsEnabledAsync(TuiJuheFeatureNames.Message.Enable))
        {
            Logger.LogWarning(
                "{0} cannot push messages because the feature {1} is not enabled",
                Name,
                TuiJuheFeatureNames.Message.Enable);
            return false;
        }
        return true;
    }

    protected async override Task PublishAsync(
        NotificationInfo notification,
        IEnumerable<UserIdentifier> identifiers,
        CancellationToken cancellationToken = default)
    {
        var notificationDefine = await NotificationDefinitionManager.GetOrNullAsync(notification.Name);
        var contentType = notificationDefine?.GetContentTypeOrDefault(MessageContentType.Text)
             ?? MessageContentType.Text;
        var serviceId = notificationDefine?.GetServiceIdOrNull();

        if (serviceId.IsNullOrWhiteSpace())
        {
            Logger.LogWarning(
                "{0} cannot push messages because the notification {1} service id is not specified",
                Name,
                notification.Name);
            return;
        }

        if (!notification.Data.NeedLocalizer())
        {
            var title = notification.Data.TryGetData("title").ToString();
            var message = notification.Data.TryGetData("message").ToString();

            await TuiJuheMessageSender.SendAsync(
                title: title,
                content: message,
                serviceId: serviceId,
                contentType: contentType,
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

            await TuiJuheMessageSender.SendAsync(
                title: title,
                content: message,
                serviceId: serviceId,
                contentType: contentType,
                cancellationToken: cancellationToken);
        }
    }

    private LocalizationResourceBase GetResource(string resourceName)
    {
        return LocalizationOptions.Resources.Values
            .First(x => x.ResourceName.Equals(resourceName));
    }
}
