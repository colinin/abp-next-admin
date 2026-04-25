using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.BlobManagement.Dtos;

public class BlobContainerGetPagedListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
