namespace LINGYUN.Abp.WeChat.Work.Security.Claims;
public static class AbpWeChatWorkClaimTypes
{
    /// <summary>
    /// 唯一标识
    /// </summary>
    public static string UserId { get; set; } = "wecom_userid";
    /// <summary>
    /// 二维码名片
    /// </summary>
    public static string QrCode { get; set; } = "wecom_qr_code";
    /// <summary>
    /// 企业邮箱
    /// </summary>
    public static string BizMail { get; set; } = "wecom_biz_mail";
    /// <summary>
    /// 地址
    /// </summary>
    public static string Address { get; set; } = "wecom_address";
}
