using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.MessageService.Groups
{
    public class GroupSearchInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
