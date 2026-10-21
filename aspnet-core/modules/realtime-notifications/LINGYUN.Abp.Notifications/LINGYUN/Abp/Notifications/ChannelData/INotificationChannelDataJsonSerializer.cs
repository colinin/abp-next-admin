using System;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知渠道数据(载荷)的 JSON 序列化抽象
/// </summary>
public interface INotificationChannelDataJsonSerializer
{
    /// <summary>
    /// 序列化
    /// </summary>
    string Serialize(object value, Type valueType);

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="json">JSON 字符串</param>
    /// <param name="targetType">目标类型</param>
    /// <returns>实例, 类型不支持时返回 null</returns>
    object? Deserialize(string json, Type targetType);
}
