namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 渠道数据(载荷)存在性判定模式
/// </summary>
public enum NotificationChannelDataPresenceMode
{
    /// <summary>
    /// 载荷始终被视为存在
    /// </summary>
    /// <remarks>
    /// 适用于不依赖通知数据的载荷(由渠道在运行时构造)。
    /// </remarks>
    None = 0,

    /// <summary>
    /// 参与匹配的属性全部存在时, 载荷被视为存在(默认)
    /// </summary>
    /// <remarks>
    /// 适用于"某个键就是载荷标识"的场景, 例如企业微信的各消息类型。
    /// </remarks>
    DeclaredProperties = 1,

    /// <summary>
    /// 参与匹配的属性存在任意一个时, 载荷被视为存在
    /// </summary>
    /// <remarks>
    /// 适用于"多个可选键中任意一个即可"的场景,
    /// 例如 PushPlus 的 webhook 与 callback 二者其一。
    /// </remarks>
    AnyDeclaredProperty = 2
}
