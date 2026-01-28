using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.AIManagement.Chats.Dtos;
public class ConversationDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiredAt { get; set; }

    public DateTime? UpdateAt { get; set; }
}
