using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.AIManagement.Chats.Dtos;
public class ConversationDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; } = default!;

    public string Workspace { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiredAt { get; set; }

    public DateTime? UpdateAt { get; set; }
}
