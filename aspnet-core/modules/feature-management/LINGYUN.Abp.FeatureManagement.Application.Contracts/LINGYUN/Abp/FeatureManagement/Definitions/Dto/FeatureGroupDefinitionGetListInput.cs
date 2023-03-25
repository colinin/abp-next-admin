using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.FeatureManagement.Definitions;
public class FeatureGroupDefinitionGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
