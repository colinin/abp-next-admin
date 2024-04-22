using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages;
/// <summary>
/// 企业微信消息
/// </summary>
public abstract class WeChatWorkMessage : WeChatWorkRequest
{
    /// <summary>
    /// 指定接收消息的成员，成员ID列表（多个接收者用‘|’分隔，最多支持1000个）。
    /// 特殊情况：指定为"@all"，则向该企业应用的全部成员发送
    /// </summary>
    [JsonProperty("touser")]
    [JsonPropertyName("touser")]
    public virtual string ToUser { get; set; }
    /// <summary>
    /// 指定接收消息的部门，部门ID列表，多个接收者用‘|’分隔，最多支持100个。
    /// 当touser为"@all"时忽略本参数
    /// </summary>
    [JsonProperty("toparty")]
    [JsonPropertyName("toparty")]
    public virtual string ToParty { get; set; }
    /// <summary>
    /// 指定接收消息的标签，标签ID列表，多个接收者用‘|’分隔，最多支持100个。
    /// 当touser为"@all"时忽略本参数
    /// </summary>
    [JsonProperty("totag")]
    [JsonPropertyName("totag")]
    public virtual string ToTag { get; set; }
    /// <summary>
    /// 消息类型
    /// </summary>
    [NotNull]
    [JsonProperty("msgtype")]
    [JsonPropertyName("msgtype")]
    public virtual string MsgType { get; protected set; }
    /// <summary>
    /// 企业应用的id，整型。
    /// 企业内部开发，可在应用的设置页面查看；
    /// 第三方服务商，可通过接口 获取企业授权信息 获取该参数值
    /// </summary>
    [NotNull]
    [JsonProperty("agentid")]
    [JsonPropertyName("agentid")]
    public virtual string AgentId { get; protected set; }
    protected WeChatWorkMessage(
        string agentId,
        string msgType,
        string toUser = "",
        string toParty = "",
        string toTag = "")
    {
        AgentId = agentId;
        MsgType = msgType;
        ToUser = toUser;
        ToParty = toParty;
        ToTag = toTag;
    }
}
