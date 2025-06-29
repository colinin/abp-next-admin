﻿using LINGYUN.Abp.RealTime.Localization;
using LINGYUN.Abp.WeChat.Work.Authorize;
using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Messages;
using LINGYUN.Abp.WeChat.Work.Messages.Models;
using LINGYUN.Abp.WeChat.Work.Settings;
using Microsoft.Extensions.Localization;
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
    protected IStringLocalizerFactory LocalizerFactory => ServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>();
    protected IWeChatWorkMessageSender WeChatWorkMessageSender => ServiceProvider.LazyGetRequiredService<IWeChatWorkMessageSender>();
    protected IWeChatWorkInternalUserFinder WeChatWorkInternalUserFinder => ServiceProvider.LazyGetRequiredService<IWeChatWorkInternalUserFinder>();
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
        NotificationInfo notification, 
        IEnumerable<UserIdentifier> identifiers, 
        CancellationToken cancellationToken = default)
    {
        var sendToAgentIds = new List<string>();
        var notificationDefine = await NotificationDefinitionManager.GetOrNullAsync(notification.Name);
        var agentId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.AgentId);
        if (agentId.IsNullOrWhiteSpace())
        {
            Logger.LogWarning("Unable to send enterprise wechat messages because agentId is not set.");
            return;
        }

        var title = "";
        var message = "";
        var description = "";
        var toTag = notification.Data.GetTagOrNull() ?? notificationDefine?.GetTagOrNull();
        var toParty = notification.Data.GetPartyOrNull() ?? notificationDefine?.GetPartyOrNull();

        if (!notification.Data.NeedLocalizer())
        {
            title = notification.Data.TryGetData("title").ToString();
            message = notification.Data.TryGetData("message").ToString();
            description = notification.Data.TryGetData("description")?.ToString() ?? "";
        }
        else
        {
            var titleInfo = notification.Data.TryGetData("title").As<LocalizableStringInfo>();
            var titleLocalizer = await LocalizerFactory.CreateByResourceNameAsync(titleInfo.ResourceName);
            title = titleLocalizer[titleInfo.Name, titleInfo.Values].Value;

            var messageInfo = notification.Data.TryGetData("message").As<LocalizableStringInfo>();
            var messageLocalizer = await LocalizerFactory.CreateByResourceNameAsync(messageInfo.ResourceName);
            message = messageLocalizer[messageInfo.Name, messageInfo.Values].Value;

            var descriptionInfo = notification.Data.TryGetData("description")?.As<LocalizableStringInfo>();
            if (descriptionInfo != null)
            {
                var descriptionLocalizer = await LocalizerFactory.CreateByResourceNameAsync(descriptionInfo.ResourceName);
                description = descriptionLocalizer[descriptionInfo.Name, descriptionInfo.Values].Value;
            }
        }

        var findUserList = await WeChatWorkInternalUserFinder
                .FindUserIdentifierListAsync(identifiers.Select(id => id.UserId));

        if (!findUserList.Any())
        {
            return;
        }

        await PublishToAgentAsync(
            agentId,
            notification,
            findUserList.JoinAsString("|"),
            title,
            message,
            description,
            toParty,
            toTag,
            cancellationToken);
    }

    protected async virtual Task PublishToAgentAsync(
        string agentId,
        NotificationInfo notification,
        string toUser,
        string title,
        string content,
        string description = "",
        string toParty = null,
        string toTag = null,
        CancellationToken cancellationToken = default)
    {
        WeChatWorkMessage message = null;

        switch (notification.ContentType)
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
            return;
        }

        message.ToUser = toUser;
        message.ToTag = toTag;
        message.ToParty = toParty;

        await WeChatWorkMessageSender.SendAsync(message, cancellationToken);
    }
}
