using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 渠道数据存在性判定提供者
/// </summary>
public interface INotificationChannelDataPresenceProvider
{
    /// <summary>
    /// 获取载荷声明的存在性判定模式
    /// </summary>
    NotificationChannelDataPresenceMode GetPresenceMode(Type channelDataType);

    /// <summary>
    /// 获取参与匹配的属性键
    /// </summary>
    IReadOnlyList<string> GetMatchingKeys(Type channelDataType);

    /// <summary>
    /// 创建存在性判定委托
    /// </summary>
    /// <exception cref="AbpNotificationChannelDataConfigurationException">
    /// 声明了按键判定, 却没有任何可参与匹配的属性
    /// </exception>
    Func<NotificationData, bool> Create(Type channelDataType);
}
