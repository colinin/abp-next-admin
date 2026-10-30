using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions.Dtos;

public class FormDefinitionGetPagedListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public bool? IsPublished { get; set; }
}
