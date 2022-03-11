using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Saas.Editions;

public class EditionGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
