using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.IdentityResources
{
    public class IdentityResourceGetByPagedInputDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
