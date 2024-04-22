namespace LINGYUN.Abp.WeChat.Work.Settings
{
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

            public static string CorpId = Prefix + ".CorpId";
        }
    }
}
