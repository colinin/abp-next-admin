using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Demo.Authors;
public class GetAuthorListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}