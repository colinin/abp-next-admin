using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.PrintDesigner.Templates.Dtos;

public class TemplateGetPagedListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
