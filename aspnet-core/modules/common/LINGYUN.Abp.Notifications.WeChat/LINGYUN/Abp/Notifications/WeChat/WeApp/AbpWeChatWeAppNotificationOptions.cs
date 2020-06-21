namespace LINGYUN.Abp.Notifications.WeChat.WeApp
{
    public class AbpWeChatWeAppNotificationOptions
    {
        /// <summary>
        /// 默认消息头部标记
        /// </summary>
        public string DefaultMsgPrefix { get; set; } = "[wx]";
        /// <summary>
        /// 默认小程序模板
        /// </summary>
        public string DefaultTemplateId { get; set; }
        /// <summary>
        /// 默认跳转小程序类型
        /// </summary>
        public string DefaultWeAppState { get; set; } = "developer";
        /// <summary>
        /// 默认小程序语言
        /// </summary>
        public string DefaultWeAppLanguage { get; set; } = "zh_CN";
    }
}
