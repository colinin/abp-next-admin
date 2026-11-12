using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.PrintDesigner.CustomElements.Dtos;

public class CustomElementGetPagedListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
