using LINGYUN.Abp.WeChat.Work.Messages;
using System;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 企业微信 Webhook 消息构造选项
/// </summary>
/// <remarks>
/// <para>
/// 渠道数据的形态到消息载体的转换集中注册在这里, 新增一种消息类型只需要
/// <c>options.Map(kind, factory)</c>, 不需要修改贡献者的发布逻辑。
/// </para>
/// </remarks>
public class AbpNotificationsWebhookWeChatWorkMessageOptions
{
    private readonly Dictionary<WeChatWorkWebhookMessageKind, Func<WeChatWorkWebhookMessageFactoryContext, WeChatWorkWebhookMessageFactoryResult>> _factories
        = new Dictionary<WeChatWorkWebhookMessageKind, Func<WeChatWorkWebhookMessageFactoryContext, WeChatWorkWebhookMessageFactoryResult>>();

    /// <summary>
    /// 已注册的消息类型
    /// </summary>
    public IReadOnlyCollection<WeChatWorkWebhookMessageKind> MessageKinds => _factories.Keys;

    /// <summary>
    /// 注册消息构造工厂
    /// </summary>
    public AbpNotificationsWebhookWeChatWorkMessageOptions Map(
        WeChatWorkWebhookMessageKind kind,
        Func<WeChatWorkWebhookMessageFactoryContext, WeChatWorkWebhookMessageFactoryResult> factory)
    {
        Check.NotNull(factory, nameof(factory));

        _factories[kind] = factory;

        return this;
    }

    /// <summary>
    /// 构造消息载体
    /// </summary>
    public WeChatWorkWebhookMessageFactoryResult Create(WeChatWorkWebhookMessageFactoryContext context)
    {
        Check.NotNull(context, nameof(context));

        var kind = context.ChannelData.Kind;

        if (!_factories.TryGetValue(kind, out var factory))
        {
            throw new AbpNotificationChannelDataConfigurationException(
                $"The WeChat Work webhook message kind '{kind}' has not been mapped, please call '{nameof(Map)}' when configuring '{nameof(AbpNotificationsWebhookWeChatWorkMessageOptions)}'.");
        }

        var result = factory(context);
        if (result == null || result.Message == null)
        {
            throw new AbpNotificationChannelDataConfigurationException(
                $"The factory of the WeChat Work webhook message kind '{kind}' returned an empty message.");
        }

        return result;
    }

    /// <summary>
    /// 获取已注册的工厂
    /// </summary>
    public bool TryGetFactory(
        WeChatWorkWebhookMessageKind kind,
        out Func<WeChatWorkWebhookMessageFactoryContext, WeChatWorkWebhookMessageFactoryResult>? factory)
    {
        return _factories.TryGetValue(kind, out factory);
    }
}
