using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.WebhooksManagement.Definitions;

public class WebhookGroupDefinitionGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
