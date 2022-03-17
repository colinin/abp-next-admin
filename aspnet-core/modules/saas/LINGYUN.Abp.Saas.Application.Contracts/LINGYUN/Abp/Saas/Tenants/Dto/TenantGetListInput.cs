using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Saas.Tenants;

public class TenantGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}