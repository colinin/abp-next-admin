using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信小程序通知消息
/// </summary>
public class WeChatWorkMiniProgramMessage : WeChatWorkMessage
{
    /// <summary>
    /// 表示是否开启id转译，0表示否，1表示是，默认0。
    /// 仅第三方应用需要用到
    /// 企业自建应用可以忽略。
    /// </summary>
    [JsonProperty("enable_id_trans")]
    [JsonPropertyName("enable_id_trans")]
    public byte EnableIdTrans { get; set; }
    /// <summary>
    /// 表示是否开启重复消息检查，0表示否，1表示是，默认0
    /// </summary>
    [JsonProperty("enable_duplicate_check")]
    [JsonPropertyName("enable_duplicate_check")]
    public byte EnableDuplicateCheck { get; set; }
    /// <summary>
    /// 表示是否重复消息检查的时间间隔，默认1800s，最大不超过4小时
    /// </summary>
    [JsonProperty("duplicate_check_interval")]
    [JsonPropertyName("duplicate_check_interval")]
    public int DuplicateCheckInterval { get; set; } = 1800;
    /// <summary>
    /// 消息内容
    /// </summary>
    [NotNull]
    [JsonProperty("miniprogram_notice")]
    [JsonPropertyName("miniprogram_notice")]
    public MiniProgramMessage MiniProgram { get; set; }
    public WeChatWorkMiniProgramMessage(
        MiniProgramMessage miniProgram,
        string toUser = "",
        string toParty = "",
        string toTag = "",
        byte enableIdTrans = 0,
        byte enableDuplicateCheck = 0,
        int duplicateCheckInterval = 1800)
        : base("", "miniprogram_notice", toUser, toParty, toTag)
    {
        MiniProgram = miniProgram;
        EnableIdTrans = enableIdTrans;
        EnableDuplicateCheck = enableDuplicateCheck;
        DuplicateCheckInterval = duplicateCheckInterval;
    }
}
