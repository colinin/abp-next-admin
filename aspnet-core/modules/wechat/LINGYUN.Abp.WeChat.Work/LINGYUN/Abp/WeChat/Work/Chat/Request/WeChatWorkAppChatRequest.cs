using JetBrains.Annotations;

namespace LINGYUN.Abp.WeChat.Work.Chat.Request;
public abstract class WeChatWorkAppChatRequest : WeChatWorkRequest
{
    /// <summary>
    /// 应用标识
    /// </summary>
    [NotNull]
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual string AgentId { get; set; }
    protected WeChatWorkAppChatRequest(string agentId)
    {
        AgentId = agentId;
    }
}
