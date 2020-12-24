using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.ApiScopes
{
    public class GetApiScopeInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
