namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;
public class AbpNotificationsWebhookWeChatWorkOptions
{
    /// <summary>
    /// 发送Markdown类型通知时是否使用MarkdownV2格式通知, 默认: true
    /// </summary>
    /// <remarks>
    /// 详见: https://developer.work.weixin.qq.com/document/path/99110#markdown-v2%E7%B1%BB%E5%9E%8B
    /// </remarks>
    public bool UseMarkdownV2 {  get; set; }
    public AbpNotificationsWebhookWeChatWorkOptions()
    {
        UseMarkdownV2 = true;
    }
}
