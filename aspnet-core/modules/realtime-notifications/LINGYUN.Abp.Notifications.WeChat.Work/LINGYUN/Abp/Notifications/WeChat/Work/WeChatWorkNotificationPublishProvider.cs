using LINGYUN.Abp.WeChat.Work.Authorize;
using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Messages;
using LINGYUN.Abp.WeChat.Work.Messages.Models;
using LINGYUN.Abp.WeChat.Work.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Notifications.WeChat.Work;
public class WeChatWorkNotificationPublishProvider : NotificationPublishProvider
{

    public const string ProviderName = NotificationProviderNames.WechatWork;
    public override string Name => ProviderName;
    protected IFeatureChecker FeatureChecker => ServiceProvider.LazyGetRequiredService<IFeatureChecker>();
    protected ISettingProvider SettingProvider => ServiceProvider.LazyGetRequiredService<ISettingProvider>();
    protected IWeChatWorkMessageSender WeChatWorkMessageSender => ServiceProvider.LazyGetRequiredService<IWeChatWorkMessageSender>();
    protected INotificationDataSerializer NotificationDataSerializer => ServiceProvider.LazyGetRequiredService<INotificationDataSerializer>();
    protected IWeChatWorkUserClaimProvider WeChatWorkInternalUserFinder => ServiceProvider.LazyGetRequiredService<IWeChatWorkUserClaimProvider>();
    protected INotificationDefinitionManager NotificationDefinitionManager => ServiceProvider.LazyGetRequiredService<INotificationDefinitionManager>();

    protected async override Task<bool> CanPublishAsync(NotificationInfo notification, CancellationToken cancellationToken = default)
    {
        if (!await FeatureChecker.IsEnabledAsync(true,
            WeChatWorkFeatureNames.Enable,
            WeChatWorkFeatureNames.Message.Enable))
        {
            Logger.LogWarning(
                "{0} cannot push messages because the feature {1} is not enabled",
                Name,
                WeChatWorkFeatureNames.Message.Enable);
            return false;
        }
        return true;
    }

    protected async override Task PublishAsync(
        NotificationPublishContext context,
        CancellationToken cancellationToken = default)
    {
        var sendToAgentIds = new List<string>();
        var notificationDefine = await NotificationDefinitionManager.GetOrNullAsync(context.Notification.Name);
        var agentId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.AgentId);
        if (agentId.IsNullOrWhiteSpace())
        {
            context.Cancel("Unable to send work weixin messages because agentId is not set.");
            Logger.LogWarning(context.Reason);
            return;
        }
        var notificationData = await NotificationDataSerializer.ToStandard(context.Notification.Data);
        var toTag = context.Notification.Data.GetTagOrNull() ?? notificationDefine?.GetTagOrNull();
        var toParty = context.Notification.Data.GetPartyOrNull() ?? notificationDefine?.GetPartyOrNull();
        var toUsers = await WeChatWorkInternalUserFinder.FindUserIdentifierListAsync(context.Users.Select(id => id.UserId));

        if (toUsers.IsNullOrEmpty() && toTag.IsNullOrWhiteSpace() && toParty.IsNullOrWhiteSpace())
        {
            // touser、toparty、totag不能同时为空：https://developer.work.weixin.qq.com/document/path/90236
            context.Cancel("Unable to send work weixin messages because The recipient/department/label cannot be empty simultaneously.");
            Logger.LogWarning(context.Reason);
            return;
        }

        // 发送到个人
        await PublishToAgentAsync(
            context,
            agentId,
            notificationData.Title,
            notificationData.Message,
            notificationData.Description,
            toTag: toTag,
            toParty: toParty,
            toUser: toUsers.JoinAsString("|"),
            cancellationToken: cancellationToken);
    }

    protected async virtual Task PublishToAgentAsync(
        NotificationPublishContext context,
        string agentId,
        string title,
        string content,
        string description = "",
        string toUser = null,
        string toParty = null,
        string toTag = null,
        CancellationToken cancellationToken = default)
    {
        WeChatWorkMessage message = null;

        switch (context.Notification.ContentType)
        {
            case NotificationContentType.Text:
                message = new WeChatWorkTextMessage(agentId, new TextMessage(content));
                break;
            case NotificationContentType.Html:
                message = new WeChatWorkTextCardMessage(agentId, new TextCardMessage(title, content, "javascript(0);"));
                break;
            case NotificationContentType.Markdown:
                message = new WeChatWorkMarkdownMessage(agentId, new MarkdownMessage(content));
                break;
            default:
                break;
        }

        if (message == null)
        {
            context.Cancel("Unable to send work weixin messages because WeChatWorkMessage is null.");
            Logger.LogWarning(context.Reason);
            return;
        }

        message.ToUser = toUser;
        message.ToTag = toTag;
        message.ToParty = toParty;

        await WeChatWorkMessageSender.SendAsync(message, cancellationToken);

        Logger.LogDebug("The notification: {0} with provider: {1} has successfully published!", context.Notification.Name, Name);
    }
}
