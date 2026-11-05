using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.OperationRateLimiting.Dtos;

public class OperationRateLimitingPolicyGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public bool? IsEnabled { get; set; }

    public string? Algorithm { get; set; }
}
