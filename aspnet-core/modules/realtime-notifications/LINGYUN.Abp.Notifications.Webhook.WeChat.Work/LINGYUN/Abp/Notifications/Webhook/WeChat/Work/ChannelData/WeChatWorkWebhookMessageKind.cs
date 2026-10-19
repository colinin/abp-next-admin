namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 企业微信 Webhook 消息载体类型
/// </summary>
/// <remarks>
/// 取值与框架 <c>WeChatWorkWebhookMessage.MsgType</c> 一致。
/// </remarks>
public enum WeChatWorkWebhookMessageKind
{
    /// <summary>
    /// 未指定
    /// </summary>
    None = 0,
    /// <summary>
    /// 文本
    /// </summary>
    Text = 1,
    /// <summary>
    /// markdown
    /// </summary>
    Markdown = 2,
    /// <summary>
    /// markdown_v2
    /// </summary>
    MarkdownV2 = 3,
    /// <summary>
    /// 图片
    /// </summary>
    Image = 4,
    /// <summary>
    /// 图文
    /// </summary>
    News = 5,
    /// <summary>
    /// 文件
    /// </summary>
    File = 6,
    /// <summary>
    /// 语音
    /// </summary>
    Voice = 7,
    /// <summary>
    /// 模板卡片
    /// </summary>
    TemplateCard = 8
}
