using System;

namespace LINGYUN.Abp.Notifications.WeChat.MiniProgram;

/// <summary>
/// 微信小程序通知数据键
/// </summary>
/// <remarks>
/// <para>
/// 键名统一带本渠道前缀(<see cref="Prefix"/>), 与其它渠道保持一致,
/// 避免不同渠道的键在同一个通知数据属性包中相互冲突。
/// </para>
/// <para>
/// 读取过程大小写不敏感(精确匹配优先), 因此生产者使用任意大小写都能命中。
/// </para>
/// </remarks>
public static class WeChatMiniProgramNotificationDataKeys
{
    /// <summary>
    /// 本渠道的数据键前缀
    /// </summary>
    public const string Prefix = "wx-mp:";

    /// <summary>
    /// 小程序模板标识(载荷标识键)
    /// </summary>
    public const string TemplateId = Prefix + "template_id";

    /// <summary>
    /// 点击跳转的小程序页面
    /// </summary>
    public const string RedirectPage = Prefix + "redirect_page";

    /// <summary>
    /// 跳转小程序类型
    /// </summary>
    public const string WeAppState = Prefix + "weapp_state";

    /// <summary>
    /// 小程序语言
    /// </summary>
    public const string WeAppLanguage = Prefix + "weapp_language";

    /// <summary>
    /// 历史遗留的无前缀键
    /// </summary>
    /// <remarks>
    /// <para>
    /// 早期版本使用无前缀键, 已安装的生产者可能仍在写入, 因此保留**只读兼容**。
    /// </para>
    /// <para>
    /// <b>已弃用</b>: 新代码请使用带前缀的常量(<see cref="TemplateId"/> 等)。
    /// 唯一允许的读取点是渠道侧的历史回退
    /// (<c>WeChatMiniProgramNotificationPublishProvider.ApplyLegacyKeys</c>),
    /// 它命中遗留键时会记录告警以推动生产者迁移。
    /// </para>
    /// </remarks>
    [Obsolete("The legacy non-prefixed keys are deprecated. Use the prefixed constants instead, for example WeChatMiniProgramNotificationDataKeys.TemplateId. They are kept for read-only fallback to remain compatible with already deployed producers.")]
    public static class Legacy
    {
#pragma warning disable CS0618 // Type or member is obsolete: the nested class references its own members

        /// <summary>
        /// 历史: 小程序模板标识
        /// </summary>
        [Obsolete("The legacy non-prefixed key is deprecated. Use WeChatMiniProgramNotificationDataKeys.TemplateId instead.")]
        public const string TemplateId = "TemplateId";

        /// <summary>
        /// 历史: 点击跳转的小程序页面
        /// </summary>
        [Obsolete("The legacy non-prefixed key is deprecated. Use WeChatMiniProgramNotificationDataKeys.RedirectPage instead.")]
        public const string RedirectPage = "RedirectPage";

        /// <summary>
        /// 历史: 跳转小程序类型
        /// </summary>
        [Obsolete("The legacy non-prefixed key is deprecated. Use WeChatMiniProgramNotificationDataKeys.WeAppState instead.")]
        public const string WeAppState = "WeAppState";

        /// <summary>
        /// 历史: 小程序语言
        /// </summary>
        [Obsolete("The legacy non-prefixed key is deprecated. Use WeChatMiniProgramNotificationDataKeys.WeAppLanguage instead.")]
        public const string WeAppLanguage = "WeAppLanguage";

        /// <summary>
        /// 全部历史键
        /// </summary>
        public static readonly string[] All =
        {
            TemplateId,
            RedirectPage,
            WeAppState,
            WeAppLanguage
        };

#pragma warning restore CS0618
    }
}
