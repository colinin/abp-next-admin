namespace LINGYUN.Abp.WeChat
{
    public class AbpWeChatGlobalConsts
    {
        /// <summary>
        /// 微信授权名称
        /// </summary>
        public static string AuthenticationScheme { get; set; }= "WeChat";
        /// <summary>
        /// 微信授权Token参数名称
        /// </summary>
        public static string TokenName  { get; set; }= "code";
        /// <summary>
        /// 微信授权显示名称
        /// </summary>
        public static string DisplayName { get; set; } = "WeChat";

        public static string HttpClient { get; set; } = "Abp.WeChat";
    }
}
