using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates.Dtos;

public class TemplateGetPagedListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public string? Category { get; set; }

    public bool? IsPublished { get; set; }

    public bool? IsEnabled { get; set; }
}
