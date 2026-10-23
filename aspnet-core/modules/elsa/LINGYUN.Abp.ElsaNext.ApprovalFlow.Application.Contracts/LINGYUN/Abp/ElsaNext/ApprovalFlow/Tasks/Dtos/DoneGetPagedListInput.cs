using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Tasks.Dtos;

public class DoneGetPagedListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
