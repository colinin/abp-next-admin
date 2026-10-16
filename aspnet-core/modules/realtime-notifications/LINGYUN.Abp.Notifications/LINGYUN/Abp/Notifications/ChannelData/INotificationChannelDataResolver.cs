using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知渠道数据(载荷)解析器
/// </summary>
public interface INotificationChannelDataResolver
{
    /// <summary>
    /// 按匹配顺序解析指定渠道的载荷
    /// </summary>
    /// <remarks>
    /// 仅返回"已写入且可解析"的载荷; 传入非法数据时会被跳过并记录日志,
    /// 渠道如果需要区分该情况, 应配合 <see cref="IsDeclared"/> 使用。
    /// </remarks>
    IReadOnlyList<NotificationChannelData> Resolve(string providerName, NotificationData notificationData);

    /// <summary>
    /// 解析指定类型的渠道数据
    /// </summary>
    /// <returns>载荷实例, 未写入或无法解析时返回 null</returns>
    TChannelData? Resolve<TChannelData>(NotificationData notificationData)
        where TChannelData : NotificationChannelData;

    /// <summary>
    /// 解析指定类型的渠道数据
    /// </summary>
    NotificationChannelData? Resolve(Type payloadType, NotificationData notificationData);

    /// <summary>
    /// 判断生产者是否声明了指定类型的渠道数据
    /// </summary>
    /// <remarks>
    /// 与解析结果配合使用, 可以区分
    /// "生产者未写入载荷" 与 "载荷已写入但无法解析(数据非法)"。
    /// 后者属于生产者错误, 渠道应当明确报错而不是静默回退。
    /// </remarks>
    bool IsDeclared(NotificationData notificationData, Type payloadType);

    /// <summary>
    /// 获取指定渠道的载荷类型集合
    /// </summary>
    IReadOnlyList<Type> GetPayloadTypes(string providerName);

    /// <summary>
    /// 获取载荷的标识键
    /// </summary>
    /// <remarks>
    /// 返回该载荷第一个"参与存在性判定"的属性键(数据中的实际键名),
    /// 用于诊断信息(例如"哪个键解析失败")。未声明可匹配属性时返回 null。
    /// </remarks>
    string? GetKey(NotificationData notificationData, Type payloadType);

    /// <summary>
    /// 弱类型读取, 并转换为目标类型
    /// </summary>
    object? Get(NotificationData notificationData, string key, Type targetType);

    /// <summary>
    /// 类型化读取渠道数据键
    /// </summary>
    /// <remarks>
    /// 用于读取"散落的标量选项"(如企业微信的 safe、enable_id_trans),
    /// 这类值不需要独立载荷类型, 但同样必须经过弱类型转换。
    /// </remarks>
    TValue? GetValue<TValue>(NotificationData notificationData, string key)
        where TValue : struct;

    /// <summary>
    /// 类型化读取渠道数据键, 未设置时返回默认值
    /// </summary>
    TValue GetValue<TValue>(NotificationData notificationData, string key, TValue defaultValue)
        where TValue : struct;

    /// <summary>
    /// 大小写不敏感地读取渠道数据键
    /// </summary>
    /// <remarks>
    /// 用于兼容历史渠道实现中大小写敏感的遗留键(如 <c>TemplateId</c>),
    /// 避免生产者因为大小写差异而静默失效。
    /// </remarks>
    string? GetOrNull(NotificationData notificationData, params string[] keys);
}
