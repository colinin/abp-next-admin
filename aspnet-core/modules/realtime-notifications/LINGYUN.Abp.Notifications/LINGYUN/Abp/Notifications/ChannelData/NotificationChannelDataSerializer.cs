using System;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知渠道数据(载荷)序列化器
/// </summary>
/// <remarks>
/// <para>
/// 生产者侧的统一入口, 负责把强类型载荷写入 <see cref="NotificationData"/>。
/// </para>
/// <para>
/// 载荷的写入是"声明式"的: 载荷类型上的 <see cref="NotificationChannelPropertyAttribute"/>
/// 决定写入哪些键以及如何转换, 因此生产者与渠道实现共用同一份契约定义,
/// 不会出现"一方写了键、另一方永远读不到"的情况。
/// </para>
/// </remarks>
public class NotificationChannelDataSerializer : INotificationChannelDataSerializer, ISingletonDependency
{
    protected INotificationChannelDataAccessor Accessor { get; }
    protected INotificationChannelDataPresenceProvider PresenceProvider { get; }

    public NotificationChannelDataSerializer(
        INotificationChannelDataAccessor accessor,
        INotificationChannelDataPresenceProvider presenceProvider)
    {
        Accessor = accessor;
        PresenceProvider = presenceProvider;
    }

    public virtual NotificationData Write<TChannelData>(NotificationData notificationData, TChannelData channelData)
        where TChannelData : class
    {
        Check.NotNull(notificationData, nameof(notificationData));
        Check.NotNull(channelData, nameof(channelData));

        return Write(notificationData, (NotificationChannelData)(object)channelData);
    }

    public virtual NotificationData Write(NotificationData notificationData, NotificationChannelData channelData)
    {
        Check.NotNull(notificationData, nameof(notificationData));
        Check.NotNull(channelData, nameof(channelData));

        foreach (var property in NotificationChannelDataPropertyCache.GetProperties(channelData.GetType()))
        {
            var value = property.GetValue(channelData);
            if (value == null)
            {
                continue;
            }

            Accessor.Set(notificationData, property.Key, property.PropertyType, value, property.Format);
        }

        return notificationData;
    }

    public virtual NotificationData Clear<TChannelData>(NotificationData notificationData)
        where TChannelData : class
    {
        Check.NotNull(notificationData, nameof(notificationData));

        return Clear(notificationData, typeof(TChannelData));
    }

    public virtual NotificationData Clear(NotificationData notificationData, Type channelDataType)
    {
        Check.NotNull(notificationData, nameof(notificationData));
        Check.NotNull(channelDataType, nameof(channelDataType));

        foreach (var property in NotificationChannelDataPropertyCache.GetProperties(channelDataType))
        {
            notificationData.ExtraProperties.Remove(property.Key);
        }

        return notificationData;
    }

    public virtual bool Has<TPayload>(NotificationData notificationData)
        where TPayload : class
    {
        Check.NotNull(notificationData, nameof(notificationData));

        return PresenceProvider.Create(typeof(TPayload))(notificationData);
    }
}
