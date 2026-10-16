using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 企业微信 Webhook 通知渠道名
/// </summary>
/// <remarks>
/// <para>
/// Webhook 的**发布**仍走 <c>WebhookNotificationPublishProvider</c>(渠道名 <c>Webhook</c>),
/// 本常量只作为**载荷注册表的键**使用:
/// 企业微信应用消息(渠道名 <c>WeChat.Work</c>)与 Webhook 是两种传输方式、
/// 两套载体模型, 因此必须使用不同的注册表键, 避免载荷被对方误解析。
/// </para>
/// <para>
/// 生产者侧: 通知定义中仍声明 <c>ProviderName</c> 为 <c>"Webhook"</c> 才会被发布,
/// 载荷用本渠道的数据键(前缀 <c>wx-work-hook:</c>)写入。
/// </para>
/// </remarks>
public static class WeChatWorkWebhookNotificationProvider
{
    /// <summary>
    /// 本渠道的载荷注册表键
    /// </summary>
    public const string ProviderName = "WeChat.Work.Webhook";
}
