using LINGYUN.Abp.AIManagement.Workspaces;
using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.AIManagement.Messages;
public abstract class UserMessageRecord : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }

    public string Workspace { get; private set; }

    public string? ConversationId { get; private set; }

    public string? ReplyMessage { get; private set; }

    protected UserMessageRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public UserMessageRecord(
        Guid id, 
        string workspace,
        Guid? tenantId = null)
        : base(id)
    {
        TenantId = tenantId;
        Workspace = workspace;
    }

    public virtual UserMessageRecord WithConversationId(string conversationId)
    {
        ConversationId = Check.NotNullOrWhiteSpace(conversationId, nameof(conversationId), UserMessageRecordConsts.MaxConversationIdLength);
        return this;
    }

    public virtual UserMessageRecord WithReply(string replyMessage)
    {
        ReplyMessage = replyMessage;
        return this;
    }

    public void Patch(UserMessageRecord otherMessage)
    {
        if (ConversationId != otherMessage.ConversationId)
        {
            ConversationId = otherMessage.ConversationId;
        }

        if (ReplyMessage != otherMessage.ReplyMessage)
        {
            ReplyMessage = otherMessage.ReplyMessage;
        }

        if (!this.HasSameExtraProperties(otherMessage))
        {
            ExtraProperties.Clear();

            foreach (var property in otherMessage.ExtraProperties)
            {
                ExtraProperties.Add(property.Key, property.Value);
            }
        }
    }
}
