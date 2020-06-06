using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientGetByPagedInputDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
