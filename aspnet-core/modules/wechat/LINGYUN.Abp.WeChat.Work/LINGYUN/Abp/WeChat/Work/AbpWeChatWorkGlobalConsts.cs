namespace LINGYUN.Abp.WeChat.Work
{
    public class AbpWeChatWorkGlobalConsts
    {
        /// <summary>
        ///  企业微信对应的Provider名称
        /// </summary>
        public static string ProviderName { get; set; } = "WeChat.Work";
        /// <summary>
        /// 企业微信授权类型
        /// </summary>
        public static string GrantType { get; set; } = "wx-work";
        /// <summary>
        /// 企业微信授权名称
        /// </summary>
        public static string AuthenticationScheme { get; set; }= "WeCom";
        /// <summary>
        /// 企业微信个人信息标识
        /// </summary>
        public static string ProfileKey { get; set; } = "wecom.profile";
        /// <summary>
        /// 企业微信授权应用标识参数
        /// </summary>
        public static string AgentId { get; set; } = "agent_id";
        /// <summary>
        /// 企业微信授权Code参数
        /// </summary>
        public static string Code  { get; set; }= "code";
        /// <summary>
        /// 企业微信授权显示名称
        /// </summary>
        public static string DisplayName { get; set; } = "企业微信";
        /// <summary>
        ///企业微信授权方法名称
        /// </summary>
        public static string AuthenticationMethod { get; set; } = "wecom";

        internal static string ApiClient { get; set; } = "Abp.WeChat.Work";
        internal static string OAuthClient { get; set; } = "Abp.WeChat.Work.OAuth";
        internal static string LoginClient { get; set; } = "Abp.WeChat.Work.Login";
    }
}
