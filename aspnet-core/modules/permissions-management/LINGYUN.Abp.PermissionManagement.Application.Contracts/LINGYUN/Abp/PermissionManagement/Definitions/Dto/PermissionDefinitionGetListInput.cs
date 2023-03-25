using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.PermissionManagement.Definitions;
public class PermissionDefinitionGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    public string GroupName { get; set; }
}
