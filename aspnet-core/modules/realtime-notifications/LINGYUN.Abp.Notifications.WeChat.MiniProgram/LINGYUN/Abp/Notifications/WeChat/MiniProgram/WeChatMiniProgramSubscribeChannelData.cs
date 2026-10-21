using LINGYUN.Abp.Notifications;

namespace LINGYUN.Abp.Notifications.WeChat.MiniProgram;

/// <summary>
/// 微信小程序渠道数据(载荷)
/// </summary>
/// <remarks>
/// <para>
/// 小程序订阅消息的载荷是"广播式"的: 除模板标识与跳转参数外,
/// 模板数据(<c>data</c>)直接来自通知数据的属性包, 因此这里只声明
/// 需要类型化读取的标量字段。
/// </para>
/// <para>
/// 键名带本渠道前缀(<see cref="WeChatMiniProgramNotificationDataKeys.Prefix"/>),
/// 读取大小写不敏感; 存在性由模板标识键推导, 因此把跳转参数声明为
/// "仅读取、不参与匹配" —— 生产者可能只写模板标识。
/// </para>
/// <para>
/// 历史遗留的无前缀键由渠道在解析时回退读取(见
/// <c>WeChatMiniProgramNotificationPublishProvider.ResolveChannelData</c>)。
/// </para>
/// </remarks>
[NotificationChannelData(WeChatMiniProgramNotificationPublishProvider.ProviderName, Priority = 100)]
public class WeChatMiniProgramSubscribeChannelData : NotificationChannelData
{
    /// <summary>
    /// 小程序模板标识(载荷标识键)
    /// </summary>
    [NotificationChannelProperty(WeChatMiniProgramNotificationDataKeys.TemplateId)]
    public virtual string? TemplateId { get; set; }

    /// <summary>
    /// 点击跳转的小程序页面
    /// </summary>
    [NotificationChannelProperty(WeChatMiniProgramNotificationDataKeys.RedirectPage, IgnoreWhenMatching = true)]
    public virtual string? RedirectPage { get; set; }

    /// <summary>
    /// 跳转小程序类型
    /// </summary>
    [NotificationChannelProperty(WeChatMiniProgramNotificationDataKeys.WeAppState, IgnoreWhenMatching = true)]
    public virtual string? WeAppState { get; set; }

    /// <summary>
    /// 小程序语言
    /// </summary>
    [NotificationChannelProperty(WeChatMiniProgramNotificationDataKeys.WeAppLanguage, IgnoreWhenMatching = true)]
    public virtual string? WeAppLanguage { get; set; }
}
