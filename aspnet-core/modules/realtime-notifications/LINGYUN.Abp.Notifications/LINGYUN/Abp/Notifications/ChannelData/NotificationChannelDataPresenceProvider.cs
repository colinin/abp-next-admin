using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 渠道数据存在性判定提供者
/// </summary>
public class NotificationChannelDataPresenceProvider : INotificationChannelDataPresenceProvider, ISingletonDependency
{
    public virtual NotificationChannelDataPresenceMode GetPresenceMode(Type channelDataType)
    {
        return NotificationChannelDataPresence.GetPresenceMode(channelDataType);
    }

    public virtual IReadOnlyList<string> GetMatchingKeys(Type channelDataType)
    {
        return NotificationChannelDataPresence.GetMatchingKeys(channelDataType);
    }

    public virtual Func<NotificationData, bool> Create(Type channelDataType)
    {
        return NotificationChannelDataPresence.Create(channelDataType);
    }
}
