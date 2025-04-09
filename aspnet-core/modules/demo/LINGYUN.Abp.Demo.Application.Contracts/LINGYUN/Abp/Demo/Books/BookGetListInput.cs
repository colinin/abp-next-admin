using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Demo.Books;

public class BookGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
