namespace LINGYUN.Abp.PushPlus.Channel;
/// <summary>
/// 发送渠道（channel）枚举
/// </summary>
public enum PushPlusChannelType
{
    /// <summary>
    /// 微信公众号
    /// </summary>
    WeChat = 0,
    /// <summary>
    /// 第三方webhook
    /// </summary>
    Webhook = 1,
    /// <summary>
    /// 企业微信应用
    /// </summary>
    WeWork = 2,
    /// <summary>
    /// 邮箱
    /// </summary>
    Email = 3,
    /// <summary>
    /// 短信
    /// </summary>
    Sms = 4
}
