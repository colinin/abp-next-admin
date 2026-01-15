using System.Linq;

namespace LINGYUN.Abp.AI.Models;
public class UserMessage
{
    public string Id { get; }
    public string ChatId { get; }
    public string Content { get; }
    public string Workspace { get; }
    public string ReplyMessage { get; private set; }
    public MediaMessage[]? Medias { get; private set; }
    public UserMessage(
        string workspace,
        string id, 
        string chatId, 
        string content)
    {
        Workspace = workspace;
        Id = id;
        ChatId = chatId;
        Content = content;
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
