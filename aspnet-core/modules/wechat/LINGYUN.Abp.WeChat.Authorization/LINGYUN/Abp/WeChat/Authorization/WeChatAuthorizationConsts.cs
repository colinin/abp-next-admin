namespace LINGYUN.Abp.WeChat.Authorization
{
    public class WeChatAuthorizationConsts
    {
        /// <summary>
        /// 微信提供者标识
        /// </summary>
        public static string ProviderKey { get; set; } = "WeChat";
        /// <summary>
        /// 微信Code参数名称
        /// </summary>
        public static string WeCahtCodeKey { get; set; } = "wx-code";
        /// <summary>
        /// 微信OpenId参数名称
        /// </summary>
        public static string WeCahtOpenIdKey { get; set; } = "wx-open-id";
        /// <summary>
        /// 微信SessionKey参数名称
        /// </summary>
        public static string WeCahtSessionKey { get; set; } = "wx-session-key";
    }
}
