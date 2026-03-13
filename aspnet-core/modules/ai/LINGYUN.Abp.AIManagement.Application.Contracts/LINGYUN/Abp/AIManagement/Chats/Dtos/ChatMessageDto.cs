using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.AIManagement.Chats.Dtos;
public abstract class ChatMessageDto : ExtensibleAuditedEntityDto<Guid>
{
    public string Workspace { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Role { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ConversationId { get; set; }

    public string? ReplyMessage { get; set; }

    public DateTime? ReplyAt { get; set; }
}
