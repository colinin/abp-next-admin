namespace LINGYUN.Abp.AI.Models;
public abstract class UserMessage
{
    /// <summary>
    /// 工作区
    /// </summary>
    public string Workspace { get; }
    /// <summary>
    /// 消息Id
    /// </summary>
    /// <remarks>
    /// 在持久化设施处更新
    /// </remarks>
    public string? Id { get; private set; }
    /// <summary>
    /// 对话Id
    /// </summary>
    /// <remarks>
    /// 用于从客户端存储中持久化和检索聊天历史的唯一标识符,如果未指定则与AI对话时无上下文关联
    /// </remarks>
    public string? ConversationId { get; private set; }
    /// <summary>
    /// AI回复消息
    /// </summary>
    public string ReplyMessage { get; private set; }
    protected UserMessage(string workspace)
    {
        Workspace = workspace;
    }

    public virtual UserMessage WithMessageId(string id)
    {
        Id = id;
        return this;
    }

    public virtual UserMessage WithConversationId(string conversationId)
    {
        ConversationId = conversationId;
        return this;
    }

    public virtual UserMessage WithReply(string replyMessage)
    {
        ReplyMessage = replyMessage;
        return this;
    }

    public virtual string GetMessagePrompt()
    {
        return string.Empty;
    }
}
