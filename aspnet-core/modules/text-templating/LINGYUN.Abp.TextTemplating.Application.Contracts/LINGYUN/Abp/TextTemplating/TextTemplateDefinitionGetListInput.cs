using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateDefinitionGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
    public bool? IsStatic { get; set; }
    public bool? IsLayout { get; set; }
}
