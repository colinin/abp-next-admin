using LINGYUN.Abp.PushPlus.Channel;
using LINGYUN.Abp.PushPlus.Features;
using LINGYUN.Abp.PushPlus.Message;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Notifications.PushPlus;

public class PushPlusNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = "PushPlus";
    public override string Name => ProviderName;
    protected IFeatureChecker FeatureChecker => ServiceProvider.LazyGetRequiredService<IFeatureChecker>();
    protected ISettingProvider SettingProvider => ServiceProvider.LazyGetRequiredService<ISettingProvider>();
    protected IPushPlusMessageSender PushPlusMessageSender => ServiceProvider.LazyGetRequiredService<IPushPlusMessageSender>();
    protected INotificationDataSerializer NotificationDataSerializer => ServiceProvider.LazyGetRequiredService<INotificationDataSerializer>();
    protected INotificationDefinitionManager NotificationDefinitionManager => ServiceProvider.LazyGetRequiredService<INotificationDefinitionManager>();

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
        var notificationData = await NotificationDataSerializer.ToStandard(notification.Data);

        await PushPlusMessageSender.SendWithChannelAsync(
            notificationData.Title,
            notificationData.Message,
            topic,
            channelType: channel,
            template: template,
            webhook: webhook,
            callbackUrl: callbackUrl,
            cancellationToken: cancellationToken);
    }
}
