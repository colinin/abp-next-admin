using System;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 渠道数据属性元数据
/// </summary>
/// <remarks>
/// 由 <see cref="NotificationChannelPropertyAttribute"/> 推导而来,
/// 描述"写入/读取哪个键、类型是什么、是否参与存在性判定"。
/// </remarks>
public sealed class NotificationChannelDataProperty
{
    /// <summary>
    /// 数据键名
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// 属性声明类型
    /// </summary>
    public Type PropertyType { get; }

    /// <summary>
    /// 存储格式
    /// </summary>
    public NotificationChannelDataFormat Format { get; }

    /// <summary>
    /// 是否在存在性判定时忽略本属性
    /// </summary>
    public bool IgnoreWhenMatching { get; }

    /// <summary>
    /// 读取委托
    /// </summary>
    public Func<object, object?> GetValue { get; }

    /// <summary>
    /// 写入委托
    /// </summary>
    public Action<object, object?> SetValue { get; }

    public NotificationChannelDataProperty(
        string key,
        Type propertyType,
        NotificationChannelDataFormat format,
        bool ignoreWhenMatching,
        Func<object, object?> getValue,
        Action<object, object?> setValue)
    {
        Key = key;
        PropertyType = propertyType;
        Format = format;
        IgnoreWhenMatching = ignoreWhenMatching;
        GetValue = getValue;
        SetValue = setValue;
    }
}
