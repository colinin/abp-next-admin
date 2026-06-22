using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.LocalizationManagement;

public class ResourceGetPagedListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
