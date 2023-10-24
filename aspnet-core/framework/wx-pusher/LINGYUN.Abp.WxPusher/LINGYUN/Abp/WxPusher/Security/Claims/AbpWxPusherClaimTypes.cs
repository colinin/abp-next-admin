namespace LINGYUN.Abp.WxPusher.Security.Claims;

public static class AbpWxPusherClaimTypes
{
    /// <summary>
    /// 用户的唯一标识
    /// </summary>
    public static string Uid { get; set; } = "wxp-uid";
    /// <summary>
    /// 用户订阅topic
    /// </summary>
    public static string Topic { get; set; } = "wxp-topic";
}
