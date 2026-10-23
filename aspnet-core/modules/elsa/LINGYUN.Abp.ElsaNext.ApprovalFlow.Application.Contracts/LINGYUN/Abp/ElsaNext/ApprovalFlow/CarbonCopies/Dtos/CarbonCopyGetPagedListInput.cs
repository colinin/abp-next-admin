using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies.Dtos;

public class CarbonCopyGetPagedListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public bool? IsRead { get; set; }
}
