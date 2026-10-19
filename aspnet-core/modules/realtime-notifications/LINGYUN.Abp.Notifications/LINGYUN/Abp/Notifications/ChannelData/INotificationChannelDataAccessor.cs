using System;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知渠道数据(载荷)存取器
/// </summary>
/// <remarks>
/// <para>
/// 通知数据在持久化、失败重试、跨进程投递等链路中必然经历 JSON 往返,
/// 往返之后 <c>ExtraPropertyDictionary</c> 中的值是 JSON 节点而不是原始 CLR 实例:
/// <c>byte 1</c> 会变成 <c>long 1</c> / <c>JsonElement(Number)</c>,
/// 原有的 <c>x as byte?</c> 将永远返回 null。
/// </para>
/// <para>
/// 本服务是渠道数据"弱类型属性包 ↔ 强类型值"转换的唯一实现,
/// 由 <see cref="INotificationChannelDataSerializer"/> 与
/// <see cref="INotificationChannelDataResolver"/> 共用,
/// 并保证与所用 JSON 实现无关。
/// </para>
/// </remarks>
public interface INotificationChannelDataAccessor
{
    /// <summary>
    /// 获取载荷的数据类型键
    /// </summary>
    /// <returns>键名, 类型无全名时返回 null</returns>
    string? GetDataTypeKey(Type payloadType);

    /// <summary>
    /// 是否存在键(大小写不敏感)
    /// </summary>
    bool IsPresent(NotificationData notificationData, string key);

    /// <summary>
    /// 读取原始值(大小写不敏感)
    /// </summary>
    /// <param name="notificationData">通知数据</param>
    /// <param name="key">键名</param>
    /// <param name="value">原始值</param>
    /// <param name="matchedKey">实际命中的键名(可能与入参大小写不同)</param>
    bool TryGetRawValue(NotificationData notificationData, string key, out object? value, out string? matchedKey);

    /// <summary>
    /// 读取并转换为目标类型
    /// </summary>
    /// <returns>转换后的值; 键不存在或无法转换时返回 null</returns>
    object? Get(NotificationData notificationData, string key, Type targetType);

    /// <summary>
    /// 类型化读取(未设置时返回 null, 以便区分"未设置"与"显式 false/0")
    /// </summary>
    TValue? GetValue<TValue>(NotificationData notificationData, string key)
        where TValue : struct;

    /// <summary>
    /// 类型化读取, 未设置时返回默认值
    /// </summary>
    TValue GetValue<TValue>(NotificationData notificationData, string key, TValue defaultValue)
        where TValue : struct;

    /// <summary>
    /// 按顺序读取第一个非空字符串值(大小写不敏感)
    /// </summary>
    string? GetOrNull(NotificationData notificationData, params string[] keys);

    /// <summary>
    /// 写入值, 复杂类型以 JSON 形式存储
    /// </summary>
    void Set(NotificationData notificationData, string key, Type valueType, object? value, NotificationChannelDataFormat format);
}
