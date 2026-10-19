namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 企业微信渠道数据(载荷)基类
/// </summary>
/// <remarks>
/// 每个载荷对应一种企业微信消息载体, 通过 <see cref="Kind"/> 显式声明形态,
/// 渠道侧使用工厂字典做 O(1) 分派, 不再依赖 if/else 探测顺序。
/// <para>
/// 存在性由 <see cref="NotificationChannelDataAttribute.Presence"/> 声明式推导:
/// 每个载荷都只有一个"标识键", 因此默认的"参与匹配的属性全部存在"即为正确语义,
/// 无需在载荷类型中实现判定方法。
/// </para>
/// </remarks>
public abstract class WeChatWorkChannelData : NotificationChannelData
{
    /// <summary>
    /// 消息载体类型
    /// </summary>
    public abstract WeChatWorkMessageKind Kind { get; }
}
