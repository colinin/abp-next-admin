using System;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 企业微信通知数据键
/// </summary>
/// <remarks>
/// 键名与历史实现保持一致(<c>wx-work:</c> 前缀), 保证已上线的生产者不受影响。
/// </remarks>
public static class WeChatWorkNotificationDataKeys
{
    public const string Prefix = "wx-work:";

    /// <summary>
    /// 指定接收消息的成员
    /// </summary>
    public const string ToUser = Prefix + "to_user";
    /// <summary>
    /// 指定接收消息的部门
    /// </summary>
    public const string ToParty = Prefix + "to_party";
    /// <summary>
    /// 指定接收消息的标签
    /// </summary>
    public const string ToTag = Prefix + "to_tag";
    /// <summary>
    /// 应用标识
    /// </summary>
    public const string AgentId = Prefix + "agent_id";
    /// <summary>
    /// 是否是保密消息
    /// </summary>
    public const string Safe = Prefix + "safe";
    /// <summary>
    /// 是否开启id转译
    /// </summary>
    public const string EnableIdTrans = Prefix + "enable_id_trans";
    /// <summary>
    /// 是否开启重复消息检查
    /// </summary>
    public const string EnableDuplicateCheck = Prefix + "enable_duplicate_check";
    /// <summary>
    /// 重复消息检查的时间间隔
    /// </summary>
    public const string DuplicateCheckInterval = Prefix + "duplicate_check_interval";

    public const string MiniProgramMessage = Prefix + "mini_program_message";
    public const string MpNewMessage = Prefix + "mp_new_message";
    public const string TemplateCardMessage = Prefix + "template_card_message";
    public const string TextCardMessage = Prefix + "text_card_message";
    public const string VideoMessage = Prefix + "video_message";
    public const string VoiceMessage = Prefix + "voice_message";
    public const string FileMessage = Prefix + "file_message";
}
