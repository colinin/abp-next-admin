namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知渠道数据(载荷)基类
/// </summary>
/// <remarks>
/// <para>
/// 渠道数据是"发布渠道能理解的强类型数据结构", 生产者用它表达渠道专属语义,
/// 渠道实现用它解析出具体的消息载体。
/// </para>
/// <para>
/// 派生类需要做两件事:
/// <list type="number">
/// <item>
/// 用 <see cref="NotificationChannelDataAttribute"/> 声明渠道、匹配顺序与
/// <see cref="NotificationChannelDataAttribute.Presence"/> 存在性判定模式;
/// </item>
/// <item>
/// 用 <see cref="NotificationChannelPropertyAttribute"/> 标注参与读写的属性。
/// </item>
/// </list>
/// </para>
/// </remarks>
public abstract class NotificationChannelData
{
}
