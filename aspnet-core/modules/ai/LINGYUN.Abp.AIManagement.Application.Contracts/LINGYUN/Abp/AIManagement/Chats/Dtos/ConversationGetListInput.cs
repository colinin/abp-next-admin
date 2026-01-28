using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.AIManagement.Chats.Dtos;
public class ConversationGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
