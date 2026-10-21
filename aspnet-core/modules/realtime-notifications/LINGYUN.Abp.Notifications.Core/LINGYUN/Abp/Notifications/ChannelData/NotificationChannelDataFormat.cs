namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知渠道数据(载荷)属性的存储格式
/// </summary>
public enum NotificationChannelDataFormat
{
    /// <summary>
    /// 自动识别(默认)
    /// </summary>
    /// <remarks>
    /// 简单类型直接转换, 复杂类型序列化为 JSON 字符串,
    /// 读取时同样兼容"对象实例"与"JSON 字符串"两种形态。
    /// </remarks>
    Auto = 0,
    /// <summary>
    /// 始终以 JSON 字符串存储
    /// </summary>
    Json = 1
}
