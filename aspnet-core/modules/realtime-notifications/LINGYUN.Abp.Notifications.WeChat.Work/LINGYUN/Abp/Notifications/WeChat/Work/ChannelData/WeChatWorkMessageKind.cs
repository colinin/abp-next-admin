namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 企业微信消息载体类型
/// </summary>
/// <remarks>
/// 显式声明渠道数据的形态, 替代旧实现中"按字段逐个探测"的隐式优先级,
/// 生产者也因此可以明确表达自己的意图。
/// </remarks>
public enum WeChatWorkMessageKind
{
    /// <summary>
    /// 未指定, 由渠道按内容类型推导
    /// </summary>
    None = 0,
    /// <summary>
    /// 小程序通知消息
    /// </summary>
    MiniProgramNotice = 1,
    /// <summary>
    /// 图文消息(mpnews)
    /// </summary>
    MpNews = 2,
    /// <summary>
    /// 模板卡片消息
    /// </summary>
    TemplateCard = 3,
    /// <summary>
    /// 文本卡片消息
    /// </summary>
    TextCard = 4,
    /// <summary>
    /// 视频消息
    /// </summary>
    Video = 5,
    /// <summary>
    /// 语音消息
    /// </summary>
    Voice = 6,
    /// <summary>
    /// 文件消息
    /// </summary>
    File = 7,
    /// <summary>
    /// 文本消息
    /// </summary>
    Text = 8,
    /// <summary>
    /// markdown消息
    /// </summary>
    Markdown = 9
}
