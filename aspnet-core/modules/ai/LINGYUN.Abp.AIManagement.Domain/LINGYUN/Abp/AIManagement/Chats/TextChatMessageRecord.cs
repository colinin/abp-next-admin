using Microsoft.Extensions.AI;
using System;
using Volo.Abp;

namespace LINGYUN.Abp.AIManagement.Chats;
public class TextChatMessageRecord : ChatMessageRecord
{
    public string Content { get; private set; }

    public TextChatMessageRecord()
    {
    }

    public TextChatMessageRecord(
        Guid id,
        string workspace,
        string content,
        ChatRole role,
        DateTime createdAt,
        Guid? tenantId = null) 
        : base(id, workspace, role, createdAt, tenantId)
    {
        SetContent(content);
    }

    public virtual TextChatMessageRecord SetContent(string content)
    {
        Content = Check.NotNullOrWhiteSpace(content, nameof(content), TextChatMessageRecordConsts.MaxContentLength);
        return this;
    }
}
