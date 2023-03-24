using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.WebhooksManagement.Definitions;

public class WebhookDefinitionGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
