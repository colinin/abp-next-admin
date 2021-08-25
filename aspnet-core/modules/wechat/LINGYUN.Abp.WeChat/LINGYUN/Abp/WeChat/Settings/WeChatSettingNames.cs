namespace LINGYUN.Abp.WeChat.Settings
{
    public static class WeChatSettingNames
    {
        public const string Prefix = "Abp.WeChat";
        /// <summary>
        /// 启用快捷登录
        /// 通过微信code登录时，如果没有注册用户信息且此配置启用时，直接创建新用户并关联openid
        /// </summary>
        public const string EnabledQuickLogin = Prefix + ".EnabledQuickLogin";
    }
}
