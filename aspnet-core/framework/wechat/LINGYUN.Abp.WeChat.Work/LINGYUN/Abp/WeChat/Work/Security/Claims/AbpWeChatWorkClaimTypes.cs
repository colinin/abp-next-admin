namespace LINGYUN.Abp.WeChat.Work.Security.Claims;
public static class AbpWeChatWorkClaimTypes
{
    /// <summary>
    /// 唯一标识
    /// </summary>
    public static string UserId { get; set; } = "userid";
    /// <summary>
    /// 二维码名片
    /// </summary>
    public static string QrCode { get; set; } = "qr_code";
    /// <summary>
    /// 企业邮箱
    /// </summary>
    public static string BizMail { get; set; } = "biz_mail";
    /// <summary>
    /// 地址
    /// </summary>
    public static string Address { get; set; } = "address";
}
