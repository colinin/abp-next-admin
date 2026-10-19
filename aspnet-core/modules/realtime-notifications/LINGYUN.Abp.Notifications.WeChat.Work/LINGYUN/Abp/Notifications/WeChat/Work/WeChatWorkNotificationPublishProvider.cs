using LINGYUN.Abp.WeChat.Work.Authorize;
using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Messages;
using LINGYUN.Abp.WeChat.Work.Messages.Models;
using LINGYUN.Abp.WeChat.Work.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 企业微信通知发布渠道
/// </summary>
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
    protected INotificationChannelDataResolver ChannelDataResolver => ServiceProvider.LazyGetRequiredService<INotificationChannelDataResolver>();
    protected IOptions<AbpWeChatWorkNotificationMessageOptions> MessageOptions => ServiceProvider.LazyGetRequiredService<IOptions<AbpWeChatWorkNotificationMessageOptions>>();

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
        var agentId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.AgentId);
        if (agentId.IsNullOrWhiteSpace())
        {
            context.Cancel("Unable to send work weixin messages because agentId is not set.");
            Logger.LogWarning(context.Reason);
            return;
        }

        var notificationData = await NotificationDataSerializer.ToStandard(context.Notification.Data);
        if (notificationData.Title.IsNullOrWhiteSpace() && notificationData.Message.IsNullOrWhiteSpace())
        {
            context.Cancel("Unable to send work weixin messages because the title and content of the message must not be empty.");
            Logger.LogWarning(context.Reason);
            return;
        }

        var notificationDefine = await NotificationDefinitionManager.GetOrNullAsync(context.Notification.Name);

        var contact = await ResolveContactAsync(context, notificationDefine, cancellationToken);
        if (contact.IsEmpty())
        {
            // touser、toparty、totag不能同时为空：https://developer.work.weixin.qq.com/document/path/90236
            context.Cancel("Unable to send work weixin messages because The recipient/department/label cannot be empty simultaneously.");
            Logger.LogWarning(context.Reason);
            return;
        }

        var options = WeChatWorkMessageOptions.From(ChannelDataResolver, context.Notification.Data);
        options.AgentId = agentId;

        var messages = ResolveMessages(context, notificationData, contact, options);
        if (messages.Count == 0)
        {
            context.Cancel("Unable to send work weixin messages because WeChatWorkMessage is null.");
            Logger.LogWarning(context.Reason);
            return;
        }
        var failures = new List<Exception>();

        foreach (var message in messages)
        {
            try
            {
                var response = await WeChatWorkMessageSender.SendAsync(message.Message, cancellationToken);

                if (!response.IsSuccessed)
                {
                    // 企业微信以业务错误码返回失败, 必须让上层感知, 否则失败会被记录为成功
                    context.Cancel(
                        $"Unable to send work weixin messages, error code: {response.ErrorCode}, error message: {response.ErrorMessage}");

                    Logger.LogWarning(
                        "The notification: {Notification} with provider: {Provider} and message kind: {Kind} failed, error code: {ErrorCode}, error message: {ErrorMessage}",
                        context.Notification.Name, Name, message.Kind, response.ErrorCode, response.ErrorMessage);
                    continue;
                }

                Logger.LogDebug(
                    "The notification: {Notification} with provider: {Provider} and message kind: {Kind} has successfully published!",
                    context.Notification.Name, Name, message.Kind);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex,
                    "Send notification {Notification} with provider {Provider} and message kind {Kind} failed",
                    context.Notification.Name, Name, message.Kind);
                failures.Add(ex);
            }
        }

        if (failures.Count > 0)
        {
            context.Cancel(
                $"Failed to send work weixin messages of notification '{context.Notification.Name}'.",
                new AggregateException(
                    $"Failed to send work weixin messages of notification '{context.Notification.Name}'.",
                    failures));
        }
    }

    /// <summary>
    /// 解析消息接收者
    /// </summary>
    /// <remarks>
    /// 成员标识由企业微信身份绑定关系解析, 部门与标签优先取通知数据, 其次取通知定义。
    /// </remarks>
    protected async virtual Task<WeChatWorkNotificationContact> ResolveContactAsync(
        NotificationPublishContext context,
        NotificationDefinition? notificationDefine,
        CancellationToken cancellationToken = default)
    {
        var toUser = await WeChatWorkInternalUserFinder.FindUserIdentifierListAsync(
            context.Users.Select(id => id.UserId),
            cancellationToken);

        var toTag = ChannelDataResolver.GetOrNull(context.Notification.Data, WeChatWorkNotificationDataKeys.ToTag)
            ?? notificationDefine?.GetTagOrNull();

        var toParty = ChannelDataResolver.GetOrNull(context.Notification.Data, WeChatWorkNotificationDataKeys.ToParty)
            ?? notificationDefine?.GetPartyOrNull();

        return WeChatWorkNotificationContact.From(
            toUser: toUser.JoinAsString("|"),
            toParty: toParty,
            toTag: toTag);
    }

    /// <summary>
    /// 解析待发送的消息载体集合
    /// </summary>
    /// <remarks>
    /// 优先使用生产者显式声明的渠道数据(载荷);
    /// 未声明时按通知内容类型推导默认载体, 保证既有生产者的行为不变。
    /// </remarks>
    protected virtual List<WeChatWorkMessageFactoryResult> ResolveMessages(
        NotificationPublishContext context,
        NotificationStandardData standardData,
        WeChatWorkNotificationContact contact,
        WeChatWorkMessageOptions options)
    {
        var messages = new List<WeChatWorkMessageFactoryResult>();

        if (TryResolveChannelData(context, out var invalidPayload, out var resolvedPayloads))
        {
            foreach (var channelData in resolvedPayloads)
            {
                messages.Add(CreateMessage(channelData, context.Notification, standardData, contact, options));
            }
        }
        else
        {
            context.Cancel(
                $"Unable to send work weixin messages because the work weixin payload '{invalidPayload}' can not be parsed, please check the notification data written by the producer.");
            Logger.LogWarning(context.Reason);
            return messages;
        }

        if (messages.Count == 0)
        {
            var defaultChannelData = CreateDefaultChannelData(context, standardData);
            if (defaultChannelData != null)
            {
                messages.Add(CreateMessage(defaultChannelData, context.Notification, standardData, contact, options));
            }
        }

        return messages;
    }

    /// <summary>
    /// 解析渠道数据(载荷)
    /// </summary>
    /// <remarks>
    /// 区分"生产者未写入载荷"与"载荷已写入但无法解析":
    /// 后者是生产者错误, 必须明确报错, 而不是静默回退到默认载体。
    /// </remarks>
    protected virtual bool TryResolveChannelData(
        NotificationPublishContext context,
        out string? invalidPayloadKey,
        out List<NotificationChannelData> resolvedPayloads)
    {
        invalidPayloadKey = null;
        resolvedPayloads = new List<NotificationChannelData>();

        foreach (var payloadType in ChannelDataResolver.GetPayloadTypes(ProviderName))
        {
            if (!ChannelDataResolver.IsDeclared(context.Notification.Data, payloadType))
            {
                continue;
            }

            var payload = ChannelDataResolver.Resolve(payloadType, context.Notification.Data);

            if (payload == null)
            {
                invalidPayloadKey = ChannelDataResolver.GetKey(context.Notification.Data, payloadType);
                return false;
            }

            resolvedPayloads.Add(payload);
        }

        return true;
    }

    /// <summary>
    /// 按通知内容类型推导默认的消息载体
    /// </summary>
    protected virtual WeChatWorkChannelData? CreateDefaultChannelData(
        NotificationPublishContext context,
        NotificationStandardData standardData)
    {
        switch (context.Notification.ContentType)
        {
            case NotificationContentType.Text:
                return new WeChatWorkTextChannelData
                {
                    Content = standardData.Message
                };

            case NotificationContentType.Html:
                return new WeChatWorkTextCardChannelData
                {
                    TextCard = new TextCardMessage(
                        standardData.Title ?? string.Empty,
                        // 旧实现丢弃了 description, 这里补全文本卡片的描述
                        standardData.Description.IsNullOrWhiteSpace() ? standardData.Message : standardData.Description!,
                        // 跳转地址应由生产者通过载荷显式指定, 默认不可点击
                        "javascript:;")
                };

            case NotificationContentType.Markdown:
                return new WeChatWorkMarkdownChannelData
                {
                    Content = standardData.Message
                };

            default:
                Logger.LogWarning(
                    "The notification: {Notification} with provider: {Provider} can not resolve a work weixin message for content type: {ContentType}.",
                    context.Notification.Name, Name, context.Notification.ContentType);
                return null;
        }
    }

    protected virtual WeChatWorkMessageFactoryResult CreateMessage(
        NotificationChannelData channelData,
        NotificationInfo notification,
        NotificationStandardData standardData,
        WeChatWorkNotificationContact contact,
        WeChatWorkMessageOptions options)
    {
        if (channelData is not WeChatWorkChannelData weChatWorkChannelData)
        {
            throw new AbpNotificationChannelDataConfigurationException(
                $"The channel data '{channelData.GetType().FullName}' is not a work weixin channel data.");
        }

        var factoryContext = new WeChatWorkMessageFactoryContext(
            notification,
            standardData,
            weChatWorkChannelData,
            contact,
            options);

        return MessageOptions.Value.Create(factoryContext);
    }
}
