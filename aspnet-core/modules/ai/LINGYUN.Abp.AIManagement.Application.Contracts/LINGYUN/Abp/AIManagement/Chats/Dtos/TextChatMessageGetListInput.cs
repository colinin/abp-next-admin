using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.AIManagement.Chats.Dtos;

public class TextChatMessageGetListInput : PagedAndSortedResultRequestDto
{
    [Required]
    public Guid ConversationId { get; set; }
}
