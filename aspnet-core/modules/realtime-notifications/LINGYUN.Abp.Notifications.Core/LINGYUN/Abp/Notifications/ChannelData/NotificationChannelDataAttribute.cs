using System;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知渠道数据契约
/// </summary>
/// <remarks>
/// <para>
/// 渠道数据(载荷)是发布渠道与生产者之间约定的数据结构。
/// 生产者通过 <see cref="INotificationChannelDataSerializer"/> 写入载荷,
/// 渠道通过 <see cref="INotificationChannelDataResolver"/> 解析载荷。
/// </para>
/// <para>
/// 载荷的存在性由 <see cref="Presence"/> 声明式表达, 结合
/// <see cref="NotificationChannelPropertyAttribute"/> 的键定义自动推导,
/// 无需在载荷类型中实现判定方法。
/// </para>
/// <para>
/// 若既未声明 <see cref="Presence"/>、又没有可参与匹配的属性,
/// 注册时会抛出 <see cref="AbpNotificationChannelDataConfigurationException"/>:
/// 让"忘了定义存在性"在启动阶段暴露, 而不是在生产环境静默丢弃通知。
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class NotificationChannelDataAttribute : Attribute
{
    /// <summary>
    /// 渠道名称, 对应 <see cref="INotificationPublishProvider.Name"/>
    /// </summary>
    public string ProviderName { get; }

    /// <summary>
    /// 载荷在同一个渠道内的匹配顺序, 数值越小越先匹配
    /// </summary>
    /// <remarks>
    /// 用于表达渠道内的显式规则, 匹配顺序必须声明,
    /// 不允许依赖注册顺序或字段探测顺序。
    /// </remarks>
    public int Priority { get; set; }

    /// <summary>
    /// 存在性判定模式, 默认按"参与匹配的属性全部存在"判定
    /// </summary>
    public NotificationChannelDataPresenceMode Presence { get; set; } = NotificationChannelDataPresenceMode.DeclaredProperties;

    public NotificationChannelDataAttribute(string providerName)
    {
        if (providerName.IsNullOrWhiteSpace())
        {
            throw new ArgumentException("The provider name of the channel data can not be null or whitespace.", nameof(providerName));
        }

        ProviderName = providerName;
    }
}
