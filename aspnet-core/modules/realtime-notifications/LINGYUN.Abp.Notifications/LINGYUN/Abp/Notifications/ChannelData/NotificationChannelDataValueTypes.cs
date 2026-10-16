using System;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 渠道数据的值类型判定
/// </summary>
/// <remarks>
/// 用于区分"可直接存入属性包"的简单类型与"需要序列化"的复杂类型。
/// </remarks>
public static class NotificationChannelDataValueTypes
{
    /// <summary>
    /// 判断是否为可直接存储的简单类型
    /// </summary>
    public static bool IsPrimitive(Type type)
    {
        return type.IsPrimitive
            || type.IsEnum
            || type == typeof(string)
            || type == typeof(decimal)
            || type == typeof(DateTime)
            || type == typeof(DateTimeOffset)
            || type == typeof(TimeSpan)
            || type == typeof(Guid);
    }
}
