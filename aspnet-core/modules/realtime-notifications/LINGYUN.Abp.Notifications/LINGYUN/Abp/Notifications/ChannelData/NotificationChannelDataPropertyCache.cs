using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 渠道数据属性元数据缓存
/// </summary>
/// <remarks>
/// 纯反射元数据缓存(无 JSON、无日志、无配置依赖), 因此保持静态以避免
/// 在序列化器、解析器、存在性提供者之间重复注入。
/// </remarks>
public static class NotificationChannelDataPropertyCache
{
    private static readonly Dictionary<Type, NotificationChannelDataProperty[]> PropertiesCache = new();
    private static readonly object SyncLock = new();

    /// <summary>
    /// 获取载荷类型的属性元数据
    /// </summary>
    public static NotificationChannelDataProperty[] GetProperties(Type channelDataType)
    {
        lock (SyncLock)
        {
            if (PropertiesCache.TryGetValue(channelDataType, out var properties))
            {
                return properties;
            }

            properties = channelDataType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(TryCreateProperty)
                .Where(x => x != null)
                .Select(x => x!)
                .ToArray();

            PropertiesCache[channelDataType] = properties;

            return properties;
        }
    }

    private static NotificationChannelDataProperty? TryCreateProperty(PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<NotificationChannelPropertyAttribute>(inherit: true);
        if (attribute == null || !property.CanRead)
        {
            return null;
        }

        var key = attribute.Name.IsNullOrWhiteSpace() ? property.Name : attribute.Name!;

        return new NotificationChannelDataProperty(
            key,
            property.PropertyType,
            NotificationChannelDataFormat.Auto,
            attribute.IgnoreWhenMatching,
            CreateGetter(property),
            property.CanWrite ? CreateSetter(property) : null!);
    }

    private static Func<object, object?> CreateGetter(PropertyInfo property)
    {
        var instance = Expression.Parameter(typeof(object), "instance");
        var typedInstance = Expression.Convert(instance, property.DeclaringType!);
        var propertyAccess = Expression.Property(typedInstance, property);

        return Expression
            .Lambda<Func<object, object?>>(Expression.Convert(propertyAccess, typeof(object)), instance)
            .Compile();
    }

    private static Action<object, object?> CreateSetter(PropertyInfo property)
    {
        var instance = Expression.Parameter(typeof(object), "instance");
        var value = Expression.Parameter(typeof(object), "value");
        var typedInstance = Expression.Convert(instance, property.DeclaringType!);

        var assign = Expression.Assign(
            Expression.Property(typedInstance, property),
            Expression.Convert(value, property.PropertyType));

        return Expression
            .Lambda<Action<object, object?>>(assign, instance, value)
            .Compile();
    }
}
