using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.PermissionManagement.Definitions;
public class PermissionGroupDefinitionGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
