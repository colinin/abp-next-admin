using System;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知渠道数据(载荷)描述符
/// </summary>
public sealed class NotificationChannelDataDescriptor
{
    /// <summary>
    /// 渠道名称
    /// </summary>
    public string ProviderName { get; }

    /// <summary>
    /// 载荷类型
    /// </summary>
    public Type PayloadType { get; }

    /// <summary>
    /// 匹配顺序, 数值越小越先匹配
    /// </summary>
    public int Priority { get; }

    /// <summary>
    /// 存在性判定模式
    /// </summary>
    public NotificationChannelDataPresenceMode Presence { get; }

    /// <summary>
    /// 判断通知数据中是否存在本载荷
    /// </summary>
    public Func<NotificationData, bool> IsPresent { get; }

    public NotificationChannelDataDescriptor(
        string providerName,
        Type payloadType,
        int priority,
        NotificationChannelDataPresenceMode presence,
        Func<NotificationData, bool> isPresent)
    {
        ProviderName = providerName;
        PayloadType = payloadType;
        Priority = priority;
        Presence = presence;
        IsPresent = isPresent;
    }

    public override string ToString()
    {
        return $"{ProviderName}:{PayloadType.Name}(Priority={Priority}, Presence={Presence})";
    }
}
