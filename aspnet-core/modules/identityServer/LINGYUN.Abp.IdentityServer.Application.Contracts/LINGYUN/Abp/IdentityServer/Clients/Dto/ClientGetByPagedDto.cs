using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.Clients;

public class ClientGetByPagedDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
