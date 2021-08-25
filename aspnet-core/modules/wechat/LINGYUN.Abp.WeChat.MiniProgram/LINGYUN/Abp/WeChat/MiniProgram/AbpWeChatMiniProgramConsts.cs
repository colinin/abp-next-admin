namespace LINGYUN.Abp.WeChat.MiniProgram
{
    public class AbpWeChatMiniProgramConsts
    {
        /// <summary>
        /// 微信小程序对应的Provider名称
        /// </summary>
        public static string ProviderName { get; set; } = "WeChat.MiniProgram";

        /// <summary>
        /// 微信小程序授权类型
        /// </summary>
        public static string GrantType { get; set; } = "wx-mp";

        /// <summary>
        /// 微信小程序授权方法名称
        /// </summary>
        public static string AuthenticationMethod { get; set; } = "wma";
        public static string HttpClient { get; set; } = "Abp.WeChat.MiniProgram";
    }
}
