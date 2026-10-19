using System;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知渠道数据(载荷)序列化器
/// </summary>
/// <remarks>
/// 生产者侧的统一入口, 负责把强类型载荷写入 <see cref="NotificationData"/>。
/// </remarks>
public interface INotificationChannelDataSerializer
{
    /// <summary>
    /// 写入渠道数据(载荷)
    /// </summary>
    /// <typeparam name="TChannelData">载荷类型</typeparam>
    /// <param name="notificationData">通知数据</param>
    /// <param name="channelData">载荷实例</param>
    NotificationData Write<TChannelData>(NotificationData notificationData, TChannelData channelData)
        where TChannelData : class;

    /// <summary>
    /// 写入渠道数据(载荷)
    /// </summary>
    /// <param name="notificationData">通知数据</param>
    /// <param name="channelData">载荷实例</param>
    NotificationData Write(NotificationData notificationData, NotificationChannelData channelData);

    /// <summary>
    /// 按载荷类型清除渠道数据
    /// </summary>
    NotificationData Clear<TChannelData>(NotificationData notificationData)
        where TChannelData : class;

    /// <summary>
    /// 判断通知数据中是否存在指定载荷
    /// </summary>
    bool Has<TPayload>(NotificationData notificationData)
        where TPayload : class;
}
