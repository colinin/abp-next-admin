namespace LINGYUN.Abp.PushPlus.Setting;
/// <summary>
/// 发送限制；
/// 0-无限制，
/// 1-禁止所有渠道发送，
/// 2-限制微信渠道，
/// 3-限制邮件渠道
/// </summary>
public enum PushPlusChannelSendLimit
{
    /// <summary>
    /// 无限制
    /// </summary>
    None = 0,
    /// <summary>
    /// 禁止所有渠道发送
    /// </summary>
    ForbidAllChannel = 1,
    /// <summary>
    /// 限制微信渠道
    /// </summary>
    ForbidWeChatChannel = 2,
    /// <summary>
    /// 限制邮件渠道
    /// </summary>
    ForbidEmailChannel = 3,
}
