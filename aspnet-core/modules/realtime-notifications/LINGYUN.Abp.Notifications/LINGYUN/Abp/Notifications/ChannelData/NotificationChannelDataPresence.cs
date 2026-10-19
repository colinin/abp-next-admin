using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 渠道数据存在性判定
/// </summary>
/// <remarks>
public static class NotificationChannelDataPresence
{
    private static readonly Dictionary<string, Func<NotificationData, bool>> Delegates = new();
    private static readonly object SyncLock = new();

    /// <summary>
    /// 获取载荷声明的存在性判定模式
    /// </summary>
    public static NotificationChannelDataPresenceMode GetPresenceMode(Type channelDataType)
    {
        var attribute = channelDataType.GetCustomAttribute<NotificationChannelDataAttribute>(inherit: false);

        return attribute?.Presence ?? NotificationChannelDataPresenceMode.DeclaredProperties;
    }

    /// <summary>
    /// 获取参与匹配的属性键
    /// </summary>
    public static IReadOnlyList<string> GetMatchingKeys(Type channelDataType)
    {
        return channelDataType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(property => new
            {
                Property = property,
                Attribute = property.GetCustomAttribute<NotificationChannelPropertyAttribute>(inherit: true)
            })
            .Where(x => x.Attribute != null && !x.Attribute.IgnoreWhenMatching)
            .Select(x => x.Attribute!.Name.IsNullOrWhiteSpace() ? x.Property.Name : x.Attribute!.Name!)
            .Distinct()
            .ToArray();
    }

    /// <summary>
    /// 校验载荷的存在性声明
    /// </summary>
    /// <exception cref="AbpNotificationChannelDataConfigurationException">
    /// 声明了按键判定, 却没有任何可参与匹配的属性
    /// </exception>
    public static void Validate(Type channelDataType)
    {
        var mode = GetPresenceMode(channelDataType);

        if (mode != NotificationChannelDataPresenceMode.None && GetMatchingKeys(channelDataType).Count == 0)
        {
            throw new AbpNotificationChannelDataConfigurationException(
                $"The channel data type '{channelDataType.FullName}' declares the presence mode '{mode}' " +
                $"but has no matchable property. Annotate at least one property with " +
                $"'{nameof(NotificationChannelPropertyAttribute)}' (and do not set " +
                $"'{nameof(NotificationChannelPropertyAttribute.IgnoreWhenMatching)}' to true), " +
                $"or set '{nameof(NotificationChannelDataAttribute.Presence)}' to " +
                $"'{NotificationChannelDataPresenceMode.None}' if the payload always exists.");
        }
    }

    /// <summary>
    /// 创建存在性判定委托
    /// </summary>
    public static Func<NotificationData, bool> Create(Type channelDataType)
    {
        Validate(channelDataType);

        var mode = GetPresenceMode(channelDataType);
        var keys = GetMatchingKeys(channelDataType);

        var cacheKey = $"{channelDataType.AssemblyQualifiedName}|{mode}|{string.Join(",", keys)}";

        lock (SyncLock)
        {
            if (Delegates.TryGetValue(cacheKey, out var cached))
            {
                return cached;
            }

            var presence = Build(mode, keys);

            Delegates[cacheKey] = presence;

            return presence;
        }
    }

    /// <summary>
    /// 判断通知数据中是否存在该键(大小写不敏感)
    /// </summary>
    /// <remarks>
    /// 精确匹配优先, 未命中时按大小写不敏感匹配,
    /// 与 <see cref="INotificationChannelDataAccessor.TryGetRawValue"/> 的规则保持一致。
    /// </remarks>
    public static bool ContainsKey(NotificationData notificationData, string key)
    {
        if (notificationData == null || key.IsNullOrWhiteSpace())
        {
            return false;
        }

        if (notificationData.ExtraProperties.ContainsKey(key))
        {
            return true;
        }

        foreach (var property in notificationData.ExtraProperties)
        {
            if (string.Equals(property.Key, key, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static Func<NotificationData, bool> Build(
        NotificationChannelDataPresenceMode mode,
        IReadOnlyList<string> keys)
    {
        if (mode == NotificationChannelDataPresenceMode.None)
        {
            return _ => true;
        }

        var notificationData = Expression.Parameter(typeof(NotificationData), "notificationData");
        var containsKeyMethod = typeof(NotificationChannelDataPresence)
            .GetMethod(nameof(ContainsKey), BindingFlags.Public | BindingFlags.Static)!;

        Expression? body = null;

        foreach (var key in keys)
        {
            var containsKey = Expression.Call(
                containsKeyMethod,
                notificationData,
                Expression.Constant(key));

            body = body == null
                ? containsKey
                : mode == NotificationChannelDataPresenceMode.AnyDeclaredProperty
                    ? Expression.OrElse(body, containsKey)
                    : Expression.AndAlso(body, containsKey);
        }

        return Expression
            .Lambda<Func<NotificationData, bool>>(body!, notificationData)
            .Compile();
    }
}
