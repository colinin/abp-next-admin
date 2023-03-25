using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.FeatureManagement.Definitions;
public class FeatureDefinitionGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    public string GroupName { get; set; }
}
