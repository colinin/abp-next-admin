using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 渠道数据属性元数据缓存
/// </summary>
/// <remarks>
/// <para>
/// 纯反射元数据缓存(无 JSON、无日志、无配置依赖), 因此保持静态以避免
/// 在序列化器、解析器、存在性提供者之间重复注入。
/// </para>
/// </remarks>
public static class NotificationChannelDataPropertyCache
{
    /// <summary>
    /// 属性元数据缓存
    /// </summary>
    /// <remarks>
    /// 值用 <see cref="Lazy{T}"/> 包装: <c>GetOrAdd</c> 的工厂在并发下可能被调用多次,
    /// 但只有一个实例会被写入并被后续读取, 其余实例被丢弃, 因此构建仍只发生一次。
    /// </remarks>
    private static readonly ConcurrentDictionary<Type, Lazy<NotificationChannelDataProperty[]>> PropertiesCache
        = new();

    /// <summary>
    /// 获取载荷类型的属性元数据
    /// </summary>
    public static NotificationChannelDataProperty[] GetProperties(Type channelDataType)
    {
        return PropertiesCache
            .GetOrAdd(
                channelDataType,
                type => new Lazy<NotificationChannelDataProperty[]>(
                    () => CreateProperties(type),
                    LazyThreadSafetyMode.ExecutionAndPublication))
            .Value;
    }

    /// <summary>
    /// 构建属性元数据
    /// </summary>
    private static NotificationChannelDataProperty[] CreateProperties(Type channelDataType)
    {
        return channelDataType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(TryCreateProperty)
            .Where(x => x != null)
            .Select(x => x!)
            .ToArray();
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
