using Microsoft.Extensions.AI;
using System;

namespace LINGYUN.Abp.AI.Models;
public abstract class ChatMessage
{
    public string Workspace { get; }

    public string? Id { get; private set; }

    public Guid? ConversationId { get; private set; }

    public string? ReplyMessage { get; private set; }

    public DateTime? ReplyAt { get; private set; }

    public ChatRole Role { get; private set; }

    public DateTime CreatedAt { get; private set; }
    protected ChatMessage(
        string workspace,
        ChatRole? role = null,
        DateTime? createdAt = null)
    {
        Workspace = workspace;
        Role = role ?? ChatRole.User;
        CreatedAt = createdAt ?? DateTime.Now;
    }

    public virtual ChatMessage WithMessageId(string id)
    {
        Id = id;
        return this;
    }

    public virtual ChatMessage WithConversationId(Guid conversationId)
    {
        ConversationId = conversationId;
        return this;
    }

    public virtual ChatMessage WithReply(string replyMessage, DateTime replyAt)
    {
        ReplyMessage = replyMessage;
        ReplyAt = replyAt;
        return this;
    }

    public virtual string GetMessagePrompt()
    {
        return string.Empty;
    }
}
