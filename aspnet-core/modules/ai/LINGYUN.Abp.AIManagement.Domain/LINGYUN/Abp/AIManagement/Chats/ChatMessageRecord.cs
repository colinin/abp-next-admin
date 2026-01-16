using Microsoft.Extensions.AI;
using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.AIManagement.Chats;
public abstract class ChatMessageRecord : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }

    public string Workspace { get; private set; }

    public ChatRole Role { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public string? ConversationId { get; private set; }

    public string? ReplyMessage { get; private set; }

    public DateTime? ReplyAt { get; private set; }

    protected ChatMessageRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public ChatMessageRecord(
        Guid id, 
        string workspace,
        ChatRole role,
        DateTime createdAt,
        Guid? tenantId = null)
        : base(id)
    {
        Workspace = workspace;
        Role = role;
        CreatedAt = createdAt;
        TenantId = tenantId;
    }

    public virtual ChatMessageRecord SetConversationId(string conversationId)
    {
        ConversationId = Check.NotNullOrWhiteSpace(conversationId, nameof(conversationId), ChatMessageRecordConsts.MaxConversationIdLength);
        return this;
    }

    public virtual ChatMessageRecord SetReply(string replyMessage, DateTime? replyAt)
    {
        ReplyMessage = replyMessage;
        ReplyAt = replyAt;
        return this;
    }
}
