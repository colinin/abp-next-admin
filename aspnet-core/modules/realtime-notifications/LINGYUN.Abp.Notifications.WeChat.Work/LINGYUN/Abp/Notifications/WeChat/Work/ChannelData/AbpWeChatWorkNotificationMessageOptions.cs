using LINGYUN.Abp.WeChat.Work.Messages;
using System;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 企业微信消息构造选项
/// </summary>
/// <remarks>
/// <para>
/// 渠道数据的形态到消息载体的转换集中注册在这里, 新增一种消息类型只需要
/// <c>options.Map(kind, factory)</c>, 不需要修改 Provider 的发布逻辑。
/// </para>
/// <para>
/// 相比旧的 <c>if/else</c> 探测链, 这里的分派是 O(1) 且显式的:
/// 载荷类型与工厂键不一致时会直接抛出异常, 而不是静默发出错误的消息。
/// </para>
/// </remarks>
public class AbpWeChatWorkNotificationMessageOptions
{
    private readonly Dictionary<WeChatWorkMessageKind, Func<WeChatWorkMessageFactoryContext, WeChatWorkMessageFactoryResult>> _factories
        = new Dictionary<WeChatWorkMessageKind, Func<WeChatWorkMessageFactoryContext, WeChatWorkMessageFactoryResult>>();

    /// <summary>
    /// 已注册的消息类型
    /// </summary>
    public IReadOnlyCollection<WeChatWorkMessageKind> MessageKinds => _factories.Keys;

    /// <summary>
    /// 注册消息构造工厂
    /// </summary>
    public AbpWeChatWorkNotificationMessageOptions Map(
        WeChatWorkMessageKind kind,
        Func<WeChatWorkMessageFactoryContext, WeChatWorkMessageFactoryResult> factory)
    {
        Check.NotNull(factory, nameof(factory));

        _factories[kind] = factory;

        return this;
    }

    /// <summary>
    /// 构造消息载体
    /// </summary>
    public WeChatWorkMessageFactoryResult Create(WeChatWorkMessageFactoryContext context)
    {
        Check.NotNull(context, nameof(context));

        var kind = context.ChannelData.Kind;

        if (!_factories.TryGetValue(kind, out var factory))
        {
            throw new AbpNotificationChannelDataConfigurationException(
                $"The WeChat Work message kind '{kind}' has not been mapped, please call '{nameof(Map)}' when configuring '{nameof(AbpWeChatWorkNotificationMessageOptions)}'.");
        }

        var result = factory(context);
        if (result == null || result.Message == null)
        {
            throw new AbpNotificationChannelDataConfigurationException(
                $"The factory of the WeChat Work message kind '{kind}' returned an empty message.");
        }

        return result;
    }

    /// <summary>
    /// 获取已注册的工厂
    /// </summary>
    public bool TryGetFactory(
        WeChatWorkMessageKind kind,
        out Func<WeChatWorkMessageFactoryContext, WeChatWorkMessageFactoryResult>? factory)
    {
        return _factories.TryGetValue(kind, out factory);
    }
}
