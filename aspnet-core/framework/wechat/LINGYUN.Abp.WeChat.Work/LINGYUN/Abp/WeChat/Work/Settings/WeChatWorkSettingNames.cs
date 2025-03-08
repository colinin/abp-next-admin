namespace LINGYUN.Abp.WeChat.Work.Settings;

public static class WeChatWorkSettingNames
{
    public const string Prefix = "Abp.WeChat.Work";

    /// <summary>
    /// 启用快捷登录
    /// </summary>
    public const string EnabledQuickLogin = Prefix + ".EnabledQuickLogin";

    public static class Connection
    {
        public const string Prefix = WeChatWorkSettingNames.Prefix + ".Connection";
        /// <summary>
        /// 企业Id
        /// </summary>
        public static string CorpId = Prefix + ".CorpId";
        /// <summary>
        /// 应用Id
        /// </summary>
        public static string AgentId = Prefix + ".AgentId";
        /// <summary>
        /// 应用密钥
        /// </summary>
        public static string Secret = Prefix + ".Secret";
        /// <summary>
        /// Token
        /// </summary>
        public static string Token = Prefix + ".Token";
        /// <summary>
        /// EncodingAESKey
        /// </summary>
        public static string EncodingAESKey = Prefix + ".EncodingAESKey";
    }
}
