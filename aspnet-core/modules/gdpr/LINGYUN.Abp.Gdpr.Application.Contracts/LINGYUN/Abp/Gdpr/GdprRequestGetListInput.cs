using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Gdpr;

public class GdprRequestGetListInput : PagedAndSortedResultRequestDto
{
    public string? CreationTime { get; set; }
    public string? ReadyTime { get; set; }
}
