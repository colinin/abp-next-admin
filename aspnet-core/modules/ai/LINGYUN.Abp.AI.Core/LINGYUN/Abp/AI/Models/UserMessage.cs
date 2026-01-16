using System.Linq;

namespace LINGYUN.Abp.AI.Models;
public class UserMessage
{
    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; }
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
    /// <summary>
    /// 媒体附件
    /// </summary>
    /// <remarks>
    /// 暂未实现
    /// </remarks>
    public MediaMessage[]? Medias { get; private set; }
    public UserMessage(
        string workspace,
        string content)
    {
        Workspace = workspace;
        Content = content;
    }

    public UserMessage WithMessageId(string id)
    {
        Id = id;
        return this;
    }

    public UserMessage WithConversationId(string conversationId)
    {
        ConversationId = conversationId;
        return this;
    }

    public UserMessage WithMedia(MediaMessage media)
    {
        Medias ??= [];
        Medias = Medias.Union([media]).ToArray();

        return this;
    }

    public UserMessage WithReply(string replyMessage)
    {
        ReplyMessage = replyMessage;
        return this;
    }
}
