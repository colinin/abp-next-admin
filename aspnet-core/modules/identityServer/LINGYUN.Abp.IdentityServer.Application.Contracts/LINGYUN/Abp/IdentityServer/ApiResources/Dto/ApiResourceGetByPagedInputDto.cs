using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.ApiResources;

public class ApiResourceGetByPagedInputDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
