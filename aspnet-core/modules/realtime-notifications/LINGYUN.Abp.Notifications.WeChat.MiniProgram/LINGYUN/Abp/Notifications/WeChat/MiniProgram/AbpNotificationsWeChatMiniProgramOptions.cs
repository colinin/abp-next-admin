namespace LINGYUN.Abp.Notifications.WeChat.MiniProgram
{
    /// <summary>
    /// TODO: 后期改进,配置项集成到 <see cref="LINGYUN.Abp.WeChat.MiniProgram.AbpWeChatMiniProgramOptions"/>
    /// </summary>
    public class AbpNotificationsWeChatMiniProgramOptions
    {
        /// <summary>
        /// 默认消息头部标记
        /// </summary>
        public string DefaultMsgPrefix { get; set; } = "[wmp]";
        /// <summary>
        /// 默认小程序模板
        /// </summary>
        public string DefaultTemplateId { get; set; }
        /// <summary>
        /// 默认跳转小程序类型
        /// </summary>
        public string DefaultState { get; set; } = "developer";
        /// <summary>
        /// 默认小程序语言
        /// </summary>
        public string DefaultLanguage { get; set; } = "zh_CN";
    }
}
