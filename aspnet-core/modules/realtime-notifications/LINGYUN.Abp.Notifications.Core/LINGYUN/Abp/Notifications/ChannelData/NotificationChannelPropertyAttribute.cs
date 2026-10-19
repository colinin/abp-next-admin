using System;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 标记通知渠道数据(载荷)的属性参与类型化读写
/// </summary>
/// <remarks>
/// <para>
/// 参与读写的属性名称默认取属性名, 也可以通过 <see cref="Name"/> 显式指定,
/// 以便兼容历史遗留的通知数据键(例如 <c>TemplateId</c> 这类未加前缀的键)。
/// </para>
/// <para>
/// 读写过程会做弱类型转换, 因此数据无论经历过多少次
/// JSON 序列化/反序列化(持久化、失败重试、跨进程投递)都不会丢失。
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class NotificationChannelPropertyAttribute : Attribute
{
    /// <summary>
    /// 通知数据中的键名, 为空时取属性名
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// 匹配载荷时是否忽略本属性
    /// </summary>
    /// <remarks>
    /// 适用于"投递选项"这类不决定载荷形态的属性,
    /// 例如企业微信的接收人(to_user/toparty/totag)。
    /// 投递选项单独存在时不代表某个消息载荷存在。
    /// </remarks>
    public bool IgnoreWhenMatching { get; set; }

    public NotificationChannelPropertyAttribute()
    {
    }

    public NotificationChannelPropertyAttribute(string name)
    {
        Name = name;
    }
}
