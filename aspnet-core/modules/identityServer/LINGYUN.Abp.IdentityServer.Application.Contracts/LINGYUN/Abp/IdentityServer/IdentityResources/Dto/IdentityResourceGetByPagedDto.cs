using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.IdentityResources;

public class IdentityResourceGetByPagedDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
