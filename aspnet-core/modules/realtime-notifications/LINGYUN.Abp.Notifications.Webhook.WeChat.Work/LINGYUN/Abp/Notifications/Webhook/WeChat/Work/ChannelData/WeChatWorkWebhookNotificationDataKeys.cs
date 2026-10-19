namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 企业微信 Webhook 通知数据键
/// </summary>
/// <remarks>
/// <para>
/// 前缀 <c>wx-work-hook:</c> 归属 Webhook 渠道自身。
/// 企业微信**应用消息**渠道使用 <c>wx-work:</c> 前缀 —— 两者是不同传输方式,
/// 键空间必须分开, 否则同一个通知被两个渠道同时订阅时,
/// 载荷会被两边都解析并各发一条。
/// </para>
/// <para>
/// 读取过程大小写不敏感(精确匹配优先)。
/// </para>
/// </remarks>
public static class WeChatWorkWebhookNotificationDataKeys
{
    /// <summary>
    /// 本渠道的数据键前缀
    /// </summary>
    public const string Prefix = "wx-work-hook:";

    /// <summary>
    /// 文本消息
    /// </summary>
    public const string Text = Prefix + "text";
    /// <summary>
    /// markdown 消息
    /// </summary>
    public const string Markdown = Prefix + "markdown";
    /// <summary>
    /// markdown_v2 消息
    /// </summary>
    public const string MarkdownV2 = Prefix + "markdown_v2";
    /// <summary>
    /// 图片消息(base64 + md5)
    /// </summary>
    public const string Image = Prefix + "image";
    /// <summary>
    /// 图文消息
    /// </summary>
    public const string News = Prefix + "news";
    /// <summary>
    /// 文件消息
    /// </summary>
    public const string File = Prefix + "file";
    /// <summary>
    /// 语音消息
    /// </summary>
    public const string Voice = Prefix + "voice";
    /// <summary>
    /// 模板卡片消息
    /// </summary>
    public const string TemplateCard = Prefix + "template_card";
}
